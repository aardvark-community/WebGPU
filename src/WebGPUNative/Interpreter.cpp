#include "dllexport.h"
#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include "dawn/webgpu_cpp.h"
#include "dawn/webgpu.h"
#include "dawn/native/DawnNative.h"
typedef enum {
    CommandEncoderFinish = 0,
    CommandEncoderBeginComputePass = 1,
    CommandEncoderBeginRenderPass = 2,
    CommandEncoderCopyBufferToBuffer = 3,
    CommandEncoderCopyBufferToTexture = 4,
    CommandEncoderCopyTextureToBuffer = 5,
    CommandEncoderCopyTextureToTexture = 6,
    CommandEncoderClearBuffer = 7,
    CommandEncoderInjectValidationError = 8,
    CommandEncoderInsertDebugMarker = 9,
    CommandEncoderPopDebugGroup = 10,
    CommandEncoderPushDebugGroup = 11,
    CommandEncoderResolveQuerySet = 12,
    CommandEncoderWriteBuffer = 13,
    CommandEncoderWriteTimestamp = 14,
    ComputePassEncoderInsertDebugMarker = 15,
    ComputePassEncoderPopDebugGroup = 16,
    ComputePassEncoderPushDebugGroup = 17,
    ComputePassEncoderSetPipeline = 18,
    ComputePassEncoderSetBindGroup = 19,
    ComputePassEncoderWriteTimestamp = 20,
    ComputePassEncoderDispatchWorkgroups = 21,
    ComputePassEncoderDispatchWorkgroupsIndirect = 22,
    ComputePassEncoderEnd = 23,
    ComputePassEncoderSetImmediateData = 24,
    RenderPassEncoderSetPipeline = 25,
    RenderPassEncoderSetBindGroup = 26,
    RenderPassEncoderDraw = 27,
    RenderPassEncoderDrawIndexed = 28,
    RenderPassEncoderDrawIndirect = 29,
    RenderPassEncoderDrawIndexedIndirect = 30,
    RenderPassEncoderMultiDrawIndirect = 31,
    RenderPassEncoderMultiDrawIndexedIndirect = 32,
    RenderPassEncoderExecuteBundles = 33,
    RenderPassEncoderInsertDebugMarker = 34,
    RenderPassEncoderPopDebugGroup = 35,
    RenderPassEncoderPushDebugGroup = 36,
    RenderPassEncoderSetStencilReference = 37,
    RenderPassEncoderSetBlendConstant = 38,
    RenderPassEncoderSetViewport = 39,
    RenderPassEncoderSetScissorRect = 40,
    RenderPassEncoderSetVertexBuffer = 41,
    RenderPassEncoderSetIndexBuffer = 42,
    RenderPassEncoderBeginOcclusionQuery = 43,
    RenderPassEncoderEndOcclusionQuery = 44,
    RenderPassEncoderWriteTimestamp = 45,
    RenderPassEncoderPixelLocalStorageBarrier = 46,
    RenderPassEncoderEnd = 47,
    RenderPassEncoderSetImmediateData = 48
} CommandStreamCommand;
size_t align8(size_t n) {
    if(n % 8 == 0) return n;
    else return ((n / 8) + 1) * 8;
}
size_t chainedSize(WGPUSType type) {
    switch(type) {
        case WGPUSType_RequestAdapterWebXROptions: return sizeof(WGPURequestAdapterWebXROptions);
        case WGPUSType_RequestAdapterWebGPUBackendOptions: return sizeof(WGPURequestAdapterWebGPUBackendOptions);
        case WGPUSType_DawnConsumeAdapterDescriptor: return sizeof(WGPUDawnConsumeAdapterDescriptor);
        case WGPUSType_DawnTogglesDescriptor: return sizeof(WGPUDawnTogglesDescriptor);
        case WGPUSType_DawnCacheDeviceDescriptor: return sizeof(WGPUDawnCacheDeviceDescriptor);
        case WGPUSType_DawnDeviceAllocatorControl: return sizeof(WGPUDawnDeviceAllocatorControl);
        case WGPUSType_DawnWGSLBlocklist: return sizeof(WGPUDawnWGSLBlocklist);
        case WGPUSType_BindGroupDynamicBindingArray: return sizeof(WGPUBindGroupDynamicBindingArray);
        case WGPUSType_StaticSamplerBindingLayout: return sizeof(WGPUStaticSamplerBindingLayout);
        case WGPUSType_ExternalTextureBindingEntry: return sizeof(WGPUExternalTextureBindingEntry);
        case WGPUSType_ExternalTextureBindingLayout: return sizeof(WGPUExternalTextureBindingLayout);
        case WGPUSType_BindGroupLayoutDynamicBindingArray: return sizeof(WGPUBindGroupLayoutDynamicBindingArray);
        case WGPUSType_BufferHostMappedPointer: return sizeof(WGPUBufferHostMappedPointer);
        case WGPUSType_DawnCompilationMessageUtf16: return sizeof(WGPUDawnCompilationMessageUtf16);
        case WGPUSType_CompatibilityModeLimits: return sizeof(WGPUCompatibilityModeLimits);
        case WGPUSType_DawnTexelCopyBufferRowAlignmentLimits: return sizeof(WGPUDawnTexelCopyBufferRowAlignmentLimits);
        case WGPUSType_DawnHostMappedPointerLimits: return sizeof(WGPUDawnHostMappedPointerLimits);
        case WGPUSType_DynamicBindingArrayLimits: return sizeof(WGPUDynamicBindingArrayLimits);
        case WGPUSType_SharedTextureMemoryAHardwareBufferProperties: return sizeof(WGPUSharedTextureMemoryAHardwareBufferProperties);
        case WGPUSType_SharedTextureMemoryVkDedicatedAllocationDescriptor: return sizeof(WGPUSharedTextureMemoryVkDedicatedAllocationDescriptor);
        case WGPUSType_SharedTextureMemoryAHardwareBufferDescriptor: return sizeof(WGPUSharedTextureMemoryAHardwareBufferDescriptor);
        case WGPUSType_SharedTextureMemoryDmaBufDescriptor: return sizeof(WGPUSharedTextureMemoryDmaBufDescriptor);
        case WGPUSType_SharedTextureMemoryOpaqueFDDescriptor: return sizeof(WGPUSharedTextureMemoryOpaqueFDDescriptor);
        case WGPUSType_SharedTextureMemoryZirconHandleDescriptor: return sizeof(WGPUSharedTextureMemoryZirconHandleDescriptor);
        case WGPUSType_SharedTextureMemoryDXGISharedHandleDescriptor: return sizeof(WGPUSharedTextureMemoryDXGISharedHandleDescriptor);
        case WGPUSType_SharedTextureMemoryIOSurfaceDescriptor: return sizeof(WGPUSharedTextureMemoryIOSurfaceDescriptor);
        case WGPUSType_SharedTextureMemoryEGLImageDescriptor: return sizeof(WGPUSharedTextureMemoryEGLImageDescriptor);
        case WGPUSType_SharedTextureMemoryVkImageLayoutBeginState: return sizeof(WGPUSharedTextureMemoryVkImageLayoutBeginState);
        case WGPUSType_SharedTextureMemoryVkImageLayoutEndState: return sizeof(WGPUSharedTextureMemoryVkImageLayoutEndState);
        case WGPUSType_SharedTextureMemoryD3DSwapchainBeginState: return sizeof(WGPUSharedTextureMemoryD3DSwapchainBeginState);
        case WGPUSType_SharedTextureMemoryD3D11BeginState: return sizeof(WGPUSharedTextureMemoryD3D11BeginState);
        case WGPUSType_SharedFenceVkSemaphoreOpaqueFDDescriptor: return sizeof(WGPUSharedFenceVkSemaphoreOpaqueFDDescriptor);
        case WGPUSType_SharedFenceSyncFDDescriptor: return sizeof(WGPUSharedFenceSyncFDDescriptor);
        case WGPUSType_SharedFenceVkSemaphoreZirconHandleDescriptor: return sizeof(WGPUSharedFenceVkSemaphoreZirconHandleDescriptor);
        case WGPUSType_SharedFenceDXGISharedHandleDescriptor: return sizeof(WGPUSharedFenceDXGISharedHandleDescriptor);
        case WGPUSType_SharedFenceMTLSharedEventDescriptor: return sizeof(WGPUSharedFenceMTLSharedEventDescriptor);
        case WGPUSType_SharedFenceEGLSyncDescriptor: return sizeof(WGPUSharedFenceEGLSyncDescriptor);
        case WGPUSType_DawnFakeBufferOOMForTesting: return sizeof(WGPUDawnFakeBufferOOMForTesting);
        case WGPUSType_DawnFakeDeviceInitializeErrorForTesting: return sizeof(WGPUDawnFakeDeviceInitializeErrorForTesting);
        case WGPUSType_SharedFenceVkSemaphoreOpaqueFDExportInfo: return sizeof(WGPUSharedFenceVkSemaphoreOpaqueFDExportInfo);
        case WGPUSType_SharedFenceSyncFDExportInfo: return sizeof(WGPUSharedFenceSyncFDExportInfo);
        case WGPUSType_SharedFenceVkSemaphoreZirconHandleExportInfo: return sizeof(WGPUSharedFenceVkSemaphoreZirconHandleExportInfo);
        case WGPUSType_SharedFenceDXGISharedHandleExportInfo: return sizeof(WGPUSharedFenceDXGISharedHandleExportInfo);
        case WGPUSType_SharedFenceMTLSharedEventExportInfo: return sizeof(WGPUSharedFenceMTLSharedEventExportInfo);
        case WGPUSType_SharedFenceEGLSyncExportInfo: return sizeof(WGPUSharedFenceEGLSyncExportInfo);
        case WGPUSType_DawnDrmFormatCapabilities: return sizeof(WGPUDawnDrmFormatCapabilities);
        case WGPUSType_DawnWireWGSLControl: return sizeof(WGPUDawnWireWGSLControl);
        case WGPUSType_DawnInjectedInvalidSType: return sizeof(WGPUDawnInjectedInvalidSType);
        case WGPUSType_PipelineLayoutPixelLocalStorage: return sizeof(WGPUPipelineLayoutPixelLocalStorage);
        case WGPUSType_DawnRenderPassColorAttachmentRenderToSingleSampled: return sizeof(WGPUDawnRenderPassColorAttachmentRenderToSingleSampled);
        case WGPUSType_RenderPassMaxDrawCount: return sizeof(WGPURenderPassMaxDrawCount);
        case WGPUSType_RenderPassDescriptorExpandResolveRect: return sizeof(WGPURenderPassDescriptorExpandResolveRect);
        case WGPUSType_RenderPassDescriptorResolveRect: return sizeof(WGPURenderPassDescriptorResolveRect);
        case WGPUSType_RenderPassPixelLocalStorage: return sizeof(WGPURenderPassPixelLocalStorage);
        case WGPUSType_ColorTargetStateExpandResolveTextureDawn: return sizeof(WGPUColorTargetStateExpandResolveTextureDawn);
        case WGPUSType_ShaderSourceSPIRV: return sizeof(WGPUShaderSourceSPIRV);
        case WGPUSType_ShaderSourceWGSL: return sizeof(WGPUShaderSourceWGSL);
        case WGPUSType_DawnShaderModuleSPIRVOptionsDescriptor: return sizeof(WGPUDawnShaderModuleSPIRVOptionsDescriptor);
        case WGPUSType_ShaderModuleCompilationOptions: return sizeof(WGPUShaderModuleCompilationOptions);
        case WGPUSType_SurfaceSourceAndroidNativeWindow: return sizeof(WGPUSurfaceSourceAndroidNativeWindow);
        case WGPUSType_EmscriptenSurfaceSourceCanvasHTMLSelector: return sizeof(WGPUEmscriptenSurfaceSourceCanvasHTMLSelector);
        case WGPUSType_SurfaceSourceMetalLayer: return sizeof(WGPUSurfaceSourceMetalLayer);
        case WGPUSType_SurfaceSourceWindowsHWND: return sizeof(WGPUSurfaceSourceWindowsHWND);
        case WGPUSType_SurfaceSourceXCBWindow: return sizeof(WGPUSurfaceSourceXCBWindow);
        case WGPUSType_SurfaceSourceXlibWindow: return sizeof(WGPUSurfaceSourceXlibWindow);
        case WGPUSType_SurfaceSourceWaylandSurface: return sizeof(WGPUSurfaceSourceWaylandSurface);
        case WGPUSType_SurfaceDescriptorFromWindowsCoreWindow: return sizeof(WGPUSurfaceDescriptorFromWindowsCoreWindow);
        case WGPUSType_SurfaceDescriptorFromWindowsUWPSwapChainPanel: return sizeof(WGPUSurfaceDescriptorFromWindowsUWPSwapChainPanel);
        case WGPUSType_SurfaceDescriptorFromWindowsWinUISwapChainPanel: return sizeof(WGPUSurfaceDescriptorFromWindowsWinUISwapChainPanel);
        case WGPUSType_SurfaceColorManagement: return sizeof(WGPUSurfaceColorManagement);
        case WGPUSType_TextureBindingViewDimensionDescriptor: return sizeof(WGPUTextureBindingViewDimensionDescriptor);
        case WGPUSType_TextureComponentSwizzleDescriptor: return sizeof(WGPUTextureComponentSwizzleDescriptor);
        case WGPUSType_YCbCrVkDescriptor: return sizeof(WGPUYCbCrVkDescriptor);
        case WGPUSType_DawnTextureInternalUsageDescriptor: return sizeof(WGPUDawnTextureInternalUsageDescriptor);
        case WGPUSType_DawnEncoderInternalUsageDescriptor: return sizeof(WGPUDawnEncoderInternalUsageDescriptor);
        case WGPUSType_DawnAdapterPropertiesPowerPreference: return sizeof(WGPUDawnAdapterPropertiesPowerPreference);
        case WGPUSType_AdapterPropertiesMemoryHeaps: return sizeof(WGPUAdapterPropertiesMemoryHeaps);
        case WGPUSType_AdapterPropertiesD3D: return sizeof(WGPUAdapterPropertiesD3D);
        case WGPUSType_AdapterPropertiesVk: return sizeof(WGPUAdapterPropertiesVk);
        case WGPUSType_DawnBufferDescriptorErrorInfoFromWireClient: return sizeof(WGPUDawnBufferDescriptorErrorInfoFromWireClient);
        case WGPUSType_AdapterPropertiesSubgroupMatrixConfigs: return sizeof(WGPUAdapterPropertiesSubgroupMatrixConfigs);
        default: return 0;
    }
}
const char* cmdName(CommandStreamCommand cmd) {
    switch(cmd) {
        case CommandEncoderFinish: return "CommandEncoderFinish";
        case CommandEncoderBeginComputePass: return "CommandEncoderBeginComputePass";
        case CommandEncoderBeginRenderPass: return "CommandEncoderBeginRenderPass";
        case CommandEncoderCopyBufferToBuffer: return "CommandEncoderCopyBufferToBuffer";
        case CommandEncoderCopyBufferToTexture: return "CommandEncoderCopyBufferToTexture";
        case CommandEncoderCopyTextureToBuffer: return "CommandEncoderCopyTextureToBuffer";
        case CommandEncoderCopyTextureToTexture: return "CommandEncoderCopyTextureToTexture";
        case CommandEncoderClearBuffer: return "CommandEncoderClearBuffer";
        case CommandEncoderInjectValidationError: return "CommandEncoderInjectValidationError";
        case CommandEncoderInsertDebugMarker: return "CommandEncoderInsertDebugMarker";
        case CommandEncoderPopDebugGroup: return "CommandEncoderPopDebugGroup";
        case CommandEncoderPushDebugGroup: return "CommandEncoderPushDebugGroup";
        case CommandEncoderResolveQuerySet: return "CommandEncoderResolveQuerySet";
        case CommandEncoderWriteBuffer: return "CommandEncoderWriteBuffer";
        case CommandEncoderWriteTimestamp: return "CommandEncoderWriteTimestamp";
        case ComputePassEncoderInsertDebugMarker: return "ComputePassEncoderInsertDebugMarker";
        case ComputePassEncoderPopDebugGroup: return "ComputePassEncoderPopDebugGroup";
        case ComputePassEncoderPushDebugGroup: return "ComputePassEncoderPushDebugGroup";
        case ComputePassEncoderSetPipeline: return "ComputePassEncoderSetPipeline";
        case ComputePassEncoderSetBindGroup: return "ComputePassEncoderSetBindGroup";
        case ComputePassEncoderWriteTimestamp: return "ComputePassEncoderWriteTimestamp";
        case ComputePassEncoderDispatchWorkgroups: return "ComputePassEncoderDispatchWorkgroups";
        case ComputePassEncoderDispatchWorkgroupsIndirect: return "ComputePassEncoderDispatchWorkgroupsIndirect";
        case ComputePassEncoderEnd: return "ComputePassEncoderEnd";
        case ComputePassEncoderSetImmediateData: return "ComputePassEncoderSetImmediateData";
        case RenderPassEncoderSetPipeline: return "RenderPassEncoderSetPipeline";
        case RenderPassEncoderSetBindGroup: return "RenderPassEncoderSetBindGroup";
        case RenderPassEncoderDraw: return "RenderPassEncoderDraw";
        case RenderPassEncoderDrawIndexed: return "RenderPassEncoderDrawIndexed";
        case RenderPassEncoderDrawIndirect: return "RenderPassEncoderDrawIndirect";
        case RenderPassEncoderDrawIndexedIndirect: return "RenderPassEncoderDrawIndexedIndirect";
        case RenderPassEncoderMultiDrawIndirect: return "RenderPassEncoderMultiDrawIndirect";
        case RenderPassEncoderMultiDrawIndexedIndirect: return "RenderPassEncoderMultiDrawIndexedIndirect";
        case RenderPassEncoderExecuteBundles: return "RenderPassEncoderExecuteBundles";
        case RenderPassEncoderInsertDebugMarker: return "RenderPassEncoderInsertDebugMarker";
        case RenderPassEncoderPopDebugGroup: return "RenderPassEncoderPopDebugGroup";
        case RenderPassEncoderPushDebugGroup: return "RenderPassEncoderPushDebugGroup";
        case RenderPassEncoderSetStencilReference: return "RenderPassEncoderSetStencilReference";
        case RenderPassEncoderSetBlendConstant: return "RenderPassEncoderSetBlendConstant";
        case RenderPassEncoderSetViewport: return "RenderPassEncoderSetViewport";
        case RenderPassEncoderSetScissorRect: return "RenderPassEncoderSetScissorRect";
        case RenderPassEncoderSetVertexBuffer: return "RenderPassEncoderSetVertexBuffer";
        case RenderPassEncoderSetIndexBuffer: return "RenderPassEncoderSetIndexBuffer";
        case RenderPassEncoderBeginOcclusionQuery: return "RenderPassEncoderBeginOcclusionQuery";
        case RenderPassEncoderEndOcclusionQuery: return "RenderPassEncoderEndOcclusionQuery";
        case RenderPassEncoderWriteTimestamp: return "RenderPassEncoderWriteTimestamp";
        case RenderPassEncoderPixelLocalStorageBarrier: return "RenderPassEncoderPixelLocalStorageBarrier";
        case RenderPassEncoderEnd: return "RenderPassEncoderEnd";
        case RenderPassEncoderSetImmediateData: return "RenderPassEncoderSetImmediateData";
        default: return "<unknown>";
    }
}
DllExport(void) gpuRunInterpreter(WGPUCommandEncoder enc, uint8_t* ptr, uint8_t* e) {
    uint8_t* start;
    WGPUCommandEncoder commandEncoder = enc;
    WGPURenderPassEncoder renderPassEncoder = NULL;
    WGPUComputePassEncoder computePassEncoder = NULL;
    while(ptr != e) {
        start = ptr;
        int32_t size = *((int32_t*)ptr);
        ptr += 4;
        CommandStreamCommand cmd = *((CommandStreamCommand*)ptr);
        ptr += sizeof(CommandStreamCommand);
        printf("%s/%d\n", cmdName(cmd), size);
        switch(cmd) {
            case CommandEncoderFinish: {
                // descriptor
                WGPUCommandBufferDescriptor* descriptorSrc = ((WGPUCommandBufferDescriptor*)(ptr + *((size_t*)ptr)));
                ptr += sizeof(void*);
                WGPUCommandBufferDescriptor descriptor = *descriptorSrc;
                if (descriptor.nextInChain != NULL) {
                    uint8_t* basePtr = (uint8_t*)descriptorSrc;
                    WGPUChainedStruct** nextLocation = (WGPUChainedStruct**)&descriptor.nextInChain;
                    while(*nextLocation != NULL) {
                        WGPUChainedStruct* realNextPtr = (WGPUChainedStruct*)((size_t)*nextLocation + basePtr);
                        size_t extSize = chainedSize(realNextPtr->sType);
                        WGPUChainedStruct* lMem = (WGPUChainedStruct*)alloca(extSize);
                        memcpy((void*)lMem, (void*)realNextPtr, extSize);
                        *nextLocation = (WGPUChainedStruct*)lMem;
                        basePtr = (uint8_t*)&realNextPtr->next;
                        nextLocation = &lMem->next;
                    }
                    // EXT!!!!
                }
                if (descriptor.label.data != NULL) descriptor.label.data = (const char*)((size_t)descriptor.label.data + (uint8_t*)&descriptor.label);
                printf("wgpuCommandEncoderFinish(0x%X,0x%X)\n", commandEncoder, &descriptor);
                wgpuCommandEncoderFinish(commandEncoder, &descriptor);
            }
            break; 
            case CommandEncoderBeginComputePass: {
                // descriptor
                WGPUComputePassDescriptor* descriptorSrc = ((WGPUComputePassDescriptor*)(ptr + *((size_t*)ptr)));
                ptr += sizeof(void*);
                WGPUComputePassDescriptor descriptor = *descriptorSrc;
                if (descriptor.nextInChain != NULL) {
                    uint8_t* basePtr = (uint8_t*)descriptorSrc;
                    WGPUChainedStruct** nextLocation = (WGPUChainedStruct**)&descriptor.nextInChain;
                    while(*nextLocation != NULL) {
                        WGPUChainedStruct* realNextPtr = (WGPUChainedStruct*)((size_t)*nextLocation + basePtr);
                        size_t extSize = chainedSize(realNextPtr->sType);
                        WGPUChainedStruct* lMem = (WGPUChainedStruct*)alloca(extSize);
                        memcpy((void*)lMem, (void*)realNextPtr, extSize);
                        *nextLocation = (WGPUChainedStruct*)lMem;
                        basePtr = (uint8_t*)&realNextPtr->next;
                        nextLocation = &lMem->next;
                    }
                    // EXT!!!!
                }
                if (descriptor.label.data != NULL) descriptor.label.data = (const char*)((size_t)descriptor.label.data + (uint8_t*)&descriptor.label);
                if (descriptor.timestampWrites != NULL) {
                    descriptor.timestampWrites = (WGPUPassTimestampWrites*)((size_t)descriptor.timestampWrites + (uint8_t*)descriptorSrc);
                    // descriptor.timestampWrites is a nested struct pointer
                    if (descriptor.timestampWrites->nextInChain != NULL) {
                        uint8_t* basePtr = (uint8_t*)descriptor.timestampWrites;
                        WGPUChainedStruct** nextLocation = (WGPUChainedStruct**)&descriptor.timestampWrites->nextInChain;
                        while(*nextLocation != NULL) {
                            WGPUChainedStruct* realNextPtr = (WGPUChainedStruct*)((size_t)*nextLocation + basePtr);
                            size_t extSize = chainedSize(realNextPtr->sType);
                            WGPUChainedStruct* lMem = (WGPUChainedStruct*)alloca(extSize);
                            memcpy((void*)lMem, (void*)realNextPtr, extSize);
                            *nextLocation = (WGPUChainedStruct*)lMem;
                            basePtr = (uint8_t*)&realNextPtr->next;
                            nextLocation = &lMem->next;
                        }
                        // EXT!!!!
                    }
                }
                printf("wgpuCommandEncoderBeginComputePass(0x%X,0x%X)\n", commandEncoder, &descriptor);
                computePassEncoder = wgpuCommandEncoderBeginComputePass(commandEncoder, &descriptor);
            }
            break; 
            case CommandEncoderBeginRenderPass: {
                // descriptor
                WGPURenderPassDescriptor* descriptorSrc = ((WGPURenderPassDescriptor*)(ptr + *((size_t*)ptr)));
                ptr += sizeof(void*);
                WGPURenderPassDescriptor descriptor = *descriptorSrc;
                if (descriptor.nextInChain != NULL) {
                    uint8_t* basePtr = (uint8_t*)descriptorSrc;
                    WGPUChainedStruct** nextLocation = (WGPUChainedStruct**)&descriptor.nextInChain;
                    while(*nextLocation != NULL) {
                        WGPUChainedStruct* realNextPtr = (WGPUChainedStruct*)((size_t)*nextLocation + basePtr);
                        size_t extSize = chainedSize(realNextPtr->sType);
                        WGPUChainedStruct* lMem = (WGPUChainedStruct*)alloca(extSize);
                        memcpy((void*)lMem, (void*)realNextPtr, extSize);
                        *nextLocation = (WGPUChainedStruct*)lMem;
                        basePtr = (uint8_t*)&realNextPtr->next;
                        nextLocation = &lMem->next;
                    }
                    // EXT!!!!
                }
                if (descriptor.label.data != NULL) descriptor.label.data = (const char*)((size_t)descriptor.label.data + (uint8_t*)&descriptor.label);
                if (descriptor.colorAttachments != NULL) {
                    descriptor.colorAttachments = (WGPURenderPassColorAttachment*)((size_t)descriptor.colorAttachments + (uint8_t*)descriptorSrc);
                    size_t colorAttachmentsSize = sizeof(WGPURenderPassColorAttachment) * descriptor.colorAttachmentCount;
                    WGPURenderPassColorAttachment* colorAttachments = (WGPURenderPassColorAttachment*)alloca(colorAttachmentsSize);
                    memcpy(colorAttachments, descriptor.colorAttachments, colorAttachmentsSize);
                    descriptor.colorAttachments = colorAttachments;
                    for(int i = 0; i < descriptor.colorAttachmentCount; i++) {
                        if (descriptor.colorAttachments[i].nextInChain != NULL) {
                            uint8_t* basePtr = (uint8_t*)&descriptor.colorAttachments[i];
                            WGPUChainedStruct** nextLocation = (WGPUChainedStruct**)&descriptor.colorAttachments[i].nextInChain;
                            while(*nextLocation != NULL) {
                                WGPUChainedStruct* realNextPtr = (WGPUChainedStruct*)((size_t)*nextLocation + basePtr);
                                size_t extSize = chainedSize(realNextPtr->sType);
                                WGPUChainedStruct* lMem = (WGPUChainedStruct*)alloca(extSize);
                                memcpy((void*)lMem, (void*)realNextPtr, extSize);
                                *nextLocation = (WGPUChainedStruct*)lMem;
                                basePtr = (uint8_t*)&realNextPtr->next;
                                nextLocation = &lMem->next;
                            }
                            // EXT!!!!
                        }
                    }
                    // STRUCT descriptor.colorAttachments has length descriptor.colorAttachmentCount
                }
                if (descriptor.depthStencilAttachment != NULL) {
                    descriptor.depthStencilAttachment = (WGPURenderPassDepthStencilAttachment*)((size_t)descriptor.depthStencilAttachment + (uint8_t*)descriptorSrc);
                    // descriptor.depthStencilAttachment is a nested struct pointer
                    if (descriptor.depthStencilAttachment->nextInChain != NULL) {
                        uint8_t* basePtr = (uint8_t*)descriptor.depthStencilAttachment;
                        WGPUChainedStruct** nextLocation = (WGPUChainedStruct**)&descriptor.depthStencilAttachment->nextInChain;
                        while(*nextLocation != NULL) {
                            WGPUChainedStruct* realNextPtr = (WGPUChainedStruct*)((size_t)*nextLocation + basePtr);
                            size_t extSize = chainedSize(realNextPtr->sType);
                            WGPUChainedStruct* lMem = (WGPUChainedStruct*)alloca(extSize);
                            memcpy((void*)lMem, (void*)realNextPtr, extSize);
                            *nextLocation = (WGPUChainedStruct*)lMem;
                            basePtr = (uint8_t*)&realNextPtr->next;
                            nextLocation = &lMem->next;
                        }
                        // EXT!!!!
                    }
                }
                if (descriptor.timestampWrites != NULL) {
                    descriptor.timestampWrites = (WGPUPassTimestampWrites*)((size_t)descriptor.timestampWrites + (uint8_t*)descriptorSrc);
                    // descriptor.timestampWrites is a nested struct pointer
                    if (descriptor.timestampWrites->nextInChain != NULL) {
                        uint8_t* basePtr = (uint8_t*)descriptor.timestampWrites;
                        WGPUChainedStruct** nextLocation = (WGPUChainedStruct**)&descriptor.timestampWrites->nextInChain;
                        while(*nextLocation != NULL) {
                            WGPUChainedStruct* realNextPtr = (WGPUChainedStruct*)((size_t)*nextLocation + basePtr);
                            size_t extSize = chainedSize(realNextPtr->sType);
                            WGPUChainedStruct* lMem = (WGPUChainedStruct*)alloca(extSize);
                            memcpy((void*)lMem, (void*)realNextPtr, extSize);
                            *nextLocation = (WGPUChainedStruct*)lMem;
                            basePtr = (uint8_t*)&realNextPtr->next;
                            nextLocation = &lMem->next;
                        }
                        // EXT!!!!
                    }
                }
                printf("wgpuCommandEncoderBeginRenderPass(0x%X,0x%X)\n", commandEncoder, &descriptor);
                renderPassEncoder = wgpuCommandEncoderBeginRenderPass(commandEncoder, &descriptor);
            }
            break; 
            case CommandEncoderCopyBufferToBuffer: {
                // source
                WGPUBuffer source = *((WGPUBuffer*)ptr);
                ptr += align8(sizeof(WGPUBuffer));
                // sourceOffset
                uint64_t sourceOffset = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                // destination
                WGPUBuffer destination = *((WGPUBuffer*)ptr);
                ptr += align8(sizeof(WGPUBuffer));
                // destinationOffset
                uint64_t destinationOffset = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                // size
                uint64_t size = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                printf("wgpuCommandEncoderCopyBufferToBuffer(0x%X,0x%X,0x%X,0x%X,0x%X,0x%X)\n", commandEncoder, source, sourceOffset, destination, destinationOffset, size);
                wgpuCommandEncoderCopyBufferToBuffer(commandEncoder, source, sourceOffset, destination, destinationOffset, size);
            }
            break; 
            case CommandEncoderCopyBufferToTexture: {
                // source
                WGPUTexelCopyBufferInfo* sourceSrc = ((WGPUTexelCopyBufferInfo*)(ptr + *((size_t*)ptr)));
                ptr += sizeof(void*);
                WGPUTexelCopyBufferInfo source = *sourceSrc;
                // destination
                WGPUTexelCopyTextureInfo* destinationSrc = ((WGPUTexelCopyTextureInfo*)(ptr + *((size_t*)ptr)));
                ptr += sizeof(void*);
                WGPUTexelCopyTextureInfo destination = *destinationSrc;
                // copySize
                WGPUExtent3D* copySizeSrc = ((WGPUExtent3D*)(ptr + *((size_t*)ptr)));
                ptr += sizeof(void*);
                WGPUExtent3D copySize = *copySizeSrc;
                printf("wgpuCommandEncoderCopyBufferToTexture(0x%X,0x%X,0x%X,0x%X)\n", commandEncoder, &source, &destination, &copySize);
                wgpuCommandEncoderCopyBufferToTexture(commandEncoder, &source, &destination, &copySize);
            }
            break; 
            case CommandEncoderCopyTextureToBuffer: {
                // source
                WGPUTexelCopyTextureInfo* sourceSrc = ((WGPUTexelCopyTextureInfo*)(ptr + *((size_t*)ptr)));
                ptr += sizeof(void*);
                WGPUTexelCopyTextureInfo source = *sourceSrc;
                // destination
                WGPUTexelCopyBufferInfo* destinationSrc = ((WGPUTexelCopyBufferInfo*)(ptr + *((size_t*)ptr)));
                ptr += sizeof(void*);
                WGPUTexelCopyBufferInfo destination = *destinationSrc;
                // copySize
                WGPUExtent3D* copySizeSrc = ((WGPUExtent3D*)(ptr + *((size_t*)ptr)));
                ptr += sizeof(void*);
                WGPUExtent3D copySize = *copySizeSrc;
                printf("wgpuCommandEncoderCopyTextureToBuffer(0x%X,0x%X,0x%X,0x%X)\n", commandEncoder, &source, &destination, &copySize);
                wgpuCommandEncoderCopyTextureToBuffer(commandEncoder, &source, &destination, &copySize);
            }
            break; 
            case CommandEncoderCopyTextureToTexture: {
                // source
                WGPUTexelCopyTextureInfo* sourceSrc = ((WGPUTexelCopyTextureInfo*)(ptr + *((size_t*)ptr)));
                ptr += sizeof(void*);
                WGPUTexelCopyTextureInfo source = *sourceSrc;
                // destination
                WGPUTexelCopyTextureInfo* destinationSrc = ((WGPUTexelCopyTextureInfo*)(ptr + *((size_t*)ptr)));
                ptr += sizeof(void*);
                WGPUTexelCopyTextureInfo destination = *destinationSrc;
                // copySize
                WGPUExtent3D* copySizeSrc = ((WGPUExtent3D*)(ptr + *((size_t*)ptr)));
                ptr += sizeof(void*);
                WGPUExtent3D copySize = *copySizeSrc;
                printf("wgpuCommandEncoderCopyTextureToTexture(0x%X,0x%X,0x%X,0x%X)\n", commandEncoder, &source, &destination, &copySize);
                wgpuCommandEncoderCopyTextureToTexture(commandEncoder, &source, &destination, &copySize);
            }
            break; 
            case CommandEncoderClearBuffer: {
                // buffer
                WGPUBuffer buffer = *((WGPUBuffer*)ptr);
                ptr += align8(sizeof(WGPUBuffer));
                // offset
                uint64_t offset = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                // size
                uint64_t size = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                printf("wgpuCommandEncoderClearBuffer(0x%X,0x%X,0x%X,0x%X)\n", commandEncoder, buffer, offset, size);
                wgpuCommandEncoderClearBuffer(commandEncoder, buffer, offset, size);
            }
            break; 
            case CommandEncoderInjectValidationError: {
                // message
                WGPUStringView* messageSrc = (WGPUStringView*)ptr;
                WGPUStringView message = *messageSrc ;
                if (message.data != NULL) message.data = (const char*)((size_t)message.data + (uint8_t*)messageSrc);
                ptr += align8(sizeof(WGPUStringView));
                printf("wgpuCommandEncoderInjectValidationError(0x%X,0x%X)\n", commandEncoder, message);
                wgpuCommandEncoderInjectValidationError(commandEncoder, message);
            }
            break; 
            case CommandEncoderInsertDebugMarker: {
                // markerLabel
                WGPUStringView* markerLabelSrc = (WGPUStringView*)ptr;
                WGPUStringView markerLabel = *markerLabelSrc ;
                if (markerLabel.data != NULL) markerLabel.data = (const char*)((size_t)markerLabel.data + (uint8_t*)markerLabelSrc);
                ptr += align8(sizeof(WGPUStringView));
                printf("wgpuCommandEncoderInsertDebugMarker(0x%X,0x%X)\n", commandEncoder, markerLabel);
                wgpuCommandEncoderInsertDebugMarker(commandEncoder, markerLabel);
            }
            break; 
            case CommandEncoderPopDebugGroup: {
                printf("wgpuCommandEncoderPopDebugGroup(0x%X)\n", commandEncoder);
                wgpuCommandEncoderPopDebugGroup(commandEncoder);
            }
            break; 
            case CommandEncoderPushDebugGroup: {
                // groupLabel
                WGPUStringView* groupLabelSrc = (WGPUStringView*)ptr;
                WGPUStringView groupLabel = *groupLabelSrc ;
                if (groupLabel.data != NULL) groupLabel.data = (const char*)((size_t)groupLabel.data + (uint8_t*)groupLabelSrc);
                ptr += align8(sizeof(WGPUStringView));
                printf("wgpuCommandEncoderPushDebugGroup(0x%X,0x%X)\n", commandEncoder, groupLabel);
                wgpuCommandEncoderPushDebugGroup(commandEncoder, groupLabel);
            }
            break; 
            case CommandEncoderResolveQuerySet: {
                // querySet
                WGPUQuerySet querySet = *((WGPUQuerySet*)ptr);
                ptr += align8(sizeof(WGPUQuerySet));
                // firstQuery
                uint32_t firstQuery = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // queryCount
                uint32_t queryCount = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // destination
                WGPUBuffer destination = *((WGPUBuffer*)ptr);
                ptr += align8(sizeof(WGPUBuffer));
                // destinationOffset
                uint64_t destinationOffset = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                printf("wgpuCommandEncoderResolveQuerySet(0x%X,0x%X,0x%X,0x%X,0x%X,0x%X)\n", commandEncoder, querySet, firstQuery, queryCount, destination, destinationOffset);
                wgpuCommandEncoderResolveQuerySet(commandEncoder, querySet, firstQuery, queryCount, destination, destinationOffset);
            }
            break; 
            case CommandEncoderWriteBuffer: {
                // buffer
                WGPUBuffer buffer = *((WGPUBuffer*)ptr);
                ptr += align8(sizeof(WGPUBuffer));
                // bufferOffset
                uint64_t bufferOffset = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                // data
                uint8_t* data = (uint8_t*)(ptr + *((size_t*)ptr));
                ptr += sizeof(void*);
                // size
                uint64_t size = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                printf("wgpuCommandEncoderWriteBuffer(0x%X,0x%X,0x%X,0x%X,0x%X)\n", commandEncoder, buffer, bufferOffset, data, size);
                wgpuCommandEncoderWriteBuffer(commandEncoder, buffer, bufferOffset, data, size);
            }
            break; 
            case CommandEncoderWriteTimestamp: {
                // querySet
                WGPUQuerySet querySet = *((WGPUQuerySet*)ptr);
                ptr += align8(sizeof(WGPUQuerySet));
                // queryIndex
                uint32_t queryIndex = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                printf("wgpuCommandEncoderWriteTimestamp(0x%X,0x%X,0x%X)\n", commandEncoder, querySet, queryIndex);
                wgpuCommandEncoderWriteTimestamp(commandEncoder, querySet, queryIndex);
            }
            break; 
            case ComputePassEncoderInsertDebugMarker: {
                // markerLabel
                WGPUStringView* markerLabelSrc = (WGPUStringView*)ptr;
                WGPUStringView markerLabel = *markerLabelSrc ;
                if (markerLabel.data != NULL) markerLabel.data = (const char*)((size_t)markerLabel.data + (uint8_t*)markerLabelSrc);
                ptr += align8(sizeof(WGPUStringView));
                printf("wgpuComputePassEncoderInsertDebugMarker(0x%X,0x%X)\n", computePassEncoder, markerLabel);
                wgpuComputePassEncoderInsertDebugMarker(computePassEncoder, markerLabel);
            }
            break; 
            case ComputePassEncoderPopDebugGroup: {
                printf("wgpuComputePassEncoderPopDebugGroup(0x%X)\n", computePassEncoder);
                wgpuComputePassEncoderPopDebugGroup(computePassEncoder);
            }
            break; 
            case ComputePassEncoderPushDebugGroup: {
                // groupLabel
                WGPUStringView* groupLabelSrc = (WGPUStringView*)ptr;
                WGPUStringView groupLabel = *groupLabelSrc ;
                if (groupLabel.data != NULL) groupLabel.data = (const char*)((size_t)groupLabel.data + (uint8_t*)groupLabelSrc);
                ptr += align8(sizeof(WGPUStringView));
                printf("wgpuComputePassEncoderPushDebugGroup(0x%X,0x%X)\n", computePassEncoder, groupLabel);
                wgpuComputePassEncoderPushDebugGroup(computePassEncoder, groupLabel);
            }
            break; 
            case ComputePassEncoderSetPipeline: {
                // pipeline
                WGPUComputePipeline pipeline = *((WGPUComputePipeline*)ptr);
                ptr += align8(sizeof(WGPUComputePipeline));
                printf("wgpuComputePassEncoderSetPipeline(0x%X,0x%X)\n", computePassEncoder, pipeline);
                wgpuComputePassEncoderSetPipeline(computePassEncoder, pipeline);
            }
            break; 
            case ComputePassEncoderSetBindGroup: {
                // groupIndex
                uint32_t groupIndex = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // group
                WGPUBindGroup group = *((WGPUBindGroup*)ptr);
                ptr += align8(sizeof(WGPUBindGroup));
                // dynamicOffsetCount
                size_t dynamicOffsetCount = *((size_t*)ptr);
                ptr += align8(sizeof(size_t));
                // dynamicOffsets
                uint32_t* dynamicOffsets = (uint32_t*)(ptr + *((size_t*)ptr));
                ptr += sizeof(void*);
                printf("wgpuComputePassEncoderSetBindGroup(0x%X,0x%X,0x%X,0x%X,0x%X)\n", computePassEncoder, groupIndex, group, dynamicOffsetCount, dynamicOffsets);
                wgpuComputePassEncoderSetBindGroup(computePassEncoder, groupIndex, group, dynamicOffsetCount, dynamicOffsets);
            }
            break; 
            case ComputePassEncoderWriteTimestamp: {
                // querySet
                WGPUQuerySet querySet = *((WGPUQuerySet*)ptr);
                ptr += align8(sizeof(WGPUQuerySet));
                // queryIndex
                uint32_t queryIndex = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                printf("wgpuComputePassEncoderWriteTimestamp(0x%X,0x%X,0x%X)\n", computePassEncoder, querySet, queryIndex);
                wgpuComputePassEncoderWriteTimestamp(computePassEncoder, querySet, queryIndex);
            }
            break; 
            case ComputePassEncoderDispatchWorkgroups: {
                // workgroupCountX
                uint32_t workgroupCountX = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // workgroupCountY
                uint32_t workgroupCountY = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // workgroupCountZ
                uint32_t workgroupCountZ = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                printf("wgpuComputePassEncoderDispatchWorkgroups(0x%X,0x%X,0x%X,0x%X)\n", computePassEncoder, workgroupCountX, workgroupCountY, workgroupCountZ);
                wgpuComputePassEncoderDispatchWorkgroups(computePassEncoder, workgroupCountX, workgroupCountY, workgroupCountZ);
            }
            break; 
            case ComputePassEncoderDispatchWorkgroupsIndirect: {
                // indirectBuffer
                WGPUBuffer indirectBuffer = *((WGPUBuffer*)ptr);
                ptr += align8(sizeof(WGPUBuffer));
                // indirectOffset
                uint64_t indirectOffset = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                printf("wgpuComputePassEncoderDispatchWorkgroupsIndirect(0x%X,0x%X,0x%X)\n", computePassEncoder, indirectBuffer, indirectOffset);
                wgpuComputePassEncoderDispatchWorkgroupsIndirect(computePassEncoder, indirectBuffer, indirectOffset);
            }
            break; 
            case ComputePassEncoderEnd: {
                printf("wgpuComputePassEncoderEnd(0x%X)\n", computePassEncoder);
                wgpuComputePassEncoderEnd(computePassEncoder);
            }
            break; 
            case ComputePassEncoderSetImmediateData: {
                // offset
                uint32_t offset = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // data
                void* data = (void*)(ptr + *((size_t*)ptr));
                ptr += sizeof(void*);
                // size
                size_t size = *((size_t*)ptr);
                ptr += align8(sizeof(size_t));
                printf("wgpuComputePassEncoderSetImmediateData(0x%X,0x%X,0x%X,0x%X)\n", computePassEncoder, offset, data, size);
                wgpuComputePassEncoderSetImmediateData(computePassEncoder, offset, data, size);
            }
            break; 
            case RenderPassEncoderSetPipeline: {
                // pipeline
                WGPURenderPipeline pipeline = *((WGPURenderPipeline*)ptr);
                ptr += align8(sizeof(WGPURenderPipeline));
                printf("wgpuRenderPassEncoderSetPipeline(0x%X,0x%X)\n", renderPassEncoder, pipeline);
                wgpuRenderPassEncoderSetPipeline(renderPassEncoder, pipeline);
            }
            break; 
            case RenderPassEncoderSetBindGroup: {
                // groupIndex
                uint32_t groupIndex = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // group
                WGPUBindGroup group = *((WGPUBindGroup*)ptr);
                ptr += align8(sizeof(WGPUBindGroup));
                // dynamicOffsetCount
                size_t dynamicOffsetCount = *((size_t*)ptr);
                ptr += align8(sizeof(size_t));
                // dynamicOffsets
                uint32_t* dynamicOffsets = (uint32_t*)(ptr + *((size_t*)ptr));
                ptr += sizeof(void*);
                printf("wgpuRenderPassEncoderSetBindGroup(0x%X,0x%X,0x%X,0x%X,0x%X)\n", renderPassEncoder, groupIndex, group, dynamicOffsetCount, dynamicOffsets);
                wgpuRenderPassEncoderSetBindGroup(renderPassEncoder, groupIndex, group, dynamicOffsetCount, dynamicOffsets);
            }
            break; 
            case RenderPassEncoderDraw: {
                // vertexCount
                uint32_t vertexCount = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // instanceCount
                uint32_t instanceCount = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // firstVertex
                uint32_t firstVertex = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // firstInstance
                uint32_t firstInstance = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                printf("wgpuRenderPassEncoderDraw(0x%X,0x%X,0x%X,0x%X,0x%X)\n", renderPassEncoder, vertexCount, instanceCount, firstVertex, firstInstance);
                wgpuRenderPassEncoderDraw(renderPassEncoder, vertexCount, instanceCount, firstVertex, firstInstance);
            }
            break; 
            case RenderPassEncoderDrawIndexed: {
                // indexCount
                uint32_t indexCount = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // instanceCount
                uint32_t instanceCount = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // firstIndex
                uint32_t firstIndex = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // baseVertex
                int32_t baseVertex = *((int32_t*)ptr);
                ptr += align8(sizeof(int32_t));
                // firstInstance
                uint32_t firstInstance = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                printf("wgpuRenderPassEncoderDrawIndexed(0x%X,0x%X,0x%X,0x%X,0x%X,0x%X)\n", renderPassEncoder, indexCount, instanceCount, firstIndex, baseVertex, firstInstance);
                wgpuRenderPassEncoderDrawIndexed(renderPassEncoder, indexCount, instanceCount, firstIndex, baseVertex, firstInstance);
            }
            break; 
            case RenderPassEncoderDrawIndirect: {
                // indirectBuffer
                WGPUBuffer indirectBuffer = *((WGPUBuffer*)ptr);
                ptr += align8(sizeof(WGPUBuffer));
                // indirectOffset
                uint64_t indirectOffset = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                printf("wgpuRenderPassEncoderDrawIndirect(0x%X,0x%X,0x%X)\n", renderPassEncoder, indirectBuffer, indirectOffset);
                wgpuRenderPassEncoderDrawIndirect(renderPassEncoder, indirectBuffer, indirectOffset);
            }
            break; 
            case RenderPassEncoderDrawIndexedIndirect: {
                // indirectBuffer
                WGPUBuffer indirectBuffer = *((WGPUBuffer*)ptr);
                ptr += align8(sizeof(WGPUBuffer));
                // indirectOffset
                uint64_t indirectOffset = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                printf("wgpuRenderPassEncoderDrawIndexedIndirect(0x%X,0x%X,0x%X)\n", renderPassEncoder, indirectBuffer, indirectOffset);
                wgpuRenderPassEncoderDrawIndexedIndirect(renderPassEncoder, indirectBuffer, indirectOffset);
            }
            break; 
            case RenderPassEncoderMultiDrawIndirect: {
                // indirectBuffer
                WGPUBuffer indirectBuffer = *((WGPUBuffer*)ptr);
                ptr += align8(sizeof(WGPUBuffer));
                // indirectOffset
                uint64_t indirectOffset = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                // maxDrawCount
                uint32_t maxDrawCount = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // drawCountBuffer
                WGPUBuffer drawCountBuffer = *((WGPUBuffer*)ptr);
                ptr += align8(sizeof(WGPUBuffer));
                // drawCountBufferOffset
                uint64_t drawCountBufferOffset = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                printf("wgpuRenderPassEncoderMultiDrawIndirect(0x%X,0x%X,0x%X,0x%X,0x%X,0x%X)\n", renderPassEncoder, indirectBuffer, indirectOffset, maxDrawCount, drawCountBuffer, drawCountBufferOffset);
                wgpuRenderPassEncoderMultiDrawIndirect(renderPassEncoder, indirectBuffer, indirectOffset, maxDrawCount, drawCountBuffer, drawCountBufferOffset);
            }
            break; 
            case RenderPassEncoderMultiDrawIndexedIndirect: {
                // indirectBuffer
                WGPUBuffer indirectBuffer = *((WGPUBuffer*)ptr);
                ptr += align8(sizeof(WGPUBuffer));
                // indirectOffset
                uint64_t indirectOffset = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                // maxDrawCount
                uint32_t maxDrawCount = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // drawCountBuffer
                WGPUBuffer drawCountBuffer = *((WGPUBuffer*)ptr);
                ptr += align8(sizeof(WGPUBuffer));
                // drawCountBufferOffset
                uint64_t drawCountBufferOffset = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                printf("wgpuRenderPassEncoderMultiDrawIndexedIndirect(0x%X,0x%X,0x%X,0x%X,0x%X,0x%X)\n", renderPassEncoder, indirectBuffer, indirectOffset, maxDrawCount, drawCountBuffer, drawCountBufferOffset);
                wgpuRenderPassEncoderMultiDrawIndexedIndirect(renderPassEncoder, indirectBuffer, indirectOffset, maxDrawCount, drawCountBuffer, drawCountBufferOffset);
            }
            break; 
            case RenderPassEncoderExecuteBundles: {
                // bundleCount
                size_t bundleCount = *((size_t*)ptr);
                ptr += align8(sizeof(size_t));
                // bundles
                WGPURenderBundle* bundles = (WGPURenderBundle*)(ptr + *((size_t*)ptr));
                ptr += sizeof(void*);
                printf("wgpuRenderPassEncoderExecuteBundles(0x%X,0x%X,0x%X)\n", renderPassEncoder, bundleCount, bundles);
                wgpuRenderPassEncoderExecuteBundles(renderPassEncoder, bundleCount, bundles);
            }
            break; 
            case RenderPassEncoderInsertDebugMarker: {
                // markerLabel
                WGPUStringView* markerLabelSrc = (WGPUStringView*)ptr;
                WGPUStringView markerLabel = *markerLabelSrc ;
                if (markerLabel.data != NULL) markerLabel.data = (const char*)((size_t)markerLabel.data + (uint8_t*)markerLabelSrc);
                ptr += align8(sizeof(WGPUStringView));
                printf("wgpuRenderPassEncoderInsertDebugMarker(0x%X,0x%X)\n", renderPassEncoder, markerLabel);
                wgpuRenderPassEncoderInsertDebugMarker(renderPassEncoder, markerLabel);
            }
            break; 
            case RenderPassEncoderPopDebugGroup: {
                printf("wgpuRenderPassEncoderPopDebugGroup(0x%X)\n", renderPassEncoder);
                wgpuRenderPassEncoderPopDebugGroup(renderPassEncoder);
            }
            break; 
            case RenderPassEncoderPushDebugGroup: {
                // groupLabel
                WGPUStringView* groupLabelSrc = (WGPUStringView*)ptr;
                WGPUStringView groupLabel = *groupLabelSrc ;
                if (groupLabel.data != NULL) groupLabel.data = (const char*)((size_t)groupLabel.data + (uint8_t*)groupLabelSrc);
                ptr += align8(sizeof(WGPUStringView));
                printf("wgpuRenderPassEncoderPushDebugGroup(0x%X,0x%X)\n", renderPassEncoder, groupLabel);
                wgpuRenderPassEncoderPushDebugGroup(renderPassEncoder, groupLabel);
            }
            break; 
            case RenderPassEncoderSetStencilReference: {
                // reference
                uint32_t reference = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                printf("wgpuRenderPassEncoderSetStencilReference(0x%X,0x%X)\n", renderPassEncoder, reference);
                wgpuRenderPassEncoderSetStencilReference(renderPassEncoder, reference);
            }
            break; 
            case RenderPassEncoderSetBlendConstant: {
                // color
                WGPUColor* colorSrc = ((WGPUColor*)(ptr + *((size_t*)ptr)));
                ptr += sizeof(void*);
                WGPUColor color = *colorSrc;
                printf("wgpuRenderPassEncoderSetBlendConstant(0x%X,0x%X)\n", renderPassEncoder, &color);
                wgpuRenderPassEncoderSetBlendConstant(renderPassEncoder, &color);
            }
            break; 
            case RenderPassEncoderSetViewport: {
                // x
                float x = *((float*)ptr);
                ptr += align8(sizeof(float));
                // y
                float y = *((float*)ptr);
                ptr += align8(sizeof(float));
                // width
                float width = *((float*)ptr);
                ptr += align8(sizeof(float));
                // height
                float height = *((float*)ptr);
                ptr += align8(sizeof(float));
                // minDepth
                float minDepth = *((float*)ptr);
                ptr += align8(sizeof(float));
                // maxDepth
                float maxDepth = *((float*)ptr);
                ptr += align8(sizeof(float));
                printf("wgpuRenderPassEncoderSetViewport(0x%X,0x%X,0x%X,0x%X,0x%X,0x%X,0x%X)\n", renderPassEncoder, x, y, width, height, minDepth, maxDepth);
                wgpuRenderPassEncoderSetViewport(renderPassEncoder, x, y, width, height, minDepth, maxDepth);
            }
            break; 
            case RenderPassEncoderSetScissorRect: {
                // x
                uint32_t x = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // y
                uint32_t y = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // width
                uint32_t width = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // height
                uint32_t height = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                printf("wgpuRenderPassEncoderSetScissorRect(0x%X,0x%X,0x%X,0x%X,0x%X)\n", renderPassEncoder, x, y, width, height);
                wgpuRenderPassEncoderSetScissorRect(renderPassEncoder, x, y, width, height);
            }
            break; 
            case RenderPassEncoderSetVertexBuffer: {
                // slot
                uint32_t slot = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // buffer
                WGPUBuffer buffer = *((WGPUBuffer*)ptr);
                ptr += align8(sizeof(WGPUBuffer));
                // offset
                uint64_t offset = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                // size
                uint64_t size = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                printf("wgpuRenderPassEncoderSetVertexBuffer(0x%X,0x%X,0x%X,0x%X,0x%X)\n", renderPassEncoder, slot, buffer, offset, size);
                wgpuRenderPassEncoderSetVertexBuffer(renderPassEncoder, slot, buffer, offset, size);
            }
            break; 
            case RenderPassEncoderSetIndexBuffer: {
                // buffer
                WGPUBuffer buffer = *((WGPUBuffer*)ptr);
                ptr += align8(sizeof(WGPUBuffer));
                // format
                WGPUIndexFormat format = *((WGPUIndexFormat*)ptr);
                ptr += align8(sizeof(WGPUIndexFormat));
                // offset
                uint64_t offset = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                // size
                uint64_t size = *((uint64_t*)ptr);
                ptr += align8(sizeof(uint64_t));
                printf("wgpuRenderPassEncoderSetIndexBuffer(0x%X,0x%X,0x%X,0x%X,0x%X)\n", renderPassEncoder, buffer, format, offset, size);
                wgpuRenderPassEncoderSetIndexBuffer(renderPassEncoder, buffer, format, offset, size);
            }
            break; 
            case RenderPassEncoderBeginOcclusionQuery: {
                // queryIndex
                uint32_t queryIndex = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                printf("wgpuRenderPassEncoderBeginOcclusionQuery(0x%X,0x%X)\n", renderPassEncoder, queryIndex);
                wgpuRenderPassEncoderBeginOcclusionQuery(renderPassEncoder, queryIndex);
            }
            break; 
            case RenderPassEncoderEndOcclusionQuery: {
                printf("wgpuRenderPassEncoderEndOcclusionQuery(0x%X)\n", renderPassEncoder);
                wgpuRenderPassEncoderEndOcclusionQuery(renderPassEncoder);
            }
            break; 
            case RenderPassEncoderWriteTimestamp: {
                // querySet
                WGPUQuerySet querySet = *((WGPUQuerySet*)ptr);
                ptr += align8(sizeof(WGPUQuerySet));
                // queryIndex
                uint32_t queryIndex = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                printf("wgpuRenderPassEncoderWriteTimestamp(0x%X,0x%X,0x%X)\n", renderPassEncoder, querySet, queryIndex);
                wgpuRenderPassEncoderWriteTimestamp(renderPassEncoder, querySet, queryIndex);
            }
            break; 
            case RenderPassEncoderPixelLocalStorageBarrier: {
                printf("wgpuRenderPassEncoderPixelLocalStorageBarrier(0x%X)\n", renderPassEncoder);
                wgpuRenderPassEncoderPixelLocalStorageBarrier(renderPassEncoder);
            }
            break; 
            case RenderPassEncoderEnd: {
                printf("wgpuRenderPassEncoderEnd(0x%X)\n", renderPassEncoder);
                wgpuRenderPassEncoderEnd(renderPassEncoder);
            }
            break; 
            case RenderPassEncoderSetImmediateData: {
                // offset
                uint32_t offset = *((uint32_t*)ptr);
                ptr += align8(sizeof(uint32_t));
                // data
                void* data = (void*)(ptr + *((size_t*)ptr));
                ptr += sizeof(void*);
                // size
                size_t size = *((size_t*)ptr);
                ptr += align8(sizeof(size_t));
                printf("wgpuRenderPassEncoderSetImmediateData(0x%X,0x%X,0x%X,0x%X)\n", renderPassEncoder, offset, data, size);
                wgpuRenderPassEncoderSetImmediateData(renderPassEncoder, offset, data, size);
            }
            break; 
            default: break;
        }
        ptr = start + (size_t)size;
    }

}
