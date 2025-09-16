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
open WebGPU.Raw.WebGPUDebug

type FrontendDeviceDescriptor = 
    {
        Next : IDeviceDescriptorExtension
        Label : string
        DebugOutput : bool
        RequiredFeatures : array<FeatureName>
        RequiredLimits : Limits
        DefaultQueue : QueueDescriptor
    }


#nowarn "9"

type private GpuEnumerateAdaptersDelegate = delegate of nativeptr<WebGPU.Raw.InstanceDescriptor> * nativeptr<WebGPU.Raw.RequestAdapterOptions> * int32 * nativeptr<nativeint> * nativeptr<nativeint> -> int

// typedef struct WGPUInstanceDescriptor {
//     WGPUChainedStruct * nextInChain;
//     size_t requiredFeatureCount;
//     WGPUInstanceFeatureName const * requiredFeatures;
//     WGPU_NULLABLE WGPUInstanceLimits const * requiredLimits;
// } WGPUInstanceDescriptor WGPU_STRUCTURE_ATTRIBUTE;

[<AbstractClass; Sealed>]
type WebGPU private() =
    
    
    static let createInstanceAndGetAdapters (desc : nativeptr<WebGPU.Raw.InstanceDescriptor>) =
        match RuntimeInformation.ProcessArchitecture with
        | Architecture.Wasm ->
            [||], new Instance(0n)
        | _ ->
            
            let gpuEnumerateAdapters =
                let lib = Aardvark.LoadLibrary(typeof<WebGPU>.Assembly, "WebGPUNative")
                let sym = Aardvark.GetProcAddress(lib, "gpuEnumerateAdapters")
                Marshal.GetDelegateForFunctionPointer<GpuEnumerateAdaptersDelegate>(sym)
                
            let opt =
                {
                    Next = null
                    CompatibleSurface = Surface.Null
                    PowerPreference = PowerPreference.Undefined
                    BackendType = BackendType.Undefined
                    ForceFallbackAdapter = false
                    FeatureLevel = FeatureLevel.Undefined
                }
            
            opt.Pin (Unchecked.defaultof<_>, fun pOpt ->
                
                let mutable arr = Array.zeroCreate 128
                use pAdapters = fixed arr
                let mutable instance = 0n
                use pInstance = fixed &instance
                let cnt = gpuEnumerateAdapters.Invoke(desc, pOpt, arr.Length, pAdapters, pInstance)
                if cnt > arr.Length then
                    WebGPU.Raw.WebGPU.InstanceRelease(instance)
                    arr <- Array.zeroCreate cnt
                    use pAdapters = fixed arr
                    gpuEnumerateAdapters.Invoke(desc, pOpt, arr.Length, pAdapters, pInstance) |> ignore
                    
                
                let adapters = arr |> Array.truncate cnt |> Array.map (fun h -> new Adapter(h))
                let instance = new Instance(instance)
                adapters, instance
            )
    static let instanceAdapters = System.Collections.Generic.Dictionary<Instance, Adapter[]>()
    static member CreateInstance(desc : InstanceDescriptor) =
        desc.Pin(Unchecked.defaultof<_>, fun pDesc ->
            let a, i = createInstanceAndGetAdapters pDesc
            lock instanceAdapters (fun () -> instanceAdapters.[i] <- a)
            i.AddRef()
            i
        )
    
    static member CreateInstance() =
        WebGPU.CreateInstance { 
            Next = null
            RequiredFeatures = [| InstanceFeatureName.TimedWaitAny |]
            RequiredLimits = { TimedWaitAnyMaxCount = 1L }
        }
    
    [<Extension>]
    static member private RequestAdapterAsync(this : Instance, options : RequestAdapterOptions) =
        let tcs = TaskCompletionSource<Adapter>()
        
        let info : RequestAdapterCallbackInfo =
            {
                Mode = CallbackMode.WaitAnyOnly
                Callback =
                    RequestAdapterCallback(fun disp status adapter message ->
                        disp.Dispose()
                        match status with
                        | RequestAdapterStatus.Success -> tcs.SetResult adapter
                        | _ -> tcs.SetException(Exception $"could not create adapter: {message}")
                    )
            }
        
        this.RequestAdapter(options, info) |> this.EnqueueWait
        tcs.Task
        
    [<Extension>]
    static member CreateAdapter (instance : Instance, choose : Adapter[] -> Adapter) =
     
        let adapters = instanceAdapters.[instance]
        
        task {
            if adapters.Length = 0 then
                return!
                    instance.RequestAdapterAsync {
                        Next = null
                        CompatibleSurface = Surface.Null
                        PowerPreference = PowerPreference.Undefined
                        BackendType = BackendType.Undefined
                        ForceFallbackAdapter = false
                        FeatureLevel = FeatureLevel.Undefined
                    } 
            else
                let res = choose adapters
                res.AddRef()
                return res
        }
    
    [<Extension>]
    static member CreateAdapter (instance : Instance) =
        
        let score (a : AdapterInfo) =
            let typeScore = 
                match a.AdapterType with
                | AdapterType.DiscreteGPU -> 3
                | AdapterType.IntegratedGPU -> 2
                | AdapterType.CPU -> 1
                | _ -> 0
            let backendScore =
                if RuntimeInformation.IsOSPlatform OSPlatform.Windows then
                    match a.BackendType with
                    | BackendType.D3D12 -> 3
                    | BackendType.D3D11 -> 2
                    | BackendType.Vulkan -> 1
                    | _ -> 0
                elif RuntimeInformation.IsOSPlatform OSPlatform.OSX then
                    match a.BackendType with
                    | BackendType.Metal -> 3
                    | _ -> 0
                elif RuntimeInformation.IsOSPlatform OSPlatform.Linux then
                    match a.BackendType with
                    | BackendType.Vulkan -> 3
                    | _ -> 0
                else
                    0
            
            typeScore * 100 + backendScore
        
        WebGPU.CreateAdapter (instance, Array.maxBy (fun a -> score a.Info))
    
    //
    // static member CreateInstance(descriptor : InstanceDescriptor) =
    //         
    //     match RuntimeInformation.ProcessArchitecture with
    //     | Architecture.Wasm ->
    //         new Instance(0n)
    //     | _ ->
    //         let (_, instance) = adaptersAndInstance.Value
    //         instance
    //         // descriptor.Pin(Unchecked.defaultof<_>, fun ptr ->
    //         //     let handle = WebGPU.Raw.WebGPU.CreateInstance(ptr)
    //         //     new Instance(handle)
    //         // )
    //
    // static member CreateInstance() =
    //     WebGPU.CreateInstance {
    //         Next = null
    //         Features = WebGPU.InstanceFeatures
    //     }


      


