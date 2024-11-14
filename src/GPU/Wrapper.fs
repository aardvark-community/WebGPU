namespace rec WebGPU.Raw
open System.Collections.Generic
open System
open System.Runtime.InteropServices
open WebGPU
#nowarn "9"
type Proc = delegate of unit -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RequestAdapterOptions = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public CompatibleSurface : nativeint
        val mutable public PowerPreference : PowerPreference
        val mutable public ForceFallbackAdapter : int
        new(nextInChain : nativeint, compatibleSurface : nativeint, powerPreference : PowerPreference, forceFallbackAdapter : int) = { NextInChain = nextInChain; CompatibleSurface = compatibleSurface; PowerPreference = powerPreference; ForceFallbackAdapter = forceFallbackAdapter }
        new(compatibleSurface : nativeint, powerPreference : PowerPreference, forceFallbackAdapter : int) = RequestAdapterOptions(0n, compatibleSurface, powerPreference, forceFallbackAdapter)
    end
type RequestAdapterCallback = delegate of status : RequestAdapterStatus * adapter : nativeint * message : nativeptr<byte> * userdata : nativeint -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type AdapterProperties = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public VendorID : uint32
        val mutable public VendorName : nativeptr<byte>
        val mutable public Architecture : nativeptr<byte>
        val mutable public DeviceID : uint32
        val mutable public Name : nativeptr<byte>
        val mutable public DriverDescription : nativeptr<byte>
        val mutable public AdapterType : AdapterType
        val mutable public BackendType : BackendType
        new(nextInChain : nativeint, vendorID : uint32, vendorName : nativeptr<byte>, architecture : nativeptr<byte>, deviceID : uint32, name : nativeptr<byte>, driverDescription : nativeptr<byte>, adapterType : AdapterType, backendType : BackendType) = { NextInChain = nextInChain; VendorID = vendorID; VendorName = vendorName; Architecture = architecture; DeviceID = deviceID; Name = name; DriverDescription = driverDescription; AdapterType = adapterType; BackendType = backendType }
        new(vendorID : uint32, vendorName : nativeptr<byte>, architecture : nativeptr<byte>, deviceID : uint32, name : nativeptr<byte>, driverDescription : nativeptr<byte>, adapterType : AdapterType, backendType : BackendType) = AdapterProperties(0n, vendorID, vendorName, architecture, deviceID, name, driverDescription, adapterType, backendType)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DeviceDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : nativeptr<byte>
        val mutable public RequiredFeaturesCount : uint32
        val mutable public RequiredFeatures : nativeptr<FeatureName>
        val mutable public RequiredLimits : nativeptr<RequiredLimits>
        val mutable public DefaultQueue : QueueDescriptor
        new(nextInChain : nativeint, label : nativeptr<byte>, requiredFeaturesCount : uint32, requiredFeatures : nativeptr<FeatureName>, requiredLimits : nativeptr<RequiredLimits>, defaultQueue : QueueDescriptor) = { NextInChain = nextInChain; Label = label; RequiredFeaturesCount = requiredFeaturesCount; RequiredFeatures = requiredFeatures; RequiredLimits = requiredLimits; DefaultQueue = defaultQueue }
        new(label : nativeptr<byte>, requiredFeaturesCount : uint32, requiredFeatures : nativeptr<FeatureName>, requiredLimits : nativeptr<RequiredLimits>, defaultQueue : QueueDescriptor) = DeviceDescriptor(0n, label, requiredFeaturesCount, requiredFeatures, requiredLimits, defaultQueue)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnTogglesDeviceDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public ForceEnabledTogglesCount : uint32
        val mutable public ForceEnabledToggles : nativeptr<nativeptr<byte>>
        val mutable public ForceDisabledTogglesCount : uint32
        val mutable public ForceDisabledToggles : nativeptr<nativeptr<byte>>
        new(nextInChain : nativeint, sType : SType, forceEnabledTogglesCount : uint32, forceEnabledToggles : nativeptr<nativeptr<byte>>, forceDisabledTogglesCount : uint32, forceDisabledToggles : nativeptr<nativeptr<byte>>) = { NextInChain = nextInChain; SType = sType; ForceEnabledTogglesCount = forceEnabledTogglesCount; ForceEnabledToggles = forceEnabledToggles; ForceDisabledTogglesCount = forceDisabledTogglesCount; ForceDisabledToggles = forceDisabledToggles }
        new(forceEnabledTogglesCount : uint32, forceEnabledToggles : nativeptr<nativeptr<byte>>, forceDisabledTogglesCount : uint32, forceDisabledToggles : nativeptr<nativeptr<byte>>) = DawnTogglesDeviceDescriptor(0n, SType.Invalid, forceEnabledTogglesCount, forceEnabledToggles, forceDisabledTogglesCount, forceDisabledToggles)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnTogglesDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public EnabledTogglesCount : uint32
        val mutable public EnabledToggles : nativeptr<nativeptr<byte>>
        val mutable public DisabledTogglesCount : uint32
        val mutable public DisabledToggles : nativeptr<nativeptr<byte>>
        new(nextInChain : nativeint, sType : SType, enabledTogglesCount : uint32, enabledToggles : nativeptr<nativeptr<byte>>, disabledTogglesCount : uint32, disabledToggles : nativeptr<nativeptr<byte>>) = { NextInChain = nextInChain; SType = sType; EnabledTogglesCount = enabledTogglesCount; EnabledToggles = enabledToggles; DisabledTogglesCount = disabledTogglesCount; DisabledToggles = disabledToggles }
        new(enabledTogglesCount : uint32, enabledToggles : nativeptr<nativeptr<byte>>, disabledTogglesCount : uint32, disabledToggles : nativeptr<nativeptr<byte>>) = DawnTogglesDescriptor(0n, SType.Invalid, enabledTogglesCount, enabledToggles, disabledTogglesCount, disabledToggles)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnCacheDeviceDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public IsolationKey : nativeptr<byte>
        new(nextInChain : nativeint, sType : SType, isolationKey : nativeptr<byte>) = { NextInChain = nextInChain; SType = sType; IsolationKey = isolationKey }
        new(isolationKey : nativeptr<byte>) = DawnCacheDeviceDescriptor(0n, SType.Invalid, isolationKey)
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
        val mutable public Label : nativeptr<byte>
        val mutable public Layout : nativeint
        val mutable public EntryCount : uint32
        val mutable public Entries : nativeptr<BindGroupEntry>
        new(nextInChain : nativeint, label : nativeptr<byte>, layout : nativeint, entryCount : uint32, entries : nativeptr<BindGroupEntry>) = { NextInChain = nextInChain; Label = label; Layout = layout; EntryCount = entryCount; Entries = entries }
        new(label : nativeptr<byte>, layout : nativeint, entryCount : uint32, entries : nativeptr<BindGroupEntry>) = BindGroupDescriptor(0n, label, layout, entryCount, entries)
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
type ExternalTextureBindingEntry = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public ExternalTexture : nativeint
        new(nextInChain : nativeint, sType : SType, externalTexture : nativeint) = { NextInChain = nextInChain; SType = sType; ExternalTexture = externalTexture }
        new(externalTexture : nativeint) = ExternalTextureBindingEntry(0n, SType.Invalid, externalTexture)
    end
[<StructLayout(LayoutKind.Explicit, Size = 4)>]
type ExternalTextureBindingLayout = struct end
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
        val mutable public Label : nativeptr<byte>
        val mutable public EntryCount : uint32
        val mutable public Entries : nativeptr<BindGroupLayoutEntry>
        new(nextInChain : nativeint, label : nativeptr<byte>, entryCount : uint32, entries : nativeptr<BindGroupLayoutEntry>) = { NextInChain = nextInChain; Label = label; EntryCount = entryCount; Entries = entries }
        new(label : nativeptr<byte>, entryCount : uint32, entries : nativeptr<BindGroupLayoutEntry>) = BindGroupLayoutDescriptor(0n, label, entryCount, entries)
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
type BufferDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : nativeptr<byte>
        val mutable public Usage : BufferUsage
        val mutable public Size : uint64
        val mutable public MappedAtCreation : int
        new(nextInChain : nativeint, label : nativeptr<byte>, usage : BufferUsage, size : uint64, mappedAtCreation : int) = { NextInChain = nextInChain; Label = label; Usage = usage; Size = size; MappedAtCreation = mappedAtCreation }
        new(label : nativeptr<byte>, usage : BufferUsage, size : uint64, mappedAtCreation : int) = BufferDescriptor(0n, label, usage, size, mappedAtCreation)
    end
