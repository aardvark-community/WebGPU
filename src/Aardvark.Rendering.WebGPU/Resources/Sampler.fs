namespace Aardvark.Rendering.WebGPU

open System.Runtime.CompilerServices
open Aardvark.Base
open FShade
open FShade.GLSL
open global.WebGPU
open Aardvark.Rendering
open System.IO
open System

[<AbstractClass; Sealed>]
type WebGPUSamplerExtensions private() =
    
    [<Extension>]
    static member CreateSampler(this : Device, samplerDesc : FShade.SamplerState) =
        let struct(min, mag, mip) =
            samplerDesc.Filter
            |> Option.defaultValue FShade.Filter.MinMagMipLinear
            |> Translations.FilterMode.ofFShade
            
        let anisotropy =
            match samplerDesc.Filter with
            | Some FShade.Filter.Anisotropic -> samplerDesc.MaxAnisotropy |> Option.defaultValue 16
            | _ -> samplerDesc.MaxAnisotropy |> Option.defaultValue 1
            
        this.CreateSampler {
            Label = null
            Next = null
            MinFilter = min
            MipmapFilter = mip
            MagFilter = mag
            AddressModeU = samplerDesc.AddressU |> Option.defaultValue FShade.WrapMode.Wrap |> Translations.AddressMode.ofFShade
            AddressModeV = samplerDesc.AddressV |> Option.defaultValue FShade.WrapMode.Wrap |> Translations.AddressMode.ofFShade
            AddressModeW = samplerDesc.AddressW |> Option.defaultValue FShade.WrapMode.Wrap |> Translations.AddressMode.ofFShade
            LodMinClamp = samplerDesc.MinLod |> Option.defaultValue 0.0f 
            LodMaxClamp = samplerDesc.MaxLod |> Option.defaultValue 1000.0f
            Compare = samplerDesc.Comparison |> Option.map Translations.CompareFunction.ofFShade |> Option.defaultValue CompareFunction.Undefined
            MaxAnisotropy = uint16 anisotropy
        }
    