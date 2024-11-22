#include <emscripten.h>
#include <emscripten/html5.h>
#include <SDL/SDL_image.h>
typedef void* WGPUExternalTexture;
#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include "webgpu.h"
 
EMSCRIPTEN_KEEPALIVE WGPUInstance gpuCreateInstance(const WGPUInstanceDescriptor* descriptor) {
    return wgpuCreateInstance(descriptor);
}
EMSCRIPTEN_KEEPALIVE WGPUProc gpuGetProcAddress(WGPUStringView procName) {
    return wgpuGetProcAddress(procName);
}
EMSCRIPTEN_KEEPALIVE WGPUStatus gpuAdapterGetLimits(WGPUAdapter self, WGPUSupportedLimits* limits) {
    return wgpuAdapterGetLimits(self, limits);
}
EMSCRIPTEN_KEEPALIVE WGPUStatus gpuAdapterGetInfo(WGPUAdapter self, WGPUAdapterInfo* info) {
    return wgpuAdapterGetInfo(self, info);
}
EMSCRIPTEN_KEEPALIVE bool gpuAdapterHasFeature(WGPUAdapter self, WGPUFeatureName feature) {
    return wgpuAdapterHasFeature(self, feature);
}
EMSCRIPTEN_KEEPALIVE void gpuAdapterGetFeatures(WGPUAdapter self, WGPUSupportedFeatures* features) {
    return wgpuAdapterGetFeatures(self, features);
}
EMSCRIPTEN_KEEPALIVE void gpuAdapterRequestDevice(WGPUAdapter self, const WGPUDeviceDescriptor* descriptor, WGPURequestDeviceCallback callback, void * userdata) {
    return wgpuAdapterRequestDevice(self, descriptor, callback, userdata);
}
EMSCRIPTEN_KEEPALIVE WGPUFuture gpuAdapterRequestDeviceF(WGPUAdapter self, const WGPUDeviceDescriptor* options, WGPURequestDeviceCallbackInfo callbackInfo) {
    return wgpuAdapterRequestDeviceF(self, options, callbackInfo);
}
EMSCRIPTEN_KEEPALIVE WGPUFuture gpuAdapterRequestDevice2(WGPUAdapter self, const WGPUDeviceDescriptor* options, WGPURequestDeviceCallbackInfo2 callbackInfo) {
    return wgpuAdapterRequestDevice2(self, options, callbackInfo);
}
EMSCRIPTEN_KEEPALIVE void gpuAdapterRelease(WGPUAdapter self) {
    return wgpuAdapterRelease(self);
}
EMSCRIPTEN_KEEPALIVE void gpuBindGroupSetLabel(WGPUBindGroup self, WGPUStringView label) {
    return wgpuBindGroupSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuBindGroupRelease(WGPUBindGroup self) {
    return wgpuBindGroupRelease(self);
}
EMSCRIPTEN_KEEPALIVE void gpuBindGroupLayoutSetLabel(WGPUBindGroupLayout self, WGPUStringView label) {
    return wgpuBindGroupLayoutSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuBindGroupLayoutRelease(WGPUBindGroupLayout self) {
    return wgpuBindGroupLayoutRelease(self);
}
typedef struct { 
   WGPUBuffer Self;
   WGPUMapMode Mode;
   size_t Offset;
   size_t Size;
   WGPUBufferMapCallback Callback;
   void * Userdata;
} WGPUBufferMapAsyncArgs;
EMSCRIPTEN_KEEPALIVE void gpuBufferMapAsync(const WGPUBufferMapAsyncArgs* args) {
    return wgpuBufferMapAsync(args->Self, args->Mode, args->Offset, args->Size, args->Callback, args->Userdata);
}
EMSCRIPTEN_KEEPALIVE WGPUFuture gpuBufferMapAsyncF(WGPUBuffer self, WGPUMapMode mode, size_t offset, size_t size, WGPUBufferMapCallbackInfo callbackInfo) {
    return wgpuBufferMapAsyncF(self, mode, offset, size, callbackInfo);
}
EMSCRIPTEN_KEEPALIVE WGPUFuture gpuBufferMapAsync2(WGPUBuffer self, WGPUMapMode mode, size_t offset, size_t size, WGPUBufferMapCallbackInfo2 callbackInfo) {
    return wgpuBufferMapAsync2(self, mode, offset, size, callbackInfo);
}
EMSCRIPTEN_KEEPALIVE void * gpuBufferGetMappedRange(WGPUBuffer self, size_t offset, size_t size) {
    return wgpuBufferGetMappedRange(self, offset, size);
}
EMSCRIPTEN_KEEPALIVE void const * gpuBufferGetConstMappedRange(WGPUBuffer self, size_t offset, size_t size) {
    return wgpuBufferGetConstMappedRange(self, offset, size);
}
EMSCRIPTEN_KEEPALIVE void gpuBufferSetLabel(WGPUBuffer self, WGPUStringView label) {
    return wgpuBufferSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE WGPUBufferUsage gpuBufferGetUsage(WGPUBuffer self) {
    return wgpuBufferGetUsage(self);
}
EMSCRIPTEN_KEEPALIVE uint64_t gpuBufferGetSize(WGPUBuffer self) {
    return wgpuBufferGetSize(self);
}
EMSCRIPTEN_KEEPALIVE WGPUBufferMapState gpuBufferGetMapState(WGPUBuffer self) {
    return wgpuBufferGetMapState(self);
}
EMSCRIPTEN_KEEPALIVE void gpuBufferUnmap(WGPUBuffer self) {
    return wgpuBufferUnmap(self);
}
EMSCRIPTEN_KEEPALIVE void gpuBufferDestroy(WGPUBuffer self) {
    return wgpuBufferDestroy(self);
}
EMSCRIPTEN_KEEPALIVE void gpuBufferRelease(WGPUBuffer self) {
    return wgpuBufferRelease(self);
}
EMSCRIPTEN_KEEPALIVE void gpuCommandBufferSetLabel(WGPUCommandBuffer self, WGPUStringView label) {
    return wgpuCommandBufferSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuCommandBufferRelease(WGPUCommandBuffer self) {
    return wgpuCommandBufferRelease(self);
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
typedef struct { 
   WGPUCommandEncoder Self;
   WGPUBuffer Buffer;
   uint64_t Offset;
   uint64_t Size;
} WGPUCommandEncoderClearBufferArgs;
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderClearBuffer(const WGPUCommandEncoderClearBufferArgs* args) {
    return wgpuCommandEncoderClearBuffer(args->Self, args->Buffer, args->Offset, args->Size);
}
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderInsertDebugMarker(WGPUCommandEncoder self, WGPUStringView markerLabel) {
    return wgpuCommandEncoderInsertDebugMarker(self, markerLabel);
}
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderPopDebugGroup(WGPUCommandEncoder self) {
    return wgpuCommandEncoderPopDebugGroup(self);
}
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderPushDebugGroup(WGPUCommandEncoder self, WGPUStringView groupLabel) {
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
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderWriteTimestamp(WGPUCommandEncoder self, WGPUQuerySet querySet, uint32_t queryIndex) {
    return wgpuCommandEncoderWriteTimestamp(self, querySet, queryIndex);
}
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderSetLabel(WGPUCommandEncoder self, WGPUStringView label) {
    return wgpuCommandEncoderSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuCommandEncoderRelease(WGPUCommandEncoder self) {
    return wgpuCommandEncoderRelease(self);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderInsertDebugMarker(WGPUComputePassEncoder self, WGPUStringView markerLabel) {
    return wgpuComputePassEncoderInsertDebugMarker(self, markerLabel);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderPopDebugGroup(WGPUComputePassEncoder self) {
    return wgpuComputePassEncoderPopDebugGroup(self);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderPushDebugGroup(WGPUComputePassEncoder self, WGPUStringView groupLabel) {
    return wgpuComputePassEncoderPushDebugGroup(self, groupLabel);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderSetPipeline(WGPUComputePassEncoder self, WGPUComputePipeline pipeline) {
    return wgpuComputePassEncoderSetPipeline(self, pipeline);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderSetBindGroup(WGPUComputePassEncoder self, uint32_t groupIndex, WGPUBindGroup group, size_t dynamicOffsetCount, const uint32_t* dynamicOffsets) {
    return wgpuComputePassEncoderSetBindGroup(self, groupIndex, group, dynamicOffsetCount, dynamicOffsets);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderWriteTimestamp(WGPUComputePassEncoder self, WGPUQuerySet querySet, uint32_t queryIndex) {
    return wgpuComputePassEncoderWriteTimestamp(self, querySet, queryIndex);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderDispatchWorkgroups(WGPUComputePassEncoder self, uint32_t workgroupCountX, uint32_t workgroupCountY, uint32_t workgroupCountZ) {
    return wgpuComputePassEncoderDispatchWorkgroups(self, workgroupCountX, workgroupCountY, workgroupCountZ);
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
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderSetLabel(WGPUComputePassEncoder self, WGPUStringView label) {
    return wgpuComputePassEncoderSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePassEncoderRelease(WGPUComputePassEncoder self) {
    return wgpuComputePassEncoderRelease(self);
}
EMSCRIPTEN_KEEPALIVE WGPUBindGroupLayout gpuComputePipelineGetBindGroupLayout(WGPUComputePipeline self, uint32_t groupIndex) {
    return wgpuComputePipelineGetBindGroupLayout(self, groupIndex);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePipelineSetLabel(WGPUComputePipeline self, WGPUStringView label) {
    return wgpuComputePipelineSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuComputePipelineRelease(WGPUComputePipeline self) {
    return wgpuComputePipelineRelease(self);
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
EMSCRIPTEN_KEEPALIVE WGPUCommandEncoder gpuDeviceCreateCommandEncoder(WGPUDevice self, const WGPUCommandEncoderDescriptor* descriptor) {
    return wgpuDeviceCreateCommandEncoder(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE WGPUComputePipeline gpuDeviceCreateComputePipeline(WGPUDevice self, const WGPUComputePipelineDescriptor* descriptor) {
    return wgpuDeviceCreateComputePipeline(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE void gpuDeviceCreateComputePipelineAsync(WGPUDevice self, const WGPUComputePipelineDescriptor* descriptor, WGPUCreateComputePipelineAsyncCallback callback, void * userdata) {
    return wgpuDeviceCreateComputePipelineAsync(self, descriptor, callback, userdata);
}
EMSCRIPTEN_KEEPALIVE WGPUFuture gpuDeviceCreateComputePipelineAsyncF(WGPUDevice self, const WGPUComputePipelineDescriptor* descriptor, WGPUCreateComputePipelineAsyncCallbackInfo callbackInfo) {
    return wgpuDeviceCreateComputePipelineAsyncF(self, descriptor, callbackInfo);
}
EMSCRIPTEN_KEEPALIVE WGPUFuture gpuDeviceCreateComputePipelineAsync2(WGPUDevice self, const WGPUComputePipelineDescriptor* descriptor, WGPUCreateComputePipelineAsyncCallbackInfo2 callbackInfo) {
    return wgpuDeviceCreateComputePipelineAsync2(self, descriptor, callbackInfo);
}
EMSCRIPTEN_KEEPALIVE WGPUPipelineLayout gpuDeviceCreatePipelineLayout(WGPUDevice self, const WGPUPipelineLayoutDescriptor* descriptor) {
    return wgpuDeviceCreatePipelineLayout(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE WGPUQuerySet gpuDeviceCreateQuerySet(WGPUDevice self, const WGPUQuerySetDescriptor* descriptor) {
    return wgpuDeviceCreateQuerySet(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE void gpuDeviceCreateRenderPipelineAsync(WGPUDevice self, const WGPURenderPipelineDescriptor* descriptor, WGPUCreateRenderPipelineAsyncCallback callback, void * userdata) {
    return wgpuDeviceCreateRenderPipelineAsync(self, descriptor, callback, userdata);
}
EMSCRIPTEN_KEEPALIVE WGPUFuture gpuDeviceCreateRenderPipelineAsyncF(WGPUDevice self, const WGPURenderPipelineDescriptor* descriptor, WGPUCreateRenderPipelineAsyncCallbackInfo callbackInfo) {
    return wgpuDeviceCreateRenderPipelineAsyncF(self, descriptor, callbackInfo);
}
EMSCRIPTEN_KEEPALIVE WGPUFuture gpuDeviceCreateRenderPipelineAsync2(WGPUDevice self, const WGPURenderPipelineDescriptor* descriptor, WGPUCreateRenderPipelineAsyncCallbackInfo2 callbackInfo) {
    return wgpuDeviceCreateRenderPipelineAsync2(self, descriptor, callbackInfo);
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
EMSCRIPTEN_KEEPALIVE WGPUTexture gpuDeviceCreateTexture(WGPUDevice self, const WGPUTextureDescriptor* descriptor) {
    return wgpuDeviceCreateTexture(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE void gpuDeviceDestroy(WGPUDevice self) {
    return wgpuDeviceDestroy(self);
}
EMSCRIPTEN_KEEPALIVE WGPUStatus gpuDeviceGetLimits(WGPUDevice self, WGPUSupportedLimits* limits) {
    return wgpuDeviceGetLimits(self, limits);
}
EMSCRIPTEN_KEEPALIVE WGPUFuture gpuDeviceGetLostFuture(WGPUDevice self) {
    return wgpuDeviceGetLostFuture(self);
}
EMSCRIPTEN_KEEPALIVE bool gpuDeviceHasFeature(WGPUDevice self, WGPUFeatureName feature) {
    return wgpuDeviceHasFeature(self, feature);
}
EMSCRIPTEN_KEEPALIVE void gpuDeviceGetFeatures(WGPUDevice self, WGPUSupportedFeatures* features) {
    return wgpuDeviceGetFeatures(self, features);
}
EMSCRIPTEN_KEEPALIVE WGPUStatus gpuDeviceGetAdapterInfo(WGPUDevice self, WGPUAdapterInfo* adapterInfo) {
    return wgpuDeviceGetAdapterInfo(self, adapterInfo);
}
EMSCRIPTEN_KEEPALIVE WGPUQueue gpuDeviceGetQueue(WGPUDevice self) {
    return wgpuDeviceGetQueue(self);
}
EMSCRIPTEN_KEEPALIVE void gpuDeviceSetUncapturedErrorCallback(WGPUDevice self, WGPUErrorCallback callback, void * userdata) {
    return wgpuDeviceSetUncapturedErrorCallback(self, callback, userdata);
}
EMSCRIPTEN_KEEPALIVE void gpuDevicePushErrorScope(WGPUDevice self, WGPUErrorFilter filter) {
    return wgpuDevicePushErrorScope(self, filter);
}
EMSCRIPTEN_KEEPALIVE void gpuDevicePopErrorScope(WGPUDevice self, WGPUErrorCallback oldCallback, void * userdata) {
    return wgpuDevicePopErrorScope(self, oldCallback, userdata);
}
EMSCRIPTEN_KEEPALIVE WGPUFuture gpuDevicePopErrorScopeF(WGPUDevice self, WGPUPopErrorScopeCallbackInfo callbackInfo) {
    return wgpuDevicePopErrorScopeF(self, callbackInfo);
}
EMSCRIPTEN_KEEPALIVE WGPUFuture gpuDevicePopErrorScope2(WGPUDevice self, WGPUPopErrorScopeCallbackInfo2 callbackInfo) {
    return wgpuDevicePopErrorScope2(self, callbackInfo);
}
EMSCRIPTEN_KEEPALIVE void gpuDeviceSetLabel(WGPUDevice self, WGPUStringView label) {
    return wgpuDeviceSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuDeviceRelease(WGPUDevice self) {
    return wgpuDeviceRelease(self);
}
EMSCRIPTEN_KEEPALIVE WGPUSurface gpuInstanceCreateSurface(WGPUInstance self, const WGPUSurfaceDescriptor* descriptor) {
    return wgpuInstanceCreateSurface(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE void gpuInstanceProcessEvents(WGPUInstance self) {
    return wgpuInstanceProcessEvents(self);
}
typedef struct { 
   WGPUInstance Self;
   size_t FutureCount;
   WGPUFutureWaitInfo* Futures;
   uint64_t TimeoutNS;
} WGPUInstanceWaitAnyArgs;
EMSCRIPTEN_KEEPALIVE WGPUWaitStatus gpuInstanceWaitAny(const WGPUInstanceWaitAnyArgs* args) {
    return wgpuInstanceWaitAny(args->Self, args->FutureCount, args->Futures, args->TimeoutNS);
}
EMSCRIPTEN_KEEPALIVE void gpuInstanceRequestAdapter(WGPUInstance self, const WGPURequestAdapterOptions* options, WGPURequestAdapterCallback callback, void * userdata) {
    return wgpuInstanceRequestAdapter(self, options, callback, userdata);
}
EMSCRIPTEN_KEEPALIVE WGPUFuture gpuInstanceRequestAdapterF(WGPUInstance self, const WGPURequestAdapterOptions* options, WGPURequestAdapterCallbackInfo callbackInfo) {
    return wgpuInstanceRequestAdapterF(self, options, callbackInfo);
}
EMSCRIPTEN_KEEPALIVE WGPUFuture gpuInstanceRequestAdapter2(WGPUInstance self, const WGPURequestAdapterOptions* options, WGPURequestAdapterCallbackInfo2 callbackInfo) {
    return wgpuInstanceRequestAdapter2(self, options, callbackInfo);
}
EMSCRIPTEN_KEEPALIVE bool gpuInstanceHasWGSLLanguageFeature(WGPUInstance self, WGPUWGSLFeatureName feature) {
    return wgpuInstanceHasWGSLLanguageFeature(self, feature);
}
EMSCRIPTEN_KEEPALIVE size_t gpuInstanceEnumerateWGSLLanguageFeatures(WGPUInstance self, WGPUWGSLFeatureName* features) {
    return wgpuInstanceEnumerateWGSLLanguageFeatures(self, features);
}
EMSCRIPTEN_KEEPALIVE void gpuInstanceRelease(WGPUInstance self) {
    return wgpuInstanceRelease(self);
}
EMSCRIPTEN_KEEPALIVE WGPUStatus gpuGetInstanceFeatures(WGPUInstanceFeatures* features) {
    return wgpuGetInstanceFeatures(features);
}
EMSCRIPTEN_KEEPALIVE void gpuPipelineLayoutSetLabel(WGPUPipelineLayout self, WGPUStringView label) {
    return wgpuPipelineLayoutSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuPipelineLayoutRelease(WGPUPipelineLayout self) {
    return wgpuPipelineLayoutRelease(self);
}
EMSCRIPTEN_KEEPALIVE void gpuQuerySetSetLabel(WGPUQuerySet self, WGPUStringView label) {
    return wgpuQuerySetSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE WGPUQueryType gpuQuerySetGetType(WGPUQuerySet self) {
    return wgpuQuerySetGetType(self);
}
EMSCRIPTEN_KEEPALIVE uint32_t gpuQuerySetGetCount(WGPUQuerySet self) {
    return wgpuQuerySetGetCount(self);
}
EMSCRIPTEN_KEEPALIVE void gpuQuerySetDestroy(WGPUQuerySet self) {
    return wgpuQuerySetDestroy(self);
}
EMSCRIPTEN_KEEPALIVE void gpuQuerySetRelease(WGPUQuerySet self) {
    return wgpuQuerySetRelease(self);
}
EMSCRIPTEN_KEEPALIVE void gpuQueueSubmit(WGPUQueue self, size_t commandCount, const WGPUCommandBuffer* commands) {
    return wgpuQueueSubmit(self, commandCount, commands);
}
EMSCRIPTEN_KEEPALIVE WGPUFuture gpuQueueOnSubmittedWorkDoneF(WGPUQueue self, WGPUQueueWorkDoneCallbackInfo callbackInfo) {
    return wgpuQueueOnSubmittedWorkDoneF(self, callbackInfo);
}
EMSCRIPTEN_KEEPALIVE WGPUFuture gpuQueueOnSubmittedWorkDone2(WGPUQueue self, WGPUQueueWorkDoneCallbackInfo2 callbackInfo) {
    return wgpuQueueOnSubmittedWorkDone2(self, callbackInfo);
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
EMSCRIPTEN_KEEPALIVE void gpuQueueSetLabel(WGPUQueue self, WGPUStringView label) {
    return wgpuQueueSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuQueueRelease(WGPUQueue self) {
    return wgpuQueueRelease(self);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleSetLabel(WGPURenderBundle self, WGPUStringView label) {
    return wgpuRenderBundleSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleRelease(WGPURenderBundle self) {
    return wgpuRenderBundleRelease(self);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderSetPipeline(WGPURenderBundleEncoder self, WGPURenderPipeline pipeline) {
    return wgpuRenderBundleEncoderSetPipeline(self, pipeline);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderSetBindGroup(WGPURenderBundleEncoder self, uint32_t groupIndex, WGPUBindGroup group, size_t dynamicOffsetCount, const uint32_t* dynamicOffsets) {
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
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderInsertDebugMarker(WGPURenderBundleEncoder self, WGPUStringView markerLabel) {
    return wgpuRenderBundleEncoderInsertDebugMarker(self, markerLabel);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderPopDebugGroup(WGPURenderBundleEncoder self) {
    return wgpuRenderBundleEncoderPopDebugGroup(self);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderPushDebugGroup(WGPURenderBundleEncoder self, WGPUStringView groupLabel) {
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
   WGPUIndexFormat Format;
   uint64_t Offset;
   uint64_t Size;
} WGPURenderBundleEncoderSetIndexBufferArgs;
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderSetIndexBuffer(const WGPURenderBundleEncoderSetIndexBufferArgs* args) {
    return wgpuRenderBundleEncoderSetIndexBuffer(args->Self, args->Buffer, args->Format, args->Offset, args->Size);
}
EMSCRIPTEN_KEEPALIVE WGPURenderBundle gpuRenderBundleEncoderFinish(WGPURenderBundleEncoder self, const WGPURenderBundleDescriptor* descriptor) {
    return wgpuRenderBundleEncoderFinish(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderSetLabel(WGPURenderBundleEncoder self, WGPUStringView label) {
    return wgpuRenderBundleEncoderSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderBundleEncoderRelease(WGPURenderBundleEncoder self) {
    return wgpuRenderBundleEncoderRelease(self);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderSetPipeline(WGPURenderPassEncoder self, WGPURenderPipeline pipeline) {
    return wgpuRenderPassEncoderSetPipeline(self, pipeline);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderSetBindGroup(WGPURenderPassEncoder self, uint32_t groupIndex, WGPUBindGroup group, size_t dynamicOffsetCount, const uint32_t* dynamicOffsets) {
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
typedef struct { 
   WGPURenderPassEncoder Self;
   WGPUBuffer IndirectBuffer;
   uint64_t IndirectOffset;
   uint32_t MaxDrawCount;
   WGPUBuffer DrawCountBuffer;
   uint64_t DrawCountBufferOffset;
} WGPURenderPassEncoderMultiDrawIndirectArgs;
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderMultiDrawIndirect(const WGPURenderPassEncoderMultiDrawIndirectArgs* args) {
    return wgpuRenderPassEncoderMultiDrawIndirect(args->Self, args->IndirectBuffer, args->IndirectOffset, args->MaxDrawCount, args->DrawCountBuffer, args->DrawCountBufferOffset);
}
typedef struct { 
   WGPURenderPassEncoder Self;
   WGPUBuffer IndirectBuffer;
   uint64_t IndirectOffset;
   uint32_t MaxDrawCount;
   WGPUBuffer DrawCountBuffer;
   uint64_t DrawCountBufferOffset;
} WGPURenderPassEncoderMultiDrawIndexedIndirectArgs;
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderMultiDrawIndexedIndirect(const WGPURenderPassEncoderMultiDrawIndexedIndirectArgs* args) {
    return wgpuRenderPassEncoderMultiDrawIndexedIndirect(args->Self, args->IndirectBuffer, args->IndirectOffset, args->MaxDrawCount, args->DrawCountBuffer, args->DrawCountBufferOffset);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderExecuteBundles(WGPURenderPassEncoder self, size_t bundleCount, const WGPURenderBundle* bundles) {
    return wgpuRenderPassEncoderExecuteBundles(self, bundleCount, bundles);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderInsertDebugMarker(WGPURenderPassEncoder self, WGPUStringView markerLabel) {
    return wgpuRenderPassEncoderInsertDebugMarker(self, markerLabel);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderPopDebugGroup(WGPURenderPassEncoder self) {
    return wgpuRenderPassEncoderPopDebugGroup(self);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderPushDebugGroup(WGPURenderPassEncoder self, WGPUStringView groupLabel) {
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
   WGPUIndexFormat Format;
   uint64_t Offset;
   uint64_t Size;
} WGPURenderPassEncoderSetIndexBufferArgs;
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderSetIndexBuffer(const WGPURenderPassEncoderSetIndexBufferArgs* args) {
    return wgpuRenderPassEncoderSetIndexBuffer(args->Self, args->Buffer, args->Format, args->Offset, args->Size);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderBeginOcclusionQuery(WGPURenderPassEncoder self, uint32_t queryIndex) {
    return wgpuRenderPassEncoderBeginOcclusionQuery(self, queryIndex);
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
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderSetLabel(WGPURenderPassEncoder self, WGPUStringView label) {
    return wgpuRenderPassEncoderSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPassEncoderRelease(WGPURenderPassEncoder self) {
    return wgpuRenderPassEncoderRelease(self);
}
EMSCRIPTEN_KEEPALIVE WGPUBindGroupLayout gpuRenderPipelineGetBindGroupLayout(WGPURenderPipeline self, uint32_t groupIndex) {
    return wgpuRenderPipelineGetBindGroupLayout(self, groupIndex);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPipelineSetLabel(WGPURenderPipeline self, WGPUStringView label) {
    return wgpuRenderPipelineSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuRenderPipelineRelease(WGPURenderPipeline self) {
    return wgpuRenderPipelineRelease(self);
}
EMSCRIPTEN_KEEPALIVE void gpuSamplerSetLabel(WGPUSampler self, WGPUStringView label) {
    return wgpuSamplerSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuSamplerRelease(WGPUSampler self) {
    return wgpuSamplerRelease(self);
}
EMSCRIPTEN_KEEPALIVE void gpuShaderModuleGetCompilationInfo(WGPUShaderModule self, WGPUCompilationInfoCallback callback, void * userdata) {
    return wgpuShaderModuleGetCompilationInfo(self, callback, userdata);
}
EMSCRIPTEN_KEEPALIVE WGPUFuture gpuShaderModuleGetCompilationInfoF(WGPUShaderModule self, WGPUCompilationInfoCallbackInfo callbackInfo) {
    return wgpuShaderModuleGetCompilationInfoF(self, callbackInfo);
}
EMSCRIPTEN_KEEPALIVE WGPUFuture gpuShaderModuleGetCompilationInfo2(WGPUShaderModule self, WGPUCompilationInfoCallbackInfo2 callbackInfo) {
    return wgpuShaderModuleGetCompilationInfo2(self, callbackInfo);
}
EMSCRIPTEN_KEEPALIVE void gpuShaderModuleSetLabel(WGPUShaderModule self, WGPUStringView label) {
    return wgpuShaderModuleSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuShaderModuleRelease(WGPUShaderModule self) {
    return wgpuShaderModuleRelease(self);
}
EMSCRIPTEN_KEEPALIVE void gpuSurfaceConfigure(WGPUSurface self, const WGPUSurfaceConfiguration* config) {
    return wgpuSurfaceConfigure(self, config);
}
EMSCRIPTEN_KEEPALIVE WGPUStatus gpuSurfaceGetCapabilities(WGPUSurface self, WGPUAdapter adapter, WGPUSurfaceCapabilities* capabilities) {
    return wgpuSurfaceGetCapabilities(self, adapter, capabilities);
}
EMSCRIPTEN_KEEPALIVE void gpuSurfaceGetCurrentTexture(WGPUSurface self, WGPUSurfaceTexture* surfaceTexture) {
    return wgpuSurfaceGetCurrentTexture(self, surfaceTexture);
}
EMSCRIPTEN_KEEPALIVE void gpuSurfacePresent(WGPUSurface self) {
    return wgpuSurfacePresent(self);
}
EMSCRIPTEN_KEEPALIVE void gpuSurfaceUnconfigure(WGPUSurface self) {
    return wgpuSurfaceUnconfigure(self);
}
EMSCRIPTEN_KEEPALIVE void gpuSurfaceSetLabel(WGPUSurface self, WGPUStringView label) {
    return wgpuSurfaceSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuSurfaceRelease(WGPUSurface self) {
    return wgpuSurfaceRelease(self);
}
EMSCRIPTEN_KEEPALIVE WGPUTextureView gpuTextureCreateView(WGPUTexture self, const WGPUTextureViewDescriptor* descriptor) {
    return wgpuTextureCreateView(self, descriptor);
}
EMSCRIPTEN_KEEPALIVE void gpuTextureSetLabel(WGPUTexture self, WGPUStringView label) {
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
EMSCRIPTEN_KEEPALIVE WGPUTextureDimension gpuTextureGetDimension(WGPUTexture self) {
    return wgpuTextureGetDimension(self);
}
EMSCRIPTEN_KEEPALIVE WGPUTextureFormat gpuTextureGetFormat(WGPUTexture self) {
    return wgpuTextureGetFormat(self);
}
EMSCRIPTEN_KEEPALIVE WGPUTextureUsage gpuTextureGetUsage(WGPUTexture self) {
    return wgpuTextureGetUsage(self);
}
EMSCRIPTEN_KEEPALIVE void gpuTextureDestroy(WGPUTexture self) {
    return wgpuTextureDestroy(self);
}
EMSCRIPTEN_KEEPALIVE void gpuTextureRelease(WGPUTexture self) {
    return wgpuTextureRelease(self);
}
EMSCRIPTEN_KEEPALIVE void gpuTextureViewSetLabel(WGPUTextureView self, WGPUStringView label) {
    return wgpuTextureViewSetLabel(self, label);
}
EMSCRIPTEN_KEEPALIVE void gpuTextureViewRelease(WGPUTextureView self) {
    return wgpuTextureViewRelease(self);
}
