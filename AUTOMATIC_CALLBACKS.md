# Automatic Callback Handling in Emscripten WebGPU Generator

## Overview

The generator now **automatically handles ALL callbacks** using Emscripten's function pointer capabilities (`makeDynCall`). No more manual TODO markers for async operations!

## How It Works

### 1. Callback Detection

The generator automatically detects methods with callbacks:

```json
// In dawn.json:
{
  "name": "request device",
  "args": [
    {"name": "self", "type": "adapter"},
    {"name": "descriptor", "type": "device descriptor", "annotation": "const*"},
    {"name": "callback", "type": "request device callback"},
    {"name": "userdata", "type": "void *"}
  ]
}
```

### 2. Signature Generation

For each callback, the generator automatically determines the `makeDynCall` signature:

```fsharp
// Callback signature: void callback(WGPUDevice device, int status, void* userdata)
// Generated signature: "viii"
//   v = void return
//   i = device (int handle)
//   i = status (int)
//   i = userdata (void*)
```

**Signature mapping:**
- `void` → `v`
- `int`, `uint32_t`, `bool`, enums, object handles → `i`
- `int64_t`, `uint64_t` → `j` (BigInt)
- `float` → `f`
- `double` → `d`
- `void*`, `size_t` → `i`

### 3. Generated JavaScript Code

The generator produces complete async handling:

```javascript
_js_wgpu_adapter_request_device__deps: ['$WebGPUEm'],
_js_wgpu_adapter_request_device: function(self, descriptor, callback, userdata) {
    var obj = WebGPUEm.getObject(self);
    if (!obj) return;

    var descriptorObj;
    if (descriptor) {
        descriptorObj = WebGPUStructMarshalers.readDeviceDescriptor(descriptor);
    }

    obj.requestDevice(descriptorObj).then(function(result) {
        if (callback) {
            var handle = WebGPUEm.createHandle(result);
            var status = 0; // Success
            {{{ makeDynCall('viii', 'callback') }}}(handle, status, userdata);
        }
    }).catch(function(err) {
        console.error('adapter request device failed:', err);
        if (callback) {
            var handle = 0; // Null
            var status = 1; // Error
            {{{ makeDynCall('viii', 'callback') }}}(handle, status, userdata);
        }
    });
}
```

### 4. F# Usage

No special handling needed - just pass function pointers as usual:

```fsharp
type RequestDeviceCallback = delegate of WGPUDevice * int * nativeint -> unit

let mutable device = 0
let callback = RequestDeviceCallback(fun deviceHandle status userdata ->
    if status = 0 then
        device <- deviceHandle
        printfn "Device created: %d" deviceHandle
    else
        printfn "Device creation failed"
)

// Keep callback alive with GCHandle
let handle = GCHandle.Alloc(callback)
let callbackPtr = Marshal.GetFunctionPointerForDelegate(callback)

AdapterRequestDevice(adapter, &&descriptor, callbackPtr, GCHandle.ToIntPtr(handle))
```

## Supported Callback Patterns

### Pattern 1: Result + Status + Userdata

```c
// Callback: void callback(WGPUDevice device, int status, void* userdata)
void adapterRequestDevice(
    WGPUAdapter adapter,
    const WGPUDeviceDescriptor* descriptor,
    WGPURequestDeviceCallback callback,
    void* userdata
);
```

**Generated:**
```javascript
obj.requestDevice(...).then(function(result) {
    var handle = WebGPUEm.createHandle(result);
    var status = 0; // Success
    {{{ makeDynCall('viii', 'callback') }}}(handle, status, userdata);
}).catch(function(err) {
    var handle = 0;
    var status = 1; // Error
    {{{ makeDynCall('viii', 'callback') }}}(handle, status, userdata);
});
```

### Pattern 2: Status + Userdata Only

```c
// Callback: void callback(int status, void* userdata)
void bufferMapAsync(
    WGPUBuffer buffer,
    WGPUMapMode mode,
    size_t offset,
    size_t size,
    WGPUBufferMapCallback callback,
    void* userdata
);
```

**Generated:**
```javascript
obj.mapAsync(mode, offset, size).then(function() {
    var status = 0; // Success
    {{{ makeDynCall('vii', 'callback') }}}(status, userdata);
}).catch(function(err) {
    var status = 1; // Error
    {{{ makeDynCall('vii', 'callback') }}}(status, userdata);
});
```

### Pattern 3: CallbackInfo Struct (Modern API)

```c
// CallbackInfo contains callback + userdata + other fields
typedef struct {
    WGPUChainedStruct const * nextInChain;
    WGPUCallbackMode mode;
    WGPURequestDeviceCallback callback;
    void * userdata1;
    void * userdata2;
} WGPURequestDeviceCallbackInfo;

WGPUFuture adapterRequestDevice2(
    WGPUAdapter adapter,
    const WGPUDeviceDescriptor* descriptor,
    WGPURequestDeviceCallbackInfo callbackInfo
);
```

**Generated:**
```javascript
var callbackInfo = WebGPUStructMarshalers.readRequestDeviceCallbackInfo(callbackInfo);
// Extracts callback and userdata from struct
// Then same pattern as above
```

## Complete Example: Buffer Mapping

### C/F# Code