type BufferMapCallback = delegate of status : BufferMapAsyncStatus * userdata : nativeint -> unit
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
        val mutable public Key : nativeptr<byte>
        val mutable public Value : double
        new(nextInChain : nativeint, key : nativeptr<byte>, value : double) = { NextInChain = nextInChain; Key = key; Value = value }
        new(key : nativeptr<byte>, value : double) = ConstantEntry(0n, key, value)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type CommandBufferDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : nativeptr<byte>
        new(nextInChain : nativeint, label : nativeptr<byte>) = { NextInChain = nextInChain; Label = label }
        new(label : nativeptr<byte>) = CommandBufferDescriptor(0n, label)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type CommandEncoderDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : nativeptr<byte>
        new(nextInChain : nativeint, label : nativeptr<byte>) = { NextInChain = nextInChain; Label = label }
        new(label : nativeptr<byte>) = CommandEncoderDescriptor(0n, label)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type CompilationInfo = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public MessageCount : uint32
        val mutable public Messages : nativeptr<CompilationMessage>
        new(nextInChain : nativeint, messageCount : uint32, messages : nativeptr<CompilationMessage>) = { NextInChain = nextInChain; MessageCount = messageCount; Messages = messages }
        new(messageCount : uint32, messages : nativeptr<CompilationMessage>) = CompilationInfo(0n, messageCount, messages)
    end
type CompilationInfoCallback = delegate of status : CompilationInfoRequestStatus * compilationInfo : nativeptr<CompilationInfo> * userdata : nativeint -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type CompilationMessage = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Message : nativeptr<byte>
        val mutable public Type : CompilationMessageType
        val mutable public LineNum : uint64
        val mutable public LinePos : uint64
        val mutable public Offset : uint64
        val mutable public Length : uint64
        val mutable public Utf16LinePos : uint64
        val mutable public Utf16Offset : uint64
        val mutable public Utf16Length : uint64
        new(nextInChain : nativeint, message : nativeptr<byte>, typ : CompilationMessageType, lineNum : uint64, linePos : uint64, offset : uint64, length : uint64, utf16LinePos : uint64, utf16Offset : uint64, utf16Length : uint64) = { NextInChain = nextInChain; Message = message; Type = typ; LineNum = lineNum; LinePos = linePos; Offset = offset; Length = length; Utf16LinePos = utf16LinePos; Utf16Offset = utf16Offset; Utf16Length = utf16Length }
        new(message : nativeptr<byte>, typ : CompilationMessageType, lineNum : uint64, linePos : uint64, offset : uint64, length : uint64, utf16LinePos : uint64, utf16Offset : uint64, utf16Length : uint64) = CompilationMessage(0n, message, typ, lineNum, linePos, offset, length, utf16LinePos, utf16Offset, utf16Length)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ComputePassDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : nativeptr<byte>
        val mutable public TimestampWriteCount : uint32
        val mutable public TimestampWrites : nativeptr<ComputePassTimestampWrite>
        new(nextInChain : nativeint, label : nativeptr<byte>, timestampWriteCount : uint32, timestampWrites : nativeptr<ComputePassTimestampWrite>) = { NextInChain = nextInChain; Label = label; TimestampWriteCount = timestampWriteCount; TimestampWrites = timestampWrites }
        new(label : nativeptr<byte>, timestampWriteCount : uint32, timestampWrites : nativeptr<ComputePassTimestampWrite>) = ComputePassDescriptor(0n, label, timestampWriteCount, timestampWrites)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ComputePassTimestampWrite = 
    struct
        val mutable public QuerySet : nativeint
        val mutable public QueryIndex : uint32
        val mutable public Location : ComputePassTimestampLocation
        new(querySet : nativeint, queryIndex : uint32, location : ComputePassTimestampLocation) = { QuerySet = querySet; QueryIndex = queryIndex; Location = location }
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ComputePipelineDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : nativeptr<byte>
        val mutable public Layout : nativeint
        val mutable public Compute : ProgrammableStageDescriptor
        new(nextInChain : nativeint, label : nativeptr<byte>, layout : nativeint, compute : ProgrammableStageDescriptor) = { NextInChain = nextInChain; Label = label; Layout = layout; Compute = compute }
        new(label : nativeptr<byte>, layout : nativeint, compute : ProgrammableStageDescriptor) = ComputePipelineDescriptor(0n, label, layout, compute)
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
type CreateComputePipelineAsyncCallback = delegate of status : CreatePipelineAsyncStatus * pipeline : nativeint * message : nativeptr<byte> * userdata : nativeint -> unit
type CreateRenderPipelineAsyncCallback = delegate of status : CreatePipelineAsyncStatus * pipeline : nativeint * message : nativeptr<byte> * userdata : nativeint -> unit
type DeviceLostCallback = delegate of reason : DeviceLostReason * message : nativeptr<byte> * userdata : nativeint -> unit
type ErrorCallback = delegate of typ : ErrorType * message : nativeptr<byte> * userdata : nativeint -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type Limits = 
    struct
        val mutable public MaxTextureDimension1D : uint32
        val mutable public MaxTextureDimension2D : uint32
        val mutable public MaxTextureDimension3D : uint32
        val mutable public MaxTextureArrayLayers : uint32
        val mutable public MaxBindGroups : uint32
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
        val mutable public MaxFragmentCombinedOutputResources : uint32
        new(maxTextureDimension1D : uint32, maxTextureDimension2D : uint32, maxTextureDimension3D : uint32, maxTextureArrayLayers : uint32, maxBindGroups : uint32, maxBindingsPerBindGroup : uint32, maxDynamicUniformBuffersPerPipelineLayout : uint32, maxDynamicStorageBuffersPerPipelineLayout : uint32, maxSampledTexturesPerShaderStage : uint32, maxSamplersPerShaderStage : uint32, maxStorageBuffersPerShaderStage : uint32, maxStorageTexturesPerShaderStage : uint32, maxUniformBuffersPerShaderStage : uint32, maxUniformBufferBindingSize : uint64, maxStorageBufferBindingSize : uint64, minUniformBufferOffsetAlignment : uint32, minStorageBufferOffsetAlignment : uint32, maxVertexBuffers : uint32, maxBufferSize : uint64, maxVertexAttributes : uint32, maxVertexBufferArrayStride : uint32, maxInterStageShaderComponents : uint32, maxInterStageShaderVariables : uint32, maxColorAttachments : uint32, maxColorAttachmentBytesPerSample : uint32, maxComputeWorkgroupStorageSize : uint32, maxComputeInvocationsPerWorkgroup : uint32, maxComputeWorkgroupSizeX : uint32, maxComputeWorkgroupSizeY : uint32, maxComputeWorkgroupSizeZ : uint32, maxComputeWorkgroupsPerDimension : uint32, maxFragmentCombinedOutputResources : uint32) = { MaxTextureDimension1D = maxTextureDimension1D; MaxTextureDimension2D = maxTextureDimension2D; MaxTextureDimension3D = maxTextureDimension3D; MaxTextureArrayLayers = maxTextureArrayLayers; MaxBindGroups = maxBindGroups; MaxBindingsPerBindGroup = maxBindingsPerBindGroup; MaxDynamicUniformBuffersPerPipelineLayout = maxDynamicUniformBuffersPerPipelineLayout; MaxDynamicStorageBuffersPerPipelineLayout = maxDynamicStorageBuffersPerPipelineLayout; MaxSampledTexturesPerShaderStage = maxSampledTexturesPerShaderStage; MaxSamplersPerShaderStage = maxSamplersPerShaderStage; MaxStorageBuffersPerShaderStage = maxStorageBuffersPerShaderStage; MaxStorageTexturesPerShaderStage = maxStorageTexturesPerShaderStage; MaxUniformBuffersPerShaderStage = maxUniformBuffersPerShaderStage; MaxUniformBufferBindingSize = maxUniformBufferBindingSize; MaxStorageBufferBindingSize = maxStorageBufferBindingSize; MinUniformBufferOffsetAlignment = minUniformBufferOffsetAlignment; MinStorageBufferOffsetAlignment = minStorageBufferOffsetAlignment; MaxVertexBuffers = maxVertexBuffers; MaxBufferSize = maxBufferSize; MaxVertexAttributes = maxVertexAttributes; MaxVertexBufferArrayStride = maxVertexBufferArrayStride; MaxInterStageShaderComponents = maxInterStageShaderComponents; MaxInterStageShaderVariables = maxInterStageShaderVariables; MaxColorAttachments = maxColorAttachments; MaxColorAttachmentBytesPerSample = maxColorAttachmentBytesPerSample; MaxComputeWorkgroupStorageSize = maxComputeWorkgroupStorageSize; MaxComputeInvocationsPerWorkgroup = maxComputeInvocationsPerWorkgroup; MaxComputeWorkgroupSizeX = maxComputeWorkgroupSizeX; MaxComputeWorkgroupSizeY = maxComputeWorkgroupSizeY; MaxComputeWorkgroupSizeZ = maxComputeWorkgroupSizeZ; MaxComputeWorkgroupsPerDimension = maxComputeWorkgroupsPerDimension; MaxFragmentCombinedOutputResources = maxFragmentCombinedOutputResources }
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
type LoggingCallback = delegate of typ : LoggingType * message : nativeptr<byte> * userdata : nativeint -> unit
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
        val mutable public Label : nativeptr<byte>
        val mutable public Plane0 : nativeint
        val mutable public Plane1 : nativeint
        val mutable public VisibleOrigin : Origin2D
        val mutable public VisibleSize : Extent2D
        val mutable public DoYuvToRgbConversionOnly : int
        val mutable public YuvToRgbConversionMatrix : nativeptr<float32>
        val mutable public SrcTransferFunctionParameters : nativeptr<float32>
        val mutable public DstTransferFunctionParameters : nativeptr<float32>
        val mutable public GamutConversionMatrix : nativeptr<float32>
        val mutable public FlipY : int
        val mutable public Rotation : ExternalTextureRotation
        new(nextInChain : nativeint, label : nativeptr<byte>, plane0 : nativeint, plane1 : nativeint, visibleOrigin : Origin2D, visibleSize : Extent2D, doYuvToRgbConversionOnly : int, yuvToRgbConversionMatrix : nativeptr<float32>, srcTransferFunctionParameters : nativeptr<float32>, dstTransferFunctionParameters : nativeptr<float32>, gamutConversionMatrix : nativeptr<float32>, flipY : int, rotation : ExternalTextureRotation) = { NextInChain = nextInChain; Label = label; Plane0 = plane0; Plane1 = plane1; VisibleOrigin = visibleOrigin; VisibleSize = visibleSize; DoYuvToRgbConversionOnly = doYuvToRgbConversionOnly; YuvToRgbConversionMatrix = yuvToRgbConversionMatrix; SrcTransferFunctionParameters = srcTransferFunctionParameters; DstTransferFunctionParameters = dstTransferFunctionParameters; GamutConversionMatrix = gamutConversionMatrix; FlipY = flipY; Rotation = rotation }
        new(label : nativeptr<byte>, plane0 : nativeint, plane1 : nativeint, visibleOrigin : Origin2D, visibleSize : Extent2D, doYuvToRgbConversionOnly : int, yuvToRgbConversionMatrix : nativeptr<float32>, srcTransferFunctionParameters : nativeptr<float32>, dstTransferFunctionParameters : nativeptr<float32>, gamutConversionMatrix : nativeptr<float32>, flipY : int, rotation : ExternalTextureRotation) = ExternalTextureDescriptor(0n, label, plane0, plane1, visibleOrigin, visibleSize, doYuvToRgbConversionOnly, yuvToRgbConversionMatrix, srcTransferFunctionParameters, dstTransferFunctionParameters, gamutConversionMatrix, flipY, rotation)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ImageCopyBuffer = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Layout : TextureDataLayout
        val mutable public Buffer : nativeint
        new(nextInChain : nativeint, layout : TextureDataLayout, buffer : nativeint) = { NextInChain = nextInChain; Layout = layout; Buffer = buffer }
        new(layout : TextureDataLayout, buffer : nativeint) = ImageCopyBuffer(0n, layout, buffer)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ImageCopyTexture = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Texture : nativeint
        val mutable public MipLevel : uint32
        val mutable public Origin : Origin3D
        val mutable public Aspect : TextureAspect
        new(nextInChain : nativeint, texture : nativeint, mipLevel : uint32, origin : Origin3D, aspect : TextureAspect) = { NextInChain = nextInChain; Texture = texture; MipLevel = mipLevel; Origin = origin; Aspect = aspect }
        new(texture : nativeint, mipLevel : uint32, origin : Origin3D, aspect : TextureAspect) = ImageCopyTexture(0n, texture, mipLevel, origin, aspect)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ImageCopyExternalTexture = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public ExternalTexture : nativeint
        val mutable public Origin : Origin3D
        new(nextInChain : nativeint, externalTexture : nativeint, origin : Origin3D) = { NextInChain = nextInChain; ExternalTexture = externalTexture; Origin = origin }
        new(externalTexture : nativeint, origin : Origin3D) = ImageCopyExternalTexture(0n, externalTexture, origin)
    end
