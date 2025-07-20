namespace FShade.WGSL

open Aardvark.Base
open FShade.Imperative
open FShade
open System
open FSharp.Data.Adaptive

type WGSLType =
    | Bool
    | Void
    | Int of signed : bool * width : int
    | Float of width : int
    | Vec of dim : int * elem : WGSLType
    | Mat of cols : int * rows : int * elem : WGSLType
    | Struct of name : string * fields : list<string * WGSLType * int> * size : int
    | Array of len : int * elem : WGSLType * stride : int
    | Image of WGSLStorageTextureType
    | Sampler of WGSLSampledTextureType
    | DynamicArray of elem : WGSLType * stride : int
    | Intrinsic of string

and WGSLStorageTextureType =
    {
        original    : System.Type
        format      : Option<ImageFormat>
        dimension   : SamplerDimension
        isArray     : bool
        isMS        : bool
        valueType   : WGSLType
    }

and WGSLSampledTextureType =
    {
        original    : System.Type
        dimension   : SamplerDimension
        isShadow    : bool
        isArray     : bool
        isMS        : bool
        valueType   : WGSLType
    }

and WGSLTextureType =
    | WGSLStorageTexture of WGSLStorageTextureType
    | WGSLTexture of WGSLSampledTextureType
    

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module WGSLType =
    open System.IO
    
    [<AutoOpen>]
    module private Helpers =
    
        let imageTypes =
            LookupTable.lookup [
                (false, false, false, SamplerDimension.Sampler1d), typedefof<Image1d<_>>
                (false, false, false, SamplerDimension.Sampler2d), typedefof<Image2d<_>>
                (false, false, false, SamplerDimension.Sampler3d), typedefof<Image3d<_>>
                (false, false, false, SamplerDimension.SamplerCube), typedefof<ImageCube<_>>

                (false, true, false, SamplerDimension.Sampler1d), typedefof<Image1dArray<_>>
                (false, true, false, SamplerDimension.Sampler2d), typedefof<Image2dArray<_>>
                (false, true, false, SamplerDimension.SamplerCube), typedefof<ImageCubeArray<_>>

                (false, false, true, SamplerDimension.Sampler2d), typedefof<Image2dMS<_>>
                (false, true, true, SamplerDimension.Sampler2d), typedefof<Image2dArrayMS<_>>

                (true, false, false, SamplerDimension.Sampler1d), typedefof<IntImage1d<_>>
                (true, false, false, SamplerDimension.Sampler2d), typedefof<IntImage2d<_>>
                (true, false, false, SamplerDimension.Sampler3d), typedefof<IntImage3d<_>>
                (true, false, false, SamplerDimension.SamplerCube), typedefof<IntImageCube<_>>

                (true, true, false, SamplerDimension.Sampler1d), typedefof<IntImage1dArray<_>>
                (true, true, false, SamplerDimension.Sampler2d), typedefof<IntImage2dArray<_>>
                (true, true, false, SamplerDimension.SamplerCube), typedefof<IntImageCubeArray<_>>

                (true, false, true, SamplerDimension.Sampler2d), typedefof<IntImage2dMS<_>>
                (true, true, true, SamplerDimension.Sampler2d), typedefof<IntImage2dArrayMS<_>>
            ]

        // integral, isArray, isMS, isShadow
        let samplerTypes =
            LookupTable.lookup [
                (false, false, false, false, SamplerDimension.Sampler1d), typeof<Sampler1d>
                (false, false, false, false, SamplerDimension.Sampler2d), typeof<Sampler2d>
                (false, false, false, false, SamplerDimension.Sampler3d), typeof<Sampler3d>
                (false, false, false, false, SamplerDimension.SamplerCube), typeof<SamplerCube>

                (false, false, false, true, SamplerDimension.Sampler1d), typeof<Sampler1dShadow>
                (false, false, false, true, SamplerDimension.Sampler2d), typeof<Sampler2dShadow>
                (false, false, false, true, SamplerDimension.SamplerCube), typeof<SamplerCubeShadow>

                (false, false, true, false, SamplerDimension.Sampler2d), typeof<Sampler2dMS>

                (false, true, false, false, SamplerDimension.Sampler1d), typeof<Sampler1dArray>
                (false, true, false, false, SamplerDimension.Sampler2d), typeof<Sampler2dArray>
                (false, true, false, false, SamplerDimension.SamplerCube), typeof<SamplerCubeArray>

                (false, true, false, true, SamplerDimension.Sampler1d), typeof<Sampler1dArrayShadow>
                (false, true, false, true, SamplerDimension.Sampler2d), typeof<Sampler2dArrayShadow>
                (false, true, false, true, SamplerDimension.SamplerCube), typeof<SamplerCubeArrayShadow>

                (false, true, true, false, SamplerDimension.Sampler2d), typeof<Sampler2dArrayMS>

                (true, false, false, false, SamplerDimension.Sampler1d), typeof<IntSampler1d>
                (true, false, false, false, SamplerDimension.Sampler2d), typeof<IntSampler2d>
                (true, false, false, false, SamplerDimension.Sampler3d), typeof<IntSampler3d>
                (true, false, false, false, SamplerDimension.SamplerCube), typeof<IntSamplerCube>

                (true, false, true, false, SamplerDimension.Sampler2d), typeof<IntSampler2dMS>

                (true, true, false, false, SamplerDimension.Sampler1d), typeof<IntSampler1dArray>
                (true, true, false, false, SamplerDimension.Sampler2d), typeof<IntSampler2dArray>
                (true, true, false, false, SamplerDimension.SamplerCube), typeof<IntSamplerCubeArray>

                (true, true, true, false, SamplerDimension.Sampler2d), typeof<IntSampler2dArrayMS>
            ]
            

        let rec isIntegral (t : WGSLType) =
            match t with
            | WGSLType.Vec(_, v) -> isIntegral v
            | WGSLType.Mat(_,_,v) -> isIntegral v
            | Float _ -> false
            | Int _ -> true
            | _ -> false

    let rec ofCType (doubleAsFloat : bool) (rev : bool) (t : CType) =
        match t with    
            | CType.CBool -> WGSLType.Bool
            | CType.CVoid -> WGSLType.Void
            | CType.CInt(signed, width) -> WGSLType.Int(signed, width)

            | CType.CFloat 64 ->
                if doubleAsFloat then WGSLType.Float 32
                else WGSLType.Float 64
            | CType.CFloat(width) -> WGSLType.Float(width)

            | CType.CVector(elem, dim) -> WGSLType.Vec(dim, ofCType doubleAsFloat rev elem)
            | CType.CMatrix(elem, r, c) -> 
                if rev then WGSLType.Mat(r, c, ofCType doubleAsFloat rev elem)
                else WGSLType.Mat(c, r, ofCType doubleAsFloat rev elem)

            | CType.CArray(elem, len) -> WGSLType.Array(len, ofCType doubleAsFloat rev elem, -1)
            | CType.CStruct(name, fields,_) -> WGSLType.Struct(name, fields |> List.map (fun (t, n) -> n, ofCType doubleAsFloat rev t, -1), -1)

            | CType.CIntrinsic a ->
                match a.tag with
                    | :? WGSLTextureType as t -> 
                        match t with
                            | WGSLStorageTexture t -> WGSLType.Image t
                            | WGSLTexture t -> WGSLType.Sampler t
                    | _ ->
                        WGSLType.Intrinsic a.intrinsicTypeName

            | CType.CPointer(_,e) -> WGSLType.DynamicArray (ofCType doubleAsFloat rev e, -1)

    let rec internal serializeInternal (dst : BinaryWriter) (t : WGSLType) =
        match t with
        | WGSLType.Bool ->
            dst.Write 0uy
        | WGSLType.Void ->
            dst.Write 1uy
        | WGSLType.Int(signed, bits) ->
            dst.Write 2uy
            dst.Write signed
            dst.Write (byte bits)
        | WGSLType.Float bits ->
            dst.Write 3uy
            dst.Write (byte bits)
        | WGSLType.Vec(dim, elem) ->
            dst.Write 4uy
            dst.Write (byte dim)
            serializeInternal dst elem
        | WGSLType.Mat(c,r,elem) ->
            dst.Write 5uy
            dst.Write (byte c)
            dst.Write (byte r)
            serializeInternal dst elem
        | WGSLType.Struct(name, fields, size) ->
            dst.Write 6uy
            dst.Write name
            dst.Write (List.length fields)
            for (fn, ft, fs) in fields do
                dst.Write(fn)
                serializeInternal dst ft
                dst.Write fs
            dst.Write size
        | WGSLType.Array(len, elem, align) ->
            dst.Write 7uy
            dst.Write len
            serializeInternal dst elem
            dst.Write align
        | WGSLType.Image img ->
            dst.Write 8uy
            dst.Write (int img.dimension)
            match img.format with
            | Some fmt ->
                dst.Write 1uy
                dst.Write (int fmt)
            | None ->
                dst.Write 0uy

            serializeInternal dst img.valueType
            dst.Write img.isArray
            dst.Write img.isMS
        | WGSLType.Sampler sam ->
            dst.Write 9uy
            dst.Write (int sam.dimension)
            serializeInternal dst sam.valueType
            dst.Write sam.isArray
            dst.Write sam.isMS
            dst.Write sam.isShadow
        | WGSLType.DynamicArray(elem, stride) ->
            dst.Write 10uy
            serializeInternal dst elem
            dst.Write stride
        | WGSLType.Intrinsic name ->
            dst.Write 11uy
            dst.Write name

    let rec internal deserializeInternal (src : BinaryReader) =
        match src.ReadByte() with
        | 0uy -> 
            WGSLType.Bool

        | 1uy ->
            WGSLType.Void

        | 2uy ->
            let signed = src.ReadBoolean()
            let bits = src.ReadByte() |> int
            WGSLType.Int(signed, bits)

        | 3uy ->
            let bits = src.ReadByte() |> int
            WGSLType.Float bits

        | 4uy ->
            let dim = src.ReadByte() |> int
            let elem = deserializeInternal src
            WGSLType.Vec(dim, elem)

        | 5uy ->
            let c = src.ReadByte() |> int
            let r = src.ReadByte() |> int
            let elem = deserializeInternal src
            WGSLType.Mat(c, r, elem)

        | 6uy ->
            let name = src.ReadString()
            let cnt = src.ReadInt32()
            let fields =
                List.init cnt (fun _ ->
                    let fn = src.ReadString()
                    let ft = deserializeInternal src
                    let fs = src.ReadInt32()
                    (fn, ft, fs)
                )
            let size = src.ReadInt32()
            WGSLType.Struct(name, fields, size)

        | 7uy ->
            let len = src.ReadInt32()
            let elem = deserializeInternal src
            let align = src.ReadInt32()
            WGSLType.Array(len, elem, align)

        | 8uy ->
            let dim = src.ReadInt32() |> unbox<SamplerDimension>
            let format =
                match src.ReadByte() with
                | 0uy -> None
                | _ -> Some (src.ReadInt32() |> unbox<ImageFormat>)

            let valueType = deserializeInternal src
            let isArray = src.ReadBoolean()
            let isMS = src.ReadBoolean()

            let original = 
                let gen = imageTypes (isIntegral valueType, isArray, isMS, dim)
                match format with
                | Some fmt -> gen.MakeGenericType [| ImageFormat.toFormatType fmt |]
                | None -> gen

            WGSLType.Image {
                WGSLStorageTextureType.dimension = dim
                WGSLStorageTextureType.isArray = isArray
                WGSLStorageTextureType.isMS = isMS
                WGSLStorageTextureType.format = format
                WGSLStorageTextureType.valueType = valueType
                WGSLStorageTextureType.original = original
            }
        | 9uy ->
            let dim = src.ReadInt32() |> unbox<SamplerDimension>
            let valueType = deserializeInternal src
            let isArray = src.ReadBoolean()
            let isMS = src.ReadBoolean()
            let isShadow = src.ReadBoolean()

            let original = samplerTypes (isIntegral valueType, isArray, isMS, isShadow, dim)
            
            WGSLType.Sampler {
                WGSLSampledTextureType.dimension = dim
                WGSLSampledTextureType.isArray = isArray
                WGSLSampledTextureType.isMS = isMS
                WGSLSampledTextureType.isShadow = isShadow
                WGSLSampledTextureType.valueType = valueType
                WGSLSampledTextureType.original = original
            }
        | 10uy ->
            let elem = deserializeInternal src
            let stride = src.ReadInt32()
            WGSLType.DynamicArray(elem, stride)
        | 11uy ->
            let name = src.ReadString()
            WGSLType.Intrinsic name
        | id ->
            failwithf "unexpected WGSLType: %A" id

