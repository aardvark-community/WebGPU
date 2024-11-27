namespace rec WebGPU.Raw
open System.Collections.Generic
open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop
open WebGPU
#nowarn "9"
[<Struct; StructLayout(LayoutKind.Sequential)>]
type INTERNAL__HAVE_EMDAWNWEBGPU_HEADER = 
    struct
        val mutable public Unused : int
        new(unused : int) = { Unused = unused }
    end
type Proc = delegate of unit -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RequestAdapterOptions = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public CompatibleSurface : nativeint
        val mutable public PowerPreference : PowerPreference
        val mutable public BackendType : BackendType
        val mutable public ForceFallbackAdapter : int
        val mutable public CompatibilityMode : int
        new(nextInChain : nativeint, compatibleSurface : nativeint, powerPreference : PowerPreference, backendType : BackendType, forceFallbackAdapter : int, compatibilityMode : int) = { NextInChain = nextInChain; CompatibleSurface = compatibleSurface; PowerPreference = powerPreference; BackendType = backendType; ForceFallbackAdapter = forceFallbackAdapter; CompatibilityMode = compatibilityMode }
        new(compatibleSurface : nativeint, powerPreference : PowerPreference, backendType : BackendType, forceFallbackAdapter : int, compatibilityMode : int) = RequestAdapterOptions(0n, compatibleSurface, powerPreference, backendType, forceFallbackAdapter, compatibilityMode)
    end
