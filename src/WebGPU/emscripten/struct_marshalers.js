/**
 * WebGPU Struct Marshalers for Emscripten
 *
 * This file contains helper functions for marshaling C structs to JavaScript objects
 * and vice versa. Each struct from dawn.json gets a marshaler function.
 *
 * Note: Struct offsets must match the C layout. In production, these should be
 * auto-generated or derived from Emscripten's generateStructInfo.
 */

var WebGPUStructMarshalers = {
    /**
     * Read an optional string from a pointer
     */
    readOptionalString: function(ptr) {
        if (!ptr) return undefined;
        return UTF8ToString(ptr);
    },

    /**
     * Read a WGPUStringView struct
     * struct WGPUStringView {
     *     const char* data;
     *     size_t length;
     * }
     */
    readStringView: function(ptr) {
        if (!ptr) return null;
        var dataPtr = {{{ makeGetValue('ptr', 0, '*') }}};
        var length = {{{ makeGetValue('ptr', 4, 'i32') }}};
        if (!dataPtr) {
            return length === 0 ? "" : undefined;
        }
        return UTF8ToString(dataPtr, length);
    },

    /**
     * Read a WGPUColor struct
     * struct WGPUColor {
     *     double r;
     *     double g;
     *     double b;
     *     double a;
     * }
     */
    readColor: function(ptr) {
        if (!ptr) return undefined;
        return {
            r: {{{ makeGetValue('ptr', 0, 'double') }}},
            g: {{{ makeGetValue('ptr', 8, 'double') }}},
            b: {{{ makeGetValue('ptr', 16, 'double') }}},
            a: {{{ makeGetValue('ptr', 24, 'double') }}}
        };
    },

    /**
     * Read a WGPUExtent3D struct
     * struct WGPUExtent3D {
     *     uint32_t width;
     *     uint32_t height;
     *     uint32_t depthOrArrayLayers;
     * }
     */
    readExtent3D: function(ptr) {
        if (!ptr) return undefined;
        return {
            width: {{{ makeGetValue('ptr', 0, 'i32') }}},
            height: {{{ makeGetValue('ptr', 4, 'i32') }}},
            depthOrArrayLayers: {{{ makeGetValue('ptr', 8, 'i32') }}}
        };
    },

    /**
     * Read a WGPUOrigin3D struct
     * struct WGPUOrigin3D {
     *     uint32_t x;
     *     uint32_t y;
     *     uint32_t z;
     * }
     */
    readOrigin3D: function(ptr) {
        if (!ptr) return undefined;
        return {
            x: {{{ makeGetValue('ptr', 0, 'i32') }}},
            y: {{{ makeGetValue('ptr', 4, 'i32') }}},
            z: {{{ makeGetValue('ptr', 8, 'i32') }}}
        };
    },

    /**
     * Read a WGPULimits struct
     * This is a large struct with many fields - showing a subset
     */
    readLimits: function(ptr) {
        if (!ptr) return undefined;
        var offset = 0;
        return {
            maxTextureDimension1D: {{{ makeGetValue('ptr', 'offset', 'i32') }}},
            maxTextureDimension2D: {{{ makeGetValue('ptr', 'offset + 4', 'i32') }}},
            maxTextureDimension3D: {{{ makeGetValue('ptr', 'offset + 8', 'i32') }}},
            maxTextureArrayLayers: {{{ makeGetValue('ptr', 'offset + 12', 'i32') }}},
            // ... add more fields as needed
        };
    },

    /**
     * Read a WGPUBufferDescriptor struct
     * struct WGPUBufferDescriptor {
     *     WGPUChainedStruct* nextInChain;
     *     const char* label;
     *     WGPUBufferUsageFlags usage;
     *     uint64_t size;
     *     int mappedAtCreation;
     * }
     */
    readBufferDescriptor: function(ptr) {
        if (!ptr) return undefined;

        var descriptor = {};
        var offset = 0;

        // nextInChain (pointer) - skip for now
        offset += {{{ POINTER_SIZE }}};

        // label (pointer to string)
        var labelPtr = {{{ makeGetValue('ptr', 'offset', '*') }}};
        if (labelPtr) {
            descriptor.label = UTF8ToString(labelPtr);
        }
        offset += {{{ POINTER_SIZE }}};

        // usage (uint32_t)
        descriptor.usage = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        offset += 4;

        // Alignment padding on 64-bit
        if ({{{ POINTER_SIZE }}} === 8) {
            offset += 4;
        }

        // size (uint64_t)
        var sizeLow = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        var sizeHigh = {{{ makeGetValue('ptr', 'offset + 4', 'i32') }}};
        descriptor.size = sizeLow + sizeHigh * 0x100000000;
        offset += 8;

        // mappedAtCreation (bool as int)
        descriptor.mappedAtCreation = !!{{{ makeGetValue('ptr', 'offset', 'i32') }}};

        return descriptor;
    },

    /**
     * Read a WGPUTextureDescriptor struct
     */
    readTextureDescriptor: function(ptr) {
        if (!ptr) return undefined;

        var descriptor = {};
        var offset = 0;

        // nextInChain
        offset += {{{ POINTER_SIZE }}};

        // label
        var labelPtr = {{{ makeGetValue('ptr', 'offset', '*') }}};
        if (labelPtr) {
            descriptor.label = UTF8ToString(labelPtr);
        }
        offset += {{{ POINTER_SIZE }}};

        // usage
        descriptor.usage = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        offset += 4;

        // dimension
        descriptor.dimension = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        offset += 4;

        // size (Extent3D)
        var sizePtr = ptr + offset;
        descriptor.size = [
            {{{ makeGetValue('sizePtr', 0, 'i32') }}},
            {{{ makeGetValue('sizePtr', 4, 'i32') }}},
            {{{ makeGetValue('sizePtr', 8, 'i32') }}}
        ];
        offset += 12;

        // format
        descriptor.format = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        offset += 4;

        // mipLevelCount
        descriptor.mipLevelCount = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        offset += 4;

        // sampleCount
        descriptor.sampleCount = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        offset += 4;

        // viewFormatCount
        var viewFormatCount = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        offset += 4;

        // viewFormats (pointer to array)
        var viewFormatsPtr = {{{ makeGetValue('ptr', 'offset', '*') }}};
        if (viewFormatsPtr && viewFormatCount > 0) {
            descriptor.viewFormats = [];
            for (var i = 0; i < viewFormatCount; i++) {
                descriptor.viewFormats.push({{{ makeGetValue('viewFormatsPtr', 'i * 4', 'i32') }}});
            }
        }

        return descriptor;
    },

    /**
     * Read a WGPUShaderModuleDescriptor struct
     * This contains a chain for different shader sources (WGSL, SPIRV, etc.)
     */
    readShaderModuleDescriptor: function(ptr) {
        if (!ptr) return undefined;

        var descriptor = {};
        var offset = 0;

        // nextInChain - need to walk the chain to find shader source
        var chainPtr = {{{ makeGetValue('ptr', 'offset', '*') }}};
        offset += {{{ POINTER_SIZE }}};

        // label
        var labelPtr = {{{ makeGetValue('ptr', 'offset', '*') }}};
        if (labelPtr) {
            descriptor.label = UTF8ToString(labelPtr);
        }

        // Walk the chain to find WGSL source
        while (chainPtr) {
            var sType = {{{ makeGetValue('chainPtr', POINTER_SIZE, 'i32') }}};

            // WGPUSType_ShaderModuleWGSLDescriptor = ?
            // For now, assume WGSL and read code field
            var codePtr = {{{ makeGetValue('chainPtr', POINTER_SIZE + 4, '*') }}};
            if (codePtr) {
                descriptor.code = UTF8ToString(codePtr);
                break;
            }

            // Next in chain
            chainPtr = {{{ makeGetValue('chainPtr', 0, '*') }}};
        }

        return descriptor;
    },

    /**
     * Read a WGPURenderPassColorAttachment struct
     */
    readRenderPassColorAttachment: function(ptr) {
        if (!ptr) return undefined;

        var attachment = {};
        var offset = 0;

        // nextInChain
        offset += {{{ POINTER_SIZE }}};

        // view (WGPUTextureView handle)
        attachment.view = WebGPUEm.getObject({{{ makeGetValue('ptr', 'offset', 'i32') }}});
        offset += 4;

        // depthSlice
        attachment.depthSlice = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        offset += 4;

        // resolveTarget (WGPUTextureView handle)
        var resolveTarget = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        if (resolveTarget) {
            attachment.resolveTarget = WebGPUEm.getObject(resolveTarget);
        }
        offset += 4;

        // loadOp
        attachment.loadOp = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        offset += 4;

        // storeOp
        attachment.storeOp = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        offset += 4;

        // Alignment
        if ({{{ POINTER_SIZE }}} === 8) {
            offset += 4;
        }

        // clearValue (WGPUColor)
        var colorPtr = ptr + offset;
        attachment.clearValue = {
            r: {{{ makeGetValue('colorPtr', 0, 'double') }}},
            g: {{{ makeGetValue('colorPtr', 8, 'double') }}},
            b: {{{ makeGetValue('colorPtr', 16, 'double') }}},
            a: {{{ makeGetValue('colorPtr', 24, 'double') }}}
        };

        return attachment;
    },

    /**
     * Read a WGPURenderPassDepthStencilAttachment struct
     */
    readRenderPassDepthStencilAttachment: function(ptr) {
        if (!ptr) return undefined;

        var attachment = {};
        var offset = 0;

        // view
        attachment.view = WebGPUEm.getObject({{{ makeGetValue('ptr', 'offset', 'i32') }}});
        offset += 4;

        // depthLoadOp
        attachment.depthLoadOp = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        offset += 4;

        // depthStoreOp
        attachment.depthStoreOp = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        offset += 4;

        // depthClearValue
        attachment.depthClearValue = {{{ makeGetValue('ptr', 'offset', 'float') }}};
        offset += 4;

        // depthReadOnly
        attachment.depthReadOnly = !!{{{ makeGetValue('ptr', 'offset', 'i32') }}};
        offset += 4;

        // stencilLoadOp
        attachment.stencilLoadOp = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        offset += 4;

        // stencilStoreOp
        attachment.stencilStoreOp = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        offset += 4;

        // stencilClearValue
        attachment.stencilClearValue = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        offset += 4;

        // stencilReadOnly
        attachment.stencilReadOnly = !!{{{ makeGetValue('ptr', 'offset', 'i32') }}};

        return attachment;
    },

    /**
     * Read a WGPURenderPassDescriptor struct
     */
    readRenderPassDescriptor: function(ptr) {
        if (!ptr) return undefined;

        var descriptor = {};
        var offset = 0;

        // nextInChain
        offset += {{{ POINTER_SIZE }}};

        // label
        var labelPtr = {{{ makeGetValue('ptr', 'offset', '*') }}};
        if (labelPtr) {
            descriptor.label = UTF8ToString(labelPtr);
        }
        offset += {{{ POINTER_SIZE }}};

        // colorAttachmentCount
        var colorAttachmentCount = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        offset += 4;

        // Alignment
        if ({{{ POINTER_SIZE }}} === 8) {
            offset += 4;
        }

        // colorAttachments (pointer to array)
        var colorAttachmentsPtr = {{{ makeGetValue('ptr', 'offset', '*') }}};
        if (colorAttachmentsPtr && colorAttachmentCount > 0) {
            descriptor.colorAttachments = [];
            // Size of WGPURenderPassColorAttachment struct
            var attachmentSize = 64; // Approximate, should be calculated
            for (var i = 0; i < colorAttachmentCount; i++) {
                var attachmentPtr = colorAttachmentsPtr + i * attachmentSize;
                descriptor.colorAttachments.push(
                    WebGPUStructMarshalers.readRenderPassColorAttachment(attachmentPtr)
                );
            }
        }
        offset += {{{ POINTER_SIZE }}};

        // depthStencilAttachment (pointer)
        var depthStencilPtr = {{{ makeGetValue('ptr', 'offset', '*') }}};
        if (depthStencilPtr) {
            descriptor.depthStencilAttachment =
                WebGPUStructMarshalers.readRenderPassDepthStencilAttachment(depthStencilPtr);
        }
        offset += {{{ POINTER_SIZE }}};

        // occlusionQuerySet
        var occlusionQuerySet = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        if (occlusionQuerySet) {
            descriptor.occlusionQuerySet = WebGPUEm.getObject(occlusionQuerySet);
        }
        offset += 4;

        // timestampWrites (pointer)
        var timestampWritesPtr = {{{ makeGetValue('ptr', 'offset', '*') }}};
        // TODO: Parse timestamp writes if needed

        return descriptor;
    },

    /**
     * Read a WGPUBindGroupEntry struct
     */
    readBindGroupEntry: function(ptr) {
        if (!ptr) return undefined;

        var entry = {};
        var offset = 0;

        // nextInChain
        offset += {{{ POINTER_SIZE }}};

        // binding
        entry.binding = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        offset += 4;

        // buffer (handle)
        var buffer = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
        if (buffer) {
            entry.resource = { buffer: WebGPUEm.getObject(buffer) };

            offset += 4;
            // Alignment
            if ({{{ POINTER_SIZE }}} === 8) {
                offset += 4;
            }

            // offset
            var offsetLow = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
            var offsetHigh = {{{ makeGetValue('ptr', 'offset + 4', 'i32') }}};
            entry.resource.offset = offsetLow + offsetHigh * 0x100000000;
            offset += 8;

            // size
            var sizeLow = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
            var sizeHigh = {{{ makeGetValue('ptr', 'offset + 4', 'i32') }}};
            entry.resource.size = sizeLow + sizeHigh * 0x100000000;
        } else {
            // Could be sampler or texture view
            offset += 4;
            if ({{{ POINTER_SIZE }}} === 8) {
                offset += 4;
            }
            offset += 16; // skip buffer offset/size

            // sampler
            var sampler = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
            if (sampler) {
                entry.resource = WebGPUEm.getObject(sampler);
            }
            offset += 4;

            // textureView
            var textureView = {{{ makeGetValue('ptr', 'offset', 'i32') }}};
            if (textureView) {
                entry.resource = WebGPUEm.getObject(textureView);
            }
        }

        return entry;
    },

    /**
     * Convert WebGPU enum from C to JavaScript string
     */
    enumToString: function(enumValue, enumType) {
        // This would map C enum values to JS strings
        // For now, pass through integer values as browsers accept them
        return enumValue;
    },

    /**
     * Generic array reader
     */
    readArray: function(ptr, count, itemSize, readFunc) {
        if (!ptr || count === 0) return [];
        var arr = [];
        for (var i = 0; i < count; i++) {
            arr.push(readFunc(ptr + i * itemSize));
        }
        return arr;
    }
};

// Export for use in library_webgpu_emscripten.js
if (typeof module !== 'undefined' && module.exports) {
    module.exports = WebGPUStructMarshalers;
}
