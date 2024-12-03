namespace WebGPU

open System
open System.Threading.Tasks
open System.Runtime.CompilerServices
open Microsoft.FSharp.NativeInterop
open System.Diagnostics
open Aardvark.Base
open WebGPU

#nowarn "9"


[<DebuggerTypeProxy(typeof<BufferRangeProxy>)>]
type BufferRange(buffer : Buffer, offset : int64, size : int64) =
    member x.Buffer = buffer
    member x.Offset = offset
    member x.Size = size
    
    override this.GetHashCode() =
        HashCode.Combine(hash buffer, hash offset, hash size)
        
    override this.Equals(obj) =
        match obj with
        | :? BufferRange as o -> o.Buffer = buffer && o.Offset = offset && o.Size = size
        | _ -> false
        
    override x.ToString() =
        sprintf "BufferRange(0x%08X, %A, %A)" buffer.Handle offset size
    
and BufferRangeProxy(range : BufferRange) =
    let buffer = range.Buffer
    let offset = range.Offset
    let size = range.Size
    
    let content = buffer.ToByteArray(offset, size)
    member x.Buffer = range.Buffer
    member x.Offset = range.Offset
    member x.Size = range.Size
       
    member x.UInt8Array = content
    member x.UInt16Array = 
        use ptr = fixed content
        let ptr = NativePtr.ofNativeInt<uint16> (NativePtr.toNativeInt ptr)
        Array.init (content.Length / 2) (fun i -> NativePtr.get ptr i)
    member x.UInt32Array = 
        use ptr = fixed content
        let ptr = NativePtr.ofNativeInt<uint32> (NativePtr.toNativeInt ptr)
        Array.init (content.Length / 4) (fun i -> NativePtr.get ptr i)
    member x.UInt64Array = 
        use ptr = fixed content
        let ptr = NativePtr.ofNativeInt<uint64> (NativePtr.toNativeInt ptr)
        Array.init (content.Length / 8) (fun i -> NativePtr.get ptr i)
    member x.Int8Array = 
        use ptr = fixed content
        let ptr = NativePtr.ofNativeInt<int8> (NativePtr.toNativeInt ptr)
        Array.init (content.Length) (fun i -> NativePtr.get ptr i)
    member x.Int16Array = 
        use ptr = fixed content
        let ptr = NativePtr.ofNativeInt<int16> (NativePtr.toNativeInt ptr)
        Array.init (content.Length / 2) (fun i -> NativePtr.get ptr i)
    member x.Int32Array = 
        use ptr = fixed content
        let ptr = NativePtr.ofNativeInt<int32> (NativePtr.toNativeInt ptr)
        Array.init (content.Length / 4) (fun i -> NativePtr.get ptr i)
    member x.Int64Array = 
        use ptr = fixed content
        let ptr = NativePtr.ofNativeInt<int64> (NativePtr.toNativeInt ptr)
        Array.init (content.Length / 8) (fun i -> NativePtr.get ptr i)
    member x.Float32Array = 
        use ptr = fixed content
        let ptr = NativePtr.ofNativeInt<float32> (NativePtr.toNativeInt ptr)
        Array.init (content.Length / 4) (fun i -> NativePtr.get ptr i)
    member x.Float64Array = 
        use ptr = fixed content
        let ptr = NativePtr.ofNativeInt<double> (NativePtr.toNativeInt ptr)
        Array.init (content.Length / 8) (fun i -> NativePtr.get ptr i)
  