type RequestAdapterCallback = delegate of status : RequestAdapterStatus * adapter : nativeint * message : StringView * userdata : nativeint -> unit
type RequestAdapterCallback2 = delegate of status : RequestAdapterStatus * adapter : nativeint * message : StringView * userdata1 : nativeint * userdata2 : nativeint -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RequestAdapterCallbackInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        val mutable public Userdata : nativeint
        new(nextInChain : nativeint, mode : CallbackMode, callback : nativeint, userdata : nativeint) = { NextInChain = nextInChain; Mode = mode; Callback = callback; Userdata = userdata }
        new(mode : CallbackMode, callback : nativeint, userdata : nativeint) = RequestAdapterCallbackInfo(0n, mode, callback, userdata)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RequestAdapterCallbackInfo2 = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        val mutable public Userdata1 : nativeint
        val mutable public Userdata2 : nativeint
        new(nextInChain : nativeint, mode : CallbackMode, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = { NextInChain = nextInChain; Mode = mode; Callback = callback; Userdata1 = userdata1; Userdata2 = userdata2 }
        new(mode : CallbackMode, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = RequestAdapterCallbackInfo2(0n, mode, callback, userdata1, userdata2)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type AdapterInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Vendor : StringView
        val mutable public Architecture : StringView
        val mutable public Device : StringView
        val mutable public Description : StringView
        val mutable public BackendType : BackendType
        val mutable public AdapterType : AdapterType
        val mutable public VendorID : uint32
        val mutable public DeviceID : uint32
        val mutable public CompatibilityMode : int
        new(nextInChain : nativeint, vendor : StringView, architecture : StringView, device : StringView, description : StringView, backendType : BackendType, adapterType : AdapterType, vendorID : uint32, deviceID : uint32, compatibilityMode : int) = { NextInChain = nextInChain; Vendor = vendor; Architecture = architecture; Device = device; Description = description; BackendType = backendType; AdapterType = adapterType; VendorID = vendorID; DeviceID = deviceID; CompatibilityMode = compatibilityMode }
        new(vendor : StringView, architecture : StringView, device : StringView, description : StringView, backendType : BackendType, adapterType : AdapterType, vendorID : uint32, deviceID : uint32, compatibilityMode : int) = AdapterInfo(0n, vendor, architecture, device, description, backendType, adapterType, vendorID, deviceID, compatibilityMode)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DeviceDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        val mutable public RequiredFeatureCount : unativeint
        val mutable public RequiredFeatures : nativeptr<FeatureName>
        val mutable public RequiredLimits : nativeptr<RequiredLimits>
        val mutable public DefaultQueue : QueueDescriptor
        val mutable public DeviceLostCallback : nativeint
        val mutable public DeviceLostUserdata : nativeint
        val mutable public DeviceLostCallbackInfo : DeviceLostCallbackInfo
        val mutable public UncapturedErrorCallbackInfo : UncapturedErrorCallbackInfo
        val mutable public DeviceLostCallbackInfo2 : DeviceLostCallbackInfo2
        val mutable public UncapturedErrorCallbackInfo2 : UncapturedErrorCallbackInfo2
        new(nextInChain : nativeint, label : StringView, requiredFeatureCount : unativeint, requiredFeatures : nativeptr<FeatureName>, requiredLimits : nativeptr<RequiredLimits>, defaultQueue : QueueDescriptor, deviceLostCallback : nativeint, deviceLostUserdata : nativeint, deviceLostCallbackInfo : DeviceLostCallbackInfo, uncapturedErrorCallbackInfo : UncapturedErrorCallbackInfo, deviceLostCallbackInfo2 : DeviceLostCallbackInfo2, uncapturedErrorCallbackInfo2 : UncapturedErrorCallbackInfo2) = { NextInChain = nextInChain; Label = label; RequiredFeatureCount = requiredFeatureCount; RequiredFeatures = requiredFeatures; RequiredLimits = requiredLimits; DefaultQueue = defaultQueue; DeviceLostCallback = deviceLostCallback; DeviceLostUserdata = deviceLostUserdata; DeviceLostCallbackInfo = deviceLostCallbackInfo; UncapturedErrorCallbackInfo = uncapturedErrorCallbackInfo; DeviceLostCallbackInfo2 = deviceLostCallbackInfo2; UncapturedErrorCallbackInfo2 = uncapturedErrorCallbackInfo2 }
        new(label : StringView, requiredFeatureCount : unativeint, requiredFeatures : nativeptr<FeatureName>, requiredLimits : nativeptr<RequiredLimits>, defaultQueue : QueueDescriptor, deviceLostCallback : nativeint, deviceLostUserdata : nativeint, deviceLostCallbackInfo : DeviceLostCallbackInfo, uncapturedErrorCallbackInfo : UncapturedErrorCallbackInfo, deviceLostCallbackInfo2 : DeviceLostCallbackInfo2, uncapturedErrorCallbackInfo2 : UncapturedErrorCallbackInfo2) = DeviceDescriptor(0n, label, requiredFeatureCount, requiredFeatures, requiredLimits, defaultQueue, deviceLostCallback, deviceLostUserdata, deviceLostCallbackInfo, uncapturedErrorCallbackInfo, deviceLostCallbackInfo2, uncapturedErrorCallbackInfo2)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnTogglesDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public EnabledToggleCount : unativeint
        val mutable public EnabledToggles : nativeptr<nativeptr<byte>>
        val mutable public DisabledToggleCount : unativeint
        val mutable public DisabledToggles : nativeptr<nativeptr<byte>>
        new(nextInChain : nativeint, sType : SType, enabledToggleCount : unativeint, enabledToggles : nativeptr<nativeptr<byte>>, disabledToggleCount : unativeint, disabledToggles : nativeptr<nativeptr<byte>>) = { NextInChain = nextInChain; SType = sType; EnabledToggleCount = enabledToggleCount; EnabledToggles = enabledToggles; DisabledToggleCount = disabledToggleCount; DisabledToggles = disabledToggles }
        new(enabledToggleCount : unativeint, enabledToggles : nativeptr<nativeptr<byte>>, disabledToggleCount : unativeint, disabledToggles : nativeptr<nativeptr<byte>>) = DawnTogglesDescriptor(0n, Unchecked.defaultof<SType>, enabledToggleCount, enabledToggles, disabledToggleCount, disabledToggles)
    end
type DawnLoadCacheDataFunction = delegate of key : nativeint * keySize : unativeint * value : nativeint * valueSize : unativeint * userdata : nativeint -> unativeint
type DawnStoreCacheDataFunction = delegate of key : nativeint * keySize : unativeint * value : nativeint * valueSize : unativeint * userdata : nativeint -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnCacheDeviceDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public IsolationKey : StringView
        val mutable public LoadDataFunction : nativeint
        val mutable public StoreDataFunction : nativeint
        val mutable public FunctionUserdata : nativeint
        new(nextInChain : nativeint, sType : SType, isolationKey : StringView, loadDataFunction : nativeint, storeDataFunction : nativeint, functionUserdata : nativeint) = { NextInChain = nextInChain; SType = sType; IsolationKey = isolationKey; LoadDataFunction = loadDataFunction; StoreDataFunction = storeDataFunction; FunctionUserdata = functionUserdata }
        new(isolationKey : StringView, loadDataFunction : nativeint, storeDataFunction : nativeint, functionUserdata : nativeint) = DawnCacheDeviceDescriptor(0n, Unchecked.defaultof<SType>, isolationKey, loadDataFunction, storeDataFunction, functionUserdata)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnWGSLBlocklist = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public BlocklistedFeatureCount : unativeint
        val mutable public BlocklistedFeatures : nativeptr<nativeptr<byte>>
        new(nextInChain : nativeint, sType : SType, blocklistedFeatureCount : unativeint, blocklistedFeatures : nativeptr<nativeptr<byte>>) = { NextInChain = nextInChain; SType = sType; BlocklistedFeatureCount = blocklistedFeatureCount; BlocklistedFeatures = blocklistedFeatures }
        new(blocklistedFeatureCount : unativeint, blocklistedFeatures : nativeptr<nativeptr<byte>>) = DawnWGSLBlocklist(0n, Unchecked.defaultof<SType>, blocklistedFeatureCount, blocklistedFeatures)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type BindGroupEntry = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Binding : uint32
        val mutable public Buffer : nativeint
        val mutable public Offset : uint64
        val mutable public Size : uint64
        val mutable public Sampler : nativeint
        val mutable public TextureView : nativeint
        new(nextInChain : nativeint, binding : uint32, buffer : nativeint, offset : uint64, size : uint64, sampler : nativeint, textureView : nativeint) = { NextInChain = nextInChain; Binding = binding; Buffer = buffer; Offset = offset; Size = size; Sampler = sampler; TextureView = textureView }
        new(binding : uint32, buffer : nativeint, offset : uint64, size : uint64, sampler : nativeint, textureView : nativeint) = BindGroupEntry(0n, binding, buffer, offset, size, sampler, textureView)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type BindGroupDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        val mutable public Layout : nativeint
        val mutable public EntryCount : unativeint
        val mutable public Entries : nativeptr<BindGroupEntry>
        new(nextInChain : nativeint, label : StringView, layout : nativeint, entryCount : unativeint, entries : nativeptr<BindGroupEntry>) = { NextInChain = nextInChain; Label = label; Layout = layout; EntryCount = entryCount; Entries = entries }
        new(label : StringView, layout : nativeint, entryCount : unativeint, entries : nativeptr<BindGroupEntry>) = BindGroupDescriptor(0n, label, layout, entryCount, entries)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type BufferBindingLayout = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Type : BufferBindingType
        val mutable public HasDynamicOffset : int
        val mutable public MinBindingSize : uint64
        new(nextInChain : nativeint, typ : BufferBindingType, hasDynamicOffset : int, minBindingSize : uint64) = { NextInChain = nextInChain; Type = typ; HasDynamicOffset = hasDynamicOffset; MinBindingSize = minBindingSize }
        new(typ : BufferBindingType, hasDynamicOffset : int, minBindingSize : uint64) = BufferBindingLayout(0n, typ, hasDynamicOffset, minBindingSize)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SamplerBindingLayout = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Type : SamplerBindingType
        new(nextInChain : nativeint, typ : SamplerBindingType) = { NextInChain = nextInChain; Type = typ }
        new(typ : SamplerBindingType) = SamplerBindingLayout(0n, typ)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type StaticSamplerBindingLayout = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Sampler : nativeint
        val mutable public SampledTextureBinding : uint32
        new(nextInChain : nativeint, sType : SType, sampler : nativeint, sampledTextureBinding : uint32) = { NextInChain = nextInChain; SType = sType; Sampler = sampler; SampledTextureBinding = sampledTextureBinding }
        new(sampler : nativeint, sampledTextureBinding : uint32) = StaticSamplerBindingLayout(0n, Unchecked.defaultof<SType>, sampler, sampledTextureBinding)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type TextureBindingLayout = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SampleType : TextureSampleType
        val mutable public ViewDimension : TextureViewDimension
        val mutable public Multisampled : int
        new(nextInChain : nativeint, sampleType : TextureSampleType, viewDimension : TextureViewDimension, multisampled : int) = { NextInChain = nextInChain; SampleType = sampleType; ViewDimension = viewDimension; Multisampled = multisampled }
        new(sampleType : TextureSampleType, viewDimension : TextureViewDimension, multisampled : int) = TextureBindingLayout(0n, sampleType, viewDimension, multisampled)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceCapabilities = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Usages : TextureUsage
        val mutable public FormatCount : unativeint
        val mutable public Formats : nativeptr<TextureFormat>
        val mutable public PresentModeCount : unativeint
        val mutable public PresentModes : nativeptr<PresentMode>
        val mutable public AlphaModeCount : unativeint
        val mutable public AlphaModes : nativeptr<CompositeAlphaMode>
        new(nextInChain : nativeint, usages : TextureUsage, formatCount : unativeint, formats : nativeptr<TextureFormat>, presentModeCount : unativeint, presentModes : nativeptr<PresentMode>, alphaModeCount : unativeint, alphaModes : nativeptr<CompositeAlphaMode>) = { NextInChain = nextInChain; Usages = usages; FormatCount = formatCount; Formats = formats; PresentModeCount = presentModeCount; PresentModes = presentModes; AlphaModeCount = alphaModeCount; AlphaModes = alphaModes }
        new(usages : TextureUsage, formatCount : unativeint, formats : nativeptr<TextureFormat>, presentModeCount : unativeint, presentModes : nativeptr<PresentMode>, alphaModeCount : unativeint, alphaModes : nativeptr<CompositeAlphaMode>) = SurfaceCapabilities(0n, usages, formatCount, formats, presentModeCount, presentModes, alphaModeCount, alphaModes)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceConfiguration = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Device : nativeint
        val mutable public Format : TextureFormat
        val mutable public Usage : TextureUsage
        val mutable public ViewFormatCount : unativeint
        val mutable public ViewFormats : nativeptr<TextureFormat>
        val mutable public AlphaMode : CompositeAlphaMode
        val mutable public Width : uint32
        val mutable public Height : uint32
        val mutable public PresentMode : PresentMode
        new(nextInChain : nativeint, device : nativeint, format : TextureFormat, usage : TextureUsage, viewFormatCount : unativeint, viewFormats : nativeptr<TextureFormat>, alphaMode : CompositeAlphaMode, width : uint32, height : uint32, presentMode : PresentMode) = { NextInChain = nextInChain; Device = device; Format = format; Usage = usage; ViewFormatCount = viewFormatCount; ViewFormats = viewFormats; AlphaMode = alphaMode; Width = width; Height = height; PresentMode = presentMode }
        new(device : nativeint, format : TextureFormat, usage : TextureUsage, viewFormatCount : unativeint, viewFormats : nativeptr<TextureFormat>, alphaMode : CompositeAlphaMode, width : uint32, height : uint32, presentMode : PresentMode) = SurfaceConfiguration(0n, device, format, usage, viewFormatCount, viewFormats, alphaMode, width, height, presentMode)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ExternalTextureBindingEntry = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public ExternalTexture : nativeint
        new(nextInChain : nativeint, sType : SType, externalTexture : nativeint) = { NextInChain = nextInChain; SType = sType; ExternalTexture = externalTexture }
        new(externalTexture : nativeint) = ExternalTextureBindingEntry(0n, Unchecked.defaultof<SType>, externalTexture)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ExternalTextureBindingLayout = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        new(nextInChain : nativeint, sType : SType) = { NextInChain = nextInChain; SType = sType }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type StorageTextureBindingLayout = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Access : StorageTextureAccess
        val mutable public Format : TextureFormat
        val mutable public ViewDimension : TextureViewDimension
        new(nextInChain : nativeint, access : StorageTextureAccess, format : TextureFormat, viewDimension : TextureViewDimension) = { NextInChain = nextInChain; Access = access; Format = format; ViewDimension = viewDimension }
        new(access : StorageTextureAccess, format : TextureFormat, viewDimension : TextureViewDimension) = StorageTextureBindingLayout(0n, access, format, viewDimension)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type BindGroupLayoutEntry = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Binding : uint32
        val mutable public Visibility : ShaderStage
        val mutable public Buffer : BufferBindingLayout
        val mutable public Sampler : SamplerBindingLayout
        val mutable public Texture : TextureBindingLayout
        val mutable public StorageTexture : StorageTextureBindingLayout
        new(nextInChain : nativeint, binding : uint32, visibility : ShaderStage, buffer : BufferBindingLayout, sampler : SamplerBindingLayout, texture : TextureBindingLayout, storageTexture : StorageTextureBindingLayout) = { NextInChain = nextInChain; Binding = binding; Visibility = visibility; Buffer = buffer; Sampler = sampler; Texture = texture; StorageTexture = storageTexture }
        new(binding : uint32, visibility : ShaderStage, buffer : BufferBindingLayout, sampler : SamplerBindingLayout, texture : TextureBindingLayout, storageTexture : StorageTextureBindingLayout) = BindGroupLayoutEntry(0n, binding, visibility, buffer, sampler, texture, storageTexture)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type BindGroupLayoutDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        val mutable public EntryCount : unativeint
        val mutable public Entries : nativeptr<BindGroupLayoutEntry>
        new(nextInChain : nativeint, label : StringView, entryCount : unativeint, entries : nativeptr<BindGroupLayoutEntry>) = { NextInChain = nextInChain; Label = label; EntryCount = entryCount; Entries = entries }
        new(label : StringView, entryCount : unativeint, entries : nativeptr<BindGroupLayoutEntry>) = BindGroupLayoutDescriptor(0n, label, entryCount, entries)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type BlendComponent = 
    struct
        val mutable public Operation : BlendOperation
        val mutable public SrcFactor : BlendFactor
        val mutable public DstFactor : BlendFactor
        new(operation : BlendOperation, srcFactor : BlendFactor, dstFactor : BlendFactor) = { Operation = operation; SrcFactor = srcFactor; DstFactor = dstFactor }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type StringView = 
    struct
        val mutable public Data : nativeptr<byte>
        val mutable public Length : unativeint
        new(data : nativeptr<byte>, length : unativeint) = { Data = data; Length = length }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type BufferDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        val mutable public Usage : BufferUsage
        val mutable public Size : uint64
        val mutable public MappedAtCreation : int
        new(nextInChain : nativeint, label : StringView, usage : BufferUsage, size : uint64, mappedAtCreation : int) = { NextInChain = nextInChain; Label = label; Usage = usage; Size = size; MappedAtCreation = mappedAtCreation }
        new(label : StringView, usage : BufferUsage, size : uint64, mappedAtCreation : int) = BufferDescriptor(0n, label, usage, size, mappedAtCreation)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type BufferHostMappedPointer = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Pointer : nativeint
        val mutable public DisposeCallback : nativeint
        val mutable public Userdata : nativeint
        new(nextInChain : nativeint, sType : SType, pointer : nativeint, disposeCallback : nativeint, userdata : nativeint) = { NextInChain = nextInChain; SType = sType; Pointer = pointer; DisposeCallback = disposeCallback; Userdata = userdata }
        new(pointer : nativeint, disposeCallback : nativeint, userdata : nativeint) = BufferHostMappedPointer(0n, Unchecked.defaultof<SType>, pointer, disposeCallback, userdata)
    end
type Callback = delegate of userdata : nativeint -> unit
type BufferMapCallback = delegate of status : BufferMapAsyncStatus * userdata : nativeint -> unit
type BufferMapCallback2 = delegate of status : MapAsyncStatus * message : StringView * userdata1 : nativeint * userdata2 : nativeint -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type BufferMapCallbackInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        val mutable public Userdata : nativeint
        new(nextInChain : nativeint, mode : CallbackMode, callback : nativeint, userdata : nativeint) = { NextInChain = nextInChain; Mode = mode; Callback = callback; Userdata = userdata }
        new(mode : CallbackMode, callback : nativeint, userdata : nativeint) = BufferMapCallbackInfo(0n, mode, callback, userdata)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type BufferMapCallbackInfo2 = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        val mutable public Userdata1 : nativeint
        val mutable public Userdata2 : nativeint
        new(nextInChain : nativeint, mode : CallbackMode, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = { NextInChain = nextInChain; Mode = mode; Callback = callback; Userdata1 = userdata1; Userdata2 = userdata2 }
        new(mode : CallbackMode, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = BufferMapCallbackInfo2(0n, mode, callback, userdata1, userdata2)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type Color = 
    struct
        val mutable public R : double
        val mutable public G : double
        val mutable public B : double
        val mutable public A : double
        new(r : double, g : double, b : double, a : double) = { R = r; G = g; B = b; A = a }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ConstantEntry = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Key : StringView
        val mutable public Value : double
        new(nextInChain : nativeint, key : StringView, value : double) = { NextInChain = nextInChain; Key = key; Value = value }
        new(key : StringView, value : double) = ConstantEntry(0n, key, value)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type CommandBufferDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        new(nextInChain : nativeint, label : StringView) = { NextInChain = nextInChain; Label = label }
        new(label : StringView) = CommandBufferDescriptor(0n, label)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type CommandEncoderDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        new(nextInChain : nativeint, label : StringView) = { NextInChain = nextInChain; Label = label }
        new(label : StringView) = CommandEncoderDescriptor(0n, label)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type CompilationInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public MessageCount : unativeint
        val mutable public Messages : nativeptr<CompilationMessage>
        new(nextInChain : nativeint, messageCount : unativeint, messages : nativeptr<CompilationMessage>) = { NextInChain = nextInChain; MessageCount = messageCount; Messages = messages }
        new(messageCount : unativeint, messages : nativeptr<CompilationMessage>) = CompilationInfo(0n, messageCount, messages)
    end
type CompilationInfoCallback = delegate of status : CompilationInfoRequestStatus * compilationInfo : nativeptr<CompilationInfo> * userdata : nativeint -> unit
type CompilationInfoCallback2 = delegate of status : CompilationInfoRequestStatus * compilationInfo : nativeptr<CompilationInfo> * userdata1 : nativeint * userdata2 : nativeint -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type CompilationInfoCallbackInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        val mutable public Userdata : nativeint
        new(nextInChain : nativeint, mode : CallbackMode, callback : nativeint, userdata : nativeint) = { NextInChain = nextInChain; Mode = mode; Callback = callback; Userdata = userdata }
        new(mode : CallbackMode, callback : nativeint, userdata : nativeint) = CompilationInfoCallbackInfo(0n, mode, callback, userdata)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type CompilationInfoCallbackInfo2 = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        val mutable public Userdata1 : nativeint
        val mutable public Userdata2 : nativeint
        new(nextInChain : nativeint, mode : CallbackMode, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = { NextInChain = nextInChain; Mode = mode; Callback = callback; Userdata1 = userdata1; Userdata2 = userdata2 }
        new(mode : CallbackMode, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = CompilationInfoCallbackInfo2(0n, mode, callback, userdata1, userdata2)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type CompilationMessage = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Message : StringView
        val mutable public Type : CompilationMessageType
        val mutable public LineNum : uint64
        val mutable public LinePos : uint64
        val mutable public Offset : uint64
        val mutable public Length : uint64
        val mutable public Utf16LinePos : uint64
        val mutable public Utf16Offset : uint64
        val mutable public Utf16Length : uint64
        new(nextInChain : nativeint, message : StringView, typ : CompilationMessageType, lineNum : uint64, linePos : uint64, offset : uint64, length : uint64, utf16LinePos : uint64, utf16Offset : uint64, utf16Length : uint64) = { NextInChain = nextInChain; Message = message; Type = typ; LineNum = lineNum; LinePos = linePos; Offset = offset; Length = length; Utf16LinePos = utf16LinePos; Utf16Offset = utf16Offset; Utf16Length = utf16Length }
        new(message : StringView, typ : CompilationMessageType, lineNum : uint64, linePos : uint64, offset : uint64, length : uint64, utf16LinePos : uint64, utf16Offset : uint64, utf16Length : uint64) = CompilationMessage(0n, message, typ, lineNum, linePos, offset, length, utf16LinePos, utf16Offset, utf16Length)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ComputePassDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        val mutable public TimestampWrites : nativeptr<ComputePassTimestampWrites>
        new(nextInChain : nativeint, label : StringView, timestampWrites : nativeptr<ComputePassTimestampWrites>) = { NextInChain = nextInChain; Label = label; TimestampWrites = timestampWrites }
        new(label : StringView, timestampWrites : nativeptr<ComputePassTimestampWrites>) = ComputePassDescriptor(0n, label, timestampWrites)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ComputePassTimestampWrites = 
    struct
        val mutable public QuerySet : nativeint
        val mutable public BeginningOfPassWriteIndex : uint32
        val mutable public EndOfPassWriteIndex : uint32
        new(querySet : nativeint, beginningOfPassWriteIndex : uint32, endOfPassWriteIndex : uint32) = { QuerySet = querySet; BeginningOfPassWriteIndex = beginningOfPassWriteIndex; EndOfPassWriteIndex = endOfPassWriteIndex }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ComputePipelineDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        val mutable public Layout : nativeint
        val mutable public Compute : ProgrammableStageDescriptor
        new(nextInChain : nativeint, label : StringView, layout : nativeint, compute : ProgrammableStageDescriptor) = { NextInChain = nextInChain; Label = label; Layout = layout; Compute = compute }
        new(label : StringView, layout : nativeint, compute : ProgrammableStageDescriptor) = ComputePipelineDescriptor(0n, label, layout, compute)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnComputePipelineFullSubgroups = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public RequiresFullSubgroups : int
        new(nextInChain : nativeint, sType : SType, requiresFullSubgroups : int) = { NextInChain = nextInChain; SType = sType; RequiresFullSubgroups = requiresFullSubgroups }
        new(requiresFullSubgroups : int) = DawnComputePipelineFullSubgroups(0n, Unchecked.defaultof<SType>, requiresFullSubgroups)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type CopyTextureForBrowserOptions = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public FlipY : int
        val mutable public NeedsColorSpaceConversion : int
        val mutable public SrcAlphaMode : AlphaMode
        val mutable public SrcTransferFunctionParameters : nativeptr<float32>
        val mutable public ConversionMatrix : nativeptr<float32>
        val mutable public DstTransferFunctionParameters : nativeptr<float32>
        val mutable public DstAlphaMode : AlphaMode
        val mutable public InternalUsage : int
        new(nextInChain : nativeint, flipY : int, needsColorSpaceConversion : int, srcAlphaMode : AlphaMode, srcTransferFunctionParameters : nativeptr<float32>, conversionMatrix : nativeptr<float32>, dstTransferFunctionParameters : nativeptr<float32>, dstAlphaMode : AlphaMode, internalUsage : int) = { NextInChain = nextInChain; FlipY = flipY; NeedsColorSpaceConversion = needsColorSpaceConversion; SrcAlphaMode = srcAlphaMode; SrcTransferFunctionParameters = srcTransferFunctionParameters; ConversionMatrix = conversionMatrix; DstTransferFunctionParameters = dstTransferFunctionParameters; DstAlphaMode = dstAlphaMode; InternalUsage = internalUsage }
        new(flipY : int, needsColorSpaceConversion : int, srcAlphaMode : AlphaMode, srcTransferFunctionParameters : nativeptr<float32>, conversionMatrix : nativeptr<float32>, dstTransferFunctionParameters : nativeptr<float32>, dstAlphaMode : AlphaMode, internalUsage : int) = CopyTextureForBrowserOptions(0n, flipY, needsColorSpaceConversion, srcAlphaMode, srcTransferFunctionParameters, conversionMatrix, dstTransferFunctionParameters, dstAlphaMode, internalUsage)
    end
type CreateComputePipelineAsyncCallback = delegate of status : CreatePipelineAsyncStatus * pipeline : nativeint * message : StringView * userdata : nativeint -> unit
type CreateComputePipelineAsyncCallback2 = delegate of status : CreatePipelineAsyncStatus * pipeline : nativeint * message : StringView * userdata1 : nativeint * userdata2 : nativeint -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type CreateComputePipelineAsyncCallbackInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        val mutable public Userdata : nativeint
        new(nextInChain : nativeint, mode : CallbackMode, callback : nativeint, userdata : nativeint) = { NextInChain = nextInChain; Mode = mode; Callback = callback; Userdata = userdata }
        new(mode : CallbackMode, callback : nativeint, userdata : nativeint) = CreateComputePipelineAsyncCallbackInfo(0n, mode, callback, userdata)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type CreateComputePipelineAsyncCallbackInfo2 = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        val mutable public Userdata1 : nativeint
        val mutable public Userdata2 : nativeint
        new(nextInChain : nativeint, mode : CallbackMode, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = { NextInChain = nextInChain; Mode = mode; Callback = callback; Userdata1 = userdata1; Userdata2 = userdata2 }
        new(mode : CallbackMode, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = CreateComputePipelineAsyncCallbackInfo2(0n, mode, callback, userdata1, userdata2)
    end
type CreateRenderPipelineAsyncCallback = delegate of status : CreatePipelineAsyncStatus * pipeline : nativeint * message : StringView * userdata : nativeint -> unit
type CreateRenderPipelineAsyncCallback2 = delegate of status : CreatePipelineAsyncStatus * pipeline : nativeint * message : StringView * userdata1 : nativeint * userdata2 : nativeint -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type CreateRenderPipelineAsyncCallbackInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        val mutable public Userdata : nativeint
        new(nextInChain : nativeint, mode : CallbackMode, callback : nativeint, userdata : nativeint) = { NextInChain = nextInChain; Mode = mode; Callback = callback; Userdata = userdata }
        new(mode : CallbackMode, callback : nativeint, userdata : nativeint) = CreateRenderPipelineAsyncCallbackInfo(0n, mode, callback, userdata)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type CreateRenderPipelineAsyncCallbackInfo2 = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        val mutable public Userdata1 : nativeint
        val mutable public Userdata2 : nativeint
        new(nextInChain : nativeint, mode : CallbackMode, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = { NextInChain = nextInChain; Mode = mode; Callback = callback; Userdata1 = userdata1; Userdata2 = userdata2 }
        new(mode : CallbackMode, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = CreateRenderPipelineAsyncCallbackInfo2(0n, mode, callback, userdata1, userdata2)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type AHardwareBufferProperties = 
    struct
        val mutable public YCbCrInfo : YCbCrVkDescriptor
        new(yCbCrInfo : YCbCrVkDescriptor) = { YCbCrInfo = yCbCrInfo }
    end
type DeviceLostCallback = delegate of reason : DeviceLostReason * message : StringView * userdata : nativeint -> unit
type DeviceLostCallbackNew = delegate of device : nativeptr<nativeint> * reason : DeviceLostReason * message : StringView * userdata : nativeint -> unit
type DeviceLostCallback2 = delegate of device : nativeptr<nativeint> * reason : DeviceLostReason * message : StringView * userdata1 : nativeint * userdata2 : nativeint -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DeviceLostCallbackInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        val mutable public Userdata : nativeint
        new(nextInChain : nativeint, mode : CallbackMode, callback : nativeint, userdata : nativeint) = { NextInChain = nextInChain; Mode = mode; Callback = callback; Userdata = userdata }
        new(mode : CallbackMode, callback : nativeint, userdata : nativeint) = DeviceLostCallbackInfo(0n, mode, callback, userdata)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DeviceLostCallbackInfo2 = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        val mutable public Userdata1 : nativeint
        val mutable public Userdata2 : nativeint
        new(nextInChain : nativeint, mode : CallbackMode, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = { NextInChain = nextInChain; Mode = mode; Callback = callback; Userdata1 = userdata1; Userdata2 = userdata2 }
        new(mode : CallbackMode, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = DeviceLostCallbackInfo2(0n, mode, callback, userdata1, userdata2)
    end
type ErrorCallback = delegate of typ : ErrorType * message : StringView * userdata : nativeint -> unit
type UncapturedErrorCallback = delegate of device : nativeptr<nativeint> * typ : ErrorType * message : StringView * userdata1 : nativeint * userdata2 : nativeint -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type UncapturedErrorCallbackInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Callback : nativeint
        val mutable public Userdata : nativeint
        new(nextInChain : nativeint, callback : nativeint, userdata : nativeint) = { NextInChain = nextInChain; Callback = callback; Userdata = userdata }
        new(callback : nativeint, userdata : nativeint) = UncapturedErrorCallbackInfo(0n, callback, userdata)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type UncapturedErrorCallbackInfo2 = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Callback : nativeint
        val mutable public Userdata1 : nativeint
        val mutable public Userdata2 : nativeint
        new(nextInChain : nativeint, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = { NextInChain = nextInChain; Callback = callback; Userdata1 = userdata1; Userdata2 = userdata2 }
        new(callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = UncapturedErrorCallbackInfo2(0n, callback, userdata1, userdata2)
    end
type PopErrorScopeCallback = delegate of status : PopErrorScopeStatus * typ : ErrorType * message : StringView * userdata : nativeint -> unit
type PopErrorScopeCallback2 = delegate of status : PopErrorScopeStatus * typ : ErrorType * message : StringView * userdata1 : nativeint * userdata2 : nativeint -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type PopErrorScopeCallbackInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        val mutable public OldCallback : nativeint
        val mutable public Userdata : nativeint
        new(nextInChain : nativeint, mode : CallbackMode, callback : nativeint, oldCallback : nativeint, userdata : nativeint) = { NextInChain = nextInChain; Mode = mode; Callback = callback; OldCallback = oldCallback; Userdata = userdata }
        new(mode : CallbackMode, callback : nativeint, oldCallback : nativeint, userdata : nativeint) = PopErrorScopeCallbackInfo(0n, mode, callback, oldCallback, userdata)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type PopErrorScopeCallbackInfo2 = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        val mutable public Userdata1 : nativeint
        val mutable public Userdata2 : nativeint
        new(nextInChain : nativeint, mode : CallbackMode, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = { NextInChain = nextInChain; Mode = mode; Callback = callback; Userdata1 = userdata1; Userdata2 = userdata2 }
        new(mode : CallbackMode, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = PopErrorScopeCallbackInfo2(0n, mode, callback, userdata1, userdata2)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type Limits = 
    struct
        val mutable public MaxTextureDimension1D : uint32
        val mutable public MaxTextureDimension2D : uint32
        val mutable public MaxTextureDimension3D : uint32
        val mutable public MaxTextureArrayLayers : uint32
        val mutable public MaxBindGroups : uint32
        val mutable public MaxBindGroupsPlusVertexBuffers : uint32
        val mutable public MaxBindingsPerBindGroup : uint32
        val mutable public MaxDynamicUniformBuffersPerPipelineLayout : uint32
        val mutable public MaxDynamicStorageBuffersPerPipelineLayout : uint32
        val mutable public MaxSampledTexturesPerShaderStage : uint32
        val mutable public MaxSamplersPerShaderStage : uint32
        val mutable public MaxStorageBuffersPerShaderStage : uint32
        val mutable public MaxStorageTexturesPerShaderStage : uint32
        val mutable public MaxUniformBuffersPerShaderStage : uint32
        val mutable public MaxUniformBufferBindingSize : uint64
        val mutable public MaxStorageBufferBindingSize : uint64
        val mutable public MinUniformBufferOffsetAlignment : uint32
        val mutable public MinStorageBufferOffsetAlignment : uint32
        val mutable public MaxVertexBuffers : uint32
        val mutable public MaxBufferSize : uint64
        val mutable public MaxVertexAttributes : uint32
        val mutable public MaxVertexBufferArrayStride : uint32
        val mutable public MaxInterStageShaderComponents : uint32
        val mutable public MaxInterStageShaderVariables : uint32
        val mutable public MaxColorAttachments : uint32
        val mutable public MaxColorAttachmentBytesPerSample : uint32
        val mutable public MaxComputeWorkgroupStorageSize : uint32
        val mutable public MaxComputeInvocationsPerWorkgroup : uint32
        val mutable public MaxComputeWorkgroupSizeX : uint32
        val mutable public MaxComputeWorkgroupSizeY : uint32
        val mutable public MaxComputeWorkgroupSizeZ : uint32
        val mutable public MaxComputeWorkgroupsPerDimension : uint32
        new(maxTextureDimension1D : uint32, maxTextureDimension2D : uint32, maxTextureDimension3D : uint32, maxTextureArrayLayers : uint32, maxBindGroups : uint32, maxBindGroupsPlusVertexBuffers : uint32, maxBindingsPerBindGroup : uint32, maxDynamicUniformBuffersPerPipelineLayout : uint32, maxDynamicStorageBuffersPerPipelineLayout : uint32, maxSampledTexturesPerShaderStage : uint32, maxSamplersPerShaderStage : uint32, maxStorageBuffersPerShaderStage : uint32, maxStorageTexturesPerShaderStage : uint32, maxUniformBuffersPerShaderStage : uint32, maxUniformBufferBindingSize : uint64, maxStorageBufferBindingSize : uint64, minUniformBufferOffsetAlignment : uint32, minStorageBufferOffsetAlignment : uint32, maxVertexBuffers : uint32, maxBufferSize : uint64, maxVertexAttributes : uint32, maxVertexBufferArrayStride : uint32, maxInterStageShaderComponents : uint32, maxInterStageShaderVariables : uint32, maxColorAttachments : uint32, maxColorAttachmentBytesPerSample : uint32, maxComputeWorkgroupStorageSize : uint32, maxComputeInvocationsPerWorkgroup : uint32, maxComputeWorkgroupSizeX : uint32, maxComputeWorkgroupSizeY : uint32, maxComputeWorkgroupSizeZ : uint32, maxComputeWorkgroupsPerDimension : uint32) = { MaxTextureDimension1D = maxTextureDimension1D; MaxTextureDimension2D = maxTextureDimension2D; MaxTextureDimension3D = maxTextureDimension3D; MaxTextureArrayLayers = maxTextureArrayLayers; MaxBindGroups = maxBindGroups; MaxBindGroupsPlusVertexBuffers = maxBindGroupsPlusVertexBuffers; MaxBindingsPerBindGroup = maxBindingsPerBindGroup; MaxDynamicUniformBuffersPerPipelineLayout = maxDynamicUniformBuffersPerPipelineLayout; MaxDynamicStorageBuffersPerPipelineLayout = maxDynamicStorageBuffersPerPipelineLayout; MaxSampledTexturesPerShaderStage = maxSampledTexturesPerShaderStage; MaxSamplersPerShaderStage = maxSamplersPerShaderStage; MaxStorageBuffersPerShaderStage = maxStorageBuffersPerShaderStage; MaxStorageTexturesPerShaderStage = maxStorageTexturesPerShaderStage; MaxUniformBuffersPerShaderStage = maxUniformBuffersPerShaderStage; MaxUniformBufferBindingSize = maxUniformBufferBindingSize; MaxStorageBufferBindingSize = maxStorageBufferBindingSize; MinUniformBufferOffsetAlignment = minUniformBufferOffsetAlignment; MinStorageBufferOffsetAlignment = minStorageBufferOffsetAlignment; MaxVertexBuffers = maxVertexBuffers; MaxBufferSize = maxBufferSize; MaxVertexAttributes = maxVertexAttributes; MaxVertexBufferArrayStride = maxVertexBufferArrayStride; MaxInterStageShaderComponents = maxInterStageShaderComponents; MaxInterStageShaderVariables = maxInterStageShaderVariables; MaxColorAttachments = maxColorAttachments; MaxColorAttachmentBytesPerSample = maxColorAttachmentBytesPerSample; MaxComputeWorkgroupStorageSize = maxComputeWorkgroupStorageSize; MaxComputeInvocationsPerWorkgroup = maxComputeInvocationsPerWorkgroup; MaxComputeWorkgroupSizeX = maxComputeWorkgroupSizeX; MaxComputeWorkgroupSizeY = maxComputeWorkgroupSizeY; MaxComputeWorkgroupSizeZ = maxComputeWorkgroupSizeZ; MaxComputeWorkgroupsPerDimension = maxComputeWorkgroupsPerDimension }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnExperimentalSubgroupLimits = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public MinSubgroupSize : uint32
        val mutable public MaxSubgroupSize : uint32
        new(nextInChain : nativeint, sType : SType, minSubgroupSize : uint32, maxSubgroupSize : uint32) = { NextInChain = nextInChain; SType = sType; MinSubgroupSize = minSubgroupSize; MaxSubgroupSize = maxSubgroupSize }
        new(minSubgroupSize : uint32, maxSubgroupSize : uint32) = DawnExperimentalSubgroupLimits(0n, Unchecked.defaultof<SType>, minSubgroupSize, maxSubgroupSize)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnExperimentalImmediateDataLimits = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public MaxImmediateDataRangeByteSize : uint32
        new(nextInChain : nativeint, sType : SType, maxImmediateDataRangeByteSize : uint32) = { NextInChain = nextInChain; SType = sType; MaxImmediateDataRangeByteSize = maxImmediateDataRangeByteSize }
        new(maxImmediateDataRangeByteSize : uint32) = DawnExperimentalImmediateDataLimits(0n, Unchecked.defaultof<SType>, maxImmediateDataRangeByteSize)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RequiredLimits = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Limits : Limits
        new(nextInChain : nativeint, limits : Limits) = { NextInChain = nextInChain; Limits = limits }
        new(limits : Limits) = RequiredLimits(0n, limits)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SupportedLimits = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Limits : Limits
        new(nextInChain : nativeint, limits : Limits) = { NextInChain = nextInChain; Limits = limits }
        new(limits : Limits) = SupportedLimits(0n, limits)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SupportedFeatures = 
    struct
        val mutable public FeatureCount : unativeint
        val mutable public Features : nativeptr<FeatureName>
        new(featureCount : unativeint, features : nativeptr<FeatureName>) = { FeatureCount = featureCount; Features = features }
    end
type LoggingCallback = delegate of typ : LoggingType * message : StringView * userdata : nativeint -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type Extent2D = 
    struct
        val mutable public Width : uint32
        val mutable public Height : uint32
        new(width : uint32, height : uint32) = { Width = width; Height = height }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type Extent3D = 
    struct
        val mutable public Width : uint32
        val mutable public Height : uint32
        val mutable public DepthOrArrayLayers : uint32
        new(width : uint32, height : uint32, depthOrArrayLayers : uint32) = { Width = width; Height = height; DepthOrArrayLayers = depthOrArrayLayers }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ExternalTextureDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        val mutable public Plane0 : nativeint
        val mutable public Plane1 : nativeint
        val mutable public VisibleOrigin : Origin2D
        val mutable public VisibleSize : Extent2D
        val mutable public CropOrigin : Origin2D
        val mutable public CropSize : Extent2D
        val mutable public ApparentSize : Extent2D
        val mutable public DoYuvToRgbConversionOnly : int
        val mutable public YuvToRgbConversionMatrix : nativeptr<float32>
        val mutable public SrcTransferFunctionParameters : nativeptr<float32>
        val mutable public DstTransferFunctionParameters : nativeptr<float32>
        val mutable public GamutConversionMatrix : nativeptr<float32>
        val mutable public Mirrored : int
        val mutable public Rotation : ExternalTextureRotation
        new(nextInChain : nativeint, label : StringView, plane0 : nativeint, plane1 : nativeint, visibleOrigin : Origin2D, visibleSize : Extent2D, cropOrigin : Origin2D, cropSize : Extent2D, apparentSize : Extent2D, doYuvToRgbConversionOnly : int, yuvToRgbConversionMatrix : nativeptr<float32>, srcTransferFunctionParameters : nativeptr<float32>, dstTransferFunctionParameters : nativeptr<float32>, gamutConversionMatrix : nativeptr<float32>, mirrored : int, rotation : ExternalTextureRotation) = { NextInChain = nextInChain; Label = label; Plane0 = plane0; Plane1 = plane1; VisibleOrigin = visibleOrigin; VisibleSize = visibleSize; CropOrigin = cropOrigin; CropSize = cropSize; ApparentSize = apparentSize; DoYuvToRgbConversionOnly = doYuvToRgbConversionOnly; YuvToRgbConversionMatrix = yuvToRgbConversionMatrix; SrcTransferFunctionParameters = srcTransferFunctionParameters; DstTransferFunctionParameters = dstTransferFunctionParameters; GamutConversionMatrix = gamutConversionMatrix; Mirrored = mirrored; Rotation = rotation }
        new(label : StringView, plane0 : nativeint, plane1 : nativeint, visibleOrigin : Origin2D, visibleSize : Extent2D, cropOrigin : Origin2D, cropSize : Extent2D, apparentSize : Extent2D, doYuvToRgbConversionOnly : int, yuvToRgbConversionMatrix : nativeptr<float32>, srcTransferFunctionParameters : nativeptr<float32>, dstTransferFunctionParameters : nativeptr<float32>, gamutConversionMatrix : nativeptr<float32>, mirrored : int, rotation : ExternalTextureRotation) = ExternalTextureDescriptor(0n, label, plane0, plane1, visibleOrigin, visibleSize, cropOrigin, cropSize, apparentSize, doYuvToRgbConversionOnly, yuvToRgbConversionMatrix, srcTransferFunctionParameters, dstTransferFunctionParameters, gamutConversionMatrix, mirrored, rotation)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedBufferMemoryProperties = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Usage : BufferUsage
        val mutable public Size : uint64
        new(nextInChain : nativeint, usage : BufferUsage, size : uint64) = { NextInChain = nextInChain; Usage = usage; Size = size }
        new(usage : BufferUsage, size : uint64) = SharedBufferMemoryProperties(0n, usage, size)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedBufferMemoryDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        new(nextInChain : nativeint, label : StringView) = { NextInChain = nextInChain; Label = label }
        new(label : StringView) = SharedBufferMemoryDescriptor(0n, label)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedTextureMemoryProperties = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Usage : TextureUsage
        val mutable public Size : Extent3D
        val mutable public Format : TextureFormat
        new(nextInChain : nativeint, usage : TextureUsage, size : Extent3D, format : TextureFormat) = { NextInChain = nextInChain; Usage = usage; Size = size; Format = format }
        new(usage : TextureUsage, size : Extent3D, format : TextureFormat) = SharedTextureMemoryProperties(0n, usage, size, format)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedTextureMemoryAHardwareBufferProperties = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public YCbCrInfo : YCbCrVkDescriptor
        new(nextInChain : nativeint, sType : SType, yCbCrInfo : YCbCrVkDescriptor) = { NextInChain = nextInChain; SType = sType; YCbCrInfo = yCbCrInfo }
        new(yCbCrInfo : YCbCrVkDescriptor) = SharedTextureMemoryAHardwareBufferProperties(0n, Unchecked.defaultof<SType>, yCbCrInfo)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedTextureMemoryDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        new(nextInChain : nativeint, label : StringView) = { NextInChain = nextInChain; Label = label }
        new(label : StringView) = SharedTextureMemoryDescriptor(0n, label)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedBufferMemoryBeginAccessDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Initialized : int
        val mutable public FenceCount : unativeint
        val mutable public Fences : nativeptr<nativeint>
        val mutable public SignaledValues : nativeptr<uint64>
        new(nextInChain : nativeint, initialized : int, fenceCount : unativeint, fences : nativeptr<nativeint>, signaledValues : nativeptr<uint64>) = { NextInChain = nextInChain; Initialized = initialized; FenceCount = fenceCount; Fences = fences; SignaledValues = signaledValues }
        new(initialized : int, fenceCount : unativeint, fences : nativeptr<nativeint>, signaledValues : nativeptr<uint64>) = SharedBufferMemoryBeginAccessDescriptor(0n, initialized, fenceCount, fences, signaledValues)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedBufferMemoryEndAccessState = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Initialized : int
        val mutable public FenceCount : unativeint
        val mutable public Fences : nativeptr<nativeint>
        val mutable public SignaledValues : nativeptr<uint64>
        new(nextInChain : nativeint, initialized : int, fenceCount : unativeint, fences : nativeptr<nativeint>, signaledValues : nativeptr<uint64>) = { NextInChain = nextInChain; Initialized = initialized; FenceCount = fenceCount; Fences = fences; SignaledValues = signaledValues }
        new(initialized : int, fenceCount : unativeint, fences : nativeptr<nativeint>, signaledValues : nativeptr<uint64>) = SharedBufferMemoryEndAccessState(0n, initialized, fenceCount, fences, signaledValues)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedTextureMemoryVkDedicatedAllocationDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public DedicatedAllocation : int
        new(nextInChain : nativeint, sType : SType, dedicatedAllocation : int) = { NextInChain = nextInChain; SType = sType; DedicatedAllocation = dedicatedAllocation }
        new(dedicatedAllocation : int) = SharedTextureMemoryVkDedicatedAllocationDescriptor(0n, Unchecked.defaultof<SType>, dedicatedAllocation)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedTextureMemoryAHardwareBufferDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Handle : nativeint
        val mutable public UseExternalFormat : int
        new(nextInChain : nativeint, sType : SType, handle : nativeint, useExternalFormat : int) = { NextInChain = nextInChain; SType = sType; Handle = handle; UseExternalFormat = useExternalFormat }
        new(handle : nativeint, useExternalFormat : int) = SharedTextureMemoryAHardwareBufferDescriptor(0n, Unchecked.defaultof<SType>, handle, useExternalFormat)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedTextureMemoryDmaBufPlane = 
    struct
        val mutable public Fd : int
        val mutable public Offset : uint64
        val mutable public Stride : uint32
        new(fd : int, offset : uint64, stride : uint32) = { Fd = fd; Offset = offset; Stride = stride }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedTextureMemoryDmaBufDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Size : Extent3D
        val mutable public DrmFormat : uint32
        val mutable public DrmModifier : uint64
        val mutable public PlaneCount : unativeint
        val mutable public Planes : nativeptr<SharedTextureMemoryDmaBufPlane>
        new(nextInChain : nativeint, sType : SType, size : Extent3D, drmFormat : uint32, drmModifier : uint64, planeCount : unativeint, planes : nativeptr<SharedTextureMemoryDmaBufPlane>) = { NextInChain = nextInChain; SType = sType; Size = size; DrmFormat = drmFormat; DrmModifier = drmModifier; PlaneCount = planeCount; Planes = planes }
        new(size : Extent3D, drmFormat : uint32, drmModifier : uint64, planeCount : unativeint, planes : nativeptr<SharedTextureMemoryDmaBufPlane>) = SharedTextureMemoryDmaBufDescriptor(0n, Unchecked.defaultof<SType>, size, drmFormat, drmModifier, planeCount, planes)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedTextureMemoryOpaqueFDDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public VkImageCreateInfo : nativeint
        val mutable public MemoryFD : int
        val mutable public MemoryTypeIndex : uint32
        val mutable public AllocationSize : uint64
        val mutable public DedicatedAllocation : int
        new(nextInChain : nativeint, sType : SType, vkImageCreateInfo : nativeint, memoryFD : int, memoryTypeIndex : uint32, allocationSize : uint64, dedicatedAllocation : int) = { NextInChain = nextInChain; SType = sType; VkImageCreateInfo = vkImageCreateInfo; MemoryFD = memoryFD; MemoryTypeIndex = memoryTypeIndex; AllocationSize = allocationSize; DedicatedAllocation = dedicatedAllocation }
        new(vkImageCreateInfo : nativeint, memoryFD : int, memoryTypeIndex : uint32, allocationSize : uint64, dedicatedAllocation : int) = SharedTextureMemoryOpaqueFDDescriptor(0n, Unchecked.defaultof<SType>, vkImageCreateInfo, memoryFD, memoryTypeIndex, allocationSize, dedicatedAllocation)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedTextureMemoryZirconHandleDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public MemoryFD : uint32
        val mutable public AllocationSize : uint64
        new(nextInChain : nativeint, sType : SType, memoryFD : uint32, allocationSize : uint64) = { NextInChain = nextInChain; SType = sType; MemoryFD = memoryFD; AllocationSize = allocationSize }
        new(memoryFD : uint32, allocationSize : uint64) = SharedTextureMemoryZirconHandleDescriptor(0n, Unchecked.defaultof<SType>, memoryFD, allocationSize)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedTextureMemoryDXGISharedHandleDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Handle : nativeint
        val mutable public UseKeyedMutex : int
        new(nextInChain : nativeint, sType : SType, handle : nativeint, useKeyedMutex : int) = { NextInChain = nextInChain; SType = sType; Handle = handle; UseKeyedMutex = useKeyedMutex }
        new(handle : nativeint, useKeyedMutex : int) = SharedTextureMemoryDXGISharedHandleDescriptor(0n, Unchecked.defaultof<SType>, handle, useKeyedMutex)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedTextureMemoryIOSurfaceDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public IoSurface : nativeint
        new(nextInChain : nativeint, sType : SType, ioSurface : nativeint) = { NextInChain = nextInChain; SType = sType; IoSurface = ioSurface }
        new(ioSurface : nativeint) = SharedTextureMemoryIOSurfaceDescriptor(0n, Unchecked.defaultof<SType>, ioSurface)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedTextureMemoryEGLImageDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Image : nativeint
        new(nextInChain : nativeint, sType : SType, image : nativeint) = { NextInChain = nextInChain; SType = sType; Image = image }
        new(image : nativeint) = SharedTextureMemoryEGLImageDescriptor(0n, Unchecked.defaultof<SType>, image)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedTextureMemoryBeginAccessDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public ConcurrentRead : int
        val mutable public Initialized : int
        val mutable public FenceCount : unativeint
        val mutable public Fences : nativeptr<nativeint>
        val mutable public SignaledValues : nativeptr<uint64>
        new(nextInChain : nativeint, concurrentRead : int, initialized : int, fenceCount : unativeint, fences : nativeptr<nativeint>, signaledValues : nativeptr<uint64>) = { NextInChain = nextInChain; ConcurrentRead = concurrentRead; Initialized = initialized; FenceCount = fenceCount; Fences = fences; SignaledValues = signaledValues }
        new(concurrentRead : int, initialized : int, fenceCount : unativeint, fences : nativeptr<nativeint>, signaledValues : nativeptr<uint64>) = SharedTextureMemoryBeginAccessDescriptor(0n, concurrentRead, initialized, fenceCount, fences, signaledValues)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedTextureMemoryEndAccessState = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Initialized : int
        val mutable public FenceCount : unativeint
        val mutable public Fences : nativeptr<nativeint>
        val mutable public SignaledValues : nativeptr<uint64>
        new(nextInChain : nativeint, initialized : int, fenceCount : unativeint, fences : nativeptr<nativeint>, signaledValues : nativeptr<uint64>) = { NextInChain = nextInChain; Initialized = initialized; FenceCount = fenceCount; Fences = fences; SignaledValues = signaledValues }
        new(initialized : int, fenceCount : unativeint, fences : nativeptr<nativeint>, signaledValues : nativeptr<uint64>) = SharedTextureMemoryEndAccessState(0n, initialized, fenceCount, fences, signaledValues)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedTextureMemoryVkImageLayoutBeginState = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public OldLayout : int
        val mutable public NewLayout : int
        new(nextInChain : nativeint, sType : SType, oldLayout : int, newLayout : int) = { NextInChain = nextInChain; SType = sType; OldLayout = oldLayout; NewLayout = newLayout }
        new(oldLayout : int, newLayout : int) = SharedTextureMemoryVkImageLayoutBeginState(0n, Unchecked.defaultof<SType>, oldLayout, newLayout)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedTextureMemoryVkImageLayoutEndState = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public OldLayout : int
        val mutable public NewLayout : int
        new(nextInChain : nativeint, sType : SType, oldLayout : int, newLayout : int) = { NextInChain = nextInChain; SType = sType; OldLayout = oldLayout; NewLayout = newLayout }
        new(oldLayout : int, newLayout : int) = SharedTextureMemoryVkImageLayoutEndState(0n, Unchecked.defaultof<SType>, oldLayout, newLayout)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedTextureMemoryD3DSwapchainBeginState = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public IsSwapchain : int
        new(nextInChain : nativeint, sType : SType, isSwapchain : int) = { NextInChain = nextInChain; SType = sType; IsSwapchain = isSwapchain }
        new(isSwapchain : int) = SharedTextureMemoryD3DSwapchainBeginState(0n, Unchecked.defaultof<SType>, isSwapchain)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedFenceDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        new(nextInChain : nativeint, label : StringView) = { NextInChain = nextInChain; Label = label }
        new(label : StringView) = SharedFenceDescriptor(0n, label)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedFenceVkSemaphoreOpaqueFDDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Handle : int
        new(nextInChain : nativeint, sType : SType, handle : int) = { NextInChain = nextInChain; SType = sType; Handle = handle }
        new(handle : int) = SharedFenceVkSemaphoreOpaqueFDDescriptor(0n, Unchecked.defaultof<SType>, handle)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedFenceSyncFDDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Handle : int
        new(nextInChain : nativeint, sType : SType, handle : int) = { NextInChain = nextInChain; SType = sType; Handle = handle }
        new(handle : int) = SharedFenceSyncFDDescriptor(0n, Unchecked.defaultof<SType>, handle)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedFenceVkSemaphoreZirconHandleDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Handle : uint32
        new(nextInChain : nativeint, sType : SType, handle : uint32) = { NextInChain = nextInChain; SType = sType; Handle = handle }
        new(handle : uint32) = SharedFenceVkSemaphoreZirconHandleDescriptor(0n, Unchecked.defaultof<SType>, handle)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedFenceDXGISharedHandleDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Handle : nativeint
        new(nextInChain : nativeint, sType : SType, handle : nativeint) = { NextInChain = nextInChain; SType = sType; Handle = handle }
        new(handle : nativeint) = SharedFenceDXGISharedHandleDescriptor(0n, Unchecked.defaultof<SType>, handle)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedFenceMTLSharedEventDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public SharedEvent : nativeint
        new(nextInChain : nativeint, sType : SType, sharedEvent : nativeint) = { NextInChain = nextInChain; SType = sType; SharedEvent = sharedEvent }
        new(sharedEvent : nativeint) = SharedFenceMTLSharedEventDescriptor(0n, Unchecked.defaultof<SType>, sharedEvent)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedFenceExportInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Type : SharedFenceType
        new(nextInChain : nativeint, typ : SharedFenceType) = { NextInChain = nextInChain; Type = typ }
        new(typ : SharedFenceType) = SharedFenceExportInfo(0n, typ)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedFenceVkSemaphoreOpaqueFDExportInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Handle : int
        new(nextInChain : nativeint, sType : SType, handle : int) = { NextInChain = nextInChain; SType = sType; Handle = handle }
        new(handle : int) = SharedFenceVkSemaphoreOpaqueFDExportInfo(0n, Unchecked.defaultof<SType>, handle)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedFenceSyncFDExportInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Handle : int
        new(nextInChain : nativeint, sType : SType, handle : int) = { NextInChain = nextInChain; SType = sType; Handle = handle }
        new(handle : int) = SharedFenceSyncFDExportInfo(0n, Unchecked.defaultof<SType>, handle)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedFenceVkSemaphoreZirconHandleExportInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Handle : uint32
        new(nextInChain : nativeint, sType : SType, handle : uint32) = { NextInChain = nextInChain; SType = sType; Handle = handle }
        new(handle : uint32) = SharedFenceVkSemaphoreZirconHandleExportInfo(0n, Unchecked.defaultof<SType>, handle)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedFenceDXGISharedHandleExportInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Handle : nativeint
        new(nextInChain : nativeint, sType : SType, handle : nativeint) = { NextInChain = nextInChain; SType = sType; Handle = handle }
        new(handle : nativeint) = SharedFenceDXGISharedHandleExportInfo(0n, Unchecked.defaultof<SType>, handle)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SharedFenceMTLSharedEventExportInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public SharedEvent : nativeint
        new(nextInChain : nativeint, sType : SType, sharedEvent : nativeint) = { NextInChain = nextInChain; SType = sType; SharedEvent = sharedEvent }
        new(sharedEvent : nativeint) = SharedFenceMTLSharedEventExportInfo(0n, Unchecked.defaultof<SType>, sharedEvent)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type FormatCapabilities = 
    struct
        val mutable public NextInChain : nativeint
        new(nextInChain : nativeint) = { NextInChain = nextInChain }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DrmFormatCapabilities = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public PropertiesCount : unativeint
        val mutable public Properties : nativeptr<DrmFormatProperties>
        new(nextInChain : nativeint, sType : SType, propertiesCount : unativeint, properties : nativeptr<DrmFormatProperties>) = { NextInChain = nextInChain; SType = sType; PropertiesCount = propertiesCount; Properties = properties }
        new(propertiesCount : unativeint, properties : nativeptr<DrmFormatProperties>) = DrmFormatCapabilities(0n, Unchecked.defaultof<SType>, propertiesCount, properties)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DrmFormatProperties = 
    struct
        val mutable public Modifier : uint64
        val mutable public ModifierPlaneCount : uint32
        new(modifier : uint64, modifierPlaneCount : uint32) = { Modifier = modifier; ModifierPlaneCount = modifierPlaneCount }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ImageCopyBuffer = 
    struct
        val mutable public Layout : TextureDataLayout
        val mutable public Buffer : nativeint
        new(layout : TextureDataLayout, buffer : nativeint) = { Layout = layout; Buffer = buffer }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ImageCopyTexture = 
    struct
        val mutable public Texture : nativeint
        val mutable public MipLevel : uint32
        val mutable public Origin : Origin3D
        val mutable public Aspect : TextureAspect
        new(texture : nativeint, mipLevel : uint32, origin : Origin3D, aspect : TextureAspect) = { Texture = texture; MipLevel = mipLevel; Origin = origin; Aspect = aspect }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ImageCopyExternalTexture = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public ExternalTexture : nativeint
        val mutable public Origin : Origin3D
        val mutable public NaturalSize : Extent2D
        new(nextInChain : nativeint, externalTexture : nativeint, origin : Origin3D, naturalSize : Extent2D) = { NextInChain = nextInChain; ExternalTexture = externalTexture; Origin = origin; NaturalSize = naturalSize }
        new(externalTexture : nativeint, origin : Origin3D, naturalSize : Extent2D) = ImageCopyExternalTexture(0n, externalTexture, origin, naturalSize)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type Future = 
    struct
        val mutable public Id : uint64
        new(id : uint64) = { Id = id }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type FutureWaitInfo = 
    struct
        val mutable public Future : Future
        val mutable public Completed : int
        new(future : Future, completed : int) = { Future = future; Completed = completed }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type InstanceFeatures = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public TimedWaitAnyEnable : int
        val mutable public TimedWaitAnyMaxCount : unativeint
        new(nextInChain : nativeint, timedWaitAnyEnable : int, timedWaitAnyMaxCount : unativeint) = { NextInChain = nextInChain; TimedWaitAnyEnable = timedWaitAnyEnable; TimedWaitAnyMaxCount = timedWaitAnyMaxCount }
        new(timedWaitAnyEnable : int, timedWaitAnyMaxCount : unativeint) = InstanceFeatures(0n, timedWaitAnyEnable, timedWaitAnyMaxCount)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type InstanceDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Features : InstanceFeatures
        new(nextInChain : nativeint, features : InstanceFeatures) = { NextInChain = nextInChain; Features = features }
        new(features : InstanceFeatures) = InstanceDescriptor(0n, features)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnWireWGSLControl = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public EnableExperimental : int
        val mutable public EnableUnsafe : int
        val mutable public EnableTesting : int
        new(nextInChain : nativeint, sType : SType, enableExperimental : int, enableUnsafe : int, enableTesting : int) = { NextInChain = nextInChain; SType = sType; EnableExperimental = enableExperimental; EnableUnsafe = enableUnsafe; EnableTesting = enableTesting }
        new(enableExperimental : int, enableUnsafe : int, enableTesting : int) = DawnWireWGSLControl(0n, Unchecked.defaultof<SType>, enableExperimental, enableUnsafe, enableTesting)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type VertexAttribute = 
    struct
        val mutable public Format : VertexFormat
        val mutable public Offset : uint64
        val mutable public ShaderLocation : uint32
        new(format : VertexFormat, offset : uint64, shaderLocation : uint32) = { Format = format; Offset = offset; ShaderLocation = shaderLocation }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type VertexBufferLayout = 
    struct
        val mutable public ArrayStride : uint64
        val mutable public StepMode : VertexStepMode
        val mutable public AttributeCount : unativeint
        val mutable public Attributes : nativeptr<VertexAttribute>
        new(arrayStride : uint64, stepMode : VertexStepMode, attributeCount : unativeint, attributes : nativeptr<VertexAttribute>) = { ArrayStride = arrayStride; StepMode = stepMode; AttributeCount = attributeCount; Attributes = attributes }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type Origin3D = 
    struct
        val mutable public X : uint32
        val mutable public Y : uint32
        val mutable public Z : uint32
        new(x : uint32, y : uint32, z : uint32) = { X = x; Y = y; Z = z }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type Origin2D = 
    struct
        val mutable public X : uint32
        val mutable public Y : uint32
        new(x : uint32, y : uint32) = { X = x; Y = y }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type PipelineLayoutDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        val mutable public BindGroupLayoutCount : unativeint
        val mutable public BindGroupLayouts : nativeptr<nativeint>
        val mutable public ImmediateDataRangeByteSize : uint32
        new(nextInChain : nativeint, label : StringView, bindGroupLayoutCount : unativeint, bindGroupLayouts : nativeptr<nativeint>, immediateDataRangeByteSize : uint32) = { NextInChain = nextInChain; Label = label; BindGroupLayoutCount = bindGroupLayoutCount; BindGroupLayouts = bindGroupLayouts; ImmediateDataRangeByteSize = immediateDataRangeByteSize }
        new(label : StringView, bindGroupLayoutCount : unativeint, bindGroupLayouts : nativeptr<nativeint>, immediateDataRangeByteSize : uint32) = PipelineLayoutDescriptor(0n, label, bindGroupLayoutCount, bindGroupLayouts, immediateDataRangeByteSize)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type PipelineLayoutPixelLocalStorage = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public TotalPixelLocalStorageSize : uint64
        val mutable public StorageAttachmentCount : unativeint
        val mutable public StorageAttachments : nativeptr<PipelineLayoutStorageAttachment>
        new(nextInChain : nativeint, sType : SType, totalPixelLocalStorageSize : uint64, storageAttachmentCount : unativeint, storageAttachments : nativeptr<PipelineLayoutStorageAttachment>) = { NextInChain = nextInChain; SType = sType; TotalPixelLocalStorageSize = totalPixelLocalStorageSize; StorageAttachmentCount = storageAttachmentCount; StorageAttachments = storageAttachments }
        new(totalPixelLocalStorageSize : uint64, storageAttachmentCount : unativeint, storageAttachments : nativeptr<PipelineLayoutStorageAttachment>) = PipelineLayoutPixelLocalStorage(0n, Unchecked.defaultof<SType>, totalPixelLocalStorageSize, storageAttachmentCount, storageAttachments)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type PipelineLayoutStorageAttachment = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Offset : uint64
        val mutable public Format : TextureFormat
        new(nextInChain : nativeint, offset : uint64, format : TextureFormat) = { NextInChain = nextInChain; Offset = offset; Format = format }
        new(offset : uint64, format : TextureFormat) = PipelineLayoutStorageAttachment(0n, offset, format)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ProgrammableStageDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Module : nativeint
        val mutable public EntryPoint : StringView
        val mutable public ConstantCount : unativeint
        val mutable public Constants : nativeptr<ConstantEntry>
        new(nextInChain : nativeint, moodule : nativeint, entryPoint : StringView, constantCount : unativeint, constants : nativeptr<ConstantEntry>) = { NextInChain = nextInChain; Module = moodule; EntryPoint = entryPoint; ConstantCount = constantCount; Constants = constants }
        new(moodule : nativeint, entryPoint : StringView, constantCount : unativeint, constants : nativeptr<ConstantEntry>) = ProgrammableStageDescriptor(0n, moodule, entryPoint, constantCount, constants)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type QuerySetDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        val mutable public Type : QueryType
        val mutable public Count : uint32
        new(nextInChain : nativeint, label : StringView, typ : QueryType, count : uint32) = { NextInChain = nextInChain; Label = label; Type = typ; Count = count }
        new(label : StringView, typ : QueryType, count : uint32) = QuerySetDescriptor(0n, label, typ, count)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type QueueDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        new(nextInChain : nativeint, label : StringView) = { NextInChain = nextInChain; Label = label }
        new(label : StringView) = QueueDescriptor(0n, label)
    end
type QueueWorkDoneCallback = delegate of status : QueueWorkDoneStatus * userdata : nativeint -> unit
type QueueWorkDoneCallback2 = delegate of status : QueueWorkDoneStatus * userdata1 : nativeint * userdata2 : nativeint -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type QueueWorkDoneCallbackInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        val mutable public Userdata : nativeint
        new(nextInChain : nativeint, mode : CallbackMode, callback : nativeint, userdata : nativeint) = { NextInChain = nextInChain; Mode = mode; Callback = callback; Userdata = userdata }
        new(mode : CallbackMode, callback : nativeint, userdata : nativeint) = QueueWorkDoneCallbackInfo(0n, mode, callback, userdata)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type QueueWorkDoneCallbackInfo2 = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        val mutable public Userdata1 : nativeint
        val mutable public Userdata2 : nativeint
        new(nextInChain : nativeint, mode : CallbackMode, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = { NextInChain = nextInChain; Mode = mode; Callback = callback; Userdata1 = userdata1; Userdata2 = userdata2 }
        new(mode : CallbackMode, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = QueueWorkDoneCallbackInfo2(0n, mode, callback, userdata1, userdata2)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RenderBundleDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        new(nextInChain : nativeint, label : StringView) = { NextInChain = nextInChain; Label = label }
        new(label : StringView) = RenderBundleDescriptor(0n, label)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RenderBundleEncoderDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        val mutable public ColorFormatCount : unativeint
        val mutable public ColorFormats : nativeptr<TextureFormat>
        val mutable public DepthStencilFormat : TextureFormat
        val mutable public SampleCount : uint32
        val mutable public DepthReadOnly : int
        val mutable public StencilReadOnly : int
        new(nextInChain : nativeint, label : StringView, colorFormatCount : unativeint, colorFormats : nativeptr<TextureFormat>, depthStencilFormat : TextureFormat, sampleCount : uint32, depthReadOnly : int, stencilReadOnly : int) = { NextInChain = nextInChain; Label = label; ColorFormatCount = colorFormatCount; ColorFormats = colorFormats; DepthStencilFormat = depthStencilFormat; SampleCount = sampleCount; DepthReadOnly = depthReadOnly; StencilReadOnly = stencilReadOnly }
        new(label : StringView, colorFormatCount : unativeint, colorFormats : nativeptr<TextureFormat>, depthStencilFormat : TextureFormat, sampleCount : uint32, depthReadOnly : int, stencilReadOnly : int) = RenderBundleEncoderDescriptor(0n, label, colorFormatCount, colorFormats, depthStencilFormat, sampleCount, depthReadOnly, stencilReadOnly)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RenderPassColorAttachment = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public View : nativeint
        val mutable public DepthSlice : uint32
        val mutable public ResolveTarget : nativeint
        val mutable public LoadOp : LoadOp
        val mutable public StoreOp : StoreOp
        val mutable public ClearValue : Color
        new(nextInChain : nativeint, view : nativeint, depthSlice : uint32, resolveTarget : nativeint, loadOp : LoadOp, storeOp : StoreOp, clearValue : Color) = { NextInChain = nextInChain; View = view; DepthSlice = depthSlice; ResolveTarget = resolveTarget; LoadOp = loadOp; StoreOp = storeOp; ClearValue = clearValue }
        new(view : nativeint, depthSlice : uint32, resolveTarget : nativeint, loadOp : LoadOp, storeOp : StoreOp, clearValue : Color) = RenderPassColorAttachment(0n, view, depthSlice, resolveTarget, loadOp, storeOp, clearValue)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnRenderPassColorAttachmentRenderToSingleSampled = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public ImplicitSampleCount : uint32
        new(nextInChain : nativeint, sType : SType, implicitSampleCount : uint32) = { NextInChain = nextInChain; SType = sType; ImplicitSampleCount = implicitSampleCount }
        new(implicitSampleCount : uint32) = DawnRenderPassColorAttachmentRenderToSingleSampled(0n, Unchecked.defaultof<SType>, implicitSampleCount)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RenderPassDepthStencilAttachment = 
    struct
        val mutable public View : nativeint
        val mutable public DepthLoadOp : LoadOp
        val mutable public DepthStoreOp : StoreOp
        val mutable public DepthClearValue : float32
        val mutable public DepthReadOnly : int
        val mutable public StencilLoadOp : LoadOp
        val mutable public StencilStoreOp : StoreOp
        val mutable public StencilClearValue : uint32
        val mutable public StencilReadOnly : int
        new(view : nativeint, depthLoadOp : LoadOp, depthStoreOp : StoreOp, depthClearValue : float32, depthReadOnly : int, stencilLoadOp : LoadOp, stencilStoreOp : StoreOp, stencilClearValue : uint32, stencilReadOnly : int) = { View = view; DepthLoadOp = depthLoadOp; DepthStoreOp = depthStoreOp; DepthClearValue = depthClearValue; DepthReadOnly = depthReadOnly; StencilLoadOp = stencilLoadOp; StencilStoreOp = stencilStoreOp; StencilClearValue = stencilClearValue; StencilReadOnly = stencilReadOnly }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RenderPassDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        val mutable public ColorAttachmentCount : unativeint
        val mutable public ColorAttachments : nativeptr<RenderPassColorAttachment>
        val mutable public DepthStencilAttachment : nativeptr<RenderPassDepthStencilAttachment>
        val mutable public OcclusionQuerySet : nativeint
        val mutable public TimestampWrites : nativeptr<RenderPassTimestampWrites>
        new(nextInChain : nativeint, label : StringView, colorAttachmentCount : unativeint, colorAttachments : nativeptr<RenderPassColorAttachment>, depthStencilAttachment : nativeptr<RenderPassDepthStencilAttachment>, occlusionQuerySet : nativeint, timestampWrites : nativeptr<RenderPassTimestampWrites>) = { NextInChain = nextInChain; Label = label; ColorAttachmentCount = colorAttachmentCount; ColorAttachments = colorAttachments; DepthStencilAttachment = depthStencilAttachment; OcclusionQuerySet = occlusionQuerySet; TimestampWrites = timestampWrites }
        new(label : StringView, colorAttachmentCount : unativeint, colorAttachments : nativeptr<RenderPassColorAttachment>, depthStencilAttachment : nativeptr<RenderPassDepthStencilAttachment>, occlusionQuerySet : nativeint, timestampWrites : nativeptr<RenderPassTimestampWrites>) = RenderPassDescriptor(0n, label, colorAttachmentCount, colorAttachments, depthStencilAttachment, occlusionQuerySet, timestampWrites)
    end
type RenderPassDescriptorMaxDrawCount = RenderPassMaxDrawCount
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RenderPassMaxDrawCount = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public MaxDrawCount : uint64
        new(nextInChain : nativeint, sType : SType, maxDrawCount : uint64) = { NextInChain = nextInChain; SType = sType; MaxDrawCount = maxDrawCount }
        new(maxDrawCount : uint64) = RenderPassMaxDrawCount(0n, Unchecked.defaultof<SType>, maxDrawCount)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RenderPassDescriptorExpandResolveRect = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public X : uint32
        val mutable public Y : uint32
        val mutable public Width : uint32
        val mutable public Height : uint32
        new(nextInChain : nativeint, sType : SType, x : uint32, y : uint32, width : uint32, height : uint32) = { NextInChain = nextInChain; SType = sType; X = x; Y = y; Width = width; Height = height }
        new(x : uint32, y : uint32, width : uint32, height : uint32) = RenderPassDescriptorExpandResolveRect(0n, Unchecked.defaultof<SType>, x, y, width, height)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RenderPassPixelLocalStorage = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public TotalPixelLocalStorageSize : uint64
        val mutable public StorageAttachmentCount : unativeint
        val mutable public StorageAttachments : nativeptr<RenderPassStorageAttachment>
        new(nextInChain : nativeint, sType : SType, totalPixelLocalStorageSize : uint64, storageAttachmentCount : unativeint, storageAttachments : nativeptr<RenderPassStorageAttachment>) = { NextInChain = nextInChain; SType = sType; TotalPixelLocalStorageSize = totalPixelLocalStorageSize; StorageAttachmentCount = storageAttachmentCount; StorageAttachments = storageAttachments }
        new(totalPixelLocalStorageSize : uint64, storageAttachmentCount : unativeint, storageAttachments : nativeptr<RenderPassStorageAttachment>) = RenderPassPixelLocalStorage(0n, Unchecked.defaultof<SType>, totalPixelLocalStorageSize, storageAttachmentCount, storageAttachments)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RenderPassStorageAttachment = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Offset : uint64
        val mutable public Storage : nativeint
        val mutable public LoadOp : LoadOp
        val mutable public StoreOp : StoreOp
        val mutable public ClearValue : Color
        new(nextInChain : nativeint, offset : uint64, storage : nativeint, loadOp : LoadOp, storeOp : StoreOp, clearValue : Color) = { NextInChain = nextInChain; Offset = offset; Storage = storage; LoadOp = loadOp; StoreOp = storeOp; ClearValue = clearValue }
        new(offset : uint64, storage : nativeint, loadOp : LoadOp, storeOp : StoreOp, clearValue : Color) = RenderPassStorageAttachment(0n, offset, storage, loadOp, storeOp, clearValue)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RenderPassTimestampWrites = 
    struct
        val mutable public QuerySet : nativeint
        val mutable public BeginningOfPassWriteIndex : uint32
        val mutable public EndOfPassWriteIndex : uint32
        new(querySet : nativeint, beginningOfPassWriteIndex : uint32, endOfPassWriteIndex : uint32) = { QuerySet = querySet; BeginningOfPassWriteIndex = beginningOfPassWriteIndex; EndOfPassWriteIndex = endOfPassWriteIndex }
    end
type RequestDeviceCallback = delegate of status : RequestDeviceStatus * device : nativeint * message : StringView * userdata : nativeint -> unit
type RequestDeviceCallback2 = delegate of status : RequestDeviceStatus * device : nativeint * message : StringView * userdata1 : nativeint * userdata2 : nativeint -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RequestDeviceCallbackInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        val mutable public Userdata : nativeint
        new(nextInChain : nativeint, mode : CallbackMode, callback : nativeint, userdata : nativeint) = { NextInChain = nextInChain; Mode = mode; Callback = callback; Userdata = userdata }
        new(mode : CallbackMode, callback : nativeint, userdata : nativeint) = RequestDeviceCallbackInfo(0n, mode, callback, userdata)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RequestDeviceCallbackInfo2 = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        val mutable public Userdata1 : nativeint
        val mutable public Userdata2 : nativeint
        new(nextInChain : nativeint, mode : CallbackMode, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = { NextInChain = nextInChain; Mode = mode; Callback = callback; Userdata1 = userdata1; Userdata2 = userdata2 }
        new(mode : CallbackMode, callback : nativeint, userdata1 : nativeint, userdata2 : nativeint) = RequestDeviceCallbackInfo2(0n, mode, callback, userdata1, userdata2)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type VertexState = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Module : nativeint
        val mutable public EntryPoint : StringView
        val mutable public ConstantCount : unativeint
        val mutable public Constants : nativeptr<ConstantEntry>
        val mutable public BufferCount : unativeint
        val mutable public Buffers : nativeptr<VertexBufferLayout>
        new(nextInChain : nativeint, moodule : nativeint, entryPoint : StringView, constantCount : unativeint, constants : nativeptr<ConstantEntry>, bufferCount : unativeint, buffers : nativeptr<VertexBufferLayout>) = { NextInChain = nextInChain; Module = moodule; EntryPoint = entryPoint; ConstantCount = constantCount; Constants = constants; BufferCount = bufferCount; Buffers = buffers }
        new(moodule : nativeint, entryPoint : StringView, constantCount : unativeint, constants : nativeptr<ConstantEntry>, bufferCount : unativeint, buffers : nativeptr<VertexBufferLayout>) = VertexState(0n, moodule, entryPoint, constantCount, constants, bufferCount, buffers)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type PrimitiveState = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Topology : PrimitiveTopology
        val mutable public StripIndexFormat : IndexFormat
        val mutable public FrontFace : FrontFace
        val mutable public CullMode : CullMode
        val mutable public UnclippedDepth : int
        new(nextInChain : nativeint, topology : PrimitiveTopology, stripIndexFormat : IndexFormat, frontFace : FrontFace, cullMode : CullMode, unclippedDepth : int) = { NextInChain = nextInChain; Topology = topology; StripIndexFormat = stripIndexFormat; FrontFace = frontFace; CullMode = cullMode; UnclippedDepth = unclippedDepth }
        new(topology : PrimitiveTopology, stripIndexFormat : IndexFormat, frontFace : FrontFace, cullMode : CullMode, unclippedDepth : int) = PrimitiveState(0n, topology, stripIndexFormat, frontFace, cullMode, unclippedDepth)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DepthStencilState = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Format : TextureFormat
        val mutable public DepthWriteEnabled : OptionalBool
        val mutable public DepthCompare : CompareFunction
        val mutable public StencilFront : StencilFaceState
        val mutable public StencilBack : StencilFaceState
        val mutable public StencilReadMask : uint32
        val mutable public StencilWriteMask : uint32
        val mutable public DepthBias : int
        val mutable public DepthBiasSlopeScale : float32
        val mutable public DepthBiasClamp : float32
        new(nextInChain : nativeint, format : TextureFormat, depthWriteEnabled : OptionalBool, depthCompare : CompareFunction, stencilFront : StencilFaceState, stencilBack : StencilFaceState, stencilReadMask : uint32, stencilWriteMask : uint32, depthBias : int, depthBiasSlopeScale : float32, depthBiasClamp : float32) = { NextInChain = nextInChain; Format = format; DepthWriteEnabled = depthWriteEnabled; DepthCompare = depthCompare; StencilFront = stencilFront; StencilBack = stencilBack; StencilReadMask = stencilReadMask; StencilWriteMask = stencilWriteMask; DepthBias = depthBias; DepthBiasSlopeScale = depthBiasSlopeScale; DepthBiasClamp = depthBiasClamp }
        new(format : TextureFormat, depthWriteEnabled : OptionalBool, depthCompare : CompareFunction, stencilFront : StencilFaceState, stencilBack : StencilFaceState, stencilReadMask : uint32, stencilWriteMask : uint32, depthBias : int, depthBiasSlopeScale : float32, depthBiasClamp : float32) = DepthStencilState(0n, format, depthWriteEnabled, depthCompare, stencilFront, stencilBack, stencilReadMask, stencilWriteMask, depthBias, depthBiasSlopeScale, depthBiasClamp)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type MultisampleState = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Count : uint32
        val mutable public Mask : uint32
        val mutable public AlphaToCoverageEnabled : int
        new(nextInChain : nativeint, count : uint32, mask : uint32, alphaToCoverageEnabled : int) = { NextInChain = nextInChain; Count = count; Mask = mask; AlphaToCoverageEnabled = alphaToCoverageEnabled }
        new(count : uint32, mask : uint32, alphaToCoverageEnabled : int) = MultisampleState(0n, count, mask, alphaToCoverageEnabled)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type FragmentState = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Module : nativeint
        val mutable public EntryPoint : StringView
        val mutable public ConstantCount : unativeint
        val mutable public Constants : nativeptr<ConstantEntry>
        val mutable public TargetCount : unativeint
        val mutable public Targets : nativeptr<ColorTargetState>
        new(nextInChain : nativeint, moodule : nativeint, entryPoint : StringView, constantCount : unativeint, constants : nativeptr<ConstantEntry>, targetCount : unativeint, targets : nativeptr<ColorTargetState>) = { NextInChain = nextInChain; Module = moodule; EntryPoint = entryPoint; ConstantCount = constantCount; Constants = constants; TargetCount = targetCount; Targets = targets }
        new(moodule : nativeint, entryPoint : StringView, constantCount : unativeint, constants : nativeptr<ConstantEntry>, targetCount : unativeint, targets : nativeptr<ColorTargetState>) = FragmentState(0n, moodule, entryPoint, constantCount, constants, targetCount, targets)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ColorTargetState = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Format : TextureFormat
        val mutable public Blend : nativeptr<BlendState>
        val mutable public WriteMask : ColorWriteMask
        new(nextInChain : nativeint, format : TextureFormat, blend : nativeptr<BlendState>, writeMask : ColorWriteMask) = { NextInChain = nextInChain; Format = format; Blend = blend; WriteMask = writeMask }
        new(format : TextureFormat, blend : nativeptr<BlendState>, writeMask : ColorWriteMask) = ColorTargetState(0n, format, blend, writeMask)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ColorTargetStateExpandResolveTextureDawn = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Enabled : int
        new(nextInChain : nativeint, sType : SType, enabled : int) = { NextInChain = nextInChain; SType = sType; Enabled = enabled }
        new(enabled : int) = ColorTargetStateExpandResolveTextureDawn(0n, Unchecked.defaultof<SType>, enabled)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type BlendState = 
    struct
        val mutable public Color : BlendComponent
        val mutable public Alpha : BlendComponent
        new(color : BlendComponent, alpha : BlendComponent) = { Color = color; Alpha = alpha }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RenderPipelineDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        val mutable public Layout : nativeint
        val mutable public Vertex : VertexState
        val mutable public Primitive : PrimitiveState
        val mutable public DepthStencil : nativeptr<DepthStencilState>
        val mutable public Multisample : MultisampleState
        val mutable public Fragment : nativeptr<FragmentState>
        new(nextInChain : nativeint, label : StringView, layout : nativeint, vertex : VertexState, primitive : PrimitiveState, depthStencil : nativeptr<DepthStencilState>, multisample : MultisampleState, fragment : nativeptr<FragmentState>) = { NextInChain = nextInChain; Label = label; Layout = layout; Vertex = vertex; Primitive = primitive; DepthStencil = depthStencil; Multisample = multisample; Fragment = fragment }
        new(label : StringView, layout : nativeint, vertex : VertexState, primitive : PrimitiveState, depthStencil : nativeptr<DepthStencilState>, multisample : MultisampleState, fragment : nativeptr<FragmentState>) = RenderPipelineDescriptor(0n, label, layout, vertex, primitive, depthStencil, multisample, fragment)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SamplerDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        val mutable public AddressModeU : AddressMode
        val mutable public AddressModeV : AddressMode
        val mutable public AddressModeW : AddressMode
        val mutable public MagFilter : FilterMode
        val mutable public MinFilter : FilterMode
        val mutable public MipmapFilter : MipmapFilterMode
        val mutable public LodMinClamp : float32
        val mutable public LodMaxClamp : float32
        val mutable public Compare : CompareFunction
        val mutable public MaxAnisotropy : uint16
        new(nextInChain : nativeint, label : StringView, addressModeU : AddressMode, addressModeV : AddressMode, addressModeW : AddressMode, magFilter : FilterMode, minFilter : FilterMode, mipmapFilter : MipmapFilterMode, lodMinClamp : float32, lodMaxClamp : float32, compare : CompareFunction, maxAnisotropy : uint16) = { NextInChain = nextInChain; Label = label; AddressModeU = addressModeU; AddressModeV = addressModeV; AddressModeW = addressModeW; MagFilter = magFilter; MinFilter = minFilter; MipmapFilter = mipmapFilter; LodMinClamp = lodMinClamp; LodMaxClamp = lodMaxClamp; Compare = compare; MaxAnisotropy = maxAnisotropy }
        new(label : StringView, addressModeU : AddressMode, addressModeV : AddressMode, addressModeW : AddressMode, magFilter : FilterMode, minFilter : FilterMode, mipmapFilter : MipmapFilterMode, lodMinClamp : float32, lodMaxClamp : float32, compare : CompareFunction, maxAnisotropy : uint16) = SamplerDescriptor(0n, label, addressModeU, addressModeV, addressModeW, magFilter, minFilter, mipmapFilter, lodMinClamp, lodMaxClamp, compare, maxAnisotropy)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ShaderModuleDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        new(nextInChain : nativeint, label : StringView) = { NextInChain = nextInChain; Label = label }
        new(label : StringView) = ShaderModuleDescriptor(0n, label)
    end
type ShaderModuleSPIRVDescriptor = ShaderSourceSPIRV
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ShaderSourceSPIRV = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public CodeSize : uint32
        val mutable public Code : nativeptr<uint32>
        new(nextInChain : nativeint, sType : SType, codeSize : uint32, code : nativeptr<uint32>) = { NextInChain = nextInChain; SType = sType; CodeSize = codeSize; Code = code }
        new(codeSize : uint32, code : nativeptr<uint32>) = ShaderSourceSPIRV(0n, Unchecked.defaultof<SType>, codeSize, code)
    end
type ShaderModuleWGSLDescriptor = ShaderSourceWGSL
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ShaderSourceWGSL = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Code : StringView
        new(nextInChain : nativeint, sType : SType, code : StringView) = { NextInChain = nextInChain; SType = sType; Code = code }
        new(code : StringView) = ShaderSourceWGSL(0n, Unchecked.defaultof<SType>, code)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnShaderModuleSPIRVOptionsDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public AllowNonUniformDerivatives : int
        new(nextInChain : nativeint, sType : SType, allowNonUniformDerivatives : int) = { NextInChain = nextInChain; SType = sType; AllowNonUniformDerivatives = allowNonUniformDerivatives }
        new(allowNonUniformDerivatives : int) = DawnShaderModuleSPIRVOptionsDescriptor(0n, Unchecked.defaultof<SType>, allowNonUniformDerivatives)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ShaderModuleCompilationOptions = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public StrictMath : int
        new(nextInChain : nativeint, sType : SType, strictMath : int) = { NextInChain = nextInChain; SType = sType; StrictMath = strictMath }
        new(strictMath : int) = ShaderModuleCompilationOptions(0n, Unchecked.defaultof<SType>, strictMath)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type StencilFaceState = 
    struct
        val mutable public Compare : CompareFunction
        val mutable public FailOp : StencilOperation
        val mutable public DepthFailOp : StencilOperation
        val mutable public PassOp : StencilOperation
        new(compare : CompareFunction, failOp : StencilOperation, depthFailOp : StencilOperation, passOp : StencilOperation) = { Compare = compare; FailOp = failOp; DepthFailOp = depthFailOp; PassOp = passOp }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        new(nextInChain : nativeint, label : StringView) = { NextInChain = nextInChain; Label = label }
        new(label : StringView) = SurfaceDescriptor(0n, label)
    end
type SurfaceDescriptorFromAndroidNativeWindow = SurfaceSourceAndroidNativeWindow
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceSourceAndroidNativeWindow = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Window : nativeint
        new(nextInChain : nativeint, sType : SType, window : nativeint) = { NextInChain = nextInChain; SType = sType; Window = window }
        new(window : nativeint) = SurfaceSourceAndroidNativeWindow(0n, Unchecked.defaultof<SType>, window)
    end
type SurfaceDescriptorFromCanvasHTMLSelector = SurfaceSourceCanvasHTMLSelector_Emscripten
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceSourceCanvasHTMLSelector_Emscripten = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Selector : StringView
        new(nextInChain : nativeint, sType : SType, selector : StringView) = { NextInChain = nextInChain; SType = sType; Selector = selector }
        new(selector : StringView) = SurfaceSourceCanvasHTMLSelector_Emscripten(0n, Unchecked.defaultof<SType>, selector)
    end
type SurfaceDescriptorFromMetalLayer = SurfaceSourceMetalLayer
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceSourceMetalLayer = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Layer : nativeint
        new(nextInChain : nativeint, sType : SType, layer : nativeint) = { NextInChain = nextInChain; SType = sType; Layer = layer }
        new(layer : nativeint) = SurfaceSourceMetalLayer(0n, Unchecked.defaultof<SType>, layer)
    end
type SurfaceDescriptorFromWindowsHWND = SurfaceSourceWindowsHWND
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceSourceWindowsHWND = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Hinstance : nativeint
        val mutable public Hwnd : nativeint
        new(nextInChain : nativeint, sType : SType, hinstance : nativeint, hwnd : nativeint) = { NextInChain = nextInChain; SType = sType; Hinstance = hinstance; Hwnd = hwnd }
        new(hinstance : nativeint, hwnd : nativeint) = SurfaceSourceWindowsHWND(0n, Unchecked.defaultof<SType>, hinstance, hwnd)
    end
type SurfaceDescriptorFromXcbWindow = SurfaceSourceXCBWindow
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceSourceXCBWindow = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Connection : nativeint
        val mutable public Window : uint32
        new(nextInChain : nativeint, sType : SType, connection : nativeint, window : uint32) = { NextInChain = nextInChain; SType = sType; Connection = connection; Window = window }
        new(connection : nativeint, window : uint32) = SurfaceSourceXCBWindow(0n, Unchecked.defaultof<SType>, connection, window)
    end
type SurfaceDescriptorFromXlibWindow = SurfaceSourceXlibWindow
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceSourceXlibWindow = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Display : nativeint
        val mutable public Window : uint64
        new(nextInChain : nativeint, sType : SType, display : nativeint, window : uint64) = { NextInChain = nextInChain; SType = sType; Display = display; Window = window }
        new(display : nativeint, window : uint64) = SurfaceSourceXlibWindow(0n, Unchecked.defaultof<SType>, display, window)
    end
type SurfaceDescriptorFromWaylandSurface = SurfaceSourceWaylandSurface
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceSourceWaylandSurface = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Display : nativeint
        val mutable public Surface : nativeint
        new(nextInChain : nativeint, sType : SType, display : nativeint, surface : nativeint) = { NextInChain = nextInChain; SType = sType; Display = display; Surface = surface }
        new(display : nativeint, surface : nativeint) = SurfaceSourceWaylandSurface(0n, Unchecked.defaultof<SType>, display, surface)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceDescriptorFromWindowsCoreWindow = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public CoreWindow : nativeint
        new(nextInChain : nativeint, sType : SType, coreWindow : nativeint) = { NextInChain = nextInChain; SType = sType; CoreWindow = coreWindow }
        new(coreWindow : nativeint) = SurfaceDescriptorFromWindowsCoreWindow(0n, Unchecked.defaultof<SType>, coreWindow)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceDescriptorFromWindowsSwapChainPanel = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public SwapChainPanel : nativeint
        new(nextInChain : nativeint, sType : SType, swapChainPanel : nativeint) = { NextInChain = nextInChain; SType = sType; SwapChainPanel = swapChainPanel }
        new(swapChainPanel : nativeint) = SurfaceDescriptorFromWindowsSwapChainPanel(0n, Unchecked.defaultof<SType>, swapChainPanel)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceTexture = 
    struct
        val mutable public Texture : nativeint
        val mutable public Suboptimal : int
        val mutable public Status : SurfaceGetCurrentTextureStatus
        new(texture : nativeint, suboptimal : int, status : SurfaceGetCurrentTextureStatus) = { Texture = texture; Suboptimal = suboptimal; Status = status }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type TextureDataLayout = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Offset : uint64
        val mutable public BytesPerRow : uint32
        val mutable public RowsPerImage : uint32
        new(nextInChain : nativeint, offset : uint64, bytesPerRow : uint32, rowsPerImage : uint32) = { NextInChain = nextInChain; Offset = offset; BytesPerRow = bytesPerRow; RowsPerImage = rowsPerImage }
        new(offset : uint64, bytesPerRow : uint32, rowsPerImage : uint32) = TextureDataLayout(0n, offset, bytesPerRow, rowsPerImage)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type TextureDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        val mutable public Usage : TextureUsage
        val mutable public Dimension : TextureDimension
        val mutable public Size : Extent3D
        val mutable public Format : TextureFormat
        val mutable public MipLevelCount : uint32
        val mutable public SampleCount : uint32
        val mutable public ViewFormatCount : unativeint
        val mutable public ViewFormats : nativeptr<TextureFormat>
        new(nextInChain : nativeint, label : StringView, usage : TextureUsage, dimension : TextureDimension, size : Extent3D, format : TextureFormat, mipLevelCount : uint32, sampleCount : uint32, viewFormatCount : unativeint, viewFormats : nativeptr<TextureFormat>) = { NextInChain = nextInChain; Label = label; Usage = usage; Dimension = dimension; Size = size; Format = format; MipLevelCount = mipLevelCount; SampleCount = sampleCount; ViewFormatCount = viewFormatCount; ViewFormats = viewFormats }
        new(label : StringView, usage : TextureUsage, dimension : TextureDimension, size : Extent3D, format : TextureFormat, mipLevelCount : uint32, sampleCount : uint32, viewFormatCount : unativeint, viewFormats : nativeptr<TextureFormat>) = TextureDescriptor(0n, label, usage, dimension, size, format, mipLevelCount, sampleCount, viewFormatCount, viewFormats)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type TextureBindingViewDimensionDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public TextureBindingViewDimension : TextureViewDimension
        new(nextInChain : nativeint, sType : SType, textureBindingViewDimension : TextureViewDimension) = { NextInChain = nextInChain; SType = sType; TextureBindingViewDimension = textureBindingViewDimension }
        new(textureBindingViewDimension : TextureViewDimension) = TextureBindingViewDimensionDescriptor(0n, Unchecked.defaultof<SType>, textureBindingViewDimension)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type TextureViewDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : StringView
        val mutable public Format : TextureFormat
        val mutable public Dimension : TextureViewDimension
        val mutable public BaseMipLevel : uint32
        val mutable public MipLevelCount : uint32
        val mutable public BaseArrayLayer : uint32
        val mutable public ArrayLayerCount : uint32
        val mutable public Aspect : TextureAspect
        val mutable public Usage : TextureUsage
        new(nextInChain : nativeint, label : StringView, format : TextureFormat, dimension : TextureViewDimension, baseMipLevel : uint32, mipLevelCount : uint32, baseArrayLayer : uint32, arrayLayerCount : uint32, aspect : TextureAspect, usage : TextureUsage) = { NextInChain = nextInChain; Label = label; Format = format; Dimension = dimension; BaseMipLevel = baseMipLevel; MipLevelCount = mipLevelCount; BaseArrayLayer = baseArrayLayer; ArrayLayerCount = arrayLayerCount; Aspect = aspect; Usage = usage }
        new(label : StringView, format : TextureFormat, dimension : TextureViewDimension, baseMipLevel : uint32, mipLevelCount : uint32, baseArrayLayer : uint32, arrayLayerCount : uint32, aspect : TextureAspect, usage : TextureUsage) = TextureViewDescriptor(0n, label, format, dimension, baseMipLevel, mipLevelCount, baseArrayLayer, arrayLayerCount, aspect, usage)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type YCbCrVkDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public VkFormat : uint32
        val mutable public VkYCbCrModel : uint32
        val mutable public VkYCbCrRange : uint32
        val mutable public VkComponentSwizzleRed : uint32
        val mutable public VkComponentSwizzleGreen : uint32
        val mutable public VkComponentSwizzleBlue : uint32
        val mutable public VkComponentSwizzleAlpha : uint32
        val mutable public VkXChromaOffset : uint32
        val mutable public VkYChromaOffset : uint32
        val mutable public VkChromaFilter : FilterMode
        val mutable public ForceExplicitReconstruction : int
        val mutable public ExternalFormat : uint64
        new(nextInChain : nativeint, sType : SType, vkFormat : uint32, vkYCbCrModel : uint32, vkYCbCrRange : uint32, vkComponentSwizzleRed : uint32, vkComponentSwizzleGreen : uint32, vkComponentSwizzleBlue : uint32, vkComponentSwizzleAlpha : uint32, vkXChromaOffset : uint32, vkYChromaOffset : uint32, vkChromaFilter : FilterMode, forceExplicitReconstruction : int, externalFormat : uint64) = { NextInChain = nextInChain; SType = sType; VkFormat = vkFormat; VkYCbCrModel = vkYCbCrModel; VkYCbCrRange = vkYCbCrRange; VkComponentSwizzleRed = vkComponentSwizzleRed; VkComponentSwizzleGreen = vkComponentSwizzleGreen; VkComponentSwizzleBlue = vkComponentSwizzleBlue; VkComponentSwizzleAlpha = vkComponentSwizzleAlpha; VkXChromaOffset = vkXChromaOffset; VkYChromaOffset = vkYChromaOffset; VkChromaFilter = vkChromaFilter; ForceExplicitReconstruction = forceExplicitReconstruction; ExternalFormat = externalFormat }
        new(vkFormat : uint32, vkYCbCrModel : uint32, vkYCbCrRange : uint32, vkComponentSwizzleRed : uint32, vkComponentSwizzleGreen : uint32, vkComponentSwizzleBlue : uint32, vkComponentSwizzleAlpha : uint32, vkXChromaOffset : uint32, vkYChromaOffset : uint32, vkChromaFilter : FilterMode, forceExplicitReconstruction : int, externalFormat : uint64) = YCbCrVkDescriptor(0n, Unchecked.defaultof<SType>, vkFormat, vkYCbCrModel, vkYCbCrRange, vkComponentSwizzleRed, vkComponentSwizzleGreen, vkComponentSwizzleBlue, vkComponentSwizzleAlpha, vkXChromaOffset, vkYChromaOffset, vkChromaFilter, forceExplicitReconstruction, externalFormat)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnTextureInternalUsageDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public InternalUsage : TextureUsage
        new(nextInChain : nativeint, sType : SType, internalUsage : TextureUsage) = { NextInChain = nextInChain; SType = sType; InternalUsage = internalUsage }
        new(internalUsage : TextureUsage) = DawnTextureInternalUsageDescriptor(0n, Unchecked.defaultof<SType>, internalUsage)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnEncoderInternalUsageDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public UseInternalUsages : int
        new(nextInChain : nativeint, sType : SType, useInternalUsages : int) = { NextInChain = nextInChain; SType = sType; UseInternalUsages = useInternalUsages }
        new(useInternalUsages : int) = DawnEncoderInternalUsageDescriptor(0n, Unchecked.defaultof<SType>, useInternalUsages)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnAdapterPropertiesPowerPreference = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public PowerPreference : PowerPreference
        new(nextInChain : nativeint, sType : SType, powerPreference : PowerPreference) = { NextInChain = nextInChain; SType = sType; PowerPreference = powerPreference }
        new(powerPreference : PowerPreference) = DawnAdapterPropertiesPowerPreference(0n, Unchecked.defaultof<SType>, powerPreference)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type MemoryHeapInfo = 
    struct
        val mutable public Properties : HeapProperty
        val mutable public Size : uint64
        new(properties : HeapProperty, size : uint64) = { Properties = properties; Size = size }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type AdapterPropertiesMemoryHeaps = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public HeapCount : unativeint
        val mutable public HeapInfo : nativeptr<MemoryHeapInfo>
        new(nextInChain : nativeint, sType : SType, heapCount : unativeint, heapInfo : nativeptr<MemoryHeapInfo>) = { NextInChain = nextInChain; SType = sType; HeapCount = heapCount; HeapInfo = heapInfo }
        new(heapCount : unativeint, heapInfo : nativeptr<MemoryHeapInfo>) = AdapterPropertiesMemoryHeaps(0n, Unchecked.defaultof<SType>, heapCount, heapInfo)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type AdapterPropertiesD3D = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public ShaderModel : uint32
        new(nextInChain : nativeint, sType : SType, shaderModel : uint32) = { NextInChain = nextInChain; SType = sType; ShaderModel = shaderModel }
        new(shaderModel : uint32) = AdapterPropertiesD3D(0n, Unchecked.defaultof<SType>, shaderModel)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type AdapterPropertiesVk = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public DriverVersion : uint32
        new(nextInChain : nativeint, sType : SType, driverVersion : uint32) = { NextInChain = nextInChain; SType = sType; DriverVersion = driverVersion }
        new(driverVersion : uint32) = AdapterPropertiesVk(0n, Unchecked.defaultof<SType>, driverVersion)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnBufferDescriptorErrorInfoFromWireClient = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public OutOfMemory : int
        new(nextInChain : nativeint, sType : SType, outOfMemory : int) = { NextInChain = nextInChain; SType = sType; OutOfMemory = outOfMemory }
        new(outOfMemory : int) = DawnBufferDescriptorErrorInfoFromWireClient(0n, Unchecked.defaultof<SType>, outOfMemory)
    end
module WebGPU = 

    [<DllImport("WebGPU", EntryPoint="gpuInstanceCreateGLFWSurface")>]
    extern nativeint InstanceCreateGLFWSurface(void* self, void* window)
    [<DllImport("WebGPU", EntryPoint="gpuCreateInstance")>]
    extern nativeint CreateInstance(InstanceDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuGetProcAddress")>]
    extern nativeint GetProcAddress(StringView procName)
    [<DllImport("WebGPU", EntryPoint="gpuAdapterGetInstance")>]
    extern nativeint AdapterGetInstance(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuAdapterGetLimits")>]
    extern Status AdapterGetLimits(nativeint self, SupportedLimits* limits)
    [<DllImport("WebGPU", EntryPoint="gpuAdapterGetInfo")>]
    extern Status AdapterGetInfo(nativeint self, AdapterInfo* info)
    [<DllImport("WebGPU", EntryPoint="gpuAdapterHasFeature")>]
    extern int AdapterHasFeature(nativeint self, FeatureName feature)
    [<DllImport("WebGPU", EntryPoint="gpuAdapterGetFeatures")>]
    extern void AdapterGetFeatures(nativeint self, SupportedFeatures* features)
    [<DllImport("WebGPU", EntryPoint="gpuAdapterRequestDevice")>]
    extern void AdapterRequestDevice(nativeint self, DeviceDescriptor* descriptor, nativeint callback, nativeint userdata)
    [<DllImport("WebGPU", EntryPoint="gpuAdapterRequestDeviceF")>]
    extern Future AdapterRequestDeviceF(nativeint self, DeviceDescriptor* options, RequestDeviceCallbackInfo callbackInfo)
    [<DllImport("WebGPU", EntryPoint="gpuAdapterRequestDevice2")>]
    extern Future AdapterRequestDevice2(nativeint self, DeviceDescriptor* options, RequestDeviceCallbackInfo2 callbackInfo)
    [<DllImport("WebGPU", EntryPoint="gpuAdapterCreateDevice")>]
    extern nativeint AdapterCreateDevice(nativeint self, DeviceDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuAdapterGetFormatCapabilities")>]
    extern Status AdapterGetFormatCapabilities(nativeint self, TextureFormat format, FormatCapabilities* capabilities)
    [<DllImport("WebGPU", EntryPoint="gpuAdapterRelease")>]
    extern void AdapterRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuBindGroupSetLabel")>]
    extern void BindGroupSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuBindGroupRelease")>]
    extern void BindGroupRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuBindGroupLayoutSetLabel")>]
    extern void BindGroupLayoutSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuBindGroupLayoutRelease")>]
    extern void BindGroupLayoutRelease(nativeint self)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type BufferMapAsyncArgs = 
        {
            Self : nativeint
            Mode : MapMode
            Offset : unativeint
            Size : unativeint
            Callback : nativeint
            Userdata : nativeint
        }

    [<DllImport("WebGPU", EntryPoint="gpuBufferMapAsync")>]
    extern void _BufferMapAsync(BufferMapAsyncArgs& args)
    let BufferMapAsync(self : nativeint, mode : MapMode, offset : unativeint, size : unativeint, callback : nativeint, userdata : nativeint) =
        let mutable args = {
            BufferMapAsyncArgs.Self = self;
            BufferMapAsyncArgs.Mode = mode;
            BufferMapAsyncArgs.Offset = offset;
            BufferMapAsyncArgs.Size = size;
            BufferMapAsyncArgs.Callback = callback;
            BufferMapAsyncArgs.Userdata = userdata;
        }
        _BufferMapAsync(&args)
    [<DllImport("WebGPU", EntryPoint="gpuBufferMapAsyncF")>]
    extern Future BufferMapAsyncF(nativeint self, MapMode mode, unativeint offset, unativeint size, BufferMapCallbackInfo callbackInfo)
    [<DllImport("WebGPU", EntryPoint="gpuBufferMapAsync2")>]
    extern Future BufferMapAsync2(nativeint self, MapMode mode, unativeint offset, unativeint size, BufferMapCallbackInfo2 callbackInfo)
    [<DllImport("WebGPU", EntryPoint="gpuBufferGetMappedRange")>]
    extern nativeint BufferGetMappedRange(nativeint self, unativeint offset, unativeint size)
    [<DllImport("WebGPU", EntryPoint="gpuBufferGetConstMappedRange")>]
    extern nativeint BufferGetConstMappedRange(nativeint self, unativeint offset, unativeint size)
    [<DllImport("WebGPU", EntryPoint="gpuBufferSetLabel")>]
    extern void BufferSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuBufferGetUsage")>]
    extern BufferUsage BufferGetUsage(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuBufferGetSize")>]
    extern uint64 BufferGetSize(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuBufferGetMapState")>]
    extern BufferMapState BufferGetMapState(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuBufferUnmap")>]
    extern void BufferUnmap(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuBufferDestroy")>]
    extern void BufferDestroy(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuBufferRelease")>]
    extern void BufferRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuCommandBufferSetLabel")>]
    extern void CommandBufferSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuCommandBufferRelease")>]
    extern void CommandBufferRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuCommandEncoderFinish")>]
    extern nativeint CommandEncoderFinish(nativeint self, CommandBufferDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuCommandEncoderBeginComputePass")>]
    extern nativeint CommandEncoderBeginComputePass(nativeint self, ComputePassDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuCommandEncoderBeginRenderPass")>]
    extern nativeint CommandEncoderBeginRenderPass(nativeint self, RenderPassDescriptor* descriptor)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type CommandEncoderCopyBufferToBufferArgs = 
        {
            Self : nativeint
            Source : nativeint
            SourceOffset : uint64
            Destination : nativeint
            DestinationOffset : uint64
            Size : uint64
        }

    [<DllImport("WebGPU", EntryPoint="gpuCommandEncoderCopyBufferToBuffer")>]
    extern void _CommandEncoderCopyBufferToBuffer(CommandEncoderCopyBufferToBufferArgs& args)
    let CommandEncoderCopyBufferToBuffer(self : nativeint, source : nativeint, sourceOffset : uint64, destination : nativeint, destinationOffset : uint64, size : uint64) =
        let mutable args = {
            CommandEncoderCopyBufferToBufferArgs.Self = self;
            CommandEncoderCopyBufferToBufferArgs.Source = source;
            CommandEncoderCopyBufferToBufferArgs.SourceOffset = sourceOffset;
            CommandEncoderCopyBufferToBufferArgs.Destination = destination;
            CommandEncoderCopyBufferToBufferArgs.DestinationOffset = destinationOffset;
            CommandEncoderCopyBufferToBufferArgs.Size = size;
        }
        _CommandEncoderCopyBufferToBuffer(&args)
    [<DllImport("WebGPU", EntryPoint="gpuCommandEncoderCopyBufferToTexture")>]
    extern void CommandEncoderCopyBufferToTexture(nativeint self, ImageCopyBuffer* source, ImageCopyTexture* destination, Extent3D* copySize)
    [<DllImport("WebGPU", EntryPoint="gpuCommandEncoderCopyTextureToBuffer")>]
    extern void CommandEncoderCopyTextureToBuffer(nativeint self, ImageCopyTexture* source, ImageCopyBuffer* destination, Extent3D* copySize)
    [<DllImport("WebGPU", EntryPoint="gpuCommandEncoderCopyTextureToTexture")>]
    extern void CommandEncoderCopyTextureToTexture(nativeint self, ImageCopyTexture* source, ImageCopyTexture* destination, Extent3D* copySize)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type CommandEncoderClearBufferArgs = 
        {
            Self : nativeint
            Buffer : nativeint
            Offset : uint64
            Size : uint64
        }

    [<DllImport("WebGPU", EntryPoint="gpuCommandEncoderClearBuffer")>]
    extern void _CommandEncoderClearBuffer(CommandEncoderClearBufferArgs& args)
    let CommandEncoderClearBuffer(self : nativeint, buffer : nativeint, offset : uint64, size : uint64) =
        let mutable args = {
            CommandEncoderClearBufferArgs.Self = self;
            CommandEncoderClearBufferArgs.Buffer = buffer;
            CommandEncoderClearBufferArgs.Offset = offset;
            CommandEncoderClearBufferArgs.Size = size;
        }
        _CommandEncoderClearBuffer(&args)
    [<DllImport("WebGPU", EntryPoint="gpuCommandEncoderInjectValidationError")>]
    extern void CommandEncoderInjectValidationError(nativeint self, StringView message)
    [<DllImport("WebGPU", EntryPoint="gpuCommandEncoderInsertDebugMarker")>]
    extern void CommandEncoderInsertDebugMarker(nativeint self, StringView markerLabel)
    [<DllImport("WebGPU", EntryPoint="gpuCommandEncoderPopDebugGroup")>]
    extern void CommandEncoderPopDebugGroup(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuCommandEncoderPushDebugGroup")>]
    extern void CommandEncoderPushDebugGroup(nativeint self, StringView groupLabel)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type CommandEncoderResolveQuerySetArgs = 
        {
            Self : nativeint
            QuerySet : nativeint
            FirstQuery : uint32
            QueryCount : uint32
            Destination : nativeint
            DestinationOffset : uint64
        }

    [<DllImport("WebGPU", EntryPoint="gpuCommandEncoderResolveQuerySet")>]
    extern void _CommandEncoderResolveQuerySet(CommandEncoderResolveQuerySetArgs& args)
    let CommandEncoderResolveQuerySet(self : nativeint, querySet : nativeint, firstQuery : uint32, queryCount : uint32, destination : nativeint, destinationOffset : uint64) =
        let mutable args = {
            CommandEncoderResolveQuerySetArgs.Self = self;
            CommandEncoderResolveQuerySetArgs.QuerySet = querySet;
            CommandEncoderResolveQuerySetArgs.FirstQuery = firstQuery;
            CommandEncoderResolveQuerySetArgs.QueryCount = queryCount;
            CommandEncoderResolveQuerySetArgs.Destination = destination;
            CommandEncoderResolveQuerySetArgs.DestinationOffset = destinationOffset;
        }
        _CommandEncoderResolveQuerySet(&args)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type CommandEncoderWriteBufferArgs = 
        {
            Self : nativeint
            Buffer : nativeint
            BufferOffset : uint64
            Data : nativeptr<uint8>
            Size : uint64
        }

    [<DllImport("WebGPU", EntryPoint="gpuCommandEncoderWriteBuffer")>]
    extern void _CommandEncoderWriteBuffer(CommandEncoderWriteBufferArgs& args)
    let CommandEncoderWriteBuffer(self : nativeint, buffer : nativeint, bufferOffset : uint64, data : nativeptr<uint8>, size : uint64) =
        let mutable args = {
            CommandEncoderWriteBufferArgs.Self = self;
            CommandEncoderWriteBufferArgs.Buffer = buffer;
            CommandEncoderWriteBufferArgs.BufferOffset = bufferOffset;
            CommandEncoderWriteBufferArgs.Data = data;
            CommandEncoderWriteBufferArgs.Size = size;
        }
        _CommandEncoderWriteBuffer(&args)
    [<DllImport("WebGPU", EntryPoint="gpuCommandEncoderWriteTimestamp")>]
    extern void CommandEncoderWriteTimestamp(nativeint self, nativeint querySet, uint32 queryIndex)
    [<DllImport("WebGPU", EntryPoint="gpuCommandEncoderSetLabel")>]
    extern void CommandEncoderSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuCommandEncoderRelease")>]
    extern void CommandEncoderRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuComputePassEncoderInsertDebugMarker")>]
    extern void ComputePassEncoderInsertDebugMarker(nativeint self, StringView markerLabel)
    [<DllImport("WebGPU", EntryPoint="gpuComputePassEncoderPopDebugGroup")>]
    extern void ComputePassEncoderPopDebugGroup(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuComputePassEncoderPushDebugGroup")>]
    extern void ComputePassEncoderPushDebugGroup(nativeint self, StringView groupLabel)
    [<DllImport("WebGPU", EntryPoint="gpuComputePassEncoderSetPipeline")>]
    extern void ComputePassEncoderSetPipeline(nativeint self, nativeint pipeline)
    [<DllImport("WebGPU", EntryPoint="gpuComputePassEncoderSetBindGroup")>]
    extern void ComputePassEncoderSetBindGroup(nativeint self, uint32 groupIndex, nativeint group, unativeint dynamicOffsetCount, uint32* dynamicOffsets)
    [<DllImport("WebGPU", EntryPoint="gpuComputePassEncoderWriteTimestamp")>]
    extern void ComputePassEncoderWriteTimestamp(nativeint self, nativeint querySet, uint32 queryIndex)
    [<DllImport("WebGPU", EntryPoint="gpuComputePassEncoderDispatchWorkgroups")>]
    extern void ComputePassEncoderDispatchWorkgroups(nativeint self, uint32 workgroupCountX, uint32 workgroupCountY, uint32 workgroupCountZ)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type ComputePassEncoderDispatchWorkgroupsIndirectArgs = 
        {
            Self : nativeint
            IndirectBuffer : nativeint
            IndirectOffset : uint64
        }

    [<DllImport("WebGPU", EntryPoint="gpuComputePassEncoderDispatchWorkgroupsIndirect")>]
    extern void _ComputePassEncoderDispatchWorkgroupsIndirect(ComputePassEncoderDispatchWorkgroupsIndirectArgs& args)
    let ComputePassEncoderDispatchWorkgroupsIndirect(self : nativeint, indirectBuffer : nativeint, indirectOffset : uint64) =
        let mutable args = {
            ComputePassEncoderDispatchWorkgroupsIndirectArgs.Self = self;
            ComputePassEncoderDispatchWorkgroupsIndirectArgs.IndirectBuffer = indirectBuffer;
            ComputePassEncoderDispatchWorkgroupsIndirectArgs.IndirectOffset = indirectOffset;
        }
        _ComputePassEncoderDispatchWorkgroupsIndirect(&args)
    [<DllImport("WebGPU", EntryPoint="gpuComputePassEncoderEnd")>]
    extern void ComputePassEncoderEnd(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuComputePassEncoderSetLabel")>]
    extern void ComputePassEncoderSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuComputePassEncoderRelease")>]
    extern void ComputePassEncoderRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuComputePipelineGetBindGroupLayout")>]
    extern nativeint ComputePipelineGetBindGroupLayout(nativeint self, uint32 groupIndex)
    [<DllImport("WebGPU", EntryPoint="gpuComputePipelineSetLabel")>]
    extern void ComputePipelineSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuComputePipelineRelease")>]
    extern void ComputePipelineRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateBindGroup")>]
    extern nativeint DeviceCreateBindGroup(nativeint self, BindGroupDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateBindGroupLayout")>]
    extern nativeint DeviceCreateBindGroupLayout(nativeint self, BindGroupLayoutDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateBuffer")>]
    extern nativeint DeviceCreateBuffer(nativeint self, BufferDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateErrorBuffer")>]
    extern nativeint DeviceCreateErrorBuffer(nativeint self, BufferDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateCommandEncoder")>]
    extern nativeint DeviceCreateCommandEncoder(nativeint self, CommandEncoderDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateComputePipeline")>]
    extern nativeint DeviceCreateComputePipeline(nativeint self, ComputePipelineDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateComputePipelineAsync")>]
    extern void DeviceCreateComputePipelineAsync(nativeint self, ComputePipelineDescriptor* descriptor, nativeint callback, nativeint userdata)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateComputePipelineAsyncF")>]
    extern Future DeviceCreateComputePipelineAsyncF(nativeint self, ComputePipelineDescriptor* descriptor, CreateComputePipelineAsyncCallbackInfo callbackInfo)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateComputePipelineAsync2")>]
    extern Future DeviceCreateComputePipelineAsync2(nativeint self, ComputePipelineDescriptor* descriptor, CreateComputePipelineAsyncCallbackInfo2 callbackInfo)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateExternalTexture")>]
    extern nativeint DeviceCreateExternalTexture(nativeint self, ExternalTextureDescriptor* externalTextureDescriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateErrorExternalTexture")>]
    extern nativeint DeviceCreateErrorExternalTexture(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreatePipelineLayout")>]
    extern nativeint DeviceCreatePipelineLayout(nativeint self, PipelineLayoutDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateQuerySet")>]
    extern nativeint DeviceCreateQuerySet(nativeint self, QuerySetDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateRenderPipelineAsync")>]
    extern void DeviceCreateRenderPipelineAsync(nativeint self, RenderPipelineDescriptor* descriptor, nativeint callback, nativeint userdata)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateRenderPipelineAsyncF")>]
    extern Future DeviceCreateRenderPipelineAsyncF(nativeint self, RenderPipelineDescriptor* descriptor, CreateRenderPipelineAsyncCallbackInfo callbackInfo)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateRenderPipelineAsync2")>]
    extern Future DeviceCreateRenderPipelineAsync2(nativeint self, RenderPipelineDescriptor* descriptor, CreateRenderPipelineAsyncCallbackInfo2 callbackInfo)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateRenderBundleEncoder")>]
    extern nativeint DeviceCreateRenderBundleEncoder(nativeint self, RenderBundleEncoderDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateRenderPipeline")>]
    extern nativeint DeviceCreateRenderPipeline(nativeint self, RenderPipelineDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateSampler")>]
    extern nativeint DeviceCreateSampler(nativeint self, SamplerDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateShaderModule")>]
    extern nativeint DeviceCreateShaderModule(nativeint self, ShaderModuleDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateErrorShaderModule")>]
    extern nativeint DeviceCreateErrorShaderModule(nativeint self, ShaderModuleDescriptor* descriptor, StringView errorMessage)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateTexture")>]
    extern nativeint DeviceCreateTexture(nativeint self, TextureDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceImportSharedBufferMemory")>]
    extern nativeint DeviceImportSharedBufferMemory(nativeint self, SharedBufferMemoryDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceImportSharedTextureMemory")>]
    extern nativeint DeviceImportSharedTextureMemory(nativeint self, SharedTextureMemoryDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceImportSharedFence")>]
    extern nativeint DeviceImportSharedFence(nativeint self, SharedFenceDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceCreateErrorTexture")>]
    extern nativeint DeviceCreateErrorTexture(nativeint self, TextureDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceDestroy")>]
    extern void DeviceDestroy(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceGetAHardwareBufferProperties")>]
    extern Status DeviceGetAHardwareBufferProperties(nativeint self, nativeint handle, AHardwareBufferProperties* properties)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceGetLimits")>]
    extern Status DeviceGetLimits(nativeint self, SupportedLimits* limits)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceGetLostFuture")>]
    extern Future DeviceGetLostFuture(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceHasFeature")>]
    extern int DeviceHasFeature(nativeint self, FeatureName feature)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceGetFeatures")>]
    extern void DeviceGetFeatures(nativeint self, SupportedFeatures* features)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceGetAdapterInfo")>]
    extern Status DeviceGetAdapterInfo(nativeint self, AdapterInfo* adapterInfo)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceGetAdapter")>]
    extern nativeint DeviceGetAdapter(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceGetQueue")>]
    extern nativeint DeviceGetQueue(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceInjectError")>]
    extern void DeviceInjectError(nativeint self, ErrorType typ, StringView message)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceForceLoss")>]
    extern void DeviceForceLoss(nativeint self, DeviceLostReason typ, StringView message)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceTick")>]
    extern void DeviceTick(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceSetUncapturedErrorCallback")>]
    extern void DeviceSetUncapturedErrorCallback(nativeint self, nativeint callback, nativeint userdata)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceSetLoggingCallback")>]
    extern void DeviceSetLoggingCallback(nativeint self, nativeint callback, nativeint userdata)
    [<DllImport("WebGPU", EntryPoint="gpuDevicePushErrorScope")>]
    extern void DevicePushErrorScope(nativeint self, ErrorFilter filter)
    [<DllImport("WebGPU", EntryPoint="gpuDevicePopErrorScope")>]
    extern void DevicePopErrorScope(nativeint self, nativeint oldCallback, nativeint userdata)
    [<DllImport("WebGPU", EntryPoint="gpuDevicePopErrorScopeF")>]
    extern Future DevicePopErrorScopeF(nativeint self, PopErrorScopeCallbackInfo callbackInfo)
    [<DllImport("WebGPU", EntryPoint="gpuDevicePopErrorScope2")>]
    extern Future DevicePopErrorScope2(nativeint self, PopErrorScopeCallbackInfo2 callbackInfo)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceSetLabel")>]
    extern void DeviceSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceValidateTextureDescriptor")>]
    extern void DeviceValidateTextureDescriptor(nativeint self, TextureDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuDeviceRelease")>]
    extern void DeviceRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuExternalTextureSetLabel")>]
    extern void ExternalTextureSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuExternalTextureDestroy")>]
    extern void ExternalTextureDestroy(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuExternalTextureExpire")>]
    extern void ExternalTextureExpire(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuExternalTextureRefresh")>]
    extern void ExternalTextureRefresh(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuExternalTextureRelease")>]
    extern void ExternalTextureRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuSharedBufferMemorySetLabel")>]
    extern void SharedBufferMemorySetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuSharedBufferMemoryGetProperties")>]
    extern Status SharedBufferMemoryGetProperties(nativeint self, SharedBufferMemoryProperties* properties)
    [<DllImport("WebGPU", EntryPoint="gpuSharedBufferMemoryCreateBuffer")>]
    extern nativeint SharedBufferMemoryCreateBuffer(nativeint self, BufferDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuSharedBufferMemoryBeginAccess")>]
    extern Status SharedBufferMemoryBeginAccess(nativeint self, nativeint buffer, SharedBufferMemoryBeginAccessDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuSharedBufferMemoryEndAccess")>]
    extern Status SharedBufferMemoryEndAccess(nativeint self, nativeint buffer, SharedBufferMemoryEndAccessState* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuSharedBufferMemoryIsDeviceLost")>]
    extern int SharedBufferMemoryIsDeviceLost(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuSharedBufferMemoryRelease")>]
    extern void SharedBufferMemoryRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuSharedTextureMemorySetLabel")>]
    extern void SharedTextureMemorySetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuSharedTextureMemoryGetProperties")>]
    extern Status SharedTextureMemoryGetProperties(nativeint self, SharedTextureMemoryProperties* properties)
    [<DllImport("WebGPU", EntryPoint="gpuSharedTextureMemoryCreateTexture")>]
    extern nativeint SharedTextureMemoryCreateTexture(nativeint self, TextureDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuSharedTextureMemoryBeginAccess")>]
    extern Status SharedTextureMemoryBeginAccess(nativeint self, nativeint texture, SharedTextureMemoryBeginAccessDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuSharedTextureMemoryEndAccess")>]
    extern Status SharedTextureMemoryEndAccess(nativeint self, nativeint texture, SharedTextureMemoryEndAccessState* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuSharedTextureMemoryIsDeviceLost")>]
    extern int SharedTextureMemoryIsDeviceLost(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuSharedTextureMemoryRelease")>]
    extern void SharedTextureMemoryRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuSharedFenceExportInfo")>]
    extern void SharedFenceExportInfo(nativeint self, SharedFenceExportInfo* info)
    [<DllImport("WebGPU", EntryPoint="gpuSharedFenceRelease")>]
    extern void SharedFenceRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuInstanceCreateSurface")>]
    extern nativeint InstanceCreateSurface(nativeint self, SurfaceDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuInstanceProcessEvents")>]
    extern void InstanceProcessEvents(nativeint self)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type InstanceWaitAnyArgs = 
        {
            Self : nativeint
            FutureCount : unativeint
            Futures : nativeptr<FutureWaitInfo>
            TimeoutNS : uint64
        }

    [<DllImport("WebGPU", EntryPoint="gpuInstanceWaitAny")>]
    extern WaitStatus _InstanceWaitAny(InstanceWaitAnyArgs& args)
    let InstanceWaitAny(self : nativeint, futureCount : unativeint, futures : nativeptr<FutureWaitInfo>, timeoutNS : uint64) =
        let mutable args = {
            InstanceWaitAnyArgs.Self = self;
            InstanceWaitAnyArgs.FutureCount = futureCount;
            InstanceWaitAnyArgs.Futures = futures;
            InstanceWaitAnyArgs.TimeoutNS = timeoutNS;
        }
        _InstanceWaitAny(&args)
    [<DllImport("WebGPU", EntryPoint="gpuInstanceRequestAdapter")>]
    extern void InstanceRequestAdapter(nativeint self, RequestAdapterOptions* options, nativeint callback, nativeint userdata)
    [<DllImport("WebGPU", EntryPoint="gpuInstanceRequestAdapterF")>]
    extern Future InstanceRequestAdapterF(nativeint self, RequestAdapterOptions* options, RequestAdapterCallbackInfo callbackInfo)
    [<DllImport("WebGPU", EntryPoint="gpuInstanceRequestAdapter2")>]
    extern Future InstanceRequestAdapter2(nativeint self, RequestAdapterOptions* options, RequestAdapterCallbackInfo2 callbackInfo)
    [<DllImport("WebGPU", EntryPoint="gpuInstanceHasWGSLLanguageFeature")>]
    extern int InstanceHasWGSLLanguageFeature(nativeint self, WGSLFeatureName feature)
    [<DllImport("WebGPU", EntryPoint="gpuInstanceEnumerateWGSLLanguageFeatures")>]
    extern unativeint InstanceEnumerateWGSLLanguageFeatures(nativeint self, WGSLFeatureName* features)
    [<DllImport("WebGPU", EntryPoint="gpuInstanceRelease")>]
    extern void InstanceRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuGetInstanceFeatures")>]
    extern Status GetInstanceFeatures(InstanceFeatures* features)
    [<DllImport("WebGPU", EntryPoint="gpuPipelineLayoutSetLabel")>]
    extern void PipelineLayoutSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuPipelineLayoutRelease")>]
    extern void PipelineLayoutRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuQuerySetSetLabel")>]
    extern void QuerySetSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuQuerySetGetType")>]
    extern QueryType QuerySetGetType(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuQuerySetGetCount")>]
    extern uint32 QuerySetGetCount(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuQuerySetDestroy")>]
    extern void QuerySetDestroy(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuQuerySetRelease")>]
    extern void QuerySetRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuQueueSubmit")>]
    extern void QueueSubmit(nativeint self, unativeint commandCount, nativeint* commands)
    [<DllImport("WebGPU", EntryPoint="gpuQueueOnSubmittedWorkDone")>]
    extern void QueueOnSubmittedWorkDone(nativeint self, nativeint callback, nativeint userdata)
    [<DllImport("WebGPU", EntryPoint="gpuQueueOnSubmittedWorkDoneF")>]
    extern Future QueueOnSubmittedWorkDoneF(nativeint self, QueueWorkDoneCallbackInfo callbackInfo)
    [<DllImport("WebGPU", EntryPoint="gpuQueueOnSubmittedWorkDone2")>]
    extern Future QueueOnSubmittedWorkDone2(nativeint self, QueueWorkDoneCallbackInfo2 callbackInfo)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type QueueWriteBufferArgs = 
        {
            Self : nativeint
            Buffer : nativeint
            BufferOffset : uint64
            Data : nativeint
            Size : unativeint
        }

    [<DllImport("WebGPU", EntryPoint="gpuQueueWriteBuffer")>]
    extern void _QueueWriteBuffer(QueueWriteBufferArgs& args)
    let QueueWriteBuffer(self : nativeint, buffer : nativeint, bufferOffset : uint64, data : nativeint, size : unativeint) =
        let mutable args = {
            QueueWriteBufferArgs.Self = self;
            QueueWriteBufferArgs.Buffer = buffer;
            QueueWriteBufferArgs.BufferOffset = bufferOffset;
            QueueWriteBufferArgs.Data = data;
            QueueWriteBufferArgs.Size = size;
        }
        _QueueWriteBuffer(&args)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type QueueWriteTextureArgs = 
        {
            Self : nativeint
            Destination : nativeptr<ImageCopyTexture>
            Data : nativeint
            DataSize : unativeint
            DataLayout : nativeptr<TextureDataLayout>
            WriteSize : nativeptr<Extent3D>
        }

    [<DllImport("WebGPU", EntryPoint="gpuQueueWriteTexture")>]
    extern void _QueueWriteTexture(QueueWriteTextureArgs& args)
    let QueueWriteTexture(self : nativeint, destination : nativeptr<ImageCopyTexture>, data : nativeint, dataSize : unativeint, dataLayout : nativeptr<TextureDataLayout>, writeSize : nativeptr<Extent3D>) =
        let mutable args = {
            QueueWriteTextureArgs.Self = self;
            QueueWriteTextureArgs.Destination = destination;
            QueueWriteTextureArgs.Data = data;
            QueueWriteTextureArgs.DataSize = dataSize;
            QueueWriteTextureArgs.DataLayout = dataLayout;
            QueueWriteTextureArgs.WriteSize = writeSize;
        }
        _QueueWriteTexture(&args)
    [<DllImport("WebGPU", EntryPoint="gpuQueueCopyTextureForBrowser")>]
    extern void QueueCopyTextureForBrowser(nativeint self, ImageCopyTexture* source, ImageCopyTexture* destination, Extent3D* copySize, CopyTextureForBrowserOptions* options)
    [<DllImport("WebGPU", EntryPoint="gpuQueueCopyExternalTextureForBrowser")>]
    extern void QueueCopyExternalTextureForBrowser(nativeint self, ImageCopyExternalTexture* source, ImageCopyTexture* destination, Extent3D* copySize, CopyTextureForBrowserOptions* options)
    [<DllImport("WebGPU", EntryPoint="gpuQueueSetLabel")>]
    extern void QueueSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuQueueRelease")>]
    extern void QueueRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuRenderBundleSetLabel")>]
    extern void RenderBundleSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuRenderBundleRelease")>]
    extern void RenderBundleRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuRenderBundleEncoderSetPipeline")>]
    extern void RenderBundleEncoderSetPipeline(nativeint self, nativeint pipeline)
    [<DllImport("WebGPU", EntryPoint="gpuRenderBundleEncoderSetBindGroup")>]
    extern void RenderBundleEncoderSetBindGroup(nativeint self, uint32 groupIndex, nativeint group, unativeint dynamicOffsetCount, uint32* dynamicOffsets)
    [<DllImport("WebGPU", EntryPoint="gpuRenderBundleEncoderDraw")>]
    extern void RenderBundleEncoderDraw(nativeint self, uint32 vertexCount, uint32 instanceCount, uint32 firstVertex, uint32 firstInstance)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type RenderBundleEncoderDrawIndexedArgs = 
        {
            Self : nativeint
            IndexCount : uint32
            InstanceCount : uint32
            FirstIndex : uint32
            BaseVertex : int
            FirstInstance : uint32
        }

    [<DllImport("WebGPU", EntryPoint="gpuRenderBundleEncoderDrawIndexed")>]
    extern void _RenderBundleEncoderDrawIndexed(RenderBundleEncoderDrawIndexedArgs& args)
    let RenderBundleEncoderDrawIndexed(self : nativeint, indexCount : uint32, instanceCount : uint32, firstIndex : uint32, baseVertex : int, firstInstance : uint32) =
        let mutable args = {
            RenderBundleEncoderDrawIndexedArgs.Self = self;
            RenderBundleEncoderDrawIndexedArgs.IndexCount = indexCount;
            RenderBundleEncoderDrawIndexedArgs.InstanceCount = instanceCount;
            RenderBundleEncoderDrawIndexedArgs.FirstIndex = firstIndex;
            RenderBundleEncoderDrawIndexedArgs.BaseVertex = baseVertex;
            RenderBundleEncoderDrawIndexedArgs.FirstInstance = firstInstance;
        }
        _RenderBundleEncoderDrawIndexed(&args)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type RenderBundleEncoderDrawIndirectArgs = 
        {
            Self : nativeint
            IndirectBuffer : nativeint
            IndirectOffset : uint64
        }

    [<DllImport("WebGPU", EntryPoint="gpuRenderBundleEncoderDrawIndirect")>]
    extern void _RenderBundleEncoderDrawIndirect(RenderBundleEncoderDrawIndirectArgs& args)
    let RenderBundleEncoderDrawIndirect(self : nativeint, indirectBuffer : nativeint, indirectOffset : uint64) =
        let mutable args = {
            RenderBundleEncoderDrawIndirectArgs.Self = self;
            RenderBundleEncoderDrawIndirectArgs.IndirectBuffer = indirectBuffer;
            RenderBundleEncoderDrawIndirectArgs.IndirectOffset = indirectOffset;
        }
        _RenderBundleEncoderDrawIndirect(&args)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type RenderBundleEncoderDrawIndexedIndirectArgs = 
        {
            Self : nativeint
            IndirectBuffer : nativeint
            IndirectOffset : uint64
        }

    [<DllImport("WebGPU", EntryPoint="gpuRenderBundleEncoderDrawIndexedIndirect")>]
    extern void _RenderBundleEncoderDrawIndexedIndirect(RenderBundleEncoderDrawIndexedIndirectArgs& args)
    let RenderBundleEncoderDrawIndexedIndirect(self : nativeint, indirectBuffer : nativeint, indirectOffset : uint64) =
        let mutable args = {
            RenderBundleEncoderDrawIndexedIndirectArgs.Self = self;
            RenderBundleEncoderDrawIndexedIndirectArgs.IndirectBuffer = indirectBuffer;
            RenderBundleEncoderDrawIndexedIndirectArgs.IndirectOffset = indirectOffset;
        }
        _RenderBundleEncoderDrawIndexedIndirect(&args)
    [<DllImport("WebGPU", EntryPoint="gpuRenderBundleEncoderInsertDebugMarker")>]
    extern void RenderBundleEncoderInsertDebugMarker(nativeint self, StringView markerLabel)
    [<DllImport("WebGPU", EntryPoint="gpuRenderBundleEncoderPopDebugGroup")>]
    extern void RenderBundleEncoderPopDebugGroup(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuRenderBundleEncoderPushDebugGroup")>]
    extern void RenderBundleEncoderPushDebugGroup(nativeint self, StringView groupLabel)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type RenderBundleEncoderSetVertexBufferArgs = 
        {
            Self : nativeint
            Slot : uint32
            Buffer : nativeint
            Offset : uint64
            Size : uint64
        }

    [<DllImport("WebGPU", EntryPoint="gpuRenderBundleEncoderSetVertexBuffer")>]
    extern void _RenderBundleEncoderSetVertexBuffer(RenderBundleEncoderSetVertexBufferArgs& args)
    let RenderBundleEncoderSetVertexBuffer(self : nativeint, slot : uint32, buffer : nativeint, offset : uint64, size : uint64) =
        let mutable args = {
            RenderBundleEncoderSetVertexBufferArgs.Self = self;
            RenderBundleEncoderSetVertexBufferArgs.Slot = slot;
            RenderBundleEncoderSetVertexBufferArgs.Buffer = buffer;
            RenderBundleEncoderSetVertexBufferArgs.Offset = offset;
            RenderBundleEncoderSetVertexBufferArgs.Size = size;
        }
        _RenderBundleEncoderSetVertexBuffer(&args)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type RenderBundleEncoderSetIndexBufferArgs = 
        {
            Self : nativeint
            Buffer : nativeint
            Format : IndexFormat
            Offset : uint64
            Size : uint64
        }

    [<DllImport("WebGPU", EntryPoint="gpuRenderBundleEncoderSetIndexBuffer")>]
    extern void _RenderBundleEncoderSetIndexBuffer(RenderBundleEncoderSetIndexBufferArgs& args)
    let RenderBundleEncoderSetIndexBuffer(self : nativeint, buffer : nativeint, format : IndexFormat, offset : uint64, size : uint64) =
        let mutable args = {
            RenderBundleEncoderSetIndexBufferArgs.Self = self;
            RenderBundleEncoderSetIndexBufferArgs.Buffer = buffer;
            RenderBundleEncoderSetIndexBufferArgs.Format = format;
            RenderBundleEncoderSetIndexBufferArgs.Offset = offset;
            RenderBundleEncoderSetIndexBufferArgs.Size = size;
        }
        _RenderBundleEncoderSetIndexBuffer(&args)
    [<DllImport("WebGPU", EntryPoint="gpuRenderBundleEncoderFinish")>]
    extern nativeint RenderBundleEncoderFinish(nativeint self, RenderBundleDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuRenderBundleEncoderSetLabel")>]
    extern void RenderBundleEncoderSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuRenderBundleEncoderRelease")>]
    extern void RenderBundleEncoderRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderSetPipeline")>]
    extern void RenderPassEncoderSetPipeline(nativeint self, nativeint pipeline)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderSetBindGroup")>]
    extern void RenderPassEncoderSetBindGroup(nativeint self, uint32 groupIndex, nativeint group, unativeint dynamicOffsetCount, uint32* dynamicOffsets)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderDraw")>]
    extern void RenderPassEncoderDraw(nativeint self, uint32 vertexCount, uint32 instanceCount, uint32 firstVertex, uint32 firstInstance)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type RenderPassEncoderDrawIndexedArgs = 
        {
            Self : nativeint
            IndexCount : uint32
            InstanceCount : uint32
            FirstIndex : uint32
            BaseVertex : int
            FirstInstance : uint32
        }

    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderDrawIndexed")>]
    extern void _RenderPassEncoderDrawIndexed(RenderPassEncoderDrawIndexedArgs& args)
    let RenderPassEncoderDrawIndexed(self : nativeint, indexCount : uint32, instanceCount : uint32, firstIndex : uint32, baseVertex : int, firstInstance : uint32) =
        let mutable args = {
            RenderPassEncoderDrawIndexedArgs.Self = self;
            RenderPassEncoderDrawIndexedArgs.IndexCount = indexCount;
            RenderPassEncoderDrawIndexedArgs.InstanceCount = instanceCount;
            RenderPassEncoderDrawIndexedArgs.FirstIndex = firstIndex;
            RenderPassEncoderDrawIndexedArgs.BaseVertex = baseVertex;
            RenderPassEncoderDrawIndexedArgs.FirstInstance = firstInstance;
        }
        _RenderPassEncoderDrawIndexed(&args)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type RenderPassEncoderDrawIndirectArgs = 
        {
            Self : nativeint
            IndirectBuffer : nativeint
            IndirectOffset : uint64
        }

    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderDrawIndirect")>]
    extern void _RenderPassEncoderDrawIndirect(RenderPassEncoderDrawIndirectArgs& args)
    let RenderPassEncoderDrawIndirect(self : nativeint, indirectBuffer : nativeint, indirectOffset : uint64) =
        let mutable args = {
            RenderPassEncoderDrawIndirectArgs.Self = self;
            RenderPassEncoderDrawIndirectArgs.IndirectBuffer = indirectBuffer;
            RenderPassEncoderDrawIndirectArgs.IndirectOffset = indirectOffset;
        }
        _RenderPassEncoderDrawIndirect(&args)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type RenderPassEncoderDrawIndexedIndirectArgs = 
        {
            Self : nativeint
            IndirectBuffer : nativeint
            IndirectOffset : uint64
        }

    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderDrawIndexedIndirect")>]
    extern void _RenderPassEncoderDrawIndexedIndirect(RenderPassEncoderDrawIndexedIndirectArgs& args)
    let RenderPassEncoderDrawIndexedIndirect(self : nativeint, indirectBuffer : nativeint, indirectOffset : uint64) =
        let mutable args = {
            RenderPassEncoderDrawIndexedIndirectArgs.Self = self;
            RenderPassEncoderDrawIndexedIndirectArgs.IndirectBuffer = indirectBuffer;
            RenderPassEncoderDrawIndexedIndirectArgs.IndirectOffset = indirectOffset;
        }
        _RenderPassEncoderDrawIndexedIndirect(&args)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type RenderPassEncoderMultiDrawIndirectArgs = 
        {
            Self : nativeint
            IndirectBuffer : nativeint
            IndirectOffset : uint64
            MaxDrawCount : uint32
            DrawCountBuffer : nativeint
            DrawCountBufferOffset : uint64
        }

    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderMultiDrawIndirect")>]
    extern void _RenderPassEncoderMultiDrawIndirect(RenderPassEncoderMultiDrawIndirectArgs& args)
    let RenderPassEncoderMultiDrawIndirect(self : nativeint, indirectBuffer : nativeint, indirectOffset : uint64, maxDrawCount : uint32, drawCountBuffer : nativeint, drawCountBufferOffset : uint64) =
        let mutable args = {
            RenderPassEncoderMultiDrawIndirectArgs.Self = self;
            RenderPassEncoderMultiDrawIndirectArgs.IndirectBuffer = indirectBuffer;
            RenderPassEncoderMultiDrawIndirectArgs.IndirectOffset = indirectOffset;
            RenderPassEncoderMultiDrawIndirectArgs.MaxDrawCount = maxDrawCount;
            RenderPassEncoderMultiDrawIndirectArgs.DrawCountBuffer = drawCountBuffer;
            RenderPassEncoderMultiDrawIndirectArgs.DrawCountBufferOffset = drawCountBufferOffset;
        }
        _RenderPassEncoderMultiDrawIndirect(&args)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type RenderPassEncoderMultiDrawIndexedIndirectArgs = 
        {
            Self : nativeint
            IndirectBuffer : nativeint
            IndirectOffset : uint64
            MaxDrawCount : uint32
            DrawCountBuffer : nativeint
            DrawCountBufferOffset : uint64
        }

    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderMultiDrawIndexedIndirect")>]
    extern void _RenderPassEncoderMultiDrawIndexedIndirect(RenderPassEncoderMultiDrawIndexedIndirectArgs& args)
    let RenderPassEncoderMultiDrawIndexedIndirect(self : nativeint, indirectBuffer : nativeint, indirectOffset : uint64, maxDrawCount : uint32, drawCountBuffer : nativeint, drawCountBufferOffset : uint64) =
        let mutable args = {
            RenderPassEncoderMultiDrawIndexedIndirectArgs.Self = self;
            RenderPassEncoderMultiDrawIndexedIndirectArgs.IndirectBuffer = indirectBuffer;
            RenderPassEncoderMultiDrawIndexedIndirectArgs.IndirectOffset = indirectOffset;
            RenderPassEncoderMultiDrawIndexedIndirectArgs.MaxDrawCount = maxDrawCount;
            RenderPassEncoderMultiDrawIndexedIndirectArgs.DrawCountBuffer = drawCountBuffer;
            RenderPassEncoderMultiDrawIndexedIndirectArgs.DrawCountBufferOffset = drawCountBufferOffset;
        }
        _RenderPassEncoderMultiDrawIndexedIndirect(&args)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderExecuteBundles")>]
    extern void RenderPassEncoderExecuteBundles(nativeint self, unativeint bundleCount, nativeint* bundles)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderInsertDebugMarker")>]
    extern void RenderPassEncoderInsertDebugMarker(nativeint self, StringView markerLabel)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderPopDebugGroup")>]
    extern void RenderPassEncoderPopDebugGroup(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderPushDebugGroup")>]
    extern void RenderPassEncoderPushDebugGroup(nativeint self, StringView groupLabel)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderSetStencilReference")>]
    extern void RenderPassEncoderSetStencilReference(nativeint self, uint32 reference)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderSetBlendConstant")>]
    extern void RenderPassEncoderSetBlendConstant(nativeint self, Color* color)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type RenderPassEncoderSetViewportArgs = 
        {
            Self : nativeint
            X : float32
            Y : float32
            Width : float32
            Height : float32
            MinDepth : float32
            MaxDepth : float32
        }

    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderSetViewport")>]
    extern void _RenderPassEncoderSetViewport(RenderPassEncoderSetViewportArgs& args)
    let RenderPassEncoderSetViewport(self : nativeint, x : float32, y : float32, width : float32, height : float32, minDepth : float32, maxDepth : float32) =
        let mutable args = {
            RenderPassEncoderSetViewportArgs.Self = self;
            RenderPassEncoderSetViewportArgs.X = x;
            RenderPassEncoderSetViewportArgs.Y = y;
            RenderPassEncoderSetViewportArgs.Width = width;
            RenderPassEncoderSetViewportArgs.Height = height;
            RenderPassEncoderSetViewportArgs.MinDepth = minDepth;
            RenderPassEncoderSetViewportArgs.MaxDepth = maxDepth;
        }
        _RenderPassEncoderSetViewport(&args)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderSetScissorRect")>]
    extern void RenderPassEncoderSetScissorRect(nativeint self, uint32 x, uint32 y, uint32 width, uint32 height)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type RenderPassEncoderSetVertexBufferArgs = 
        {
            Self : nativeint
            Slot : uint32
            Buffer : nativeint
            Offset : uint64
            Size : uint64
        }

    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderSetVertexBuffer")>]
    extern void _RenderPassEncoderSetVertexBuffer(RenderPassEncoderSetVertexBufferArgs& args)
    let RenderPassEncoderSetVertexBuffer(self : nativeint, slot : uint32, buffer : nativeint, offset : uint64, size : uint64) =
        let mutable args = {
            RenderPassEncoderSetVertexBufferArgs.Self = self;
            RenderPassEncoderSetVertexBufferArgs.Slot = slot;
            RenderPassEncoderSetVertexBufferArgs.Buffer = buffer;
            RenderPassEncoderSetVertexBufferArgs.Offset = offset;
            RenderPassEncoderSetVertexBufferArgs.Size = size;
        }
        _RenderPassEncoderSetVertexBuffer(&args)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type RenderPassEncoderSetIndexBufferArgs = 
        {
            Self : nativeint
            Buffer : nativeint
            Format : IndexFormat
            Offset : uint64
            Size : uint64
        }

    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderSetIndexBuffer")>]
    extern void _RenderPassEncoderSetIndexBuffer(RenderPassEncoderSetIndexBufferArgs& args)
    let RenderPassEncoderSetIndexBuffer(self : nativeint, buffer : nativeint, format : IndexFormat, offset : uint64, size : uint64) =
        let mutable args = {
            RenderPassEncoderSetIndexBufferArgs.Self = self;
            RenderPassEncoderSetIndexBufferArgs.Buffer = buffer;
            RenderPassEncoderSetIndexBufferArgs.Format = format;
            RenderPassEncoderSetIndexBufferArgs.Offset = offset;
            RenderPassEncoderSetIndexBufferArgs.Size = size;
        }
        _RenderPassEncoderSetIndexBuffer(&args)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderBeginOcclusionQuery")>]
    extern void RenderPassEncoderBeginOcclusionQuery(nativeint self, uint32 queryIndex)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderEndOcclusionQuery")>]
    extern void RenderPassEncoderEndOcclusionQuery(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderWriteTimestamp")>]
    extern void RenderPassEncoderWriteTimestamp(nativeint self, nativeint querySet, uint32 queryIndex)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderPixelLocalStorageBarrier")>]
    extern void RenderPassEncoderPixelLocalStorageBarrier(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderEnd")>]
    extern void RenderPassEncoderEnd(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderSetLabel")>]
    extern void RenderPassEncoderSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPassEncoderRelease")>]
    extern void RenderPassEncoderRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPipelineGetBindGroupLayout")>]
    extern nativeint RenderPipelineGetBindGroupLayout(nativeint self, uint32 groupIndex)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPipelineSetLabel")>]
    extern void RenderPipelineSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuRenderPipelineRelease")>]
    extern void RenderPipelineRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuSamplerSetLabel")>]
    extern void SamplerSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuSamplerRelease")>]
    extern void SamplerRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuShaderModuleGetCompilationInfo")>]
    extern void ShaderModuleGetCompilationInfo(nativeint self, nativeint callback, nativeint userdata)
    [<DllImport("WebGPU", EntryPoint="gpuShaderModuleGetCompilationInfoF")>]
    extern Future ShaderModuleGetCompilationInfoF(nativeint self, CompilationInfoCallbackInfo callbackInfo)
    [<DllImport("WebGPU", EntryPoint="gpuShaderModuleGetCompilationInfo2")>]
    extern Future ShaderModuleGetCompilationInfo2(nativeint self, CompilationInfoCallbackInfo2 callbackInfo)
    [<DllImport("WebGPU", EntryPoint="gpuShaderModuleSetLabel")>]
    extern void ShaderModuleSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuShaderModuleRelease")>]
    extern void ShaderModuleRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuSurfaceConfigure")>]
    extern void SurfaceConfigure(nativeint self, SurfaceConfiguration* config)
    [<DllImport("WebGPU", EntryPoint="gpuSurfaceGetCapabilities")>]
    extern Status SurfaceGetCapabilities(nativeint self, nativeint adapter, SurfaceCapabilities* capabilities)
    [<DllImport("WebGPU", EntryPoint="gpuSurfaceGetCurrentTexture")>]
    extern void SurfaceGetCurrentTexture(nativeint self, SurfaceTexture* surfaceTexture)
    [<DllImport("WebGPU", EntryPoint="gpuSurfacePresent")>]
    extern void SurfacePresent(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuSurfaceUnconfigure")>]
    extern void SurfaceUnconfigure(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuSurfaceSetLabel")>]
    extern void SurfaceSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuSurfaceRelease")>]
    extern void SurfaceRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuTextureCreateView")>]
    extern nativeint TextureCreateView(nativeint self, TextureViewDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuTextureCreateErrorView")>]
    extern nativeint TextureCreateErrorView(nativeint self, TextureViewDescriptor* descriptor)
    [<DllImport("WebGPU", EntryPoint="gpuTextureSetLabel")>]
    extern void TextureSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuTextureGetWidth")>]
    extern uint32 TextureGetWidth(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuTextureGetHeight")>]
    extern uint32 TextureGetHeight(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuTextureGetDepthOrArrayLayers")>]
    extern uint32 TextureGetDepthOrArrayLayers(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuTextureGetMipLevelCount")>]
    extern uint32 TextureGetMipLevelCount(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuTextureGetSampleCount")>]
    extern uint32 TextureGetSampleCount(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuTextureGetDimension")>]
    extern TextureDimension TextureGetDimension(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuTextureGetFormat")>]
    extern TextureFormat TextureGetFormat(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuTextureGetUsage")>]
    extern TextureUsage TextureGetUsage(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuTextureDestroy")>]
    extern void TextureDestroy(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuTextureRelease")>]
    extern void TextureRelease(nativeint self)
    [<DllImport("WebGPU", EntryPoint="gpuTextureViewSetLabel")>]
    extern void TextureViewSetLabel(nativeint self, StringView label)
    [<DllImport("WebGPU", EntryPoint="gpuTextureViewRelease")>]
    extern void TextureViewRelease(nativeint self)
type WebGPUCallbacks() =
    static let requestAdapterCallbackCallbacks = Dictionary<nativeint, RequestAdapterCallback>()
    static let mutable requestAdapterCallbackCurrent = 0n
    static let requestAdapterCallbackDelegate = System.Delegate.CreateDelegate(typeof<RequestAdapterCallback>, typeof<WebGPUCallbacks>.GetMethod "RequestAdapterCallback")
    static let requestAdapterCallbackGC = GCHandle.Alloc(requestAdapterCallbackDelegate)
    static let requestAdapterCallbackPtr = Marshal.GetFunctionPointerForDelegate(requestAdapterCallbackDelegate)
    static let requestAdapterCallback2Callbacks = Dictionary<nativeint, RequestAdapterCallback2>()
    static let mutable requestAdapterCallback2Current = 0n
    static let requestAdapterCallback2Delegate = System.Delegate.CreateDelegate(typeof<RequestAdapterCallback2>, typeof<WebGPUCallbacks>.GetMethod "RequestAdapterCallback2")
    static let requestAdapterCallback2GC = GCHandle.Alloc(requestAdapterCallback2Delegate)
    static let requestAdapterCallback2Ptr = Marshal.GetFunctionPointerForDelegate(requestAdapterCallback2Delegate)
    static let dawnLoadCacheDataFunctionCallbacks = Dictionary<nativeint, DawnLoadCacheDataFunction>()
    static let mutable dawnLoadCacheDataFunctionCurrent = 0n
    static let dawnLoadCacheDataFunctionDelegate = System.Delegate.CreateDelegate(typeof<DawnLoadCacheDataFunction>, typeof<WebGPUCallbacks>.GetMethod "DawnLoadCacheDataFunction")
    static let dawnLoadCacheDataFunctionGC = GCHandle.Alloc(dawnLoadCacheDataFunctionDelegate)
    static let dawnLoadCacheDataFunctionPtr = Marshal.GetFunctionPointerForDelegate(dawnLoadCacheDataFunctionDelegate)
    static let dawnStoreCacheDataFunctionCallbacks = Dictionary<nativeint, DawnStoreCacheDataFunction>()
    static let mutable dawnStoreCacheDataFunctionCurrent = 0n
    static let dawnStoreCacheDataFunctionDelegate = System.Delegate.CreateDelegate(typeof<DawnStoreCacheDataFunction>, typeof<WebGPUCallbacks>.GetMethod "DawnStoreCacheDataFunction")
    static let dawnStoreCacheDataFunctionGC = GCHandle.Alloc(dawnStoreCacheDataFunctionDelegate)
    static let dawnStoreCacheDataFunctionPtr = Marshal.GetFunctionPointerForDelegate(dawnStoreCacheDataFunctionDelegate)
    static let callbackCallbacks = Dictionary<nativeint, Callback>()
    static let mutable callbackCurrent = 0n
    static let callbackDelegate = System.Delegate.CreateDelegate(typeof<Callback>, typeof<WebGPUCallbacks>.GetMethod "Callback")
    static let callbackGC = GCHandle.Alloc(callbackDelegate)
    static let callbackPtr = Marshal.GetFunctionPointerForDelegate(callbackDelegate)
    static let bufferMapCallbackCallbacks = Dictionary<nativeint, BufferMapCallback>()
    static let mutable bufferMapCallbackCurrent = 0n
    static let bufferMapCallbackDelegate = System.Delegate.CreateDelegate(typeof<BufferMapCallback>, typeof<WebGPUCallbacks>.GetMethod "BufferMapCallback")
    static let bufferMapCallbackGC = GCHandle.Alloc(bufferMapCallbackDelegate)
    static let bufferMapCallbackPtr = Marshal.GetFunctionPointerForDelegate(bufferMapCallbackDelegate)
    static let bufferMapCallback2Callbacks = Dictionary<nativeint, BufferMapCallback2>()
    static let mutable bufferMapCallback2Current = 0n
    static let bufferMapCallback2Delegate = System.Delegate.CreateDelegate(typeof<BufferMapCallback2>, typeof<WebGPUCallbacks>.GetMethod "BufferMapCallback2")
    static let bufferMapCallback2GC = GCHandle.Alloc(bufferMapCallback2Delegate)
    static let bufferMapCallback2Ptr = Marshal.GetFunctionPointerForDelegate(bufferMapCallback2Delegate)
    static let compilationInfoCallbackCallbacks = Dictionary<nativeint, CompilationInfoCallback>()
    static let mutable compilationInfoCallbackCurrent = 0n
    static let compilationInfoCallbackDelegate = System.Delegate.CreateDelegate(typeof<CompilationInfoCallback>, typeof<WebGPUCallbacks>.GetMethod "CompilationInfoCallback")
    static let compilationInfoCallbackGC = GCHandle.Alloc(compilationInfoCallbackDelegate)
    static let compilationInfoCallbackPtr = Marshal.GetFunctionPointerForDelegate(compilationInfoCallbackDelegate)
    static let compilationInfoCallback2Callbacks = Dictionary<nativeint, CompilationInfoCallback2>()
    static let mutable compilationInfoCallback2Current = 0n
    static let compilationInfoCallback2Delegate = System.Delegate.CreateDelegate(typeof<CompilationInfoCallback2>, typeof<WebGPUCallbacks>.GetMethod "CompilationInfoCallback2")
    static let compilationInfoCallback2GC = GCHandle.Alloc(compilationInfoCallback2Delegate)
    static let compilationInfoCallback2Ptr = Marshal.GetFunctionPointerForDelegate(compilationInfoCallback2Delegate)
    static let createComputePipelineAsyncCallbackCallbacks = Dictionary<nativeint, CreateComputePipelineAsyncCallback>()
    static let mutable createComputePipelineAsyncCallbackCurrent = 0n
    static let createComputePipelineAsyncCallbackDelegate = System.Delegate.CreateDelegate(typeof<CreateComputePipelineAsyncCallback>, typeof<WebGPUCallbacks>.GetMethod "CreateComputePipelineAsyncCallback")
    static let createComputePipelineAsyncCallbackGC = GCHandle.Alloc(createComputePipelineAsyncCallbackDelegate)
    static let createComputePipelineAsyncCallbackPtr = Marshal.GetFunctionPointerForDelegate(createComputePipelineAsyncCallbackDelegate)
    static let createComputePipelineAsyncCallback2Callbacks = Dictionary<nativeint, CreateComputePipelineAsyncCallback2>()
    static let mutable createComputePipelineAsyncCallback2Current = 0n
    static let createComputePipelineAsyncCallback2Delegate = System.Delegate.CreateDelegate(typeof<CreateComputePipelineAsyncCallback2>, typeof<WebGPUCallbacks>.GetMethod "CreateComputePipelineAsyncCallback2")
    static let createComputePipelineAsyncCallback2GC = GCHandle.Alloc(createComputePipelineAsyncCallback2Delegate)
    static let createComputePipelineAsyncCallback2Ptr = Marshal.GetFunctionPointerForDelegate(createComputePipelineAsyncCallback2Delegate)
    static let createRenderPipelineAsyncCallbackCallbacks = Dictionary<nativeint, CreateRenderPipelineAsyncCallback>()
    static let mutable createRenderPipelineAsyncCallbackCurrent = 0n
    static let createRenderPipelineAsyncCallbackDelegate = System.Delegate.CreateDelegate(typeof<CreateRenderPipelineAsyncCallback>, typeof<WebGPUCallbacks>.GetMethod "CreateRenderPipelineAsyncCallback")
    static let createRenderPipelineAsyncCallbackGC = GCHandle.Alloc(createRenderPipelineAsyncCallbackDelegate)
    static let createRenderPipelineAsyncCallbackPtr = Marshal.GetFunctionPointerForDelegate(createRenderPipelineAsyncCallbackDelegate)
    static let createRenderPipelineAsyncCallback2Callbacks = Dictionary<nativeint, CreateRenderPipelineAsyncCallback2>()
    static let mutable createRenderPipelineAsyncCallback2Current = 0n
    static let createRenderPipelineAsyncCallback2Delegate = System.Delegate.CreateDelegate(typeof<CreateRenderPipelineAsyncCallback2>, typeof<WebGPUCallbacks>.GetMethod "CreateRenderPipelineAsyncCallback2")
    static let createRenderPipelineAsyncCallback2GC = GCHandle.Alloc(createRenderPipelineAsyncCallback2Delegate)
    static let createRenderPipelineAsyncCallback2Ptr = Marshal.GetFunctionPointerForDelegate(createRenderPipelineAsyncCallback2Delegate)
    static let deviceLostCallbackCallbacks = Dictionary<nativeint, DeviceLostCallback>()
    static let mutable deviceLostCallbackCurrent = 0n
    static let deviceLostCallbackDelegate = System.Delegate.CreateDelegate(typeof<DeviceLostCallback>, typeof<WebGPUCallbacks>.GetMethod "DeviceLostCallback")
    static let deviceLostCallbackGC = GCHandle.Alloc(deviceLostCallbackDelegate)
    static let deviceLostCallbackPtr = Marshal.GetFunctionPointerForDelegate(deviceLostCallbackDelegate)
    static let deviceLostCallbackNewCallbacks = Dictionary<nativeint, DeviceLostCallbackNew>()
    static let mutable deviceLostCallbackNewCurrent = 0n
    static let deviceLostCallbackNewDelegate = System.Delegate.CreateDelegate(typeof<DeviceLostCallbackNew>, typeof<WebGPUCallbacks>.GetMethod "DeviceLostCallbackNew")
    static let deviceLostCallbackNewGC = GCHandle.Alloc(deviceLostCallbackNewDelegate)
    static let deviceLostCallbackNewPtr = Marshal.GetFunctionPointerForDelegate(deviceLostCallbackNewDelegate)
    static let deviceLostCallback2Callbacks = Dictionary<nativeint, DeviceLostCallback2>()
    static let mutable deviceLostCallback2Current = 0n
    static let deviceLostCallback2Delegate = System.Delegate.CreateDelegate(typeof<DeviceLostCallback2>, typeof<WebGPUCallbacks>.GetMethod "DeviceLostCallback2")
    static let deviceLostCallback2GC = GCHandle.Alloc(deviceLostCallback2Delegate)
    static let deviceLostCallback2Ptr = Marshal.GetFunctionPointerForDelegate(deviceLostCallback2Delegate)
    static let errorCallbackCallbacks = Dictionary<nativeint, ErrorCallback>()
    static let mutable errorCallbackCurrent = 0n
    static let errorCallbackDelegate = System.Delegate.CreateDelegate(typeof<ErrorCallback>, typeof<WebGPUCallbacks>.GetMethod "ErrorCallback")
    static let errorCallbackGC = GCHandle.Alloc(errorCallbackDelegate)
    static let errorCallbackPtr = Marshal.GetFunctionPointerForDelegate(errorCallbackDelegate)
    static let uncapturedErrorCallbackCallbacks = Dictionary<nativeint, UncapturedErrorCallback>()
    static let mutable uncapturedErrorCallbackCurrent = 0n
    static let uncapturedErrorCallbackDelegate = System.Delegate.CreateDelegate(typeof<UncapturedErrorCallback>, typeof<WebGPUCallbacks>.GetMethod "UncapturedErrorCallback")
    static let uncapturedErrorCallbackGC = GCHandle.Alloc(uncapturedErrorCallbackDelegate)
    static let uncapturedErrorCallbackPtr = Marshal.GetFunctionPointerForDelegate(uncapturedErrorCallbackDelegate)
    static let popErrorScopeCallbackCallbacks = Dictionary<nativeint, PopErrorScopeCallback>()
    static let mutable popErrorScopeCallbackCurrent = 0n
    static let popErrorScopeCallbackDelegate = System.Delegate.CreateDelegate(typeof<PopErrorScopeCallback>, typeof<WebGPUCallbacks>.GetMethod "PopErrorScopeCallback")
    static let popErrorScopeCallbackGC = GCHandle.Alloc(popErrorScopeCallbackDelegate)
    static let popErrorScopeCallbackPtr = Marshal.GetFunctionPointerForDelegate(popErrorScopeCallbackDelegate)
    static let popErrorScopeCallback2Callbacks = Dictionary<nativeint, PopErrorScopeCallback2>()
    static let mutable popErrorScopeCallback2Current = 0n
    static let popErrorScopeCallback2Delegate = System.Delegate.CreateDelegate(typeof<PopErrorScopeCallback2>, typeof<WebGPUCallbacks>.GetMethod "PopErrorScopeCallback2")
    static let popErrorScopeCallback2GC = GCHandle.Alloc(popErrorScopeCallback2Delegate)
    static let popErrorScopeCallback2Ptr = Marshal.GetFunctionPointerForDelegate(popErrorScopeCallback2Delegate)
    static let loggingCallbackCallbacks = Dictionary<nativeint, LoggingCallback>()
    static let mutable loggingCallbackCurrent = 0n
    static let loggingCallbackDelegate = System.Delegate.CreateDelegate(typeof<LoggingCallback>, typeof<WebGPUCallbacks>.GetMethod "LoggingCallback")
    static let loggingCallbackGC = GCHandle.Alloc(loggingCallbackDelegate)
    static let loggingCallbackPtr = Marshal.GetFunctionPointerForDelegate(loggingCallbackDelegate)
    static let queueWorkDoneCallbackCallbacks = Dictionary<nativeint, QueueWorkDoneCallback>()
    static let mutable queueWorkDoneCallbackCurrent = 0n
    static let queueWorkDoneCallbackDelegate = System.Delegate.CreateDelegate(typeof<QueueWorkDoneCallback>, typeof<WebGPUCallbacks>.GetMethod "QueueWorkDoneCallback")
    static let queueWorkDoneCallbackGC = GCHandle.Alloc(queueWorkDoneCallbackDelegate)
    static let queueWorkDoneCallbackPtr = Marshal.GetFunctionPointerForDelegate(queueWorkDoneCallbackDelegate)
    static let queueWorkDoneCallback2Callbacks = Dictionary<nativeint, QueueWorkDoneCallback2>()
    static let mutable queueWorkDoneCallback2Current = 0n
    static let queueWorkDoneCallback2Delegate = System.Delegate.CreateDelegate(typeof<QueueWorkDoneCallback2>, typeof<WebGPUCallbacks>.GetMethod "QueueWorkDoneCallback2")
    static let queueWorkDoneCallback2GC = GCHandle.Alloc(queueWorkDoneCallback2Delegate)
    static let queueWorkDoneCallback2Ptr = Marshal.GetFunctionPointerForDelegate(queueWorkDoneCallback2Delegate)
    static let requestDeviceCallbackCallbacks = Dictionary<nativeint, RequestDeviceCallback>()
    static let mutable requestDeviceCallbackCurrent = 0n
    static let requestDeviceCallbackDelegate = System.Delegate.CreateDelegate(typeof<RequestDeviceCallback>, typeof<WebGPUCallbacks>.GetMethod "RequestDeviceCallback")
    static let requestDeviceCallbackGC = GCHandle.Alloc(requestDeviceCallbackDelegate)
    static let requestDeviceCallbackPtr = Marshal.GetFunctionPointerForDelegate(requestDeviceCallbackDelegate)
    static let requestDeviceCallback2Callbacks = Dictionary<nativeint, RequestDeviceCallback2>()
    static let mutable requestDeviceCallback2Current = 0n
    static let requestDeviceCallback2Delegate = System.Delegate.CreateDelegate(typeof<RequestDeviceCallback2>, typeof<WebGPUCallbacks>.GetMethod "RequestDeviceCallback2")
    static let requestDeviceCallback2GC = GCHandle.Alloc(requestDeviceCallback2Delegate)
    static let requestDeviceCallback2Ptr = Marshal.GetFunctionPointerForDelegate(requestDeviceCallback2Delegate)
    static member RequestAdapterCallback(status : RequestAdapterStatus, adapter : nativeint, message : StringView, userdata : nativeint) =
        let callback = 
            lock requestAdapterCallbackCallbacks (fun () ->
                match requestAdapterCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(status, adapter, message, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : RequestAdapterCallback) =
        lock requestAdapterCallbackCallbacks (fun () ->
            let id = requestAdapterCallbackCurrent
            requestAdapterCallbackCurrent <- requestAdapterCallbackCurrent + 1n
            requestAdapterCallbackCallbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = requestAdapterCallbackCallbacks.Remove(id) |> ignore }
            struct(requestAdapterCallbackPtr, id, disp)
        )
    static member RequestAdapterCallback2(status : RequestAdapterStatus, adapter : nativeint, message : StringView, userdata1 : nativeint, userdata2 : nativeint) =
        let callback = 
            lock requestAdapterCallback2Callbacks (fun () ->
                match requestAdapterCallback2Callbacks.TryGetValue(userdata1) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(status, adapter, message, 0n, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : RequestAdapterCallback2) =
        lock requestAdapterCallback2Callbacks (fun () ->
            let id = requestAdapterCallback2Current
            requestAdapterCallback2Current <- requestAdapterCallback2Current + 1n
            requestAdapterCallback2Callbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = requestAdapterCallback2Callbacks.Remove(id) |> ignore }
            struct(requestAdapterCallback2Ptr, id, disp)
        )
    static member DawnLoadCacheDataFunction(key : nativeint, keySize : unativeint, value : nativeint, valueSize : unativeint, userdata : nativeint) =
        let callback = 
            lock dawnLoadCacheDataFunctionCallbacks (fun () ->
                match dawnLoadCacheDataFunctionCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(key, keySize, value, valueSize, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : DawnLoadCacheDataFunction) =
        lock dawnLoadCacheDataFunctionCallbacks (fun () ->
            let id = dawnLoadCacheDataFunctionCurrent
            dawnLoadCacheDataFunctionCurrent <- dawnLoadCacheDataFunctionCurrent + 1n
            dawnLoadCacheDataFunctionCallbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = dawnLoadCacheDataFunctionCallbacks.Remove(id) |> ignore }
            struct(dawnLoadCacheDataFunctionPtr, id, disp)
        )
    static member DawnStoreCacheDataFunction(key : nativeint, keySize : unativeint, value : nativeint, valueSize : unativeint, userdata : nativeint) =
        let callback = 
            lock dawnStoreCacheDataFunctionCallbacks (fun () ->
                match dawnStoreCacheDataFunctionCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(key, keySize, value, valueSize, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : DawnStoreCacheDataFunction) =
        lock dawnStoreCacheDataFunctionCallbacks (fun () ->
            let id = dawnStoreCacheDataFunctionCurrent
            dawnStoreCacheDataFunctionCurrent <- dawnStoreCacheDataFunctionCurrent + 1n
            dawnStoreCacheDataFunctionCallbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = dawnStoreCacheDataFunctionCallbacks.Remove(id) |> ignore }
            struct(dawnStoreCacheDataFunctionPtr, id, disp)
        )
    static member Callback(userdata : nativeint) =
        let callback = 
            lock callbackCallbacks (fun () ->
                match callbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : Callback) =
        lock callbackCallbacks (fun () ->
            let id = callbackCurrent
            callbackCurrent <- callbackCurrent + 1n
            callbackCallbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = callbackCallbacks.Remove(id) |> ignore }
            struct(callbackPtr, id, disp)
        )
    static member BufferMapCallback(status : BufferMapAsyncStatus, userdata : nativeint) =
        let callback = 
            lock bufferMapCallbackCallbacks (fun () ->
                match bufferMapCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(status, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : BufferMapCallback) =
        lock bufferMapCallbackCallbacks (fun () ->
            let id = bufferMapCallbackCurrent
            bufferMapCallbackCurrent <- bufferMapCallbackCurrent + 1n
            bufferMapCallbackCallbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = bufferMapCallbackCallbacks.Remove(id) |> ignore }
            struct(bufferMapCallbackPtr, id, disp)
        )
    static member BufferMapCallback2(status : MapAsyncStatus, message : StringView, userdata1 : nativeint, userdata2 : nativeint) =
        let callback = 
            lock bufferMapCallback2Callbacks (fun () ->
                match bufferMapCallback2Callbacks.TryGetValue(userdata1) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(status, message, 0n, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : BufferMapCallback2) =
        lock bufferMapCallback2Callbacks (fun () ->
            let id = bufferMapCallback2Current
            bufferMapCallback2Current <- bufferMapCallback2Current + 1n
            bufferMapCallback2Callbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = bufferMapCallback2Callbacks.Remove(id) |> ignore }
            struct(bufferMapCallback2Ptr, id, disp)
        )
    static member CompilationInfoCallback(status : CompilationInfoRequestStatus, compilationInfo : nativeptr<CompilationInfo>, userdata : nativeint) =
        let callback = 
            lock compilationInfoCallbackCallbacks (fun () ->
                match compilationInfoCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(status, compilationInfo, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : CompilationInfoCallback) =
        lock compilationInfoCallbackCallbacks (fun () ->
            let id = compilationInfoCallbackCurrent
            compilationInfoCallbackCurrent <- compilationInfoCallbackCurrent + 1n
            compilationInfoCallbackCallbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = compilationInfoCallbackCallbacks.Remove(id) |> ignore }
            struct(compilationInfoCallbackPtr, id, disp)
        )
    static member CompilationInfoCallback2(status : CompilationInfoRequestStatus, compilationInfo : nativeptr<CompilationInfo>, userdata1 : nativeint, userdata2 : nativeint) =
        let callback = 
            lock compilationInfoCallback2Callbacks (fun () ->
                match compilationInfoCallback2Callbacks.TryGetValue(userdata1) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(status, compilationInfo, 0n, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : CompilationInfoCallback2) =
        lock compilationInfoCallback2Callbacks (fun () ->
            let id = compilationInfoCallback2Current
            compilationInfoCallback2Current <- compilationInfoCallback2Current + 1n
            compilationInfoCallback2Callbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = compilationInfoCallback2Callbacks.Remove(id) |> ignore }
            struct(compilationInfoCallback2Ptr, id, disp)
        )
    static member CreateComputePipelineAsyncCallback(status : CreatePipelineAsyncStatus, pipeline : nativeint, message : StringView, userdata : nativeint) =
        let callback = 
            lock createComputePipelineAsyncCallbackCallbacks (fun () ->
                match createComputePipelineAsyncCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(status, pipeline, message, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : CreateComputePipelineAsyncCallback) =
        lock createComputePipelineAsyncCallbackCallbacks (fun () ->
            let id = createComputePipelineAsyncCallbackCurrent
            createComputePipelineAsyncCallbackCurrent <- createComputePipelineAsyncCallbackCurrent + 1n
            createComputePipelineAsyncCallbackCallbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = createComputePipelineAsyncCallbackCallbacks.Remove(id) |> ignore }
            struct(createComputePipelineAsyncCallbackPtr, id, disp)
        )
    static member CreateComputePipelineAsyncCallback2(status : CreatePipelineAsyncStatus, pipeline : nativeint, message : StringView, userdata1 : nativeint, userdata2 : nativeint) =
        let callback = 
            lock createComputePipelineAsyncCallback2Callbacks (fun () ->
                match createComputePipelineAsyncCallback2Callbacks.TryGetValue(userdata1) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(status, pipeline, message, 0n, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : CreateComputePipelineAsyncCallback2) =
        lock createComputePipelineAsyncCallback2Callbacks (fun () ->
            let id = createComputePipelineAsyncCallback2Current
            createComputePipelineAsyncCallback2Current <- createComputePipelineAsyncCallback2Current + 1n
            createComputePipelineAsyncCallback2Callbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = createComputePipelineAsyncCallback2Callbacks.Remove(id) |> ignore }
            struct(createComputePipelineAsyncCallback2Ptr, id, disp)
        )
    static member CreateRenderPipelineAsyncCallback(status : CreatePipelineAsyncStatus, pipeline : nativeint, message : StringView, userdata : nativeint) =
        let callback = 
            lock createRenderPipelineAsyncCallbackCallbacks (fun () ->
                match createRenderPipelineAsyncCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(status, pipeline, message, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : CreateRenderPipelineAsyncCallback) =
        lock createRenderPipelineAsyncCallbackCallbacks (fun () ->
            let id = createRenderPipelineAsyncCallbackCurrent
            createRenderPipelineAsyncCallbackCurrent <- createRenderPipelineAsyncCallbackCurrent + 1n
            createRenderPipelineAsyncCallbackCallbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = createRenderPipelineAsyncCallbackCallbacks.Remove(id) |> ignore }
            struct(createRenderPipelineAsyncCallbackPtr, id, disp)
        )
    static member CreateRenderPipelineAsyncCallback2(status : CreatePipelineAsyncStatus, pipeline : nativeint, message : StringView, userdata1 : nativeint, userdata2 : nativeint) =
        let callback = 
            lock createRenderPipelineAsyncCallback2Callbacks (fun () ->
                match createRenderPipelineAsyncCallback2Callbacks.TryGetValue(userdata1) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(status, pipeline, message, 0n, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : CreateRenderPipelineAsyncCallback2) =
        lock createRenderPipelineAsyncCallback2Callbacks (fun () ->
            let id = createRenderPipelineAsyncCallback2Current
            createRenderPipelineAsyncCallback2Current <- createRenderPipelineAsyncCallback2Current + 1n
            createRenderPipelineAsyncCallback2Callbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = createRenderPipelineAsyncCallback2Callbacks.Remove(id) |> ignore }
            struct(createRenderPipelineAsyncCallback2Ptr, id, disp)
        )
    static member DeviceLostCallback(reason : DeviceLostReason, message : StringView, userdata : nativeint) =
        let callback = 
            lock deviceLostCallbackCallbacks (fun () ->
                match deviceLostCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(reason, message, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : DeviceLostCallback) =
        lock deviceLostCallbackCallbacks (fun () ->
            let id = deviceLostCallbackCurrent
            deviceLostCallbackCurrent <- deviceLostCallbackCurrent + 1n
            deviceLostCallbackCallbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = deviceLostCallbackCallbacks.Remove(id) |> ignore }
            struct(deviceLostCallbackPtr, id, disp)
        )
    static member DeviceLostCallbackNew(device : nativeptr<nativeint>, reason : DeviceLostReason, message : StringView, userdata : nativeint) =
        let callback = 
            lock deviceLostCallbackNewCallbacks (fun () ->
                match deviceLostCallbackNewCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(device, reason, message, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : DeviceLostCallbackNew) =
        lock deviceLostCallbackNewCallbacks (fun () ->
            let id = deviceLostCallbackNewCurrent
            deviceLostCallbackNewCurrent <- deviceLostCallbackNewCurrent + 1n
            deviceLostCallbackNewCallbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = deviceLostCallbackNewCallbacks.Remove(id) |> ignore }
            struct(deviceLostCallbackNewPtr, id, disp)
        )
    static member DeviceLostCallback2(device : nativeptr<nativeint>, reason : DeviceLostReason, message : StringView, userdata1 : nativeint, userdata2 : nativeint) =
        let callback = 
            lock deviceLostCallback2Callbacks (fun () ->
                match deviceLostCallback2Callbacks.TryGetValue(userdata1) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(device, reason, message, 0n, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : DeviceLostCallback2) =
        lock deviceLostCallback2Callbacks (fun () ->
            let id = deviceLostCallback2Current
            deviceLostCallback2Current <- deviceLostCallback2Current + 1n
            deviceLostCallback2Callbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = deviceLostCallback2Callbacks.Remove(id) |> ignore }
            struct(deviceLostCallback2Ptr, id, disp)
        )
    static member ErrorCallback(typ : ErrorType, message : StringView, userdata : nativeint) =
        let callback = 
            lock errorCallbackCallbacks (fun () ->
                match errorCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(typ, message, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : ErrorCallback) =
        lock errorCallbackCallbacks (fun () ->
            let id = errorCallbackCurrent
            errorCallbackCurrent <- errorCallbackCurrent + 1n
            errorCallbackCallbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = errorCallbackCallbacks.Remove(id) |> ignore }
            struct(errorCallbackPtr, id, disp)
        )
    static member UncapturedErrorCallback(device : nativeptr<nativeint>, typ : ErrorType, message : StringView, userdata1 : nativeint, userdata2 : nativeint) =
        let callback = 
            lock uncapturedErrorCallbackCallbacks (fun () ->
                match uncapturedErrorCallbackCallbacks.TryGetValue(userdata1) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(device, typ, message, 0n, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : UncapturedErrorCallback) =
        lock uncapturedErrorCallbackCallbacks (fun () ->
            let id = uncapturedErrorCallbackCurrent
            uncapturedErrorCallbackCurrent <- uncapturedErrorCallbackCurrent + 1n
            uncapturedErrorCallbackCallbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = uncapturedErrorCallbackCallbacks.Remove(id) |> ignore }
            struct(uncapturedErrorCallbackPtr, id, disp)
        )
    static member PopErrorScopeCallback(status : PopErrorScopeStatus, typ : ErrorType, message : StringView, userdata : nativeint) =
        let callback = 
            lock popErrorScopeCallbackCallbacks (fun () ->
                match popErrorScopeCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(status, typ, message, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : PopErrorScopeCallback) =
        lock popErrorScopeCallbackCallbacks (fun () ->
            let id = popErrorScopeCallbackCurrent
            popErrorScopeCallbackCurrent <- popErrorScopeCallbackCurrent + 1n
            popErrorScopeCallbackCallbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = popErrorScopeCallbackCallbacks.Remove(id) |> ignore }
            struct(popErrorScopeCallbackPtr, id, disp)
        )
    static member PopErrorScopeCallback2(status : PopErrorScopeStatus, typ : ErrorType, message : StringView, userdata1 : nativeint, userdata2 : nativeint) =
        let callback = 
            lock popErrorScopeCallback2Callbacks (fun () ->
                match popErrorScopeCallback2Callbacks.TryGetValue(userdata1) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(status, typ, message, 0n, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : PopErrorScopeCallback2) =
        lock popErrorScopeCallback2Callbacks (fun () ->
            let id = popErrorScopeCallback2Current
            popErrorScopeCallback2Current <- popErrorScopeCallback2Current + 1n
            popErrorScopeCallback2Callbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = popErrorScopeCallback2Callbacks.Remove(id) |> ignore }
            struct(popErrorScopeCallback2Ptr, id, disp)
        )
    static member LoggingCallback(typ : LoggingType, message : StringView, userdata : nativeint) =
        let callback = 
            lock loggingCallbackCallbacks (fun () ->
                match loggingCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(typ, message, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : LoggingCallback) =
        lock loggingCallbackCallbacks (fun () ->
            let id = loggingCallbackCurrent
            loggingCallbackCurrent <- loggingCallbackCurrent + 1n
            loggingCallbackCallbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = loggingCallbackCallbacks.Remove(id) |> ignore }
            struct(loggingCallbackPtr, id, disp)
        )
    static member QueueWorkDoneCallback(status : QueueWorkDoneStatus, userdata : nativeint) =
        let callback = 
            lock queueWorkDoneCallbackCallbacks (fun () ->
                match queueWorkDoneCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(status, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : QueueWorkDoneCallback) =
        lock queueWorkDoneCallbackCallbacks (fun () ->
            let id = queueWorkDoneCallbackCurrent
            queueWorkDoneCallbackCurrent <- queueWorkDoneCallbackCurrent + 1n
            queueWorkDoneCallbackCallbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = queueWorkDoneCallbackCallbacks.Remove(id) |> ignore }
            struct(queueWorkDoneCallbackPtr, id, disp)
        )
    static member QueueWorkDoneCallback2(status : QueueWorkDoneStatus, userdata1 : nativeint, userdata2 : nativeint) =
        let callback = 
            lock queueWorkDoneCallback2Callbacks (fun () ->
                match queueWorkDoneCallback2Callbacks.TryGetValue(userdata1) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(status, 0n, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : QueueWorkDoneCallback2) =
        lock queueWorkDoneCallback2Callbacks (fun () ->
            let id = queueWorkDoneCallback2Current
            queueWorkDoneCallback2Current <- queueWorkDoneCallback2Current + 1n
            queueWorkDoneCallback2Callbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = queueWorkDoneCallback2Callbacks.Remove(id) |> ignore }
            struct(queueWorkDoneCallback2Ptr, id, disp)
        )
    static member RequestDeviceCallback(status : RequestDeviceStatus, device : nativeint, message : StringView, userdata : nativeint) =
        let callback = 
            lock requestDeviceCallbackCallbacks (fun () ->
                match requestDeviceCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(status, device, message, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : RequestDeviceCallback) =
        lock requestDeviceCallbackCallbacks (fun () ->
            let id = requestDeviceCallbackCurrent
            requestDeviceCallbackCurrent <- requestDeviceCallbackCurrent + 1n
            requestDeviceCallbackCallbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = requestDeviceCallbackCallbacks.Remove(id) |> ignore }
            struct(requestDeviceCallbackPtr, id, disp)
        )
    static member RequestDeviceCallback2(status : RequestDeviceStatus, device : nativeint, message : StringView, userdata1 : nativeint, userdata2 : nativeint) =
        let callback = 
            lock requestDeviceCallback2Callbacks (fun () ->
                match requestDeviceCallback2Callbacks.TryGetValue(userdata1) with
                | (true, cb) ->
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(status, device, message, 0n, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : RequestDeviceCallback2) =
        lock requestDeviceCallback2Callbacks (fun () ->
            let id = requestDeviceCallback2Current
            requestDeviceCallback2Current <- requestDeviceCallback2Current + 1n
            requestDeviceCallback2Callbacks.[id] <- cb
            let disp = { new System.IDisposable with member x.Dispose() = requestDeviceCallback2Callbacks.Remove(id) |> ignore }
            struct(requestDeviceCallback2Ptr, id, disp)
        )
