namespace Aardvark.Rendering.WebGPU

open System.Runtime.CompilerServices
open Aardvark.Base
open FShade
open FShade.GLSL
open WebGPU.ShaderTranspiler
open System.IO
open Microsoft.FSharp.NativeInterop
open WebGPU
open Aardvark.Rendering

#nowarn "9"

type WGSLShader =
    {
        codes : Map<FShade.ShaderStage, string>
        iface : GLSLProgramInterface
    }

module WGSLShader =
    open System.IO
    open FShade.GLSL
    
    let writeTo (dst : Stream) (shader : WGSLShader) =
        use dst = new BinaryWriter(dst, System.Text.Encoding.UTF8, true)
        dst.Write "WGSLShader"
        
        dst.Write (shader.codes.Count)
        for KeyValue(stage, code) in shader.codes do
            dst.Write (int stage)
            dst.Write code
        
        GLSLProgramInterface.serializeInternal dst shader.iface

    let tryReadFrom (src : Stream) =
        use src = new BinaryReader(src, System.Text.Encoding.UTF8, true)
        if src.ReadString() = "WGSLShader" then
            let codes =
                let cnt = src.ReadInt32()
                let mutable map = Map.empty
                for i in 0 .. cnt - 1 do
                    let stage = src.ReadInt32() |> unbox<FShade.ShaderStage>
                    let code = src.ReadString()
                    map <- Map.add stage code map
                map
                
            let iface = GLSLProgramInterface.deserializeInternal src
            Some { codes = codes; iface = iface }
        else
            None
    
    let tryUnpickle (src : byte[]) =
        use ms = new MemoryStream(src)
        tryReadFrom ms
        
    let unpickle (src : byte[]) =
        match tryUnpickle src with
        | Some res -> res
        | None -> failwith "could not unpickle WGSLShader"
        
    let pickle (src : WGSLShader) =
        use ms = new MemoryStream()
        writeTo ms src
        ms.ToArray()
        
        
    let ofGLSL (glsl : GLSLShader) =
        let stagesAndEntries = 
            match glsl.iface.shaders with
            | GLSLProgramShaders.Graphics shaders ->
                shaders.stages |> MapExt.toList |> List.map (fun (stage, info) ->
                    stage, info.shaderEntry
                )
            | GLSLProgramShaders.Compute info ->
                [FShade.ShaderStage.Compute, info.shaderEntry]
            | _ ->
                []
                
        let codes =
            stagesAndEntries |> List.map (fun (stage, entry) ->
                let gpuStage =
                    match stage with
                    | FShade.ShaderStage.Vertex -> WebGPU.ShaderStage.Vertex
                    | FShade.ShaderStage.Fragment -> WebGPU.ShaderStage.Fragment
                    | _ -> WebGPU.ShaderStage.Compute
                stage, toWGSL gpuStage entry glsl.code    
            )
                
        { codes = Map.ofList codes; iface = glsl.iface }
         

type internal ShaderCacheKey =
    {
        EffectId : string
        Outputs : Map<int, AttachmentSignature>
        Depth : option<TextureFormat>
    }

module internal ShaderCacheKey =
    let serialize (dst : BinaryWriter) (src : ShaderCacheKey) =
        dst.Write(src.EffectId)
        dst.Write(src.Outputs.Count)
        for KeyValue(k, v) in src.Outputs do
            dst.Write(k)
            dst.Write (int v.Format)
            dst.Write (string v.Name)
        match src.Depth with
        | Some d ->
            dst.Write (int d)
        | None ->
            dst.Write 0

    let computeHash (src : ShaderCacheKey) =
        use ms = new MemoryStream()
        use bw = new BinaryWriter(ms)
        serialize bw src
        ms.Position <- 0L
        let hash = System.Security.Cryptography.SHA1.HashData ms
        "wgsl_" + System.Convert.ToBase64String(hash).Replace("=", ".").Replace("/", "_").Replace("+", "-")

