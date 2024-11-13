#include <emscripten.h>
#include <emscripten/html5.h>
#include <SDL/SDL_image.h>
#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include <webgpu/webgpu.h>
 
WGPUInstance gpuAdapterGetInstance(WGPUAdapter self) {
    return wgpuAdapterGetInstance(self);
}
int gpuAdapterGetLimits(WGPUAdapter self, WGPUSupportedLimits* limits) {
    return wgpuAdapterGetLimits(self, limits);
}
int gpuAdapterGetInfo(WGPUAdapter self, WGPUAdapterInfo* info) {
    return wgpuAdapterGetInfo(self, info);
}
bool gpuAdapterHasFeature(WGPUAdapter self, int feature) {
    return wgpuAdapterHasFeature(self, feature);
}
size_t gpuAdapterEnumerateFeatures(WGPUAdapter self, int* features) {
    return wgpuAdapterEnumerateFeatures(self, features);
}
void gpuAdapterGetFeatures(WGPUAdapter self, WGPUSupportedFeatures* features) {
    return wgpuAdapterGetFeatures(self, features);
}
void gpuAdapterRequestDevice(WGPUAdapter self, const WGPUDeviceDescriptor* descriptor, WGPURequestDeviceCallback callback, void * userdata) {
    return wgpuAdapterRequestDevice(self, descriptor, callback, userdata);
}
WGPUFuture gpuAdapterRequestDeviceF(WGPUAdapter self, const WGPUDeviceDescriptor* options, WGPURequestDeviceCallbackInfo callbackInfo) {
    return wgpuAdapterRequestDeviceF(self, options, callbackInfo);
}
WGPUFuture gpuAdapterRequestDevice2(WGPUAdapter self, const WGPUDeviceDescriptor* options, WGPURequestDeviceCallbackInfo2 callbackInfo) {
    return wgpuAdapterRequestDevice2(self, options, callbackInfo);
}
WGPUDevice gpuAdapterCreateDevice(WGPUAdapter self, const WGPUDeviceDescriptor* descriptor) {
    return wgpuAdapterCreateDevice(self, descriptor);
}
int gpuAdapterGetFormatCapabilities(WGPUAdapter self, int format, WGPUFormatCapabilities* capabilities) {
    return wgpuAdapterGetFormatCapabilities(self, format, capabilities);
}
void gpuBindGroupSetLabel(WGPUBindGroup self, WGPUStringView label) {
    return wgpuBindGroupSetLabel(self, label);
}
void gpuBindGroupLayoutSetLabel(WGPUBindGroupLayout self, WGPUStringView label) {
    return wgpuBindGroupLayoutSetLabel(self, label);
}
"BufferMapAsync"
typedef struct { 
   WGPUBuffer Self;
   int Mode;
   size_t Offset;
   size_t Size;
   WGPUBufferMapCallback Callback;
   void * Userdata;
} WGPUBufferMapAsyncArgs;
void gpuBufferMapAsync(const WGPUBufferMapAsyncArgs* args) {
    return wgpuBufferMapAsync(args->Self, args->Mode, args->Offset, args->Size, args->Callback, args->Userdata);
}
WGPUFuture gpuBufferMapAsyncF(WGPUBuffer self, int mode, size_t offset, size_t size, WGPUBufferMapCallbackInfo callbackInfo) {
    return wgpuBufferMapAsyncF(self, mode, offset, size, callbackInfo);
}
WGPUFuture gpuBufferMapAsync2(WGPUBuffer self, int mode, size_t offset, size_t size, WGPUBufferMapCallbackInfo2 callbackInfo) {
    return wgpuBufferMapAsync2(self, mode, offset, size, callbackInfo);
}
void * gpuBufferGetMappedRange(WGPUBuffer self, size_t offset, size_t size) {
    return wgpuBufferGetMappedRange(self, offset, size);
}
void const * gpuBufferGetConstMappedRange(WGPUBuffer self, size_t offset, size_t size) {
    return wgpuBufferGetConstMappedRange(self, offset, size);
}
void gpuBufferSetLabel(WGPUBuffer self, WGPUStringView label) {
    return wgpuBufferSetLabel(self, label);
}
int gpuBufferGetUsage(WGPUBuffer self) {
    return wgpuBufferGetUsage(self);
}
uint64_t gpuBufferGetSize(WGPUBuffer self) {
    return wgpuBufferGetSize(self);
}
int gpuBufferGetMapState(WGPUBuffer self) {
    return wgpuBufferGetMapState(self);
}
void gpuBufferUnmap(WGPUBuffer self) {
    return wgpuBufferUnmap(self);
}
void gpuBufferDestroy(WGPUBuffer self) {
    return wgpuBufferDestroy(self);
}
void gpuCommandBufferSetLabel(WGPUCommandBuffer self, WGPUStringView label) {
    return wgpuCommandBufferSetLabel(self, label);
}
WGPUCommandBuffer gpuCommandEncoderFinish(WGPUCommandEncoder self, const WGPUCommandBufferDescriptor* descriptor) {
    return wgpuCommandEncoderFinish(self, descriptor);
}
WGPUComputePassEncoder gpuCommandEncoderBeginComputePass(WGPUCommandEncoder self, const WGPUComputePassDescriptor* descriptor) {
    return wgpuCommandEncoderBeginComputePass(self, descriptor);
}
WGPURenderPassEncoder gpuCommandEncoderBeginRenderPass(WGPUCommandEncoder self, const WGPURenderPassDescriptor* descriptor) {
    return wgpuCommandEncoderBeginRenderPass(self, descriptor);
}
"CommandEncoderCopyBufferToBuffer"
typedef struct { 
   WGPUCommandEncoder Self;
   WGPUBuffer Source;
   uint64_t SourceOffset;
   WGPUBuffer Destination;
   uint64_t DestinationOffset;
   uint64_t Size;
} WGPUCommandEncoderCopyBufferToBufferArgs;
void gpuCommandEncoderCopyBufferToBuffer(const WGPUCommandEncoderCopyBufferToBufferArgs* args) {
    return wgpuCommandEncoderCopyBufferToBuffer(args->Self, args->Source, args->SourceOffset, args->Destination, args->DestinationOffset, args->Size);
}
void gpuCommandEncoderCopyBufferToTexture(WGPUCommandEncoder self, const WGPUImageCopyBuffer* source, const WGPUImageCopyTexture* destination, const WGPUExtent3D* copySize) {
    return wgpuCommandEncoderCopyBufferToTexture(self, source, destination, copySize);
}
void gpuCommandEncoderCopyTextureToBuffer(WGPUCommandEncoder self, const WGPUImageCopyTexture* source, const WGPUImageCopyBuffer* destination, const WGPUExtent3D* copySize) {
    return wgpuCommandEncoderCopyTextureToBuffer(self, source, destination, copySize);
}
void gpuCommandEncoderCopyTextureToTexture(WGPUCommandEncoder self, const WGPUImageCopyTexture* source, const WGPUImageCopyTexture* destination, const WGPUExtent3D* copySize) {
    return wgpuCommandEncoderCopyTextureToTexture(self, source, destination, copySize);
}
"CommandEncoderClearBuffer"
typedef struct { 
   WGPUCommandEncoder Self;
   WGPUBuffer Buffer;
   uint64_t Offset;
   uint64_t Size;
} WGPUCommandEncoderClearBufferArgs;
void gpuCommandEncoderClearBuffer(const WGPUCommandEncoderClearBufferArgs* args) {
    return wgpuCommandEncoderClearBuffer(args->Self, args->Buffer, args->Offset, args->Size);
}
void gpuCommandEncoderInjectValidationError(WGPUCommandEncoder self, WGPUStringView message) {
    return wgpuCommandEncoderInjectValidationError(self, message);
}
void gpuCommandEncoderInsertDebugMarker(WGPUCommandEncoder self, WGPUStringView markerLabel) {
    return wgpuCommandEncoderInsertDebugMarker(self, markerLabel);
}
void gpuCommandEncoderPopDebugGroup(WGPUCommandEncoder self) {
    return wgpuCommandEncoderPopDebugGroup(self);
}
void gpuCommandEncoderPushDebugGroup(WGPUCommandEncoder self, WGPUStringView groupLabel) {
    return wgpuCommandEncoderPushDebugGroup(self, groupLabel);
}
"CommandEncoderResolveQuerySet"
typedef struct { 
   WGPUCommandEncoder Self;
   WGPUQuerySet QuerySet;
   uint32_t FirstQuery;
   uint32_t QueryCount;
   WGPUBuffer Destination;
   uint64_t DestinationOffset;
} WGPUCommandEncoderResolveQuerySetArgs;
void gpuCommandEncoderResolveQuerySet(const WGPUCommandEncoderResolveQuerySetArgs* args) {
    return wgpuCommandEncoderResolveQuerySet(args->Self, args->QuerySet, args->FirstQuery, args->QueryCount, args->Destination, args->DestinationOffset);
}
"CommandEncoderWriteBuffer"
typedef struct { 
   WGPUCommandEncoder Self;
   WGPUBuffer Buffer;
   uint64_t BufferOffset;
   const uint8_t* Data;
   uint64_t Size;
} WGPUCommandEncoderWriteBufferArgs;
void gpuCommandEncoderWriteBuffer(const WGPUCommandEncoderWriteBufferArgs* args) {
    return wgpuCommandEncoderWriteBuffer(args->Self, args->Buffer, args->BufferOffset, args->Data, args->Size);
}
void gpuCommandEncoderWriteTimestamp(WGPUCommandEncoder self, WGPUQuerySet querySet, uint32_t queryIndex) {
    return wgpuCommandEncoderWriteTimestamp(self, querySet, queryIndex);
}
void gpuCommandEncoderSetLabel(WGPUCommandEncoder self, WGPUStringView label) {
    return wgpuCommandEncoderSetLabel(self, label);
}
void gpuComputePassEncoderInsertDebugMarker(WGPUComputePassEncoder self, WGPUStringView markerLabel) {
    return wgpuComputePassEncoderInsertDebugMarker(self, markerLabel);
}
void gpuComputePassEncoderPopDebugGroup(WGPUComputePassEncoder self) {
    return wgpuComputePassEncoderPopDebugGroup(self);
}
void gpuComputePassEncoderPushDebugGroup(WGPUComputePassEncoder self, WGPUStringView groupLabel) {
    return wgpuComputePassEncoderPushDebugGroup(self, groupLabel);
}
void gpuComputePassEncoderSetPipeline(WGPUComputePassEncoder self, WGPUComputePipeline pipeline) {
    return wgpuComputePassEncoderSetPipeline(self, pipeline);
}
void gpuComputePassEncoderSetBindGroup(WGPUComputePassEncoder self, uint32_t groupIndex, WGPUBindGroup group, size_t dynamicOffsetCount, const uint32_t* dynamicOffsets) {
    return wgpuComputePassEncoderSetBindGroup(self, groupIndex, group, dynamicOffsetCount, dynamicOffsets);
}
void gpuComputePassEncoderWriteTimestamp(WGPUComputePassEncoder self, WGPUQuerySet querySet, uint32_t queryIndex) {
    return wgpuComputePassEncoderWriteTimestamp(self, querySet, queryIndex);
}
void gpuComputePassEncoderDispatchWorkgroups(WGPUComputePassEncoder self, uint32_t workgroupCountX, uint32_t workgroupCountY, uint32_t workgroupCountZ) {
    return wgpuComputePassEncoderDispatchWorkgroups(self, workgroupCountX, workgroupCountY, workgroupCountZ);
}
"ComputePassEncoderDispatchWorkgroupsIndirect"
typedef struct { 
   WGPUComputePassEncoder Self;
   WGPUBuffer IndirectBuffer;
   uint64_t IndirectOffset;
} WGPUComputePassEncoderDispatchWorkgroupsIndirectArgs;
void gpuComputePassEncoderDispatchWorkgroupsIndirect(const WGPUComputePassEncoderDispatchWorkgroupsIndirectArgs* args) {
    return wgpuComputePassEncoderDispatchWorkgroupsIndirect(args->Self, args->IndirectBuffer, args->IndirectOffset);
}
void gpuComputePassEncoderEnd(WGPUComputePassEncoder self) {
    return wgpuComputePassEncoderEnd(self);
}
void gpuComputePassEncoderSetLabel(WGPUComputePassEncoder self, WGPUStringView label) {
    return wgpuComputePassEncoderSetLabel(self, label);
}
WGPUBindGroupLayout gpuComputePipelineGetBindGroupLayout(WGPUComputePipeline self, uint32_t groupIndex) {
    return wgpuComputePipelineGetBindGroupLayout(self, groupIndex);
}
void gpuComputePipelineSetLabel(WGPUComputePipeline self, WGPUStringView label) {
    return wgpuComputePipelineSetLabel(self, label);
}
WGPUBindGroup gpuDeviceCreateBindGroup(WGPUDevice self, const WGPUBindGroupDescriptor* descriptor) {
    return wgpuDeviceCreateBindGroup(self, descriptor);
}
WGPUBindGroupLayout gpuDeviceCreateBindGroupLayout(WGPUDevice self, const WGPUBindGroupLayoutDescriptor* descriptor) {
    return wgpuDeviceCreateBindGroupLayout(self, descriptor);
}
WGPUBuffer gpuDeviceCreateBuffer(WGPUDevice self, const WGPUBufferDescriptor* descriptor) {
    return wgpuDeviceCreateBuffer(self, descriptor);
}
WGPUBuffer gpuDeviceCreateErrorBuffer(WGPUDevice self, const WGPUBufferDescriptor* descriptor) {
    return wgpuDeviceCreateErrorBuffer(self, descriptor);
}
WGPUCommandEncoder gpuDeviceCreateCommandEncoder(WGPUDevice self, const WGPUCommandEncoderDescriptor* descriptor) {
    return wgpuDeviceCreateCommandEncoder(self, descriptor);
}
WGPUComputePipeline gpuDeviceCreateComputePipeline(WGPUDevice self, const WGPUComputePipelineDescriptor* descriptor) {
    return wgpuDeviceCreateComputePipeline(self, descriptor);
}
void gpuDeviceCreateComputePipelineAsync(WGPUDevice self, const WGPUComputePipelineDescriptor* descriptor, WGPUCreateComputePipelineAsyncCallback callback, void * userdata) {
    return wgpuDeviceCreateComputePipelineAsync(self, descriptor, callback, userdata);
}
WGPUFuture gpuDeviceCreateComputePipelineAsyncF(WGPUDevice self, const WGPUComputePipelineDescriptor* descriptor, WGPUCreateComputePipelineAsyncCallbackInfo callbackInfo) {
    return wgpuDeviceCreateComputePipelineAsyncF(self, descriptor, callbackInfo);
}
WGPUFuture gpuDeviceCreateComputePipelineAsync2(WGPUDevice self, const WGPUComputePipelineDescriptor* descriptor, WGPUCreateComputePipelineAsyncCallbackInfo2 callbackInfo) {
    return wgpuDeviceCreateComputePipelineAsync2(self, descriptor, callbackInfo);
}
WGPUExternalTexture gpuDeviceCreateExternalTexture(WGPUDevice self, const WGPUExternalTextureDescriptor* externalTextureDescriptor) {
    return wgpuDeviceCreateExternalTexture(self, externalTextureDescriptor);
}
WGPUExternalTexture gpuDeviceCreateErrorExternalTexture(WGPUDevice self) {
    return wgpuDeviceCreateErrorExternalTexture(self);
}
WGPUPipelineLayout gpuDeviceCreatePipelineLayout(WGPUDevice self, const WGPUPipelineLayoutDescriptor* descriptor) {
    return wgpuDeviceCreatePipelineLayout(self, descriptor);
}
WGPUQuerySet gpuDeviceCreateQuerySet(WGPUDevice self, const WGPUQuerySetDescriptor* descriptor) {
    return wgpuDeviceCreateQuerySet(self, descriptor);
}
void gpuDeviceCreateRenderPipelineAsync(WGPUDevice self, const WGPURenderPipelineDescriptor* descriptor, WGPUCreateRenderPipelineAsyncCallback callback, void * userdata) {
    return wgpuDeviceCreateRenderPipelineAsync(self, descriptor, callback, userdata);
}
WGPUFuture gpuDeviceCreateRenderPipelineAsyncF(WGPUDevice self, const WGPURenderPipelineDescriptor* descriptor, WGPUCreateRenderPipelineAsyncCallbackInfo callbackInfo) {
    return wgpuDeviceCreateRenderPipelineAsyncF(self, descriptor, callbackInfo);
}
WGPUFuture gpuDeviceCreateRenderPipelineAsync2(WGPUDevice self, const WGPURenderPipelineDescriptor* descriptor, WGPUCreateRenderPipelineAsyncCallbackInfo2 callbackInfo) {
    return wgpuDeviceCreateRenderPipelineAsync2(self, descriptor, callbackInfo);
}
WGPURenderBundleEncoder gpuDeviceCreateRenderBundleEncoder(WGPUDevice self, const WGPURenderBundleEncoderDescriptor* descriptor) {
    return wgpuDeviceCreateRenderBundleEncoder(self, descriptor);
}
WGPURenderPipeline gpuDeviceCreateRenderPipeline(WGPUDevice self, const WGPURenderPipelineDescriptor* descriptor) {
    return wgpuDeviceCreateRenderPipeline(self, descriptor);
}
WGPUSampler gpuDeviceCreateSampler(WGPUDevice self, const WGPUSamplerDescriptor* descriptor) {
    return wgpuDeviceCreateSampler(self, descriptor);
}
WGPUShaderModule gpuDeviceCreateShaderModule(WGPUDevice self, const WGPUShaderModuleDescriptor* descriptor) {
    return wgpuDeviceCreateShaderModule(self, descriptor);
}
WGPUShaderModule gpuDeviceCreateErrorShaderModule(WGPUDevice self, const WGPUShaderModuleDescriptor* descriptor, WGPUStringView errorMessage) {
    return wgpuDeviceCreateErrorShaderModule(self, descriptor, errorMessage);
}
WGPUTexture gpuDeviceCreateTexture(WGPUDevice self, const WGPUTextureDescriptor* descriptor) {
    return wgpuDeviceCreateTexture(self, descriptor);
}
WGPUSharedBufferMemory gpuDeviceImportSharedBufferMemory(WGPUDevice self, const WGPUSharedBufferMemoryDescriptor* descriptor) {
    return wgpuDeviceImportSharedBufferMemory(self, descriptor);
}
WGPUSharedTextureMemory gpuDeviceImportSharedTextureMemory(WGPUDevice self, const WGPUSharedTextureMemoryDescriptor* descriptor) {
    return wgpuDeviceImportSharedTextureMemory(self, descriptor);
}
WGPUSharedFence gpuDeviceImportSharedFence(WGPUDevice self, const WGPUSharedFenceDescriptor* descriptor) {
    return wgpuDeviceImportSharedFence(self, descriptor);
}
WGPUTexture gpuDeviceCreateErrorTexture(WGPUDevice self, const WGPUTextureDescriptor* descriptor) {
    return wgpuDeviceCreateErrorTexture(self, descriptor);
}
void gpuDeviceDestroy(WGPUDevice self) {
    return wgpuDeviceDestroy(self);
}
int gpuDeviceGetAHardwareBufferProperties(WGPUDevice self, void * handle, WGPUAHardwareBufferProperties* properties) {
    return wgpuDeviceGetAHardwareBufferProperties(self, handle, properties);
}
int gpuDeviceGetLimits(WGPUDevice self, WGPUSupportedLimits* limits) {
    return wgpuDeviceGetLimits(self, limits);
}
WGPUFuture gpuDeviceGetLostFuture(WGPUDevice self) {
    return wgpuDeviceGetLostFuture(self);
}
bool gpuDeviceHasFeature(WGPUDevice self, int feature) {
    return wgpuDeviceHasFeature(self, feature);
}
size_t gpuDeviceEnumerateFeatures(WGPUDevice self, int* features) {
    return wgpuDeviceEnumerateFeatures(self, features);
}
void gpuDeviceGetFeatures(WGPUDevice self, WGPUSupportedFeatures* features) {
    return wgpuDeviceGetFeatures(self, features);
}
WGPUAdapter gpuDeviceGetAdapter(WGPUDevice self) {
    return wgpuDeviceGetAdapter(self);
}
WGPUQueue gpuDeviceGetQueue(WGPUDevice self) {
    return wgpuDeviceGetQueue(self);
}
void gpuDeviceInjectError(WGPUDevice self, int type, WGPUStringView message) {
    return wgpuDeviceInjectError(self, type, message);
}
void gpuDeviceForceLoss(WGPUDevice self, int type, WGPUStringView message) {
    return wgpuDeviceForceLoss(self, type, message);
}
void gpuDeviceTick(WGPUDevice self) {
    return wgpuDeviceTick(self);
}
void gpuDeviceSetUncapturedErrorCallback(WGPUDevice self, WGPUErrorCallback callback, void * userdata) {
    return wgpuDeviceSetUncapturedErrorCallback(self, callback, userdata);
}
void gpuDeviceSetLoggingCallback(WGPUDevice self, WGPULoggingCallback callback, void * userdata) {
    return wgpuDeviceSetLoggingCallback(self, callback, userdata);
}
void gpuDeviceSetDeviceLostCallback(WGPUDevice self, WGPUDeviceLostCallback callback, void * userdata) {
    return wgpuDeviceSetDeviceLostCallback(self, callback, userdata);
}
void gpuDevicePushErrorScope(WGPUDevice self, int filter) {
    return wgpuDevicePushErrorScope(self, filter);
}
void gpuDevicePopErrorScope(WGPUDevice self, WGPUErrorCallback oldCallback, void * userdata) {
    return wgpuDevicePopErrorScope(self, oldCallback, userdata);
}
WGPUFuture gpuDevicePopErrorScopeF(WGPUDevice self, WGPUPopErrorScopeCallbackInfo callbackInfo) {
    return wgpuDevicePopErrorScopeF(self, callbackInfo);
}
WGPUFuture gpuDevicePopErrorScope2(WGPUDevice self, WGPUPopErrorScopeCallbackInfo2 callbackInfo) {
    return wgpuDevicePopErrorScope2(self, callbackInfo);
}
void gpuDeviceSetLabel(WGPUDevice self, WGPUStringView label) {
    return wgpuDeviceSetLabel(self, label);
}
void gpuDeviceValidateTextureDescriptor(WGPUDevice self, const WGPUTextureDescriptor* descriptor) {
    return wgpuDeviceValidateTextureDescriptor(self, descriptor);
}
void gpuExternalTextureSetLabel(WGPUExternalTexture self, WGPUStringView label) {
    return wgpuExternalTextureSetLabel(self, label);
}
void gpuExternalTextureDestroy(WGPUExternalTexture self) {
    return wgpuExternalTextureDestroy(self);
}
void gpuExternalTextureExpire(WGPUExternalTexture self) {
    return wgpuExternalTextureExpire(self);
}
void gpuExternalTextureRefresh(WGPUExternalTexture self) {
    return wgpuExternalTextureRefresh(self);
}
void gpuSharedBufferMemorySetLabel(WGPUSharedBufferMemory self, WGPUStringView label) {
    return wgpuSharedBufferMemorySetLabel(self, label);
}
int gpuSharedBufferMemoryGetProperties(WGPUSharedBufferMemory self, WGPUSharedBufferMemoryProperties* properties) {
    return wgpuSharedBufferMemoryGetProperties(self, properties);
}
WGPUBuffer gpuSharedBufferMemoryCreateBuffer(WGPUSharedBufferMemory self, const WGPUBufferDescriptor* descriptor) {
    return wgpuSharedBufferMemoryCreateBuffer(self, descriptor);
}
int gpuSharedBufferMemoryBeginAccess(WGPUSharedBufferMemory self, WGPUBuffer buffer, const WGPUSharedBufferMemoryBeginAccessDescriptor* descriptor) {
    return wgpuSharedBufferMemoryBeginAccess(self, buffer, descriptor);
}
int gpuSharedBufferMemoryEndAccess(WGPUSharedBufferMemory self, WGPUBuffer buffer, WGPUSharedBufferMemoryEndAccessState* descriptor) {
    return wgpuSharedBufferMemoryEndAccess(self, buffer, descriptor);
}
bool gpuSharedBufferMemoryIsDeviceLost(WGPUSharedBufferMemory self) {
    return wgpuSharedBufferMemoryIsDeviceLost(self);
}
void gpuSharedTextureMemorySetLabel(WGPUSharedTextureMemory self, WGPUStringView label) {
    return wgpuSharedTextureMemorySetLabel(self, label);
}
int gpuSharedTextureMemoryGetProperties(WGPUSharedTextureMemory self, WGPUSharedTextureMemoryProperties* properties) {
    return wgpuSharedTextureMemoryGetProperties(self, properties);
}
WGPUTexture gpuSharedTextureMemoryCreateTexture(WGPUSharedTextureMemory self, const WGPUTextureDescriptor* descriptor) {
    return wgpuSharedTextureMemoryCreateTexture(self, descriptor);
}
int gpuSharedTextureMemoryBeginAccess(WGPUSharedTextureMemory self, WGPUTexture texture, const WGPUSharedTextureMemoryBeginAccessDescriptor* descriptor) {
    return wgpuSharedTextureMemoryBeginAccess(self, texture, descriptor);
}
int gpuSharedTextureMemoryEndAccess(WGPUSharedTextureMemory self, WGPUTexture texture, WGPUSharedTextureMemoryEndAccessState* descriptor) {
    return wgpuSharedTextureMemoryEndAccess(self, texture, descriptor);
}
bool gpuSharedTextureMemoryIsDeviceLost(WGPUSharedTextureMemory self) {
    return wgpuSharedTextureMemoryIsDeviceLost(self);
}
void gpuSharedFenceExportInfo(WGPUSharedFence self, WGPUSharedFenceExportInfo* info) {
    return wgpuSharedFenceExportInfo(self, info);
}
WGPUSurface gpuInstanceCreateSurface(WGPUInstance self, const WGPUSurfaceDescriptor* descriptor) {
    return wgpuInstanceCreateSurface(self, descriptor);
}
void gpuInstanceProcessEvents(WGPUInstance self) {
    return wgpuInstanceProcessEvents(self);
}
"InstanceWaitAny"
typedef struct { 
   WGPUInstance Self;
   size_t FutureCount;
   WGPUFutureWaitInfo* Futures;
   uint64_t TimeoutNS;
} WGPUInstanceWaitAnyArgs;
int gpuInstanceWaitAny(const WGPUInstanceWaitAnyArgs* args) {
    return wgpuInstanceWaitAny(args->Self, args->FutureCount, args->Futures, args->TimeoutNS);
}
void gpuInstanceRequestAdapter(WGPUInstance self, const WGPURequestAdapterOptions* options, WGPURequestAdapterCallback callback, void * userdata) {
    return wgpuInstanceRequestAdapter(self, options, callback, userdata);
}
WGPUFuture gpuInstanceRequestAdapterF(WGPUInstance self, const WGPURequestAdapterOptions* options, WGPURequestAdapterCallbackInfo callbackInfo) {
    return wgpuInstanceRequestAdapterF(self, options, callbackInfo);
}
WGPUFuture gpuInstanceRequestAdapter2(WGPUInstance self, const WGPURequestAdapterOptions* options, WGPURequestAdapterCallbackInfo2 callbackInfo) {
    return wgpuInstanceRequestAdapter2(self, options, callbackInfo);
}
bool gpuInstanceHasWGSLLanguageFeature(WGPUInstance self, int feature) {
    return wgpuInstanceHasWGSLLanguageFeature(self, feature);
}
size_t gpuInstanceEnumerateWGSLLanguageFeatures(WGPUInstance self, int* features) {
    return wgpuInstanceEnumerateWGSLLanguageFeatures(self, features);
}
void gpuPipelineLayoutSetLabel(WGPUPipelineLayout self, WGPUStringView label) {
    return wgpuPipelineLayoutSetLabel(self, label);
}
void gpuQuerySetSetLabel(WGPUQuerySet self, WGPUStringView label) {
    return wgpuQuerySetSetLabel(self, label);
}
int gpuQuerySetGetType(WGPUQuerySet self) {
    return wgpuQuerySetGetType(self);
}
uint32_t gpuQuerySetGetCount(WGPUQuerySet self) {
    return wgpuQuerySetGetCount(self);
}
void gpuQuerySetDestroy(WGPUQuerySet self) {
    return wgpuQuerySetDestroy(self);
}
void gpuQueueSubmit(WGPUQueue self, size_t commandCount, const WGPUCommandBuffer* commands) {
    return wgpuQueueSubmit(self, commandCount, commands);
}
void gpuQueueOnSubmittedWorkDone(WGPUQueue self, WGPUQueueWorkDoneCallback callback, void * userdata) {
    return wgpuQueueOnSubmittedWorkDone(self, callback, userdata);
}
WGPUFuture gpuQueueOnSubmittedWorkDoneF(WGPUQueue self, WGPUQueueWorkDoneCallbackInfo callbackInfo) {
    return wgpuQueueOnSubmittedWorkDoneF(self, callbackInfo);
}
WGPUFuture gpuQueueOnSubmittedWorkDone2(WGPUQueue self, WGPUQueueWorkDoneCallbackInfo2 callbackInfo) {
    return wgpuQueueOnSubmittedWorkDone2(self, callbackInfo);
}
"QueueWriteBuffer"
typedef struct { 
   WGPUQueue Self;
   WGPUBuffer Buffer;
   uint64_t BufferOffset;
   const void* Data;
   size_t Size;
} WGPUQueueWriteBufferArgs;
void gpuQueueWriteBuffer(const WGPUQueueWriteBufferArgs* args) {
    return wgpuQueueWriteBuffer(args->Self, args->Buffer, args->BufferOffset, args->Data, args->Size);
}
"QueueWriteTexture"
typedef struct { 
   WGPUQueue Self;
   const WGPUImageCopyTexture* Destination;
   const void* Data;
   size_t DataSize;
   const WGPUTextureDataLayout* DataLayout;
   const WGPUExtent3D* WriteSize;
} WGPUQueueWriteTextureArgs;
void gpuQueueWriteTexture(const WGPUQueueWriteTextureArgs* args) {
    return wgpuQueueWriteTexture(args->Self, args->Destination, args->Data, args->DataSize, args->DataLayout, args->WriteSize);
}
void gpuQueueCopyTextureForBrowser(WGPUQueue self, const WGPUImageCopyTexture* source, const WGPUImageCopyTexture* destination, const WGPUExtent3D* copySize, const WGPUCopyTextureForBrowserOptions* options) {
    return wgpuQueueCopyTextureForBrowser(self, source, destination, copySize, options);
}
void gpuQueueCopyExternalTextureForBrowser(WGPUQueue self, const WGPUImageCopyExternalTexture* source, const WGPUImageCopyTexture* destination, const WGPUExtent3D* copySize, const WGPUCopyTextureForBrowserOptions* options) {
    return wgpuQueueCopyExternalTextureForBrowser(self, source, destination, copySize, options);
}
void gpuQueueSetLabel(WGPUQueue self, WGPUStringView label) {
    return wgpuQueueSetLabel(self, label);
}
void gpuRenderBundleSetLabel(WGPURenderBundle self, WGPUStringView label) {
    return wgpuRenderBundleSetLabel(self, label);
}
void gpuRenderBundleEncoderSetPipeline(WGPURenderBundleEncoder self, WGPURenderPipeline pipeline) {
    return wgpuRenderBundleEncoderSetPipeline(self, pipeline);
}
void gpuRenderBundleEncoderSetBindGroup(WGPURenderBundleEncoder self, uint32_t groupIndex, WGPUBindGroup group, size_t dynamicOffsetCount, const uint32_t* dynamicOffsets) {
    return wgpuRenderBundleEncoderSetBindGroup(self, groupIndex, group, dynamicOffsetCount, dynamicOffsets);
}
void gpuRenderBundleEncoderDraw(WGPURenderBundleEncoder self, uint32_t vertexCount, uint32_t instanceCount, uint32_t firstVertex, uint32_t firstInstance) {
    return wgpuRenderBundleEncoderDraw(self, vertexCount, instanceCount, firstVertex, firstInstance);
}
"RenderBundleEncoderDrawIndexed"
typedef struct { 
   WGPURenderBundleEncoder Self;
   uint32_t IndexCount;
   uint32_t InstanceCount;
   uint32_t FirstIndex;
   int32_t BaseVertex;
   uint32_t FirstInstance;
} WGPURenderBundleEncoderDrawIndexedArgs;
void gpuRenderBundleEncoderDrawIndexed(const WGPURenderBundleEncoderDrawIndexedArgs* args) {
    return wgpuRenderBundleEncoderDrawIndexed(args->Self, args->IndexCount, args->InstanceCount, args->FirstIndex, args->BaseVertex, args->FirstInstance);
}
"RenderBundleEncoderDrawIndirect"
typedef struct { 
   WGPURenderBundleEncoder Self;
   WGPUBuffer IndirectBuffer;
   uint64_t IndirectOffset;
} WGPURenderBundleEncoderDrawIndirectArgs;
void gpuRenderBundleEncoderDrawIndirect(const WGPURenderBundleEncoderDrawIndirectArgs* args) {
    return wgpuRenderBundleEncoderDrawIndirect(args->Self, args->IndirectBuffer, args->IndirectOffset);
}
"RenderBundleEncoderDrawIndexedIndirect"
typedef struct { 
   WGPURenderBundleEncoder Self;
   WGPUBuffer IndirectBuffer;
   uint64_t IndirectOffset;
} WGPURenderBundleEncoderDrawIndexedIndirectArgs;
void gpuRenderBundleEncoderDrawIndexedIndirect(const WGPURenderBundleEncoderDrawIndexedIndirectArgs* args) {
    return wgpuRenderBundleEncoderDrawIndexedIndirect(args->Self, args->IndirectBuffer, args->IndirectOffset);
}
void gpuRenderBundleEncoderInsertDebugMarker(WGPURenderBundleEncoder self, WGPUStringView markerLabel) {
    return wgpuRenderBundleEncoderInsertDebugMarker(self, markerLabel);
}
void gpuRenderBundleEncoderPopDebugGroup(WGPURenderBundleEncoder self) {
    return wgpuRenderBundleEncoderPopDebugGroup(self);
}
void gpuRenderBundleEncoderPushDebugGroup(WGPURenderBundleEncoder self, WGPUStringView groupLabel) {
    return wgpuRenderBundleEncoderPushDebugGroup(self, groupLabel);
}
"RenderBundleEncoderSetVertexBuffer"
typedef struct { 
   WGPURenderBundleEncoder Self;
   uint32_t Slot;
   WGPUBuffer Buffer;
   uint64_t Offset;
   uint64_t Size;
} WGPURenderBundleEncoderSetVertexBufferArgs;
void gpuRenderBundleEncoderSetVertexBuffer(const WGPURenderBundleEncoderSetVertexBufferArgs* args) {
    return wgpuRenderBundleEncoderSetVertexBuffer(args->Self, args->Slot, args->Buffer, args->Offset, args->Size);
}
"RenderBundleEncoderSetIndexBuffer"
typedef struct { 
   WGPURenderBundleEncoder Self;
   WGPUBuffer Buffer;
   int Format;
   uint64_t Offset;
   uint64_t Size;
} WGPURenderBundleEncoderSetIndexBufferArgs;
void gpuRenderBundleEncoderSetIndexBuffer(const WGPURenderBundleEncoderSetIndexBufferArgs* args) {
    return wgpuRenderBundleEncoderSetIndexBuffer(args->Self, args->Buffer, args->Format, args->Offset, args->Size);
}
WGPURenderBundle gpuRenderBundleEncoderFinish(WGPURenderBundleEncoder self, const WGPURenderBundleDescriptor* descriptor) {
    return wgpuRenderBundleEncoderFinish(self, descriptor);
}
void gpuRenderBundleEncoderSetLabel(WGPURenderBundleEncoder self, WGPUStringView label) {
    return wgpuRenderBundleEncoderSetLabel(self, label);
}
void gpuRenderPassEncoderSetPipeline(WGPURenderPassEncoder self, WGPURenderPipeline pipeline) {
    return wgpuRenderPassEncoderSetPipeline(self, pipeline);
}
void gpuRenderPassEncoderSetBindGroup(WGPURenderPassEncoder self, uint32_t groupIndex, WGPUBindGroup group, size_t dynamicOffsetCount, const uint32_t* dynamicOffsets) {
    return wgpuRenderPassEncoderSetBindGroup(self, groupIndex, group, dynamicOffsetCount, dynamicOffsets);
}
void gpuRenderPassEncoderDraw(WGPURenderPassEncoder self, uint32_t vertexCount, uint32_t instanceCount, uint32_t firstVertex, uint32_t firstInstance) {
    return wgpuRenderPassEncoderDraw(self, vertexCount, instanceCount, firstVertex, firstInstance);
}
"RenderPassEncoderDrawIndexed"
typedef struct { 
   WGPURenderPassEncoder Self;
   uint32_t IndexCount;
   uint32_t InstanceCount;
   uint32_t FirstIndex;
   int32_t BaseVertex;
   uint32_t FirstInstance;
} WGPURenderPassEncoderDrawIndexedArgs;
void gpuRenderPassEncoderDrawIndexed(const WGPURenderPassEncoderDrawIndexedArgs* args) {
    return wgpuRenderPassEncoderDrawIndexed(args->Self, args->IndexCount, args->InstanceCount, args->FirstIndex, args->BaseVertex, args->FirstInstance);
}
"RenderPassEncoderDrawIndirect"
typedef struct { 
   WGPURenderPassEncoder Self;
   WGPUBuffer IndirectBuffer;
   uint64_t IndirectOffset;
} WGPURenderPassEncoderDrawIndirectArgs;
void gpuRenderPassEncoderDrawIndirect(const WGPURenderPassEncoderDrawIndirectArgs* args) {
    return wgpuRenderPassEncoderDrawIndirect(args->Self, args->IndirectBuffer, args->IndirectOffset);
}
"RenderPassEncoderDrawIndexedIndirect"
typedef struct { 
   WGPURenderPassEncoder Self;
   WGPUBuffer IndirectBuffer;
   uint64_t IndirectOffset;
} WGPURenderPassEncoderDrawIndexedIndirectArgs;
void gpuRenderPassEncoderDrawIndexedIndirect(const WGPURenderPassEncoderDrawIndexedIndirectArgs* args) {
    return wgpuRenderPassEncoderDrawIndexedIndirect(args->Self, args->IndirectBuffer, args->IndirectOffset);
}
"RenderPassEncoderMultiDrawIndirect"
typedef struct { 
   WGPURenderPassEncoder Self;
   WGPUBuffer IndirectBuffer;
   uint64_t IndirectOffset;
   uint32_t MaxDrawCount;
   WGPUBuffer DrawCountBuffer;
   uint64_t DrawCountBufferOffset;
} WGPURenderPassEncoderMultiDrawIndirectArgs;
void gpuRenderPassEncoderMultiDrawIndirect(const WGPURenderPassEncoderMultiDrawIndirectArgs* args) {
    return wgpuRenderPassEncoderMultiDrawIndirect(args->Self, args->IndirectBuffer, args->IndirectOffset, args->MaxDrawCount, args->DrawCountBuffer, args->DrawCountBufferOffset);
}
"RenderPassEncoderMultiDrawIndexedIndirect"
typedef struct { 
   WGPURenderPassEncoder Self;
   WGPUBuffer IndirectBuffer;
   uint64_t IndirectOffset;
   uint32_t MaxDrawCount;
   WGPUBuffer DrawCountBuffer;
   uint64_t DrawCountBufferOffset;
} WGPURenderPassEncoderMultiDrawIndexedIndirectArgs;
void gpuRenderPassEncoderMultiDrawIndexedIndirect(const WGPURenderPassEncoderMultiDrawIndexedIndirectArgs* args) {
    return wgpuRenderPassEncoderMultiDrawIndexedIndirect(args->Self, args->IndirectBuffer, args->IndirectOffset, args->MaxDrawCount, args->DrawCountBuffer, args->DrawCountBufferOffset);
}
void gpuRenderPassEncoderExecuteBundles(WGPURenderPassEncoder self, size_t bundleCount, const WGPURenderBundle* bundles) {
    return wgpuRenderPassEncoderExecuteBundles(self, bundleCount, bundles);
}
void gpuRenderPassEncoderInsertDebugMarker(WGPURenderPassEncoder self, WGPUStringView markerLabel) {
    return wgpuRenderPassEncoderInsertDebugMarker(self, markerLabel);
}
void gpuRenderPassEncoderPopDebugGroup(WGPURenderPassEncoder self) {
    return wgpuRenderPassEncoderPopDebugGroup(self);
}
void gpuRenderPassEncoderPushDebugGroup(WGPURenderPassEncoder self, WGPUStringView groupLabel) {
    return wgpuRenderPassEncoderPushDebugGroup(self, groupLabel);
}
void gpuRenderPassEncoderSetStencilReference(WGPURenderPassEncoder self, uint32_t reference) {
    return wgpuRenderPassEncoderSetStencilReference(self, reference);
}
void gpuRenderPassEncoderSetBlendConstant(WGPURenderPassEncoder self, const WGPUColor* color) {
    return wgpuRenderPassEncoderSetBlendConstant(self, color);
}
"RenderPassEncoderSetViewport"
typedef struct { 
   WGPURenderPassEncoder Self;
   float X;
   float Y;
   float Width;
   float Height;
   float MinDepth;
   float MaxDepth;
} WGPURenderPassEncoderSetViewportArgs;
void gpuRenderPassEncoderSetViewport(const WGPURenderPassEncoderSetViewportArgs* args) {
    return wgpuRenderPassEncoderSetViewport(args->Self, args->X, args->Y, args->Width, args->Height, args->MinDepth, args->MaxDepth);
}
void gpuRenderPassEncoderSetScissorRect(WGPURenderPassEncoder self, uint32_t x, uint32_t y, uint32_t width, uint32_t height) {
    return wgpuRenderPassEncoderSetScissorRect(self, x, y, width, height);
}
"RenderPassEncoderSetVertexBuffer"
typedef struct { 
   WGPURenderPassEncoder Self;
   uint32_t Slot;
   WGPUBuffer Buffer;
   uint64_t Offset;
   uint64_t Size;
} WGPURenderPassEncoderSetVertexBufferArgs;
void gpuRenderPassEncoderSetVertexBuffer(const WGPURenderPassEncoderSetVertexBufferArgs* args) {
    return wgpuRenderPassEncoderSetVertexBuffer(args->Self, args->Slot, args->Buffer, args->Offset, args->Size);
}
"RenderPassEncoderSetIndexBuffer"
typedef struct { 
   WGPURenderPassEncoder Self;
   WGPUBuffer Buffer;
   int Format;
   uint64_t Offset;
   uint64_t Size;
} WGPURenderPassEncoderSetIndexBufferArgs;
void gpuRenderPassEncoderSetIndexBuffer(const WGPURenderPassEncoderSetIndexBufferArgs* args) {
    return wgpuRenderPassEncoderSetIndexBuffer(args->Self, args->Buffer, args->Format, args->Offset, args->Size);
}
void gpuRenderPassEncoderBeginOcclusionQuery(WGPURenderPassEncoder self, uint32_t queryIndex) {
    return wgpuRenderPassEncoderBeginOcclusionQuery(self, queryIndex);
}
void gpuRenderPassEncoderEndOcclusionQuery(WGPURenderPassEncoder self) {
    return wgpuRenderPassEncoderEndOcclusionQuery(self);
}
void gpuRenderPassEncoderWriteTimestamp(WGPURenderPassEncoder self, WGPUQuerySet querySet, uint32_t queryIndex) {
    return wgpuRenderPassEncoderWriteTimestamp(self, querySet, queryIndex);
}
void gpuRenderPassEncoderPixelLocalStorageBarrier(WGPURenderPassEncoder self) {
    return wgpuRenderPassEncoderPixelLocalStorageBarrier(self);
}
void gpuRenderPassEncoderEnd(WGPURenderPassEncoder self) {
    return wgpuRenderPassEncoderEnd(self);
}
void gpuRenderPassEncoderSetLabel(WGPURenderPassEncoder self, WGPUStringView label) {
    return wgpuRenderPassEncoderSetLabel(self, label);
}
WGPUBindGroupLayout gpuRenderPipelineGetBindGroupLayout(WGPURenderPipeline self, uint32_t groupIndex) {
    return wgpuRenderPipelineGetBindGroupLayout(self, groupIndex);
}
void gpuRenderPipelineSetLabel(WGPURenderPipeline self, WGPUStringView label) {
    return wgpuRenderPipelineSetLabel(self, label);
}
void gpuSamplerSetLabel(WGPUSampler self, WGPUStringView label) {
    return wgpuSamplerSetLabel(self, label);
}
void gpuShaderModuleGetCompilationInfo(WGPUShaderModule self, WGPUCompilationInfoCallback callback, void * userdata) {
    return wgpuShaderModuleGetCompilationInfo(self, callback, userdata);
}
WGPUFuture gpuShaderModuleGetCompilationInfoF(WGPUShaderModule self, WGPUCompilationInfoCallbackInfo callbackInfo) {
    return wgpuShaderModuleGetCompilationInfoF(self, callbackInfo);
}
WGPUFuture gpuShaderModuleGetCompilationInfo2(WGPUShaderModule self, WGPUCompilationInfoCallbackInfo2 callbackInfo) {
    return wgpuShaderModuleGetCompilationInfo2(self, callbackInfo);
}
void gpuShaderModuleSetLabel(WGPUShaderModule self, WGPUStringView label) {
    return wgpuShaderModuleSetLabel(self, label);
}
void gpuSurfaceConfigure(WGPUSurface self, const WGPUSurfaceConfiguration* config) {
    return wgpuSurfaceConfigure(self, config);
}
int gpuSurfaceGetCapabilities(WGPUSurface self, WGPUAdapter adapter, WGPUSurfaceCapabilities* capabilities) {
    return wgpuSurfaceGetCapabilities(self, adapter, capabilities);
}
void gpuSurfaceGetCurrentTexture(WGPUSurface self, WGPUSurfaceTexture* surfaceTexture) {
    return wgpuSurfaceGetCurrentTexture(self, surfaceTexture);
}
void gpuSurfacePresent(WGPUSurface self) {
    return wgpuSurfacePresent(self);
}
void gpuSurfaceUnconfigure(WGPUSurface self) {
    return wgpuSurfaceUnconfigure(self);
}
void gpuSurfaceSetLabel(WGPUSurface self, WGPUStringView label) {
    return wgpuSurfaceSetLabel(self, label);
}
WGPUTextureView gpuTextureCreateView(WGPUTexture self, const WGPUTextureViewDescriptor* descriptor) {
    return wgpuTextureCreateView(self, descriptor);
}
WGPUTextureView gpuTextureCreateErrorView(WGPUTexture self, const WGPUTextureViewDescriptor* descriptor) {
    return wgpuTextureCreateErrorView(self, descriptor);
}
void gpuTextureSetLabel(WGPUTexture self, WGPUStringView label) {
    return wgpuTextureSetLabel(self, label);
}
uint32_t gpuTextureGetWidth(WGPUTexture self) {
    return wgpuTextureGetWidth(self);
}
uint32_t gpuTextureGetHeight(WGPUTexture self) {
    return wgpuTextureGetHeight(self);
}
uint32_t gpuTextureGetDepthOrArrayLayers(WGPUTexture self) {
    return wgpuTextureGetDepthOrArrayLayers(self);
}
uint32_t gpuTextureGetMipLevelCount(WGPUTexture self) {
    return wgpuTextureGetMipLevelCount(self);
}
uint32_t gpuTextureGetSampleCount(WGPUTexture self) {
    return wgpuTextureGetSampleCount(self);
}
int gpuTextureGetDimension(WGPUTexture self) {
    return wgpuTextureGetDimension(self);
}
int gpuTextureGetFormat(WGPUTexture self) {
    return wgpuTextureGetFormat(self);
}
int gpuTextureGetUsage(WGPUTexture self) {
    return wgpuTextureGetUsage(self);
}
void gpuTextureDestroy(WGPUTexture self) {
    return wgpuTextureDestroy(self);
}
void gpuTextureViewSetLabel(WGPUTextureView self, WGPUStringView label) {
    return wgpuTextureViewSetLabel(self, label);
}
