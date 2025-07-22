
open Aardvark.Application
open Aardvark.Application.Slim
open Aardvark.Base
open Aardvark.Rendering
open System.Threading
open Aardvark.Rendering.WebGPU
open global.WebGPU
open FSharp.Data.Adaptive
open Microsoft.FSharp.NativeInterop


#nowarn "9"
type Blitter(device : Device, format : TextureFormat) =
    
    static let localSizeX = 8
    static let localSizeY = 8
    
    static let ceilDiv (a : int) (b : int) =
        if a % b = 0 then a / b
        else 1 + a / b
    
    
    let code =
        let wgslfmt = string(format).ToLower()
        $"""
        @group(0) @binding(0) var src : texture_2d<f32>;
        @group(0) @binding(1) var sam : sampler;
        @group(0) @binding(2) var dst : texture_storage_2d<{wgslfmt}, write>;
        
        @compute @workgroup_size({localSizeX}, {localSizeY})
        fn main(@builtin(global_invocation_id) GlobalInvocationID : vec3u) {{
            let size = textureDimensions(dst);
            let tc = (vec2f(GlobalInvocationID.xy) + vec2f(0.5, 0.5)) / vec2f(size);
            textureStore(dst, GlobalInvocationID.xy, textureSampleLevel(src, sam, tc, 0.0));
        }}
        """
        
    let shader =
        device.CreateShaderModule {
            Next = { Next = null; Code = code }
            Label = "Blitter"
        }
    
    let groupLayout =
        device.CreateBindGroupLayout {
            Label = "BlitterGroupLayout"
            Entries =
                [|
                    BindGroupLayoutEntry.Texture(
                        0, ShaderStage.Compute, {
                            SampleType = TextureSampleType.Float
                            ViewDimension = TextureViewDimension.D2D
                            Multisampled = false
                        }
                    )
                       
                    BindGroupLayoutEntry.Sampler(
                        1, ShaderStage.Compute,
                        SamplerBindingType.Filtering
                    )
             
                    BindGroupLayoutEntry.StorageTexture(
                        2, ShaderStage.Compute, {
                            Access = StorageTextureAccess.WriteOnly
                            Format = format
                            ViewDimension = TextureViewDimension.D2D
                        }
                    )
                    
                |]
        }
    
    let layout =
        device.CreatePipelineLayout {
            Next = null
            Label = "BlitterLayout"
            BindGroupLayouts = [|groupLayout |]
            ImmediateSize = 0
        }
    
    let pipeline =
        device.CreateComputePipeline {
            Label = "BlitterPipeline"
            Layout = layout
            Compute =
                {
                    Module = shader
                    EntryPoint = "main"
                    Constants = [||]
                }
        }
    
    let sampler =
        device.CreateSampler {
            Next = null
            Label = null
            AddressModeU = AddressMode.ClampToEdge
            AddressModeV = AddressMode.ClampToEdge
            AddressModeW = AddressMode.ClampToEdge
            MagFilter = FilterMode.Linear
            MinFilter = FilterMode.Linear
            MipmapFilter = MipmapFilterMode.Nearest
            LodMinClamp = 0.0f
            LodMaxClamp = 1000.0f
            Compare = CompareFunction.Undefined
            MaxAnisotropy = 1us
        }
    
    
    member x.Run(input : Texture, inputLevel : int, output : Texture, outputLevel : int) =
        
        let outputSize =
            let f = 1 <<< outputLevel
            V2i(max 1 (output.Width / f), max 1 (output.Height / f))
        
        use output = output.CreateView(TextureUsage.StorageBinding, outputLevel)
        use input = input.CreateView(TextureUsage.TextureBinding, inputLevel)
        
        use enc = device.CreateCommandEncoder { Label = null; Next = null }
        
        use cenc = enc.BeginComputePass { Label = null; TimestampWrites = undefined }
    
        
        use group =
            device.CreateBindGroup {
                Label = null
                Layout = groupLayout
                Entries =
                    [|
                        BindGroupEntry.TextureView(0, input)
                        BindGroupEntry.Sampler(1, sampler)
                        BindGroupEntry.TextureView(2, output)
                    |]
            }
        
        cenc.SetPipeline pipeline
        cenc.SetBindGroup(0, group, [||])
        cenc.DispatchWorkgroups(ceilDiv outputSize.X localSizeX, ceilDiv outputSize.Y localSizeY, 1)
        cenc.End()
        
        use buf = enc.Finish { Label = null }
        device.Queue.Submit [| buf |]

