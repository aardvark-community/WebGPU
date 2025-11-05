# Emscripten WebGPU Generator

This generator creates C and JavaScript bindings for WebGPU that work with Emscripten and Blazor WebAssembly. Unlike the existing bindings that use Dawn's native implementation, these bindings directly interface with the browser's native WebGPU API.

## Overview

The generator reads `dawn.json` and produces three files:

1. **webgpu_emscripten.h** - C header with function declarations
2. **webgpu_emscripten.c** - C implementation with Emscripten interop
3. **library_webgpu_emscripten.js** - JavaScript library implementing actual WebGPU calls

## Architecture

### Object Handle System

WebGPU objects (Device, Buffer, Texture, etc.) are represented as **integer handles** on the C side:

```c
typedef int WGPUDevice;
typedef int WGPUBuffer;
typedef int WGPUTexture;
// ... etc
```

On the JavaScript side, a table maps these integers to actual WebGPU objects:

```javascript
WebGPUEm.objects = {
  1: <GPUDevice>,
  2: <GPUBuffer>,
  3: <GPUTexture>,
  // ... etc
}
```

### Data Flow

```
F# / C# Code (DllImport)
        ↓
C Functions (webgpu_emscripten.c)
        ↓
Emscripten JS Library (library_webgpu_emscripten.js)
        ↓
Browser WebGPU API (navigator.gpu)
```

### Struct Marshaling

C structs are passed as pointers and marshaled in JavaScript:

```c
// C side
typedef struct {
    uint64_t size;
    WGPUBufferUsageFlags usage;
    int mappedAtCreation;
    const char* label;
} WGPUBufferDescriptor;

WGPUBuffer wgpuDeviceCreateBuffer(WGPUDevice device, const WGPUBufferDescriptor* descriptor);
```

```javascript
// JavaScript side
_js_wgpu_device_create_buffer: function(device, descriptorPtr) {
    var deviceObj = WebGPUEm.getObject(device);

    // Read struct fields from WASM memory
    var size = getValue(descriptorPtr + 0, 'i64');
    var usage = getValue(descriptorPtr + 8, 'i32');
    var mappedAtCreation = getValue(descriptorPtr + 12, 'i32');
    var labelPtr = getValue(descriptorPtr + 16, '*');

    // Create JavaScript descriptor object
    var descriptor = {
        size: size,
        usage: usage,
        mappedAtCreation: !!mappedAtCreation
    };

    if (labelPtr) {
        descriptor.label = UTF8ToString(labelPtr);
    }

    // Call actual WebGPU API
    var buffer = deviceObj.createBuffer(descriptor);

    // Return handle
    return WebGPUEm.createHandle(buffer);
}
```

## Usage

### Step 1: Generate Bindings

Run the generator script with F# Interactive:

```bash
dotnet fsi EmscriptenGenerator.fsx
```

This will create files in `src/WebGPU/emscripten/`:
- `webgpu_emscripten.h`
- `webgpu_emscripten.c`
- `library_webgpu_emscripten.js`

### Step 2: Integrate with Blazor WASM Project

1. Add the C file to your project:

```xml
<!-- In your .fsproj or .csproj -->
<ItemGroup>
  <NativeFileReference Include="emscripten/webgpu_emscripten.c" />
  <None Include="emscripten/library_webgpu_emscripten.js">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </None>
</ItemGroup>
```

2. Configure Emscripten to use the JavaScript library:

```xml
<PropertyGroup>
  <WasmBuildNative>true</WasmBuildNative>
  <EmccExtraLDFlags>--js-library emscripten/library_webgpu_emscripten.js</EmccExtraLDFlags>
</PropertyGroup>
```

### Step 3: Use in F# / C# Code

Create DllImport bindings:

```fsharp
// F# example
open System.Runtime.InteropServices

[<DllImport("__Internal", EntryPoint="wgpuEmInstanceRequestAdapter")>]
extern void InstanceRequestAdapter(
    int instance,
    nativeptr<RequestAdapterOptions> options,
    nativeint callback,
    nativeint userdata)

[<DllImport("__Internal", EntryPoint="wgpuEmDeviceCreateBuffer")>]
extern int DeviceCreateBuffer(int device, nativeptr<BufferDescriptor> descriptor)
```

Note: Use `"__Internal"` as the library name for Emscripten/WASM.

## Differences from Dawn Bindings

### Dawn (Current Approach)
- Uses Dawn's C++ WebGPU implementation
- Dawn compiled to WASM with Emscripten
- Supports both native and web targets
- Larger binary size
- Full Dawn feature set

### Emscripten Bindings (This Generator)
- Direct browser WebGPU API calls
- Thin JavaScript shim layer
- Web-only (Blazor WASM)
- Smaller binary size
- Browser feature set only

## API Compatibility

The generator uses `dawn.json` as the source of truth, which may include features not available in browser WebGPU implementations. Key differences:

### Dawn-Only Features (May Not Work)
- Adapter enumeration (Dawn native)
- Surface creation from native windows
- Some advanced features

### Browser-Only Features (Not in dawn.json)
- Canvas integration
- ImageBitmap textures
- Some modern WebGPU features

### Core Compatible Features
- Device/adapter creation
- Buffer operations
- Texture operations
- Shader modules (WGSL)
- Render/compute pipelines
- Command encoding
- Queue operations

## Handling Asynchronous Operations

WebGPU has several async operations that need special handling:

### Request Adapter/Device

```javascript
// Async operations use callbacks
_js_wgpu_instance_request_adapter: function(instance, options, callback, userdata) {
    WebGPUEm.gpu.requestAdapter(options).then(function(adapter) {
        var handle = WebGPUEm.createHandle(adapter);
        // Call back to C
        {{{ makeDynCall('viii', 'callback') }}}(handle, 0, userdata);
    });
}
```

