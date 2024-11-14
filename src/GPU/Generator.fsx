open System.Text.Json
open System.IO
open System.Collections.Generic

let doc = JsonDocument.Parse (File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "dawn.json")))

let toOption ((exists : bool, value : 'a)) =
    if exists then Some value
    else None
let toOptionString ((exists : bool, value : JsonElement)) =
    if exists && value.ValueKind = JsonValueKind.String then Some (value.GetString())
    else None
           
module List =
    let mapOption (mapping : 'a -> option<'b>) (l : list<'a>) =
        let r = l |> List.map mapping
        if r |> List.forall Option.isSome then
            r |> List.map Option.get |> Some
        else
            None
type TypeRef = { TypeName : string; Annotation : option<string> }

module TypeRef =
    let isFloat (t : TypeRef) =
        match t.Annotation with
        | None ->
            t.TypeName = "float" || t.TypeName = "double"
        | _ ->
            false
            
    let is64Bit (t : TypeRef) =
        match t.Annotation with
        | None ->
            t.TypeName = "double" || t.TypeName = "int64_t"  || t.TypeName = "uint64_t"
        | _ ->
            false
            
type FieldDef =
    {
        Type : TypeRef
        Name : string
        Default : option<JsonElement>
        Optional : bool
        Length : option<string>
    }

module FieldDef =
    let tryParse (name : string) (e : JsonElement) =
        match e.TryGetProperty("type") with
        | (true, typ) when typ.ValueKind = JsonValueKind.String ->
            let typ = typ.GetString()
            let def = e.TryGetProperty("default") |> toOption
            let annotation = e.TryGetProperty("annotation") |> toOptionString
            let optional = e.TryGetProperty("optional") |> toOption |> Option.map (fun v -> v.GetBoolean()) |> Option.defaultValue false
            let length = e.TryGetProperty("length") |> toOptionString
            Some { Type = { TypeName = typ; Annotation = annotation }; Name = name; Default = def; Optional = optional; Length = length }
        | _ ->
            None

[<RequireQualifiedAccess>]
type Direction =
    | In
    | Out

type StructDef =
    {
        Name : string
        Extensible : option<Direction>
        ChainRoots : list<string>
        Chained : option<Direction>
        Fields : list<FieldDef>
    }

type FunctionDef =
    {
        Name : string
        Tags : list<string>
        Return : TypeRef
        Args : list<FieldDef>
    }

module FunctionDef =
    let tryParse (name : string) (obj : JsonElement) =
        let args = 
            match obj.TryGetProperty("args") with
            | (true, a) when a.ValueKind = JsonValueKind.Array ->
                let len = a.GetArrayLength()
                List.init len (fun i -> a.[i])
            | _ ->
                []
                
        let args = 
            args |> List.mapOption (fun a ->
                match a.TryGetProperty "name" with
                | (true, name) when name.ValueKind = JsonValueKind.String ->
                    FieldDef.tryParse (name.GetString()) a
                | _ ->
                    None
            )
               
        let returns =
            match obj.TryGetProperty("returns") with
            | (true, r) when r.ValueKind = JsonValueKind.String ->
                r.GetString()
            | _ ->
                "void"
        
        let tags =
            match obj.TryGetProperty "tags" with
            | (true, r) when r.ValueKind = JsonValueKind.Array ->
                List.init (r.GetArrayLength()) (fun i -> r[i].GetString())
            | _ ->
                []
               
        match args with
        | Some args ->
            // TODO annotation??
            Some { Name = name; Tags = tags; Return = { TypeName = returns; Annotation = None }; Args = args }
        | None ->
            None

    let isBadWasmFunction (f : FunctionDef) =
        f.Args.Length > 5 ||
        f.Args |> List.exists (fun a -> TypeRef.is64Bit a.Type || TypeRef.isFloat a.Type)

type Enum =
    {
        Name : string
        Values : list<string * int>
        Flags : bool
    }
    
type Alias =
    {
        Name : string
        Type : TypeRef
    }

type Native =
    {
        Name : string
        WasmType : option<string>
    }

type Object =
    {
        Name : string
        Tags : list<string>
        Methods : list<FunctionDef>
    }


type Definition =
    | Enum of Enum
    | Delegate of FunctionDef
    | Struct of StructDef
    | Function of FunctionDef
    | Alias of Alias
    | Native of Native
    | Object of Object
    | CallbackInfo of StructDef
    
    
    member x.Name =
        match x with
        | Enum { Name = n } -> n
        | Delegate { Name = n } -> n
        | Struct { Name = n } -> n
        | Function { Name = n } -> n
        | Alias { Name = n } -> n
        | Native { Name = n } -> n
        | Object { Name = n } -> n
        | CallbackInfo { Name = n } -> n
        
    member x.ReferencedTypes =
        match x with
        | Enum _ | Native _ ->
            Seq.empty
        | Delegate { Args = a; Return = r } | Function { Args = a; Return = r } ->
            Seq.append (Seq.singleton r) (a |> Seq.map (fun a -> a.Type))
        | Struct { Fields = f } | CallbackInfo { Fields = f } ->
            f |> Seq.map (fun f -> f.Type)
        | Alias { Type = t } -> Seq.singleton t
        | Object { Methods = ms } ->
            ms |> Seq.collect (fun m ->
                m.Args |> Seq.map (fun a -> a.Type)    
            )
        
    
    


let allList = ResizeArray()

module StructDef =
    let parse (name : string) (obj : JsonElement) =
        let extensible =
            match obj.TryGetProperty "extensible" with
            | (true, e) when e.ValueKind = JsonValueKind.String ->
                match e.GetString() with
                | "in" -> Some Direction.In
                | "out" -> Some Direction.Out
                | e -> failwithf "bad direction: %A" e
            | _ ->
                None
            
        let fields =
            match obj.TryGetProperty("members") with
            | (true, mems) ->
                let len = mems.GetArrayLength()
                List.init len (fun i ->
                    let mem = mems[i]
                    let typName = mem.GetProperty("type").GetString()
                    let name = mem.GetProperty("name").GetString()
                    let def = mem.TryGetProperty("default") |> toOption
                    let annotation = mem.TryGetProperty("annotation") |> toOptionString
                    let optional = mem.TryGetProperty("optional") |> toOption |> Option.map (fun v -> v.GetBoolean()) |> Option.defaultValue false
                    let length = mem.TryGetProperty("length") |> toOptionString
                    
                    let typRef = { TypeName = typName; Annotation = annotation }
                    { Type = typRef; Name = name; Default = def; Optional = optional; Length = length }
                )
            | _ ->
                []
        
        let chainRoots =
            match obj.TryGetProperty "chain roots" with
            | (true, roots) when roots.ValueKind = JsonValueKind.Array ->
                List.init (roots.GetArrayLength()) (fun i -> roots[i].GetString())
            | _ ->
                []
                
        let chained =
            match obj.TryGetProperty "chained" with
            | (true, c) when c.ValueKind = JsonValueKind.String ->
                match c.GetString() with
                | "in" -> Some Direction.In
                | "out" -> Some Direction.Out
                | e -> failwithf "bad direction: %A" e
            | _ ->
                None
                
        {
            Extensible = extensible
            Chained = chained
            Name = name
            ChainRoots = chainRoots 
            Fields = fields
        }

for kv in doc.RootElement.EnumerateObject() do
    let obj = kv.Value
    if obj.ValueKind = JsonValueKind.Object then
        match obj.TryGetProperty "category" with
        | (true, cat) ->
            match cat.GetString() with
            | "callback info" ->
                let name = kv.Name
                let s = StructDef.parse name obj
                allList.Add (CallbackInfo s)
                
            | "object" ->
                
                let tags =
                    match obj.TryGetProperty "tags" with
                    | (true, t) when t.ValueKind = JsonValueKind.Array ->
                        List.init (t.GetArrayLength()) (fun i -> t[i].GetString())
                    | _ ->
                        []
                match obj.TryGetProperty "methods" with
                | (true, ms) when ms.ValueKind = JsonValueKind.Array ->
                    let meths = 
                        List.init (ms.GetArrayLength()) (fun i ->
                            match ms.[i].TryGetProperty "name" with
                            | (true, n) when n.ValueKind = JsonValueKind.String ->
                                match FunctionDef.tryParse (n.GetString()) ms.[i] with
                                | Some f -> Some f
                                | None -> None
                            | _ ->
                                None
                        )
                        
                    match meths |> List.mapOption id with
                    | Some meths ->
                        allList.Add (Object { Name = kv.Name; Tags = tags; Methods = meths })
                    | None ->
                        ()
                | _ ->
                    allList.Add (Object { Name = kv.Name; Tags = tags; Methods = [] })
                    
                () // TODO
                
            | "native" ->
                let w = 
                    match obj.TryGetProperty "wasm type" with
                    | (true, w) when w.ValueKind = JsonValueKind.String ->
                        let w = w.GetString()
                        Some w
                    | _ ->
                        None
                allList.Add(Native { Name = kv.Name; WasmType =  w })
                
            | "typedef" ->
                match obj.TryGetProperty "type" with
                | (true, t) when t.ValueKind = JsonValueKind.String ->
                    let annotation = obj.TryGetProperty("annotation") |> toOptionString
                    allList.Add(Alias { Name = kv.Name; Type = { TypeName = t.GetString(); Annotation = annotation } })
                | _ ->
                    ()
                
            | "constant" ->
                () // simple
                
            | "enum" | "bitmask" ->
                let values =
                    match obj.TryGetProperty "values" with
                    | (true, vs) when vs.ValueKind = JsonValueKind.Array ->
                        List.init (vs.GetArrayLength()) (fun i ->
                            let v = vs.[i]
                            let name = v.GetProperty("name").GetString()
                            let v = v.GetProperty("value").GetInt32()
                            name, v
                        )
                    | _ ->
                        []
                let e = { Name = kv.Name; Values = values; Flags = cat.GetString() = "bitmask" }
                allList.Add (Enum e)
            
            | "function" -> 
                match FunctionDef.tryParse kv.Name obj with
                | Some f ->
                    allList.Add (Function f)
                | None ->
                    ()
            
            | "function pointer" | "callback function"  ->
                match FunctionDef.tryParse kv.Name obj with
                | Some f ->
                    allList.Add (Delegate f)
                | None ->
                    ()
            | "structure" ->
                let name = kv.Name
                let s = StructDef.parse name obj
                allList.Add (Struct s)
            
            | cat ->
                failwithf "UNKNOWN CATEGORY: %A" cat
            
        | _ ->
            ()
    

let nonExistentTypes = Set.ofList ["external texture binding layout"; "logging callback"; "external texture"; "image copy external texture"; "copy texture for browser options"]



let all =
    Seq.toArray allList
    |> Array.choose (fun a ->
        match a with
        
        | Object o ->
            
            let meths = 
                o.Methods |> List.filter (fun m ->
                    let deprecated = m.Tags |> List.exists (fun t -> t = "deprecated")
                    let dawnOnly =
                        m.Tags |> List.exists (fun t -> t = "dawn") &&
                        not (m.Tags |> List.exists (fun t -> t = "emscripten"))
                    
                    let bad =
                        Set.contains m.Return.TypeName nonExistentTypes ||
                        m.Args |> Seq.exists (fun a -> Set.contains a.Type.TypeName nonExistentTypes)
                    not dawnOnly && not deprecated && not bad
                )
            Some (Object { o with Methods = meths })
        | _ ->
            Some a
    )
   
let table = Dictionary()
for a in all do
    table.[a.Name] <- a
    
let tryResolveType (t : TypeRef) =
    match table.TryGetValue t.TypeName with
    | (true, entry) ->
        Some entry
    | _ ->
        None
    

let rx = System.Text.RegularExpressions.Regex "^[0-9]+.*$"
                
    
// print native wrapper
let pascalCase (str : string) =
    let res = 
        str.Split(" ")
        |> Array.map (fun str -> str.Substring(0, 1).ToUpper() + str.Substring(1))
        |> String.concat ""

    if rx.IsMatch res then "D" + res
    else res

let camelCase (str : string) =
    let res = 
        str.Split(" ")
        |> Array.mapi (fun i str -> if i > 0 then str.Substring(0, 1).ToUpper() + str.Substring(1) else str)
        |> String.concat ""

    if res = "type" then "typ"
    elif res = "module" then "moodule"
    elif rx.IsMatch res then "d" + res
    else res



module Native =
        
    let rec nativeTypeName (t : TypeRef) =
        let def = table.[t.TypeName]
        
        let baseType = 
            match def with
            | Object o -> "WGPU" + pascalCase o.Name
            | Enum e -> "int"
            | Delegate d -> "void*" //"WGPU" + pascalCase d.Name
            | Alias a -> nativeTypeName a.Type
            | Struct a -> "WGPU" + pascalCase a.Name
            | Function _ -> failwith "not a type"
            | CallbackInfo c -> "WGPU" + pascalCase c.Name
            | Native n -> n.Name
            
            
        match t.Annotation with
        | None -> baseType
        | Some a ->
            match a with
            | "*" -> baseType + "*"
            | "const*" -> "const " + baseType + "*"
            | "const*const*" -> "const " + baseType + "* const*"
            | _ -> failwith "asdasdsad"
        
    let print() =
        let b = System.Text.StringBuilder()
        let printfn fmt = fmt |> Printf.kprintf (fun str -> b.AppendLine str |> ignore)
        
        
        
        printfn "#include <emscripten.h>"
        printfn "#include <emscripten/html5.h>"
        printfn "#include <SDL/SDL_image.h>"
        printfn "#include <string.h>"
        printfn "#include <stdlib.h>"
        printfn "#include <stdio.h>"
        printfn "#include <stdint.h>"
        printfn "#include <webgpu/webgpu.h>"
        printfn " "
        printfn "typedef void* WGPUExternalTexture;"
        for a in all do
            match a with
            | Enum _ | Delegate _ | Alias _ | Native _ | CallbackInfo _ | Struct _ ->
                ()
            | Function f ->
                ()
            | Object o ->
                if o.Tags <> ["dawn"] then
                    for m in o.Methods do
                        let name = o.Name + " " + m.Name
                        
                        let args = { Name = "self"; Type = { TypeName = o.Name; Annotation = None }; Default = None; Optional = false; Length = None } :: m.Args
                        let m = { m with Args = args }
                        
                        
                        if FunctionDef.isBadWasmFunction m then
                            
                            
                            printfn "typedef struct { "
                            for a in args do
                                printfn "   %s %s;" (nativeTypeName a.Type) (pascalCase a.Name)
                            printfn "} WGPU%sArgs;" (pascalCase name)
                            
                            
                            let argdef = sprintf "const WGPU%sArgs* args" (pascalCase name)
                            let argref = args |> List.map (fun a -> sprintf "args->%s" (pascalCase a.Name)) |> String.concat ", "
                            
                            printfn $"EMSCRIPTEN_KEEPALIVE {nativeTypeName m.Return} gpu{pascalCase name}({argdef}) {{"
                            printfn $"    return wgpu{pascalCase name}({argref});"
                            printfn $"}}"
                        else
                                    
                            let argdef = args |> List.map (fun a -> nativeTypeName a.Type + " " + camelCase a.Name) |> String.concat ", "
                            let argref = args |> List.map (fun a -> camelCase a.Name) |> String.concat ", "
                            
                            printfn $"EMSCRIPTEN_KEEPALIVE {nativeTypeName m.Return} gpu{pascalCase name}({argdef}) {{"
                            printfn $"    return wgpu{pascalCase name}({argref});"
                            printfn $"}}"
                            
                            ()
                        
                    ()
        File.WriteAllText(Path.Combine(__SOURCE_DIRECTORY__, "Native.c"), b.ToString())

module Enums =
    
    let print() =
        let b = System.Text.StringBuilder()
        let printfn fmt = fmt |> Printf.kprintf (fun str -> b.AppendLine str |> ignore)
        
        
        printfn "namespace rec WebGPU"
        printfn "open System"
        printfn "#nowarn \"9\""
        
        for a in all do
            match a with
            | Enum e ->
                
                if e.Flags then printfn "[<Flags>]"
                printfn "type %s =" (pascalCase e.Name)
                for (name, value) in e.Values do
                    
                    printfn "    | %s = %d" (pascalCase name) value
            | _ ->
                ()
        File.WriteAllText(Path.Combine(__SOURCE_DIRECTORY__, "Enums.fs"), b.ToString())


module RawWrapper =
    
    let rec externName (t : TypeRef) =
        let def = table.[t.TypeName]
        
        let baseType = 
            match def with
            | Object o -> "nativeint"
            | Enum e -> pascalCase e.Name
            | Delegate d -> "nativeint" //pascalCase d.Name
            | Alias a -> externName a.Type
            | Struct a -> pascalCase a.Name
            | Function _ -> failwith "not a type"
            | CallbackInfo c -> pascalCase c.Name
            | Native n ->
                match n.Name with
                | "int8_t" -> "int8"
                | "uint8_t" -> "uint8"
                | "int16_t" -> "int16"
                | "uint16_t" -> "uint16"
                | "int32_t" -> "int"
                | "uint32_t" -> "uint32"
                | "int64_t" -> "int64"
                | "uint64_t" -> "uint64"
                | "void" -> "void"
                | "bool" -> "int"
                | "char" -> "byte"
                | "float" -> "float32"
                | "double" -> "double"
                | "size_t" -> "unativeint"
                | "void *" | "void const *" -> "nativeint"
                | _ -> failwithf "bad native type: %A" n.Name
            
            
        match t.Annotation with
        | None -> baseType
        | Some a ->
            match a with
            | "*" -> baseType + "*"
            | "const*" -> baseType + "*"
            | "const*const*" -> baseType + "**"
            | _ -> failwith "asdasdsad"
      
    let rec fsharpName (t : TypeRef) =
        
        let def = table.[t.TypeName]
        
        let baseType = 
            match def with
            | Object o -> "nativeint"
            | Enum e -> pascalCase e.Name
            | Delegate d -> "nativeint" //pascalCase d.Name
            | Alias a -> fsharpName a.Type
            | Struct a -> pascalCase a.Name
            | Function _ -> failwith "not a type"
            | CallbackInfo c -> pascalCase c.Name
            | Native n ->
                match n.Name with
                | "int8_t" -> "int8"
                | "uint8_t" -> "uint8"
                | "int16_t" -> "int16"
                | "uint16_t" -> "uint16"
                | "int32_t" -> "int"
                | "uint32_t" -> "uint32"
                | "int64_t" -> "int64"
                | "uint64_t" -> "uint64"
                | "void" -> "unit"
                | "bool" -> "int"
                | "char" -> "byte"
                | "float" -> "float32"
                | "double" -> "double"
                | "size_t" -> "unativeint"
                | "void *" | "void const *" -> "nativeint"
                | _ -> failwithf "bad native type: %A" n.Name
            
            
        match t.Annotation with
        | None -> baseType
        | Some a ->
            if baseType = "unit" then
                "nativeint"
            else
                match a with
                | "*" -> $"nativeptr<{baseType}>"
                | "const*" -> $"nativeptr<{baseType}>"
                | "const*const*" -> $"nativeptr<nativeptr<{baseType}>>"
                | _ -> failwith "asdasdsad"
      
    let print() =
        let b = System.Text.StringBuilder()
        let printfn fmt = fmt |> Printf.kprintf (fun str -> b.AppendLine str |> ignore)
        
        
        printfn "namespace rec WebGPU.Raw"
        printfn "open System.Collections.Generic"
        printfn "open System"
        printfn "open System.Runtime.InteropServices"
        printfn "open WebGPU"
        printfn "#nowarn \"9\""
        
        
        
        for a in all do
            match a with
            | Enum e ->
                ()
                //
                // if e.Flags then printfn "[<Flags>]"
                // printfn "type %s =" (pascalCase e.Name)
                // for (name, value) in e.Values do
                //     
                //     printfn "    | %s = %d" (pascalCase name) value
            | Alias a ->
                printfn "type %s = %s" (pascalCase a.Name) (fsharpName a.Type)
        
            | Delegate d ->
                let ret = fsharpName d.Return
                match d.Args with
                | [] ->
                    printfn "type %s = delegate of unit -> %s" (pascalCase d.Name) ret
                    
                | _ -> 
                    let args = d.Args |> List.map (fun a -> sprintf "%s : %s" (camelCase a.Name) (fsharpName a.Type)) |> String.concat " * "
                    printfn "type %s = delegate of %s -> %s" (pascalCase d.Name) args ret
            | Struct s | CallbackInfo s ->
                if s.Fields.IsEmpty then
                    printfn "[<StructLayout(LayoutKind.Explicit, Size = 4)>]"
                    printfn "type %s = struct end" (pascalCase s.Name)
                else
                    
                    
                    let fields = s.Fields
                    
                    let fields =
                        if Option.isSome s.Chained then { Name = "s type"; Type = { TypeName = "s type"; Annotation = None }; Default = None; Optional = false; Length = None } :: fields
                        else fields
                    
                    let fields =
                        if Option.isSome s.Extensible || Option.isSome s.Chained then { Name = "next in chain"; Type = { TypeName = "void"; Annotation = Some "const*" }; Optional = true; Default = None; Length = None } :: fields
                        else fields
                        
                    let args = 
                        fields |> List.map (fun f ->
                            let typ = fsharpName f.Type
                            let name = camelCase f.Name
                            $"{name} : {typ}"
                        )
                        |> String.concat ", "
                    
                    printfn "[<Struct; StructLayout(LayoutKind.Sequential)>]"
                    printfn "type %s = " (pascalCase s.Name)
                    printfn "    struct"
                    for f in fields do
                        printfn $"        val mutable public {pascalCase f.Name} : {fsharpName f.Type}"
                        // let typ = fsharpName f.Type
                        // let name = pascalCase f.Name
                        // printfn $"    member _.{pascalCase f.Name} : {typ} = {camelCase f.Name}"
                        
                    let ctorArgs = 
                        fields |> List.map (fun f ->
                            let typ = fsharpName f.Type
                            let name = camelCase f.Name
                            $"{name} : {typ}"
                        )
                        |> String.concat ", "
                        
                    let ass = fields |> List.map (fun f -> $"{pascalCase f.Name} = {camelCase f.Name}") |> String.concat "; "
                    printfn $"        new({ctorArgs}) = {{ {ass} }}"
                        
                    if Option.isSome s.Extensible || Option.isSome s.Chained then
                        let ctorArgs = 
                            s.Fields |> List.map (fun f ->
                                let typ = fsharpName f.Type
                                let name = camelCase f.Name
                                $"{name} : {typ}"
                            )
                            |> String.concat ", "
                            
                        let ctoruse =
                            let u = s.Fields |> List.map (fun f -> camelCase f.Name)
                            let args = 
                                if Option.isSome s.Chained then
                                    "0n" :: "SType.Invalid" :: u
                                elif Option.isSome s.Extensible then
                                    "0n" :: u
                                else
                                    u
                            String.concat ", " args
                        printfn $"        new({ctorArgs}) = {pascalCase s.Name}({ctoruse})"
                        
                    printfn "    end"
            | Function _ | Native _ | Object _ ->
                () // TODO
        
        let methods =
            all |> Seq.collect (fun d ->
                match d with
                | Object o ->
                    o.Methods |> Seq.map (fun m ->
                        { m with Name = o.Name + " " + m.Name; Args = { Name = "self"; Type = { TypeName = o.Name; Annotation = None }; Default = None; Optional = false; Length = None } :: m.Args }    
                    )
                | Function d ->
                    Seq.singleton d
                | _ ->
                    Seq.empty
            )
        
        
        printfn "module WebGPU = "
        printfn ""
        for m in methods do
            if FunctionDef.isBadWasmFunction m then
                let structName = $"{(pascalCase m.Name)}Args"
                printfn "    [<Struct; StructLayout(LayoutKind.Sequential)>]"
                printfn "    type %sArgs = " (pascalCase m.Name)
                printfn "        {"
                for a in m.Args do
                    printfn "            %s : %s" (pascalCase a.Name) (fsharpName a.Type)
                printfn "        }"
                printfn ""
                printfn $"    [<DllImport(\"Native\", EntryPoint=\"gpu{pascalCase m.Name}\")>]"
                printfn $"    extern {externName m.Return} _{pascalCase m.Name}({(pascalCase m.Name)}Args& args)"
            
                let argdef = m.Args |> Seq.map (fun a -> $"{camelCase a.Name} : {fsharpName a.Type}") |> String.concat ", "
                let argref = m.Args |> Seq.map (fun a -> $"{camelCase a.Name}") |> String.concat ", "
                printfn $"    let {pascalCase m.Name}({argdef}) ="
                printfn $"        let mutable args = {{"
                for a in m.Args do
                    printfn $"            {structName}.{pascalCase a.Name} = {camelCase a.Name};"
                printfn $"        }}"
                printfn $"        _{pascalCase m.Name}(&args)"
            else
                let args = m.Args |> Seq.map (fun a -> $"{externName a.Type} {camelCase a.Name}") |> String.concat ", "
                printfn $"    [<DllImport(\"Native\", EntryPoint=\"gpu{pascalCase m.Name}\")>]"
                printfn $"    extern {externName m.Return} {pascalCase m.Name}({args})"
        
        let delegates =
            all |> Seq.choose (fun v ->
                match v with
                | Delegate d ->
                    if d.Args |> List.exists (fun a -> a.Name = "userdata") then
                        Some d
                    else
                        None
                | _ ->
                    None
            )
        

        printfn "type WebGPUCallbacks() ="
        for d in delegates do
            printfn $"    static let {camelCase d.Name}Callbacks = Dictionary<nativeint, {pascalCase d.Name}>()"
            printfn $"    static let mutable {camelCase d.Name}Current = 0n"
            printfn $"    static let {camelCase d.Name}Delegate = System.Delegate.CreateDelegate(typeof<{pascalCase d.Name}>, typeof<WebGPUCallbacks>.GetMethod \"{pascalCase d.Name}\")"
            printfn $"    static let {camelCase d.Name}Ptr = Marshal.GetFunctionPointerForDelegate({camelCase d.Name}Delegate)"
        for d in delegates do
            let dictName = $"{camelCase d.Name}Callbacks"
            let currentName = $"{camelCase d.Name}Current"
            let ptrName = $"{camelCase d.Name}Ptr"
            let args = d.Args |> List.map (fun a -> $"{camelCase a.Name} : {fsharpName a.Type}") |> String.concat ", "
            let arguse = d.Args |> List.map (fun a -> if a.Name = "userdata" then "0n" else camelCase a.Name) |> String.concat ", "
            printfn $"    [<UnmanagedCallersOnly>]"
            printfn $"    static member {pascalCase d.Name}({args}) ="
            printfn $"        let callback = "
            printfn $"            lock {dictName} (fun () ->"
            printfn $"                match {dictName}.TryGetValue(userdata) with"
            printfn $"                | (true, cb) ->"
            printfn $"                    {dictName}.Remove(userdata) |> ignore"
            printfn $"                    Some cb"
            printfn $"                | _ ->"
            printfn $"                    None"
            printfn $"            )"
            printfn $"        match callback with"
            printfn $"        | Some cb -> cb.Invoke({arguse})"
            printfn $"        | None -> ()"
            printfn $""
            printfn $"    static member Register(cb : {pascalCase d.Name}) ="
            printfn $"        lock {dictName} (fun () ->"
            printfn $"            let id = {currentName}"
            printfn $"            {currentName} <- {currentName} + 1n"
            printfn $"            {dictName}.[id] <- cb"
            printfn $"            struct({ptrName}, id)"
            printfn $"        )"
        
        File.WriteAllText(Path.Combine(__SOURCE_DIRECTORY__, "Wrapper.fs"), b.ToString())

module Frontend =
     
    let rec frontendName (moreThanOne : bool) (t : TypeRef) =
        match table.TryGetValue t.TypeName with
        | (true, def) ->
            let baseType = 
                match def with
                | Object o -> pascalCase o.Name
                | Enum e -> pascalCase e.Name
                | Delegate d -> pascalCase d.Name
                | Alias a -> frontendName moreThanOne a.Type
                | Struct a -> pascalCase a.Name
                | Function _ -> failwith "not a type"
                | CallbackInfo c -> pascalCase c.Name
                | Native n ->
                    match n.Name with
                    | "int8_t" -> "int8"
                    | "uint8_t" -> "uint8"
                    | "int16_t" -> "int16"
                    | "uint16_t" -> "uint16"
                    | "int32_t" -> "int"
                    | "uint32_t" -> "uint32"
                    | "int64_t" -> "int64"
                    | "uint64_t" -> "uint64"
                    | "void" -> "unit"
                    | "bool" -> "bool"
                    | "char" -> "byte"
                    | "float" -> "float32"
                    | "double" -> "double"
                    | "size_t" -> "unativeint"
                    | "void *" | "void const *" -> "nativeint"
                    | _ -> failwithf "bad native type: %A" n.Name
                
                
            match t.Annotation with
            | None -> baseType
            | Some a ->
                if baseType = "unit" then
                    "nativeint"
                else
                    match def with
                    | Object _ | Struct _ ->
                        if moreThanOne then
                            match a with
                            | "*" -> $"array<{baseType}>"
                            | "const*" -> $"array<{baseType}>"
                            | "const*const*" -> $"array<array<{baseType}>>"
                            | _ -> failwith "asdasdsad"
                        else
                            match a with
                            | "*" -> $"byref<{baseType}>"
                            | "const*" -> $"{baseType}"
                            | _ -> failwith "asdasdsad"
                            
                    | _ ->
                        match a with
                        | "*" -> $"nativeptr<{baseType}>"
                        | "const*" -> $"nativeptr<{baseType}>"
                        | "const*const*" -> $"nativeptr<nativeptr<{baseType}>>"
                        | _ -> failwith "asdasdsad"
        | _ ->
            t.TypeName
            
    type FieldTrafo =
        {
            FrontendField   : FieldDef
            BackendFields   : string -> list<string * FieldDef>
            PinFrontend     : string -> list<string> -> list<string>
            ReadBackend     : list<string> -> string
        }
                
    let pinStruct (valueName : string) (trafo : list<FieldTrafo>) (innercode : list<string * FieldDef> -> list<string>) =
        let rec pin (pinned : list<string * FieldDef>) (trafo : list<FieldTrafo>) (innercode : list<string * FieldDef> -> list<string>) =
            match trafo with
            | [] -> innercode pinned
            | h :: t ->
                let access = $"{valueName}.{pascalCase h.FrontendField.Name}"
                let p = pinned @ (h.BackendFields access)
                let innercode = pin p t innercode
                h.PinFrontend access innercode
        pin [] trafo innercode
                  
    let pinArgs (trafo : list<FieldTrafo>) (innercode : list<string * FieldDef> -> list<string>) =
        let rec pin (pinned : list<string * FieldDef>) (trafo : list<FieldTrafo>) (innercode : list<string * FieldDef> -> list<string>) =
            match trafo with
            | [] -> innercode pinned
            | h :: t ->
                let access = $"{camelCase h.FrontendField.Name}"
                let p = pinned @ (h.BackendFields access)
                let innercode = pin p t innercode
                h.PinFrontend access innercode
        pin [] trafo innercode
         
    let (|FieldOfType|_|) (d : FieldDef) =
        match tryResolveType d.Type with
        | Some t -> Some(t, d)
        | None -> None
               
    let (|Ptr|_|) (d : FieldDef) =
        match d.Type.Annotation with
        | Some "*" -> Some (Ptr(true, d))
        | Some "const*" -> Some(Ptr(false, d))
        | _ -> None
        
    let rec marshal (a : list<FieldDef>) =
        match a with
        | cnt :: Ptr(mut, ptr) :: t when ptr.Length = Some cnt.Name ->
            let innerType = frontendName true { TypeName = ptr.Type.TypeName; Annotation = None }
            
            let handleField = $"{camelCase ptr.Name}Handles"
            let ptrField = $"{camelCase ptr.Name}Ptr"
            let lenField = $"{camelCase ptr.Name}Len"
            let self =
                {
                    FrontendField = { Name = ptr.Name; Type = { TypeName = $"array<{innerType}>"; Annotation = None } ; Default = None; Optional = false; Length = None }
                    BackendFields = fun var -> [lenField, cnt; ptrField, ptr]
                    PinFrontend = fun var code ->
                        [
                            match ptr with
                            | FieldOfType(Object _, _) ->
                                yield $"let {handleField} = {var} |> Array.map (fun a -> a.Handle)"
                                yield $"use {ptrField} = fixed ({handleField})"
                                yield $"let {lenField} = uint32 {var}.Length"
                                yield! code
                            | FieldOfType(Struct _, _) ->
                                yield $"WebGPU.Raw.Pinnable.pinArray {var} (fun {ptrField} ->"
                                yield $"    let {lenField} = uint32 {var}.Length"
                                for c in code do
                                    yield $"    {c}"
                                yield ")"
                            | _ ->
                                yield $"use {ptrField} = fixed ({var})"
                                yield $"let {lenField} = uint32 {var}.Length"
                                yield! code
                        ]
                    ReadBackend = fun vars ->
                        match vars with
                        | [lenField; ptrField] ->
                            match ptr with
                            | FieldOfType(Object _, _) ->
                                $"let ptr = {ptrField} in Array.init (int {lenField}) (fun i -> new {innerType}(NativePtr.get ptr i))"//TODO3 {lenField} {ptrField}"
                            | FieldOfType(Struct _, _) ->
                                $"let ptr = {ptrField} in Array.init (int {lenField}) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in {innerType}.Read(&r))"
                            | _ ->
                                $"let ptr = {ptrField} in Array.init (int {lenField}) (fun i -> NativePtr.get ptr i)"
                        | _ -> failwith "bad"
                }
            self :: marshal t
        
        | h :: t ->
            let inline simple() =
                let self =
                    {
                        FrontendField = h
                        BackendFields = fun var -> [var, h]
                        PinFrontend = fun _ code -> code
                        ReadBackend = fun var -> List.head var
                    }
                self :: marshal t
                
            if h.Type.TypeName = "char" && h.Type.Annotation = Some "const*" then
                let varName = $"_{camelCase h.Name}Ptr"
                let self = 
                    {
                        FrontendField = { FieldDef.Name = h.Name; Type = { TypeName = "string"; Annotation = None }; Default = None; Optional = false; Length = None }
                        BackendFields = fun _var -> [varName, h]
                        PinFrontend = fun var code -> $"use {varName} = fixed (Encoding.UTF8.GetBytes({var}))" :: code
                        ReadBackend = fun var -> $"Marshal.PtrToStringAnsi(NativePtr.toNativeInt {List.head var})"
                    }
                self :: marshal t
            elif h.Type.TypeName = "bool" then
                let self = 
                    {
                        FrontendField = h
                        BackendFields = fun var -> [$"(if {var} then 1 else 0)", h]
                        PinFrontend = fun _ code -> code
                        ReadBackend = fun var -> $"({List.head var} <> 0)"
                    }
                self :: marshal t
            elif h.Type.TypeName = "uint32_t" then
                let self = 
                    {
                        FrontendField = { h with Type = { h.Type with TypeName = "int" } }
                        BackendFields = fun var -> [$"uint32({var})", h]
                        PinFrontend = fun _ code -> code
                        ReadBackend = fun var -> $"int({List.head var})"
                    }
                self :: marshal t
            else
                match tryResolveType h.Type with
                | Some (Object _) when Option.isNone h.Type.Annotation->
                    let self =
                        {
                            FrontendField = h
                            BackendFields = fun var -> [$"{var}.Handle", h]
                            PinFrontend = fun _ code -> code
                            ReadBackend = fun var -> $"new {frontendName false h.Type}({List.head var})"
                        }
                    self :: marshal t
                | Some (Struct s | CallbackInfo s) ->
                    let varName = $"_{camelCase h.Name}Ptr"
                    
                    let wrap (var : string) (code : list<string>) =
                        [
                            $"{var}.Pin(fun {varName} ->"
                            for c in code do
                                $"    {c}"
                            $")"
                        ]
                    
                    let access = 
                        match h.Type.Annotation with
                        | Some ("*" | "const*") ->
                            varName
                        | _ ->
                            $"NativePtr.read {varName}"
                    
                    let self =
                        {
                            FrontendField = h
                            BackendFields = fun var -> [access, h]
                            PinFrontend = wrap
                            ReadBackend = fun var ->
                                match h.Type.Annotation with
                                | Some ("*" | "const*") ->
                                    $"let m = NativePtr.toByRef {List.head var} in {frontendName false h.Type}.Read(&m)" //$"//TODO1 {var}"
                                | _ ->
                                    $"{frontendName false h.Type}.Read(&{List.head var})"
                        }
                    self :: marshal t
                | Some (Delegate d) ->
                    match t with
                    | h1 :: t ->
                        if h1.Name.ToLower() = "userdata"  then
                            let delName = $"_{camelCase h.Name}Del"
                            let varName = $"_{camelCase h.Name}Ptr"
                            let userDataName = $"_{camelCase h.Name}UserData"
                            let mm = marshal d.Args
                    
                            let backendArgDef =
                                mm |> List.collect (fun m ->
                                    m.BackendFields (camelCase m.FrontendField.Name)
                                    |> List.map snd
                                    |> List.map (fun f -> $"{camelCase f.Name}")
                                )
                                |> String.concat " "
                            
                            let realArgs =
                                mm
                                |> List.map (fun m -> $"_{camelCase m.FrontendField.Name}")
                                |> String.concat ", "
                            
                            let wrap (var : string) (code : list<string>) =
                                [
                                    
                                    yield $"let {delName} = WebGPU.Raw.{frontendName false h.Type}(fun {backendArgDef} ->"
                                    for a in mm do
                                        let name = camelCase a.FrontendField.Name
                                        let code = a.BackendFields name |> List.map (fun (_, f) -> camelCase f.Name) |> a.ReadBackend
                                        yield $"    let _{name} = {code}"
                                    yield $"    {var}.Invoke({realArgs})"
                                    yield $")"
                                    yield $"let struct({varName}, {userDataName}) = WebGPU.Raw.WebGPUCallbacks.Register({delName})"
                                    // yield $"let {gcName} = GCHandle.Alloc({delName})"
                                    // yield $"let {varName} = Marshal.GetFunctionPointerForDelegate({delName})"
                                    yield! code
                                ]
                            let self =
                                {
                                    FrontendField = h
                                    BackendFields = fun var -> [varName, h; userDataName, h1]
                                    PinFrontend = wrap
                                    ReadBackend = fun var -> $"//TODO2 {var}"
                                }
                            self :: marshal t
                            
                            
                        else
                            simple()
                    | _ ->
                        simple()
                | _ ->
                    simple()
        | [] ->
            []
      
    let print() =
        
        let b = System.Text.StringBuilder()
        let printfn fmt = fmt |> Printf.kprintf (fun str -> b.AppendLine str |> ignore)
        
        
        printfn "namespace rec WebGPU"
        printfn "open System"
        printfn "open System.Text"
        printfn "open System.Runtime.InteropServices"
        printfn "open Microsoft.FSharp.NativeInterop"
        printfn "#nowarn \"9\""
        
        let chainRootTypes =
            all |> Seq.collect (fun a ->
                match a with
                | Struct def -> def.ChainRoots
                | _ -> []
            ) |> Set.ofSeq
            
        for c in chainRootTypes do
            printfn "[<AllowNullLiteral>]"
            printfn $"type I{pascalCase c}Extension ="
            printfn "    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r"
            
        printfn "[<AbstractClass; Sealed>]"
        printfn "type private PinHelper() ="
        for c in chainRootTypes do
            printfn $"    static member inline PinNullable<'r>(x : I{pascalCase c}Extension, action : nativeint -> 'r) = "
            printfn $"        if isNull x then action 0n"
            printfn $"        else x.Pin action"
        
        
        for a in all do
            match a with
            | Enum e ->
                ()
            | Alias a ->
                printfn $"type {pascalCase a.Name} = {frontendName false a.Type}"
            | Delegate d ->
                let marshal = marshal d.Args
                let types =
                    match marshal with
                    | [] -> "unit"
                    | _ -> marshal |> List.map (fun m -> $"{camelCase m.FrontendField.Name} : {frontendName false m.FrontendField.Type}") |> String.concat " * "
                let ret = frontendName false d.Return
                printfn $"type {pascalCase d.Name} = delegate of {types} -> {ret}"
                
            | Native _ | CallbackInfo _ | Function _  ->
                ()
            | Struct s ->
                
                let marshal = marshal s.Fields
                
                let extensible =
                    Option.isSome s.Extensible && Set.contains s.Name chainRootTypes
                    
                let chained =
                    Option.isSome s.Chained 
                    
                match marshal with
                | [] ->
                    printfn $"type {pascalCase s.Name}() ="
                    printfn $"    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.{pascalCase s.Name}> -> 'r) : 'r = "
                    printfn $"        action (NativePtr.ofNativeInt 0n)"
                | _ ->
                    let fielddef =
                        marshal |> List.map (fun m ->
                            let f = m.FrontendField
                            let name = pascalCase f.Name
                            $"{name} : {frontendName false f.Type}"
                        )
                        
                    printfn $"type {pascalCase s.Name} = "
                    printfn "    {"
                    if extensible || chained then
                        let rootName = 
                            if chained then
                                match s.ChainRoots with
                                | [a] -> a
                                | roots -> List.head roots //failwithf "bad roots: %A" roots
                            else s.Name
                            
                        
                        printfn $"        Next : I{pascalCase rootName}Extension"
                    
                    for d in fielddef do
                        printfn $"        {d}"
                    printfn "    }"
                    
                    printfn $"    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.{pascalCase s.Name}> -> 'r) : 'r = "
                    //
                    // let realAndFields =
                    //     transforms
                    //     |> List.collect (fun (_,_,_,real) -> real)
                    //     |> List.zip s.Fields
                    //
                    
                    let marshal =
                        if chained then
                            {
                                BackendFields = fun _ ->
                                    [
                                        "nextInChain", { Name = "next in chain"; Type = { TypeName = "void*"; Annotation = None }; Default = None; Optional = false; Length = None }
                                        "sType", { Name = "s type"; Type = { TypeName = "s type"; Annotation = None }; Default = None; Optional = false; Length = None }
                                    ]
                                FrontendField = { Name = "Next"; Type = { TypeName = ""; Annotation = None }; Default = None; Optional = false; Length = None }
                                ReadBackend = fun a -> List.head a
                                PinFrontend = fun a b ->
                                    [
                                        yield $"PinHelper.PinNullable({a}, fun nextInChain ->"
                                        yield $"    let sType = SType.{pascalCase s.Name}"
                                        for b in b do yield $"    {b}"
                                        yield ")"
                                    ]
                            } :: marshal
                        elif extensible then
                            {
                                BackendFields = fun _ ->
                                    [
                                        "nextInChain", { Name = "next in chain"; Type = { TypeName = "void*"; Annotation = None }; Default = None; Optional = false; Length = None }
                                    ]
                                FrontendField = { Name = "Next"; Type = { TypeName = ""; Annotation = None }; Default = None; Optional = false; Length = None }
                                ReadBackend = fun a -> List.head a
                                PinFrontend = fun a b ->
                                    [
                                        yield $"PinHelper.PinNullable({a}, fun nextInChain ->"
                                        for b in b do yield $"    {b}"
                                        yield ")"
                                    ]
                            } :: marshal
                        else
                            marshal
                    
                    let body = 
                        pinStruct "this" marshal (fun pinned ->
                            let backendName = $"WebGPU.Raw.{pascalCase s.Name}"
                            let args = List.map fst pinned 
                            [
                                yield $"let mutable value ="
                                yield $"    new {backendName}("
                                for i, a in List.indexed args do
                                    if i < args.Length - 1 then
                                        yield $"        {a},"
                                    else
                                        yield $"        {a}"
                                yield $"    )"
                                yield "use ptr = fixed &value"
                                yield "action ptr"
                            ]
                        )
                        
                    for b in body do
                        printfn "        %s" b
                        
                    
                    for r in s.ChainRoots do
                        printfn $"    interface I{pascalCase r}Extension with"
                        printfn $"        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))"
                    
                        
                    // printfn $"        use ptr ="
                    // printfn "            fixed [| {"
                    // for (f, real) in realAndFields do
                    //     
                    //     printfn "                WebGPU.Raw.%s.%s = %s" (pascalCase s.Name) (pascalCase f.Name) real
                    // printfn "            } |]"
                    // printfn "        action ptr"
                    () // TODO
                   
                    
                printfn $"    interface WebGPU.Raw.IPinnable<WebGPU.Raw.{pascalCase s.Name}> with"
                printfn $"        member x.Pin(action) = x.Pin(action)"
                    
                printfn $"    static member Read(backend : byref<WebGPU.Raw.{pascalCase s.Name}>) = "
                
                match marshal with
                | [] ->
                    printfn $"        {pascalCase s.Name}()"
                | _ -> 
                    let values =
                        marshal |> List.map (fun m ->
                            let fields = m.BackendFields (pascalCase m.FrontendField.Name) |> List.map (fun (_, f) -> $"backend.{pascalCase f.Name}")
                            pascalCase m.FrontendField.Name, m.ReadBackend fields
                        )
                    
                    printfn "        {"
                    if extensible || chained then
                        printfn "            Next = null"
                    for (name, value) in values do
                        
                        printfn $"            {name} = {value}"
                    printfn "        }"
                
            | Object o ->
                printfn $"type {pascalCase o.Name} internal(handle : nativeint) ="
                printfn "    member x.Handle = handle"
                
                printfn $"    static member Null = {pascalCase o.Name}(0n)"
                
                let (|Getter|_|) (m : FunctionDef) =
                    match m.Args with
                    | [arg] when arg.Type.Annotation = Some "*" && m.Name.StartsWith "get " ->
                        let arg1 = { arg with Type = { arg.Type with Annotation = None } }
                        match tryResolveType m.Return with
                        | Some (Native { Name = "bool" }) ->
                            Some (true, arg1)
                        | Some (Native { Name = "void" }) ->
                            Some (false, arg1)
                        | _ ->
                            None
                    | _ ->
                        None
                
                for m in o.Methods do
                    match m with
                    | Getter(hasStatus, arg) ->
                        let ret = marshal [arg] |> List.head
                        let methName = o.Name + " " + m.Name
                        let read = ret.ReadBackend ["res"]
                        
                        let propertyName =
                            if m.Name.StartsWith "get " then m.Name.Substring 4
                            else m.Name
                        
                        printfn $"    member _.{pascalCase propertyName} : {frontendName false ret.FrontendField.Type} ="
                        printfn "        let mutable res = Unchecked.defaultof<_>"
                        printfn "        let ptr = fixed &res"
                        if hasStatus then
                            printfn $"        let status = WebGPU.Raw.WebGPU.{pascalCase methName}(handle, ptr)"
                            printfn $"        if status = 0 then failwith \"{pascalCase m.Name} failed\""
                        else
                            printfn $"        WebGPU.Raw.WebGPU.{pascalCase methName}(handle, ptr)"
                            
                        printfn $"        {read}"
                    | _ -> 
                        let ret =
                            marshal [{ Name = "result"; Type = m.Return; Default = None; Optional = false; Length = None }]
                            |> List.head
                            
                        let mm = marshal m.Args

                        let argdef =
                            mm |> List.map (fun m ->
                                $"{camelCase m.FrontendField.Name} : {frontendName false m.FrontendField.Type}" 
                            )
                            |> String.concat ", "
                                   
                        let methName = o.Name + " " + m.Name
                        printfn $"    member _.{pascalCase m.Name}({argdef}) : {frontendName false ret.FrontendField.Type} ="
                        
                        let body = 
                            pinArgs mm (fun pinned ->
                                let arguse =
                                    "handle" :: List.map fst pinned
                                    |> String.concat ", "
                                
                                
                                let call = $"WebGPU.Raw.WebGPU.{pascalCase methName}({arguse})"
                                
                                [
                                    ret.ReadBackend [call]
                                ]
                                //
                                // match tryResolveType m.Return with
                                // | Some (Object _) ->
                                //     [$"new {frontendName false m.Return}({call})"]
                                // | _ ->
                                //     [call]
                            )
                        
                        for b in body do
                            printfn "        %s" b
                
                let hasDestroy = o.Methods |> List.exists (fun m -> m.Name = "destroy")
                if hasDestroy then
                    printfn "    member x.Dispose() = x.Destroy()"
                    printfn "    interface System.IDisposable with"
                    printfn "        member x.Dispose() = x.Dispose()"
                
        File.WriteAllText(Path.Combine(__SOURCE_DIRECTORY__, "Frontend.fs"), b.ToString())

        
Enums.print()
Native.print()
RawWrapper.print()
Frontend.print()
//     
// let referenced = HashSet()
// for a in all do
//     referenced.UnionWith a.ReferencedTypes
//     
// let annotations = referenced |> Seq.choose (fun t -> t.Annotation) |> Set.ofSeq
//
// for a in annotations do
//     printfn "%s" a
//     
// for a in referenced do
//     if not (table.ContainsKey a.TypeName) then
//         failwithf "MISSING TYPE: %A" a.TypeName
//     
//         
// let countPointers (a : option<string>) =
//     match a with
//     | Some a -> a.ToCharArray() |> Array.sumBy(fun c -> if c = '*' then 1 else 0)
//     | None -> 0
//     

//               
//         
// let nativeTypeName (t : TypeRef) (annotation : option<string>) =
//     let mutable res = 
//         match t.TypeName with
//         | "size_t" -> "nativeint"
//         | "void *" -> "nativeint"
//         | "string view" -> "nativeptr<byte>"
//         | _ -> pascalCase t.TypeName
//     let ptr = countPointers annotation
//     for i in 1 .. ptr do
//         res <- sprintf "nativeptr<%s>" res
//     res
//     
//
//     
// let printStruct (s : StructDef) =
//     printfn "[<Struct; StructLayout(LayoutKind.Sequential)>]"
//     printfn "type %s =" s.Name
//     printfn "    {"
//     for f in s.Fields do
//         let typ = nativeTypeName f.Type f.Annotation
//         let name = f.Name
//         
//         printfn "        %s : %s" name typ
//     printfn "    }"
//     printfn ""
//     
// for KeyValue(name, ptr) in functionPointers do
//     let args =
//         ptr.Args |> List.map (fun f ->
//             let typ = nativeTypeName f.Type f.Annotation
//             let name = f.Name
//             sprintf "%s : %s" name typ
//         ) |> String.concat " * "
//     
//     printfn "type %s = delegate of %s -> unit" (pascalCase name) args
//     
//     
//         
// printStruct  structs.["DeviceDescriptor"]
//



