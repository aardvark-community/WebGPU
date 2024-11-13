namespace WebGPU

type BufferDescriptor =
    {
        Size : uint64
        Usage : BufferUsage
        MappedAtCreation : bool
    }

type Buffer(handle : int) =
    member x.Handle = handle