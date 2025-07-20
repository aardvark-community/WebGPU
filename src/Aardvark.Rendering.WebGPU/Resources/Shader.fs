namespace Aardvark.Rendering.WebGPU

open System.Runtime.CompilerServices
open Aardvark.Base
open FShade
open FShade.GLSL
open global.WebGPU
open Aardvark.Rendering
open System.IO
open Microsoft.FSharp.NativeInterop
#nowarn "9"


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
        System.Convert.ToBase64String(hash).Replace("=", ".").Replace("/", "_").Replace("+", "-")

type ShaderProgram(shaderModules : MapExt<FShade.ShaderStage, ShaderModule>, code : GLSLShader, groupLayouts : MapExt<int, BindGroupLayout>, pipelineLayout : PipelineLayout, samplers : Map<int, Map<int, Sampler>>) =
    
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
    
    static let wgslCache = ConcurrentDict<IFramebufferSignature * FShade.Effect, GLSLShader>(Dict())
    
    static let shaderProgramCache = ConcurrentDict<Device * FShade.GLSL.GLSLShader, ShaderProgram>(Dict())
    
    static let lineRx = System.Text.RegularExpressions.Regex @"\r\n|\n|\r"
    
    static member FShadeBackend = glslBackend
    
    [<Extension>]
    static member GetWGSLCode(this : FShade.Effect, signature : IFramebufferSignature) =
        wgslCache.GetOrCreate((signature, this), fun (signature, effect) ->
            
            let fileName =
                ShaderCacheKey.computeHash {
                    EffectId = effect.Id
                    Outputs = signature.ColorAttachments
                    Depth = signature.DepthStencilAttachment
                }
                
            let cacheFile = Path.Combine(cachePath, fileName + ".bin")
            
            let inline write (shader : FShade.GLSL.GLSLShader) =
                Log.start "shader"
                for l in lineRx.Split shader.code do
                    Log.line "%s" l
                Log.stop()
                let data = FShade.GLSL.GLSLShader.pickle shader
                File.WriteAllBytes(cacheFile, data)
                shader
            
            if false && File.Exists cacheFile then
                try
                    let data = File.ReadAllBytes cacheFile
                    FShade.GLSL.GLSLShader.unpickle data
                with e ->
                    Log.warn "could not read cache-file: %A" e
                    effect.Link(signature)
                    |> ModuleCompiler.compileGLSL glslBackend
                    |> write
            else
                effect.Link(signature)
                |> ModuleCompiler.compileGLSL glslBackend
                |> write
        )
    
    [<Extension>]
    static member CreateShaderProgram(this : Device, glsl : FShade.GLSL.GLSLShader) =
        shaderProgramCache.GetOrCreate((this, glsl), fun (device, glsl) ->
            let entries = 
                match glsl.iface.shaders with
                | GLSLProgramShaders.Graphics shaders ->
                    shaders.stages |> MapExt.map (fun stage info ->
                        info.shaderEntry    
                    )
                | GLSLProgramShaders.Compute info ->
                    MapExt.singleton FShade.ShaderStage.Compute info.shaderEntry
                | _ ->
                    failwith "no raytracing"
        
            let shaderCodes =
                entries |> MapExt.map (fun stage entry ->
                    let gpuStage =
                        match stage with
                        | FShade.ShaderStage.Vertex -> WebGPU.ShaderStage.Vertex
                        | FShade.ShaderStage.Fragment -> WebGPU.ShaderStage.Fragment
                        | _ -> WebGPU.ShaderStage.Compute
                        
                    let wgsl = ShaderTranspiler.toWGSL gpuStage "main" glsl.code
                    
                    wgsl
                )
            let shaderModules =
                shaderCodes |> MapExt.map (fun stage wgsl ->
                    device.CompileShader(wgsl, string (wgsl.Substring(0, 100)))
                )
            let samplers = 
                glsl.iface.samplerStates
                |> MapExt.toList |> List.map (fun (k, v) ->
                    let sam = List.exactlyOne v.samplerStates
                    v.samplerSet, (v.samplerBinding, device.CreateSampler(sam))
                )
                |> List.groupBy fst
                |> List.map (fun (gid, samplers) -> gid, samplers |> List.map snd |> Map.ofList)
                |> Map.ofList
                
            let groupLayouts,_ = device.CreateBindGroupLayouts glsl.iface
            
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
            
            new ShaderProgram(shaderModules, glsl, groupLayouts, pipelineLayout, samplers)   
        )

    [<Extension>]
    static member CreateShaderProgram(this : Device, effect : FShade.Effect, signature : IFramebufferSignature) =
        let wgsl = effect.GetWGSLCode(signature)
        this.CreateShaderProgram(wgsl)
