namespace WebGPU


type Queue(label : string, handle : int) =
    member x.Label = label
    member x.Handle = handle
