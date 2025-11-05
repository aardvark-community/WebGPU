#!/usr/bin/env dotnet fsi

#r "nuget: FSharp.Data"

open System
open System.IO
open System.Text
open System.Collections.Generic
open FSharp.Data

// Reuse type definitions and parsing logic from Generator.fsx
#load "Generator.fsx"
open Generator

module EmscriptenBindings =

    /// Convert WebGPU type to C type name
    let rec cTypeName (t : TypeRef) =
        let def = table.[t.TypeName]

        let baseType =
            match def with
            | Object o -> "int" // Objects represented as integer handles
            | Enum e -> "int" // Enums as integers
            | Delegate d -> "void*" // Function pointers
            | Alias a -> cTypeName a.Type
            | Struct a -> "WGPU" + pascalCase a.Name + "*"
            | Function _ -> failwith "not a type"
            | CallbackInfo c -> "WGPU" + pascalCase c.Name + "*"
            | Native n ->
                match n.Name with
                | "int8_t" | "uint8_t" | "int16_t" | "uint16_t"
                | "int32_t" | "uint32_t" | "int" -> "int"
                | "int64_t" | "uint64_t" -> "int64_t"
                | "void" -> "void"
                | "bool" -> "int"
                | "char" -> "char"
                | "float" -> "float"
                | "double" -> "double"
                | "size_t" -> "size_t"
                | "void *" | "void const *" -> "void*"
                | _ -> n.Name

        match t.Annotation with
        | None -> baseType
        | Some "*" when baseType = "int" -> "int" // Object pointers stay as int
        | Some "*" -> baseType + "*"
        | Some "const*" -> "const " + baseType + "*"
        | Some "const*const*" -> "const " + baseType + "* const*"
        | _ -> baseType

    /// Check if a type is an object handle
    let isObjectHandle (t : TypeRef) =
        match table.[t.TypeName] with
        | Object _ -> true
        | _ -> false

    /// Check if a type is a struct
    let isStruct (t : TypeRef) =
        match table.[t.TypeName] with
        | Struct _ -> true
        | _ -> false

    /// Get JavaScript type for a field
    let jsTypeName (t : TypeRef) =
        let def = table.[t.TypeName]
        match def with
        | Object _ -> "handle"
        | Enum _ -> "i32"
        | Native n ->
            match n.Name with
            | "int8_t" -> "i8"
            | "uint8_t" -> "u8"
            | "int16_t" -> "i16"
            | "uint16_t" -> "u16"
            | "int32_t" | "int" -> "i32"
            | "uint32_t" -> "u32"
            | "int64_t" -> "i64"
            | "uint64_t" -> "u64"
            | "bool" -> "i32"
            | "float" -> "float"
            | "double" -> "double"
            | "size_t" -> "size_t"
            | "void *" | "void const *" -> "*"
            | _ -> "*"
        | Struct _ -> "struct"
        | _ -> "*"

    /// Map C method name to JavaScript method name
    /// e.g., "create buffer" -> "createBuffer"
    /// e.g., "get size" -> "size" (property access)
    /// e.g., "set label" -> "label" (property setter)
    let toJsMethodName (methodName : string) =
        let parts = methodName.Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
        if parts.Length = 0 then ""
        else
            match parts.[0].ToLowerInvariant() with
            | "get" when parts.Length = 2 ->
                // get size -> size property
                camelCase parts.[1]
            | "set" when parts.Length = 2 ->
                // set label -> label property
                camelCase parts.[1]
            | _ ->
                // create buffer -> createBuffer
                parts |> Array.map camelCase |> String.concat ""