type WGSLParameter =
    {
        paramType           : WGSLType
        paramName           : string
        paramLocation       : int
        paramSemantic       : string
        paramInterpolation  : Option<InterpolationMode>
    }

type WGSLIntrinsic =
    {
        name    : string
        tag    : obj
        args    : WGSLType[]
        ret     : WGSLType
    }

type WGSLTexture =
    {
        textureGroup    : int
        textureBinding  : int
        textureName     : string
        textureCount    : int
        textureNames    : list<string>
        textureType     : WGSLSampledTextureType
    }

type WGSLSampler =
    {
        samplerName     : string
        samplerGroup    : int
        samplerBinding  : int
        samplerState    : SamplerState
    }
    
type WGSLImage =
    {
        imageGroup      : int
        imageBinding    : int
        imageName       : string
        imageType       : WGSLStorageTextureType
    }

type WGSLStorageBuffer =
    {
        ssbGroup        : int
        ssbBinding      : int
        ssbName         : string
        ssbType         : WGSLType
        ssbReadOnly     : bool
    }

type WGSLUniformBufferField =
    {
        ufName      : string
        ufType      : WGSLType
        ufOffset    : int
    }

type WGSLUniformBuffer =
    {
        ubGroup         : int
        ubBinding       : int
        ubName          : string
        ubFields        : list<WGSLUniformBufferField>
        ubSize          : int
    }

type WGSLWinding =
    | CCW
    | CW

type WGSLSpacing =
    | Equal
    | FractionalEven
    | FractionalOdd

type WGSLShaderDecoration =
    | WGSLInvocations of int
    | WGSLInputTopology of InputTopology
    | WGSLOutputTopology of OutputTopology
    | WGSLMaxVertices of int
    | WGSLSpacing of WGSLSpacing
    | WGSLWinding of WGSLWinding
    | WGSLLocalSize of V3i