[<AbstractClass; Sealed>]
type WebGPUExtensions private() =
    
    static let enumRx = Regex @"([a-zA-Z_0-9]+)::"
    static let lineRx = System.Text.RegularExpressions.Regex @"\r\n|\n|\r"
    
   
        
    [<Extension>]
    static member RequestDeviceAsync(this : Adapter, options : FrontendDeviceDescriptor) =
        let tcs = TaskCompletionSource<Device>()
            
        let err =
            if options.DebugOutput then
                {
                    Mode = CallbackMode.AllowSpontaneous
                    DeviceLostCallbackInfo.Callback =
                        DeviceLostCallback(fun _disp device typ message ->
                            let t = System.Diagnostics.StackTrace(4) |> string
                            let message = WebGPU.Raw.WebGPUDebug.processMessage message
                            let message = enumRx.Replace(message, "$1.") + "\n" + t
                            let lines = message.Split('\n')
                            Report.ErrorNoPrefix($"{typ} ERROR:")
                            for l in lines do
                                Report.ErrorNoPrefix($"  {l}")
                        )
                }
            else
                DeviceLostCallbackInfo.Null
        
        let errCb : UncapturedErrorCallbackInfo =
            {
                Callback = UncapturedErrorCallback(fun _ device typ str ->
                    let str = WebGPU.Raw.WebGPUDebug.processMessage str
                    Report.ErrorNoPrefix($"{typ} ERROR: {str}")
                )
            }
        
        let realOptions =
            {
                Next = options.Next
                Label = options.Label
                RequiredFeatures = options.RequiredFeatures
                RequiredLimits = options.RequiredLimits
                DefaultQueue = options.DefaultQueue
                DeviceLostCallbackInfo = err
                UncapturedErrorCallbackInfo = errCb
            }
        
        let info : RequestDeviceCallbackInfo =
            {
                Mode = CallbackMode.WaitAnyOnly
                Callback =
                    RequestDeviceCallback(fun disp status device message ->
                        disp.Dispose()
                        match status with
                        | RequestDeviceStatus.Success -> tcs.SetResult device
                        | _ -> tcs.SetException(Exception $"could not create device: {message}")
                    )
            }
        
        let f = this.RequestDevice(realOptions, info)
        let info : FutureWaitInfo = { Future = f; Completed = false }
        let mutable status = WaitStatus.TimedOut
        while status <> WaitStatus.Success do
            status <- this.Instance.WaitAny([|info|], 0L)
            
        task {
            let! dev = tcs.Task
            
            if options.DebugOutput then
                
                let info : LoggingCallbackInfo =
                    {
                        Callback =
                            LoggingCallback(fun _ t str ->
                                let str = WebGPU.Raw.WebGPUDebug.processMessage str
                                let lines = str.Split("\n")
                                for line in lines do 
                                    match t with
                                    | LoggingType.Error -> Report.ErrorNoPrefix("{0}", line)
                                    | LoggingType.Warning -> Report.WarnNoPrefix("{0}", line)
                                    | LoggingType.Info -> Report.Line("{0}", line)
                                    | _ -> Report.Line(4, "{0}", line)
                            )
                    }
                
                dev.SetLoggingCallback(info)
            
            return dev   
        }

    [<Extension>]
    static member GetFormatCapabilities(this : Adapter, format : TextureFormat) =
         let arr = Array.zeroCreate<_> 128
         use pArr = fixed arr
         let mutable drm = WebGPU.Raw.DawnDrmFormatCapabilities(0n, SType.DawnDrmFormatCapabilities, 128un, pArr)
         use pDrm = fixed &drm
         let mutable rr = WebGPU.Raw.DawnFormatCapabilities(NativePtr.toNativeInt pDrm)
         use ptr = fixed &rr
         let status = WebGPU.Raw.WebGPU.AdapterGetFormatCapabilities(this.Handle, format, ptr)
         if status <> Status.Success then
             failwith $"could not get format capabilities: {status}"
         DawnFormatCapabilities.Read(Unchecked.defaultof<_>, ptr, false)
         
    [<Extension>]
    static member GetCapabilities(this : Surface, adapter : Adapter) =
        let mutable res = Unchecked.defaultof<WebGPU.Raw.SurfaceCapabilities>
        use ptr = fixed &res
        let status = WebGPU.Raw.WebGPU.SurfaceGetCapabilities(this.Handle, adapter.Handle, ptr)
        if status <> Status.Success then
            failwith $"could not get surface capabilities: {status}"
        SurfaceCapabilities.Read(Unchecked.defaultof<_>, ptr, false)
    
   
    [<Extension>]
    static member GetWGSLLanguageFeatures(this : Instance) =
        let arr = [| Unchecked.defaultof<Raw.SupportedWGSLLanguageFeatures> |]
        use ptr = fixed arr
        WebGPU.Raw.WebGPU.InstanceGetWGSLLanguageFeatures(this.Handle, ptr)
        let res = arr.[0]
        Array.init (int res.FeatureCount) (fun i -> NativePtr.get res.Features i)
        
    [<Extension>]
    static member Wait(this : Queue) =
        let tcs = TaskCompletionSource()
        this.OnSubmittedWorkDone {
            QueueWorkDoneCallbackInfo.Mode = CallbackMode.WaitAnyOnly
            Callback =
                QueueWorkDoneCallback(fun d s msg ->
                    match s with
                    | QueueWorkDoneStatus.Success -> tcs.SetResult()
                    | _ -> tcs.SetException(Exception (sprintf "could not wait for queue %A: %s" s msg))
                )
        } |> this.Device.EnqueueWait
        tcs.Task
   
    [<Extension>]
    static member PopErrorScope(device : Device) =
        let tcs = TaskCompletionSource<_>()
        device.PopErrorScope {
            Mode = CallbackMode.WaitAnyOnly
            Callback = PopErrorScopeCallback (fun d status typ message ->
                d.Dispose()
                tcs.SetResult(typ, message)
                ()
            )
        } |> device.EnqueueWait
        tcs.Task

    [<Extension>]
    static member GetCompilationInfo(this : ShaderModule) =
        let tcs = TaskCompletionSource<_>()
        this.GetCompilationInfo {
            Mode = CallbackMode.WaitAnyOnly
            Callback = CompilationInfoCallback(fun d status info ->
                tcs.SetResult(info)
            )
        } |> this.Device.EnqueueWait
        tcs.Task

    [<Extension>]
    static member CompileShader(device : Device, shaderCode : string, ?label : string) =
        
        let shader =
            let label = defaultArg label (nolabel())
            WebGPU.Raw.WebGPUDebug.registerShaderModuleCode label shaderCode
            device.CreateShaderModule {
                Label = label
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
        


    [<Extension>]
    static member CompileShader(device : Device, spirv : uint32[]) =
        
        let shader =
            device.CreateShaderModule {
                Label = WebGPU.Raw.WebGPUDebug.nolabel()
                Next = { ShaderSourceSPIRV.Next = null; ShaderSourceSPIRV.Code = spirv }
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
        

    
[<AbstractClass; Sealed>]
type BufferRangeExtensions private() =
    
    [<Extension>]
    static member CreateBuffer<'a when 'a : unmanaged>(device : Device, usage : BufferUsage, data : ReadOnlySpan<'a>, ?label : string) =
        let size = int64 data.Length * int64 sizeof<'a>
        let isMappable = usage &&& BufferUsage.MapWrite <> BufferUsage.None
        let result =
            device.CreateBuffer {
                Next = null
                Label = defaultArg label (nolabel())
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
                    Label = nolabel()
                    Usage = BufferUsage.MapWrite ||| BufferUsage.CopySrc
                    Size = size
                    MappedAtCreation = true
                }
                
            let dst = tmp.GetMappedRange(0L, size)
            data.CopyTo(System.Span<'a>(NativePtr.toVoidPtr (NativePtr.ofNativeInt<byte> dst), data.Length))
            tmp.Unmap()
            
            use enc = device.CreateCommandEncoder { Label = nolabel(); Next = null }
            enc.CopyBufferToBuffer(tmp, 0L, result, 0L, size)
            use cmd = enc.Finish { Label = nolabel() }
            task {
                do! device.Queue.Submit [| cmd |]
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
    static member CreateView(tex : Texture, usage : TextureUsage, level : int) =
        tex.CreateView {
            Next = null
            Label = nolabel()
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
            Aspect = TextureAspect.All
            Usage = usage
        }
        
    [<Extension>]
    static member CreateView(tex : Texture, usage : TextureUsage, ?viewDimension : TextureViewDimension) =
        
        let viewDimension =
            match viewDimension with
            | Some d -> d
            | None ->
                match tex.Dimension with
                | TextureDimension.D1D -> TextureViewDimension.D1D
                | TextureDimension.D2D -> TextureViewDimension.D2D
                | TextureDimension.D3D -> TextureViewDimension.D3D
                | _ -> failwith "bad dim"
        
        let layerCount =
            match viewDimension with
            | TextureViewDimension.Cube -> 6
            | _ -> 1
        
        tex.CreateView {
            Next = null
            Label = nolabel() 
            Format = tex.Format
            Dimension = viewDimension
            BaseMipLevel = 0
            MipLevelCount = tex.MipLevelCount
            BaseArrayLayer = 0
            ArrayLayerCount = layerCount
            Aspect = TextureAspect.All
            Usage = usage
        }

[<AutoOpen>]
module ``F# Extensions`` =
    
    let inline nolabel() =
        WebGPU.Raw.WebGPUDebug.nolabel()
    
    let mipMapLevels1d (size : int) =
        1 + int (log2 (float size) |> floor)
  
    let mipMapLevels2d (size : V2i) =
        1 + int (log2 (float (max size.X size.Y)) |> floor)
  
    let mipMapLevels3d (size : V3i) =
        1 + int (log2 (float (max (max size.X size.Y) size.Z)) |> floor)
  
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
        
        static member Buffer(binding : int, buffer : Buffer, ?offset : int64, ?size : int64) =
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
                BindingArraySize = 0
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
                BindingArraySize = 0
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
                BindingArraySize = 0
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
                BindingArraySize = 0
            }
    type CompilationInfo with
        member x.HasErrors = x.Messages |> Array.exists (fun m -> m.Type <= CompilationMessageType.Error)
        member x.HasWarnings = x.Messages |> Array.exists (fun m -> m.Type <= CompilationMessageType.Warning)
    
        
    
    type Instance with
        member this.WGSLLanguageFeatures = this.GetWGSLLanguageFeatures()
        
    let inline RenderBundleEncoderDescriptor (e : RenderBundleEncoderDescriptor) = e
        
    let inline undefined<'a when 'a : (static member Null : 'a)> : 'a =
        'a.Null

        