module StructMarshalerGen =
    open EmscriptenBindings

    let rec generateFieldReader (fieldName : string) (fieldType : TypeRef) (offset : string) =
        let def = table.[fieldType.TypeName]

        match fieldType.Annotation with
        | None ->
            match def with
            | Object _ ->
                sprintf "WebGPUEm.getObject({{{ makeGetValue('ptr', %s, 'i32') }}})" offset
            | Enum _ ->
                sprintf "{{{ makeGetValue('ptr', %s, 'i32') }}}" offset
            | Native n ->
                match n.Name with
                | "bool" ->
                    sprintf "!!{{{ makeGetValue('ptr', %s, 'i32') }}}" offset
                | "int8_t" -> sprintf "{{{ makeGetValue('ptr', %s, 'i8') }}}" offset
                | "uint8_t" -> sprintf "{{{ makeGetValue('ptr', %s, 'u8') }}}" offset
                | "int16_t" -> sprintf "{{{ makeGetValue('ptr', %s, 'i16') }}}" offset
                | "uint16_t" -> sprintf "{{{ makeGetValue('ptr', %s, 'u16') }}}" offset
                | "int32_t" | "int" -> sprintf "{{{ makeGetValue('ptr', %s, 'i32') }}}" offset
                | "uint32_t" -> sprintf "{{{ makeGetValue('ptr', %s, 'u32') }}}" offset
                | "size_t" -> sprintf "{{{ makeGetValue('ptr', %s, 'size_t') }}}" offset
                | "float" -> sprintf "{{{ makeGetValue('ptr', %s, 'float') }}}" offset
                | "double" -> sprintf "{{{ makeGetValue('ptr', %s, 'double') }}}" offset
                | "int64_t" | "uint64_t" ->
                    // Read as two 32-bit values
                    sprintf "({{{ makeGetValue('ptr', %s, 'i32') }}} + {{{ makeGetValue('ptr', '%s + 4', 'i32') }}} * 0x100000000)" offset offset
                | _ ->
                    sprintf "{{{ makeGetValue('ptr', %s, '*') }}}" offset
            | Struct s ->
                sprintf "WebGPUStructMarshalers.read%s(ptr + %s)" (pascalCase s.Name) offset
            | _ ->
                sprintf "{{{ makeGetValue('ptr', %s, '*') }}}" offset
        | Some "*" | Some "const*" ->
            match def with
            | Native n when n.Name = "char" ->
                // String pointer
                sprintf "WebGPUEm.readString({{{ makeGetValue('ptr', %s, '*') }}})" offset
            | Object _ ->
                // Object handle
                sprintf "WebGPUEm.getObject({{{ makeGetValue('ptr', %s, 'i32') }}})" offset
            | Struct s ->
                // Pointer to struct
                sprintf "WebGPUStructMarshalers.read%s({{{ makeGetValue('ptr', %s, '*') }}})" (pascalCase s.Name) offset
            | _ ->
                sprintf "{{{ makeGetValue('ptr', %s, '*') }}}" offset
        | Some "const*const*" ->
            sprintf "{{{ makeGetValue('ptr', %s, '*') }}}" offset
        | _ ->
            sprintf "{{{ makeGetValue('ptr', %s, '*') }}}" offset

    let generateStructMarshaler (s : StructDef) =
        let b = StringBuilder()
        let printfn fmt = Printf.kprintf (fun str -> b.AppendLine(str) |> ignore) fmt

        printfn "    read%s: function(ptr) {" (pascalCase s.Name)
        printfn "      if (!ptr) return undefined;"
        printfn "      var obj = {};"
        printfn "      var offset = 0;"
        printfn ""

        // Generate field readers
        for field in s.Fields do
            let jsFieldName = camelCase field.Name
            let fieldReader = generateFieldReader field.Name field.Type "offset"

            // Check if optional
            if field.Optional then
                printfn "      var %sPtr = {{{ makeGetValue('ptr', 'offset', '*') }}};" jsFieldName
                printfn "      if (%sPtr) {" jsFieldName
                printfn "        obj.%s = %s;" jsFieldName fieldReader
                printfn "      }"
            else
                printfn "      obj.%s = %s;" jsFieldName fieldReader

            // Advance offset (simplified - assumes all pointers are 4 bytes for WASM32)
            let fieldSize =
                match jsTypeName field.Type with
                | "i8" | "u8" -> "1"
                | "i16" | "u16" -> "2"
                | "i32" | "u32" | "float" | "handle" -> "4"
                | "i64" | "u64" | "double" -> "8"
                | "*" -> "{{{ POINTER_SIZE }}}"
                | "struct" ->
                    match table.[field.Type.TypeName] with
                    | Struct s -> sprintf "STRUCT_SIZE_%s" (pascalCase s.Name)
                    | _ -> "4"
                | _ -> "4"

            printfn "      offset += %s;" fieldSize

            // Add alignment padding for next field
            printfn "      offset = (offset + 3) & ~3; // Align to 4 bytes"
            printfn ""

        printfn "      return obj;"
        printfn "    },"
        printfn ""

        b.ToString()

    let generate (fileName : string) =
        let b = StringBuilder()
        let printfn fmt = Printf.kprintf (fun str -> b.AppendLine(str) |> ignore) fmt

        printfn "/**"
        printfn " * Auto-generated WebGPU Struct Marshalers"
        printfn " * Generated from dawn.json"
        printfn " */"
        printfn ""
        printfn "var WebGPUStructMarshalers = {"
        printfn "  // Helper functions"
        printfn "  readString: function(ptr) {"
        printfn "    if (!ptr) return undefined;"
        printfn "    return UTF8ToString(ptr);"
        printfn "  },"
        printfn ""
        printfn "  readStringView: function(ptr) {"
        printfn "    if (!ptr) return undefined;"
        printfn "    var dataPtr = {{{ makeGetValue('ptr', 0, '*') }}};"
        printfn "    var length = {{{ makeGetValue('ptr', 4, 'size_t') }}};"
        printfn "    if (!dataPtr) return length === 0 ? '' : undefined;"
        printfn "    return UTF8ToString(dataPtr, length);"
        printfn "  },"
        printfn ""

        // Generate marshalers for all structs
        for a in all do
            match a with
            | Struct s when isEmscripten s.Tags ->
                printfn "%s" (generateStructMarshaler s)
            | _ -> ()

        printfn "};"
        printfn ""
        printfn "if (typeof module !== 'undefined' && module.exports) {"
        printfn "  module.exports = WebGPUStructMarshalers;"
        printfn "}"

        File.WriteAllText(fileName, b.ToString())