[<AutoOpen>]
module private Tools =
    
    let private formatName =
        LookupTable.lookup [
            // 'bgra8unorm', 'r32float', 'r32sint', 'r32uint', 'r8unorm', 'rg32float', 'rg32sint', 'rg32uint', 'rgba16float', 'rgba16sint',
            // 'rgba16uint', 'rgba32float', 'rgba32sint', 'rgba32uint', 'rgba8sint', 'rgba8snorm', 'rgba8uint', 'rgba8unorm'
            //
            // ImageFormat.Bgra8, "bgra8unorm"
            // ImageFormat.Rgba8, "rgba8unorm"
            ImageFormat.R32f, "r32float"
            ImageFormat.R32i, "r32sint"
            ImageFormat.R32ui, "r32uint"
            ImageFormat.R8, "r8unorm"
            ImageFormat.Rg32f, "rg32float"
            ImageFormat.Rg32i, "rg32sint"
            ImageFormat.Rg32ui, "rg32uint"
            ImageFormat.Rgba16f, "rgba16float"
            ImageFormat.Rgba16i, "rgba16sint"
            ImageFormat.Rgba16ui, "rgba16uint"
            ImageFormat.Rgba32f, "rgba32float"
            ImageFormat.Rgba32i, "rgba32sint"
            ImageFormat.Rgba32ui, "rgba32uint"
            ImageFormat.Rgba8i, "rgba8sint"
            ImageFormat.Rgba8Snorm, "rgba8snorm"
            ImageFormat.Rgba8ui, "rgba8uint"
            ImageFormat.Rgba8, "rgba8unorm"
            
            
            
            // TODO
        ]
    
    let lines (str : string) =
        str.Split([| "\r\n" |], StringSplitOptions.None) :> seq<_>
             

    let many (entries : list<Option<string>>) =
        entries |> Seq.choose id |> String.concat "\r\n"

    let section (name : string) (entries : list<Option<string>>) =
        let entries = entries |> List.choose id
        match entries with
            | [] -> None
            | _ -> name + ":\r\n" + (entries |> Seq.collect lines |> Seq.map (fun v -> "    " + v) |> String.concat "\r\n") |> Some

    module WGSLType =

        let samplerName (t : WGSLSampledTextureType) =
            let dimStr =
                match t.dimension with
                    | SamplerDimension.Sampler1d -> "1d"
                    | SamplerDimension.Sampler2d -> "2d"
                    | SamplerDimension.Sampler3d -> "3d"
                    | SamplerDimension.SamplerCube -> "cube"
                    | _ -> failwith "unsupported sampler dimension"

            let shadowSuffix = if t.isShadow then "depth_" else ""
            let msSuffix = if t.isMS then "multisampled_" else ""
            let typeArgSuffix =
                if t.isShadow then
                    ""
                else
                    match t.valueType with
                    | Vec(_,Int _) -> "<i32>"
                    | _ -> "<f32>"
                 
            if t.isArray then $"texture_{shadowSuffix}{msSuffix}{dimStr}_array{typeArgSuffix}"
            else $"texture_{shadowSuffix}{msSuffix}{dimStr}{typeArgSuffix}"
            
        let imageName (t : WGSLStorageTextureType) =
            let dimStr =
                match t.dimension with
                    | SamplerDimension.Sampler1d -> "1d"
                    | SamplerDimension.Sampler2d -> "2d"
                    | SamplerDimension.Sampler3d -> "3d"
                    | SamplerDimension.SamplerCube -> "cube"
                    | _ -> failwith "unsupported sampler dimension"

            let msSuffix = if t.isMS then "multisampled_" else ""
            let typeArgSuffix =
                match t.format with
                | Some fmt ->
                    sprintf "<%s, read_write>" (formatName fmt)
                | None ->
                    match t.valueType with
                    | Vec(_,Int _) -> "<i32, read_write>"
                    | _ -> "<f32, read_write>"
                 
            if t.isArray then $"texture_{msSuffix}{dimStr}_array{typeArgSuffix}"
            else $"texture_storage_{msSuffix}{dimStr}{typeArgSuffix}"
            

        let rec toString (t : WGSLType) =
            match t with
                | WGSLType.Intrinsic n -> n
                | WGSLType.Bool -> "bool"
                | WGSLType.Void -> "void" 

                | WGSLType.Int(true, 8) -> "i8"
                | WGSLType.Int(true, 16) -> "i16"
                | WGSLType.Int(true, 32) -> "i32"
                | WGSLType.Int(true, 64) -> "i64"
                | WGSLType.Int(true, b) -> sprintf "i%d" b
                
                | WGSLType.Int(false, 8) -> "u8"
                | WGSLType.Int(false, 16) -> "u16"
                | WGSLType.Int(false, 32) -> "u32"
                | WGSLType.Int(false, 64) -> "u53"
                | WGSLType.Int(false, b) -> sprintf "u%d" b
                
                | WGSLType.Float 16 -> "f16"
                | WGSLType.Float (32 | 64) -> "f32"
                | WGSLType.Float b -> sprintf "f%d" b

                | WGSLType.Vec(dim, elem) -> sprintf "vec%d<%s>" dim (toString elem)
                | WGSLType.Mat(c, r, elem) -> sprintf "mat%dx%d<%s>" c r (toString elem)

                | WGSLType.Struct(name,_,_) -> name
                | WGSLType.Array(len, elem,_) -> sprintf "array<%s, %d>" (toString elem) len
                | WGSLType.Image img -> imageName img
                | WGSLType.Sampler sam -> samplerName sam
                | WGSLType.DynamicArray(elem,_) -> sprintf "array<%s>" (toString elem)

[<CustomEquality; NoComparison>]
type WGSLShaderInterface =
    {
        program                      : WGSLProgramInterface
        shaderStage                  : ShaderStage
        shaderEntry                  : string
        shaderInputs                 : list<WGSLParameter>
        shaderOutputs                : list<WGSLParameter>
        shaderSamplers               : HashSet<string>
        shaderImages                 : HashSet<string>
        shaderStorageBuffers         : HashSet<string>
        shaderUniformBuffers         : HashSet<string>
        shaderAccelerationStructures : HashSet<string>
        shaderBuiltInFunctions       : HashSet<WGSLIntrinsic>
        shaderIntrinsicInputs        : HashSet<FShade.WGSL.Utilities.WGSLIntrinsicInput>
        shaderDecorations            : list<WGSLShaderDecoration>
        shaderBuiltIns               : MapExt<ParameterKind, MapExt<string, WGSLType>>
    }

    override x.GetHashCode() =
        HashCode.Combine(
            x.shaderStage.GetHashCode(),
            x.shaderEntry.GetHashCode(),
            x.shaderInputs.GetHashCode(),
            x.shaderOutputs.GetHashCode(),
            x.shaderSamplers.GetHashCode(),
            x.shaderImages.GetHashCode(),
            x.shaderStorageBuffers.GetHashCode(),
            x.shaderUniformBuffers.GetHashCode(),
            x.shaderAccelerationStructures.GetHashCode(),
            x.shaderBuiltInFunctions.GetHashCode(),
            x.shaderDecorations.GetHashCode(),
            x.shaderBuiltIns.GetHashCode(),
            x.shaderIntrinsicInputs.GetHashCode()
        )
    override x.Equals(o) =
        match o with
            | :? WGSLShaderInterface as o ->
                x.shaderStage = o.shaderStage &&
                x.shaderEntry = o.shaderEntry &&
                x.shaderInputs = o.shaderInputs &&
                x.shaderOutputs = o.shaderOutputs &&
                x.shaderSamplers = o.shaderSamplers &&
                x.shaderImages = o.shaderImages &&
                x.shaderStorageBuffers = o.shaderStorageBuffers &&
                x.shaderUniformBuffers = o.shaderUniformBuffers &&
                x.shaderAccelerationStructures = o.shaderAccelerationStructures &&
                x.shaderBuiltInFunctions = o.shaderBuiltInFunctions &&
                x.shaderDecorations = o.shaderDecorations &&
                x.shaderBuiltIns = o.shaderBuiltIns &&
                x.shaderIntrinsicInputs = o.shaderIntrinsicInputs
            | _ ->
                false

    member x.shaderBuiltInInputs = MapExt.tryFind ParameterKind.Input x.shaderBuiltIns |> Option.defaultValue MapExt.empty
    member x.shaderBuiltInOutputs = MapExt.tryFind ParameterKind.Output x.shaderBuiltIns |> Option.defaultValue MapExt.empty

    override x.ToString() =
        many [
            yield section (string x.shaderStage) [
                        
                yield sprintf "entry: \"%s\"" x.shaderEntry |> Some

                match x.shaderDecorations with
                    | [] -> ()
                    | dec ->
                        yield "attributes: {" |> Some
                        for d in dec do
                            match d with
                                | WGSLMaxVertices d -> yield sprintf "    max-vertices: %d" d |> Some
                                | WGSLInputTopology d -> yield sprintf "    input-top: %A" d |> Some
                                | WGSLOutputTopology d -> yield sprintf "    output-top: %A" d |> Some
                                | WGSLInvocations d -> yield sprintf "    invocations: %A" d |> Some
                                | WGSLLocalSize d -> yield sprintf "    local-size: %dx%dx%d" d.X d.Y d.Z |> Some
                                | WGSLSpacing d -> yield sprintf "    spacing: %A" d |> Some
                                | WGSLWinding d -> yield sprintf "    winding: %A" d |> Some
                        yield "}" |> Some


                let usedUniforms =
                    Seq.concat [
                        x.shaderAccelerationStructures |> Seq.map (sprintf "acc::%s")
                        x.shaderUniformBuffers |> Seq.map (sprintf "ub::%s")
                        x.shaderStorageBuffers |> Seq.map (sprintf "ssb::%s")
                        x.shaderSamplers |> Seq.map (sprintf "sam::%s")
                        x.shaderImages |> Seq.map (sprintf "img::%s")
                    ]

                yield sprintf "uniform {%s}" (String.concat ", " usedUniforms) |> Some

                let called = 
                    x.shaderBuiltInFunctions |> HashSet.toList |> List.map (fun f ->
                        let args = 
                            match f.args.Length with
                                | 0 -> [| WGSLType.Void |]
                                | _ -> f.args

                        sprintf "%s : %s -> %s" f.name (args |> Seq.map WGSLType.toString |> String.concat " -> ") (WGSLType.toString f.ret)
                    )

                match called with
                    | [] -> yield "called {}" |> Some
                    | called ->
                        yield "called {" |> Some
                        for c in called do
                            yield "    " + c |> Some
                        yield "}" |> Some
                        
                for i in x.shaderInputs do
                    yield sprintf "in %s : %s // location: %d semantic: %s" i.paramName (WGSLType.toString i.paramType) i.paramLocation i.paramSemantic |> Some
                
                for (name, typ) in MapExt.toSeq x.shaderBuiltInInputs do
                    yield sprintf "in %s : %s " name (WGSLType.toString typ) |> Some
                     
                     
                for i in x.shaderOutputs do
                    yield sprintf "out %s : %s // location: %d semantic: %s" i.paramName (WGSLType.toString i.paramType) i.paramLocation i.paramSemantic |> Some
                
                for (name, typ) in MapExt.toSeq x.shaderBuiltInOutputs do
                    yield sprintf "out %s : %s " name (WGSLType.toString typ) |> Some
            ]
        ]
                       

