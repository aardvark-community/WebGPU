namespace FShade.WGSL

open System
open System.Reflection

open Aardvark.Base
open Aardvark.Base.Monads.State

open FShade
open FShade.Imperative
open FShade.WGSL.Utilities
open FShade.WGSL.WGSLShaderInterface
open FSharp.Data.Adaptive

[<AutoOpen>]
module private Utils =
    
    let getSamplerName (n : string) =
        $"wgsl_{n}_sampler"
    
    
    let private rx = System.Text.RegularExpressions.Regex @"^wgsl_([a-zA-Z_0-9]+)_sampler$"
    
    let tryGetTextureName (n : string) =
        let m = rx.Match n
        if m.Success then Some m.Groups.[1].Value
        else None
        
type BindingMode =
    | None = 0
    | Global = 1
    | PerKind = 2


type Config =
    {
        createUniformBuffers    : bool

       
        bindingMode             : BindingMode
        createDescriptorSets    : bool
        stepDescriptorSets      : bool
        createInputLocations    : bool
        createOutputLocations   : bool
        createPassingLocations  : bool
        createPerStageUniforms  : bool
        reverseMatrixLogic      : bool

        doubleAsFloat           : bool
        
        depthWriteMode          : bool
        useInOut                : bool
    }

type WGSLShader =
    {
        code        : string
        iface       : WGSLProgramInterface
    }

    override x.ToString() =
        String.Join(Environment.NewLine,
            "====================== CODE ======================",
            x.code,
            "====================== CODE ======================",
            "======================= IO =======================",
            x.iface.ToString(),
            "======================= IO =======================")
     
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module WGSLShader =
    open System.IO

    let serialize (dst : Stream) (shader : WGSLShader) =
        use w = new BinaryWriter(dst, System.Text.Encoding.UTF8, true)
        w.Write "WGSLShader"
        WGSLProgramInterface.serializeInternal w shader.iface
        w.Write shader.code
        
    let deserialize (src : Stream) =
        use r = new BinaryReader(src, System.Text.Encoding.UTF8, true)
        let s = r.ReadString()
        if s <> "WGSLShader" then failwith "not a WGSLShader"
        let iface = WGSLProgramInterface.deserializeInternal r
        let code = r.ReadString()
        { iface = iface; code = code }


    let tryDeserialize (src : Stream) =
        try deserialize src |> Some
        with _ -> None

    let pickle (shader : WGSLShader) =
        use ms = new MemoryStream()
        serialize ms shader
        ms.ToArray()

    let unpickle (data : byte[]) =
        use ms = new MemoryStream(data)
        deserialize ms
        
    let tryUnpickle (data : byte[]) =
        use ms = new MemoryStream(data)
        tryDeserialize ms

       

type Backend private(config : Config) =
    inherit Compiler.Backend()
    static let table = System.Collections.Concurrent.ConcurrentDictionary<Config, Backend>()

    member x.Config = config

    static member Create (config : Config) =
        table.GetOrAdd(config, fun config ->
            Backend(config)
        )

    override x.TryGetIntrinsicMethod (c : MethodInfo) =
        match c with
            | IntrinsicFunction f -> Some f
            | TextureLookup (fmt, exts) -> Some ({ CIntrinsic.tagged fmt with additional = exts })
            | _ -> c.Intrinsic<GLSLIntrinsicAttribute>()

    override x.TryGetIntrinsicCtor (c : ConstructorInfo) =
        None

    override x.TryGetIntrinsicType (t : Type) =
        match t with
            | SamplerType(dim, arr, shadow, ms, valueType) ->
                let typ =
                    {
                        original = t
                        dimension = dim
                        isShadow = shadow
                        isArray = arr
                        isMS = ms
                        valueType = WGSLType.ofCType config.doubleAsFloat config.reverseMatrixLogic (CType.ofType x valueType)
                    }
                    
                let name = WGSLType.samplerName typ
                Some {
                    intrinsicTypeName = name
                    tag = WGSLTextureType.WGSLTexture typ
                }

            | ImageType(tFmt, dim, arr, ms, valueType) ->

                let typ =
                    {
                        original = t
                        dimension = dim
                        format = ImageFormat.ofFormatType tFmt
                        isArray = arr
                        isMS = ms
                        valueType = WGSLType.ofCType config.doubleAsFloat config.reverseMatrixLogic (CType.ofType x valueType)
                    }
                
                let name = WGSLType.imageName typ
                Some {
                    intrinsicTypeName = name
                    tag = WGSLTextureType.WGSLStorageTexture typ
                }

            | AccelerationStructure ->
                Some {
                    intrinsicTypeName = "accelerationStructureEXT"
                    tag = t
                }

            | _ ->
                None


type InputKind =
    | Any = 0
    | UniformBuffer = 1
    | StorageBuffer = 2
    | Texture = 3
    | Sampler = 4
    | Image = 5

type AssemblerState =
    {
        config                  : Config
        stages                  : ShaderStageDescription
        localSize               : V3i
        currentDescriptorSet    : int
        currentBinding          : Map<InputKind, int>
        currentInputLocation    : int
        currentOutputLocation   : int
        requiredExtensions      : Set<string>
        ifaceNew                : WGSLProgramInterface
        
        currentFunction         : Option<CFunctionSignature>
        functionInfo            : HashMap<CFunctionSignature, WGSLShaderInterface>

        uniformBuffers          : MapExt<string, WGSLUniformBuffer>
        textures                : MapExt<string, WGSLTexture>
        samplers                : MapExt<string, WGSLSampler>
        images                  : MapExt<string, WGSLImage>
        storageBuffers          : MapExt<string, WGSLStorageBuffer>

        textureInfos            : Map<string, list<string * SamplerState>>
        
        conditionals : MapExt<string, CExpr * CExpr * CExpr>
    }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module AssemblerState =
    let ofConfig (c : Config) =
        {
            config = c
            localSize = V3i.Zero
            stages = ShaderStageDescription.Graphics { prev = None; self = ShaderStage.Vertex; next = None }
            currentDescriptorSet = 0
            currentBinding = Map.empty
            currentInputLocation = 0
            currentOutputLocation = 0
            requiredExtensions = Set.empty
            ifaceNew =
                {
                    inputs                  = []
                    outputs                 = []
                    textures                = MapExt.empty
                    samplers                = MapExt.empty
                    images                  = MapExt.empty
                    storageBuffers          = MapExt.empty
                    uniformBuffers          = MapExt.empty
                    shaders                 = WGSLProgramShaders.Graphics { stages = MapExt.empty }
                }
                
            currentFunction = None
            functionInfo = HashMap.empty

            uniformBuffers          = MapExt.empty
            textures                = MapExt.empty
            samplers                = MapExt.empty
            images                  = MapExt.empty
            storageBuffers          = MapExt.empty

            
            textureInfos = Map.empty
            conditionals = MapExt.empty
        }


    let stages = State.get |> State.map (fun s -> s.stages)

    let tryGetBuiltInName (kind : ParameterKind) (name : string) =
        state {
            let! stages = stages
            match kind with
                | ParameterKind.Input ->
                    return Map.tryFind name builtInInputs.[stages.Stage]
                | ParameterKind.Output ->
                    if name = Intrinsics.Position && stages.Next = Some ShaderStage.Fragment then
                        return Some "position"
                    else
                        return Map.tryFind name builtInOutputs.[stages.Stage]
                | _ ->
                    return None
        }

    let reverseMatrixLogic = State.get |> State.map (fun s -> s.config.reverseMatrixLogic)
    let config = State.get |> State.map (fun s -> s.config)

    let newBinding (kind : InputKind) (cnt : int) =
        State.custom (fun s ->
            let c = s.config
            match c.bindingMode with
                | BindingMode.None ->
                    s, -1

                | BindingMode.Global ->
                    let b = Map.tryFind InputKind.Any s.currentBinding |> Option.defaultValue 0
                    { s with currentBinding = Map.add InputKind.Any (b + cnt) s.currentBinding }, b
                | _ -> //BindingMode.PerKind ->
                    
                    let b = Map.tryFind kind s.currentBinding |> Option.defaultValue 0
                    { s with currentBinding = Map.add kind (b + cnt) s.currentBinding }, b

        )

    let newSet =
        State.custom (fun s ->
            let c = s.config
            if c.createDescriptorSets then
                let set = s.currentDescriptorSet
                if c.stepDescriptorSets then
                    { s with currentDescriptorSet = set + 1; currentBinding = Map.empty }, set
                else
                    s, set
                    
            else
                s, -1
        )

    let rec private neededLocations (rev : bool) (t : CType) =
        match t with
            | CPointer(_,et) -> 
                neededLocations rev et

            | CType.CBool | CType.CFloat _ | CType.CInt _ ->
                1

            | CMatrix(et, rows, cols) ->
                if rev then
                    let l = CVector(et, cols) |> neededLocations rev
                    l * rows
                else
                    let l = CVector(et, rows) |> neededLocations rev
                    l * cols
            | CVector(et,_) -> 
                1
            | CArray(et, len) ->
                let inner = neededLocations rev et
                inner * len

            | CStruct _ | CIntrinsic _ | CVoid ->
                1 //failwith "[GLSL] no struct inputs allowed"

    let newLocation (kind : ParameterKind) (t : CType) =
        State.custom (fun s ->
            match kind with
                | ParameterKind.Input ->
                    let free = s.currentInputLocation
                    { s with currentInputLocation = free + neededLocations s.config.reverseMatrixLogic t }, free
                | ParameterKind.Output ->
                    let free = s.currentOutputLocation
                    { s with currentOutputLocation = free + neededLocations s.config.reverseMatrixLogic t }, free
                | _ ->
                    s, -1
                    
        )

  
