namespace WebGPU
    
open System
open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open Aardvark.Base
open WebGPU
open Microsoft.FSharp.NativeInterop

#nowarn "9"
    

type ITextureFormatVisitor<'r> =
    abstract member Accept<'t when 't : unmanaged> : Col.Format -> 'r
    
[<AutoOpen>]
module TextureFormatExtensions =
    type TextureFormat with
        member fmt.Visit (v : ITextureFormatVisitor<'r>) =
            match fmt with
            | TextureFormat.R8Sint -> v.Accept<int8>(Col.Format.Gray)
            | TextureFormat.R8Uint -> v.Accept<uint8>(Col.Format.Gray)
            | TextureFormat.R8Snorm -> v.Accept<int8>(Col.Format.Gray)
            | TextureFormat.R8Unorm -> v.Accept<uint8>(Col.Format.Gray)
            | TextureFormat.R16Sint -> v.Accept<int16>(Col.Format.Gray)
            | TextureFormat.R16Uint -> v.Accept<uint16>(Col.Format.Gray)
            | TextureFormat.R16Unorm -> v.Accept<uint16>(Col.Format.Gray)
            | TextureFormat.R16Float -> v.Accept<System.Half>(Col.Format.Gray)
            | TextureFormat.R32Sint -> v.Accept<int32>(Col.Format.Gray)
            | TextureFormat.R32Uint -> v.Accept<uint32>(Col.Format.Gray)
            | TextureFormat.R32Float -> v.Accept<float32>(Col.Format.Gray)
            | TextureFormat.RG8Sint -> v.Accept<int8>(Col.Format.RG)
            | TextureFormat.RG8Uint -> v.Accept<uint8>(Col.Format.RG)
            | TextureFormat.RG8Snorm -> v.Accept<int8>(Col.Format.RG)
            | TextureFormat.RG8Unorm -> v.Accept<uint8>(Col.Format.RG)
            | TextureFormat.RG16Sint -> v.Accept<int16>(Col.Format.RG)
            | TextureFormat.RG16Uint -> v.Accept<uint16>(Col.Format.RG)
            | TextureFormat.RG32Sint -> v.Accept<int32>(Col.Format.RG)
            | TextureFormat.RG32Uint -> v.Accept<uint32>(Col.Format.RG)
            | TextureFormat.RG32Float -> v.Accept<float32>(Col.Format.RG)
            | TextureFormat.RGBA8Sint -> v.Accept<int8>(Col.Format.RGBA)
            | TextureFormat.RGBA8Uint -> v.Accept<uint8>(Col.Format.RGBA)
            | TextureFormat.RGBA8Snorm -> v.Accept<int8>(Col.Format.RGBA)
            | TextureFormat.RGBA8Unorm -> v.Accept<uint8>(Col.Format.RGBA)
            | TextureFormat.RGBA8UnormSrgb -> v.Accept<uint8>(Col.Format.RGBA)
            | TextureFormat.RGBA16Sint -> v.Accept<int16>(Col.Format.RGBA)
            | TextureFormat.RGBA16Uint -> v.Accept<uint16>(Col.Format.RGBA)
            | TextureFormat.BGRA8Unorm -> v.Accept<uint8>(Col.Format.BGRA)
            | TextureFormat.BGRA8UnormSrgb -> v.Accept<uint8>(Col.Format.BGRA)
            | TextureFormat.RGBA16Float -> v.Accept<System.Half>(Col.Format.RGBA)
            | TextureFormat.RGBA32Sint -> v.Accept<int32>(Col.Format.RGBA)
            | TextureFormat.RGBA32Uint -> v.Accept<uint32>(Col.Format.RGBA)
            | TextureFormat.RGBA32Float -> v.Accept<float32>(Col.Format.RGBA)
            | TextureFormat.Depth16Unorm -> v.Accept<uint16>(Col.Format.Gray)
            | TextureFormat.Depth24Plus -> v.Accept<uint32>(Col.Format.Gray)
            | TextureFormat.Depth24PlusStencil8 -> v.Accept<uint32>(Col.Format.Gray)
            | TextureFormat.Depth32Float -> v.Accept<float32>(Col.Format.Gray)
            | TextureFormat.Stencil8 -> v.Accept<uint8>(Col.Format.Gray)
            | fmt -> failwithf "bad visitor format: %A" fmt
            
            
type NativeImage =
    {
        Format          : PixFormat
        Size            : V3i
        BytesPerRow     : nativeint
        BytesPerImage   : nativeint
        Data            : nativeint
    }
            
    
[<AbstractClass; Sealed>]
type ImageExtensions private() =
    static member internal CopyImageToTexture(this : CommandEncoder, data : nativeint, tex : Texture, dstLevel : int, dstOffset : V3i, size : V3i, volumeInfo : Type -> int -> Col.Format -> list<Range1i * Choice<Tensor4Info, obj>>) =
        tex.Format.Visit {
            new ITextureFormatVisitor<_> with
                member x.Accept<'a when 'a : unmanaged>(fmt : Col.Format) =
                    let device = this.Device
                    //let size = V3i(tex.Width, tex.Height, tex.DepthOrArrayLayers)
                    let elementSize = sizeof<'a>
                    let channels = fmt.ChannelCount()
                    let infos = volumeInfo typeof<'a> elementSize fmt
                    
                    // https://developer.mozilla.org/en-US/docs/Web/API/GPUCommandEncoder/copyBufferToTexture
                    let pixelSize = channels * elementSize
                    let bpr = size.X * pixelSize
                    let fakebpr =
                        let align = Fun.LeastCommonMultiple(pixelSize, 256)
                        if bpr % align = 0 then bpr
                        else (bpr / align + 1) * align
                    
                    use tmp =
                        device.CreateBuffer {
                            Next = null
                            Label = null
                            Usage = BufferUsage.CopySrc ||| BufferUsage.MapWrite
                            Size = int64 fakebpr * int64 size.Y
                            MappedAtCreation = true
                        }
                        
                    let tmpPtr = tmp.GetMappedRange(0L, tmp.Size)
                    
                    
                    let fakeWidth = fakebpr / pixelSize
                    
                    let dy = int64 channels * int64 fakeWidth
                    let dz = dy * int64 size.Y
                    let dstTensor =
                        NativeTensor4<'a>(
                            NativePtr.ofNativeInt tmpPtr,
                            Tensor4Info(
                                0L,
                                V4l(fakeWidth, size.Y, size.Z, channels),
                                V4l(channels, dy, dz, 1L)
                            )
                        )
                    
                    for channelRange, info in infos do
                        let dstPart = dstTensor.SubTensor4(V4l(0, 0, 0, channelRange.Min), V4l(size.X, size.Y, size.Z, 1 + channelRange.Max - channelRange.Min))
                        match info with
                        | Choice1Of2 info ->
                            let src = NativeTensor4<'a>(NativePtr.ofNativeInt data, info)
                            NativeTensor4.copy src dstPart
                        | Choice2Of2 value ->
                            NativeTensor4.set (value :?> 'a) dstPart
                            
                    tmp.Unmap()
                    
                    //use enc = device.CreateCommandEncoder { Label = null; Next = null }
                    let src : TexelCopyBufferInfo =
                        {
                            Layout =
                                {
                                    Offset = 0L
                                    BytesPerRow = fakebpr
                                    RowsPerImage = size.Y
                                }
                            Buffer = tmp
                        }
                    let dst : TexelCopyTextureInfo =
                        {
                            Texture = tex
                            Origin = { X = dstOffset.X; Y = dstOffset.Y; Z = dstOffset.Z }
                            Aspect = TextureAspect.All
                            MipLevel = dstLevel
                        }
                        
                    this.CopyBufferToTexture(src, dst, { Width = size.X; Height = size.Y; DepthOrArrayLayers = 1 })
                    
                    1
        } |> ignore
    
    static member internal CopyImageToTexture2d(this : CommandEncoder, data : nativeint, tex : Texture, dstLevel : int, dstOffset : V2i, size : V2i, volumeInfo : Type -> int -> Col.Format -> list<Range1i * Choice<VolumeInfo, obj>>) =
        ImageExtensions.CopyImageToTexture(this, data, tex, dstLevel, dstOffset.XYO, size.XYI, fun elemType elemSize fmt ->
            volumeInfo elemType elemSize fmt |> List.map (fun (r, i) ->
                match i with
                | Choice1Of2 vi ->
                    let ti =
                        Tensor4Info(
                            vi.Origin,
                            V4l(vi.SX, vi.SY, 1L, vi.SZ),
                            V4l(vi.DX, vi.DY, vi.DY * vi.SY, vi.DZ)
                        )
                    
                    r, Choice1Of2 ti
                | Choice2Of2 v ->
                    r, Choice2Of2 v
            )
            
            
            
        )
    [<Extension>]
    static member CopyImageToTexture(this : CommandEncoder, data : PixImage, tex : Texture, dstLevel : int) =
        data.Visit {
            new IPixImageVisitor<_> with
                member x.Visit (image: PixImage<'i>) =
                    let gc = GCHandle.Alloc(image.Volume.Data, GCHandleType.Pinned)
                    let ptr = gc.AddrOfPinnedObject()
                    ImageExtensions.CopyImageToTexture2d(this, ptr, tex, dstLevel, V2i.Zero, data.Size, fun typ _ c ->
                        if typ <> typeof<'i> then failwithf $"bad channel-type {typeof<'i>} (expected {typ})"
                        
                        if c = image.Format then
                            [Range1i(0, c.ChannelCount() - 1), Choice1Of2 image.Volume.Info]
                        else
                            match c with
                            | Col.Format.RGBA ->
                                match image.Format with
                                | Col.Format.BGRA ->
                                    let info = image.Volume.Info
                                    let getChannel i = info.SubVolume(V3l(0,0,i), V3l(info.SX, info.SY, 1L))
                                    
                                    [
                                        Range1i(0, 0), Choice1Of2 (getChannel 2)
                                        Range1i(1, 1), Choice1Of2 (getChannel 1)
                                        Range1i(2, 2), Choice1Of2 (getChannel 0)
                                        Range1i(3, 3), Choice1Of2 (getChannel 3)
                                    ]
                                | Col.Format.RGB ->
                                    let value =
                                        if typ = typeof<byte> then 255uy :> obj
                                        elif typ = typeof<int8> then 0y :> obj
                                        elif typ = typeof<float32> then 1.0f :> obj
                                        else failwith $"bad channel-type {typ}"
                                        
                                    [Range1i(0, 2), Choice1Of2 image.Volume.Info; Range1i(3,3), Choice2Of2 value]
                                | _ ->
                                    failwith $"bad format {image.Format} (expected {c})"
                            | _ ->
                                failwith $"bad format {image.Format} (expected {c})"
                            
                    )
                    gc.Free()
                    1
        } |> ignore


    [<Extension>]
    static member CopyImageToTexture(this : CommandEncoder, data : NativeImage, tex : Texture, dstLevel : int, dstOffset : V3i) =
        ImageExtensions.CopyImageToTexture(this, data.Data, tex, dstLevel, dstOffset, data.Size, fun elementType elementSize dstFormat ->
            if data.Format.Type.GetCLRSize() <> elementSize then
                failwith $"bad element-size {elementSize} (expected {data.Format.Type.GetCLRSize()})"
         
            if data.Format.Format = dstFormat then
                let channels = data.Format.ChannelCount
                let info =
                    Tensor4Info(
                        0L,
                        V4l(data.Size.X, data.Size.Y, data.Size.Z, channels),
                        V4l(int64 channels, int64 data.BytesPerRow / int64 elementSize, int64 data.BytesPerImage / int64 elementSize, 1L)
                    )
                
                [Range1i(0, dstFormat.ChannelCount() - 1), Choice1Of2 info]
            else
                failwith $"bad format {data.Format.Format} (expected {dstFormat})"
        )
