namespace WebGPU
open System
open System.Text
open System.Diagnostics
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop
open WebGPU.Raw
#nowarn "9"
#nowarn "26"
#nowarn "1182"
[<AutoOpen>]
module private NativeUtilities =
    let inline nsize<'a> = nativeint sizeof<'a>
    let inline sum (n : int) (ptr : nativeptr<'x>) ([<InlineIfLambda>] action : 'x -> 'a) =
        let mutable res = LanguagePrimitives.GenericZero<'a>
        for i in 0 .. n - 1 do res <- res + action (NativePtr.get ptr i)
        res
    let inline step (aux : byref<nativeint>) (size : nativeint) =
        let n = aux + size
        if n &&& 7n = 0n then aux <- n
        else aux <- (1n + (n >>> 3)) <<< 3
    let inline align8 (n : nativeint) =
        if n &&& 7n = 0n then n
        else (1n + (n >>> 3)) <<< 3
type CommandStreamCommand =
    | CommandEncoderFinish = 0
    | CommandEncoderBeginComputePass = 1
    | CommandEncoderBeginRenderPass = 2
    | CommandEncoderCopyBufferToBuffer = 3
    | CommandEncoderCopyBufferToTexture = 4
    | CommandEncoderCopyTextureToBuffer = 5
    | CommandEncoderCopyTextureToTexture = 6
    | CommandEncoderClearBuffer = 7
    | CommandEncoderInjectValidationError = 8
    | CommandEncoderInsertDebugMarker = 9
    | CommandEncoderPopDebugGroup = 10
    | CommandEncoderPushDebugGroup = 11
    | CommandEncoderResolveQuerySet = 12
    | CommandEncoderWriteBuffer = 13
    | CommandEncoderWriteTimestamp = 14
    | ComputePassEncoderInsertDebugMarker = 15
    | ComputePassEncoderPopDebugGroup = 16
    | ComputePassEncoderPushDebugGroup = 17
    | ComputePassEncoderSetPipeline = 18
    | ComputePassEncoderSetBindGroup = 19
    | ComputePassEncoderWriteTimestamp = 20
    | ComputePassEncoderDispatchWorkgroups = 21
    | ComputePassEncoderDispatchWorkgroupsIndirect = 22
    | ComputePassEncoderEnd = 23
    | ComputePassEncoderSetImmediateData = 24
    | RenderPassEncoderSetPipeline = 25
    | RenderPassEncoderSetBindGroup = 26
    | RenderPassEncoderDraw = 27
    | RenderPassEncoderDrawIndexed = 28
    | RenderPassEncoderDrawIndirect = 29
    | RenderPassEncoderDrawIndexedIndirect = 30
    | RenderPassEncoderMultiDrawIndirect = 31
    | RenderPassEncoderMultiDrawIndexedIndirect = 32
    | RenderPassEncoderExecuteBundles = 33
    | RenderPassEncoderInsertDebugMarker = 34
    | RenderPassEncoderPopDebugGroup = 35
    | RenderPassEncoderPushDebugGroup = 36
    | RenderPassEncoderSetStencilReference = 37
    | RenderPassEncoderSetBlendConstant = 38
    | RenderPassEncoderSetViewport = 39
    | RenderPassEncoderSetScissorRect = 40
    | RenderPassEncoderSetVertexBuffer = 41
    | RenderPassEncoderSetIndexBuffer = 42
    | RenderPassEncoderBeginOcclusionQuery = 43
    | RenderPassEncoderEndOcclusionQuery = 44
    | RenderPassEncoderWriteTimestamp = 45
    | RenderPassEncoderPixelLocalStorageBarrier = 46
    | RenderPassEncoderEnd = 47
    | RenderPassEncoderSetImmediateData = 48
module private RawCommandStreamExt =
    [<DllImport("WebGPUNative")>]
    extern void gpuRunInterpreter(void* cmd, void* start, void* e)
type RawCommandStream() =
    let memory = Marshal.AllocHGlobal(32 <<< 20)
    let mutable ptr = memory
    member this.CommandEncoderFinish(descriptor : nativeptr<CommandBufferDescriptor>) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.CommandEncoderFinish
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint>
        let mutable aux = ptr + argSize
        // descriptor
        let _descriptor = NativePtr.read descriptor
        let _descriptorStorage = aux
        step &aux nsize<CommandBufferDescriptor>
        _descriptor.CopyTo(_descriptorStorage, &aux)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_descriptorStorage - ptr)
        step &ptr nsize<nativeint>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.CommandEncoderBeginComputePass(descriptor : nativeptr<ComputePassDescriptor>) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.CommandEncoderBeginComputePass
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint>
        let mutable aux = ptr + argSize
        // descriptor
        let _descriptor = NativePtr.read descriptor
        let _descriptorStorage = aux
        step &aux nsize<ComputePassDescriptor>
        _descriptor.CopyTo(_descriptorStorage, &aux)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_descriptorStorage - ptr)
        step &ptr nsize<nativeint>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.CommandEncoderBeginRenderPass(descriptor : nativeptr<RenderPassDescriptor>) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.CommandEncoderBeginRenderPass
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint>
        let mutable aux = ptr + argSize
        // descriptor
        let _descriptor = NativePtr.read descriptor
        let _descriptorStorage = aux
        step &aux nsize<RenderPassDescriptor>
        _descriptor.CopyTo(_descriptorStorage, &aux)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_descriptorStorage - ptr)
        step &ptr nsize<nativeint>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.CommandEncoderCopyBufferToBuffer(source : nativeint, sourceOffset : uint64, destination : nativeint, destinationOffset : uint64, size : uint64) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.CommandEncoderCopyBufferToBuffer
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint> * 2n + align8 nsize<uint64> * 3n
        let mutable aux = ptr + argSize
        // source
        NativePtr.write (NativePtr.ofNativeInt ptr) source
        step &ptr nsize<nativeint>
        // sourceOffset
        NativePtr.write (NativePtr.ofNativeInt ptr) sourceOffset
        step &ptr nsize<uint64>
        // destination
        NativePtr.write (NativePtr.ofNativeInt ptr) destination
        step &ptr nsize<nativeint>
        // destinationOffset
        NativePtr.write (NativePtr.ofNativeInt ptr) destinationOffset
        step &ptr nsize<uint64>
        // size
        NativePtr.write (NativePtr.ofNativeInt ptr) size
        step &ptr nsize<uint64>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.CommandEncoderCopyBufferToTexture(source : nativeptr<TexelCopyBufferInfo>, destination : nativeptr<TexelCopyTextureInfo>, copySize : nativeptr<Extent3D>) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.CommandEncoderCopyBufferToTexture
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint> * 3n
        let mutable aux = ptr + argSize
        // source
        let _source = NativePtr.read source
        let _sourceStorage = aux
        step &aux nsize<TexelCopyBufferInfo>
        _source.CopyTo(_sourceStorage, &aux)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_sourceStorage - ptr)
        step &ptr nsize<nativeint>
        // destination
        let _destination = NativePtr.read destination
        let _destinationStorage = aux
        step &aux nsize<TexelCopyTextureInfo>
        _destination.CopyTo(_destinationStorage, &aux)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_destinationStorage - ptr)
        step &ptr nsize<nativeint>
        // copySize
        let _copySize = NativePtr.read copySize
        let _copySizeStorage = aux
        step &aux nsize<Extent3D>
        _copySize.CopyTo(_copySizeStorage, &aux)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_copySizeStorage - ptr)
        step &ptr nsize<nativeint>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.CommandEncoderCopyTextureToBuffer(source : nativeptr<TexelCopyTextureInfo>, destination : nativeptr<TexelCopyBufferInfo>, copySize : nativeptr<Extent3D>) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.CommandEncoderCopyTextureToBuffer
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint> * 3n
        let mutable aux = ptr + argSize
        // source
        let _source = NativePtr.read source
        let _sourceStorage = aux
        step &aux nsize<TexelCopyTextureInfo>
        _source.CopyTo(_sourceStorage, &aux)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_sourceStorage - ptr)
        step &ptr nsize<nativeint>
        // destination
        let _destination = NativePtr.read destination
        let _destinationStorage = aux
        step &aux nsize<TexelCopyBufferInfo>
        _destination.CopyTo(_destinationStorage, &aux)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_destinationStorage - ptr)
        step &ptr nsize<nativeint>
        // copySize
        let _copySize = NativePtr.read copySize
        let _copySizeStorage = aux
        step &aux nsize<Extent3D>
        _copySize.CopyTo(_copySizeStorage, &aux)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_copySizeStorage - ptr)
        step &ptr nsize<nativeint>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.CommandEncoderCopyTextureToTexture(source : nativeptr<TexelCopyTextureInfo>, destination : nativeptr<TexelCopyTextureInfo>, copySize : nativeptr<Extent3D>) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.CommandEncoderCopyTextureToTexture
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint> * 3n
        let mutable aux = ptr + argSize
        // source
        let _source = NativePtr.read source
        let _sourceStorage = aux
        step &aux nsize<TexelCopyTextureInfo>
        _source.CopyTo(_sourceStorage, &aux)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_sourceStorage - ptr)
        step &ptr nsize<nativeint>
        // destination
        let _destination = NativePtr.read destination
        let _destinationStorage = aux
        step &aux nsize<TexelCopyTextureInfo>
        _destination.CopyTo(_destinationStorage, &aux)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_destinationStorage - ptr)
        step &ptr nsize<nativeint>
        // copySize
        let _copySize = NativePtr.read copySize
        let _copySizeStorage = aux
        step &aux nsize<Extent3D>
        _copySize.CopyTo(_copySizeStorage, &aux)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_copySizeStorage - ptr)
        step &ptr nsize<nativeint>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.CommandEncoderClearBuffer(buffer : nativeint, offset : uint64, size : uint64) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.CommandEncoderClearBuffer
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint> + align8 nsize<uint64> * 2n
        let mutable aux = ptr + argSize
        // buffer
        NativePtr.write (NativePtr.ofNativeInt ptr) buffer
        step &ptr nsize<nativeint>
        // offset
        NativePtr.write (NativePtr.ofNativeInt ptr) offset
        step &ptr nsize<uint64>
        // size
        NativePtr.write (NativePtr.ofNativeInt ptr) size
        step &ptr nsize<uint64>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.CommandEncoderInjectValidationError(message : StringView) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.CommandEncoderInjectValidationError
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<StringView>
        let mutable aux = ptr + argSize
        // message
        message.CopyTo(ptr, &aux)
        step &ptr nsize<StringView>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.CommandEncoderInsertDebugMarker(markerLabel : StringView) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.CommandEncoderInsertDebugMarker
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<StringView>
        let mutable aux = ptr + argSize
        // markerLabel
        markerLabel.CopyTo(ptr, &aux)
        step &ptr nsize<StringView>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.CommandEncoderPopDebugGroup() : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.CommandEncoderPopDebugGroup
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = 0n
        let mutable aux = ptr + argSize
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.CommandEncoderPushDebugGroup(groupLabel : StringView) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.CommandEncoderPushDebugGroup
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<StringView>
        let mutable aux = ptr + argSize
        // groupLabel
        groupLabel.CopyTo(ptr, &aux)
        step &ptr nsize<StringView>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.CommandEncoderResolveQuerySet(querySet : nativeint, firstQuery : uint32, queryCount : uint32, destination : nativeint, destinationOffset : uint64) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.CommandEncoderResolveQuerySet
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint> * 2n + align8 nsize<uint32> * 2n + align8 nsize<uint64>
        let mutable aux = ptr + argSize
        // querySet
        NativePtr.write (NativePtr.ofNativeInt ptr) querySet
        step &ptr nsize<nativeint>
        // firstQuery
        NativePtr.write (NativePtr.ofNativeInt ptr) firstQuery
        step &ptr nsize<uint32>
        // queryCount
        NativePtr.write (NativePtr.ofNativeInt ptr) queryCount
        step &ptr nsize<uint32>
        // destination
        NativePtr.write (NativePtr.ofNativeInt ptr) destination
        step &ptr nsize<nativeint>
        // destinationOffset
        NativePtr.write (NativePtr.ofNativeInt ptr) destinationOffset
        step &ptr nsize<uint64>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.CommandEncoderWriteBuffer(buffer : nativeint, bufferOffset : uint64, data : nativeptr<uint8>, size : uint64) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.CommandEncoderWriteBuffer
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint> * 2n + align8 nsize<uint64> * 2n
        let mutable aux = ptr + argSize
        // buffer
        NativePtr.write (NativePtr.ofNativeInt ptr) buffer
        step &ptr nsize<nativeint>
        // bufferOffset
        NativePtr.write (NativePtr.ofNativeInt ptr) bufferOffset
        step &ptr nsize<uint64>
        // data
        let _dataStorage = aux
        let _dataSize = nsize<uint8> * nativeint size
        step &aux _dataSize
        let srcSpan = System.Span<byte>(NativePtr.toVoidPtr data, int _dataSize)
        let dstSpan = System.Span<byte>(NativePtr.toVoidPtr (NativePtr.ofNativeInt<byte> _dataStorage), int _dataSize)
        srcSpan.CopyTo(dstSpan)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_dataStorage - ptr)
        step &ptr nsize<nativeint>
        // size
        NativePtr.write (NativePtr.ofNativeInt ptr) size
        step &ptr nsize<uint64>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.CommandEncoderWriteTimestamp(querySet : nativeint, queryIndex : uint32) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.CommandEncoderWriteTimestamp
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint> + align8 nsize<uint32>
        let mutable aux = ptr + argSize
        // querySet
        NativePtr.write (NativePtr.ofNativeInt ptr) querySet
        step &ptr nsize<nativeint>
        // queryIndex
        NativePtr.write (NativePtr.ofNativeInt ptr) queryIndex
        step &ptr nsize<uint32>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.ComputePassEncoderInsertDebugMarker(markerLabel : StringView) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.ComputePassEncoderInsertDebugMarker
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<StringView>
        let mutable aux = ptr + argSize
        // markerLabel
        markerLabel.CopyTo(ptr, &aux)
        step &ptr nsize<StringView>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.ComputePassEncoderPopDebugGroup() : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.ComputePassEncoderPopDebugGroup
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = 0n
        let mutable aux = ptr + argSize
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.ComputePassEncoderPushDebugGroup(groupLabel : StringView) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.ComputePassEncoderPushDebugGroup
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<StringView>
        let mutable aux = ptr + argSize
        // groupLabel
        groupLabel.CopyTo(ptr, &aux)
        step &ptr nsize<StringView>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.ComputePassEncoderSetPipeline(pipeline : nativeint) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.ComputePassEncoderSetPipeline
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint>
        let mutable aux = ptr + argSize
        // pipeline
        NativePtr.write (NativePtr.ofNativeInt ptr) pipeline
        step &ptr nsize<nativeint>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.ComputePassEncoderSetBindGroup(groupIndex : uint32, group : nativeint, dynamicOffsetCount : unativeint, dynamicOffsets : nativeptr<uint32>) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.ComputePassEncoderSetBindGroup
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<uint32> + align8 nsize<nativeint> * 2n + align8 nsize<unativeint>
        let mutable aux = ptr + argSize
        // groupIndex
        NativePtr.write (NativePtr.ofNativeInt ptr) groupIndex
        step &ptr nsize<uint32>
        // group
        NativePtr.write (NativePtr.ofNativeInt ptr) group
        step &ptr nsize<nativeint>
        // dynamicOffsetCount
        NativePtr.write (NativePtr.ofNativeInt ptr) dynamicOffsetCount
        step &ptr nsize<unativeint>
        // dynamicOffsets
        let _dynamicOffsetsStorage = aux
        let _dynamicOffsetsSize = nsize<uint32> * nativeint dynamicOffsetCount
        step &aux _dynamicOffsetsSize
        let srcSpan = System.Span<byte>(NativePtr.toVoidPtr dynamicOffsets, int _dynamicOffsetsSize)
        let dstSpan = System.Span<byte>(NativePtr.toVoidPtr (NativePtr.ofNativeInt<byte> _dynamicOffsetsStorage), int _dynamicOffsetsSize)
        srcSpan.CopyTo(dstSpan)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_dynamicOffsetsStorage - ptr)
        step &ptr nsize<nativeint>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.ComputePassEncoderWriteTimestamp(querySet : nativeint, queryIndex : uint32) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.ComputePassEncoderWriteTimestamp
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint> + align8 nsize<uint32>
        let mutable aux = ptr + argSize
        // querySet
        NativePtr.write (NativePtr.ofNativeInt ptr) querySet
        step &ptr nsize<nativeint>
        // queryIndex
        NativePtr.write (NativePtr.ofNativeInt ptr) queryIndex
        step &ptr nsize<uint32>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.ComputePassEncoderDispatchWorkgroups(workgroupCountX : uint32, workgroupCountY : uint32, workgroupCountZ : uint32) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.ComputePassEncoderDispatchWorkgroups
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<uint32> * 3n
        let mutable aux = ptr + argSize
        // workgroupCountX
        NativePtr.write (NativePtr.ofNativeInt ptr) workgroupCountX
        step &ptr nsize<uint32>
        // workgroupCountY
        NativePtr.write (NativePtr.ofNativeInt ptr) workgroupCountY
        step &ptr nsize<uint32>
        // workgroupCountZ
        NativePtr.write (NativePtr.ofNativeInt ptr) workgroupCountZ
        step &ptr nsize<uint32>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.ComputePassEncoderDispatchWorkgroupsIndirect(indirectBuffer : nativeint, indirectOffset : uint64) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.ComputePassEncoderDispatchWorkgroupsIndirect
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint> + align8 nsize<uint64>
        let mutable aux = ptr + argSize
        // indirectBuffer
        NativePtr.write (NativePtr.ofNativeInt ptr) indirectBuffer
        step &ptr nsize<nativeint>
        // indirectOffset
        NativePtr.write (NativePtr.ofNativeInt ptr) indirectOffset
        step &ptr nsize<uint64>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.ComputePassEncoderEnd() : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.ComputePassEncoderEnd
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = 0n
        let mutable aux = ptr + argSize
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.ComputePassEncoderSetImmediateData(offset : uint32, data : nativeint, size : unativeint) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.ComputePassEncoderSetImmediateData
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<uint32> + align8 nsize<nativeint> + align8 nsize<unativeint>
        let mutable aux = ptr + argSize
        // offset
        NativePtr.write (NativePtr.ofNativeInt ptr) offset
        step &ptr nsize<uint32>
        // data
        let _dataStorage = aux
        let _dataSize = nsize<unit> * nativeint size
        step &aux _dataSize
        let srcSpan = System.Span<byte>(NativePtr.toVoidPtr (NativePtr.ofNativeInt<byte> data), int _dataSize)
        let dstSpan = System.Span<byte>(NativePtr.toVoidPtr (NativePtr.ofNativeInt<byte> _dataStorage), int _dataSize)
        srcSpan.CopyTo(dstSpan)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_dataStorage - ptr)
        step &ptr nsize<nativeint>
        // size
        NativePtr.write (NativePtr.ofNativeInt ptr) size
        step &ptr nsize<unativeint>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderSetPipeline(pipeline : nativeint) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderSetPipeline
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint>
        let mutable aux = ptr + argSize
        // pipeline
        NativePtr.write (NativePtr.ofNativeInt ptr) pipeline
        step &ptr nsize<nativeint>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderSetBindGroup(groupIndex : uint32, group : nativeint, dynamicOffsetCount : unativeint, dynamicOffsets : nativeptr<uint32>) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderSetBindGroup
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<uint32> + align8 nsize<nativeint> * 2n + align8 nsize<unativeint>
        let mutable aux = ptr + argSize
        // groupIndex
        NativePtr.write (NativePtr.ofNativeInt ptr) groupIndex
        step &ptr nsize<uint32>
        // group
        NativePtr.write (NativePtr.ofNativeInt ptr) group
        step &ptr nsize<nativeint>
        // dynamicOffsetCount
        NativePtr.write (NativePtr.ofNativeInt ptr) dynamicOffsetCount
        step &ptr nsize<unativeint>
        // dynamicOffsets
        let _dynamicOffsetsStorage = aux
        let _dynamicOffsetsSize = nsize<uint32> * nativeint dynamicOffsetCount
        step &aux _dynamicOffsetsSize
        let srcSpan = System.Span<byte>(NativePtr.toVoidPtr dynamicOffsets, int _dynamicOffsetsSize)
        let dstSpan = System.Span<byte>(NativePtr.toVoidPtr (NativePtr.ofNativeInt<byte> _dynamicOffsetsStorage), int _dynamicOffsetsSize)
        srcSpan.CopyTo(dstSpan)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_dynamicOffsetsStorage - ptr)
        step &ptr nsize<nativeint>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderDraw(vertexCount : uint32, instanceCount : uint32, firstVertex : uint32, firstInstance : uint32) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderDraw
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<uint32> * 4n
        let mutable aux = ptr + argSize
        // vertexCount
        NativePtr.write (NativePtr.ofNativeInt ptr) vertexCount
        step &ptr nsize<uint32>
        // instanceCount
        NativePtr.write (NativePtr.ofNativeInt ptr) instanceCount
        step &ptr nsize<uint32>
        // firstVertex
        NativePtr.write (NativePtr.ofNativeInt ptr) firstVertex
        step &ptr nsize<uint32>
        // firstInstance
        NativePtr.write (NativePtr.ofNativeInt ptr) firstInstance
        step &ptr nsize<uint32>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderDrawIndexed(indexCount : uint32, instanceCount : uint32, firstIndex : uint32, baseVertex : int, firstInstance : uint32) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderDrawIndexed
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<uint32> * 4n + align8 nsize<int>
        let mutable aux = ptr + argSize
        // indexCount
        NativePtr.write (NativePtr.ofNativeInt ptr) indexCount
        step &ptr nsize<uint32>
        // instanceCount
        NativePtr.write (NativePtr.ofNativeInt ptr) instanceCount
        step &ptr nsize<uint32>
        // firstIndex
        NativePtr.write (NativePtr.ofNativeInt ptr) firstIndex
        step &ptr nsize<uint32>
        // baseVertex
        NativePtr.write (NativePtr.ofNativeInt ptr) baseVertex
        step &ptr nsize<int>
        // firstInstance
        NativePtr.write (NativePtr.ofNativeInt ptr) firstInstance
        step &ptr nsize<uint32>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderDrawIndirect(indirectBuffer : nativeint, indirectOffset : uint64) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderDrawIndirect
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint> + align8 nsize<uint64>
        let mutable aux = ptr + argSize
        // indirectBuffer
        NativePtr.write (NativePtr.ofNativeInt ptr) indirectBuffer
        step &ptr nsize<nativeint>
        // indirectOffset
        NativePtr.write (NativePtr.ofNativeInt ptr) indirectOffset
        step &ptr nsize<uint64>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderDrawIndexedIndirect(indirectBuffer : nativeint, indirectOffset : uint64) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderDrawIndexedIndirect
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint> + align8 nsize<uint64>
        let mutable aux = ptr + argSize
        // indirectBuffer
        NativePtr.write (NativePtr.ofNativeInt ptr) indirectBuffer
        step &ptr nsize<nativeint>
        // indirectOffset
        NativePtr.write (NativePtr.ofNativeInt ptr) indirectOffset
        step &ptr nsize<uint64>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderMultiDrawIndirect(indirectBuffer : nativeint, indirectOffset : uint64, maxDrawCount : uint32, drawCountBuffer : nativeint, drawCountBufferOffset : uint64) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderMultiDrawIndirect
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint> * 2n + align8 nsize<uint64> * 2n + align8 nsize<uint32>
        let mutable aux = ptr + argSize
        // indirectBuffer
        NativePtr.write (NativePtr.ofNativeInt ptr) indirectBuffer
        step &ptr nsize<nativeint>
        // indirectOffset
        NativePtr.write (NativePtr.ofNativeInt ptr) indirectOffset
        step &ptr nsize<uint64>
        // maxDrawCount
        NativePtr.write (NativePtr.ofNativeInt ptr) maxDrawCount
        step &ptr nsize<uint32>
        // drawCountBuffer
        NativePtr.write (NativePtr.ofNativeInt ptr) drawCountBuffer
        step &ptr nsize<nativeint>
        // drawCountBufferOffset
        NativePtr.write (NativePtr.ofNativeInt ptr) drawCountBufferOffset
        step &ptr nsize<uint64>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderMultiDrawIndexedIndirect(indirectBuffer : nativeint, indirectOffset : uint64, maxDrawCount : uint32, drawCountBuffer : nativeint, drawCountBufferOffset : uint64) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderMultiDrawIndexedIndirect
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint> * 2n + align8 nsize<uint64> * 2n + align8 nsize<uint32>
        let mutable aux = ptr + argSize
        // indirectBuffer
        NativePtr.write (NativePtr.ofNativeInt ptr) indirectBuffer
        step &ptr nsize<nativeint>
        // indirectOffset
        NativePtr.write (NativePtr.ofNativeInt ptr) indirectOffset
        step &ptr nsize<uint64>
        // maxDrawCount
        NativePtr.write (NativePtr.ofNativeInt ptr) maxDrawCount
        step &ptr nsize<uint32>
        // drawCountBuffer
        NativePtr.write (NativePtr.ofNativeInt ptr) drawCountBuffer
        step &ptr nsize<nativeint>
        // drawCountBufferOffset
        NativePtr.write (NativePtr.ofNativeInt ptr) drawCountBufferOffset
        step &ptr nsize<uint64>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderExecuteBundles(bundleCount : unativeint, bundles : nativeptr<nativeint>) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderExecuteBundles
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<unativeint> + align8 nsize<nativeint>
        let mutable aux = ptr + argSize
        // bundleCount
        NativePtr.write (NativePtr.ofNativeInt ptr) bundleCount
        step &ptr nsize<unativeint>
        // bundles
        let _bundlesStorage = aux
        let _bundlesSize = nsize<nativeint> * nativeint bundleCount
        step &aux _bundlesSize
        let srcSpan = System.Span<byte>(NativePtr.toVoidPtr bundles, int _bundlesSize)
        let dstSpan = System.Span<byte>(NativePtr.toVoidPtr (NativePtr.ofNativeInt<byte> _bundlesStorage), int _bundlesSize)
        srcSpan.CopyTo(dstSpan)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_bundlesStorage - ptr)
        step &ptr nsize<nativeint>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderInsertDebugMarker(markerLabel : StringView) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderInsertDebugMarker
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<StringView>
        let mutable aux = ptr + argSize
        // markerLabel
        markerLabel.CopyTo(ptr, &aux)
        step &ptr nsize<StringView>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderPopDebugGroup() : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderPopDebugGroup
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = 0n
        let mutable aux = ptr + argSize
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderPushDebugGroup(groupLabel : StringView) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderPushDebugGroup
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<StringView>
        let mutable aux = ptr + argSize
        // groupLabel
        groupLabel.CopyTo(ptr, &aux)
        step &ptr nsize<StringView>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderSetStencilReference(reference : uint32) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderSetStencilReference
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<uint32>
        let mutable aux = ptr + argSize
        // reference
        NativePtr.write (NativePtr.ofNativeInt ptr) reference
        step &ptr nsize<uint32>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderSetBlendConstant(color : nativeptr<Color>) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderSetBlendConstant
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint>
        let mutable aux = ptr + argSize
        // color
        let _color = NativePtr.read color
        let _colorStorage = aux
        step &aux nsize<Color>
        _color.CopyTo(_colorStorage, &aux)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_colorStorage - ptr)
        step &ptr nsize<nativeint>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderSetViewport(x : float32, y : float32, width : float32, height : float32, minDepth : float32, maxDepth : float32) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderSetViewport
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<float32> * 6n
        let mutable aux = ptr + argSize
        // x
        NativePtr.write (NativePtr.ofNativeInt ptr) x
        step &ptr nsize<float32>
        // y
        NativePtr.write (NativePtr.ofNativeInt ptr) y
        step &ptr nsize<float32>
        // width
        NativePtr.write (NativePtr.ofNativeInt ptr) width
        step &ptr nsize<float32>
        // height
        NativePtr.write (NativePtr.ofNativeInt ptr) height
        step &ptr nsize<float32>
        // minDepth
        NativePtr.write (NativePtr.ofNativeInt ptr) minDepth
        step &ptr nsize<float32>
        // maxDepth
        NativePtr.write (NativePtr.ofNativeInt ptr) maxDepth
        step &ptr nsize<float32>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderSetScissorRect(x : uint32, y : uint32, width : uint32, height : uint32) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderSetScissorRect
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<uint32> * 4n
        let mutable aux = ptr + argSize
        // x
        NativePtr.write (NativePtr.ofNativeInt ptr) x
        step &ptr nsize<uint32>
        // y
        NativePtr.write (NativePtr.ofNativeInt ptr) y
        step &ptr nsize<uint32>
        // width
        NativePtr.write (NativePtr.ofNativeInt ptr) width
        step &ptr nsize<uint32>
        // height
        NativePtr.write (NativePtr.ofNativeInt ptr) height
        step &ptr nsize<uint32>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderSetVertexBuffer(slot : uint32, buffer : nativeint, offset : uint64, size : uint64) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderSetVertexBuffer
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<uint32> + align8 nsize<nativeint> + align8 nsize<uint64> * 2n
        let mutable aux = ptr + argSize
        // slot
        NativePtr.write (NativePtr.ofNativeInt ptr) slot
        step &ptr nsize<uint32>
        // buffer
        NativePtr.write (NativePtr.ofNativeInt ptr) buffer
        step &ptr nsize<nativeint>
        // offset
        NativePtr.write (NativePtr.ofNativeInt ptr) offset
        step &ptr nsize<uint64>
        // size
        NativePtr.write (NativePtr.ofNativeInt ptr) size
        step &ptr nsize<uint64>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderSetIndexBuffer(buffer : nativeint, format : IndexFormat, offset : uint64, size : uint64) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderSetIndexBuffer
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint> + align8 nsize<IndexFormat> + align8 nsize<uint64> * 2n
        let mutable aux = ptr + argSize
        // buffer
        NativePtr.write (NativePtr.ofNativeInt ptr) buffer
        step &ptr nsize<nativeint>
        // format
        NativePtr.write (NativePtr.ofNativeInt ptr) format
        step &ptr nsize<IndexFormat>
        // offset
        NativePtr.write (NativePtr.ofNativeInt ptr) offset
        step &ptr nsize<uint64>
        // size
        NativePtr.write (NativePtr.ofNativeInt ptr) size
        step &ptr nsize<uint64>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderBeginOcclusionQuery(queryIndex : uint32) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderBeginOcclusionQuery
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<uint32>
        let mutable aux = ptr + argSize
        // queryIndex
        NativePtr.write (NativePtr.ofNativeInt ptr) queryIndex
        step &ptr nsize<uint32>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderEndOcclusionQuery() : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderEndOcclusionQuery
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = 0n
        let mutable aux = ptr + argSize
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderWriteTimestamp(querySet : nativeint, queryIndex : uint32) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderWriteTimestamp
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<nativeint> + align8 nsize<uint32>
        let mutable aux = ptr + argSize
        // querySet
        NativePtr.write (NativePtr.ofNativeInt ptr) querySet
        step &ptr nsize<nativeint>
        // queryIndex
        NativePtr.write (NativePtr.ofNativeInt ptr) queryIndex
        step &ptr nsize<uint32>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderPixelLocalStorageBarrier() : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderPixelLocalStorageBarrier
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = 0n
        let mutable aux = ptr + argSize
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderEnd() : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderEnd
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = 0n
        let mutable aux = ptr + argSize
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member this.RenderPassEncoderSetImmediateData(offset : uint32, data : nativeint, size : unativeint) : unit =
        let start = ptr
        ptr <- ptr + 4n
        NativePtr.write (NativePtr.ofNativeInt ptr) CommandStreamCommand.RenderPassEncoderSetImmediateData
        ptr <- ptr + nsize<CommandStreamCommand>
        let argSize = align8 nsize<uint32> + align8 nsize<nativeint> + align8 nsize<unativeint>
        let mutable aux = ptr + argSize
        // offset
        NativePtr.write (NativePtr.ofNativeInt ptr) offset
        step &ptr nsize<uint32>
        // data
        let _dataStorage = aux
        let _dataSize = nsize<unit> * nativeint size
        step &aux _dataSize
        let srcSpan = System.Span<byte>(NativePtr.toVoidPtr (NativePtr.ofNativeInt<byte> data), int _dataSize)
        let dstSpan = System.Span<byte>(NativePtr.toVoidPtr (NativePtr.ofNativeInt<byte> _dataStorage), int _dataSize)
        srcSpan.CopyTo(dstSpan)
        NativePtr.write (NativePtr.ofNativeInt ptr) (_dataStorage - ptr)
        step &ptr nsize<nativeint>
        // size
        NativePtr.write (NativePtr.ofNativeInt ptr) size
        step &ptr nsize<unativeint>
        // end
        ptr <- aux
        let size = aux - start
        NativePtr.write (NativePtr.ofNativeInt start) (int size)
    member x.Run(cmd : nativeint) =
        RawCommandStreamExt.gpuRunInterpreter(cmd, memory, ptr)
    member x.Start = memory
    member x.End = ptr
    member x.Reset() =
        ptr <- memory
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        Marshal.FreeHGlobal(memory)
    member x.Dispose() = x.Dispose true
    override x.Finalize() = x.Dispose false
    interface IDisposable with
        member x.Dispose() = x.Dispose true
