namespace rec Aardvark.Rendering.WebGPU

open System.Runtime.CompilerServices
open Microsoft.FSharp.NativeInterop
open Aardvark.Base
open Aardvark.Rendering
open global.WebGPU

#nowarn "9"

[<AbstractClass; Sealed>]
type TextureExtensions private() =
    
    [<Extension>]
    static member CopyImageToTexture(this : CommandEncoder, src : INativeTexture, dst : Texture) =
        let device = this.Device
        let level0 = src.[0,0]
        
        let size =
            match src.Dimension with
            | TextureDimension.Texture1D -> V3i(level0.Size.X, 1, src.Count)
            | TextureDimension.Texture2D | TextureDimension.TextureCube -> V3i(level0.Size.X, level0.Size.Y, src.Count)
            | TextureDimension.Texture3D -> level0.Size
            | _ -> failwith $"bad texture dimension: {src.Dimension}"
        
        let levels =
            if src.WantMipMaps then
                match src.Dimension with
                | TextureDimension.Texture1D -> mipMapLevels1d level0.Size.X
                | TextureDimension.Texture2D | TextureDimension.TextureCube -> mipMapLevels2d level0.Size.XY
                | TextureDimension.Texture3D -> mipMapLevels3d level0.Size
                | _ -> failwith $"bad texture dimension: {src.Dimension}"
            else
                1
        
        let getLevelSize (level : int) =
            let f = 1 <<< level
            match src.Dimension with
            | TextureDimension.Texture1D ->
                V3i(max 1 (size.X / f), 1, src.Count)
            | TextureDimension.Texture2D | TextureDimension.TextureCube ->
                V3i(max 1 (size.X / f), max 1 (size.Y / f), src.Count)
            | TextureDimension.Texture3D ->
                V3i(max 1 (size.X / f), max 1 (size.Y / f), max 1 (size.Z / f))
            | _ ->
                failwith $"bad texture dimension: {src.Dimension}"
                
        let pixelSize = src.Format.PixelSizeInBytes

        let levelInfo =
            Array.init levels (fun l ->
                let s = getLevelSize l
                let bpr = size.X * pixelSize
                
                let fakebpr =
                    let align = Fun.LeastCommonMultiple(pixelSize, 256)
                    if bpr % align = 0 then bpr
                    else (bpr / align + 1) * align
                    
                let sliceSize =
                    match src.Dimension with
                    | TextureDimension.Texture1D -> fakebpr
                    | TextureDimension.Texture2D | TextureDimension.TextureCube -> fakebpr * s.Y
                    | TextureDimension.Texture3D -> fakebpr * s.Y * s.Z
                    | _ -> failwith $"bad texture dimension: {src.Dimension}"
                    
                struct(fakebpr, sliceSize, int64 fakebpr * int64 s.Y * int64 s.Z)
            )
            
        let totalBufferSize = levelInfo |> Array.sumBy(fun struct(_, _, s) -> s)
            
        let level0 = ()
        use tmp =
            device.CreateBuffer {
                Next = null
                Label = null
                Usage = BufferUsage.CopySrc ||| BufferUsage.MapWrite
                Size = totalBufferSize
                MappedAtCreation = true
            }
            
        let mutable bufferOffset = 0L
        let mutable tmpPtr = tmp.GetMappedRange(0L, tmp.Size)
        
        for level in 0 .. levels - 1 do
            let struct (fakebpr, sliceSize, _) = levelInfo.[level]
            for slice in 0 .. src.Count - 1 do
                let data = src.[slice, level]
                data.Use(fun dataPtr ->
                    let src = System.Span<byte>(NativePtr.toVoidPtr (NativePtr.ofNativeInt<byte> dataPtr), sliceSize)
                    let dst = System.Span<byte>(NativePtr.toVoidPtr (NativePtr.ofNativeInt<byte> tmpPtr), sliceSize)
                    src.CopyTo dst
                )
                
                let srcDesc =
                    {
                        Buffer = tmp
                        Layout = {
                            Offset = bufferOffset
                            BytesPerRow = fakebpr
                            RowsPerImage = size.Y
                        }
                    }
                
                let dstDesc : TexelCopyTextureInfo =
                    {
                        Texture = dst
                        MipLevel = level
                        Origin = { X = 0; Y = 0; Z = 0 }
                        Aspect = TextureAspect.All
                    }
                this.CopyBufferToTexture(srcDesc, dstDesc, { Width = size.X; Height = size.Y; DepthOrArrayLayers = size.Z })
                
                bufferOffset <- bufferOffset + int64 sliceSize
                tmpPtr <- tmpPtr + nativeint sliceSize
                
        tmp.Unmap()
        
    [<Extension>]
    static member CreateTexture(device : Device, tex : ITexture) =
        use cmd = device.CreateCommandEncoder { Label = null; Next = null }
        let texture = 
            match tex with
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
                        ViewFormats = [||]
                    }
                    
                cmd.CopyImageToTexture(img, tex, 0, 0)
                    
                if levels > 1 then
                    let blitter = Blitter.Get(device, fmt)
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
                        ViewFormats = [||]
                    }
                    
                
                cmd.CopyImageToTexture(t, tex)
                        
                if generate then
                    let blitter = Blitter.Get(device, fmt)
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
                        ViewFormats = [||]
                    }
                    
                let copyLevels = min levels t.PixImageMipMap.LevelCount
                for l in 0 .. copyLevels - 1 do
                    cmd.CopyImageToTexture(t.PixImageMipMap.[l], tex, l, 0)
                        
                if generate then
                    let blitter = Blitter.Get(device, fmt)
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
                        ViewFormats = [||]
                    }
                    
                cmd.CopyImageToTexture(t.PixVolume, tex, 0)
                        
                if levels > 1 then
                    let blitter = Blitter.Get(device, fmt)
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
                        ViewFormats = [||]
                    }
                    
                let copyLevels = min levels anyFace.LevelCount
                
                let slices = [| 0;1; 3;2; 4;5 |]
                
                for f in 0 .. 5 do
                    let face = unbox<CubeSide> f
                    let slice = slices.[f]
                    for l in 0 .. copyLevels - 1 do
                        cmd.CopyImageToTexture(t.PixCube.[face].[l], tex, l, slice)
                        
                if generate then
                    let blitter = Blitter.Get(device, fmt)
                    for l in 1 .. levels - 1 do
                        blitter.Enqueue(cmd, tex, l - 1, tex, l)
                    
                        
                tex
            
            | :? Texture as t ->
                t.AddRef()
                t
            | _ ->
                failwith ""

        use cmd = cmd.Finish { Label = null }
        task {
            do! device.Queue.Submit [| cmd |]
            return texture
        }

    [<Extension>]
    static member CreateTexture(device : Device, tex : PixImage, ?wantMipMaps : bool) =
        let wantMipMaps = defaultArg wantMipMaps false
        TextureExtensions.CreateTexture(device, PixTexture2d(PixImageMipMap [|tex|], { TextureParams.empty with wantMipMaps = wantMipMaps }))

    [<Extension>]
    static member CreateTexture(device : Device, file : string, ?wantMipMaps : bool) =
        let wantMipMaps = defaultArg wantMipMaps false
        TextureExtensions.CreateTexture(device, FileTexture(file, { TextureParams.empty with wantMipMaps = wantMipMaps }))

    [<Extension>]
    static member CreateTexture(device : Device, format : Aardvark.Rendering.TextureFormat, size : V2i, ?levels : int, ?layers : int, ?samples : int) =
        let layers = defaultArg layers 1
        let levels = defaultArg levels 1
        let samples = defaultArg samples 1
        let fmt = Translations.TextureFormat.ofAardvark format
        device.CreateTexture {
            Next = null
            Label = null
            Usage = TextureUsage.RenderAttachment ||| TextureUsage.StorageBinding ||| TextureUsage.TextureBinding ||| TextureUsage.CopyDst ||| TextureUsage.CopySrc
            Dimension = TextureDimension.D2D
            Size = { Width = size.X; Height = size.Y; DepthOrArrayLayers = layers}
            Format = fmt
            MipLevelCount = levels
            SampleCount = samples
            ViewFormats = [||]
        }

    [<Extension>]
    static member DownloadPixImage(this : Device, tex : Texture, ?level : int, ?offset : V3i, ?size : V3i) =
        
        let level = defaultArg level 0
        let offset = defaultArg offset V3i.Zero
        let s = V3i(tex.Width, tex.Height, tex.DepthOrArrayLayers)
        let f = 1 <<< level
        let levelSize = V3i(max 1 (s.X / f), max 1 (s.Y / f), max 1 (s.Z / f))
        
        let fmt = tex.Format |> Translations.TextureFormat.toAardvark
        let pixFormat = Aardvark.Rendering.TextureFormat.toDownloadFormat fmt
        let size =
            match size with
            | Some s -> s
            | None -> levelSize - offset
        
        let pixelSize = pixFormat.Type.GetCLRSize() * pixFormat.ChannelCount
        
        let bytesPerRow = size.X * pixelSize
        let mutable paddedBytesPerRow = bytesPerRow
        if paddedBytesPerRow % 256 <> 0 then
            paddedBytesPerRow <- (1 + paddedBytesPerRow / 256) * 256
            
        if paddedBytesPerRow % pixelSize <> 0 then
            paddedBytesPerRow <- (1 + paddedBytesPerRow / pixelSize) * pixelSize
            
        let paddedWidth = paddedBytesPerRow / pixelSize
            
        task {
            let sizeInBytes = int64 paddedBytesPerRow * int64 size.Y
            use tmp =
                this.CreateBuffer {
                    Next = null
                    Label = null
                    Usage = BufferUsage.CopyDst ||| BufferUsage.MapRead
                    Size = sizeInBytes
                    MappedAtCreation = false
                }
                
            let src : TexelCopyTextureInfo =
                {
                    Texture = tex
                    MipLevel = level
                    Origin = { X = offset.X; Y = offset.Y; Z = offset.Z }
                    Aspect = TextureAspect.All
                }
            
            let dst : TexelCopyBufferInfo =
                {
                    
                    Layout = { Offset = 0L; BytesPerRow = paddedBytesPerRow; RowsPerImage = size.Y }
                    Buffer = tmp
                }
            
            
            use enc = this.CreateCommandEncoder { Label = null; Next = null }
            enc.CopyTextureToBuffer(src, dst, { Width = size.X; Height = size.Y; DepthOrArrayLayers = size.Z })
            use cmd = enc.Finish { Label = null }
            do! this.Queue.Submit [| cmd |]
            return!
                tmp.Mapped (MapMode.Read, fun ptr ->
                    let img = PixImage.Create(pixFormat, size.X, size.Y)
                    tex.Format.Visit {
                        new ITextureFormatVisitor<_> with
                            member x.Accept<'a when 'a : unmanaged>(fmt : Col.Format) =
                                let channels = fmt.ChannelCount()
                                let pSrc = NativePtr.ofNativeInt<'a> ptr
                                let img = img :?> PixImage<'a>
                                NativeVolume.using img.Volume (fun dst ->
                                    let src =
                                        NativeVolume<'a>(
                                            pSrc,
                                            VolumeInfo(
                                                0L,
                                                V3l(paddedWidth, size.Y, channels),
                                                V3l(channels, paddedWidth, 1)
                                            )
                                        )
                                    src.SubVolume(V3i.Zero, V3i(size.X, size.Y, channels)).CopyTo dst
                                )
                                0
                    } |> ignore
                        
                    img
                )
        }

    [<Extension>]
    static member ClearColor(tex : Texture, value : Color) =
        use enc = tex.Device.CreateCommandEncoder { Label = null; Next = null }
        use view = tex.CreateView TextureUsage.RenderAttachment
        let cc : RenderPassColorAttachment =
            {
                Next = null
                View = view
                DepthSlice = -1
                ResolveTarget = TextureView.Null
                LoadOp = LoadOp.Clear
                StoreOp = StoreOp.Store
                ClearValue = value
            }
             
        use render =
            enc.BeginRenderPass {
                Next = null
                Label = null
                ColorAttachments = [| cc |]
                DepthStencilAttachment = undefined
                OcclusionQuerySet = undefined
                TimestampWrites = undefined
            }
        render.End()
        use cmd = enc.Finish { Label = null }
        tex.Device.Queue.Submit [| cmd |]
         
    [<Extension>]
    static member ClearDepthStencil(tex : Texture, ?depth : float32, ?stencil : int) =
        use enc = tex.Device.CreateCommandEncoder { Label = null; Next = null }
        use view = tex.CreateView TextureUsage.RenderAttachment
        
        let depthLoadOp =
            match depth with
            | Some _ -> LoadOp.Clear
            | None -> LoadOp.Load
        
        let depthStoreOp =
            match depth with
            | Some _ -> StoreOp.Store
            | None -> StoreOp.Discard
            
        let stencilLoadOp =
            match stencil with
            | Some _ -> LoadOp.Clear
            | None -> LoadOp.Load
        
        let stencilStoreOp =
            match stencil with
            | Some _ -> StoreOp.Store
            | None -> StoreOp.Discard
            
        let dd : RenderPassDepthStencilAttachment =
            {
                View = view
                DepthLoadOp = depthLoadOp
                DepthStoreOp = depthStoreOp
                DepthClearValue = defaultArg depth 0.0f
                DepthReadOnly = false
                StencilLoadOp = stencilLoadOp
                StencilStoreOp = stencilStoreOp
                StencilClearValue = defaultArg stencil 0
                StencilReadOnly = true
            }
             
        use render =
            enc.BeginRenderPass {
                Next = null
                Label = null
                ColorAttachments = [| |]
                DepthStencilAttachment = dd
                OcclusionQuerySet = undefined
                TimestampWrites = undefined
            }
        render.End()
        use cmd = enc.Finish { Label = null }
        tex.Device.Queue.Submit [| cmd |]
            
    [<Extension>]
    static member inline Clear(tex : Texture, value : C4f) =
        TextureExtensions.ClearColor(tex, { R = float value.R; G = float value.G; B = float value.B; A = float value.A })
        
    [<Extension>]
    static member inline Clear(tex : Texture, value : C4d) =
        TextureExtensions.ClearColor(tex, { R = value.R; G = value.G; B = value.B; A = value.A })
        
    [<Extension>]
    static member inline Clear(tex : Texture, value : C4b) =
        TextureExtensions.Clear(tex, value.ToC4f())
        
    [<Extension>]
    static member inline Clear(tex : Texture, value : C4us) =
        TextureExtensions.Clear(tex, value.ToC4f())
        
    [<Extension>]
    static member inline Clear(tex : Texture, value : C4ui) =
        TextureExtensions.Clear(tex, value.ToC4f())
        
    [<Extension>]
    static member inline Clear(tex : Texture, value : V4f) =
        TextureExtensions.ClearColor(tex, { R = float value.X; G = float value.Y; B = float value.Z; A = float value.W })
        
    [<Extension>]
    static member inline Clear(tex : Texture, value : int) =
        TextureExtensions.ClearColor(tex, { R = float value; G = float value; B = float value; A = float value })
        
    [<Extension>]
    static member inline Clear(tex : Texture, value : uint32) =
        TextureExtensions.ClearColor(tex, { R = float value; G = float value; B = float value; A = float value })
        
    [<Extension>]
    static member inline Clear(tex : Texture, value : float) =
        TextureExtensions.ClearColor(tex, { R = value; G = value; B = value; A = value })
        
    [<Extension>]
    static member inline Clear(tex : Texture, value : float32) =
        TextureExtensions.ClearColor(tex, { R = float value; G = float value; B = float value; A = float value })
        
    
type Blitter(device : Device, format : TextureFormat) =
    
    static let localSizeX = 8
    static let localSizeY = 8
    
    static let ceilDiv (a : int) (b : int) =
        if a % b = 0 then a / b
        else 1 + a / b
    
    
    static let blitters = Dict<Device * TextureFormat, Blitter>()
    
    
    
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
    
    
    static member Get (device : Device, fmt : TextureFormat) : Blitter =
        lock blitters (fun () ->
            blitters.GetOrCreate((device, fmt), fun (device, fmt) -> new Blitter(device, fmt))    
        )
    
    member x.Run(input : Texture, inputLevel : int, output : Texture, outputLevel : int) : System.Threading.Tasks.Task =
        
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

        
    
    member x.Enqueue(enc : CommandEncoder, input : Texture, inputLevel : int, output : Texture, outputLevel : int) : unit =
        
        let outputSize =
            let f = 1 <<< outputLevel
            V2i(max 1 (output.Width / f), max 1 (output.Height / f))
        
        use output = output.CreateView(TextureUsage.StorageBinding, outputLevel)
        use input = input.CreateView(TextureUsage.TextureBinding, inputLevel)
        
        //use enc = device.CreateCommandEncoder { Label = null; Next = null }
        
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
        
        