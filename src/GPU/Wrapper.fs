namespace rec WebGPU.Raw
open System.Collections.Generic
open System
open System.Runtime.InteropServices
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
type RequestAdapterCallback2 = delegate of status : RequestAdapterStatus * adapter : nativeint * message : StringView -> unit
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
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        new(mode : CallbackMode, callback : nativeint) = { Mode = mode; Callback = callback }
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
type DawnLoadCacheDataFunction = delegate of key : nativeint * keySize : unativeint * value : nativeint * valueSize : unativeint * userdata : nativeint -> unativeint
type DawnStoreCacheDataFunction = delegate of key : nativeint * keySize : unativeint * value : nativeint * valueSize : unativeint * userdata : nativeint -> unit
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
type Callback = delegate of userdata : nativeint -> unit
type BufferMapCallback = delegate of status : BufferMapAsyncStatus * userdata : nativeint -> unit
type BufferMapCallback2 = delegate of status : MapAsyncStatus * message : StringView -> unit
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
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        new(mode : CallbackMode, callback : nativeint) = { Mode = mode; Callback = callback }
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
type CompilationInfoCallback2 = delegate of status : CompilationInfoRequestStatus * compilationInfo : nativeptr<CompilationInfo> -> unit
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
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        new(mode : CallbackMode, callback : nativeint) = { Mode = mode; Callback = callback }
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
type CreateComputePipelineAsyncCallback = delegate of status : CreatePipelineAsyncStatus * pipeline : nativeint * message : StringView * userdata : nativeint -> unit
type CreateComputePipelineAsyncCallback2 = delegate of status : CreatePipelineAsyncStatus * pipeline : nativeint * message : StringView -> unit
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
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        new(mode : CallbackMode, callback : nativeint) = { Mode = mode; Callback = callback }
    end
type CreateRenderPipelineAsyncCallback = delegate of status : CreatePipelineAsyncStatus * pipeline : nativeint * message : StringView * userdata : nativeint -> unit
type CreateRenderPipelineAsyncCallback2 = delegate of status : CreatePipelineAsyncStatus * pipeline : nativeint * message : StringView -> unit
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
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        new(mode : CallbackMode, callback : nativeint) = { Mode = mode; Callback = callback }
    end
type DeviceLostCallback = delegate of reason : DeviceLostReason * message : StringView * userdata : nativeint -> unit
type DeviceLostCallbackNew = delegate of device : nativeptr<nativeint> * reason : DeviceLostReason * message : StringView * userdata : nativeint -> unit
type DeviceLostCallback2 = delegate of device : nativeptr<nativeint> * reason : DeviceLostReason * message : StringView -> unit
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
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        new(mode : CallbackMode, callback : nativeint) = { Mode = mode; Callback = callback }
    end
type ErrorCallback = delegate of typ : ErrorType * message : StringView * userdata : nativeint -> unit
type UncapturedErrorCallback = delegate of device : nativeptr<nativeint> * typ : ErrorType * message : StringView -> unit
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
        val mutable public Callback : nativeint
        new(callback : nativeint) = { Callback = callback }
    end
type PopErrorScopeCallback = delegate of status : PopErrorScopeStatus * typ : ErrorType * message : StringView * userdata : nativeint -> unit
type PopErrorScopeCallback2 = delegate of status : PopErrorScopeStatus * typ : ErrorType * message : StringView -> unit
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
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        new(mode : CallbackMode, callback : nativeint) = { Mode = mode; Callback = callback }
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
type Extent3D = 
    struct
        val mutable public Width : uint32
        val mutable public Height : uint32
        val mutable public DepthOrArrayLayers : uint32
        new(width : uint32, height : uint32, depthOrArrayLayers : uint32) = { Width = width; Height = height; DepthOrArrayLayers = depthOrArrayLayers }
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
type QueueWorkDoneCallback2 = delegate of status : QueueWorkDoneStatus -> unit
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
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        new(mode : CallbackMode, callback : nativeint) = { Mode = mode; Callback = callback }
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
type RenderPassTimestampWrites = 
    struct
        val mutable public QuerySet : nativeint
        val mutable public BeginningOfPassWriteIndex : uint32
        val mutable public EndOfPassWriteIndex : uint32
        new(querySet : nativeint, beginningOfPassWriteIndex : uint32, endOfPassWriteIndex : uint32) = { QuerySet = querySet; BeginningOfPassWriteIndex = beginningOfPassWriteIndex; EndOfPassWriteIndex = endOfPassWriteIndex }
    end
