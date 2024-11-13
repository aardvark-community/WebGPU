namespace WebGPU

open System
open System.Text
open System.Threading.Tasks
open System.Runtime.InteropServices

#nowarn "9"
    
type Adapter private(handle : int) =
    
    let vendor, architecture, device, description =
        let data = Array.zeroCreate<byte> (4 * 512)
        use ptr = fixed data
        WebGPURaw.my_wgpu_adapter_get_info(handle, ptr)
        
        let vendor = Encoding.UTF8.GetString(data, 0, 512)
        let architecture = Encoding.UTF8.GetString(data, 512, 512)
        let device = Encoding.UTF8.GetString(data, 1024, 512)
        let description = Encoding.UTF8.GetString(data, 1536, 512)
        
        vendor, architecture, device, description
        
    let features =
        WebGPURaw.my_wgpu_adapter_or_device_get_features(handle) |> unbox<Features>
        
    let limits =
        let mutable limits = Unchecked.defaultof<Limits>
        WebGPURaw.my_wgpu_adapter_or_device_get_limits(handle, &limits)
        limits
        
    let isFallback =
        WebGPURaw.my_wgpu_adapter_is_fallback_adapter(handle) <> 0
        
    static member Request(options : AdapterOptions) =
        let tcs = TaskCompletionSource<Adapter>()
        
        if WebGPURaw.wgpuSupported() <> 0 then
            let mutable options =
                {
                    WebGPURaw.PowerPreference = unbox (int options.PowerPreference)
                    WebGPURaw.ForceFallbackAdapter = if options.ForceFallbackAdapter then 1 else 0
                }
            let cbid =
                WebGPURaw.WGPUCallbacks.RegisterCallback(fun handle ->
                    tcs.SetResult(new Adapter(handle))    
                )
            if WebGPURaw.my_navigator_gpu_request_adapter_async(&options, WebGPURaw.delegatePtr, cbid) = 0 then
                tcs.SetException(WebGPUException "Failed to request adapter")
        else
            tcs.SetException(WebGPUException "WebGPU is not supported")
        tcs.Task

    member x.Handle = handle
    member x.IsFallback = isFallback
    member x.Features = features
    member x.Vendor = vendor
    member x.Architecture = architecture
    member x.Device = device
    member x.Description = description
    member x.Limits = limits

    member x.RequestDevice(descriptor : DeviceDescriptor) =
        use pQueueLabel = fixed (Encoding.UTF8.GetBytes descriptor.DefaultQueue.Label)
        let tcs = TaskCompletionSource<Device>()
        if sizeof<nativeint> = 4 then
            let mutable desc =
                {
                    WebGPURaw.DefaultQueue = { WebGPURaw.Label = pQueueLabel; WebGPURaw.Dummy = 0u }
                    WebGPURaw.RequiredFeatures = descriptor.RequiredFeatures |> uint32
                    WebGPURaw.RequiredLimits = descriptor.RequiredLimits
                    WebGPURaw.UnusedPadding = 0u 
                }
                
            if descriptor.RequiredFeatures &&& features = descriptor.RequiredFeatures then
                if Limits.allSmallerOrEqual descriptor.RequiredLimits limits then
                    let cbid =
                        WebGPURaw.WGPUCallbacks.RegisterCallback(fun a ->
                            tcs.SetResult(new Device(descriptor, a))    
                        )
                    WebGPURaw.my_wgpu_adapter_request_device_async32(handle, &desc, WebGPURaw.delegatePtr, cbid)
            
                else
                    tcs.SetException(WebGPUException "unsupported limits")
            else
                tcs.SetException(WebGPUException "unsupported features")
       
        else
            let mutable desc =
                {
                    WebGPURaw.DeviceDescriptor64.DefaultQueue = { WebGPURaw.QueueDescriptor64.Label = pQueueLabel }
                    WebGPURaw.DeviceDescriptor64.RequiredFeatures = descriptor.RequiredFeatures |> uint32
                    WebGPURaw.DeviceDescriptor64.RequiredLimits = descriptor.RequiredLimits
                    WebGPURaw.DeviceDescriptor64.UnusedPadding = 0u 
                }
                
            if descriptor.RequiredFeatures &&& features = descriptor.RequiredFeatures then
                if Limits.allSmallerOrEqual descriptor.RequiredLimits limits then
                    let cbid =
                        WebGPURaw.WGPUCallbacks.RegisterCallback(fun a ->
                            tcs.SetResult(new Device(descriptor, a))    
                        )
                    WebGPURaw.my_wgpu_adapter_request_device_async64(handle, &desc, WebGPURaw.delegatePtr, cbid)
            
                else
                    tcs.SetException(WebGPUException "unsupported limits")
            else
                tcs.SetException(WebGPUException "unsupported features")
        tcs.Task
    
    member x.RequestDevice() =
        x.RequestDevice {
            RequiredFeatures = features
            RequiredLimits = limits
            DefaultQueue = { Label = "Main" } 
        }
