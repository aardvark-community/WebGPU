namespace Aardvark.Rendering.WebGPU

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
            ImmediateDataRangeByteSize = 0
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

        
    
    member x.Enqueue(enc : CommandEncoder, input : Texture, inputLevel : int, output : Texture, outputLevel : int) =
        
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
        
        