#include <emscripten.h>
#include <emscripten/html5.h>
#include <SDL/SDL_image.h>
#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include <webgpu/webgpu.h>
 
typedef void* WGPUExternalTexture;
EMSCRIPTEN_KEEPALIVE bool gpuAdapterGetLimits(WGPUAdapter self, WGPUSupportedLimits* limits) {
    return wgpuAdapterGetLimits(self, limits);
}
EMSCRIPTEN_KEEPALIVE void gpuAdapterGetProperties(WGPUAdapter self, WGPUAdapterProperties* properties) {
    return wgpuAdapterGetProperties(self, properties);
}
EMSCRIPTEN_KEEPALIVE bool gpuAdapterHasFeature(WGPUAdapter self, int feature) {
    return wgpuAdapterHasFeature(self, feature);
}
EMSCRIPTEN_KEEPALIVE size_t gpuAdapterEnumerateFeatures(WGPUAdapter self, int* features) {
    return wgpuAdapterEnumerateFeatures(self, features);
}
EMSCRIPTEN_KEEPALIVE void gpuAdapterRequestDevice(WGPUAdapter self, const WGPUDeviceDescriptor* descriptor, nativeint callback, void* userdata) {
    return wgpuAdapterRequestDevice(self, descriptor, callback, userdata);
}
EMSCRIPTEN_KEEPALIVE WGPUDevice gpuAdapterCreateDevice(WGPUAdapter self, const WGPUDeviceDescriptor* descriptor) {
    return wgpuAdapterCreateDevice(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE void gpuBindGroupSetLabel(WGPUBindGroup self, const char* label) {
    return wgpuBindGroupSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuBindGroupLayoutSetLabel(WGPUBindGroupLayout self, const char* label) {
    return wgpuBindGroupLayoutSetLabel(self, label);
}
typedef struct { 
   WGPUBuffer Self;
   int Mode;
   size_t Offset;
   size_t Size;
   nativeint Callback;
   void* Userdata;
} WGPUBufferMapAsyncArgs;
EMSCRIPTEN_KEEPALIVE void gpuBufferMapAsync(const WGPUBufferMapAsyncArgs* args) {
    return wgpuBufferMapAsync(args->Self, args->Mode, args->Offset, args->Size, args->Callback, args->Userdata);
}
EMSCRIPTEN_KEEPALIVE void * gpuBufferGetMappedRange(WGPUBuffer self, size_t offset, size_t size) {
    return wgpuBufferGetMappedRange(self, offset, size);
}
EMSCRIPTEN_KEEPALIVE void const * gpuBufferGetConstMappedRange(WGPUBuffer self, size_t offset, size_t size) {
    return wgpuBufferGetConstMappedRange(self, offset, size);
}
EMSCRIPTEN_KEEPALIVE void gpuBufferSetLabel(WGPUBuffer self, const char* label) {
    return wgpuBufferSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE int gpuBufferGetUsage(WGPUBuffer self) {
    return wgpuBufferGetUsage(self);
}
EMSCRIPTEN_KEEPALIVE uint64_t gpuBufferGetSize(WGPUBuffer self) {
    return wgpuBufferGetSize(self);
}
EMSCRIPTEN_KEEPALIVE int gpuBufferGetMapState(WGPUBuffer self) {
    return wgpuBufferGetMapState(self);
}
EMSCRIPTEN_KEEPALIVE void gpuBufferUnmap(WGPUBuffer self) {
    return wgpuBufferUnmap(self);
}
EMSCRIPTEN_KEEPALIVE void gpuBufferDestroy(WGPUBuffer self) {
    return wgpuBufferDestroy(self);
}
EMSCRIPTEN_KEEPALIVE void gpuCommandBufferSetLabel(WGPUCommandBuffer self, const char* label) {
    return wgpuCommandBufferSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE WGPUCommandBuffer gpuCommandEncoderFinish(WGPUCommandEncoder self, const WGPUCommandBufferDescriptor* descriptor) {
    return wgpuCommandEncoderFinish(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE WGPUComputePassEncoder gpuCommandEncoderBeginComputePass(WGPUCommandEncoder self, const WGPUComputePassDescriptor* descriptor) {
    return wgpuCommandEncoderBeginComputePass(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE WGPURenderPassEncoder gpuCommandEncoderBeginRenderPass(WGPUCommandEncoder self, const WGPURenderPassDescriptor* descriptor) {
    return wgpuCommandEncoderBeginRenderPass(self, descriptor);
}
typedef struct { 
   WGPUCommandEncoder Self;
   WGPUBuffer Source;
   uint64_t SourceOffset;
   WGPUBuffer Destination;
   uint64_t DestinationOffset;
   uint64_t Size;
} WGPUCommandEncoderCopyBufferToBufferArgs;
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderCopyBufferToBuffer(const WGPUCommandEncoderCopyBufferToBufferArgs* args) {
    return wgpuCommandEncoderCopyBufferToBuffer(args->Self, args->Source, args->SourceOffset, args->Destination, args->DestinationOffset, args->Size);
}
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderCopyBufferToTexture(WGPUCommandEncoder self, const WGPUImageCopyBuffer* source, const WGPUImageCopyTexture* destination, const WGPUExtent3D* copySize) {
    return wgpuCommandEncoderCopyBufferToTexture(self, source, destination, copySize);
}
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderCopyTextureToBuffer(WGPUCommandEncoder self, const WGPUImageCopyTexture* source, const WGPUImageCopyBuffer* destination, const WGPUExtent3D* copySize) {
    return wgpuCommandEncoderCopyTextureToBuffer(self, source, destination, copySize);
}
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderCopyTextureToTexture(WGPUCommandEncoder self, const WGPUImageCopyTexture* source, const WGPUImageCopyTexture* destination, const WGPUExtent3D* copySize) {
    return wgpuCommandEncoderCopyTextureToTexture(self, source, destination, copySize);
}
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderCopyTextureToTextureInternal(WGPUCommandEncoder self, const WGPUImageCopyTexture* source, const WGPUImageCopyTexture* destination, const WGPUExtent3D* copySize) {
    return wgpuCommandEncoderCopyTextureToTextureInternal(self, source, destination, copySize);
}
typedef struct { 
   WGPUCommandEncoder Self;
   WGPUBuffer Buffer;
   uint64_t Offset;
   uint64_t Size;
} WGPUCommandEncoderClearBufferArgs;
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderClearBuffer(const WGPUCommandEncoderClearBufferArgs* args) {
    return wgpuCommandEncoderClearBuffer(args->Self, args->Buffer, args->Offset, args->Size);
}
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderInjectValidationError(WGPUCommandEncoder self, const char* message) {
    return wgpuCommandEncoderInjectValidationError(self, message);
}
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderInsertDebugMarker(WGPUCommandEncoder self, const char* markerLabel) {
    return wgpuCommandEncoderInsertDebugMarker(self, markerLabel);
}
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderPopDebugGroup(WGPUCommandEncoder self) {
    return wgpuCommandEncoderPopDebugGroup(self);
}
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderPushDebugGroup(WGPUCommandEncoder self, const char* groupLabel) {
    return wgpuCommandEncoderPushDebugGroup(self, groupLabel);
}
typedef struct { 
   WGPUCommandEncoder Self;
   WGPUQuerySet QuerySet;
   uint32_t FirstQuery;
   uint32_t QueryCount;
   WGPUBuffer Destination;
   uint64_t DestinationOffset;
} WGPUCommandEncoderResolveQuerySetArgs;
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderResolveQuerySet(const WGPUCommandEncoderResolveQuerySetArgs* args) {
    return wgpuCommandEncoderResolveQuerySet(args->Self, args->QuerySet, args->FirstQuery, args->QueryCount, args->Destination, args->DestinationOffset);
}
typedef struct { 
   WGPUCommandEncoder Self;
   WGPUBuffer Buffer;
   uint64_t BufferOffset;
   const uint8_t* Data;
   uint64_t Size;
} WGPUCommandEncoderWriteBufferArgs;
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderWriteBuffer(const WGPUCommandEncoderWriteBufferArgs* args) {
    return wgpuCommandEncoderWriteBuffer(args->Self, args->Buffer, args->BufferOffset, args->Data, args->Size);
}
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderWriteTimestamp(WGPUCommandEncoder self, WGPUQuerySet querySet, uint32_t queryIndex) {
    return wgpuCommandEncoderWriteTimestamp(self, querySet, queryIndex);
}
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderSetLabel(WGPUCommandEncoder self, const char* label) {
    return wgpuCommandEncoderSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderInsertDebugMarker(WGPUComputePassEncoder self, const char* markerLabel) {
    return wgpuComputePassEncoderInsertDebugMarker(self, markerLabel);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderPopDebugGroup(WGPUComputePassEncoder self) {
    return wgpuComputePassEncoderPopDebugGroup(self);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderPushDebugGroup(WGPUComputePassEncoder self, const char* groupLabel) {
    return wgpuComputePassEncoderPushDebugGroup(self, groupLabel);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderSetPipeline(WGPUComputePassEncoder self, WGPUComputePipeline pipeline) {
    return wgpuComputePassEncoderSetPipeline(self, pipeline);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderSetBindGroup(WGPUComputePassEncoder self, uint32_t groupIndex, WGPUBindGroup group, uint32_t dynamicOffsetCount, const uint32_t* dynamicOffsets) {
    return wgpuComputePassEncoderSetBindGroup(self, groupIndex, group, dynamicOffsetCount, dynamicOffsets);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderWriteTimestamp(WGPUComputePassEncoder self, WGPUQuerySet querySet, uint32_t queryIndex) {
    return wgpuComputePassEncoderWriteTimestamp(self, querySet, queryIndex);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderBeginPipelineStatisticsQuery(WGPUComputePassEncoder self, WGPUQuerySet querySet, uint32_t queryIndex) {
    return wgpuComputePassEncoderBeginPipelineStatisticsQuery(self, querySet, queryIndex);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderDispatch(WGPUComputePassEncoder self, uint32_t workgroupCountX, uint32_t workgroupCountY, uint32_t workgroupCountZ) {
    return wgpuComputePassEncoderDispatch(self, workgroupCountX, workgroupCountY, workgroupCountZ);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderDispatchWorkgroups(WGPUComputePassEncoder self, uint32_t workgroupCountX, uint32_t workgroupCountY, uint32_t workgroupCountZ) {
    return wgpuComputePassEncoderDispatchWorkgroups(self, workgroupCountX, workgroupCountY, workgroupCountZ);
}
typedef struct { 
   WGPUComputePassEncoder Self;
   WGPUBuffer IndirectBuffer;
   uint64_t IndirectOffset;
} WGPUComputePassEncoderDispatchIndirectArgs;
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderDispatchIndirect(const WGPUComputePassEncoderDispatchIndirectArgs* args) {
    return wgpuComputePassEncoderDispatchIndirect(args->Self, args->IndirectBuffer, args->IndirectOffset);
}
typedef struct { 
   WGPUComputePassEncoder Self;
   WGPUBuffer IndirectBuffer;
   uint64_t IndirectOffset;
} WGPUComputePassEncoderDispatchWorkgroupsIndirectArgs;
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderDispatchWorkgroupsIndirect(const WGPUComputePassEncoderDispatchWorkgroupsIndirectArgs* args) {
    return wgpuComputePassEncoderDispatchWorkgroupsIndirect(args->Self, args->IndirectBuffer, args->IndirectOffset);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderEnd(WGPUComputePassEncoder self) {
    return wgpuComputePassEncoderEnd(self);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderEndPass(WGPUComputePassEncoder self) {
    return wgpuComputePassEncoderEndPass(self);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderEndPipelineStatisticsQuery(WGPUComputePassEncoder self) {
    return wgpuComputePassEncoderEndPipelineStatisticsQuery(self);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderSetLabel(WGPUComputePassEncoder self, const char* label) {
    return wgpuComputePassEncoderSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE WGPUBindGroupLayout gpuComputePipelineGetBindGroupLayout(WGPUComputePipeline self, uint32_t groupIndex) {
    return wgpuComputePipelineGetBindGroupLayout(self, groupIndex);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePipelineSetLabel(WGPUComputePipeline self, const char* label) {
    return wgpuComputePipelineSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE WGPUBindGroup gpuDeviceCreateBindGroup(WGPUDevice self, const WGPUBindGroupDescriptor* descriptor) {
    return wgpuDeviceCreateBindGroup(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE WGPUBindGroupLayout gpuDeviceCreateBindGroupLayout(WGPUDevice self, const WGPUBindGroupLayoutDescriptor* descriptor) {
    return wgpuDeviceCreateBindGroupLayout(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE WGPUBuffer gpuDeviceCreateBuffer(WGPUDevice self, const WGPUBufferDescriptor* descriptor) {
    return wgpuDeviceCreateBuffer(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE WGPUBuffer gpuDeviceCreateErrorBuffer(WGPUDevice self, const WGPUBufferDescriptor* descriptor) {
    return wgpuDeviceCreateErrorBuffer(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE WGPUCommandEncoder gpuDeviceCreateCommandEncoder(WGPUDevice self, const WGPUCommandEncoderDescriptor* descriptor) {
    return wgpuDeviceCreateCommandEncoder(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE WGPUComputePipeline gpuDeviceCreateComputePipeline(WGPUDevice self, const WGPUComputePipelineDescriptor* descriptor) {
    return wgpuDeviceCreateComputePipeline(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE void gpuDeviceCreateComputePipelineAsync(WGPUDevice self, const WGPUComputePipelineDescriptor* descriptor, nativeint callback, void* userdata) {
    return wgpuDeviceCreateComputePipelineAsync(self, descriptor, callback, userdata);
}
EMSCRIPTEN_KEEPALIVE WGPUPipelineLayout gpuDeviceCreatePipelineLayout(WGPUDevice self, const WGPUPipelineLayoutDescriptor* descriptor) {
    return wgpuDeviceCreatePipelineLayout(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE WGPUQuerySet gpuDeviceCreateQuerySet(WGPUDevice self, const WGPUQuerySetDescriptor* descriptor) {
    return wgpuDeviceCreateQuerySet(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE void gpuDeviceCreateRenderPipelineAsync(WGPUDevice self, const WGPURenderPipelineDescriptor* descriptor, nativeint callback, void* userdata) {
    return wgpuDeviceCreateRenderPipelineAsync(self, descriptor, callback, userdata);
}
EMSCRIPTEN_KEEPALIVE WGPURenderBundleEncoder gpuDeviceCreateRenderBundleEncoder(WGPUDevice self, const WGPURenderBundleEncoderDescriptor* descriptor) {
    return wgpuDeviceCreateRenderBundleEncoder(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE WGPURenderPipeline gpuDeviceCreateRenderPipeline(WGPUDevice self, const WGPURenderPipelineDescriptor* descriptor) {
    return wgpuDeviceCreateRenderPipeline(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE WGPUSampler gpuDeviceCreateSampler(WGPUDevice self, const WGPUSamplerDescriptor* descriptor) {
    return wgpuDeviceCreateSampler(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE WGPUShaderModule gpuDeviceCreateShaderModule(WGPUDevice self, const WGPUShaderModuleDescriptor* descriptor) {
    return wgpuDeviceCreateShaderModule(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE WGPUSwapChain gpuDeviceCreateSwapChain(WGPUDevice self, WGPUSurface surface, const WGPUSwapChainDescriptor* descriptor) {
    return wgpuDeviceCreateSwapChain(self, surface, descriptor);
}
EMSCRIPTEN_KEEPALIVE WGPUTexture gpuDeviceCreateTexture(WGPUDevice self, const WGPUTextureDescriptor* descriptor) {
    return wgpuDeviceCreateTexture(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE WGPUTexture gpuDeviceCreateErrorTexture(WGPUDevice self, const WGPUTextureDescriptor* descriptor) {
    return wgpuDeviceCreateErrorTexture(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE void gpuDeviceDestroy(WGPUDevice self) {
    return wgpuDeviceDestroy(self);
}
EMSCRIPTEN_KEEPALIVE bool gpuDeviceGetLimits(WGPUDevice self, WGPUSupportedLimits* limits) {
    return wgpuDeviceGetLimits(self, limits);
}
EMSCRIPTEN_KEEPALIVE bool gpuDeviceHasFeature(WGPUDevice self, int feature) {
    return wgpuDeviceHasFeature(self, feature);
}
EMSCRIPTEN_KEEPALIVE size_t gpuDeviceEnumerateFeatures(WGPUDevice self, int* features) {
    return wgpuDeviceEnumerateFeatures(self, features);
}
EMSCRIPTEN_KEEPALIVE WGPUAdapter gpuDeviceGetAdapter(WGPUDevice self) {
    return wgpuDeviceGetAdapter(self);
}
EMSCRIPTEN_KEEPALIVE WGPUQueue gpuDeviceGetQueue(WGPUDevice self) {
    return wgpuDeviceGetQueue(self);
}
EMSCRIPTEN_KEEPALIVE void gpuDeviceInjectError(WGPUDevice self, int typ, const char* message) {
    return wgpuDeviceInjectError(self, typ, message);
}
EMSCRIPTEN_KEEPALIVE void gpuDeviceForceLoss(WGPUDevice self, int typ, const char* message) {
    return wgpuDeviceForceLoss(self, typ, message);
}
EMSCRIPTEN_KEEPALIVE void gpuDeviceTick(WGPUDevice self) {
    return wgpuDeviceTick(self);
}
EMSCRIPTEN_KEEPALIVE void gpuDeviceSetUncapturedErrorCallback(WGPUDevice self, nativeint callback, void* userdata) {
    return wgpuDeviceSetUncapturedErrorCallback(self, callback, userdata);
}
EMSCRIPTEN_KEEPALIVE void gpuDeviceSetDeviceLostCallback(WGPUDevice self, nativeint callback, void* userdata) {
    return wgpuDeviceSetDeviceLostCallback(self, callback, userdata);
}
EMSCRIPTEN_KEEPALIVE void gpuDevicePushErrorScope(WGPUDevice self, int filter) {
    return wgpuDevicePushErrorScope(self, filter);
}
EMSCRIPTEN_KEEPALIVE bool gpuDevicePopErrorScope(WGPUDevice self, nativeint callback, void* userdata) {
    return wgpuDevicePopErrorScope(self, callback, userdata);
}
EMSCRIPTEN_KEEPALIVE void gpuDeviceSetLabel(WGPUDevice self, const char* label) {
    return wgpuDeviceSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuDeviceValidateTextureDescriptor(WGPUDevice self, const WGPUTextureDescriptor* descriptor) {
    return wgpuDeviceValidateTextureDescriptor(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE void gpuExternalTextureSetLabel(WGPUExternalTexture self, const char* label) {
    return wgpuExternalTextureSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuExternalTextureDestroy(WGPUExternalTexture self) {
    return wgpuExternalTextureDestroy(self);
}
EMSCRIPTEN_KEEPALIVE void gpuExternalTextureExpire(WGPUExternalTexture self) {
    return wgpuExternalTextureExpire(self);
}
EMSCRIPTEN_KEEPALIVE void gpuExternalTextureRefresh(WGPUExternalTexture self) {
    return wgpuExternalTextureRefresh(self);
}
EMSCRIPTEN_KEEPALIVE WGPUSurface gpuInstanceCreateSurface(WGPUInstance self, const WGPUSurfaceDescriptor* descriptor) {
    return wgpuInstanceCreateSurface(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE void gpuInstanceProcessEvents(WGPUInstance self) {
    return wgpuInstanceProcessEvents(self);
}
EMSCRIPTEN_KEEPALIVE void gpuInstanceRequestAdapter(WGPUInstance self, const WGPURequestAdapterOptions* options, nativeint callback, void* userdata) {
    return wgpuInstanceRequestAdapter(self, options, callback, userdata);
}
EMSCRIPTEN_KEEPALIVE void gpuPipelineLayoutSetLabel(WGPUPipelineLayout self, const char* label) {
    return wgpuPipelineLayoutSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuQuerySetSetLabel(WGPUQuerySet self, const char* label) {
    return wgpuQuerySetSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE int gpuQuerySetGetType(WGPUQuerySet self) {
    return wgpuQuerySetGetType(self);
}
EMSCRIPTEN_KEEPALIVE uint32_t gpuQuerySetGetCount(WGPUQuerySet self) {
    return wgpuQuerySetGetCount(self);
}
EMSCRIPTEN_KEEPALIVE void gpuQuerySetDestroy(WGPUQuerySet self) {
    return wgpuQuerySetDestroy(self);
}
EMSCRIPTEN_KEEPALIVE void gpuQueueSubmit(WGPUQueue self, uint32_t commandCount, const WGPUCommandBuffer* commands) {
    return wgpuQueueSubmit(self, commandCount, commands);
}
typedef struct { 
   WGPUQueue Self;
   uint64_t SignalValue;
   nativeint Callback;
   void* Userdata;
} WGPUQueueOnSubmittedWorkDoneArgs;
EMSCRIPTEN_KEEPALIVE void gpuQueueOnSubmittedWorkDone(const WGPUQueueOnSubmittedWorkDoneArgs* args) {
    return wgpuQueueOnSubmittedWorkDone(args->Self, args->SignalValue, args->Callback, args->Userdata);
}
typedef struct { 
   WGPUQueue Self;
   WGPUBuffer Buffer;
   uint64_t BufferOffset;
   const void* Data;
   size_t Size;
} WGPUQueueWriteBufferArgs;
EMSCRIPTEN_KEEPALIVE void gpuQueueWriteBuffer(const WGPUQueueWriteBufferArgs* args) {
    return wgpuQueueWriteBuffer(args->Self, args->Buffer, args->BufferOffset, args->Data, args->Size);
}
typedef struct { 
   WGPUQueue Self;
   const WGPUImageCopyTexture* Destination;
   const void* Data;
   size_t DataSize;
   const WGPUTextureDataLayout* DataLayout;
   const WGPUExtent3D* WriteSize;
} WGPUQueueWriteTextureArgs;
EMSCRIPTEN_KEEPALIVE void gpuQueueWriteTexture(const WGPUQueueWriteTextureArgs* args) {
    return wgpuQueueWriteTexture(args->Self, args->Destination, args->Data, args->DataSize, args->DataLayout, args->WriteSize);
}
EMSCRIPTEN_KEEPALIVE void gpuQueueSetLabel(WGPUQueue self, const char* label) {
    return wgpuQueueSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderSetPipeline(WGPURenderBundleEncoder self, WGPURenderPipeline pipeline) {
    return wgpuRenderBundleEncoderSetPipeline(self, pipeline);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderSetBindGroup(WGPURenderBundleEncoder self, uint32_t groupIndex, WGPUBindGroup group, uint32_t dynamicOffsetCount, const uint32_t* dynamicOffsets) {
    return wgpuRenderBundleEncoderSetBindGroup(self, groupIndex, group, dynamicOffsetCount, dynamicOffsets);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderDraw(WGPURenderBundleEncoder self, uint32_t vertexCount, uint32_t instanceCount, uint32_t firstVertex, uint32_t firstInstance) {
    return wgpuRenderBundleEncoderDraw(self, vertexCount, instanceCount, firstVertex, firstInstance);
}
typedef struct { 
   WGPURenderBundleEncoder Self;
   uint32_t IndexCount;
   uint32_t InstanceCount;
   uint32_t FirstIndex;
   int32_t BaseVertex;
   uint32_t FirstInstance;
} WGPURenderBundleEncoderDrawIndexedArgs;
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderDrawIndexed(const WGPURenderBundleEncoderDrawIndexedArgs* args) {
    return wgpuRenderBundleEncoderDrawIndexed(args->Self, args->IndexCount, args->InstanceCount, args->FirstIndex, args->BaseVertex, args->FirstInstance);
}
typedef struct { 
   WGPURenderBundleEncoder Self;
   WGPUBuffer IndirectBuffer;
   uint64_t IndirectOffset;
} WGPURenderBundleEncoderDrawIndirectArgs;
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderDrawIndirect(const WGPURenderBundleEncoderDrawIndirectArgs* args) {
    return wgpuRenderBundleEncoderDrawIndirect(args->Self, args->IndirectBuffer, args->IndirectOffset);
}
typedef struct { 
   WGPURenderBundleEncoder Self;
   WGPUBuffer IndirectBuffer;
   uint64_t IndirectOffset;
} WGPURenderBundleEncoderDrawIndexedIndirectArgs;
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderDrawIndexedIndirect(const WGPURenderBundleEncoderDrawIndexedIndirectArgs* args) {
    return wgpuRenderBundleEncoderDrawIndexedIndirect(args->Self, args->IndirectBuffer, args->IndirectOffset);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderInsertDebugMarker(WGPURenderBundleEncoder self, const char* markerLabel) {
    return wgpuRenderBundleEncoderInsertDebugMarker(self, markerLabel);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderPopDebugGroup(WGPURenderBundleEncoder self) {
    return wgpuRenderBundleEncoderPopDebugGroup(self);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderPushDebugGroup(WGPURenderBundleEncoder self, const char* groupLabel) {
    return wgpuRenderBundleEncoderPushDebugGroup(self, groupLabel);
}
typedef struct { 
   WGPURenderBundleEncoder Self;
   uint32_t Slot;
   WGPUBuffer Buffer;
   uint64_t Offset;
   uint64_t Size;
} WGPURenderBundleEncoderSetVertexBufferArgs;
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderSetVertexBuffer(const WGPURenderBundleEncoderSetVertexBufferArgs* args) {
    return wgpuRenderBundleEncoderSetVertexBuffer(args->Self, args->Slot, args->Buffer, args->Offset, args->Size);
}
typedef struct { 
   WGPURenderBundleEncoder Self;
   WGPUBuffer Buffer;
   int Format;
   uint64_t Offset;
   uint64_t Size;
} WGPURenderBundleEncoderSetIndexBufferArgs;
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderSetIndexBuffer(const WGPURenderBundleEncoderSetIndexBufferArgs* args) {
    return wgpuRenderBundleEncoderSetIndexBuffer(args->Self, args->Buffer, args->Format, args->Offset, args->Size);
}
EMSCRIPTEN_KEEPALIVE WGPURenderBundle gpuRenderBundleEncoderFinish(WGPURenderBundleEncoder self, const WGPURenderBundleDescriptor* descriptor) {
    return wgpuRenderBundleEncoderFinish(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderSetLabel(WGPURenderBundleEncoder self, const char* label) {
    return wgpuRenderBundleEncoderSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderSetPipeline(WGPURenderPassEncoder self, WGPURenderPipeline pipeline) {
    return wgpuRenderPassEncoderSetPipeline(self, pipeline);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderSetBindGroup(WGPURenderPassEncoder self, uint32_t groupIndex, WGPUBindGroup group, uint32_t dynamicOffsetCount, const uint32_t* dynamicOffsets) {
    return wgpuRenderPassEncoderSetBindGroup(self, groupIndex, group, dynamicOffsetCount, dynamicOffsets);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderDraw(WGPURenderPassEncoder self, uint32_t vertexCount, uint32_t instanceCount, uint32_t firstVertex, uint32_t firstInstance) {
    return wgpuRenderPassEncoderDraw(self, vertexCount, instanceCount, firstVertex, firstInstance);
}
typedef struct { 
   WGPURenderPassEncoder Self;
   uint32_t IndexCount;
   uint32_t InstanceCount;
   uint32_t FirstIndex;
   int32_t BaseVertex;
   uint32_t FirstInstance;
} WGPURenderPassEncoderDrawIndexedArgs;
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderDrawIndexed(const WGPURenderPassEncoderDrawIndexedArgs* args) {
    return wgpuRenderPassEncoderDrawIndexed(args->Self, args->IndexCount, args->InstanceCount, args->FirstIndex, args->BaseVertex, args->FirstInstance);
}
typedef struct { 
   WGPURenderPassEncoder Self;
   WGPUBuffer IndirectBuffer;
   uint64_t IndirectOffset;
} WGPURenderPassEncoderDrawIndirectArgs;
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderDrawIndirect(const WGPURenderPassEncoderDrawIndirectArgs* args) {
    return wgpuRenderPassEncoderDrawIndirect(args->Self, args->IndirectBuffer, args->IndirectOffset);
}
typedef struct { 
   WGPURenderPassEncoder Self;
   WGPUBuffer IndirectBuffer;
   uint64_t IndirectOffset;
} WGPURenderPassEncoderDrawIndexedIndirectArgs;
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderDrawIndexedIndirect(const WGPURenderPassEncoderDrawIndexedIndirectArgs* args) {
    return wgpuRenderPassEncoderDrawIndexedIndirect(args->Self, args->IndirectBuffer, args->IndirectOffset);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderExecuteBundles(WGPURenderPassEncoder self, uint32_t bundleCount, const WGPURenderBundle* bundles) {
    return wgpuRenderPassEncoderExecuteBundles(self, bundleCount, bundles);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderInsertDebugMarker(WGPURenderPassEncoder self, const char* markerLabel) {
    return wgpuRenderPassEncoderInsertDebugMarker(self, markerLabel);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderPopDebugGroup(WGPURenderPassEncoder self) {
    return wgpuRenderPassEncoderPopDebugGroup(self);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderPushDebugGroup(WGPURenderPassEncoder self, const char* groupLabel) {
    return wgpuRenderPassEncoderPushDebugGroup(self, groupLabel);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderSetStencilReference(WGPURenderPassEncoder self, uint32_t reference) {
    return wgpuRenderPassEncoderSetStencilReference(self, reference);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderSetBlendConstant(WGPURenderPassEncoder self, const WGPUColor* color) {
    return wgpuRenderPassEncoderSetBlendConstant(self, color);
}
typedef struct { 
   WGPURenderPassEncoder Self;
   float X;
   float Y;
   float Width;
   float Height;
   float MinDepth;
   float MaxDepth;
} WGPURenderPassEncoderSetViewportArgs;
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderSetViewport(const WGPURenderPassEncoderSetViewportArgs* args) {
    return wgpuRenderPassEncoderSetViewport(args->Self, args->X, args->Y, args->Width, args->Height, args->MinDepth, args->MaxDepth);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderSetScissorRect(WGPURenderPassEncoder self, uint32_t x, uint32_t y, uint32_t width, uint32_t height) {
    return wgpuRenderPassEncoderSetScissorRect(self, x, y, width, height);
}
typedef struct { 
   WGPURenderPassEncoder Self;
   uint32_t Slot;
   WGPUBuffer Buffer;
   uint64_t Offset;
   uint64_t Size;
} WGPURenderPassEncoderSetVertexBufferArgs;
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderSetVertexBuffer(const WGPURenderPassEncoderSetVertexBufferArgs* args) {
    return wgpuRenderPassEncoderSetVertexBuffer(args->Self, args->Slot, args->Buffer, args->Offset, args->Size);
}
typedef struct { 
   WGPURenderPassEncoder Self;
   WGPUBuffer Buffer;
   int Format;
   uint64_t Offset;
   uint64_t Size;
} WGPURenderPassEncoderSetIndexBufferArgs;
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderSetIndexBuffer(const WGPURenderPassEncoderSetIndexBufferArgs* args) {
    return wgpuRenderPassEncoderSetIndexBuffer(args->Self, args->Buffer, args->Format, args->Offset, args->Size);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderBeginOcclusionQuery(WGPURenderPassEncoder self, uint32_t queryIndex) {
    return wgpuRenderPassEncoderBeginOcclusionQuery(self, queryIndex);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderBeginPipelineStatisticsQuery(WGPURenderPassEncoder self, WGPUQuerySet querySet, uint32_t queryIndex) {
    return wgpuRenderPassEncoderBeginPipelineStatisticsQuery(self, querySet, queryIndex);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderEndOcclusionQuery(WGPURenderPassEncoder self) {
    return wgpuRenderPassEncoderEndOcclusionQuery(self);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderWriteTimestamp(WGPURenderPassEncoder self, WGPUQuerySet querySet, uint32_t queryIndex) {
    return wgpuRenderPassEncoderWriteTimestamp(self, querySet, queryIndex);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderEnd(WGPURenderPassEncoder self) {
    return wgpuRenderPassEncoderEnd(self);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderEndPass(WGPURenderPassEncoder self) {
    return wgpuRenderPassEncoderEndPass(self);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderEndPipelineStatisticsQuery(WGPURenderPassEncoder self) {
    return wgpuRenderPassEncoderEndPipelineStatisticsQuery(self);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderSetLabel(WGPURenderPassEncoder self, const char* label) {
    return wgpuRenderPassEncoderSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE WGPUBindGroupLayout gpuRenderPipelineGetBindGroupLayout(WGPURenderPipeline self, uint32_t groupIndex) {
    return wgpuRenderPipelineGetBindGroupLayout(self, groupIndex);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPipelineSetLabel(WGPURenderPipeline self, const char* label) {
    return wgpuRenderPipelineSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuSamplerSetLabel(WGPUSampler self, const char* label) {
    return wgpuSamplerSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuShaderModuleGetCompilationInfo(WGPUShaderModule self, nativeint callback, void* userdata) {
    return wgpuShaderModuleGetCompilationInfo(self, callback, userdata);
}
EMSCRIPTEN_KEEPALIVE void gpuShaderModuleSetLabel(WGPUShaderModule self, const char* label) {
    return wgpuShaderModuleSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE int gpuSurfaceGetPreferredFormat(WGPUSurface self, WGPUAdapter adapter) {
    return wgpuSurfaceGetPreferredFormat(self, adapter);
}
EMSCRIPTEN_KEEPALIVE void gpuSwapChainConfigure(WGPUSwapChain self, int format, int allowedUsage, uint32_t width, uint32_t height) {
    return wgpuSwapChainConfigure(self, format, allowedUsage, width, height);
}
EMSCRIPTEN_KEEPALIVE WGPUTextureView gpuSwapChainGetCurrentTextureView(WGPUSwapChain self) {
    return wgpuSwapChainGetCurrentTextureView(self);
}
EMSCRIPTEN_KEEPALIVE void gpuSwapChainPresent(WGPUSwapChain self) {
    return wgpuSwapChainPresent(self);
}
EMSCRIPTEN_KEEPALIVE WGPUTextureView gpuTextureCreateView(WGPUTexture self, const WGPUTextureViewDescriptor* descriptor) {
    return wgpuTextureCreateView(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE void gpuTextureSetLabel(WGPUTexture self, const char* label) {
    return wgpuTextureSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE uint32_t gpuTextureGetWidth(WGPUTexture self) {
    return wgpuTextureGetWidth(self);
}
EMSCRIPTEN_KEEPALIVE uint32_t gpuTextureGetHeight(WGPUTexture self) {
    return wgpuTextureGetHeight(self);
}
EMSCRIPTEN_KEEPALIVE uint32_t gpuTextureGetDepthOrArrayLayers(WGPUTexture self) {
    return wgpuTextureGetDepthOrArrayLayers(self);
}
EMSCRIPTEN_KEEPALIVE uint32_t gpuTextureGetMipLevelCount(WGPUTexture self) {
    return wgpuTextureGetMipLevelCount(self);
}
EMSCRIPTEN_KEEPALIVE uint32_t gpuTextureGetSampleCount(WGPUTexture self) {
    return wgpuTextureGetSampleCount(self);
}
EMSCRIPTEN_KEEPALIVE int gpuTextureGetDimension(WGPUTexture self) {
    return wgpuTextureGetDimension(self);
}
EMSCRIPTEN_KEEPALIVE int gpuTextureGetFormat(WGPUTexture self) {
    return wgpuTextureGetFormat(self);
}
EMSCRIPTEN_KEEPALIVE int gpuTextureGetUsage(WGPUTexture self) {
    return wgpuTextureGetUsage(self);
}
EMSCRIPTEN_KEEPALIVE void gpuTextureDestroy(WGPUTexture self) {
    return wgpuTextureDestroy(self);
}
EMSCRIPTEN_KEEPALIVE void gpuTextureViewSetLabel(WGPUTextureView self, const char* label) {
    return wgpuTextureViewSetLabel(self, label);
}
