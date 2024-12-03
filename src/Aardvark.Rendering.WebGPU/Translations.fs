namespace Aardvark.Rendering.WebGPU

open Aardvark.Base
open Aardvark.Rendering
open global.WebGPU

module Translations =
    //
    // module BufferUsage =
    //     let ofAardvark (usage : Aardvark.Rendering.BufferUsage) =
    //         let mutable res = WebGPU.BufferUsage.None
    //         
    //         if usage.HasFlag (Aardvark.Rendering.BufferUsage.Index) then res <- res ||| WebGPU.BufferUsage.Index
    //         if usage.HasFlag (Aardvark.Rendering.BufferUsage.Vertex) then res <- res ||| WebGPU.BufferUsage.Vertex
    //         if usage.HasFlag (Aardvark.Rendering.BufferUsage.Uniform) then res <- res ||| WebGPU.BufferUsage.Uniform
    //         if usage.HasFlag (Aardvark.Rendering.BufferUsage.Storage) then res <- res ||| WebGPU.BufferUsage.Storage
    //         if usage.HasFlag (Aardvark.Rendering.BufferUsage.Indirect) then res <- res ||| WebGPU.BufferUsage.Indirect
    //         if usage.HasFlag (Aardvark.Rendering.BufferUsage.Read) then res <- res ||| WebGPU.BufferUsage.MapRead
    //         if usage.HasFlag (Aardvark.Rendering.BufferUsage.Write) then res <- res ||| WebGPU.BufferUsage.MapWrite
    //         
    //         
    
    module FilterMode =
        
        let ofAardvark =
            LookupTable.lookup [
                Aardvark.Rendering.FilterMode.Point, FilterMode.Nearest
                Aardvark.Rendering.FilterMode.Linear, FilterMode.Linear
            ]
        let ofFShade =
            LookupTable.lookup [
                FShade.Filter.MinMagPoint, struct(FilterMode.Nearest, FilterMode.Nearest, MipmapFilterMode.Undefined)
                FShade.Filter.MinMagLinear, struct(FilterMode.Linear, FilterMode.Linear, MipmapFilterMode.Undefined)
                FShade.Filter.MinPointMagLinear, struct(FilterMode.Nearest, FilterMode.Linear, MipmapFilterMode.Undefined)
                FShade.Filter.MinLinearMagPoint, struct(FilterMode.Linear, FilterMode.Nearest, MipmapFilterMode.Undefined)
                FShade.Filter.MinMagMipPoint,               struct(FilterMode.Nearest, FilterMode.Nearest, MipmapFilterMode.Nearest)
                FShade.Filter.MinPointMagLinearMipPoint,    struct(FilterMode.Nearest, FilterMode.Linear, MipmapFilterMode.Nearest)
                FShade.Filter.MinMagPointMipLinear,         struct(FilterMode.Nearest, FilterMode.Nearest, MipmapFilterMode.Linear)
                FShade.Filter.MinPointMagMipLinear,         struct(FilterMode.Nearest, FilterMode.Linear, MipmapFilterMode.Linear)
                FShade.Filter.MinLinearMagMipPoint,         struct(FilterMode.Linear, FilterMode.Nearest, MipmapFilterMode.Nearest)
                FShade.Filter.MinLinearMagPointMipLinear,   struct(FilterMode.Linear, FilterMode.Nearest, MipmapFilterMode.Linear)
                FShade.Filter.MinMagLinearMipPoint,         struct(FilterMode.Linear, FilterMode.Linear, MipmapFilterMode.Nearest)
                FShade.Filter.MinMagMipLinear,              struct(FilterMode.Linear, FilterMode.Linear, MipmapFilterMode.Linear)
                FShade.Filter.Anisotropic,                  struct(FilterMode.Linear, FilterMode.Linear, MipmapFilterMode.Linear)
            ]
    
    module AddressMode =
        let ofFShade =
            LookupTable.lookup [
                FShade.WrapMode.Wrap, AddressMode.Repeat
                FShade.WrapMode.Mirror, AddressMode.MirrorRepeat
                FShade.WrapMode.Clamp, AddressMode.ClampToEdge
                FShade.WrapMode.Border, AddressMode.Undefined
                FShade.WrapMode.MirrorOnce, AddressMode.MirrorRepeat
            ]
    
    module BlendOperation =
        let ofAardvark =
            LookupTable.lookup [
                Aardvark.Rendering.BlendOperation.Add, BlendOperation.Add
                Aardvark.Rendering.BlendOperation.Subtract, BlendOperation.Subtract
                Aardvark.Rendering.BlendOperation.ReverseSubtract, BlendOperation.ReverseSubtract
                Aardvark.Rendering.BlendOperation.Minimum, BlendOperation.Min
                Aardvark.Rendering.BlendOperation.Maximum, BlendOperation.Max
            ]
    
    module BlendFactor =
        let ofAardvark =
            LookupTable.lookup [
                Aardvark.Rendering.BlendFactor.One, BlendFactor.One
                Aardvark.Rendering.BlendFactor.Zero, BlendFactor.Zero
                Aardvark.Rendering.BlendFactor.SourceColor, WebGPU.BlendFactor.Src
                Aardvark.Rendering.BlendFactor.InvSourceColor, WebGPU.BlendFactor.OneMinusSrc
                Aardvark.Rendering.BlendFactor.SourceAlpha, WebGPU.BlendFactor.SrcAlpha
                Aardvark.Rendering.BlendFactor.InvSourceAlpha, WebGPU.BlendFactor.OneMinusSrcAlpha
                Aardvark.Rendering.BlendFactor.DestinationColor, WebGPU.BlendFactor.Dst
                Aardvark.Rendering.BlendFactor.InvDestinationColor, WebGPU.BlendFactor.OneMinusDst
                Aardvark.Rendering.BlendFactor.DestinationAlpha, WebGPU.BlendFactor.DstAlpha
                Aardvark.Rendering.BlendFactor.InvDestinationAlpha, WebGPU.BlendFactor.OneMinusDstAlpha
                Aardvark.Rendering.BlendFactor.SourceAlphaSaturate, WebGPU.BlendFactor.SrcAlphaSaturated
                Aardvark.Rendering.BlendFactor.ConstantColor, WebGPU.BlendFactor.Constant
                Aardvark.Rendering.BlendFactor.InvConstantColor, WebGPU.BlendFactor.OneMinusConstant
                Aardvark.Rendering.BlendFactor.SecondarySourceAlpha, WebGPU.BlendFactor.Src1Alpha
                Aardvark.Rendering.BlendFactor.InvSecondarySourceAlpha, WebGPU.BlendFactor.OneMinusSrc1Alpha
                Aardvark.Rendering.BlendFactor.SecondarySourceColor, WebGPU.BlendFactor.Src1
                Aardvark.Rendering.BlendFactor.InvSecondarySourceColor, WebGPU.BlendFactor.OneMinusSrc1
                
                
                
            ]
 
    module TextureFormat =
        let ofAardvark =
            LookupTable.lookup [

                //Aardvark.Rendering.TextureFormat.Bgr8 = 1234
                Aardvark.Rendering.TextureFormat.Bgra8, WebGPU.TextureFormat.BGRA8Unorm
                //Aardvark.Rendering.TextureFormat.R3G3B2,
                //Aardvark.Rendering.TextureFormat.Rgb4 = 32847
                //Aardvark.Rendering.TextureFormat.Rgb5 = 32848
                //Aardvark.Rendering.TextureFormat.Rgb8 = 32849
                //Aardvark.Rendering.TextureFormat.Rgb10 = 32850
                //Aardvark.Rendering.TextureFormat.Rgb12 = 32851
                //Aardvark.Rendering.TextureFormat.Rgb16 = 32852
                //Aardvark.Rendering.TextureFormat.Rgba2, WebGPU.TextureFormat.Rgba2
                //Aardvark.Rendering.TextureFormat.Rgba4 = 32854
                //Aardvark.Rendering.TextureFormat.Rgb5A1 = 32855
                Aardvark.Rendering.TextureFormat.Rgba8, WebGPU.TextureFormat.RGBA8Unorm
                Aardvark.Rendering.TextureFormat.Rgb10A2, WebGPU.TextureFormat.RGB10A2Uint
                //Aardvark.Rendering.TextureFormat.Rgba12, WebGPU.TextureFormat.Rgba12
                Aardvark.Rendering.TextureFormat.Rgba16, WebGPU.TextureFormat.RGBA16Unorm
                Aardvark.Rendering.TextureFormat.R8, WebGPU.TextureFormat.R8Unorm
                Aardvark.Rendering.TextureFormat.R16, WebGPU.TextureFormat.R16Unorm
                Aardvark.Rendering.TextureFormat.Rg8, WebGPU.TextureFormat.RG8Unorm
                Aardvark.Rendering.TextureFormat.Rg16, WebGPU.TextureFormat.RG16Unorm
                Aardvark.Rendering.TextureFormat.R16f, WebGPU.TextureFormat.R16Float
                Aardvark.Rendering.TextureFormat.R32f, WebGPU.TextureFormat.R32Float
                Aardvark.Rendering.TextureFormat.Rg16f, WebGPU.TextureFormat.RG16Float
                Aardvark.Rendering.TextureFormat.Rg32f, WebGPU.TextureFormat.RG32Float
                Aardvark.Rendering.TextureFormat.R8i, WebGPU.TextureFormat.R8Sint
                Aardvark.Rendering.TextureFormat.R8ui, WebGPU.TextureFormat.R8Uint
                Aardvark.Rendering.TextureFormat.R16i, WebGPU.TextureFormat.R16Sint
                Aardvark.Rendering.TextureFormat.R16ui, WebGPU.TextureFormat.R16Uint
                Aardvark.Rendering.TextureFormat.R32i, WebGPU.TextureFormat.R32Sint
                Aardvark.Rendering.TextureFormat.R32ui, WebGPU.TextureFormat.R32Uint
                Aardvark.Rendering.TextureFormat.Rg8i, WebGPU.TextureFormat.RG8Sint
                Aardvark.Rendering.TextureFormat.Rg8ui, WebGPU.TextureFormat.RG8Uint
                Aardvark.Rendering.TextureFormat.Rg16i, WebGPU.TextureFormat.RG16Sint
                Aardvark.Rendering.TextureFormat.Rg16ui, WebGPU.TextureFormat.RG16Uint
                Aardvark.Rendering.TextureFormat.Rg32i, WebGPU.TextureFormat.RG32Sint
                Aardvark.Rendering.TextureFormat.Rg32ui, WebGPU.TextureFormat.RG32Uint
                Aardvark.Rendering.TextureFormat.Rgba32f, WebGPU.TextureFormat.RGBA32Float
                //Aardvark.Rendering.TextureFormat.Rgb32f, WebGPU.TextureFormat.RGB32Float
                Aardvark.Rendering.TextureFormat.Rgba16f, WebGPU.TextureFormat.RGBA16Float
                //Aardvark.Rendering.TextureFormat.Rgb16f, WebGPU.TextureFormat.RGB16Float
                Aardvark.Rendering.TextureFormat.R11fG11fB10f, WebGPU.TextureFormat.RG11B10Ufloat
                Aardvark.Rendering.TextureFormat.Rgb9E5, WebGPU.TextureFormat.RGB9E5Ufloat
                //Aardvark.Rendering.TextureFormat.Srgb8, WebGPU.TextureFormat.RGB8Unorm
                Aardvark.Rendering.TextureFormat.Srgb8Alpha8, WebGPU.TextureFormat.RGBA8UnormSrgb
                Aardvark.Rendering.TextureFormat.Rgba32ui, WebGPU.TextureFormat.RGBA32Uint
                //Aardvark.Rendering.TextureFormat.Rgb32ui, WebGPU.TextureFormat.RGB32Uint
                Aardvark.Rendering.TextureFormat.Rgba16ui, WebGPU.TextureFormat.RGBA16Uint
                //Aardvark.Rendering.TextureFormat.Rgb16ui, WebGPU.TextureFormat.RGB16Uint
                Aardvark.Rendering.TextureFormat.Rgba8ui, WebGPU.TextureFormat.RGBA8Uint
                //Aardvark.Rendering.TextureFormat.Rgb8ui, WebGPU.TextureFormat.RGB8Uint
                Aardvark.Rendering.TextureFormat.Rgba32i, WebGPU.TextureFormat.RGBA32Sint
                //Aardvark.Rendering.TextureFormat.Rgb32i, WebGPU.TextureFormat.RGB32Sint
                Aardvark.Rendering.TextureFormat.Rgba16i, WebGPU.TextureFormat.RGBA16Sint
                //Aardvark.Rendering.TextureFormat.Rgb16i = 36233
                Aardvark.Rendering.TextureFormat.Rgba8i, WebGPU.TextureFormat.RGBA8Sint
                //Aardvark.Rendering.TextureFormat.Rgb8i = 36239
                Aardvark.Rendering.TextureFormat.R8Snorm, WebGPU.TextureFormat.R8Snorm
                Aardvark.Rendering.TextureFormat.Rg8Snorm, WebGPU.TextureFormat.RG8Snorm
                //Aardvark.Rendering.TextureFormat.Rgb8Snorm = 36758
                Aardvark.Rendering.TextureFormat.Rgba8Snorm, WebGPU.TextureFormat.RGBA8Snorm
                Aardvark.Rendering.TextureFormat.R16Snorm, WebGPU.TextureFormat.R16Snorm
                Aardvark.Rendering.TextureFormat.Rg16Snorm, WebGPU.TextureFormat.RG16Snorm
                //Aardvark.Rendering.TextureFormat.Rgb16Snorm = 36762
                Aardvark.Rendering.TextureFormat.Rgba16Snorm, WebGPU.TextureFormat.RGBA16Snorm
                Aardvark.Rendering.TextureFormat.Rgb10A2ui, WebGPU.TextureFormat.RGB10A2Uint
                Aardvark.Rendering.TextureFormat.DepthComponent16, WebGPU.TextureFormat.Depth16Unorm
                Aardvark.Rendering.TextureFormat.DepthComponent24, WebGPU.TextureFormat.Depth24Plus
                Aardvark.Rendering.TextureFormat.DepthComponent32, WebGPU.TextureFormat.Depth32Float
                Aardvark.Rendering.TextureFormat.DepthComponent32f, WebGPU.TextureFormat.Depth32Float
                Aardvark.Rendering.TextureFormat.Depth24Stencil8, WebGPU.TextureFormat.Depth24PlusStencil8
                Aardvark.Rendering.TextureFormat.Depth32fStencil8, WebGPU.TextureFormat.Depth32FloatStencil8
                Aardvark.Rendering.TextureFormat.StencilIndex8, WebGPU.TextureFormat.Stencil8
                // Aardvark.Rendering.TextureFormat.CompressedRgbS3tcDxt1, WebGPU.TextureFormat.BC
                // Aardvark.Rendering.TextureFormat.CompressedSrgbS3tcDxt1 = 35916
                // Aardvark.Rendering.TextureFormat.CompressedRgbaS3tcDxt1 = 33777
                // Aardvark.Rendering.TextureFormat.CompressedSrgbAlphaS3tcDxt1 = 35917
                // Aardvark.Rendering.TextureFormat.CompressedRgbaS3tcDxt3 = 33778            // BC2
                // Aardvark.Rendering.TextureFormat.CompressedSrgbAlphaS3tcDxt3 = 35918
                // Aardvark.Rendering.TextureFormat.CompressedRgbaS3tcDxt5 = 33779            // BC3
                // Aardvark.Rendering.TextureFormat.CompressedSrgbAlphaS3tcDxt5 = 35919
                // Aardvark.Rendering.TextureFormat.CompressedRedRgtc1 = 36283                // BC4
                // Aardvark.Rendering.TextureFormat.CompressedSignedRedRgtc1 = 36284
                // Aardvark.Rendering.TextureFormat.CompressedRgRgtc2 = 36285                 // BC5
                // Aardvark.Rendering.TextureFormat.CompressedSignedRgRgtc2 = 36286
                // Aardvark.Rendering.TextureFormat.CompressedRgbBptcSignedFloat = 36494      // BC6h
                // Aardvark.Rendering.TextureFormat.CompressedRgbBptcUnsignedFloat = 36495
                // Aardvark.Rendering.TextureFormat.CompressedRgbaBptcUnorm = 36492           // BC7
                // Aardvark.Rendering.TextureFormat.CompressedSrgbAlphaBptcUnorm = 36493
            ] 
        let toAardvark =
            LookupTable.lookup [
                WebGPU.TextureFormat.BGRA8Unorm, Aardvark.Rendering.TextureFormat.Bgra8
                WebGPU.TextureFormat.RGBA8Unorm, Aardvark.Rendering.TextureFormat.Rgba8
                WebGPU.TextureFormat.R8Unorm, Aardvark.Rendering.TextureFormat.R8
                WebGPU.TextureFormat.RG8Unorm, Aardvark.Rendering.TextureFormat.Rg8
                WebGPU.TextureFormat.R16Float, Aardvark.Rendering.TextureFormat.R16f
                WebGPU.TextureFormat.R32Float, Aardvark.Rendering.TextureFormat.R32f
                WebGPU.TextureFormat.RG16Float, Aardvark.Rendering.TextureFormat.Rg16f
                WebGPU.TextureFormat.RG32Float, Aardvark.Rendering.TextureFormat.Rg32f
                WebGPU.TextureFormat.R8Sint, Aardvark.Rendering.TextureFormat.R8i
                WebGPU.TextureFormat.R8Uint, Aardvark.Rendering.TextureFormat.R8ui
                WebGPU.TextureFormat.R16Sint, Aardvark.Rendering.TextureFormat.R16i
                WebGPU.TextureFormat.R16Uint, Aardvark.Rendering.TextureFormat.R16ui
                WebGPU.TextureFormat.R32Sint, Aardvark.Rendering.TextureFormat.R32i
                WebGPU.TextureFormat.R32Uint, Aardvark.Rendering.TextureFormat.R32ui
                WebGPU.TextureFormat.RG8Sint, Aardvark.Rendering.TextureFormat.Rg8i
                WebGPU.TextureFormat.RG8Uint, Aardvark.Rendering.TextureFormat.Rg8ui
                WebGPU.TextureFormat.RG16Sint, Aardvark.Rendering.TextureFormat.Rg16i
                WebGPU.TextureFormat.RG16Uint, Aardvark.Rendering.TextureFormat.Rg16ui
                WebGPU.TextureFormat.RG32Sint, Aardvark.Rendering.TextureFormat.Rg32i
                WebGPU.TextureFormat.RG32Uint, Aardvark.Rendering.TextureFormat.Rg32ui
                WebGPU.TextureFormat.RGBA32Float, Aardvark.Rendering.TextureFormat.Rgba32f
                WebGPU.TextureFormat.RGBA16Float, Aardvark.Rendering.TextureFormat.Rgba16f
                WebGPU.TextureFormat.RG11B10Ufloat, Aardvark.Rendering.TextureFormat.R11fG11fB10f
                WebGPU.TextureFormat.RGB9E5Ufloat, Aardvark.Rendering.TextureFormat.Rgb9E5
                WebGPU.TextureFormat.RGBA8UnormSrgb, Aardvark.Rendering.TextureFormat.Srgb8Alpha8
                WebGPU.TextureFormat.RGBA32Uint, Aardvark.Rendering.TextureFormat.Rgba32ui
                WebGPU.TextureFormat.RGBA16Uint, Aardvark.Rendering.TextureFormat.Rgba16ui
                WebGPU.TextureFormat.RGBA8Uint, Aardvark.Rendering.TextureFormat.Rgba8ui
                WebGPU.TextureFormat.RGBA32Sint, Aardvark.Rendering.TextureFormat.Rgba32i
                WebGPU.TextureFormat.RGBA16Sint, Aardvark.Rendering.TextureFormat.Rgba16i
                WebGPU.TextureFormat.RGBA8Sint, Aardvark.Rendering.TextureFormat.Rgba8i
                WebGPU.TextureFormat.R8Snorm, Aardvark.Rendering.TextureFormat.R8Snorm
                WebGPU.TextureFormat.RG8Snorm, Aardvark.Rendering.TextureFormat.Rg8Snorm
                WebGPU.TextureFormat.RGBA8Snorm, Aardvark.Rendering.TextureFormat.Rgba8Snorm
                WebGPU.TextureFormat.RGB10A2Uint, Aardvark.Rendering.TextureFormat.Rgb10A2ui
                WebGPU.TextureFormat.Depth16Unorm, Aardvark.Rendering.TextureFormat.DepthComponent16
                WebGPU.TextureFormat.Depth24Plus, Aardvark.Rendering.TextureFormat.DepthComponent24
                WebGPU.TextureFormat.Depth32Float, Aardvark.Rendering.TextureFormat.DepthComponent32f
                WebGPU.TextureFormat.Depth24PlusStencil8, Aardvark.Rendering.TextureFormat.Depth24Stencil8
                WebGPU.TextureFormat.Depth32FloatStencil8, Aardvark.Rendering.TextureFormat.Depth32fStencil8
                WebGPU.TextureFormat.Stencil8, Aardvark.Rendering.TextureFormat.StencilIndex8
            ]
    
    module BlendState =
        let ofAardvark (state : Aardvark.Rendering.BlendMode) : BlendState =
            if state.Enabled then
                {
                    Color = {
                        Operation = BlendOperation.ofAardvark state.ColorOperation
                        SrcFactor = BlendFactor.ofAardvark state.SourceColorFactor
                        DstFactor = BlendFactor.ofAardvark state.DestinationColorFactor
                    }
                    Alpha = {
                        Operation = BlendOperation.ofAardvark state.AlphaOperation
                        SrcFactor = BlendFactor.ofAardvark state.SourceAlphaFactor
                        DstFactor = BlendFactor.ofAardvark state.DestinationAlphaFactor
                    }
                }
            else
                BlendState.Null

    module ColorWriteMask =
        let ofAardvark (mask : ColorMask) =
            let mutable res = ColorWriteMask.None
            if mask.HasFlag ColorMask.Red then res <- res ||| ColorWriteMask.Red
            if mask.HasFlag ColorMask.Green then res <- res ||| ColorWriteMask.Green
            if mask.HasFlag ColorMask.Blue then res <- res ||| ColorWriteMask.Blue
            if mask.HasFlag ColorMask.Alpha then res <- res ||| ColorWriteMask.Alpha
            res

    module PrimitiveTopology =
        let ofAardvark =
            LookupTable.lookup [
                IndexedGeometryMode.LineList, PrimitiveTopology.LineList
                IndexedGeometryMode.LineStrip, PrimitiveTopology.LineStrip
                IndexedGeometryMode.PointList, PrimitiveTopology.PointList
                IndexedGeometryMode.TriangleList, PrimitiveTopology.TriangleList
                IndexedGeometryMode.TriangleStrip, PrimitiveTopology.TriangleStrip
                
            ]

    module IndexFormat =
        let ofAardvark =
            LookupTable.tryLookup [
                typeof<uint16>, IndexFormat.Uint16
                typeof<uint32>, IndexFormat.Uint32
            ]
    
    module FrontFace =
        let ofAardvark =
            LookupTable.lookup [
                Aardvark.Rendering.WindingOrder.Clockwise, FrontFace.CW
                Aardvark.Rendering.WindingOrder.CounterClockwise, FrontFace.CCW
            ]

    module CullMode =
        let ofAardvark =
            LookupTable.lookup [
                Aardvark.Rendering.CullMode.Back, CullMode.Back
                Aardvark.Rendering.CullMode.Front, CullMode.Front
                Aardvark.Rendering.CullMode.None, CullMode.None
            ]
    
    module CompareFunction =
        
        let ofFShade =
            LookupTable.lookup [
                FShade.ComparisonFunction.Never, CompareFunction.Never
                FShade.ComparisonFunction.Less, CompareFunction.Less
                FShade.ComparisonFunction.LessOrEqual, CompareFunction.LessEqual
                FShade.ComparisonFunction.Greater, CompareFunction.Greater
                FShade.ComparisonFunction.GreaterOrEqual, CompareFunction.GreaterEqual
                FShade.ComparisonFunction.Equal, CompareFunction.Equal
                FShade.ComparisonFunction.NotEqual, CompareFunction.NotEqual
                FShade.ComparisonFunction.Always, CompareFunction.Always
                
            ]
        
        let ofDepthTest =
            LookupTable.lookup [
                Aardvark.Rendering.DepthTest.None, CompareFunction.Undefined
                Aardvark.Rendering.DepthTest.Never, CompareFunction.Never
                Aardvark.Rendering.DepthTest.Less, CompareFunction.Less
                Aardvark.Rendering.DepthTest.LessOrEqual, CompareFunction.LessEqual
                Aardvark.Rendering.DepthTest.Greater, CompareFunction.Greater
                Aardvark.Rendering.DepthTest.GreaterOrEqual, CompareFunction.GreaterEqual
                Aardvark.Rendering.DepthTest.Equal, CompareFunction.Equal
                Aardvark.Rendering.DepthTest.NotEqual, CompareFunction.NotEqual
                Aardvark.Rendering.DepthTest.Always, CompareFunction.Always
            ]
        let ofComparisonFunction =
            LookupTable.lookup [
                Aardvark.Rendering.ComparisonFunction.Never, CompareFunction.Never
                Aardvark.Rendering.ComparisonFunction.Less, CompareFunction.Less
                Aardvark.Rendering.ComparisonFunction.LessOrEqual, CompareFunction.LessEqual
                Aardvark.Rendering.ComparisonFunction.Greater, CompareFunction.Greater
                Aardvark.Rendering.ComparisonFunction.GreaterOrEqual, CompareFunction.GreaterEqual
                Aardvark.Rendering.ComparisonFunction.Equal, CompareFunction.Equal
                Aardvark.Rendering.ComparisonFunction.NotEqual, CompareFunction.NotEqual
                Aardvark.Rendering.ComparisonFunction.Always, CompareFunction.Always
            ]
    
    module StencilOperation =
        let ofAardvark =
            LookupTable.lookup [
                Aardvark.Rendering.StencilOperation.Decrement, WebGPU.StencilOperation.DecrementClamp
                Aardvark.Rendering.StencilOperation.DecrementWrap, WebGPU.StencilOperation.DecrementWrap
                Aardvark.Rendering.StencilOperation.Increment, WebGPU.StencilOperation.IncrementClamp
                Aardvark.Rendering.StencilOperation.IncrementWrap, WebGPU.StencilOperation.IncrementWrap
                Aardvark.Rendering.StencilOperation.Invert, WebGPU.StencilOperation.Invert
                Aardvark.Rendering.StencilOperation.Keep, WebGPU.StencilOperation.Keep
                Aardvark.Rendering.StencilOperation.Replace, WebGPU.StencilOperation.Replace
                Aardvark.Rendering.StencilOperation.Zero, WebGPU.StencilOperation.Zero
                
            ]

    module WGSLType =
        open FShade.WGSL
        open PrimitiveValueConverter.Interop

        let toType =
            LookupTable.lookupTable [
                Bool, typeof<int>
                Void, typeof<unit>

                Int(true, 8), typeof<sbyte>
                Int(true, 16), typeof<int16>
                Int(true, 32), typeof<int32>
                Int(true, 64), typeof<int64>

                Int(false, 8), typeof<byte>
                Int(false, 16), typeof<uint16>
                Int(false, 32), typeof<uint32>
                Int(false, 64), typeof<uint64>

                Float(16), typeof<float16>
                Float(32), typeof<float32>
                Float(64), typeof<float32>

                Vec(2, Int(true, 32)), typeof<V2i>
                Vec(3, Int(true, 32)), typeof<V3i>
                Vec(4, Int(true, 32)), typeof<V4i>

                Vec(2, Int(false, 32)), typeof<V2ui>
                Vec(3, Int(false, 32)), typeof<V3ui>
                Vec(4, Int(false, 32)), typeof<V4ui>

                Vec(2, Float(32)), typeof<V2f>
                Vec(3, Float(32)), typeof<V3f>
                Vec(4, Float(32)), typeof<V4f>

                Vec(2, Float(64)), typeof<V2f>
                Vec(3, Float(64)), typeof<V3f>
                Vec(4, Float(64)), typeof<V4f>

                Mat(2,2,Int(true,32)), typeof<M22i>
                Mat(2,3,Int(true,32)), typeof<M23i>
                Mat(3,3,Int(true,32)), typeof<M34i>
                Mat(3,4,Int(true,32)), typeof<M34i>
                Mat(4,4,Int(true,32)), typeof<M44i>

                Mat(2,2,Float(32)), typeof<M24f> // Matrix rows need to be padded to 4 elements according to std140
                Mat(2,3,Float(32)), typeof<M24f>
                Mat(3,3,Float(32)), typeof<M34f>
                Mat(3,4,Float(32)), typeof<M34f>
                Mat(4,4,Float(32)), typeof<M44f>

                Mat(2,2,Float(64)), typeof<M24f>
                Mat(2,3,Float(64)), typeof<M24f>
                Mat(3,3,Float(64)), typeof<M34f>
                Mat(3,4,Float(64)), typeof<M34f>
                Mat(4,4,Float(64)), typeof<M44f>
            ]
            