module Interface =
    let private modify (f : AssemblerState -> WGSLProgramInterface -> WGSLProgramInterface) =
        State.modify (fun (s : AssemblerState) ->
            { s with ifaceNew = f s s.ifaceNew }
        )

    let private emptyShader =
        {
            program                         = Unchecked.defaultof<_>
            shaderStage                     = ShaderStage.Compute
            shaderEntry                     = ""

            shaderInputs                    = []
            shaderOutputs                   = []
            shaderSamplers                  = HashSet.empty
            shaderImages                    = HashSet.empty
            shaderStorageBuffers            = HashSet.empty
            shaderUniformBuffers            = HashSet.empty
            shaderAccelerationStructures    = HashSet.empty
            shaderBuiltInFunctions          = HashSet.empty
            shaderDecorations               = []
            shaderBuiltIns                  = MapExt.empty
            shaderIntrinsicInputs           = HashSet.empty
        }

    let private updateShaderInterface (action : AssemblerState -> WGSLShaderInterface -> WGSLShaderInterface) =
        State.modify (fun s ->
            match s.currentFunction with
                | Some f ->
                    let info =
                        match HashMap.tryFind f s.functionInfo with
                            | Some i -> i
                            | None -> emptyShader
                    let newInfo  = action s info
                    { s with functionInfo = HashMap.add f newInfo s.functionInfo }
                | None ->
                    { s with
                        ifaceNew =
                            { s.ifaceNew with
                                shaders =
                                    (s.stages, s.ifaceNew.shaders) ||> WGSLProgramShaders.alter (function Some sh -> Some (action s sh) | None -> None)
                            }
                    }

        )

    let addDecorations (stage : ShaderStage) (dec : list<EntryDecoration>) =
        let dec = 
            dec |> List.choose (fun e ->
                match e with
                    | EntryDecoration.InputTopology t -> WGSLInputTopology t |> Some
                    | EntryDecoration.Invocations 1 -> None
                    | EntryDecoration.Invocations t -> WGSLInvocations t |> Some
                    | EntryDecoration.LocalSize t -> WGSLLocalSize t |> Some
                    | EntryDecoration.OutputTopology t -> WGSLOutputTopology t |> Some
                    | EntryDecoration.OutputVertices t -> WGSLMaxVertices t |> Some
                    | EntryDecoration.Stages _ -> None
            )

        let dec =
            match stage with
                | ShaderStage.TessEval -> [WGSLSpacing WGSLSpacing.Equal; WGSLWinding WGSLWinding.CCW] @ dec
                | _ -> dec

        match dec with
            | [] -> State.value ()
            | _ -> updateShaderInterface (fun _ s -> { s with shaderDecorations = s.shaderDecorations @ dec })



    let newShader (entry : string) =
        modify (fun s iface ->
            let adjust (o : Option<WGSLShaderInterface>) =
                match o with
                    | Some o ->
                        Some o
                    | None ->
                        match s.stages.Stage with
                            | ShaderStage.Fragment ->
                                Some {
                                    emptyShader with 
                                        shaderStage             = ShaderStage.Fragment
                                        shaderEntry             = entry
                                        shaderBuiltIns          = MapExt.ofList [ ParameterKind.Input, MapExt.ofList ["gl_Position", WGSLType.Vec(4, WGSLType.Float 32) ] ]
                                }

                            | stage -> 
                                Some {
                                    emptyShader with 
                                        shaderStage             = stage
                                        shaderEntry             = entry
                                }

            { iface with shaders = (s.stages, iface.shaders) ||> WGSLProgramShaders.alter adjust }

        )

    let useBuiltIn (kind : ParameterKind) (name : string) (ctype : CType) =
        modify (fun state iface ->
            let shaders = 
                (state.stages, iface.shaders) ||> WGSLProgramShaders.alter (
                    function 
                    | Some o ->
                        Some { 
                            o with 
                                shaderBuiltIns =
                                    o.shaderBuiltIns |> MapExt.alter kind (fun s ->
                                        let s = s |> Option.defaultValue MapExt.empty
                                        MapExt.add name (WGSLType.ofCType state.config.doubleAsFloat state.config.reverseMatrixLogic ctype) s |> Some
                                    )
                        }
                    | None ->
                        None

                )

            { iface with shaders = shaders }
        )

    let addInput (location : int) (name : string) (parameter : CEntryParameter) =
        modify (fun s iface ->
            
            let ip =
                {
                    paramType           = WGSLType.ofCType s.config.doubleAsFloat s.config.reverseMatrixLogic parameter.cParamType
                    paramLocation       = location
                    paramName           = name
                    paramSemantic       = parameter.cParamSemantic
                    paramInterpolation  = parameter.cParamDecorations |> Seq.tryPick (function ParameterDecoration.Interpolation m -> Some m | _ -> None)
                }
                
            let iface = 
                match s.stages.Previous with
                | None -> { iface with  inputs = iface.inputs @ [ip] }
                | Some _ -> iface

            { iface with
                shaders = 
                    (s.stages, iface.shaders)
                    ||> WGSLProgramShaders.alter (
                        function 
                        | Some o -> Some { o with shaderInputs = o.shaderInputs @ [ip] } 
                        | None -> None
                    )
            }
        )

    let addOutput (location : int) (name : string) (parameter : CEntryParameter) =
        modify (fun s iface ->
            
            let op =
                {
                    paramType           = WGSLType.ofCType s.config.doubleAsFloat s.config.reverseMatrixLogic parameter.cParamType
                    paramLocation       = location
                    paramName           = name
                    paramSemantic       = parameter.cParamSemantic
                    paramInterpolation  = parameter.cParamDecorations |> Seq.tryPick (function ParameterDecoration.Interpolation m -> Some m | _ -> None)
                }
                
            let iface = 
                match s.stages.Next with
                | None -> { iface with outputs = iface.outputs @ [op] }
                | Some _ -> iface

            { iface with
                shaders = 
                    (s.stages, iface.shaders)
                    ||> WGSLProgramShaders.alter (
                        function 
                        | Some o -> Some { o with shaderOutputs = o.shaderOutputs @ [op] } 
                        | None -> None
                    )
            }
        )

    let addStorageBuffer (ssb : WGSLStorageBuffer) =
        State.modify (fun (s : AssemblerState) ->
            let iface = 
                { s.ifaceNew with
                    storageBuffers = MapExt.add ssb.ssbName ssb s.ifaceNew.storageBuffers
                    shaders = 
                        (s.stages, s.ifaceNew.shaders)
                        ||> WGSLProgramShaders.alter (
                            function 
                            | Some s -> Some { s with shaderStorageBuffers = HashSet.add ssb.ssbName s.shaderStorageBuffers } 
                            | None -> None
                        )
                }
                
            { s with ifaceNew = iface; storageBuffers = MapExt.add ssb.ssbName ssb s.storageBuffers }
        )
 
    let addUniformBuffer (ub : WGSLUniformBuffer) =
        State.modify (fun (s : AssemblerState) ->
            let iface = 
                { s.ifaceNew with
                    uniformBuffers = MapExt.add ub.ubName ub s.ifaceNew.uniformBuffers
                    shaders = 
                        (s.stages, s.ifaceNew.shaders)
                        ||> WGSLProgramShaders.alter (
                            function 
                            | Some s -> Some { s with shaderUniformBuffers = HashSet.add ub.ubName s.shaderUniformBuffers } 
                            | None -> None
                        )
                }

            let buffers = ub.ubFields |> List.map (fun f -> f.ufName, ub) |> MapExt.ofList
            { s with ifaceNew = iface; uniformBuffers = MapExt.union s.uniformBuffers buffers }
        )
    
    let addTexture (sampler : WGSLTexture) =
        State.modify (fun (s : AssemblerState) ->
            match Map.tryFind sampler.textureName s.textureInfos with
            | Some infos -> 
                let sampler = { sampler with textureNames = infos |> List.map fst }
                let iface = 
                    { s.ifaceNew with
                        textures = MapExt.add sampler.textureName sampler s.ifaceNew.textures
                        shaders = 
                            (s.stages, s.ifaceNew.shaders)
                            ||> WGSLProgramShaders.alter (
                                function 
                                | Some s -> Some { s with shaderSamplers = HashSet.add sampler.textureName s.shaderSamplers } 
                                | None -> None
                            )
                    }
                    
                { s with ifaceNew = iface; textures = MapExt.add sampler.textureName sampler s.textures }
            | None ->
                s
        )
 
    let addSampler (sampler : WGSLSampler) =
        State.modify (fun (s : AssemblerState) ->
            let iface = 
                { s.ifaceNew with
                    samplers = MapExt.add sampler.samplerName sampler s.ifaceNew.samplers
                }
                
            { s with ifaceNew = iface; samplers = MapExt.add sampler.samplerName sampler s.samplers }
        )
 
    let addImage (image : WGSLImage) =
        State.modify (fun (s : AssemblerState) ->
            let iface = 
                { s.ifaceNew with
                    images = MapExt.add image.imageName image s.ifaceNew.images
                    shaders = 
                        (s.stages, s.ifaceNew.shaders)
                        ||> WGSLProgramShaders.alter (
                            function 
                            | Some s -> Some { s with shaderImages = HashSet.add image.imageName s.shaderImages } 
                            | None -> None
                        )
                }
                
            { s with ifaceNew = iface; images = MapExt.add image.imageName image s.images }
        )

    let newFunction (f : CFunctionSignature) =
        State.modify (fun s -> { s with currentFunction = Some f })
        
    let endFunction =
        State.modify (fun s -> { s with currentFunction = None })

    let callBuiltIn (name : string) (tag : obj) (args : CType[]) (ret : CType) =
        updateShaderInterface (fun s iface ->
            let f =
                {
                    name = name
                    tag = tag
                    args = args |> Array.map (WGSLType.ofCType s.config.doubleAsFloat s.config.reverseMatrixLogic)
                    ret = ret |> WGSLType.ofCType s.config.doubleAsFloat s.config.reverseMatrixLogic
                }
            { iface with shaderBuiltInFunctions = HashSet.add f iface.shaderBuiltInFunctions}
        )

    let useIntrinsicInput (name : FShade.WGSL.Utilities.WGSLIntrinsicInput) =
        updateShaderInterface (fun s iface ->
            { iface with shaderIntrinsicInputs = HashSet.add name iface.shaderIntrinsicInputs }    
        )
    
    let callFunction (f : CFunctionSignature) =
        updateShaderInterface (fun s iface ->
            match HashMap.tryFind f s.functionInfo with
                | Some info ->
                    { iface with
                        shaderUniformBuffers = HashSet.union iface.shaderUniformBuffers info.shaderUniformBuffers
                        shaderStorageBuffers = HashSet.union iface.shaderStorageBuffers info.shaderStorageBuffers
                        shaderSamplers = HashSet.union iface.shaderSamplers info.shaderSamplers
                        shaderImages = HashSet.union iface.shaderImages info.shaderImages
                        shaderAccelerationStructures = HashSet.union iface.shaderAccelerationStructures info.shaderAccelerationStructures
                        shaderBuiltInFunctions = HashSet.union iface.shaderBuiltInFunctions info.shaderBuiltInFunctions
                    }
                | None ->
                    iface
        )
     

    let useUniform (name : string) =
        updateShaderInterface (fun s shader ->
            let uniform = 
                MapExt.tryFind name s.uniformBuffers 
                |> Option.map (fun v s -> { s with shaderUniformBuffers = HashSet.add v.ubName s.shaderUniformBuffers })

            let storage =
                MapExt.tryFind name s.storageBuffers 
                |> Option.map (fun v s -> { s with shaderStorageBuffers = HashSet.add v.ssbName s.shaderStorageBuffers })

            let sampler = 
                MapExt.tryFind name s.textures 
                |> Option.map (fun v s -> { s with shaderSamplers = HashSet.add v.textureName s.shaderSamplers })

            let image =
                MapExt.tryFind name s.images 
                |> Option.map (fun v s -> { s with shaderImages = HashSet.add v.imageName s.shaderImages })
     
            let all =
                [
                    match uniform with | Some u -> yield u | None -> ()
                    match storage with | Some u -> yield u | None -> ()
                    match sampler with | Some u -> yield u | None -> ()
                    match image with | Some u -> yield u | None -> ()
                ]
            all |> List.fold (fun s v -> v s) shader
        )
      
         

