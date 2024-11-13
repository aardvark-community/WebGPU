namespace rec WebGPU
open System
open System.Text
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop
#nowarn "9"
type Proc = delegate of unit -> unit
type RequestAdapterOptions = 
    {
        CompatibleSurface : Surface
        PowerPreference : PowerPreference
        ForceFallbackAdapter : bool
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.RequestAdapterOptions> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.RequestAdapterOptions(
                this.CompatibleSurface.Handle,
                this.PowerPreference,
                (if this.ForceFallbackAdapter then 1 else 0)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RequestAdapterOptions> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.RequestAdapterOptions>) = 
        {
            CompatibleSurface = new Surface(backend.CompatibleSurface)
            PowerPreference = backend.PowerPreference
            ForceFallbackAdapter = (backend.ForceFallbackAdapter <> 0)
        }
type RequestAdapterCallback = delegate of status : RequestAdapterStatus * adapter : Adapter * message : string * userdata : nativeint -> unit
type Adapter internal(handle : nativeint) =
    member x.Handle = handle
    member _.Limits : SupportedLimits =
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        let status = WebGPU.Raw.WebGPU.AdapterGetLimits(handle, ptr)
        if status = 0 then failwith "GetLimits failed"
        SupportedLimits.Read(&res)
    member _.Properties : AdapterProperties =
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        WebGPU.Raw.WebGPU.AdapterGetProperties(handle, ptr)
        AdapterProperties.Read(&res)
    member _.HasFeature(feature : FeatureName) : bool =
        (WebGPU.Raw.WebGPU.AdapterHasFeature(handle, feature) <> 0)
    member _.EnumerateFeatures(features : nativeptr<FeatureName>) : unativeint =
        WebGPU.Raw.WebGPU.AdapterEnumerateFeatures(handle, features)
    member _.RequestDevice(descriptor : DeviceDescriptor, callback : RequestDeviceCallback) : unit =
        descriptor.Pin(fun _descriptorPtr ->
            let _callbackDel = WebGPU.Raw.RequestDeviceCallback(fun status device message userdata ->
                let _status = status
                let _device = new Device(device)
                let _message = Marshal.PtrToStringAnsi(NativePtr.toNativeInt message)
                let _userdata = userdata
                callback.Invoke(_status, _device, _message, _userdata)
            )
            let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
            WebGPU.Raw.WebGPU.AdapterRequestDevice(handle, _descriptorPtr, _callbackPtr, _callbackUserData)
        )
    member _.CreateDevice(descriptor : DeviceDescriptor) : Device =
        descriptor.Pin(fun _descriptorPtr ->
            new Device(WebGPU.Raw.WebGPU.AdapterCreateDevice(handle, _descriptorPtr))
        )
type AdapterProperties = 
    {
        VendorID : int
        VendorName : string
        Architecture : string
        DeviceID : int
        Name : string
        DriverDescription : string
        AdapterType : AdapterType
        BackendType : BackendType
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.AdapterProperties> -> 'r) : 'r = 
        use _vendorNamePtr = fixed (Encoding.UTF8.GetBytes(this.VendorName))
        use _architecturePtr = fixed (Encoding.UTF8.GetBytes(this.Architecture))
        use _namePtr = fixed (Encoding.UTF8.GetBytes(this.Name))
        use _driverDescriptionPtr = fixed (Encoding.UTF8.GetBytes(this.DriverDescription))
        let mutable value =
            new WebGPU.Raw.AdapterProperties(
                uint32(this.VendorID),
                _vendorNamePtr,
                _architecturePtr,
                uint32(this.DeviceID),
                _namePtr,
                _driverDescriptionPtr,
                this.AdapterType,
                this.BackendType
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.AdapterProperties> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.AdapterProperties>) = 
        {
            VendorID = int(backend.VendorID)
            VendorName = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.VendorName)
            Architecture = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Architecture)
            DeviceID = int(backend.DeviceID)
            Name = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Name)
            DriverDescription = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.DriverDescription)
            AdapterType = backend.AdapterType
            BackendType = backend.BackendType
        }
