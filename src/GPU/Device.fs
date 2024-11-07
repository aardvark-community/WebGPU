namespace WebGPU

type Device internal(desc : DeviceDescriptor, handle : int) as this =
    
    let features =
        WebGPURaw.my_wgpu_adapter_or_device_get_features(handle) |> unbox<Features>
    
    let limits =
        let mutable limits = Unchecked.defaultof<Limits>
        WebGPURaw.my_wgpu_adapter_or_device_get_limits(handle, &limits)
        limits
    
    let queue =
        Queue(desc.DefaultQueue.Label, WebGPURaw.my_wgpu_device_get_queue(handle))
    
    member x.Handle = handle
    member x.Features = features
    member x.Limits = limits
    
    member x.Queue = queue