type ShaderProgram(shaderModules : Map<FShade.ShaderStage, ShaderModule>, code : WGSLShader, groupLayouts : MapExt<int, BindGroupLayout>, pipelineLayout : PipelineLayout, samplers : Map<int, Map<int, Sampler>>) =
    
    let bindGroupCount =
        match MapExt.tryMax groupLayouts with
        | Some maxKey -> 1 + maxKey
        | None -> 0
    
    member x.PipelineLayout = pipelineLayout
    member x.BindGroupLayouts = groupLayouts
    member x.ShaderModules = shaderModules
    member x.ShaderCode = code
    member x.Samplers = samplers
    member x.BindGroupCount = bindGroupCount
    
    member x.Dispose() =
        for KeyValue(_, shaderModule) in shaderModules do
            shaderModule.Dispose()
        for KeyValue(_, group) in samplers do
            for KeyValue(_, s) in group do
                s.Dispose()
            
    interface IBackendSurface with
        member x.Handle = 0UL
        member x.Dispose() = x.Dispose()

[<AbstractClass; Sealed>]
type WebGPUShaderExtensions private() =
    
    static let mutable shaderCaching = true
    
    static let glslBackend =
        FShade.GLSL.Backend.Create {
            FShade.GLSL.version                     = GLSLVersion(4,5,0)
            FShade.GLSL.enabledExtensions           = Set.ofList [ "GL_ARB_tessellation_shader"; "GL_ARB_separate_shader_objects"; "GL_ARB_shading_language_420pack" ]
            FShade.GLSL.availableExtensions         = Map.empty
            FShade.GLSL.createUniformBuffers        = true
            FShade.GLSL.pushConstants               = false
            FShade.GLSL.bindingMode                 = BindingMode.Global
            FShade.GLSL.createDescriptorSets        = true
            FShade.GLSL.stepDescriptorSets          = false
            FShade.GLSL.createInputLocations        = true
            FShade.GLSL.createOutputLocations       = true
            FShade.GLSL.createPassingLocations      = true
            FShade.GLSL.createPerStageUniforms      = true
            FShade.GLSL.reverseMatrixLogic          = true
            FShade.GLSL.reverseTessellationWinding  = true
            FShade.GLSL.depthWriteMode              = true
            FShade.GLSL.useInOut                    = true
            FShade.GLSL.separateTexturesAndSamplers = true
        }
    
    static let cachePath =
        Path.combine [
            CachingProperties.CacheDirectory
            "Shaders"
            "WebGPU"
        ]
        
    static do
        if not (Directory.Exists cachePath) then
            Directory.CreateDirectory cachePath |> ignore
    
    static let wgslCache = ConcurrentDict<IFramebufferSignature * FShade.Effect, WGSLShader>(Dict())
    static let wgslComputeCache = ConcurrentDict<FShade.ComputeShader, WGSLShader>(Dict())
    
    static let shaderProgramCache = ConcurrentDict<Device * WGSLShader, ShaderProgram>(Dict())
    
    static let lineRx = System.Text.RegularExpressions.Regex @"\r\n|\n|\r"
    
    static member FShadeBackend = glslBackend
    
    static member ShaderCaching
        with get() = shaderCaching
        and set v = shaderCaching <- v
    
    [<Extension>]
    static member GetWGSLCode(this : FShade.Effect, signature : IFramebufferSignature) =
        wgslCache.GetOrCreate((signature, this), fun (signature, effect) ->
            if shaderCaching then
                let fileName =
                    ShaderCacheKey.computeHash {
                        EffectId = effect.Id
                        Outputs = signature.ColorAttachments
                        Depth = signature.DepthStencilAttachment
                    }
                    
                let cacheFile = Path.Combine(cachePath, fileName + ".bin")
                
                let inline write (shader : WGSLShader) =
                    let data = WGSLShader.pickle shader
                    File.WriteAllBytes(cacheFile, data)
                    shader
                
                if File.Exists cacheFile then
                    try
                        let data = File.ReadAllBytes cacheFile
                        WGSLShader.unpickle data
                    with e ->
                        Log.warn "could not read cache-file: %A" e
                        effect.Link(signature)
                        |> ModuleCompiler.compileGLSL glslBackend
                        |> WGSLShader.ofGLSL
                        
                        |> write
                else
                    effect.Link(signature)
                    |> ModuleCompiler.compileGLSL glslBackend
                    |> WGSLShader.ofGLSL
                    |> write
            else
                effect.Link(signature)
                |> ModuleCompiler.compileGLSL glslBackend
                |> WGSLShader.ofGLSL
                
        )
    
    [<Extension>]
    static member GetWGSLCode(this : FShade.ComputeShader) =
        wgslComputeCache.GetOrCreate(this, fun computeShader ->
            if shaderCaching then
                let fileName =
                    ShaderCacheKey.computeHash {
                        EffectId = this.csId
                        Outputs = Map.empty
                        Depth = None
                    }
                let cacheFile = Path.Combine(cachePath, fileName + ".bin")
                
                let inline write (shader : WGSLShader) =
                    let data = WGSLShader.pickle shader
                    File.WriteAllBytes(cacheFile, data)
                    shader
                
                if shaderCaching && File.Exists cacheFile then
                    try
                        let data = File.ReadAllBytes cacheFile
                        WGSLShader.unpickle data
                    with e ->
                        Log.warn "could not read cache-file: %A" e
                        computeShader
                        |> FShade.ComputeShader.toModule
                        |> ModuleCompiler.compileGLSL glslBackend
                        |> WGSLShader.ofGLSL
                        |> write
                else
                    computeShader
                    |> FShade.ComputeShader.toModule
                    |> ModuleCompiler.compileGLSL glslBackend
                    |> WGSLShader.ofGLSL
                    |> write
            else
                computeShader
                |> FShade.ComputeShader.toModule
                |> ModuleCompiler.compileGLSL glslBackend
                |> WGSLShader.ofGLSL
        )
        
    [<Extension>]
    static member CreateShaderProgram(this : Device, wgsl : WGSLShader) =
        shaderProgramCache.GetOrCreate((this, wgsl), fun (device, wgsl) ->

            let shaderModules =
                wgsl.codes |> Map.map (fun stage wgsl ->
                    device.CompileShader(wgsl)
                )
            let samplers = 
                wgsl.iface.samplerStates
                |> MapExt.toList |> List.map (fun (k, v) ->
                    let sam = List.exactlyOne v.samplerStates
                    v.samplerSet, (v.samplerBinding, device.CreateSampler(sam))
                )
                |> List.groupBy fst
                |> List.map (fun (gid, samplers) -> gid, samplers |> List.map snd |> Map.ofList)
                |> Map.ofList
                
            let groupLayouts,_ = device.CreateBindGroupLayouts wgsl.iface
            
            let pipelineLayout =
                let entries = 
                    match MapExt.tryMax groupLayouts with
                    | Some maxKey ->
                        let arr = Array.create (maxKey + 1) BindGroupLayout.Null
                        groupLayouts |> MapExt.iter (fun k v -> arr.[k] <- v)
                        arr
                    | None ->
                        [||]
                device.CreatePipelineLayout {
                    Label = null
                    Next = null
                    BindGroupLayouts = entries
                    ImmediateSize = 0
                }
            
            new ShaderProgram(shaderModules, wgsl, groupLayouts, pipelineLayout, samplers)   
        )

    [<Extension>]
    static member CreateShaderProgram(this : Device, effect : FShade.Effect, signature : IFramebufferSignature) =
        let glsl = effect.GetWGSLCode(signature)
        this.CreateShaderProgram(glsl)
