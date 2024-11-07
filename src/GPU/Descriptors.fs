namespace WebGPU

open System
open System.Runtime.InteropServices

    
exception WebGPUException of string

[<Struct; StructLayout(LayoutKind.Sequential)>]
type Limits =
    {
        /// required >= 65536
        MaxUniformBufferBindingSize : uint64
        
        /// required >= 128*1024*1024 (128MB)
        MaxStorageBufferBindingSize : uint64
        
        /// required >= 256*1024*1024 (256MB)
        MaxBufferSize : uint64
        
        /// required >= 8192
        MaxTextureDimension1D : uint32
        
        /// required >= 8192
        MaxTextureDimension2D : uint32
        
        /// required >= 2048
        MaxTextureDimension3D : uint32
        
        /// required >= 256
        MaxTextureArrayLayers : uint32
        
        /// required >= 4
        MaxBindGroups : uint32
        
        /// required >= 24
        MaxBindGroupsPlusVertexBuffers : uint32
        
        /// required >= 1000
        MaxBindingsPerBindGroup : uint32
        
        /// required >= 8
        MaxDynamicUniformBuffersPerPipelineLayout : uint32
        
        /// required >= 4
        MaxDynamicStorageBuffersPerPipelineLayout : uint32
        
        /// required >= 16
        MaxSampledTexturesPerShaderStage : uint32
        
        /// required >= 16
        MaxSamplersPerShaderStage : uint32
        
        /// required >= 8
        MaxStorageBuffersPerShaderStage : uint32
        
        /// required >= 4
        MaxStorageTexturesPerShaderStage : uint32
        
        /// required >= 12
        MaxUniformBuffersPerShaderStage : uint32
        
        /// required >= 256 bytes
        MinUniformBufferOffsetAlignment : uint32
        
        /// required >= 256 bytes
        MinStorageBufferOffsetAlignment : uint32
        
        /// required >= 8
        MaxVertexBuffers : uint32
        
        /// required >= 16
        MaxVertexAttributes : uint32
        
        /// required >= 2048
        MaxVertexBufferArrayStride : uint32
        
        /// required >= 16
        MaxInterStageShaderVariables : uint32
        
        /// required >= 8
        MaxColorAttachments : uint32
        
        /// required >= 32
        MaxColorAttachmentBytesPerSample : uint32
        
        /// required >= 16384 bytes
        MaxComputeWorkgroupStorageSize : uint32
        
        /// required >= 256
        MaxComputeInvocationsPerWorkgroup : uint32
        
        /// required >= 256
        MaxComputeWorkgroupSizeX : uint32
        
        /// required >= 256
        MaxComputeWorkgroupSizeY : uint32
        
        /// required >= 64
        MaxComputeWorkgroupSizeZ : uint32
        
        /// unused
        UnusedPadding : uint32
    }

module Limits =
    let allSmallerOrEqual (a : Limits) (b : Limits) =
        a.MaxBindGroups <= b.MaxBindGroups &&
        a.MaxBindGroupsPlusVertexBuffers <= b.MaxBindGroupsPlusVertexBuffers &&
        a.MaxBindingsPerBindGroup <= b.MaxBindingsPerBindGroup &&
        a.MaxBufferSize <= b.MaxBufferSize &&
        a.MaxColorAttachmentBytesPerSample <= b.MaxColorAttachmentBytesPerSample &&
        a.MaxColorAttachments <= b.MaxColorAttachments &&
        a.MaxComputeInvocationsPerWorkgroup <= b.MaxComputeInvocationsPerWorkgroup &&
        a.MaxComputeWorkgroupSizeX <= b.MaxComputeWorkgroupSizeX &&
        a.MaxComputeWorkgroupSizeY <= b.MaxComputeWorkgroupSizeY &&
        a.MaxComputeWorkgroupSizeZ <= b.MaxComputeWorkgroupSizeZ &&
        a.MaxComputeWorkgroupStorageSize <= b.MaxComputeWorkgroupStorageSize &&
        a.MaxDynamicStorageBuffersPerPipelineLayout <= b.MaxDynamicStorageBuffersPerPipelineLayout &&
        a.MaxDynamicUniformBuffersPerPipelineLayout <= b.MaxDynamicUniformBuffersPerPipelineLayout &&
        a.MaxInterStageShaderVariables <= b.MaxInterStageShaderVariables &&
        a.MaxSampledTexturesPerShaderStage <= b.MaxSampledTexturesPerShaderStage &&
        a.MaxSamplersPerShaderStage <= b.MaxSamplersPerShaderStage &&
        a.MaxStorageBuffersPerShaderStage <= b.MaxStorageBuffersPerShaderStage &&
        a.MaxStorageTexturesPerShaderStage <= b.MaxStorageTexturesPerShaderStage &&
        a.MaxTextureArrayLayers <= b.MaxTextureArrayLayers &&
        a.MaxTextureDimension1D <= b.MaxTextureDimension1D &&
        a.MaxTextureDimension2D <= b.MaxTextureDimension2D &&
        a.MaxTextureDimension3D <= b.MaxTextureDimension3D &&
        a.MaxUniformBufferBindingSize <= b.MaxUniformBufferBindingSize &&
        a.MaxUniformBuffersPerShaderStage <= b.MaxUniformBuffersPerShaderStage &&
        a.MaxVertexBufferArrayStride <= b.MaxVertexBufferArrayStride &&
        a.MaxVertexAttributes <= b.MaxVertexAttributes &&
        a.MaxVertexBuffers <= b.MaxVertexBuffers &&
        a.MinStorageBufferOffsetAlignment <= b.MinStorageBufferOffsetAlignment &&
        a.MinUniformBufferOffsetAlignment <= b.MinUniformBufferOffsetAlignment
 
type PowerPreference =
    | Undefined = 0
    | Low = 1
    | High = 2

type AdapterOptions =
    {
        PowerPreference : PowerPreference
        ForceFallbackAdapter : bool
    }
    
[<Flags>]
type Features =
    | DepthClipControl = 0x01
    | Depth32FloatStencil8 = 0x02
    | TextureCompressionBC = 0x04
    | TextureCompressionBCSliced3D = 0x08
    | TextureCompressionETC2 = 0x10
    | TextureCompressionASTC = 0x20
    | TimestampQuery = 0x40
    | IndirectFirstInstance = 0x80
    | ShaderF16 = 0x100
    | RG11B10UFloatRenderable = 0x200
    | BGRA8UNormStorage = 0x400
    | Float32Filterable = 0x800
    | ClipDistances = 0x1000
    | DualSourceBlending = 0x2000
    | FirstUnusedBit = 0x4000
 
type QueueDescriptor =
    {
        Label : string
    }

type DeviceDescriptor =
    {
        RequiredLimits      : Limits
        DefaultQueue        : QueueDescriptor
        RequiredFeatures    : Features
    }