and [<RequireQualifiedAccess>] WGSLProgramShaders =
    | Compute    of WGSLShaderInterface
    | Graphics   of WGSLGraphicsShaders

    member x.Slots =
        match x with
        | Compute c -> MapExt.ofList [ ShaderSlot.Compute, c]
        | Graphics g -> g.Slots

    member x.Item(slot : ShaderSlot) =
        x.Slots.[slot]

    override x.ToString() =
        match x with
        | Compute c -> c.ToString()
        | Graphics g -> g.ToString()

and WGSLGraphicsShaders =
    {
        stages : MapExt<ShaderStage, WGSLShaderInterface>
    }

    member x.firstShader = x.stages.[MapExt.min x.stages]
    member x.lastShader = x.stages.[MapExt.max x.stages]

    member x.Slots =
        x.stages |> MapExt.mapMonotonic (fun stage shader ->
            let slot =
                match stage with
                | ShaderStage.Vertex ->      ShaderSlot.Vertex
                | ShaderStage.Fragment ->    ShaderSlot.Fragment
                | ShaderStage.Geometry ->    ShaderSlot.Geometry
                | ShaderStage.TessControl -> ShaderSlot.TessControl
                | ShaderStage.TessEval ->    ShaderSlot.TessEval
                | _ -> failwithf "unsupported shader stage: %A" stage

            slot, shader
        )

    override x.ToString() =
        many [
            for (name, typ) in MapExt.toSeq x.firstShader.shaderBuiltInInputs do
                yield sprintf "in %s : %s " name (WGSLType.toString typ) |> Some

            for (name, typ) in MapExt.toSeq x.lastShader.shaderBuiltInOutputs do
                yield sprintf "out %s : %s " name (WGSLType.toString typ) |> Some

            for (_, shader) in MapExt.toSeq x.stages do
                yield Some (shader.ToString())
        ]

and [<StructuredFormatDisplay("{AsString}")>] WGSLProgramInterface =
    {
        inputs                  : list<WGSLParameter>
        outputs                 : list<WGSLParameter>
        textures                : MapExt<string, WGSLTexture>
        samplers                : MapExt<string, WGSLSampler>
        images                  : MapExt<string, WGSLImage>
        storageBuffers          : MapExt<string, WGSLStorageBuffer>
        uniformBuffers          : MapExt<string, WGSLUniformBuffer>
        shaders                 : WGSLProgramShaders
    }

    override x.ToString() =
        many [
            for i in x.inputs do
                yield sprintf "in %s : %s // location: %d semantic: %s" i.paramName (WGSLType.toString i.paramType) i.paramLocation i.paramSemantic |> Some

            for i in x.outputs do
                yield sprintf "out %s : %s // location: %d semantic: %s" i.paramName (WGSLType.toString i.paramType) i.paramLocation i.paramSemantic |> Some

            for (_,b) in MapExt.toSeq x.uniformBuffers do
                let name = sprintf "ub %s { // set: %d binding: %d" b.ubName b.ubGroup b.ubBinding
                yield Some name
                for f in b.ubFields do
                    yield sprintf "    %s : %s // offset: %d" f.ufName (WGSLType.toString f.ufType) f.ufOffset |> Some
                yield Some "}"

            for (_,s) in MapExt.toSeq x.storageBuffers do
                yield sprintf "ssb %s : %s[] // set: %d binding: %d" s.ssbName (WGSLType.toString s.ssbType) s.ssbGroup s.ssbBinding |> Some

            for (_,s) in MapExt.toSeq x.textures do
                let suffix =
                    if s.textureCount > 1 then  sprintf "[%d]" s.textureCount
                    else ""
                yield sprintf "sam %s : %s%s // set: %d binding: %d" s.textureName (WGSLType.toString (WGSLType.Sampler s.textureType)) suffix s.textureGroup s.textureBinding |> Some

            for (_,s) in MapExt.toSeq x.images do
                yield sprintf "img %s : %s // set: %d binding: %d" s.imageName (WGSLType.toString (WGSLType.Image s.imageType)) s.imageGroup s.imageBinding |> Some

            yield section "shaders" [
                yield Some (x.shaders.ToString())
            ]

        ]

    member private x.AsString = x.ToString()