open WebGPU
type CommandStreamInternal(device : Device) =
    let raw = new RawCommandStream()
    member x.Device = device
    member _.CommandEncoderFinish(descriptor : CommandBufferDescriptor) : unit =
        descriptor.Pin(device, fun _descriptorPtr ->
            raw.CommandEncoderFinish(_descriptorPtr)
        )
    member _.CommandEncoderBeginComputePass(descriptor : ComputePassDescriptor) : unit =
        descriptor.Pin(device, fun _descriptorPtr ->
            raw.CommandEncoderBeginComputePass(_descriptorPtr)
        )
    member _.CommandEncoderBeginRenderPass(descriptor : RenderPassDescriptor) : unit =
        descriptor.Pin(device, fun _descriptorPtr ->
            raw.CommandEncoderBeginRenderPass(_descriptorPtr)
        )
    member _.CommandEncoderCopyBufferToBuffer(source : Buffer, sourceOffset : int64, destination : Buffer, destinationOffset : int64, size : int64) : unit =
        raw.CommandEncoderCopyBufferToBuffer(source.Handle, uint64(sourceOffset), destination.Handle, uint64(destinationOffset), uint64(size))
    member _.CommandEncoderCopyBufferToTexture(source : TexelCopyBufferInfo, destination : TexelCopyTextureInfo, copySize : Extent3D) : unit =
        source.Pin(device, fun _sourcePtr ->
            destination.Pin(device, fun _destinationPtr ->
                copySize.Pin(device, fun _copySizePtr ->
                    raw.CommandEncoderCopyBufferToTexture(_sourcePtr, _destinationPtr, _copySizePtr)
                )
            )
        )
    member _.CommandEncoderCopyTextureToBuffer(source : TexelCopyTextureInfo, destination : TexelCopyBufferInfo, copySize : Extent3D) : unit =
        source.Pin(device, fun _sourcePtr ->
            destination.Pin(device, fun _destinationPtr ->
                copySize.Pin(device, fun _copySizePtr ->
                    raw.CommandEncoderCopyTextureToBuffer(_sourcePtr, _destinationPtr, _copySizePtr)
                )
            )
        )
    member _.CommandEncoderCopyTextureToTexture(source : TexelCopyTextureInfo, destination : TexelCopyTextureInfo, copySize : Extent3D) : unit =
        source.Pin(device, fun _sourcePtr ->
            destination.Pin(device, fun _destinationPtr ->
                copySize.Pin(device, fun _copySizePtr ->
                    raw.CommandEncoderCopyTextureToTexture(_sourcePtr, _destinationPtr, _copySizePtr)
                )
            )
        )
    member _.CommandEncoderClearBuffer(buffer : Buffer, offset : int64, size : int64) : unit =
        raw.CommandEncoderClearBuffer(buffer.Handle, uint64(offset), uint64(size))
    member _.CommandEncoderInjectValidationError(message : string) : unit =
        let _messageArr = if isNull message then null else Encoding.UTF8.GetBytes(message)
        use _messagePtr = fixed _messageArr
        try
            let _messageLen = WebGPU.Raw.StringView(_messagePtr, if isNull _messageArr then 0un else unativeint _messageArr.Length)
            raw.CommandEncoderInjectValidationError(_messageLen)
        finally
            ()
    member _.CommandEncoderInsertDebugMarker(markerLabel : string) : unit =
        let _markerLabelArr = if isNull markerLabel then null else Encoding.UTF8.GetBytes(markerLabel)
        use _markerLabelPtr = fixed _markerLabelArr
        try
            let _markerLabelLen = WebGPU.Raw.StringView(_markerLabelPtr, if isNull _markerLabelArr then 0un else unativeint _markerLabelArr.Length)
            raw.CommandEncoderInsertDebugMarker(_markerLabelLen)
        finally
            ()
    member _.CommandEncoderPopDebugGroup() : unit =
        raw.CommandEncoderPopDebugGroup()
    member _.CommandEncoderPushDebugGroup(groupLabel : string) : unit =
        let _groupLabelArr = if isNull groupLabel then null else Encoding.UTF8.GetBytes(groupLabel)
        use _groupLabelPtr = fixed _groupLabelArr
        try
            let _groupLabelLen = WebGPU.Raw.StringView(_groupLabelPtr, if isNull _groupLabelArr then 0un else unativeint _groupLabelArr.Length)
            raw.CommandEncoderPushDebugGroup(_groupLabelLen)
        finally
            ()
    member _.CommandEncoderResolveQuerySet(querySet : QuerySet, firstQuery : int, queryCount : int, destination : Buffer, destinationOffset : int64) : unit =
        raw.CommandEncoderResolveQuerySet(querySet.Handle, uint32(firstQuery), uint32(queryCount), destination.Handle, uint64(destinationOffset))
    member _.CommandEncoderWriteBuffer(buffer : Buffer, bufferOffset : int64, data : array<uint8>, size : int64) : unit =
        use dataPtr = fixed (data)
        try
            let dataLen = uint64 data.Length
            raw.CommandEncoderWriteBuffer(buffer.Handle, uint64(bufferOffset), dataPtr, uint64(size))
        finally
            ()
    member _.CommandEncoderWriteTimestamp(querySet : QuerySet, queryIndex : int) : unit =
        raw.CommandEncoderWriteTimestamp(querySet.Handle, uint32(queryIndex))
    member _.ComputePassEncoderInsertDebugMarker(markerLabel : string) : unit =
        let _markerLabelArr = if isNull markerLabel then null else Encoding.UTF8.GetBytes(markerLabel)
        use _markerLabelPtr = fixed _markerLabelArr
        try
            let _markerLabelLen = WebGPU.Raw.StringView(_markerLabelPtr, if isNull _markerLabelArr then 0un else unativeint _markerLabelArr.Length)
            raw.ComputePassEncoderInsertDebugMarker(_markerLabelLen)
        finally
            ()
    member _.ComputePassEncoderPopDebugGroup() : unit =
        raw.ComputePassEncoderPopDebugGroup()
    member _.ComputePassEncoderPushDebugGroup(groupLabel : string) : unit =
        let _groupLabelArr = if isNull groupLabel then null else Encoding.UTF8.GetBytes(groupLabel)
        use _groupLabelPtr = fixed _groupLabelArr
        try
            let _groupLabelLen = WebGPU.Raw.StringView(_groupLabelPtr, if isNull _groupLabelArr then 0un else unativeint _groupLabelArr.Length)
            raw.ComputePassEncoderPushDebugGroup(_groupLabelLen)
        finally
            ()
    member _.ComputePassEncoderSetPipeline(pipeline : ComputePipeline) : unit =
        raw.ComputePassEncoderSetPipeline(pipeline.Handle)
    member _.ComputePassEncoderSetBindGroup(groupIndex : int, group : BindGroup, dynamicOffsets : array<uint32>) : unit =
        use dynamicOffsetsPtr = fixed (dynamicOffsets)
        try
            let dynamicOffsetsLen = unativeint dynamicOffsets.Length
            raw.ComputePassEncoderSetBindGroup(uint32(groupIndex), group.Handle, dynamicOffsetsLen, dynamicOffsetsPtr)
        finally
            ()
    member _.ComputePassEncoderWriteTimestamp(querySet : QuerySet, queryIndex : int) : unit =
        raw.ComputePassEncoderWriteTimestamp(querySet.Handle, uint32(queryIndex))
    member _.ComputePassEncoderDispatchWorkgroups(workgroupCountX : int, workgroupCountY : int, workgroupCountZ : int) : unit =
        raw.ComputePassEncoderDispatchWorkgroups(uint32(workgroupCountX), uint32(workgroupCountY), uint32(workgroupCountZ))
    member _.ComputePassEncoderDispatchWorkgroupsIndirect(indirectBuffer : Buffer, indirectOffset : int64) : unit =
        raw.ComputePassEncoderDispatchWorkgroupsIndirect(indirectBuffer.Handle, uint64(indirectOffset))
    member _.ComputePassEncoderEnd() : unit =
        raw.ComputePassEncoderEnd()
    member _.ComputePassEncoderSetImmediateData(offset : int, data : nativeint, size : int64) : unit =
        raw.ComputePassEncoderSetImmediateData(uint32(offset), data, unativeint(size))
    member _.RenderPassEncoderSetPipeline(pipeline : RenderPipeline) : unit =
        raw.RenderPassEncoderSetPipeline(pipeline.Handle)
    member _.RenderPassEncoderSetBindGroup(groupIndex : int, group : BindGroup, dynamicOffsets : array<uint32>) : unit =
        use dynamicOffsetsPtr = fixed (dynamicOffsets)
        try
            let dynamicOffsetsLen = unativeint dynamicOffsets.Length
            raw.RenderPassEncoderSetBindGroup(uint32(groupIndex), group.Handle, dynamicOffsetsLen, dynamicOffsetsPtr)
        finally
            ()
    member _.RenderPassEncoderDraw(vertexCount : int, instanceCount : int, firstVertex : int, firstInstance : int) : unit =
        raw.RenderPassEncoderDraw(uint32(vertexCount), uint32(instanceCount), uint32(firstVertex), uint32(firstInstance))
    member _.RenderPassEncoderDrawIndexed(indexCount : int, instanceCount : int, firstIndex : int, baseVertex : int, firstInstance : int) : unit =
        raw.RenderPassEncoderDrawIndexed(uint32(indexCount), uint32(instanceCount), uint32(firstIndex), baseVertex, uint32(firstInstance))
    member _.RenderPassEncoderDrawIndirect(indirectBuffer : Buffer, indirectOffset : int64) : unit =
        raw.RenderPassEncoderDrawIndirect(indirectBuffer.Handle, uint64(indirectOffset))
    member _.RenderPassEncoderDrawIndexedIndirect(indirectBuffer : Buffer, indirectOffset : int64) : unit =
        raw.RenderPassEncoderDrawIndexedIndirect(indirectBuffer.Handle, uint64(indirectOffset))
    member _.RenderPassEncoderMultiDrawIndirect(indirectBuffer : Buffer, indirectOffset : int64, maxDrawCount : int, drawCountBuffer : Buffer, drawCountBufferOffset : int64) : unit =
        raw.RenderPassEncoderMultiDrawIndirect(indirectBuffer.Handle, uint64(indirectOffset), uint32(maxDrawCount), drawCountBuffer.Handle, uint64(drawCountBufferOffset))
    member _.RenderPassEncoderMultiDrawIndexedIndirect(indirectBuffer : Buffer, indirectOffset : int64, maxDrawCount : int, drawCountBuffer : Buffer, drawCountBufferOffset : int64) : unit =
        raw.RenderPassEncoderMultiDrawIndexedIndirect(indirectBuffer.Handle, uint64(indirectOffset), uint32(maxDrawCount), drawCountBuffer.Handle, uint64(drawCountBufferOffset))
    member _.RenderPassEncoderExecuteBundles(bundles : array<RenderBundle>) : unit =
        let bundlesHandles = bundles |> Array.map (fun a -> a.Handle)
        use bundlesPtr = fixed (bundlesHandles)
        try
            let bundlesLen = unativeint bundles.Length
            raw.RenderPassEncoderExecuteBundles(bundlesLen, bundlesPtr)
        finally
            ()
    member _.RenderPassEncoderInsertDebugMarker(markerLabel : string) : unit =
        let _markerLabelArr = if isNull markerLabel then null else Encoding.UTF8.GetBytes(markerLabel)
        use _markerLabelPtr = fixed _markerLabelArr
        try
            let _markerLabelLen = WebGPU.Raw.StringView(_markerLabelPtr, if isNull _markerLabelArr then 0un else unativeint _markerLabelArr.Length)
            raw.RenderPassEncoderInsertDebugMarker(_markerLabelLen)
        finally
            ()
    member _.RenderPassEncoderPopDebugGroup() : unit =
        raw.RenderPassEncoderPopDebugGroup()
    member _.RenderPassEncoderPushDebugGroup(groupLabel : string) : unit =
        let _groupLabelArr = if isNull groupLabel then null else Encoding.UTF8.GetBytes(groupLabel)
        use _groupLabelPtr = fixed _groupLabelArr
        try
            let _groupLabelLen = WebGPU.Raw.StringView(_groupLabelPtr, if isNull _groupLabelArr then 0un else unativeint _groupLabelArr.Length)
            raw.RenderPassEncoderPushDebugGroup(_groupLabelLen)
        finally
            ()
    member _.RenderPassEncoderSetStencilReference(reference : int) : unit =
        raw.RenderPassEncoderSetStencilReference(uint32(reference))
    member _.RenderPassEncoderSetBlendConstant(color : Color) : unit =
        color.Pin(device, fun _colorPtr ->
            raw.RenderPassEncoderSetBlendConstant(_colorPtr)
        )
    member _.RenderPassEncoderSetViewport(x : float32, y : float32, width : float32, height : float32, minDepth : float32, maxDepth : float32) : unit =
        raw.RenderPassEncoderSetViewport(x, y, width, height, minDepth, maxDepth)
    member _.RenderPassEncoderSetScissorRect(x : int, y : int, width : int, height : int) : unit =
        raw.RenderPassEncoderSetScissorRect(uint32(x), uint32(y), uint32(width), uint32(height))
    member _.RenderPassEncoderSetVertexBuffer(slot : int, buffer : Buffer, offset : int64, size : int64) : unit =
        raw.RenderPassEncoderSetVertexBuffer(uint32(slot), buffer.Handle, uint64(offset), uint64(size))
    member _.RenderPassEncoderSetIndexBuffer(buffer : Buffer, format : IndexFormat, offset : int64, size : int64) : unit =
        raw.RenderPassEncoderSetIndexBuffer(buffer.Handle, format, uint64(offset), uint64(size))
    member _.RenderPassEncoderBeginOcclusionQuery(queryIndex : int) : unit =
        raw.RenderPassEncoderBeginOcclusionQuery(uint32(queryIndex))
    member _.RenderPassEncoderEndOcclusionQuery() : unit =
        raw.RenderPassEncoderEndOcclusionQuery()
    member _.RenderPassEncoderWriteTimestamp(querySet : QuerySet, queryIndex : int) : unit =
        raw.RenderPassEncoderWriteTimestamp(querySet.Handle, uint32(queryIndex))
    member _.RenderPassEncoderPixelLocalStorageBarrier() : unit =
        raw.RenderPassEncoderPixelLocalStorageBarrier()
    member _.RenderPassEncoderEnd() : unit =
        raw.RenderPassEncoderEnd()
    member _.RenderPassEncoderSetImmediateData(offset : int, data : nativeint, size : int64) : unit =
        raw.RenderPassEncoderSetImmediateData(uint32(offset), data, unativeint(size))
    member x.Commands =
        let relativePointers = true
        let res = ResizeArray()
        let mutable ptr = raw.Start
        let mutable start = 0n
        while ptr <> raw.End do
            start <- ptr
            let size = NativePtr.read (NativePtr.ofNativeInt<int> ptr)
            ptr <- ptr + nsize<int>
            let cmd = NativePtr.read (NativePtr.ofNativeInt<CommandStreamCommand> ptr)
            ptr <- ptr + nsize<CommandStreamCommand>
            match cmd with
            | CommandStreamCommand.CommandEncoderFinish ->
                let _descriptor = NativePtr.ofNativeInt<WebGPU.Raw.CommandBufferDescriptor> (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let descriptor = CommandBufferDescriptor.Read(device, _descriptor, relativePointers)
                res.Add (cmd, [| descriptor :> obj |])
                ()
            | CommandStreamCommand.CommandEncoderBeginComputePass ->
                let _descriptor = NativePtr.ofNativeInt<WebGPU.Raw.ComputePassDescriptor> (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let descriptor = ComputePassDescriptor.Read(device, _descriptor, relativePointers)
                res.Add (cmd, [| descriptor :> obj |])
                ()
            | CommandStreamCommand.CommandEncoderBeginRenderPass ->
                let _descriptor = NativePtr.ofNativeInt<WebGPU.Raw.RenderPassDescriptor> (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let descriptor = RenderPassDescriptor.Read(device, _descriptor, relativePointers)
                res.Add (cmd, [| descriptor :> obj |])
                ()
            | CommandStreamCommand.CommandEncoderCopyBufferToBuffer ->
                let _source = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _sourceOffset = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let _destination = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _destinationOffset = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let _size = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let source = new Buffer(device, _source)
                let sourceOffset = int64(_sourceOffset)
                let destination = new Buffer(device, _destination)
                let destinationOffset = int64(_destinationOffset)
                let size = int64(_size)
                res.Add (cmd, [| source :> obj; sourceOffset :> obj; destination :> obj; destinationOffset :> obj; size :> obj |])
                ()
            | CommandStreamCommand.CommandEncoderCopyBufferToTexture ->
                let _source = NativePtr.ofNativeInt<WebGPU.Raw.TexelCopyBufferInfo> (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let _destination = NativePtr.ofNativeInt<WebGPU.Raw.TexelCopyTextureInfo> (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let _copySize = NativePtr.ofNativeInt<WebGPU.Raw.Extent3D> (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let source = TexelCopyBufferInfo.Read(device, _source, relativePointers)
                let destination = TexelCopyTextureInfo.Read(device, _destination, relativePointers)
                let copySize = Extent3D.Read(device, _copySize, relativePointers)
                res.Add (cmd, [| source :> obj; destination :> obj; copySize :> obj |])
                ()
            | CommandStreamCommand.CommandEncoderCopyTextureToBuffer ->
                let _source = NativePtr.ofNativeInt<WebGPU.Raw.TexelCopyTextureInfo> (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let _destination = NativePtr.ofNativeInt<WebGPU.Raw.TexelCopyBufferInfo> (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let _copySize = NativePtr.ofNativeInt<WebGPU.Raw.Extent3D> (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let source = TexelCopyTextureInfo.Read(device, _source, relativePointers)
                let destination = TexelCopyBufferInfo.Read(device, _destination, relativePointers)
                let copySize = Extent3D.Read(device, _copySize, relativePointers)
                res.Add (cmd, [| source :> obj; destination :> obj; copySize :> obj |])
                ()
            | CommandStreamCommand.CommandEncoderCopyTextureToTexture ->
                let _source = NativePtr.ofNativeInt<WebGPU.Raw.TexelCopyTextureInfo> (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let _destination = NativePtr.ofNativeInt<WebGPU.Raw.TexelCopyTextureInfo> (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let _copySize = NativePtr.ofNativeInt<WebGPU.Raw.Extent3D> (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let source = TexelCopyTextureInfo.Read(device, _source, relativePointers)
                let destination = TexelCopyTextureInfo.Read(device, _destination, relativePointers)
                let copySize = Extent3D.Read(device, _copySize, relativePointers)
                res.Add (cmd, [| source :> obj; destination :> obj; copySize :> obj |])
                ()
            | CommandStreamCommand.CommandEncoderClearBuffer ->
                let _buffer = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _offset = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let _size = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let buffer = new Buffer(device, _buffer)
                let offset = int64(_offset)
                let size = int64(_size)
                res.Add (cmd, [| buffer :> obj; offset :> obj; size :> obj |])
                ()
            | CommandStreamCommand.CommandEncoderInjectValidationError ->
                let mutable _message = NativePtr.read (NativePtr.ofNativeInt<WebGPU.Raw.StringView> ptr)
                _message.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt _message.Data + ptr)
                step &ptr nsize<WebGPU.Raw.StringView>
                let message = let _messagePtr = NativePtr.toNativeInt(_message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(_message.Length))
                res.Add (cmd, [| message :> obj |])
                ()
            | CommandStreamCommand.CommandEncoderInsertDebugMarker ->
                let mutable _markerLabel = NativePtr.read (NativePtr.ofNativeInt<WebGPU.Raw.StringView> ptr)
                _markerLabel.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt _markerLabel.Data + ptr)
                step &ptr nsize<WebGPU.Raw.StringView>
                let markerLabel = let _markerLabelPtr = NativePtr.toNativeInt(_markerLabel.Data) in if _markerLabelPtr = 0n then null else Marshal.PtrToStringUTF8(_markerLabelPtr, int(_markerLabel.Length))
                res.Add (cmd, [| markerLabel :> obj |])
                ()
            | CommandStreamCommand.CommandEncoderPopDebugGroup ->
                res.Add (cmd, [|  |])
                ()
            | CommandStreamCommand.CommandEncoderPushDebugGroup ->
                let mutable _groupLabel = NativePtr.read (NativePtr.ofNativeInt<WebGPU.Raw.StringView> ptr)
                _groupLabel.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt _groupLabel.Data + ptr)
                step &ptr nsize<WebGPU.Raw.StringView>
                let groupLabel = let _groupLabelPtr = NativePtr.toNativeInt(_groupLabel.Data) in if _groupLabelPtr = 0n then null else Marshal.PtrToStringUTF8(_groupLabelPtr, int(_groupLabel.Length))
                res.Add (cmd, [| groupLabel :> obj |])
                ()
            | CommandStreamCommand.CommandEncoderResolveQuerySet ->
                let _querySet = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _firstQuery = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _queryCount = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _destination = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _destinationOffset = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let querySet = new QuerySet(device, _querySet)
                let firstQuery = int(_firstQuery)
                let queryCount = int(_queryCount)
                let destination = new Buffer(device, _destination)
                let destinationOffset = int64(_destinationOffset)
                res.Add (cmd, [| querySet :> obj; firstQuery :> obj; queryCount :> obj; destination :> obj; destinationOffset :> obj |])
                ()
            | CommandStreamCommand.CommandEncoderWriteBuffer ->
                let _buffer = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _bufferOffset = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let _data = NativePtr.ofNativeInt<uint8> (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let _size = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let buffer = new Buffer(device, _buffer)
                let bufferOffset = int64(_bufferOffset)
                let data = let ptr = _data in Array.init (int _size) (fun i -> NativePtr.get ptr i)
                let size = int64(_size)
                res.Add (cmd, [| buffer :> obj; bufferOffset :> obj; data :> obj; size :> obj |])
                ()
            | CommandStreamCommand.CommandEncoderWriteTimestamp ->
                let _querySet = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _queryIndex = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let querySet = new QuerySet(device, _querySet)
                let queryIndex = int(_queryIndex)
                res.Add (cmd, [| querySet :> obj; queryIndex :> obj |])
                ()
            | CommandStreamCommand.ComputePassEncoderInsertDebugMarker ->
                let mutable _markerLabel = NativePtr.read (NativePtr.ofNativeInt<WebGPU.Raw.StringView> ptr)
                _markerLabel.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt _markerLabel.Data + ptr)
                step &ptr nsize<WebGPU.Raw.StringView>
                let markerLabel = let _markerLabelPtr = NativePtr.toNativeInt(_markerLabel.Data) in if _markerLabelPtr = 0n then null else Marshal.PtrToStringUTF8(_markerLabelPtr, int(_markerLabel.Length))
                res.Add (cmd, [| markerLabel :> obj |])
                ()
            | CommandStreamCommand.ComputePassEncoderPopDebugGroup ->
                res.Add (cmd, [|  |])
                ()
            | CommandStreamCommand.ComputePassEncoderPushDebugGroup ->
                let mutable _groupLabel = NativePtr.read (NativePtr.ofNativeInt<WebGPU.Raw.StringView> ptr)
                _groupLabel.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt _groupLabel.Data + ptr)
                step &ptr nsize<WebGPU.Raw.StringView>
                let groupLabel = let _groupLabelPtr = NativePtr.toNativeInt(_groupLabel.Data) in if _groupLabelPtr = 0n then null else Marshal.PtrToStringUTF8(_groupLabelPtr, int(_groupLabel.Length))
                res.Add (cmd, [| groupLabel :> obj |])
                ()
            | CommandStreamCommand.ComputePassEncoderSetPipeline ->
                let _pipeline = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let pipeline = new ComputePipeline(_pipeline)
                res.Add (cmd, [| pipeline :> obj |])
                ()
            | CommandStreamCommand.ComputePassEncoderSetBindGroup ->
                let _groupIndex = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _group = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _dynamicOffsetCount = NativePtr.read (NativePtr.ofNativeInt<unativeint> ptr)
                step &ptr nsize<unativeint>
                let _dynamicOffsets = NativePtr.ofNativeInt<uint32> (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let groupIndex = int(_groupIndex)
                let group = new BindGroup(device, _group)
                let dynamicOffsets = let ptr = _dynamicOffsets in Array.init (int _dynamicOffsetCount) (fun i -> NativePtr.get ptr i)
                res.Add (cmd, [| groupIndex :> obj; group :> obj; dynamicOffsets :> obj |])
                ()
            | CommandStreamCommand.ComputePassEncoderWriteTimestamp ->
                let _querySet = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _queryIndex = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let querySet = new QuerySet(device, _querySet)
                let queryIndex = int(_queryIndex)
                res.Add (cmd, [| querySet :> obj; queryIndex :> obj |])
                ()
            | CommandStreamCommand.ComputePassEncoderDispatchWorkgroups ->
                let _workgroupCountX = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _workgroupCountY = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _workgroupCountZ = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let workgroupCountX = int(_workgroupCountX)
                let workgroupCountY = int(_workgroupCountY)
                let workgroupCountZ = int(_workgroupCountZ)
                res.Add (cmd, [| workgroupCountX :> obj; workgroupCountY :> obj; workgroupCountZ :> obj |])
                ()
            | CommandStreamCommand.ComputePassEncoderDispatchWorkgroupsIndirect ->
                let _indirectBuffer = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _indirectOffset = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let indirectBuffer = new Buffer(device, _indirectBuffer)
                let indirectOffset = int64(_indirectOffset)
                res.Add (cmd, [| indirectBuffer :> obj; indirectOffset :> obj |])
                ()
            | CommandStreamCommand.ComputePassEncoderEnd ->
                res.Add (cmd, [|  |])
                ()
            | CommandStreamCommand.ComputePassEncoderSetImmediateData ->
                let _offset = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _data = (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let _size = NativePtr.read (NativePtr.ofNativeInt<unativeint> ptr)
                step &ptr nsize<unativeint>
                let offset = int(_offset)
                let data = _data
                let size = int64(_size)
                res.Add (cmd, [| offset :> obj; data :> obj; size :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderSetPipeline ->
                let _pipeline = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let pipeline = new RenderPipeline(_pipeline)
                res.Add (cmd, [| pipeline :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderSetBindGroup ->
                let _groupIndex = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _group = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _dynamicOffsetCount = NativePtr.read (NativePtr.ofNativeInt<unativeint> ptr)
                step &ptr nsize<unativeint>
                let _dynamicOffsets = NativePtr.ofNativeInt<uint32> (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let groupIndex = int(_groupIndex)
                let group = new BindGroup(device, _group)
                let dynamicOffsets = let ptr = _dynamicOffsets in Array.init (int _dynamicOffsetCount) (fun i -> NativePtr.get ptr i)
                res.Add (cmd, [| groupIndex :> obj; group :> obj; dynamicOffsets :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderDraw ->
                let _vertexCount = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _instanceCount = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _firstVertex = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _firstInstance = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let vertexCount = int(_vertexCount)
                let instanceCount = int(_instanceCount)
                let firstVertex = int(_firstVertex)
                let firstInstance = int(_firstInstance)
                res.Add (cmd, [| vertexCount :> obj; instanceCount :> obj; firstVertex :> obj; firstInstance :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderDrawIndexed ->
                let _indexCount = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _instanceCount = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _firstIndex = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _baseVertex = NativePtr.read (NativePtr.ofNativeInt<int> ptr)
                step &ptr nsize<int>
                let _firstInstance = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let indexCount = int(_indexCount)
                let instanceCount = int(_instanceCount)
                let firstIndex = int(_firstIndex)
                let baseVertex = _baseVertex
                let firstInstance = int(_firstInstance)
                res.Add (cmd, [| indexCount :> obj; instanceCount :> obj; firstIndex :> obj; baseVertex :> obj; firstInstance :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderDrawIndirect ->
                let _indirectBuffer = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _indirectOffset = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let indirectBuffer = new Buffer(device, _indirectBuffer)
                let indirectOffset = int64(_indirectOffset)
                res.Add (cmd, [| indirectBuffer :> obj; indirectOffset :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderDrawIndexedIndirect ->
                let _indirectBuffer = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _indirectOffset = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let indirectBuffer = new Buffer(device, _indirectBuffer)
                let indirectOffset = int64(_indirectOffset)
                res.Add (cmd, [| indirectBuffer :> obj; indirectOffset :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderMultiDrawIndirect ->
                let _indirectBuffer = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _indirectOffset = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let _maxDrawCount = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _drawCountBuffer = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _drawCountBufferOffset = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let indirectBuffer = new Buffer(device, _indirectBuffer)
                let indirectOffset = int64(_indirectOffset)
                let maxDrawCount = int(_maxDrawCount)
                let drawCountBuffer = new Buffer(device, _drawCountBuffer)
                let drawCountBufferOffset = int64(_drawCountBufferOffset)
                res.Add (cmd, [| indirectBuffer :> obj; indirectOffset :> obj; maxDrawCount :> obj; drawCountBuffer :> obj; drawCountBufferOffset :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderMultiDrawIndexedIndirect ->
                let _indirectBuffer = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _indirectOffset = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let _maxDrawCount = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _drawCountBuffer = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _drawCountBufferOffset = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let indirectBuffer = new Buffer(device, _indirectBuffer)
                let indirectOffset = int64(_indirectOffset)
                let maxDrawCount = int(_maxDrawCount)
                let drawCountBuffer = new Buffer(device, _drawCountBuffer)
                let drawCountBufferOffset = int64(_drawCountBufferOffset)
                res.Add (cmd, [| indirectBuffer :> obj; indirectOffset :> obj; maxDrawCount :> obj; drawCountBuffer :> obj; drawCountBufferOffset :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderExecuteBundles ->
                let _bundleCount = NativePtr.read (NativePtr.ofNativeInt<unativeint> ptr)
                step &ptr nsize<unativeint>
                let _bundles = NativePtr.ofNativeInt<nativeint> (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let bundles = let ptr = _bundles in Array.init (int _bundleCount) (fun i -> new RenderBundle(NativePtr.get ptr i))
                res.Add (cmd, [| bundles :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderInsertDebugMarker ->
                let mutable _markerLabel = NativePtr.read (NativePtr.ofNativeInt<WebGPU.Raw.StringView> ptr)
                _markerLabel.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt _markerLabel.Data + ptr)
                step &ptr nsize<WebGPU.Raw.StringView>
                let markerLabel = let _markerLabelPtr = NativePtr.toNativeInt(_markerLabel.Data) in if _markerLabelPtr = 0n then null else Marshal.PtrToStringUTF8(_markerLabelPtr, int(_markerLabel.Length))
                res.Add (cmd, [| markerLabel :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderPopDebugGroup ->
                res.Add (cmd, [|  |])
                ()
            | CommandStreamCommand.RenderPassEncoderPushDebugGroup ->
                let mutable _groupLabel = NativePtr.read (NativePtr.ofNativeInt<WebGPU.Raw.StringView> ptr)
                _groupLabel.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt _groupLabel.Data + ptr)
                step &ptr nsize<WebGPU.Raw.StringView>
                let groupLabel = let _groupLabelPtr = NativePtr.toNativeInt(_groupLabel.Data) in if _groupLabelPtr = 0n then null else Marshal.PtrToStringUTF8(_groupLabelPtr, int(_groupLabel.Length))
                res.Add (cmd, [| groupLabel :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderSetStencilReference ->
                let _reference = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let reference = int(_reference)
                res.Add (cmd, [| reference :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderSetBlendConstant ->
                let _color = NativePtr.ofNativeInt<WebGPU.Raw.Color> (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let color = Color.Read(device, _color, relativePointers)
                res.Add (cmd, [| color :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderSetViewport ->
                let _x = NativePtr.read (NativePtr.ofNativeInt<float32> ptr)
                step &ptr nsize<float32>
                let _y = NativePtr.read (NativePtr.ofNativeInt<float32> ptr)
                step &ptr nsize<float32>
                let _width = NativePtr.read (NativePtr.ofNativeInt<float32> ptr)
                step &ptr nsize<float32>
                let _height = NativePtr.read (NativePtr.ofNativeInt<float32> ptr)
                step &ptr nsize<float32>
                let _minDepth = NativePtr.read (NativePtr.ofNativeInt<float32> ptr)
                step &ptr nsize<float32>
                let _maxDepth = NativePtr.read (NativePtr.ofNativeInt<float32> ptr)
                step &ptr nsize<float32>
                let x = _x
                let y = _y
                let width = _width
                let height = _height
                let minDepth = _minDepth
                let maxDepth = _maxDepth
                res.Add (cmd, [| x :> obj; y :> obj; width :> obj; height :> obj; minDepth :> obj; maxDepth :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderSetScissorRect ->
                let _x = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _y = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _width = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _height = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let x = int(_x)
                let y = int(_y)
                let width = int(_width)
                let height = int(_height)
                res.Add (cmd, [| x :> obj; y :> obj; width :> obj; height :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderSetVertexBuffer ->
                let _slot = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _buffer = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _offset = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let _size = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let slot = int(_slot)
                let buffer = new Buffer(device, _buffer)
                let offset = int64(_offset)
                let size = int64(_size)
                res.Add (cmd, [| slot :> obj; buffer :> obj; offset :> obj; size :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderSetIndexBuffer ->
                let _buffer = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _format = NativePtr.read (NativePtr.ofNativeInt<IndexFormat> ptr)
                step &ptr nsize<IndexFormat>
                let _offset = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let _size = NativePtr.read (NativePtr.ofNativeInt<uint64> ptr)
                step &ptr nsize<uint64>
                let buffer = new Buffer(device, _buffer)
                let format = _format
                let offset = int64(_offset)
                let size = int64(_size)
                res.Add (cmd, [| buffer :> obj; format :> obj; offset :> obj; size :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderBeginOcclusionQuery ->
                let _queryIndex = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let queryIndex = int(_queryIndex)
                res.Add (cmd, [| queryIndex :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderEndOcclusionQuery ->
                res.Add (cmd, [|  |])
                ()
            | CommandStreamCommand.RenderPassEncoderWriteTimestamp ->
                let _querySet = NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr)
                step &ptr nsize<nativeint>
                let _queryIndex = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let querySet = new QuerySet(device, _querySet)
                let queryIndex = int(_queryIndex)
                res.Add (cmd, [| querySet :> obj; queryIndex :> obj |])
                ()
            | CommandStreamCommand.RenderPassEncoderPixelLocalStorageBarrier ->
                res.Add (cmd, [|  |])
                ()
            | CommandStreamCommand.RenderPassEncoderEnd ->
                res.Add (cmd, [|  |])
                ()
            | CommandStreamCommand.RenderPassEncoderSetImmediateData ->
                let _offset = NativePtr.read (NativePtr.ofNativeInt<uint32> ptr)
                step &ptr nsize<uint32>
                let _data = (NativePtr.read (NativePtr.ofNativeInt<nativeint> ptr) + start)
                step &ptr nsize<nativeint>
                let _size = NativePtr.read (NativePtr.ofNativeInt<unativeint> ptr)
                step &ptr nsize<unativeint>
                let offset = int(_offset)
                let data = _data
                let size = int64(_size)
                res.Add (cmd, [| offset :> obj; data :> obj; size :> obj |])
                ()
            | _ -> ()
            ptr <- start + nativeint size
        res.ToArray()
    member x.Run(cmd : CommandEncoder) =
        raw.Run(cmd.Handle)
    member x.Reset() = raw.Reset()
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        raw.Dispose()
    member x.Dispose() = x.Dispose true
    override x.Finalize() = x.Dispose false
    interface IDisposable with
        member x.Dispose() = x.Dispose true
type RenderPassStream internal(cmd : CommandStreamInternal) =
    member _.SetPipeline(pipeline : RenderPipeline) = cmd.RenderPassEncoderSetPipeline(pipeline)
    member _.SetBindGroup(groupIndex : int, group : BindGroup, dynamicOffsets : array<uint32>) = cmd.RenderPassEncoderSetBindGroup(groupIndex, group, dynamicOffsets)
    member _.Draw(vertexCount : int, instanceCount : int, firstVertex : int, firstInstance : int) = cmd.RenderPassEncoderDraw(vertexCount, instanceCount, firstVertex, firstInstance)
    member _.DrawIndexed(indexCount : int, instanceCount : int, firstIndex : int, baseVertex : int, firstInstance : int) = cmd.RenderPassEncoderDrawIndexed(indexCount, instanceCount, firstIndex, baseVertex, firstInstance)
    member _.DrawIndirect(indirectBuffer : Buffer, indirectOffset : int64) = cmd.RenderPassEncoderDrawIndirect(indirectBuffer, indirectOffset)
    member _.DrawIndexedIndirect(indirectBuffer : Buffer, indirectOffset : int64) = cmd.RenderPassEncoderDrawIndexedIndirect(indirectBuffer, indirectOffset)
    member _.MultiDrawIndirect(indirectBuffer : Buffer, indirectOffset : int64, maxDrawCount : int, drawCountBuffer : Buffer, drawCountBufferOffset : int64) = cmd.RenderPassEncoderMultiDrawIndirect(indirectBuffer, indirectOffset, maxDrawCount, drawCountBuffer, drawCountBufferOffset)
    member _.MultiDrawIndexedIndirect(indirectBuffer : Buffer, indirectOffset : int64, maxDrawCount : int, drawCountBuffer : Buffer, drawCountBufferOffset : int64) = cmd.RenderPassEncoderMultiDrawIndexedIndirect(indirectBuffer, indirectOffset, maxDrawCount, drawCountBuffer, drawCountBufferOffset)
    member _.ExecuteBundles(bundles : array<RenderBundle>) = cmd.RenderPassEncoderExecuteBundles(bundles)
    member _.InsertDebugMarker(markerLabel : string) = cmd.RenderPassEncoderInsertDebugMarker(markerLabel)
    member _.PopDebugGroup() = cmd.RenderPassEncoderPopDebugGroup()
    member _.PushDebugGroup(groupLabel : string) = cmd.RenderPassEncoderPushDebugGroup(groupLabel)
    member _.SetStencilReference(reference : int) = cmd.RenderPassEncoderSetStencilReference(reference)
    member _.SetBlendConstant(color : Color) = cmd.RenderPassEncoderSetBlendConstant(color)
    member _.SetViewport(x : float32, y : float32, width : float32, height : float32, minDepth : float32, maxDepth : float32) = cmd.RenderPassEncoderSetViewport(x, y, width, height, minDepth, maxDepth)
    member _.SetScissorRect(x : int, y : int, width : int, height : int) = cmd.RenderPassEncoderSetScissorRect(x, y, width, height)
    member _.SetVertexBuffer(slot : int, buffer : Buffer, offset : int64, size : int64) = cmd.RenderPassEncoderSetVertexBuffer(slot, buffer, offset, size)
    member _.SetIndexBuffer(buffer : Buffer, format : IndexFormat, offset : int64, size : int64) = cmd.RenderPassEncoderSetIndexBuffer(buffer, format, offset, size)
    member _.BeginOcclusionQuery(queryIndex : int) = cmd.RenderPassEncoderBeginOcclusionQuery(queryIndex)
    member _.EndOcclusionQuery() = cmd.RenderPassEncoderEndOcclusionQuery()
    member _.WriteTimestamp(querySet : QuerySet, queryIndex : int) = cmd.RenderPassEncoderWriteTimestamp(querySet, queryIndex)
    member _.PixelLocalStorageBarrier() = cmd.RenderPassEncoderPixelLocalStorageBarrier()
    member _.End() = cmd.RenderPassEncoderEnd()
    member _.SetImmediateData(offset : int, data : nativeint, size : int64) = cmd.RenderPassEncoderSetImmediateData(offset, data, size)
type ComputePassStream internal(cmd : CommandStreamInternal) =
    member _.InsertDebugMarker(markerLabel : string) = cmd.ComputePassEncoderInsertDebugMarker(markerLabel)
    member _.PopDebugGroup() = cmd.ComputePassEncoderPopDebugGroup()
    member _.PushDebugGroup(groupLabel : string) = cmd.ComputePassEncoderPushDebugGroup(groupLabel)
    member _.SetPipeline(pipeline : ComputePipeline) = cmd.ComputePassEncoderSetPipeline(pipeline)
    member _.SetBindGroup(groupIndex : int, group : BindGroup, dynamicOffsets : array<uint32>) = cmd.ComputePassEncoderSetBindGroup(groupIndex, group, dynamicOffsets)
    member _.WriteTimestamp(querySet : QuerySet, queryIndex : int) = cmd.ComputePassEncoderWriteTimestamp(querySet, queryIndex)
    member _.DispatchWorkgroups(workgroupCountX : int, workgroupCountY : int, workgroupCountZ : int) = cmd.ComputePassEncoderDispatchWorkgroups(workgroupCountX, workgroupCountY, workgroupCountZ)
    member _.DispatchWorkgroupsIndirect(indirectBuffer : Buffer, indirectOffset : int64) = cmd.ComputePassEncoderDispatchWorkgroupsIndirect(indirectBuffer, indirectOffset)
    member _.End() = cmd.ComputePassEncoderEnd()
    member _.SetImmediateData(offset : int, data : nativeint, size : int64) = cmd.ComputePassEncoderSetImmediateData(offset, data, size)
type CommandStream internal(cmd : CommandStreamInternal, ownsHandle : bool) =
    member _.BeginComputePass(descriptor : ComputePassDescriptor) =
        cmd.CommandEncoderBeginComputePass(descriptor)
        ComputePassStream(cmd)
    member _.BeginRenderPass(descriptor : RenderPassDescriptor) =
        cmd.CommandEncoderBeginRenderPass(descriptor)
        RenderPassStream(cmd)
    member _.CopyBufferToBuffer(source : Buffer, sourceOffset : int64, destination : Buffer, destinationOffset : int64, size : int64) =
        cmd.CommandEncoderCopyBufferToBuffer(source, sourceOffset, destination, destinationOffset, size)
    member _.CopyBufferToTexture(source : TexelCopyBufferInfo, destination : TexelCopyTextureInfo, copySize : Extent3D) =
        cmd.CommandEncoderCopyBufferToTexture(source, destination, copySize)
    member _.CopyTextureToBuffer(source : TexelCopyTextureInfo, destination : TexelCopyBufferInfo, copySize : Extent3D) =
        cmd.CommandEncoderCopyTextureToBuffer(source, destination, copySize)
    member _.CopyTextureToTexture(source : TexelCopyTextureInfo, destination : TexelCopyTextureInfo, copySize : Extent3D) =
        cmd.CommandEncoderCopyTextureToTexture(source, destination, copySize)
    member _.ClearBuffer(buffer : Buffer, offset : int64, size : int64) =
        cmd.CommandEncoderClearBuffer(buffer, offset, size)
    member _.InjectValidationError(message : string) =
        cmd.CommandEncoderInjectValidationError(message)
    member _.InsertDebugMarker(markerLabel : string) =
        cmd.CommandEncoderInsertDebugMarker(markerLabel)
    member _.PopDebugGroup() =
        cmd.CommandEncoderPopDebugGroup()
    member _.PushDebugGroup(groupLabel : string) =
        cmd.CommandEncoderPushDebugGroup(groupLabel)
    member _.ResolveQuerySet(querySet : QuerySet, firstQuery : int, queryCount : int, destination : Buffer, destinationOffset : int64) =
        cmd.CommandEncoderResolveQuerySet(querySet, firstQuery, queryCount, destination, destinationOffset)
    member _.WriteBuffer(buffer : Buffer, bufferOffset : int64, data : array<uint8>, size : int64) =
        cmd.CommandEncoderWriteBuffer(buffer, bufferOffset, data, size)
    member _.WriteTimestamp(querySet : QuerySet, queryIndex : int) =
        cmd.CommandEncoderWriteTimestamp(querySet, queryIndex)
    member private x.Dispose(disposing : bool) =
        if ownsHandle then cmd.Dispose()
        if disposing then System.GC.SuppressFinalize(x)
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface IDisposable with
        member x.Dispose() = x.Dispose(true)
    member x.Commands = cmd.Commands
    member x.Finish(desc : CommandBufferDescriptor) : CommandBuffer =
        use enc = cmd.Device.CreateCommandEncoder { Label = null; Next = null }
        cmd.Run(enc)
        enc.Finish desc
    new(device : Device) = new CommandStream(new CommandStreamInternal(device), true)
