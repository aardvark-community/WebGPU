namespace WebGPU

open System.Runtime.InteropServices
open System.Collections.Generic
open Microsoft.FSharp.NativeInterop

#nowarn "9"

module NativeInt =
    let ofVoidPtr (ptr : voidptr) =
        NativePtr.ofVoidPtr<byte> ptr |> NativePtr.toNativeInt
        
    let ofNativePtr (ptr : nativeptr<'a>) =
        NativePtr.toNativeInt ptr
        

module WebGPURaw =
    
    [<Struct>]
    type AdapterCreateInfo =
        {
            PowerPreference : PowerPreference
            ForceFallbackAdapter : int
        }
        
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type QueueDescriptor32 =
        {
            Label : nativeptr<byte>
            Dummy : uint32
        }
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type DeviceDescriptor32 =
        {
            RequiredLimits : Limits
            DefaultQueue : QueueDescriptor32
            RequiredFeatures : uint32
            UnusedPadding : uint32
        }

    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type QueueDescriptor64 =
        {
            Label : nativeptr<byte>
        }
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type DeviceDescriptor64 =
        {
            RequiredLimits : Limits
            DefaultQueue : QueueDescriptor64
            RequiredFeatures : uint32
            UnusedPadding : uint32
        }    
    [<DllImport("WebGPU")>]
    extern int wgpuSupported()
    
    [<DllImport("WebGPU")>]
    extern int my_navigator_gpu_request_adapter_async(AdapterCreateInfo& info, void* callback, void* cbid)

    [<DllImport("WebGPU")>]
    extern void my_wgpu_adapter_get_info(int adapter, byte* data)
    
    [<DllImport("WebGPU")>]
    extern int my_wgpu_adapter_or_device_get_features(int adapter)
    
    [<DllImport("WebGPU")>]
    extern void my_wgpu_adapter_or_device_get_limits(int adapter, Limits& limits)
    
    [<DllImport("WebGPU")>]
    extern int my_wgpu_adapter_is_fallback_adapter(int adapter)
    
    [<DllImport("WebGPU", EntryPoint="my_wgpu_adapter_request_device_async")>]
    extern void my_wgpu_adapter_request_device_async32(int adapter, DeviceDescriptor32& desc, void* callback, void* cbid)
    
    [<DllImport("WebGPU", EntryPoint="my_wgpu_adapter_request_device_async")>]
    extern void my_wgpu_adapter_request_device_async64(int adapter, DeviceDescriptor64& desc, void* callback, void* cbid)
    
    
    [<DllImport("WebGPU")>]
    extern int my_wgpu_device_get_queue(int device)
    
        
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type ImageCopyExternalImage =
        {
            Source : int
            Origin : Origin2d
            FlipY : int
        }

    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type ImageCopyTextureTagged =
        {
            Texture : int
            MipLevel : uint32
            Origin : Origin3d
            Aspect : TextureAspect
            ColorSpace : ColorSpace
            PremultipliedAlpha : int
        }
    
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type ImageCopyTexture =
        {
            Texture : int
            MipLevel : int
            Origin : Origin3d
            Aspect : TextureAspect
        }
        
    [<DllImport("WebGPU")>]
    extern nativeint gpuCreateInstance()
    
    //WGPUInstance instance, WGPURequestAdapterOptions const * options, WGPURequestAdapterCallback callback, void * userdata) {
    //     WGPUChainedStruct const * nextInChain;
    // WGPUSurface compatibleSurface; // nullable
    // WGPUPowerPreference powerPreference;
    // bool forceFallbackAdapter;
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type RequestAdapterOptions =
        {
            NextInChain : nativeint
            CompatibleSurface : nativeint
            PowerPreference : PowerPreference
            ForceFallbackAdapter : int
        }
    
    [<DllImport("WebGPU")>]
    extern void gpuInstanceRequestAdapter(nativeint instance, RequestAdapterOptions& options, void* callback, void* userdata)
    
    [<DllImport("WebGPU")>]
    extern void my_wgpu_queue_copy_external_image_to_texture(int queue, ImageCopyExternalImage& source, ImageCopyTextureTagged& destination, uint32 copyWidth, uint32 copyHeight, uint32 copyDepthOrArrayLayers)
 
    [<DllImport("WebGPU")>]
    extern void my_wgpu_queue_set_on_submitted_work_done_callback(int queue, void* callback, void* cbid)
    
    [<DllImport("WebGPU")>]
    extern void my_wgpu_queue_submit_multiple_and_destroy(int queue, int* commands, int count)
    
    [<DllImport("WebGPU")>]
    //void my_wgpu_queue_write_buffer(WGpuQueue queue, WGpuBuffer buffer, double_int53_t bufferOffset, const void *data, double_int53_t size) {
    extern void my_wgpu_queue_write_buffer(int queue, int buffer, int64& bufferOffset, void* data, int64& size)
 
    [<DllImport("WebGPU")>]
    extern void my_wgpu_queue_write_texture(int queue, ImageCopyTexture& destination, void* data, uint32 bytesPerBlockRow, uint32 blockRowsPerImage, uint32 writeWidth, uint32 writeHeight, uint32 writeDepthOrArrayLayers)
    //EMSCRIPTEN_KEEPALIVE void my_wgpu_queue_write_texture(WGpuQueue queue, const WGpuImageCopyTexture *destination, const void *data, uint32_t bytesPerBlockRow, uint32_t blockRowsPerImage, uint32_t writeWidth, uint32_t writeHeight, uint32_t writeDepthOrArrayLayers) {

    [<DllImport("WebGPU")>]
    extern void my_wgpu_device_set_lost_callback(int device, void* callback, void* cbid)
       
    [<DllImport("WebGPU")>]
    extern void my_wgpu_object_destroy(int handle)
       
    [<DllImport("WebGPU")>]
    extern void my_wgpu_device_set_uncapturederror_callback(int device, void* callback, void* cbid)
       
    // typedef struct WGpuBufferDescriptor
    // {
    //   uint64_t size;
    //   WGPU_BUFFER_USAGE_FLAGS usage;
    //   WGPU_BOOL mappedAtCreation; // Note: it is valid to set mappedAtCreation to true without MAP_READ or MAP_WRITE in usage. This can be used to set the buffer’s initial data.
    // } WGpuBufferDescriptor;
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type BufferDescriptor =
        {
            Size : uint64
            Usage : BufferUsage
            MappedAtCreation : int
        }
       
    [<DllImport("WebGPU")>]
    extern int my_wgpu_device_create_buffer(int device, BufferDescriptor& desc)
       
    type WGPUCallbacks() =
        
        static let mutable currentId = 0n
        static let mutable callbacks = Dictionary<nativeint, int -> unit>()
        static let mutable callbacks2 = Dictionary<nativeint, int -> int -> string -> unit>()
        
        [<UnmanagedCallersOnly>]
        static member Callback(a : int, cbid : nativeint) =
            match callbacks.TryGetValue(cbid) with
            | (true, cb) ->
                callbacks.Remove cbid |> ignore
                cb a
            | _ ->
                printfn "BAD CALLBACK: %A" cbid

        [<UnmanagedCallersOnly>]
        static member Callback2(a : int, b : int, str : nativeint, cbid : nativeint) =
            match callbacks2.TryGetValue(cbid) with
            | (true, cb) ->
                callbacks2.Remove cbid |> ignore
                cb a b (Marshal.PtrToStringUTF8(str))
            | _ ->
                printfn "BAD CALLBACK: %A" cbid

        static member RegisterCallback(cb : int -> unit) =
            let id = currentId
            currentId <- currentId + 1n
            callbacks.Add(id, cb)
            id
    //typedef void (*WGpuDeviceLostCallback)(WGpuDevice device, WGPU_DEVICE_LOST_REASON deviceLostReason, const char *message NOTNULL, void *userData);

        static member RegisterCallback(cb : int -> int -> string -> unit) =
            let id = currentId
            currentId <- currentId + 1n
            callbacks2.Add(id, cb)
            id

    type WGPUDelegate = delegate of int * nativeint -> unit
    type WGPUDelegate2 = delegate of int * int * nativeint * nativeint -> unit

    let del = System.Delegate.CreateDelegate(typeof<WGPUDelegate>, typeof<WGPUCallbacks>.GetMethod "Callback")
    let delegatePtr = Marshal.GetFunctionPointerForDelegate(del)
    
    let del2 = System.Delegate.CreateDelegate(typeof<WGPUDelegate2>, typeof<WGPUCallbacks>.GetMethod "Callback2")
    let delegatePtr2 = Marshal.GetFunctionPointerForDelegate(del2)