module JsLibraryGen =
    open EmscriptenBindings

    /// Determine the pattern of a method
    type MethodPattern =
        | Create    // Returns a new object handle
        | Get       // Returns a property value
        | Set       // Sets a property (void return)
        | Action    // Performs an action (void return)
        | Query     // Returns a value (not a property)
        | AsyncOp   // Async operation with callback

    let getMethodPattern (m : FunctionDef) (objName : string option) =
        let name = m.Name.ToLowerInvariant()

        // Check for callbacks in args - indicates async
        let hasCallback = m.Args |> List.exists (fun arg ->
            match table.[arg.Type.TypeName] with
            | Delegate _ -> true
            | CallbackInfo _ -> true
            | _ -> false
        )

        if hasCallback then AsyncOp
        elif name.Contains("create") || name.Contains("import") then Create
        elif name.StartsWith("get ") then Get
        elif name.StartsWith("set ") then Set
        elif m.Return.TypeName = "void" then Action
        else Query

    /// Generate makeDynCall signature for a callback
    let rec getCallbackSignature (callbackType : TypeRef) =
        match table.[callbackType.TypeName] with
        | Delegate d ->
            // Build signature: return type + argument types
            let returnSig =
                match d.Return.TypeName with
                | "void" -> "v"
                | _ -> "i" // Most returns are status codes (int)

            let argSigs =
                d.Args
                |> List.map (fun arg ->
                    match table.[arg.Type.TypeName] with
                    | Native n ->
                        match n.Name with
                        | "int8_t" | "uint8_t" | "int16_t" | "uint16_t"
                        | "int32_t" | "uint32_t" | "int" | "bool" -> "i"
                        | "int64_t" | "uint64_t" -> "j" // BigInt
                        | "float" -> "f"
                        | "double" -> "d"
                        | "void *" | "void const *" | "size_t" -> "i"
                        | _ -> "i"
                    | Object _ -> "i" // Handle
                    | Enum _ -> "i"
                    | _ -> "i"
                )
                |> String.concat ""

            returnSig + argSigs

        | CallbackInfo c ->
            // CallbackInfo has a callback field - need to find it
            let callbackField = c.Fields |> List.tryFind (fun f ->
                match table.[f.Type.TypeName] with
                | Delegate _ -> true
                | _ -> false
            )
            match callbackField with
            | Some field -> getCallbackSignature field.Type
            | None -> "v" // Fallback

        | _ -> "v" // Fallback

    let generateJsFunction (objName : string option) (m : FunctionDef) =
        let b = StringBuilder()
        let printfn fmt = Printf.kprintf (fun str -> b.AppendLine(str) |> ignore) fmt

        let fullName =
            match objName with
            | Some o -> o + " " + m.Name
            | None -> m.Name

        let funcName = "_js_wgpu_" + (camelCase fullName).Replace(" ", "_")
        let jsMethodName = toJsMethodName m.Name
        let pattern = getMethodPattern m objName

        // Generate function signature
        printfn "  %s__deps: ['$WebGPUEm']," funcName

        let argNames = m.Args |> List.map (fun a -> camelCase a.Name) |> String.concat ", "
        printfn "  %s: function(%s) {" funcName argNames

        // Get the object if this is a method
        match objName with
        | Some o ->
            let selfArg = m.Args |> List.tryHead
            match selfArg with
            | Some self ->
                printfn "    var obj = WebGPUEm.getObject(%s);" (camelCase self.Name)
                printfn "    if (!obj) {"
                match pattern with
                | Create | Query -> printfn "      return 0;"
                | _ -> printfn "      return;"
                printfn "    }"
            | None -> ()
        | None ->
            // Standalone function - may need to access GPU
            if fullName.Contains("instance") then
                printfn "    if (!WebGPUEm.ensureGPU()) {"
                printfn "      console.error('WebGPU not supported');"
                match pattern with
                | Create | Query -> printfn "      return 0;"
                | _ -> printfn "      return;"
                printfn "    }"

        printfn ""

        // Marshal descriptor arguments
        let descriptorArgs = m.Args |> List.filter (fun arg ->
            match table.[arg.Type.TypeName] with
            | Struct s when arg.Type.Annotation = Some "*" || arg.Type.Annotation = Some "const*" ->
                // Check if it's a CallbackInfo struct
                match table.[arg.Type.TypeName] with
                | CallbackInfo _ -> false // Handle separately
                | _ -> true
            | _ -> false
        )

        for arg in descriptorArgs do
            match table.[arg.Type.TypeName] with
            | Struct s ->
                let argName = camelCase arg.Name
                printfn "    var %sObj;" argName
                printfn "    if (%s) {" argName
                printfn "      %sObj = WebGPUStructMarshalers.read%s(%s);" argName (pascalCase s.Name) argName
                printfn "    }"
                printfn ""
            | _ -> ()

        // Generate the actual call based on pattern
        match pattern with
        | AsyncOp ->
            // Find callback and userdata args
            let callbackIdx = m.Args |> List.tryFindIndex (fun arg ->
                match table.[arg.Type.TypeName] with
                | Delegate _ | CallbackInfo _ -> true
                | _ -> false
            )

            match callbackIdx with
            | Some idx ->
                let callbackArg = m.Args.[idx]
                let userdataArg =
                    if idx + 1 < m.Args.Length then Some m.Args.[idx + 1]
                    else None

                let callbackName = camelCase callbackArg.Name
                let userdataName = userdataArg |> Option.map (fun a -> camelCase a.Name) |> Option.defaultValue "0"

                // Determine if it's CallbackInfo or plain callback
                match table.[callbackArg.Type.TypeName] with
                | CallbackInfo c ->
                    // CallbackInfo struct - extract callback and userdata from struct
                    printfn "    var callbackInfo = WebGPUStructMarshalers.read%s(%s);" (pascalCase c.Name) callbackName
                    printfn "    // Extract callback function and userdata from CallbackInfo struct"
                    printfn "    // TODO: Read callback pointer and userdata from struct"

                | Delegate d ->
                    // Plain callback function pointer
                    let signature = getCallbackSignature callbackArg.Type

                    // Build argument list for JavaScript call (exclude self, callback, userdata)
                    let jsArgs =
                        m.Args
                        |> List.indexed
                        |> List.filter (fun (i, _) -> i <> 0 && i <> idx && (match userdataArg with Some _ -> i <> idx + 1 | None -> true))
                        |> List.map (fun (_, arg) ->
                            let argName = camelCase arg.Name
                            match table.[arg.Type.TypeName] with
                            | Struct _ when arg.Type.Annotation = Some "*" || arg.Type.Annotation = Some "const*" ->
                                argName + "Obj"
                            | Object _ when arg.Type.Annotation = Some "*" ->
                                "WebGPUEm.getObject(" + argName + ")"
                            | _ -> argName
                        )

                    let jsArgStr = if jsArgs.IsEmpty then "" else jsArgs |> String.concat ", "

                    printfn "    obj.%s(%s).then(function(result) {" jsMethodName jsArgStr
                    printfn "      if (%s) {" callbackName

                    // Determine what to pass to callback based on return type
                    match d.Return.TypeName with
                    | "void" ->
                        // Callback signature like: void callback(status, userdata)
                        if d.Args.Length = 2 then
                            printfn "        var status = 0; // Success"
                            printfn "        {{{ makeDynCall('%s', '%s') }}}(status, %s);" signature callbackName userdataName
                        else
                            // Callback signature like: void callback(device, status, userdata)
                            printfn "        var handle = WebGPUEm.createHandle(result);"
                            printfn "        var status = 0; // Success"
                            printfn "        {{{ makeDynCall('%s', '%s') }}}(handle, status, %s);" signature callbackName userdataName
                    | _ ->
                        // Other return types
                        printfn "        var handle = WebGPUEm.createHandle(result);"
                        printfn "        {{{ makeDynCall('%s', '%s') }}}(handle, %s);" signature callbackName userdataName

                    printfn "      }"
                    printfn "    }).catch(function(err) {"
                    printfn "      console.error('%s failed:', err);" fullName
                    printfn "      if (%s) {" callbackName

                    // Call callback with error status
                    if d.Args.Length = 2 then
                        printfn "        var status = 1; // Error"
                        printfn "        {{{ makeDynCall('%s', '%s') }}}(status, %s);" signature callbackName userdataName
                    else
                        printfn "        var handle = 0; // Null"
                        printfn "        var status = 1; // Error"
                        printfn "        {{{ makeDynCall('%s', '%s') }}}(handle, status, %s);" signature callbackName userdataName

                    printfn "      }"
                    printfn "    });"

                | _ ->
                    printfn "    // Unknown callback type"

            | None ->
                printfn "    // Async operation but no callback found"

        | Create ->
            let returnType = table.[m.Return.TypeName]
            match returnType with
            | Object _ ->
                printfn "    var result = obj.%s(" jsMethodName
                // Add arguments
                let nonSelfArgs = m.Args |> List.skip 1
                for i, arg in nonSelfArgs |> List.indexed do
                    let argName = camelCase arg.Name
                    let argValue =
                        match table.[arg.Type.TypeName] with
                        | Struct _ when arg.Type.Annotation = Some "*" || arg.Type.Annotation = Some "const*" ->
                            argName + "Obj"
                        | Object _ when arg.Type.Annotation = Some "*" ->
                            "WebGPUEm.getObject(" + argName + ")"
                        | _ -> argName

                    if i = nonSelfArgs.Length - 1 then
                        printfn "      %s" argValue
                    else
                        printfn "      %s," argValue
                printfn "    );"
                printfn "    return WebGPUEm.createHandle(result);"
            | _ ->
                printfn "    // TODO: Implement %s" fullName
                printfn "    return 0;"

        | Get ->
            // Property getter
            let propName = m.Name.Replace("get ", "")
            printfn "    return obj.%s;" (camelCase propName)

        | Set ->
            // Property setter
            let propName = m.Name.Replace("set ", "")
            let valueArg = m.Args |> List.last
            printfn "    obj.%s = %s;" (camelCase propName) (camelCase valueArg.Name)

        | Action ->
            // Method call with no return
            printfn "    obj.%s(" jsMethodName
            let nonSelfArgs = if objName.IsSome then m.Args |> List.skip 1 else m.Args
            for i, arg in nonSelfArgs |> List.indexed do
                let argName = camelCase arg.Name
                let argValue =
                    match table.[arg.Type.TypeName] with
                    | Struct _ when arg.Type.Annotation = Some "*" || arg.Type.Annotation = Some "const*" ->
                        argName + "Obj"
                    | Object _ when arg.Type.Annotation = Some "*" ->
                        "WebGPUEm.getObject(" + argName + ")"
                    | _ -> argName

                if i = nonSelfArgs.Length - 1 then
                    printfn "      %s" argValue
                else
                    printfn "      %s," argValue
            printfn "    );"

        | Query ->
            // Method call with return value
            printfn "    var result = obj.%s(" jsMethodName
            let nonSelfArgs = if objName.IsSome then m.Args |> List.skip 1 else m.Args
            for i, arg in nonSelfArgs |> List.indexed do
                let argName = camelCase arg.Name
                if i = nonSelfArgs.Length - 1 then
                    printfn "      %s" argName
                else
                    printfn "      %s," argName
            printfn "    );"

            // Return appropriate type
            match table.[m.Return.TypeName] with
            | Object _ -> printfn "    return WebGPUEm.createHandle(result);"
            | _ -> printfn "    return result;"

        printfn "  },"
        printfn ""

        b.ToString()

    let generate (fileName : string) =
        let b = StringBuilder()
        let printfn fmt = Printf.kprintf (fun str -> b.AppendLine(str) |> ignore) fmt

        printfn "/**"
        printfn " * Auto-generated Emscripten WebGPU Library"
        printfn " * Generated from dawn.json"
        printfn " */"
        printfn ""
        printfn "var LibraryWebGPUEmscripten = {"
        printfn "  $WebGPUEm: {"
        printfn "    nextHandle: 1,"
        printfn "    objects: {},"
        printfn "    "
        printfn "    createHandle: function(obj) {"
        printfn "      if (!obj) return 0;"
        printfn "      var handle = WebGPUEm.nextHandle++;"
        printfn "      WebGPUEm.objects[handle] = obj;"
        printfn "      return handle;"
        printfn "    },"
        printfn "    "
        printfn "    getObject: function(handle) {"
        printfn "      if (handle === 0) return null;"
        printfn "      return WebGPUEm.objects[handle] || null;"
        printfn "    },"
        printfn "    "
        printfn "    releaseHandle: function(handle) {"
        printfn "      if (handle !== 0) {"
        printfn "        delete WebGPUEm.objects[handle];"
        printfn "      }"
        printfn "    },"
        printfn "    "
        printfn "    readString: function(ptr) {"
        printfn "      if (!ptr) return undefined;"
        printfn "      return UTF8ToString(ptr);"
        printfn "    },"
        printfn "    "
        printfn "    gpu: null,"
        printfn "    "
        printfn "    ensureGPU: function() {"
        printfn "      if (!WebGPUEm.gpu && navigator.gpu) {"
        printfn "        WebGPUEm.gpu = navigator.gpu;"
        printfn "      }"
        printfn "      return WebGPUEm.gpu !== null;"
        printfn "    }"
        printfn "  },"
        printfn ""

        // Generate all functions
        for a in all do
            let functions =
                match a with
                | Function f when isEmscripten f.Tags ->
                    [(None, f)]
                | Object o when isEmscripten o.Tags ->
                    o.Methods
                    |> List.filter (fun m -> isEmscripten m.Tags)
                    |> List.map (fun m ->
                        // Add self parameter
                        let args = { Name = "self"; Tags = []; Type = { TypeName = o.Name; Annotation = None };
                                    Default = None; Optional = false; Length = None } :: m.Args
                        (Some o.Name, { m with Args = args })
                    )
                | _ -> []

            for (objName, func) in functions do
                printfn "%s" (generateJsFunction objName func)

        // Reference counting (no-ops)
        printfn "  _js_wgpu_object_reference__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_object_reference: function(handle) {"
        printfn "    // No-op: JavaScript uses garbage collection"
        printfn "  },"
        printfn ""
        printfn "  _js_wgpu_object_release__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_object_release: function(handle) {"
        printfn "    WebGPUEm.releaseHandle(handle);"
        printfn "  },"
        printfn ""

        printfn "};"
        printfn ""
        printfn "autoAddDeps(LibraryWebGPUEmscripten, '$WebGPUEm');"
        printfn "mergeInto(LibraryManager.library, LibraryWebGPUEmscripten);"

        File.WriteAllText(fileName, b.ToString())

