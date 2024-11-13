#include "lib_wgpu.h"

#include <webgpu/webgpu.h>

// EMSCRIPTEN_KEEPALIVE int my_navigator_gpu_request_adapter_async(WGpuRequestAdapterCallback adapterCallback, void *userData) {
//     WGpuRequestAdapterOptions options;
//     options.powerPreference = WGPU_POWER_PREFERENCE_HIGH_PERFORMANCE;
//     options.forceFallbackAdapter = WGPU_FALSE;
//     return navigator_gpu_request_adapter_async(&options, adapterCallback, userData);
// }

EMSCRIPTEN_KEEPALIVE void* gpuCreateInstance() {
    WGPUInstanceDescriptor descriptor;
    descriptor.nextInChain = NULL;
    
    return wgpuCreateInstance(&descriptor);
}

EMSCRIPTEN_KEEPALIVE void gpuInstanceRequestAdapter(WGPUInstance instance, WGPURequestAdapterOptions const * options, WGPURequestAdapterCallback callback, void * userdata) {
    wgpuInstanceRequestAdapter(instance, options, callback, userdata);
}

EMSCRIPTEN_KEEPALIVE int wgpuSupported()
{
    return EM_ASM_INT({
        return (typeof navigator.gpu !== 'undefined' ? 1 : 0);
    });
}