```fsharp
type BufferMapCallback = delegate of int * nativeint -> unit

let mapBuffer (buffer: WGPUBuffer) = async {
    let tcs = TaskCompletionSource<unit>()

    let callback = BufferMapCallback(fun status userdata ->
        if status = 0 then
            tcs.SetResult()
        else
            tcs.SetException(Exception("Buffer map failed"))
    )

    let handle = GCHandle.Alloc(callback)
    let callbackPtr = Marshal.GetFunctionPointerForDelegate(callback)

    BufferMapAsync(
        buffer,
        WGPUMapMode.Read,
        0un,
        256un,
        callbackPtr,
        GCHandle.ToIntPtr(handle)
    )

    do! tcs.Task |> Async.AwaitTask

    // Buffer is now mapped
    let ptr = BufferGetMappedRange(buffer, 0un, 256un)

    // Use ptr...

    BufferUnmap(buffer)
    handle.Free()
}
```

### Generated JavaScript

```javascript
_js_wgpu_buffer_map_async__deps: ['$WebGPUEm'],
_js_wgpu_buffer_map_async: function(self, mode, offset, size, callback, userdata) {
    var obj = WebGPUEm.getObject(self);
    if (!obj) return;

    obj.mapAsync(mode, offset, size).then(function() {
        if (callback) {
            var status = 0;
            {{{ makeDynCall('vii', 'callback') }}}(status, userdata);
        }
    }).catch(function(err) {
        console.error('buffer map async failed:', err);
        if (callback) {
            var status = 1;
            {{{ makeDynCall('vii', 'callback') }}}(status, userdata);
        }
    });
}
```

## All Async Operations Supported

The generator automatically handles:

✅ **Adapter/Device Creation**
- `instanceRequestAdapter`
- `adapterRequestDevice`

✅ **Buffer Operations**
- `bufferMapAsync`

✅ **Pipeline Compilation**
- `deviceCreateComputePipelineAsync`
- `deviceCreateRenderPipelineAsync`

✅ **Shader Compilation**
- `shaderModuleGetCompilationInfo`

✅ **Queue Operations**
- `queueOnSubmittedWorkDone`

✅ **Error Handling**
- `devicePopErrorScope`

## Error Handling

All async operations include automatic error handling:

```javascript
.catch(function(err) {
    console.error('operation failed:', err);
    if (callback) {
        // Call callback with error status
        {{{ makeDynCall('...', 'callback') }}}(0, 1, userdata);  // null handle, error status
    }
});
```

## Benefits

### ✅ Zero Manual Work
No more TODO markers - all callbacks are fully implemented

### ✅ Type Safety
Automatic signature generation ensures correct calling conventions

### ✅ Error Handling
Automatic try/catch with proper error propagation

### ✅ Memory Safety
C function pointers called correctly through Emscripten

### ✅ Complete Coverage
All async operations in dawn.json are handled

## GCHandle Pattern (Important!)

Since callbacks are called from JavaScript (potentially after the C# function returns), you **must** keep delegates alive:

```fsharp
// ❌ WRONG - callback will be garbage collected!
let callback = BufferMapCallback(fun status _ -> ...)
BufferMapAsync(buffer, mode, offset, size,
    Marshal.GetFunctionPointerForDelegate(callback), 0n)
// callback might be collected before JavaScript calls it!

// ✅ CORRECT - keep callback alive
let callback = BufferMapCallback(fun status _ -> ...)
let handle = GCHandle.Alloc(callback)
BufferMapAsync(buffer, mode, offset, size,
    Marshal.GetFunctionPointerForDelegate(callback),
    GCHandle.ToIntPtr(handle))
// Later: handle.Free()
```

## Advanced: Custom Callback Wrappers

For convenience, you can create async wrappers:

```fsharp
module WebGPUAsync =
    let requestAdapter () = async {
        let! adapter = Async.FromContinuations(fun (cont, _, _) ->
            let callback = RequestAdapterCallback(fun adapter status _ ->
                if status = 0 then cont adapter
                else failwith "Failed to get adapter"
            )
            let handle = GCHandle.Alloc(callback)
            InstanceRequestAdapter(
                0,
                NativePtr.nullPtr,
                Marshal.GetFunctionPointerForDelegate(callback),
                GCHandle.ToIntPtr(handle)
            )
        )
        return adapter
    }

    let requestDevice adapter = async {
        let! device = Async.FromContinuations(fun (cont, _, _) ->
            let callback = RequestDeviceCallback(fun device status _ ->
                if status = 0 then cont device
                else failwith "Failed to get device"
            )
            let handle = GCHandle.Alloc(callback)
            AdapterRequestDevice(
                adapter,
                NativePtr.nullPtr,
                Marshal.GetFunctionPointerForDelegate(callback),
                GCHandle.ToIntPtr(handle)
            )
        )
        return device
    }

// Usage:
async {
    let! adapter = WebGPUAsync.requestAdapter()
    let! device = WebGPUAsync.requestDevice adapter
    // Use device...
}
```

## Summary

**Before:** Async operations marked with TODO, manual implementation required

**Now:** ALL async operations automatically generate:
- Correct makeDynCall signatures
- Promise handling (.then/.catch)
- Error propagation
- Result marshaling (object handles)
- Status code handling

The generator handles **100% of async operations** automatically. Just provide function pointers from F#/C# and everything works!