[<AbstractClass; Sealed>]
type WebGPUBufferExtensions private() =
    
    [<Extension>]
    static member Mapped<'r>(buffer : Buffer, mode : MapMode, offset : int64, size : int64, action : nativeint -> 'r) =
        match buffer.MapState with
        | BufferMapState.Mapped ->
            let ptr =
                match mode with
                | MapMode.Write -> buffer.GetMappedRange(offset, size)
                | _ -> buffer.GetConstMappedRange(offset, size)
            try action ptr |> Task.FromResult
            finally buffer.Unmap()
        | _ ->
            let tcs = TaskCompletionSource<_>()
            
            let info : BufferMapCallbackInfo2 =
                {
                    Mode = CallbackMode.AllowSpontaneous
                    Callback = BufferMapCallback2(fun d status msg ->
                        d.Dispose()
                        match status with
                        | MapAsyncStatus.Success ->
                            let ptr = 
                                match mode with
                                | MapMode.Write -> buffer.GetMappedRange(offset, size)
                                | _ -> buffer.GetConstMappedRange(offset, size)
                            let res = action ptr
                            buffer.Unmap()
                            tcs.SetResult res
                        | s ->
                            tcs.SetException (Exception (sprintf "could not map buffer: %A" s))  
                    )
                }
            
            buffer.MapAsync2(mode, offset, size, info) |> ignore
           
            tcs.Task
    
    [<Extension>]
    static member Mapped<'r>(buffer : Buffer, mode : MapMode, action : nativeint -> 'r) =
        buffer.Mapped(mode, 0L, buffer.Size, action)
         
    [<Extension>]
    static member WriteBuffer<'a when 'a : unmanaged>(this : Queue, buffer : Buffer, data : System.ReadOnlySpan<'a>) =
        use ptr = fixed data
        this.WriteBuffer(buffer, 0L, NativePtr.toNativeInt ptr, int64 data.Length * int64 sizeof<'a>)

    [<Extension>]
    static member WriteBuffer<'a when 'a : unmanaged>(this : Queue, buffer : Buffer, data : System.Span<'a>) =
        let data : System.ReadOnlySpan<'a> = System.Span<'a>.op_Implicit data
        this.WriteBuffer(buffer, data)

    [<Extension>]
    static member WriteBuffer<'a when 'a : unmanaged>(this : Queue, buffer : Buffer, data : System.Memory<'a>) =
        this.WriteBuffer(buffer, data.Span)
        
    [<Extension>]
    static member WriteBuffer<'a when 'a : unmanaged>(this : Queue, buffer : Buffer, data : System.ReadOnlyMemory<'a>) =
        this.WriteBuffer(buffer, data.Span)

    [<Extension>]
    static member WriteBuffer<'a when 'a : unmanaged>(this : Queue, buffer : Buffer, data : 'a[]) =
        this.WriteBuffer(buffer, System.Span<_>(data))

    
    [<Extension>]
    static member Upload<'a when 'a : unmanaged>(this : CommandEncoder, src : System.ReadOnlySpan<'a>, dst : Buffer, dstOffset : int64) =
        use tmp = dst.Device.CreateBuffer { Next = null; Label = null; Size = int64 src.Length; Usage = BufferUsage.MapWrite ||| BufferUsage.CopySrc; MappedAtCreation = true }
        let dstPtr = tmp.GetMappedRange(0L, int64 src.Length)
        let dstSpan = System.Span<'a>(NativePtr.toVoidPtr (NativePtr.ofNativeInt<byte> dstPtr), src.Length)
        src.CopyTo(dstSpan)
        tmp.Unmap()
        this.CopyBufferToBuffer(tmp, 0L, dst, dstOffset, int64 src.Length)
 
    [<Extension>]
    static member Upload<'a when 'a : unmanaged>(this : CommandEncoder, src : System.ReadOnlySpan<'a>, dst : Buffer) =
        this.Upload(src, dst, 0L)
        
    [<Extension>]
    static member Upload<'a when 'a : unmanaged>(this : CommandEncoder, src : System.Span<'a>, dst : Buffer, dstOffset : int64) =
        let src : System.ReadOnlySpan<'a> = System.Span<'a>.op_Implicit src
        this.Upload(src, dst, dstOffset)
        
    [<Extension>]
    static member Upload<'a when 'a : unmanaged>(this : CommandEncoder, src : System.Span<'a>, dst : Buffer) =
        this.Upload(src, dst, 0L)
        
    [<Extension>]
    static member Upload<'a when 'a : unmanaged>(this : CommandEncoder, src : System.ReadOnlyMemory<'a>, dst : Buffer, dstOffset : int64) =
        this.Upload(src.Span, dst, dstOffset)
        
    [<Extension>]
    static member Upload<'a when 'a : unmanaged>(this : CommandEncoder, src : System.ReadOnlyMemory<'a>, dst : Buffer) =
        this.Upload(src.Span, dst, 0L)
        
    [<Extension>]
    static member Upload<'a when 'a : unmanaged>(this : CommandEncoder, src : System.Memory<'a>, dst : Buffer, dstOffset : int64) =
        this.Upload(src.Span, dst, dstOffset)
        
    [<Extension>]
    static member Upload<'a when 'a : unmanaged>(this : CommandEncoder, src : System.Memory<'a>, dst : Buffer) =
        this.Upload(src, dst, 0L)
        
    [<Extension>]
    static member Upload<'a when 'a : unmanaged>(this : CommandEncoder, src : 'a[], dst : Buffer, dstOffset : int64) =
        this.Upload(System.ReadOnlySpan<'a>(src), dst, dstOffset)
        
    [<Extension>]
    static member Upload<'a when 'a : unmanaged>(this : CommandEncoder, src : 'a[], dst : Buffer) =
        this.Upload(src, dst, 0L)
        
    
    [<Extension>]
    static member Sub(buffer : Buffer, offset : int64, size : int64) =
        if offset < 0L then raise <| ArgumentOutOfRangeException $"offset must be non-negative: {offset}"
        if size < 0L then raise <| ArgumentOutOfRangeException $"size must be non-negative: {size}"
        if offset + size > buffer.Size then raise <| ArgumentOutOfRangeException $"size is out of range: {offset}, {size}, {buffer.Size}"
        if offset &&& 3L <> 0L then raise <| ArgumentOutOfRangeException $"offset must be a multiple of 4: {offset}"
        if size &&& 3L <> 0L then raise <| ArgumentOutOfRangeException $"size must be a multiple of 4: {offset}"
        BufferRange(buffer, offset, size)

    [<Extension>]
    static member Sub(buffer : Buffer, offset : int64) =
        buffer.Sub(offset, buffer.Size - offset)

    [<Extension>]
    static member Sub(range : BufferRange, offset : int64, size : int64) =
        range.Buffer.Sub(range.Offset + offset, size)

    [<Extension>]
    static member Sub(range : BufferRange, offset : int64) =
        range.Sub(offset, range.Size - offset)

    
    [<Extension>]
    static member GetSlice(buffer : Buffer, min : option<int64>, max : option<int64>) =
        match min with
        | Some min ->
            match max with
            | Some max -> buffer.Sub(min, 1L + max - min)
            | None -> buffer.Sub(min)
        | None ->
            match max with
            | Some max -> buffer.Sub(0L, 1L + max)
            | None -> buffer.Sub(0L, buffer.Size)
            
    [<Extension>]
    static member GetSlice(buffer : Buffer, min : option<int>, max : option<int>) =
        match min with
        | Some min ->
            match max with
            | Some max -> buffer.Sub(min, 1L + int64 max - int64 min)
            | None -> buffer.Sub(int64 min)
        | None ->
            match max with
            | Some max -> buffer.Sub(0L, 1L + int64 max)
            | None -> buffer.Sub(0L, buffer.Size)
            
    
    [<Extension>]
    static member GetSlice(range : BufferRange, min : option<int64>, max : option<int64>) =
        match min with
        | Some min ->
            match max with
            | Some max -> range.Sub(min, 1L + max - min)
            | None -> range.Sub(min)
        | None ->
            match max with
            | Some max -> range.Sub(0L, 1L + max)
            | None -> range.Sub(0L, range.Size)
            
    [<Extension>]
    static member GetSlice(range : BufferRange, min : option<int>, max : option<int>) =
        match min with
        | Some min ->
            match max with
            | Some max -> range.Sub(min, 1L + int64 max - int64 min)
            | None -> range.Sub(int64 min)
        | None ->
            match max with
            | Some max -> range.Sub(0L, 1L + int64 max)
            | None -> range.Sub(0L, range.Size)
            
           

