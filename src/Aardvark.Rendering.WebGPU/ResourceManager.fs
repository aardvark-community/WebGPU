namespace Aardvark.Rendering.WebGPU

open System
open System.Threading
open FShade.Intrinsics
open FSharp.Data.Adaptive
open Aardvark.Rendering
open Aardvark.Base
open global.WebGPU
open Microsoft.FSharp.NativeInterop
open Aardvark.Rendering.WebGPU

#nowarn "9"


[<AbstractClass>]
type AdaptiveResource() =
    inherit AdaptiveObject()
    
    abstract member Acquire : unit -> unit
    abstract member Release : unit -> unit
    abstract member Update : CommandEncoder * AdaptiveToken -> unit

    
[<AbstractClass>]
type AdaptiveResource<'a>() =
    inherit AdaptiveResource()
    
    let mutable refCount = 0
    let mutable handle = None
    
    abstract member Create : CommandEncoder * AdaptiveToken -> 'a
    abstract member Destroy : 'a -> unit
    abstract member TryUpdate : 'a * CommandEncoder * AdaptiveToken -> bool
    abstract member AfterDestroy : unit -> unit
    abstract member BeforeCreate : unit -> unit
    
    member x.TryUpdate(cmd : CommandEncoder, token : AdaptiveToken) =
        x.EvaluateAlways token (fun token ->
            if x.OutOfDate then
                match handle with
                | Some h -> x.TryUpdate(h, cmd, token)
                | None -> false
            else
                true
        )
    
    default x.AfterDestroy() = ()
    default x.BeforeCreate() = ()
    
    override x.Acquire() =
        if Interlocked.Increment(&refCount) = 1 then
            ()
    
    override x.Release() =
        if Interlocked.Decrement(&refCount) = 0 then
            lock x (fun () ->
                match handle with
                | Some h ->
                    handle <- None
                    x.Destroy h
                    x.AfterDestroy()
                | None -> ()
            )
    
    member x.GetHandle(cmd : CommandEncoder, token : AdaptiveToken) =
        x.EvaluateAlways token (fun token ->
            if refCount = 0 then
                failwith $"cannot get handle for resource with zero references"
            match handle with
            | Some h ->
                if x.OutOfDate then
                    match handle with
                    | Some h ->
                        if x.TryUpdate(h, cmd, token) then
                            h
                        else
                            x.Destroy h
                            let h = x.Create(cmd, token)
                            handle <- Some h
                            h
                    | None ->
                        let h = x.Create(cmd, token)
                        handle <- Some h
                        h
                else
                    h
            | None ->
                x.BeforeCreate()
                let h = x.Create(cmd, token)
                handle <- Some h
                h
        )
   
    override x.Update(cmd, token) =
        x.GetHandle(cmd, token) |> ignore
        
type InputDescription =
    {
        Type        : Type
        Stride      : int
        PerInstance : bool
    }
   
type AdaptiveRenderPipelineDescription =
    {
        Shader              : ShaderProgram
        Signature           : FramebufferSignature    
        Mode                : IndexedGeometryMode
        InputDescriptions   : MapExt<int, InputDescription>
        RasterizerState     : RasterizerState
        DepthState          : Aardvark.Rendering.DepthState
        StencilState        : Aardvark.Rendering.StencilState
        BlendState          : Aardvark.Rendering.BlendState
        IndexType           : option<Type>
    }
    
