namespace WebGPU

open System

type Device internal(desc : DeviceDescriptor, handle : int) =
    let mutable handle = handle
    
    let features =
        WebGPURaw.my_wgpu_adapter_or_device_get_features(handle) |> unbox<Features>
    
    let limits =
        let mutable limits = Unchecked.defaultof<Limits>
        WebGPURaw.my_wgpu_adapter_or_device_get_limits(handle, &limits)
        limits
    
    let queue =
        Queue(desc.DefaultQueue.Label, WebGPURaw.my_wgpu_device_get_queue(handle))
    
    let lostEvent =
        let e = Event<DeviceLostReason * string>()
        let cbid = WebGPURaw.WGPUCallbacks.RegisterCallback(fun _ reason message -> e.Trigger((unbox<DeviceLostReason> reason, message)))
        WebGPURaw.my_wgpu_device_set_lost_callback(handle, WebGPURaw.delegatePtr2, cbid)
        e
        
    let uncapturedError =
        let e = Event<ErrorType * string>()
        let cbid = WebGPURaw.WGPUCallbacks.RegisterCallback(fun _ reason message -> e.Trigger((unbox<ErrorType> reason, message)))
        WebGPURaw.my_wgpu_device_set_uncapturederror_callback(handle, WebGPURaw.delegatePtr2, cbid)
        e
    
    member x.Handle = handle
    member x.Features = features
    member x.Limits = limits
    
    member x.Queue = queue
    
    [<CLIEvent>]
    member x.Lost = lostEvent.Publish

    [<CLIEvent>]
    member x.UncapturedError = uncapturedError.Publish
    
    member x.Dispose() =
        if handle <> 0 then 
            WebGPURaw.my_wgpu_object_destroy(handle)
            handle <- 0
    
    member x.CreateBuffer(desc : BufferDescriptor) =
        let mutable desc =
            {
                WebGPURaw.BufferDescriptor.Size = desc.Size
                WebGPURaw.BufferDescriptor.Usage = desc.Usage
                WebGPURaw.BufferDescriptor.MappedAtCreation = if desc.MappedAtCreation then 1 else 0 
            }
        let handle = WebGPURaw.my_wgpu_device_create_buffer(handle, &desc)
        Buffer(handle)
        
    interface IDisposable with
        member x.Dispose() = x.Dispose()