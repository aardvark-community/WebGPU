namespace rec WebGPU
open System
open System.Text
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop
#nowarn "9"
[<AllowNullLiteral>]
type IAdapterInfoExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type IBindGroupEntryExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type IBindGroupLayoutEntryExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type IBufferDescriptorExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type IColorTargetStateExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type ICommandEncoderDescriptorExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type IComputePipelineDescriptorExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type IDeviceDescriptorExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type IFormatCapabilitiesExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type IInstanceDescriptorExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type IPipelineLayoutDescriptorExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type IRenderPassColorAttachmentExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type IRenderPassDescriptorExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type IRequestAdapterOptionsExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type ISamplerDescriptorExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type IShaderModuleDescriptorExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type ISharedFenceDescriptorExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type ISharedFenceExportInfoExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type ISharedTextureMemoryBeginAccessDescriptorExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type ISharedTextureMemoryDescriptorExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type ISharedTextureMemoryEndAccessStateExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type ISharedTextureMemoryPropertiesExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type ISupportedLimitsExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type ISurfaceDescriptorExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type ITextureDescriptorExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type ITextureViewDescriptorExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AbstractClass; Sealed>]
type private PinHelper() =
    static member inline PinNullable<'r>(x : IAdapterInfoExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : IBindGroupEntryExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : IBindGroupLayoutEntryExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : IBufferDescriptorExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : IColorTargetStateExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : ICommandEncoderDescriptorExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : IComputePipelineDescriptorExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : IDeviceDescriptorExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : IFormatCapabilitiesExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : IInstanceDescriptorExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : IPipelineLayoutDescriptorExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : IRenderPassColorAttachmentExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : IRenderPassDescriptorExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : IRequestAdapterOptionsExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : ISamplerDescriptorExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : IShaderModuleDescriptorExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : ISharedFenceDescriptorExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : ISharedFenceExportInfoExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : ISharedTextureMemoryBeginAccessDescriptorExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : ISharedTextureMemoryDescriptorExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : ISharedTextureMemoryEndAccessStateExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : ISharedTextureMemoryPropertiesExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : ISupportedLimitsExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : ISurfaceDescriptorExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : ITextureDescriptorExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
    static member inline PinNullable<'r>(x : ITextureViewDescriptorExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
type INTERNAL__HAVE_EMDAWNWEBGPU_HEADER = 
    {
        Unused : bool
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.INTERNAL__HAVE_EMDAWNWEBGPU_HEADER> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.INTERNAL__HAVE_EMDAWNWEBGPU_HEADER(
                (if this.Unused then 1 else 0)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.INTERNAL__HAVE_EMDAWNWEBGPU_HEADER> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.INTERNAL__HAVE_EMDAWNWEBGPU_HEADER>) = 
        {
            Unused = (backend.Unused <> 0)
        }
type Proc = delegate of unit -> unit
type RequestAdapterOptions = 
    {
        Next : IRequestAdapterOptionsExtension
        CompatibleSurface : Surface
        PowerPreference : PowerPreference
        BackendType : BackendType
        ForceFallbackAdapter : bool
        CompatibilityMode : bool
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RequestAdapterOptions> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            let mutable value =
                new WebGPU.Raw.RequestAdapterOptions(
                    nextInChain,
                    this.CompatibleSurface.Handle,
                    this.PowerPreference,
                    this.BackendType,
                    (if this.ForceFallbackAdapter then 1 else 0),
                    (if this.CompatibilityMode then 1 else 0)
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RequestAdapterOptions> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RequestAdapterOptions>) = 
        {
            Next = null
            CompatibleSurface = new Surface(backend.CompatibleSurface)
            PowerPreference = backend.PowerPreference
            BackendType = backend.BackendType
            ForceFallbackAdapter = (backend.ForceFallbackAdapter <> 0)
            CompatibilityMode = (backend.CompatibilityMode <> 0)
        }
type RequestAdapterCallback = delegate of status : RequestAdapterStatus * adapter : Adapter * message : StringView * userdata : nativeint -> unit
type RequestAdapterCallback2 = delegate of status : RequestAdapterStatus * adapter : Adapter * message : StringView -> unit
type RequestAdapterCallbackInfo = 
    {
        Mode : CallbackMode
        Callback : RequestAdapterCallback
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RequestAdapterCallbackInfo> -> 'r) : 'r = 
        let _callbackDel = WebGPU.Raw.RequestAdapterCallback(fun status adapter message userdata ->
            let _status = status
            let _adapter = new Adapter(adapter)
            let _message = StringView.Read(&message)
            let _userdata = userdata
            this.Callback.Invoke(_status, _adapter, _message, _userdata)
        )
        let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
        let mutable value =
            new WebGPU.Raw.RequestAdapterCallbackInfo(
                this.Mode,
                _callbackPtr,
                _callbackUserData
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RequestAdapterCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RequestAdapterCallbackInfo>) = 
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (userdata, backend.Userdata)]
        }
type RequestAdapterCallbackInfo2 = 
    {
        Mode : CallbackMode
        Callback : RequestAdapterCallback2
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RequestAdapterCallbackInfo2> -> 'r) : 'r = 
        let mutable _callbackGC = Unchecked.defaultof<GCHandle>
        let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.RequestAdapterCallback2>
        _callbackDel <- WebGPU.Raw.RequestAdapterCallback2(fun status adapter message ->
            _callbackGC.Free()
            let _status = status
            let _adapter = new Adapter(adapter)
            let _message = StringView.Read(&message)
            this.Callback.Invoke(_status, _adapter, _message)
        )
        _callbackGC <- GCHandle.Alloc(_callbackDel)
        let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
        let mutable value =
            new WebGPU.Raw.RequestAdapterCallbackInfo2(
                this.Mode,
                _callbackPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RequestAdapterCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RequestAdapterCallbackInfo2>) = 
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode)]
        }
type Adapter internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = Adapter(0n)
    member _.GetLimits(limits : byref<SupportedLimits>) : Status =
        limits.Pin(fun _limitsPtr ->
            let res = WebGPU.Raw.WebGPU.AdapterGetLimits(handle, _limitsPtr)
            res
        )
    member _.GetInfo(info : byref<AdapterInfo>) : Status =
        info.Pin(fun _infoPtr ->
            let res = WebGPU.Raw.WebGPU.AdapterGetInfo(handle, _infoPtr)
            res
        )
    member _.HasFeature(feature : FeatureName) : bool =
        let res = WebGPU.Raw.WebGPU.AdapterHasFeature(handle, feature)
        (res <> 0)
    member _.Features : SupportedFeatures =
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        WebGPU.Raw.WebGPU.AdapterGetFeatures(handle, ptr)
        SupportedFeatures.Read(&res)
    member _.RequestDevice(descriptor : DeviceDescriptor, callback : RequestDeviceCallback) : unit =
        descriptor.Pin(fun _descriptorPtr ->
            let _callbackDel = WebGPU.Raw.RequestDeviceCallback(fun status device message userdata ->
                let _status = status
                let _device = new Device(device)
                let _message = StringView.Read(&message)
                let _userdata = userdata
                callback.Invoke(_status, _device, _message, _userdata)
            )
            let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
            let res = WebGPU.Raw.WebGPU.AdapterRequestDevice(handle, _descriptorPtr, _callbackPtr, _callbackUserData)
            res
        )
    member _.RequestDeviceF(options : DeviceDescriptor, callbackInfo : RequestDeviceCallbackInfo) : Future =
        options.Pin(fun _optionsPtr ->
            callbackInfo.Pin(fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.AdapterRequestDeviceF(handle, _optionsPtr, NativePtr.read _callbackInfoPtr)
                Future.Read(&res)
            )
        )
    member _.RequestDevice2(options : DeviceDescriptor, callbackInfo : RequestDeviceCallbackInfo2) : Future =
        options.Pin(fun _optionsPtr ->
            callbackInfo.Pin(fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.AdapterRequestDevice2(handle, _optionsPtr, NativePtr.read _callbackInfoPtr)
                Future.Read(&res)
            )
        )