[<StructLayout(LayoutKind.Explicit, Size = 4)>]
type InstanceDescriptor = struct end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnInstanceDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public AdditionalRuntimeSearchPathsCount : uint32
        val mutable public AdditionalRuntimeSearchPaths : nativeptr<nativeptr<byte>>
        new(nextInChain : nativeint, sType : SType, additionalRuntimeSearchPathsCount : uint32, additionalRuntimeSearchPaths : nativeptr<nativeptr<byte>>) = { NextInChain = nextInChain; SType = sType; AdditionalRuntimeSearchPathsCount = additionalRuntimeSearchPathsCount; AdditionalRuntimeSearchPaths = additionalRuntimeSearchPaths }
        new(additionalRuntimeSearchPathsCount : uint32, additionalRuntimeSearchPaths : nativeptr<nativeptr<byte>>) = DawnInstanceDescriptor(0n, SType.Invalid, additionalRuntimeSearchPathsCount, additionalRuntimeSearchPaths)
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
        val mutable public AttributeCount : uint32
        val mutable public Attributes : nativeptr<VertexAttribute>
        new(arrayStride : uint64, stepMode : VertexStepMode, attributeCount : uint32, attributes : nativeptr<VertexAttribute>) = { ArrayStride = arrayStride; StepMode = stepMode; AttributeCount = attributeCount; Attributes = attributes }
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
        val mutable public Label : nativeptr<byte>
        val mutable public BindGroupLayoutCount : uint32
        val mutable public BindGroupLayouts : nativeptr<nativeint>
        new(nextInChain : nativeint, label : nativeptr<byte>, bindGroupLayoutCount : uint32, bindGroupLayouts : nativeptr<nativeint>) = { NextInChain = nextInChain; Label = label; BindGroupLayoutCount = bindGroupLayoutCount; BindGroupLayouts = bindGroupLayouts }
        new(label : nativeptr<byte>, bindGroupLayoutCount : uint32, bindGroupLayouts : nativeptr<nativeint>) = PipelineLayoutDescriptor(0n, label, bindGroupLayoutCount, bindGroupLayouts)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ProgrammableStageDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Module : nativeint
        val mutable public EntryPoint : nativeptr<byte>
        val mutable public ConstantCount : uint32
        val mutable public Constants : nativeptr<ConstantEntry>
        new(nextInChain : nativeint, moodule : nativeint, entryPoint : nativeptr<byte>, constantCount : uint32, constants : nativeptr<ConstantEntry>) = { NextInChain = nextInChain; Module = moodule; EntryPoint = entryPoint; ConstantCount = constantCount; Constants = constants }
        new(moodule : nativeint, entryPoint : nativeptr<byte>, constantCount : uint32, constants : nativeptr<ConstantEntry>) = ProgrammableStageDescriptor(0n, moodule, entryPoint, constantCount, constants)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type QuerySetDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : nativeptr<byte>
        val mutable public Type : QueryType
        val mutable public Count : uint32
        val mutable public PipelineStatistics : nativeptr<PipelineStatisticName>
        val mutable public PipelineStatisticsCount : uint32
        new(nextInChain : nativeint, label : nativeptr<byte>, typ : QueryType, count : uint32, pipelineStatistics : nativeptr<PipelineStatisticName>, pipelineStatisticsCount : uint32) = { NextInChain = nextInChain; Label = label; Type = typ; Count = count; PipelineStatistics = pipelineStatistics; PipelineStatisticsCount = pipelineStatisticsCount }
        new(label : nativeptr<byte>, typ : QueryType, count : uint32, pipelineStatistics : nativeptr<PipelineStatisticName>, pipelineStatisticsCount : uint32) = QuerySetDescriptor(0n, label, typ, count, pipelineStatistics, pipelineStatisticsCount)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type QueueDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : nativeptr<byte>
        new(nextInChain : nativeint, label : nativeptr<byte>) = { NextInChain = nextInChain; Label = label }
        new(label : nativeptr<byte>) = QueueDescriptor(0n, label)
    end