module CHeaderGen =
    open EmscriptenBindings

    let generate (fileName : string) =
        let b = StringBuilder()
        let printfn fmt = Printf.kprintf (fun str -> b.AppendLine(str) |> ignore) fmt

        printfn "#ifndef WEBGPU_EMSCRIPTEN_H"
        printfn "#define WEBGPU_EMSCRIPTEN_H"
        printfn ""
        printfn "#include <stdint.h>"
        printfn "#include <stddef.h>"
        printfn ""
        printfn "#ifdef __cplusplus"
        printfn "extern \"C\" {"
        printfn "#endif"
        printfn ""
        printfn "// WebGPU object handles (represented as integers)"

        // Generate typedefs for object handles
        for a in all do
            match a with
            | Object o when isEmscripten o.Tags ->
                printfn "typedef int WGPU%s;" (pascalCase o.Name)
            | _ -> ()

        printfn ""
        printfn "// Forward declarations for structs"
        for a in all do
            match a with
            | Struct s when isEmscripten s.Tags ->
                printfn "typedef struct WGPU%s WGPU%s;" (pascalCase s.Name) (pascalCase s.Name)
            | _ -> ()

        printfn ""
        printfn "// Enum definitions"
        for a in all do
            match a with
            | Enum e ->
                printfn "typedef enum WGPU%s {" (pascalCase e.Name)
                for (name, value) in e.Values do
                    printfn "    WGPU%s_%s = %d," (pascalCase e.Name) (pascalCase name) value
                printfn "} WGPU%s;" (pascalCase e.Name)
                printfn ""
            | _ -> ()

        printfn ""
        printfn "// Struct definitions"
        for a in all do
            match a with
            | Struct s when isEmscripten s.Tags ->
                printfn "struct WGPU%s {" (pascalCase s.Name)
                for field in s.Fields do
                    printfn "    %s %s;" (cTypeName field.Type) (camelCase field.Name)
                printfn "};"
                printfn ""
            | _ -> ()

        printfn ""
        printfn "// Function declarations"

        // Generate function declarations
        for a in all do
            let functions =
                match a with
                | Function f when isEmscripten f.Tags -> [f]
                | Object o when isEmscripten o.Tags ->
                    o.Methods
                    |> List.filter (fun m -> isEmscripten m.Tags)
                    |> List.map (fun m ->
                        let name = o.Name + " " + m.Name
                        let args =
                            { Name = "self"; Tags = []; Type = { TypeName = o.Name; Annotation = None };
                              Default = None; Optional = false; Length = None } :: m.Args
                        { m with Name = name; Args = args }
                    )
                | _ -> []

            for f in functions do
                let returnType = cTypeName f.Return
                let funcName = "wgpuEm" + pascalCase f.Name

                if FunctionDef.isBadWasmFunction f then
                    // Generate struct for packed arguments
                    printfn "typedef struct {"
                    for arg in f.Args do
                        printfn "    %s %s;" (cTypeName arg.Type) (pascalCase arg.Name)
                    printfn "} WGPU%sArgs;" (pascalCase f.Name)
                    printfn "%s %s(const WGPU%sArgs* args);" returnType funcName (pascalCase f.Name)
                else
                    let argList =
                        f.Args
                        |> List.map (fun arg -> sprintf "%s %s" (cTypeName arg.Type) (camelCase arg.Name))
                        |> String.concat ", "
                    let argList = if argList = "" then "void" else argList
                    printfn "%s %s(%s);" returnType funcName argList
                printfn ""

        printfn "#ifdef __cplusplus"
        printfn "}"
        printfn "#endif"
        printfn ""
        printfn "#endif // WEBGPU_EMSCRIPTEN_H"

        File.WriteAllText(fileName, b.ToString())

