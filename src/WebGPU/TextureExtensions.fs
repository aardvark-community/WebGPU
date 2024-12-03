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
            
    
[<AbstractClass; Sealed>]
type ImageExtensions private() =
    static member internal CopyImageToTexture(this : CommandEncoder, data : nativeint, tex : Texture, volumeInfo : Type -> int -> Col.Format -> list<Range1i * Choice<VolumeInfo, obj>>) =
        tex.Format.Visit {
            new ITextureFormatVisitor<_> with
                member x.Accept<'a when 'a : unmanaged>(fmt : Col.Format) =
                    let device = this.Device
                    let size = V2i(tex.Width, tex.Height)
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
                    let dstVolume = NativeVolume<'a>(NativePtr.ofNativeInt tmpPtr, VolumeInfo(0L, V3l(fakeWidth, size.Y, channels), V3l(channels, int64 channels * int64 fakeWidth, 1L)))
                    
                    for channelRange, info in infos do
                        let dstPart = dstVolume.SubVolume(V3l(0, 0, channelRange.Min), V3l(size.X, size.Y, 1 + channelRange.Max - channelRange.Min))
                        match info with
                        | Choice1Of2 info ->
                            let src = NativeVolume<'a>(NativePtr.ofNativeInt data, info)
                            NativeVolume.copy src dstPart
                        | Choice2Of2 value ->
                            NativeVolume.set (value :?> 'a) dstPart
                            
                    tmp.Unmap()
                    
                    //use enc = device.CreateCommandEncoder { Label = null; Next = null }
                    let src : ImageCopyBuffer =
                        {
                            Layout =
                                {
                                    Offset = 0L
                                    BytesPerRow = fakebpr
                                    RowsPerImage = size.Y
                                }
                            Buffer = tmp
                        }
                    let dst : ImageCopyTexture =
                        {
                            Texture = tex
                            Origin = { X = 0; Y = 0; Z = 0 }
                            Aspect = TextureAspect.All
                            MipLevel = 0
                        }
                        
                    this.CopyBufferToTexture(src, dst, { Width = size.X; Height = size.Y; DepthOrArrayLayers = 1 })
                    
                    1
        } |> ignore
    
    [<Extension>]
    static member CopyImageToTexture(this : CommandEncoder, data : PixImage, tex : Texture) =
        data.Visit {
            new IPixImageVisitor<_> with
                member x.Visit (image: PixImage<'i>) =
                    let gc = GCHandle.Alloc(image.Volume.Data, GCHandleType.Pinned)
                    let ptr = gc.AddrOfPinnedObject()
                    ImageExtensions.CopyImageToTexture(this, ptr, tex, fun typ _ c ->
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


