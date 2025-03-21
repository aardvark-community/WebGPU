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
        
        
        