module WGSLParameter =
    open System.IO

    let internal serializeInternal (dst : BinaryWriter) (i : WGSLParameter) =
        dst.Write i.paramLocation
        dst.Write i.paramName
        dst.Write i.paramSemantic
        WGSLType.serializeInternal dst i.paramType
        match i.paramInterpolation with
        | Some i ->
            dst.Write 1uy
            dst.Write (int i)
        | None ->
            dst.Write 0uy

    let internal deserializeInternal (src : BinaryReader) =
        let paramLocation = src.ReadInt32()
        let paramName = src.ReadString()
        let paramSemantic = src.ReadString()
        let paramType = WGSLType.deserializeInternal src
        let interpolation =
            match src.ReadByte() with
            | 0uy -> None
            | _ -> Some (src.ReadInt32() |> unbox<InterpolationMode>)
        {
            paramLocation = paramLocation
            paramName = paramName
            paramSemantic = paramSemantic
            paramType = paramType
            paramInterpolation = interpolation
        }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module WGSLShaderInterface =

    let private discardFun = 
        {
            tag = null
            name = "discard"
            args = [||]
            ret = WGSLType.Void
        }


    let inline program (i : WGSLShaderInterface) = i.program
    let inline stage (i : WGSLShaderInterface) = i.shaderStage
    let inline entry (i : WGSLShaderInterface) = i.shaderEntry
    let inline inputs (i : WGSLShaderInterface) = i.shaderInputs
    let inline outputs (i : WGSLShaderInterface) = i.shaderOutputs
    let inline samplers (i : WGSLShaderInterface) = i.shaderSamplers
    let inline images (i : WGSLShaderInterface) = i.shaderImages
    let inline storageBuffers (i : WGSLShaderInterface) = i.shaderStorageBuffers
    let inline uniformBuffers (i : WGSLShaderInterface) = i.shaderUniformBuffers
    let inline accelerationStructures (i : WGSLShaderInterface) = i.shaderAccelerationStructures
    let inline builtInFunctions (i : WGSLShaderInterface) = i.shaderBuiltInFunctions
    let inline decorations (i : WGSLShaderInterface) = i.shaderDecorations
    let inline builtIns (i : WGSLShaderInterface) = i.shaderBuiltIns
    
    let writesPointSize (iface : WGSLShaderInterface) =
        if iface.shaderStage <> ShaderStage.Fragment then
            MapExt.containsKey "gl_PointSize" iface.shaderBuiltInOutputs
        else
            false


    let usesDiscard (iface : WGSLShaderInterface) =
        if iface.shaderStage = ShaderStage.Fragment then
            HashSet.contains discardFun iface.shaderBuiltInFunctions
        else
            false

    open System.IO

    let internal serializeInternal (dst : BinaryWriter) (s : WGSLShaderInterface) =

        dst.Write (int s.shaderStage)
        dst.Write s.shaderEntry

        dst.Write (List.length s.shaderInputs)
        for i in s.shaderInputs do
            WGSLParameter.serializeInternal dst i

        dst.Write (List.length s.shaderOutputs)
        for i in s.shaderOutputs do
            WGSLParameter.serializeInternal dst i

        dst.Write s.shaderSamplers.Count
        for v in s.shaderSamplers do dst.Write v
        
        dst.Write s.shaderImages.Count
        for v in s.shaderImages do dst.Write v
        
        dst.Write s.shaderStorageBuffers.Count
        for v in s.shaderStorageBuffers do dst.Write v
        
        dst.Write s.shaderUniformBuffers.Count
        for v in s.shaderUniformBuffers do dst.Write v

        dst.Write s.shaderAccelerationStructures.Count
        for v in s.shaderAccelerationStructures do dst.Write v
        
        dst.Write s.shaderBuiltInFunctions.Count
        for v in s.shaderBuiltInFunctions do 
            dst.Write v.name
            WGSLType.serializeInternal dst v.ret
            dst.Write v.args.Length
            for a in v.args do WGSLType.serializeInternal dst a

        dst.Write (List.length s.shaderDecorations)
        for v in s.shaderDecorations do
            match v with
            | WGSLInvocations v ->
                dst.Write 0uy; dst.Write v
            | WGSLInputTopology top ->
                dst.Write 1uy
                match top with
                | InputTopology.Point -> dst.Write 0uy
                | InputTopology.Line -> dst.Write 1uy
                | InputTopology.LineAdjacency -> dst.Write 2uy 
                | InputTopology.Triangle -> dst.Write 3uy
                | InputTopology.TriangleAdjacency -> dst.Write 4uy
                | InputTopology.Patch c -> dst.Write 5uy; dst.Write c
            | WGSLOutputTopology top ->
                dst.Write 2uy
                match top with
                | OutputTopology.Points -> dst.Write 0uy
                | OutputTopology.LineStrip -> dst.Write 1uy
                | OutputTopology.TriangleStrip -> dst.Write 2uy
            | WGSLMaxVertices v ->
                dst.Write 3uy
                dst.Write v
            | WGSLSpacing s ->
                dst.Write 4uy
                match s with
                | WGSLSpacing.Equal -> dst.Write 0uy
                | WGSLSpacing.FractionalEven -> dst.Write 1uy
                | WGSLSpacing.FractionalOdd -> dst.Write 2uy
            | WGSLWinding w ->
                dst.Write 5uy
                match w with
                | WGSLWinding.CW -> dst.Write 0uy
                | WGSLWinding.CCW -> dst.Write 1uy
            | WGSLLocalSize s ->
                dst.Write 6uy
                dst.Write s.X
                dst.Write s.Y
                dst.Write s.Z

        dst.Write (s.shaderBuiltIns.Count)
        for KeyValue(kind, m) in s.shaderBuiltIns do
            dst.Write (int kind)
            dst.Write m.Count
            for KeyValue(name, t) in m do
                dst.Write name
                WGSLType.serializeInternal dst t

        dst.Write (s.shaderIntrinsicInputs.Count)
        for i in s.shaderIntrinsicInputs do
            dst.Write (int i)
    
    let internal deserializeInternal (src : BinaryReader) =
        let stage = src.ReadInt32() |> unbox<ShaderStage>
        let entry = src.ReadString() 

        let input =
            let cnt = src.ReadInt32()
            List.init cnt (fun _ -> WGSLParameter.deserializeInternal src)
     
        let output =
            let cnt = src.ReadInt32()
            List.init cnt (fun _ -> WGSLParameter.deserializeInternal src)

        let shaderSamplers =
            let cnt = src.ReadInt32()
            List.init cnt (fun _ ->  src.ReadString()) |> HashSet.ofList
            
        let shaderImages =
            let cnt = src.ReadInt32()
            List.init cnt (fun _ ->  src.ReadString()) |> HashSet.ofList
            
        let shaderStorageBuffers =
            let cnt = src.ReadInt32()
            List.init cnt (fun _ ->  src.ReadString()) |> HashSet.ofList
            
        let shaderUniformBuffers =
            let cnt = src.ReadInt32()
            List.init cnt (fun _ ->  src.ReadString()) |> HashSet.ofList
            
        let shaderAccelerationStructures =
            let cnt = src.ReadInt32()
            List.init cnt (fun _ ->  src.ReadString()) |> HashSet.ofList
            
        let shaderBuiltInFunctions =
            let cnt = src.ReadInt32()
            List.init cnt (fun _ ->  
                let name = src.ReadString()
                let ret = WGSLType.deserializeInternal src
                let args = Array.init (src.ReadInt32()) (fun _ -> WGSLType.deserializeInternal src)
                { name = name; tag = null; ret = ret; args = args }
            )
            |> HashSet.ofList

        let shaderDecorations =
            let cnt = src.ReadInt32()
            List.init cnt (fun _ ->
                match src.ReadByte() with
                | 0uy -> 
                    src.ReadInt32() |> WGSLInvocations
                | 1uy -> 
                    match src.ReadByte() with
                    | 0uy -> WGSLInputTopology InputTopology.Point
                    | 1uy -> WGSLInputTopology InputTopology.Line
                    | 2uy -> WGSLInputTopology InputTopology.LineAdjacency
                    | 3uy -> WGSLInputTopology InputTopology.Triangle
                    | 4uy -> WGSLInputTopology InputTopology.TriangleAdjacency
                    | _ -> WGSLInputTopology (InputTopology.Patch(src.ReadInt32()))
                | 2uy ->
                    match src.ReadByte() with
                    | 0uy -> WGSLOutputTopology OutputTopology.Points
                    | 1uy -> WGSLOutputTopology OutputTopology.LineStrip
                    | _ -> WGSLOutputTopology OutputTopology.TriangleStrip
                | 3uy ->
                    WGSLMaxVertices (src.ReadInt32())
                | 4uy ->
                    match src.ReadByte() with
                    | 0uy -> WGSLSpacing WGSLSpacing.Equal
                    | 1uy -> WGSLSpacing WGSLSpacing.FractionalEven
                    | _ -> WGSLSpacing WGSLSpacing.FractionalOdd
                | 5uy ->
                    match src.ReadByte() with
                    | 0uy -> WGSLWinding WGSLWinding.CW
                    | _ -> WGSLWinding WGSLWinding.CCW
                | _ ->
                    WGSLLocalSize(V3i(src.ReadInt32(), src.ReadInt32(), src.ReadInt32()))
            )
            
        let shaderBuiltIns =
            let cnt = src.ReadInt32()
            List.init cnt (fun _ ->
                let kind = src.ReadInt32() |> unbox<ParameterKind>
                let cnt = src.ReadInt32()
                kind, List.init cnt (fun _ ->
                    let name = src.ReadString()
                    let typ = WGSLType.deserializeInternal src
                    name, typ
                )
                |> MapExt.ofList
            )
            |> MapExt.ofList
        let shaderIntrinsicInputs =
            let cnt = src.ReadInt32()
            List.init cnt (fun _ ->
                src.ReadInt32() |> unbox<FShade.WGSL.Utilities.WGSLIntrinsicInput>
            )
            |> HashSet.ofList
        {
            program                      = Unchecked.defaultof<_>
            shaderStage                  = stage
            shaderEntry                  = entry
            shaderInputs                 = input
            shaderOutputs                = output
            shaderSamplers               = shaderSamplers
            shaderImages                 = shaderImages
            shaderStorageBuffers         = shaderStorageBuffers
            shaderUniformBuffers         = shaderUniformBuffers
            shaderAccelerationStructures = shaderAccelerationStructures
            shaderBuiltInFunctions       = shaderBuiltInFunctions
            shaderDecorations            = shaderDecorations
            shaderBuiltIns               = shaderBuiltIns
            shaderIntrinsicInputs        = shaderIntrinsicInputs
        }

     

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module WGSLGraphicsShaders =

    let empty =
        { stages = MapExt.empty }

    let usesDiscard (shaders : WGSLGraphicsShaders) =
        match MapExt.tryFind ShaderStage.Fragment shaders.stages with
        | Some shader -> WGSLShaderInterface.usesDiscard shader
        | None -> false

    let usesPointSize (shaders : WGSLGraphicsShaders) =
        match MapExt.neighboursAt (shaders.stages.Count-1) shaders.stages with
        | Some (_,prev), Some(_, frag), _ when frag.shaderStage = ShaderStage.Fragment ->
            WGSLShaderInterface.writesPointSize prev
        | _ ->
            false

    let alter (mapping : WGSLShaderInterface option -> WGSLShaderInterface option)
              (stage : ShaderStage) (shaders : WGSLGraphicsShaders) =
        { shaders with stages = shaders.stages |> MapExt.alter stage mapping }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module WGSLProgramShaders =

    let getCompute = function
        | WGSLProgramShaders.Compute c -> Some c
        | _ -> None

    let getGraphics = function
        | WGSLProgramShaders.Graphics g -> Some g
        | _ -> None

    let iter (action : WGSLShaderInterface -> unit) (shaders : WGSLProgramShaders) =
        match shaders with
        | WGSLProgramShaders.Compute c ->
            action c

        | WGSLProgramShaders.Graphics g ->
            g.stages |> MapExt.iter (fun _ s -> action s)

    let alter (mapping : WGSLShaderInterface option -> WGSLShaderInterface option)
              (stage : ShaderStageDescription) (shaders : WGSLProgramShaders) =

        match stage with
        | ShaderStageDescription.Compute ->
            getCompute shaders
            |> mapping
            |> Option.get
            |> WGSLProgramShaders.Compute

        | ShaderStageDescription.Graphics s ->
            getGraphics shaders
            |> Option.defaultValue WGSLGraphicsShaders.empty
            |> WGSLGraphicsShaders.alter mapping s.self
            |> WGSLProgramShaders.Graphics
        | _ ->
            shaders
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module WGSLProgramInterface =

    let inline inputs (i : WGSLProgramInterface) = i.inputs
    let inline outputs (i : WGSLProgramInterface) = i.outputs
    let inline samplers (i : WGSLProgramInterface) = i.textures
    let inline images (i : WGSLProgramInterface) = i.images
    let inline storageBuffers (i : WGSLProgramInterface) = i.storageBuffers
    let inline uniformBuffers (i : WGSLProgramInterface) = i.uniformBuffers
    let inline shaders (i : WGSLProgramInterface) = i.shaders

    let usesDiscard (iface : WGSLProgramInterface) =
        match iface.shaders with
        | WGSLProgramShaders.Graphics g -> WGSLGraphicsShaders.usesDiscard g
        | _ -> false

    let usesPointSize (iface : WGSLProgramInterface) =
        match iface.shaders with
        | WGSLProgramShaders.Graphics g -> WGSLGraphicsShaders.usesPointSize g
        | _ -> false

    let toString(iface : WGSLProgramInterface) =
        iface.ToString()

    let log (iface : WGSLProgramInterface) =
        let str = iface.ToString()
        for line in lines str do
            Log.line "%s" line

    let print (iface : WGSLProgramInterface) =
        Console.WriteLine("{0}", iface)


    open System.IO


    module private SamplerState =

        let serialize (dst : BinaryWriter) (s : SamplerState) =
            match s.AddressU with
            | Some v -> dst.Write 1uy; dst.Write (int v)
            | None -> dst.Write 0uy
            match s.AddressV with
            | Some v -> dst.Write 1uy; dst.Write (int v)
            | None -> dst.Write 0uy
            match s.AddressW with
            | Some v -> dst.Write 1uy; dst.Write (int v)
            | None -> dst.Write 0uy
            match s.Filter with
            | Some v -> dst.Write 1uy; dst.Write (int v)
            | None -> dst.Write 0uy
            match s.Comparison with
            | Some v -> dst.Write 1uy; dst.Write (int v)
            | None -> dst.Write 0uy
            match s.BorderColor with
            | Some v -> dst.Write 1uy; dst.Write v.R; dst.Write v.G; dst.Write v.B; dst.Write v.A
            | None -> dst.Write 0uy
            match s.MaxAnisotropy with
            | Some v -> dst.Write 1uy; dst.Write v
            | None -> dst.Write 0uy
            match s.MaxLod with
            | Some v -> dst.Write 1uy; dst.Write v
            | None -> dst.Write 0uy
            match s.MinLod with
            | Some v -> dst.Write 1uy; dst.Write v
            | None -> dst.Write 0uy
            match s.MipLodBias with
            | Some v -> dst.Write 1uy; dst.Write v
            | None -> dst.Write 0uy

        let deserialize (src : BinaryReader) =
            let addressU = 
                match src.ReadByte() with
                | 0uy -> None
                | _ -> Some (src.ReadInt32() |> unbox<WrapMode>)
            let addressV = 
                match src.ReadByte() with
                | 0uy -> None
                | _ -> Some (src.ReadInt32() |> unbox<WrapMode>)
            let addressW = 
                match src.ReadByte() with
                | 0uy -> None
                | _ -> Some (src.ReadInt32() |> unbox<WrapMode>)
            let filter = 
                match src.ReadByte() with
                | 0uy -> None
                | _ -> Some (src.ReadInt32() |> unbox<Filter>)
            let comparison = 
                match src.ReadByte() with
                | 0uy -> None
                | _ -> Some (src.ReadInt32() |> unbox<ComparisonFunction>)
            let borderColor = 
                match src.ReadByte() with
                | 0uy -> None
                | _ -> Some (Aardvark.Base.C4f(src.ReadSingle(), src.ReadSingle(), src.ReadSingle(), src.ReadSingle()))
            let maxAnisotropy = 
                match src.ReadByte() with
                | 0uy -> None
                | _ -> Some (src.ReadInt32())
            let maxLod = 
                match src.ReadByte() with
                | 0uy -> None
                | _ -> Some (src.ReadDouble())
            let minLod = 
                match src.ReadByte() with
                | 0uy -> None
                | _ -> Some (src.ReadDouble())
            let mipLodBias = 
                match src.ReadByte() with
                | 0uy -> None
                | _ -> Some (src.ReadDouble())

            {
                FilterReduction = None
                AddressU = addressU
                AddressV = addressV
                AddressW = addressW
                Filter = filter
                Comparison = comparison
                BorderColor = borderColor
                MaxAnisotropy = maxAnisotropy
                MaxLod = maxLod
                MinLod = minLod
                MipLodBias = mipLodBias
            }

    module private Reflection =
        open System.Reflection
        open Aardvark.Base.IL

        let setShaderParent : WGSLShaderInterface -> WGSLProgramInterface -> unit =
            let tShader = typeof<WGSLShaderInterface>
            let fParent = tShader.GetField("program@", System.Reflection.BindingFlags.NonPublic ||| System.Reflection.BindingFlags.Instance)
            if isNull fParent then
                failwith "[FShade] internal error: cannot get parent field for WGSLShaderInterface"
            cil {
                do! IL.ldarg 0
                do! IL.ldarg 1
                do! IL.stfld fParent
                do! IL.ret
            }

    let internal serializeInternal (dst : BinaryWriter) (program : WGSLProgramInterface) =
        dst.Write (List.length program.inputs)
        for p in program.inputs do WGSLParameter.serializeInternal dst p
        
        dst.Write (List.length program.outputs)
        for p in program.outputs do WGSLParameter.serializeInternal dst p

        dst.Write program.textures.Count
        for KeyValue(name, sampler) in program.textures do
            dst.Write name
            dst.Write sampler.textureBinding
            dst.Write sampler.textureCount
            dst.Write sampler.textureName
            dst.Write sampler.textureGroup
            WGSLType.serializeInternal dst (WGSLType.Sampler sampler.textureType)
            
            dst.Write (List.length sampler.textureNames)
            for (name) in sampler.textureNames do
                dst.Write name
                
        for KeyValue(name, sampler) in program.samplers do
            dst.Write name
            dst.Write sampler.samplerBinding
            dst.Write sampler.samplerGroup
            SamplerState.serialize dst sampler.samplerState
            
        dst.Write program.images.Count
        for KeyValue(name, image) in program.images do
            dst.Write name
            dst.Write image.imageBinding
            dst.Write image.imageName
            dst.Write image.imageGroup
            WGSLType.serializeInternal dst (WGSLType.Image image.imageType)
            
        dst.Write program.storageBuffers.Count
        for KeyValue(name, ssb) in program.storageBuffers do
            dst.Write name
            dst.Write ssb.ssbBinding
            dst.Write ssb.ssbName
            dst.Write ssb.ssbGroup
            dst.Write (if ssb.ssbReadOnly then 1 else 0)
            WGSLType.serializeInternal dst ssb.ssbType
            
        dst.Write program.uniformBuffers.Count
        for KeyValue(name, ub) in program.uniformBuffers do
            dst.Write name
            dst.Write ub.ubBinding
            dst.Write ub.ubName
            dst.Write ub.ubGroup
            dst.Write ub.ubSize
            dst.Write (List.length ub.ubFields)
            for f in ub.ubFields do
                dst.Write f.ufName
                dst.Write f.ufOffset
                WGSLType.serializeInternal dst f.ufType

        match program.shaders with
        | WGSLProgramShaders.Graphics gr ->
            dst.Write 0uy
            dst.Write gr.stages.Count
            for KeyValue(stage, shader) in gr.stages do
                dst.Write (int stage)
                WGSLShaderInterface.serializeInternal dst shader

        | WGSLProgramShaders.Compute comp ->
            dst.Write 1uy
            WGSLShaderInterface.serializeInternal dst comp
            ()
    let internal deserializeInternal (src : BinaryReader) =

        let allShaders = System.Collections.Generic.List<WGSLShaderInterface>()
        let inline add s = allShaders.Add s; s

        let inputs =
            let cnt = src.ReadInt32()
            List.init cnt (fun _ -> WGSLParameter.deserializeInternal src)
        let outputs =
            let cnt = src.ReadInt32()
            List.init cnt (fun _ -> WGSLParameter.deserializeInternal src)

        let textures =
            let cnt = src.ReadInt32()
            List.init cnt (fun _ ->
                let name = src.ReadString()
                let samplerBinding = src.ReadInt32()
                let samplerCount = src.ReadInt32()
                let samplerName = src.ReadString()
                let samplerSet = src.ReadInt32()
                let samplerType =
                    match WGSLType.deserializeInternal src with
                    | WGSLType.Sampler s -> s
                    | t -> failwithf "unexpected SamplerType: %A" t

                let samplerTextures =
                    let cnt = src.ReadInt32()
                    List.init cnt (fun _ ->
                        src.ReadString()
                    )

                name, {
                    textureBinding = samplerBinding
                    textureCount = samplerCount
                    textureName = samplerName
                    textureGroup = samplerSet
                    textureType = samplerType
                    textureNames = samplerTextures
                }

            )
            |> MapExt.ofList

        let samplers =
            let cnt = src.ReadInt32()
            List.init cnt (fun _ ->
                let name = src.ReadString()
                let samplerBinding = src.ReadInt32()
                let samplerGroup = src.ReadInt32()
                let samplerState = SamplerState.deserialize src
                name, { samplerName = name; samplerGroup = samplerGroup; samplerBinding = samplerBinding; samplerState = samplerState }
            )
            |> MapExt.ofList
        
        let images =
            let cnt = src.ReadInt32()
            List.init cnt (fun _ ->
                let name = src.ReadString()
                let imageBinding = src.ReadInt32()
                let imageName = src.ReadString()
                let imageSet = src.ReadInt32()
                let imageType =
                    match WGSLType.deserializeInternal src with
                    | WGSLType.Image i -> i
                    | t -> failwithf "bad ImageType: %A" t

                name, {
                    imageBinding = imageBinding
                    imageName = imageName
                    imageGroup = imageSet
                    imageType = imageType
                }
            )   
            |> MapExt.ofList
            
        let storageBuffers =
            let cnt = src.ReadInt32()
            List.init cnt (fun _ ->
                let name = src.ReadString()
                let ssbBinding = src.ReadInt32()
                let ssbName = src.ReadString()
                let ssbSet = src.ReadInt32()
                let ssbReadOnly = src.ReadInt32() <> 0
                let ssbType = WGSLType.deserializeInternal src
                name, {
                    ssbBinding = ssbBinding
                    ssbName = ssbName
                    ssbGroup = ssbSet
                    ssbType = ssbType
                    ssbReadOnly = ssbReadOnly
                }
            )   
            |> MapExt.ofList

        let uniformBuffers =
            let cnt = src.ReadInt32()
            List.init cnt (fun _ ->
                let name = src.ReadString()
                let ubBinding = src.ReadInt32()
                let ubName = src.ReadString()
                let ubSet = src.ReadInt32()
                let ubSize = src.ReadInt32()
                let ubFields =
                    let cnt = src.ReadInt32()
                    List.init cnt (fun _ ->
                        let ufName = src.ReadString()
                        let ufOffset = src.ReadInt32()
                        let ufType = WGSLType.deserializeInternal src
                        { ufName = ufName; ufOffset = ufOffset; ufType = ufType }
                    )
                name, {
                    ubGroup = ubSet
                    ubBinding = ubBinding
                    ubName = ubName
                    ubSize = ubSize
                    ubFields = ubFields
                }
            ) |> MapExt.ofList


        let shaders = 
            match src.ReadByte() with
            | 0uy ->
                let shaders =
                    let cnt = src.ReadInt32()
                    List.init cnt (fun _ -> 
                        let stage = src.ReadInt32() |> unbox<ShaderStage>
                        stage, add (WGSLShaderInterface.deserializeInternal src)
                    )
                    |> MapExt.ofList

                WGSLProgramShaders.Graphics { WGSLGraphicsShaders.stages = shaders }
            | _ ->
                let comp = WGSLShaderInterface.deserializeInternal src
                WGSLProgramShaders.Compute comp
          

        let result = 
            {
                inputs = inputs
                outputs = outputs
                textures = textures
                samplers = samplers
                images = images
                storageBuffers = storageBuffers
                uniformBuffers = uniformBuffers
                shaders = shaders
            }

        for s in allShaders do 
            Reflection.setShaderParent s result


        result

