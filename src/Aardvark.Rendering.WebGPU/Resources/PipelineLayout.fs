namespace Aardvark.Rendering.WebGPU

open System.Runtime.CompilerServices
open Aardvark.Base
open FShade
open FShade.GLSL
open WebGPU

type PipelineLayout(layout : WebGPU.PipelineLayout, groupLayouts : BindGroupLayout[]) =
    member x.Layout = layout
    member x.GroupLayouts = groupLayouts


[<RequireQualifiedAccess>]   
type WGSLBindGroupEntry =
    | StorageBuffer of GLSL.GLSLStorageBuffer
    | StorageTexture of GLSL.GLSLImage
    | UniformBuffer of GLSL.GLSLUniformBuffer
    | Texture of GLSL.GLSLTexture
    | Sampler of GLSL.GLSLSamplerState
                
type WGSLBindGroupLayout(entries : MapExt<int, MapExt<int, WGSLBindGroupEntry>>) =
    
    member x.Entries = entries
    
    member x.BindGroupCount =
        match MapExt.tryMax x.Entries with
        | Some maxKey -> 1 + maxKey
        | None -> 0
    
    
    
    


[<AbstractClass; Sealed>]
type WebGPUPipelineLayoutExtensions private() =
        
    [<Extension>]
    static member CreateBindGroupLayouts (device : Device, iface : FShade.GLSL.GLSLProgramInterface) : MapExt<int, BindGroupLayout> * WGSLBindGroupLayout =
        
        let mutable groupDescriptors = MapExt.empty
        let mutable frontendDescriptors = MapExt.empty
        
        
        let toStage (stage : FShade.ShaderStage) =
            match stage with
            | FShade.ShaderStage.Vertex -> WebGPU.ShaderStage.Vertex
            | FShade.ShaderStage.Fragment -> WebGPU.ShaderStage.Fragment
            | FShade.ShaderStage.Compute -> WebGPU.ShaderStage.Compute
            | _ -> WebGPU.ShaderStage.None
        
        let getStages (name : string) =
            let mutable user = WebGPU.ShaderStage.None
            for stage in iface.GetUniformStages(name) do
                user <- (toStage stage) ||| user
            user
        
        let isCompute =
            match iface.shaders with
            | GLSLProgramShaders.Compute _ -> true
            | _ -> false
        
        for KeyValue(_, b) in iface.images do
            let sampleType =
                match b.imageType.valueType with
                | GLSL.GLSLType.Int(true,_) -> TextureSampleType.Sint
                | GLSL.GLSLType.Int(false,_) -> TextureSampleType.Uint
                | GLSL.GLSLType.Float _ -> TextureSampleType.Float
                | GLSL.GLSLType.Vec(_, GLSL.GLSLType.Int(true,_)) -> TextureSampleType.Sint
                | GLSL.GLSLType.Vec(_, GLSL.GLSLType.Int(false,_)) -> TextureSampleType.Uint
                | GLSL.GLSLType.Vec(_, GLSL.GLSLType.Float _) -> TextureSampleType.Float
                | _ -> TextureSampleType.Undefined
            
            let viewDimension =
                match b.imageType.dimension with
                | SamplerDimension.Sampler1d -> TextureViewDimension.D1D
                | SamplerDimension.Sampler2d -> TextureViewDimension.D2D
                | SamplerDimension.Sampler3d -> TextureViewDimension.D3D
                | SamplerDimension.SamplerCube -> TextureViewDimension.Cube
                | _ -> TextureViewDimension.Undefined
                
            let format =
                match b.imageType.format with
                | Some ImageFormat.Rgba8 -> WebGPU.TextureFormat.RGBA8Unorm
                | Some ImageFormat.Rgba32f -> WebGPU.TextureFormat.RGBA32Float
                | Some ImageFormat.R32f -> WebGPU.TextureFormat.R32Float
                | Some ImageFormat.R32ui -> WebGPU.TextureFormat.R32Uint
                | _ -> WebGPU.TextureFormat.Undefined
                
            groupDescriptors <-
                groupDescriptors |> MapExt.alter b.imageSet (fun g ->
                    let g = 
                        match g with
                        | Some g -> g
                        | None -> MapExt.empty
                    g |> MapExt.add b.imageBinding (
                        if isCompute then
                            BindGroupLayoutEntry.StorageTexture(b.imageBinding, getStages b.imageName, {
                                StorageTextureBindingLayout.ViewDimension = viewDimension
                                StorageTextureBindingLayout.Access = StorageTextureAccess.ReadWrite
                                StorageTextureBindingLayout.Format = format
                            })
                        else
                            BindGroupLayoutEntry.Texture(b.imageBinding, getStages b.imageName, {
                                TextureBindingLayout.Multisampled = b.imageType.isMS
                                TextureBindingLayout.SampleType = sampleType
                                TextureBindingLayout.ViewDimension = viewDimension
                                
                            })
                    ) |> Some
                )
            frontendDescriptors <-
                frontendDescriptors |> MapExt.alter b.imageSet (fun g ->
                    g |> Option.defaultValue MapExt.empty |> MapExt.add b.imageBinding (WGSLBindGroupEntry.StorageTexture b) |> Some
                )
            
            
               
        // for KeyValue(_, b) in iface.samplers do
        //     groupDescriptors <-
        //         groupDescriptors |> MapExt.alter b.samplerSet (fun g ->
        //             let g = 
        //                 match g with
        //                 | Some g -> g
        //                 | None -> MapExt.empty
        //             g |> MapExt.add b.samplerBinding (
        //                 BindGroupLayoutEntry.Sampler(b.samplerBinding, stages, SamplerBindingType.Filtering)
        //             ) |> Some
        //         )
        //     frontendDescriptors <-
        //         frontendDescriptors |> MapExt.alter b.samplerSet (fun g ->
        //             g |> Option.defaultValue MapExt.empty |> MapExt.add b.samplerBinding (WGSLBindGroupEntry.Sampler b) |> Some
        //         )
                
        for KeyValue(_, t) in iface.textures do
            let sampleType =
                match t.textureType.valueType with
                | GLSL.GLSLType.Int(true,_) -> TextureSampleType.Sint
                | GLSL.GLSLType.Int(false,_) -> TextureSampleType.Uint
                | GLSL.GLSLType.Float _ -> TextureSampleType.Float
                | GLSL.GLSLType.Vec(_, GLSL.GLSLType.Float _) -> TextureSampleType.Float
                | GLSL.GLSLType.Vec(_, GLSL.GLSLType.Int(true,_)) -> TextureSampleType.Sint
                | GLSL.GLSLType.Vec(_, GLSL.GLSLType.Int(false,_)) -> TextureSampleType.Uint
                | _ -> TextureSampleType.Undefined
            
            let viewDimension =
                match t.textureType.dimension with
                | SamplerDimension.Sampler1d -> TextureViewDimension.D1D
                | SamplerDimension.Sampler2d -> TextureViewDimension.D2D
                | SamplerDimension.Sampler3d -> TextureViewDimension.D3D
                | SamplerDimension.SamplerCube -> TextureViewDimension.Cube
                | _ -> TextureViewDimension.Undefined
                
            groupDescriptors <-
                groupDescriptors |> MapExt.alter t.textureSet (fun g ->
                    let g = 
                        match g with
                        | Some g -> g
                        | None -> MapExt.empty
                    g |> MapExt.add t.textureBinding (
                        BindGroupLayoutEntry.Texture(t.textureBinding, getStages t.textureName, {
                            TextureBindingLayout.Multisampled = t.textureType.isMS
                            TextureBindingLayout.SampleType = sampleType
                            TextureBindingLayout.ViewDimension = viewDimension
                        })
                    ) |> Some
                )
            frontendDescriptors <-
                frontendDescriptors |> MapExt.alter t.textureSet (fun g ->
                    g |> Option.defaultValue MapExt.empty |> MapExt.add t.textureBinding (WGSLBindGroupEntry.Texture t) |> Some
                )
               
        
        for KeyValue(_, s) in iface.samplerStates do
            groupDescriptors <-
                groupDescriptors |> MapExt.alter s.samplerSet (fun g ->
                    let g = 
                        match g with
                        | Some g -> g
                        | None -> MapExt.empty
                    g |> MapExt.add s.samplerBinding (
                        BindGroupLayoutEntry.Sampler(s.samplerBinding, getStages s.samplerName, SamplerBindingType.Filtering)
                    ) |> Some
                )
            frontendDescriptors <-
                frontendDescriptors |> MapExt.alter s.samplerSet (fun g ->
                    g |> Option.defaultValue MapExt.empty |> MapExt.add s.samplerBinding (WGSLBindGroupEntry.Sampler s) |> Some
                )
               
               
        for KeyValue(name, b) in iface.storageBuffers do
            groupDescriptors <-
                groupDescriptors |> MapExt.alter b.ssbSet (fun g ->
                    let g = 
                        match g with
                        | Some g -> g
                        | None -> MapExt.empty
                        
                        
                    // TODO: fshade needs to report using stages as well!!!!
                    // let stages =
                    //     if stages.HasFlag WebGPU.ShaderStage.Vertex then stages &&& ~~~WebGPU.ShaderStage.Vertex
                    //     else stages
                        
                    let typ = //BufferBindingType.Storage
                        if b.ssbAccess.HasFlag(FShade.StorageAccess.Write) then
                            BufferBindingType.Storage
                        else
                            BufferBindingType.ReadOnlyStorage
                           
                        
                    g |> MapExt.add b.ssbBinding (
                        BindGroupLayoutEntry.Buffer(b.ssbBinding, getStages name, {
                            BufferBindingLayout.Type = typ
                            BufferBindingLayout.HasDynamicOffset = false
                            BufferBindingLayout.MinBindingSize = 0L
                        })
                    ) |> Some
                )
            frontendDescriptors <-
                frontendDescriptors |> MapExt.alter b.ssbSet (fun g ->
                    g |> Option.defaultValue MapExt.empty |> MapExt.add b.ssbBinding (WGSLBindGroupEntry.StorageBuffer b) |> Some
                )
        
        for KeyValue(_, b) in iface.uniformBuffers do
            groupDescriptors <-
                groupDescriptors |> MapExt.alter b.ubSet (fun g ->
                    let g = 
                        match g with
                        | Some g -> g
                        | None -> MapExt.empty
                    g |> MapExt.add b.ubBinding (
                        BindGroupLayoutEntry.Buffer(b.ubBinding, getStages b.ubName, {
                            BufferBindingLayout.Type = BufferBindingType.Uniform
                            BufferBindingLayout.HasDynamicOffset = false
                            BufferBindingLayout.MinBindingSize = int64 b.ubSize
                        })
                    ) |> Some
                )
        
            frontendDescriptors <-
                frontendDescriptors |> MapExt.alter b.ubSet (fun g ->
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
    static member CreatePipelineLayout (device : Device, iface : FShade.GLSL.GLSLProgramInterface) =
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
        
        