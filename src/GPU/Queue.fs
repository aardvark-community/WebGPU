namespace WebGPU

open System
open System.Runtime.InteropServices
open System.Threading.Tasks
open WebGPU
open Microsoft.FSharp.NativeInterop

#nowarn "9"

type ImageCopyExternalImage =
    {
        Source : int
        Origin : Origin2d
        FlipY : bool
    }

type ImageCopyTextureTagged =
    {
        Texture : Texture
        MipLevel : int
        Origin : Origin3d
        Aspect : TextureAspect
        ColorSpace : ColorSpace
        PremultipliedAlpha : bool
    }

type ImageCopyTexture =
    {
        Texture : Texture
        MipLevel : int
        Origin : Origin3d
        Aspect : TextureAspect
    }

type Queue(label : string, handle : int) =
    member x.Label = label
    member x.Handle = handle

    member x.CopyExternalImageToTexture(source : ImageCopyExternalImage, destination : ImageCopyTextureTagged, size : Size3d) =
        let mutable source =
            {
                WebGPURaw.Source = source.Source
                WebGPURaw.Origin = source.Origin
                WebGPURaw.FlipY = if source.FlipY then 1 else 0
            }
        
        let mutable destination =
            {
                WebGPURaw.ImageCopyTextureTagged.Texture = destination.Texture.Handle
                WebGPURaw.ImageCopyTextureTagged.MipLevel = uint32 destination.MipLevel
                WebGPURaw.ImageCopyTextureTagged.Origin = destination.Origin
                WebGPURaw.ImageCopyTextureTagged.Aspect = destination.Aspect
                WebGPURaw.ImageCopyTextureTagged.ColorSpace = destination.ColorSpace
                WebGPURaw.ImageCopyTextureTagged.PremultipliedAlpha = if destination.PremultipliedAlpha then 1 else 0
            }
            
        WebGPURaw.my_wgpu_queue_copy_external_image_to_texture(handle, &source, &destination, uint32 size.X, uint32 size.Y, uint32 size.Z)
        
    member x.Submit([<ParamArray>] buffers : CommandBuffer[]) =
        let handles = buffers |> Array.map _.Handle
        use ptr = fixed handles
        WebGPURaw.my_wgpu_queue_submit_multiple_and_destroy(handle, ptr, handles.Length)
        
    member x.WriteBuffer(src : nativeint, dst : Buffer, dstOffset : int64, size : int64) =
        let mutable dstOffset = dstOffset
        let mutable size = size
        WebGPURaw.my_wgpu_queue_write_buffer(handle, dst.Handle, &dstOffset, src, &size)
        
    member x.WriteBuffer<'a when 'a : unmanaged>(src : System.ReadOnlyMemory<'a>, dst : Buffer, [<Optional; DefaultParameterValue(0L)>] dstOffset : int64) =
        use m = src.Pin()
        let mutable dstOffset = dstOffset
        let mutable size = int64 src.Length * int64 sizeof<'a>
        WebGPURaw.my_wgpu_queue_write_buffer(handle, dst.Handle, &dstOffset, NativeInt.ofVoidPtr m.Pointer, &size)
        
    member x.WriteBuffer<'a when 'a : unmanaged>(src : System.ReadOnlySpan<'a>, dst : Buffer, [<Optional; DefaultParameterValue(0L)>] dstOffset : int64) =
        use ptr = fixed src
        let mutable dstOffset = dstOffset
        let mutable size = int64 src.Length * int64 sizeof<'a>
        WebGPURaw.my_wgpu_queue_write_buffer(handle, dst.Handle, &dstOffset, NativeInt.ofNativePtr ptr, &size)
        
    member x.WriteBuffer<'a when 'a : unmanaged>(src : System.Memory<'a>, dst : Buffer, [<Optional; DefaultParameterValue(0L)>] dstOffset : int64) =
        use m = src.Pin()
        let mutable dstOffset = dstOffset
        let mutable size = int64 src.Length * int64 sizeof<'a>
        WebGPURaw.my_wgpu_queue_write_buffer(handle, dst.Handle, &dstOffset, NativeInt.ofVoidPtr m.Pointer, &size)
        
    member x.WriteBuffer<'a when 'a : unmanaged>(src : System.Span<'a>, dst : Buffer, [<Optional; DefaultParameterValue(0L)>] dstOffset : int64) =
        use ptr = fixed src
        let mutable dstOffset = dstOffset
        let mutable size = int64 src.Length * int64 sizeof<'a>
        WebGPURaw.my_wgpu_queue_write_buffer(handle, dst.Handle, &dstOffset, NativeInt.ofNativePtr ptr, &size)
        
    member x.WriteBuffer<'a when 'a : unmanaged>(src : 'a[], dst : Buffer, [<Optional; DefaultParameterValue(0L)>] dstOffset : int64) =
        use ptr = fixed src
        let mutable dstOffset = dstOffset
        let mutable size = int64 src.Length * int64 sizeof<'a>
        WebGPURaw.my_wgpu_queue_write_buffer(handle, dst.Handle, &dstOffset, NativeInt.ofNativePtr ptr, &size)
        
    member x.WriteBuffer(src : System.Array, dst : Buffer, [<Optional; DefaultParameterValue(0L)>] dstOffset : int64) =
        let gc = GCHandle.Alloc(src, GCHandleType.Pinned)
        let mutable dstOffset = dstOffset
        let mutable size = int64 src.Length * int64 (Marshal.SizeOf (src.GetType().GetElementType()))
        try WebGPURaw.my_wgpu_queue_write_buffer(handle, dst.Handle, &dstOffset, gc.AddrOfPinnedObject(), &size)
        finally gc.Free()
        
    member x.WriteTexture(src : nativeint, dst : ImageCopyTexture, bytesPerBlockRow : uint32, blockRowsPerImage : uint32, writeSize : Size3d) =
        let mutable dst =
            {
                WebGPURaw.ImageCopyTexture.Texture = dst.Texture.Handle
                WebGPURaw.ImageCopyTexture.Aspect = dst.Aspect
                WebGPURaw.ImageCopyTexture.Origin = dst.Origin
                WebGPURaw.ImageCopyTexture.MipLevel = dst.MipLevel 
            }
            
        WebGPURaw.my_wgpu_queue_write_texture(handle, &dst, src, bytesPerBlockRow, blockRowsPerImage, uint32 writeSize.X, uint32 writeSize.Y, uint32 writeSize.Z)
        
    member x.WriteTexture<'a when 'a : unmanaged>(src : System.ReadOnlyMemory<'a>, dst : ImageCopyTexture, bytesPerBlockRow : uint32, blockRowsPerImage : uint32, writeSize : Size3d) =
        use mem = src.Pin()
        x.WriteTexture(NativeInt.ofVoidPtr mem.Pointer, dst, bytesPerBlockRow, blockRowsPerImage, writeSize)
        
    member x.WriteTexture<'a when 'a : unmanaged>(src : System.Memory<'a>, dst : ImageCopyTexture, bytesPerBlockRow : uint32, blockRowsPerImage : uint32, writeSize : Size3d) =
        use mem = src.Pin()
        x.WriteTexture(NativeInt.ofVoidPtr mem.Pointer, dst, bytesPerBlockRow, blockRowsPerImage, writeSize)
        
    member x.WriteTexture<'a when 'a : unmanaged>(src : System.Span<'a>, dst : ImageCopyTexture, bytesPerBlockRow : uint32, blockRowsPerImage : uint32, writeSize : Size3d) =
        use mem = fixed src
        x.WriteTexture(NativeInt.ofNativePtr mem, dst, bytesPerBlockRow, blockRowsPerImage, writeSize)
        
    member x.WriteTexture<'a when 'a : unmanaged>(src : System.ReadOnlySpan<'a>, dst : ImageCopyTexture, bytesPerBlockRow : uint32, blockRowsPerImage : uint32, writeSize : Size3d) =
        use mem = fixed src
        x.WriteTexture(NativeInt.ofNativePtr mem, dst, bytesPerBlockRow, blockRowsPerImage, writeSize)
        
    member x.WriteTexture<'a when 'a : unmanaged>(src : 'a[], dst : ImageCopyTexture, bytesPerBlockRow : uint32, blockRowsPerImage : uint32, writeSize : Size3d) =
        use mem = fixed src
        x.WriteTexture(NativeInt.ofNativePtr mem, dst, bytesPerBlockRow, blockRowsPerImage, writeSize)
            
    member x.WriteTexture(src : System.Array, dst : ImageCopyTexture, bytesPerBlockRow : uint32, blockRowsPerImage : uint32, writeSize : Size3d) =
        let gc = GCHandle.Alloc(src, GCHandleType.Pinned)
        try x.WriteTexture(gc.AddrOfPinnedObject(), dst, bytesPerBlockRow, blockRowsPerImage, writeSize)
        finally gc.Free()
        
    member x.Wait() =
        let tcs = TaskCompletionSource()
        let cbid = WebGPURaw.WGPUCallbacks.RegisterCallback(fun _ -> tcs.SetResult())
        WebGPURaw.my_wgpu_queue_set_on_submitted_work_done_callback(handle, WebGPURaw.delegatePtr, cbid)
        tcs.Task
        
    member x.OnSubmittedWorkDone() =
        x.Wait()