type QueueWorkDoneCallback = delegate of status : QueueWorkDoneStatus * userdata : nativeint -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RenderBundleDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : nativeptr<byte>
        new(nextInChain : nativeint, label : nativeptr<byte>) = { NextInChain = nextInChain; Label = label }
        new(label : nativeptr<byte>) = RenderBundleDescriptor(0n, label)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RenderBundleEncoderDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : nativeptr<byte>
        val mutable public ColorFormatsCount : uint32
        val mutable public ColorFormats : nativeptr<TextureFormat>
        val mutable public DepthStencilFormat : TextureFormat
        val mutable public SampleCount : uint32
        val mutable public DepthReadOnly : int
        val mutable public StencilReadOnly : int
        new(nextInChain : nativeint, label : nativeptr<byte>, colorFormatsCount : uint32, colorFormats : nativeptr<TextureFormat>, depthStencilFormat : TextureFormat, sampleCount : uint32, depthReadOnly : int, stencilReadOnly : int) = { NextInChain = nextInChain; Label = label; ColorFormatsCount = colorFormatsCount; ColorFormats = colorFormats; DepthStencilFormat = depthStencilFormat; SampleCount = sampleCount; DepthReadOnly = depthReadOnly; StencilReadOnly = stencilReadOnly }
        new(label : nativeptr<byte>, colorFormatsCount : uint32, colorFormats : nativeptr<TextureFormat>, depthStencilFormat : TextureFormat, sampleCount : uint32, depthReadOnly : int, stencilReadOnly : int) = RenderBundleEncoderDescriptor(0n, label, colorFormatsCount, colorFormats, depthStencilFormat, sampleCount, depthReadOnly, stencilReadOnly)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RenderPassColorAttachment = 
    struct
        val mutable public View : nativeint
        val mutable public ResolveTarget : nativeint
        val mutable public LoadOp : LoadOp
        val mutable public StoreOp : StoreOp
        val mutable public ClearValue : Color
        new(view : nativeint, resolveTarget : nativeint, loadOp : LoadOp, storeOp : StoreOp, clearValue : Color) = { View = view; ResolveTarget = resolveTarget; LoadOp = loadOp; StoreOp = storeOp; ClearValue = clearValue }
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
        val mutable public Label : nativeptr<byte>
        val mutable public ColorAttachmentCount : uint32
        val mutable public ColorAttachments : nativeptr<RenderPassColorAttachment>
        val mutable public DepthStencilAttachment : nativeptr<RenderPassDepthStencilAttachment>
        val mutable public OcclusionQuerySet : nativeint
        val mutable public TimestampWriteCount : uint32
        val mutable public TimestampWrites : nativeptr<RenderPassTimestampWrite>
        new(nextInChain : nativeint, label : nativeptr<byte>, colorAttachmentCount : uint32, colorAttachments : nativeptr<RenderPassColorAttachment>, depthStencilAttachment : nativeptr<RenderPassDepthStencilAttachment>, occlusionQuerySet : nativeint, timestampWriteCount : uint32, timestampWrites : nativeptr<RenderPassTimestampWrite>) = { NextInChain = nextInChain; Label = label; ColorAttachmentCount = colorAttachmentCount; ColorAttachments = colorAttachments; DepthStencilAttachment = depthStencilAttachment; OcclusionQuerySet = occlusionQuerySet; TimestampWriteCount = timestampWriteCount; TimestampWrites = timestampWrites }
        new(label : nativeptr<byte>, colorAttachmentCount : uint32, colorAttachments : nativeptr<RenderPassColorAttachment>, depthStencilAttachment : nativeptr<RenderPassDepthStencilAttachment>, occlusionQuerySet : nativeint, timestampWriteCount : uint32, timestampWrites : nativeptr<RenderPassTimestampWrite>) = RenderPassDescriptor(0n, label, colorAttachmentCount, colorAttachments, depthStencilAttachment, occlusionQuerySet, timestampWriteCount, timestampWrites)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RenderPassDescriptorMaxDrawCount = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public MaxDrawCount : uint64
        new(nextInChain : nativeint, sType : SType, maxDrawCount : uint64) = { NextInChain = nextInChain; SType = sType; MaxDrawCount = maxDrawCount }
        new(maxDrawCount : uint64) = RenderPassDescriptorMaxDrawCount(0n, SType.Invalid, maxDrawCount)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type RenderPassTimestampWrite = 
    struct
        val mutable public QuerySet : nativeint
        val mutable public QueryIndex : uint32
        val mutable public Location : RenderPassTimestampLocation
        new(querySet : nativeint, queryIndex : uint32, location : RenderPassTimestampLocation) = { QuerySet = querySet; QueryIndex = queryIndex; Location = location }
    end
