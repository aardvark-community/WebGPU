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

    /// Convert WebGPU type to JavaScript equivalent description
    let jsTypeDescription (t : TypeRef) =
        let def = table.[t.TypeName]
        match def with
        | Object o -> "GPU" + pascalCase o.Name
        | Enum e -> "enum"
        | Delegate d -> "callback"
        | Struct s -> "descriptor object"
        | Native n -> n.Name
        | _ -> "unknown"

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
            | Object o ->
                if isEmscripten o.Tags then
                    printfn "typedef int WGPU%s;" (pascalCase o.Name)
            | _ -> ()

        printfn ""
        printfn "// Forward declarations for structs"
        for a in all do
            match a with
            | Struct s ->
                if isEmscripten s.Tags then
                    printfn "typedef struct WGPU%s WGPU%s;" (pascalCase s.Name) (pascalCase s.Name)
            | _ -> ()

        printfn ""
        printfn "// Enum definitions"
        for a in all do
            match a with
            | Enum e ->
                if isEmscripten e.Tags then
                    printfn "typedef enum WGPU%s {" (pascalCase e.Name)
                    for (name, value) in e.Values do
                        printfn "    WGPU%s_%s = %d," (pascalCase e.Name) (pascalCase name) value
                    printfn "} WGPU%s;" (pascalCase e.Name)
                    printfn ""
            | _ -> ()

        printfn ""
        printfn "// Function declarations"

        // Generate function declarations for all objects and their methods
        for a in all do
            let functions =
                match a with
                | Function f ->
                    if isEmscripten f.Tags then [f]
                    else []
                | Object o ->
                    if isEmscripten o.Tags then
                        o.Methods
                        |> List.filter (fun m -> isEmscripten m.Tags)
                        |> List.map (fun m ->
                            let name = o.Name + " " + m.Name
                            let args =
                                { Name = "self"; Tags = []; Type = { TypeName = o.Name; Annotation = None };
                                  Default = None; Optional = false; Length = None } :: m.Args
                            { m with Name = name; Args = args }
                        )
                    else []
                | _ -> []

            for f in functions do
                let returnType = cTypeName f.Return
                let funcName = "wgpuEm" + pascalCase f.Name

                if FunctionDef.isBadWasmFunction f then
                    // Generate struct for packed arguments
                    printfn ""
                    printfn "// Packed args struct for %s" f.Name
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

    let generate (fileName : string) (useJsLibrary : bool) =
        let b = StringBuilder()
        let printfn fmt = Printf.kprintf (fun str -> b.AppendLine(str) |> ignore) fmt

        printfn "#include \"webgpu_emscripten.h\""
        printfn "#include <emscripten.h>"
        printfn "#include <emscripten/em_asm.h>"
        printfn ""

        if not useJsLibrary then
            printfn "// Using inline EM_ASM for JavaScript interop"
            printfn ""

        // Generate implementations for all functions
        for a in all do
            let functions =
                match a with
                | Function f ->
                    if isEmscripten f.Tags then [f]
                    else []
                | Object o ->
                    if isEmscripten o.Tags then
                        o.Methods
                        |> List.filter (fun m -> isEmscripten m.Tags)
                        |> List.map (fun m ->
                            let name = o.Name + " " + m.Name
                            let args =
                                { Name = "self"; Tags = []; Type = { TypeName = o.Name; Annotation = None };
                                  Default = None; Optional = false; Length = None } :: m.Args
                            { m with Name = name; Args = args }
                        )
                    else []
                | _ -> []

            for f in functions do
                let returnType = cTypeName f.Return
                let funcName = "wgpuEm" + pascalCase f.Name

                if FunctionDef.isBadWasmFunction f then
                    printfn "%s %s(const WGPU%sArgs* args) {" returnType funcName (pascalCase f.Name)

                    if useJsLibrary then
                        // Call JS library function
                        let argRefs =
                            f.Args
                            |> List.map (fun arg -> sprintf "args->%s" (pascalCase arg.Name))
                            |> String.concat ", "
                        printfn "    return _js_wgpu_%s(%s);" (camelCase f.Name) argRefs
                    else
                        // Use inline EM_ASM
                        printfn "    // TODO: Implement inline EM_ASM for %s" f.Name
                        if returnType <> "void" then
                            printfn "    return 0;"

                    printfn "}"
                else
                    let argList =
                        f.Args
                        |> List.map (fun arg -> sprintf "%s %s" (cTypeName arg.Type) (camelCase arg.Name))
                        |> String.concat ", "
                    let argList = if argList = "" then "void" else argList

                    printfn "%s %s(%s) {" returnType funcName argList

                    if useJsLibrary then
                        // Call JS library function
                        let argRefs =
                            f.Args
                            |> List.map (fun arg -> camelCase arg.Name)
                            |> String.concat ", "
                        printfn "    return _js_wgpu_%s(%s);" (camelCase f.Name) argRefs
                    else
                        // Use inline EM_ASM
                        printfn "    // TODO: Implement inline EM_ASM for %s" f.Name
                        if returnType <> "void" then
                            printfn "    return 0;"

                    printfn "}"
                printfn ""

        File.WriteAllText(fileName, b.ToString())