module Assembler =
    
    [<Struct>]
    type Identifier(str : string) = 
        member x.Name = str

    open System.Text.RegularExpressions

    [<AutoOpen>] 
    module Patterns =

        let (|CTexture|_|) (t : CType) =
            match t with   
            | CIntrinsic { tag = (:? WGSLTextureType as t)} -> Some (t, 1)
            | CArray(CIntrinsic { tag = (:? WGSLTextureType as t)}, len) -> Some (t, len)
            | _ -> None

        let (|CAccelerationStructure|_|) (t : CType) =
            match t with
            | CIntrinsic { tag = (:? Type as t) } when t = typeof<Scene> -> Some ()
            | _ -> None


    let reservedNames =
        let str = 
            String.concat "|" [
               
                "i8|i16|i32|i64|u8|u16|u32|u64|f32|f64|vec2|vec3|vec4"
                "input|output"
                "precision|highp|mediump|lowp"
                "break|case|continue|default|discard|do|else|for|if|return|switch|while"
                "void|bool|int|uint|float|double|vec[2|3|4]|dvec[2|3|4]|bvec[2|3|4]|ivec[2|3|4]|uvec[2|3|4]|mat[2|3|4]"
                "mat2x2|mat2x3|mat2x4|mat3x2|mat3x3|mat3x4|mat4x2|mat4x3|mat4x4|dmat2|dmat3|dmat4|dmat2x2|dmat2x3|dmat2x4|dmat3x2|dmat3x3|dmat3x4|dmat4x2|dmat4x3|dmat4x4"
                "sampler[1|2|3]D|image[1|2|3]D|samplerCube|imageCube|sampler2DRect|image2DRect|sampler[1|2]DArray|image[1|2]DArray|samplerBuffer|imageBuffer|sampler2DMS|image2DMS|sampler2DMSArray|image2DMSArray|samplerCubeArray|imageCubeArray|sampler[1|2]DShadow|sampler2DRectShadow|sampler[1|2]DArrayShadow|samplerCubeShadow|samplerCubeArrayShadow|isampler[1|2|3]D|iimage[1|2|3]D|isamplerCube|iimageCube|isampler2DRect|iimage2DRect|isampler[1|2]DArray|iimage[1|2]DArray|isamplerBuffer|iimageBuffer|isampler2DMS|iimage2DMS|isampler2DMSArray|iimage2DMSArray|isamplerCubeArray|iimageCubeArray|atomic_uint|usampler[1|2|3]D|uimage[1|2|3]D|usamplerCube|uimageCube|usampler2DRect|uimage2DRect|usampler[1|2]DArray|uimage[1|2]DArray|usamplerBuffer|uimageBuffer|usampler2DMS|uimage2DMS|usampler2DMSArray|uimage2DMSArray|usamplerCubeArray|uimageCubeArray|struct"
                "asm|class|union|enum|typedef|template|this|packed|goto|switch|defaultinlinenoinline|volatile|public|static|extern|external|interface|long|short|double|half|fixed|unsigned|lowp|mediump|highp|precision|input|output|hvec2|hvec3|hvec4|dvec2|dvec3|dvec4|fvec2|fvec3|fvec4|sampler2DRect|sampler3DRect|sampler2DRectShadow|sizeof|castnamespace|using"
                "layout|attribute|centroid|sampler|patch|const|flat|in|inout|invariant|noperspective|out|smooth|uniform|varying|buffer|shared|coherent|readonly|writeonly"
                "abs|acos|all|any|asin|atan|ceil|clamp|cos|cross|degrees|dFdx|dFdy|distance|dot|equal|exp|exp2|faceforward|floor|fract|ftransform|fwidth|greaterThan|greaterThanEqual|inversesqrt|length|lessThan|lessThanEqual|log|log2|matrixCompMult|max|min|mix|mod|noise[1-4]|normalize|not|notEqual|outerProduct|pow|radians|reflect|refract|shadow1D|shadow1DLod|shadow1DProj|shadow1DProjLod|shadow2D|shadow2DLod|shadow2DProj|shadow2DProjLod|sign|sin|smoothstep|sqrt|step|tan|texture1D|texture1DLod|texture1DProj|texture1DProjLod|texture2D|texture2DLod|texture2DProj|texture2DProjLod|texture3D|texture3DLod|texture3DProj|texture3DProjLod|textureCube|textureCubeLod|transpose"
                "(gl_.*)"
            ]
        Regex("^(" + str + ")$")


    let checkName (name : string) =
        if name.Contains "__" then
            failwithf "[WGSL] %A is not a WGSL compatible name" name

        if reservedNames.IsMatch name then
            failwithf "[WGSL] %A is not a WGSL compatible name" name
            
        Identifier(name)

    let wgslName (name : string) =
        let name = name.Replace("__", "_")
        if reservedNames.IsMatch name then
            Identifier("_" + name)
        else
            Identifier(name)

    let parameterNameS (kind : ParameterKind) (name : string) =
        state {
            let! stages = AssemblerState.stages
            // let name =
            //     if name = Intrinsics.FragmentPosition then Intrinsics.Position
            //     else name

            
            match kind, stages.Previous, stages.Next with
            | _ when stages.Stage = ShaderStage.Compute ->
                return checkName name
            | ParameterKind.Input, None, _ ->
                return checkName name
            | ParameterKind.Input, _, _ ->
                return ShaderStage.prefix stages.Stage + "_" + name |> wgslName
            | ParameterKind.Output, _, Some n ->
                if name = Intrinsics.Position && n = ShaderStage.Fragment then
                    return "position" |> wgslName
                elif name = Intrinsics.FragmentPosition && n = ShaderStage.Fragment then
                    return ShaderStage.prefix n + "_" + Intrinsics.Position |> wgslName
                else
                    return ShaderStage.prefix n + "_" + name |> wgslName
            | ParameterKind.Output, _, None ->
                  let name = name + "Out"
                  return checkName name

            | _ ->
                return checkName name
        }

    let rec assembleType (doubleAsFloat : bool) (rev : bool) (t : CType) =
        match t with
            | CType.CBool                               -> "bool"  |> Identifier
            | CType.CVoid                               -> "void"  |> Identifier
            | CType.CInt(true, (8 | 16 | 32))           -> "i32"   |> Identifier
            | CType.CInt(false, (8 | 16 | 32))          -> "u32"  |> Identifier

            | CType.CInt(true, 64)                      -> "i64" |> Identifier
            | CType.CInt(false, 64)                     -> "u64" |> Identifier

            | CType.CFloat(16)                          -> "f16"  |> Identifier
            | CType.CFloat 32                           -> "f32" |> Identifier
            | CType.CFloat 64                           -> Identifier (if doubleAsFloat then "f32" else "f64")
                
            | CType.CVector(CType.CInt(true, (8 | 16 | 32 | 64)), d)   -> "vec" + string d + "i" |> Identifier
            | CType.CVector(CType.CInt(false, (8 | 16 | 32 | 64)), d)  -> "vec" + string d + "u" |> Identifier
            | CType.CVector(CType.CFloat(32), d)   -> "vec" + string d + "f" |> Identifier
            | CType.CVector(CType.CFloat(64), d)   ->
                if doubleAsFloat then "vec" + string d + "f" |> Identifier
                else "vec" + string d + "<f64>" |> Identifier
                
            | CType.CMatrix(CType.CFloat(bits), r,c) ->
                let name = 
                    if rev then "mat" + string r + "x" + string c
                    else "mat" + string c + "x" + string r
                if bits = 32 || doubleAsFloat then Identifier (name + "<f32>")
                else Identifier (name + "<f64>")

            | CType.CArray(t, l)                        -> $"array<{(assembleType doubleAsFloat rev t).Name}, {l}>" |> Identifier
            | CType.CStruct(n,_,_)                      -> wgslName n

            | CType.CPointer(_, t)                      -> (assembleType doubleAsFloat rev t).Name |> sprintf "array<%s>" |> Identifier

            | CType.CIntrinsic it                       -> it.intrinsicTypeName |> Identifier

            | _ -> failwithf "[GLSL] cannot assemble type %A" t 

    let assembleDeclaration (doubleAsFloat : bool) (rev : bool) (t : CType) (name : Identifier) =
        sprintf "%s : %s" name.Name (assembleType doubleAsFloat rev t).Name
        

    let assembleParameter (doubleAsFloat : bool) (rev : bool) (p : CParameter) =
        let modifier =
            match p.modifier with
                | CParameterModifier.In -> ""
                | CParameterModifier.ByRef -> failwith "inout "
                | CParameterModifier.Out -> failwith "out "

        let parType =
            match p.ctype with
            | CType.CBool -> CType.CInt(true, 32)
            | t -> t
        let decl = assembleDeclaration doubleAsFloat rev parType (wgslName p.name)
        sprintf "%s%s" modifier decl

    let assembleFunctionSignature (doubleAsFloat : bool) (rev : bool) (s : CFunctionSignature) =
        let ret = s.returnType |> assembleType doubleAsFloat rev
        let args = s.parameters |> Seq.map (assembleParameter doubleAsFloat rev) |> String.concat ", "
        let name = wgslName s.name
        sprintf "fn %s(%s) -> %s" name.Name args ret.Name

    let assembleLiteral (t : CType) (l : CLiteral) =
        match l with
        | CLiteral.CBool v -> if v then "true" else "false"
        | CLiteral.Null -> "null"
        | CLiteral.CIntegral v ->
            let suffix =
                match t with
                | CType.CInt(false, _) -> "u"
                | _ -> ""

            string v + suffix

        | CLiteral.CFractional v ->
            let str = v.ToString(System.Globalization.CultureInfo.InvariantCulture)
            if str.Contains "." || str.Contains "E" || str.Contains "e" then str
            else str + ".0"

        | CLiteral.CString v -> "\"" + v + "\""

    let rec assembleVecSwizzle (c : list<CVecComponent>) =
        match c with
            | CVecComponent.X :: rest -> "x" + assembleVecSwizzle rest
            | CVecComponent.Y :: rest -> "y" + assembleVecSwizzle rest
            | CVecComponent.Z :: rest -> "z" + assembleVecSwizzle rest
            | CVecComponent.W :: rest -> "w" + assembleVecSwizzle rest
            | _ -> ""

    let rec assembleExprS (e : CExpr) =
        state {
            let! config = AssemblerState.config
            match e with
                | CVar v ->
                    let name = wgslName v.name
                    return name.Name

                | CValue(t, v) ->
                    return assembleLiteral t v

                | CCall(func, args) ->
                    do! Interface.callFunction func
                    let name = wgslName func.name
                    let! args = args |> assembleExprsS ", "
                    return sprintf "%s(%s)" name.Name args

                | CReadInput(kind, typ, name, index) ->
                    let! name = parameterNameS kind name
                    match kind with
                    | ParameterKind.Uniform -> do! Interface.useUniform name.Name
                    | _ -> ()

                    let! s = State.get
                    match index with
                    | Some index ->
                        let! index = assembleExprS index
                        
                        let expr = 
                            match kind with
                            | ParameterKind.Input | ParameterKind.Argument ->
                                if s.stages.Stage = ShaderStage.Compute then $"{name.Name}[{index}]"
                                else $"input.{name.Name}[{index}]" //sprintf "%s[%s]" name.Name index
                            | ParameterKind.Uniform ->
                                match MapExt.tryFind name.Name s.uniformBuffers with
                                | Some ub -> $"{ub.ubName}.{name.Name}[{index}]"
                                | None ->
                                    match MapExt.tryFind name.Name s.storageBuffers with
                                    | Some sb -> $"{name.Name}[{index}]"
                                    | None ->
                                        match MapExt.tryFind name.Name s.textures with
                                        | Some sam -> $"{name.Name}[{index}]"
                                        | None -> $"{name.Name}[{index}]"
                            | _ ->
                                failwithf "cannot read: %A" kind
                            
                        match typ with
                        | CType.CBool -> return $"({expr} != 0)"
                        | _ -> return expr
                    | None ->
                        match kind with
                        | ParameterKind.Input | ParameterKind.Argument ->
                            if s.stages.Stage = ShaderStage.Compute then return name.Name
                            else return $"input.{name.Name}" //sprintf "%s[%s]" name.Name index
                        | ParameterKind.Uniform ->
                            let! s = State.get
                            let expr = 
                                match MapExt.tryFind name.Name s.uniformBuffers with
                                | Some ub -> $"{ub.ubName}.{name.Name}"
                                | None ->
                                    match MapExt.tryFind name.Name s.storageBuffers with
                                    | Some sb -> $"{name.Name}"
                                    | None -> 
                                        match MapExt.tryFind name.Name s.textures with
                                        | Some sam -> name.Name
                                        | None -> name.Name
                            match typ with
                            | CType.CBool -> return $"({expr} != 0)"
                            | _ -> return expr
                        | _ ->
                            return failwithf "cannot read: %A" kind

                | CCallIntrinsic(ret, func, args) ->
                    match func.additional with
                        | null -> ()
                        | :? Set<string> as exts -> do! State.modify (fun s -> { s with requiredExtensions = Set.union s.requiredExtensions exts})
                        | _ -> ()

                    match func.tag with
                        | null ->
                            do! Interface.callBuiltIn func.intrinsicName null (args |> Array.map (fun e -> e.ctype)) ret
                            let! args = args |> assembleExprsS ", "
                            return sprintf "%s(%s)" func.intrinsicName args

                        | :? WGSLIntrinsicInput as input ->
                            do! Interface.useIntrinsicInput input
                            match input with
                            | WGSLIntrinsicInput.WorkGroupSize ->
                                let! s = State.get
                                return $"vec3i({s.localSize.X}, {s.localSize.Y}, {s.localSize.Z})"
                            | WGSLIntrinsicInput.GlobalId
                            | WGSLIntrinsicInput.LocalId
                            | WGSLIntrinsicInput.NumWorkGroups
                            | WGSLIntrinsicInput.WorkGroupId ->
                                return $"vec3i({WGSLIntrinsicInput.name input})"
                            
                            | WGSLIntrinsicInput.LocalIndex ->
                                return $"i32({WGSLIntrinsicInput.name input})"
                            | _ ->
                                return failwith $"bad WGSLIntrinsicInput: {input}"
                            
                        | :? string as format ->
                            let name =
                                let i = format.IndexOf("(")
                                if i >= 0 then format.Substring(0,i)
                                else format

                            do! Interface.callBuiltIn name format (args |> Array.map (fun e -> e.ctype)) ret

                            let! args = args |> Array.mapS (assembleExprS >> State.map (fun a -> a :> obj))
                            return 
                                try String.Format(format, args)
                                with _ -> failwithf "[FShade] invalid string format: %A (%d args)" format args.Length
                            
                        | _ ->
                            return failwithf "[FShade] invalid GLSL intrinsic: %A" func

                | CConditional(_, cond, i, e) ->
                    let! s = State.get
                    
                    
                    let! cond = assembleExprS cond
                    let! i = assembleExprS i
                    let! e = assembleExprS e
                    return sprintf "(%s ? %s : %s)" cond i e

                | CNeg(_, v) ->
                    let! v = assembleExprS v
                    return sprintf "(-%s)" v

                | CNot(_, v) ->
                    let! v = assembleExprS v
                    return sprintf "(!%s)" v

                | CAdd(_, l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "(%s + %s)" l r

                | CSub(_, l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "(%s - %s)" l r

                | CMul(_, l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "(%s * %s)" l r

                | CDiv(_, l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "(%s / %s)" l r

                | CMod(t, l, r) ->
                    match t with
                        | CInt _ | CVector(CInt _, _) | CMatrix(CInt _, _, _) ->
                            let! l = assembleExprS l
                            let! r = assembleExprS r
                            return sprintf "(%s %% %s)" l r
                        | _ ->
                            let! l = assembleExprS l
                            let! r = assembleExprS r
                            return sprintf "mod(%s, %s)" l r
                            

                | CMulMatMat(_, l, r) | CMulMatVec(_, l, r) | CMulVecMat(_, l, r) ->
                    let! reverse = AssemblerState.reverseMatrixLogic
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    if reverse then return sprintf "(%s * %s)" r l
                    else return sprintf "(%s * %s)" l r

                | CMatrixElement(_, m, r, c) ->
                    let! reverse = AssemblerState.reverseMatrixLogic
                    let! m = assembleExprS m
                    let! r = assembleExprS r
                    let! c = assembleExprS c
                    if reverse then return sprintf "%s[%s][%s]" m r c
                    else return sprintf "%s[%s][%s]" m c r

                | CConvertMatrix(t, m) ->
                    let t = assembleType config.doubleAsFloat config.reverseMatrixLogic t
                    let! m = assembleExprS m
                    return sprintf "%s(%s)" t.Name m


                | CTranspose(_, m) ->
                    let! m = assembleExprS m
                    return sprintf "transpose(%s)" m

                | CDot(_, l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "dot(%s, %s)" l r

                | CCross(_, l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "cross(%s, %s)" l r

                | CVecSwizzle(_, v, s) ->
                    let! v = assembleExprS v
                    let s = assembleVecSwizzle s
                    return sprintf "%s.%s" v s

                | CVecItem(_, v, i) ->
                    let! v = assembleExprS v
                    let! i = assembleExprS i
                    return sprintf "%s[%s]" v i
                    

                | CNewVector(r, args) ->
                    let! args = assembleExprsS ", " args
                    let t = assembleType config.doubleAsFloat config.reverseMatrixLogic r
                    return sprintf "%s(%s)" t.Name args

                | CVecLength(_, v) ->
                    let! v = assembleExprS v
                    return sprintf "length(%s)" v

                | CConvert(t, v) ->
                    let t = assembleType config.doubleAsFloat config.reverseMatrixLogic t
                    let! v = assembleExprS v
                    return sprintf "%s(%s)" t.Name v

                | CAnd(l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "(%s && %s)" l r

                | COr(l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "(%s || %s)" l r

                | CBitAnd(_, l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "(%s & %s)" l r

                | CBitOr(_, l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "(%s | %s)" l r

                | CBitXor(_, l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "(%s ^ %s)" l r
                    
                | CBitNot(_,v) ->
                    let! v = assembleExprS v
                    return sprintf "(~%s)" v

                | CLeftShift(_, l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "(%s << %s)" l r

                | CRightShift(_, l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "(%s >> %s)" l r

                | CVecAnyEqual(l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "any(%s == %s)" l r

                | CVecAllNotEqual(l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "all(%s != %s)" l r

                | CVecAnyLess(l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "any(%s < %s)" l r

                | CVecAllLess(l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "all(%s < %s)" l r

                | CVecAnyLequal(l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "any(%s <= %s)" l r

                | CVecAllLequal(l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "all(%s <= %s)" l r

                | CVecAnyGreater(l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "any(%s > %s)" l r

                | CVecAllGreater(l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "all(%s > %s)" l r

                | CVecAnyGequal(l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "any(%s >= %s)" l r

                | CVecAllGequal(l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "all(%s >= %s)" l r

                | CLess(l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "(%s < %s)" l r

                | CLequal(l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "(%s <= %s)" l r

                | CGreater(l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "(%s > %s)" l r

                | CGequal(l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "(%s >= %s)" l r

                | CEqual(l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "(%s == %s)" l r

                | CNotEqual(l, r) ->
                    let! l = assembleExprS l
                    let! r = assembleExprS r
                    return sprintf "(%s != %s)" l r

                | CAddressOf(_, v) ->
                    let! v = assembleExprS v
                    return sprintf "(&%s)" v

                | CField(_, v, f) ->
                    let! v = assembleExprS v
                    let f = wgslName f
                    return sprintf "%s.%s" v f.Name

                | CItem(_, CItem(_, (CReadInput _ as v), i), j) ->
                    let! v = assembleExprS v
                    let! i = assembleExprS i
                    let! j = assembleExprS j
                    return sprintf "%s[%s].data[%s]" v i j
                    

                | CItem(_, v, i) ->
                    let! v = assembleExprS v
                    let! i = assembleExprS i
                    return sprintf "%s[%s]" v i

                | CMatrixFromCols(t,cols) ->
                    let! cols = cols |> List.mapS assembleExprS
                    let! rev = AssemblerState.reverseMatrixLogic
                    let t = assembleType config.doubleAsFloat rev t

                    let code = sprintf "%s(%s)" t.Name (String.concat ", " cols)
                    if rev then return sprintf "transpose(%s)" code
                    else return code

                | CMatrixFromRows(t, rows) ->
                    let! rows = rows |> List.mapS assembleExprS
                    let! rev = AssemblerState.reverseMatrixLogic
                    let t = assembleType config.doubleAsFloat rev t

                    let code = sprintf "%s(%s)" t.Name (String.concat ", " rows)
                    if rev then return code
                    else return sprintf "transpose(%s)" code

                | CNewMatrix(t, elems) ->
                    match t with
                        | CMatrix(_,rows,cols) ->
                            let! elems = elems |> List.mapS assembleExprS
                            let! rev = AssemblerState.reverseMatrixLogic
                            let t = assembleType config.doubleAsFloat rev t

                            if rev then 
                                return sprintf "%s(%s)" t.Name (String.concat ", " elems)
                            else
                                // transpose
                                let elems =
                                    let elems = List.toArray elems
                                    [
                                        for ri in 0 .. rows - 1 do
                                            for ci in 0 .. cols - 1 do
                                                yield elems.[rows * ci + ri]
                                    ]

                                return sprintf "%s(%s)" t.Name (String.concat ", " elems)

                        | t ->
                            return failwithf "[GLSL] invalid matrix type %A" t
                
                | CMatrixRow(t, m, row) ->
                    match t with
                        | CVector(_,d) ->
                            let! m = assembleExprS m
                            let! row = assembleExprS row
                            let! rev = AssemblerState.reverseMatrixLogic
                            if rev then 
                                return sprintf "%s[%s]" m row
                            else 
                                let t = assembleType config.doubleAsFloat rev t
                                let args = List.init d (fun i -> sprintf "%s[%d][%s]" m i row)
                                return sprintf "%s(%s)" t.Name (String.concat ", " args)
                        | _ ->
                            return failwith "sadsadsad"

                | CMatrixCol(t, m, col) ->
                    match t with
                        | CVector(_,d) ->
                            let! m = assembleExprS m
                            let! col = assembleExprS col
                            let! rev = AssemblerState.reverseMatrixLogic
                            if rev then 
                                let t = assembleType config.doubleAsFloat rev t
                                let args = List.init d (fun i -> sprintf "%s[%d][%s]" m i col)
                                return sprintf "%s(%s)" t.Name (String.concat ", " args)
                            else
                                return sprintf "%s[%s]" m col
                        | _ ->
                            return failwith "sadsadsad"

                | CDebugPrintf(fmt, values) ->
                    let! fmt = assembleExprS fmt
                    let! values = values |> Array.mapS assembleExprS

                    if values.Length = 0 then
                        return sprintf "debugPrintfEXT(%s)" fmt
                    else
                        let values = values |> String.concat ", "
                        return sprintf "debugPrintfEXT(%s, %s)" fmt values
        }
    
    and assembleExprsS (join : string) (args : seq<CExpr>) =
        args |> Seq.mapS assembleExprS |> State.map (String.concat join)
     
    let assembleLExprS (e : CLExpr) =
        e |> CLExpr.toExpr |> assembleExprS
      
    let assembleRExprS (e : CRExpr) =
        state {
            let! config = AssemblerState.config
            match e with
                | CRExpr e -> 
                    return! assembleExprS e

                | CRArray(t, args) ->
                    let et =
                        match t with
                            | CArray(t,_) -> t
                            | CPointer(_,t) -> t
                            | _ -> t

                    let ct = assembleType config.doubleAsFloat config.reverseMatrixLogic et
                    let! args = args |> List.mapS assembleExprS |> State.map (String.concat ", ")
                    return failwith "array literals" //sprintf "%s[]( %s )" ct.Name args
        }

    let rec assembleStatementS (singleLine : bool) (s : CStatement) =
        state {
            let! config = AssemblerState.config
            let seq (s : seq<string>) =
                if singleLine then s |> String.concat " "
                else s |> String.concat "\r\n"

            match s with
                | CNop ->
                    return ""

                | CDo e ->
                    let! e = assembleExprS e
                    return sprintf "%s;" e

                | CDeclare(v, r) ->
                    let name = wgslName v.name
                    let! r = r |> Option.mapS assembleRExprS
                    let decl = assembleDeclaration config.doubleAsFloat config.reverseMatrixLogic v.ctype name
                    match r with
                        | Some r -> return sprintf "var %s = %s;" decl r
                        | None -> return sprintf "var %s;" decl

                | CWriteOutput(name, index, value) ->
                    let! name = parameterNameS ParameterKind.Output name
                    let! index = index |> Option.mapS assembleExprS

                    let name =
                        match index with
                            | Some index -> sprintf "%s[%s]" name.Name index
                            | None -> name.Name

                    let! s = State.get
                    match value with
                        | CRArray(t, args) ->
                            let! args = args |> List.mapS assembleExprS
                            if s.stages.Stage = ShaderStage.Compute then return args |> Seq.mapi (sprintf "%s[%d] = %s;" name) |> seq
                            else return args |> Seq.mapi (sprintf "output.%s[%d] = %s;" name) |> seq
                        | CRExpr e ->
                            let! value = assembleExprS e
                            if s.stages.Stage = ShaderStage.Compute then return sprintf "%s = %s;" name value
                            else return sprintf "output.%s = %s;" name value

                | CWrite(l, r) ->
                    let! l = assembleLExprS l
                    let! r = assembleExprS r
                    return sprintf "%s = %s;" l r

                | CIncrement(pre, v) ->
                    let! v = assembleLExprS v
                    if pre then return sprintf "++%s;" v
                    else return sprintf "%s++;" v

                | CDecrement(pre, v) ->
                    let! v = assembleLExprS v
                    if pre then return sprintf "--%s;" v
                    else return sprintf "%s--;" v

                | CIsolated statements ->
                    let! statements = statements |> List.mapS (assembleStatementS false) 
                    let inner = statements |> seq
                    return sprintf "{\r\n%s\r\n}" (String.indent inner)

                | CSequential statements ->
                    let! statements = statements |> List.mapS (assembleStatementS false) 
                    return statements |> seq

                | CReturnValue v ->
                    let! v = assembleExprS v
                    return sprintf "return %s;" v

                | CReturn -> 
                    return "return;"

                | CBreak -> 
                    return "break;"

                | CContinue -> 
                    return "continue;"

                | CFor(init, cond, step, body) ->
                    let! init = assembleStatementS true init
                    let! cond = assembleExprS cond
                    let! step = assembleStatementS true step
                    let! body = assembleStatementS false body

                    let init =
                        if init.EndsWith ";" then init
                        else init + ";"

                    let step =
                        if step.EndsWith ";" then step.Substring(0, step.Length - 1)
                        else step

                    return sprintf "for(%s %s; %s)\r\n{\r\n%s\r\n}" init cond step (String.indent body)

                | CWhile(guard, body) ->
                    let! guard = assembleExprS guard
                    let! body = assembleStatementS false body
                    return sprintf "while(%s)\r\n{\r\n%s\r\n}" guard (String.indent body)

                | CDoWhile(guard, body) ->
                    let! guard = assembleExprS guard
                    let! body = assembleStatementS false body
                    return sprintf "do\r\n{\r\n%s\r\n}\r\nwhile(%s);" (String.indent body) guard
                    
                | CIfThenElse(cond, i, CNop) ->
                    let! cond = assembleExprS cond
                    let! i = assembleStatementS false i
                    return sprintf "if(%s)\r\n{\r\n%s\r\n}" cond (String.indent i)

                | CIfThenElse(cond, CNop, e) ->
                    let! cond = assembleExprS cond
                    let! e = assembleStatementS false e
                    return sprintf "if(!(%s))\r\n{\r\n%s\r\n}" cond (String.indent e)

                | CIfThenElse(cond, i, e) ->
                    let! cond = assembleExprS cond
                    let! i = assembleStatementS false i
                    let! e = assembleStatementS false e
                    return sprintf "if(%s)\r\n{\r\n%s\r\n}\r\nelse\r\n{\r\n%s\r\n}" cond (String.indent i) (String.indent e)

                | CSwitch(value, cases) ->
                    return failwith "[WGSL] switch not implemented"

        }

    let private uniformLayout (isUniformBuffer : bool) (decorations : list<UniformDecoration>) (set : int) (binding : int) =

        let decorations =
            decorations |> List.choose (fun d ->
                match d with
                    | UniformDecoration.Format t -> Some t.Name
                    | UniformDecoration.BufferBinding _
                    | UniformDecoration.BufferDescriptorSet _ 
                    | UniformDecoration.FieldIndex _ -> None
            )
            
        let decorations =
            if binding >= 0 then (sprintf "@binding(%d)" binding) :: decorations
            else decorations

        let decorations =
            if set >= 0 then (sprintf "@group(%d)" set) :: decorations
            else decorations
        //
        // let decorations =
        //     if isUniformBuffer then "std140" :: decorations
        //     else decorations

        
        match decorations with
            | [] -> ""
            | d -> String.concat " " decorations + " "



    let assembleUniformsS (uniforms : list<CUniform>) =
        state {
            let! s = State.get
            let! config = AssemblerState.config
            let buffers =
                uniforms 
                    |> List.map (fun u -> match u.cUniformType with | CIntrinsic { tag = (:? WGSLTextureType) } -> { u with cUniformBuffer = None } | _ -> u)
                    |> List.groupBy (fun u -> u.cUniformBuffer)
                    |> List.collect (function (Some a, f) -> [Some a, f] | (None, f) -> f |> List.map (fun f -> None, [f]))

            let allHaveSets =
                buffers |> List.forall (fun (name, fields) ->
                    fields |> List.exists (fun u -> 
                        u.cUniformDecorations |> List.exists (function 
                            | UniformDecoration.BufferDescriptorSet s -> true
                            | _ -> false
                        )
                    )
                )
            let! set = 
                if allHaveSets then State.value 0
                else AssemblerState.newSet

            let! definitions =
                buffers |> List.mapS (fun (name, fields) ->
                    state {
                        let set =
                            let userSet = 
                                fields |> List.tryPick (fun u -> 
                                    u.cUniformDecorations |> List.tryPick (function 
                                        | UniformDecoration.BufferDescriptorSet s -> Some s
                                        | _ -> None
                                    )
                                )
                            match userSet with
                            | Some set -> set
                            | None -> set

                        let getBinding (kind : InputKind) (cnt : int) (fields : list<CUniform>) =
                            let userGiven =     
                                fields |> List.tryPick (fun f ->
                                    f.cUniformDecorations |> List.tryPick (function UniformDecoration.BufferBinding b -> Some b | _ -> None)
                                )
                            match userGiven with
                            | Some u -> State.value u
                            | None -> AssemblerState.newBinding kind cnt
                        
                        match name with
                            | Some "SharedMemory" ->
                                let defs = 
                                    fields |> List.map (fun u ->
                                        let def = assembleDeclaration config.doubleAsFloat config.reverseMatrixLogic u.cUniformType (checkName u.cUniformName)
                                        sprintf "var<shared> %s;" def
                                    )
                                return String.concat "\r\n" defs

                            | Some "StorageBuffer" ->
                               
                                let! buffers =
                                    fields |> List.mapS (fun field ->
                                        state {
                                            let! binding = getBinding InputKind.StorageBuffer 1 [field]
                                            let prefix = uniformLayout false [] set binding
                                            let name = checkName field.cUniformName

                                            match field.cUniformType with
                                            | CType.CPointer(_,ct) ->
                                                do! Interface.addStorageBuffer {
                                                    ssbGroup = set
                                                    ssbBinding = binding
                                                    ssbName = name.Name
                                                    ssbType = WGSLType.ofCType config.doubleAsFloat config.reverseMatrixLogic ct
                                                    ssbReadOnly = s.stages.Stage = ShaderStage.Compute
                                                }
                                                let typ = assembleType config.doubleAsFloat config.reverseMatrixLogic ct
                                                let! s = State.get
                                                let access =
                                                    if s.stages.Stage = ShaderStage.Compute then "read_write"
                                                    else "read" 
                                                return sprintf "%svar<storage, %s> %s : array<%s>;" prefix access name.Name typ.Name
                                            
                                            | ct ->
                                                return failwithf "[GLSL] not a storage buffer type: %A" ct

                                        }
                                    )

                                return buffers |> String.concat "\r\n"
                                
                            | Some bufferName when config.createUniformBuffers ->
                                let bufferName = checkName bufferName
                                let! binding = getBinding InputKind.UniformBuffer 1 fields
                                let decorations  = uniformLayout true [] set binding
                            
                                let fieldStr = 
                                    fields |> List.map (fun u ->
                                        let cType =
                                            match u.cUniformType with
                                            | CType.CBool -> CType.CInt(true, 32)
                                            | t -> t
                                            
                                        let decl = assembleDeclaration config.doubleAsFloat config.reverseMatrixLogic cType (checkName u.cUniformName)
                                        sprintf "%s" decl
                                    ) |> String.concat ",\r\n"
                                do! Interface.addUniformBuffer {
                                    ubGroup = set
                                    ubBinding = binding
                                    ubName = bufferName.Name
                                    ubFields = fields |> List.map (fun u -> { ufName = checkName(u.cUniformName).Name; ufType = WGSLType.ofCType config.doubleAsFloat config.reverseMatrixLogic u.cUniformType; ufOffset = -1 })
                                    ubSize = -1
                                }
                             
                                let code =
                                    String.concat "\r\n" [
                                        $"struct {bufferName.Name}Data {{"
                                        String.indent fieldStr
                                        $"}}"
                                        ""
                                        $"{decorations}var<uniform> {bufferName.Name} : {bufferName.Name}Data;"
                                    ]
                                return code
                                //return sprintf "%suniform %s\r\n{\r\n%s\r\n};" prefix bufferName.Name (String.indent fieldStr)

                            | _ ->
                                let! definitions = 
                                    fields |> List.mapS (fun u ->
                                        state {
                                            let mutable prefix = ""
                                            //let mutable samplerDef = None
                                            let mutable scope = None
                                            
                                            match u.cUniformType with
                                            | CIntrinsic t when t.intrinsicTypeName = "sampler" || t.intrinsicTypeName = "sampler_comparison" ->
                                                let! binding = getBinding InputKind.Sampler 1 [u]
                                                
                                                match tryGetTextureName u.cUniformName with
                                                | Some texName ->
                                                    let! s = State.get
                                                    match Map.tryFind texName s.textureInfos with
                                                    | Some l ->
                                                        let samplerState = l |> List.head |> snd
                                                
                                                        do! Interface.addSampler {
                                                            samplerName = u.cUniformName
                                                            samplerGroup = set
                                                            samplerBinding = binding
                                                            samplerState = samplerState
                                                        }
                                                    | None ->
                                                        failwithf "bad texture-name: %s" texName
                                                | None ->
                                                    failwithf "bad sampler-name: %s" u.cUniformName
                                                    
                                                
                                                prefix <- uniformLayout false u.cUniformDecorations set binding
                                                scope <- None
                                            | CTexture (t, cnt) ->
                                                match t with
                                                    | WGSLTextureType.WGSLTexture samplerType -> 
                                                        let! binding = getBinding InputKind.Texture cnt [u]
                                                        prefix <- uniformLayout false u.cUniformDecorations set binding

                                                        // let! samplerBinding = getBinding InputKind.Sampler 1 [u]
                                                        // samplerDef <- Some (sprintf "@group(%d) @binding(%d) var %sSampler : sampler;" set samplerBinding (checkName u.cUniformName).Name)
                                                        
                                                        
                                                        do! Interface.addTexture { 
                                                            textureGroup = set
                                                            textureBinding = binding
                                                            textureName = checkName(u.cUniformName).Name
                                                            textureCount = cnt
                                                            textureNames = [] // filled in addTexture
                                                            textureType = samplerType
                                                        }
                                                           
                                                    | WGSLTextureType.WGSLStorageTexture imageType ->
                                                        let! binding = getBinding InputKind.Image cnt [u]
                                                        prefix <- uniformLayout false u.cUniformDecorations set binding

                                                        do! Interface.addImage { 
                                                            imageGroup = set
                                                            imageBinding = binding
                                                            imageName = checkName(u.cUniformName).Name
                                                            imageType = imageType
                                                        }

                                            | CAccelerationStructure ->
                                                failwith "no RTX"

                                            | _ ->
                                                let! binding = getBinding InputKind.UniformBuffer 1 [u]
                                                prefix <- uniformLayout false u.cUniformDecorations set binding
                                                scope <- Some "uniform"
                                                ()

                                            let decl = assembleDeclaration config.doubleAsFloat config.reverseMatrixLogic u.cUniformType (checkName u.cUniformName)
                                            
                                            let scopeStr =
                                                match scope with
                                                | Some s -> $"<{s}>"
                                                | None -> ""
                                            let decl = sprintf "%svar%s %s;" prefix scopeStr decl
                                            return decl
                                            // match samplerDef with
                                            // | Some samplerDef -> return $"{decl}\r\n{samplerDef}"
                                            // | None -> return decl
                                        }
                                    )

                                return definitions |> String.concat "\r\n"
                    }
                )
                
            return String.concat "\r\n\r\n" definitions

                
        }

    let assembleDepthWriteMode (mode : DepthWriteMode) =
        match mode with
            | DepthWriteMode.Equal -> "depth_unchanged"
            | DepthWriteMode.OnlyLess -> "depth_less"
            | DepthWriteMode.OnlyGreater -> "depth_greater"
            | _ -> "depth_any"

    let assembleInterpolationMode (mode : InterpolationMode) =
        [ if mode.HasFlag InterpolationMode.Centroid then "centroid"
          if mode.HasFlag InterpolationMode.Flat then "flat"
          if mode.HasFlag InterpolationMode.NoPerspective then "noperspective"
          if mode.HasFlag InterpolationMode.Sample then "sample" ]

    let assembleEntryParameterS (kind : ParameterKind) (p : CEntryParameter) =
        state {
            let depthWrite = p.cParamDecorations |> Seq.tryPick (function ParameterDecoration.DepthWrite m -> Some m | _ -> None) |> Option.defaultValue DepthWriteMode.None
            let! stages = AssemblerState.stages
            let! builtIn = AssemblerState.tryGetBuiltInName kind p.cParamSemantic
            let prevStage = stages.Previous
            let selfStage = stages.Stage
            let nextStage = stages.Next
            
            let! config = AssemblerState.config

            let! set = 
                if config.createDescriptorSets then AssemblerState.newSet
                else State.value -1

            let mutable bSet = -1
            let mutable bBinding = -1

            let! decorations =
                p.cParamDecorations 
                |> Set.toList
                |> List.collectS (fun d ->
                    state {
                        match d with
                        | ParameterDecoration.DepthWrite _ ->
                            return []

                        | ParameterDecoration.Const -> 
                            return ["@const"]

                        | ParameterDecoration.Interpolation m ->
                                
                            let isFragmentInput =
                                (selfStage = ShaderStage.Fragment && kind = ParameterKind.Input) ||
                                (nextStage = Some ShaderStage.Fragment && kind = ParameterKind.Output)
                    
                            let isTessPatch =
                                (selfStage = ShaderStage.TessEval && kind = ParameterKind.Input) ||
                                (selfStage = ShaderStage.TessControl && kind = ParameterKind.Output)

                            match isTessPatch, isFragmentInput, m with
                                | _, true, mode -> return assembleInterpolationMode mode
                                | true, _, mode when mode.HasFlag InterpolationMode.PerPatch -> return ["@patch"]
                                | _ -> return []

                        | ParameterDecoration.StorageBuffer(read, write) ->
                            let! binding = AssemblerState.newBinding InputKind.StorageBuffer 1

                            let args = []

                            let args =
                                if binding >= 0 then sprintf "@binding(%d)" binding :: args
                                else args

                            let args =
                                if set >= 0 then sprintf "@group(%d)" set :: args
                                else args

                            bSet <- set
                            bBinding <- binding

                            //let args = args |> String.concat " "

                            let rw =
                                match read, write with
                                    | true, false -> "read"
                                    | _ -> "read_write"

                            let decorations = String.concat " " args
                            let typeName = assembleType config.doubleAsFloat config.reverseMatrixLogic p.cParamType
                            return [$"{decorations} var<storage, {rw}> {p.cParamName} : {typeName.Name};"]

                        | ParameterDecoration.Shared -> 
                            return ["@shared"]

                        | ParameterDecoration.Memory _ | ParameterDecoration.Slot _ ->
                            return []
                    }

                )

            let decorations = 
                match builtIn with
                | Some builtIn ->
                    $"@builtin({builtIn})" :: decorations
                | None ->
                    decorations
            
            let isBuffer = p.cParamDecorations |> Seq.tryPick (function ParameterDecoration.StorageBuffer(_, w) -> Some (not w) | _ -> None)
            
            let slot =
                p.cParamDecorations |> Seq.tryPick (function
                    | ParameterDecoration.Slot s -> Some s
                    | _ -> None
                )

            let! location =
                match builtIn with
                | Some _ ->
                    State.value None
                | None ->
                    match slot with
                    | Some slot -> State.value (Some slot)
                    | _ -> AssemblerState.newLocation kind p.cParamType |> State.map Some

            match isBuffer with
            | Some readOnly -> 
                match p.cParamType with
                | CType.CPointer(m,ct) ->
                    do! Interface.addStorageBuffer {
                        ssbGroup = bSet
                        ssbBinding = bBinding
                        ssbName = p.cParamName
                        ssbType = WGSLType.ofCType config.doubleAsFloat config.reverseMatrixLogic ct
                        ssbReadOnly = readOnly
                    }
                | _ ->
                    failwithf "[GLSL] not a storage buffer type: %A" p.cParamType
               
                return String.concat " " decorations |> Some
            | _ ->
            
                let layoutParams =
                    match location with
                    | Some location ->
                        match kind with
                        | ParameterKind.Input when config.createInputLocations && prevStage = None ->
                            [ sprintf "@location(%d)" location]
                        | ParameterKind.Output when config.createOutputLocations && nextStage = None ->
                            [ sprintf "@location(%d)" location]
                        | ParameterKind.Input | ParameterKind.Output when config.createPassingLocations ->
                            [ sprintf "@location(%d)" location]
                        | _ ->
                            []
                    | None ->
                        []

                let decorations =
                    match layoutParams with
                    | [] -> decorations
                    | _ -> sprintf "%s" (String.concat " " layoutParams) :: decorations
            
                let decorations = 
                    match decorations with
                    | [] -> ""
                    | _ -> String.concat " " decorations + " "

                let! name = parameterNameS kind p.cParamName

                let decorations, prefix, suffix =
                    match kind with
                    | ParameterKind.Input -> decorations, "", ""
                    | ParameterKind.Output -> decorations, "", ""
                    | _ -> decorations, "", ""
                


                if kind = ParameterKind.Input then
                    match location with
                    | Some location ->
                        do! Interface.addInput location name.Name p
                    | None ->
                        ()

                if kind = ParameterKind.Output then
                    match location with
                    | Some location ->
                        do! Interface.addOutput location name.Name p
                    | None ->
                        ()

                return sprintf "%s%s%s : %s" decorations prefix name.Name (assembleType config.doubleAsFloat config.reverseMatrixLogic p.cParamType).Name |> Some
        }
    
    
    let functionName (c : CIntrinsic) =
        match c.tag with
        | :? string as format ->
            let i = format.IndexOf("(")
            if i >= 0 then format.Substring(0,i)
            else format
        | _ ->
            c.intrinsicName
    
    
    let getSamplerExpr (samplerNames : Map<string, string>) (tex : CExpr) =
        match tex with
        | CExpr.CReadInput(ParameterKind.Uniform, _, name, None) ->
            (CExpr.CReadInput(ParameterKind.Uniform, CType.CIntrinsic { intrinsicTypeName = "sampler"; tag = null }, samplerNames.[name], None))
        | CExpr.CVar v ->
            (CExpr.CVar { name = samplerNames.[v.name]; ctype = CType.CIntrinsic { intrinsicTypeName = "sampler"; tag = null }})
        | _ ->
            failwith $"bad texture sample argument {tex}"
    
    let addSamplerArgsFunctionSignature (signature : CFunctionSignature) =
        let mutable samplerNames = Map.empty<string, string>
        let newParameters = 
            signature.parameters |> Array.collect (fun p ->
                match p.ctype with
                | CTexture(tex,_) ->
                    match tex with
                    | WGSLTexture s ->
                        let samplerType = if s.isShadow then "sampler_comparison" else "sampler"
                        let samName = getSamplerName p.name
                        samplerNames <- Map.add p.name samName samplerNames
                        [|
                            p
                            { name = samName; ctype = CType.CIntrinsic { intrinsicTypeName = samplerType; tag = null }; modifier = CParameterModifier.In }
                        |]
                    | _ ->
                        [| p  |]
                | _ ->
                    [| p |]
            )
        
        { signature with parameters = newParameters }, samplerNames
    
    let rec addSamplerArgsExpr (samplerNames : Map<string, string>) (e : CExpr) : CExpr =
        match e with
        | CCallIntrinsic(ret, f, args) ->
            match f.tag with
            | :? string as fmt when fmt.StartsWith "textureSample" ->
                let argRegex = System.Text.RegularExpressions.Regex @"\{([0-9]+)\}"
                let realFmt = 
                    argRegex.Replace(fmt, fun m ->
                        let id =  int m.Groups.[1].Value
                        if id = 0 then "{0}, {1}"
                        else $"{{{id+1}}}"
                    )
                    
                let tex = args.[0]
                let samplerInput = getSamplerExpr samplerNames tex
                
                let newArgs = Array.zeroCreate<CExpr> (args.Length + 1)
                newArgs.[0] <- tex
                newArgs.[1] <- samplerInput
                for i in 1 .. args.Length - 1 do
                    newArgs.[i+1] <- args.[i] 
                CCallIntrinsic(ret, CIntrinsic.tagged realFmt ,newArgs)
                
            | _ ->
                CCallIntrinsic(ret, f, args |> Array.map (addSamplerArgsExpr samplerNames))
            // match functionName f with
            // | "textureSample" ->
            //     let tex = args.[0]
            //     let samplerInput = getSamplerExpr samplerNames tex
            //     
            //     let newArgs = Array.zeroCreate<CExpr> (args.Length + 1)
            //     newArgs.[0] <- tex
            //     newArgs.[1] <- samplerInput
            //     for i in 1 .. args.Length - 1 do
            //         newArgs.[i+1] <- args.[i] 
            //     CCallIntrinsic(ret, CIntrinsic.simple "textureSample",newArgs)
            //     
            // | _ ->
            //     e
        | CAdd(t, l, r) -> CAdd(t, addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CSub(t, l, r) -> CSub(t, addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CMul(t, l, r) -> CMul(t, addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CDiv(t, l, r) -> CDiv(t, addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CMod(t, l, r) -> CMod(t, addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CAnd(l, r) -> CAnd(addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | COr(l, r) -> COr(addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CValue _ | CVar _ -> e
        | CCall(func, args) ->
            let newArgs =
                args |> Array.collect (fun a ->
                    let a = addSamplerArgsExpr samplerNames a
                    match a.ctype with
                    | CTexture _ ->
                        let sam = getSamplerExpr samplerNames a
                        [|
                            a
                            sam
                        |]
                    | _ ->
                        [|a|]
                )
            let newFunc, _ = addSamplerArgsFunctionSignature func
            CCall(newFunc, newArgs)
        | CConditional(ct, c, i, e) ->
            CConditional(ct, addSamplerArgsExpr samplerNames c, addSamplerArgsExpr samplerNames i, addSamplerArgsExpr samplerNames e)

        | CReadInput(kind, typ, name, index) ->
            match index with
            | Some index -> CReadInput(kind, typ, name, Some (addSamplerArgsExpr samplerNames index))
            | None -> e
            

        | CNeg(t, e) -> CNeg(t, addSamplerArgsExpr samplerNames e)
        | CNot(t, e) -> CNot(t, addSamplerArgsExpr samplerNames e)

        | CTranspose(t, e) -> CTranspose(t, addSamplerArgsExpr samplerNames e)
        
        | CMulMatMat(t, a, b) -> CMulMatMat(t, addSamplerArgsExpr samplerNames a, addSamplerArgsExpr samplerNames b)
        | CMulMatVec(t, a, b) -> CMulMatVec(t, addSamplerArgsExpr samplerNames a, addSamplerArgsExpr samplerNames b)
        | CMulVecMat(t, a, b) -> CMulVecMat(t, addSamplerArgsExpr samplerNames a, addSamplerArgsExpr samplerNames b)
        | CDot(t, a, b) -> CDot(t, addSamplerArgsExpr samplerNames a, addSamplerArgsExpr samplerNames b)
        | CCross(t, a, b) -> CCross(t, addSamplerArgsExpr samplerNames a, addSamplerArgsExpr samplerNames b)
        | CVecSwizzle(t, a, b) -> CVecSwizzle(t, addSamplerArgsExpr samplerNames a, b)
        | CVecItem(t, a, b) -> CVecItem(t, addSamplerArgsExpr samplerNames a, addSamplerArgsExpr samplerNames b)
        | CMatrixElement(t, m, r, c) -> CMatrixElement(t, addSamplerArgsExpr samplerNames m, addSamplerArgsExpr samplerNames r, addSamplerArgsExpr samplerNames c)
        | CConvertMatrix(t, m) -> CConvertMatrix(t, addSamplerArgsExpr samplerNames m)
        | CNewVector(t, components) -> CNewVector(t, components |> List.map (addSamplerArgsExpr samplerNames))
        | CNewMatrix(t, elements) -> CNewMatrix(t, elements |> List.map (addSamplerArgsExpr samplerNames))
        | CMatrixFromRows(t, rows) -> CMatrixFromRows(t, rows |> List.map (addSamplerArgsExpr samplerNames))
        | CMatrixFromCols(t, cols) -> CMatrixFromCols(t, cols |> List.map (addSamplerArgsExpr samplerNames))
        | CMatrixRow(t, m, r) -> CMatrixRow(t, addSamplerArgsExpr samplerNames m, addSamplerArgsExpr samplerNames r)
        | CMatrixCol(t, m, c) -> CMatrixCol(t, addSamplerArgsExpr samplerNames m, addSamplerArgsExpr samplerNames c)
        | CVecLength(t, v) -> CVecLength(t, addSamplerArgsExpr samplerNames v)
        | CConvert(t, e) -> CConvert(t, addSamplerArgsExpr samplerNames e)
        | CBitAnd(t, l, r) -> CBitAnd(t, addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CBitOr(t, l, r) -> CBitOr(t, addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CBitXor(t, l, r) -> CBitXor(t, addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CBitNot(t, e) -> CBitNot(t, addSamplerArgsExpr samplerNames e)
        | CLeftShift(t, l, r) -> CLeftShift(t, addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CRightShift(t, l, r) -> CRightShift(t, addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CVecAnyEqual(l, r) -> CVecAnyEqual(addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CVecAllNotEqual(l, r) -> CVecAllNotEqual(addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CVecAnyLess(l, r) -> CVecAnyLess(addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CVecAllLess(l, r) -> CVecAllLess(addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CVecAnyLequal(l, r) -> CVecAnyLequal(addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CVecAllLequal(l, r) -> CVecAllLequal(addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CVecAnyGreater(l, r) -> CVecAnyGreater(addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CVecAllGreater(l, r) -> CVecAllGreater(addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CVecAnyGequal(l, r) -> CVecAnyGequal(addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CVecAllGequal(l, r) -> CVecAllGequal(addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CLess(l, r) -> CLess(addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CLequal(l, r) -> CLequal(addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CGreater(l, r) -> CGreater(addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CGequal(l, r) -> CGequal(addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CEqual(l, r) -> CEqual(addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CNotEqual(l, r) -> CNotEqual(addSamplerArgsExpr samplerNames l, addSamplerArgsExpr samplerNames r)
        | CAddressOf(t, target) -> CAddressOf(t, addSamplerArgsExpr samplerNames target)
        | CField(t, target, fieldName) -> CField(t, addSamplerArgsExpr samplerNames target, fieldName)
        | CItem(t, target, index) -> CItem(t, addSamplerArgsExpr samplerNames target, addSamplerArgsExpr samplerNames index)
        | CDebugPrintf(fmt, values) -> CDebugPrintf(fmt, values |> Array.map (addSamplerArgsExpr samplerNames))
        
    let rec addSamplerArgsRExpr (samplerNames : Map<string, string>) (e : CRExpr) =
        match e with
        | CRExpr.CRExpr e -> CRExpr.CRExpr (addSamplerArgsExpr samplerNames e)
        | CRExpr.CRArray(t, args) -> CRExpr.CRArray(t, args |> List.map (addSamplerArgsExpr samplerNames))
        
    let rec addSamplerArgsLExpr (samplerNames : Map<string, string>) (e : CLExpr) =
        // type CLExpr =
        //     | CLVar of CVar
        //     | CLField of CType * CLExpr * string
        //     | CLItem of CType * CExpr * CExpr
        //     | CLPtr of CType * CExpr
        //     | CLVecSwizzle of CType * CLExpr * list<CVecComponent>
        //     | CLMatrixElement of t : CType * m : CLExpr * r : CExpr * c : CExpr
        //     | CLInput of kind : ParameterKind * ctype : CType * name : string * index : Option<CExpr>


        match e with
        | CLVar v -> CLVar v
        | CLField(t, target, fieldName) -> CLField(t, addSamplerArgsLExpr samplerNames target, fieldName)
        | CLItem(t, target, index) -> CLItem(t, addSamplerArgsExpr samplerNames target, addSamplerArgsExpr samplerNames index)
        | CLPtr(t, target) -> CLPtr(t, addSamplerArgsExpr samplerNames target)
        | CLVecSwizzle(t, target, components) -> CLVecSwizzle(t, addSamplerArgsLExpr samplerNames target, components)
        | CLMatrixElement(t, m, r, c) -> CLMatrixElement(t, addSamplerArgsLExpr samplerNames m, addSamplerArgsExpr samplerNames r, addSamplerArgsExpr samplerNames c)
        | CLInput(kind, ctype, name, index) -> CLInput(kind, ctype, name, index |> Option.map (addSamplerArgsExpr samplerNames))
        
        
        
    let rec addSamplerArgsStmt (samplerNames : Map<string, string>) (s : CStatement) =
        match s with
        | CNop -> s
        | CDo e -> CDo (addSamplerArgsExpr samplerNames e)
        | CDeclare(v, r) -> CDeclare(v, r |> Option.map (addSamplerArgsRExpr samplerNames))
        | CWrite(l, r) -> CWrite(l, addSamplerArgsExpr samplerNames r)
        | CIncrement(p, v) -> CIncrement(p, addSamplerArgsLExpr samplerNames v)
        | CDecrement(p, v) -> CDecrement(p, addSamplerArgsLExpr samplerNames v)
        | CSequential stmts -> CSequential (stmts |> List.map (addSamplerArgsStmt samplerNames))
        | CIsolated stmts -> CIsolated (stmts |> List.map (addSamplerArgsStmt samplerNames))
        | CReturn -> s
        | CWriteOutput(name, index, value) -> CWriteOutput(name, index, addSamplerArgsRExpr samplerNames value)
        | CReturnValue v -> CReturnValue (addSamplerArgsExpr samplerNames v)
        | CBreak -> s
        | CContinue -> s
        | CFor(init, cond, step, body) -> CFor(addSamplerArgsStmt samplerNames init, addSamplerArgsExpr samplerNames cond, addSamplerArgsStmt samplerNames step, addSamplerArgsStmt samplerNames body)
        | CWhile(guard, body) -> CWhile(addSamplerArgsExpr samplerNames guard, addSamplerArgsStmt samplerNames body)
        | CDoWhile(guard, body) -> CDoWhile(addSamplerArgsExpr samplerNames guard, addSamplerArgsStmt samplerNames body)
        | CIfThenElse(cond, ifTrue, ifFalse) -> CIfThenElse(addSamplerArgsExpr samplerNames cond, addSamplerArgsStmt samplerNames ifTrue, addSamplerArgsStmt samplerNames ifFalse)
        | CSwitch(value, cases) -> CSwitch(addSamplerArgsExpr samplerNames value, cases |> Array.map (fun (l, s) -> l, addSamplerArgsStmt samplerNames s))
        
    let rec addSamplerArgsValueDef (samplerNames : Map<string, string>) (s : CValueDef) =
        match s with
        | CConstant(t, n, init) -> CConstant(t, n, addSamplerArgsRExpr samplerNames init)
        | CFunctionDef(signature, body) ->
            let newSignature, additionalSamplerNames = addSamplerArgsFunctionSignature signature
            let samplerNames = Map.union samplerNames additionalSamplerNames
            
            CFunctionDef(newSignature, addSamplerArgsStmt samplerNames body)
        | CEntryDef e -> CEntryDef { e with cBody = addSamplerArgsStmt samplerNames e.cBody }
        | CConditionalDef(d, inner) -> CConditionalDef(d, inner |> List.map (addSamplerArgsValueDef samplerNames))
        | CUniformDef us -> CUniformDef us
        | CRaytracingDataDef ds -> CRaytracingDataDef ds
        
    
    let rec addSamplerArgs (mm : CModule) : CModule =
        let mutable samplerNames = Map.empty
        
        
        let rec run (v : CValueDef) =
            match v with
            | CValueDef.CConditionalDef(cond, children) ->
                CValueDef.CConditionalDef(cond, List.map run children)
            | CValueDef.CUniformDef uniforms ->
                let newUniforms = 
                    uniforms |> List.collect (fun u ->
                        match u.cUniformType with
                        | CTexture(tex, _) ->
                            match tex with
                            | WGSLTexture t ->
                                let samplerType = if t.isShadow then "sampler_comparison" else "sampler"
                                let samName = getSamplerName u.cUniformName //$"wgsl_{u.cUniformName}Sampler"
                                samplerNames <- Map.add u.cUniformName samName samplerNames
                                [
                                    u
                                    { cUniformBuffer = None; cUniformName = samName; cUniformType = CIntrinsic { intrinsicTypeName = samplerType; tag = null }; cUniformDecorations = [] }
                                ]
                            | _ ->
                                [u]
                        | _ ->
                            [u]
                    )
                
                CValueDef.CUniformDef newUniforms
            | _ ->
                v
        
        let mm =
            { mm with
                values = mm.values |> List.map run
            }

     
        { mm with
            values = mm.values |> List.map (addSamplerArgsValueDef samplerNames)
        }
    
    
    let assembleEntryS (e : CEntryDef) =
        state {
            let stages =
                e.cDecorations 
                    |> List.tryPick (function EntryDecoration.Stages t -> Some t | _ -> None) 
                    |> Option.defaultValue (
                        ShaderStageDescription.Graphics {
                            prev = None
                            self = ShaderStage.Vertex
                            next = None
                        }
                    )

            let localSize =
                e.cDecorations |> List.tryPick (function EntryDecoration.LocalSize s -> Some s | _ -> None)

            let entryName = checkName (stages.Stage.ToString().ToLower())

            do! State.modify (fun s ->
                { s with
                    AssemblerState.stages = stages
                    AssemblerState.currentInputLocation = 0
                    AssemblerState.currentOutputLocation = 0
                }
            )
            do! Interface.newShader entryName.Name 
            do! Interface.addDecorations stages.Stage e.cDecorations

            
            let prefix = 
                match stages.Stage with
                    | ShaderStage.Compute ->
                        let localSize = e.cDecorations |> List.tryPick (function EntryDecoration.LocalSize s when s.AllGreater 0 -> Some s | _ -> None) 
                        [
                            yield "@compute"
                            match localSize with
                            | Some s -> yield sprintf "@workgroup_size(%d, %d, %d)" s.X s.Y s.Z
                            | None -> failwith "WGSL does not support variable workgroup_size"
                        ]
                    | ShaderStage.Vertex ->
                        [
                            "@vertex"
                        ]
                        
                    | ShaderStage.Fragment ->
                        [
                            "@fragment"
                        ]
                    | _ ->
                        []

            let! inputs = e.cInputs |> List.chooseS (assembleEntryParameterS ParameterKind.Input)
            let! outputs = e.cOutputs |> List.chooseS (assembleEntryParameterS ParameterKind.Output)
            let! args = e.cArguments |> List.chooseS (assembleEntryParameterS ParameterKind.Argument)
            let! body = assembleStatementS false e.cBody
            
            let! s = State.get
            let builtIns = 
                match s.ifaceNew.shaders with
                | WGSLProgramShaders.Compute c ->c.shaderIntrinsicInputs
                | _ -> HashSet.empty
                
            let functionArgs =
                builtIns |> Seq.choose (fun b ->
                    match b with
                    | WGSLIntrinsicInput.GlobalId -> Some $"@builtin(global_invocation_id) {WGSLIntrinsicInput.name b} : vec3u"
                    | WGSLIntrinsicInput.LocalId -> Some $"@builtin(local_invocation_id) {WGSLIntrinsicInput.name b} : vec3u"
                    | WGSLIntrinsicInput.WorkGroupId -> Some $"@builtin(workgroup_id) {WGSLIntrinsicInput.name b} : vec3u"
                    | WGSLIntrinsicInput.LocalIndex -> Some $"@builtin(local_invocation_index) {WGSLIntrinsicInput.name b} : u32"
                    | WGSLIntrinsicInput.NumWorkGroups -> Some $"@builtin(num_workgroups) {WGSLIntrinsicInput.name b} : vec3u"
                    | _ ->
                        None
                )
                |> Seq.toList
                
                
            //let allArgs = inputs @ args
            
            let rec withIsLast (l : list<'a>) =
                match l with
                | [] -> []
                | [x] -> [x, true]
                | x :: xs -> (x, false) :: withIsLast xs
            
            let inputName = $"{stages.Stage}Input"
            let outputName = $"{stages.Stage}Output"
            
            
            let inputStruct =
                match inputs with
                | [] -> []
                | _ ->
                    [
                        $"struct {inputName} {{"
                        for i, last in withIsLast inputs do
                            if last then $"    {i}"
                            else $"    {i},"
                        $"}}"
                    ]
                
            let outputStruct =
                match outputs with
                | [] -> []
                | _ ->
                    [
                        $"struct {outputName} {{"
                        for i, last in withIsLast outputs do
                            if last then $"    {i}"
                            else $"    {i},"
                        $"}}"
                    ]
            
            
            let functionArgs =
                match inputs with
                | [] -> functionArgs
                | _ -> $"input : {inputName}" :: functionArgs
            
            let functionArgString = String.concat ", " functionArgs
            
            return 
                String.concat "\r\n" [
                    yield! inputStruct
                    yield! outputStruct
                    yield! args
                    yield! prefix
                    match functionArgs, outputs with
                    | [], [] -> yield $"fn {entryName.Name}() {{"
                    | args, [] -> yield $"fn {entryName.Name}({functionArgString}) {{"
                    | [], _ ->
                        yield $"fn {entryName.Name}() -> {outputName} {{"
                        yield $"    var output : {outputName};"
                    | _, _ ->
                        yield $"fn {entryName.Name}({functionArgString}) -> {outputName} {{"
                        yield $"    var output : {outputName};"
                    
                    yield String.indent body
                    match outputs with
                    | [] -> ()
                    | _ -> yield "    return output;"
                    yield "}"
                    //yield sprintf "fn %s(%s) -> %s\r\n{\r\n%s\r\n}" entryName.Name allArgs (assembleType config.doubleAsFloat config.reverseMatrixLogic e.cReturnType).Name (String.indent body)
                ]
        }

    let rec assembleValueDefS (d : CValueDef) =
        state {
            let! config = AssemblerState.config
            match d with
            | CConditionalDef(d, inner) ->
                let! inner = inner |> List.mapS assembleValueDefS
                let inner = inner |> List.filter (String.IsNullOrEmpty >> not) |> String.concat "\r\n\r\n"
                return inner
                //return sprintf "\r\n#ifdef %s\r\n\r\n%s\r\n\r\n#endif\r\n" d inner

            | CEntryDef e -> 
                return! assembleEntryS e

            | CFunctionDef(signature, body) ->
                do! Interface.newFunction signature
                let signature = assembleFunctionSignature config.doubleAsFloat config.reverseMatrixLogic signature
                let! body = assembleStatementS false body
                do! Interface.endFunction
                return sprintf "%s\r\n{\r\n%s\r\n}\r\n" signature (String.indent body)

            | CConstant(t, n, init) ->
                let! init = assembleRExprS init
                let n = wgslName n
                match t with
                    | CArray(t,l) -> return sprintf "const %s %s[%d] = %s;" (assembleType config.doubleAsFloat config.reverseMatrixLogic t).Name n.Name l init
                    | CPointer(_,t) -> return sprintf "const %s %s[] = %s;" (assembleType config.doubleAsFloat config.reverseMatrixLogic t).Name n.Name init
                    | _ -> return sprintf "const %s %s = %s;" (assembleType config.doubleAsFloat config.reverseMatrixLogic t).Name n.Name init

            | CUniformDef us ->
                let! config = AssemblerState.config
                if config.createPerStageUniforms then
                    return! assembleUniformsS us
                else
                    return ""

            | CRaytracingDataDef r ->
                return ""
        }

    let assembleTypeDef (doubleAsFloat : bool) (rev : bool) (d : CTypeDef) =
        match d with
            | CStructDef(name, fields) ->
                let fields = fields |> List.map (fun (t, n) -> sprintf "%s : %s," (wgslName n).Name (assembleType doubleAsFloat rev t).Name) |> String.concat "\r\n"
                sprintf "struct %s\r\n{\r\n%s\r\n}" (wgslName name).Name (String.indent fields)
    
    module private Reflection =
        open System.Reflection
        open Aardvark.Base.IL

        let setShaderParent : WGSLShaderInterface -> WGSLProgramInterface -> unit =
            let tShader = typeof<WGSLShaderInterface>
            let fParent = tShader.GetField("program@", System.Reflection.BindingFlags.NonPublic ||| System.Reflection.BindingFlags.Instance)
            if isNull fParent then
                failwith "[FShade] internal error: cannot get parent field for GLSLShaderInterface"
            cil {
                do! IL.ldarg 0
                do! IL.ldarg 1
                do! IL.stfld fParent
                do! IL.ret
            }




    let assemble (backend : Backend) (m : CModule) =
        let m = addSamplerArgs m
        let c = backend.Config
        let definitions =
            state {
                
                let types = m.types |> List.map (assembleTypeDef c.doubleAsFloat c.reverseMatrixLogic)

                let! uniforms = 
                    if not c.createPerStageUniforms then assembleUniformsS m.uniforms |> State.map List.singleton
                    else State.value []

                let! values = m.values |> List.mapS assembleValueDefS

                let! s = State.get
                return 
                    List.concat [
                        types
                        uniforms
                        values
                    ]
            }
        
        

        let mutable state = AssemblerState.ofConfig c

        match m.cuserData with  
            | :? Effect as e ->
                state <- 
                    { state with 
                        textureInfos =
                            e.Uniforms |> Map.choose (fun name p ->
                                match p.uniformValue with
                                    | UniformValue.Sampler(name, s) -> Some [name, s]
                                    | UniformValue.SamplerArray arr -> Some (Array.toList arr)
                                    | _ -> None
                            )
                    }

            | :? RaytracingEffect as e ->
                state <-
                    { state with 
                        textureInfos =
                            e.Uniforms |> Map.choose (fun name p ->
                                match p.uniformValue with
                                    | UniformValue.Sampler(name, s) -> Some [name, s]
                                    | UniformValue.SamplerArray arr -> Some (Array.toList arr)
                                    | _ -> None
                            )
                    }

            | :? ComputeShader as c ->
                let res = 
                    c.csSamplerStates |> Map.toList |> List.map (fun ((samName,index), state) ->
                        let texName =
                            match Map.tryFind (samName,index) c.csTextureNames with
                                | Some name -> name
                                | None -> samName
                                
                        (samName, (index, texName, state))
                    )
                    |> List.groupBy fst
                    |> List.map (fun (samName, elements) ->
                        let elems = 
                            elements
                            |> List.sortBy (fun (_,(i,_,_)) -> i)
                            |> List.map (fun (_,(_,n,s)) -> n,s)
                        samName, elems
                    )
                    |> Map.ofList

                state <- { state with textureInfos = res; localSize = c.csLocalSize }
                
            | _ ->
                ()

        let code = definitions.Run(&state) |> String.concat "\r\n\r\n"
        let iface = LayoutStd140.apply state.ifaceNew

        // unsafely mutate the shader's parent
        iface.shaders |> WGSLProgramShaders.iter (fun shader ->
            Reflection.setShaderParent shader iface
        )

        { code = code; iface = iface }
