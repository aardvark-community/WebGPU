namespace Aardvark.Rendering.WebGPU

open System
open System.Threading
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
                        Some (p.paramLocation, { Type = elemType; Stride = 0; PerInstance = false })
                    | None ->
                        let t = att.ElementType
                        let stride =
                            if att.Stride = 0 then t.GetCLRSize()
                            else att.Stride
                        Some (p.paramLocation, { Type = t; Stride = stride; PerInstance = perInstance })
                
                match o.VertexAttributes.TryGetAttribute (Symbol.Create p.paramSemantic) with
                | ValueSome att -> ofBufferView false att
                | ValueNone ->
                    match o.InstanceAttributes.TryGetAttribute (Symbol.Create p.paramSemantic) with
                    | ValueSome att -> ofBufferView true att
                    | ValueNone -> None
                        
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
    let uboCache = ConcurrentDict<FShade.GLSL.GLSLUniformBuffer * list<IAdaptiveValue>, AdaptiveResource<Buffer>>(Dict())
    let bindGroupsCache = ConcurrentDict<MapExt<int, BindGroupLayout> * array<list<AdaptiveBindGroupEntry>>, AdaptiveResource<BindGroup[]>>(Dict())
    
    let getBlitter (fmt : TextureFormat) =
        Blitter.Get(device, fmt)
    
    static let acquire (v : IAdaptiveValue) =
        match v with
        | :? IAdaptiveResource as r -> r.Acquire()
        | _ -> ()
        
    static let release (v : IAdaptiveValue) =
        match v with
        | :? IAdaptiveResource as r -> r.Release()
        | _ -> ()
    
    member private x.CreateSingleValueBuffer(usage : BufferUsage, value : IAdaptiveValue) =
        bufferCache.GetOrCreate((usage, value), fun (usage, adaptiveValue) ->
            let usage = usage ||| BufferUsage.CopyDst ||| BufferUsage.CopySrc
            
            let inline uploadSingleValue (cmd : CommandEncoder) (res : Buffer) (value : obj)=
                match value with
                | :? V4f as value -> cmd.Upload([|value|], res)
                | :? C4f as value -> cmd.Upload([|value|], res)
                | :? V4i as value -> cmd.Upload([|value|], res)
                | :? C4b as value -> cmd.Upload([|value|], res)
                | _ -> failwith $"bad singlevalue: {value.GetType().FullName}"
            
            { new AdaptiveResource<Buffer>() with
                override x.Create(cmd, token) =
                    let value = adaptiveValue.GetValueUntyped token
                    let res =
                        device.CreateBuffer {
                            Label = null
                            Next = null
                            Size = adaptiveValue.ContentType.GetCLRSize()
                            Usage = usage
                            MappedAtCreation = false
                        }
                    
                    uploadSingleValue cmd res value
                    res
                    
                override x.Destroy b =
                    b.Dispose()
                    
                override x.TryUpdate(buffer, cmd, token) =
                    let value = adaptiveValue.GetValueUntyped token
                    uploadSingleValue cmd buffer value
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
                        | :? Buffer ->
                            false
                        | _ ->
                            let size = value.GetSizeInBytes()
                            if size = buffer.Size then
                                cmd.CopyIBufferToBuffer(value, buffer)
                                true
                            else
                                false
                    else
                        match value.GetValueUntyped token with
                        | :? Buffer as b ->
                            b = buffer
                        | _ ->
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
                    let img = PixImage.Load(t.FileName)
                    let fmt = TextureFormat.ofPixFormat img.PixFormat t.TextureParams |> Translations.TextureFormat.ofAardvark
                    
                    let levels =
                        if t.TextureParams.wantMipMaps then
                            mipMapLevels2d img.Size
                        else
                            1
                            
                    let tex =
                        device.CreateTexture {
                            Next = null
                            Label = null
                            Usage = TextureUsage.StorageBinding ||| TextureUsage.TextureBinding ||| TextureUsage.CopyDst ||| TextureUsage.CopySrc
                            Dimension = TextureDimension.D2D
                            Size = { Width = img.Size.X; Height = img.Size.Y; DepthOrArrayLayers = 1 }
                            Format = fmt
                            MipLevelCount = levels
                            SampleCount = 1
                            ViewFormats = [| fmt |]
                        }
                        
                    cmd.CopyImageToTexture(img, tex, 0, 0)
                        
                    if levels > 1 then
                        let blitter = getBlitter fmt
                        for l in 1 .. levels - 1 do
                            blitter.Enqueue(cmd, tex, l - 1, tex, l)
                        
                    tex
                
                | :? INativeTexture as t ->
                    let fmt = Translations.TextureFormat.ofAardvark t.Format
                    let level0 = t.[0,0]
                    
                    
                    let levels, generate =
                        if t.WantMipMaps then
                            if t.MipMapLevels > 1 then
                                t.MipMapLevels, false
                            else
                                let levels = 
                                    match t.Dimension with
                                    | TextureDimension.Texture1D -> mipMapLevels1d level0.Size.X
                                    | TextureDimension.Texture2D | TextureDimension.TextureCube -> mipMapLevels2d level0.Size.XY
                                    | _ -> mipMapLevels3d level0.Size.XYZ
                                levels, true
                        else
                            1, false
                    
                    let dim =
                        match t.Dimension with
                        | TextureDimension.Texture1D -> TextureDimension.D1D
                        | TextureDimension.Texture2D -> TextureDimension.D2D
                        | TextureDimension.TextureCube -> TextureDimension.D2D
                        | _ -> TextureDimension.D3D
                        
                    let texSize =
                        match t.Dimension with
                        | TextureDimension.Texture1D ->V3i(level0.Size.X, t.Count, 1)
                        | TextureDimension.Texture2D -> V3i(level0.Size.XY, t.Count)
                        | TextureDimension.TextureCube -> V3i(level0.Size.XY, 6*t.Count)
                        | _ -> level0.Size
                    
                    
                    let tex =
                        device.CreateTexture {
                            Next = null
                            Label = null
                            Usage = TextureUsage.StorageBinding ||| TextureUsage.TextureBinding ||| TextureUsage.CopyDst ||| TextureUsage.CopySrc
                            Dimension = dim
                            Size = { Width = texSize.X; Height = texSize.Y; DepthOrArrayLayers = texSize.Z }
                            Format = fmt
                            MipLevelCount = levels
                            SampleCount = 1
                            ViewFormats = [| fmt |]
                        }
                        
                    
                    cmd.CopyImageToTexture(t, tex)
                            
                    if generate then
                        let blitter = getBlitter fmt
                        for l in 1 .. levels - 1 do
                            blitter.Enqueue(cmd, tex, l - 1, tex, l)
                        
                    tex
                    
                | :? PixTexture2d as t ->
                    let fmt = TextureFormat.ofPixFormat t.PixImageMipMap.[0].PixFormat t.TextureParams |> Translations.TextureFormat.ofAardvark
                    let level0 = t.PixImageMipMap.[0]
                    
                    let levels, generate =
                        if t.TextureParams.wantMipMaps then
                            if t.PixImageMipMap.LevelCount > 1 then
                                t.PixImageMipMap.LevelCount, false
                            else
                                mipMapLevels2d level0.Size.XY, true
                        else
                            1, false
                    
                    let tex =
                        device.CreateTexture {
                            Next = null
                            Label = null
                            Usage = TextureUsage.StorageBinding ||| TextureUsage.TextureBinding ||| TextureUsage.CopyDst ||| TextureUsage.CopySrc
                            Dimension = TextureDimension.D2D
                            Size = { Width = level0.Size.X; Height = level0.Size.Y; DepthOrArrayLayers = 1 }
                            Format = fmt
                            MipLevelCount = levels
                            SampleCount = 1
                            ViewFormats = [| fmt |]
                        }
                        
                    let copyLevels = min levels t.PixImageMipMap.LevelCount
                    for l in 0 .. copyLevels - 1 do
                        cmd.CopyImageToTexture(t.PixImageMipMap.[l], tex, l, 0)
                            
                    if generate then
                        let blitter = getBlitter fmt
                        for l in 1 .. levels - 1 do
                            blitter.Enqueue(cmd, tex, l - 1, tex, l)
                        
                    tex
                
                | :? PixTexture3d as t ->
                    let fmt = TextureFormat.ofPixFormat t.PixVolume.PixFormat t.TextureParams |> Translations.TextureFormat.ofAardvark
                    let level0 = t.PixVolume
                    
                    let levels =
                        if t.TextureParams.wantMipMaps then
                            mipMapLevels3d level0.Size
                        else
                            1
                    
                    let tex =
                        device.CreateTexture {
                            Next = null
                            Label = null
                            Usage = TextureUsage.StorageBinding ||| TextureUsage.TextureBinding ||| TextureUsage.CopyDst ||| TextureUsage.CopySrc
                            Dimension = TextureDimension.D3D
                            Size = { Width = level0.Size.X; Height = level0.Size.Y; DepthOrArrayLayers = level0.Size.Z }
                            Format = fmt
                            MipLevelCount = levels
                            SampleCount = 1
                            ViewFormats = [| fmt |]
                        }
                        
                    cmd.CopyImageToTexture(t.PixVolume, tex, 0)
                            
                    if levels > 1 then
                        let blitter = getBlitter fmt
                        for l in 1 .. levels - 1 do
                            blitter.Enqueue(cmd, tex, l - 1, tex, l)
                        
                    tex
                    
                | :? PixTextureCube as t ->
                    let fmt = TextureFormat.ofPixFormat t.PixCube.PixFormat t.TextureParams |> Translations.TextureFormat.ofAardvark
                    let anyFace = t.PixCube.[CubeSide.NegativeX]
                    
                    let levels, generate =
                        if t.TextureParams.wantMipMaps then
                            if anyFace.LevelCount > 1 then
                                anyFace.LevelCount, false
                            else
                                mipMapLevels2d anyFace.BaseSize, true
                        else
                            1, false
                    
                    let tex =
                        device.CreateTexture {
                            Next = null
                            Label = null
                            Usage = TextureUsage.StorageBinding ||| TextureUsage.TextureBinding ||| TextureUsage.CopyDst ||| TextureUsage.CopySrc
                            Dimension = TextureDimension.D2D
                            Size = { Width = anyFace.BaseSize.X; Height = anyFace.BaseSize.Y; DepthOrArrayLayers = 6 }
                            Format = fmt
                            MipLevelCount = levels
                            SampleCount = 1
                            ViewFormats = [| fmt |]
                        }
                        
                    let copyLevels = min levels anyFace.LevelCount
                    
                    let slices = [| 0;1; 3;2; 4;5 |]
                    
                    for f in 0 .. 5 do
                        let face = unbox<CubeSide> f
                        let slice = slices.[f]
                        for l in 0 .. copyLevels - 1 do
                            cmd.CopyImageToTexture(t.PixCube.[face].[l], tex, l, slice)
                            
                    if generate then
                        let blitter = getBlitter fmt
                        for l in 1 .. levels - 1 do
                            blitter.Enqueue(cmd, tex, l - 1, tex, l)
                        
                            
                    tex
                
                | :? Texture as t ->
                    t.AddRef()
                    t
                | _ ->
                    failwith ""
            member x.TryUpdate(handle, cmd, token) =
                match value.GetValueUntyped token with
                | :? Texture as t ->
                    t = handle
                | _ ->
                    false
            member x.Destroy(handle) =
                handle.Dispose()
        }
         
    member x.CreateStorageTextureView(tex : AdaptiveResource<Texture>, level : int) =
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
         
    member x.CreateSampledTextureView(tex : AdaptiveResource<Texture>, viewDimension : TextureViewDimension) =
        { new AdaptiveResource<TextureView>() with
            member x.Create(cmd, token) =
                let tex = tex.GetHandle(cmd, token)
                tex.CreateView(TextureUsage.TextureBinding, viewDimension)
                
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
                    let shaders = desc.Shader.ShaderModules
                    
                    let buffers =
                        match MapExt.tryMax desc.InputDescriptions with
                        | Some maxIndex ->
                            let empty =
                                {
                                    VertexBufferLayout.StepMode = VertexStepMode.Undefined
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
                                Module = shaders.[FShade.ShaderStage.Vertex]
                                EntryPoint = "main"
                                Buffers = buffers
                                Constants = [||]
                            }
                            Fragment = {
                                Module = shaders.[FShade.ShaderStage.Fragment]
                                EntryPoint = "main"
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
                
    member x.CreateUniformBuffer(template : FShade.GLSL.GLSLUniformBuffer, uniforms : IUniformProvider) =
        let args = 
            template.ubFields |> List.map (fun f ->
                match uniforms.TryGetUniform(Ag.Scope.Root, Symbol.Create f.ufName)  with
                | ValueSome v -> v
                | ValueNone ->
                    match Uniforms.tryGetDerivedUniform f.ufName uniforms with
                    | ValueSome v -> v
                    | ValueNone ->
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
            groupBindings.[ubo.ubSet] <- (AdaptiveBindGroupEntry.Buffer(ubo.ubBinding, b)) :: groupBindings.[ubo.ubSet]
        
        for KeyValue(name, ssb) in iface.storageBuffers do
            match uniforms.TryGetUniform(Ag.Scope.Root, Symbol.Create ssb.ssbName) with
            | ValueSome data ->
                if typeof<IBuffer>.IsAssignableFrom data.ContentType then
                    let buffer = x.CreateBuffer(BufferUsage.Storage, data)
                    groupBindings.[ssb.ssbSet] <- (AdaptiveBindGroupEntry.Buffer(ssb.ssbBinding, buffer)) :: groupBindings.[ssb.ssbSet]
                elif data.ContentType.IsArray then
                    let srcType = data.ContentType.GetElementType()
                    let dstType = WGSLType.toType ssb.ssbType
                    let conv = PrimitiveValueConverter.getArrayConverter srcType dstType
                    let b = x.CreateBuffer(BufferUsage.Storage, AVal.custom (fun t -> ArrayBuffer(conv(data.GetValueUntyped(t) :?> System.Array))))              
                    groupBindings.[ssb.ssbSet] <- (AdaptiveBindGroupEntry.Buffer(ssb.ssbBinding, b)) :: groupBindings.[ssb.ssbSet]
                else
                    failwith $"unsupported storage buffer type: {data.ContentType}"
            | ValueNone ->
                ()
        
        for KeyValue(name, tex) in iface.images do
            match uniforms.TryGetUniform(Ag.Scope.Root, Symbol.Create tex.imageName) with
            | ValueSome data ->
                let texture = x.CreateTexture(data)
                let view = x.CreateStorageTextureView(texture, 0)
                groupBindings.[tex.imageSet] <- (AdaptiveBindGroupEntry.TextureView(tex.imageBinding, view)) :: groupBindings.[tex.imageSet]
            | ValueNone ->
                ()
        
        for KeyValue(name, tex) in iface.textures do
            let sem = tex.textureSemantics |> List.head // TODO: multiple????
            let viewDim =
                match tex.textureType.dimension with
                | FShade.SamplerDimension.Sampler1d -> TextureViewDimension.D1D
                | FShade.SamplerDimension.Sampler2d -> TextureViewDimension.D2D
                | FShade.SamplerDimension.SamplerCube -> TextureViewDimension.Cube
                | _ -> TextureViewDimension.D3D
                
            match uniforms.TryGetUniform(Ag.Scope.Root, Symbol.Create sem) with
            | ValueSome data ->
                let texture = x.CreateTexture(data)
                let view = x.CreateSampledTextureView(texture, viewDim)
                groupBindings.[tex.textureSet] <- (AdaptiveBindGroupEntry.TextureView(tex.textureBinding, view)) :: groupBindings.[tex.textureSet]
            | ValueNone ->
                ()
        
        for KeyValue(name, sam) in iface.samplerStates do
            let sampler = program.Samplers.[sam.samplerSet].[sam.samplerBinding]
            groupBindings.[sam.samplerSet] <- (AdaptiveBindGroupEntry.Sampler(sam.samplerBinding, sampler)) :: groupBindings.[sam.samplerSet]
            
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
                        Some (p.paramLocation, int64 att.Offset, buffer)
                    | None ->
                        let buffer = x.CreateBuffer(BufferUsage.Vertex, att.Buffer)
                        Some (p.paramLocation, int64 att.Offset, buffer)
                
                match vertexAttributes.TryGetAttribute (Symbol.Create p.paramSemantic) with
                | ValueSome att -> ofBufferView false att
                | ValueNone ->
                    match instanceAttributes.TryGetAttribute (Symbol.Create p.paramSemantic) with
                    | ValueSome att -> ofBufferView true att
                    | ValueNone ->
                        None
                
                
            )
        inputs |> Array.ofList
            
            
        
                