module JsLibraryGen =

    open EmscriptenBindings

    let generateObjectName (objName : string) =
        // Convert object name to JavaScript GPU API name
        // e.g., "device" -> "device", "buffer" -> "buffer", etc.
        camelCase objName

    let generateJsMethodName (objName : string) (methodName : string) =
        // Convert method name to JavaScript method
        // e.g., "create buffer" -> "createBuffer"
        camelCase methodName

    let generate (fileName : string) =
        let b = StringBuilder()
        let printfn fmt = Printf.kprintf (fun str -> b.AppendLine(str) |> ignore) fmt

        printfn "/**"
        printfn " * Emscripten WebGPU Library"
        printfn " * Provides JavaScript bindings for WebGPU API"
        printfn " * Generated from dawn.json"
        printfn " */"
        printfn ""
        printfn "var LibraryWebGPUEmscripten = {"
        printfn "  $WebGPUEm: {"
        printfn "    // Object handle management"
        printfn "    nextHandle: 1,"
        printfn "    objects: {}, // Map from handle (int) to WebGPU object"
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
        printfn "    // Struct marshaling helpers"
        printfn "    readString: function(ptr, length) {"
        printfn "      if (!ptr) return null;"
        printfn "      if (length === undefined) {"
        printfn "        return UTF8ToString(ptr);"
        printfn "      }"
        printfn "      return UTF8ToString(ptr, length);"
        printfn "    },"
        printfn "    "
        printfn "    readStringView: function(ptr) {"
        printfn "      if (!ptr) return null;"
        printfn "      var dataPtr = {{{ makeGetValue('ptr', 0, '*') }}};"
        printfn "      var length = {{{ makeGetValue('ptr', 4, 'i32') }}};"
        printfn "      return WebGPUEm.readString(dataPtr, length);"
        printfn "    },"
        printfn "    "
        printfn "    // Initialize GPU adapter"
        printfn "    gpu: null,"
        printfn "    adapter: null,"
        printfn "    "
        printfn "    ensureGPU: function() {"
        printfn "      if (!WebGPUEm.gpu && navigator.gpu) {"
        printfn "        WebGPUEm.gpu = navigator.gpu;"
        printfn "      }"
        printfn "      return WebGPUEm.gpu !== null;"
        printfn "    }"
        printfn "  },"
        printfn ""

        // Generate instance functions
        printfn "  // Instance functions"
        printfn "  _js_wgpu_instance_request_adapter__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_instance_request_adapter: function(instance, options, callback, userdata) {"
        printfn "    if (!WebGPUEm.ensureGPU()) {"
        printfn "      console.error('WebGPU not supported');"
        printfn "      return;"
        printfn "    }"
        printfn "    "
        printfn "    // Parse options if provided"
        printfn "    var requestOptions = {};"
        printfn "    // TODO: Parse options from struct pointer"
        printfn "    "
        printfn "    WebGPUEm.gpu.requestAdapter(requestOptions).then(function(adapter) {"
        printfn "      var handle = WebGPUEm.createHandle(adapter);"
        printfn "      // TODO: Call callback with handle"
        printfn "      if (callback) {"
        printfn "        {{{ makeDynCall('viii', 'callback') }}}(handle, 0, userdata);"
        printfn "      }"
        printfn "    }).catch(function(err) {"
        printfn "      console.error('requestAdapter failed:', err);"
        printfn "      if (callback) {"
        printfn "        {{{ makeDynCall('viii', 'callback') }}}(0, 1, userdata);"
        printfn "      }"
        printfn "    });"
        printfn "  },"
        printfn ""

        // Generate adapter functions
        printfn "  // Adapter functions"
        printfn "  _js_wgpu_adapter_request_device__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_adapter_request_device: function(adapter, descriptor, callback, userdata) {"
        printfn "    var adapterObj = WebGPUEm.getObject(adapter);"
        printfn "    if (!adapterObj) {"
        printfn "      console.error('Invalid adapter handle');"
        printfn "      return;"
        printfn "    }"
        printfn "    "
        printfn "    // Parse descriptor if provided"
        printfn "    var deviceDescriptor = {};"
        printfn "    // TODO: Parse descriptor from struct pointer"
        printfn "    "
        printfn "    adapterObj.requestDevice(deviceDescriptor).then(function(device) {"
        printfn "      var handle = WebGPUEm.createHandle(device);"
        printfn "      var queueHandle = WebGPUEm.createHandle(device.queue);"
        printfn "      // TODO: Call callback with device handle"
        printfn "      if (callback) {"
        printfn "        {{{ makeDynCall('viii', 'callback') }}}(handle, 0, userdata);"
        printfn "      }"
        printfn "    }).catch(function(err) {"
        printfn "      console.error('requestDevice failed:', err);"
        printfn "      if (callback) {"
        printfn "        {{{ makeDynCall('viii', 'callback') }}}(0, 1, userdata);"
        printfn "      }"
        printfn "    });"
        printfn "  },"
        printfn ""

        // Generate device functions (examples)
        printfn "  // Device functions"
        printfn "  _js_wgpu_device_create_buffer__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_device_create_buffer: function(device, descriptor) {"
        printfn "    var deviceObj = WebGPUEm.getObject(device);"
        printfn "    if (!deviceObj) return 0;"
        printfn "    "
        printfn "    // Parse buffer descriptor"
        printfn "    // TODO: Implement full descriptor parsing"
        printfn "    var bufferDescriptor = {"
        printfn "      size: {{{ makeGetValue('descriptor', 0, 'i32') }}},"
        printfn "      usage: {{{ makeGetValue('descriptor', 4, 'i32') }}}"
        printfn "    };"
        printfn "    "
        printfn "    var buffer = deviceObj.createBuffer(bufferDescriptor);"
        printfn "    return WebGPUEm.createHandle(buffer);"
        printfn "  },"
        printfn ""

        printfn "  _js_wgpu_device_create_shader_module__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_device_create_shader_module: function(device, descriptor) {"
        printfn "    var deviceObj = WebGPUEm.getObject(device);"
        printfn "    if (!deviceObj) return 0;"
        printfn "    "
        printfn "    // TODO: Parse shader module descriptor (WGSL code)"
        printfn "    var shaderDescriptor = {"
        printfn "      code: '' // TODO: Read from descriptor"
        printfn "    };"
        printfn "    "
        printfn "    var module = deviceObj.createShaderModule(shaderDescriptor);"
        printfn "    return WebGPUEm.createHandle(module);"
        printfn "  },"
        printfn ""

        printfn "  _js_wgpu_device_create_command_encoder__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_device_create_command_encoder: function(device, descriptor) {"
        printfn "    var deviceObj = WebGPUEm.getObject(device);"
        printfn "    if (!deviceObj) return 0;"
        printfn "    "
        printfn "    var encoder = deviceObj.createCommandEncoder();"
        printfn "    return WebGPUEm.createHandle(encoder);"
        printfn "  },"
        printfn ""

        printfn "  _js_wgpu_device_get_queue__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_device_get_queue: function(device) {"
        printfn "    var deviceObj = WebGPUEm.getObject(device);"
        printfn "    if (!deviceObj) return 0;"
        printfn "    return WebGPUEm.createHandle(deviceObj.queue);"
        printfn "  },"
        printfn ""

        // Generate buffer functions
        printfn "  // Buffer functions"
        printfn "  _js_wgpu_buffer_get_size__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_buffer_get_size: function(buffer) {"
        printfn "    var bufferObj = WebGPUEm.getObject(buffer);"
        printfn "    if (!bufferObj) return 0;"
        printfn "    return bufferObj.size;"
        printfn "  },"
        printfn ""

        printfn "  _js_wgpu_buffer_get_usage__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_buffer_get_usage: function(buffer) {"
        printfn "    var bufferObj = WebGPUEm.getObject(buffer);"
        printfn "    if (!bufferObj) return 0;"
        printfn "    return bufferObj.usage;"
        printfn "  },"
        printfn ""

        printfn "  _js_wgpu_buffer_map_async__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_buffer_map_async: function(buffer, mode, offset, size, callback, userdata) {"
        printfn "    var bufferObj = WebGPUEm.getObject(buffer);"
        printfn "    if (!bufferObj) return;"
        printfn "    "
        printfn "    bufferObj.mapAsync(mode, offset, size).then(function() {"
        printfn "      if (callback) {"
        printfn "        {{{ makeDynCall('vii', 'callback') }}}(0, userdata);"
        printfn "      }"
        printfn "    }).catch(function(err) {"
        printfn "      console.error('mapAsync failed:', err);"
        printfn "      if (callback) {"
        printfn "        {{{ makeDynCall('vii', 'callback') }}}(1, userdata);"
        printfn "      }"
        printfn "    });"
        printfn "  },"
        printfn ""

        printfn "  _js_wgpu_buffer_get_mapped_range__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_buffer_get_mapped_range: function(buffer, offset, size) {"
        printfn "    var bufferObj = WebGPUEm.getObject(buffer);"
        printfn "    if (!bufferObj) return 0;"
        printfn "    "
        printfn "    var arrayBuffer = bufferObj.getMappedRange(offset, size);"
        printfn "    // TODO: Return pointer to WASM heap copy or direct access"
        printfn "    return 0;"
        printfn "  },"
        printfn ""

        printfn "  _js_wgpu_buffer_unmap__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_buffer_unmap: function(buffer) {"
        printfn "    var bufferObj = WebGPUEm.getObject(buffer);"
        printfn "    if (!bufferObj) return;"
        printfn "    bufferObj.unmap();"
        printfn "  },"
        printfn ""

        printfn "  _js_wgpu_buffer_destroy__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_buffer_destroy: function(buffer) {"
        printfn "    var bufferObj = WebGPUEm.getObject(buffer);"
        printfn "    if (!bufferObj) return;"
        printfn "    bufferObj.destroy();"
        printfn "    WebGPUEm.releaseHandle(buffer);"
        printfn "  },"
        printfn ""

        // Generate queue functions
        printfn "  // Queue functions"
        printfn "  _js_wgpu_queue_submit__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_queue_submit: function(queue, commandCount, commands) {"
        printfn "    var queueObj = WebGPUEm.getObject(queue);"
        printfn "    if (!queueObj) return;"
        printfn "    "
        printfn "    var commandBuffers = [];"
        printfn "    for (var i = 0; i < commandCount; i++) {"
        printfn "      var handle = {{{ makeGetValue('commands', 'i*4', 'i32') }}};"
        printfn "      var cmdBuf = WebGPUEm.getObject(handle);"
        printfn "      if (cmdBuf) commandBuffers.push(cmdBuf);"
        printfn "    }"
        printfn "    "
        printfn "    queueObj.submit(commandBuffers);"
        printfn "  },"
        printfn ""

        printfn "  _js_wgpu_queue_write_buffer__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_queue_write_buffer: function(queue, buffer, bufferOffset, data, size) {"
        printfn "    var queueObj = WebGPUEm.getObject(queue);"
        printfn "    var bufferObj = WebGPUEm.getObject(buffer);"
        printfn "    if (!queueObj || !bufferObj) return;"
        printfn "    "
        printfn "    var dataView = new Uint8Array(HEAPU8.buffer, data, size);"
        printfn "    queueObj.writeBuffer(bufferObj, bufferOffset, dataView);"
        printfn "  },"
        printfn ""

        // Generate command encoder functions
        printfn "  // Command encoder functions"
        printfn "  _js_wgpu_command_encoder_finish__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_command_encoder_finish: function(encoder, descriptor) {"
        printfn "    var encoderObj = WebGPUEm.getObject(encoder);"
        printfn "    if (!encoderObj) return 0;"
        printfn "    "
        printfn "    var cmdBuffer = encoderObj.finish();"
        printfn "    return WebGPUEm.createHandle(cmdBuffer);"
        printfn "  },"
        printfn ""

        printfn "  _js_wgpu_command_encoder_begin_render_pass__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_command_encoder_begin_render_pass: function(encoder, descriptor) {"
        printfn "    var encoderObj = WebGPUEm.getObject(encoder);"
        printfn "    if (!encoderObj) return 0;"
        printfn "    "
        printfn "    // TODO: Parse render pass descriptor"
        printfn "    var renderPassDescriptor = {};"
        printfn "    "
        printfn "    var passEncoder = encoderObj.beginRenderPass(renderPassDescriptor);"
        printfn "    return WebGPUEm.createHandle(passEncoder);"
        printfn "  },"
        printfn ""

        printfn "  _js_wgpu_command_encoder_begin_compute_pass__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_command_encoder_begin_compute_pass: function(encoder, descriptor) {"
        printfn "    var encoderObj = WebGPUEm.getObject(encoder);"
        printfn "    if (!encoderObj) return 0;"
        printfn "    "
        printfn "    var passEncoder = encoderObj.beginComputePass();"
        printfn "    return WebGPUEm.createHandle(passEncoder);"
        printfn "  },"
        printfn ""

        // Add generic reference counting (no-ops for JavaScript)
        printfn "  // Reference counting (no-ops for JavaScript GC)"
        printfn "  _js_wgpu_object_reference__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_object_reference: function(handle) {"
        printfn "    // No-op: JavaScript uses garbage collection"
        printfn "  },"
        printfn ""

        printfn "  _js_wgpu_object_release__deps: ['$WebGPUEm'],"
        printfn "  _js_wgpu_object_release: function(handle) {"
        printfn "    // Release handle from our table"
        printfn "    WebGPUEm.releaseHandle(handle);"
        printfn "  },"
        printfn ""

        printfn "};"
        printfn ""
        printfn "autoAddDeps(LibraryWebGPUEmscripten, '$WebGPUEm');"
        printfn "mergeInto(LibraryManager.library, LibraryWebGPUEmscripten);"

        File.WriteAllText(fileName, b.ToString())

// Main generation function
let generateAll() =
    printfn "Generating Emscripten WebGPU bindings..."

    let outputDir = Path.Combine(__SOURCE_DIRECTORY__, "src", "WebGPU", "emscripten")
    Directory.CreateDirectory(outputDir) |> ignore

    let headerFile = Path.Combine(outputDir, "webgpu_emscripten.h")
    let implFile = Path.Combine(outputDir, "webgpu_emscripten.c")
    let jsLibFile = Path.Combine(outputDir, "library_webgpu_emscripten.js")

    printfn "Generating C header: %s" headerFile
    CHeaderGen.generate headerFile

    printfn "Generating C implementation: %s" implFile
    CImplGen.generate implFile true // Use JS library instead of inline EM_ASM

    printfn "Generating JavaScript library: %s" jsLibFile
    JsLibraryGen.generate jsLibFile

    printfn "Done! Generated Emscripten WebGPU bindings."

// Run the generator
generateAll()
