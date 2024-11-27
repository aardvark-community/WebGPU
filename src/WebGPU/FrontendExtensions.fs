namespace WebGPU

open Microsoft.FSharp.NativeInterop
open System
open System.Diagnostics
open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open System.Threading.Tasks
open Aardvark.Base
open System.Text.RegularExpressions
open WebGPU

type ITextureFormatVisitor<'r> =
    abstract member Accept<'t when 't : unmanaged> : Col.Format -> 'r
    
[<AutoOpen>]
module TextureFormatExtensions =
    type TextureFormat with
        member fmt.Visit (v : ITextureFormatVisitor<'r>) =
            match fmt with
            | TextureFormat.R8Sint -> v.Accept<int8>(Col.Format.Gray)
            | TextureFormat.R8Uint -> v.Accept<uint8>(Col.Format.Gray)
            | TextureFormat.R8Snorm -> v.Accept<int8>(Col.Format.Gray)
            | TextureFormat.R8Unorm -> v.Accept<uint8>(Col.Format.Gray)
            | TextureFormat.R16Sint -> v.Accept<int16>(Col.Format.Gray)
            | TextureFormat.R16Uint -> v.Accept<uint16>(Col.Format.Gray)
            | TextureFormat.R16Unorm -> v.Accept<uint16>(Col.Format.Gray)
            | TextureFormat.R16Float -> v.Accept<System.Half>(Col.Format.Gray)
            | TextureFormat.R32Sint -> v.Accept<int32>(Col.Format.Gray)
            | TextureFormat.R32Uint -> v.Accept<uint32>(Col.Format.Gray)
            | TextureFormat.R32Float -> v.Accept<float32>(Col.Format.Gray)
            | TextureFormat.RG8Sint -> v.Accept<int8>(Col.Format.RG)
            | TextureFormat.RG8Uint -> v.Accept<uint8>(Col.Format.RG)
            | TextureFormat.RG8Snorm -> v.Accept<int8>(Col.Format.RG)
            | TextureFormat.RG8Unorm -> v.Accept<uint8>(Col.Format.RG)
            | TextureFormat.RG16Sint -> v.Accept<int16>(Col.Format.RG)
            | TextureFormat.RG16Uint -> v.Accept<uint16>(Col.Format.RG)
            | TextureFormat.RG32Sint -> v.Accept<int32>(Col.Format.RG)
            | TextureFormat.RG32Uint -> v.Accept<uint32>(Col.Format.RG)
            | TextureFormat.RG32Float -> v.Accept<float32>(Col.Format.RG)
            | TextureFormat.RGBA8Sint -> v.Accept<int8>(Col.Format.RGBA)
            | TextureFormat.RGBA8Uint -> v.Accept<uint8>(Col.Format.RGBA)
            | TextureFormat.RGBA8Snorm -> v.Accept<int8>(Col.Format.RGBA)
            | TextureFormat.RGBA8Unorm -> v.Accept<uint8>(Col.Format.RGBA)
            | TextureFormat.RGBA8UnormSrgb -> v.Accept<uint8>(Col.Format.RGBA)
            | TextureFormat.RGBA16Sint -> v.Accept<int16>(Col.Format.RGBA)
            | TextureFormat.RGBA16Uint -> v.Accept<uint16>(Col.Format.RGBA)
            | TextureFormat.BGRA8Unorm -> v.Accept<uint8>(Col.Format.BGRA)
            | TextureFormat.BGRA8UnormSrgb -> v.Accept<uint8>(Col.Format.BGRA)
            | TextureFormat.RGBA16Float -> v.Accept<System.Half>(Col.Format.RGBA)
            | TextureFormat.RGBA32Sint -> v.Accept<int32>(Col.Format.RGBA)
            | TextureFormat.RGBA32Uint -> v.Accept<uint32>(Col.Format.RGBA)
            | TextureFormat.RGBA32Float -> v.Accept<float32>(Col.Format.RGBA)
            | TextureFormat.Depth16Unorm -> v.Accept<uint16>(Col.Format.Gray)
            | TextureFormat.Depth24Plus -> v.Accept<uint32>(Col.Format.Gray)
            | TextureFormat.Depth24PlusStencil8 -> v.Accept<uint32>(Col.Format.Gray)
            | TextureFormat.Depth32Float -> v.Accept<float32>(Col.Format.Gray)
            | TextureFormat.Stencil8 -> v.Accept<uint8>(Col.Format.Gray)
            | fmt -> failwithf "bad visitor format: %A" fmt
            

type FrontendDeviceDescriptor = 
    {
        Next : IDeviceDescriptorExtension
        Label : string
        DebugOutput : bool
        RequiredFeatures : array<FeatureName>
        RequiredLimits : RequiredLimits
        DefaultQueue : QueueDescriptor
    }

type GLFWSurfaceDescriptor = 
    {
        Label : string
        Window : nativeptr<Silk.NET.GLFW.WindowHandle>
    }

#nowarn "9"
[<AbstractClass; Sealed>]
type WebGPU private() =
    static do
        match RuntimeInformation.ProcessArchitecture with
        | Architecture.Wasm -> ()
        | _ ->
            Aardvark.LoadLibrary(typeof<WebGPU>.Assembly, "webgpu_dawn") |> ignore
            let ptr = Aardvark.LoadLibrary(typeof<WebGPU>.Assembly, "libglfw.3.dylib")
            ()
        
    static let enumRx = Regex @"([a-zA-Z_0-9]+)::"
    
    static let instanceFeatures =
        lazy (
            match RuntimeInformation.ProcessArchitecture with
            | Architecture.Wasm ->
                { TimedWaitAnyEnable = false; TimedWaitAnyMaxCount = 0L }
            | _ -> 
                let mutable ftrs = Unchecked.defaultof<WebGPU.Raw.InstanceFeatures>
                use ptr = fixed &ftrs
                let status = WebGPU.Raw.WebGPU.GetInstanceFeatures(ptr)
                if status <> Status.Success then
                    failwith $"could not get instance features: {status}"
                InstanceFeatures.Read(Unchecked.defaultof<_>, &ftrs)
        )
    
    static member InstanceFeatures = instanceFeatures.Value

    static member CreateInstance(descriptor : InstanceDescriptor) =
        match RuntimeInformation.ProcessArchitecture with
        | Architecture.Wasm ->
            new Instance(0n)
        | _ ->
            descriptor.Pin(Unchecked.defaultof<_>, fun ptr ->
                let handle = WebGPU.Raw.WebGPU.CreateInstance(ptr)
                new Instance(handle)
            )
    
    static member CreateInstance() =
        WebGPU.CreateInstance {
            Next = null
            Features = WebGPU.InstanceFeatures
        }
            
    [<Extension>]
    static member CreateGLFWSurface(this : Instance, descriptor : GLFWSurfaceDescriptor) =
        match RuntimeInformation.ProcessArchitecture with
        | Architecture.Wasm ->
            failwith "GLFW is not supported in WASM"
        | _ ->
            let handle = WebGPU.Raw.WebGPU.InstanceCreateGLFWSurface(this.Handle, NativePtr.toNativeInt descriptor.Window)
            let surf = new Surface(handle)
            if not (isNull descriptor.Label) then surf.SetLabel descriptor.Label
            surf
        
    [<Extension>]
    static member RequestDeviceAsync(this : Adapter, options : FrontendDeviceDescriptor) =
        let tcs = TaskCompletionSource<Device>()
        
        let err =
            if options.DebugOutput then
                {
                    UncapturedErrorCallbackInfo.Callback =
                        ErrorCallback(fun _disp typ message ->
                            let t = System.Diagnostics.StackTrace(4) |> string
                            let message = enumRx.Replace(message, "$1.") + "\n" + t
                            let lines = message.Split('\n')
                            Report.ErrorNoPrefix($"{typ} ERROR:")
                            for l in lines do
                                Report.ErrorNoPrefix($"  {l}")
                        )
                }
            else
                UncapturedErrorCallbackInfo.Null
        
        let realOptions =
            {
                Next = options.Next
                Label = options.Label
                RequiredFeatures = options.RequiredFeatures
                RequiredLimits = options.RequiredLimits
                DefaultQueue = options.DefaultQueue
                DeviceLostCallbackInfo = DeviceLostCallbackInfo.Null
                UncapturedErrorCallbackInfo = err
                DeviceLostCallbackInfo2 = DeviceLostCallbackInfo2.Null
                UncapturedErrorCallbackInfo2 = UncapturedErrorCallbackInfo2.Null
            }
        
        this.RequestDevice(realOptions, RequestDeviceCallback(fun disp status device message ->
            disp.Dispose()
            match status with
            | RequestDeviceStatus.Success -> tcs.SetResult device
            | _ -> tcs.SetException(Exception $"could not create device: {message}")
        ))
        task {
            let! dev = tcs.Task
            
            if options.DebugOutput then
                dev.SetLoggingCallback(LoggingCallback(fun _ t str ->
                    let lines = str.Split("\n")
                    for line in lines do 
                        match t with
                        | LoggingType.Error -> Report.ErrorNoPrefix("{0}", line)
                        | LoggingType.Warning -> Report.WarnNoPrefix("{0}", line)
                        | LoggingType.Info -> Report.Line("{0}", line)
                        | _ -> Report.Line(4, "{0}", line)
                ))
            
            return dev   
        }

    [<Extension>]
    static member GetFormatCapabilities(this : Adapter, format : TextureFormat) =
         let mutable res = Unchecked.defaultof<WebGPU.Raw.FormatCapabilities>
         use ptr = fixed &res
         let status = WebGPU.Raw.WebGPU.AdapterGetFormatCapabilities(this.Handle, format, ptr)
         if status <> Status.Success then
             failwith $"could not get format capabilities: {status}"
         FormatCapabilities.Read(Unchecked.defaultof<_>, &res)
         
    [<Extension>]
    static member GetCapabilities(this : Surface, adapter : Adapter) =
        let mutable res = Unchecked.defaultof<WebGPU.Raw.SurfaceCapabilities>
        use ptr = fixed &res
        let status = WebGPU.Raw.WebGPU.SurfaceGetCapabilities(this.Handle, adapter.Handle, ptr)
        if status <> Status.Success then
            failwith $"could not get surface capabilities: {status}"
        SurfaceCapabilities.Read(Unchecked.defaultof<_>, &res)
    
    [<Extension>]
    static member RequestAdapterAsync(this : Instance, options : RequestAdapterOptions) =
        let tcs = TaskCompletionSource<Adapter>()
        this.RequestAdapter(options, RequestAdapterCallback(fun disp status adapter message ->
            disp.Dispose()
            match status with
            | RequestAdapterStatus.Success -> tcs.SetResult adapter
            | _ -> tcs.SetException(Exception $"could not create adapter: {message}")
        ))
        tcs.Task
   
    [<Extension>]
    static member GetWGSLLanguageFeatures(this : Instance) =
        let cnt = WebGPU.Raw.WebGPU.InstanceEnumerateWGSLLanguageFeatures(this.Handle, NativePtr.ofNativeInt 0n)
        let arr = Array.zeroCreate (int cnt)
        use ptr = fixed arr
        WebGPU.Raw.WebGPU.InstanceEnumerateWGSLLanguageFeatures(this.Handle, ptr) |> ignore
        arr
        
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
    static member Wait(this : Queue) =
        let tcs = TaskCompletionSource()
        this.OnSubmittedWorkDone2 {
            QueueWorkDoneCallbackInfo2.Mode = CallbackMode.AllowSpontaneous
            Callback =
                QueueWorkDoneCallback2(fun d s ->
                    match s with
                    | QueueWorkDoneStatus.Success -> tcs.SetResult()
                    | _ -> tcs.SetException(Exception (sprintf "could not wait for queue: %A" s))
                )
        } |> ignore
        tcs.Task
        
    [<Extension>]
    static member WriteBuffer<'a when 'a : unmanaged>(this : Queue, buffer : Buffer, data : System.Span<'a>) =
        use ptr = fixed data
        this.WriteBuffer(buffer, 0L, NativePtr.toNativeInt ptr, int64 data.Length * int64 sizeof<'a>)

    [<Extension>]
    static member PopErrorScope(device : Device) =
        let tcs = TaskCompletionSource<_>()
        device.PopErrorScope2 {
            Mode = CallbackMode.AllowProcessEvents
            Callback = PopErrorScopeCallback2 (fun d status typ message ->
                d.Dispose()
                tcs.SetResult(typ, message)
                ()
            )
        } |> ignore
        tcs.Task

    [<Extension>]
    static member GetCompilationInfo(this : ShaderModule) =
        let tcs = TaskCompletionSource<_>()
        this.GetCompilationInfo2 {
            Mode = CallbackMode.AllowProcessEvents
            Callback = CompilationInfoCallback2(fun d status info ->
                tcs.SetResult(info)
            )
        } |> ignore
        tcs.Task

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
type BufferRangeExtensions private() =
    
    static let lineRx = System.Text.RegularExpressions.Regex @"\r\n|\n|\r"
    
    [<Extension>]
    static member CreateBuffer<'a when 'a : unmanaged>(device : Device, usage : BufferUsage, data : ReadOnlySpan<'a>, ?label : string) =
        let size = int64 data.Length * int64 sizeof<'a>
        let isMappable = usage &&& BufferUsage.MapWrite <> BufferUsage.None
        let result =
            device.CreateBuffer {
                Next = null
                Label = defaultArg label null
                Usage = usage ||| BufferUsage.CopySrc ||| BufferUsage.CopyDst
                Size = size
                MappedAtCreation = isMappable
            }
            
        if isMappable then
            let dst = result.GetMappedRange(0L, size)
            data.CopyTo(System.Span<'a>(NativePtr.toVoidPtr (NativePtr.ofNativeInt<byte> dst), data.Length))
            result.Unmap()
            Task.FromResult result
        else
            use tmp = 
                device.CreateBuffer {
                    Next = null
                    Label = null
                    Usage = BufferUsage.MapWrite ||| BufferUsage.CopySrc
                    Size = size
                    MappedAtCreation = true
                }
                
            let dst = tmp.GetMappedRange(0L, size)
            data.CopyTo(System.Span<'a>(NativePtr.toVoidPtr (NativePtr.ofNativeInt<byte> dst), data.Length))
            tmp.Unmap()
            
            use enc = device.CreateCommandEncoder { Label = null; Next = null }
            enc.CopyBufferToBuffer(tmp, 0L, result, 0L, size)
            use cmd = enc.Finish { Label = null }
            device.Queue.Submit [| cmd |]
            task {
                do! device.Queue.Wait()
                return result
            }
            
    [<Extension>]
    static member CreateBuffer<'a when 'a : unmanaged>(device : Device, usage : BufferUsage, data : Span<'a>, ?label : string) =
        device.CreateBuffer(usage, System.Span.op_Implicit data, ?label = label)
    
    [<Extension>]
    static member CreateBuffer<'a when 'a : unmanaged>(device : Device, usage : BufferUsage, data : ReadOnlyMemory<'a>, ?label : string) =
        device.CreateBuffer(usage, data.Span, ?label = label)
        
    [<Extension>]
    static member CreateBuffer<'a when 'a : unmanaged>(device : Device, usage : BufferUsage, data : Memory<'a>, ?label : string) =
        device.CreateBuffer(usage, data.Span, ?label = label)
    
    [<Extension>]
    static member CreateBuffer<'a when 'a : unmanaged>(device : Device, usage : BufferUsage, data : 'a[], ?label : string) =
        device.CreateBuffer(usage, System.Span.op_Implicit data, ?label = label)
    
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
            
           
    [<Extension>]
    static member CreateView(tex : Texture, usage : TextureUsage, level : int) =
        tex.CreateView {
            Next = null
            Label = null
            Format = tex.Format
            Dimension =
                match tex.Dimension with
                | TextureDimension.D1D -> TextureViewDimension.D1D
                | TextureDimension.D2D -> TextureViewDimension.D2D
                | TextureDimension.D3D -> TextureViewDimension.D3D
                | _ -> failwith "bad dim"
            BaseMipLevel = level
            MipLevelCount = 1
            BaseArrayLayer = 0
            ArrayLayerCount = 1
            Aspect = TextureAspect.Plane0Only
            Usage = usage
        }
        
    [<Extension>]
    static member CreateView(tex : Texture, usage : TextureUsage) =
        tex.CreateView {
            Next = null
            Label = null
            Format = tex.Format
            Dimension =
                match tex.Dimension with
                | TextureDimension.D1D -> TextureViewDimension.D1D
                | TextureDimension.D2D -> TextureViewDimension.D2D
                | TextureDimension.D3D -> TextureViewDimension.D3D
                | _ -> failwith "bad dim"
            BaseMipLevel = 0
            MipLevelCount = tex.MipLevelCount
            BaseArrayLayer = 0
            ArrayLayerCount = 1
            Aspect = TextureAspect.All
            Usage = usage
        }
    
        
    static member private CopyImageToTexture(this : CommandEncoder, data : nativeint, tex : Texture, volumeInfo : Type -> int -> Col.Format -> list<Range1i * Choice<VolumeInfo, obj>>) =
        tex.Format.Visit {
            new ITextureFormatVisitor<_> with
                member x.Accept<'a when 'a : unmanaged>(fmt : Col.Format) =
                    let device = this.Device
                    let size = V2i(tex.Width, tex.Height)
                    let elementSize = sizeof<'a>
                    let channels = fmt.ChannelCount()
                    let infos = volumeInfo typeof<'a> elementSize fmt
                    
                    // https://developer.mozilla.org/en-US/docs/Web/API/GPUCommandEncoder/copyBufferToTexture
                    let pixelSize = channels * elementSize
                    let bpr = size.X * pixelSize
                    let fakebpr =
                        let align = Fun.LeastCommonMultiple(pixelSize, 256)
                        if bpr % align = 0 then bpr
                        else (bpr / align + 1) * align
                    
                    use tmp =
                        device.CreateBuffer {
                            Next = null
                            Label = null
                            Usage = BufferUsage.CopySrc ||| BufferUsage.MapWrite
                            Size = int64 fakebpr * int64 size.Y
                            MappedAtCreation = true
                        }
                        
                    let tmpPtr = tmp.GetMappedRange(0L, tmp.Size)
                    
                    
                    let fakeWidth = fakebpr / pixelSize
                    let dstVolume = NativeVolume<'a>(NativePtr.ofNativeInt tmpPtr, VolumeInfo(0L, V3l(fakeWidth, size.Y, channels), V3l(channels, int64 channels * int64 fakeWidth, 1L)))
                    
                    for channelRange, info in infos do
                        let dstPart = dstVolume.SubVolume(V3l(0, 0, channelRange.Min), V3l(size.X, size.Y, 1 + channelRange.Max - channelRange.Min))
                        match info with
                        | Choice1Of2 info ->
                            let src = NativeVolume<'a>(NativePtr.ofNativeInt data, info)
                            NativeVolume.copy src dstPart
                        | Choice2Of2 value ->
                            NativeVolume.set (value :?> 'a) dstPart
                            
                    tmp.Unmap()
                    
                    //use enc = device.CreateCommandEncoder { Label = null; Next = null }
                    let src : ImageCopyBuffer =
                        {
                            Layout =
                                {
                                    Offset = 0L
                                    BytesPerRow = fakebpr
                                    RowsPerImage = size.Y
                                }
                            Buffer = tmp
                        }
                    let dst : ImageCopyTexture =
                        {
                            Texture = tex
                            Origin = { X = 0; Y = 0; Z = 0 }
                            Aspect = TextureAspect.All
                            MipLevel = 0
                        }
                        
                    this.CopyBufferToTexture(src, dst, { Width = size.X; Height = size.Y; DepthOrArrayLayers = 1 })
                    
                    1
        } |> ignore
    
    [<Extension>]
    static member CopyImageToTexture(this : CommandEncoder, data : PixImage, tex : Texture) =
        data.Visit {
            new IPixImageVisitor<_> with
                member x.Visit (image: PixImage<'i>) =
                    let gc = GCHandle.Alloc(image.Volume.Data, GCHandleType.Pinned)
                    let ptr = gc.AddrOfPinnedObject()
                    BufferRangeExtensions.CopyImageToTexture(this, ptr, tex, fun typ _ c ->
                        if typ <> typeof<'i> then failwithf $"bad channel-type {typeof<'i>} (expected {typ})"
                        
                        if c = image.Format then
                            [Range1i(0, c.ChannelCount() - 1), Choice1Of2 image.Volume.Info]
                        else
                            match c with
                            | Col.Format.RGBA ->
                                match image.Format with
                                | Col.Format.BGRA ->
                                    let info = image.Volume.Info
                                    let getChannel i = info.SubVolume(V3l(0,0,i), V3l(info.SX, info.SY, 1L))
                                    
                                    [
                                        Range1i(0, 0), Choice1Of2 (getChannel 2)
                                        Range1i(1, 1), Choice1Of2 (getChannel 1)
                                        Range1i(2, 2), Choice1Of2 (getChannel 0)
                                        Range1i(3, 3), Choice1Of2 (getChannel 3)
                                    ]
                                | Col.Format.RGB ->
                                    let value =
                                        if typ = typeof<byte> then 255uy :> obj
                                        elif typ = typeof<int8> then 0y :> obj
                                        elif typ = typeof<float32> then 1.0f :> obj
                                        else failwith $"bad channel-type {typ}"
                                        
                                    [Range1i(0, 2), Choice1Of2 image.Volume.Info; Range1i(3,3), Choice2Of2 value]
                                | _ ->
                                    failwith $"bad format {image.Format} (expected {c})"
                            | _ ->
                                failwith $"bad format {image.Format} (expected {c})"
                            
                    )
                    gc.Free()
                    1
        } |> ignore
        
    [<Extension>]
    static member CompileShader(device : Device, shaderCode : string) =
        
        let shader =
            device.CreateShaderModule {
                Label = null
                Next = { ShaderSourceWGSL.Next = null; ShaderSourceWGSL.Code = shaderCode }
            }
            
        let info = shader.GetCompilationInfo().Result
        let hasErrors = info.Messages |> Array.exists (fun m -> m.Type <= CompilationMessageType.Error)
        let hasWarnings = info.Messages |> Array.exists (fun m -> m.Type <= CompilationMessageType.Warning)
        if hasWarnings then
            Report.Begin "shader compile"
            for m in info.Messages do
                let str = sprintf "@%d,%d: %s" m.LineNum m.LinePos m.Message
                for line in lineRx.Split str do
                    match m.Type with
                    | CompilationMessageType.Error -> Report.ErrorNoPrefix("{0}", line)
                    | CompilationMessageType.Warning -> Report.WarnNoPrefix("{0}", line)
                    | _ -> Report.Line("{0}", line)
            Report.End() |> ignore
        
        if hasErrors then failwith $"shader compilation failed: {info.Messages}"
        shader
        
[<AutoOpen>]
module ``F# Extensions`` =
    
    let mipMapLevels (size : V2i) =
        1 + int (log2 (float (max size.X size.Y)) |> floor)
  
    module BindGroupEntry =
        let (|UniformBuffer|Sampler|TextureView|) (g : BindGroupEntry) =
            if g.Buffer.Handle <> 0n then
                UniformBuffer(g.Binding, g.Buffer, g.Offset, g.Size)
            elif g.Sampler.Handle <> 0n then
                Sampler(g.Binding, g.Sampler)
            elif g.TextureView.Handle <> 0n then
                TextureView(g.Binding, g.TextureView)
            else
                failwith "bad bindgroup entry"
                
    type BindGroupEntry =
        
        static member UniformBuffer(binding : int, buffer : Buffer, ?offset : int64, ?size : int64) =
            let offset = defaultArg offset 0
            let size =
                match size with
                | Some size -> size
                | None -> buffer.Size - offset
            {
                Next = null
                Binding = binding
                Offset = offset
                Size = size
                Buffer = buffer
                Sampler = Sampler.Null
                TextureView = TextureView.Null
            }

        static member Sampler(binding : int, sam : Sampler) =
            {
                Next = null
                Binding = binding
                Offset = 0L
                Size = 0L
                Buffer = Buffer.Null
                Sampler = sam
                TextureView = TextureView.Null
            }
            
        static member TextureView(binding : int, tex : TextureView) =
            {
                Next = null
                Binding = binding
                Offset = 0L
                Size = 0L
                Buffer = Buffer.Null
                Sampler = Sampler.Null
                TextureView = tex
            }
       
    module BindGroupLayoutEntry =
        let (|UniformBuffer|Sampler|Texture|StorageTexture|) (g : BindGroupLayoutEntry) =
            if g.Buffer.Type <> BufferBindingType.Undefined then
                UniformBuffer(g.Binding, g.Visibility, g.Buffer)
            elif g.Sampler.Type <> SamplerBindingType.Undefined then
                Sampler(g.Binding, g.Visibility, g.Sampler.Type)
            elif g.Texture.ViewDimension <> TextureViewDimension.Undefined then
                Texture(g.Binding, g.Visibility, g.Texture)
            elif g.StorageTexture.ViewDimension <> TextureViewDimension.Undefined then
                StorageTexture(g.Binding, g.Visibility, g.StorageTexture)
            else
                failwith "bad bindgroup entry"
    
    type BindGroupLayoutEntry =
        static member Buffer(binding : int, visibility : ShaderStage, layout : BufferBindingLayout) =
            {
                Next = null
                Binding = binding
                Visibility = visibility
                Buffer = layout
                Sampler = SamplerBindingLayout.Null
                Texture = TextureBindingLayout.Null
                StorageTexture = StorageTextureBindingLayout.Null
            }
            
        static member Sampler(binding : int, visibility : ShaderStage, bindingType : SamplerBindingType) =
            {
                Next = null
                Binding = binding
                Visibility = visibility
                Buffer = BufferBindingLayout.Null
                Sampler = { Type = bindingType }
                Texture = TextureBindingLayout.Null
                StorageTexture = StorageTextureBindingLayout.Null
            }
    
        static member Texture(binding : int, visibility : ShaderStage, layout : TextureBindingLayout) =
            {
                Next = null
                Binding = binding
                Visibility = visibility
                Buffer = BufferBindingLayout.Null
                Sampler = SamplerBindingLayout.Null  
                Texture = layout
                StorageTexture = StorageTextureBindingLayout.Null
            }
    
    
        static member StorageTexture(binding : int, visibility : ShaderStage, layout : StorageTextureBindingLayout) =
            {
                Next = null
                Binding = binding
                Visibility = visibility
                Buffer = BufferBindingLayout.Null
                Sampler = SamplerBindingLayout.Null  
                Texture = TextureBindingLayout.Null
                StorageTexture = layout
            }
    type CompilationInfo with
        member x.HasErrors = x.Messages |> Array.exists (fun m -> m.Type <= CompilationMessageType.Error)
        member x.HasWarnings = x.Messages |> Array.exists (fun m -> m.Type <= CompilationMessageType.Warning)
    
        
    
    type Instance with
        member this.WGSLLanguageFeatures = WebGPU.GetWGSLLanguageFeatures(this)
        
    let inline RenderBundleEncoderDescriptor (e : RenderBundleEncoderDescriptor) = e
        
    let inline undefined<'a when 'a : (static member Null : 'a)> : 'a =
        'a.Null
        
    // type Adapter with
    //     member this.Info = WebGPU.GetInfo(this)
    //     member this.Limits = WebGPU.GetLimits(this)
    //     
    // type Device with
    //     member this.Queue = this.GetQueue()
    //     member this.Limits = WebGPU.GetLimits(this)