type RequestDeviceCallback = delegate of status : RequestDeviceStatus * device : nativeint * message : nativeptr<byte> * userdata : nativeint -> unit
[<Struct; StructLayout(LayoutKind.Sequential)>]
type VertexState = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Module : nativeint
        val mutable public EntryPoint : nativeptr<byte>
        val mutable public ConstantCount : uint32
        val mutable public Constants : nativeptr<ConstantEntry>
        val mutable public BufferCount : uint32
        val mutable public Buffers : nativeptr<VertexBufferLayout>
        new(nextInChain : nativeint, moodule : nativeint, entryPoint : nativeptr<byte>, constantCount : uint32, constants : nativeptr<ConstantEntry>, bufferCount : uint32, buffers : nativeptr<VertexBufferLayout>) = { NextInChain = nextInChain; Module = moodule; EntryPoint = entryPoint; ConstantCount = constantCount; Constants = constants; BufferCount = bufferCount; Buffers = buffers }
        new(moodule : nativeint, entryPoint : nativeptr<byte>, constantCount : uint32, constants : nativeptr<ConstantEntry>, bufferCount : uint32, buffers : nativeptr<VertexBufferLayout>) = VertexState(0n, moodule, entryPoint, constantCount, constants, bufferCount, buffers)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type PrimitiveState = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Topology : PrimitiveTopology
        val mutable public StripIndexFormat : IndexFormat
        val mutable public FrontFace : FrontFace
        val mutable public CullMode : CullMode
        new(nextInChain : nativeint, topology : PrimitiveTopology, stripIndexFormat : IndexFormat, frontFace : FrontFace, cullMode : CullMode) = { NextInChain = nextInChain; Topology = topology; StripIndexFormat = stripIndexFormat; FrontFace = frontFace; CullMode = cullMode }
        new(topology : PrimitiveTopology, stripIndexFormat : IndexFormat, frontFace : FrontFace, cullMode : CullMode) = PrimitiveState(0n, topology, stripIndexFormat, frontFace, cullMode)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type PrimitiveDepthClipControl = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public UnclippedDepth : int
        new(nextInChain : nativeint, sType : SType, unclippedDepth : int) = { NextInChain = nextInChain; SType = sType; UnclippedDepth = unclippedDepth }
        new(unclippedDepth : int) = PrimitiveDepthClipControl(0n, SType.Invalid, unclippedDepth)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DepthStencilState = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Format : TextureFormat
        val mutable public DepthWriteEnabled : int
        val mutable public DepthCompare : CompareFunction
        val mutable public StencilFront : StencilFaceState
        val mutable public StencilBack : StencilFaceState
        val mutable public StencilReadMask : uint32
        val mutable public StencilWriteMask : uint32
        val mutable public DepthBias : int
        val mutable public DepthBiasSlopeScale : float32
        val mutable public DepthBiasClamp : float32
        new(nextInChain : nativeint, format : TextureFormat, depthWriteEnabled : int, depthCompare : CompareFunction, stencilFront : StencilFaceState, stencilBack : StencilFaceState, stencilReadMask : uint32, stencilWriteMask : uint32, depthBias : int, depthBiasSlopeScale : float32, depthBiasClamp : float32) = { NextInChain = nextInChain; Format = format; DepthWriteEnabled = depthWriteEnabled; DepthCompare = depthCompare; StencilFront = stencilFront; StencilBack = stencilBack; StencilReadMask = stencilReadMask; StencilWriteMask = stencilWriteMask; DepthBias = depthBias; DepthBiasSlopeScale = depthBiasSlopeScale; DepthBiasClamp = depthBiasClamp }
        new(format : TextureFormat, depthWriteEnabled : int, depthCompare : CompareFunction, stencilFront : StencilFaceState, stencilBack : StencilFaceState, stencilReadMask : uint32, stencilWriteMask : uint32, depthBias : int, depthBiasSlopeScale : float32, depthBiasClamp : float32) = DepthStencilState(0n, format, depthWriteEnabled, depthCompare, stencilFront, stencilBack, stencilReadMask, stencilWriteMask, depthBias, depthBiasSlopeScale, depthBiasClamp)
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
        val mutable public EntryPoint : nativeptr<byte>
        val mutable public ConstantCount : uint32
        val mutable public Constants : nativeptr<ConstantEntry>
        val mutable public TargetCount : uint32
        val mutable public Targets : nativeptr<ColorTargetState>
        new(nextInChain : nativeint, moodule : nativeint, entryPoint : nativeptr<byte>, constantCount : uint32, constants : nativeptr<ConstantEntry>, targetCount : uint32, targets : nativeptr<ColorTargetState>) = { NextInChain = nextInChain; Module = moodule; EntryPoint = entryPoint; ConstantCount = constantCount; Constants = constants; TargetCount = targetCount; Targets = targets }
        new(moodule : nativeint, entryPoint : nativeptr<byte>, constantCount : uint32, constants : nativeptr<ConstantEntry>, targetCount : uint32, targets : nativeptr<ColorTargetState>) = FragmentState(0n, moodule, entryPoint, constantCount, constants, targetCount, targets)
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
        val mutable public Label : nativeptr<byte>
        val mutable public Layout : nativeint
        val mutable public Vertex : VertexState
        val mutable public Primitive : PrimitiveState
        val mutable public DepthStencil : nativeptr<DepthStencilState>
        val mutable public Multisample : MultisampleState
        val mutable public Fragment : nativeptr<FragmentState>
        new(nextInChain : nativeint, label : nativeptr<byte>, layout : nativeint, vertex : VertexState, primitive : PrimitiveState, depthStencil : nativeptr<DepthStencilState>, multisample : MultisampleState, fragment : nativeptr<FragmentState>) = { NextInChain = nextInChain; Label = label; Layout = layout; Vertex = vertex; Primitive = primitive; DepthStencil = depthStencil; Multisample = multisample; Fragment = fragment }
        new(label : nativeptr<byte>, layout : nativeint, vertex : VertexState, primitive : PrimitiveState, depthStencil : nativeptr<DepthStencilState>, multisample : MultisampleState, fragment : nativeptr<FragmentState>) = RenderPipelineDescriptor(0n, label, layout, vertex, primitive, depthStencil, multisample, fragment)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SamplerDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : nativeptr<byte>
        val mutable public AddressModeU : AddressMode
        val mutable public AddressModeV : AddressMode
        val mutable public AddressModeW : AddressMode
        val mutable public MagFilter : FilterMode
        val mutable public MinFilter : FilterMode
        val mutable public MipmapFilter : FilterMode
        val mutable public LodMinClamp : float32
        val mutable public LodMaxClamp : float32
        val mutable public Compare : CompareFunction
        val mutable public MaxAnisotropy : uint16
        new(nextInChain : nativeint, label : nativeptr<byte>, addressModeU : AddressMode, addressModeV : AddressMode, addressModeW : AddressMode, magFilter : FilterMode, minFilter : FilterMode, mipmapFilter : FilterMode, lodMinClamp : float32, lodMaxClamp : float32, compare : CompareFunction, maxAnisotropy : uint16) = { NextInChain = nextInChain; Label = label; AddressModeU = addressModeU; AddressModeV = addressModeV; AddressModeW = addressModeW; MagFilter = magFilter; MinFilter = minFilter; MipmapFilter = mipmapFilter; LodMinClamp = lodMinClamp; LodMaxClamp = lodMaxClamp; Compare = compare; MaxAnisotropy = maxAnisotropy }
        new(label : nativeptr<byte>, addressModeU : AddressMode, addressModeV : AddressMode, addressModeW : AddressMode, magFilter : FilterMode, minFilter : FilterMode, mipmapFilter : FilterMode, lodMinClamp : float32, lodMaxClamp : float32, compare : CompareFunction, maxAnisotropy : uint16) = SamplerDescriptor(0n, label, addressModeU, addressModeV, addressModeW, magFilter, minFilter, mipmapFilter, lodMinClamp, lodMaxClamp, compare, maxAnisotropy)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ShaderModuleDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : nativeptr<byte>
        val mutable public HintCount : uint32
        val mutable public Hints : nativeptr<ShaderModuleCompilationHint>
        new(nextInChain : nativeint, label : nativeptr<byte>, hintCount : uint32, hints : nativeptr<ShaderModuleCompilationHint>) = { NextInChain = nextInChain; Label = label; HintCount = hintCount; Hints = hints }
        new(label : nativeptr<byte>, hintCount : uint32, hints : nativeptr<ShaderModuleCompilationHint>) = ShaderModuleDescriptor(0n, label, hintCount, hints)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ShaderModuleCompilationHint = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public EntryPoint : nativeptr<byte>
        val mutable public Layout : nativeint
        new(nextInChain : nativeint, entryPoint : nativeptr<byte>, layout : nativeint) = { NextInChain = nextInChain; EntryPoint = entryPoint; Layout = layout }
        new(entryPoint : nativeptr<byte>, layout : nativeint) = ShaderModuleCompilationHint(0n, entryPoint, layout)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ShaderModuleSPIRVDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public CodeSize : uint32
        val mutable public Code : nativeptr<uint32>
        new(nextInChain : nativeint, sType : SType, codeSize : uint32, code : nativeptr<uint32>) = { NextInChain = nextInChain; SType = sType; CodeSize = codeSize; Code = code }
        new(codeSize : uint32, code : nativeptr<uint32>) = ShaderModuleSPIRVDescriptor(0n, SType.Invalid, codeSize, code)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type ShaderModuleWGSLDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Source : nativeptr<byte>
        val mutable public Code : nativeptr<byte>
        new(nextInChain : nativeint, sType : SType, source : nativeptr<byte>, code : nativeptr<byte>) = { NextInChain = nextInChain; SType = sType; Source = source; Code = code }
        new(source : nativeptr<byte>, code : nativeptr<byte>) = ShaderModuleWGSLDescriptor(0n, SType.Invalid, source, code)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnShaderModuleSPIRVOptionsDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public AllowNonUniformDerivatives : int
        new(nextInChain : nativeint, sType : SType, allowNonUniformDerivatives : int) = { NextInChain = nextInChain; SType = sType; AllowNonUniformDerivatives = allowNonUniformDerivatives }
        new(allowNonUniformDerivatives : int) = DawnShaderModuleSPIRVOptionsDescriptor(0n, SType.Invalid, allowNonUniformDerivatives)
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
        val mutable public Label : nativeptr<byte>
        new(nextInChain : nativeint, label : nativeptr<byte>) = { NextInChain = nextInChain; Label = label }
        new(label : nativeptr<byte>) = SurfaceDescriptor(0n, label)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceDescriptorFromAndroidNativeWindow = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Window : nativeint
        new(nextInChain : nativeint, sType : SType, window : nativeint) = { NextInChain = nextInChain; SType = sType; Window = window }
        new(window : nativeint) = SurfaceDescriptorFromAndroidNativeWindow(0n, SType.Invalid, window)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceDescriptorFromCanvasHTMLSelector = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Selector : nativeptr<byte>
        new(nextInChain : nativeint, sType : SType, selector : nativeptr<byte>) = { NextInChain = nextInChain; SType = sType; Selector = selector }
        new(selector : nativeptr<byte>) = SurfaceDescriptorFromCanvasHTMLSelector(0n, SType.Invalid, selector)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceDescriptorFromMetalLayer = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Layer : nativeint
        new(nextInChain : nativeint, sType : SType, layer : nativeint) = { NextInChain = nextInChain; SType = sType; Layer = layer }
        new(layer : nativeint) = SurfaceDescriptorFromMetalLayer(0n, SType.Invalid, layer)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceDescriptorFromWindowsHWND = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Hinstance : nativeint
        val mutable public Hwnd : nativeint
        new(nextInChain : nativeint, sType : SType, hinstance : nativeint, hwnd : nativeint) = { NextInChain = nextInChain; SType = sType; Hinstance = hinstance; Hwnd = hwnd }
        new(hinstance : nativeint, hwnd : nativeint) = SurfaceDescriptorFromWindowsHWND(0n, SType.Invalid, hinstance, hwnd)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceDescriptorFromXcbWindow = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Connection : nativeint
        val mutable public Window : uint32
        new(nextInChain : nativeint, sType : SType, connection : nativeint, window : uint32) = { NextInChain = nextInChain; SType = sType; Connection = connection; Window = window }
        new(connection : nativeint, window : uint32) = SurfaceDescriptorFromXcbWindow(0n, SType.Invalid, connection, window)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceDescriptorFromXlibWindow = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Display : nativeint
        val mutable public Window : uint32
        new(nextInChain : nativeint, sType : SType, display : nativeint, window : uint32) = { NextInChain = nextInChain; SType = sType; Display = display; Window = window }
        new(display : nativeint, window : uint32) = SurfaceDescriptorFromXlibWindow(0n, SType.Invalid, display, window)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceDescriptorFromWaylandSurface = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public Display : nativeint
        val mutable public Surface : nativeint
        new(nextInChain : nativeint, sType : SType, display : nativeint, surface : nativeint) = { NextInChain = nextInChain; SType = sType; Display = display; Surface = surface }
        new(display : nativeint, surface : nativeint) = SurfaceDescriptorFromWaylandSurface(0n, SType.Invalid, display, surface)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceDescriptorFromWindowsCoreWindow = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public CoreWindow : nativeint
        new(nextInChain : nativeint, sType : SType, coreWindow : nativeint) = { NextInChain = nextInChain; SType = sType; CoreWindow = coreWindow }
        new(coreWindow : nativeint) = SurfaceDescriptorFromWindowsCoreWindow(0n, SType.Invalid, coreWindow)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SurfaceDescriptorFromWindowsSwapChainPanel = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public SwapChainPanel : nativeint
        new(nextInChain : nativeint, sType : SType, swapChainPanel : nativeint) = { NextInChain = nextInChain; SType = sType; SwapChainPanel = swapChainPanel }
        new(swapChainPanel : nativeint) = SurfaceDescriptorFromWindowsSwapChainPanel(0n, SType.Invalid, swapChainPanel)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type SwapChainDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : nativeptr<byte>
        val mutable public Usage : TextureUsage
        val mutable public Format : TextureFormat
        val mutable public Width : uint32
        val mutable public Height : uint32
        val mutable public PresentMode : PresentMode
        val mutable public Implementation : uint64
        new(nextInChain : nativeint, label : nativeptr<byte>, usage : TextureUsage, format : TextureFormat, width : uint32, height : uint32, presentMode : PresentMode, implementation : uint64) = { NextInChain = nextInChain; Label = label; Usage = usage; Format = format; Width = width; Height = height; PresentMode = presentMode; Implementation = implementation }
        new(label : nativeptr<byte>, usage : TextureUsage, format : TextureFormat, width : uint32, height : uint32, presentMode : PresentMode, implementation : uint64) = SwapChainDescriptor(0n, label, usage, format, width, height, presentMode, implementation)
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
        val mutable public Label : nativeptr<byte>
        val mutable public Usage : TextureUsage
        val mutable public Dimension : TextureDimension
        val mutable public Size : Extent3D
        val mutable public Format : TextureFormat
        val mutable public MipLevelCount : uint32
        val mutable public SampleCount : uint32
        val mutable public ViewFormatCount : uint32
        val mutable public ViewFormats : nativeptr<TextureFormat>
        new(nextInChain : nativeint, label : nativeptr<byte>, usage : TextureUsage, dimension : TextureDimension, size : Extent3D, format : TextureFormat, mipLevelCount : uint32, sampleCount : uint32, viewFormatCount : uint32, viewFormats : nativeptr<TextureFormat>) = { NextInChain = nextInChain; Label = label; Usage = usage; Dimension = dimension; Size = size; Format = format; MipLevelCount = mipLevelCount; SampleCount = sampleCount; ViewFormatCount = viewFormatCount; ViewFormats = viewFormats }
        new(label : nativeptr<byte>, usage : TextureUsage, dimension : TextureDimension, size : Extent3D, format : TextureFormat, mipLevelCount : uint32, sampleCount : uint32, viewFormatCount : uint32, viewFormats : nativeptr<TextureFormat>) = TextureDescriptor(0n, label, usage, dimension, size, format, mipLevelCount, sampleCount, viewFormatCount, viewFormats)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type TextureViewDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public Label : nativeptr<byte>
        val mutable public Format : TextureFormat
        val mutable public Dimension : TextureViewDimension
        val mutable public BaseMipLevel : uint32
        val mutable public MipLevelCount : uint32
        val mutable public BaseArrayLayer : uint32
        val mutable public ArrayLayerCount : uint32
        val mutable public Aspect : TextureAspect
        new(nextInChain : nativeint, label : nativeptr<byte>, format : TextureFormat, dimension : TextureViewDimension, baseMipLevel : uint32, mipLevelCount : uint32, baseArrayLayer : uint32, arrayLayerCount : uint32, aspect : TextureAspect) = { NextInChain = nextInChain; Label = label; Format = format; Dimension = dimension; BaseMipLevel = baseMipLevel; MipLevelCount = mipLevelCount; BaseArrayLayer = baseArrayLayer; ArrayLayerCount = arrayLayerCount; Aspect = aspect }
        new(label : nativeptr<byte>, format : TextureFormat, dimension : TextureViewDimension, baseMipLevel : uint32, mipLevelCount : uint32, baseArrayLayer : uint32, arrayLayerCount : uint32, aspect : TextureAspect) = TextureViewDescriptor(0n, label, format, dimension, baseMipLevel, mipLevelCount, baseArrayLayer, arrayLayerCount, aspect)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnTextureInternalUsageDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public InternalUsage : TextureUsage
        new(nextInChain : nativeint, sType : SType, internalUsage : TextureUsage) = { NextInChain = nextInChain; SType = sType; InternalUsage = internalUsage }
        new(internalUsage : TextureUsage) = DawnTextureInternalUsageDescriptor(0n, SType.Invalid, internalUsage)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnEncoderInternalUsageDescriptor = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public UseInternalUsages : int
        new(nextInChain : nativeint, sType : SType, useInternalUsages : int) = { NextInChain = nextInChain; SType = sType; UseInternalUsages = useInternalUsages }
        new(useInternalUsages : int) = DawnEncoderInternalUsageDescriptor(0n, SType.Invalid, useInternalUsages)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnAdapterPropertiesPowerPreference = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public PowerPreference : PowerPreference
        new(nextInChain : nativeint, sType : SType, powerPreference : PowerPreference) = { NextInChain = nextInChain; SType = sType; PowerPreference = powerPreference }
        new(powerPreference : PowerPreference) = DawnAdapterPropertiesPowerPreference(0n, SType.Invalid, powerPreference)
    end
