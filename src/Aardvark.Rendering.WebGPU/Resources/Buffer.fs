namespace Aardvark.Rendering.WebGPU

open System.Runtime.CompilerServices
open Microsoft.FSharp.NativeInterop
open Aardvark.Rendering
open global.WebGPU

#nowarn "9"

[<AbstractClass; Sealed>]
type BufferExtensions private() =
    
    [<Extension>]
    static member CopyIBufferToBuffer (encoder : CommandEncoder, src : IBuffer, srcOffset : int64, dst : Buffer, dstOffset : int64, size : int64) =
        match src with
        | :? INativeBuffer as src ->
            src.Use (fun srcPtr ->
                let size =
                    if size <= 0L then int64 src.SizeInBytes
                    else size
                use tmp =
                    encoder.Device.CreateBuffer {
                        Label = null
                        Next = null
                        Size = size
                        Usage = BufferUsage.MapWrite ||| BufferUsage.CopySrc
                        MappedAtCreation = true
                    }
                    
                let dstPtr = tmp.GetMappedRange(0L, size)
                let srcSpan = System.Span<byte>(NativePtr.toVoidPtr (NativePtr.ofNativeInt<byte> (srcPtr + nativeint srcOffset)), int size)
                let dstSpan = System.Span<byte>(NativePtr.toVoidPtr (NativePtr.ofNativeInt<byte> dstPtr), int size)
                srcSpan.CopyTo(dstSpan)
                tmp.Unmap()
                
                encoder.CopyBufferToBuffer(tmp, 0L, dst, dstOffset, size)
                
            )
        | b ->
            failwith $"CopyIBufferToBuffer: src buffer is a bad buffer: {b}"

    [<Extension>]
    static member CopyIBufferToBuffer (encoder : CommandEncoder, src : IBuffer, dst : Buffer) =
        encoder.CopyIBufferToBuffer(src, 0L, dst, 0L, 0L)
        
    [<Extension>]
    static member GetSizeInBytes(buffer : IBuffer) =
        match buffer with
        | :? INativeBuffer as b -> int64 b.SizeInBytes
        | _ -> failwith "GetSizeInBytes: buffer is not a native buffer"
        