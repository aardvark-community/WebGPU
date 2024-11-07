namespace WebGPU

open System.Runtime.InteropServices
open System.Collections.Generic


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
    
    
    type WGPUCallbacks() =
        
        static let mutable currentId = 0n
        static let mutable callbacks = Dictionary<nativeint, int -> unit>()
        
        [<UnmanagedCallersOnly>]
        static member Callback(a : int, cbid : nativeint) =
            match callbacks.TryGetValue(cbid) with
            | (true, cb) ->
                callbacks.Remove cbid |> ignore
                cb a
            | _ ->
                printfn "BAD CALLBACK: %A" cbid

        static member RegisterCallback(cb : int -> unit) =
            let id = currentId
            currentId <- currentId + 1n
            callbacks.Add(id, cb)
            id

    type WGPUDelegate = delegate of int * nativeint -> unit

    