module LayoutStd140 =
    // https://www.khronos.org/registry/OpenGL/extensions/ARB/ARB_uniform_buffer_object.txt
    //
    //(1) If the member is a scalar consuming <N> basic machine units, the
    //    base alignment is <N>.
    //
    //(2) If the member is a two- or four-component vector with components
    //    consuming <N> basic machine units, the base alignment is 2<N> or
    //    4<N>, respectively.
    //
    //(3) If the member is a three-component vector with components consuming
    //    <N> basic machine units, the base alignment is 4<N>.
    //
    //(4) If the member is an array of scalars or vectors, the base alignment
    //    and array stride are set to match the base alignment of a single
    //    array element, according to rules (1), (2), and (3), and rounded up
    //    to the base alignment of a vec4. The array may have padding at the
    //    end; the base offset of the member following the array is rounded up
    //    to the next multiple of the base alignment.
    //
    //(5) If the member is a column-major matrix with <C> columns and <R>
    //    rows, the matrix is stored identically to an array of <C> column
    //    vectors with <R> components each, according to rule (4).
    //
    //(6) If the member is an array of <S> column-major matrices with <C>
    //    columns and <R> rows, the matrix is stored identically to a row of
    //    <S>*<C> column vectors with <R> components each, according to rule
    //    (4).
    //
    //(7) If the member is a row-major matrix with <C> columns and <R> rows,
    //    the matrix is stored identically to an array of <R> row vectors
    //    with <C> components each, according to rule (4).
    //
    //(8) If the member is an array of <S> row-major matrices with <C> columns
    //    and <R> rows, the matrix is stored identically to a row of <S>*<R>
    //    row vectors with <C> components each, according to rule (4).
    //
    //(9) If the member is a structure, the base alignment of the structure is
    //    <N>, where <N> is the largest base alignment value of any of its
    //    members, and rounded up to the base alignment of a vec4. The
    //    individual members of this sub-structure are then assigned offsets 
    //    by applying this set of rules recursively, where the base offset of
    //    the first member of the sub-structure is equal to the aligned offset
    //    of the structure. The structure may have padding at the end; the 
    //    base offset of the member following the sub-structure is rounded up
    //    to the next multiple of the base alignment of the structure.
    //
    //(10) If the member is an array of <S> structures, the <S> elements of
    //     the array are laid out in order, according to rule (9).
    let private next (a : int) (v : int) =
        if v % a = 0 then v
        else v + (a - (v % a))

    let rec layout (t : WGSLType) =
        match t with
            | WGSLType.Void ->
                t, 1, 0

            | WGSLType.Bool ->
                t, 4, 4

            | WGSLType.Int(_,w) ->
                let s = w / 8
                t, s, s

            | WGSLType.Float(64) ->
                t, 4, 4
                
            | WGSLType.Float(w) ->
                let s = w / 8 
                t, s, s

            | WGSLType.Vec(3, bt) ->
                let bt, a, s = layout bt
                WGSLType.Vec(3, bt), 4 * a, 3 * s
                
            | WGSLType.Vec(d, bt) ->
                let bt, a, s = layout bt
                WGSLType.Vec(d, bt), d * s, d * s

            | WGSLType.Array(len, bt, _) ->
                let bt, a, s = layout bt

                let s = next 16 s
                let a = next 16 a

                WGSLType.Array(len, bt, s), a, len * s

            | WGSLType.Mat(cols, rows, bt) ->
                let narr, a, s = layout (WGSLType.Array(cols, WGSLType.Vec(rows, bt), -1))
                let bt = 
                    match narr with
                        | WGSLType.Array(_, WGSLType.Vec(_,bt), _) -> bt
                        | _ -> failwith "that was unexpected"
                WGSLType.Mat(cols, rows, bt), a, s

            | WGSLType.Struct(name, fields, _) ->
                let mutable offset = 0
                let mutable largestAlign = 0

                let newFields =
                    fields |> List.map (fun (name, typ,_) ->
                        let (typ, align, size) = layout typ

                        if offset % align <> 0 then
                            offset <- offset + (align - offset % align)

                        largestAlign <- max largestAlign align
                        let res = name, typ, offset
                        offset <- offset + size

                        res

                    )

                let align = next 16 largestAlign
                let size = next 16 offset

                WGSLType.Struct(name, newFields, offset), align, size
                
            | WGSLType.DynamicArray(e,_) ->
                let (e,align,s) = layout e
                let align = next 16 align

                WGSLType.DynamicArray(e, s), align, 0

            | WGSLType.Image i ->
                WGSLType.Image i, 1, 0
                
            | WGSLType.Sampler s ->
                WGSLType.Sampler s, 1, 0
                
            | WGSLType.Intrinsic s ->
                WGSLType.Intrinsic s, 1, 0

    let applyLayout (ub : WGSLUniformBuffer) : WGSLUniformBuffer =
        let mutable offset = 0

        let newFields =
            ub.ubFields |> List.map (fun uf ->
                let nufType, align, size = layout uf.ufType

                if offset % align <> 0 then
                    offset <- offset + (align - offset % align)

                let res = offset
                offset <- offset + size

                { uf with ufOffset = res; ufType = nufType }
            )
            
        let size = next 16 offset
                    
        { ub with ubFields = newFields; ubSize = size }

    let apply (iface : WGSLProgramInterface) =
        // (a,(b,c))
        // ((a,b), (c,d))
        let inline layout a = let (t,_,_) = layout a in t

        let applyToInterface (s : WGSLShaderInterface) =
            { s with
                shaderInputs = s.shaderInputs |> List.map (fun p -> { p with paramType = layout p.paramType})
                shaderOutputs = s.shaderOutputs |> List.map (fun p -> { p with paramType = layout p.paramType}) }

        let applyToGraphics (s : WGSLGraphicsShaders) =
            { s with stages = s.stages |> MapExt.map (fun _ x -> applyToInterface x) }

        { iface with
            storageBuffers = iface.storageBuffers |> MapExt.map (fun _ s -> { s with ssbType = layout s.ssbType })
            uniformBuffers = iface.uniformBuffers |> MapExt.map (fun _ -> applyLayout)
            inputs = iface.inputs |> List.map (fun p -> { p with paramType = layout p.paramType})
            outputs = iface.outputs |> List.map (fun p -> { p with paramType = layout p.paramType})
            shaders =
                match iface.shaders with
                | WGSLProgramShaders.Graphics g   -> WGSLProgramShaders.Graphics   <| applyToGraphics g
                | WGSLProgramShaders.Compute c    -> WGSLProgramShaders.Compute    <| applyToInterface c
        }




