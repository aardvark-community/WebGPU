namespace Aardvark.Rendering.WebGPU

open System.Runtime.CompilerServices
open Aardvark.Base
open FShade
open FShade.WGSL
open global.WebGPU
open Aardvark.Rendering

type PipelineLayout(layout : WebGPU.PipelineLayout, groupLayouts : BindGroupLayout[]) =
    member x.Layout = layout
    member x.GroupLayouts = groupLayouts

[<AbstractClass; Sealed>]
type WebGPUPipelineLayoutExtensions private() =
        
    [<Extension>]
    static member CreateBindGroupLayouts (device : Device, iface : FShade.WGSL.WGSLProgramInterface) =
        
        let mutable groupDescriptors = MapExt.empty
        
        let stages = WebGPU.ShaderStage.Vertex ||| WebGPU.ShaderStage.Fragment
        for KeyValue(_, b) in iface.images do
            let sampleType =
                match b.imageType.valueType with
                | WGSL.WGSLType.Int(true,_) -> TextureSampleType.Sint
                | WGSL.WGSLType.Int(false,_) -> TextureSampleType.Uint
                | WGSL.WGSLType.Float _ -> TextureSampleType.Float
                | _ -> TextureSampleType.Undefined
            
            let viewDimension =
                match b.imageType.dimension with
                | SamplerDimension.Sampler1d -> TextureViewDimension.D1D
                | SamplerDimension.Sampler2d -> TextureViewDimension.D2D
                | SamplerDimension.Sampler3d -> TextureViewDimension.D3D
                | SamplerDimension.SamplerCube -> TextureViewDimension.Cube
                | _ -> TextureViewDimension.Undefined
                
            groupDescriptors <-
                groupDescriptors |> MapExt.alter b.imageGroup (fun g ->
                    let g = 
                        match g with
                        | Some g -> g
                        | None -> MapExt.empty
                    g |> MapExt.add b.imageBinding (
                        BindGroupLayoutEntry.Texture(b.imageBinding, stages, {
                            TextureBindingLayout.Multisampled = b.imageType.isMS
                            TextureBindingLayout.SampleType = sampleType
                            TextureBindingLayout.ViewDimension = viewDimension
                        })
                    ) |> Some
                )
               
        for KeyValue(_, b) in iface.samplers do
            groupDescriptors <-
                groupDescriptors |> MapExt.alter b.samplerGroup (fun g ->
                    let g = 
                        match g with
                        | Some g -> g
                        | None -> MapExt.empty
                    g |> MapExt.add b.samplerBinding (
                        BindGroupLayoutEntry.Sampler(b.samplerBinding, stages, SamplerBindingType.Filtering)
                    ) |> Some
                )
                
        for KeyValue(_, t) in iface.textures do
            let sampleType =
                match t.textureType.valueType with
                | WGSL.WGSLType.Int(true,_) -> TextureSampleType.Sint
                | WGSL.WGSLType.Int(false,_) -> TextureSampleType.Uint
                | WGSL.WGSLType.Float _ -> TextureSampleType.Float
                | _ -> TextureSampleType.Undefined
            
            let viewDimension =
                match t.textureType.dimension with
                | SamplerDimension.Sampler1d -> TextureViewDimension.D1D
                | SamplerDimension.Sampler2d -> TextureViewDimension.D2D
                | SamplerDimension.Sampler3d -> TextureViewDimension.D3D
                | SamplerDimension.SamplerCube -> TextureViewDimension.Cube
                | _ -> TextureViewDimension.Undefined
                
            groupDescriptors <-
                groupDescriptors |> MapExt.alter t.textureGroup (fun g ->
                    let g = 
                        match g with
                        | Some g -> g
                        | None -> MapExt.empty
                    g |> MapExt.add t.textureBinding (
                        BindGroupLayoutEntry.Texture(t.textureBinding, stages, {
                            TextureBindingLayout.Multisampled = t.textureType.isMS
                            TextureBindingLayout.SampleType = sampleType
                            TextureBindingLayout.ViewDimension = viewDimension
                        })
                    ) |> Some
                )
               
        for KeyValue(_, b) in iface.storageBuffers do
            groupDescriptors <-
                groupDescriptors |> MapExt.alter b.ssbGroup (fun g ->
                    let g = 
                        match g with
                        | Some g -> g
                        | None -> MapExt.empty
                    g |> MapExt.add b.ssbBinding (
                        BindGroupLayoutEntry.Buffer(b.ssbBinding, stages, {
                            BufferBindingLayout.Type = BufferBindingType.Storage
                            BufferBindingLayout.HasDynamicOffset = false
                            BufferBindingLayout.MinBindingSize = 0L
                        })
                    ) |> Some
                )
        
        for KeyValue(_, b) in iface.uniformBuffers do
            groupDescriptors <-
                groupDescriptors |> MapExt.alter b.ubGroup (fun g ->
                    let g = 
                        match g with
                        | Some g -> g
                        | None -> MapExt.empty
                    g |> MapExt.add b.ubBinding (
                        BindGroupLayoutEntry.Buffer(b.ubBinding, stages, {
                            BufferBindingLayout.Type = BufferBindingType.Uniform
                            BufferBindingLayout.HasDynamicOffset = false
                            BufferBindingLayout.MinBindingSize = int64 b.ubSize
                        })
                    ) |> Some
                )
        
       
        let groupLayouts =
            groupDescriptors |> MapExt.map (fun gi bindings ->
                device.CreateBindGroupLayout {
                    Label = null
                    Entries = bindings |> MapExt.values |> Array.ofSeq
                }    
            )
        groupLayouts
 
    [<Extension>]
    static member CreatePipelineLayout (device : Device, iface : FShade.WGSL.WGSLProgramInterface) =
        let groupLayouts = device.CreateBindGroupLayouts(iface)
        let maxKey = MapExt.tryMax groupLayouts
            
        let groupLayouts =
            match maxKey with
            | Some maxIndex ->
                let arr = Array.create (maxIndex + 1) BindGroupLayout.Null
                groupLayouts |> MapExt.iter (fun k v -> arr.[k] <- v)
                arr
            | None ->
                [||]
            
        let layout = 
            device.CreatePipelineLayout {
                Next = null
                Label = null
                BindGroupLayouts = groupLayouts
                ImmediateDataRangeByteSize = 0
            }
        
        PipelineLayout(layout, groupLayouts)
        
        