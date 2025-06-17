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


[<RequireQualifiedAccess>]   
type WGSLBindGroupEntry =
    | StorageBuffer of WGSLStorageBuffer
    | StorageTexture of WGSLImage
    | UniformBuffer of WGSLUniformBuffer
    | Texture of WGSLTexture
    | Sampler of WGSLSampler
                
type WGSLBindGroupLayout(entries : MapExt<int, MapExt<int, WGSLBindGroupEntry>>) =
    
    member x.Entries = entries
    
    member x.BindGroupCount =
        match MapExt.tryMax x.Entries with
        | Some maxKey -> 1 + maxKey
        | None -> 0
    
    
    
    


[<AbstractClass; Sealed>]
type WebGPUPipelineLayoutExtensions private() =
        
    [<Extension>]
    static member CreateBindGroupLayouts (device : Device, iface : FShade.WGSL.WGSLProgramInterface) : MapExt<int, BindGroupLayout> * WGSLBindGroupLayout =
        
        let mutable groupDescriptors = MapExt.empty
        let mutable frontendDescriptors = MapExt.empty
        let mutable stages = WebGPU.ShaderStage.None
        for slot in iface.shaders.Slots |> MapExt.keys do
            match slot with
            | ShaderSlot.Vertex -> 
                stages <- stages ||| WebGPU.ShaderStage.Vertex
            | ShaderSlot.Fragment ->
                stages <- stages ||| WebGPU.ShaderStage.Fragment
            | ShaderSlot.Compute ->
                stages <- stages ||| WebGPU.ShaderStage.Compute
            | _ ->
                ()
            
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
            frontendDescriptors <-
                frontendDescriptors |> MapExt.alter b.imageGroup (fun g ->
                    g |> Option.defaultValue MapExt.empty |> MapExt.add b.imageBinding (WGSLBindGroupEntry.StorageTexture b) |> Some
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
            frontendDescriptors <-
                frontendDescriptors |> MapExt.alter b.samplerGroup (fun g ->
                    g |> Option.defaultValue MapExt.empty |> MapExt.add b.samplerBinding (WGSLBindGroupEntry.Sampler b) |> Some
                )
                
        for KeyValue(_, t) in iface.textures do
            let sampleType =
                match t.textureType.valueType with
                | WGSL.WGSLType.Int(true,_) -> TextureSampleType.Sint
                | WGSL.WGSLType.Int(false,_) -> TextureSampleType.Uint
                | WGSL.WGSLType.Float _ -> TextureSampleType.Float
                | WGSL.WGSLType.Vec(_, WGSL.WGSLType.Float _) -> TextureSampleType.Float
                | WGSL.WGSLType.Vec(_, WGSL.WGSLType.Int(true,_)) -> TextureSampleType.Sint
                | WGSL.WGSLType.Vec(_, WGSL.WGSLType.Int(false,_)) -> TextureSampleType.Uint
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
            frontendDescriptors <-
                frontendDescriptors |> MapExt.alter t.textureGroup (fun g ->
                    g |> Option.defaultValue MapExt.empty |> MapExt.add t.textureBinding (WGSLBindGroupEntry.Texture t) |> Some
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
                            BufferBindingLayout.Type = (if b.ssbReadOnly then BufferBindingType.ReadOnlyStorage else BufferBindingType.Storage)
                            BufferBindingLayout.HasDynamicOffset = false
                            BufferBindingLayout.MinBindingSize = 0L
                        })
                    ) |> Some
                )
            frontendDescriptors <-
                frontendDescriptors |> MapExt.alter b.ssbGroup (fun g ->
                    g |> Option.defaultValue MapExt.empty |> MapExt.add b.ssbBinding (WGSLBindGroupEntry.StorageBuffer b) |> Some
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
        
            frontendDescriptors <-
                frontendDescriptors |> MapExt.alter b.ubGroup (fun g ->
                    g |> Option.defaultValue MapExt.empty |> MapExt.add b.ubBinding (WGSLBindGroupEntry.UniformBuffer b) |> Some
                )
       
        let groupLayouts =
            groupDescriptors |> MapExt.map (fun gi bindings ->
                device.CreateBindGroupLayout {
                    Label = null
                    Entries = bindings |> MapExt.values |> Array.ofSeq
                }    
            )
        groupLayouts, WGSLBindGroupLayout(frontendDescriptors)
 
    [<Extension>]
    static member CreatePipelineLayout (device : Device, iface : FShade.WGSL.WGSLProgramInterface) =
        let groupLayouts, _  = device.CreateBindGroupLayouts(iface)
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
                ImmediateSize = 0
            }
        
        PipelineLayout(layout, groupLayouts)
        
        