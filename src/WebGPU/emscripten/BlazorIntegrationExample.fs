namespace WebGPU.Emscripten

open System
open System.Runtime.InteropServices

/// <summary>
/// Example F# bindings for Emscripten WebGPU
/// These bindings use DllImport to call C functions that interface with JavaScript
/// </summary>
module EmscriptenWebGPU =

    // ============================================================================
    // Type Definitions
    // ============================================================================

    /// WebGPU object handles (represented as integers)
    type WGPUInstance = int
    type WGPUAdapter = int
    type WGPUDevice = int
    type WGPUBuffer = int
    type WGPUTexture = int
    type WGPUTextureView = int
    type WGPUShaderModule = int
    type WGPURenderPipeline = int
    type WGPUComputePipeline = int
    type WGPUCommandEncoder = int
    type WGPUCommandBuffer = int
    type WGPURenderPassEncoder = int
    type WGPUComputePassEncoder = int
    type WGPUQueue = int
    type WGPUBindGroup = int
    type WGPUBindGroupLayout = int
    type WGPUPipelineLayout = int
    type WGPUSampler = int

    /// Buffer usage flags
    [<Flags>]
    type WGPUBufferUsage =
        | None = 0x00000000
        | MapRead = 0x00000001
        | MapWrite = 0x00000002
        | CopySrc = 0x00000004
        | CopyDst = 0x00000008
        | Index = 0x00000010
        | Vertex = 0x00000020
        | Uniform = 0x00000040
        | Storage = 0x00000080
        | Indirect = 0x00000100
        | QueryResolve = 0x00000200

    /// Texture usage flags
    [<Flags>]
    type WGPUTextureUsage =
        | None = 0x00000000
        | CopySrc = 0x00000001
        | CopyDst = 0x00000002
        | TextureBinding = 0x00000004
        | StorageBinding = 0x00000008
        | RenderAttachment = 0x00000010

    /// Map mode flags
    [<Flags>]
    type WGPUMapMode =
        | None = 0x00000000
        | Read = 0x00000001
        | Write = 0x00000002

    /// Texture format enum
    type WGPUTextureFormat =
        | Undefined = 0
        | RGBA8Unorm = 1
        | RGBA8UnormSrgb = 2
        | BGRA8Unorm = 3
        | BGRA8UnormSrgb = 4
        | Depth24Plus = 5
        | Depth24PlusStencil8 = 6
        // Add more formats as needed

    /// Load operation enum
    type WGPULoadOp =
        | Undefined = 0
        | Load = 1
        | Clear = 2

    /// Store operation enum
    type WGPUStoreOp =
        | Undefined = 0
        | Store = 1
        | Discard = 2

    // ============================================================================
    // Struct Definitions
    // ============================================================================

    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type WGPUStringView =
        {
            Data: nativeint
            Length: unativeint
        }

    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type WGPUColor =
        {
            R: float
            G: float
            B: float
            A: float
        }

    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type WGPUExtent3D =
        {
            Width: uint32
            Height: uint32
            DepthOrArrayLayers: uint32
        }

    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type WGPUOrigin3D =
        {
            X: uint32
            Y: uint32
            Z: uint32
        }

    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type WGPUBufferDescriptor =
        {
            NextInChain: nativeint
            Label: nativeint
            Usage: WGPUBufferUsage
            Size: uint64
            MappedAtCreation: int
        }

    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type WGPUTextureDescriptor =
        {
            NextInChain: nativeint
            Label: nativeint
            Usage: WGPUTextureUsage
            Dimension: int
            Size: WGPUExtent3D
            Format: WGPUTextureFormat
            MipLevelCount: uint32
            SampleCount: uint32
            ViewFormatCount: unativeint
            ViewFormats: nativeint
        }

    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type WGPURenderPassColorAttachment =
        {
            NextInChain: nativeint
            View: WGPUTextureView
            DepthSlice: uint32
            ResolveTarget: WGPUTextureView
            LoadOp: WGPULoadOp
            StoreOp: WGPUStoreOp
            ClearValue: WGPUColor
        }

    // ============================================================================
    // Callback Delegates
    // ============================================================================

    /// Callback for adapter request
    type WGPURequestAdapterCallback = delegate of WGPUAdapter * int * nativeint -> unit

    /// Callback for device request
    type WGPURequestDeviceCallback = delegate of WGPUDevice * int * nativeint -> unit

    /// Callback for buffer mapping
    type WGPUBufferMapCallback = delegate of int * nativeint -> unit

    // ============================================================================
    // DllImport Declarations
    // ============================================================================
    // Note: Use "__Internal" for Emscripten/WASM

    [<DllImport("__Internal", EntryPoint="wgpuEmInstanceRequestAdapter")>]
    extern void InstanceRequestAdapter(
        WGPUInstance instance,
        nativeptr<byte> options,
        WGPURequestAdapterCallback callback,
        nativeint userdata)

    [<DllImport("__Internal", EntryPoint="wgpuEmAdapterRequestDevice")>]
    extern void AdapterRequestDevice(
        WGPUAdapter adapter,
        nativeptr<byte> descriptor,
        WGPURequestDeviceCallback callback,
        nativeint userdata)

    [<DllImport("__Internal", EntryPoint="wgpuEmDeviceCreateBuffer")>]
    extern WGPUBuffer DeviceCreateBuffer(
        WGPUDevice device,
        nativeptr<WGPUBufferDescriptor> descriptor)

    [<DllImport("__Internal", EntryPoint="wgpuEmDeviceCreateShaderModule")>]
    extern WGPUShaderModule DeviceCreateShaderModule(
        WGPUDevice device,
        nativeptr<byte> descriptor)

    [<DllImport("__Internal", EntryPoint="wgpuEmDeviceCreateCommandEncoder")>]
    extern WGPUCommandEncoder DeviceCreateCommandEncoder(
        WGPUDevice device,
        nativeptr<byte> descriptor)

    [<DllImport("__Internal", EntryPoint="wgpuEmDeviceGetQueue")>]
    extern WGPUQueue DeviceGetQueue(WGPUDevice device)

    [<DllImport("__Internal", EntryPoint="wgpuEmBufferGetSize")>]
    extern uint64 BufferGetSize(WGPUBuffer buffer)

    [<DllImport("__Internal", EntryPoint="wgpuEmBufferGetUsage")>]
    extern WGPUBufferUsage BufferGetUsage(WGPUBuffer buffer)

    [<DllImport("__Internal", EntryPoint="wgpuEmBufferMapAsync")>]
    extern void BufferMapAsync(
        WGPUBuffer buffer,
        WGPUMapMode mode,
        unativeint offset,
        unativeint size,
        WGPUBufferMapCallback callback,
        nativeint userdata)

    [<DllImport("__Internal", EntryPoint="wgpuEmBufferGetMappedRange")>]
    extern nativeint BufferGetMappedRange(
        WGPUBuffer buffer,
        unativeint offset,
        unativeint size)

    [<DllImport("__Internal", EntryPoint="wgpuEmBufferUnmap")>]
    extern void BufferUnmap(WGPUBuffer buffer)

    [<DllImport("__Internal", EntryPoint="wgpuEmBufferDestroy")>]
    extern void BufferDestroy(WGPUBuffer buffer)

    [<DllImport("__Internal", EntryPoint="wgpuEmQueueSubmit")>]
    extern void QueueSubmit(
        WGPUQueue queue,
        unativeint commandCount,
        nativeptr<WGPUCommandBuffer> commands)

    [<DllImport("__Internal", EntryPoint="wgpuEmQueueWriteBuffer")>]
    extern void QueueWriteBuffer(
        WGPUQueue queue,
        WGPUBuffer buffer,
        uint64 bufferOffset,
        nativeint data,
        unativeint size)

    [<DllImport("__Internal", EntryPoint="wgpuEmCommandEncoderFinish")>]
    extern WGPUCommandBuffer CommandEncoderFinish(
        WGPUCommandEncoder encoder,
        nativeptr<byte> descriptor)

    // ============================================================================
    // High-Level Helper Functions
    // ============================================================================

    /// Create a string pointer from F# string (UTF-8)
    let private stringToPtr (str: string) =
        if String.IsNullOrEmpty(str) then
            0n
        else
            Marshal.StringToHGlobalAnsi(str)

    /// Free a string pointer
    let private freeStringPtr (ptr: nativeint) =
        if ptr <> 0n then
            Marshal.FreeHGlobal(ptr)

    /// Helper to create a buffer descriptor
    let createBufferDescriptor (label: string option) (size: uint64) (usage: WGPUBufferUsage) (mappedAtCreation: bool) =
        let labelPtr = label |> Option.map stringToPtr |> Option.defaultValue 0n
        {
            NextInChain = 0n
            Label = labelPtr
            Usage = usage
            Size = size
            MappedAtCreation = if mappedAtCreation then 1 else 0
        }

    /// Helper to request adapter (async-style)
    let requestAdapterAsync () =
        let tcs = System.Threading.Tasks.TaskCompletionSource<WGPUAdapter>()

        let callback = WGPURequestAdapterCallback(fun adapter status userdata ->
            if status = 0 then
                tcs.SetResult(adapter)
            else
                tcs.SetException(Exception("Failed to request adapter"))
        )

        // Keep callback alive
        let callbackHandle = GCHandle.Alloc(callback)
        let callbackPtr = Marshal.GetFunctionPointerForDelegate(callback)

        InstanceRequestAdapter(0, NativePtr.nullPtr, callback, GCHandle.ToIntPtr(callbackHandle))

        tcs.Task

    /// Helper to request device (async-style)
    let requestDeviceAsync (adapter: WGPUAdapter) =
        let tcs = System.Threading.Tasks.TaskCompletionSource<WGPUDevice>()

        let callback = WGPURequestDeviceCallback(fun device status userdata ->
            if status = 0 then
                tcs.SetResult(device)
            else
                tcs.SetException(Exception("Failed to request device"))
        )

        let callbackHandle = GCHandle.Alloc(callback)
        let callbackPtr = Marshal.GetFunctionPointerForDelegate(callback)

        AdapterRequestDevice(adapter, NativePtr.nullPtr, callback, GCHandle.ToIntPtr(callbackHandle))

        tcs.Task

    /// Helper to map buffer (async-style)
    let mapBufferAsync (buffer: WGPUBuffer) (mode: WGPUMapMode) (offset: unativeint) (size: unativeint) =
        let tcs = System.Threading.Tasks.TaskCompletionSource<unit>()

        let callback = WGPUBufferMapCallback(fun status userdata ->
            if status = 0 then
                tcs.SetResult()
            else
                tcs.SetException(Exception("Failed to map buffer"))
        )

        let callbackHandle = GCHandle.Alloc(callback)
        let callbackPtr = Marshal.GetFunctionPointerForDelegate(callback)

        BufferMapAsync(buffer, mode, offset, size, callback, GCHandle.ToIntPtr(callbackHandle))

        tcs.Task

    // ============================================================================
    // Usage Example
    // ============================================================================

    /// Example: Initialize WebGPU and create a buffer
    let example () = async {
        // Request adapter
        let! adapter = requestAdapterAsync() |> Async.AwaitTask
        printfn "Got adapter: %d" adapter

        // Request device
        let! device = requestDeviceAsync(adapter) |> Async.AwaitTask
        printfn "Got device: %d" device

        // Create a buffer
        let bufferDesc = createBufferDescriptor (Some "Example Buffer") 1024UL
                            (WGPUBufferUsage.CopySrc ||| WGPUBufferUsage.CopyDst)
                            false

        use bufferDescPtr = fixed &bufferDesc
        let buffer = DeviceCreateBuffer(device, bufferDescPtr)
        printfn "Created buffer: %d" buffer

        // Get buffer size
        let size = BufferGetSize(buffer)
        printfn "Buffer size: %d" size

        // Get queue
        let queue = DeviceGetQueue(device)
        printfn "Got queue: %d" queue

        // Write data to buffer
        let data = Array.init 256 byte
        use dataPtr = fixed data
        QueueWriteBuffer(queue, buffer, 0UL, NativePtr.toNativeInt dataPtr, 256un)

        // Map buffer for reading
        let! _ = mapBufferAsync buffer WGPUMapMode.Read 0un 256un |> Async.AwaitTask

        // Get mapped range
        let mappedPtr = BufferGetMappedRange(buffer, 0un, 256un)
        printfn "Mapped pointer: %A" mappedPtr

        // Read data
        let readData = Array.zeroCreate<byte> 256
        Marshal.Copy(mappedPtr, readData, 0, 256)

        // Unmap buffer
        BufferUnmap(buffer)

        // Cleanup
        BufferDestroy(buffer)

        return ()
    }