EMSCRIPTEN_KEEPALIVE uint32_t my_wgpu_get_num_live_objects(void) {
    return wgpu_get_num_live_objects();
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_object_destroy(WGpuObjectBase wgpuObject) {
    wgpu_object_destroy(wgpuObject);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_destroy_all_objects(void) {
    wgpu_destroy_all_objects();
}

EMSCRIPTEN_KEEPALIVE WGpuCanvasContext my_wgpu_canvas_get_webgpu_context(const char *canvasSelector) {
    return wgpu_canvas_get_webgpu_context(canvasSelector);
}

EMSCRIPTEN_KEEPALIVE WGpuCanvasContext my_wgpu_offscreen_canvas_get_webgpu_context(OffscreenCanvasId id) {
    return wgpu_offscreen_canvas_get_webgpu_context(id);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_valid_object(WGpuObjectBase obj) {
    return wgpu_is_valid_object(obj);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_object_set_label(WGpuObjectBase obj, const char *label) {
    wgpu_object_set_label(obj, label);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_object_get_label(WGpuObjectBase obj, char *dstLabel, uint32_t dstLabelSize) {
    return wgpu_object_get_label(obj, dstLabel, dstLabelSize);
}

EMSCRIPTEN_KEEPALIVE int my_navigator_gpu_available(void) {
    return navigator_gpu_available();
}

EMSCRIPTEN_KEEPALIVE void my_navigator_delete_webgpu_api_access(void) {
    navigator_delete_webgpu_api_access();
}

EMSCRIPTEN_KEEPALIVE int my_navigator_gpu_request_adapter_async(const WGpuRequestAdapterOptions *options, WGpuRequestAdapterCallback adapterCallback, void *userData) {
    return navigator_gpu_request_adapter_async(options, adapterCallback, userData);
}

EMSCRIPTEN_KEEPALIVE WGpuAdapter my_navigator_gpu_request_adapter_sync(const WGpuRequestAdapterOptions *options) {
    return navigator_gpu_request_adapter_sync(options);
}

EMSCRIPTEN_KEEPALIVE void my_navigator_gpu_request_adapter_async_simple(WGpuRequestAdapterCallback adapterCallback) {
    navigator_gpu_request_adapter_async_simple(adapterCallback);
}

EMSCRIPTEN_KEEPALIVE WGpuAdapter my_navigator_gpu_request_adapter_sync_simple(void) {
    return navigator_gpu_request_adapter_sync_simple();
}

EMSCRIPTEN_KEEPALIVE WGPU_TEXTURE_FORMAT my_navigator_gpu_get_preferred_canvas_format(void) {
    return navigator_gpu_get_preferred_canvas_format();
}

EMSCRIPTEN_KEEPALIVE const char *const *my_navigator_gpu_get_wgsl_language_features(void) {
    return navigator_gpu_get_wgsl_language_features();
}

EMSCRIPTEN_KEEPALIVE int my_navigator_gpu_is_wgsl_language_feature_supported(const char *feature) {
    return navigator_gpu_is_wgsl_language_feature_supported(feature);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_adapter(WGpuObjectBase object) {
    return wgpu_is_adapter(object);
}

EMSCRIPTEN_KEEPALIVE WGPU_FEATURES_BITFIELD my_wgpu_adapter_or_device_get_features(WGpuAdapter adapter) {
    return wgpu_adapter_or_device_get_features(adapter);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_adapter_or_device_supports_feature(WGpuAdapter adapter, WGPU_FEATURES_BITFIELD feature) {
    return wgpu_adapter_or_device_supports_feature(adapter, feature);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_adapter_or_device_get_limits(WGpuAdapter adapter, WGpuSupportedLimits *limits) {
    wgpu_adapter_or_device_get_limits(adapter, limits);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_adapter_get_info(WGpuAdapter adapter, WGpuAdapterInfo *adapterInfo) {
    wgpu_adapter_get_info(adapter, adapterInfo);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_adapter_is_fallback_adapter(WGpuAdapter adapter) {
    return wgpu_adapter_is_fallback_adapter(adapter);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_adapter_request_device_async(WGpuAdapter adapter, const WGpuDeviceDescriptor *descriptor, WGpuRequestDeviceCallback deviceCallback, void *userData) {
    wgpu_adapter_request_device_async(adapter, descriptor, deviceCallback, userData);
}

EMSCRIPTEN_KEEPALIVE WGpuDevice my_wgpu_adapter_request_device_sync(WGpuAdapter adapter, const WGpuDeviceDescriptor *descriptor) {
    return wgpu_adapter_request_device_sync(adapter, descriptor);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_adapter_request_device_async_simple(WGpuAdapter adapter, WGpuRequestDeviceCallback deviceCallback) {
    wgpu_adapter_request_device_async_simple(adapter, deviceCallback);
}

EMSCRIPTEN_KEEPALIVE WGpuDevice my_wgpu_adapter_request_device_sync_simple(WGpuAdapter adapter) {
    return wgpu_adapter_request_device_sync_simple(adapter);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_device(WGpuObjectBase object) {
    return wgpu_is_device(object);
}

EMSCRIPTEN_KEEPALIVE WGpuQueue my_wgpu_device_get_queue(WGpuDevice device) {
    return wgpu_device_get_queue(device);
}

EMSCRIPTEN_KEEPALIVE WGpuBuffer my_wgpu_device_create_buffer(WGpuDevice device, const WGpuBufferDescriptor *bufferDesc) {
    return wgpu_device_create_buffer(device, bufferDesc);
}

EMSCRIPTEN_KEEPALIVE WGpuTexture my_wgpu_device_create_texture(WGpuDevice device, const WGpuTextureDescriptor *textureDesc) {
    return wgpu_device_create_texture(device, textureDesc);
}

EMSCRIPTEN_KEEPALIVE WGpuSampler my_wgpu_device_create_sampler(WGpuDevice device, const WGpuSamplerDescriptor *samplerDesc) {
    return wgpu_device_create_sampler(device, samplerDesc);
}

EMSCRIPTEN_KEEPALIVE WGpuExternalTexture my_wgpu_device_import_external_texture(WGpuDevice device, const WGpuExternalTextureDescriptor *externalTextureDesc) {
    return wgpu_device_import_external_texture(device, externalTextureDesc);
}

EMSCRIPTEN_KEEPALIVE WGpuBindGroupLayout my_wgpu_device_create_bind_group_layout(WGpuDevice device, const WGpuBindGroupLayoutEntry *bindGroupLayoutEntries, int numEntries) {
    return wgpu_device_create_bind_group_layout(device, bindGroupLayoutEntries, numEntries);
}

EMSCRIPTEN_KEEPALIVE WGpuPipelineLayout my_wgpu_device_create_pipeline_layout(WGpuDevice device, const WGpuBindGroupLayout *bindGroupLayouts, int numLayouts) {
    return wgpu_device_create_pipeline_layout(device, bindGroupLayouts, numLayouts);
}

EMSCRIPTEN_KEEPALIVE WGpuBindGroup my_wgpu_device_create_bind_group(WGpuDevice device, WGpuBindGroupLayout bindGroupLayout, const WGpuBindGroupEntry *entries, int numEntries) {
    return wgpu_device_create_bind_group(device, bindGroupLayout, entries, numEntries);
}

EMSCRIPTEN_KEEPALIVE WGpuShaderModule my_wgpu_device_create_shader_module(WGpuDevice device, const WGpuShaderModuleDescriptor *shaderModuleDesc) {
    return wgpu_device_create_shader_module(device, shaderModuleDesc);
}

EMSCRIPTEN_KEEPALIVE WGpuComputePipeline my_wgpu_device_create_compute_pipeline(WGpuDevice device, WGpuShaderModule computeModule, const char *entryPoint, WGpuPipelineLayout layout, const WGpuPipelineConstant *constants, int numConstants) {
    return wgpu_device_create_compute_pipeline(device, computeModule, entryPoint, layout, constants, numConstants);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_device_create_compute_pipeline_async(WGpuDevice device, WGpuShaderModule computeModule, const char *entryPoint, WGpuPipelineLayout layout, const WGpuPipelineConstant *constants, int numConstants, WGpuCreatePipelineCallback callback, void *userData) {
    wgpu_device_create_compute_pipeline_async(device, computeModule, entryPoint, layout, constants, numConstants, callback, userData);
}

EMSCRIPTEN_KEEPALIVE WGpuRenderPipeline my_wgpu_device_create_render_pipeline(WGpuDevice device, const WGpuRenderPipelineDescriptor *renderPipelineDesc) {
    return wgpu_device_create_render_pipeline(device, renderPipelineDesc);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_device_create_render_pipeline_async(WGpuDevice device, const WGpuRenderPipelineDescriptor *renderPipelineDesc, WGpuCreatePipelineCallback callback, void *userData) {
    wgpu_device_create_render_pipeline_async(device, renderPipelineDesc, callback, userData);
}

EMSCRIPTEN_KEEPALIVE WGpuCommandEncoder my_wgpu_device_create_command_encoder(WGpuDevice device, const WGpuCommandEncoderDescriptor *commandEncoderDesc) {
    return wgpu_device_create_command_encoder(device, commandEncoderDesc);
}

EMSCRIPTEN_KEEPALIVE WGpuCommandEncoder my_wgpu_device_create_command_encoder_simple(WGpuDevice device) {
    return wgpu_device_create_command_encoder_simple(device);
}

EMSCRIPTEN_KEEPALIVE WGpuRenderBundleEncoder my_wgpu_device_create_render_bundle_encoder(WGpuDevice device, const WGpuRenderBundleEncoderDescriptor *renderBundleEncoderDesc) {
    return wgpu_device_create_render_bundle_encoder(device, renderBundleEncoderDesc);
}

EMSCRIPTEN_KEEPALIVE WGpuQuerySet my_wgpu_device_create_query_set(WGpuDevice device, const WGpuQuerySetDescriptor *querySetDesc) {
    return wgpu_device_create_query_set(device, querySetDesc);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_buffer(WGpuObjectBase object) {
    return wgpu_is_buffer(object);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_buffer_map_async(WGpuBuffer buffer, WGpuBufferMapCallback callback, void *userData, WGPU_MAP_MODE_FLAGS mode, double_int53_t offset, double_int53_t size) {
    wgpu_buffer_map_async(buffer, callback, userData, mode, offset, size);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_buffer_map_sync(WGpuBuffer buffer, WGPU_MAP_MODE_FLAGS mode, double_int53_t offset, double_int53_t size) {
    wgpu_buffer_map_sync(buffer, mode, offset, size);
}

EMSCRIPTEN_KEEPALIVE double_int53_t my_wgpu_buffer_get_mapped_range(WGpuBuffer buffer, double_int53_t startOffset, double_int53_t size) {
    return wgpu_buffer_get_mapped_range(buffer, startOffset, size);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_buffer_read_mapped_range(WGpuBuffer buffer, double_int53_t startOffset, double_int53_t subOffset, void *dst, double_int53_t size) {
    wgpu_buffer_read_mapped_range(buffer, startOffset, subOffset, dst, size);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_buffer_write_mapped_range(WGpuBuffer buffer, double_int53_t startOffset, double_int53_t subOffset, const void *src, double_int53_t size) {
    wgpu_buffer_write_mapped_range(buffer, startOffset, subOffset, src, size);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_buffer_unmap(WGpuBuffer buffer) {
    wgpu_buffer_unmap(buffer);
}

EMSCRIPTEN_KEEPALIVE double_int53_t my_wgpu_buffer_size(WGpuBuffer buffer) {
    return wgpu_buffer_size(buffer);
}

EMSCRIPTEN_KEEPALIVE WGPU_BUFFER_USAGE_FLAGS my_wgpu_buffer_usage(WGpuBuffer buffer) {
    return wgpu_buffer_usage(buffer);
}

EMSCRIPTEN_KEEPALIVE WGPU_BUFFER_MAP_STATE my_wgpu_buffer_map_state(WGpuBuffer buffer) {
    return wgpu_buffer_map_state(buffer);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_texture(WGpuObjectBase object) {
    return wgpu_is_texture(object);
}

EMSCRIPTEN_KEEPALIVE WGpuTextureView my_wgpu_texture_create_view(WGpuTexture texture, const WGpuTextureViewDescriptor *textureViewDesc) {
    return wgpu_texture_create_view(texture, textureViewDesc);
}

EMSCRIPTEN_KEEPALIVE WGpuTextureView my_wgpu_texture_create_view_simple(WGpuTexture texture) {
    return wgpu_texture_create_view_simple(texture);
}

EMSCRIPTEN_KEEPALIVE uint32_t my_wgpu_texture_width(WGpuTexture texture) {
    return wgpu_texture_width(texture);
}

EMSCRIPTEN_KEEPALIVE uint32_t my_wgpu_texture_height(WGpuTexture texture) {
    return wgpu_texture_height(texture);
}

EMSCRIPTEN_KEEPALIVE uint32_t my_wgpu_texture_depth_or_array_layers(WGpuTexture texture) {
    return wgpu_texture_depth_or_array_layers(texture);
}

EMSCRIPTEN_KEEPALIVE uint32_t my_wgpu_texture_mip_level_count(WGpuTexture texture) {
    return wgpu_texture_mip_level_count(texture);
}

EMSCRIPTEN_KEEPALIVE uint32_t my_wgpu_texture_sample_count(WGpuTexture texture) {
    return wgpu_texture_sample_count(texture);
}

EMSCRIPTEN_KEEPALIVE WGPU_TEXTURE_DIMENSION my_wgpu_texture_dimension(WGpuTexture texture) {
    return wgpu_texture_dimension(texture);
}

EMSCRIPTEN_KEEPALIVE WGPU_TEXTURE_FORMAT my_wgpu_texture_format(WGpuTexture texture) {
    return wgpu_texture_format(texture);
}

EMSCRIPTEN_KEEPALIVE WGPU_TEXTURE_USAGE_FLAGS my_wgpu_texture_usage(WGpuTexture texture) {
    return wgpu_texture_usage(texture);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_texture_view(WGpuObjectBase object) {
    return wgpu_is_texture_view(object);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_external_texture(WGpuObjectBase object) {
    return wgpu_is_external_texture(object);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_sampler(WGpuObjectBase object) {
    return wgpu_is_sampler(object);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_bind_group_layout(WGpuObjectBase object) {
    return wgpu_is_bind_group_layout(object);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_bind_group(WGpuObjectBase object) {
    return wgpu_is_bind_group(object);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_pipeline_layout(WGpuObjectBase object) {
    return wgpu_is_pipeline_layout(object);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_shader_module(WGpuObjectBase object) {
    return wgpu_is_shader_module(object);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_shader_module_get_compilation_info_async(WGpuShaderModule shaderModule, WGpuGetCompilationInfoCallback callback, void *userData) {
    wgpu_shader_module_get_compilation_info_async(shaderModule, callback, userData);
}

EMSCRIPTEN_KEEPALIVE const char *my_wgpu_compilation_message_type_to_string(WGPU_COMPILATION_MESSAGE_TYPE type) {
    return wgpu_compilation_message_type_to_string(type);
}

EMSCRIPTEN_KEEPALIVE WGpuBindGroupLayout my_wgpu_pipeline_get_bind_group_layout(WGpuObjectBase pipelineBase, uint32_t index) {
    return wgpu_pipeline_get_bind_group_layout(pipelineBase, index);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_compute_pipeline(WGpuObjectBase object) {
    return wgpu_is_compute_pipeline(object);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_render_pipeline(WGpuObjectBase object) {
    return wgpu_is_render_pipeline(object);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_command_buffer(WGpuObjectBase object) {
    return wgpu_is_command_buffer(object);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_encoder_push_debug_group(WGpuDebugCommandsMixin encoder, const char *groupLabel) {
    wgpu_encoder_push_debug_group(encoder, groupLabel);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_encoder_pop_debug_group(WGpuDebugCommandsMixin encoder) {
    wgpu_encoder_pop_debug_group(encoder);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_encoder_insert_debug_marker(WGpuDebugCommandsMixin encoder, const char *markerLabel) {
    wgpu_encoder_insert_debug_marker(encoder, markerLabel);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_command_encoder(WGpuObjectBase object) {
    return wgpu_is_command_encoder(object);
}

EMSCRIPTEN_KEEPALIVE WGpuRenderPassEncoder my_wgpu_command_encoder_begin_render_pass(WGpuCommandEncoder commandEncoder, const WGpuRenderPassDescriptor *renderPassDesc) {
    return wgpu_command_encoder_begin_render_pass(commandEncoder, renderPassDesc);
}

EMSCRIPTEN_KEEPALIVE WGpuComputePassEncoder my_wgpu_command_encoder_begin_compute_pass(WGpuCommandEncoder commandEncoder, const WGpuComputePassDescriptor *computePassDesc) {
    return wgpu_command_encoder_begin_compute_pass(commandEncoder, computePassDesc);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_command_encoder_copy_buffer_to_buffer(WGpuCommandEncoder commandEncoder, WGpuBuffer source, double_int53_t sourceOffset, WGpuBuffer destination, double_int53_t destinationOffset, double_int53_t size) {
    wgpu_command_encoder_copy_buffer_to_buffer(commandEncoder, source, sourceOffset, destination, destinationOffset, size);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_command_encoder_copy_buffer_to_texture(WGpuCommandEncoder commandEncoder, const WGpuImageCopyBuffer *source, const WGpuImageCopyTexture *destination, uint32_t copyWidth, uint32_t copyHeight, uint32_t copyDepthOrArrayLayers) {
    wgpu_command_encoder_copy_buffer_to_texture(commandEncoder, source, destination, copyWidth, copyHeight, copyDepthOrArrayLayers);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_command_encoder_copy_texture_to_buffer(WGpuCommandEncoder commandEncoder, const WGpuImageCopyTexture *source, const WGpuImageCopyBuffer *destination, uint32_t copyWidth, uint32_t copyHeight, uint32_t copyDepthOrArrayLayers) {
    wgpu_command_encoder_copy_texture_to_buffer(commandEncoder, source, destination, copyWidth, copyHeight, copyDepthOrArrayLayers);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_command_encoder_copy_texture_to_texture(WGpuCommandEncoder commandEncoder, const WGpuImageCopyTexture *source, const WGpuImageCopyTexture *destination, uint32_t copyWidth, uint32_t copyHeight, uint32_t copyDepthOrArrayLayers) {
    wgpu_command_encoder_copy_texture_to_texture(commandEncoder, source, destination, copyWidth, copyHeight, copyDepthOrArrayLayers);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_command_encoder_clear_buffer(WGpuCommandEncoder commandEncoder, WGpuBuffer buffer, double_int53_t offset, double_int53_t size) {
    wgpu_command_encoder_clear_buffer(commandEncoder, buffer, offset, size);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_command_encoder_resolve_query_set(WGpuCommandEncoder commandEncoder, WGpuQuerySet querySet, uint32_t firstQuery, uint32_t queryCount, WGpuBuffer destination, double_int53_t destinationOffset) {
    wgpu_command_encoder_resolve_query_set(commandEncoder, querySet, firstQuery, queryCount, destination, destinationOffset);
}

EMSCRIPTEN_KEEPALIVE WGpuObjectBase my_wgpu_encoder_finish(WGpuObjectBase commandOrRenderBundleEncoder) {
    return wgpu_encoder_finish(commandOrRenderBundleEncoder);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_binding_commands_mixin(WGpuObjectBase object) {
    return wgpu_is_binding_commands_mixin(object);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_encoder_set_bind_group(WGpuBindingCommandsMixin encoder, uint32_t index, WGpuBindGroup bindGroup, const uint32_t *dynamicOffsets, uint32_t numDynamicOffsets) {
    wgpu_encoder_set_bind_group(encoder, index, bindGroup, dynamicOffsets, numDynamicOffsets);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_encoder_set_pipeline(WGpuBindingCommandsMixin encoder, WGpuObjectBase pipeline) {
    wgpu_encoder_set_pipeline(encoder, pipeline);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_encoder_end(WGpuBindingCommandsMixin encoder) {
    wgpu_encoder_end(encoder);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_compute_pass_encoder(WGpuObjectBase object) {
    return wgpu_is_compute_pass_encoder(object);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_compute_pass_encoder_dispatch_workgroups(WGpuComputePassEncoder encoder, uint32_t workgroupCountX, uint32_t workgroupCountY, uint32_t workgroupCountZ) {
    wgpu_compute_pass_encoder_dispatch_workgroups(encoder, workgroupCountX, workgroupCountY, workgroupCountZ);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_compute_pass_encoder_dispatch_workgroups_indirect(WGpuComputePassEncoder encoder, WGpuBuffer indirectBuffer, double_int53_t indirectOffset) {
    wgpu_compute_pass_encoder_dispatch_workgroups_indirect(encoder, indirectBuffer, indirectOffset);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_render_commands_mixin(WGpuObjectBase object) {
    return wgpu_is_render_commands_mixin(object);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_render_commands_mixin_set_index_buffer(WGpuRenderCommandsMixin renderCommandsMixin, WGpuBuffer buffer, WGPU_INDEX_FORMAT indexFormat, double_int53_t offset, double_int53_t size) {
    wgpu_render_commands_mixin_set_index_buffer(renderCommandsMixin, buffer, indexFormat, offset, size);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_render_commands_mixin_set_vertex_buffer(WGpuRenderCommandsMixin renderCommandsMixin, int32_t slot, WGpuBuffer buffer, double_int53_t offset, double_int53_t size) {
    wgpu_render_commands_mixin_set_vertex_buffer(renderCommandsMixin, slot, buffer, offset, size);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_render_commands_mixin_draw(WGpuRenderCommandsMixin renderCommandsMixin, uint32_t vertexCount, uint32_t instanceCount, uint32_t firstVertex, uint32_t firstInstance) {
    wgpu_render_commands_mixin_draw(renderCommandsMixin, vertexCount, instanceCount, firstVertex, firstInstance);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_render_commands_mixin_draw_indexed(WGpuRenderCommandsMixin renderCommandsMixin, uint32_t indexCount, uint32_t instanceCount, uint32_t firstVertex, int32_t baseVertex, uint32_t firstInstance) {
    wgpu_render_commands_mixin_draw_indexed(renderCommandsMixin, indexCount, instanceCount, firstVertex, baseVertex, firstInstance);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_render_commands_mixin_draw_indirect(WGpuRenderCommandsMixin renderCommandsMixin, WGpuBuffer indirectBuffer, double_int53_t indirectOffset) {
    wgpu_render_commands_mixin_draw_indirect(renderCommandsMixin, indirectBuffer, indirectOffset);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_render_commands_mixin_draw_indexed_indirect(WGpuRenderCommandsMixin renderCommandsMixin, WGpuBuffer indirectBuffer, double_int53_t indirectOffset) {
    wgpu_render_commands_mixin_draw_indexed_indirect(renderCommandsMixin, indirectBuffer, indirectOffset);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_render_pass_encoder(WGpuObjectBase object) {
    return wgpu_is_render_pass_encoder(object);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_render_pass_encoder_set_viewport(WGpuRenderPassEncoder encoder, float x, float y, float width, float height, float minDepth, float maxDepth) {
    wgpu_render_pass_encoder_set_viewport(encoder, x, y, width, height, minDepth, maxDepth);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_render_pass_encoder_set_scissor_rect(WGpuRenderPassEncoder encoder, uint32_t x, uint32_t y, uint32_t width, uint32_t height) {
    wgpu_render_pass_encoder_set_scissor_rect(encoder, x, y, width, height);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_render_pass_encoder_set_blend_constant(WGpuRenderPassEncoder encoder, double r, double g, double b, double a) {
    wgpu_render_pass_encoder_set_blend_constant(encoder, r, g, b, a);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_render_pass_encoder_set_stencil_reference(WGpuRenderPassEncoder encoder, uint32_t stencilValue) {
    wgpu_render_pass_encoder_set_stencil_reference(encoder, stencilValue);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_render_pass_encoder_begin_occlusion_query(WGpuRenderPassEncoder encoder, int32_t queryIndex) {
    wgpu_render_pass_encoder_begin_occlusion_query(encoder, queryIndex);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_render_pass_encoder_end_occlusion_query(WGpuRenderPassEncoder encoder) {
    wgpu_render_pass_encoder_end_occlusion_query(encoder);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_render_pass_encoder_execute_bundles(WGpuRenderPassEncoder encoder, const WGpuRenderBundle *bundles, int numBundles) {
    wgpu_render_pass_encoder_execute_bundles(encoder, bundles, numBundles);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_render_bundle(WGpuObjectBase object) {
    return wgpu_is_render_bundle(object);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_render_bundle_encoder(WGpuObjectBase object) {
    return wgpu_is_render_bundle_encoder(object);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_queue(WGpuObjectBase object) {
    return wgpu_is_queue(object);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_queue_submit_one_and_destroy(WGpuQueue queue, WGpuCommandBuffer commandBuffer) {
    wgpu_queue_submit_one_and_destroy(queue, commandBuffer);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_queue_submit_multiple_and_destroy(WGpuQueue queue, const WGpuCommandBuffer *commandBuffers, int numCommandBuffers) {
    wgpu_queue_submit_multiple_and_destroy(queue, commandBuffers, numCommandBuffers);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_queue_set_on_submitted_work_done_callback(WGpuQueue queue, WGpuOnSubmittedWorkDoneCallback callback, void *userData) {
    wgpu_queue_set_on_submitted_work_done_callback(queue, callback, userData);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_queue_write_buffer(WGpuQueue queue, WGpuBuffer buffer, int64_t* bufferOffset, const void *data, int64_t* size) {
    wgpu_queue_write_buffer(queue, buffer, (double)*bufferOffset, data, (double)*size);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_queue_write_texture(WGpuQueue queue, const WGpuImageCopyTexture *destination, const void *data, uint32_t bytesPerBlockRow, uint32_t blockRowsPerImage, uint32_t writeWidth, uint32_t writeHeight, uint32_t writeDepthOrArrayLayers) {
    wgpu_queue_write_texture(queue, destination, data, bytesPerBlockRow, blockRowsPerImage, writeWidth, writeHeight, writeDepthOrArrayLayers);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_queue_copy_external_image_to_texture(WGpuQueue queue, const WGpuImageCopyExternalImage *source, const WGpuImageCopyTextureTagged *destination, uint32_t copyWidth, uint32_t copyHeight, uint32_t copyDepthOrArrayLayers) {
    wgpu_queue_copy_external_image_to_texture(queue, source, destination, copyWidth, copyHeight, copyDepthOrArrayLayers);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_query_set(WGpuObjectBase object) {
    return wgpu_is_query_set(object);
}

EMSCRIPTEN_KEEPALIVE WGPU_QUERY_TYPE my_wgpu_query_set_type(WGpuQuerySet querySet) {
    return wgpu_query_set_type(querySet);
}

EMSCRIPTEN_KEEPALIVE uint32_t my_wgpu_query_set_count(WGpuQuerySet querySet) {
    return wgpu_query_set_count(querySet);
}

EMSCRIPTEN_KEEPALIVE int my_wgpu_is_canvas_context(WGpuObjectBase object) {
    return wgpu_is_canvas_context(object);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_canvas_context_configure(WGpuCanvasContext canvasContext, const WGpuCanvasConfiguration *config) {
    wgpu_canvas_context_configure(canvasContext, config);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_canvas_context_unconfigure(WGpuCanvasContext canvasContext) {
    wgpu_canvas_context_unconfigure(canvasContext);
}

EMSCRIPTEN_KEEPALIVE WGpuTexture my_wgpu_canvas_context_get_current_texture(WGpuCanvasContext canvasContext) {
    return wgpu_canvas_context_get_current_texture(canvasContext);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_canvas_context_present(WGpuCanvasContext canvasContext) {
    wgpu_canvas_context_present(canvasContext);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_device_set_lost_callback(WGpuDevice device, WGpuDeviceLostCallback callback, void *userData) {
    wgpu_device_set_lost_callback(device, callback, userData);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_device_push_error_scope(WGpuDevice device, WGPU_ERROR_FILTER filter) {
    wgpu_device_push_error_scope(device, filter);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_device_pop_error_scope_async(WGpuDevice device, WGpuDeviceErrorCallback callback, void *userData) {
    wgpu_device_pop_error_scope_async(device, callback, userData);
}

EMSCRIPTEN_KEEPALIVE WGPU_ERROR_TYPE my_wgpu_device_pop_error_scope_sync(WGpuDevice device, char *dstErrorMessage, int errorMessageLength) {
    return wgpu_device_pop_error_scope_sync(device, dstErrorMessage, errorMessageLength);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_device_set_uncapturederror_callback(WGpuDevice device, WGpuDeviceErrorCallback callback, void *userData) {
    wgpu_device_set_uncapturederror_callback(device, callback, userData);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_load_image_bitmap_from_url_async(const char *url, int flipY, WGpuLoadImageBitmapCallback callback, void *userData) {
    wgpu_load_image_bitmap_from_url_async(url, flipY, callback, userData);
}

EMSCRIPTEN_KEEPALIVE void my_wgpu_present_all_rendering_and_wait_for_next_animation_frame(void) {
    wgpu_present_all_rendering_and_wait_for_next_animation_frame();
}

EMSCRIPTEN_KEEPALIVE void my_offscreen_canvas_create(OffscreenCanvasId id, int width, int height) {
    offscreen_canvas_create(id, width, height);
}

EMSCRIPTEN_KEEPALIVE void my_canvas_transfer_control_to_offscreen(const char *canvasSelector, OffscreenCanvasId id) {
    canvas_transfer_control_to_offscreen(canvasSelector, id);
}

EMSCRIPTEN_KEEPALIVE void my_offscreen_canvas_post_to_worker(OffscreenCanvasId id, int worker) {
    offscreen_canvas_post_to_worker(id, worker);
}

EMSCRIPTEN_KEEPALIVE void my_offscreen_canvas_post_to_pthread(OffscreenCanvasId id, pthread_t pthread) {
    offscreen_canvas_post_to_pthread(id, pthread);
}

EMSCRIPTEN_KEEPALIVE int my_offscreen_canvas_is_valid(OffscreenCanvasId id) {
    return offscreen_canvas_is_valid(id);
}

EMSCRIPTEN_KEEPALIVE void my_offscreen_canvas_destroy(OffscreenCanvasId id) {
    offscreen_canvas_destroy(id);
}

EMSCRIPTEN_KEEPALIVE int my_offscreen_canvas_width(OffscreenCanvasId id) {
    return offscreen_canvas_width(id);
}

EMSCRIPTEN_KEEPALIVE int my_offscreen_canvas_height(OffscreenCanvasId id) {
    return offscreen_canvas_height(id);
}

EMSCRIPTEN_KEEPALIVE void my_offscreen_canvas_size(OffscreenCanvasId id, int *outWidth, int *outHeight) {
    offscreen_canvas_size(id, outWidth, outHeight);
}

EMSCRIPTEN_KEEPALIVE void my_offscreen_canvas_set_size(OffscreenCanvasId id, int width, int height) {
    offscreen_canvas_set_size(id, width, height);
}