type RequestDeviceCallback = delegate of status : RequestDeviceStatus * device : nativeint * message : StringView * userdata : nativeint -> unit
type RequestDeviceCallback2 = delegate of status : RequestDeviceStatus * device : nativeint * message : StringView -> unit
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
        val mutable public Mode : CallbackMode
        val mutable public Callback : nativeint
        new(mode : CallbackMode, callback : nativeint) = { Mode = mode; Callback = callback }
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
type SurfaceDescriptorFromWindowsHWND = SurfaceSourceWindowsHWND
type SurfaceDescriptorFromXcbWindow = SurfaceSourceXCBWindow
type SurfaceDescriptorFromXlibWindow = SurfaceSourceXlibWindow
type SurfaceDescriptorFromWaylandSurface = SurfaceSourceWaylandSurface
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
module WebGPU = 

    [<DllImport("Native", EntryPoint="gpuCreateInstance")>]
    extern nativeint CreateInstance(InstanceDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuGetProcAddress")>]
    extern nativeint GetProcAddress(StringView procName)
    [<DllImport("Native", EntryPoint="gpuAdapterGetLimits")>]
    extern Status AdapterGetLimits(nativeint self, SupportedLimits* limits)
    [<DllImport("Native", EntryPoint="gpuAdapterGetInfo")>]
    extern Status AdapterGetInfo(nativeint self, AdapterInfo* info)
    [<DllImport("Native", EntryPoint="gpuAdapterHasFeature")>]
    extern int AdapterHasFeature(nativeint self, FeatureName feature)
    [<DllImport("Native", EntryPoint="gpuAdapterGetFeatures")>]
    extern void AdapterGetFeatures(nativeint self, SupportedFeatures* features)
    [<DllImport("Native", EntryPoint="gpuAdapterRequestDevice")>]
    extern void AdapterRequestDevice(nativeint self, DeviceDescriptor* descriptor, nativeint callback, nativeint userdata)
    [<DllImport("Native", EntryPoint="gpuAdapterRequestDeviceF")>]
    extern Future AdapterRequestDeviceF(nativeint self, DeviceDescriptor* options, RequestDeviceCallbackInfo callbackInfo)
    [<DllImport("Native", EntryPoint="gpuAdapterRequestDevice2")>]
    extern Future AdapterRequestDevice2(nativeint self, DeviceDescriptor* options, RequestDeviceCallbackInfo2 callbackInfo)
    [<DllImport("Native", EntryPoint="gpuBindGroupSetLabel")>]
    extern void BindGroupSetLabel(nativeint self, StringView label)
    [<DllImport("Native", EntryPoint="gpuBindGroupLayoutSetLabel")>]
    extern void BindGroupLayoutSetLabel(nativeint self, StringView label)
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

    [<DllImport("Native", EntryPoint="gpuBufferMapAsync")>]
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
    [<DllImport("Native", EntryPoint="gpuBufferMapAsyncF")>]
    extern Future BufferMapAsyncF(nativeint self, MapMode mode, unativeint offset, unativeint size, BufferMapCallbackInfo callbackInfo)
    [<DllImport("Native", EntryPoint="gpuBufferMapAsync2")>]
    extern Future BufferMapAsync2(nativeint self, MapMode mode, unativeint offset, unativeint size, BufferMapCallbackInfo2 callbackInfo)
    [<DllImport("Native", EntryPoint="gpuBufferGetMappedRange")>]
    extern nativeint BufferGetMappedRange(nativeint self, unativeint offset, unativeint size)
    [<DllImport("Native", EntryPoint="gpuBufferGetConstMappedRange")>]
    extern nativeint BufferGetConstMappedRange(nativeint self, unativeint offset, unativeint size)
    [<DllImport("Native", EntryPoint="gpuBufferSetLabel")>]
    extern void BufferSetLabel(nativeint self, StringView label)
    [<DllImport("Native", EntryPoint="gpuBufferGetUsage")>]
    extern BufferUsage BufferGetUsage(nativeint self)
    [<DllImport("Native", EntryPoint="gpuBufferGetSize")>]
    extern uint64 BufferGetSize(nativeint self)
    [<DllImport("Native", EntryPoint="gpuBufferGetMapState")>]
    extern BufferMapState BufferGetMapState(nativeint self)
    [<DllImport("Native", EntryPoint="gpuBufferUnmap")>]
    extern void BufferUnmap(nativeint self)
    [<DllImport("Native", EntryPoint="gpuBufferDestroy")>]
    extern void BufferDestroy(nativeint self)
    [<DllImport("Native", EntryPoint="gpuCommandBufferSetLabel")>]
    extern void CommandBufferSetLabel(nativeint self, StringView label)
    [<DllImport("Native", EntryPoint="gpuCommandEncoderFinish")>]
    extern nativeint CommandEncoderFinish(nativeint self, CommandBufferDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuCommandEncoderBeginComputePass")>]
    extern nativeint CommandEncoderBeginComputePass(nativeint self, ComputePassDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuCommandEncoderBeginRenderPass")>]
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

    [<DllImport("Native", EntryPoint="gpuCommandEncoderCopyBufferToBuffer")>]
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
    [<DllImport("Native", EntryPoint="gpuCommandEncoderCopyBufferToTexture")>]
    extern void CommandEncoderCopyBufferToTexture(nativeint self, ImageCopyBuffer* source, ImageCopyTexture* destination, Extent3D* copySize)
    [<DllImport("Native", EntryPoint="gpuCommandEncoderCopyTextureToBuffer")>]
    extern void CommandEncoderCopyTextureToBuffer(nativeint self, ImageCopyTexture* source, ImageCopyBuffer* destination, Extent3D* copySize)
    [<DllImport("Native", EntryPoint="gpuCommandEncoderCopyTextureToTexture")>]
    extern void CommandEncoderCopyTextureToTexture(nativeint self, ImageCopyTexture* source, ImageCopyTexture* destination, Extent3D* copySize)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type CommandEncoderClearBufferArgs = 
        {
            Self : nativeint
            Buffer : nativeint
            Offset : uint64
            Size : uint64
        }

    [<DllImport("Native", EntryPoint="gpuCommandEncoderClearBuffer")>]
    extern void _CommandEncoderClearBuffer(CommandEncoderClearBufferArgs& args)
    let CommandEncoderClearBuffer(self : nativeint, buffer : nativeint, offset : uint64, size : uint64) =
        let mutable args = {
            CommandEncoderClearBufferArgs.Self = self;
            CommandEncoderClearBufferArgs.Buffer = buffer;
            CommandEncoderClearBufferArgs.Offset = offset;
            CommandEncoderClearBufferArgs.Size = size;
        }
        _CommandEncoderClearBuffer(&args)
    [<DllImport("Native", EntryPoint="gpuCommandEncoderInsertDebugMarker")>]
    extern void CommandEncoderInsertDebugMarker(nativeint self, StringView markerLabel)
    [<DllImport("Native", EntryPoint="gpuCommandEncoderPopDebugGroup")>]
    extern void CommandEncoderPopDebugGroup(nativeint self)
    [<DllImport("Native", EntryPoint="gpuCommandEncoderPushDebugGroup")>]
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

    [<DllImport("Native", EntryPoint="gpuCommandEncoderResolveQuerySet")>]
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
    [<DllImport("Native", EntryPoint="gpuCommandEncoderWriteTimestamp")>]
    extern void CommandEncoderWriteTimestamp(nativeint self, nativeint querySet, uint32 queryIndex)
    [<DllImport("Native", EntryPoint="gpuCommandEncoderSetLabel")>]
    extern void CommandEncoderSetLabel(nativeint self, StringView label)
    [<DllImport("Native", EntryPoint="gpuComputePassEncoderInsertDebugMarker")>]
    extern void ComputePassEncoderInsertDebugMarker(nativeint self, StringView markerLabel)
    [<DllImport("Native", EntryPoint="gpuComputePassEncoderPopDebugGroup")>]
    extern void ComputePassEncoderPopDebugGroup(nativeint self)
    [<DllImport("Native", EntryPoint="gpuComputePassEncoderPushDebugGroup")>]
    extern void ComputePassEncoderPushDebugGroup(nativeint self, StringView groupLabel)
    [<DllImport("Native", EntryPoint="gpuComputePassEncoderSetPipeline")>]
    extern void ComputePassEncoderSetPipeline(nativeint self, nativeint pipeline)
    [<DllImport("Native", EntryPoint="gpuComputePassEncoderSetBindGroup")>]
    extern void ComputePassEncoderSetBindGroup(nativeint self, uint32 groupIndex, nativeint group, unativeint dynamicOffsetCount, uint32* dynamicOffsets)
    [<DllImport("Native", EntryPoint="gpuComputePassEncoderWriteTimestamp")>]
    extern void ComputePassEncoderWriteTimestamp(nativeint self, nativeint querySet, uint32 queryIndex)
    [<DllImport("Native", EntryPoint="gpuComputePassEncoderDispatchWorkgroups")>]
    extern void ComputePassEncoderDispatchWorkgroups(nativeint self, uint32 workgroupCountX, uint32 workgroupCountY, uint32 workgroupCountZ)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type ComputePassEncoderDispatchWorkgroupsIndirectArgs = 
        {
            Self : nativeint
            IndirectBuffer : nativeint
            IndirectOffset : uint64
        }

    [<DllImport("Native", EntryPoint="gpuComputePassEncoderDispatchWorkgroupsIndirect")>]
    extern void _ComputePassEncoderDispatchWorkgroupsIndirect(ComputePassEncoderDispatchWorkgroupsIndirectArgs& args)
    let ComputePassEncoderDispatchWorkgroupsIndirect(self : nativeint, indirectBuffer : nativeint, indirectOffset : uint64) =
        let mutable args = {
            ComputePassEncoderDispatchWorkgroupsIndirectArgs.Self = self;
            ComputePassEncoderDispatchWorkgroupsIndirectArgs.IndirectBuffer = indirectBuffer;
            ComputePassEncoderDispatchWorkgroupsIndirectArgs.IndirectOffset = indirectOffset;
        }
        _ComputePassEncoderDispatchWorkgroupsIndirect(&args)
    [<DllImport("Native", EntryPoint="gpuComputePassEncoderEnd")>]
    extern void ComputePassEncoderEnd(nativeint self)
    [<DllImport("Native", EntryPoint="gpuComputePassEncoderSetLabel")>]
    extern void ComputePassEncoderSetLabel(nativeint self, StringView label)
    [<DllImport("Native", EntryPoint="gpuComputePipelineGetBindGroupLayout")>]
    extern nativeint ComputePipelineGetBindGroupLayout(nativeint self, uint32 groupIndex)
    [<DllImport("Native", EntryPoint="gpuComputePipelineSetLabel")>]
    extern void ComputePipelineSetLabel(nativeint self, StringView label)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateBindGroup")>]
    extern nativeint DeviceCreateBindGroup(nativeint self, BindGroupDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateBindGroupLayout")>]
    extern nativeint DeviceCreateBindGroupLayout(nativeint self, BindGroupLayoutDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateBuffer")>]
    extern nativeint DeviceCreateBuffer(nativeint self, BufferDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateCommandEncoder")>]
    extern nativeint DeviceCreateCommandEncoder(nativeint self, CommandEncoderDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateComputePipeline")>]
    extern nativeint DeviceCreateComputePipeline(nativeint self, ComputePipelineDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateComputePipelineAsync")>]
    extern void DeviceCreateComputePipelineAsync(nativeint self, ComputePipelineDescriptor* descriptor, nativeint callback, nativeint userdata)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateComputePipelineAsyncF")>]
    extern Future DeviceCreateComputePipelineAsyncF(nativeint self, ComputePipelineDescriptor* descriptor, CreateComputePipelineAsyncCallbackInfo callbackInfo)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateComputePipelineAsync2")>]
    extern Future DeviceCreateComputePipelineAsync2(nativeint self, ComputePipelineDescriptor* descriptor, CreateComputePipelineAsyncCallbackInfo2 callbackInfo)
    [<DllImport("Native", EntryPoint="gpuDeviceCreatePipelineLayout")>]
    extern nativeint DeviceCreatePipelineLayout(nativeint self, PipelineLayoutDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateQuerySet")>]
    extern nativeint DeviceCreateQuerySet(nativeint self, QuerySetDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateRenderPipelineAsync")>]
    extern void DeviceCreateRenderPipelineAsync(nativeint self, RenderPipelineDescriptor* descriptor, nativeint callback, nativeint userdata)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateRenderPipelineAsyncF")>]
    extern Future DeviceCreateRenderPipelineAsyncF(nativeint self, RenderPipelineDescriptor* descriptor, CreateRenderPipelineAsyncCallbackInfo callbackInfo)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateRenderPipelineAsync2")>]
    extern Future DeviceCreateRenderPipelineAsync2(nativeint self, RenderPipelineDescriptor* descriptor, CreateRenderPipelineAsyncCallbackInfo2 callbackInfo)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateRenderBundleEncoder")>]
    extern nativeint DeviceCreateRenderBundleEncoder(nativeint self, RenderBundleEncoderDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateRenderPipeline")>]
    extern nativeint DeviceCreateRenderPipeline(nativeint self, RenderPipelineDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateSampler")>]
    extern nativeint DeviceCreateSampler(nativeint self, SamplerDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateShaderModule")>]
    extern nativeint DeviceCreateShaderModule(nativeint self, ShaderModuleDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateTexture")>]
    extern nativeint DeviceCreateTexture(nativeint self, TextureDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceDestroy")>]
    extern void DeviceDestroy(nativeint self)
    [<DllImport("Native", EntryPoint="gpuDeviceGetLimits")>]
    extern Status DeviceGetLimits(nativeint self, SupportedLimits* limits)
    [<DllImport("Native", EntryPoint="gpuDeviceGetLostFuture")>]
    extern Future DeviceGetLostFuture(nativeint self)
    [<DllImport("Native", EntryPoint="gpuDeviceHasFeature")>]
    extern int DeviceHasFeature(nativeint self, FeatureName feature)
    [<DllImport("Native", EntryPoint="gpuDeviceGetFeatures")>]
    extern void DeviceGetFeatures(nativeint self, SupportedFeatures* features)
    [<DllImport("Native", EntryPoint="gpuDeviceGetAdapterInfo")>]
    extern Status DeviceGetAdapterInfo(nativeint self, AdapterInfo* adapterInfo)
    [<DllImport("Native", EntryPoint="gpuDeviceGetQueue")>]
    extern nativeint DeviceGetQueue(nativeint self)
    [<DllImport("Native", EntryPoint="gpuDeviceSetUncapturedErrorCallback")>]
    extern void DeviceSetUncapturedErrorCallback(nativeint self, nativeint callback, nativeint userdata)
    [<DllImport("Native", EntryPoint="gpuDevicePushErrorScope")>]
    extern void DevicePushErrorScope(nativeint self, ErrorFilter filter)
    [<DllImport("Native", EntryPoint="gpuDevicePopErrorScope")>]
    extern void DevicePopErrorScope(nativeint self, nativeint oldCallback, nativeint userdata)
    [<DllImport("Native", EntryPoint="gpuDevicePopErrorScopeF")>]
    extern Future DevicePopErrorScopeF(nativeint self, PopErrorScopeCallbackInfo callbackInfo)
    [<DllImport("Native", EntryPoint="gpuDevicePopErrorScope2")>]
    extern Future DevicePopErrorScope2(nativeint self, PopErrorScopeCallbackInfo2 callbackInfo)
    [<DllImport("Native", EntryPoint="gpuDeviceSetLabel")>]
    extern void DeviceSetLabel(nativeint self, StringView label)
    [<DllImport("Native", EntryPoint="gpuInstanceCreateSurface")>]
    extern nativeint InstanceCreateSurface(nativeint self, SurfaceDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuInstanceProcessEvents")>]
    extern void InstanceProcessEvents(nativeint self)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type InstanceWaitAnyArgs = 
        {
            Self : nativeint
            FutureCount : unativeint
            Futures : nativeptr<FutureWaitInfo>
            TimeoutNS : uint64
        }

    [<DllImport("Native", EntryPoint="gpuInstanceWaitAny")>]
    extern WaitStatus _InstanceWaitAny(InstanceWaitAnyArgs& args)
    let InstanceWaitAny(self : nativeint, futureCount : unativeint, futures : nativeptr<FutureWaitInfo>, timeoutNS : uint64) =
        let mutable args = {
            InstanceWaitAnyArgs.Self = self;
            InstanceWaitAnyArgs.FutureCount = futureCount;
            InstanceWaitAnyArgs.Futures = futures;
            InstanceWaitAnyArgs.TimeoutNS = timeoutNS;
        }
        _InstanceWaitAny(&args)
    [<DllImport("Native", EntryPoint="gpuInstanceRequestAdapter")>]
    extern void InstanceRequestAdapter(nativeint self, RequestAdapterOptions* options, nativeint callback, nativeint userdata)
    [<DllImport("Native", EntryPoint="gpuInstanceRequestAdapterF")>]
    extern Future InstanceRequestAdapterF(nativeint self, RequestAdapterOptions* options, RequestAdapterCallbackInfo callbackInfo)
    [<DllImport("Native", EntryPoint="gpuInstanceRequestAdapter2")>]
    extern Future InstanceRequestAdapter2(nativeint self, RequestAdapterOptions* options, RequestAdapterCallbackInfo2 callbackInfo)
    [<DllImport("Native", EntryPoint="gpuInstanceHasWGSLLanguageFeature")>]
    extern int InstanceHasWGSLLanguageFeature(nativeint self, WGSLFeatureName feature)
    [<DllImport("Native", EntryPoint="gpuInstanceEnumerateWGSLLanguageFeatures")>]
    extern unativeint InstanceEnumerateWGSLLanguageFeatures(nativeint self, WGSLFeatureName* features)
    [<DllImport("Native", EntryPoint="gpuGetInstanceFeatures")>]
    extern Status GetInstanceFeatures(InstanceFeatures* features)
    [<DllImport("Native", EntryPoint="gpuPipelineLayoutSetLabel")>]
    extern void PipelineLayoutSetLabel(nativeint self, StringView label)
    [<DllImport("Native", EntryPoint="gpuQuerySetSetLabel")>]
    extern void QuerySetSetLabel(nativeint self, StringView label)
    [<DllImport("Native", EntryPoint="gpuQuerySetGetType")>]
    extern QueryType QuerySetGetType(nativeint self)
    [<DllImport("Native", EntryPoint="gpuQuerySetGetCount")>]
    extern uint32 QuerySetGetCount(nativeint self)
    [<DllImport("Native", EntryPoint="gpuQuerySetDestroy")>]
    extern void QuerySetDestroy(nativeint self)
    [<DllImport("Native", EntryPoint="gpuQueueSubmit")>]
    extern void QueueSubmit(nativeint self, unativeint commandCount, nativeint* commands)
    [<DllImport("Native", EntryPoint="gpuQueueOnSubmittedWorkDoneF")>]
    extern Future QueueOnSubmittedWorkDoneF(nativeint self, QueueWorkDoneCallbackInfo callbackInfo)
    [<DllImport("Native", EntryPoint="gpuQueueOnSubmittedWorkDone2")>]
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

    [<DllImport("Native", EntryPoint="gpuQueueWriteBuffer")>]
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

    [<DllImport("Native", EntryPoint="gpuQueueWriteTexture")>]
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
    [<DllImport("Native", EntryPoint="gpuQueueSetLabel")>]
    extern void QueueSetLabel(nativeint self, StringView label)
    [<DllImport("Native", EntryPoint="gpuRenderBundleSetLabel")>]
    extern void RenderBundleSetLabel(nativeint self, StringView label)
    [<DllImport("Native", EntryPoint="gpuRenderBundleEncoderSetPipeline")>]
    extern void RenderBundleEncoderSetPipeline(nativeint self, nativeint pipeline)
    [<DllImport("Native", EntryPoint="gpuRenderBundleEncoderSetBindGroup")>]
    extern void RenderBundleEncoderSetBindGroup(nativeint self, uint32 groupIndex, nativeint group, unativeint dynamicOffsetCount, uint32* dynamicOffsets)
    [<DllImport("Native", EntryPoint="gpuRenderBundleEncoderDraw")>]
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

    [<DllImport("Native", EntryPoint="gpuRenderBundleEncoderDrawIndexed")>]
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

    [<DllImport("Native", EntryPoint="gpuRenderBundleEncoderDrawIndirect")>]
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

    [<DllImport("Native", EntryPoint="gpuRenderBundleEncoderDrawIndexedIndirect")>]
    extern void _RenderBundleEncoderDrawIndexedIndirect(RenderBundleEncoderDrawIndexedIndirectArgs& args)
    let RenderBundleEncoderDrawIndexedIndirect(self : nativeint, indirectBuffer : nativeint, indirectOffset : uint64) =
        let mutable args = {
            RenderBundleEncoderDrawIndexedIndirectArgs.Self = self;
            RenderBundleEncoderDrawIndexedIndirectArgs.IndirectBuffer = indirectBuffer;
            RenderBundleEncoderDrawIndexedIndirectArgs.IndirectOffset = indirectOffset;
        }
        _RenderBundleEncoderDrawIndexedIndirect(&args)
    [<DllImport("Native", EntryPoint="gpuRenderBundleEncoderInsertDebugMarker")>]
    extern void RenderBundleEncoderInsertDebugMarker(nativeint self, StringView markerLabel)
    [<DllImport("Native", EntryPoint="gpuRenderBundleEncoderPopDebugGroup")>]
    extern void RenderBundleEncoderPopDebugGroup(nativeint self)
    [<DllImport("Native", EntryPoint="gpuRenderBundleEncoderPushDebugGroup")>]
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

    [<DllImport("Native", EntryPoint="gpuRenderBundleEncoderSetVertexBuffer")>]
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

    [<DllImport("Native", EntryPoint="gpuRenderBundleEncoderSetIndexBuffer")>]
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
    [<DllImport("Native", EntryPoint="gpuRenderBundleEncoderFinish")>]
    extern nativeint RenderBundleEncoderFinish(nativeint self, RenderBundleDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuRenderBundleEncoderSetLabel")>]
    extern void RenderBundleEncoderSetLabel(nativeint self, StringView label)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderSetPipeline")>]
    extern void RenderPassEncoderSetPipeline(nativeint self, nativeint pipeline)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderSetBindGroup")>]
    extern void RenderPassEncoderSetBindGroup(nativeint self, uint32 groupIndex, nativeint group, unativeint dynamicOffsetCount, uint32* dynamicOffsets)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderDraw")>]
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

    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderDrawIndexed")>]
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

    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderDrawIndirect")>]
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

    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderDrawIndexedIndirect")>]
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

    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderMultiDrawIndirect")>]
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

    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderMultiDrawIndexedIndirect")>]
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
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderExecuteBundles")>]
    extern void RenderPassEncoderExecuteBundles(nativeint self, unativeint bundleCount, nativeint* bundles)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderInsertDebugMarker")>]
    extern void RenderPassEncoderInsertDebugMarker(nativeint self, StringView markerLabel)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderPopDebugGroup")>]
    extern void RenderPassEncoderPopDebugGroup(nativeint self)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderPushDebugGroup")>]
    extern void RenderPassEncoderPushDebugGroup(nativeint self, StringView groupLabel)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderSetStencilReference")>]
    extern void RenderPassEncoderSetStencilReference(nativeint self, uint32 reference)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderSetBlendConstant")>]
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

    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderSetViewport")>]
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
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderSetScissorRect")>]
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

    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderSetVertexBuffer")>]
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

    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderSetIndexBuffer")>]
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
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderBeginOcclusionQuery")>]
    extern void RenderPassEncoderBeginOcclusionQuery(nativeint self, uint32 queryIndex)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderEndOcclusionQuery")>]
    extern void RenderPassEncoderEndOcclusionQuery(nativeint self)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderWriteTimestamp")>]
    extern void RenderPassEncoderWriteTimestamp(nativeint self, nativeint querySet, uint32 queryIndex)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderEnd")>]
    extern void RenderPassEncoderEnd(nativeint self)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderSetLabel")>]
    extern void RenderPassEncoderSetLabel(nativeint self, StringView label)
    [<DllImport("Native", EntryPoint="gpuRenderPipelineGetBindGroupLayout")>]
    extern nativeint RenderPipelineGetBindGroupLayout(nativeint self, uint32 groupIndex)
    [<DllImport("Native", EntryPoint="gpuRenderPipelineSetLabel")>]
    extern void RenderPipelineSetLabel(nativeint self, StringView label)
    [<DllImport("Native", EntryPoint="gpuSamplerSetLabel")>]
    extern void SamplerSetLabel(nativeint self, StringView label)
    [<DllImport("Native", EntryPoint="gpuShaderModuleGetCompilationInfo")>]
    extern void ShaderModuleGetCompilationInfo(nativeint self, nativeint callback, nativeint userdata)
    [<DllImport("Native", EntryPoint="gpuShaderModuleGetCompilationInfoF")>]
    extern Future ShaderModuleGetCompilationInfoF(nativeint self, CompilationInfoCallbackInfo callbackInfo)
    [<DllImport("Native", EntryPoint="gpuShaderModuleGetCompilationInfo2")>]
    extern Future ShaderModuleGetCompilationInfo2(nativeint self, CompilationInfoCallbackInfo2 callbackInfo)
    [<DllImport("Native", EntryPoint="gpuShaderModuleSetLabel")>]
    extern void ShaderModuleSetLabel(nativeint self, StringView label)
    [<DllImport("Native", EntryPoint="gpuSurfaceConfigure")>]
    extern void SurfaceConfigure(nativeint self, SurfaceConfiguration* config)
    [<DllImport("Native", EntryPoint="gpuSurfaceGetCapabilities")>]
    extern Status SurfaceGetCapabilities(nativeint self, nativeint adapter, SurfaceCapabilities* capabilities)
    [<DllImport("Native", EntryPoint="gpuSurfaceGetCurrentTexture")>]
    extern void SurfaceGetCurrentTexture(nativeint self, SurfaceTexture* surfaceTexture)
    [<DllImport("Native", EntryPoint="gpuSurfacePresent")>]
    extern void SurfacePresent(nativeint self)
    [<DllImport("Native", EntryPoint="gpuSurfaceUnconfigure")>]
    extern void SurfaceUnconfigure(nativeint self)
    [<DllImport("Native", EntryPoint="gpuSurfaceSetLabel")>]
    extern void SurfaceSetLabel(nativeint self, StringView label)
    [<DllImport("Native", EntryPoint="gpuTextureCreateView")>]
    extern nativeint TextureCreateView(nativeint self, TextureViewDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuTextureSetLabel")>]
    extern void TextureSetLabel(nativeint self, StringView label)
    [<DllImport("Native", EntryPoint="gpuTextureGetWidth")>]
    extern uint32 TextureGetWidth(nativeint self)
    [<DllImport("Native", EntryPoint="gpuTextureGetHeight")>]
    extern uint32 TextureGetHeight(nativeint self)
    [<DllImport("Native", EntryPoint="gpuTextureGetDepthOrArrayLayers")>]
    extern uint32 TextureGetDepthOrArrayLayers(nativeint self)
    [<DllImport("Native", EntryPoint="gpuTextureGetMipLevelCount")>]
    extern uint32 TextureGetMipLevelCount(nativeint self)
    [<DllImport("Native", EntryPoint="gpuTextureGetSampleCount")>]
    extern uint32 TextureGetSampleCount(nativeint self)
    [<DllImport("Native", EntryPoint="gpuTextureGetDimension")>]
    extern TextureDimension TextureGetDimension(nativeint self)
    [<DllImport("Native", EntryPoint="gpuTextureGetFormat")>]
    extern TextureFormat TextureGetFormat(nativeint self)
    [<DllImport("Native", EntryPoint="gpuTextureGetUsage")>]
    extern TextureUsage TextureGetUsage(nativeint self)
    [<DllImport("Native", EntryPoint="gpuTextureDestroy")>]
    extern void TextureDestroy(nativeint self)
    [<DllImport("Native", EntryPoint="gpuTextureViewSetLabel")>]
    extern void TextureViewSetLabel(nativeint self, StringView label)
type WebGPUCallbacks() =
    static let requestAdapterCallbackCallbacks = Dictionary<nativeint, RequestAdapterCallback>()
    static let mutable requestAdapterCallbackCurrent = 0n
    static let requestAdapterCallbackDelegate = System.Delegate.CreateDelegate(typeof<RequestAdapterCallback>, typeof<WebGPUCallbacks>.GetMethod "RequestAdapterCallback")
    static let requestAdapterCallbackPtr = Marshal.GetFunctionPointerForDelegate(requestAdapterCallbackDelegate)
    static let dawnLoadCacheDataFunctionCallbacks = Dictionary<nativeint, DawnLoadCacheDataFunction>()
    static let mutable dawnLoadCacheDataFunctionCurrent = 0n
    static let dawnLoadCacheDataFunctionDelegate = System.Delegate.CreateDelegate(typeof<DawnLoadCacheDataFunction>, typeof<WebGPUCallbacks>.GetMethod "DawnLoadCacheDataFunction")
    static let dawnLoadCacheDataFunctionPtr = Marshal.GetFunctionPointerForDelegate(dawnLoadCacheDataFunctionDelegate)
    static let dawnStoreCacheDataFunctionCallbacks = Dictionary<nativeint, DawnStoreCacheDataFunction>()
    static let mutable dawnStoreCacheDataFunctionCurrent = 0n
    static let dawnStoreCacheDataFunctionDelegate = System.Delegate.CreateDelegate(typeof<DawnStoreCacheDataFunction>, typeof<WebGPUCallbacks>.GetMethod "DawnStoreCacheDataFunction")
    static let dawnStoreCacheDataFunctionPtr = Marshal.GetFunctionPointerForDelegate(dawnStoreCacheDataFunctionDelegate)
    static let callbackCallbacks = Dictionary<nativeint, Callback>()
    static let mutable callbackCurrent = 0n
    static let callbackDelegate = System.Delegate.CreateDelegate(typeof<Callback>, typeof<WebGPUCallbacks>.GetMethod "Callback")
    static let callbackPtr = Marshal.GetFunctionPointerForDelegate(callbackDelegate)
    static let bufferMapCallbackCallbacks = Dictionary<nativeint, BufferMapCallback>()
    static let mutable bufferMapCallbackCurrent = 0n
    static let bufferMapCallbackDelegate = System.Delegate.CreateDelegate(typeof<BufferMapCallback>, typeof<WebGPUCallbacks>.GetMethod "BufferMapCallback")
    static let bufferMapCallbackPtr = Marshal.GetFunctionPointerForDelegate(bufferMapCallbackDelegate)
    static let compilationInfoCallbackCallbacks = Dictionary<nativeint, CompilationInfoCallback>()
    static let mutable compilationInfoCallbackCurrent = 0n
    static let compilationInfoCallbackDelegate = System.Delegate.CreateDelegate(typeof<CompilationInfoCallback>, typeof<WebGPUCallbacks>.GetMethod "CompilationInfoCallback")
    static let compilationInfoCallbackPtr = Marshal.GetFunctionPointerForDelegate(compilationInfoCallbackDelegate)
    static let createComputePipelineAsyncCallbackCallbacks = Dictionary<nativeint, CreateComputePipelineAsyncCallback>()
    static let mutable createComputePipelineAsyncCallbackCurrent = 0n
    static let createComputePipelineAsyncCallbackDelegate = System.Delegate.CreateDelegate(typeof<CreateComputePipelineAsyncCallback>, typeof<WebGPUCallbacks>.GetMethod "CreateComputePipelineAsyncCallback")
    static let createComputePipelineAsyncCallbackPtr = Marshal.GetFunctionPointerForDelegate(createComputePipelineAsyncCallbackDelegate)
    static let createRenderPipelineAsyncCallbackCallbacks = Dictionary<nativeint, CreateRenderPipelineAsyncCallback>()
    static let mutable createRenderPipelineAsyncCallbackCurrent = 0n
    static let createRenderPipelineAsyncCallbackDelegate = System.Delegate.CreateDelegate(typeof<CreateRenderPipelineAsyncCallback>, typeof<WebGPUCallbacks>.GetMethod "CreateRenderPipelineAsyncCallback")
    static let createRenderPipelineAsyncCallbackPtr = Marshal.GetFunctionPointerForDelegate(createRenderPipelineAsyncCallbackDelegate)
    static let deviceLostCallbackCallbacks = Dictionary<nativeint, DeviceLostCallback>()
    static let mutable deviceLostCallbackCurrent = 0n
    static let deviceLostCallbackDelegate = System.Delegate.CreateDelegate(typeof<DeviceLostCallback>, typeof<WebGPUCallbacks>.GetMethod "DeviceLostCallback")
    static let deviceLostCallbackPtr = Marshal.GetFunctionPointerForDelegate(deviceLostCallbackDelegate)
    static let deviceLostCallbackNewCallbacks = Dictionary<nativeint, DeviceLostCallbackNew>()
    static let mutable deviceLostCallbackNewCurrent = 0n
    static let deviceLostCallbackNewDelegate = System.Delegate.CreateDelegate(typeof<DeviceLostCallbackNew>, typeof<WebGPUCallbacks>.GetMethod "DeviceLostCallbackNew")
    static let deviceLostCallbackNewPtr = Marshal.GetFunctionPointerForDelegate(deviceLostCallbackNewDelegate)
    static let errorCallbackCallbacks = Dictionary<nativeint, ErrorCallback>()
    static let mutable errorCallbackCurrent = 0n
    static let errorCallbackDelegate = System.Delegate.CreateDelegate(typeof<ErrorCallback>, typeof<WebGPUCallbacks>.GetMethod "ErrorCallback")
    static let errorCallbackPtr = Marshal.GetFunctionPointerForDelegate(errorCallbackDelegate)
    static let popErrorScopeCallbackCallbacks = Dictionary<nativeint, PopErrorScopeCallback>()
    static let mutable popErrorScopeCallbackCurrent = 0n
    static let popErrorScopeCallbackDelegate = System.Delegate.CreateDelegate(typeof<PopErrorScopeCallback>, typeof<WebGPUCallbacks>.GetMethod "PopErrorScopeCallback")
    static let popErrorScopeCallbackPtr = Marshal.GetFunctionPointerForDelegate(popErrorScopeCallbackDelegate)
    static let loggingCallbackCallbacks = Dictionary<nativeint, LoggingCallback>()
    static let mutable loggingCallbackCurrent = 0n
    static let loggingCallbackDelegate = System.Delegate.CreateDelegate(typeof<LoggingCallback>, typeof<WebGPUCallbacks>.GetMethod "LoggingCallback")
    static let loggingCallbackPtr = Marshal.GetFunctionPointerForDelegate(loggingCallbackDelegate)
    static let queueWorkDoneCallbackCallbacks = Dictionary<nativeint, QueueWorkDoneCallback>()
    static let mutable queueWorkDoneCallbackCurrent = 0n
    static let queueWorkDoneCallbackDelegate = System.Delegate.CreateDelegate(typeof<QueueWorkDoneCallback>, typeof<WebGPUCallbacks>.GetMethod "QueueWorkDoneCallback")
    static let queueWorkDoneCallbackPtr = Marshal.GetFunctionPointerForDelegate(queueWorkDoneCallbackDelegate)
    static let requestDeviceCallbackCallbacks = Dictionary<nativeint, RequestDeviceCallback>()
    static let mutable requestDeviceCallbackCurrent = 0n
    static let requestDeviceCallbackDelegate = System.Delegate.CreateDelegate(typeof<RequestDeviceCallback>, typeof<WebGPUCallbacks>.GetMethod "RequestDeviceCallback")
    static let requestDeviceCallbackPtr = Marshal.GetFunctionPointerForDelegate(requestDeviceCallbackDelegate)
    static member RequestAdapterCallback(status : RequestAdapterStatus, adapter : nativeint, message : StringView, userdata : nativeint) =
        let callback = 
            lock requestAdapterCallbackCallbacks (fun () ->
                match requestAdapterCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    requestAdapterCallbackCallbacks.Remove(userdata) |> ignore
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
            struct(requestAdapterCallbackPtr, id)
        )
    static member DawnLoadCacheDataFunction(key : nativeint, keySize : unativeint, value : nativeint, valueSize : unativeint, userdata : nativeint) =
        let callback = 
            lock dawnLoadCacheDataFunctionCallbacks (fun () ->
                match dawnLoadCacheDataFunctionCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    dawnLoadCacheDataFunctionCallbacks.Remove(userdata) |> ignore
                    Some cb
                | _ ->
                    None
            )
        match callback with
        | Some cb -> cb.Invoke(key, keySize, 0n, valueSize, 0n)
        | None -> Unchecked.defaultof<_>

    static member Register(cb : DawnLoadCacheDataFunction) =
        lock dawnLoadCacheDataFunctionCallbacks (fun () ->
            let id = dawnLoadCacheDataFunctionCurrent
            dawnLoadCacheDataFunctionCurrent <- dawnLoadCacheDataFunctionCurrent + 1n
            dawnLoadCacheDataFunctionCallbacks.[id] <- cb
            struct(dawnLoadCacheDataFunctionPtr, id)
        )
    static member DawnStoreCacheDataFunction(key : nativeint, keySize : unativeint, value : nativeint, valueSize : unativeint, userdata : nativeint) =
        let callback = 
            lock dawnStoreCacheDataFunctionCallbacks (fun () ->
                match dawnStoreCacheDataFunctionCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    dawnStoreCacheDataFunctionCallbacks.Remove(userdata) |> ignore
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
            struct(dawnStoreCacheDataFunctionPtr, id)
        )
    static member Callback(userdata : nativeint) =
        let callback = 
            lock callbackCallbacks (fun () ->
                match callbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    callbackCallbacks.Remove(userdata) |> ignore
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
            struct(callbackPtr, id)
        )
    static member BufferMapCallback(status : BufferMapAsyncStatus, userdata : nativeint) =
        let callback = 
            lock bufferMapCallbackCallbacks (fun () ->
                match bufferMapCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    bufferMapCallbackCallbacks.Remove(userdata) |> ignore
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
            struct(bufferMapCallbackPtr, id)
        )
    static member CompilationInfoCallback(status : CompilationInfoRequestStatus, compilationInfo : nativeptr<CompilationInfo>, userdata : nativeint) =
        let callback = 
            lock compilationInfoCallbackCallbacks (fun () ->
                match compilationInfoCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    compilationInfoCallbackCallbacks.Remove(userdata) |> ignore
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
            struct(compilationInfoCallbackPtr, id)
        )
    static member CreateComputePipelineAsyncCallback(status : CreatePipelineAsyncStatus, pipeline : nativeint, message : StringView, userdata : nativeint) =
        let callback = 
            lock createComputePipelineAsyncCallbackCallbacks (fun () ->
                match createComputePipelineAsyncCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    createComputePipelineAsyncCallbackCallbacks.Remove(userdata) |> ignore
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
            struct(createComputePipelineAsyncCallbackPtr, id)
        )
    static member CreateRenderPipelineAsyncCallback(status : CreatePipelineAsyncStatus, pipeline : nativeint, message : StringView, userdata : nativeint) =
        let callback = 
            lock createRenderPipelineAsyncCallbackCallbacks (fun () ->
                match createRenderPipelineAsyncCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    createRenderPipelineAsyncCallbackCallbacks.Remove(userdata) |> ignore
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
            struct(createRenderPipelineAsyncCallbackPtr, id)
        )
    static member DeviceLostCallback(reason : DeviceLostReason, message : StringView, userdata : nativeint) =
        let callback = 
            lock deviceLostCallbackCallbacks (fun () ->
                match deviceLostCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    deviceLostCallbackCallbacks.Remove(userdata) |> ignore
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
            struct(deviceLostCallbackPtr, id)
        )
    static member DeviceLostCallbackNew(device : nativeptr<nativeint>, reason : DeviceLostReason, message : StringView, userdata : nativeint) =
        let callback = 
            lock deviceLostCallbackNewCallbacks (fun () ->
                match deviceLostCallbackNewCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    deviceLostCallbackNewCallbacks.Remove(userdata) |> ignore
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
            struct(deviceLostCallbackNewPtr, id)
        )
    static member ErrorCallback(typ : ErrorType, message : StringView, userdata : nativeint) =
        let callback = 
            lock errorCallbackCallbacks (fun () ->
                match errorCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    errorCallbackCallbacks.Remove(userdata) |> ignore
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
            struct(errorCallbackPtr, id)
        )
    static member PopErrorScopeCallback(status : PopErrorScopeStatus, typ : ErrorType, message : StringView, userdata : nativeint) =
        let callback = 
            lock popErrorScopeCallbackCallbacks (fun () ->
                match popErrorScopeCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    popErrorScopeCallbackCallbacks.Remove(userdata) |> ignore
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
            struct(popErrorScopeCallbackPtr, id)
        )
    static member LoggingCallback(typ : LoggingType, message : StringView, userdata : nativeint) =
        let callback = 
            lock loggingCallbackCallbacks (fun () ->
                match loggingCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    loggingCallbackCallbacks.Remove(userdata) |> ignore
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
            struct(loggingCallbackPtr, id)
        )
    static member QueueWorkDoneCallback(status : QueueWorkDoneStatus, userdata : nativeint) =
        let callback = 
            lock queueWorkDoneCallbackCallbacks (fun () ->
                match queueWorkDoneCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    queueWorkDoneCallbackCallbacks.Remove(userdata) |> ignore
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
            struct(queueWorkDoneCallbackPtr, id)
        )
    static member RequestDeviceCallback(status : RequestDeviceStatus, device : nativeint, message : StringView, userdata : nativeint) =
        let callback = 
            lock requestDeviceCallbackCallbacks (fun () ->
                match requestDeviceCallbackCallbacks.TryGetValue(userdata) with
                | (true, cb) ->
                    requestDeviceCallbackCallbacks.Remove(userdata) |> ignore
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
            struct(requestDeviceCallbackPtr, id)
        )