module AdaptiveRenderPipelineDescription =
    
    let ofRenderObject (signature : FramebufferSignature) (o : RenderObject) =
        
        let shader =
            match o.Surface with
            | Surface.Effect e -> signature.Device.CreateShaderProgram(e, signature)
            | _ -> failwith "unsupported surface"
        
        
        let inputs =
            shader.ShaderCode.iface.inputs |> List.choose (fun p ->
                
                let ofBufferView (perInstance : bool) (att : BufferView) =
                    match att.SingleValue with
                    | Some singleValue ->
                        let elemType = singleValue.ContentType
                        Some (p.paramLocation, { Type = elemType; Stride = 0; PerInstance = true })
                    | None ->
                        let stride =
                            if att.Stride = 0 then att.ElementType.GetCLRSize()
                            else att.Stride
                        let t = att.ElementType
                        Some (p.paramLocation, { Type = t; Stride = stride; PerInstance = perInstance })
                
                match o.VertexAttributes.TryGetAttribute (Symbol.Create p.paramSemantic) with
                | Some att -> ofBufferView false att
                | None ->
                    match o.InstanceAttributes.TryGetAttribute (Symbol.Create p.paramSemantic) with
                    | Some att -> ofBufferView true att
                    | None -> None
                        
            ) |> MapExt.ofList
        
        {
            Shader = shader
            Signature = signature
            Mode = o.Mode
            InputDescriptions = inputs
            RasterizerState = o.RasterizerState
            DepthState = o.DepthState
            StencilState = o.StencilState
            BlendState = o.BlendState
            IndexType = (match o.Indices with | Some i -> Some i.ElementType | None -> None)
        }
  
[<RequireQualifiedAccess>]
type AdaptiveBindGroupEntry =
    | Buffer of int * AdaptiveResource<Buffer>
    | Sampler of int * Sampler
    | TextureView of int * AdaptiveResource<TextureView>
    

