namespace Aardvark.Rendering.WebGPU

open System.Runtime.CompilerServices
open Aardvark.Base
open FShade
open WebGPU

type ComputeShader private (device : Device, pipeline : ComputePipeline, groupLayouts : BindGroupLayout[], groupLayout : WGSLBindGroupLayout, wgsl : GLSL.GLSLProgramInterface) =
    
    let cleanName (name : string) =
        if name.StartsWith "cs_" then name.Substring 3
        else name
        
    let defaultSamplers =
        groupLayout.Entries |> MapExt.map (fun _ entries ->
            entries |> MapExt.choose (fun _ entry ->
                match entry with
                | WGSLBindGroupEntry.Sampler sam ->
                    let state = List.head sam.samplerStates
                    let sam = device.CreateSampler state
                    Some sam
                | _ ->
                    None
            )
        )
    
    let tryGetDefaultSampler (gi : int) (bi : int) =
        match MapExt.tryFind gi defaultSamplers with
        | Some entries -> MapExt.tryFind bi entries
        | None -> None
        
    let uniformBufferReaders =
        groupLayout.Entries |> MapExt.map (fun _ entries ->
            entries |> MapExt.choose (fun _ entry ->
                match entry with
                | WGSLBindGroupEntry.UniformBuffer ub ->
                    let buffer =
                        device.CreateBuffer {
                            Next = null
                            Label = null
                            Usage = BufferUsage.Uniform ||| BufferUsage.CopyDst
                            Size = int64 ub.ubSize
                            MappedAtCreation = false
                        }
                        
                    let memory =
                        System.Runtime.InteropServices.Marshal.AllocHGlobal ub.ubSize
                        
                        
                    let update (m : MapExt<string, obj>) =
                        for f in ub.ubFields do
                            match MapExt.tryFind (cleanName f.ufName) m with
                            | Some value ->
                                let w = UniformWriters.getWriter 0 f.ufType (value.GetType())
                                w.WriteUnsafeValue(value, memory + nativeint f.ufOffset)
                            | None ->
                                Log.warn "missing uniform %s" f.ufName
                                
                        device.Queue.WriteBuffer(buffer, 0L, memory, ub.ubSize)
                                
                    let free() =
                        buffer.Dispose()
                        System.Runtime.InteropServices.Marshal.FreeHGlobal memory
                                
                    Some (buffer, update, free)
                | _ ->
                    None
            )
        )
        
        
    let localSize =
        match wgsl.shaders with
        | GLSL.GLSLProgramShaders.Compute c ->
            c.shaderDecorations |> List.pick (function GLSL.GLSLShaderDecoration.GLSLLocalSize s -> Some s | _ -> None)
        | _ ->
            failwith "bad program"
        
    member x.LocalSize = localSize
    member x.GroupLayout = groupLayout
    member x.WGSL = wgsl
    member x.Pipeline = pipeline
    
    member x.Run(workGroups : V3i, inputs : MapExt<string, obj>) =
        let groups = 
            groupLayout.Entries |> MapExt.map (fun gi entries ->
                let cnt = 1 + MapExt.max entries
                let data =
                    Array.init cnt (fun bi ->
                        match MapExt.tryFind bi entries with
                        | Some entry ->
                            match entry with
                            | WGSLBindGroupEntry.StorageBuffer buffer ->
                                match MapExt.tryFind (cleanName buffer.ssbName) inputs with
                                | Some (:? Buffer as b) ->
                                    BindGroupEntry.Buffer(bi, b, 0L, b.Size)
                                | _ ->
                                    Log.warn "Missing buffer %s" buffer.ssbName
                                    BindGroupEntry.Null
                                    
                            | WGSLBindGroupEntry.Texture tex ->
                                match MapExt.tryFind (cleanName (List.head tex.textureSemantics)) inputs with
                                | Some (:? Texture as b) ->
                                    BindGroupEntry.TextureView(bi, b.CreateView(TextureUsage.TextureBinding, 0))
                                | Some (:? TextureView as b) ->
                                    BindGroupEntry.TextureView(bi, b)
                                | _ ->
                                    Log.warn "Missing buffer %A" tex.textureSemantics
                                    BindGroupEntry.Null
                                    
                            | WGSLBindGroupEntry.StorageTexture tex ->
                                match MapExt.tryFind (cleanName tex.imageName) inputs with
                                | Some (:? Texture as b) ->
                                    BindGroupEntry.TextureView(bi, b.CreateView(TextureUsage.StorageBinding, 0))
                                | Some (:? TextureView as b) ->
                                    BindGroupEntry.TextureView(bi, b)
                                | _ ->
                                    Log.warn "Missing storage image %A" tex.imageName
                                    BindGroupEntry.Null
                                    
                            | WGSLBindGroupEntry.Sampler sam ->
                                match MapExt.tryFind (cleanName sam.samplerName) inputs with
                                | Some (:? Sampler as s) ->
                                    BindGroupEntry.Sampler(bi, s)
                                | _ ->
                                    match tryGetDefaultSampler gi bi with
                                    | Some s -> BindGroupEntry.Sampler(bi, s)
                                    | None ->
                                        Log.warn "Missing sampler %s" sam.samplerName
                                        BindGroupEntry.Null
                                    
                            | WGSLBindGroupEntry.UniformBuffer buffer ->
                                match MapExt.tryFind gi uniformBufferReaders with
                                | Some e ->
                                    match MapExt.tryFind bi e with
                                    | Some (buf, update, free) ->
                                        update inputs
                                        BindGroupEntry.Buffer(bi, buf, 0L, buf.Size)
                                    | None ->
                                        BindGroupEntry.Null
                                | None ->
                                    BindGroupEntry.Null
                                    
                                    
                        | None ->
                            BindGroupEntry.Null    
                    )
                device.CreateBindGroup {
                    Label = sprintf "group %d" gi
                    Layout = groupLayouts.[gi]
                    Entries = data
                }
            )
        
        
        task {
            try
                use enc = device.CreateCommandEncoder { Label = null; Next = null }
                use pass = enc.BeginComputePass { Label = null; TimestampWrites = undefined }
                
                pass.SetPipeline pipeline
                for KeyValue(gi, group) in groups do
                    pass.SetBindGroup(gi, group, [||])
                    
                pass.DispatchWorkgroups(workGroups.X, workGroups.Y, workGroups.Z)
                pass.End()
                use cmd = enc.Finish { Label = null }
                
                
                do! device.Queue.Submit([| cmd |])
            finally
                for KeyValue(_, group) in groups do
                    group.Dispose()
        }

    member x.Run(workGroups : V3i, inputs : list<string * obj>) = x.Run (workGroups, MapExt.ofList inputs)
    
    member x.Run(workGroups : V2i, inputs : MapExt<string, obj>) = x.Run(workGroups.XYI, inputs)
    member x.Run(workGroups : V2i, inputs : list<string * obj>) = x.Run(workGroups.XYI, inputs)
    member x.Run(workGroups : int,  inputs : MapExt<string, obj>) = x.Run(V3i(workGroups, 1, 1), inputs)
    member x.Run(workGroups : int,  inputs : list<string * obj>) = x.Run(V3i(workGroups, 1, 1), inputs)
    
    static member Compile(device : Device, shader : FShade.ComputeShader) =
            
        let wgsl = shader.GetWGSLCode()
        
        let sm =
            device.CreateShaderModule {
                Label = null
                Next = { ShaderSourceWGSL.Next = null; ShaderSourceWGSL.Code = wgsl.codes.[FShade.ShaderStage.Compute] }
            }
            
        let compute =
            {
                Module = sm
                EntryPoint = "main"
                Constants = [||]
            }
            
        let groupLayoutTable, groups = device.CreateBindGroupLayouts(wgsl.iface)
        let groupLayouts =
            match MapExt.tryMax groupLayoutTable with
            | Some max ->
                let len = max + 1
                Array.init len (fun i ->
                    match MapExt.tryFind i groupLayoutTable with
                    | Some v -> v
                    | None -> BindGroupLayout.Null
                )
            | None ->
                [||]
            
        let layout =
            device.CreatePipelineLayout {
                Next = null
                Label = null
                BindGroupLayouts = groupLayouts
                ImmediateSize = 0
            }
        
        let pipe = 
            device.CreateComputePipeline {
                Label = null
                Layout = layout
                Compute = compute
            }
            
        new ComputeShader(device, pipe, groupLayouts, groups, wgsl.iface)
    
    static member Compile(device : Device, shader : 'a -> 'b) =
        let sh = FShade.ComputeShader.ofFunction (V3i(1024, 1024, 1024)) shader
        ComputeShader.Compile(device, sh)

    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize x
        pipeline.Dispose()
        for g in groupLayouts do
            g.Dispose()
            
        for KeyValue(_, e) in defaultSamplers do
            for KeyValue(_, s) in e do s.Dispose()
            
        for KeyValue(_, e) in uniformBufferReaders do
            for KeyValue(_, (_,_,free)) in e do free()
           
    member x.Dispose() =
        x.Dispose(true)
            
        
    interface System.IDisposable with
        member x.Dispose() =
            x.Dispose(true)
            
    override x.Finalize() =
        x.Dispose false

[<AbstractClass; Sealed>]
type ComputeShaderExtensions private() =
    
    [<Extension>]
    static member CompileCompute(device : Device, shader : FShade.ComputeShader) =
        ComputeShader.Compile(device, shader)

    [<Extension>]
    static member CompileCompute(device : Device, func : 'a -> 'b) =
        let limit = V3i(device.Limits.MaxComputeWorkgroupSizeX, device.Limits.MaxComputeWorkgroupSizeY, device.Limits.MaxComputeWorkgroupSizeZ)
        let shader = FShade.ComputeShader.ofFunction limit func
        ComputeShader.Compile(device, shader)