module CImplGen =
    open EmscriptenBindings

    let generate (fileName : string) =
        let b = StringBuilder()
        let printfn fmt = Printf.kprintf (fun str -> b.AppendLine(str) |> ignore) fmt

        printfn "#include \"webgpu_emscripten.h\""
        printfn "#include <emscripten.h>"
        printfn ""
        printfn "// JavaScript library function declarations"
        printfn "// These are implemented in library_webgpu_emscripten.js"
        printfn ""

        // Generate implementations
        for a in all do
            let functions =
                match a with
                | Function f when isEmscripten f.Tags -> [f]
                | Object o when isEmscripten o.Tags ->
                    o.Methods
                    |> List.filter (fun m -> isEmscripten m.Tags)
                    |> List.map (fun m ->
                        let name = o.Name + " " + m.Name
                        let args =
                            { Name = "self"; Tags = []; Type = { TypeName = o.Name; Annotation = None };
                              Default = None; Optional = false; Length = None } :: m.Args
                        { m with Name = name; Args = args }
                    )
                | _ -> []

            for f in functions do
                let returnType = cTypeName f.Return
                let funcName = "wgpuEm" + pascalCase f.Name
                let jsFuncName = "_js_wgpu_" + (camelCase f.Name).Replace(" ", "_")

                if FunctionDef.isBadWasmFunction f then
                    printfn "extern %s %s(const WGPU%sArgs* args);" returnType jsFuncName (pascalCase f.Name)
                    printfn "%s %s(const WGPU%sArgs* args) {" returnType funcName (pascalCase f.Name)
                    let argRefs =
                        f.Args
                        |> List.map (fun a -> sprintf "args->%s" (pascalCase a.Name))
                        |> String.concat ", "
                    if returnType = "void" then
                        printfn "    %s(%s);" jsFuncName argRefs
                    else
                        printfn "    return %s(%s);" jsFuncName argRefs
                    printfn "}"
                else
                    let argList =
                        f.Args
                        |> List.map (fun arg -> sprintf "%s %s" (cTypeName arg.Type) (camelCase arg.Name))
                        |> String.concat ", "
                    let argList = if argList = "" then "void" else argList

                    let argRefs =
                        f.Args
                        |> List.map (fun a -> camelCase a.Name)
                        |> String.concat ", "

                    printfn "extern %s %s(%s);" returnType jsFuncName argList
                    printfn "%s %s(%s) {" returnType funcName argList
                    if returnType = "void" then
                        printfn "    %s(%s);" jsFuncName argRefs
                    else
                        printfn "    return %s(%s);" jsFuncName argRefs
                    printfn "}"
                printfn ""

        File.WriteAllText(fileName, b.ToString())

