namespace Aardvark.Rendering.WebGPU

open System.Runtime.CompilerServices
open Aardvark.Base
open FShade
open FShade.WGSL
open global.WebGPU
open Aardvark.Rendering
open System.IO

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

type ShaderProgram(shaderModule : ShaderModule, code : WGSLShader, groupLayouts : MapExt<int, BindGroupLayout>, pipelineLayout : PipelineLayout, samplers : Map<int, Map<int, Sampler>>) =
    
    let bindGroupCount =
        match MapExt.tryMax groupLayouts with
        | Some maxKey -> 1 + maxKey
        | None -> 0
    
    member x.PipelineLayout = pipelineLayout
    member x.BindGroupLayouts = groupLayouts
    member x.ShaderModule = shaderModule
    member x.ShaderCode = code
    member x.Samplers = samplers
    member x.BindGroupCount = bindGroupCount
    
    member x.Dispose() =
        shaderModule.Dispose()
        for KeyValue(_, group) in samplers do
            for KeyValue(_, s) in group do
                s.Dispose()
            
    interface IBackendSurface with
        member x.Handle = shaderModule.Handle
        member x.Dispose() = x.Dispose()

[<AbstractClass; Sealed>]
type WebGPUShaderExtensions private() =
    
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
    
    static let shaderProgramCache = ConcurrentDict<Device * FShade.WGSL.WGSLShader, ShaderProgram>(Dict())
    
    static let lineRx = System.Text.RegularExpressions.Regex @"\r\n|\n|\r"
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
            
            let inline write (shader : FShade.WGSL.WGSLShader) =
                Log.start "shader"
                for l in lineRx.Split shader.code do
                    Log.line "%s" l
                Log.stop()
                let data = FShade.WGSL.WGSLShader.pickle shader
                File.WriteAllBytes(cacheFile, data)
                shader
            
            if false && File.Exists cacheFile then
                try
                    let data = File.ReadAllBytes cacheFile
                    FShade.WGSL.WGSLShader.unpickle data
                with e ->
                    Log.warn "could not read cache-file: %A" e
                    effect.Link(signature)
                    |> ModuleCompiler.compileWGSL
                    |> write
            else
                effect.Link(signature)
                |> ModuleCompiler.compileWGSL
                |> write
        )
    
    [<Extension>]
    static member CreateShaderProgram(this : Device, wgsl : FShade.WGSL.WGSLShader) =
        shaderProgramCache.GetOrCreate((this, wgsl), fun (device, wgsl) ->
            let shaderModule = device.CompileShader(wgsl.code)
            let samplers = 
                wgsl.iface.samplers
                |> MapExt.toList |> List.map (fun (k, v) ->
                    v.samplerGroup, (v.samplerBinding, device.CreateSampler(v.samplerState))
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
            
            new ShaderProgram(shaderModule, wgsl, groupLayouts, pipelineLayout, samplers)   
        )

    [<Extension>]
    static member CreateShaderProgram(this : Device, effect : FShade.Effect, signature : IFramebufferSignature) =
        let wgsl = effect.GetWGSLCode(signature)
        this.CreateShaderProgram(wgsl)

    
       