type ResourceManager(device : Device) =
    let bufferCache = ConcurrentDict<BufferUsage * IAdaptiveValue, AdaptiveResource<Buffer>>(Dict())
    let pipelineCache = ConcurrentDict<AdaptiveRenderPipelineDescription, AdaptiveResource<RenderPipeline>>(Dict())
    let uboCache = ConcurrentDict<FShade.WGSL.WGSLUniformBuffer * list<IAdaptiveValue>, AdaptiveResource<Buffer>>(Dict())
    let bindGroupsCache = ConcurrentDict<MapExt<int, BindGroupLayout> * array<list<AdaptiveBindGroupEntry>>, AdaptiveResource<BindGroup[]>>(Dict())
    
    static let acquire (v : IAdaptiveValue) =
        match v with
        | :? IAdaptiveResource as r -> r.Acquire()
        | _ -> ()
        
    static let release (v : IAdaptiveValue) =
        match v with
        | :? IAdaptiveResource as r -> r.Release()
        | _ -> ()
    
    member private x.CreateSingleValueBuffer(usage : BufferUsage, value : IAdaptiveValue) =
        bufferCache.GetOrCreate((usage, value), fun (usage, value) ->
            { new AdaptiveResource<Buffer>() with
                override x.Create(cmd, token) =
                    let value = value.GetValueUntyped token
                    
                    let res =
                        device.CreateBuffer {
                            Label = null
                            Next = null
                            Size = sizeof<V4f>
                            Usage = usage
                            MappedAtCreation = false
                        }
                    
                    match value with
                    | :? V4f as value -> cmd.Upload([|value|], res)
                    | :? V4i as value -> cmd.Upload([|value|], res)
                    | _ -> failwith $"bad singlevalue: {value}"
                    res
                    
                override x.Destroy b =
                    b.Dispose()
                    
                override x.TryUpdate(buffer, cmd, token) =
                    let value = value.GetValueUntyped token
                    match value with
                    | :? V4f as value -> cmd.Upload([|value|], buffer)
                    | :? V4i as value -> cmd.Upload([|value|], buffer)
                    | _ -> failwith $"bad singlevalue: {value}"
                    true
            
            }
        )
    
    member private x.CreateBuffer(usage : BufferUsage, value : IAdaptiveValue) =
        let usage = usage ||| BufferUsage.CopyDst ||| BufferUsage.CopySrc
        bufferCache.GetOrCreate((usage, value), fun (usage, value) ->
            let mutable ownsHandle = true
            { new AdaptiveResource<Buffer>() with
                override x.Create(cmd, token) =
                    let value = value.GetValueUntyped token :?> IBuffer
                    
                    match value with
                    | :? Buffer as b ->
                        ownsHandle <- false
                        b.AddRef()
                        b
                    | _ ->
                        ownsHandle <- true
                        let size = value.GetSizeInBytes()
                        let res =
                            device.CreateBuffer {
                                Label = null
                                Next = null
                                Size = size
                                Usage = usage
                                MappedAtCreation = false
                            }
                        cmd.CopyIBufferToBuffer(value, res)
                        res
                    
                override x.BeforeCreate() =
                    acquire value
                    
                override x.AfterDestroy() =
                    release value
                    bufferCache.Remove((usage, value)) |> ignore
                    
                override x.Destroy(buffer) =
                    buffer.Dispose()
                    
                override x.TryUpdate(buffer, cmd, token) =
                    if ownsHandle then
                        let value = value.GetValueUntyped token :?> IBuffer
                        match value with
                        | :? Buffer as b ->
                            false
                        | _ ->
                            let size = value.GetSizeInBytes()
                            if size = buffer.Size then
                                cmd.CopyIBufferToBuffer(value, buffer)
                                true
                            else
                                false
                    else
                        false
            }
        )
    
    member x.CreateBuffer(usage : BufferUsage, value : aval<IBuffer>) =
        x.CreateBuffer(usage, value :> IAdaptiveValue)
        
    member x.CreateBuffer(usage : BufferUsage, value : aval<IBackendBuffer>) =
        x.CreateBuffer(usage, value :> IAdaptiveValue)
        
    member private x.CreateTexture(value : IAdaptiveValue) =
        
        
        { new AdaptiveResource<Texture>() with
            member x.Create(cmd, token) =
                match value.GetValueUntyped token with
                | :? FileTexture as t ->
                    failwith ""
                | :? INativeTexture as t ->
                    
                    failwith ""
                | :? PixTexture2d as t ->
                    failwith ""
                | :? PixTexture3d as t ->
                    failwith ""
                | :? PixTextureCube as t ->
                    failwith ""
                | :? Texture as t ->
                    t.AddRef()
                    t
                | _ ->
                    failwith ""
            member x.TryUpdate(handle, cmd, token) =
                false
            member x.Destroy(handle) =
                handle.Dispose()
        }
         
    member x.CreateStorageTextureView(tex : AdaptiveResource<Texture>, level : int, usage : TextureUsage) =
        { new AdaptiveResource<TextureView>() with
            member x.Create(cmd, token) =
                let tex = tex.GetHandle(cmd, token)
                tex.CreateView(TextureUsage.StorageBinding ||| TextureUsage.RenderAttachment, level)
                
            member x.TryUpdate(handle, cmd, token) =
                tex.TryUpdate(cmd, token)
                
            member x.Destroy(handle) =
                handle.Dispose()
                
            member x.BeforeCreate() =
                tex.Acquire()
            member x.AfterDestroy() =
                tex.Release()
        }
         
    member x.CreateSampledTextureView(tex : AdaptiveResource<Texture>, usage : TextureUsage) =
        { new AdaptiveResource<TextureView>() with
            member x.Create(cmd, token) =
                let tex = tex.GetHandle(cmd, token)
                tex.CreateView(TextureUsage.TextureBinding)
                
            member x.TryUpdate(handle, cmd, token) =
                tex.TryUpdate(cmd, token)
                
            member x.Destroy(handle) =
                handle.Dispose()
                
            member x.BeforeCreate() =
                tex.Acquire()
            member x.AfterDestroy() =
                tex.Release()
        }
         
         
    member x.CreateRenderPipeline(desc : AdaptiveRenderPipelineDescription) =
        pipelineCache.GetOrCreate(desc, fun desc ->
            { new AdaptiveResource<RenderPipeline>() with
                override x.Create(cmd, token) =
                    let layout = desc.Shader.PipelineLayout
                    let shader = desc.Shader.ShaderModule
                    
                    let buffers =
                        match MapExt.tryMax desc.InputDescriptions with
                        | Some maxIndex ->
                            let empty =
                                {
                                    VertexBufferLayout.StepMode = VertexStepMode.VertexBufferNotUsed
                                    ArrayStride = 0L
                                    Attributes = [||]
                                }
                            let arr = Array.create (maxIndex + 1) empty
                            desc.InputDescriptions |> MapExt.iter (fun k v ->
                                let step = if v.PerInstance then VertexStepMode.Instance else VertexStepMode.Vertex
                                
                                
                                let contentType, cnt, offsetPerAtt =
                                    if v.Type = typeof<float32> then VertexFormat.Float32, 1, 0
                                    elif v.Type = typeof<V2f> then VertexFormat.Float32x2, 1, 0
                                    elif v.Type = typeof<V3f> then VertexFormat.Float32x3, 1, 0
                                    elif v.Type = typeof<V4f> then VertexFormat.Float32x4, 1, 0
                                    elif v.Type = typeof<int> then VertexFormat.Sint32, 1, 0
                                    elif v.Type = typeof<V2i> then VertexFormat.Sint32x2, 1, 0
                                    elif v.Type = typeof<V3i> then VertexFormat.Sint32x3, 1, 0
                                    elif v.Type = typeof<V4i> then VertexFormat.Sint32x4, 1, 0
                                    elif v.Type = typeof<uint> then VertexFormat.Uint32, 1, 0
                                    elif v.Type = typeof<C4b> then VertexFormat.Unorm8x4BGRA, 1, 0
                                    elif v.Type = typeof<C4f> then VertexFormat.Float32x4, 1, 0
                                    elif v.Type = typeof<M34f> then VertexFormat.Float32x4, 3, sizeof<V4f>
                                    elif v.Type = typeof<M44f> then VertexFormat.Float32x4, 4, sizeof<V4f>
                                    elif v.Type = typeof<M33f> then VertexFormat.Float32x3, 3, sizeof<V3f>
                                    elif v.Type = typeof<M22f> then VertexFormat.Float32x2, 2, sizeof<V2f>
                                    else failwith $"unsupported vertex type: {v.Type}"
                                
                                let attributes =
                                    Array.init cnt (fun i ->
                                        let result = 
                                            {
                                                VertexAttribute.ShaderLocation = k + i
                                                VertexAttribute.Format = contentType
                                                VertexAttribute.Offset = int64 offsetPerAtt * int64 i
                                            }
                                        result
                                    )
                                    
                                
                                
                                arr.[k] <-
                                    {
                                        VertexBufferLayout.StepMode = step
                                        ArrayStride = int64 v.Stride
                                        Attributes = attributes
                                    }
                            )
                            arr
                        | None ->
                            [||]
                    
                    let targets =
                        let result = ResizeArray()
                        desc.Signature.ColorAttachments |> Map.iter (fun slot signature ->
                            while slot >= result.Count do
                                result.Add {
                                    Next = null
                                    Format = TextureFormat.Undefined
                                    Blend = BlendState.Null
                                    WriteMask = ColorWriteMask.None
                                }
                                
                            let blend =
                                let map = desc.BlendState.AttachmentMode.GetValue token
                                match Map.tryFind signature.Name map with
                                | Some mode -> mode
                                | None -> desc.BlendState.Mode.GetValue token
                                 
                            let writeMask =
                                let map = desc.BlendState.AttachmentWriteMask.GetValue token
                                match Map.tryFind signature.Name map with
                                | Some mask -> mask
                                | None -> desc.BlendState.ColorWriteMask.GetValue token
                                
                            result.[slot] <- {
                                Next = null
                                Format = Translations.TextureFormat.ofAardvark signature.Format
                                Blend = Translations.BlendState.ofAardvark blend
                                WriteMask = Translations.ColorWriteMask.ofAardvark writeMask
                            }   
                        )
                        result.ToArray()
                    
                    
                    let frontFacing = desc.RasterizerState.FrontFacing.GetValue token
                    let cullMode = desc.RasterizerState.CullMode.GetValue token
                    
                    let indexType =
                        match desc.IndexType with
                        | Some t ->
                            match Translations.IndexFormat.ofAardvark t with
                            | Some f -> f
                            | None -> IndexFormat.Undefined
                        | None -> IndexFormat.Undefined
                        
                    let depthFormat =
                        match desc.Signature.DepthStencilAttachment with
                        | Some depth -> Translations.TextureFormat.ofAardvark depth
                        | None -> TextureFormat.Undefined
                    
                    let depthWrite =
                        match desc.Signature.DepthStencilAttachment with
                        | Some _ -> 
                            let v = desc.DepthState.WriteMask.GetValue token
                            if v then OptionalBool.True
                            else OptionalBool.False
                        | None ->
                            OptionalBool.Undefined
                    let depthCompare =
                        match desc.Signature.DepthStencilAttachment with
                        | Some _ -> 
                            desc.DepthState.Test.GetValue token
                            |> Translations.CompareFunction.ofDepthTest
                        | None ->
                            CompareFunction.Undefined
                    let depthBias = desc.DepthState.Bias.GetValue(token)
                    
                    let stencilFront = desc.StencilState.ModeFront.GetValue(token)
                    let stencilBack = desc.StencilState.ModeBack.GetValue(token)
                    
                    let pipeline = 
                        device.CreateRenderPipeline {
                            Label = null
                            Layout = layout
                            Vertex = {
                                Module = shader
                                EntryPoint = "vertex"
                                Buffers = buffers
                                Constants = [||]
                            }
                            Fragment = {
                                Module = shader
                                EntryPoint = "fragment"
                                Targets = targets
                                Constants = [||]
                            }
                            Primitive = {
                                Topology = Translations.PrimitiveTopology.ofAardvark desc.Mode
                                StripIndexFormat = indexType
                                FrontFace = Translations.FrontFace.ofAardvark frontFacing
                                CullMode = Translations.CullMode.ofAardvark cullMode
                                UnclippedDepth = false
                            }
                            DepthStencil = {
                                Format = depthFormat
                                DepthWriteEnabled = depthWrite
                                DepthCompare = depthCompare
                                StencilFront = {
                                    Compare = Translations.CompareFunction.ofComparisonFunction stencilFront.Comparison
                                    FailOp = Translations.StencilOperation.ofAardvark stencilFront.Fail
                                    PassOp = Translations.StencilOperation.ofAardvark stencilFront.Pass
                                    DepthFailOp = Translations.StencilOperation.ofAardvark stencilFront.DepthFail
                                }
                                StencilBack = {
                                    Compare = Translations.CompareFunction.ofComparisonFunction stencilBack.Comparison
                                    FailOp = Translations.StencilOperation.ofAardvark stencilBack.Fail
                                    PassOp = Translations.StencilOperation.ofAardvark stencilBack.Pass
                                    DepthFailOp = Translations.StencilOperation.ofAardvark stencilBack.DepthFail
                                }
                                StencilReadMask = int (desc.StencilState.ModeFront.GetValue(token).CompareMask.Value)
                                StencilWriteMask = int (desc.StencilState.WriteMaskFront.GetValue(token).Value)
                                DepthBias = int depthBias.Constant // TODO????
                                DepthBiasSlopeScale = float32 depthBias.SlopeScale
                                DepthBiasClamp = float32 depthBias.Clamp
                            }
                            Multisample = {
                                Count = desc.Signature.Samples
                                Mask = 0xFFFFFFFF
                                AlphaToCoverageEnabled = false
                            }
                        }
                    
                    
                    pipeline
                override x.Destroy(pipeline) =
                    pipeline.Dispose()
                override x.TryUpdate(pipeline, cmd, token) =
                    false
                    
                override x.AfterDestroy() =
                    pipelineCache.Remove(desc) |> ignore
                    
            }
        )
                
    member x.CreateUniformBuffer(template : FShade.WGSL.WGSLUniformBuffer, uniforms : IUniformProvider) =
        let args = 
            template.ubFields |> List.map (fun f ->
                match uniforms.TryGetUniform(Ag.Scope.Root, Symbol.Create f.ufName)  with
                | Some v -> v
                | None ->
                    match Uniforms.tryGetDerivedUniform f.ufName uniforms with
                    | Some v -> v
                    | None ->
                        failwith $"missing uniform: {f.ufName}"
            )
        uboCache.GetOrCreate((template, args), fun (template, args) ->
            //let buffer = device.CreateBuffer { Label = null; Next = null; Size = int64 template.ubSize; Usage = BufferUsage.CopyDst ||| BufferUsage.Uniform; MappedAtCreation = false }
            let writers =
                (template.ubFields, args) ||> List.map2 (fun f v ->
                    let offset = nativeint f.ufOffset
                    offset, v, UniformWriters.getWriter 0 f.ufType v.ContentType
                )
               
            let data = Array.create template.ubSize 0uy
            { new AdaptiveResource<Buffer>() with
                override x.Create(enc, token) =
                    let buffer = device.CreateBuffer { Label = null; Next = null; Size = int64 template.ubSize; Usage = BufferUsage.CopyDst ||| BufferUsage.Uniform; MappedAtCreation = false }
                    use ptr = fixed data
                    writers |> List.iter (fun (offset, value, writer) ->
                        writer.Write(token, value, NativePtr.toNativeInt ptr + offset)
                    )
                    enc.Upload(data, buffer)
                    buffer
                override x.TryUpdate(buffer, enc, token) =
                    use ptr = fixed data
                    writers |> List.iter (fun (offset, value, writer) ->
                        writer.Write(token, value, NativePtr.toNativeInt ptr + offset)
                    )
                    enc.Upload(data, buffer)
                    true
                override x.Destroy(buffer) =
                    buffer.Dispose()
                    
                override x.BeforeCreate() =
                    args |> List.iter acquire
                    
                override x.AfterDestroy() =
                    args |> List.iter release
                    uboCache.Remove((template, args)) |> ignore
            }
        )
        
    member x.CreateBindGroups(program : ShaderProgram, uniforms : IUniformProvider) =
        let iface = program.ShaderCode.iface
        
        let groupBindings =
            Array.create program.BindGroupCount []
        
        for KeyValue(name, ubo) in iface.uniformBuffers do
            let b = x.CreateUniformBuffer(ubo, uniforms)
            groupBindings.[ubo.ubGroup] <- (AdaptiveBindGroupEntry.Buffer(ubo.ubBinding, b)) :: groupBindings.[ubo.ubGroup]
        
        for KeyValue(name, ssb) in iface.storageBuffers do
            match uniforms.TryGetUniform(Ag.Scope.Root, Symbol.Create ssb.ssbName) with
            | Some data ->
                if typeof<IBuffer>.IsAssignableFrom data.ContentType then
                    let buffer = x.CreateBuffer(BufferUsage.Storage, data)
                    groupBindings.[ssb.ssbGroup] <- (AdaptiveBindGroupEntry.Buffer(ssb.ssbBinding, buffer)) :: groupBindings.[ssb.ssbGroup]
                elif data.ContentType.IsArray then
                    let srcType = data.ContentType.GetElementType()
                    let dstType = WGSLType.toType ssb.ssbType
                    let conv = PrimitiveValueConverter.getArrayConverter srcType dstType
                    let b = x.CreateBuffer(BufferUsage.Storage, AVal.custom (fun t -> ArrayBuffer(conv(data.GetValueUntyped(t) :?> System.Array))))              
                    groupBindings.[ssb.ssbGroup] <- (AdaptiveBindGroupEntry.Buffer(ssb.ssbBinding, b)) :: groupBindings.[ssb.ssbGroup]
                else
                    failwith $"unsupported storage buffer type: {data.ContentType}"
            | None ->
                ()
        
        for KeyValue(name, tex) in iface.images do
            match uniforms.TryGetUniform(Ag.Scope.Root, Symbol.Create tex.imageName) with
            | Some data ->
                let texture = x.CreateTexture(data)
                let view = x.CreateStorageTextureView(texture, 0, TextureUsage.StorageBinding ||| TextureUsage.CopyDst ||| TextureUsage.CopySrc)
                groupBindings.[tex.imageGroup] <- (AdaptiveBindGroupEntry.TextureView(tex.imageBinding, view)) :: groupBindings.[tex.imageGroup]
            | None ->
                ()
        
        for KeyValue(name, tex) in iface.textures do
            match uniforms.TryGetUniform(Ag.Scope.Root, Symbol.Create tex.textureName) with
            | Some data ->
                let texture = x.CreateTexture(data)
                let view = x.CreateStorageTextureView(texture, 0, TextureUsage.TextureBinding ||| TextureUsage.CopyDst ||| TextureUsage.CopySrc)
                groupBindings.[tex.textureGroup] <- (AdaptiveBindGroupEntry.TextureView(tex.textureBinding, view)) :: groupBindings.[tex.textureGroup]
            | None ->
                ()
        
        for KeyValue(name, sam) in iface.samplers do
            let sampler = program.Samplers.[sam.samplerGroup].[sam.samplerBinding]
            groupBindings.[sam.samplerGroup] <- (AdaptiveBindGroupEntry.Sampler(sam.samplerBinding, sampler)) :: groupBindings.[sam.samplerGroup]
            
        bindGroupsCache.GetOrCreate((program.BindGroupLayouts, groupBindings), fun (bindGroupLayouts, groupBindings) ->
            let program = ()
            let acquire() =
                for group in groupBindings do
                    for entry in group do
                        match entry with
                        | AdaptiveBindGroupEntry.Buffer(_, b) -> b.Acquire()
                        | AdaptiveBindGroupEntry.Sampler(_, s) -> s.AddRef()
                        | AdaptiveBindGroupEntry.TextureView(_, t) -> t.Acquire()
            
            let release() =
                for group in groupBindings do
                    for entry in group do
                        match entry with
                        | AdaptiveBindGroupEntry.Buffer(_, b) -> b.Release()
                        | AdaptiveBindGroupEntry.Sampler(_, s) -> s.Release()
                        | AdaptiveBindGroupEntry.TextureView(_, t) -> t.Release()
            
            
            { new AdaptiveResource<BindGroup[]>() with
                
                override x.BeforeCreate() =
                    acquire()
                
                override x.AfterDestroy() =
                    release()
                
                override _.Create(enc, token) =
                    groupBindings |> Array.mapi (fun group entries ->
                        let entries = 
                            entries |> List.map (fun e ->
                                match e with
                                | AdaptiveBindGroupEntry.Buffer(bid, b) ->
                                    BindGroupEntry.Buffer(bid, b.GetHandle(enc, token))
                                | AdaptiveBindGroupEntry.Sampler(bid, s) ->
                                    BindGroupEntry.Sampler(bid, s)
                                | AdaptiveBindGroupEntry.TextureView(bid, t) ->
                                    BindGroupEntry.TextureView(bid, t.GetHandle(enc, token))
                            )
                        device.CreateBindGroup {
                            Label = null
                            Layout = bindGroupLayouts.[group]
                            Entries = List.toArray entries
                        }
                    )
                    
                override x.TryUpdate(groups, enc, token) =
                    groupBindings |> Array.forall (fun entries ->
                        entries |> List.forall (fun e ->
                            match e with
                            | AdaptiveBindGroupEntry.Buffer(_, b) -> b.TryUpdate(enc, token)
                            | AdaptiveBindGroupEntry.Sampler(_, s) -> true
                            | AdaptiveBindGroupEntry.TextureView(_, t) -> t.TryUpdate(enc, token)
                        )    
                    )
                    
                override x.Destroy(groups) =
                    release()
                    for group in groups do group.Dispose()
            }
        )
      
    member x.CreateVertexBuffers(program : ShaderProgram, vertexAttributes : IAttributeProvider, instanceAttributes : IAttributeProvider) =
        let iface = program.ShaderCode.iface
        let inputs =
            iface.inputs |> List.choose (fun p ->
                
                let ofBufferView (perInstance : bool) (att : BufferView) =
                    match att.SingleValue with
                    | Some singleValue ->
                        let buffer = x.CreateSingleValueBuffer(BufferUsage.Vertex, singleValue)
                        Some (p.paramLocation, buffer)
                    | None ->
                        let buffer = x.CreateBuffer(BufferUsage.Vertex, att.Buffer)
                        Some (p.paramLocation, buffer)
                
                match vertexAttributes.TryGetAttribute (Symbol.Create p.paramSemantic) with
                | Some att -> ofBufferView false att
                | None ->
                    match instanceAttributes.TryGetAttribute (Symbol.Create p.paramSemantic) with
                    | Some att -> ofBufferView true att
                    | None ->
                        None
                
                
            )
        inputs |> Array.ofList
            
            
        
                