// Main generation function
let generateAll() =
    printfn "Generating Emscripten WebGPU bindings from dawn.json..."

    let outputDir = Path.Combine(__SOURCE_DIRECTORY__, "src", "WebGPU", "emscripten")
    Directory.CreateDirectory(outputDir) |> ignore

    let headerFile = Path.Combine(outputDir, "webgpu_emscripten.h")
    let implFile = Path.Combine(outputDir, "webgpu_emscripten.c")
    let jsLibFile = Path.Combine(outputDir, "library_webgpu_emscripten.js")
    let structMarshalFile = Path.Combine(outputDir, "struct_marshalers_generated.js")

    printfn "Generating C header: %s" headerFile
    CHeaderGen.generate headerFile

    printfn "Generating C implementation: %s" implFile
    CImplGen.generate implFile

    printfn "Generating struct marshalers: %s" structMarshalFile
    StructMarshalerGen.generate structMarshalFile

    printfn "Generating JavaScript library: %s" jsLibFile
    JsLibraryGen.generate jsLibFile

    printfn ""
    printfn "Done! Generated %d files." 4
    printfn ""
    printfn "Note: The generated JavaScript implementations are templates."
    printfn "Some complex operations (especially async with callbacks) may need manual refinement."
    printfn "However, all the core synchronous operations should work out of the box."

// Run the generator
generateAll()