On the C# side, you'll need to provide callback delegates:

```fsharp
type RequestAdapterCallback = delegate of int * int * nativeint -> unit

let mutable adapter = 0
let callback = RequestAdapterCallback(fun adapterHandle status userdata ->
    adapter <- adapterHandle
    printfn "Received adapter: %d" adapterHandle
)

let callbackPtr = Marshal.GetFunctionPointerForDelegate(callback)
InstanceRequestAdapter(instance, NativePtr.nullPtr, callbackPtr, 0n)
```

### Buffer Mapping

```fsharp
type BufferMapCallback = delegate of int * nativeint -> unit

let callback = BufferMapCallback(fun status userdata ->
    if status = 0 then
        printfn "Buffer mapped successfully"
        // Access mapped range
        let ptr = BufferGetMappedRange(buffer, 0UL, size)
        // ... use pointer ...
        BufferUnmap(buffer)
)
```

## Struct Layout Considerations

C struct layout must match JavaScript reading offsets:

```fsharp
[<Struct; StructLayout(LayoutKind.Sequential)>]
type BufferDescriptor = {
    NextInChain: nativeint
    Label: nativeint  // Pointer to UTF-8 string
    Usage: BufferUsage
    Size: uint64
    MappedAtCreation: int  // Boolean as int
}
```

In JavaScript:

```javascript
// Assuming C_STRUCTS.WGPUBufferDescriptor offsets are:
// nextInChain: 0
// label: 4 (or 8 on 64-bit)
// usage: 8 (or 12)
// size: 12 (or 16)
// mappedAtCreation: 20 (or 24)

var nextInChain = getValue(ptr + C_STRUCTS.WGPUBufferDescriptor.nextInChain, '*');
var label = getValue(ptr + C_STRUCTS.WGPUBufferDescriptor.label, '*');
// ... etc
```

## Extension Chains

WebGPU uses extension chains for optional features:

```c
typedef struct WGPUChainedStruct {
    struct WGPUChainedStruct* next;
    WGPUSType sType;
} WGPUChainedStruct;

typedef struct WGPUBufferDescriptor {
    WGPUChainedStruct* nextInChain;
    // ... other fields
} WGPUBufferDescriptor;
```

The JavaScript marshaler needs to walk the chain:

```javascript
function parseChainedStruct(ptr) {
    var extensions = [];
    while (ptr) {
        var sType = getValue(ptr + 4, 'i32');
        // Parse extension based on sType
        extensions.push(parseExtension(ptr, sType));
        ptr = getValue(ptr, '*'); // next pointer
    }
    return extensions;
}
```

## Debugging

### Enable Verbose Logging

```javascript
WebGPUEm.debug = true;

_js_wgpu_device_create_buffer: function(device, descriptor) {
    if (WebGPUEm.debug) {
        console.log('createBuffer called:', {
            device: device,
            descriptor: descriptor
        });
    }
    // ... implementation
}
```

### Inspect Object Table

```javascript
// In browser console:
console.log(Module.WebGPUEm.objects);
```

### Handle Validation

```javascript
getObject: function(handle) {
    if (handle === 0) {
        console.warn('Attempted to get null handle');
        return null;
    }
    if (!(handle in WebGPUEm.objects)) {
        console.error('Invalid handle:', handle);
        throw new Error('Invalid WebGPU handle: ' + handle);
    }
    return WebGPUEm.objects[handle];
}
```

## Performance Considerations

### Handle Management
- Handles are never reused (simple incrementing counter)
- Released handles are deleted from the table
- Consider handle recycling for high-churn scenarios

### Struct Marshaling
- Copying struct data from WASM memory to JS has overhead
- Consider batching operations when possible
- Large arrays (vertex buffers, etc.) use typed array views to avoid copies

### String Handling
- UTF-8 encoding/decoding has cost
- Cache frequently used strings
- Consider using numeric labels for hot paths

## .NET 8 Emscripten Version

.NET 8 uses Emscripten 3.1.34. Key features:

- BigInt support for 64-bit integers
- Async/await support with ASYNCIFY
- Modern JavaScript output

Ensure compatibility:

```javascript
// Use BigInt for 64-bit values
var size = getValue(ptr, 'i64'); // Returns BigInt in Emscripten 3.x

// Convert to Number if needed
var sizeNum = Number(size);
```

## Future Enhancements

### Auto-Generated Struct Marshalers
Generate JavaScript marshalers for all structs automatically:

```javascript
var Marshalers = {
    WGPUBufferDescriptor: function(ptr) {
        return {
            size: getValue(ptr + offsetof_size, 'i64'),
            usage: getValue(ptr + offsetof_usage, 'i32'),
            mappedAtCreation: !!getValue(ptr + offsetof_mapped, 'i32'),
            label: readOptionalString(ptr + offsetof_label)
        };
    }
};
```

### TypeScript Definitions
Generate `.d.ts` files for type safety:

```typescript
export interface WebGPUEmscripten {
    createHandle(obj: GPUDevice | GPUBuffer | ...): number;
    getObject(handle: number): GPUDevice | GPUBuffer | ...;
    releaseHandle(handle: number): void;
}
```

### Error Handling
Improved error propagation from WebGPU to C:

```javascript
try {
    var buffer = deviceObj.createBuffer(descriptor);
    return WebGPUEm.createHandle(buffer);
} catch (err) {
    WebGPUEm.lastError = err.message;
    return 0; // null handle
}
```

## Contributing

To add support for new WebGPU features:

1. Update `dawn.json` (if needed)
2. Run `EmscriptenGenerator.fsx`
3. Implement JavaScript marshalers for new structs
4. Test with Blazor WASM project

## License

This generator and generated code follow the same license as the WebGPU project.