type DeviceDescriptor = 
    {
        Label : string
        RequiredFeatures : array<FeatureName>
        RequiredLimits : RequiredLimits
        DefaultQueue : QueueDescriptor
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.DeviceDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        use requiredFeaturesPtr = fixed (this.RequiredFeatures)
        let requiredFeaturesLen = uint32 this.RequiredFeatures.Length
        this.RequiredLimits.Pin(fun _requiredLimitsPtr ->
            this.DefaultQueue.Pin(fun _defaultQueuePtr ->
                let mutable value =
                    new WebGPU.Raw.DeviceDescriptor(
                        _labelPtr,
                        requiredFeaturesLen,
                        requiredFeaturesPtr,
                        _requiredLimitsPtr,
                        NativePtr.read _defaultQueuePtr
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DeviceDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.DeviceDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
            RequiredFeatures = let ptr = backend.RequiredFeatures in Array.init (int backend.RequiredFeaturesCount) (fun i -> NativePtr.get ptr i)
            RequiredLimits = let m = NativePtr.toByRef backend.RequiredLimits in RequiredLimits.Read(&m)
            DefaultQueue = QueueDescriptor.Read(&backend.DefaultQueue)
        }
type DawnTogglesDeviceDescriptor = 
    {
        ForceEnabledTogglesCount : int
        ForceEnabledToggles : nativeptr<nativeptr<byte>>
        ForceDisabledTogglesCount : int
        ForceDisabledToggles : nativeptr<nativeptr<byte>>
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.DawnTogglesDeviceDescriptor> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.DawnTogglesDeviceDescriptor(
                uint32(this.ForceEnabledTogglesCount),
                this.ForceEnabledToggles,
                uint32(this.ForceDisabledTogglesCount),
                this.ForceDisabledToggles
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnTogglesDeviceDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.DawnTogglesDeviceDescriptor>) = 
        {
            ForceEnabledTogglesCount = int(backend.ForceEnabledTogglesCount)
            ForceEnabledToggles = backend.ForceEnabledToggles
            ForceDisabledTogglesCount = int(backend.ForceDisabledTogglesCount)
            ForceDisabledToggles = backend.ForceDisabledToggles
        }
type DawnTogglesDescriptor = 
    {
        EnabledTogglesCount : int
        EnabledToggles : nativeptr<nativeptr<byte>>
        DisabledTogglesCount : int
        DisabledToggles : nativeptr<nativeptr<byte>>
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.DawnTogglesDescriptor> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.DawnTogglesDescriptor(
                uint32(this.EnabledTogglesCount),
                this.EnabledToggles,
                uint32(this.DisabledTogglesCount),
                this.DisabledToggles
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnTogglesDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.DawnTogglesDescriptor>) = 
        {
            EnabledTogglesCount = int(backend.EnabledTogglesCount)
            EnabledToggles = backend.EnabledToggles
            DisabledTogglesCount = int(backend.DisabledTogglesCount)
            DisabledToggles = backend.DisabledToggles
        }
type DawnCacheDeviceDescriptor = 
    {
        IsolationKey : string
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.DawnCacheDeviceDescriptor> -> 'r) : 'r = 
        use _isolationKeyPtr = fixed (Encoding.UTF8.GetBytes(this.IsolationKey))
        let mutable value =
            new WebGPU.Raw.DawnCacheDeviceDescriptor(
                _isolationKeyPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnCacheDeviceDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.DawnCacheDeviceDescriptor>) = 
        {
            IsolationKey = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.IsolationKey)
        }
type BindGroup internal(handle : nativeint) =
    member x.Handle = handle
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.BindGroupSetLabel(handle, _labelPtr)
type BindGroupEntry = 
    {
        Binding : int
        Buffer : Buffer
        Offset : uint64
        Size : uint64
        Sampler : Sampler
        TextureView : TextureView
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.BindGroupEntry> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.BindGroupEntry(
                uint32(this.Binding),
                this.Buffer.Handle,
                this.Offset,
                this.Size,
                this.Sampler.Handle,
                this.TextureView.Handle
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BindGroupEntry> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.BindGroupEntry>) = 
        {
            Binding = int(backend.Binding)
            Buffer = new Buffer(backend.Buffer)
            Offset = backend.Offset
            Size = backend.Size
            Sampler = new Sampler(backend.Sampler)
            TextureView = new TextureView(backend.TextureView)
        }
type BindGroupDescriptor = 
    {
        Label : string
        Layout : BindGroupLayout
        Entries : array<BindGroupEntry>
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.BindGroupDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        WebGPU.Raw.Pinnable.pinArray this.Entries (fun entriesPtr ->
            let entriesLen = uint32 this.Entries.Length
            let mutable value =
                new WebGPU.Raw.BindGroupDescriptor(
                    _labelPtr,
                    this.Layout.Handle,
                    entriesLen,
                    entriesPtr
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BindGroupDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.BindGroupDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
            Layout = new BindGroupLayout(backend.Layout)
            Entries = let ptr = backend.Entries in Array.init (int backend.EntryCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in BindGroupEntry.Read(&r))
        }
type BindGroupLayout internal(handle : nativeint) =
    member x.Handle = handle
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.BindGroupLayoutSetLabel(handle, _labelPtr)
type BufferBindingLayout = 
    {
        Type : BufferBindingType
        HasDynamicOffset : bool
        MinBindingSize : uint64
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.BufferBindingLayout> -> 'r) : 'r = 
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
    static member Read(backend : byref<WebGPU.Raw.BufferBindingLayout>) = 
        {
            Type = backend.Type
            HasDynamicOffset = (backend.HasDynamicOffset <> 0)
            MinBindingSize = backend.MinBindingSize
        }
type SamplerBindingLayout = 
    {
        Type : SamplerBindingType
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.SamplerBindingLayout> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.SamplerBindingLayout(
                this.Type
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SamplerBindingLayout> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.SamplerBindingLayout>) = 
        {
            Type = backend.Type
        }
type TextureBindingLayout = 
    {
        SampleType : TextureSampleType
        ViewDimension : TextureViewDimension
        Multisampled : bool
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.TextureBindingLayout> -> 'r) : 'r = 
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
    static member Read(backend : byref<WebGPU.Raw.TextureBindingLayout>) = 
        {
            SampleType = backend.SampleType
            ViewDimension = backend.ViewDimension
            Multisampled = (backend.Multisampled <> 0)
        }
type ExternalTextureBindingEntry = 
    {
        ExternalTexture : ExternalTexture
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.ExternalTextureBindingEntry> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.ExternalTextureBindingEntry(
                this.ExternalTexture.Handle
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ExternalTextureBindingEntry> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.ExternalTextureBindingEntry>) = 
        {
            ExternalTexture = new ExternalTexture(backend.ExternalTexture)
        }
type ExternalTextureBindingLayout() =
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.ExternalTextureBindingLayout> -> 'r) : 'r = 
        action (NativePtr.ofNativeInt 0n)
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ExternalTextureBindingLayout> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.ExternalTextureBindingLayout>) = 
        ExternalTextureBindingLayout()
type StorageTextureBindingLayout = 
    {
        Access : StorageTextureAccess
        Format : TextureFormat
        ViewDimension : TextureViewDimension
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.StorageTextureBindingLayout> -> 'r) : 'r = 
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
    static member Read(backend : byref<WebGPU.Raw.StorageTextureBindingLayout>) = 
        {
            Access = backend.Access
            Format = backend.Format
            ViewDimension = backend.ViewDimension
        }
type BindGroupLayoutEntry = 
    {
        Binding : int
        Visibility : ShaderStage
        Buffer : BufferBindingLayout
        Sampler : SamplerBindingLayout
        Texture : TextureBindingLayout
        StorageTexture : StorageTextureBindingLayout
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.BindGroupLayoutEntry> -> 'r) : 'r = 
        this.Buffer.Pin(fun _bufferPtr ->
            this.Sampler.Pin(fun _samplerPtr ->
                this.Texture.Pin(fun _texturePtr ->
                    this.StorageTexture.Pin(fun _storageTexturePtr ->
                        let mutable value =
                            new WebGPU.Raw.BindGroupLayoutEntry(
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
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BindGroupLayoutEntry> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.BindGroupLayoutEntry>) = 
        {
            Binding = int(backend.Binding)
            Visibility = backend.Visibility
            Buffer = BufferBindingLayout.Read(&backend.Buffer)
            Sampler = SamplerBindingLayout.Read(&backend.Sampler)
            Texture = TextureBindingLayout.Read(&backend.Texture)
            StorageTexture = StorageTextureBindingLayout.Read(&backend.StorageTexture)
        }
type BindGroupLayoutDescriptor = 
    {
        Label : string
        Entries : array<BindGroupLayoutEntry>
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.BindGroupLayoutDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        WebGPU.Raw.Pinnable.pinArray this.Entries (fun entriesPtr ->
            let entriesLen = uint32 this.Entries.Length
            let mutable value =
                new WebGPU.Raw.BindGroupLayoutDescriptor(
                    _labelPtr,
                    entriesLen,
                    entriesPtr
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BindGroupLayoutDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.BindGroupLayoutDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
            Entries = let ptr = backend.Entries in Array.init (int backend.EntryCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in BindGroupLayoutEntry.Read(&r))
        }
type BlendComponent = 
    {
        Operation : BlendOperation
        SrcFactor : BlendFactor
        DstFactor : BlendFactor
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.BlendComponent> -> 'r) : 'r = 
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
    static member Read(backend : byref<WebGPU.Raw.BlendComponent>) = 
        {
            Operation = backend.Operation
            SrcFactor = backend.SrcFactor
            DstFactor = backend.DstFactor
        }
type Buffer internal(handle : nativeint) =
    member x.Handle = handle
    member _.MapAsync(mode : MapMode, offset : unativeint, size : unativeint, callback : BufferMapCallback) : unit =
        let _callbackDel = WebGPU.Raw.BufferMapCallback(fun status userdata ->
            let _status = status
            let _userdata = userdata
            callback.Invoke(_status, _userdata)
        )
        let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
        WebGPU.Raw.WebGPU.BufferMapAsync(handle, mode, offset, size, _callbackPtr, _callbackUserData)
    member _.GetMappedRange(offset : unativeint, size : unativeint) : nativeint =
        WebGPU.Raw.WebGPU.BufferGetMappedRange(handle, offset, size)
    member _.GetConstMappedRange(offset : unativeint, size : unativeint) : nativeint =
        WebGPU.Raw.WebGPU.BufferGetConstMappedRange(handle, offset, size)
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.BufferSetLabel(handle, _labelPtr)
    member _.GetUsage() : BufferUsage =
        WebGPU.Raw.WebGPU.BufferGetUsage(handle)
    member _.GetSize() : uint64 =
        WebGPU.Raw.WebGPU.BufferGetSize(handle)
    member _.GetMapState() : BufferMapState =
        WebGPU.Raw.WebGPU.BufferGetMapState(handle)
    member _.Unmap() : unit =
        WebGPU.Raw.WebGPU.BufferUnmap(handle)
    member _.Destroy() : unit =
        WebGPU.Raw.WebGPU.BufferDestroy(handle)
    member x.Dispose() = x.Destroy()
    interface System.IDisposable with
        member x.Dispose() = x.Dispose()
type BufferDescriptor = 
    {
        Label : string
        Usage : BufferUsage
        Size : uint64
        MappedAtCreation : bool
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.BufferDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        let mutable value =
            new WebGPU.Raw.BufferDescriptor(
                _labelPtr,
                this.Usage,
                this.Size,
                (if this.MappedAtCreation then 1 else 0)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BufferDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.BufferDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
            Usage = backend.Usage
            Size = backend.Size
            MappedAtCreation = (backend.MappedAtCreation <> 0)
        }
type BufferMapCallback = delegate of status : BufferMapAsyncStatus * userdata : nativeint -> unit
type Color = 
    {
        R : double
        G : double
        B : double
        A : double
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.Color> -> 'r) : 'r = 
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
    static member Read(backend : byref<WebGPU.Raw.Color>) = 
        {
            R = backend.R
            G = backend.G
            B = backend.B
            A = backend.A
        }
type ConstantEntry = 
    {
        Key : string
        Value : double
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.ConstantEntry> -> 'r) : 'r = 
        use _keyPtr = fixed (Encoding.UTF8.GetBytes(this.Key))
        let mutable value =
            new WebGPU.Raw.ConstantEntry(
                _keyPtr,
                this.Value
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ConstantEntry> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.ConstantEntry>) = 
        {
            Key = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Key)
            Value = backend.Value
        }
type CommandBuffer internal(handle : nativeint) =
    member x.Handle = handle
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.CommandBufferSetLabel(handle, _labelPtr)
type CommandBufferDescriptor = 
    {
        Label : string
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.CommandBufferDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        let mutable value =
            new WebGPU.Raw.CommandBufferDescriptor(
                _labelPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CommandBufferDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.CommandBufferDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
        }
type CommandEncoder internal(handle : nativeint) =
    member x.Handle = handle
    member _.Finish(descriptor : CommandBufferDescriptor) : CommandBuffer =
        descriptor.Pin(fun _descriptorPtr ->
            new CommandBuffer(WebGPU.Raw.WebGPU.CommandEncoderFinish(handle, _descriptorPtr))
        )
    member _.BeginComputePass(descriptor : ComputePassDescriptor) : ComputePassEncoder =
        descriptor.Pin(fun _descriptorPtr ->
            new ComputePassEncoder(WebGPU.Raw.WebGPU.CommandEncoderBeginComputePass(handle, _descriptorPtr))
        )
    member _.BeginRenderPass(descriptor : RenderPassDescriptor) : RenderPassEncoder =
        descriptor.Pin(fun _descriptorPtr ->
            new RenderPassEncoder(WebGPU.Raw.WebGPU.CommandEncoderBeginRenderPass(handle, _descriptorPtr))
        )
    member _.CopyBufferToBuffer(source : Buffer, sourceOffset : uint64, destination : Buffer, destinationOffset : uint64, size : uint64) : unit =
        WebGPU.Raw.WebGPU.CommandEncoderCopyBufferToBuffer(handle, source.Handle, sourceOffset, destination.Handle, destinationOffset, size)
    member _.CopyBufferToTexture(source : ImageCopyBuffer, destination : ImageCopyTexture, copySize : Extent3D) : unit =
        source.Pin(fun _sourcePtr ->
            destination.Pin(fun _destinationPtr ->
                copySize.Pin(fun _copySizePtr ->
                    WebGPU.Raw.WebGPU.CommandEncoderCopyBufferToTexture(handle, _sourcePtr, _destinationPtr, _copySizePtr)
                )
            )
        )
    member _.CopyTextureToBuffer(source : ImageCopyTexture, destination : ImageCopyBuffer, copySize : Extent3D) : unit =
        source.Pin(fun _sourcePtr ->
            destination.Pin(fun _destinationPtr ->
                copySize.Pin(fun _copySizePtr ->
                    WebGPU.Raw.WebGPU.CommandEncoderCopyTextureToBuffer(handle, _sourcePtr, _destinationPtr, _copySizePtr)
                )
            )
        )
    member _.CopyTextureToTexture(source : ImageCopyTexture, destination : ImageCopyTexture, copySize : Extent3D) : unit =
        source.Pin(fun _sourcePtr ->
            destination.Pin(fun _destinationPtr ->
                copySize.Pin(fun _copySizePtr ->
                    WebGPU.Raw.WebGPU.CommandEncoderCopyTextureToTexture(handle, _sourcePtr, _destinationPtr, _copySizePtr)
                )
            )
        )
    member _.CopyTextureToTextureInternal(source : ImageCopyTexture, destination : ImageCopyTexture, copySize : Extent3D) : unit =
        source.Pin(fun _sourcePtr ->
            destination.Pin(fun _destinationPtr ->
                copySize.Pin(fun _copySizePtr ->
                    WebGPU.Raw.WebGPU.CommandEncoderCopyTextureToTextureInternal(handle, _sourcePtr, _destinationPtr, _copySizePtr)
                )
            )
        )
    member _.ClearBuffer(buffer : Buffer, offset : uint64, size : uint64) : unit =
        WebGPU.Raw.WebGPU.CommandEncoderClearBuffer(handle, buffer.Handle, offset, size)
    member _.InjectValidationError(message : string) : unit =
        use _messagePtr = fixed (Encoding.UTF8.GetBytes(message))
        WebGPU.Raw.WebGPU.CommandEncoderInjectValidationError(handle, _messagePtr)
    member _.InsertDebugMarker(markerLabel : string) : unit =
        use _markerLabelPtr = fixed (Encoding.UTF8.GetBytes(markerLabel))
        WebGPU.Raw.WebGPU.CommandEncoderInsertDebugMarker(handle, _markerLabelPtr)
    member _.PopDebugGroup() : unit =
        WebGPU.Raw.WebGPU.CommandEncoderPopDebugGroup(handle)
    member _.PushDebugGroup(groupLabel : string) : unit =
        use _groupLabelPtr = fixed (Encoding.UTF8.GetBytes(groupLabel))
        WebGPU.Raw.WebGPU.CommandEncoderPushDebugGroup(handle, _groupLabelPtr)
    member _.ResolveQuerySet(querySet : QuerySet, firstQuery : int, queryCount : int, destination : Buffer, destinationOffset : uint64) : unit =
        WebGPU.Raw.WebGPU.CommandEncoderResolveQuerySet(handle, querySet.Handle, uint32(firstQuery), uint32(queryCount), destination.Handle, destinationOffset)
    member _.WriteBuffer(buffer : Buffer, bufferOffset : uint64, data : nativeptr<uint8>, size : uint64) : unit =
        WebGPU.Raw.WebGPU.CommandEncoderWriteBuffer(handle, buffer.Handle, bufferOffset, data, size)
    member _.WriteTimestamp(querySet : QuerySet, queryIndex : int) : unit =
        WebGPU.Raw.WebGPU.CommandEncoderWriteTimestamp(handle, querySet.Handle, uint32(queryIndex))
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.CommandEncoderSetLabel(handle, _labelPtr)
type CommandEncoderDescriptor = 
    {
        Label : string
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.CommandEncoderDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        let mutable value =
            new WebGPU.Raw.CommandEncoderDescriptor(
                _labelPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CommandEncoderDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.CommandEncoderDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
        }
type CompilationInfo = 
    {
        Messages : array<CompilationMessage>
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.CompilationInfo> -> 'r) : 'r = 
        WebGPU.Raw.Pinnable.pinArray this.Messages (fun messagesPtr ->
            let messagesLen = uint32 this.Messages.Length
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
    static member Read(backend : byref<WebGPU.Raw.CompilationInfo>) = 
        {
            Messages = let ptr = backend.Messages in Array.init (int backend.MessageCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in CompilationMessage.Read(&r))
        }
type CompilationInfoCallback = delegate of status : CompilationInfoRequestStatus * compilationInfo : CompilationInfo * userdata : nativeint -> unit
type CompilationMessage = 
    {
        Message : string
        Type : CompilationMessageType
        LineNum : uint64
        LinePos : uint64
        Offset : uint64
        Length : uint64
        Utf16LinePos : uint64
        Utf16Offset : uint64
        Utf16Length : uint64
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.CompilationMessage> -> 'r) : 'r = 
        use _messagePtr = fixed (Encoding.UTF8.GetBytes(this.Message))
        let mutable value =
            new WebGPU.Raw.CompilationMessage(
                _messagePtr,
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
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CompilationMessage> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.CompilationMessage>) = 
        {
            Message = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Message)
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
        Label : string
        TimestampWrites : array<ComputePassTimestampWrite>
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.ComputePassDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        WebGPU.Raw.Pinnable.pinArray this.TimestampWrites (fun timestampWritesPtr ->
            let timestampWritesLen = uint32 this.TimestampWrites.Length
            let mutable value =
                new WebGPU.Raw.ComputePassDescriptor(
                    _labelPtr,
                    timestampWritesLen,
                    timestampWritesPtr
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ComputePassDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.ComputePassDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
            TimestampWrites = let ptr = backend.TimestampWrites in Array.init (int backend.TimestampWriteCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in ComputePassTimestampWrite.Read(&r))
        }
type ComputePassEncoder internal(handle : nativeint) =
    member x.Handle = handle
    member _.InsertDebugMarker(markerLabel : string) : unit =
        use _markerLabelPtr = fixed (Encoding.UTF8.GetBytes(markerLabel))
        WebGPU.Raw.WebGPU.ComputePassEncoderInsertDebugMarker(handle, _markerLabelPtr)
    member _.PopDebugGroup() : unit =
        WebGPU.Raw.WebGPU.ComputePassEncoderPopDebugGroup(handle)
    member _.PushDebugGroup(groupLabel : string) : unit =
        use _groupLabelPtr = fixed (Encoding.UTF8.GetBytes(groupLabel))
        WebGPU.Raw.WebGPU.ComputePassEncoderPushDebugGroup(handle, _groupLabelPtr)
    member _.SetPipeline(pipeline : ComputePipeline) : unit =
        WebGPU.Raw.WebGPU.ComputePassEncoderSetPipeline(handle, pipeline.Handle)
    member _.SetBindGroup(groupIndex : int, group : BindGroup, dynamicOffsets : array<uint32>) : unit =
        use dynamicOffsetsPtr = fixed (dynamicOffsets)
        let dynamicOffsetsLen = uint32 dynamicOffsets.Length
        WebGPU.Raw.WebGPU.ComputePassEncoderSetBindGroup(handle, uint32(groupIndex), group.Handle, dynamicOffsetsLen, dynamicOffsetsPtr)
    member _.WriteTimestamp(querySet : QuerySet, queryIndex : int) : unit =
        WebGPU.Raw.WebGPU.ComputePassEncoderWriteTimestamp(handle, querySet.Handle, uint32(queryIndex))
    member _.BeginPipelineStatisticsQuery(querySet : QuerySet, queryIndex : int) : unit =
        WebGPU.Raw.WebGPU.ComputePassEncoderBeginPipelineStatisticsQuery(handle, querySet.Handle, uint32(queryIndex))
    member _.Dispatch(workgroupCountX : int, workgroupCountY : int, workgroupCountZ : int) : unit =
        WebGPU.Raw.WebGPU.ComputePassEncoderDispatch(handle, uint32(workgroupCountX), uint32(workgroupCountY), uint32(workgroupCountZ))
    member _.DispatchWorkgroups(workgroupCountX : int, workgroupCountY : int, workgroupCountZ : int) : unit =
        WebGPU.Raw.WebGPU.ComputePassEncoderDispatchWorkgroups(handle, uint32(workgroupCountX), uint32(workgroupCountY), uint32(workgroupCountZ))
    member _.DispatchIndirect(indirectBuffer : Buffer, indirectOffset : uint64) : unit =
        WebGPU.Raw.WebGPU.ComputePassEncoderDispatchIndirect(handle, indirectBuffer.Handle, indirectOffset)
    member _.DispatchWorkgroupsIndirect(indirectBuffer : Buffer, indirectOffset : uint64) : unit =
        WebGPU.Raw.WebGPU.ComputePassEncoderDispatchWorkgroupsIndirect(handle, indirectBuffer.Handle, indirectOffset)
    member _.End() : unit =
        WebGPU.Raw.WebGPU.ComputePassEncoderEnd(handle)
    member _.EndPass() : unit =
        WebGPU.Raw.WebGPU.ComputePassEncoderEndPass(handle)
    member _.EndPipelineStatisticsQuery() : unit =
        WebGPU.Raw.WebGPU.ComputePassEncoderEndPipelineStatisticsQuery(handle)
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.ComputePassEncoderSetLabel(handle, _labelPtr)
type ComputePassTimestampWrite = 
    {
        QuerySet : QuerySet
        QueryIndex : int
        Location : ComputePassTimestampLocation
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.ComputePassTimestampWrite> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.ComputePassTimestampWrite(
                this.QuerySet.Handle,
                uint32(this.QueryIndex),
                this.Location
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ComputePassTimestampWrite> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.ComputePassTimestampWrite>) = 
        {
            QuerySet = new QuerySet(backend.QuerySet)
            QueryIndex = int(backend.QueryIndex)
            Location = backend.Location
        }
type ComputePipeline internal(handle : nativeint) =
    member x.Handle = handle
    member _.GetBindGroupLayout(groupIndex : int) : BindGroupLayout =
        new BindGroupLayout(WebGPU.Raw.WebGPU.ComputePipelineGetBindGroupLayout(handle, uint32(groupIndex)))
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.ComputePipelineSetLabel(handle, _labelPtr)
type ComputePipelineDescriptor = 
    {
        Label : string
        Layout : PipelineLayout
        Compute : ProgrammableStageDescriptor
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.ComputePipelineDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        this.Compute.Pin(fun _computePtr ->
            let mutable value =
                new WebGPU.Raw.ComputePipelineDescriptor(
                    _labelPtr,
                    this.Layout.Handle,
                    NativePtr.read _computePtr
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ComputePipelineDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.ComputePipelineDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
            Layout = new PipelineLayout(backend.Layout)
            Compute = ProgrammableStageDescriptor.Read(&backend.Compute)
        }
type CopyTextureForBrowserOptions = 
    {
        FlipY : bool
        NeedsColorSpaceConversion : bool
        SrcAlphaMode : AlphaMode
        SrcTransferFunctionParameters : nativeptr<float32>
        ConversionMatrix : nativeptr<float32>
        DstTransferFunctionParameters : nativeptr<float32>
        DstAlphaMode : AlphaMode
        InternalUsage : bool
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.CopyTextureForBrowserOptions> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.CopyTextureForBrowserOptions(
                (if this.FlipY then 1 else 0),
                (if this.NeedsColorSpaceConversion then 1 else 0),
                this.SrcAlphaMode,
                this.SrcTransferFunctionParameters,
                this.ConversionMatrix,
                this.DstTransferFunctionParameters,
                this.DstAlphaMode,
                (if this.InternalUsage then 1 else 0)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CopyTextureForBrowserOptions> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.CopyTextureForBrowserOptions>) = 
        {
            FlipY = (backend.FlipY <> 0)
            NeedsColorSpaceConversion = (backend.NeedsColorSpaceConversion <> 0)
            SrcAlphaMode = backend.SrcAlphaMode
            SrcTransferFunctionParameters = backend.SrcTransferFunctionParameters
            ConversionMatrix = backend.ConversionMatrix
            DstTransferFunctionParameters = backend.DstTransferFunctionParameters
            DstAlphaMode = backend.DstAlphaMode
            InternalUsage = (backend.InternalUsage <> 0)
        }
type CreateComputePipelineAsyncCallback = delegate of status : CreatePipelineAsyncStatus * pipeline : ComputePipeline * message : string * userdata : nativeint -> unit
type CreateRenderPipelineAsyncCallback = delegate of status : CreatePipelineAsyncStatus * pipeline : RenderPipeline * message : string * userdata : nativeint -> unit
type Device internal(handle : nativeint) =
    member x.Handle = handle
    member _.CreateBindGroup(descriptor : BindGroupDescriptor) : BindGroup =
        descriptor.Pin(fun _descriptorPtr ->
            new BindGroup(WebGPU.Raw.WebGPU.DeviceCreateBindGroup(handle, _descriptorPtr))
        )
    member _.CreateBindGroupLayout(descriptor : BindGroupLayoutDescriptor) : BindGroupLayout =
        descriptor.Pin(fun _descriptorPtr ->
            new BindGroupLayout(WebGPU.Raw.WebGPU.DeviceCreateBindGroupLayout(handle, _descriptorPtr))
        )
    member _.CreateBuffer(descriptor : BufferDescriptor) : Buffer =
        descriptor.Pin(fun _descriptorPtr ->
            new Buffer(WebGPU.Raw.WebGPU.DeviceCreateBuffer(handle, _descriptorPtr))
        )
    member _.CreateErrorBuffer(descriptor : BufferDescriptor) : Buffer =
        descriptor.Pin(fun _descriptorPtr ->
            new Buffer(WebGPU.Raw.WebGPU.DeviceCreateErrorBuffer(handle, _descriptorPtr))
        )
    member _.CreateCommandEncoder(descriptor : CommandEncoderDescriptor) : CommandEncoder =
        descriptor.Pin(fun _descriptorPtr ->
            new CommandEncoder(WebGPU.Raw.WebGPU.DeviceCreateCommandEncoder(handle, _descriptorPtr))
        )
    member _.CreateComputePipeline(descriptor : ComputePipelineDescriptor) : ComputePipeline =
        descriptor.Pin(fun _descriptorPtr ->
            new ComputePipeline(WebGPU.Raw.WebGPU.DeviceCreateComputePipeline(handle, _descriptorPtr))
        )
    member _.CreateComputePipelineAsync(descriptor : ComputePipelineDescriptor, callback : CreateComputePipelineAsyncCallback) : unit =
        descriptor.Pin(fun _descriptorPtr ->
            let _callbackDel = WebGPU.Raw.CreateComputePipelineAsyncCallback(fun status pipeline message userdata ->
                let _status = status
                let _pipeline = new ComputePipeline(pipeline)
                let _message = Marshal.PtrToStringAnsi(NativePtr.toNativeInt message)
                let _userdata = userdata
                callback.Invoke(_status, _pipeline, _message, _userdata)
            )
            let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
            WebGPU.Raw.WebGPU.DeviceCreateComputePipelineAsync(handle, _descriptorPtr, _callbackPtr, _callbackUserData)
        )
    member _.CreatePipelineLayout(descriptor : PipelineLayoutDescriptor) : PipelineLayout =
        descriptor.Pin(fun _descriptorPtr ->
            new PipelineLayout(WebGPU.Raw.WebGPU.DeviceCreatePipelineLayout(handle, _descriptorPtr))
        )
    member _.CreateQuerySet(descriptor : QuerySetDescriptor) : QuerySet =
        descriptor.Pin(fun _descriptorPtr ->
            new QuerySet(WebGPU.Raw.WebGPU.DeviceCreateQuerySet(handle, _descriptorPtr))
        )
    member _.CreateRenderPipelineAsync(descriptor : RenderPipelineDescriptor, callback : CreateRenderPipelineAsyncCallback) : unit =
        descriptor.Pin(fun _descriptorPtr ->
            let _callbackDel = WebGPU.Raw.CreateRenderPipelineAsyncCallback(fun status pipeline message userdata ->
                let _status = status
                let _pipeline = new RenderPipeline(pipeline)
                let _message = Marshal.PtrToStringAnsi(NativePtr.toNativeInt message)
                let _userdata = userdata
                callback.Invoke(_status, _pipeline, _message, _userdata)
            )
            let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
            WebGPU.Raw.WebGPU.DeviceCreateRenderPipelineAsync(handle, _descriptorPtr, _callbackPtr, _callbackUserData)
        )
    member _.CreateRenderBundleEncoder(descriptor : RenderBundleEncoderDescriptor) : RenderBundleEncoder =
        descriptor.Pin(fun _descriptorPtr ->
            new RenderBundleEncoder(WebGPU.Raw.WebGPU.DeviceCreateRenderBundleEncoder(handle, _descriptorPtr))
        )
    member _.CreateRenderPipeline(descriptor : RenderPipelineDescriptor) : RenderPipeline =
        descriptor.Pin(fun _descriptorPtr ->
            new RenderPipeline(WebGPU.Raw.WebGPU.DeviceCreateRenderPipeline(handle, _descriptorPtr))
        )
    member _.CreateSampler(descriptor : SamplerDescriptor) : Sampler =
        descriptor.Pin(fun _descriptorPtr ->
            new Sampler(WebGPU.Raw.WebGPU.DeviceCreateSampler(handle, _descriptorPtr))
        )
    member _.CreateShaderModule(descriptor : ShaderModuleDescriptor) : ShaderModule =
        descriptor.Pin(fun _descriptorPtr ->
            new ShaderModule(WebGPU.Raw.WebGPU.DeviceCreateShaderModule(handle, _descriptorPtr))
        )
    member _.CreateSwapChain(surface : Surface, descriptor : SwapChainDescriptor) : SwapChain =
        descriptor.Pin(fun _descriptorPtr ->
            new SwapChain(WebGPU.Raw.WebGPU.DeviceCreateSwapChain(handle, surface.Handle, _descriptorPtr))
        )
    member _.CreateTexture(descriptor : TextureDescriptor) : Texture =
        descriptor.Pin(fun _descriptorPtr ->
            new Texture(WebGPU.Raw.WebGPU.DeviceCreateTexture(handle, _descriptorPtr))
        )
    member _.CreateErrorTexture(descriptor : TextureDescriptor) : Texture =
        descriptor.Pin(fun _descriptorPtr ->
            new Texture(WebGPU.Raw.WebGPU.DeviceCreateErrorTexture(handle, _descriptorPtr))
        )
    member _.Destroy() : unit =
        WebGPU.Raw.WebGPU.DeviceDestroy(handle)
    member _.Limits : SupportedLimits =
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        let status = WebGPU.Raw.WebGPU.DeviceGetLimits(handle, ptr)
        if status = 0 then failwith "GetLimits failed"
        SupportedLimits.Read(&res)
    member _.HasFeature(feature : FeatureName) : bool =
        (WebGPU.Raw.WebGPU.DeviceHasFeature(handle, feature) <> 0)
    member _.EnumerateFeatures(features : nativeptr<FeatureName>) : unativeint =
        WebGPU.Raw.WebGPU.DeviceEnumerateFeatures(handle, features)
    member _.GetAdapter() : Adapter =
        new Adapter(WebGPU.Raw.WebGPU.DeviceGetAdapter(handle))
    member _.GetQueue() : Queue =
        new Queue(WebGPU.Raw.WebGPU.DeviceGetQueue(handle))
    member _.InjectError(typ : ErrorType, message : string) : unit =
        use _messagePtr = fixed (Encoding.UTF8.GetBytes(message))
        WebGPU.Raw.WebGPU.DeviceInjectError(handle, typ, _messagePtr)
    member _.ForceLoss(typ : DeviceLostReason, message : string) : unit =
        use _messagePtr = fixed (Encoding.UTF8.GetBytes(message))
        WebGPU.Raw.WebGPU.DeviceForceLoss(handle, typ, _messagePtr)
    member _.Tick() : unit =
        WebGPU.Raw.WebGPU.DeviceTick(handle)
    member _.SetUncapturedErrorCallback(callback : ErrorCallback) : unit =
        let _callbackDel = WebGPU.Raw.ErrorCallback(fun typ message userdata ->
            let _typ = typ
            let _message = Marshal.PtrToStringAnsi(NativePtr.toNativeInt message)
            let _userdata = userdata
            callback.Invoke(_typ, _message, _userdata)
        )
        let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
        WebGPU.Raw.WebGPU.DeviceSetUncapturedErrorCallback(handle, _callbackPtr, _callbackUserData)
    member _.SetDeviceLostCallback(callback : DeviceLostCallback) : unit =
        let _callbackDel = WebGPU.Raw.DeviceLostCallback(fun reason message userdata ->
            let _reason = reason
            let _message = Marshal.PtrToStringAnsi(NativePtr.toNativeInt message)
            let _userdata = userdata
            callback.Invoke(_reason, _message, _userdata)
        )
        let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
        WebGPU.Raw.WebGPU.DeviceSetDeviceLostCallback(handle, _callbackPtr, _callbackUserData)
    member _.PushErrorScope(filter : ErrorFilter) : unit =
        WebGPU.Raw.WebGPU.DevicePushErrorScope(handle, filter)
    member _.PopErrorScope(callback : ErrorCallback) : bool =
        let _callbackDel = WebGPU.Raw.ErrorCallback(fun typ message userdata ->
            let _typ = typ
            let _message = Marshal.PtrToStringAnsi(NativePtr.toNativeInt message)
            let _userdata = userdata
            callback.Invoke(_typ, _message, _userdata)
        )
        let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
        (WebGPU.Raw.WebGPU.DevicePopErrorScope(handle, _callbackPtr, _callbackUserData) <> 0)
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.DeviceSetLabel(handle, _labelPtr)
    member _.ValidateTextureDescriptor(descriptor : TextureDescriptor) : unit =
        descriptor.Pin(fun _descriptorPtr ->
            WebGPU.Raw.WebGPU.DeviceValidateTextureDescriptor(handle, _descriptorPtr)
        )
    member x.Dispose() = x.Destroy()
    interface System.IDisposable with
        member x.Dispose() = x.Dispose()
type DeviceLostCallback = delegate of reason : DeviceLostReason * message : string * userdata : nativeint -> unit
type ErrorCallback = delegate of typ : ErrorType * message : string * userdata : nativeint -> unit
type Limits = 
    {
        MaxTextureDimension1D : int
        MaxTextureDimension2D : int
        MaxTextureDimension3D : int
        MaxTextureArrayLayers : int
        MaxBindGroups : int
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
        MaxFragmentCombinedOutputResources : int
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.Limits> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.Limits(
                uint32(this.MaxTextureDimension1D),
                uint32(this.MaxTextureDimension2D),
                uint32(this.MaxTextureDimension3D),
                uint32(this.MaxTextureArrayLayers),
                uint32(this.MaxBindGroups),
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
                uint32(this.MaxComputeWorkgroupsPerDimension),
                uint32(this.MaxFragmentCombinedOutputResources)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.Limits> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.Limits>) = 
        {
            MaxTextureDimension1D = int(backend.MaxTextureDimension1D)
            MaxTextureDimension2D = int(backend.MaxTextureDimension2D)
            MaxTextureDimension3D = int(backend.MaxTextureDimension3D)
            MaxTextureArrayLayers = int(backend.MaxTextureArrayLayers)
            MaxBindGroups = int(backend.MaxBindGroups)
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
            MaxFragmentCombinedOutputResources = int(backend.MaxFragmentCombinedOutputResources)
        }
type RequiredLimits = 
    {
        Limits : Limits
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.RequiredLimits> -> 'r) : 'r = 
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
    static member Read(backend : byref<WebGPU.Raw.RequiredLimits>) = 
        {
            Limits = Limits.Read(&backend.Limits)
        }
type SupportedLimits = 
    {
        Limits : Limits
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.SupportedLimits> -> 'r) : 'r = 
        this.Limits.Pin(fun _limitsPtr ->
            let mutable value =
                new WebGPU.Raw.SupportedLimits(
                    NativePtr.read _limitsPtr
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SupportedLimits> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.SupportedLimits>) = 
        {
            Limits = Limits.Read(&backend.Limits)
        }
type LoggingCallback = delegate of typ : LoggingType * message : string * userdata : nativeint -> unit
type Extent2D = 
    {
        Width : int
        Height : int
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.Extent2D> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.Extent2D(
                uint32(this.Width),
                uint32(this.Height)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.Extent2D> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.Extent2D>) = 
        {
            Width = int(backend.Width)
            Height = int(backend.Height)
        }
type Extent3D = 
    {
        Width : int
        Height : int
        DepthOrArrayLayers : int
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.Extent3D> -> 'r) : 'r = 
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
    static member Read(backend : byref<WebGPU.Raw.Extent3D>) = 
        {
            Width = int(backend.Width)
            Height = int(backend.Height)
            DepthOrArrayLayers = int(backend.DepthOrArrayLayers)
        }
type ExternalTexture internal(handle : nativeint) =
    member x.Handle = handle
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.ExternalTextureSetLabel(handle, _labelPtr)
    member _.Destroy() : unit =
        WebGPU.Raw.WebGPU.ExternalTextureDestroy(handle)
    member _.Expire() : unit =
        WebGPU.Raw.WebGPU.ExternalTextureExpire(handle)
    member _.Refresh() : unit =
        WebGPU.Raw.WebGPU.ExternalTextureRefresh(handle)
    member x.Dispose() = x.Destroy()
    interface System.IDisposable with
        member x.Dispose() = x.Dispose()
type ExternalTextureDescriptor = 
    {
        Label : string
        Plane0 : TextureView
        Plane1 : TextureView
        VisibleOrigin : Origin2D
        VisibleSize : Extent2D
        DoYuvToRgbConversionOnly : bool
        YuvToRgbConversionMatrix : nativeptr<float32>
        SrcTransferFunctionParameters : nativeptr<float32>
        DstTransferFunctionParameters : nativeptr<float32>
        GamutConversionMatrix : nativeptr<float32>
        FlipY : bool
        Rotation : ExternalTextureRotation
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.ExternalTextureDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        this.VisibleOrigin.Pin(fun _visibleOriginPtr ->
            this.VisibleSize.Pin(fun _visibleSizePtr ->
                let mutable value =
                    new WebGPU.Raw.ExternalTextureDescriptor(
                        _labelPtr,
                        this.Plane0.Handle,
                        this.Plane1.Handle,
                        NativePtr.read _visibleOriginPtr,
                        NativePtr.read _visibleSizePtr,
                        (if this.DoYuvToRgbConversionOnly then 1 else 0),
                        this.YuvToRgbConversionMatrix,
                        this.SrcTransferFunctionParameters,
                        this.DstTransferFunctionParameters,
                        this.GamutConversionMatrix,
                        (if this.FlipY then 1 else 0),
                        this.Rotation
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ExternalTextureDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.ExternalTextureDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
            Plane0 = new TextureView(backend.Plane0)
            Plane1 = new TextureView(backend.Plane1)
            VisibleOrigin = Origin2D.Read(&backend.VisibleOrigin)
            VisibleSize = Extent2D.Read(&backend.VisibleSize)
            DoYuvToRgbConversionOnly = (backend.DoYuvToRgbConversionOnly <> 0)
            YuvToRgbConversionMatrix = backend.YuvToRgbConversionMatrix
            SrcTransferFunctionParameters = backend.SrcTransferFunctionParameters
            DstTransferFunctionParameters = backend.DstTransferFunctionParameters
            GamutConversionMatrix = backend.GamutConversionMatrix
            FlipY = (backend.FlipY <> 0)
            Rotation = backend.Rotation
        }
type ImageCopyBuffer = 
    {
        Layout : TextureDataLayout
        Buffer : Buffer
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.ImageCopyBuffer> -> 'r) : 'r = 
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
    static member Read(backend : byref<WebGPU.Raw.ImageCopyBuffer>) = 
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
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.ImageCopyTexture> -> 'r) : 'r = 
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
    static member Read(backend : byref<WebGPU.Raw.ImageCopyTexture>) = 
        {
            Texture = new Texture(backend.Texture)
            MipLevel = int(backend.MipLevel)
            Origin = Origin3D.Read(&backend.Origin)
            Aspect = backend.Aspect
        }
type ImageCopyExternalTexture = 
    {
        ExternalTexture : ExternalTexture
        Origin : Origin3D
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.ImageCopyExternalTexture> -> 'r) : 'r = 
        this.Origin.Pin(fun _originPtr ->
            let mutable value =
                new WebGPU.Raw.ImageCopyExternalTexture(
                    this.ExternalTexture.Handle,
                    NativePtr.read _originPtr
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ImageCopyExternalTexture> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.ImageCopyExternalTexture>) = 
        {
            ExternalTexture = new ExternalTexture(backend.ExternalTexture)
            Origin = Origin3D.Read(&backend.Origin)
        }
type Instance internal(handle : nativeint) =
    member x.Handle = handle
    member _.CreateSurface(descriptor : SurfaceDescriptor) : Surface =
        descriptor.Pin(fun _descriptorPtr ->
            new Surface(WebGPU.Raw.WebGPU.InstanceCreateSurface(handle, _descriptorPtr))
        )
    member _.ProcessEvents() : unit =
        WebGPU.Raw.WebGPU.InstanceProcessEvents(handle)
    member _.RequestAdapter(options : RequestAdapterOptions, callback : RequestAdapterCallback) : unit =
        options.Pin(fun _optionsPtr ->
            let _callbackDel = WebGPU.Raw.RequestAdapterCallback(fun status adapter message userdata ->
                let _status = status
                let _adapter = new Adapter(adapter)
                let _message = Marshal.PtrToStringAnsi(NativePtr.toNativeInt message)
                let _userdata = userdata
                callback.Invoke(_status, _adapter, _message, _userdata)
            )
            let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
            WebGPU.Raw.WebGPU.InstanceRequestAdapter(handle, _optionsPtr, _callbackPtr, _callbackUserData)
        )
type InstanceDescriptor() =
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.InstanceDescriptor> -> 'r) : 'r = 
        action (NativePtr.ofNativeInt 0n)
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.InstanceDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.InstanceDescriptor>) = 
        InstanceDescriptor()
type DawnInstanceDescriptor = 
    {
        AdditionalRuntimeSearchPathsCount : int
        AdditionalRuntimeSearchPaths : nativeptr<nativeptr<byte>>
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.DawnInstanceDescriptor> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.DawnInstanceDescriptor(
                uint32(this.AdditionalRuntimeSearchPathsCount),
                this.AdditionalRuntimeSearchPaths
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnInstanceDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.DawnInstanceDescriptor>) = 
        {
            AdditionalRuntimeSearchPathsCount = int(backend.AdditionalRuntimeSearchPathsCount)
            AdditionalRuntimeSearchPaths = backend.AdditionalRuntimeSearchPaths
        }
type VertexAttribute = 
    {
        Format : VertexFormat
        Offset : uint64
        ShaderLocation : int
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.VertexAttribute> -> 'r) : 'r = 
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
    static member Read(backend : byref<WebGPU.Raw.VertexAttribute>) = 
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
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.VertexBufferLayout> -> 'r) : 'r = 
        WebGPU.Raw.Pinnable.pinArray this.Attributes (fun attributesPtr ->
            let attributesLen = uint32 this.Attributes.Length
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
    static member Read(backend : byref<WebGPU.Raw.VertexBufferLayout>) = 
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
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.Origin3D> -> 'r) : 'r = 
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
    static member Read(backend : byref<WebGPU.Raw.Origin3D>) = 
        {
            X = int(backend.X)
            Y = int(backend.Y)
            Z = int(backend.Z)
        }
type Origin2D = 
    {
        X : int
        Y : int
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.Origin2D> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.Origin2D(
                uint32(this.X),
                uint32(this.Y)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.Origin2D> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.Origin2D>) = 
        {
            X = int(backend.X)
            Y = int(backend.Y)
        }
type PipelineLayout internal(handle : nativeint) =
    member x.Handle = handle
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.PipelineLayoutSetLabel(handle, _labelPtr)
type PipelineLayoutDescriptor = 
    {
        Label : string
        BindGroupLayouts : array<BindGroupLayout>
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.PipelineLayoutDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        let bindGroupLayoutsHandles = this.BindGroupLayouts |> Array.map (fun a -> a.Handle)
        use bindGroupLayoutsPtr = fixed (bindGroupLayoutsHandles)
        let bindGroupLayoutsLen = uint32 this.BindGroupLayouts.Length
        let mutable value =
            new WebGPU.Raw.PipelineLayoutDescriptor(
                _labelPtr,
                bindGroupLayoutsLen,
                bindGroupLayoutsPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.PipelineLayoutDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.PipelineLayoutDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
            BindGroupLayouts = let ptr = backend.BindGroupLayouts in Array.init (int backend.BindGroupLayoutCount) (fun i -> new BindGroupLayout(NativePtr.get ptr i))
        }
type ProgrammableStageDescriptor = 
    {
        Module : ShaderModule
        EntryPoint : string
        Constants : array<ConstantEntry>
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.ProgrammableStageDescriptor> -> 'r) : 'r = 
        use _entryPointPtr = fixed (Encoding.UTF8.GetBytes(this.EntryPoint))
        WebGPU.Raw.Pinnable.pinArray this.Constants (fun constantsPtr ->
            let constantsLen = uint32 this.Constants.Length
            let mutable value =
                new WebGPU.Raw.ProgrammableStageDescriptor(
                    this.Module.Handle,
                    _entryPointPtr,
                    constantsLen,
                    constantsPtr
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ProgrammableStageDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.ProgrammableStageDescriptor>) = 
        {
            Module = new ShaderModule(backend.Module)
            EntryPoint = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.EntryPoint)
            Constants = let ptr = backend.Constants in Array.init (int backend.ConstantCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in ConstantEntry.Read(&r))
        }
type QuerySet internal(handle : nativeint) =
    member x.Handle = handle
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.QuerySetSetLabel(handle, _labelPtr)
    member _.GetType() : QueryType =
        WebGPU.Raw.WebGPU.QuerySetGetType(handle)
    member _.GetCount() : int =
        int(WebGPU.Raw.WebGPU.QuerySetGetCount(handle))
    member _.Destroy() : unit =
        WebGPU.Raw.WebGPU.QuerySetDestroy(handle)
    member x.Dispose() = x.Destroy()
    interface System.IDisposable with
        member x.Dispose() = x.Dispose()
type QuerySetDescriptor = 
    {
        Label : string
        Type : QueryType
        Count : int
        PipelineStatistics : nativeptr<PipelineStatisticName>
        PipelineStatisticsCount : int
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.QuerySetDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        let mutable value =
            new WebGPU.Raw.QuerySetDescriptor(
                _labelPtr,
                this.Type,
                uint32(this.Count),
                this.PipelineStatistics,
                uint32(this.PipelineStatisticsCount)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.QuerySetDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.QuerySetDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
            Type = backend.Type
            Count = int(backend.Count)
            PipelineStatistics = backend.PipelineStatistics
            PipelineStatisticsCount = int(backend.PipelineStatisticsCount)
        }
type Queue internal(handle : nativeint) =
    member x.Handle = handle
    member _.Submit(commands : array<CommandBuffer>) : unit =
        let commandsHandles = commands |> Array.map (fun a -> a.Handle)
        use commandsPtr = fixed (commandsHandles)
        let commandsLen = uint32 commands.Length
        WebGPU.Raw.WebGPU.QueueSubmit(handle, commandsLen, commandsPtr)
    member _.OnSubmittedWorkDone(signalValue : uint64, callback : QueueWorkDoneCallback) : unit =
        let _callbackDel = WebGPU.Raw.QueueWorkDoneCallback(fun status userdata ->
            let _status = status
            let _userdata = userdata
            callback.Invoke(_status, _userdata)
        )
        let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
        WebGPU.Raw.WebGPU.QueueOnSubmittedWorkDone(handle, signalValue, _callbackPtr, _callbackUserData)
    member _.WriteBuffer(buffer : Buffer, bufferOffset : uint64, data : nativeint, size : unativeint) : unit =
        WebGPU.Raw.WebGPU.QueueWriteBuffer(handle, buffer.Handle, bufferOffset, data, size)
    member _.WriteTexture(destination : ImageCopyTexture, data : nativeint, dataSize : unativeint, dataLayout : TextureDataLayout, writeSize : Extent3D) : unit =
        destination.Pin(fun _destinationPtr ->
            dataLayout.Pin(fun _dataLayoutPtr ->
                writeSize.Pin(fun _writeSizePtr ->
                    WebGPU.Raw.WebGPU.QueueWriteTexture(handle, _destinationPtr, data, dataSize, _dataLayoutPtr, _writeSizePtr)
                )
            )
        )
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.QueueSetLabel(handle, _labelPtr)
type QueueDescriptor = 
    {
        Label : string
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.QueueDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        let mutable value =
            new WebGPU.Raw.QueueDescriptor(
                _labelPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.QueueDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.QueueDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
        }
type QueueWorkDoneCallback = delegate of status : QueueWorkDoneStatus * userdata : nativeint -> unit
type RenderBundle internal(handle : nativeint) =
    member x.Handle = handle
type RenderBundleEncoder internal(handle : nativeint) =
    member x.Handle = handle
    member _.SetPipeline(pipeline : RenderPipeline) : unit =
        WebGPU.Raw.WebGPU.RenderBundleEncoderSetPipeline(handle, pipeline.Handle)
    member _.SetBindGroup(groupIndex : int, group : BindGroup, dynamicOffsets : array<uint32>) : unit =
        use dynamicOffsetsPtr = fixed (dynamicOffsets)
        let dynamicOffsetsLen = uint32 dynamicOffsets.Length
        WebGPU.Raw.WebGPU.RenderBundleEncoderSetBindGroup(handle, uint32(groupIndex), group.Handle, dynamicOffsetsLen, dynamicOffsetsPtr)
    member _.Draw(vertexCount : int, instanceCount : int, firstVertex : int, firstInstance : int) : unit =
        WebGPU.Raw.WebGPU.RenderBundleEncoderDraw(handle, uint32(vertexCount), uint32(instanceCount), uint32(firstVertex), uint32(firstInstance))
    member _.DrawIndexed(indexCount : int, instanceCount : int, firstIndex : int, baseVertex : int, firstInstance : int) : unit =
        WebGPU.Raw.WebGPU.RenderBundleEncoderDrawIndexed(handle, uint32(indexCount), uint32(instanceCount), uint32(firstIndex), baseVertex, uint32(firstInstance))
    member _.DrawIndirect(indirectBuffer : Buffer, indirectOffset : uint64) : unit =
        WebGPU.Raw.WebGPU.RenderBundleEncoderDrawIndirect(handle, indirectBuffer.Handle, indirectOffset)
    member _.DrawIndexedIndirect(indirectBuffer : Buffer, indirectOffset : uint64) : unit =
        WebGPU.Raw.WebGPU.RenderBundleEncoderDrawIndexedIndirect(handle, indirectBuffer.Handle, indirectOffset)
    member _.InsertDebugMarker(markerLabel : string) : unit =
        use _markerLabelPtr = fixed (Encoding.UTF8.GetBytes(markerLabel))
        WebGPU.Raw.WebGPU.RenderBundleEncoderInsertDebugMarker(handle, _markerLabelPtr)
    member _.PopDebugGroup() : unit =
        WebGPU.Raw.WebGPU.RenderBundleEncoderPopDebugGroup(handle)
    member _.PushDebugGroup(groupLabel : string) : unit =
        use _groupLabelPtr = fixed (Encoding.UTF8.GetBytes(groupLabel))
        WebGPU.Raw.WebGPU.RenderBundleEncoderPushDebugGroup(handle, _groupLabelPtr)
    member _.SetVertexBuffer(slot : int, buffer : Buffer, offset : uint64, size : uint64) : unit =
        WebGPU.Raw.WebGPU.RenderBundleEncoderSetVertexBuffer(handle, uint32(slot), buffer.Handle, offset, size)
    member _.SetIndexBuffer(buffer : Buffer, format : IndexFormat, offset : uint64, size : uint64) : unit =
        WebGPU.Raw.WebGPU.RenderBundleEncoderSetIndexBuffer(handle, buffer.Handle, format, offset, size)
    member _.Finish(descriptor : RenderBundleDescriptor) : RenderBundle =
        descriptor.Pin(fun _descriptorPtr ->
            new RenderBundle(WebGPU.Raw.WebGPU.RenderBundleEncoderFinish(handle, _descriptorPtr))
        )
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.RenderBundleEncoderSetLabel(handle, _labelPtr)
type RenderBundleDescriptor = 
    {
        Label : string
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.RenderBundleDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        let mutable value =
            new WebGPU.Raw.RenderBundleDescriptor(
                _labelPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderBundleDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.RenderBundleDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
        }
type RenderBundleEncoderDescriptor = 
    {
        Label : string
        ColorFormats : array<TextureFormat>
        DepthStencilFormat : TextureFormat
        SampleCount : int
        DepthReadOnly : bool
        StencilReadOnly : bool
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.RenderBundleEncoderDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        use colorFormatsPtr = fixed (this.ColorFormats)
        let colorFormatsLen = uint32 this.ColorFormats.Length
        let mutable value =
            new WebGPU.Raw.RenderBundleEncoderDescriptor(
                _labelPtr,
                colorFormatsLen,
                colorFormatsPtr,
                this.DepthStencilFormat,
                uint32(this.SampleCount),
                (if this.DepthReadOnly then 1 else 0),
                (if this.StencilReadOnly then 1 else 0)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderBundleEncoderDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.RenderBundleEncoderDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
            ColorFormats = let ptr = backend.ColorFormats in Array.init (int backend.ColorFormatsCount) (fun i -> NativePtr.get ptr i)
            DepthStencilFormat = backend.DepthStencilFormat
            SampleCount = int(backend.SampleCount)
            DepthReadOnly = (backend.DepthReadOnly <> 0)
            StencilReadOnly = (backend.StencilReadOnly <> 0)
        }
type RenderPassColorAttachment = 
    {
        View : TextureView
        ResolveTarget : TextureView
        LoadOp : LoadOp
        StoreOp : StoreOp
        ClearValue : Color
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.RenderPassColorAttachment> -> 'r) : 'r = 
        this.ClearValue.Pin(fun _clearValuePtr ->
            let mutable value =
                new WebGPU.Raw.RenderPassColorAttachment(
                    this.View.Handle,
                    this.ResolveTarget.Handle,
                    this.LoadOp,
                    this.StoreOp,
                    NativePtr.read _clearValuePtr
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPassColorAttachment> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.RenderPassColorAttachment>) = 
        {
            View = new TextureView(backend.View)
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
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.RenderPassDepthStencilAttachment> -> 'r) : 'r = 
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
    static member Read(backend : byref<WebGPU.Raw.RenderPassDepthStencilAttachment>) = 
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
        Label : string
        ColorAttachments : array<RenderPassColorAttachment>
        DepthStencilAttachment : RenderPassDepthStencilAttachment
        OcclusionQuerySet : QuerySet
        TimestampWrites : array<RenderPassTimestampWrite>
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.RenderPassDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        WebGPU.Raw.Pinnable.pinArray this.ColorAttachments (fun colorAttachmentsPtr ->
            let colorAttachmentsLen = uint32 this.ColorAttachments.Length
            this.DepthStencilAttachment.Pin(fun _depthStencilAttachmentPtr ->
                WebGPU.Raw.Pinnable.pinArray this.TimestampWrites (fun timestampWritesPtr ->
                    let timestampWritesLen = uint32 this.TimestampWrites.Length
                    let mutable value =
                        new WebGPU.Raw.RenderPassDescriptor(
                            _labelPtr,
                            colorAttachmentsLen,
                            colorAttachmentsPtr,
                            _depthStencilAttachmentPtr,
                            this.OcclusionQuerySet.Handle,
                            timestampWritesLen,
                            timestampWritesPtr
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPassDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.RenderPassDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
            ColorAttachments = let ptr = backend.ColorAttachments in Array.init (int backend.ColorAttachmentCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in RenderPassColorAttachment.Read(&r))
            DepthStencilAttachment = let m = NativePtr.toByRef backend.DepthStencilAttachment in RenderPassDepthStencilAttachment.Read(&m)
            OcclusionQuerySet = new QuerySet(backend.OcclusionQuerySet)
            TimestampWrites = let ptr = backend.TimestampWrites in Array.init (int backend.TimestampWriteCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in RenderPassTimestampWrite.Read(&r))
        }
type RenderPassDescriptorMaxDrawCount = 
    {
        MaxDrawCount : uint64
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.RenderPassDescriptorMaxDrawCount> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.RenderPassDescriptorMaxDrawCount(
                this.MaxDrawCount
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPassDescriptorMaxDrawCount> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.RenderPassDescriptorMaxDrawCount>) = 
        {
            MaxDrawCount = backend.MaxDrawCount
        }
type RenderPassEncoder internal(handle : nativeint) =
    member x.Handle = handle
    member _.SetPipeline(pipeline : RenderPipeline) : unit =
        WebGPU.Raw.WebGPU.RenderPassEncoderSetPipeline(handle, pipeline.Handle)
    member _.SetBindGroup(groupIndex : int, group : BindGroup, dynamicOffsets : array<uint32>) : unit =
        use dynamicOffsetsPtr = fixed (dynamicOffsets)
        let dynamicOffsetsLen = uint32 dynamicOffsets.Length
        WebGPU.Raw.WebGPU.RenderPassEncoderSetBindGroup(handle, uint32(groupIndex), group.Handle, dynamicOffsetsLen, dynamicOffsetsPtr)
    member _.Draw(vertexCount : int, instanceCount : int, firstVertex : int, firstInstance : int) : unit =
        WebGPU.Raw.WebGPU.RenderPassEncoderDraw(handle, uint32(vertexCount), uint32(instanceCount), uint32(firstVertex), uint32(firstInstance))
    member _.DrawIndexed(indexCount : int, instanceCount : int, firstIndex : int, baseVertex : int, firstInstance : int) : unit =
        WebGPU.Raw.WebGPU.RenderPassEncoderDrawIndexed(handle, uint32(indexCount), uint32(instanceCount), uint32(firstIndex), baseVertex, uint32(firstInstance))
    member _.DrawIndirect(indirectBuffer : Buffer, indirectOffset : uint64) : unit =
        WebGPU.Raw.WebGPU.RenderPassEncoderDrawIndirect(handle, indirectBuffer.Handle, indirectOffset)
    member _.DrawIndexedIndirect(indirectBuffer : Buffer, indirectOffset : uint64) : unit =
        WebGPU.Raw.WebGPU.RenderPassEncoderDrawIndexedIndirect(handle, indirectBuffer.Handle, indirectOffset)
    member _.ExecuteBundles(bundles : array<RenderBundle>) : unit =
        let bundlesHandles = bundles |> Array.map (fun a -> a.Handle)
        use bundlesPtr = fixed (bundlesHandles)
        let bundlesLen = uint32 bundles.Length
        WebGPU.Raw.WebGPU.RenderPassEncoderExecuteBundles(handle, bundlesLen, bundlesPtr)
    member _.InsertDebugMarker(markerLabel : string) : unit =
        use _markerLabelPtr = fixed (Encoding.UTF8.GetBytes(markerLabel))
        WebGPU.Raw.WebGPU.RenderPassEncoderInsertDebugMarker(handle, _markerLabelPtr)
    member _.PopDebugGroup() : unit =
        WebGPU.Raw.WebGPU.RenderPassEncoderPopDebugGroup(handle)
    member _.PushDebugGroup(groupLabel : string) : unit =
        use _groupLabelPtr = fixed (Encoding.UTF8.GetBytes(groupLabel))
        WebGPU.Raw.WebGPU.RenderPassEncoderPushDebugGroup(handle, _groupLabelPtr)
    member _.SetStencilReference(reference : int) : unit =
        WebGPU.Raw.WebGPU.RenderPassEncoderSetStencilReference(handle, uint32(reference))
    member _.SetBlendConstant(color : Color) : unit =
        color.Pin(fun _colorPtr ->
            WebGPU.Raw.WebGPU.RenderPassEncoderSetBlendConstant(handle, _colorPtr)
        )
    member _.SetViewport(x : float32, y : float32, width : float32, height : float32, minDepth : float32, maxDepth : float32) : unit =
        WebGPU.Raw.WebGPU.RenderPassEncoderSetViewport(handle, x, y, width, height, minDepth, maxDepth)
    member _.SetScissorRect(x : int, y : int, width : int, height : int) : unit =
        WebGPU.Raw.WebGPU.RenderPassEncoderSetScissorRect(handle, uint32(x), uint32(y), uint32(width), uint32(height))
    member _.SetVertexBuffer(slot : int, buffer : Buffer, offset : uint64, size : uint64) : unit =
        WebGPU.Raw.WebGPU.RenderPassEncoderSetVertexBuffer(handle, uint32(slot), buffer.Handle, offset, size)
    member _.SetIndexBuffer(buffer : Buffer, format : IndexFormat, offset : uint64, size : uint64) : unit =
        WebGPU.Raw.WebGPU.RenderPassEncoderSetIndexBuffer(handle, buffer.Handle, format, offset, size)
    member _.BeginOcclusionQuery(queryIndex : int) : unit =
        WebGPU.Raw.WebGPU.RenderPassEncoderBeginOcclusionQuery(handle, uint32(queryIndex))
    member _.BeginPipelineStatisticsQuery(querySet : QuerySet, queryIndex : int) : unit =
        WebGPU.Raw.WebGPU.RenderPassEncoderBeginPipelineStatisticsQuery(handle, querySet.Handle, uint32(queryIndex))
    member _.EndOcclusionQuery() : unit =
        WebGPU.Raw.WebGPU.RenderPassEncoderEndOcclusionQuery(handle)
    member _.WriteTimestamp(querySet : QuerySet, queryIndex : int) : unit =
        WebGPU.Raw.WebGPU.RenderPassEncoderWriteTimestamp(handle, querySet.Handle, uint32(queryIndex))
    member _.End() : unit =
        WebGPU.Raw.WebGPU.RenderPassEncoderEnd(handle)
    member _.EndPass() : unit =
        WebGPU.Raw.WebGPU.RenderPassEncoderEndPass(handle)
    member _.EndPipelineStatisticsQuery() : unit =
        WebGPU.Raw.WebGPU.RenderPassEncoderEndPipelineStatisticsQuery(handle)
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.RenderPassEncoderSetLabel(handle, _labelPtr)
type RenderPassTimestampWrite = 
    {
        QuerySet : QuerySet
        QueryIndex : int
        Location : RenderPassTimestampLocation
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.RenderPassTimestampWrite> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.RenderPassTimestampWrite(
                this.QuerySet.Handle,
                uint32(this.QueryIndex),
                this.Location
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPassTimestampWrite> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.RenderPassTimestampWrite>) = 
        {
            QuerySet = new QuerySet(backend.QuerySet)
            QueryIndex = int(backend.QueryIndex)
            Location = backend.Location
        }
type RenderPipeline internal(handle : nativeint) =
    member x.Handle = handle
    member _.GetBindGroupLayout(groupIndex : int) : BindGroupLayout =
        new BindGroupLayout(WebGPU.Raw.WebGPU.RenderPipelineGetBindGroupLayout(handle, uint32(groupIndex)))
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.RenderPipelineSetLabel(handle, _labelPtr)
type RequestDeviceCallback = delegate of status : RequestDeviceStatus * device : Device * message : string * userdata : nativeint -> unit
type VertexState = 
    {
        Module : ShaderModule
        EntryPoint : string
        Constants : array<ConstantEntry>
        Buffers : array<VertexBufferLayout>
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.VertexState> -> 'r) : 'r = 
        use _entryPointPtr = fixed (Encoding.UTF8.GetBytes(this.EntryPoint))
        WebGPU.Raw.Pinnable.pinArray this.Constants (fun constantsPtr ->
            let constantsLen = uint32 this.Constants.Length
            WebGPU.Raw.Pinnable.pinArray this.Buffers (fun buffersPtr ->
                let buffersLen = uint32 this.Buffers.Length
                let mutable value =
                    new WebGPU.Raw.VertexState(
                        this.Module.Handle,
                        _entryPointPtr,
                        constantsLen,
                        constantsPtr,
                        buffersLen,
                        buffersPtr
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.VertexState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.VertexState>) = 
        {
            Module = new ShaderModule(backend.Module)
            EntryPoint = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.EntryPoint)
            Constants = let ptr = backend.Constants in Array.init (int backend.ConstantCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in ConstantEntry.Read(&r))
            Buffers = let ptr = backend.Buffers in Array.init (int backend.BufferCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in VertexBufferLayout.Read(&r))
        }
type PrimitiveState = 
    {
        Topology : PrimitiveTopology
        StripIndexFormat : IndexFormat
        FrontFace : FrontFace
        CullMode : CullMode
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.PrimitiveState> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.PrimitiveState(
                this.Topology,
                this.StripIndexFormat,
                this.FrontFace,
                this.CullMode
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.PrimitiveState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.PrimitiveState>) = 
        {
            Topology = backend.Topology
            StripIndexFormat = backend.StripIndexFormat
            FrontFace = backend.FrontFace
            CullMode = backend.CullMode
        }
type PrimitiveDepthClipControl = 
    {
        UnclippedDepth : bool
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.PrimitiveDepthClipControl> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.PrimitiveDepthClipControl(
                (if this.UnclippedDepth then 1 else 0)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.PrimitiveDepthClipControl> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.PrimitiveDepthClipControl>) = 
        {
            UnclippedDepth = (backend.UnclippedDepth <> 0)
        }
type DepthStencilState = 
    {
        Format : TextureFormat
        DepthWriteEnabled : bool
        DepthCompare : CompareFunction
        StencilFront : StencilFaceState
        StencilBack : StencilFaceState
        StencilReadMask : int
        StencilWriteMask : int
        DepthBias : int
        DepthBiasSlopeScale : float32
        DepthBiasClamp : float32
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.DepthStencilState> -> 'r) : 'r = 
        this.StencilFront.Pin(fun _stencilFrontPtr ->
            this.StencilBack.Pin(fun _stencilBackPtr ->
                let mutable value =
                    new WebGPU.Raw.DepthStencilState(
                        this.Format,
                        (if this.DepthWriteEnabled then 1 else 0),
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
    static member Read(backend : byref<WebGPU.Raw.DepthStencilState>) = 
        {
            Format = backend.Format
            DepthWriteEnabled = (backend.DepthWriteEnabled <> 0)
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
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.MultisampleState> -> 'r) : 'r = 
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
    static member Read(backend : byref<WebGPU.Raw.MultisampleState>) = 
        {
            Count = int(backend.Count)
            Mask = int(backend.Mask)
            AlphaToCoverageEnabled = (backend.AlphaToCoverageEnabled <> 0)
        }
type FragmentState = 
    {
        Module : ShaderModule
        EntryPoint : string
        Constants : array<ConstantEntry>
        Targets : array<ColorTargetState>
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.FragmentState> -> 'r) : 'r = 
        use _entryPointPtr = fixed (Encoding.UTF8.GetBytes(this.EntryPoint))
        WebGPU.Raw.Pinnable.pinArray this.Constants (fun constantsPtr ->
            let constantsLen = uint32 this.Constants.Length
            WebGPU.Raw.Pinnable.pinArray this.Targets (fun targetsPtr ->
                let targetsLen = uint32 this.Targets.Length
                let mutable value =
                    new WebGPU.Raw.FragmentState(
                        this.Module.Handle,
                        _entryPointPtr,
                        constantsLen,
                        constantsPtr,
                        targetsLen,
                        targetsPtr
                    )
                use ptr = fixed &value
                action ptr
            )
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.FragmentState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.FragmentState>) = 
        {
            Module = new ShaderModule(backend.Module)
            EntryPoint = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.EntryPoint)
            Constants = let ptr = backend.Constants in Array.init (int backend.ConstantCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in ConstantEntry.Read(&r))
            Targets = let ptr = backend.Targets in Array.init (int backend.TargetCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in ColorTargetState.Read(&r))
        }
type ColorTargetState = 
    {
        Format : TextureFormat
        Blend : BlendState
        WriteMask : ColorWriteMask
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.ColorTargetState> -> 'r) : 'r = 
        this.Blend.Pin(fun _blendPtr ->
            let mutable value =
                new WebGPU.Raw.ColorTargetState(
                    this.Format,
                    _blendPtr,
                    this.WriteMask
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ColorTargetState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.ColorTargetState>) = 
        {
            Format = backend.Format
            Blend = let m = NativePtr.toByRef backend.Blend in BlendState.Read(&m)
            WriteMask = backend.WriteMask
        }
type BlendState = 
    {
        Color : BlendComponent
        Alpha : BlendComponent
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.BlendState> -> 'r) : 'r = 
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
    static member Read(backend : byref<WebGPU.Raw.BlendState>) = 
        {
            Color = BlendComponent.Read(&backend.Color)
            Alpha = BlendComponent.Read(&backend.Alpha)
        }
type RenderPipelineDescriptor = 
    {
        Label : string
        Layout : PipelineLayout
        Vertex : VertexState
        Primitive : PrimitiveState
        DepthStencil : DepthStencilState
        Multisample : MultisampleState
        Fragment : FragmentState
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.RenderPipelineDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        this.Vertex.Pin(fun _vertexPtr ->
            this.Primitive.Pin(fun _primitivePtr ->
                this.DepthStencil.Pin(fun _depthStencilPtr ->
                    this.Multisample.Pin(fun _multisamplePtr ->
                        this.Fragment.Pin(fun _fragmentPtr ->
                            let mutable value =
                                new WebGPU.Raw.RenderPipelineDescriptor(
                                    _labelPtr,
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
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPipelineDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.RenderPipelineDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
            Layout = new PipelineLayout(backend.Layout)
            Vertex = VertexState.Read(&backend.Vertex)
            Primitive = PrimitiveState.Read(&backend.Primitive)
            DepthStencil = let m = NativePtr.toByRef backend.DepthStencil in DepthStencilState.Read(&m)
            Multisample = MultisampleState.Read(&backend.Multisample)
            Fragment = let m = NativePtr.toByRef backend.Fragment in FragmentState.Read(&m)
        }
type Sampler internal(handle : nativeint) =
    member x.Handle = handle
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.SamplerSetLabel(handle, _labelPtr)
type SamplerDescriptor = 
    {
        Label : string
        AddressModeU : AddressMode
        AddressModeV : AddressMode
        AddressModeW : AddressMode
        MagFilter : FilterMode
        MinFilter : FilterMode
        MipmapFilter : FilterMode
        LodMinClamp : float32
        LodMaxClamp : float32
        Compare : CompareFunction
        MaxAnisotropy : uint16
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.SamplerDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        let mutable value =
            new WebGPU.Raw.SamplerDescriptor(
                _labelPtr,
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
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SamplerDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.SamplerDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
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
    member _.GetCompilationInfo(callback : CompilationInfoCallback) : unit =
        let _callbackDel = WebGPU.Raw.CompilationInfoCallback(fun status compilationInfo userdata ->
            let _status = status
            let _compilationInfo = let m = NativePtr.toByRef compilationInfo in CompilationInfo.Read(&m)
            let _userdata = userdata
            callback.Invoke(_status, _compilationInfo, _userdata)
        )
        let struct(_callbackPtr, _callbackUserData) = WebGPU.Raw.WebGPUCallbacks.Register(_callbackDel)
        WebGPU.Raw.WebGPU.ShaderModuleGetCompilationInfo(handle, _callbackPtr, _callbackUserData)
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.ShaderModuleSetLabel(handle, _labelPtr)
type ShaderModuleDescriptor = 
    {
        Label : string
        Hints : array<ShaderModuleCompilationHint>
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.ShaderModuleDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        WebGPU.Raw.Pinnable.pinArray this.Hints (fun hintsPtr ->
            let hintsLen = uint32 this.Hints.Length
            let mutable value =
                new WebGPU.Raw.ShaderModuleDescriptor(
                    _labelPtr,
                    hintsLen,
                    hintsPtr
                )
            use ptr = fixed &value
            action ptr
        )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ShaderModuleDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.ShaderModuleDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
            Hints = let ptr = backend.Hints in Array.init (int backend.HintCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in ShaderModuleCompilationHint.Read(&r))
        }
type ShaderModuleCompilationHint = 
    {
        EntryPoint : string
        Layout : PipelineLayout
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.ShaderModuleCompilationHint> -> 'r) : 'r = 
        use _entryPointPtr = fixed (Encoding.UTF8.GetBytes(this.EntryPoint))
        let mutable value =
            new WebGPU.Raw.ShaderModuleCompilationHint(
                _entryPointPtr,
                this.Layout.Handle
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ShaderModuleCompilationHint> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.ShaderModuleCompilationHint>) = 
        {
            EntryPoint = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.EntryPoint)
            Layout = new PipelineLayout(backend.Layout)
        }
type ShaderModuleSPIRVDescriptor = 
    {
        Code : array<uint32>
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.ShaderModuleSPIRVDescriptor> -> 'r) : 'r = 
        use codePtr = fixed (this.Code)
        let codeLen = uint32 this.Code.Length
        let mutable value =
            new WebGPU.Raw.ShaderModuleSPIRVDescriptor(
                codeLen,
                codePtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ShaderModuleSPIRVDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.ShaderModuleSPIRVDescriptor>) = 
        {
            Code = let ptr = backend.Code in Array.init (int backend.CodeSize) (fun i -> NativePtr.get ptr i)
        }
type ShaderModuleWGSLDescriptor = 
    {
        Source : string
        Code : string
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.ShaderModuleWGSLDescriptor> -> 'r) : 'r = 
        use _sourcePtr = fixed (Encoding.UTF8.GetBytes(this.Source))
        use _codePtr = fixed (Encoding.UTF8.GetBytes(this.Code))
        let mutable value =
            new WebGPU.Raw.ShaderModuleWGSLDescriptor(
                _sourcePtr,
                _codePtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ShaderModuleWGSLDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.ShaderModuleWGSLDescriptor>) = 
        {
            Source = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Source)
            Code = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Code)
        }
type DawnShaderModuleSPIRVOptionsDescriptor = 
    {
        AllowNonUniformDerivatives : bool
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.DawnShaderModuleSPIRVOptionsDescriptor> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.DawnShaderModuleSPIRVOptionsDescriptor(
                (if this.AllowNonUniformDerivatives then 1 else 0)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnShaderModuleSPIRVOptionsDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.DawnShaderModuleSPIRVOptionsDescriptor>) = 
        {
            AllowNonUniformDerivatives = (backend.AllowNonUniformDerivatives <> 0)
        }
type StencilFaceState = 
    {
        Compare : CompareFunction
        FailOp : StencilOperation
        DepthFailOp : StencilOperation
        PassOp : StencilOperation
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.StencilFaceState> -> 'r) : 'r = 
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
    static member Read(backend : byref<WebGPU.Raw.StencilFaceState>) = 
        {
            Compare = backend.Compare
            FailOp = backend.FailOp
            DepthFailOp = backend.DepthFailOp
            PassOp = backend.PassOp
        }
type Surface internal(handle : nativeint) =
    member x.Handle = handle
    member _.GetPreferredFormat(adapter : Adapter) : TextureFormat =
        WebGPU.Raw.WebGPU.SurfaceGetPreferredFormat(handle, adapter.Handle)
type SurfaceDescriptor = 
    {
        Label : string
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.SurfaceDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        let mutable value =
            new WebGPU.Raw.SurfaceDescriptor(
                _labelPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.SurfaceDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
        }
type SurfaceDescriptorFromAndroidNativeWindow = 
    {
        Window : nativeint
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.SurfaceDescriptorFromAndroidNativeWindow> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.SurfaceDescriptorFromAndroidNativeWindow(
                this.Window
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceDescriptorFromAndroidNativeWindow> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.SurfaceDescriptorFromAndroidNativeWindow>) = 
        {
            Window = backend.Window
        }
type SurfaceDescriptorFromCanvasHTMLSelector = 
    {
        Selector : string
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.SurfaceDescriptorFromCanvasHTMLSelector> -> 'r) : 'r = 
        use _selectorPtr = fixed (Encoding.UTF8.GetBytes(this.Selector))
        let mutable value =
            new WebGPU.Raw.SurfaceDescriptorFromCanvasHTMLSelector(
                _selectorPtr
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceDescriptorFromCanvasHTMLSelector> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.SurfaceDescriptorFromCanvasHTMLSelector>) = 
        {
            Selector = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Selector)
        }
type SurfaceDescriptorFromMetalLayer = 
    {
        Layer : nativeint
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.SurfaceDescriptorFromMetalLayer> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.SurfaceDescriptorFromMetalLayer(
                this.Layer
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceDescriptorFromMetalLayer> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.SurfaceDescriptorFromMetalLayer>) = 
        {
            Layer = backend.Layer
        }
type SurfaceDescriptorFromWindowsHWND = 
    {
        Hinstance : nativeint
        Hwnd : nativeint
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.SurfaceDescriptorFromWindowsHWND> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.SurfaceDescriptorFromWindowsHWND(
                this.Hinstance,
                this.Hwnd
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceDescriptorFromWindowsHWND> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.SurfaceDescriptorFromWindowsHWND>) = 
        {
            Hinstance = backend.Hinstance
            Hwnd = backend.Hwnd
        }
type SurfaceDescriptorFromXcbWindow = 
    {
        Connection : nativeint
        Window : int
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.SurfaceDescriptorFromXcbWindow> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.SurfaceDescriptorFromXcbWindow(
                this.Connection,
                uint32(this.Window)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceDescriptorFromXcbWindow> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.SurfaceDescriptorFromXcbWindow>) = 
        {
            Connection = backend.Connection
            Window = int(backend.Window)
        }
type SurfaceDescriptorFromXlibWindow = 
    {
        Display : nativeint
        Window : int
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.SurfaceDescriptorFromXlibWindow> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.SurfaceDescriptorFromXlibWindow(
                this.Display,
                uint32(this.Window)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceDescriptorFromXlibWindow> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.SurfaceDescriptorFromXlibWindow>) = 
        {
            Display = backend.Display
            Window = int(backend.Window)
        }
type SurfaceDescriptorFromWaylandSurface = 
    {
        Display : nativeint
        Surface : nativeint
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.SurfaceDescriptorFromWaylandSurface> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.SurfaceDescriptorFromWaylandSurface(
                this.Display,
                this.Surface
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceDescriptorFromWaylandSurface> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.SurfaceDescriptorFromWaylandSurface>) = 
        {
            Display = backend.Display
            Surface = backend.Surface
        }
type SurfaceDescriptorFromWindowsCoreWindow = 
    {
        CoreWindow : nativeint
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.SurfaceDescriptorFromWindowsCoreWindow> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.SurfaceDescriptorFromWindowsCoreWindow(
                this.CoreWindow
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceDescriptorFromWindowsCoreWindow> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.SurfaceDescriptorFromWindowsCoreWindow>) = 
        {
            CoreWindow = backend.CoreWindow
        }
type SurfaceDescriptorFromWindowsSwapChainPanel = 
    {
        SwapChainPanel : nativeint
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.SurfaceDescriptorFromWindowsSwapChainPanel> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.SurfaceDescriptorFromWindowsSwapChainPanel(
                this.SwapChainPanel
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceDescriptorFromWindowsSwapChainPanel> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.SurfaceDescriptorFromWindowsSwapChainPanel>) = 
        {
            SwapChainPanel = backend.SwapChainPanel
        }
type SwapChain internal(handle : nativeint) =
    member x.Handle = handle
    member _.Configure(format : TextureFormat, allowedUsage : TextureUsage, width : int, height : int) : unit =
        WebGPU.Raw.WebGPU.SwapChainConfigure(handle, format, allowedUsage, uint32(width), uint32(height))
    member _.GetCurrentTextureView() : TextureView =
        new TextureView(WebGPU.Raw.WebGPU.SwapChainGetCurrentTextureView(handle))
    member _.Present() : unit =
        WebGPU.Raw.WebGPU.SwapChainPresent(handle)
type SwapChainDescriptor = 
    {
        Label : string
        Usage : TextureUsage
        Format : TextureFormat
        Width : int
        Height : int
        PresentMode : PresentMode
        Implementation : uint64
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.SwapChainDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        let mutable value =
            new WebGPU.Raw.SwapChainDescriptor(
                _labelPtr,
                this.Usage,
                this.Format,
                uint32(this.Width),
                uint32(this.Height),
                this.PresentMode,
                this.Implementation
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SwapChainDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.SwapChainDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
            Usage = backend.Usage
            Format = backend.Format
            Width = int(backend.Width)
            Height = int(backend.Height)
            PresentMode = backend.PresentMode
            Implementation = backend.Implementation
        }
type Texture internal(handle : nativeint) =
    member x.Handle = handle
    member _.CreateView(descriptor : TextureViewDescriptor) : TextureView =
        descriptor.Pin(fun _descriptorPtr ->
            new TextureView(WebGPU.Raw.WebGPU.TextureCreateView(handle, _descriptorPtr))
        )
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.TextureSetLabel(handle, _labelPtr)
    member _.GetWidth() : int =
        int(WebGPU.Raw.WebGPU.TextureGetWidth(handle))
    member _.GetHeight() : int =
        int(WebGPU.Raw.WebGPU.TextureGetHeight(handle))
    member _.GetDepthOrArrayLayers() : int =
        int(WebGPU.Raw.WebGPU.TextureGetDepthOrArrayLayers(handle))
    member _.GetMipLevelCount() : int =
        int(WebGPU.Raw.WebGPU.TextureGetMipLevelCount(handle))
    member _.GetSampleCount() : int =
        int(WebGPU.Raw.WebGPU.TextureGetSampleCount(handle))
    member _.GetDimension() : TextureDimension =
        WebGPU.Raw.WebGPU.TextureGetDimension(handle)
    member _.GetFormat() : TextureFormat =
        WebGPU.Raw.WebGPU.TextureGetFormat(handle)
    member _.GetUsage() : TextureUsage =
        WebGPU.Raw.WebGPU.TextureGetUsage(handle)
    member _.Destroy() : unit =
        WebGPU.Raw.WebGPU.TextureDestroy(handle)
    member x.Dispose() = x.Destroy()
    interface System.IDisposable with
        member x.Dispose() = x.Dispose()
type TextureDataLayout = 
    {
        Offset : uint64
        BytesPerRow : int
        RowsPerImage : int
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.TextureDataLayout> -> 'r) : 'r = 
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
    static member Read(backend : byref<WebGPU.Raw.TextureDataLayout>) = 
        {
            Offset = backend.Offset
            BytesPerRow = int(backend.BytesPerRow)
            RowsPerImage = int(backend.RowsPerImage)
        }
type TextureDescriptor = 
    {
        Label : string
        Usage : TextureUsage
        Dimension : TextureDimension
        Size : Extent3D
        Format : TextureFormat
        MipLevelCount : int
        SampleCount : int
        ViewFormats : array<TextureFormat>
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.TextureDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        this.Size.Pin(fun _sizePtr ->
            use viewFormatsPtr = fixed (this.ViewFormats)
            let viewFormatsLen = uint32 this.ViewFormats.Length
            let mutable value =
                new WebGPU.Raw.TextureDescriptor(
                    _labelPtr,
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
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.TextureDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.TextureDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
            Usage = backend.Usage
            Dimension = backend.Dimension
            Size = Extent3D.Read(&backend.Size)
            Format = backend.Format
            MipLevelCount = int(backend.MipLevelCount)
            SampleCount = int(backend.SampleCount)
            ViewFormats = let ptr = backend.ViewFormats in Array.init (int backend.ViewFormatCount) (fun i -> NativePtr.get ptr i)
        }
type TextureViewDescriptor = 
    {
        Label : string
        Format : TextureFormat
        Dimension : TextureViewDimension
        BaseMipLevel : int
        MipLevelCount : int
        BaseArrayLayer : int
        ArrayLayerCount : int
        Aspect : TextureAspect
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.TextureViewDescriptor> -> 'r) : 'r = 
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(this.Label))
        let mutable value =
            new WebGPU.Raw.TextureViewDescriptor(
                _labelPtr,
                this.Format,
                this.Dimension,
                uint32(this.BaseMipLevel),
                uint32(this.MipLevelCount),
                uint32(this.BaseArrayLayer),
                uint32(this.ArrayLayerCount),
                this.Aspect
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.TextureViewDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.TextureViewDescriptor>) = 
        {
            Label = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Label)
            Format = backend.Format
            Dimension = backend.Dimension
            BaseMipLevel = int(backend.BaseMipLevel)
            MipLevelCount = int(backend.MipLevelCount)
            BaseArrayLayer = int(backend.BaseArrayLayer)
            ArrayLayerCount = int(backend.ArrayLayerCount)
            Aspect = backend.Aspect
        }
type TextureView internal(handle : nativeint) =
    member x.Handle = handle
    member _.SetLabel(label : string) : unit =
        use _labelPtr = fixed (Encoding.UTF8.GetBytes(label))
        WebGPU.Raw.WebGPU.TextureViewSetLabel(handle, _labelPtr)
type DawnTextureInternalUsageDescriptor = 
    {
        InternalUsage : TextureUsage
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.DawnTextureInternalUsageDescriptor> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.DawnTextureInternalUsageDescriptor(
                this.InternalUsage
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnTextureInternalUsageDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.DawnTextureInternalUsageDescriptor>) = 
        {
            InternalUsage = backend.InternalUsage
        }
type DawnEncoderInternalUsageDescriptor = 
    {
        UseInternalUsages : bool
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.DawnEncoderInternalUsageDescriptor> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.DawnEncoderInternalUsageDescriptor(
                (if this.UseInternalUsages then 1 else 0)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnEncoderInternalUsageDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.DawnEncoderInternalUsageDescriptor>) = 
        {
            UseInternalUsages = (backend.UseInternalUsages <> 0)
        }
type DawnAdapterPropertiesPowerPreference = 
    {
        PowerPreference : PowerPreference
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.DawnAdapterPropertiesPowerPreference> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.DawnAdapterPropertiesPowerPreference(
                this.PowerPreference
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnAdapterPropertiesPowerPreference> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.DawnAdapterPropertiesPowerPreference>) = 
        {
            PowerPreference = backend.PowerPreference
        }
type DawnBufferDescriptorErrorInfoFromWireClient = 
    {
        OutOfMemory : bool
    }
    member inline this.Pin<'r>([<InlineIfLambda>] action : nativeptr<WebGPU.Raw.DawnBufferDescriptorErrorInfoFromWireClient> -> 'r) : 'r = 
        let mutable value =
            new WebGPU.Raw.DawnBufferDescriptorErrorInfoFromWireClient(
                (if this.OutOfMemory then 1 else 0)
            )
        use ptr = fixed &value
        action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnBufferDescriptorErrorInfoFromWireClient> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : byref<WebGPU.Raw.DawnBufferDescriptorErrorInfoFromWireClient>) = 
        {
            OutOfMemory = (backend.OutOfMemory <> 0)
        }