module Shader =
    open FShade
    
    let sammy2 =
        sampler2d {
            texture uniform?DiffuseColorTexture
            addressU WrapMode.Wrap
            addressV WrapMode.Wrap
            filter Filter.Anisotropic
            maxAnisotropy 16
        }
    
    let sammm (v : Effects.Vertex) =
        fragment {
            let dx = ddx v.tc
            let dy = ddy v.tc
            return sammy2.SampleGrad(v.tc,dx, dy)
        }
    
    type Vertex =
        {
            [<Position>] pos : V4f
            [<TexCoord>] tc : V2f
            [<Normal>] n : V3f
            [<Color>] col : V4f
        }
        

    let tex =
        sampler2dShadow {
            texture uniform?DiffuseColorTexture
            addressU WrapMode.Wrap
            addressV WrapMode.Wrap
            filter Filter.MinMagMipLinear
        }
        
        
    [<ReflectedDefinition>]
    let sammy (s : Sampler2dShadow) (tc : V2f) =
        s.Sample(tc, 0.5f)
 

    let env =
        samplerCube {
            texture uniform?EnvMap
            addressU WrapMode.Wrap
            addressV WrapMode.Wrap
            addressW WrapMode.Wrap
            filter Filter.MinMagMipLinear
        }

    type UniformScope with
        member x.Hansi : V4f[] = uniform?StorageBuffer?Hansi
        member x.HansiIndex : int = uniform?HansiIndex
    
    let envMap (v : Effects.Vertex) =
        fragment {
            let vp = uniform.ProjTrafoInv * V4f(v.pos.X, v.pos.Y, -1.0f, 1.0f)
            let vp = vp.XYZ / vp.W
            let dir = (uniform.ViewTrafoInv * V4f(vp, 0.0f)).XYZ |> Vec.normalize
            return env.Sample(dir) * uniform.Hansi.[uniform.HansiIndex]
        }

    let reverseTrafo (v : Effects.Vertex) =
        vertex {
            let wp = uniform.ViewProjTrafoInv * v.pos
            return { v with wp = wp / wp.W }
        }
           
    let fragment (v : Vertex) =
        fragment {
            return V4f(1.0, 0.0, 0.0, 1.0) //Vec.normalize v.n * 0.5 + V3d.Half, 1.0)
        }

    
    
    [<LocalSize(X = 64)>]
    let computer (x : int) (a : int[]) (b : int[]) (c : int[]) =
        compute {
            let id = getGlobalId().X
            let s = getWorkGroupSize()
            c.[id] <- a.[id] + b.[id] + x - s.X
        }

    let lineRx = System.Text.RegularExpressions.Regex @"\r\n|\n|\r"
    
    let withDevice (action : Device -> 'r) =
        let mutable running = true
        use instance =
            WebGPU.CreateInstance()
            
        let start =
            ThreadStart(fun () ->
                while running do
                    instance.ProcessEvents()
            )
        let thread = Thread(start, IsBackground = true)
        thread.Start()

        use adapter = 
            instance.CreateAdapter().Result
     
        use device = 
            adapter.RequestDeviceAsync({
                Next = null
                DebugOutput = true
                Label = "Devon"
                RequiredFeatures = adapter.Features.Features
                RequiredLimits = adapter.Limits
                DefaultQueue = { Label = "Quentin" }
            }).Result
        
        
        try
            action device
        finally 
            running <- false
            thread.Join()

module Scene =
    open Aardvark.SceneGraph
    
    type Marker = Marker
    
    let getImage =
        let names = typeof<Marker>.Assembly.GetManifestResourceNames()
        let load (name : string) =
            let name = names |> Array.find (fun str -> str.EndsWith name)
            use s = typeof<Marker>.Assembly.GetManifestResourceStream(name)
            PixImage.Load(s)

        load
    
    let skybox (name : string) =
        AVal.custom (fun _ ->
            let env =
                let trafo t (img : PixImage) = img.TransformedPixImage t

                PixCube [|
                    PixImageMipMap(
                        getImage (name.Replace("$", "rt"))
                        |> trafo ImageTrafo.Rot90
                    )
                    PixImageMipMap(
                        getImage (name.Replace("$", "lf"))
                        |> trafo ImageTrafo.Rot270
                    )
                
                    PixImageMipMap(
                        getImage (name.Replace("$", "bk"))
                    )
                    PixImageMipMap(
                        getImage (name.Replace("$", "ft"))
                        |> trafo ImageTrafo.Rot180
                    )
                
                    PixImageMipMap(
                        getImage (name.Replace("$", "up"))
                        |> trafo ImageTrafo.Rot90
                    )
                    PixImageMipMap(
                        getImage (name.Replace("$", "dn"))
                        |> trafo ImageTrafo.Rot90
                    )
                |]

            PixTextureCube(env, TextureParams.empty) :> ITexture
        )
    
    
    let scene (hi : aval<int>) (size : aval<V2i>) (view : aval<Trafo3d>)  =
        
        
        
        Sg.ofList [
            Sg.farPlaneQuad
            |> Sg.texture "EnvMap" (skybox "miramar_$.png")
            |> Sg.shader {
                do! Shader.reverseTrafo
                do! Shader.envMap
            }
            |> Sg.uniform "HansiIndex" hi
            |> Sg.uniform' "Hansi" [| V4f.IIII; V4f.IOOI; V4f.OIOI; V4f.OOII |]
        
        
            Sg.box' C4b.Green Box3d.Unit
            |> Sg.shader {
                do! DefaultSurfaces.trafo
                do! Shader.sammm
                //do! DefaultSurfaces.constantColor C4f.White
                //do! DefaultSurfaces.simpleLighting
            }
            |> Sg.diffuseFileTexture "/Users/schorsch/Desktop/stuff/GettyImages-121786088-58a4cc5a5f9b58a3c955c5fb.jpg" true
        ]
        |> Sg.viewTrafo view
        |> Sg.projTrafo (size |> AVal.map (fun s -> Frustum.perspective 90.0 0.1 100.0 (float s.X / float s.Y) |> Frustum.projTrafo))
    
    let renderTask (hi : aval<int>) (size : aval<V2i>) (view : aval<Trafo3d>) (device : Device) =
        //let rt = Runtime(device) 
        let signature = device.CreateFramebufferSignature(1, Map.ofList [0, { Name = DefaultSemantic.Colors; Format = TextureFormat.Bgra8 }], Some TextureFormat.Depth24Stencil8)
        let objs = Aardvark.SceneGraph.Semantics.RenderObjectSemantics.Semantic.renderObjects Ag.Scope.Root (scene hi size view)
        //let objs = objs |> ASet.force |> ASet.ofSeq)
        let task = new RenderTask(device, signature, objs)
        task

module ComputeShader =
    open FShade
    
    type UniformScope with
        member x.Factor : float32 = uniform?Factor
    
    [<LocalSize(X = 64)>]
    let computer (a : float32[]) (b : float32[]) (c : float32[]) =
        compute {
            let id = getGlobalId().X
            c.[id] <- (a.[id] + b.[id]) * uniform.Factor
        }
        
    let sam =
        sampler2d {
            texture uniform?a
            addressU WrapMode.Clamp
            addressV WrapMode.Clamp
            filter Filter.MinMagMipLinear
        }
        
    [<LocalSize(X = 8, Y = 8)>]
    let blur  (b : Image2d<Formats.r32f>) =
        compute {
            let id = getGlobalId().XY
            let size = b.Size
            if id.X < size.X && id.Y < size.Y then
                let mutable sum = V4f.Zero
                let mutable cnt = 0
                let kernelSize = 32
                for dx in -kernelSize .. kernelSize do
                    for dy in -kernelSize .. kernelSize do
                        let px = id + V2i(dx, dy)
                        if px.X >= 0 && px.Y >= 0 && px.X <= size.X && px.Y <= size.Y then
                            sum <- sum + sam.[px]
                            cnt <- cnt + 1
                let avg = sum / float32 cnt
                b.[id] <- avg
            
        }
        

    type UniformScope with
        member x.Offset : V2i = uniform?Offset
        member x.Size : V2i = uniform?Size
        member x.ViewportSize : V2i = uniform?ViewportSize
        member x.ModelViewProjTrafo : M44f = uniform?ModelViewProjTrafo
        
    [<LocalSize(X = 8, Y = 8)>]
    let rasterize (vertices : V4f[]) (colors : uint32[]) (color : UIntImage2d<Formats.r32ui>) (depth : Image2d<Formats.r32f>) =
        compute {
            let tri = getGlobalId().Z
            let triOff = 3*tri
            let id = getGlobalId().XY
            if id.X < uniform.Size.X && id.Y < uniform.Size.Y then
                let id = id + uniform.Offset
                
                let p0 = uniform.ModelViewProjTrafo * vertices.[triOff + 0]
                let p1 = uniform.ModelViewProjTrafo * vertices.[triOff + 1]
                let p2 = uniform.ModelViewProjTrafo * vertices.[triOff + 2]
                
                let col0 = unpackUnorm4x8(colors.[triOff + 0]).ZYXW
                let col1 = unpackUnorm4x8(colors.[triOff + 1]).ZYXW
                let col2 = unpackUnorm4x8(colors.[triOff + 2]).ZYXW
                
                let tc = (V2f id + V2f.Half) / V2f uniform.ViewportSize
                let ndc = 2.0f * tc - V2f.II
                // (a*p0.XY + b*p1.XY + c*p2.XY) / (a*p0.W + b*p1.W + c*p2.W) = ndc
                // (a*p0.XY + b*p1.XY + c*p2.XY) = ndc * (a*p0.W + b*p1.W + c*p2.W)
                
                
                // a*p0.XY + b*p1.XY + c*p2.XY = a*ndc*p0.W + b*ndc*p1.W + c*ndc*p2.W
                // a*(p0.XY - ndc*p0.W) + b*(p1.XY - ndc*p1.W) + c*(p2.XY - ndc*p2.W) = 0
                
                let f0 = p0.XY - ndc*p0.W
                let f1 = p1.XY - ndc*p1.W
                let f2 = p2.XY - ndc*p2.W
                
                // a*f0 + b*f1 + c*f2 = 0
                // a + b + c = 1
                
                // a*f0 + b*f1 + f2-a*f2-b*f2 = 0
                // a*(f0 - f2) + b*(f1 - f2) = -f2
                
                let c0 = f0 - f2
                let c1 = f1 - f2
                
                let det = c0.X*c1.Y - c0.Y*c1.X
                let r0 = V2f(c1.Y / det, -c1.X / det)
                let r1 = V2f(-c0.Y / det, c0.X / det)
                
                let a = -Vec.dot r0 f2
                let b = -Vec.dot r1 f2
                let c = 1.0f - a - b
                
                if a >= 0.0f && b >= 0.0f && c >= 0.0f && a <= 1.0f && b <= 1.0f && c <= 1.0f then
                    let pos = a*p0 + b*p1 + c*p2
                    if pos.X >= -pos.W && pos.X <= pos.W &&
                       pos.Y >= -pos.W && pos.Y <= pos.W &&
                       pos.Z >= -pos.W && pos.Z <= pos.W then
                            let c = a*col0 + b*col1 + c*col2
                            let projected = pos.XYZ / pos.W
                            
                            //let c = V4d.IOOI
                            color.[id] <- V4ui(packUnorm4x8 c)
                            depth.[id] <- V4f(projected.Z)
                        
                
                
                // a*(f0x - f2x) + b*(f1x - f2x) = -f2x
                // a*(f0y - f2y) + b*(f1y - f2y) = -f2y
                
                // a*(f0x - f2x)*(f0y - f2y) + b*(f1x - f2x)*(f0y - f2y) = -f2x*(f0y - f2y)
                // -a*(f0x - f2x)*(f0y - f2y) - b*(f0x - f2x)*(f1y - f2y) = +f2y*(f0x - f2x)
                
                // b*[(f1x - f2x)*(f0y - f2y) - (f0x - f2x)*(f1y - f2y)] = [f2y*(f0x - f2x) - f2x*(f0y - f2y)]
                // b*[(f1x - f2x)*(f0y - f2y) - (f0x - f2x)*(f1y - f2y)] = [f2y*(f0x - f2x) - f2x*(f0y - f2y)]
                ()
            
        }

open FShade
open FShade.WGSL

type CompiledComputeShader private (device : Device, pipeline : ComputePipeline, groupLayouts : BindGroupLayout[], groupLayout : WGSLBindGroupLayout, wgsl : GLSL.GLSLProgramInterface) =
    
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
                Next = { ShaderSourceWGSL.Next = null; ShaderSourceWGSL.Code = wgsl.codes.[ShaderStage.Compute] }
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
            
        new CompiledComputeShader(device, pipe, groupLayouts, groups, wgsl.iface)
    
    static member Compile(device : Device, shader : 'a -> 'b) =
        let sh = FShade.ComputeShader.ofFunction (V3i(1024, 1024, 1024)) shader
        CompiledComputeShader.Compile(device, sh)

    member x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize x
        pipeline.Dispose()
        for g in groupLayouts do
            g.Dispose()
            
        for KeyValue(_, e) in defaultSamplers do
            for KeyValue(_, s) in e do s.Dispose()
            
        for KeyValue(_, e) in uniformBufferReaders do
            for KeyValue(_, (_,_,free)) in e do free()
            
        
    interface System.IDisposable with
        member x.Dispose() =
            x.Dispose(true)
            
    override x.Finalize() =
        x.Dispose false



let computeTest (device : Device) =
    
    let ceilDiv (a : int) (b : int) =
        if a % b = 0 then a / b
        else 1 + a / b
    let cnt = 2048
    let factor = 2.5
    
    use a = device.CreateBuffer(BufferUsage.Storage, Array.init cnt float32).Result
    use b = device.CreateBuffer(BufferUsage.Storage, Array.init cnt (fun i -> float32 cnt - float32 i)).Result
    use c = device.CreateBuffer(BufferUsage.Storage, Array.zeroCreate<float32> cnt).Result
    
    // use tex =
    //     device.CreateTexture {
    //         Next = null
    //         Label : string
    //         Usage : TextureUsage
    //         Dimension : TextureDimension
    //         Size : Extent3D
    //         Format : TextureFormat
    //         MipLevelCount : int
    //         SampleCount : int
    //         ViewFormats : array<TextureFormat>
    //     }
    
    
    let inline ceilDiv (a : int) (b : int) =
        if a % b = 0 then a / b
        else 1 + a / b
    
    use shader = CompiledComputeShader.Compile(device, ComputeShader.computer)
    let groups = cnt / shader.LocalSize.X
    
    let task = 
        shader.Run(groups, [
            "a", a :> obj
            "b", b
            "c", c
            "Factor", factor
        ])
    task.Wait()
    
    let arr = device.Download<float32>(c).Result
    let check = arr |> Array.forall (fun v -> float v = factor * float cnt)
    printfn "ARR: %A (%A)" arr check
    
    
    let a =
        let img = PixImage.Load "/Users/schorsch/Desktop/stuff/GettyImages-121786088-58a4cc5a5f9b58a3c955c5fb.jpg"
        let img = img.ToPixImage<float32>(Col.Format.RGBA).ToGrayscalePixImage()
        let img = device.CreateTexture(img).Result
        img
    let b = device.CreateTexture(TextureFormat.R32f, V2i(a.Width, a.Height))
    
    use shader = CompiledComputeShader.Compile(device, ComputeShader.blur)
    
    let groups = V2i(ceilDiv a.Width shader.LocalSize.X, ceilDiv a.Height shader.LocalSize.Y)
    
    
    
    let task = 
        shader.Run(groups, [
            "a", a.CreateView(TextureUsage.StorageBinding ||| TextureUsage.TextureBinding) :> obj
            "b", b.CreateView(TextureUsage.StorageBinding)
        ])
    task.Wait()
    let img = device.DownloadPixImage(b).Result
    img.ToPixImage<byte>(Col.Format.Gray).SaveAsPng "/Users/schorsch/Desktop/test.png"
    
    
    use shader = CompiledComputeShader.Compile(device, ComputeShader.rasterize)
    
    let vertices =
        [|
            V4f(0.0f, 0.0f, 0.0f, 1.0f); V4f(0.5f, 0.0f, 0.0f, 1.0f); V4f(0.0f, 0.5f, 0.0f, 1.0f)
            V4f(0.0f, 0.5f, 0.0f, 1.0f); V4f(0.5f, 0.0f, 0.0f, 1.0f); V4f(0.5f, 0.5f, 0.0f, 1.0f); 
        |]
    
    let colors =
        [|
            C4b.Red; C4b.Green; C4b.Blue;
            C4b.Blue; C4b.Green; C4b.White;
        |]
        
    let vertexBuffer = device.CreateBuffer(BufferUsage.Storage, vertices).Result
    let colorBuffer = device.CreateBuffer(BufferUsage.Storage, colors).Result
    
    let size = V2i(1024, 1024)
    let color = device.CreateTexture(TextureFormat.R32ui, size)
    let depth = device.CreateTexture(TextureFormat.R32f, size)
    let groups = V2i(ceilDiv size.X shader.LocalSize.X, ceilDiv size.Y shader.LocalSize.Y)
    
    color.Clear(0xFF000000u).Wait()
    
    let task = 
        shader.Run(V3i(groups, vertices.Length), [
            "vertices", vertexBuffer :> obj
            "colors", colorBuffer
            "Offset", V2i.Zero 
            "Size", size
            "ViewportSize", size
            "color", color.CreateView(TextureUsage.StorageBinding ||| TextureUsage.TextureBinding) :> obj
            "depth", depth.CreateView(TextureUsage.StorageBinding)
            "ModelViewProjTrafo", M44f.Identity
        ])
    task.Wait()
    let img = device.DownloadPixImage(color).Result :?> PixImage<uint32>
    let color = PixImage<byte>(Col.Format.RGBA, size)
    
    color.GetMatrix<C4b>().SetMap(img.GetChannel 0L, fun v ->
        let a = byte (v >>> 24)    
        let b = byte (v >>> 16) 
        let g = byte (v >>> 8)  
        let r = byte v
        C4b(r,g,b,a)
    ) |> ignore
    
    color.SaveAsPng "/Users/schorsch/Desktop/raster.png"
    //
    //
    //
    // let vertices =
    //     [|
    //         V4f(0.0f, 0.0f, 0.0f, 1.0f); V4f(0.5f, 0.0f, 0.0f, 1.0f); V4f(0.0f, 0.5f, 0.0f, 1.0f)
    //         V4f(0.0f, 0.5f, 0.0f, 1.0f); V4f(0.5f, 0.0f, 0.0f, 1.0f); V4f(0.5f, 0.5f, 0.0f, 1.0f); 
    //     |]
    //
    // let colors =
    //     [|
    //         C4b.Red; C4b.Green; C4b.Blue;
    //         C4b.Blue; C4b.Green; C4b.White;
    //     |]
    //     
    // let shader = CompiledComputeShader.Compile(device, TiledRasterizer.rasterizeTiled)
    //     
    // let viewport = V2i(1024, 1024)
    // let tileSize = shader.LocalSize.XY
    // let tileQueueCapacity = 32
    //
    // let tiles = V2i(ceilDiv viewport.X tileSize.X, ceilDiv viewport.Y tileSize.Y)
    //     
    // let tileQueues = Array.zeroCreate (tiles.X * tiles.Y * (2 + tileQueueCapacity))
    //
    // for i in 0 .. 3 .. vertices.Length - 3 do
    //     let p0 = vertices.[i].XY * 0.5f + V2f.Half
    //     let p1 = vertices.[i+1].XY * 0.5f + V2f.Half
    //     let p2 = vertices.[i+2].XY * 0.5f + V2f.Half
    //     let box = Box2f([|p0; p1; p2|])
    //
    //     let minPixel = V2d box.Min * V2d viewport |> floor |> V2i
    //     let maxPixel = V2d box.Max * V2d viewport |> floor |> V2i
    //     
    //     
    //     let minTile = minPixel / tileSize
    //     let maxTile = maxPixel / tileSize
    //     
    //     for tx in minTile.X .. maxTile.X do
    //         for ty in minTile.Y .. maxTile.Y do
    //             let tile = V2i(tx, ty)
    //             let tileQueueOffset = (2 + tileQueueCapacity) * (tile.X + tile.Y * tileSize.X)
    //             
    //             let first = tileQueues.[tileQueueOffset]
    //             let cnt = tileQueues.[tileQueueOffset+1]
    //             
    //             let e = (first + cnt) % tileQueueCapacity
    //             tileQueues.[tileQueueOffset + 2 + e] <- i / 3
    //             tileQueues.[tileQueueOffset+1] <- cnt + 1
    //     
    //     
    //
    //     
    //
    //     
    // let vertexBuffer = device.CreateBuffer(BufferUsage.Storage, vertices).Result
    // let colorBuffer = device.CreateBuffer(BufferUsage.Storage, colors).Result
    // let color = device.CreateTexture(TextureFormat.R32ui, viewport)
    // let depth = device.CreateTexture(TextureFormat.R32f, viewport)
    // let groups = tiles
    //
    // color.Clear(0xFF000000u).Wait()
    //
    // let task = 
    //     shader.Run(V3i(groups, vertices.Length), [
    //         "vertices", vertexBuffer :> obj
    //         "colors", colorBuffer
    //         "ViewportSize", viewport
    //         "color", color.CreateView(TextureUsage.StorageBinding ||| TextureUsage.TextureBinding) :> obj
    //         "depth", depth.CreateView(TextureUsage.StorageBinding)
    //         "TileQueueCapacity", tileQueueCapacity
    //         "tileQueues", device.CreateBuffer(BufferUsage.Storage, tileQueues) :> obj
    //     ])
    // task.Wait()
    // let img = device.DownloadPixImage(color).Result :?> PixImage<uint32>
    // let color = PixImage<byte>(Col.Format.RGBA, viewport)
    //
    // color.GetMatrix<C4b>().SetMap(img.GetChannel 0L, fun v ->
    //     let a = byte (v >>> 24)    
    //     let b = byte (v >>> 16) 
    //     let g = byte (v >>> 8)  
    //     let r = byte v
    //     C4b(r,g,b,a)
    // ) |> ignore
    //
    // color.SaveAsPng "/Users/schorsch/Desktop/raster2.png"
    //
    
        
        
    
    
    
    
        
        
    ()

module Resolve =
    open FShade
    open Aardvark.SceneGraph
    
    type UniformScope with
        member x.Input : UIntImage2d<Formats.r32ui> = uniform?Input
    
    let blit (v : Effects.Vertex) =
        fragment {
            let value = uniform.Input.[V2i(v.tc * V2f uniform.ViewportSize)].X
            return unpackUnorm4x8 value
        }

    let compileRender (signature : IFramebufferSignature) (inputTex : aval<ITexture>) =
        let size = inputTex |> AVal.map (fun t -> (t :?> IBackendTexture).Size.XY)
        let sg = 
            Sg.fullScreenQuad
            |> Sg.shader {
                do! blit
            }
            |> Sg.uniform "Input" inputTex
            |> Sg.uniform "ViewportSize" size
            
        let objs = Aardvark.SceneGraph.Semantics.RenderObjectSemantics.Semantic.renderObjects Ag.Scope.Root sg
        //let objs = objs |> ASet.force |> ASet.ofSeq)
        let rt = signature.Runtime :?> Runtime
        let task = new RenderTask(rt.Device, signature :?> FramebufferSignature, objs)
        task

let computeRasterizerTask (signature : IFramebufferSignature) (mvp : aval<Trafo3d>) (device : Device) =
    let shader = CompiledComputeShader.Compile(device, ComputeShader.rasterize)
    
    let vertices =
        [|
            V4f(0.0f, 0.0f, 0.0f, 1.0f); V4f(0.5f, 0.0f, 0.0f, 1.0f); V4f(0.0f, 0.5f, 0.0f, 1.0f)
            V4f(0.0f, 0.5f, 0.0f, 1.0f); V4f(0.5f, 0.0f, 0.0f, 1.0f); V4f(0.5f, 0.5f, 0.0f, 1.0f); 
        |]
    
    let colors =
        [|
            C4b.Red; C4b.Green; C4b.Blue;
            C4b.Blue; C4b.Green; C4b.White;
        |]
        
    let vertexBuffer = device.CreateBuffer(BufferUsage.Storage, vertices).Result
    let colorBuffer = device.CreateBuffer(BufferUsage.Storage, colors).Result
    
    
    let mutable texSize = V2i.Zero
    let mutable color = Unchecked.defaultof<Texture>
    let mutable depth = Unchecked.defaultof<Texture>
    let csize = cval texSize
    let ctex = cval Unchecked.defaultof<ITexture>
    
    let ceilDiv (a : int) (b : int) =
        if a % b = 0 then a / b
        else 1 + a / b
    
    let resolve = Resolve.compileRender signature ctex
    
    RenderTask.custom (fun (t, _, o) ->
        let size = o.viewport.Size
        let mvp = mvp.GetValue t
        
        if size <> texSize then
            if not (isNull (color :> obj)) then
                color.Dispose()
                depth.Dispose()
        
            color <- device.CreateTexture(TextureFormat.R32ui, size)
            depth <- device.CreateTexture(TextureFormat.R32f, size)
            texSize <- size
            transact (fun () -> csize.Value <- size; ctex.Value <- color)
            
        let groups = V2i(ceilDiv size.X shader.LocalSize.X, ceilDiv size.Y shader.LocalSize.Y)
        
        
        color.Clear(0xFF000000u).Wait()
        
        let task = 
            shader.Run(V3i(groups, vertices.Length), [
                "vertices", vertexBuffer :> obj
                "colors", colorBuffer
                "Offset", V2i.Zero 
                "Size", size
                "ViewportSize", size
                "color", color.CreateView(TextureUsage.StorageBinding ||| TextureUsage.TextureBinding)
                "depth", depth.CreateView(TextureUsage.StorageBinding)
                "ModelViewProjTrafo", M44f mvp.Forward
            ])
        task.Wait()
        
        use cmd = device.CreateCommandEncoder { Label = null; Next = null }
        
        resolve.Run(o)
    )
    

[<EntryPoint>]
let main _argv =
    Aardvark.Init()
     
    let app = WebGPUApplication.Create(true).Result
    //
    // computeTest app.Device
    // exit 0
    
    let win = app.CreateGameWindow(vsync = true)
    
    let cam =
        CameraView.lookAt (V3d(4,3,2)) V3d.Zero V3d.OOI
        |> DefaultCameraController.control win.Mouse win.Keyboard win.Time
        |> AVal.map CameraView.viewTrafo
    
    let frustum =
        win.Sizes |> AVal.map (fun s ->
            Frustum.perspective 90.0 0.1 100.0 (float s.X / float s.Y) |> Frustum.projTrafo
        )
    
    let mvp = AVal.map2 (*) cam frustum
  
    let task =
        computeRasterizerTask win.FramebufferSignature mvp app.Device

        
    win.RenderTask <- task
    win.Run()
        
        
    0