[<Struct; StructLayout(LayoutKind.Sequential)>]
type DawnBufferDescriptorErrorInfoFromWireClient = 
    struct
        val mutable public NextInChain : nativeint
        val mutable public SType : SType
        val mutable public OutOfMemory : int
        new(nextInChain : nativeint, sType : SType, outOfMemory : int) = { NextInChain = nextInChain; SType = sType; OutOfMemory = outOfMemory }
        new(outOfMemory : int) = DawnBufferDescriptorErrorInfoFromWireClient(0n, SType.Invalid, outOfMemory)
    end
module WebGPU = 

    [<DllImport("Native", EntryPoint="gpuCreateInstance")>]
    extern nativeint CreateInstance(InstanceDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuGetProcAddress")>]
    extern nativeint GetProcAddress(nativeint device, byte* procName)
    [<DllImport("Native", EntryPoint="gpuAdapterGetLimits")>]
    extern int AdapterGetLimits(nativeint self, SupportedLimits* limits)
    [<DllImport("Native", EntryPoint="gpuAdapterGetProperties")>]
    extern void AdapterGetProperties(nativeint self, AdapterProperties* properties)
    [<DllImport("Native", EntryPoint="gpuAdapterHasFeature")>]
    extern int AdapterHasFeature(nativeint self, FeatureName feature)
    [<DllImport("Native", EntryPoint="gpuAdapterEnumerateFeatures")>]
    extern unativeint AdapterEnumerateFeatures(nativeint self, FeatureName* features)
    [<DllImport("Native", EntryPoint="gpuAdapterRequestDevice")>]
    extern void AdapterRequestDevice(nativeint self, DeviceDescriptor* descriptor, nativeint callback, void* userdata)
    [<DllImport("Native", EntryPoint="gpuBindGroupSetLabel")>]
    extern void BindGroupSetLabel(nativeint self, byte* label)
    [<DllImport("Native", EntryPoint="gpuBindGroupLayoutSetLabel")>]
    extern void BindGroupLayoutSetLabel(nativeint self, byte* label)
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
    [<DllImport("Native", EntryPoint="gpuBufferGetMappedRange")>]
    extern nativeint BufferGetMappedRange(nativeint self, unativeint offset, unativeint size)
    [<DllImport("Native", EntryPoint="gpuBufferGetConstMappedRange")>]
    extern nativeint BufferGetConstMappedRange(nativeint self, unativeint offset, unativeint size)
    [<DllImport("Native", EntryPoint="gpuBufferSetLabel")>]
    extern void BufferSetLabel(nativeint self, byte* label)
    [<DllImport("Native", EntryPoint="gpuBufferGetUsage")>]
    extern BufferUsage BufferGetUsage(nativeint self)
    [<DllImport("Native", EntryPoint="gpuBufferGetSize")>]
    extern uint64 BufferGetSize(nativeint self)
    [<DllImport("Native", EntryPoint="gpuBufferUnmap")>]
    extern void BufferUnmap(nativeint self)
    [<DllImport("Native", EntryPoint="gpuBufferDestroy")>]
    extern void BufferDestroy(nativeint self)
    [<DllImport("Native", EntryPoint="gpuCommandBufferSetLabel")>]
    extern void CommandBufferSetLabel(nativeint self, byte* label)
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
    extern void CommandEncoderInsertDebugMarker(nativeint self, byte* markerLabel)
    [<DllImport("Native", EntryPoint="gpuCommandEncoderPopDebugGroup")>]
    extern void CommandEncoderPopDebugGroup(nativeint self)
    [<DllImport("Native", EntryPoint="gpuCommandEncoderPushDebugGroup")>]
    extern void CommandEncoderPushDebugGroup(nativeint self, byte* groupLabel)
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
    extern void CommandEncoderSetLabel(nativeint self, byte* label)
    [<DllImport("Native", EntryPoint="gpuComputePassEncoderInsertDebugMarker")>]
    extern void ComputePassEncoderInsertDebugMarker(nativeint self, byte* markerLabel)
    [<DllImport("Native", EntryPoint="gpuComputePassEncoderPopDebugGroup")>]
    extern void ComputePassEncoderPopDebugGroup(nativeint self)
    [<DllImport("Native", EntryPoint="gpuComputePassEncoderPushDebugGroup")>]
    extern void ComputePassEncoderPushDebugGroup(nativeint self, byte* groupLabel)
    [<DllImport("Native", EntryPoint="gpuComputePassEncoderSetPipeline")>]
    extern void ComputePassEncoderSetPipeline(nativeint self, nativeint pipeline)
    [<DllImport("Native", EntryPoint="gpuComputePassEncoderSetBindGroup")>]
    extern void ComputePassEncoderSetBindGroup(nativeint self, uint32 groupIndex, nativeint group, uint32 dynamicOffsetCount, uint32* dynamicOffsets)
    [<DllImport("Native", EntryPoint="gpuComputePassEncoderWriteTimestamp")>]
    extern void ComputePassEncoderWriteTimestamp(nativeint self, nativeint querySet, uint32 queryIndex)
    [<DllImport("Native", EntryPoint="gpuComputePassEncoderBeginPipelineStatisticsQuery")>]
    extern void ComputePassEncoderBeginPipelineStatisticsQuery(nativeint self, nativeint querySet, uint32 queryIndex)
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
    [<DllImport("Native", EntryPoint="gpuComputePassEncoderEndPipelineStatisticsQuery")>]
    extern void ComputePassEncoderEndPipelineStatisticsQuery(nativeint self)
    [<DllImport("Native", EntryPoint="gpuComputePassEncoderSetLabel")>]
    extern void ComputePassEncoderSetLabel(nativeint self, byte* label)
    [<DllImport("Native", EntryPoint="gpuComputePipelineGetBindGroupLayout")>]
    extern nativeint ComputePipelineGetBindGroupLayout(nativeint self, uint32 groupIndex)
    [<DllImport("Native", EntryPoint="gpuComputePipelineSetLabel")>]
    extern void ComputePipelineSetLabel(nativeint self, byte* label)
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
    extern void DeviceCreateComputePipelineAsync(nativeint self, ComputePipelineDescriptor* descriptor, nativeint callback, void* userdata)
    [<DllImport("Native", EntryPoint="gpuDeviceCreatePipelineLayout")>]
    extern nativeint DeviceCreatePipelineLayout(nativeint self, PipelineLayoutDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateQuerySet")>]
    extern nativeint DeviceCreateQuerySet(nativeint self, QuerySetDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateRenderPipelineAsync")>]
    extern void DeviceCreateRenderPipelineAsync(nativeint self, RenderPipelineDescriptor* descriptor, nativeint callback, void* userdata)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateRenderBundleEncoder")>]
    extern nativeint DeviceCreateRenderBundleEncoder(nativeint self, RenderBundleEncoderDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateRenderPipeline")>]
    extern nativeint DeviceCreateRenderPipeline(nativeint self, RenderPipelineDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateSampler")>]
    extern nativeint DeviceCreateSampler(nativeint self, SamplerDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateShaderModule")>]
    extern nativeint DeviceCreateShaderModule(nativeint self, ShaderModuleDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateSwapChain")>]
    extern nativeint DeviceCreateSwapChain(nativeint self, nativeint surface, SwapChainDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceCreateTexture")>]
    extern nativeint DeviceCreateTexture(nativeint self, TextureDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuDeviceDestroy")>]
    extern void DeviceDestroy(nativeint self)
    [<DllImport("Native", EntryPoint="gpuDeviceGetLimits")>]
    extern int DeviceGetLimits(nativeint self, SupportedLimits* limits)
    [<DllImport("Native", EntryPoint="gpuDeviceHasFeature")>]
    extern int DeviceHasFeature(nativeint self, FeatureName feature)
    [<DllImport("Native", EntryPoint="gpuDeviceEnumerateFeatures")>]
    extern unativeint DeviceEnumerateFeatures(nativeint self, FeatureName* features)
    [<DllImport("Native", EntryPoint="gpuDeviceGetQueue")>]
    extern nativeint DeviceGetQueue(nativeint self)
    [<DllImport("Native", EntryPoint="gpuDeviceSetUncapturedErrorCallback")>]
    extern void DeviceSetUncapturedErrorCallback(nativeint self, nativeint callback, void* userdata)
    [<DllImport("Native", EntryPoint="gpuDeviceSetDeviceLostCallback")>]
    extern void DeviceSetDeviceLostCallback(nativeint self, nativeint callback, void* userdata)
    [<DllImport("Native", EntryPoint="gpuDevicePushErrorScope")>]
    extern void DevicePushErrorScope(nativeint self, ErrorFilter filter)
    [<DllImport("Native", EntryPoint="gpuDevicePopErrorScope")>]
    extern int DevicePopErrorScope(nativeint self, nativeint callback, void* userdata)
    [<DllImport("Native", EntryPoint="gpuDeviceSetLabel")>]
    extern void DeviceSetLabel(nativeint self, byte* label)
    [<DllImport("Native", EntryPoint="gpuExternalTextureSetLabel")>]
    extern void ExternalTextureSetLabel(nativeint self, byte* label)
    [<DllImport("Native", EntryPoint="gpuExternalTextureDestroy")>]
    extern void ExternalTextureDestroy(nativeint self)
    [<DllImport("Native", EntryPoint="gpuExternalTextureExpire")>]
    extern void ExternalTextureExpire(nativeint self)
    [<DllImport("Native", EntryPoint="gpuExternalTextureRefresh")>]
    extern void ExternalTextureRefresh(nativeint self)
    [<DllImport("Native", EntryPoint="gpuInstanceCreateSurface")>]
    extern nativeint InstanceCreateSurface(nativeint self, SurfaceDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuInstanceProcessEvents")>]
    extern void InstanceProcessEvents(nativeint self)
    [<DllImport("Native", EntryPoint="gpuInstanceRequestAdapter")>]
    extern void InstanceRequestAdapter(nativeint self, RequestAdapterOptions* options, nativeint callback, void* userdata)
    [<DllImport("Native", EntryPoint="gpuPipelineLayoutSetLabel")>]
    extern void PipelineLayoutSetLabel(nativeint self, byte* label)
    [<DllImport("Native", EntryPoint="gpuQuerySetSetLabel")>]
    extern void QuerySetSetLabel(nativeint self, byte* label)
    [<DllImport("Native", EntryPoint="gpuQuerySetGetType")>]
    extern QueryType QuerySetGetType(nativeint self)
    [<DllImport("Native", EntryPoint="gpuQuerySetGetCount")>]
    extern uint32 QuerySetGetCount(nativeint self)
    [<DllImport("Native", EntryPoint="gpuQuerySetDestroy")>]
    extern void QuerySetDestroy(nativeint self)
    [<DllImport("Native", EntryPoint="gpuQueueSubmit")>]
    extern void QueueSubmit(nativeint self, uint32 commandCount, nativeint* commands)
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type QueueOnSubmittedWorkDoneArgs = 
        {
            Self : nativeint
            SignalValue : uint64
            Callback : nativeint
            Userdata : nativeint
        }

    [<DllImport("Native", EntryPoint="gpuQueueOnSubmittedWorkDone")>]
    extern void _QueueOnSubmittedWorkDone(QueueOnSubmittedWorkDoneArgs& args)
    let QueueOnSubmittedWorkDone(self : nativeint, signalValue : uint64, callback : nativeint, userdata : nativeint) =
        let mutable args = {
            QueueOnSubmittedWorkDoneArgs.Self = self;
            QueueOnSubmittedWorkDoneArgs.SignalValue = signalValue;
            QueueOnSubmittedWorkDoneArgs.Callback = callback;
            QueueOnSubmittedWorkDoneArgs.Userdata = userdata;
        }
        _QueueOnSubmittedWorkDone(&args)
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
    extern void QueueSetLabel(nativeint self, byte* label)
    [<DllImport("Native", EntryPoint="gpuRenderBundleEncoderSetPipeline")>]
    extern void RenderBundleEncoderSetPipeline(nativeint self, nativeint pipeline)
    [<DllImport("Native", EntryPoint="gpuRenderBundleEncoderSetBindGroup")>]
    extern void RenderBundleEncoderSetBindGroup(nativeint self, uint32 groupIndex, nativeint group, uint32 dynamicOffsetCount, uint32* dynamicOffsets)
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
    extern void RenderBundleEncoderInsertDebugMarker(nativeint self, byte* markerLabel)
    [<DllImport("Native", EntryPoint="gpuRenderBundleEncoderPopDebugGroup")>]
    extern void RenderBundleEncoderPopDebugGroup(nativeint self)
    [<DllImport("Native", EntryPoint="gpuRenderBundleEncoderPushDebugGroup")>]
    extern void RenderBundleEncoderPushDebugGroup(nativeint self, byte* groupLabel)
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
    extern void RenderBundleEncoderSetLabel(nativeint self, byte* label)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderSetPipeline")>]
    extern void RenderPassEncoderSetPipeline(nativeint self, nativeint pipeline)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderSetBindGroup")>]
    extern void RenderPassEncoderSetBindGroup(nativeint self, uint32 groupIndex, nativeint group, uint32 dynamicOffsetCount, uint32* dynamicOffsets)
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
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderExecuteBundles")>]
    extern void RenderPassEncoderExecuteBundles(nativeint self, uint32 bundleCount, nativeint* bundles)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderInsertDebugMarker")>]
    extern void RenderPassEncoderInsertDebugMarker(nativeint self, byte* markerLabel)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderPopDebugGroup")>]
    extern void RenderPassEncoderPopDebugGroup(nativeint self)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderPushDebugGroup")>]
    extern void RenderPassEncoderPushDebugGroup(nativeint self, byte* groupLabel)
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
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderBeginPipelineStatisticsQuery")>]
    extern void RenderPassEncoderBeginPipelineStatisticsQuery(nativeint self, nativeint querySet, uint32 queryIndex)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderEndOcclusionQuery")>]
    extern void RenderPassEncoderEndOcclusionQuery(nativeint self)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderWriteTimestamp")>]
    extern void RenderPassEncoderWriteTimestamp(nativeint self, nativeint querySet, uint32 queryIndex)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderEnd")>]
    extern void RenderPassEncoderEnd(nativeint self)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderEndPipelineStatisticsQuery")>]
    extern void RenderPassEncoderEndPipelineStatisticsQuery(nativeint self)
    [<DllImport("Native", EntryPoint="gpuRenderPassEncoderSetLabel")>]
    extern void RenderPassEncoderSetLabel(nativeint self, byte* label)
    [<DllImport("Native", EntryPoint="gpuRenderPipelineGetBindGroupLayout")>]
    extern nativeint RenderPipelineGetBindGroupLayout(nativeint self, uint32 groupIndex)
    [<DllImport("Native", EntryPoint="gpuRenderPipelineSetLabel")>]
    extern void RenderPipelineSetLabel(nativeint self, byte* label)
    [<DllImport("Native", EntryPoint="gpuSamplerSetLabel")>]
    extern void SamplerSetLabel(nativeint self, byte* label)
    [<DllImport("Native", EntryPoint="gpuShaderModuleGetCompilationInfo")>]
    extern void ShaderModuleGetCompilationInfo(nativeint self, nativeint callback, void* userdata)
    [<DllImport("Native", EntryPoint="gpuShaderModuleSetLabel")>]
    extern void ShaderModuleSetLabel(nativeint self, byte* label)
    [<DllImport("Native", EntryPoint="gpuSurfaceGetPreferredFormat")>]
    extern TextureFormat SurfaceGetPreferredFormat(nativeint self, nativeint adapter)
    [<DllImport("Native", EntryPoint="gpuSwapChainGetCurrentTextureView")>]
    extern nativeint SwapChainGetCurrentTextureView(nativeint self)
    [<DllImport("Native", EntryPoint="gpuSwapChainPresent")>]
    extern void SwapChainPresent(nativeint self)
    [<DllImport("Native", EntryPoint="gpuTextureCreateView")>]
    extern nativeint TextureCreateView(nativeint self, TextureViewDescriptor* descriptor)
    [<DllImport("Native", EntryPoint="gpuTextureSetLabel")>]
    extern void TextureSetLabel(nativeint self, byte* label)
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
    extern void TextureViewSetLabel(nativeint self, byte* label)
type WebGPUCallbacks() =
    static let requestAdapterCallbackCallbacks = Dictionary<nativeint, RequestAdapterCallback>()
    static let mutable requestAdapterCallbackCurrent = 0n
    static let requestAdapterCallbackDelegate = System.Delegate.CreateDelegate(typeof<RequestAdapterCallback>, typeof<WebGPUCallbacks>.GetMethod "RequestAdapterCallback")
    static let requestAdapterCallbackPtr = Marshal.GetFunctionPointerForDelegate(requestAdapterCallbackDelegate)
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
    static let errorCallbackCallbacks = Dictionary<nativeint, ErrorCallback>()
    static let mutable errorCallbackCurrent = 0n
    static let errorCallbackDelegate = System.Delegate.CreateDelegate(typeof<ErrorCallback>, typeof<WebGPUCallbacks>.GetMethod "ErrorCallback")
    static let errorCallbackPtr = Marshal.GetFunctionPointerForDelegate(errorCallbackDelegate)
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
    [<UnmanagedCallersOnly>]
    static member RequestAdapterCallback(status : RequestAdapterStatus, adapter : nativeint, message : nativeptr<byte>, userdata : nativeint) =
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
        | None -> ()

    static member Register(cb : RequestAdapterCallback) =
        lock requestAdapterCallbackCallbacks (fun () ->
            let id = requestAdapterCallbackCurrent
            requestAdapterCallbackCurrent <- requestAdapterCallbackCurrent + 1n
            requestAdapterCallbackCallbacks.[id] <- cb
            struct(requestAdapterCallbackPtr, id)
        )
    [<UnmanagedCallersOnly>]
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
        | None -> ()

    static member Register(cb : BufferMapCallback) =
        lock bufferMapCallbackCallbacks (fun () ->
            let id = bufferMapCallbackCurrent
            bufferMapCallbackCurrent <- bufferMapCallbackCurrent + 1n
            bufferMapCallbackCallbacks.[id] <- cb
            struct(bufferMapCallbackPtr, id)
        )
    [<UnmanagedCallersOnly>]
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
        | None -> ()

    static member Register(cb : CompilationInfoCallback) =
        lock compilationInfoCallbackCallbacks (fun () ->
            let id = compilationInfoCallbackCurrent
            compilationInfoCallbackCurrent <- compilationInfoCallbackCurrent + 1n
            compilationInfoCallbackCallbacks.[id] <- cb
            struct(compilationInfoCallbackPtr, id)
        )
    [<UnmanagedCallersOnly>]
    static member CreateComputePipelineAsyncCallback(status : CreatePipelineAsyncStatus, pipeline : nativeint, message : nativeptr<byte>, userdata : nativeint) =
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
        | None -> ()

    static member Register(cb : CreateComputePipelineAsyncCallback) =
        lock createComputePipelineAsyncCallbackCallbacks (fun () ->
            let id = createComputePipelineAsyncCallbackCurrent
            createComputePipelineAsyncCallbackCurrent <- createComputePipelineAsyncCallbackCurrent + 1n
            createComputePipelineAsyncCallbackCallbacks.[id] <- cb
            struct(createComputePipelineAsyncCallbackPtr, id)
        )
    [<UnmanagedCallersOnly>]
    static member CreateRenderPipelineAsyncCallback(status : CreatePipelineAsyncStatus, pipeline : nativeint, message : nativeptr<byte>, userdata : nativeint) =
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
        | None -> ()

    static member Register(cb : CreateRenderPipelineAsyncCallback) =
        lock createRenderPipelineAsyncCallbackCallbacks (fun () ->
            let id = createRenderPipelineAsyncCallbackCurrent
            createRenderPipelineAsyncCallbackCurrent <- createRenderPipelineAsyncCallbackCurrent + 1n
            createRenderPipelineAsyncCallbackCallbacks.[id] <- cb
            struct(createRenderPipelineAsyncCallbackPtr, id)
        )
    [<UnmanagedCallersOnly>]
    static member DeviceLostCallback(reason : DeviceLostReason, message : nativeptr<byte>, userdata : nativeint) =
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
        | None -> ()

    static member Register(cb : DeviceLostCallback) =
        lock deviceLostCallbackCallbacks (fun () ->
            let id = deviceLostCallbackCurrent
            deviceLostCallbackCurrent <- deviceLostCallbackCurrent + 1n
            deviceLostCallbackCallbacks.[id] <- cb
            struct(deviceLostCallbackPtr, id)
        )
    [<UnmanagedCallersOnly>]
    static member ErrorCallback(typ : ErrorType, message : nativeptr<byte>, userdata : nativeint) =
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
        | None -> ()

    static member Register(cb : ErrorCallback) =
        lock errorCallbackCallbacks (fun () ->
            let id = errorCallbackCurrent
            errorCallbackCurrent <- errorCallbackCurrent + 1n
            errorCallbackCallbacks.[id] <- cb
            struct(errorCallbackPtr, id)
        )
    [<UnmanagedCallersOnly>]
    static member LoggingCallback(typ : LoggingType, message : nativeptr<byte>, userdata : nativeint) =
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
        | None -> ()

    static member Register(cb : LoggingCallback) =
        lock loggingCallbackCallbacks (fun () ->
            let id = loggingCallbackCurrent
            loggingCallbackCurrent <- loggingCallbackCurrent + 1n
            loggingCallbackCallbacks.[id] <- cb
            struct(loggingCallbackPtr, id)
        )
    [<UnmanagedCallersOnly>]
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
        | None -> ()

    static member Register(cb : QueueWorkDoneCallback) =
        lock queueWorkDoneCallbackCallbacks (fun () ->
            let id = queueWorkDoneCallbackCurrent
            queueWorkDoneCallbackCurrent <- queueWorkDoneCallbackCurrent + 1n
            queueWorkDoneCallbackCallbacks.[id] <- cb
            struct(queueWorkDoneCallbackPtr, id)
        )
    [<UnmanagedCallersOnly>]
    static member RequestDeviceCallback(status : RequestDeviceStatus, device : nativeint, message : nativeptr<byte>, userdata : nativeint) =
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
        | None -> ()

    static member Register(cb : RequestDeviceCallback) =
        lock requestDeviceCallbackCallbacks (fun () ->
            let id = requestDeviceCallbackCurrent
            requestDeviceCallbackCurrent <- requestDeviceCallbackCurrent + 1n
            requestDeviceCallbackCallbacks.[id] <- cb
            struct(requestDeviceCallbackPtr, id)
        )