type AdapterInfo = 
    {
        Next : IAdapterInfoExtension
        Vendor : StringView
        Architecture : StringView
        Device : StringView
        Description : StringView
        BackendType : BackendType
        AdapterType : AdapterType
        VendorID : int
        DeviceID : int
        CompatibilityMode : bool
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.AdapterInfo> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            this.Vendor.Pin(fun _vendorPtr ->
                this.Architecture.Pin(fun _architecturePtr ->
                    this.Device.Pin(fun _devicePtr ->
                        this.Description.Pin(fun _descriptionPtr ->
                            let mutable value =
                                new WebGPU.Raw.AdapterInfo(
                                    nextInChain,
                                    NativePtr.read _vendorPtr,
                                    NativePtr.read _architecturePtr,
                                    NativePtr.read _devicePtr,
                                    NativePtr.read _descriptionPtr,
                                    this.BackendType,
                                    this.AdapterType,
                                    uint32(this.VendorID),
                                    uint32(this.DeviceID),
                                    (if this.CompatibilityMode then 1 else 0)
                                )
                            use ptr = fixed &value
                            action ptr
                        )
                    )
                )
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.AdapterInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.AdapterInfo>) = 
        {
            Next = null
            Vendor = StringView.Read(&backend.Vendor)
            Architecture = StringView.Read(&backend.Architecture)
            Device = StringView.Read(&backend.Device)
            Description = StringView.Read(&backend.Description)
            BackendType = backend.BackendType
            AdapterType = backend.AdapterType
            VendorID = int(backend.VendorID)
            DeviceID = int(backend.DeviceID)
            CompatibilityMode = (backend.CompatibilityMode <> 0)
        }
type DeviceDescriptor = 
    {
        Next : IDeviceDescriptorExtension
        Label : StringView
        RequiredFeatures : array<FeatureName>
        RequiredLimits : RequiredLimits
        DefaultQueue : QueueDescriptor
        DeviceLostCallbackInfo : DeviceLostCallbackInfo
        UncapturedErrorCallbackInfo : UncapturedErrorCallbackInfo
        DeviceLostCallbackInfo2 : DeviceLostCallbackInfo2
        UncapturedErrorCallbackInfo2 : UncapturedErrorCallbackInfo2
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DeviceDescriptor> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            this.Label.Pin(fun _labelPtr ->
                use requiredFeaturesPtr = fixed (this.RequiredFeatures)
                let requiredFeaturesLen = unativeint this.RequiredFeatures.Length
                this.RequiredLimits.Pin(fun _requiredLimitsPtr ->
                    this.DefaultQueue.Pin(fun _defaultQueuePtr ->
                        this.DeviceLostCallbackInfo.Pin(fun _deviceLostCallbackInfoPtr ->
                            this.UncapturedErrorCallbackInfo.Pin(fun _uncapturedErrorCallbackInfoPtr ->
                                this.DeviceLostCallbackInfo2.Pin(fun _deviceLostCallbackInfo2Ptr ->
                                    this.UncapturedErrorCallbackInfo2.Pin(fun _uncapturedErrorCallbackInfo2Ptr ->
                                        let mutable value =
                                            new WebGPU.Raw.DeviceDescriptor(
                                                nextInChain,
                                                NativePtr.read _labelPtr,
                                                requiredFeaturesLen,
                                                requiredFeaturesPtr,
                                                _requiredLimitsPtr,
                                                NativePtr.read _defaultQueuePtr,
                                                Unchecked.defaultof<_>,
                                                Unchecked.defaultof<_>,
                                                NativePtr.read _deviceLostCallbackInfoPtr,
                                                NativePtr.read _uncapturedErrorCallbackInfoPtr,
                                                NativePtr.read _deviceLostCallbackInfo2Ptr,
                                                NativePtr.read _uncapturedErrorCallbackInfo2Ptr
                                            )
                                        use ptr = fixed &value
                                        action ptr
                                    )
                                )
                            )
                        )
                    )
                )
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DeviceDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DeviceDescriptor>) = 
        {
            Next = null
            Label = StringView.Read(&backend.Label)
            RequiredFeatures = let ptr = backend.RequiredFeatures in Array.init (int backend.RequiredFeatureCount) (fun i -> NativePtr.get ptr i)
            RequiredLimits = let m = NativePtr.toByRef backend.RequiredLimits in RequiredLimits.Read(&m)
            DefaultQueue = QueueDescriptor.Read(&backend.DefaultQueue)
            DeviceLostCallbackInfo = DeviceLostCallbackInfo.Read(&backend.DeviceLostCallbackInfo)
            UncapturedErrorCallbackInfo = UncapturedErrorCallbackInfo.Read(&backend.UncapturedErrorCallbackInfo)
            DeviceLostCallbackInfo2 = DeviceLostCallbackInfo2.Read(&backend.DeviceLostCallbackInfo2)
            UncapturedErrorCallbackInfo2 = UncapturedErrorCallbackInfo2.Read(&backend.UncapturedErrorCallbackInfo2)
        }
type DawnLoadCacheDataFunction = delegate of key : nativeint * keySize : unativeint * value : nativeint * valueSize : unativeint * userdata : nativeint -> unativeint
type DawnStoreCacheDataFunction = delegate of key : nativeint * keySize : unativeint * value : nativeint * valueSize : unativeint * userdata : nativeint -> unit
type BindGroup internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = BindGroup(0n)
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.BindGroupSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
type BindGroupEntry = 
    {
        Next : IBindGroupEntryExtension
        Binding : int
        Buffer : Buffer
        Offset : uint64
        Size : uint64
        Sampler : Sampler
        TextureView : TextureView
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BindGroupEntry> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            let mutable value =
                new WebGPU.Raw.BindGroupEntry(
                    nextInChain,
                    uint32(this.Binding),
                    this.Buffer.Handle,
                    this.Offset,
                    this.Size,
                    this.Sampler.Handle,
                    this.TextureView.Handle
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BindGroupEntry> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BindGroupEntry>) = 
        {
            Next = null
            Binding = int(backend.Binding)
            Buffer = new Buffer(backend.Buffer)
            Offset = backend.Offset
            Size = backend.Size
            Sampler = new Sampler(backend.Sampler)
            TextureView = new TextureView(backend.TextureView)
        }
type BindGroupDescriptor = 
    {
        Label : StringView
        Layout : BindGroupLayout
        Entries : array<BindGroupEntry>
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BindGroupDescriptor> -> 'r) : 'r = 
        this.Label.Pin(fun _labelPtr ->
            WebGPU.Raw.Pinnable.pinArray this.Entries (fun entriesPtr ->
                let entriesLen = unativeint this.Entries.Length
                let mutable value =
                    new WebGPU.Raw.BindGroupDescriptor(
                        NativePtr.read _labelPtr,
                        this.Layout.Handle,
                        entriesLen,
                        entriesPtr
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BindGroupDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BindGroupDescriptor>) = 
        {
            Label = StringView.Read(&backend.Label)
            Layout = new BindGroupLayout(backend.Layout)
            Entries = let ptr = backend.Entries in Array.init (int backend.EntryCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in BindGroupEntry.Read(&r))
        }
type BindGroupLayout internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = BindGroupLayout(0n)
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.BindGroupLayoutSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
type BufferBindingLayout = 
    {
        Type : BufferBindingType
        HasDynamicOffset : bool
        MinBindingSize : uint64
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BufferBindingLayout> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.BufferBindingLayout(
                this.Type,
                (if this.HasDynamicOffset then 1 else 0),
                this.MinBindingSize
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BufferBindingLayout> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BufferBindingLayout>) = 
        {
            Type = backend.Type
            HasDynamicOffset = (backend.HasDynamicOffset <> 0)
            MinBindingSize = backend.MinBindingSize
        }
type SamplerBindingLayout = 
    {
        Type : SamplerBindingType
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SamplerBindingLayout> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.SamplerBindingLayout(
                this.Type
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SamplerBindingLayout> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SamplerBindingLayout>) = 
        {
            Type = backend.Type
        }
type TextureBindingLayout = 
    {
        SampleType : TextureSampleType
        ViewDimension : TextureViewDimension
        Multisampled : bool
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.TextureBindingLayout> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.TextureBindingLayout(
                this.SampleType,
                this.ViewDimension,
                (if this.Multisampled then 1 else 0)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.TextureBindingLayout> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.TextureBindingLayout>) = 
        {
            SampleType = backend.SampleType
            ViewDimension = backend.ViewDimension
            Multisampled = (backend.Multisampled <> 0)
        }
type SurfaceCapabilities = 
    {
        Usages : TextureUsage
        Formats : array<TextureFormat>
        PresentModes : array<PresentMode>
        AlphaModes : array<CompositeAlphaMode>
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SurfaceCapabilities> -> 'r) : 'r = 
        use formatsPtr = fixed (this.Formats)
        let formatsLen = unativeint this.Formats.Length
        use presentModesPtr = fixed (this.PresentModes)
        let presentModesLen = unativeint this.PresentModes.Length
        use alphaModesPtr = fixed (this.AlphaModes)
        let alphaModesLen = unativeint this.AlphaModes.Length
        let mutable value =
            new WebGPU.Raw.SurfaceCapabilities(
                this.Usages,
                formatsLen,
                formatsPtr,
                presentModesLen,
                presentModesPtr,
                alphaModesLen,
                alphaModesPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceCapabilities> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SurfaceCapabilities>) = 
        {
            Usages = backend.Usages
            Formats = let ptr = backend.Formats in Array.init (int backend.FormatCount) (fun i -> NativePtr.get ptr i)
            PresentModes = let ptr = backend.PresentModes in Array.init (int backend.PresentModeCount) (fun i -> NativePtr.get ptr i)
            AlphaModes = let ptr = backend.AlphaModes in Array.init (int backend.AlphaModeCount) (fun i -> NativePtr.get ptr i)
        }
type SurfaceConfiguration = 
    {
        Device : Device
        Format : TextureFormat
        Usage : TextureUsage
        ViewFormats : array<TextureFormat>
        AlphaMode : CompositeAlphaMode
        Width : int
        Height : int
        PresentMode : PresentMode
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SurfaceConfiguration> -> 'r) : 'r = 
        use viewFormatsPtr = fixed (this.ViewFormats)
        let viewFormatsLen = unativeint this.ViewFormats.Length
        let mutable value =
            new WebGPU.Raw.SurfaceConfiguration(
                this.Device.Handle,
                this.Format,
                this.Usage,
                viewFormatsLen,
                viewFormatsPtr,
                this.AlphaMode,
                uint32(this.Width),
                uint32(this.Height),
                this.PresentMode
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceConfiguration> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SurfaceConfiguration>) = 
        {
            Device = new Device(backend.Device)
            Format = backend.Format
            Usage = backend.Usage
            ViewFormats = let ptr = backend.ViewFormats in Array.init (int backend.ViewFormatCount) (fun i -> NativePtr.get ptr i)
            AlphaMode = backend.AlphaMode
            Width = int(backend.Width)
            Height = int(backend.Height)
            PresentMode = backend.PresentMode
        }
type StorageTextureBindingLayout = 
    {
        Access : StorageTextureAccess
        Format : TextureFormat
        ViewDimension : TextureViewDimension
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.StorageTextureBindingLayout> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.StorageTextureBindingLayout(
                this.Access,
                this.Format,
                this.ViewDimension
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.StorageTextureBindingLayout> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.StorageTextureBindingLayout>) = 
        {
            Access = backend.Access
            Format = backend.Format
            ViewDimension = backend.ViewDimension
        }
type BindGroupLayoutEntry = 
    {
        Next : IBindGroupLayoutEntryExtension
        Binding : int
        Visibility : ShaderStage
        Buffer : BufferBindingLayout
        Sampler : SamplerBindingLayout
        Texture : TextureBindingLayout
        StorageTexture : StorageTextureBindingLayout
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BindGroupLayoutEntry> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            this.Buffer.Pin(fun _bufferPtr ->
                this.Sampler.Pin(fun _samplerPtr ->
                    this.Texture.Pin(fun _texturePtr ->
                        this.StorageTexture.Pin(fun _storageTexturePtr ->
                            let mutable value =
                                new WebGPU.Raw.BindGroupLayoutEntry(
                                    nextInChain,
                                    uint32(this.Binding),
                                    this.Visibility,
                                    NativePtr.read _bufferPtr,
                                    NativePtr.read _samplerPtr,
                                    NativePtr.read _texturePtr,
                                    NativePtr.read _storageTexturePtr
                                )
                            use ptr = fixed &value
                            action ptr
                        )
                    )
                )
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BindGroupLayoutEntry> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BindGroupLayoutEntry>) = 
        {
            Next = null
            Binding = int(backend.Binding)
            Visibility = backend.Visibility
            Buffer = BufferBindingLayout.Read(&backend.Buffer)
            Sampler = SamplerBindingLayout.Read(&backend.Sampler)
            Texture = TextureBindingLayout.Read(&backend.Texture)
            StorageTexture = StorageTextureBindingLayout.Read(&backend.StorageTexture)
        }
type BindGroupLayoutDescriptor = 
    {
        Label : StringView
        Entries : array<BindGroupLayoutEntry>
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BindGroupLayoutDescriptor> -> 'r) : 'r = 
        this.Label.Pin(fun _labelPtr ->
            WebGPU.Raw.Pinnable.pinArray this.Entries (fun entriesPtr ->
                let entriesLen = unativeint this.Entries.Length
                let mutable value =
                    new WebGPU.Raw.BindGroupLayoutDescriptor(
                        NativePtr.read _labelPtr,
                        entriesLen,
                        entriesPtr
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BindGroupLayoutDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BindGroupLayoutDescriptor>) = 
        {
            Label = StringView.Read(&backend.Label)
            Entries = let ptr = backend.Entries in Array.init (int backend.EntryCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in BindGroupLayoutEntry.Read(&r))
        }
type BlendComponent = 
    {
        Operation : BlendOperation
        SrcFactor : BlendFactor
        DstFactor : BlendFactor
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BlendComponent> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.BlendComponent(
                this.Operation,
                this.SrcFactor,
                this.DstFactor
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BlendComponent> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BlendComponent>) = 
        {
            Operation = backend.Operation
            SrcFactor = backend.SrcFactor
            DstFactor = backend.DstFactor
        }
type StringView = 
    {
        Data : string
        Length : unativeint
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.StringView> -> 'r) : 'r = 
        use _dataPtr = fixed (Encoding.UTF8.GetBytes(this.Data))
        let mutable value =
            new WebGPU.Raw.StringView(
                _dataPtr,
                this.Length
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.StringView> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.StringView>) = 
        {
            Data = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Data)
            Length = backend.Length
        }
type Buffer internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = Buffer(0n)
    member _.MapAsync(mode : MapMode, offset : unativeint, size : unativeint, callback : BufferMapCallback) : unit =
        let _callbackDel = WebGPU.Raw.BufferMapCallback(fun status userdata ->
            let _status = status
            let _userdata = userdata
            callback.Invoke(_status, _userdata)
        )
        let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
        let res = WebGPU.Raw.WebGPU.BufferMapAsync(handle, mode, offset, size, _callbackPtr, _callbackUserData)
        res
    member _.MapAsyncF(mode : MapMode, offset : unativeint, size : unativeint, callbackInfo : BufferMapCallbackInfo) : Future =
        callbackInfo.Pin(fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.BufferMapAsyncF(handle, mode, offset, size, NativePtr.read _callbackInfoPtr)
            Future.Read(&res)
        )
    member _.MapAsync2(mode : MapMode, offset : unativeint, size : unativeint, callbackInfo : BufferMapCallbackInfo2) : Future =
        callbackInfo.Pin(fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.BufferMapAsync2(handle, mode, offset, size, NativePtr.read _callbackInfoPtr)
            Future.Read(&res)
        )
    member _.GetMappedRange(offset : unativeint, size : unativeint) : nativeint =
        let res = WebGPU.Raw.WebGPU.BufferGetMappedRange(handle, offset, size)
        res
    member _.GetConstMappedRange(offset : unativeint, size : unativeint) : nativeint =
        let res = WebGPU.Raw.WebGPU.BufferGetConstMappedRange(handle, offset, size)
        res
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.BufferSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
    member _.GetUsage() : BufferUsage =
        let res = WebGPU.Raw.WebGPU.BufferGetUsage(handle)
        res
    member _.GetSize() : uint64 =
        let res = WebGPU.Raw.WebGPU.BufferGetSize(handle)
        res
    member _.GetMapState() : BufferMapState =
        let res = WebGPU.Raw.WebGPU.BufferGetMapState(handle)
        res
    member _.Unmap() : unit =
        let res = WebGPU.Raw.WebGPU.BufferUnmap(handle)
        res
    member _.Destroy() : unit =
        let res = WebGPU.Raw.WebGPU.BufferDestroy(handle)
        res
    member x.Dispose() = x.Destroy()
    interface System.IDisposable with
        member x.Dispose() = x.Dispose()
type BufferDescriptor = 
    {
        Next : IBufferDescriptorExtension
        Label : StringView
        Usage : BufferUsage
        Size : uint64
        MappedAtCreation : bool
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BufferDescriptor> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            this.Label.Pin(fun _labelPtr ->
                let mutable value =
                    new WebGPU.Raw.BufferDescriptor(
                        nextInChain,
                        NativePtr.read _labelPtr,
                        this.Usage,
                        this.Size,
                        (if this.MappedAtCreation then 1 else 0)
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BufferDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BufferDescriptor>) = 
        {
            Next = null
            Label = StringView.Read(&backend.Label)
            Usage = backend.Usage
            Size = backend.Size
            MappedAtCreation = (backend.MappedAtCreation <> 0)
        }
type Callback = delegate of userdata : nativeint -> unit
type BufferMapCallback = delegate of status : BufferMapAsyncStatus * userdata : nativeint -> unit
type BufferMapCallback2 = delegate of status : MapAsyncStatus * message : StringView -> unit
type BufferMapCallbackInfo = 
    {
        Mode : CallbackMode
        Callback : BufferMapCallback
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BufferMapCallbackInfo> -> 'r) : 'r = 
        let _callbackDel = WebGPU.Raw.BufferMapCallback(fun status userdata ->
            let _status = status
            let _userdata = userdata
            this.Callback.Invoke(_status, _userdata)
        )
        let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
        let mutable value =
            new WebGPU.Raw.BufferMapCallbackInfo(
                this.Mode,
                _callbackPtr,
                _callbackUserData
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BufferMapCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BufferMapCallbackInfo>) = 
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (userdata, backend.Userdata)]
        }
type BufferMapCallbackInfo2 = 
    {
        Mode : CallbackMode
        Callback : BufferMapCallback2
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BufferMapCallbackInfo2> -> 'r) : 'r = 
        let mutable _callbackGC = Unchecked.defaultof<GCHandle>
        let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.BufferMapCallback2>
        _callbackDel <- WebGPU.Raw.BufferMapCallback2(fun status message ->
            _callbackGC.Free()
            let _status = status
            let _message = StringView.Read(&message)
            this.Callback.Invoke(_status, _message)
        )
        _callbackGC <- GCHandle.Alloc(_callbackDel)
        let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
        let mutable value =
            new WebGPU.Raw.BufferMapCallbackInfo2(
                this.Mode,
                _callbackPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BufferMapCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BufferMapCallbackInfo2>) = 
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode)]
        }
type Color = 
    {
        R : double
        G : double
        B : double
        A : double
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.Color> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.Color(
                this.R,
                this.G,
                this.B,
                this.A
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.Color> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.Color>) = 
        {
            R = backend.R
            G = backend.G
            B = backend.B
            A = backend.A
        }
type ConstantEntry = 
    {
        Key : StringView
        Value : double
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ConstantEntry> -> 'r) : 'r = 
        this.Key.Pin(fun _keyPtr ->
            let mutable value =
                new WebGPU.Raw.ConstantEntry(
                    NativePtr.read _keyPtr,
                    this.Value
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ConstantEntry> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ConstantEntry>) = 
        {
            Key = StringView.Read(&backend.Key)
            Value = backend.Value
        }
type CommandBuffer internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = CommandBuffer(0n)
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.CommandBufferSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
type CommandBufferDescriptor = 
    {
        Label : StringView
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CommandBufferDescriptor> -> 'r) : 'r = 
        this.Label.Pin(fun _labelPtr ->
            let mutable value =
                new WebGPU.Raw.CommandBufferDescriptor(
                    NativePtr.read _labelPtr
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CommandBufferDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CommandBufferDescriptor>) = 
        {
            Label = StringView.Read(&backend.Label)
        }
type CommandEncoder internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = CommandEncoder(0n)
    member _.Finish(descriptor : CommandBufferDescriptor) : CommandBuffer =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.CommandEncoderFinish(handle, _descriptorPtr)
            new CommandBuffer(res)
        )
    member _.BeginComputePass(descriptor : ComputePassDescriptor) : ComputePassEncoder =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.CommandEncoderBeginComputePass(handle, _descriptorPtr)
            new ComputePassEncoder(res)
        )
    member _.BeginRenderPass(descriptor : RenderPassDescriptor) : RenderPassEncoder =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.CommandEncoderBeginRenderPass(handle, _descriptorPtr)
            new RenderPassEncoder(res)
        )
    member _.CopyBufferToBuffer(source : Buffer, sourceOffset : uint64, destination : Buffer, destinationOffset : uint64, size : uint64) : unit =
        let res = WebGPU.Raw.WebGPU.CommandEncoderCopyBufferToBuffer(handle, source.Handle, sourceOffset, destination.Handle, destinationOffset, size)
        res
    member _.CopyBufferToTexture(source : ImageCopyBuffer, destination : ImageCopyTexture, copySize : Extent3D) : unit =
        source.Pin(fun _sourcePtr ->
            destination.Pin(fun _destinationPtr ->
                copySize.Pin(fun _copySizePtr ->
                    let res = WebGPU.Raw.WebGPU.CommandEncoderCopyBufferToTexture(handle, _sourcePtr, _destinationPtr, _copySizePtr)
                    res
                )
            )
        )
    member _.CopyTextureToBuffer(source : ImageCopyTexture, destination : ImageCopyBuffer, copySize : Extent3D) : unit =
        source.Pin(fun _sourcePtr ->
            destination.Pin(fun _destinationPtr ->
                copySize.Pin(fun _copySizePtr ->
                    let res = WebGPU.Raw.WebGPU.CommandEncoderCopyTextureToBuffer(handle, _sourcePtr, _destinationPtr, _copySizePtr)
                    res
                )
            )
        )
    member _.CopyTextureToTexture(source : ImageCopyTexture, destination : ImageCopyTexture, copySize : Extent3D) : unit =
        source.Pin(fun _sourcePtr ->
            destination.Pin(fun _destinationPtr ->
                copySize.Pin(fun _copySizePtr ->
                    let res = WebGPU.Raw.WebGPU.CommandEncoderCopyTextureToTexture(handle, _sourcePtr, _destinationPtr, _copySizePtr)
                    res
                )
            )
        )
    member _.ClearBuffer(buffer : Buffer, offset : uint64, size : uint64) : unit =
        let res = WebGPU.Raw.WebGPU.CommandEncoderClearBuffer(handle, buffer.Handle, offset, size)
        res
    member _.InsertDebugMarker(markerLabel : StringView) : unit =
        markerLabel.Pin(fun _markerLabelPtr ->
            let res = WebGPU.Raw.WebGPU.CommandEncoderInsertDebugMarker(handle, NativePtr.read _markerLabelPtr)
            res
        )
    member _.PopDebugGroup() : unit =
        let res = WebGPU.Raw.WebGPU.CommandEncoderPopDebugGroup(handle)
        res
    member _.PushDebugGroup(groupLabel : StringView) : unit =
        groupLabel.Pin(fun _groupLabelPtr ->
            let res = WebGPU.Raw.WebGPU.CommandEncoderPushDebugGroup(handle, NativePtr.read _groupLabelPtr)
            res
        )
    member _.ResolveQuerySet(querySet : QuerySet, firstQuery : int, queryCount : int, destination : Buffer, destinationOffset : uint64) : unit =
        let res = WebGPU.Raw.WebGPU.CommandEncoderResolveQuerySet(handle, querySet.Handle, uint32(firstQuery), uint32(queryCount), destination.Handle, destinationOffset)
        res
    member _.WriteTimestamp(querySet : QuerySet, queryIndex : int) : unit =
        let res = WebGPU.Raw.WebGPU.CommandEncoderWriteTimestamp(handle, querySet.Handle, uint32(queryIndex))
        res
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.CommandEncoderSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
type CommandEncoderDescriptor = 
    {
        Next : ICommandEncoderDescriptorExtension
        Label : StringView
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CommandEncoderDescriptor> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            this.Label.Pin(fun _labelPtr ->
                let mutable value =
                    new WebGPU.Raw.CommandEncoderDescriptor(
                        nextInChain,
                        NativePtr.read _labelPtr
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CommandEncoderDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CommandEncoderDescriptor>) = 
        {
            Next = null
            Label = StringView.Read(&backend.Label)
        }
type CompilationInfo = 
    {
        Messages : array<CompilationMessage>
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CompilationInfo> -> 'r) : 'r = 
        WebGPU.Raw.Pinnable.pinArray this.Messages (fun messagesPtr ->
            let messagesLen = unativeint this.Messages.Length
            let mutable value =
                new WebGPU.Raw.CompilationInfo(
                    messagesLen,
                    messagesPtr
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CompilationInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CompilationInfo>) = 
        {
            Messages = let ptr = backend.Messages in Array.init (int backend.MessageCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in CompilationMessage.Read(&r))
        }
type CompilationInfoCallback = delegate of status : CompilationInfoRequestStatus * compilationInfo : CompilationInfo * userdata : nativeint -> unit
type CompilationInfoCallback2 = delegate of status : CompilationInfoRequestStatus * compilationInfo : CompilationInfo -> unit
type CompilationInfoCallbackInfo = 
    {
        Mode : CallbackMode
        Callback : CompilationInfoCallback
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CompilationInfoCallbackInfo> -> 'r) : 'r = 
        let _callbackDel = WebGPU.Raw.CompilationInfoCallback(fun status compilationInfo userdata ->
            let _status = status
            let _compilationInfo = let m = NativePtr.toByRef compilationInfo in CompilationInfo.Read(&m)
            let _userdata = userdata
            this.Callback.Invoke(_status, _compilationInfo, _userdata)
        )
        let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
        let mutable value =
            new WebGPU.Raw.CompilationInfoCallbackInfo(
                this.Mode,
                _callbackPtr,
                _callbackUserData
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CompilationInfoCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CompilationInfoCallbackInfo>) = 
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (userdata, backend.Userdata)]
        }
type CompilationInfoCallbackInfo2 = 
    {
        Mode : CallbackMode
        Callback : CompilationInfoCallback2
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CompilationInfoCallbackInfo2> -> 'r) : 'r = 
        let mutable _callbackGC = Unchecked.defaultof<GCHandle>
        let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.CompilationInfoCallback2>
        _callbackDel <- WebGPU.Raw.CompilationInfoCallback2(fun status compilationInfo ->
            _callbackGC.Free()
            let _status = status
            let _compilationInfo = let m = NativePtr.toByRef compilationInfo in CompilationInfo.Read(&m)
            this.Callback.Invoke(_status, _compilationInfo)
        )
        _callbackGC <- GCHandle.Alloc(_callbackDel)
        let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
        let mutable value =
            new WebGPU.Raw.CompilationInfoCallbackInfo2(
                this.Mode,
                _callbackPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CompilationInfoCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CompilationInfoCallbackInfo2>) = 
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode)]
        }
type CompilationMessage = 
    {
        Message : StringView
        Type : CompilationMessageType
        LineNum : uint64
        LinePos : uint64
        Offset : uint64
        Length : uint64
        Utf16LinePos : uint64
        Utf16Offset : uint64
        Utf16Length : uint64
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CompilationMessage> -> 'r) : 'r = 
        this.Message.Pin(fun _messagePtr ->
            let mutable value =
                new WebGPU.Raw.CompilationMessage(
                    NativePtr.read _messagePtr,
                    this.Type,
                    this.LineNum,
                    this.LinePos,
                    this.Offset,
                    this.Length,
                    this.Utf16LinePos,
                    this.Utf16Offset,
                    this.Utf16Length
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CompilationMessage> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CompilationMessage>) = 
        {
            Message = StringView.Read(&backend.Message)
            Type = backend.Type
            LineNum = backend.LineNum
            LinePos = backend.LinePos
            Offset = backend.Offset
            Length = backend.Length
            Utf16LinePos = backend.Utf16LinePos
            Utf16Offset = backend.Utf16Offset
            Utf16Length = backend.Utf16Length
        }
type ComputePassDescriptor = 
    {
        Label : StringView
        TimestampWrites : ComputePassTimestampWrites
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ComputePassDescriptor> -> 'r) : 'r = 
        this.Label.Pin(fun _labelPtr ->
            this.TimestampWrites.Pin(fun _timestampWritesPtr ->
                let mutable value =
                    new WebGPU.Raw.ComputePassDescriptor(
                        NativePtr.read _labelPtr,
                        _timestampWritesPtr
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ComputePassDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ComputePassDescriptor>) = 
        {
            Label = StringView.Read(&backend.Label)
            TimestampWrites = let m = NativePtr.toByRef backend.TimestampWrites in ComputePassTimestampWrites.Read(&m)
        }
type ComputePassEncoder internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = ComputePassEncoder(0n)
    member _.InsertDebugMarker(markerLabel : StringView) : unit =
        markerLabel.Pin(fun _markerLabelPtr ->
            let res = WebGPU.Raw.WebGPU.ComputePassEncoderInsertDebugMarker(handle, NativePtr.read _markerLabelPtr)
            res
        )
    member _.PopDebugGroup() : unit =
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderPopDebugGroup(handle)
        res
    member _.PushDebugGroup(groupLabel : StringView) : unit =
        groupLabel.Pin(fun _groupLabelPtr ->
            let res = WebGPU.Raw.WebGPU.ComputePassEncoderPushDebugGroup(handle, NativePtr.read _groupLabelPtr)
            res
        )
    member _.SetPipeline(pipeline : ComputePipeline) : unit =
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderSetPipeline(handle, pipeline.Handle)
        res
    member _.SetBindGroup(groupIndex : int, group : BindGroup, dynamicOffsets : array<uint32>) : unit =
        use dynamicOffsetsPtr = fixed (dynamicOffsets)
        let dynamicOffsetsLen = unativeint dynamicOffsets.Length
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderSetBindGroup(handle, uint32(groupIndex), group.Handle, dynamicOffsetsLen, dynamicOffsetsPtr)
        res
    member _.WriteTimestamp(querySet : QuerySet, queryIndex : int) : unit =
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderWriteTimestamp(handle, querySet.Handle, uint32(queryIndex))
        res
    member _.DispatchWorkgroups(workgroupCountX : int, workgroupCountY : int, workgroupCountZ : int) : unit =
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderDispatchWorkgroups(handle, uint32(workgroupCountX), uint32(workgroupCountY), uint32(workgroupCountZ))
        res
    member _.DispatchWorkgroupsIndirect(indirectBuffer : Buffer, indirectOffset : uint64) : unit =
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderDispatchWorkgroupsIndirect(handle, indirectBuffer.Handle, indirectOffset)
        res
    member _.End() : unit =
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderEnd(handle)
        res
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.ComputePassEncoderSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
type ComputePassTimestampWrites = 
    {
        QuerySet : QuerySet
        BeginningOfPassWriteIndex : int
        EndOfPassWriteIndex : int
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ComputePassTimestampWrites> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.ComputePassTimestampWrites(
                this.QuerySet.Handle,
                uint32(this.BeginningOfPassWriteIndex),
                uint32(this.EndOfPassWriteIndex)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ComputePassTimestampWrites> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ComputePassTimestampWrites>) = 
        {
            QuerySet = new QuerySet(backend.QuerySet)
            BeginningOfPassWriteIndex = int(backend.BeginningOfPassWriteIndex)
            EndOfPassWriteIndex = int(backend.EndOfPassWriteIndex)
        }
type ComputePipeline internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = ComputePipeline(0n)
    member _.GetBindGroupLayout(groupIndex : int) : BindGroupLayout =
        let res = WebGPU.Raw.WebGPU.ComputePipelineGetBindGroupLayout(handle, uint32(groupIndex))
        new BindGroupLayout(res)
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.ComputePipelineSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
type ComputePipelineDescriptor = 
    {
        Next : IComputePipelineDescriptorExtension
        Label : StringView
        Layout : PipelineLayout
        Compute : ProgrammableStageDescriptor
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ComputePipelineDescriptor> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            this.Label.Pin(fun _labelPtr ->
                this.Compute.Pin(fun _computePtr ->
                    let mutable value =
                        new WebGPU.Raw.ComputePipelineDescriptor(
                            nextInChain,
                            NativePtr.read _labelPtr,
                            this.Layout.Handle,
                            NativePtr.read _computePtr
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ComputePipelineDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ComputePipelineDescriptor>) = 
        {
            Next = null
            Label = StringView.Read(&backend.Label)
            Layout = new PipelineLayout(backend.Layout)
            Compute = ProgrammableStageDescriptor.Read(&backend.Compute)
        }
type CreateComputePipelineAsyncCallback = delegate of status : CreatePipelineAsyncStatus * pipeline : ComputePipeline * message : StringView * userdata : nativeint -> unit
type CreateComputePipelineAsyncCallback2 = delegate of status : CreatePipelineAsyncStatus * pipeline : ComputePipeline * message : StringView -> unit
type CreateComputePipelineAsyncCallbackInfo = 
    {
        Mode : CallbackMode
        Callback : CreateComputePipelineAsyncCallback
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo> -> 'r) : 'r = 
        let _callbackDel = WebGPU.Raw.CreateComputePipelineAsyncCallback(fun status pipeline message userdata ->
            let _status = status
            let _pipeline = new ComputePipeline(pipeline)
            let _message = StringView.Read(&message)
            let _userdata = userdata
            this.Callback.Invoke(_status, _pipeline, _message, _userdata)
        )
        let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
        let mutable value =
            new WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo(
                this.Mode,
                _callbackPtr,
                _callbackUserData
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo>) = 
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (userdata, backend.Userdata)]
        }
type CreateComputePipelineAsyncCallbackInfo2 = 
    {
        Mode : CallbackMode
        Callback : CreateComputePipelineAsyncCallback2
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo2> -> 'r) : 'r = 
        let mutable _callbackGC = Unchecked.defaultof<GCHandle>
        let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.CreateComputePipelineAsyncCallback2>
        _callbackDel <- WebGPU.Raw.CreateComputePipelineAsyncCallback2(fun status pipeline message ->
            _callbackGC.Free()
            let _status = status
            let _pipeline = new ComputePipeline(pipeline)
            let _message = StringView.Read(&message)
            this.Callback.Invoke(_status, _pipeline, _message)
        )
        _callbackGC <- GCHandle.Alloc(_callbackDel)
        let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
        let mutable value =
            new WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo2(
                this.Mode,
                _callbackPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo2>) = 
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode)]
        }
type CreateRenderPipelineAsyncCallback = delegate of status : CreatePipelineAsyncStatus * pipeline : RenderPipeline * message : StringView * userdata : nativeint -> unit
type CreateRenderPipelineAsyncCallback2 = delegate of status : CreatePipelineAsyncStatus * pipeline : RenderPipeline * message : StringView -> unit
type CreateRenderPipelineAsyncCallbackInfo = 
    {
        Mode : CallbackMode
        Callback : CreateRenderPipelineAsyncCallback
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo> -> 'r) : 'r = 
        let _callbackDel = WebGPU.Raw.CreateRenderPipelineAsyncCallback(fun status pipeline message userdata ->
            let _status = status
            let _pipeline = new RenderPipeline(pipeline)
            let _message = StringView.Read(&message)
            let _userdata = userdata
            this.Callback.Invoke(_status, _pipeline, _message, _userdata)
        )
        let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
        let mutable value =
            new WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo(
                this.Mode,
                _callbackPtr,
                _callbackUserData
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo>) = 
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (userdata, backend.Userdata)]
        }
type CreateRenderPipelineAsyncCallbackInfo2 = 
    {
        Mode : CallbackMode
        Callback : CreateRenderPipelineAsyncCallback2
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo2> -> 'r) : 'r = 
        let mutable _callbackGC = Unchecked.defaultof<GCHandle>
        let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.CreateRenderPipelineAsyncCallback2>
        _callbackDel <- WebGPU.Raw.CreateRenderPipelineAsyncCallback2(fun status pipeline message ->
            _callbackGC.Free()
            let _status = status
            let _pipeline = new RenderPipeline(pipeline)
            let _message = StringView.Read(&message)
            this.Callback.Invoke(_status, _pipeline, _message)
        )
        _callbackGC <- GCHandle.Alloc(_callbackDel)
        let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
        let mutable value =
            new WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo2(
                this.Mode,
                _callbackPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo2>) = 
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode)]
        }
type Device internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = Device(0n)
    member _.CreateBindGroup(descriptor : BindGroupDescriptor) : BindGroup =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateBindGroup(handle, _descriptorPtr)
            new BindGroup(res)
        )
    member _.CreateBindGroupLayout(descriptor : BindGroupLayoutDescriptor) : BindGroupLayout =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateBindGroupLayout(handle, _descriptorPtr)
            new BindGroupLayout(res)
        )
    member _.CreateBuffer(descriptor : BufferDescriptor) : Buffer =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateBuffer(handle, _descriptorPtr)
            new Buffer(res)
        )
    member _.CreateCommandEncoder(descriptor : CommandEncoderDescriptor) : CommandEncoder =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateCommandEncoder(handle, _descriptorPtr)
            new CommandEncoder(res)
        )
    member _.CreateComputePipeline(descriptor : ComputePipelineDescriptor) : ComputePipeline =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateComputePipeline(handle, _descriptorPtr)
            new ComputePipeline(res)
        )
    member _.CreateComputePipelineAsync(descriptor : ComputePipelineDescriptor, callback : CreateComputePipelineAsyncCallback) : unit =
        descriptor.Pin(fun _descriptorPtr ->
            let _callbackDel = WebGPU.Raw.CreateComputePipelineAsyncCallback(fun status pipeline message userdata ->
                let _status = status
                let _pipeline = new ComputePipeline(pipeline)
                let _message = StringView.Read(&message)
                let _userdata = userdata
                callback.Invoke(_status, _pipeline, _message, _userdata)
            )
            let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
            let res = WebGPU.Raw.WebGPU.DeviceCreateComputePipelineAsync(handle, _descriptorPtr, _callbackPtr, _callbackUserData)
            res
        )
    member _.CreateComputePipelineAsyncF(descriptor : ComputePipelineDescriptor, callbackInfo : CreateComputePipelineAsyncCallbackInfo) : Future =
        descriptor.Pin(fun _descriptorPtr ->
            callbackInfo.Pin(fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.DeviceCreateComputePipelineAsyncF(handle, _descriptorPtr, NativePtr.read _callbackInfoPtr)
                Future.Read(&res)
            )
        )
    member _.CreateComputePipelineAsync2(descriptor : ComputePipelineDescriptor, callbackInfo : CreateComputePipelineAsyncCallbackInfo2) : Future =
        descriptor.Pin(fun _descriptorPtr ->
            callbackInfo.Pin(fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.DeviceCreateComputePipelineAsync2(handle, _descriptorPtr, NativePtr.read _callbackInfoPtr)
                Future.Read(&res)
            )
        )
    member _.CreatePipelineLayout(descriptor : PipelineLayoutDescriptor) : PipelineLayout =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreatePipelineLayout(handle, _descriptorPtr)
            new PipelineLayout(res)
        )
    member _.CreateQuerySet(descriptor : QuerySetDescriptor) : QuerySet =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateQuerySet(handle, _descriptorPtr)
            new QuerySet(res)
        )
    member _.CreateRenderPipelineAsync(descriptor : RenderPipelineDescriptor, callback : CreateRenderPipelineAsyncCallback) : unit =
        descriptor.Pin(fun _descriptorPtr ->
            let _callbackDel = WebGPU.Raw.CreateRenderPipelineAsyncCallback(fun status pipeline message userdata ->
                let _status = status
                let _pipeline = new RenderPipeline(pipeline)
                let _message = StringView.Read(&message)
                let _userdata = userdata
                callback.Invoke(_status, _pipeline, _message, _userdata)
            )
            let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
            let res = WebGPU.Raw.WebGPU.DeviceCreateRenderPipelineAsync(handle, _descriptorPtr, _callbackPtr, _callbackUserData)
            res
        )
    member _.CreateRenderPipelineAsyncF(descriptor : RenderPipelineDescriptor, callbackInfo : CreateRenderPipelineAsyncCallbackInfo) : Future =
        descriptor.Pin(fun _descriptorPtr ->
            callbackInfo.Pin(fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.DeviceCreateRenderPipelineAsyncF(handle, _descriptorPtr, NativePtr.read _callbackInfoPtr)
                Future.Read(&res)
            )
        )
    member _.CreateRenderPipelineAsync2(descriptor : RenderPipelineDescriptor, callbackInfo : CreateRenderPipelineAsyncCallbackInfo2) : Future =
        descriptor.Pin(fun _descriptorPtr ->
            callbackInfo.Pin(fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.DeviceCreateRenderPipelineAsync2(handle, _descriptorPtr, NativePtr.read _callbackInfoPtr)
                Future.Read(&res)
            )
        )
    member _.CreateRenderBundleEncoder(descriptor : RenderBundleEncoderDescriptor) : RenderBundleEncoder =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateRenderBundleEncoder(handle, _descriptorPtr)
            new RenderBundleEncoder(res)
        )
    member _.CreateRenderPipeline(descriptor : RenderPipelineDescriptor) : RenderPipeline =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateRenderPipeline(handle, _descriptorPtr)
            new RenderPipeline(res)
        )
    member _.CreateSampler(descriptor : SamplerDescriptor) : Sampler =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateSampler(handle, _descriptorPtr)
            new Sampler(res)
        )
    member _.CreateShaderModule(descriptor : ShaderModuleDescriptor) : ShaderModule =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateShaderModule(handle, _descriptorPtr)
            new ShaderModule(res)
        )
    member _.CreateTexture(descriptor : TextureDescriptor) : Texture =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateTexture(handle, _descriptorPtr)
            new Texture(res)
        )
    member _.Destroy() : unit =
        let res = WebGPU.Raw.WebGPU.DeviceDestroy(handle)
        res
    member _.GetLimits(limits : byref<SupportedLimits>) : Status =
        limits.Pin(fun _limitsPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceGetLimits(handle, _limitsPtr)
            res
        )
    member _.GetLostFuture() : Future =
        let res = WebGPU.Raw.WebGPU.DeviceGetLostFuture(handle)
        Future.Read(&res)
    member _.HasFeature(feature : FeatureName) : bool =
        let res = WebGPU.Raw.WebGPU.DeviceHasFeature(handle, feature)
        (res <> 0)
    member _.Features : SupportedFeatures =
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        WebGPU.Raw.WebGPU.DeviceGetFeatures(handle, ptr)
        SupportedFeatures.Read(&res)
    member _.GetAdapterInfo(adapterInfo : byref<AdapterInfo>) : Status =
        adapterInfo.Pin(fun _adapterInfoPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceGetAdapterInfo(handle, _adapterInfoPtr)
            res
        )
    member _.GetQueue() : Queue =
        let res = WebGPU.Raw.WebGPU.DeviceGetQueue(handle)
        new Queue(res)
    member _.SetUncapturedErrorCallback(callback : ErrorCallback) : unit =
        let _callbackDel = WebGPU.Raw.ErrorCallback(fun typ message userdata ->
            let _typ = typ
            let _message = StringView.Read(&message)
            let _userdata = userdata
            callback.Invoke(_typ, _message, _userdata)
        )
        let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
        let res = WebGPU.Raw.WebGPU.DeviceSetUncapturedErrorCallback(handle, _callbackPtr, _callbackUserData)
        res
    member _.PushErrorScope(filter : ErrorFilter) : unit =
        let res = WebGPU.Raw.WebGPU.DevicePushErrorScope(handle, filter)
        res
    member _.PopErrorScope(oldCallback : ErrorCallback) : unit =
        let _oldCallbackDel = WebGPU.Raw.ErrorCallback(fun typ message userdata ->
            let _typ = typ
            let _message = StringView.Read(&message)
            let _userdata = userdata
            oldCallback.Invoke(_typ, _message, _userdata)
        )
        let struct(_oldCallbackPtr, _oldCallbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_oldCallbackDel)
        let res = WebGPU.Raw.WebGPU.DevicePopErrorScope(handle, _oldCallbackPtr, _oldCallbackUserData)
        res
    member _.PopErrorScopeF(callbackInfo : PopErrorScopeCallbackInfo) : Future =
        callbackInfo.Pin(fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.DevicePopErrorScopeF(handle, NativePtr.read _callbackInfoPtr)
            Future.Read(&res)
        )
    member _.PopErrorScope2(callbackInfo : PopErrorScopeCallbackInfo2) : Future =
        callbackInfo.Pin(fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.DevicePopErrorScope2(handle, NativePtr.read _callbackInfoPtr)
            Future.Read(&res)
        )
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
    member x.Dispose() = x.Destroy()
    interface System.IDisposable with
        member x.Dispose() = x.Dispose()
type DeviceLostCallback = delegate of reason : DeviceLostReason * message : StringView * userdata : nativeint -> unit
type DeviceLostCallbackNew = delegate of device : Device * reason : DeviceLostReason * message : StringView * userdata : nativeint -> unit
type DeviceLostCallback2 = delegate of device : Device * reason : DeviceLostReason * message : StringView -> unit
type DeviceLostCallbackInfo = 
    {
        Mode : CallbackMode
        Callback : DeviceLostCallbackNew
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DeviceLostCallbackInfo> -> 'r) : 'r = 
        let _callbackDel = WebGPU.Raw.DeviceLostCallbackNew(fun device reason message userdata ->
            let _device = let ptr = device in new Device(NativePtr.read ptr)
            let _reason = reason
            let _message = StringView.Read(&message)
            let _userdata = userdata
            this.Callback.Invoke(_device, _reason, _message, _userdata)
        )
        let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
        let mutable value =
            new WebGPU.Raw.DeviceLostCallbackInfo(
                this.Mode,
                _callbackPtr,
                _callbackUserData
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DeviceLostCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DeviceLostCallbackInfo>) = 
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (userdata, backend.Userdata)]
        }
type DeviceLostCallbackInfo2 = 
    {
        Mode : CallbackMode
        Callback : DeviceLostCallback2
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DeviceLostCallbackInfo2> -> 'r) : 'r = 
        let mutable _callbackGC = Unchecked.defaultof<GCHandle>
        let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.DeviceLostCallback2>
        _callbackDel <- WebGPU.Raw.DeviceLostCallback2(fun device reason message ->
            _callbackGC.Free()
            let _device = let ptr = device in new Device(NativePtr.read ptr)
            let _reason = reason
            let _message = StringView.Read(&message)
            this.Callback.Invoke(_device, _reason, _message)
        )
        _callbackGC <- GCHandle.Alloc(_callbackDel)
        let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
        let mutable value =
            new WebGPU.Raw.DeviceLostCallbackInfo2(
                this.Mode,
                _callbackPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DeviceLostCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DeviceLostCallbackInfo2>) = 
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode)]
        }
type ErrorCallback = delegate of typ : ErrorType * message : StringView * userdata : nativeint -> unit
type UncapturedErrorCallback = delegate of device : Device * typ : ErrorType * message : StringView -> unit
type UncapturedErrorCallbackInfo = 
    {
        Callback : ErrorCallback
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.UncapturedErrorCallbackInfo> -> 'r) : 'r = 
        let _callbackDel = WebGPU.Raw.ErrorCallback(fun typ message userdata ->
            let _typ = typ
            let _message = StringView.Read(&message)
            let _userdata = userdata
            this.Callback.Invoke(_typ, _message, _userdata)
        )
        let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
        let mutable value =
            new WebGPU.Raw.UncapturedErrorCallbackInfo(
                _callbackPtr,
                _callbackUserData
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.UncapturedErrorCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.UncapturedErrorCallbackInfo>) = 
        {
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (userdata, backend.Userdata)]
        }
type UncapturedErrorCallbackInfo2 = 
    {
        Callback : UncapturedErrorCallback
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.UncapturedErrorCallbackInfo2> -> 'r) : 'r = 
        let mutable _callbackGC = Unchecked.defaultof<GCHandle>
        let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.UncapturedErrorCallback>
        _callbackDel <- WebGPU.Raw.UncapturedErrorCallback(fun device typ message ->
            _callbackGC.Free()
            let _device = let ptr = device in new Device(NativePtr.read ptr)
            let _typ = typ
            let _message = StringView.Read(&message)
            this.Callback.Invoke(_device, _typ, _message)
        )
        _callbackGC <- GCHandle.Alloc(_callbackDel)
        let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
        let mutable value =
            new WebGPU.Raw.UncapturedErrorCallbackInfo2(
                _callbackPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.UncapturedErrorCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.UncapturedErrorCallbackInfo2>) = 
        {
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback)]
        }
type PopErrorScopeCallback = delegate of status : PopErrorScopeStatus * typ : ErrorType * message : StringView * userdata : nativeint -> unit
type PopErrorScopeCallback2 = delegate of status : PopErrorScopeStatus * typ : ErrorType * message : StringView -> unit
type PopErrorScopeCallbackInfo = 
    {
        Mode : CallbackMode
        Callback : PopErrorScopeCallback
        OldCallback : ErrorCallback
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.PopErrorScopeCallbackInfo> -> 'r) : 'r = 
        let mutable _callbackGC = Unchecked.defaultof<GCHandle>
        let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.PopErrorScopeCallback>
        _callbackDel <- WebGPU.Raw.PopErrorScopeCallback(fun status typ message userdata ->
            _callbackGC.Free()
            let _status = status
            let _typ = typ
            let _message = StringView.Read(&message)
            let _userdata = userdata
            this.Callback.Invoke(_status, _typ, _message, _userdata)
        )
        _callbackGC <- GCHandle.Alloc(_callbackDel)
        let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
        let _oldCallbackDel = WebGPU.Raw.ErrorCallback(fun typ message userdata ->
            let _typ = typ
            let _message = StringView.Read(&message)
            let _userdata = userdata
            this.OldCallback.Invoke(_typ, _message, _userdata)
        )
        let struct(_oldCallbackPtr, _oldCallbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_oldCallbackDel)
        let mutable value =
            new WebGPU.Raw.PopErrorScopeCallbackInfo(
                this.Mode,
                _callbackPtr,
                _oldCallbackPtr,
                _oldCallbackUserData
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.PopErrorScopeCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.PopErrorScopeCallbackInfo>) = 
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (old callback, backend.OldCallback); ... ]
            OldCallback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (old callback, backend.OldCallback); ... ]
        }
type PopErrorScopeCallbackInfo2 = 
    {
        Mode : CallbackMode
        Callback : PopErrorScopeCallback2
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.PopErrorScopeCallbackInfo2> -> 'r) : 'r = 
        let mutable _callbackGC = Unchecked.defaultof<GCHandle>
        let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.PopErrorScopeCallback2>
        _callbackDel <- WebGPU.Raw.PopErrorScopeCallback2(fun status typ message ->
            _callbackGC.Free()
            let _status = status
            let _typ = typ
            let _message = StringView.Read(&message)
            this.Callback.Invoke(_status, _typ, _message)
        )
        _callbackGC <- GCHandle.Alloc(_callbackDel)
        let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
        let mutable value =
            new WebGPU.Raw.PopErrorScopeCallbackInfo2(
                this.Mode,
                _callbackPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.PopErrorScopeCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.PopErrorScopeCallbackInfo2>) = 
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode)]
        }
type Limits = 
    {
        MaxTextureDimension1D : int
        MaxTextureDimension2D : int
        MaxTextureDimension3D : int
        MaxTextureArrayLayers : int
        MaxBindGroups : int
        MaxBindGroupsPlusVertexBuffers : int
        MaxBindingsPerBindGroup : int
        MaxDynamicUniformBuffersPerPipelineLayout : int
        MaxDynamicStorageBuffersPerPipelineLayout : int
        MaxSampledTexturesPerShaderStage : int
        MaxSamplersPerShaderStage : int
        MaxStorageBuffersPerShaderStage : int
        MaxStorageTexturesPerShaderStage : int
        MaxUniformBuffersPerShaderStage : int
        MaxUniformBufferBindingSize : uint64
        MaxStorageBufferBindingSize : uint64
        MinUniformBufferOffsetAlignment : int
        MinStorageBufferOffsetAlignment : int
        MaxVertexBuffers : int
        MaxBufferSize : uint64
        MaxVertexAttributes : int
        MaxVertexBufferArrayStride : int
        MaxInterStageShaderComponents : int
        MaxInterStageShaderVariables : int
        MaxColorAttachments : int
        MaxColorAttachmentBytesPerSample : int
        MaxComputeWorkgroupStorageSize : int
        MaxComputeInvocationsPerWorkgroup : int
        MaxComputeWorkgroupSizeX : int
        MaxComputeWorkgroupSizeY : int
        MaxComputeWorkgroupSizeZ : int
        MaxComputeWorkgroupsPerDimension : int
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.Limits> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.Limits(
                uint32(this.MaxTextureDimension1D),
                uint32(this.MaxTextureDimension2D),
                uint32(this.MaxTextureDimension3D),
                uint32(this.MaxTextureArrayLayers),
                uint32(this.MaxBindGroups),
                uint32(this.MaxBindGroupsPlusVertexBuffers),
                uint32(this.MaxBindingsPerBindGroup),
                uint32(this.MaxDynamicUniformBuffersPerPipelineLayout),
                uint32(this.MaxDynamicStorageBuffersPerPipelineLayout),
                uint32(this.MaxSampledTexturesPerShaderStage),
                uint32(this.MaxSamplersPerShaderStage),
                uint32(this.MaxStorageBuffersPerShaderStage),
                uint32(this.MaxStorageTexturesPerShaderStage),
                uint32(this.MaxUniformBuffersPerShaderStage),
                this.MaxUniformBufferBindingSize,
                this.MaxStorageBufferBindingSize,
                uint32(this.MinUniformBufferOffsetAlignment),
                uint32(this.MinStorageBufferOffsetAlignment),
                uint32(this.MaxVertexBuffers),
                this.MaxBufferSize,
                uint32(this.MaxVertexAttributes),
                uint32(this.MaxVertexBufferArrayStride),
                uint32(this.MaxInterStageShaderComponents),
                uint32(this.MaxInterStageShaderVariables),
                uint32(this.MaxColorAttachments),
                uint32(this.MaxColorAttachmentBytesPerSample),
                uint32(this.MaxComputeWorkgroupStorageSize),
                uint32(this.MaxComputeInvocationsPerWorkgroup),
                uint32(this.MaxComputeWorkgroupSizeX),
                uint32(this.MaxComputeWorkgroupSizeY),
                uint32(this.MaxComputeWorkgroupSizeZ),
                uint32(this.MaxComputeWorkgroupsPerDimension)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.Limits> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.Limits>) = 
        {
            MaxTextureDimension1D = int(backend.MaxTextureDimension1D)
            MaxTextureDimension2D = int(backend.MaxTextureDimension2D)
            MaxTextureDimension3D = int(backend.MaxTextureDimension3D)
            MaxTextureArrayLayers = int(backend.MaxTextureArrayLayers)
            MaxBindGroups = int(backend.MaxBindGroups)
            MaxBindGroupsPlusVertexBuffers = int(backend.MaxBindGroupsPlusVertexBuffers)
            MaxBindingsPerBindGroup = int(backend.MaxBindingsPerBindGroup)
            MaxDynamicUniformBuffersPerPipelineLayout = int(backend.MaxDynamicUniformBuffersPerPipelineLayout)
            MaxDynamicStorageBuffersPerPipelineLayout = int(backend.MaxDynamicStorageBuffersPerPipelineLayout)
            MaxSampledTexturesPerShaderStage = int(backend.MaxSampledTexturesPerShaderStage)
            MaxSamplersPerShaderStage = int(backend.MaxSamplersPerShaderStage)
            MaxStorageBuffersPerShaderStage = int(backend.MaxStorageBuffersPerShaderStage)
            MaxStorageTexturesPerShaderStage = int(backend.MaxStorageTexturesPerShaderStage)
            MaxUniformBuffersPerShaderStage = int(backend.MaxUniformBuffersPerShaderStage)
            MaxUniformBufferBindingSize = backend.MaxUniformBufferBindingSize
            MaxStorageBufferBindingSize = backend.MaxStorageBufferBindingSize
            MinUniformBufferOffsetAlignment = int(backend.MinUniformBufferOffsetAlignment)
            MinStorageBufferOffsetAlignment = int(backend.MinStorageBufferOffsetAlignment)
            MaxVertexBuffers = int(backend.MaxVertexBuffers)
            MaxBufferSize = backend.MaxBufferSize
            MaxVertexAttributes = int(backend.MaxVertexAttributes)
            MaxVertexBufferArrayStride = int(backend.MaxVertexBufferArrayStride)
            MaxInterStageShaderComponents = int(backend.MaxInterStageShaderComponents)
            MaxInterStageShaderVariables = int(backend.MaxInterStageShaderVariables)
            MaxColorAttachments = int(backend.MaxColorAttachments)
            MaxColorAttachmentBytesPerSample = int(backend.MaxColorAttachmentBytesPerSample)
            MaxComputeWorkgroupStorageSize = int(backend.MaxComputeWorkgroupStorageSize)
            MaxComputeInvocationsPerWorkgroup = int(backend.MaxComputeInvocationsPerWorkgroup)
            MaxComputeWorkgroupSizeX = int(backend.MaxComputeWorkgroupSizeX)
            MaxComputeWorkgroupSizeY = int(backend.MaxComputeWorkgroupSizeY)
            MaxComputeWorkgroupSizeZ = int(backend.MaxComputeWorkgroupSizeZ)
            MaxComputeWorkgroupsPerDimension = int(backend.MaxComputeWorkgroupsPerDimension)
        }
type RequiredLimits = 
    {
        Limits : Limits
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RequiredLimits> -> 'r) : 'r = 
        this.Limits.Pin(fun _limitsPtr ->
            let mutable value =
                new WebGPU.Raw.RequiredLimits(
                    NativePtr.read _limitsPtr
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RequiredLimits> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RequiredLimits>) = 
        {
            Limits = Limits.Read(&backend.Limits)
        }
type SupportedLimits = 
    {
        Next : ISupportedLimitsExtension
        Limits : Limits
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SupportedLimits> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            this.Limits.Pin(fun _limitsPtr ->
                let mutable value =
                    new WebGPU.Raw.SupportedLimits(
                        nextInChain,
                        NativePtr.read _limitsPtr
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SupportedLimits> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SupportedLimits>) = 
        {
            Next = null
            Limits = Limits.Read(&backend.Limits)
        }
type SupportedFeatures = 
    {
        Features : array<FeatureName>
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SupportedFeatures> -> 'r) : 'r = 
        use featuresPtr = fixed (this.Features)
        let featuresLen = unativeint this.Features.Length
        let mutable value =
            new WebGPU.Raw.SupportedFeatures(
                featuresLen,
                featuresPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SupportedFeatures> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SupportedFeatures>) = 
        {
            Features = let ptr = backend.Features in Array.init (int backend.FeatureCount) (fun i -> NativePtr.get ptr i)
        }
type LoggingCallback = delegate of typ : LoggingType * message : StringView * userdata : nativeint -> unit
type Extent3D = 
    {
        Width : int
        Height : int
        DepthOrArrayLayers : int
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.Extent3D> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.Extent3D(
                uint32(this.Width),
                uint32(this.Height),
                uint32(this.DepthOrArrayLayers)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.Extent3D> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.Extent3D>) = 
        {
            Width = int(backend.Width)
            Height = int(backend.Height)
            DepthOrArrayLayers = int(backend.DepthOrArrayLayers)
        }
type ImageCopyBuffer = 
    {
        Layout : TextureDataLayout
        Buffer : Buffer
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ImageCopyBuffer> -> 'r) : 'r = 
        this.Layout.Pin(fun _layoutPtr ->
            let mutable value =
                new WebGPU.Raw.ImageCopyBuffer(
                    NativePtr.read _layoutPtr,
                    this.Buffer.Handle
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ImageCopyBuffer> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ImageCopyBuffer>) = 
        {
            Layout = TextureDataLayout.Read(&backend.Layout)
            Buffer = new Buffer(backend.Buffer)
        }
type ImageCopyTexture = 
    {
        Texture : Texture
        MipLevel : int
        Origin : Origin3D
        Aspect : TextureAspect
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ImageCopyTexture> -> 'r) : 'r = 
        this.Origin.Pin(fun _originPtr ->
            let mutable value =
                new WebGPU.Raw.ImageCopyTexture(
                    this.Texture.Handle,
                    uint32(this.MipLevel),
                    NativePtr.read _originPtr,
                    this.Aspect
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ImageCopyTexture> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ImageCopyTexture>) = 
        {
            Texture = new Texture(backend.Texture)
            MipLevel = int(backend.MipLevel)
            Origin = Origin3D.Read(&backend.Origin)
            Aspect = backend.Aspect
        }
type Instance internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = Instance(0n)
    member _.CreateSurface(descriptor : SurfaceDescriptor) : Surface =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.InstanceCreateSurface(handle, _descriptorPtr)
            new Surface(res)
        )
    member _.ProcessEvents() : unit =
        let res = WebGPU.Raw.WebGPU.InstanceProcessEvents(handle)
        res
    member _.WaitAny(futures : array<FutureWaitInfo>, timeoutNS : uint64) : WaitStatus =
        WebGPU.Raw.Pinnable.pinArray futures (fun futuresPtr ->
            let futuresLen = unativeint futures.Length
            let res = WebGPU.Raw.WebGPU.InstanceWaitAny(handle, futuresLen, futuresPtr, timeoutNS)
            res
        )
    member _.RequestAdapter(options : RequestAdapterOptions, callback : RequestAdapterCallback) : unit =
        options.Pin(fun _optionsPtr ->
            let _callbackDel = WebGPU.Raw.RequestAdapterCallback(fun status adapter message userdata ->
                let _status = status
                let _adapter = new Adapter(adapter)
                let _message = StringView.Read(&message)
                let _userdata = userdata
                callback.Invoke(_status, _adapter, _message, _userdata)
            )
            let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
            let res = WebGPU.Raw.WebGPU.InstanceRequestAdapter(handle, _optionsPtr, _callbackPtr, _callbackUserData)
            res
        )
    member _.RequestAdapterF(options : RequestAdapterOptions, callbackInfo : RequestAdapterCallbackInfo) : Future =
        options.Pin(fun _optionsPtr ->
            callbackInfo.Pin(fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.InstanceRequestAdapterF(handle, _optionsPtr, NativePtr.read _callbackInfoPtr)
                Future.Read(&res)
            )
        )
    member _.RequestAdapter2(options : RequestAdapterOptions, callbackInfo : RequestAdapterCallbackInfo2) : Future =
        options.Pin(fun _optionsPtr ->
            callbackInfo.Pin(fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.InstanceRequestAdapter2(handle, _optionsPtr, NativePtr.read _callbackInfoPtr)
                Future.Read(&res)
            )
        )
    member _.HasWGSLLanguageFeature(feature : WGSLFeatureName) : bool =
        let res = WebGPU.Raw.WebGPU.InstanceHasWGSLLanguageFeature(handle, feature)
        (res <> 0)
    member _.EnumerateWGSLLanguageFeatures(features : WGSLFeatureName) : unativeint =
        let mutable featuresHandle = features
        use featuresPtr = fixed (&featuresHandle)
        let res = WebGPU.Raw.WebGPU.InstanceEnumerateWGSLLanguageFeatures(handle, featuresPtr)
        res
type Future = 
    {
        Id : uint64
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.Future> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.Future(
                this.Id
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.Future> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.Future>) = 
        {
            Id = backend.Id
        }
type FutureWaitInfo = 
    {
        Future : Future
        Completed : bool
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.FutureWaitInfo> -> 'r) : 'r = 
        this.Future.Pin(fun _futurePtr ->
            let mutable value =
                new WebGPU.Raw.FutureWaitInfo(
                    NativePtr.read _futurePtr,
                    (if this.Completed then 1 else 0)
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.FutureWaitInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.FutureWaitInfo>) = 
        {
            Future = Future.Read(&backend.Future)
            Completed = (backend.Completed <> 0)
        }
type InstanceFeatures = 
    {
        TimedWaitAnyEnable : bool
        TimedWaitAnyMaxCount : unativeint
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.InstanceFeatures> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.InstanceFeatures(
                (if this.TimedWaitAnyEnable then 1 else 0),
                this.TimedWaitAnyMaxCount
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.InstanceFeatures> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.InstanceFeatures>) = 
        {
            TimedWaitAnyEnable = (backend.TimedWaitAnyEnable <> 0)
            TimedWaitAnyMaxCount = backend.TimedWaitAnyMaxCount
        }
type InstanceDescriptor = 
    {
        Next : IInstanceDescriptorExtension
        Features : InstanceFeatures
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.InstanceDescriptor> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            this.Features.Pin(fun _featuresPtr ->
                let mutable value =
                    new WebGPU.Raw.InstanceDescriptor(
                        nextInChain,
                        NativePtr.read _featuresPtr
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.InstanceDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.InstanceDescriptor>) = 
        {
            Next = null
            Features = InstanceFeatures.Read(&backend.Features)
        }
type VertexAttribute = 
    {
        Format : VertexFormat
        Offset : uint64
        ShaderLocation : int
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.VertexAttribute> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.VertexAttribute(
                this.Format,
                this.Offset,
                uint32(this.ShaderLocation)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.VertexAttribute> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.VertexAttribute>) = 
        {
            Format = backend.Format
            Offset = backend.Offset
            ShaderLocation = int(backend.ShaderLocation)
        }
type VertexBufferLayout = 
    {
        ArrayStride : uint64
        StepMode : VertexStepMode
        Attributes : array<VertexAttribute>
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.VertexBufferLayout> -> 'r) : 'r = 
        WebGPU.Raw.Pinnable.pinArray this.Attributes (fun attributesPtr ->
            let attributesLen = unativeint this.Attributes.Length
            let mutable value =
                new WebGPU.Raw.VertexBufferLayout(
                    this.ArrayStride,
                    this.StepMode,
                    attributesLen,
                    attributesPtr
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.VertexBufferLayout> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.VertexBufferLayout>) = 
        {
            ArrayStride = backend.ArrayStride
            StepMode = backend.StepMode
            Attributes = let ptr = backend.Attributes in Array.init (int backend.AttributeCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in VertexAttribute.Read(&r))
        }
type Origin3D = 
    {
        X : int
        Y : int
        Z : int
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.Origin3D> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.Origin3D(
                uint32(this.X),
                uint32(this.Y),
                uint32(this.Z)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.Origin3D> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.Origin3D>) = 
        {
            X = int(backend.X)
            Y = int(backend.Y)
            Z = int(backend.Z)
        }
type PipelineLayout internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = PipelineLayout(0n)
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.PipelineLayoutSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
type PipelineLayoutDescriptor = 
    {
        Next : IPipelineLayoutDescriptorExtension
        Label : StringView
        BindGroupLayouts : array<BindGroupLayout>
        ImmediateDataRangeByteSize : int
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.PipelineLayoutDescriptor> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            this.Label.Pin(fun _labelPtr ->
                let bindGroupLayoutsHandles = this.BindGroupLayouts |> Array.map (fun a -> a.Handle)
                use bindGroupLayoutsPtr = fixed (bindGroupLayoutsHandles)
                let bindGroupLayoutsLen = unativeint this.BindGroupLayouts.Length
                let mutable value =
                    new WebGPU.Raw.PipelineLayoutDescriptor(
                        nextInChain,
                        NativePtr.read _labelPtr,
                        bindGroupLayoutsLen,
                        bindGroupLayoutsPtr,
                        uint32(this.ImmediateDataRangeByteSize)
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.PipelineLayoutDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.PipelineLayoutDescriptor>) = 
        {
            Next = null
            Label = StringView.Read(&backend.Label)
            BindGroupLayouts = let ptr = backend.BindGroupLayouts in Array.init (int backend.BindGroupLayoutCount) (fun i -> new BindGroupLayout(NativePtr.get ptr i))
            ImmediateDataRangeByteSize = int(backend.ImmediateDataRangeByteSize)
        }
type ProgrammableStageDescriptor = 
    {
        Module : ShaderModule
        EntryPoint : StringView
        Constants : array<ConstantEntry>
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ProgrammableStageDescriptor> -> 'r) : 'r = 
        this.EntryPoint.Pin(fun _entryPointPtr ->
            WebGPU.Raw.Pinnable.pinArray this.Constants (fun constantsPtr ->
                let constantsLen = unativeint this.Constants.Length
                let mutable value =
                    new WebGPU.Raw.ProgrammableStageDescriptor(
                        this.Module.Handle,
                        NativePtr.read _entryPointPtr,
                        constantsLen,
                        constantsPtr
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ProgrammableStageDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ProgrammableStageDescriptor>) = 
        {
            Module = new ShaderModule(backend.Module)
            EntryPoint = StringView.Read(&backend.EntryPoint)
            Constants = let ptr = backend.Constants in Array.init (int backend.ConstantCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in ConstantEntry.Read(&r))
        }
type QuerySet internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = QuerySet(0n)
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.QuerySetSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
    member _.GetType() : QueryType =
        let res = WebGPU.Raw.WebGPU.QuerySetGetType(handle)
        res
    member _.GetCount() : int =
        let res = WebGPU.Raw.WebGPU.QuerySetGetCount(handle)
        int(res)
    member _.Destroy() : unit =
        let res = WebGPU.Raw.WebGPU.QuerySetDestroy(handle)
        res
    member x.Dispose() = x.Destroy()
    interface System.IDisposable with
        member x.Dispose() = x.Dispose()
type QuerySetDescriptor = 
    {
        Label : StringView
        Type : QueryType
        Count : int
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.QuerySetDescriptor> -> 'r) : 'r = 
        this.Label.Pin(fun _labelPtr ->
            let mutable value =
                new WebGPU.Raw.QuerySetDescriptor(
                    NativePtr.read _labelPtr,
                    this.Type,
                    uint32(this.Count)
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.QuerySetDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.QuerySetDescriptor>) = 
        {
            Label = StringView.Read(&backend.Label)
            Type = backend.Type
            Count = int(backend.Count)
        }
type Queue internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = Queue(0n)
    member _.Submit(commands : array<CommandBuffer>) : unit =
        let commandsHandles = commands |> Array.map (fun a -> a.Handle)
        use commandsPtr = fixed (commandsHandles)
        let commandsLen = unativeint commands.Length
        let res = WebGPU.Raw.WebGPU.QueueSubmit(handle, commandsLen, commandsPtr)
        res
    member _.OnSubmittedWorkDoneF(callbackInfo : QueueWorkDoneCallbackInfo) : Future =
        callbackInfo.Pin(fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.QueueOnSubmittedWorkDoneF(handle, NativePtr.read _callbackInfoPtr)
            Future.Read(&res)
        )
    member _.OnSubmittedWorkDone2(callbackInfo : QueueWorkDoneCallbackInfo2) : Future =
        callbackInfo.Pin(fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.QueueOnSubmittedWorkDone2(handle, NativePtr.read _callbackInfoPtr)
            Future.Read(&res)
        )
    member _.WriteBuffer(buffer : Buffer, bufferOffset : uint64, data : nativeint, size : unativeint) : unit =
        let res = WebGPU.Raw.WebGPU.QueueWriteBuffer(handle, buffer.Handle, bufferOffset, data, size)
        res
    member _.WriteTexture(destination : ImageCopyTexture, data : nativeint, dataSize : unativeint, dataLayout : TextureDataLayout, writeSize : Extent3D) : unit =
        destination.Pin(fun _destinationPtr ->
            dataLayout.Pin(fun _dataLayoutPtr ->
                writeSize.Pin(fun _writeSizePtr ->
                    let res = WebGPU.Raw.WebGPU.QueueWriteTexture(handle, _destinationPtr, data, dataSize, _dataLayoutPtr, _writeSizePtr)
                    res
                )
            )
        )
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.QueueSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
type QueueDescriptor = 
    {
        Label : StringView
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.QueueDescriptor> -> 'r) : 'r = 
        this.Label.Pin(fun _labelPtr ->
            let mutable value =
                new WebGPU.Raw.QueueDescriptor(
                    NativePtr.read _labelPtr
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.QueueDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.QueueDescriptor>) = 
        {
            Label = StringView.Read(&backend.Label)
        }
type QueueWorkDoneCallback = delegate of status : QueueWorkDoneStatus * userdata : nativeint -> unit
type QueueWorkDoneCallback2 = delegate of status : QueueWorkDoneStatus -> unit
type QueueWorkDoneCallbackInfo = 
    {
        Mode : CallbackMode
        Callback : QueueWorkDoneCallback
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.QueueWorkDoneCallbackInfo> -> 'r) : 'r = 
        let _callbackDel = WebGPU.Raw.QueueWorkDoneCallback(fun status userdata ->
            let _status = status
            let _userdata = userdata
            this.Callback.Invoke(_status, _userdata)
        )
        let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
        let mutable value =
            new WebGPU.Raw.QueueWorkDoneCallbackInfo(
                this.Mode,
                _callbackPtr,
                _callbackUserData
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.QueueWorkDoneCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.QueueWorkDoneCallbackInfo>) = 
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (userdata, backend.Userdata)]
        }
type QueueWorkDoneCallbackInfo2 = 
    {
        Mode : CallbackMode
        Callback : QueueWorkDoneCallback2
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.QueueWorkDoneCallbackInfo2> -> 'r) : 'r = 
        let mutable _callbackGC = Unchecked.defaultof<GCHandle>
        let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.QueueWorkDoneCallback2>
        _callbackDel <- WebGPU.Raw.QueueWorkDoneCallback2(fun status ->
            _callbackGC.Free()
            let _status = status
            this.Callback.Invoke(_status)
        )
        _callbackGC <- GCHandle.Alloc(_callbackDel)
        let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
        let mutable value =
            new WebGPU.Raw.QueueWorkDoneCallbackInfo2(
                this.Mode,
                _callbackPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.QueueWorkDoneCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.QueueWorkDoneCallbackInfo2>) = 
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode)]
        }
type RenderBundle internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = RenderBundle(0n)
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.RenderBundleSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
type RenderBundleEncoder internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = RenderBundleEncoder(0n)
    member _.SetPipeline(pipeline : RenderPipeline) : unit =
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderSetPipeline(handle, pipeline.Handle)
        res
    member _.SetBindGroup(groupIndex : int, group : BindGroup, dynamicOffsets : array<uint32>) : unit =
        use dynamicOffsetsPtr = fixed (dynamicOffsets)
        let dynamicOffsetsLen = unativeint dynamicOffsets.Length
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderSetBindGroup(handle, uint32(groupIndex), group.Handle, dynamicOffsetsLen, dynamicOffsetsPtr)
        res
    member _.Draw(vertexCount : int, instanceCount : int, firstVertex : int, firstInstance : int) : unit =
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderDraw(handle, uint32(vertexCount), uint32(instanceCount), uint32(firstVertex), uint32(firstInstance))
        res
    member _.DrawIndexed(indexCount : int, instanceCount : int, firstIndex : int, baseVertex : int, firstInstance : int) : unit =
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderDrawIndexed(handle, uint32(indexCount), uint32(instanceCount), uint32(firstIndex), baseVertex, uint32(firstInstance))
        res
    member _.DrawIndirect(indirectBuffer : Buffer, indirectOffset : uint64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderDrawIndirect(handle, indirectBuffer.Handle, indirectOffset)
        res
    member _.DrawIndexedIndirect(indirectBuffer : Buffer, indirectOffset : uint64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderDrawIndexedIndirect(handle, indirectBuffer.Handle, indirectOffset)
        res
    member _.InsertDebugMarker(markerLabel : StringView) : unit =
        markerLabel.Pin(fun _markerLabelPtr ->
            let res = WebGPU.Raw.WebGPU.RenderBundleEncoderInsertDebugMarker(handle, NativePtr.read _markerLabelPtr)
            res
        )
    member _.PopDebugGroup() : unit =
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderPopDebugGroup(handle)
        res
    member _.PushDebugGroup(groupLabel : StringView) : unit =
        groupLabel.Pin(fun _groupLabelPtr ->
            let res = WebGPU.Raw.WebGPU.RenderBundleEncoderPushDebugGroup(handle, NativePtr.read _groupLabelPtr)
            res
        )
    member _.SetVertexBuffer(slot : int, buffer : Buffer, offset : uint64, size : uint64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderSetVertexBuffer(handle, uint32(slot), buffer.Handle, offset, size)
        res
    member _.SetIndexBuffer(buffer : Buffer, format : IndexFormat, offset : uint64, size : uint64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderSetIndexBuffer(handle, buffer.Handle, format, offset, size)
        res
    member _.Finish(descriptor : RenderBundleDescriptor) : RenderBundle =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.RenderBundleEncoderFinish(handle, _descriptorPtr)
            new RenderBundle(res)
        )
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.RenderBundleEncoderSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
type RenderBundleDescriptor = 
    {
        Label : StringView
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderBundleDescriptor> -> 'r) : 'r = 
        this.Label.Pin(fun _labelPtr ->
            let mutable value =
                new WebGPU.Raw.RenderBundleDescriptor(
                    NativePtr.read _labelPtr
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderBundleDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderBundleDescriptor>) = 
        {
            Label = StringView.Read(&backend.Label)
        }
type RenderBundleEncoderDescriptor = 
    {
        Label : StringView
        ColorFormats : array<TextureFormat>
        DepthStencilFormat : TextureFormat
        SampleCount : int
        DepthReadOnly : bool
        StencilReadOnly : bool
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderBundleEncoderDescriptor> -> 'r) : 'r = 
        this.Label.Pin(fun _labelPtr ->
            use colorFormatsPtr = fixed (this.ColorFormats)
            let colorFormatsLen = unativeint this.ColorFormats.Length
            let mutable value =
                new WebGPU.Raw.RenderBundleEncoderDescriptor(
                    NativePtr.read _labelPtr,
                    colorFormatsLen,
                    colorFormatsPtr,
                    this.DepthStencilFormat,
                    uint32(this.SampleCount),
                    (if this.DepthReadOnly then 1 else 0),
                    (if this.StencilReadOnly then 1 else 0)
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderBundleEncoderDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderBundleEncoderDescriptor>) = 
        {
            Label = StringView.Read(&backend.Label)
            ColorFormats = let ptr = backend.ColorFormats in Array.init (int backend.ColorFormatCount) (fun i -> NativePtr.get ptr i)
            DepthStencilFormat = backend.DepthStencilFormat
            SampleCount = int(backend.SampleCount)
            DepthReadOnly = (backend.DepthReadOnly <> 0)
            StencilReadOnly = (backend.StencilReadOnly <> 0)
        }
type RenderPassColorAttachment = 
    {
        Next : IRenderPassColorAttachmentExtension
        View : TextureView
        DepthSlice : int
        ResolveTarget : TextureView
        LoadOp : LoadOp
        StoreOp : StoreOp
        ClearValue : Color
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderPassColorAttachment> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            this.ClearValue.Pin(fun _clearValuePtr ->
                let mutable value =
                    new WebGPU.Raw.RenderPassColorAttachment(
                        nextInChain,
                        this.View.Handle,
                        uint32(this.DepthSlice),
                        this.ResolveTarget.Handle,
                        this.LoadOp,
                        this.StoreOp,
                        NativePtr.read _clearValuePtr
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPassColorAttachment> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderPassColorAttachment>) = 
        {
            Next = null
            View = new TextureView(backend.View)
            DepthSlice = int(backend.DepthSlice)
            ResolveTarget = new TextureView(backend.ResolveTarget)
            LoadOp = backend.LoadOp
            StoreOp = backend.StoreOp
            ClearValue = Color.Read(&backend.ClearValue)
        }
type RenderPassDepthStencilAttachment = 
    {
        View : TextureView
        DepthLoadOp : LoadOp
        DepthStoreOp : StoreOp
        DepthClearValue : float32
        DepthReadOnly : bool
        StencilLoadOp : LoadOp
        StencilStoreOp : StoreOp
        StencilClearValue : int
        StencilReadOnly : bool
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderPassDepthStencilAttachment> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.RenderPassDepthStencilAttachment(
                this.View.Handle,
                this.DepthLoadOp,
                this.DepthStoreOp,
                this.DepthClearValue,
                (if this.DepthReadOnly then 1 else 0),
                this.StencilLoadOp,
                this.StencilStoreOp,
                uint32(this.StencilClearValue),
                (if this.StencilReadOnly then 1 else 0)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPassDepthStencilAttachment> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderPassDepthStencilAttachment>) = 
        {
            View = new TextureView(backend.View)
            DepthLoadOp = backend.DepthLoadOp
            DepthStoreOp = backend.DepthStoreOp
            DepthClearValue = backend.DepthClearValue
            DepthReadOnly = (backend.DepthReadOnly <> 0)
            StencilLoadOp = backend.StencilLoadOp
            StencilStoreOp = backend.StencilStoreOp
            StencilClearValue = int(backend.StencilClearValue)
            StencilReadOnly = (backend.StencilReadOnly <> 0)
        }
type RenderPassDescriptor = 
    {
        Next : IRenderPassDescriptorExtension
        Label : StringView
        ColorAttachments : array<RenderPassColorAttachment>
        DepthStencilAttachment : RenderPassDepthStencilAttachment
        OcclusionQuerySet : QuerySet
        TimestampWrites : RenderPassTimestampWrites
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderPassDescriptor> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            this.Label.Pin(fun _labelPtr ->
                WebGPU.Raw.Pinnable.pinArray this.ColorAttachments (fun colorAttachmentsPtr ->
                    let colorAttachmentsLen = unativeint this.ColorAttachments.Length
                    this.DepthStencilAttachment.Pin(fun _depthStencilAttachmentPtr ->
                        this.TimestampWrites.Pin(fun _timestampWritesPtr ->
                            let mutable value =
                                new WebGPU.Raw.RenderPassDescriptor(
                                    nextInChain,
                                    NativePtr.read _labelPtr,
                                    colorAttachmentsLen,
                                    colorAttachmentsPtr,
                                    _depthStencilAttachmentPtr,
                                    this.OcclusionQuerySet.Handle,
                                    _timestampWritesPtr
                                )
                            use ptr = fixed &value
                            action ptr
                        )
                    )
                )
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPassDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderPassDescriptor>) = 
        {
            Next = null
            Label = StringView.Read(&backend.Label)
            ColorAttachments = let ptr = backend.ColorAttachments in Array.init (int backend.ColorAttachmentCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in RenderPassColorAttachment.Read(&r))
            DepthStencilAttachment = let m = NativePtr.toByRef backend.DepthStencilAttachment in RenderPassDepthStencilAttachment.Read(&m)
            OcclusionQuerySet = new QuerySet(backend.OcclusionQuerySet)
            TimestampWrites = let m = NativePtr.toByRef backend.TimestampWrites in RenderPassTimestampWrites.Read(&m)
        }
type RenderPassDescriptorMaxDrawCount = RenderPassMaxDrawCount
type RenderPassMaxDrawCount = 
    {
        Next : IRenderPassDescriptorExtension
        MaxDrawCount : uint64
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderPassMaxDrawCount> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            let sType = SType.RenderPassMaxDrawCount
            let mutable value =
                new WebGPU.Raw.RenderPassMaxDrawCount(
                    nextInChain,
                    sType,
                    this.MaxDrawCount
                )
            use ptr = fixed &value
            action ptr
        )
    interface IRenderPassDescriptorExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPassMaxDrawCount> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderPassMaxDrawCount>) = 
        {
            Next = null
            MaxDrawCount = backend.MaxDrawCount
        }
type RenderPassEncoder internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = RenderPassEncoder(0n)
    member _.SetPipeline(pipeline : RenderPipeline) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetPipeline(handle, pipeline.Handle)
        res
    member _.SetBindGroup(groupIndex : int, group : BindGroup, dynamicOffsets : array<uint32>) : unit =
        use dynamicOffsetsPtr = fixed (dynamicOffsets)
        let dynamicOffsetsLen = unativeint dynamicOffsets.Length
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetBindGroup(handle, uint32(groupIndex), group.Handle, dynamicOffsetsLen, dynamicOffsetsPtr)
        res
    member _.Draw(vertexCount : int, instanceCount : int, firstVertex : int, firstInstance : int) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderDraw(handle, uint32(vertexCount), uint32(instanceCount), uint32(firstVertex), uint32(firstInstance))
        res
    member _.DrawIndexed(indexCount : int, instanceCount : int, firstIndex : int, baseVertex : int, firstInstance : int) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderDrawIndexed(handle, uint32(indexCount), uint32(instanceCount), uint32(firstIndex), baseVertex, uint32(firstInstance))
        res
    member _.DrawIndirect(indirectBuffer : Buffer, indirectOffset : uint64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderDrawIndirect(handle, indirectBuffer.Handle, indirectOffset)
        res
    member _.DrawIndexedIndirect(indirectBuffer : Buffer, indirectOffset : uint64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderDrawIndexedIndirect(handle, indirectBuffer.Handle, indirectOffset)
        res
    member _.MultiDrawIndirect(indirectBuffer : Buffer, indirectOffset : uint64, maxDrawCount : int, drawCountBuffer : Buffer, drawCountBufferOffset : uint64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderMultiDrawIndirect(handle, indirectBuffer.Handle, indirectOffset, uint32(maxDrawCount), drawCountBuffer.Handle, drawCountBufferOffset)
        res
    member _.MultiDrawIndexedIndirect(indirectBuffer : Buffer, indirectOffset : uint64, maxDrawCount : int, drawCountBuffer : Buffer, drawCountBufferOffset : uint64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderMultiDrawIndexedIndirect(handle, indirectBuffer.Handle, indirectOffset, uint32(maxDrawCount), drawCountBuffer.Handle, drawCountBufferOffset)
        res
    member _.ExecuteBundles(bundles : array<RenderBundle>) : unit =
        let bundlesHandles = bundles |> Array.map (fun a -> a.Handle)
        use bundlesPtr = fixed (bundlesHandles)
        let bundlesLen = unativeint bundles.Length
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderExecuteBundles(handle, bundlesLen, bundlesPtr)
        res
    member _.InsertDebugMarker(markerLabel : StringView) : unit =
        markerLabel.Pin(fun _markerLabelPtr ->
            let res = WebGPU.Raw.WebGPU.RenderPassEncoderInsertDebugMarker(handle, NativePtr.read _markerLabelPtr)
            res
        )
    member _.PopDebugGroup() : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderPopDebugGroup(handle)
        res
    member _.PushDebugGroup(groupLabel : StringView) : unit =
        groupLabel.Pin(fun _groupLabelPtr ->
            let res = WebGPU.Raw.WebGPU.RenderPassEncoderPushDebugGroup(handle, NativePtr.read _groupLabelPtr)
            res
        )
    member _.SetStencilReference(reference : int) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetStencilReference(handle, uint32(reference))
        res
    member _.SetBlendConstant(color : Color) : unit =
        color.Pin(fun _colorPtr ->
            let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetBlendConstant(handle, _colorPtr)
            res
        )
    member _.SetViewport(x : float32, y : float32, width : float32, height : float32, minDepth : float32, maxDepth : float32) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetViewport(handle, x, y, width, height, minDepth, maxDepth)
        res
    member _.SetScissorRect(x : int, y : int, width : int, height : int) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetScissorRect(handle, uint32(x), uint32(y), uint32(width), uint32(height))
        res
    member _.SetVertexBuffer(slot : int, buffer : Buffer, offset : uint64, size : uint64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetVertexBuffer(handle, uint32(slot), buffer.Handle, offset, size)
        res
    member _.SetIndexBuffer(buffer : Buffer, format : IndexFormat, offset : uint64, size : uint64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetIndexBuffer(handle, buffer.Handle, format, offset, size)
        res
    member _.BeginOcclusionQuery(queryIndex : int) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderBeginOcclusionQuery(handle, uint32(queryIndex))
        res
    member _.EndOcclusionQuery() : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderEndOcclusionQuery(handle)
        res
    member _.WriteTimestamp(querySet : QuerySet, queryIndex : int) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderWriteTimestamp(handle, querySet.Handle, uint32(queryIndex))
        res
    member _.End() : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderEnd(handle)
        res
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
type RenderPassTimestampWrites = 
    {
        QuerySet : QuerySet
        BeginningOfPassWriteIndex : int
        EndOfPassWriteIndex : int
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderPassTimestampWrites> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.RenderPassTimestampWrites(
                this.QuerySet.Handle,
                uint32(this.BeginningOfPassWriteIndex),
                uint32(this.EndOfPassWriteIndex)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPassTimestampWrites> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderPassTimestampWrites>) = 
        {
            QuerySet = new QuerySet(backend.QuerySet)
            BeginningOfPassWriteIndex = int(backend.BeginningOfPassWriteIndex)
            EndOfPassWriteIndex = int(backend.EndOfPassWriteIndex)
        }
type RenderPipeline internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = RenderPipeline(0n)
    member _.GetBindGroupLayout(groupIndex : int) : BindGroupLayout =
        let res = WebGPU.Raw.WebGPU.RenderPipelineGetBindGroupLayout(handle, uint32(groupIndex))
        new BindGroupLayout(res)
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.RenderPipelineSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
type RequestDeviceCallback = delegate of status : RequestDeviceStatus * device : Device * message : StringView * userdata : nativeint -> unit
type RequestDeviceCallback2 = delegate of status : RequestDeviceStatus * device : Device * message : StringView -> unit
type RequestDeviceCallbackInfo = 
    {
        Mode : CallbackMode
        Callback : RequestDeviceCallback
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RequestDeviceCallbackInfo> -> 'r) : 'r = 
        let _callbackDel = WebGPU.Raw.RequestDeviceCallback(fun status device message userdata ->
            let _status = status
            let _device = new Device(device)
            let _message = StringView.Read(&message)
            let _userdata = userdata
            this.Callback.Invoke(_status, _device, _message, _userdata)
        )
        let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
        let mutable value =
            new WebGPU.Raw.RequestDeviceCallbackInfo(
                this.Mode,
                _callbackPtr,
                _callbackUserData
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RequestDeviceCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RequestDeviceCallbackInfo>) = 
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (userdata, backend.Userdata)]
        }
type RequestDeviceCallbackInfo2 = 
    {
        Mode : CallbackMode
        Callback : RequestDeviceCallback2
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RequestDeviceCallbackInfo2> -> 'r) : 'r = 
        let mutable _callbackGC = Unchecked.defaultof<GCHandle>
        let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.RequestDeviceCallback2>
        _callbackDel <- WebGPU.Raw.RequestDeviceCallback2(fun status device message ->
            _callbackGC.Free()
            let _status = status
            let _device = new Device(device)
            let _message = StringView.Read(&message)
            this.Callback.Invoke(_status, _device, _message)
        )
        _callbackGC <- GCHandle.Alloc(_callbackDel)
        let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
        let mutable value =
            new WebGPU.Raw.RequestDeviceCallbackInfo2(
                this.Mode,
                _callbackPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RequestDeviceCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RequestDeviceCallbackInfo2>) = 
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode)]
        }
type VertexState = 
    {
        Module : ShaderModule
        EntryPoint : StringView
        Constants : array<ConstantEntry>
        Buffers : array<VertexBufferLayout>
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.VertexState> -> 'r) : 'r = 
        this.EntryPoint.Pin(fun _entryPointPtr ->
            WebGPU.Raw.Pinnable.pinArray this.Constants (fun constantsPtr ->
                let constantsLen = unativeint this.Constants.Length
                WebGPU.Raw.Pinnable.pinArray this.Buffers (fun buffersPtr ->
                    let buffersLen = unativeint this.Buffers.Length
                    let mutable value =
                        new WebGPU.Raw.VertexState(
                            this.Module.Handle,
                            NativePtr.read _entryPointPtr,
                            constantsLen,
                            constantsPtr,
                            buffersLen,
                            buffersPtr
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.VertexState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.VertexState>) = 
        {
            Module = new ShaderModule(backend.Module)
            EntryPoint = StringView.Read(&backend.EntryPoint)
            Constants = let ptr = backend.Constants in Array.init (int backend.ConstantCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in ConstantEntry.Read(&r))
            Buffers = let ptr = backend.Buffers in Array.init (int backend.BufferCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in VertexBufferLayout.Read(&r))
        }
type PrimitiveState = 
    {
        Topology : PrimitiveTopology
        StripIndexFormat : IndexFormat
        FrontFace : FrontFace
        CullMode : CullMode
        UnclippedDepth : bool
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.PrimitiveState> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.PrimitiveState(
                this.Topology,
                this.StripIndexFormat,
                this.FrontFace,
                this.CullMode,
                (if this.UnclippedDepth then 1 else 0)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.PrimitiveState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.PrimitiveState>) = 
        {
            Topology = backend.Topology
            StripIndexFormat = backend.StripIndexFormat
            FrontFace = backend.FrontFace
            CullMode = backend.CullMode
            UnclippedDepth = (backend.UnclippedDepth <> 0)
        }
type DepthStencilState = 
    {
        Format : TextureFormat
        DepthWriteEnabled : OptionalBool
        DepthCompare : CompareFunction
        StencilFront : StencilFaceState
        StencilBack : StencilFaceState
        StencilReadMask : int
        StencilWriteMask : int
        DepthBias : int
        DepthBiasSlopeScale : float32
        DepthBiasClamp : float32
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DepthStencilState> -> 'r) : 'r = 
        this.StencilFront.Pin(fun _stencilFrontPtr ->
            this.StencilBack.Pin(fun _stencilBackPtr ->
                let mutable value =
                    new WebGPU.Raw.DepthStencilState(
                        this.Format,
                        this.DepthWriteEnabled,
                        this.DepthCompare,
                        NativePtr.read _stencilFrontPtr,
                        NativePtr.read _stencilBackPtr,
                        uint32(this.StencilReadMask),
                        uint32(this.StencilWriteMask),
                        this.DepthBias,
                        this.DepthBiasSlopeScale,
                        this.DepthBiasClamp
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DepthStencilState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DepthStencilState>) = 
        {
            Format = backend.Format
            DepthWriteEnabled = backend.DepthWriteEnabled
            DepthCompare = backend.DepthCompare
            StencilFront = StencilFaceState.Read(&backend.StencilFront)
            StencilBack = StencilFaceState.Read(&backend.StencilBack)
            StencilReadMask = int(backend.StencilReadMask)
            StencilWriteMask = int(backend.StencilWriteMask)
            DepthBias = backend.DepthBias
            DepthBiasSlopeScale = backend.DepthBiasSlopeScale
            DepthBiasClamp = backend.DepthBiasClamp
        }
type MultisampleState = 
    {
        Count : int
        Mask : int
        AlphaToCoverageEnabled : bool
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.MultisampleState> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.MultisampleState(
                uint32(this.Count),
                uint32(this.Mask),
                (if this.AlphaToCoverageEnabled then 1 else 0)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.MultisampleState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.MultisampleState>) = 
        {
            Count = int(backend.Count)
            Mask = int(backend.Mask)
            AlphaToCoverageEnabled = (backend.AlphaToCoverageEnabled <> 0)
        }
type FragmentState = 
    {
        Module : ShaderModule
        EntryPoint : StringView
        Constants : array<ConstantEntry>
        Targets : array<ColorTargetState>
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.FragmentState> -> 'r) : 'r = 
        this.EntryPoint.Pin(fun _entryPointPtr ->
            WebGPU.Raw.Pinnable.pinArray this.Constants (fun constantsPtr ->
                let constantsLen = unativeint this.Constants.Length
                WebGPU.Raw.Pinnable.pinArray this.Targets (fun targetsPtr ->
                    let targetsLen = unativeint this.Targets.Length
                    let mutable value =
                        new WebGPU.Raw.FragmentState(
                            this.Module.Handle,
                            NativePtr.read _entryPointPtr,
                            constantsLen,
                            constantsPtr,
                            targetsLen,
                            targetsPtr
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.FragmentState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.FragmentState>) = 
        {
            Module = new ShaderModule(backend.Module)
            EntryPoint = StringView.Read(&backend.EntryPoint)
            Constants = let ptr = backend.Constants in Array.init (int backend.ConstantCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in ConstantEntry.Read(&r))
            Targets = let ptr = backend.Targets in Array.init (int backend.TargetCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in ColorTargetState.Read(&r))
        }
type ColorTargetState = 
    {
        Next : IColorTargetStateExtension
        Format : TextureFormat
        Blend : BlendState
        WriteMask : ColorWriteMask
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ColorTargetState> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            this.Blend.Pin(fun _blendPtr ->
                let mutable value =
                    new WebGPU.Raw.ColorTargetState(
                        nextInChain,
                        this.Format,
                        _blendPtr,
                        this.WriteMask
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ColorTargetState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ColorTargetState>) = 
        {
            Next = null
            Format = backend.Format
            Blend = let m = NativePtr.toByRef backend.Blend in BlendState.Read(&m)
            WriteMask = backend.WriteMask
        }
type BlendState = 
    {
        Color : BlendComponent
        Alpha : BlendComponent
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BlendState> -> 'r) : 'r = 
        this.Color.Pin(fun _colorPtr ->
            this.Alpha.Pin(fun _alphaPtr ->
                let mutable value =
                    new WebGPU.Raw.BlendState(
                        NativePtr.read _colorPtr,
                        NativePtr.read _alphaPtr
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BlendState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BlendState>) = 
        {
            Color = BlendComponent.Read(&backend.Color)
            Alpha = BlendComponent.Read(&backend.Alpha)
        }
type RenderPipelineDescriptor = 
    {
        Label : StringView
        Layout : PipelineLayout
        Vertex : VertexState
        Primitive : PrimitiveState
        DepthStencil : DepthStencilState
        Multisample : MultisampleState
        Fragment : FragmentState
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderPipelineDescriptor> -> 'r) : 'r = 
        this.Label.Pin(fun _labelPtr ->
            this.Vertex.Pin(fun _vertexPtr ->
                this.Primitive.Pin(fun _primitivePtr ->
                    this.DepthStencil.Pin(fun _depthStencilPtr ->
                        this.Multisample.Pin(fun _multisamplePtr ->
                            this.Fragment.Pin(fun _fragmentPtr ->
                                let mutable value =
                                    new WebGPU.Raw.RenderPipelineDescriptor(
                                        NativePtr.read _labelPtr,
                                        this.Layout.Handle,
                                        NativePtr.read _vertexPtr,
                                        NativePtr.read _primitivePtr,
                                        _depthStencilPtr,
                                        NativePtr.read _multisamplePtr,
                                        _fragmentPtr
                                    )
                                use ptr = fixed &value
                                action ptr
                            )
                        )
                    )
                )
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPipelineDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderPipelineDescriptor>) = 
        {
            Label = StringView.Read(&backend.Label)
            Layout = new PipelineLayout(backend.Layout)
            Vertex = VertexState.Read(&backend.Vertex)
            Primitive = PrimitiveState.Read(&backend.Primitive)
            DepthStencil = let m = NativePtr.toByRef backend.DepthStencil in DepthStencilState.Read(&m)
            Multisample = MultisampleState.Read(&backend.Multisample)
            Fragment = let m = NativePtr.toByRef backend.Fragment in FragmentState.Read(&m)
        }
type Sampler internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = Sampler(0n)
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.SamplerSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
type SamplerDescriptor = 
    {
        Next : ISamplerDescriptorExtension
        Label : StringView
        AddressModeU : AddressMode
        AddressModeV : AddressMode
        AddressModeW : AddressMode
        MagFilter : FilterMode
        MinFilter : FilterMode
        MipmapFilter : MipmapFilterMode
        LodMinClamp : float32
        LodMaxClamp : float32
        Compare : CompareFunction
        MaxAnisotropy : uint16
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SamplerDescriptor> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            this.Label.Pin(fun _labelPtr ->
                let mutable value =
                    new WebGPU.Raw.SamplerDescriptor(
                        nextInChain,
                        NativePtr.read _labelPtr,
                        this.AddressModeU,
                        this.AddressModeV,
                        this.AddressModeW,
                        this.MagFilter,
                        this.MinFilter,
                        this.MipmapFilter,
                        this.LodMinClamp,
                        this.LodMaxClamp,
                        this.Compare,
                        this.MaxAnisotropy
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SamplerDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SamplerDescriptor>) = 
        {
            Next = null
            Label = StringView.Read(&backend.Label)
            AddressModeU = backend.AddressModeU
            AddressModeV = backend.AddressModeV
            AddressModeW = backend.AddressModeW
            MagFilter = backend.MagFilter
            MinFilter = backend.MinFilter
            MipmapFilter = backend.MipmapFilter
            LodMinClamp = backend.LodMinClamp
            LodMaxClamp = backend.LodMaxClamp
            Compare = backend.Compare
            MaxAnisotropy = backend.MaxAnisotropy
        }
type ShaderModule internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = ShaderModule(0n)
    member _.GetCompilationInfo(callback : CompilationInfoCallback) : unit =
        let _callbackDel = WebGPU.Raw.CompilationInfoCallback(fun status compilationInfo userdata ->
            let _status = status
            let _compilationInfo = let m = NativePtr.toByRef compilationInfo in CompilationInfo.Read(&m)
            let _userdata = userdata
            callback.Invoke(_status, _compilationInfo, _userdata)
        )
        let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
        let res = WebGPU.Raw.WebGPU.ShaderModuleGetCompilationInfo(handle, _callbackPtr, _callbackUserData)
        res
    member _.GetCompilationInfoF(callbackInfo : CompilationInfoCallbackInfo) : Future =
        callbackInfo.Pin(fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.ShaderModuleGetCompilationInfoF(handle, NativePtr.read _callbackInfoPtr)
            Future.Read(&res)
        )
    member _.GetCompilationInfo2(callbackInfo : CompilationInfoCallbackInfo2) : Future =
        callbackInfo.Pin(fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.ShaderModuleGetCompilationInfo2(handle, NativePtr.read _callbackInfoPtr)
            Future.Read(&res)
        )
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.ShaderModuleSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
type ShaderModuleDescriptor = 
    {
        Next : IShaderModuleDescriptorExtension
        Label : StringView
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ShaderModuleDescriptor> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            this.Label.Pin(fun _labelPtr ->
                let mutable value =
                    new WebGPU.Raw.ShaderModuleDescriptor(
                        nextInChain,
                        NativePtr.read _labelPtr
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ShaderModuleDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ShaderModuleDescriptor>) = 
        {
            Next = null
            Label = StringView.Read(&backend.Label)
        }
type ShaderModuleSPIRVDescriptor = ShaderSourceSPIRV
type ShaderSourceSPIRV = 
    {
        Next : IShaderModuleDescriptorExtension
        Code : array<uint32>
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ShaderSourceSPIRV> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            let sType = SType.ShaderSourceSPIRV
            use codePtr = fixed (this.Code)
            let codeLen = uint32 this.Code.Length
            let mutable value =
                new WebGPU.Raw.ShaderSourceSPIRV(
                    nextInChain,
                    sType,
                    codeLen,
                    codePtr
                )
            use ptr = fixed &value
            action ptr
        )
    interface IShaderModuleDescriptorExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ShaderSourceSPIRV> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ShaderSourceSPIRV>) = 
        {
            Next = null
            Code = let ptr = backend.Code in Array.init (int backend.CodeSize) (fun i -> NativePtr.get ptr i)
        }
type ShaderModuleWGSLDescriptor = ShaderSourceWGSL
type ShaderSourceWGSL = 
    {
        Next : IShaderModuleDescriptorExtension
        Code : StringView
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ShaderSourceWGSL> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            let sType = SType.ShaderSourceWGSL
            this.Code.Pin(fun _codePtr ->
                let mutable value =
                    new WebGPU.Raw.ShaderSourceWGSL(
                        nextInChain,
                        sType,
                        NativePtr.read _codePtr
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface IShaderModuleDescriptorExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ShaderSourceWGSL> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ShaderSourceWGSL>) = 
        {
            Next = null
            Code = StringView.Read(&backend.Code)
        }
type StencilFaceState = 
    {
        Compare : CompareFunction
        FailOp : StencilOperation
        DepthFailOp : StencilOperation
        PassOp : StencilOperation
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.StencilFaceState> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.StencilFaceState(
                this.Compare,
                this.FailOp,
                this.DepthFailOp,
                this.PassOp
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.StencilFaceState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.StencilFaceState>) = 
        {
            Compare = backend.Compare
            FailOp = backend.FailOp
            DepthFailOp = backend.DepthFailOp
            PassOp = backend.PassOp
        }
type Surface internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = Surface(0n)
    member _.Configure(config : SurfaceConfiguration) : unit =
        config.Pin(fun _configPtr ->
            let res = WebGPU.Raw.WebGPU.SurfaceConfigure(handle, _configPtr)
            res
        )
    member _.GetCapabilities(adapter : Adapter, capabilities : byref<SurfaceCapabilities>) : Status =
        capabilities.Pin(fun _capabilitiesPtr ->
            let res = WebGPU.Raw.WebGPU.SurfaceGetCapabilities(handle, adapter.Handle, _capabilitiesPtr)
            res
        )
    member _.CurrentTexture : SurfaceTexture =
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        WebGPU.Raw.WebGPU.SurfaceGetCurrentTexture(handle, ptr)
        SurfaceTexture.Read(&res)
    member _.Present() : unit =
        let res = WebGPU.Raw.WebGPU.SurfacePresent(handle)
        res
    member _.Unconfigure() : unit =
        let res = WebGPU.Raw.WebGPU.SurfaceUnconfigure(handle)
        res
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.SurfaceSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
type SurfaceDescriptor = 
    {
        Next : ISurfaceDescriptorExtension
        Label : StringView
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SurfaceDescriptor> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            this.Label.Pin(fun _labelPtr ->
                let mutable value =
                    new WebGPU.Raw.SurfaceDescriptor(
                        nextInChain,
                        NativePtr.read _labelPtr
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SurfaceDescriptor>) = 
        {
            Next = null
            Label = StringView.Read(&backend.Label)
        }
type SurfaceDescriptorFromAndroidNativeWindow = SurfaceSourceAndroidNativeWindow
type SurfaceDescriptorFromCanvasHTMLSelector = SurfaceSourceCanvasHTMLSelector_Emscripten
type SurfaceSourceCanvasHTMLSelector_Emscripten = 
    {
        Next : ISurfaceDescriptorExtension
        Selector : StringView
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SurfaceSourceCanvasHTMLSelector_Emscripten> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            let sType = SType.SurfaceSourceCanvasHTMLSelector_Emscripten
            this.Selector.Pin(fun _selectorPtr ->
                let mutable value =
                    new WebGPU.Raw.SurfaceSourceCanvasHTMLSelector_Emscripten(
                        nextInChain,
                        sType,
                        NativePtr.read _selectorPtr
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface ISurfaceDescriptorExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceSourceCanvasHTMLSelector_Emscripten> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SurfaceSourceCanvasHTMLSelector_Emscripten>) = 
        {
            Next = null
            Selector = StringView.Read(&backend.Selector)
        }
type SurfaceDescriptorFromMetalLayer = SurfaceSourceMetalLayer
type SurfaceDescriptorFromWindowsHWND = SurfaceSourceWindowsHWND
type SurfaceDescriptorFromXcbWindow = SurfaceSourceXCBWindow
type SurfaceDescriptorFromXlibWindow = SurfaceSourceXlibWindow
type SurfaceDescriptorFromWaylandSurface = SurfaceSourceWaylandSurface
type SurfaceTexture = 
    {
        Texture : Texture
        Suboptimal : bool
        Status : SurfaceGetCurrentTextureStatus
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SurfaceTexture> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.SurfaceTexture(
                this.Texture.Handle,
                (if this.Suboptimal then 1 else 0),
                this.Status
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceTexture> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SurfaceTexture>) = 
        {
            Texture = new Texture(backend.Texture)
            Suboptimal = (backend.Suboptimal <> 0)
            Status = backend.Status
        }
type Texture internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = Texture(0n)
    member _.CreateView(descriptor : TextureViewDescriptor) : TextureView =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.TextureCreateView(handle, _descriptorPtr)
            new TextureView(res)
        )
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.TextureSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
    member _.GetWidth() : int =
        let res = WebGPU.Raw.WebGPU.TextureGetWidth(handle)
        int(res)
    member _.GetHeight() : int =
        let res = WebGPU.Raw.WebGPU.TextureGetHeight(handle)
        int(res)
    member _.GetDepthOrArrayLayers() : int =
        let res = WebGPU.Raw.WebGPU.TextureGetDepthOrArrayLayers(handle)
        int(res)
    member _.GetMipLevelCount() : int =
        let res = WebGPU.Raw.WebGPU.TextureGetMipLevelCount(handle)
        int(res)
    member _.GetSampleCount() : int =
        let res = WebGPU.Raw.WebGPU.TextureGetSampleCount(handle)
        int(res)
    member _.GetDimension() : TextureDimension =
        let res = WebGPU.Raw.WebGPU.TextureGetDimension(handle)
        res
    member _.GetFormat() : TextureFormat =
        let res = WebGPU.Raw.WebGPU.TextureGetFormat(handle)
        res
    member _.GetUsage() : TextureUsage =
        let res = WebGPU.Raw.WebGPU.TextureGetUsage(handle)
        res
    member _.Destroy() : unit =
        let res = WebGPU.Raw.WebGPU.TextureDestroy(handle)
        res
    member x.Dispose() = x.Destroy()
    interface System.IDisposable with
        member x.Dispose() = x.Dispose()
type TextureDataLayout = 
    {
        Offset : uint64
        BytesPerRow : int
        RowsPerImage : int
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.TextureDataLayout> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.TextureDataLayout(
                this.Offset,
                uint32(this.BytesPerRow),
                uint32(this.RowsPerImage)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.TextureDataLayout> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.TextureDataLayout>) = 
        {
            Offset = backend.Offset
            BytesPerRow = int(backend.BytesPerRow)
            RowsPerImage = int(backend.RowsPerImage)
        }
type TextureDescriptor = 
    {
        Next : ITextureDescriptorExtension
        Label : StringView
        Usage : TextureUsage
        Dimension : TextureDimension
        Size : Extent3D
        Format : TextureFormat
        MipLevelCount : int
        SampleCount : int
        ViewFormats : array<TextureFormat>
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.TextureDescriptor> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            this.Label.Pin(fun _labelPtr ->
                this.Size.Pin(fun _sizePtr ->
                    use viewFormatsPtr = fixed (this.ViewFormats)
                    let viewFormatsLen = unativeint this.ViewFormats.Length
                    let mutable value =
                        new WebGPU.Raw.TextureDescriptor(
                            nextInChain,
                            NativePtr.read _labelPtr,
                            this.Usage,
                            this.Dimension,
                            NativePtr.read _sizePtr,
                            this.Format,
                            uint32(this.MipLevelCount),
                            uint32(this.SampleCount),
                            viewFormatsLen,
                            viewFormatsPtr
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.TextureDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.TextureDescriptor>) = 
        {
            Next = null
            Label = StringView.Read(&backend.Label)
            Usage = backend.Usage
            Dimension = backend.Dimension
            Size = Extent3D.Read(&backend.Size)
            Format = backend.Format
            MipLevelCount = int(backend.MipLevelCount)
            SampleCount = int(backend.SampleCount)
            ViewFormats = let ptr = backend.ViewFormats in Array.init (int backend.ViewFormatCount) (fun i -> NativePtr.get ptr i)
        }
type TextureBindingViewDimensionDescriptor = 
    {
        Next : ITextureDescriptorExtension
        TextureBindingViewDimension : TextureViewDimension
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.TextureBindingViewDimensionDescriptor> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            let sType = SType.TextureBindingViewDimensionDescriptor
            let mutable value =
                new WebGPU.Raw.TextureBindingViewDimensionDescriptor(
                    nextInChain,
                    sType,
                    this.TextureBindingViewDimension
                )
            use ptr = fixed &value
            action ptr
        )
    interface ITextureDescriptorExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.TextureBindingViewDimensionDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.TextureBindingViewDimensionDescriptor>) = 
        {
            Next = null
            TextureBindingViewDimension = backend.TextureBindingViewDimension
        }
type TextureViewDescriptor = 
    {
        Next : ITextureViewDescriptorExtension
        Label : StringView
        Format : TextureFormat
        Dimension : TextureViewDimension
        BaseMipLevel : int
        MipLevelCount : int
        BaseArrayLayer : int
        ArrayLayerCount : int
        Aspect : TextureAspect
        Usage : TextureUsage
    }
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.TextureViewDescriptor> -> 'r) : 'r = 
        PinHelper.PinNullable(this.Next, fun nextInChain ->
            this.Label.Pin(fun _labelPtr ->
                let mutable value =
                    new WebGPU.Raw.TextureViewDescriptor(
                        nextInChain,
                        NativePtr.read _labelPtr,
                        this.Format,
                        this.Dimension,
                        uint32(this.BaseMipLevel),
                        uint32(this.MipLevelCount),
                        uint32(this.BaseArrayLayer),
                        uint32(this.ArrayLayerCount),
                        this.Aspect,
                        this.Usage
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.TextureViewDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.TextureViewDescriptor>) = 
        {
            Next = null
            Label = StringView.Read(&backend.Label)
            Format = backend.Format
            Dimension = backend.Dimension
            BaseMipLevel = int(backend.BaseMipLevel)
            MipLevelCount = int(backend.MipLevelCount)
            BaseArrayLayer = int(backend.BaseArrayLayer)
            ArrayLayerCount = int(backend.ArrayLayerCount)
            Aspect = backend.Aspect
            Usage = backend.Usage
        }
type TextureView internal(handle : nativeint) =
    member x.Handle = handle
    static member Null = TextureView(0n)
    member _.SetLabel(label : StringView) : unit =
        label.Pin(fun _labelPtr ->
            let res = WebGPU.Raw.WebGPU.TextureViewSetLabel(handle, NativePtr.read _labelPtr)
            res
        )
