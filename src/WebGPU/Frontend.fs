namespace rec WebGPU
open System
open System.Text
open System.Diagnostics
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop
#nowarn "9"
#nowarn "26"
#nowarn "51"
#nowarn "1182"
[<AllowNullLiteral>]
type IExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type IAdapterInfoExtension = inherit IExtension
[<AllowNullLiteral>]
type IBindGroupDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type IBindGroupEntryExtension = inherit IExtension
[<AllowNullLiteral>]
type IBindGroupLayoutDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type IBindGroupLayoutEntryExtension = inherit IExtension
[<AllowNullLiteral>]
type IBufferDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type IColorTargetStateExtension = inherit IExtension
[<AllowNullLiteral>]
type ICommandEncoderDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type ICompilationMessageExtension = inherit IExtension
[<AllowNullLiteral>]
type IDawnFormatCapabilitiesExtension = inherit IExtension
[<AllowNullLiteral>]
type IDeviceDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type IInstanceDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type ILimitsExtension = inherit IExtension
[<AllowNullLiteral>]
type IPipelineLayoutDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type IRenderPassColorAttachmentExtension = inherit IExtension
[<AllowNullLiteral>]
type IRenderPassDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type IRequestAdapterOptionsExtension = inherit IExtension
[<AllowNullLiteral>]
type ISamplerDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type IShaderModuleDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type ISharedFenceDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type ISharedFenceExportInfoExtension = inherit IExtension
[<AllowNullLiteral>]
type ISharedTextureMemoryBeginAccessDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type ISharedTextureMemoryDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type ISharedTextureMemoryEndAccessStateExtension = inherit IExtension
[<AllowNullLiteral>]
type ISharedTextureMemoryPropertiesExtension = inherit IExtension
[<AllowNullLiteral>]
type ISurfaceDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type ITextureDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type ITextureViewDescriptorExtension = inherit IExtension
module private ExtensionDecoder =
    let decode<'a when 'a :> IExtension> (device : Device) (relativePointers : bool) (ptr : nativeint) : 'a =
        if ptr = 0n then
            Unchecked.defaultof<'a>
        else
            let sType = NativePtr.read (NativePtr.ofNativeInt<SType> (ptr + nativeint sizeof<nativeint>))
            if typeof<'a> = typeof<IAdapterInfoExtension> then
                match sType with
                | SType.DawnAdapterPropertiesPowerPreference ->
                    DawnAdapterPropertiesPowerPreference.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnAdapterPropertiesPowerPreference> (ptr)), relativePointers) :> obj :?> 'a
                | SType.AdapterPropertiesMemoryHeaps ->
                    AdapterPropertiesMemoryHeaps.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.AdapterPropertiesMemoryHeaps> (ptr)), relativePointers) :> obj :?> 'a
                | SType.AdapterPropertiesD3D ->
                    AdapterPropertiesD3D.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.AdapterPropertiesD3D> (ptr)), relativePointers) :> obj :?> 'a
                | SType.AdapterPropertiesVk ->
                    AdapterPropertiesVk.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.AdapterPropertiesVk> (ptr)), relativePointers) :> obj :?> 'a
                | SType.AdapterPropertiesSubgroupMatrixConfigs ->
                    AdapterPropertiesSubgroupMatrixConfigs.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.AdapterPropertiesSubgroupMatrixConfigs> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IBindGroupDescriptorExtension> then
                match sType with
                | SType.BindGroupDynamicBindingArray ->
                    BindGroupDynamicBindingArray.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.BindGroupDynamicBindingArray> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IBindGroupEntryExtension> then
                match sType with
                | SType.ExternalTextureBindingEntry ->
                    ExternalTextureBindingEntry.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.ExternalTextureBindingEntry> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IBindGroupLayoutDescriptorExtension> then
                match sType with
                | SType.BindGroupLayoutDynamicBindingArray ->
                    BindGroupLayoutDynamicBindingArray.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.BindGroupLayoutDynamicBindingArray> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IBindGroupLayoutEntryExtension> then
                match sType with
                | SType.StaticSamplerBindingLayout ->
                    StaticSamplerBindingLayout.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.StaticSamplerBindingLayout> (ptr)), relativePointers) :> obj :?> 'a
                | SType.ExternalTextureBindingLayout ->
                    ExternalTextureBindingLayout.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.ExternalTextureBindingLayout> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IBufferDescriptorExtension> then
                match sType with
                | SType.BufferHostMappedPointer ->
                    BufferHostMappedPointer.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.BufferHostMappedPointer> (ptr)), relativePointers) :> obj :?> 'a
                | SType.DawnFakeBufferOOMForTesting ->
                    DawnFakeBufferOOMForTesting.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnFakeBufferOOMForTesting> (ptr)), relativePointers) :> obj :?> 'a
                | SType.DawnBufferDescriptorErrorInfoFromWireClient ->
                    DawnBufferDescriptorErrorInfoFromWireClient.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnBufferDescriptorErrorInfoFromWireClient> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IColorTargetStateExtension> then
                match sType with
                | SType.ColorTargetStateExpandResolveTextureDawn ->
                    ColorTargetStateExpandResolveTextureDawn.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.ColorTargetStateExpandResolveTextureDawn> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ICommandEncoderDescriptorExtension> then
                match sType with
                | SType.DawnEncoderInternalUsageDescriptor ->
                    DawnEncoderInternalUsageDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnEncoderInternalUsageDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ICompilationMessageExtension> then
                match sType with
                | SType.DawnCompilationMessageUtf16 ->
                    DawnCompilationMessageUtf16.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnCompilationMessageUtf16> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IDawnFormatCapabilitiesExtension> then
                match sType with
                | SType.DawnDrmFormatCapabilities ->
                    DawnDrmFormatCapabilities.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnDrmFormatCapabilities> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IDeviceDescriptorExtension> then
                match sType with
                | SType.DawnConsumeAdapterDescriptor ->
                    DawnConsumeAdapterDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnConsumeAdapterDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.DawnTogglesDescriptor ->
                    DawnTogglesDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnTogglesDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.DawnCacheDeviceDescriptor ->
                    DawnCacheDeviceDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnCacheDeviceDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.DawnDeviceAllocatorControl ->
                    DawnDeviceAllocatorControl.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnDeviceAllocatorControl> (ptr)), relativePointers) :> obj :?> 'a
                | SType.DawnFakeDeviceInitializeErrorForTesting ->
                    DawnFakeDeviceInitializeErrorForTesting.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnFakeDeviceInitializeErrorForTesting> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IInstanceDescriptorExtension> then
                match sType with
                | SType.DawnTogglesDescriptor ->
                    DawnTogglesDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnTogglesDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.DawnWGSLBlocklist ->
                    DawnWGSLBlocklist.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnWGSLBlocklist> (ptr)), relativePointers) :> obj :?> 'a
                | SType.DawnWireWGSLControl ->
                    DawnWireWGSLControl.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnWireWGSLControl> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ILimitsExtension> then
                match sType with
                | SType.CompatibilityModeLimits ->
                    CompatibilityModeLimits.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.CompatibilityModeLimits> (ptr)), relativePointers) :> obj :?> 'a
                | SType.DawnTexelCopyBufferRowAlignmentLimits ->
                    DawnTexelCopyBufferRowAlignmentLimits.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnTexelCopyBufferRowAlignmentLimits> (ptr)), relativePointers) :> obj :?> 'a
                | SType.DawnHostMappedPointerLimits ->
                    DawnHostMappedPointerLimits.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnHostMappedPointerLimits> (ptr)), relativePointers) :> obj :?> 'a
                | SType.DynamicBindingArrayLimits ->
                    DynamicBindingArrayLimits.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DynamicBindingArrayLimits> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IPipelineLayoutDescriptorExtension> then
                match sType with
                | SType.PipelineLayoutPixelLocalStorage ->
                    PipelineLayoutPixelLocalStorage.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.PipelineLayoutPixelLocalStorage> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IRenderPassColorAttachmentExtension> then
                match sType with
                | SType.DawnRenderPassColorAttachmentRenderToSingleSampled ->
                    DawnRenderPassColorAttachmentRenderToSingleSampled.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnRenderPassColorAttachmentRenderToSingleSampled> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IRenderPassDescriptorExtension> then
                match sType with
                | SType.RenderPassMaxDrawCount ->
                    RenderPassMaxDrawCount.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.RenderPassMaxDrawCount> (ptr)), relativePointers) :> obj :?> 'a
                | SType.RenderPassDescriptorExpandResolveRect ->
                    RenderPassDescriptorExpandResolveRect.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.RenderPassDescriptorExpandResolveRect> (ptr)), relativePointers) :> obj :?> 'a
                | SType.RenderPassDescriptorResolveRect ->
                    RenderPassDescriptorResolveRect.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.RenderPassDescriptorResolveRect> (ptr)), relativePointers) :> obj :?> 'a
                | SType.RenderPassPixelLocalStorage ->
                    RenderPassPixelLocalStorage.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.RenderPassPixelLocalStorage> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IRequestAdapterOptionsExtension> then
                match sType with
                | SType.RequestAdapterWebXROptions ->
                    RequestAdapterWebXROptions.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.RequestAdapterWebXROptions> (ptr)), relativePointers) :> obj :?> 'a
                | SType.RequestAdapterWebGPUBackendOptions ->
                    RequestAdapterWebGPUBackendOptions.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.RequestAdapterWebGPUBackendOptions> (ptr)), relativePointers) :> obj :?> 'a
                | SType.DawnTogglesDescriptor ->
                    DawnTogglesDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnTogglesDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ISamplerDescriptorExtension> then
                match sType with
                | SType.YCbCrVkDescriptor ->
                    YCbCrVkDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.YCbCrVkDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IShaderModuleDescriptorExtension> then
                match sType with
                | SType.ShaderSourceSPIRV ->
                    ShaderSourceSPIRV.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.ShaderSourceSPIRV> (ptr)), relativePointers) :> obj :?> 'a
                | SType.ShaderSourceWGSL ->
                    ShaderSourceWGSL.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.ShaderSourceWGSL> (ptr)), relativePointers) :> obj :?> 'a
                | SType.DawnShaderModuleSPIRVOptionsDescriptor ->
                    DawnShaderModuleSPIRVOptionsDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnShaderModuleSPIRVOptionsDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.ShaderModuleCompilationOptions ->
                    ShaderModuleCompilationOptions.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.ShaderModuleCompilationOptions> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ISharedFenceDescriptorExtension> then
                match sType with
                | SType.SharedFenceVkSemaphoreOpaqueFDDescriptor ->
                    SharedFenceVkSemaphoreOpaqueFDDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedFenceSyncFDDescriptor ->
                    SharedFenceSyncFDDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceSyncFDDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedFenceVkSemaphoreZirconHandleDescriptor ->
                    SharedFenceVkSemaphoreZirconHandleDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedFenceDXGISharedHandleDescriptor ->
                    SharedFenceDXGISharedHandleDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceDXGISharedHandleDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedFenceMTLSharedEventDescriptor ->
                    SharedFenceMTLSharedEventDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceMTLSharedEventDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedFenceEGLSyncDescriptor ->
                    SharedFenceEGLSyncDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceEGLSyncDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ISharedFenceExportInfoExtension> then
                match sType with
                | SType.SharedFenceVkSemaphoreOpaqueFDExportInfo ->
                    SharedFenceVkSemaphoreOpaqueFDExportInfo.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDExportInfo> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedFenceSyncFDExportInfo ->
                    SharedFenceSyncFDExportInfo.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceSyncFDExportInfo> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedFenceVkSemaphoreZirconHandleExportInfo ->
                    SharedFenceVkSemaphoreZirconHandleExportInfo.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleExportInfo> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedFenceDXGISharedHandleExportInfo ->
                    SharedFenceDXGISharedHandleExportInfo.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceDXGISharedHandleExportInfo> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedFenceMTLSharedEventExportInfo ->
                    SharedFenceMTLSharedEventExportInfo.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceMTLSharedEventExportInfo> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedFenceEGLSyncExportInfo ->
                    SharedFenceEGLSyncExportInfo.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceEGLSyncExportInfo> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ISharedTextureMemoryBeginAccessDescriptorExtension> then
                match sType with
                | SType.SharedTextureMemoryVkImageLayoutBeginState ->
                    SharedTextureMemoryVkImageLayoutBeginState.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryVkImageLayoutBeginState> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedTextureMemoryD3DSwapchainBeginState ->
                    SharedTextureMemoryD3DSwapchainBeginState.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryD3DSwapchainBeginState> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedTextureMemoryD3D11BeginState ->
                    SharedTextureMemoryD3D11BeginState.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryD3D11BeginState> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ISharedTextureMemoryDescriptorExtension> then
                match sType with
                | SType.SharedTextureMemoryVkDedicatedAllocationDescriptor ->
                    SharedTextureMemoryVkDedicatedAllocationDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryVkDedicatedAllocationDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedTextureMemoryAHardwareBufferDescriptor ->
                    SharedTextureMemoryAHardwareBufferDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryAHardwareBufferDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedTextureMemoryDmaBufDescriptor ->
                    SharedTextureMemoryDmaBufDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryDmaBufDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedTextureMemoryOpaqueFDDescriptor ->
                    SharedTextureMemoryOpaqueFDDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryOpaqueFDDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedTextureMemoryZirconHandleDescriptor ->
                    SharedTextureMemoryZirconHandleDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryZirconHandleDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedTextureMemoryDXGISharedHandleDescriptor ->
                    SharedTextureMemoryDXGISharedHandleDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryDXGISharedHandleDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedTextureMemoryIOSurfaceDescriptor ->
                    SharedTextureMemoryIOSurfaceDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryIOSurfaceDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SharedTextureMemoryEGLImageDescriptor ->
                    SharedTextureMemoryEGLImageDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryEGLImageDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ISharedTextureMemoryEndAccessStateExtension> then
                match sType with
                | SType.SharedTextureMemoryVkImageLayoutEndState ->
                    SharedTextureMemoryVkImageLayoutEndState.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryVkImageLayoutEndState> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ISharedTextureMemoryPropertiesExtension> then
                match sType with
                | SType.SharedTextureMemoryAHardwareBufferProperties ->
                    SharedTextureMemoryAHardwareBufferProperties.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryAHardwareBufferProperties> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ISurfaceDescriptorExtension> then
                match sType with
                | SType.SurfaceSourceAndroidNativeWindow ->
                    SurfaceSourceAndroidNativeWindow.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceSourceAndroidNativeWindow> (ptr)), relativePointers) :> obj :?> 'a
                | SType.EmscriptenSurfaceSourceCanvasHTMLSelector ->
                    EmscriptenSurfaceSourceCanvasHTMLSelector.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.EmscriptenSurfaceSourceCanvasHTMLSelector> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SurfaceSourceMetalLayer ->
                    SurfaceSourceMetalLayer.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceSourceMetalLayer> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SurfaceSourceWindowsHWND ->
                    SurfaceSourceWindowsHWND.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceSourceWindowsHWND> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SurfaceSourceXCBWindow ->
                    SurfaceSourceXCBWindow.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceSourceXCBWindow> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SurfaceSourceXlibWindow ->
                    SurfaceSourceXlibWindow.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceSourceXlibWindow> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SurfaceSourceWaylandSurface ->
                    SurfaceSourceWaylandSurface.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceSourceWaylandSurface> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SurfaceDescriptorFromWindowsCoreWindow ->
                    SurfaceDescriptorFromWindowsCoreWindow.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceDescriptorFromWindowsCoreWindow> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SurfaceDescriptorFromWindowsUWPSwapChainPanel ->
                    SurfaceDescriptorFromWindowsUWPSwapChainPanel.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceDescriptorFromWindowsUWPSwapChainPanel> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SurfaceDescriptorFromWindowsWinUISwapChainPanel ->
                    SurfaceDescriptorFromWindowsWinUISwapChainPanel.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceDescriptorFromWindowsWinUISwapChainPanel> (ptr)), relativePointers) :> obj :?> 'a
                | SType.SurfaceColorManagement ->
                    SurfaceColorManagement.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceColorManagement> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ITextureDescriptorExtension> then
                match sType with
                | SType.TextureBindingViewDimensionDescriptor ->
                    TextureBindingViewDimensionDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.TextureBindingViewDimensionDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.DawnTextureInternalUsageDescriptor ->
                    DawnTextureInternalUsageDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.DawnTextureInternalUsageDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ITextureViewDescriptorExtension> then
                match sType with
                | SType.TextureComponentSwizzleDescriptor ->
                    TextureComponentSwizzleDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.TextureComponentSwizzleDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | SType.YCbCrVkDescriptor ->
                    YCbCrVkDescriptor.Read(device, (NativePtr.ofNativeInt<WebGPU.Raw.YCbCrVkDescriptor> (ptr)), relativePointers) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IExtension> then
                Unchecked.defaultof<'a> // TODO
            else
                failwithf "bad extension type: %A" typeof<'a>
[<AbstractClass; Sealed>]
type private PinHelper() =
    static member inline PinNullable<'r>(x : IExtension, [<InlineIfLambda>] action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
type Proc = delegate of IDisposable -> unit
type RequestAdapterOptions = 
    {
        Next : IRequestAdapterOptionsExtension
        FeatureLevel : FeatureLevel
        PowerPreference : PowerPreference
        ForceFallbackAdapter : bool
        BackendType : BackendType
        CompatibleSurface : Surface
    }
    static member Null = Unchecked.defaultof<RequestAdapterOptions>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.RequestAdapterOptions> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable value =
                    new WebGPU.Raw.RequestAdapterOptions(
                        nextInChain,
                        this.FeatureLevel,
                        this.PowerPreference,
                        (if this.ForceFallbackAdapter then 1 else 0),
                        this.BackendType,
                        this.CompatibleSurface.Handle
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.RequestAdapterOptions> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.RequestAdapterOptions>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IRequestAdapterOptionsExtension> device relativePointers backend.NextInChain
            FeatureLevel = backend.FeatureLevel
            PowerPreference = backend.PowerPreference
            ForceFallbackAdapter = (backend.ForceFallbackAdapter <> 0)
            BackendType = backend.BackendType
            CompatibleSurface = new Surface(backend.CompatibleSurface)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.RequestAdapterOptions>) = 
        use ptr = fixed &r
        RequestAdapterOptions.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        RequestAdapterOptions.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.RequestAdapterOptions>
type RequestAdapterWebXROptions = 
    {
        Next : IRequestAdapterOptionsExtension
        XrCompatible : bool
    }
    static member Null = Unchecked.defaultof<RequestAdapterWebXROptions>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.RequestAdapterWebXROptions> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.RequestAdapterWebXROptions
                let mutable value =
                    new WebGPU.Raw.RequestAdapterWebXROptions(
                        nextInChain,
                        sType,
                        (if this.XrCompatible then 1 else 0)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IRequestAdapterOptionsExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.RequestAdapterWebXROptions> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.RequestAdapterWebXROptions>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IRequestAdapterOptionsExtension> device relativePointers backend.NextInChain
            XrCompatible = (backend.XrCompatible <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.RequestAdapterWebXROptions>) = 
        use ptr = fixed &r
        RequestAdapterWebXROptions.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        RequestAdapterWebXROptions.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.RequestAdapterWebXROptions>
type RequestAdapterWebGPUBackendOptions = 
    {
        Next : IRequestAdapterOptionsExtension
    }
    static member Null = Unchecked.defaultof<RequestAdapterWebGPUBackendOptions>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.RequestAdapterWebGPUBackendOptions> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.RequestAdapterWebGPUBackendOptions
                let mutable value =
                    new WebGPU.Raw.RequestAdapterWebGPUBackendOptions(
                        nextInChain,
                        sType
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IRequestAdapterOptionsExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.RequestAdapterWebGPUBackendOptions> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.RequestAdapterWebGPUBackendOptions>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IRequestAdapterOptionsExtension> device relativePointers backend.NextInChain
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.RequestAdapterWebGPUBackendOptions>) = 
        use ptr = fixed &r
        RequestAdapterWebGPUBackendOptions.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        RequestAdapterWebGPUBackendOptions.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.RequestAdapterWebGPUBackendOptions>
type RequestAdapterCallback = delegate of IDisposable * status : RequestAdapterStatus * adapter : Adapter * message : string -> unit
type RequestAdapterCallbackInfo = 
    {
        Mode : CallbackMode
        Callback : RequestAdapterCallback
    }
    static member Null = Unchecked.defaultof<RequestAdapterCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.RequestAdapterCallbackInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable _callbackPtr = 0n
            if not (isNull (this.Callback :> obj)) then
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.RequestAdapterCallback>
                _callbackDel <- WebGPU.Raw.RequestAdapterCallback(fun status adapter message userdata1 userdata2 ->
                    let _status = status
                    let _adapter = new Adapter(adapter)
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _adapter, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                _callbackPtr <- Marshal.GetFunctionPointerForDelegate(_callbackDel)
            let mutable value =
                new WebGPU.Raw.RequestAdapterCallbackInfo(
                    nextInChain,
                    this.Mode,
                    _callbackPtr,
                    Unchecked.defaultof<_>,
                    Unchecked.defaultof<_>
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.RequestAdapterCallbackInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.RequestAdapterCallbackInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.RequestAdapterCallbackInfo>) = 
        use ptr = fixed &r
        RequestAdapterCallbackInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        RequestAdapterCallbackInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.RequestAdapterCallbackInfo>
type Adapter internal(handle : nativeint) =
    static let device = Unchecked.defaultof<Device>
    static let nullptr = new Adapter(Unchecked.defaultof<_>)
    let instance =
        lazy (
            let relativePointers = false
            let mutable res = WebGPU.Raw.WebGPU.AdapterGetInstance(handle)
            new Instance(res)
        )
    member x.Handle = handle
    override x.ToString() = $"Adapter(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? Adapter as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.Instance : Instance =
        instance.Value
    member this.Limits : Limits =
        let relativePointers = false
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        try
            let status = WebGPU.Raw.WebGPU.AdapterGetLimits(handle, ptr)
            if status <> Status.Success then failwith "GetLimits failed"
            use pppp = fixed &res in Limits.Read(device, pppp, relativePointers)
        finally
            ()
    member this.Info : AdapterInfo =
        let relativePointers = false
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        try
            let status = WebGPU.Raw.WebGPU.AdapterGetInfo(handle, ptr)
            if status <> Status.Success then failwith "GetInfo failed"
            use pppp = fixed &res in AdapterInfo.Read(device, pppp, relativePointers)
        finally
            ()
    member this.HasFeature(feature : FeatureName) : bool =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.AdapterHasFeature(handle, feature)
        (res <> 0)
    member this.Features : SupportedFeatures =
        let relativePointers = false
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        try
            WebGPU.Raw.WebGPU.AdapterGetFeatures(handle, ptr)
            use pppp = fixed &res in SupportedFeatures.Read(device, pppp, relativePointers)
        finally
            ()
    member this.RequestDevice(descriptor : DeviceDescriptor, callbackInfo : RequestDeviceCallbackInfo) : Future =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            callbackInfo.Pin(device, fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.AdapterRequestDevice(handle, _descriptorPtr, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
                use pppp = fixed &res in Future.Read(device, pppp, relativePointers)
            )
        )
    member this.CreateDevice(descriptor : DeviceDescriptor) : Device =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.AdapterCreateDevice(handle, _descriptorPtr)
            new Device(res)
        )
    member this.GetFormatCapabilities(format : TextureFormat, capabilities : byref<DawnFormatCapabilities>) : Status =
        let relativePointers = false
        let mutable capabilitiesCopy = capabilities
        try
            capabilities.Pin(device, fun _capabilitiesPtr ->
                if NativePtr.toNativeInt _capabilitiesPtr = 0n then
                    let mutable capabilitiesNative = Unchecked.defaultof<WebGPU.Raw.DawnFormatCapabilities>
                    use _capabilitiesPtr = fixed &capabilitiesNative
                    try
                        let res = WebGPU.Raw.WebGPU.AdapterGetFormatCapabilities(handle, format, _capabilitiesPtr)
                        let _ret = res
                        capabilitiesCopy <- DawnFormatCapabilities.Read(device, _capabilitiesPtr, relativePointers)
                        _ret
                    finally
                        ()
                else
                    let res = WebGPU.Raw.WebGPU.AdapterGetFormatCapabilities(handle, format, _capabilitiesPtr)
                    let _ret = res
                    capabilitiesCopy <- DawnFormatCapabilities.Read(device, _capabilitiesPtr, relativePointers)
                    _ret
                )
        finally
            capabilities <- capabilitiesCopy
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.AdapterRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.AdapterAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type AdapterInfo = 
    {
        Next : IAdapterInfoExtension
        Vendor : string
        Architecture : string
        Device : string
        Description : string
        BackendType : BackendType
        AdapterType : AdapterType
        VendorID : int
        DeviceID : int
        SubgroupMinSize : int
        SubgroupMaxSize : int
    }
    static member Null = Unchecked.defaultof<AdapterInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.AdapterInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _vendorArr = if isNull this.Vendor then null else Encoding.UTF8.GetBytes(this.Vendor)
                use _vendorPtr = fixed _vendorArr
                try
                    let _vendorLen = WebGPU.Raw.StringView(_vendorPtr, if isNull _vendorArr then 0un else unativeint _vendorArr.Length)
                    let _architectureArr = if isNull this.Architecture then null else Encoding.UTF8.GetBytes(this.Architecture)
                    use _architecturePtr = fixed _architectureArr
                    try
                        let _architectureLen = WebGPU.Raw.StringView(_architecturePtr, if isNull _architectureArr then 0un else unativeint _architectureArr.Length)
                        let _deviceArr = if isNull this.Device then null else Encoding.UTF8.GetBytes(this.Device)
                        use _devicePtr = fixed _deviceArr
                        try
                            let _deviceLen = WebGPU.Raw.StringView(_devicePtr, if isNull _deviceArr then 0un else unativeint _deviceArr.Length)
                            let _descriptionArr = if isNull this.Description then null else Encoding.UTF8.GetBytes(this.Description)
                            use _descriptionPtr = fixed _descriptionArr
                            try
                                let _descriptionLen = WebGPU.Raw.StringView(_descriptionPtr, if isNull _descriptionArr then 0un else unativeint _descriptionArr.Length)
                                let mutable value =
                                    new WebGPU.Raw.AdapterInfo(
                                        nextInChain,
                                        _vendorLen,
                                        _architectureLen,
                                        _deviceLen,
                                        _descriptionLen,
                                        this.BackendType,
                                        this.AdapterType,
                                        uint32(this.VendorID),
                                        uint32(this.DeviceID),
                                        uint32(this.SubgroupMinSize),
                                        uint32(this.SubgroupMaxSize)
                                    )
                                use ptr = fixed &value
                                try action ptr
                                finally ()
                            finally
                                ()
                        finally
                            ()
                    finally
                        ()
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.AdapterInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.AdapterInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Vendor.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Vendor - NativePtr.toNativeInt &&backend
                backend.Vendor.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Vendor.Data)
            if NativePtr.toNativeInt backend.Architecture.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Architecture - NativePtr.toNativeInt &&backend
                backend.Architecture.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Architecture.Data)
            if NativePtr.toNativeInt backend.Device.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Device - NativePtr.toNativeInt &&backend
                backend.Device.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Device.Data)
            if NativePtr.toNativeInt backend.Description.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Description - NativePtr.toNativeInt &&backend
                backend.Description.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Description.Data)
        {
            Next = ExtensionDecoder.decode<IAdapterInfoExtension> device relativePointers backend.NextInChain
            Vendor = let _vendorPtr = NativePtr.toNativeInt(backend.Vendor.Data) in if _vendorPtr = 0n then null else Marshal.PtrToStringUTF8(_vendorPtr, int(backend.Vendor.Length))
            Architecture = let _architecturePtr = NativePtr.toNativeInt(backend.Architecture.Data) in if _architecturePtr = 0n then null else Marshal.PtrToStringUTF8(_architecturePtr, int(backend.Architecture.Length))
            Device = let _devicePtr = NativePtr.toNativeInt(backend.Device.Data) in if _devicePtr = 0n then null else Marshal.PtrToStringUTF8(_devicePtr, int(backend.Device.Length))
            Description = let _descriptionPtr = NativePtr.toNativeInt(backend.Description.Data) in if _descriptionPtr = 0n then null else Marshal.PtrToStringUTF8(_descriptionPtr, int(backend.Description.Length))
            BackendType = backend.BackendType
            AdapterType = backend.AdapterType
            VendorID = int(backend.VendorID)
            DeviceID = int(backend.DeviceID)
            SubgroupMinSize = int(backend.SubgroupMinSize)
            SubgroupMaxSize = int(backend.SubgroupMaxSize)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.AdapterInfo>) = 
        use ptr = fixed &r
        AdapterInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        AdapterInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.AdapterInfo>
type DeviceDescriptor = 
    {
        Next : IDeviceDescriptorExtension
        Label : string
        RequiredFeatures : array<FeatureName>
        RequiredLimits : Limits
        DefaultQueue : QueueDescriptor
        DeviceLostCallbackInfo : DeviceLostCallbackInfo
        UncapturedErrorCallbackInfo : UncapturedErrorCallbackInfo
    }
    static member Null = Unchecked.defaultof<DeviceDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DeviceDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                try
                    let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                    use requiredFeaturesPtr = fixed (this.RequiredFeatures)
                    try
                        let requiredFeaturesLen = unativeint this.RequiredFeatures.Length
                        this.RequiredLimits.Pin(device, fun _requiredLimitsPtr ->
                            this.DefaultQueue.Pin(device, fun _defaultQueuePtr ->
                                this.DeviceLostCallbackInfo.Pin(device, fun _deviceLostCallbackInfoPtr ->
                                    this.UncapturedErrorCallbackInfo.Pin(device, fun _uncapturedErrorCallbackInfoPtr ->
                                        let mutable value =
                                            new WebGPU.Raw.DeviceDescriptor(
                                                nextInChain,
                                                _labelLen,
                                                requiredFeaturesLen,
                                                requiredFeaturesPtr,
                                                _requiredLimitsPtr,
                                                (if NativePtr.toNativeInt _defaultQueuePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _defaultQueuePtr),
                                                (if NativePtr.toNativeInt _deviceLostCallbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _deviceLostCallbackInfoPtr),
                                                (if NativePtr.toNativeInt _uncapturedErrorCallbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _uncapturedErrorCallbackInfoPtr)
                                            )
                                        use ptr = fixed &value
                                        try action ptr
                                        finally ()
                                    )
                                )
                            )
                        )
                    finally
                        ()
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DeviceDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DeviceDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
            if NativePtr.toNativeInt backend.RequiredFeatures <> 0n then
                backend.RequiredFeatures <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.RequiredFeatures)
            if NativePtr.toNativeInt backend.RequiredLimits <> 0n then
                backend.RequiredLimits <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.RequiredLimits)
        {
            Next = ExtensionDecoder.decode<IDeviceDescriptorExtension> device relativePointers backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            RequiredFeatures = let ptr = backend.RequiredFeatures in Array.init (int backend.RequiredFeatureCount) (fun i -> NativePtr.get ptr i)
            RequiredLimits = Limits.Read(device, backend.RequiredLimits, relativePointers)
            DefaultQueue = use pppp = fixed &backend.DefaultQueue in QueueDescriptor.Read(device, pppp, relativePointers)
            DeviceLostCallbackInfo = use pppp = fixed &backend.DeviceLostCallbackInfo in DeviceLostCallbackInfo.Read(device, pppp, relativePointers)
            UncapturedErrorCallbackInfo = use pppp = fixed &backend.UncapturedErrorCallbackInfo in UncapturedErrorCallbackInfo.Read(device, pppp, relativePointers)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DeviceDescriptor>) = 
        use ptr = fixed &r
        DeviceDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DeviceDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DeviceDescriptor>
type DawnConsumeAdapterDescriptor = 
    {
        Next : IDeviceDescriptorExtension
        ConsumeAdapter : bool
    }
    static member Null = Unchecked.defaultof<DawnConsumeAdapterDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnConsumeAdapterDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnConsumeAdapterDescriptor
                let mutable value =
                    new WebGPU.Raw.DawnConsumeAdapterDescriptor(
                        nextInChain,
                        sType,
                        (if this.ConsumeAdapter then 1 else 0)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IDeviceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnConsumeAdapterDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnConsumeAdapterDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IDeviceDescriptorExtension> device relativePointers backend.NextInChain
            ConsumeAdapter = (backend.ConsumeAdapter <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnConsumeAdapterDescriptor>) = 
        use ptr = fixed &r
        DawnConsumeAdapterDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnConsumeAdapterDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnConsumeAdapterDescriptor>
type DawnTogglesDescriptor = 
    {
        Next : IInstanceDescriptorExtension
        EnabledToggleCount : int64
        EnabledToggles : nativeptr<nativeptr<byte>>
        DisabledToggleCount : int64
        DisabledToggles : nativeptr<nativeptr<byte>>
    }
    static member Null = Unchecked.defaultof<DawnTogglesDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnTogglesDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnTogglesDescriptor
                let mutable value =
                    new WebGPU.Raw.DawnTogglesDescriptor(
                        nextInChain,
                        sType,
                        unativeint(this.EnabledToggleCount),
                        this.EnabledToggles,
                        unativeint(this.DisabledToggleCount),
                        this.DisabledToggles
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IInstanceDescriptorExtension
    interface IRequestAdapterOptionsExtension
    interface IDeviceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnTogglesDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnTogglesDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.EnabledToggles <> 0n then
                backend.EnabledToggles <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.EnabledToggles)
            if NativePtr.toNativeInt backend.DisabledToggles <> 0n then
                backend.DisabledToggles <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.DisabledToggles)
        {
            Next = ExtensionDecoder.decode<IInstanceDescriptorExtension> device relativePointers backend.NextInChain
            EnabledToggleCount = int64(backend.EnabledToggleCount)
            EnabledToggles = backend.EnabledToggles
            DisabledToggleCount = int64(backend.DisabledToggleCount)
            DisabledToggles = backend.DisabledToggles
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnTogglesDescriptor>) = 
        use ptr = fixed &r
        DawnTogglesDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnTogglesDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnTogglesDescriptor>
type DawnLoadCacheDataFunction = delegate of IDisposable * key : nativeint * keySize : int64 * value : nativeint * valueSize : int64 -> unativeint
type DawnStoreCacheDataFunction = delegate of IDisposable * key : nativeint * keySize : int64 * value : nativeint * valueSize : int64 -> unit
type DawnCacheDeviceDescriptor = 
    {
        Next : IDeviceDescriptorExtension
        IsolationKey : string
        LoadDataFunction : DawnLoadCacheDataFunction
        StoreDataFunction : DawnStoreCacheDataFunction
    }
    static member Null = Unchecked.defaultof<DawnCacheDeviceDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnCacheDeviceDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnCacheDeviceDescriptor
                let _isolationKeyArr = if isNull this.IsolationKey then null else Encoding.UTF8.GetBytes(this.IsolationKey)
                use _isolationKeyPtr = fixed _isolationKeyArr
                try
                    let _isolationKeyLen = WebGPU.Raw.StringView(_isolationKeyPtr, if isNull _isolationKeyArr then 0un else unativeint _isolationKeyArr.Length)
                    let mutable _loadDataFunctionPtr = 0n
                    if not (isNull (this.LoadDataFunction :> obj)) then
                        let mutable _loadDataFunctionGC = Unchecked.defaultof<GCHandle>
                        let mutable _loadDataFunctionDel = Unchecked.defaultof<WebGPU.Raw.DawnLoadCacheDataFunction>
                        _loadDataFunctionDel <- WebGPU.Raw.DawnLoadCacheDataFunction(fun key keySize value valueSize userdata ->
                            let _key = key
                            let _keySize = int64(keySize)
                            let _value = value
                            let _valueSize = int64(valueSize)
                            this.LoadDataFunction.Invoke({ new IDisposable with member __.Dispose() = _loadDataFunctionGC.Free() }, _key, _keySize, _value, _valueSize)
                        )
                        _loadDataFunctionGC <- GCHandle.Alloc(_loadDataFunctionDel)
                        _loadDataFunctionPtr <- Marshal.GetFunctionPointerForDelegate(_loadDataFunctionDel)
                    let mutable _storeDataFunctionPtr = 0n
                    if not (isNull (this.StoreDataFunction :> obj)) then
                        let mutable _storeDataFunctionGC = Unchecked.defaultof<GCHandle>
                        let mutable _storeDataFunctionDel = Unchecked.defaultof<WebGPU.Raw.DawnStoreCacheDataFunction>
                        _storeDataFunctionDel <- WebGPU.Raw.DawnStoreCacheDataFunction(fun key keySize value valueSize userdata ->
                            let _key = key
                            let _keySize = int64(keySize)
                            let _value = value
                            let _valueSize = int64(valueSize)
                            this.StoreDataFunction.Invoke({ new IDisposable with member __.Dispose() = _storeDataFunctionGC.Free() }, _key, _keySize, _value, _valueSize)
                        )
                        _storeDataFunctionGC <- GCHandle.Alloc(_storeDataFunctionDel)
                        _storeDataFunctionPtr <- Marshal.GetFunctionPointerForDelegate(_storeDataFunctionDel)
                    let mutable value =
                        new WebGPU.Raw.DawnCacheDeviceDescriptor(
                            nextInChain,
                            sType,
                            _isolationKeyLen,
                            _loadDataFunctionPtr,
                            _storeDataFunctionPtr,
                            Unchecked.defaultof<_>
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                finally
                    ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IDeviceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnCacheDeviceDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnCacheDeviceDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.IsolationKey.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.IsolationKey - NativePtr.toNativeInt &&backend
                backend.IsolationKey.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.IsolationKey.Data)
        {
            Next = ExtensionDecoder.decode<IDeviceDescriptorExtension> device relativePointers backend.NextInChain
            IsolationKey = let _isolationKeyPtr = NativePtr.toNativeInt(backend.IsolationKey.Data) in if _isolationKeyPtr = 0n then null else Marshal.PtrToStringUTF8(_isolationKeyPtr, int(backend.IsolationKey.Length))
            LoadDataFunction = failwith "cannot read callbacks"//TODO2 map [(function userdata, backend.FunctionUserdata); (isolation key, backend.IsolationKey); (load data function, backend.LoadDataFunction); ... ]
            StoreDataFunction = failwith "cannot read callbacks"//TODO2 map [(function userdata, backend.FunctionUserdata); (isolation key, backend.IsolationKey); (load data function, backend.LoadDataFunction); ... ]
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnCacheDeviceDescriptor>) = 
        use ptr = fixed &r
        DawnCacheDeviceDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnCacheDeviceDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnCacheDeviceDescriptor>
type DawnDeviceAllocatorControl = 
    {
        Next : IDeviceDescriptorExtension
        AllocatorHeapBlockSize : int64
    }
    static member Null = Unchecked.defaultof<DawnDeviceAllocatorControl>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnDeviceAllocatorControl> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnDeviceAllocatorControl
                let mutable value =
                    new WebGPU.Raw.DawnDeviceAllocatorControl(
                        nextInChain,
                        sType,
                        unativeint(this.AllocatorHeapBlockSize)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IDeviceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnDeviceAllocatorControl> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnDeviceAllocatorControl>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IDeviceDescriptorExtension> device relativePointers backend.NextInChain
            AllocatorHeapBlockSize = int64(backend.AllocatorHeapBlockSize)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnDeviceAllocatorControl>) = 
        use ptr = fixed &r
        DawnDeviceAllocatorControl.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnDeviceAllocatorControl.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnDeviceAllocatorControl>
type DawnWGSLBlocklist = 
    {
        Next : IInstanceDescriptorExtension
        BlocklistedFeatureCount : int64
        BlocklistedFeatures : nativeptr<nativeptr<byte>>
    }
    static member Null = Unchecked.defaultof<DawnWGSLBlocklist>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnWGSLBlocklist> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnWGSLBlocklist
                let mutable value =
                    new WebGPU.Raw.DawnWGSLBlocklist(
                        nextInChain,
                        sType,
                        unativeint(this.BlocklistedFeatureCount),
                        this.BlocklistedFeatures
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IInstanceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnWGSLBlocklist> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnWGSLBlocklist>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.BlocklistedFeatures <> 0n then
                backend.BlocklistedFeatures <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.BlocklistedFeatures)
        {
            Next = ExtensionDecoder.decode<IInstanceDescriptorExtension> device relativePointers backend.NextInChain
            BlocklistedFeatureCount = int64(backend.BlocklistedFeatureCount)
            BlocklistedFeatures = backend.BlocklistedFeatures
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnWGSLBlocklist>) = 
        use ptr = fixed &r
        DawnWGSLBlocklist.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnWGSLBlocklist.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnWGSLBlocklist>
type BindGroup internal(device : Device, handle : nativeint) =
    static let nullptr = new BindGroup(Unchecked.defaultof<_>, Unchecked.defaultof<_>)
    member x.Handle = handle
    member x.Device = device
    override x.ToString() = $"BindGroup(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? BindGroup as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.BindGroupSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.BindGroupRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.BindGroupAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type BindGroupEntry = 
    {
        Next : IBindGroupEntryExtension
        Binding : int
        Buffer : Buffer
        Offset : int64
        Size : int64
        Sampler : Sampler
        TextureView : TextureView
    }
    static member Null = Unchecked.defaultof<BindGroupEntry>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.BindGroupEntry> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable value =
                    new WebGPU.Raw.BindGroupEntry(
                        nextInChain,
                        uint32(this.Binding),
                        this.Buffer.Handle,
                        uint64(this.Offset),
                        uint64(this.Size),
                        this.Sampler.Handle,
                        this.TextureView.Handle
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.BindGroupEntry> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.BindGroupEntry>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IBindGroupEntryExtension> device relativePointers backend.NextInChain
            Binding = int(backend.Binding)
            Buffer = new Buffer(device, backend.Buffer)
            Offset = int64(backend.Offset)
            Size = int64(backend.Size)
            Sampler = new Sampler(device, backend.Sampler)
            TextureView = new TextureView(backend.TextureView)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.BindGroupEntry>) = 
        use ptr = fixed &r
        BindGroupEntry.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        BindGroupEntry.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.BindGroupEntry>
type BindGroupDynamicBindingArray = 
    {
        Next : IBindGroupDescriptorExtension
        DynamicArraySize : int
    }
    static member Null = Unchecked.defaultof<BindGroupDynamicBindingArray>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.BindGroupDynamicBindingArray> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.BindGroupDynamicBindingArray
                let mutable value =
                    new WebGPU.Raw.BindGroupDynamicBindingArray(
                        nextInChain,
                        sType,
                        uint32(this.DynamicArraySize)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IBindGroupDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.BindGroupDynamicBindingArray> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.BindGroupDynamicBindingArray>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IBindGroupDescriptorExtension> device relativePointers backend.NextInChain
            DynamicArraySize = int(backend.DynamicArraySize)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.BindGroupDynamicBindingArray>) = 
        use ptr = fixed &r
        BindGroupDynamicBindingArray.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        BindGroupDynamicBindingArray.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.BindGroupDynamicBindingArray>
type BindGroupDescriptor = 
    {
        Next : IBindGroupDescriptorExtension
        Label : string
        Layout : BindGroupLayout
        Entries : array<BindGroupEntry>
    }
    static member Null = Unchecked.defaultof<BindGroupDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.BindGroupDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                try
                    let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                    WebGPU.Raw.Pinnable.pinArray device this.Entries (fun entriesPtr ->
                        let entriesLen = unativeint this.Entries.Length
                        let mutable value =
                            new WebGPU.Raw.BindGroupDescriptor(
                                nextInChain,
                                _labelLen,
                                this.Layout.Handle,
                                entriesLen,
                                entriesPtr
                            )
                        use ptr = fixed &value
                        try action ptr
                        finally ()
                    )
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.BindGroupDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.BindGroupDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
            if NativePtr.toNativeInt backend.Entries <> 0n then
                backend.Entries <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Entries)
        {
            Next = ExtensionDecoder.decode<IBindGroupDescriptorExtension> device relativePointers backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            Layout = new BindGroupLayout(backend.Layout)
            Entries = let ptr = backend.Entries in Array.init (int backend.EntryCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in BindGroupEntry.Read(device, NativePtr.add ptr i, relativePointers))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.BindGroupDescriptor>) = 
        use ptr = fixed &r
        BindGroupDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        BindGroupDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.BindGroupDescriptor>
type BindGroupLayout internal(handle : nativeint) =
    static let device = Unchecked.defaultof<Device>
    static let nullptr = new BindGroupLayout(Unchecked.defaultof<_>)
    member x.Handle = handle
    override x.ToString() = $"BindGroupLayout(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? BindGroupLayout as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.BindGroupLayoutSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.BindGroupLayoutRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.BindGroupLayoutAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type BufferBindingLayout = 
    {
        Type : BufferBindingType
        HasDynamicOffset : bool
        MinBindingSize : int64
    }
    static member Null = Unchecked.defaultof<BufferBindingLayout>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.BufferBindingLayout> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable value =
                new WebGPU.Raw.BufferBindingLayout(
                    nextInChain,
                    this.Type,
                    (if this.HasDynamicOffset then 1 else 0),
                    uint64(this.MinBindingSize)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.BufferBindingLayout> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.BufferBindingLayout>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Type = backend.Type
            HasDynamicOffset = (backend.HasDynamicOffset <> 0)
            MinBindingSize = int64(backend.MinBindingSize)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.BufferBindingLayout>) = 
        use ptr = fixed &r
        BufferBindingLayout.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        BufferBindingLayout.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.BufferBindingLayout>
type SamplerBindingLayout = 
    {
        Type : SamplerBindingType
    }
    static member Null = Unchecked.defaultof<SamplerBindingLayout>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SamplerBindingLayout> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable value =
                new WebGPU.Raw.SamplerBindingLayout(
                    nextInChain,
                    this.Type
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SamplerBindingLayout> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SamplerBindingLayout>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Type = backend.Type
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SamplerBindingLayout>) = 
        use ptr = fixed &r
        SamplerBindingLayout.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SamplerBindingLayout.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SamplerBindingLayout>
type StaticSamplerBindingLayout = 
    {
        Next : IBindGroupLayoutEntryExtension
        Sampler : Sampler
        SampledTextureBinding : int
    }
    static member Null = Unchecked.defaultof<StaticSamplerBindingLayout>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.StaticSamplerBindingLayout> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.StaticSamplerBindingLayout
                let mutable value =
                    new WebGPU.Raw.StaticSamplerBindingLayout(
                        nextInChain,
                        sType,
                        this.Sampler.Handle,
                        uint32(this.SampledTextureBinding)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IBindGroupLayoutEntryExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.StaticSamplerBindingLayout> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.StaticSamplerBindingLayout>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IBindGroupLayoutEntryExtension> device relativePointers backend.NextInChain
            Sampler = new Sampler(device, backend.Sampler)
            SampledTextureBinding = int(backend.SampledTextureBinding)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.StaticSamplerBindingLayout>) = 
        use ptr = fixed &r
        StaticSamplerBindingLayout.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        StaticSamplerBindingLayout.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.StaticSamplerBindingLayout>
type TextureBindingLayout = 
    {
        SampleType : TextureSampleType
        ViewDimension : TextureViewDimension
        Multisampled : bool
    }
    static member Null = Unchecked.defaultof<TextureBindingLayout>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.TextureBindingLayout> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable value =
                new WebGPU.Raw.TextureBindingLayout(
                    nextInChain,
                    this.SampleType,
                    this.ViewDimension,
                    (if this.Multisampled then 1 else 0)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.TextureBindingLayout> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.TextureBindingLayout>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            SampleType = backend.SampleType
            ViewDimension = backend.ViewDimension
            Multisampled = (backend.Multisampled <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.TextureBindingLayout>) = 
        use ptr = fixed &r
        TextureBindingLayout.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        TextureBindingLayout.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.TextureBindingLayout>
type SurfaceCapabilities = 
    {
        Usages : TextureUsage
        Formats : array<TextureFormat>
        PresentModes : array<PresentMode>
        AlphaModes : array<CompositeAlphaMode>
    }
    static member Null = Unchecked.defaultof<SurfaceCapabilities>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SurfaceCapabilities> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            use formatsPtr = fixed (this.Formats)
            try
                let formatsLen = unativeint this.Formats.Length
                use presentModesPtr = fixed (this.PresentModes)
                try
                    let presentModesLen = unativeint this.PresentModes.Length
                    use alphaModesPtr = fixed (this.AlphaModes)
                    try
                        let alphaModesLen = unativeint this.AlphaModes.Length
                        let mutable value =
                            new WebGPU.Raw.SurfaceCapabilities(
                                nextInChain,
                                this.Usages,
                                formatsLen,
                                formatsPtr,
                                presentModesLen,
                                presentModesPtr,
                                alphaModesLen,
                                alphaModesPtr
                            )
                        use ptr = fixed &value
                        try action ptr
                        finally ()
                    finally
                        ()
                finally
                    ()
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SurfaceCapabilities> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SurfaceCapabilities>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Formats <> 0n then
                backend.Formats <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Formats)
            if NativePtr.toNativeInt backend.PresentModes <> 0n then
                backend.PresentModes <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.PresentModes)
            if NativePtr.toNativeInt backend.AlphaModes <> 0n then
                backend.AlphaModes <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.AlphaModes)
        {
            Usages = backend.Usages
            Formats = let ptr = backend.Formats in Array.init (int backend.FormatCount) (fun i -> NativePtr.get ptr i)
            PresentModes = let ptr = backend.PresentModes in Array.init (int backend.PresentModeCount) (fun i -> NativePtr.get ptr i)
            AlphaModes = let ptr = backend.AlphaModes in Array.init (int backend.AlphaModeCount) (fun i -> NativePtr.get ptr i)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SurfaceCapabilities>) = 
        use ptr = fixed &r
        SurfaceCapabilities.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SurfaceCapabilities.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SurfaceCapabilities>
type SurfaceConfiguration = 
    {
        Device : Device
        Format : TextureFormat
        Usage : TextureUsage
        Width : int
        Height : int
        ViewFormats : array<TextureFormat>
        AlphaMode : CompositeAlphaMode
        PresentMode : PresentMode
    }
    static member Null = Unchecked.defaultof<SurfaceConfiguration>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SurfaceConfiguration> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            use viewFormatsPtr = fixed (this.ViewFormats)
            try
                let viewFormatsLen = unativeint this.ViewFormats.Length
                let mutable value =
                    new WebGPU.Raw.SurfaceConfiguration(
                        nextInChain,
                        this.Device.Handle,
                        this.Format,
                        this.Usage,
                        uint32(this.Width),
                        uint32(this.Height),
                        viewFormatsLen,
                        viewFormatsPtr,
                        this.AlphaMode,
                        this.PresentMode
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SurfaceConfiguration> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SurfaceConfiguration>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.ViewFormats <> 0n then
                backend.ViewFormats <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.ViewFormats)
        {
            Device = new Device(backend.Device)
            Format = backend.Format
            Usage = backend.Usage
            Width = int(backend.Width)
            Height = int(backend.Height)
            ViewFormats = let ptr = backend.ViewFormats in Array.init (int backend.ViewFormatCount) (fun i -> NativePtr.get ptr i)
            AlphaMode = backend.AlphaMode
            PresentMode = backend.PresentMode
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SurfaceConfiguration>) = 
        use ptr = fixed &r
        SurfaceConfiguration.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SurfaceConfiguration.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SurfaceConfiguration>
type ExternalTextureBindingEntry = 
    {
        Next : IBindGroupEntryExtension
        ExternalTexture : ExternalTexture
    }
    static member Null = Unchecked.defaultof<ExternalTextureBindingEntry>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.ExternalTextureBindingEntry> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.ExternalTextureBindingEntry
                let mutable value =
                    new WebGPU.Raw.ExternalTextureBindingEntry(
                        nextInChain,
                        sType,
                        this.ExternalTexture.Handle
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IBindGroupEntryExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.ExternalTextureBindingEntry> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.ExternalTextureBindingEntry>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IBindGroupEntryExtension> device relativePointers backend.NextInChain
            ExternalTexture = new ExternalTexture(device, backend.ExternalTexture)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.ExternalTextureBindingEntry>) = 
        use ptr = fixed &r
        ExternalTextureBindingEntry.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        ExternalTextureBindingEntry.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.ExternalTextureBindingEntry>
type ExternalTextureBindingLayout = 
    {
        Next : IBindGroupLayoutEntryExtension
    }
    static member Null = Unchecked.defaultof<ExternalTextureBindingLayout>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.ExternalTextureBindingLayout> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.ExternalTextureBindingLayout
                let mutable value =
                    new WebGPU.Raw.ExternalTextureBindingLayout(
                        nextInChain,
                        sType
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IBindGroupLayoutEntryExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.ExternalTextureBindingLayout> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.ExternalTextureBindingLayout>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IBindGroupLayoutEntryExtension> device relativePointers backend.NextInChain
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.ExternalTextureBindingLayout>) = 
        use ptr = fixed &r
        ExternalTextureBindingLayout.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        ExternalTextureBindingLayout.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.ExternalTextureBindingLayout>
type StorageTextureBindingLayout = 
    {
        Access : StorageTextureAccess
        Format : TextureFormat
        ViewDimension : TextureViewDimension
    }
    static member Null = Unchecked.defaultof<StorageTextureBindingLayout>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.StorageTextureBindingLayout> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable value =
                new WebGPU.Raw.StorageTextureBindingLayout(
                    nextInChain,
                    this.Access,
                    this.Format,
                    this.ViewDimension
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.StorageTextureBindingLayout> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.StorageTextureBindingLayout>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Access = backend.Access
            Format = backend.Format
            ViewDimension = backend.ViewDimension
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.StorageTextureBindingLayout>) = 
        use ptr = fixed &r
        StorageTextureBindingLayout.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        StorageTextureBindingLayout.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.StorageTextureBindingLayout>
type BindGroupLayoutEntry = 
    {
        Next : IBindGroupLayoutEntryExtension
        Binding : int
        Visibility : ShaderStage
        BindingArraySize : int
        Buffer : BufferBindingLayout
        Sampler : SamplerBindingLayout
        Texture : TextureBindingLayout
        StorageTexture : StorageTextureBindingLayout
    }
    static member Null = Unchecked.defaultof<BindGroupLayoutEntry>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.BindGroupLayoutEntry> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                this.Buffer.Pin(device, fun _bufferPtr ->
                    this.Sampler.Pin(device, fun _samplerPtr ->
                        this.Texture.Pin(device, fun _texturePtr ->
                            this.StorageTexture.Pin(device, fun _storageTexturePtr ->
                                let mutable value =
                                    new WebGPU.Raw.BindGroupLayoutEntry(
                                        nextInChain,
                                        uint32(this.Binding),
                                        this.Visibility,
                                        uint32(this.BindingArraySize),
                                        (if NativePtr.toNativeInt _bufferPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _bufferPtr),
                                        (if NativePtr.toNativeInt _samplerPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _samplerPtr),
                                        (if NativePtr.toNativeInt _texturePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _texturePtr),
                                        (if NativePtr.toNativeInt _storageTexturePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _storageTexturePtr)
                                    )
                                use ptr = fixed &value
                                try action ptr
                                finally ()
                            )
                        )
                    )
                )
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.BindGroupLayoutEntry> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.BindGroupLayoutEntry>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IBindGroupLayoutEntryExtension> device relativePointers backend.NextInChain
            Binding = int(backend.Binding)
            Visibility = backend.Visibility
            BindingArraySize = int(backend.BindingArraySize)
            Buffer = use pppp = fixed &backend.Buffer in BufferBindingLayout.Read(device, pppp, relativePointers)
            Sampler = use pppp = fixed &backend.Sampler in SamplerBindingLayout.Read(device, pppp, relativePointers)
            Texture = use pppp = fixed &backend.Texture in TextureBindingLayout.Read(device, pppp, relativePointers)
            StorageTexture = use pppp = fixed &backend.StorageTexture in StorageTextureBindingLayout.Read(device, pppp, relativePointers)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.BindGroupLayoutEntry>) = 
        use ptr = fixed &r
        BindGroupLayoutEntry.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        BindGroupLayoutEntry.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.BindGroupLayoutEntry>
type DynamicBindingArrayLayout = 
    {
        Start : int
        Kind : DynamicBindingKind
    }
    static member Null = Unchecked.defaultof<DynamicBindingArrayLayout>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DynamicBindingArrayLayout> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable value =
                new WebGPU.Raw.DynamicBindingArrayLayout(
                    nextInChain,
                    uint32(this.Start),
                    this.Kind
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DynamicBindingArrayLayout> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DynamicBindingArrayLayout>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Start = int(backend.Start)
            Kind = backend.Kind
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DynamicBindingArrayLayout>) = 
        use ptr = fixed &r
        DynamicBindingArrayLayout.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DynamicBindingArrayLayout.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DynamicBindingArrayLayout>
type BindGroupLayoutDynamicBindingArray = 
    {
        Next : IBindGroupLayoutDescriptorExtension
        DynamicArray : DynamicBindingArrayLayout
    }
    static member Null = Unchecked.defaultof<BindGroupLayoutDynamicBindingArray>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.BindGroupLayoutDynamicBindingArray> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.BindGroupLayoutDynamicBindingArray
                this.DynamicArray.Pin(device, fun _dynamicArrayPtr ->
                    let mutable value =
                        new WebGPU.Raw.BindGroupLayoutDynamicBindingArray(
                            nextInChain,
                            sType,
                            (if NativePtr.toNativeInt _dynamicArrayPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _dynamicArrayPtr)
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                )
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IBindGroupLayoutDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.BindGroupLayoutDynamicBindingArray> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.BindGroupLayoutDynamicBindingArray>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IBindGroupLayoutDescriptorExtension> device relativePointers backend.NextInChain
            DynamicArray = use pppp = fixed &backend.DynamicArray in DynamicBindingArrayLayout.Read(device, pppp, relativePointers)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.BindGroupLayoutDynamicBindingArray>) = 
        use ptr = fixed &r
        BindGroupLayoutDynamicBindingArray.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        BindGroupLayoutDynamicBindingArray.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.BindGroupLayoutDynamicBindingArray>
type BindGroupLayoutDescriptor = 
    {
        Next : IBindGroupLayoutDescriptorExtension
        Label : string
        Entries : array<BindGroupLayoutEntry>
    }
    static member Null = Unchecked.defaultof<BindGroupLayoutDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.BindGroupLayoutDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                try
                    let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                    WebGPU.Raw.Pinnable.pinArray device this.Entries (fun entriesPtr ->
                        let entriesLen = unativeint this.Entries.Length
                        let mutable value =
                            new WebGPU.Raw.BindGroupLayoutDescriptor(
                                nextInChain,
                                _labelLen,
                                entriesLen,
                                entriesPtr
                            )
                        use ptr = fixed &value
                        try action ptr
                        finally ()
                    )
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.BindGroupLayoutDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.BindGroupLayoutDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
            if NativePtr.toNativeInt backend.Entries <> 0n then
                backend.Entries <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Entries)
        {
            Next = ExtensionDecoder.decode<IBindGroupLayoutDescriptorExtension> device relativePointers backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            Entries = let ptr = backend.Entries in Array.init (int backend.EntryCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in BindGroupLayoutEntry.Read(device, NativePtr.add ptr i, relativePointers))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.BindGroupLayoutDescriptor>) = 
        use ptr = fixed &r
        BindGroupLayoutDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        BindGroupLayoutDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.BindGroupLayoutDescriptor>
type BlendComponent = 
    {
        Operation : BlendOperation
        SrcFactor : BlendFactor
        DstFactor : BlendFactor
    }
    static member Null = Unchecked.defaultof<BlendComponent>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.BlendComponent> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.BlendComponent(
                    this.Operation,
                    this.SrcFactor,
                    this.DstFactor
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.BlendComponent> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.BlendComponent>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            Operation = backend.Operation
            SrcFactor = backend.SrcFactor
            DstFactor = backend.DstFactor
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.BlendComponent>) = 
        use ptr = fixed &r
        BlendComponent.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        BlendComponent.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.BlendComponent>
type StringView = 
    {
        Data : string
        Length : int64
    }
    static member Null = Unchecked.defaultof<StringView>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.StringView> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            use _dataPtr = fixed (if isNull this.Data then null else Encoding.UTF8.GetBytes(this.Data))
            try
                let mutable value =
                    new WebGPU.Raw.StringView(
                        _dataPtr,
                        unativeint(this.Length)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.StringView> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.StringView>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if NativePtr.toNativeInt backend.Data <> 0n then
                backend.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Data)
        {
            Data = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Data)
            Length = int64(backend.Length)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.StringView>) = 
        use ptr = fixed &r
        StringView.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        StringView.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.StringView>
[<DebuggerTypeProxy(typeof<BufferProxy>)>]
type Buffer internal(device : Device, handle : nativeint) =
    static let nullptr = new Buffer(Unchecked.defaultof<_>, Unchecked.defaultof<_>)
    let usage =
        lazy (
            let relativePointers = false
            let mutable res = WebGPU.Raw.WebGPU.BufferGetUsage(handle)
            res
        )
    let size =
        lazy (
            let relativePointers = false
            let mutable res = WebGPU.Raw.WebGPU.BufferGetSize(handle)
            int64(res)
        )
    let mapState =
        lazy (
            let relativePointers = false
            let mutable res = WebGPU.Raw.WebGPU.BufferGetMapState(handle)
            res
        )
    let mutable name : string = null
    member x.Handle = handle
    member x.Device = device
    override x.ToString() = $"Buffer(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? Buffer as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    interface Aardvark.Rendering.IBufferRange with
        member x.Buffer = x
        member x.Offset = 0UL
        member x.SizeInBytes = uint64 x.Size
    
    interface Aardvark.Rendering.IBackendBuffer with
        member x.Handle = uint64 handle
        member x.Runtime = device.Runtime
        member x.Name
            with get() = name
            and set(v) = name <- v
    member this.MapAsync(mode : MapMode, offset : int64, size : int64, callbackInfo : BufferMapCallbackInfo) : Future =
        let relativePointers = false
        callbackInfo.Pin(device, fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.BufferMapAsync(handle, mode, unativeint(offset), unativeint(size), (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
            use pppp = fixed &res in Future.Read(device, pppp, relativePointers)
        )
    member this.GetMappedRange(offset : int64, size : int64) : nativeint =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.BufferGetMappedRange(handle, unativeint(offset), unativeint(size))
        res
    member this.GetConstMappedRange(offset : int64, size : int64) : nativeint =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.BufferGetConstMappedRange(handle, unativeint(offset), unativeint(size))
        res
    member this.WriteMappedRange(offset : int64, data : nativeint, size : int64) : Status =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.BufferWriteMappedRange(handle, unativeint(offset), data, unativeint(size))
        res
    member this.ReadMappedRange(offset : int64, data : nativeint, size : int64) : Status =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.BufferReadMappedRange(handle, unativeint(offset), data, unativeint(size))
        res
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.BufferSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Usage : BufferUsage =
        usage.Value
    member this.Size : int64 =
        size.Value
    member this.MapState : BufferMapState =
        mapState.Value
    member this.Unmap() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.BufferUnmap(handle)
        res
    member this.Destroy() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.BufferDestroy(handle)
        res
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.BufferRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.BufferAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
    member buffer.ToByteArray(offset : int64, size : int64) : byte[] =
        let device = buffer.Device
        let queue = device.Queue
        let arr = Array.zeroCreate<byte> (int size)
        use tmp = device.CreateBuffer { Next = null; Label = null; Size = size; MappedAtCreation = false; Usage = BufferUsage.MapRead ||| BufferUsage.CopyDst }
        use enc = buffer.Device.CreateCommandEncoder { Label = null; Next = null }
        enc.CopyBufferToBuffer(buffer, offset, tmp, 0L, size)
        use cmd = enc.Finish { Label = null }
        queue.Submit [| cmd |] |> ignore
        let f = queue.OnSubmittedWorkDone { Mode = CallbackMode.WaitAnyOnly; Callback = QueueWorkDoneCallback (fun _ _ _ -> ()) }
        device.Instance.WaitAny([| { FutureWaitInfo.Future = f; Completed = false } |], 1000000000L) |> ignore
        let info : BufferMapCallbackInfo =
           {
               Mode = CallbackMode.WaitAnyOnly
               Callback = BufferMapCallback(fun d status msg -> ())
           }
        let f = tmp.MapAsync(MapMode.Read, 0L, size, info)
        device.Instance.WaitAny([| { FutureWaitInfo.Future = f; Completed = false } |], 1000000000L) |> ignore
        let ptr = tmp.GetConstMappedRange(0L, size)
        System.Runtime.InteropServices.Marshal.Copy(ptr, arr, 0, arr.Length)
        tmp.Unmap()
        arr
type BufferDescriptor = 
    {
        Next : IBufferDescriptorExtension
        Label : string
        Usage : BufferUsage
        Size : int64
        MappedAtCreation : bool
    }
    static member Null = Unchecked.defaultof<BufferDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.BufferDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                try
                    let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                    let mutable value =
                        new WebGPU.Raw.BufferDescriptor(
                            nextInChain,
                            _labelLen,
                            this.Usage,
                            uint64(this.Size),
                            (if this.MappedAtCreation then 1 else 0)
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.BufferDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.BufferDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
        {
            Next = ExtensionDecoder.decode<IBufferDescriptorExtension> device relativePointers backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            Usage = backend.Usage
            Size = int64(backend.Size)
            MappedAtCreation = (backend.MappedAtCreation <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.BufferDescriptor>) = 
        use ptr = fixed &r
        BufferDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        BufferDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.BufferDescriptor>
type BufferHostMappedPointer = 
    {
        Next : IBufferDescriptorExtension
        Pointer : nativeint
        DisposeCallback : Callback
    }
    static member Null = Unchecked.defaultof<BufferHostMappedPointer>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.BufferHostMappedPointer> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.BufferHostMappedPointer
                let mutable _disposeCallbackPtr = 0n
                if not (isNull (this.DisposeCallback :> obj)) then
                    let mutable _disposeCallbackGC = Unchecked.defaultof<GCHandle>
                    let mutable _disposeCallbackDel = Unchecked.defaultof<WebGPU.Raw.Callback>
                    _disposeCallbackDel <- WebGPU.Raw.Callback(fun userdata ->
                        this.DisposeCallback.Invoke({ new IDisposable with member __.Dispose() = _disposeCallbackGC.Free() })
                    )
                    _disposeCallbackGC <- GCHandle.Alloc(_disposeCallbackDel)
                    _disposeCallbackPtr <- Marshal.GetFunctionPointerForDelegate(_disposeCallbackDel)
                let mutable value =
                    new WebGPU.Raw.BufferHostMappedPointer(
                        nextInChain,
                        sType,
                        this.Pointer,
                        _disposeCallbackPtr,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IBufferDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.BufferHostMappedPointer> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.BufferHostMappedPointer>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IBufferDescriptorExtension> device relativePointers backend.NextInChain
            Pointer = backend.Pointer
            DisposeCallback = failwith "cannot read callbacks"//TODO2 map [(dispose callback, backend.DisposeCallback); (next in chain, backend.NextInChain); (pointer, backend.Pointer); ... ]
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.BufferHostMappedPointer>) = 
        use ptr = fixed &r
        BufferHostMappedPointer.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        BufferHostMappedPointer.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.BufferHostMappedPointer>
type Callback = delegate of IDisposable -> unit
type BufferMapCallback = delegate of IDisposable * status : MapAsyncStatus * message : string -> unit
type BufferMapCallbackInfo = 
    {
        Mode : CallbackMode
        Callback : BufferMapCallback
    }
    static member Null = Unchecked.defaultof<BufferMapCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.BufferMapCallbackInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable _callbackPtr = 0n
            if not (isNull (this.Callback :> obj)) then
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.BufferMapCallback>
                _callbackDel <- WebGPU.Raw.BufferMapCallback(fun status message userdata1 userdata2 ->
                    let _status = status
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                _callbackPtr <- Marshal.GetFunctionPointerForDelegate(_callbackDel)
            let mutable value =
                new WebGPU.Raw.BufferMapCallbackInfo(
                    nextInChain,
                    this.Mode,
                    _callbackPtr,
                    Unchecked.defaultof<_>,
                    Unchecked.defaultof<_>
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.BufferMapCallbackInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.BufferMapCallbackInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.BufferMapCallbackInfo>) = 
        use ptr = fixed &r
        BufferMapCallbackInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        BufferMapCallbackInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.BufferMapCallbackInfo>
type Color = 
    {
        R : double
        G : double
        B : double
        A : double
    }
    static member Null = Unchecked.defaultof<Color>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.Color> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.Color(
                    this.R,
                    this.G,
                    this.B,
                    this.A
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.Color> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.Color>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            R = backend.R
            G = backend.G
            B = backend.B
            A = backend.A
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.Color>) = 
        use ptr = fixed &r
        Color.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        Color.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.Color>
type ConstantEntry = 
    {
        Key : string
        Value : double
    }
    static member Null = Unchecked.defaultof<ConstantEntry>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.ConstantEntry> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let _keyArr = if isNull this.Key then null else Encoding.UTF8.GetBytes(this.Key)
            use _keyPtr = fixed _keyArr
            try
                let _keyLen = WebGPU.Raw.StringView(_keyPtr, if isNull _keyArr then 0un else unativeint _keyArr.Length)
                let mutable value =
                    new WebGPU.Raw.ConstantEntry(
                        nextInChain,
                        _keyLen,
                        this.Value
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.ConstantEntry> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.ConstantEntry>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Key.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Key - NativePtr.toNativeInt &&backend
                backend.Key.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Key.Data)
        {
            Key = let _keyPtr = NativePtr.toNativeInt(backend.Key.Data) in if _keyPtr = 0n then null else Marshal.PtrToStringUTF8(_keyPtr, int(backend.Key.Length))
            Value = backend.Value
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.ConstantEntry>) = 
        use ptr = fixed &r
        ConstantEntry.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        ConstantEntry.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.ConstantEntry>
type CommandBuffer internal(handle : nativeint) =
    static let device = Unchecked.defaultof<Device>
    static let nullptr = new CommandBuffer(Unchecked.defaultof<_>)
    let mutable cleanup : ResizeArray<unit -> unit> = null
    let mutable afterRun : ResizeArray<unit -> System.Threading.Tasks.Task> = null
    member internal x.Cleanup
        with get() = cleanup
        and set(v) = cleanup <- v
    member internal x.AfterRun
        with get() = afterRun
        and set(v) = afterRun <- v
    member internal x.RunCleanup() = 
        if not (isNull cleanup) then
            for a in cleanup do a()
            cleanup <- null
    member internal x.RunCompleted() = 
        task {
            if not (isNull afterRun) then
                for a in afterRun do do! a()
                afterRun <- null
        }
    member x.Handle = handle
    override x.ToString() = $"CommandBuffer(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? CommandBuffer as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.CommandBufferSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.CommandBufferRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.CommandBufferAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.RunCleanup()
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type CommandBufferDescriptor = 
    {
        Label : string
    }
    static member Null = Unchecked.defaultof<CommandBufferDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.CommandBufferDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
            use _labelPtr = fixed _labelArr
            try
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                let mutable value =
                    new WebGPU.Raw.CommandBufferDescriptor(
                        nextInChain,
                        _labelLen
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.CommandBufferDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.CommandBufferDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
        {
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.CommandBufferDescriptor>) = 
        use ptr = fixed &r
        CommandBufferDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        CommandBufferDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.CommandBufferDescriptor>
type CommandEncoder internal(device : Device, handle : nativeint) =
    static let nullptr = new CommandEncoder(Unchecked.defaultof<_>, Unchecked.defaultof<_>)
    let mutable afterRun : ResizeArray<unit -> System.Threading.Tasks.Task> = null
    let mutable cleanup : ResizeArray<unit -> unit> = null
    member x.AddCleanup(action : unit -> unit) =
        if isNull cleanup then cleanup <- ResizeArray()
        cleanup.Add(action)
    member x.AddAfterRun(action : unit -> System.Threading.Tasks.Task) =
        if isNull afterRun then afterRun <- ResizeArray()
        afterRun.Add(action)
    member x.Handle = handle
    member x.Device = device
    override x.ToString() = $"CommandEncoder(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? CommandEncoder as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.Finish(descriptor : CommandBufferDescriptor) : CommandBuffer =
        let relativePointers = false
        let res =
            descriptor.Pin(device, fun _descriptorPtr ->
                let res = WebGPU.Raw.WebGPU.CommandEncoderFinish(handle, _descriptorPtr)
                new CommandBuffer(res)
            )
        res.Cleanup <- cleanup
        res.AfterRun <- afterRun
        cleanup <- null
        afterRun <- null
        res
    member this.BeginComputePass(descriptor : ComputePassDescriptor) : ComputePassEncoder =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.CommandEncoderBeginComputePass(handle, _descriptorPtr)
            new ComputePassEncoder(res)
        )
    member this.BeginRenderPass(descriptor : RenderPassDescriptor) : RenderPassEncoder =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.CommandEncoderBeginRenderPass(handle, _descriptorPtr)
            new RenderPassEncoder(res)
        )
    member this.CopyBufferToBuffer(source : Buffer, sourceOffset : int64, destination : Buffer, destinationOffset : int64, size : int64) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.CommandEncoderCopyBufferToBuffer(handle, source.Handle, uint64(sourceOffset), destination.Handle, uint64(destinationOffset), uint64(size))
        res
    member this.CopyBufferToTexture(source : TexelCopyBufferInfo, destination : TexelCopyTextureInfo, copySize : Extent3D) : unit =
        let relativePointers = false
        source.Pin(device, fun _sourcePtr ->
            destination.Pin(device, fun _destinationPtr ->
                copySize.Pin(device, fun _copySizePtr ->
                    let res = WebGPU.Raw.WebGPU.CommandEncoderCopyBufferToTexture(handle, _sourcePtr, _destinationPtr, _copySizePtr)
                    res
                )
            )
        )
    member this.CopyTextureToBuffer(source : TexelCopyTextureInfo, destination : TexelCopyBufferInfo, copySize : Extent3D) : unit =
        let relativePointers = false
        source.Pin(device, fun _sourcePtr ->
            destination.Pin(device, fun _destinationPtr ->
                copySize.Pin(device, fun _copySizePtr ->
                    let res = WebGPU.Raw.WebGPU.CommandEncoderCopyTextureToBuffer(handle, _sourcePtr, _destinationPtr, _copySizePtr)
                    res
                )
            )
        )
    member this.CopyTextureToTexture(source : TexelCopyTextureInfo, destination : TexelCopyTextureInfo, copySize : Extent3D) : unit =
        let relativePointers = false
        source.Pin(device, fun _sourcePtr ->
            destination.Pin(device, fun _destinationPtr ->
                copySize.Pin(device, fun _copySizePtr ->
                    let res = WebGPU.Raw.WebGPU.CommandEncoderCopyTextureToTexture(handle, _sourcePtr, _destinationPtr, _copySizePtr)
                    res
                )
            )
        )
    member this.ClearBuffer(buffer : Buffer, offset : int64, size : int64) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.CommandEncoderClearBuffer(handle, buffer.Handle, uint64(offset), uint64(size))
        res
    member this.InjectValidationError(message : string) : unit =
        let relativePointers = false
        let _messageArr = if isNull message then null else Encoding.UTF8.GetBytes(message)
        use _messagePtr = fixed _messageArr
        try
            let _messageLen = WebGPU.Raw.StringView(_messagePtr, if isNull _messageArr then 0un else unativeint _messageArr.Length)
            let res = WebGPU.Raw.WebGPU.CommandEncoderInjectValidationError(handle, _messageLen)
            res
        finally
            ()
    member this.InsertDebugMarker(markerLabel : string) : unit =
        let relativePointers = false
        let _markerLabelArr = if isNull markerLabel then null else Encoding.UTF8.GetBytes(markerLabel)
        use _markerLabelPtr = fixed _markerLabelArr
        try
            let _markerLabelLen = WebGPU.Raw.StringView(_markerLabelPtr, if isNull _markerLabelArr then 0un else unativeint _markerLabelArr.Length)
            let res = WebGPU.Raw.WebGPU.CommandEncoderInsertDebugMarker(handle, _markerLabelLen)
            res
        finally
            ()
    member this.PopDebugGroup() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.CommandEncoderPopDebugGroup(handle)
        res
    member this.PushDebugGroup(groupLabel : string) : unit =
        let relativePointers = false
        let _groupLabelArr = if isNull groupLabel then null else Encoding.UTF8.GetBytes(groupLabel)
        use _groupLabelPtr = fixed _groupLabelArr
        try
            let _groupLabelLen = WebGPU.Raw.StringView(_groupLabelPtr, if isNull _groupLabelArr then 0un else unativeint _groupLabelArr.Length)
            let res = WebGPU.Raw.WebGPU.CommandEncoderPushDebugGroup(handle, _groupLabelLen)
            res
        finally
            ()
    member this.ResolveQuerySet(querySet : QuerySet, firstQuery : int, queryCount : int, destination : Buffer, destinationOffset : int64) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.CommandEncoderResolveQuerySet(handle, querySet.Handle, uint32(firstQuery), uint32(queryCount), destination.Handle, uint64(destinationOffset))
        res
    member this.WriteBuffer(buffer : Buffer, bufferOffset : int64, data : array<uint8>, size : int64) : unit =
        let relativePointers = false
        use dataPtr = fixed (data)
        try
            let dataLen = uint64 data.Length
            let res = WebGPU.Raw.WebGPU.CommandEncoderWriteBuffer(handle, buffer.Handle, uint64(bufferOffset), dataPtr, uint64(size))
            res
        finally
            ()
    member this.WriteTimestamp(querySet : QuerySet, queryIndex : int) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.CommandEncoderWriteTimestamp(handle, querySet.Handle, uint32(queryIndex))
        res
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.CommandEncoderSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.CommandEncoderRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.CommandEncoderAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type CommandEncoderDescriptor = 
    {
        Next : ICommandEncoderDescriptorExtension
        Label : string
    }
    static member Null = Unchecked.defaultof<CommandEncoderDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.CommandEncoderDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                try
                    let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                    let mutable value =
                        new WebGPU.Raw.CommandEncoderDescriptor(
                            nextInChain,
                            _labelLen
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.CommandEncoderDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.CommandEncoderDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
        {
            Next = ExtensionDecoder.decode<ICommandEncoderDescriptorExtension> device relativePointers backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.CommandEncoderDescriptor>) = 
        use ptr = fixed &r
        CommandEncoderDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        CommandEncoderDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.CommandEncoderDescriptor>
type CompilationInfo = 
    {
        Messages : array<CompilationMessage>
    }
    static member Null = Unchecked.defaultof<CompilationInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.CompilationInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            WebGPU.Raw.Pinnable.pinArray device this.Messages (fun messagesPtr ->
                let messagesLen = unativeint this.Messages.Length
                let mutable value =
                    new WebGPU.Raw.CompilationInfo(
                        nextInChain,
                        messagesLen,
                        messagesPtr
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.CompilationInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.CompilationInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Messages <> 0n then
                backend.Messages <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Messages)
        {
            Messages = let ptr = backend.Messages in Array.init (int backend.MessageCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in CompilationMessage.Read(device, NativePtr.add ptr i, relativePointers))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.CompilationInfo>) = 
        use ptr = fixed &r
        CompilationInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        CompilationInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.CompilationInfo>
type CompilationInfoCallback = delegate of IDisposable * status : CompilationInfoRequestStatus * compilationInfo : CompilationInfo -> unit
type CompilationInfoCallbackInfo = 
    {
        Mode : CallbackMode
        Callback : CompilationInfoCallback
    }
    static member Null = Unchecked.defaultof<CompilationInfoCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.CompilationInfoCallbackInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable _callbackPtr = 0n
            if not (isNull (this.Callback :> obj)) then
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.CompilationInfoCallback>
                _callbackDel <- WebGPU.Raw.CompilationInfoCallback(fun status compilationInfo userdata1 userdata2 ->
                    let _status = status
                    let _compilationInfo = CompilationInfo.Read(device, compilationInfo, relativePointers)
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _compilationInfo)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                _callbackPtr <- Marshal.GetFunctionPointerForDelegate(_callbackDel)
            let mutable value =
                new WebGPU.Raw.CompilationInfoCallbackInfo(
                    nextInChain,
                    this.Mode,
                    _callbackPtr,
                    Unchecked.defaultof<_>,
                    Unchecked.defaultof<_>
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.CompilationInfoCallbackInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.CompilationInfoCallbackInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.CompilationInfoCallbackInfo>) = 
        use ptr = fixed &r
        CompilationInfoCallbackInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        CompilationInfoCallbackInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.CompilationInfoCallbackInfo>
type CompilationMessage = 
    {
        Next : ICompilationMessageExtension
        Message : string
        Type : CompilationMessageType
        LineNum : int64
        LinePos : int64
        Offset : int64
        Length : int64
    }
    static member Null = Unchecked.defaultof<CompilationMessage>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.CompilationMessage> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _messageArr = if isNull this.Message then null else Encoding.UTF8.GetBytes(this.Message)
                use _messagePtr = fixed _messageArr
                try
                    let _messageLen = WebGPU.Raw.StringView(_messagePtr, if isNull _messageArr then 0un else unativeint _messageArr.Length)
                    let mutable value =
                        new WebGPU.Raw.CompilationMessage(
                            nextInChain,
                            _messageLen,
                            this.Type,
                            uint64(this.LineNum),
                            uint64(this.LinePos),
                            uint64(this.Offset),
                            uint64(this.Length)
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.CompilationMessage> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.CompilationMessage>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Message.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Message - NativePtr.toNativeInt &&backend
                backend.Message.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Message.Data)
        {
            Next = ExtensionDecoder.decode<ICompilationMessageExtension> device relativePointers backend.NextInChain
            Message = let _messagePtr = NativePtr.toNativeInt(backend.Message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(backend.Message.Length))
            Type = backend.Type
            LineNum = int64(backend.LineNum)
            LinePos = int64(backend.LinePos)
            Offset = int64(backend.Offset)
            Length = int64(backend.Length)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.CompilationMessage>) = 
        use ptr = fixed &r
        CompilationMessage.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        CompilationMessage.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.CompilationMessage>
type DawnCompilationMessageUtf16 = 
    {
        Next : ICompilationMessageExtension
        LinePos : int64
        Offset : int64
        Length : int64
    }
    static member Null = Unchecked.defaultof<DawnCompilationMessageUtf16>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnCompilationMessageUtf16> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnCompilationMessageUtf16
                let mutable value =
                    new WebGPU.Raw.DawnCompilationMessageUtf16(
                        nextInChain,
                        sType,
                        uint64(this.LinePos),
                        uint64(this.Offset),
                        uint64(this.Length)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ICompilationMessageExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnCompilationMessageUtf16> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnCompilationMessageUtf16>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ICompilationMessageExtension> device relativePointers backend.NextInChain
            LinePos = int64(backend.LinePos)
            Offset = int64(backend.Offset)
            Length = int64(backend.Length)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnCompilationMessageUtf16>) = 
        use ptr = fixed &r
        DawnCompilationMessageUtf16.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnCompilationMessageUtf16.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnCompilationMessageUtf16>
type ComputePassDescriptor = 
    {
        Label : string
        TimestampWrites : PassTimestampWrites
    }
    static member Null = Unchecked.defaultof<ComputePassDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.ComputePassDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
            use _labelPtr = fixed _labelArr
            try
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                this.TimestampWrites.Pin(device, fun _timestampWritesPtr ->
                    let mutable value =
                        new WebGPU.Raw.ComputePassDescriptor(
                            nextInChain,
                            _labelLen,
                            _timestampWritesPtr
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                )
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.ComputePassDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.ComputePassDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
            if NativePtr.toNativeInt backend.TimestampWrites <> 0n then
                backend.TimestampWrites <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.TimestampWrites)
        {
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            TimestampWrites = PassTimestampWrites.Read(device, backend.TimestampWrites, relativePointers)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.ComputePassDescriptor>) = 
        use ptr = fixed &r
        ComputePassDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        ComputePassDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.ComputePassDescriptor>
type ComputePassEncoder internal(handle : nativeint) =
    static let device = Unchecked.defaultof<Device>
    static let nullptr = new ComputePassEncoder(Unchecked.defaultof<_>)
    member x.Handle = handle
    override x.ToString() = $"ComputePassEncoder(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? ComputePassEncoder as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.InsertDebugMarker(markerLabel : string) : unit =
        let relativePointers = false
        let _markerLabelArr = if isNull markerLabel then null else Encoding.UTF8.GetBytes(markerLabel)
        use _markerLabelPtr = fixed _markerLabelArr
        try
            let _markerLabelLen = WebGPU.Raw.StringView(_markerLabelPtr, if isNull _markerLabelArr then 0un else unativeint _markerLabelArr.Length)
            let res = WebGPU.Raw.WebGPU.ComputePassEncoderInsertDebugMarker(handle, _markerLabelLen)
            res
        finally
            ()
    member this.PopDebugGroup() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderPopDebugGroup(handle)
        res
    member this.PushDebugGroup(groupLabel : string) : unit =
        let relativePointers = false
        let _groupLabelArr = if isNull groupLabel then null else Encoding.UTF8.GetBytes(groupLabel)
        use _groupLabelPtr = fixed _groupLabelArr
        try
            let _groupLabelLen = WebGPU.Raw.StringView(_groupLabelPtr, if isNull _groupLabelArr then 0un else unativeint _groupLabelArr.Length)
            let res = WebGPU.Raw.WebGPU.ComputePassEncoderPushDebugGroup(handle, _groupLabelLen)
            res
        finally
            ()
    member this.SetPipeline(pipeline : ComputePipeline) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderSetPipeline(handle, pipeline.Handle)
        res
    member this.SetBindGroup(groupIndex : int, group : BindGroup, dynamicOffsets : array<uint32>) : unit =
        let relativePointers = false
        use dynamicOffsetsPtr = fixed (dynamicOffsets)
        try
            let dynamicOffsetsLen = unativeint dynamicOffsets.Length
            let res = WebGPU.Raw.WebGPU.ComputePassEncoderSetBindGroup(handle, uint32(groupIndex), group.Handle, dynamicOffsetsLen, dynamicOffsetsPtr)
            res
        finally
            ()
    member this.WriteTimestamp(querySet : QuerySet, queryIndex : int) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderWriteTimestamp(handle, querySet.Handle, uint32(queryIndex))
        res
    member this.DispatchWorkgroups(workgroupCountX : int, workgroupCountY : int, workgroupCountZ : int) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderDispatchWorkgroups(handle, uint32(workgroupCountX), uint32(workgroupCountY), uint32(workgroupCountZ))
        res
    member this.DispatchWorkgroupsIndirect(indirectBuffer : Buffer, indirectOffset : int64) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderDispatchWorkgroupsIndirect(handle, indirectBuffer.Handle, uint64(indirectOffset))
        res
    member this.End() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderEnd(handle)
        res
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.ComputePassEncoderSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.SetImmediateData(offset : int, data : nativeint, size : int64) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderSetImmediateData(handle, uint32(offset), data, unativeint(size))
        res
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type ComputePipeline internal(handle : nativeint) =
    static let device = Unchecked.defaultof<Device>
    static let nullptr = new ComputePipeline(Unchecked.defaultof<_>)
    member x.Handle = handle
    override x.ToString() = $"ComputePipeline(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? ComputePipeline as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.GetBindGroupLayout(groupIndex : int) : BindGroupLayout =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ComputePipelineGetBindGroupLayout(handle, uint32(groupIndex))
        new BindGroupLayout(res)
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.ComputePipelineSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ComputePipelineRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ComputePipelineAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type ComputePipelineDescriptor = 
    {
        Label : string
        Layout : PipelineLayout
        Compute : ComputeState
    }
    static member Null = Unchecked.defaultof<ComputePipelineDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.ComputePipelineDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
            use _labelPtr = fixed _labelArr
            try
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                this.Compute.Pin(device, fun _computePtr ->
                    let mutable value =
                        new WebGPU.Raw.ComputePipelineDescriptor(
                            nextInChain,
                            _labelLen,
                            this.Layout.Handle,
                            (if NativePtr.toNativeInt _computePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _computePtr)
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                )
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.ComputePipelineDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.ComputePipelineDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
        {
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            Layout = new PipelineLayout(device, backend.Layout)
            Compute = use pppp = fixed &backend.Compute in ComputeState.Read(device, pppp, relativePointers)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.ComputePipelineDescriptor>) = 
        use ptr = fixed &r
        ComputePipelineDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        ComputePipelineDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.ComputePipelineDescriptor>
type CopyTextureForBrowserOptions = 
    {
        FlipY : bool
        NeedsColorSpaceConversion : bool
        SrcAlphaMode : AlphaMode
        SrcTransferFunctionParameters : float32
        ConversionMatrix : float32
        DstTransferFunctionParameters : float32
        DstAlphaMode : AlphaMode
        InternalUsage : bool
    }
    static member Null = Unchecked.defaultof<CopyTextureForBrowserOptions>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.CopyTextureForBrowserOptions> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable srcTransferFunctionParametersHandle = this.SrcTransferFunctionParameters
            use srcTransferFunctionParametersPtr = fixed (&srcTransferFunctionParametersHandle)
            try
                let mutable conversionMatrixHandle = this.ConversionMatrix
                use conversionMatrixPtr = fixed (&conversionMatrixHandle)
                try
                    let mutable dstTransferFunctionParametersHandle = this.DstTransferFunctionParameters
                    use dstTransferFunctionParametersPtr = fixed (&dstTransferFunctionParametersHandle)
                    try
                        let mutable value =
                            new WebGPU.Raw.CopyTextureForBrowserOptions(
                                nextInChain,
                                (if this.FlipY then 1 else 0),
                                (if this.NeedsColorSpaceConversion then 1 else 0),
                                this.SrcAlphaMode,
                                srcTransferFunctionParametersPtr,
                                conversionMatrixPtr,
                                dstTransferFunctionParametersPtr,
                                this.DstAlphaMode,
                                (if this.InternalUsage then 1 else 0)
                            )
                        use ptr = fixed &value
                        try action ptr
                        finally ()
                    finally
                        ()
                finally
                    ()
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.CopyTextureForBrowserOptions> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.CopyTextureForBrowserOptions>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.SrcTransferFunctionParameters <> 0n then
                backend.SrcTransferFunctionParameters <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.SrcTransferFunctionParameters)
            if NativePtr.toNativeInt backend.ConversionMatrix <> 0n then
                backend.ConversionMatrix <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.ConversionMatrix)
            if NativePtr.toNativeInt backend.DstTransferFunctionParameters <> 0n then
                backend.DstTransferFunctionParameters <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.DstTransferFunctionParameters)
        {
            FlipY = (backend.FlipY <> 0)
            NeedsColorSpaceConversion = (backend.NeedsColorSpaceConversion <> 0)
            SrcAlphaMode = backend.SrcAlphaMode
            SrcTransferFunctionParameters = let ptr = backend.SrcTransferFunctionParameters in NativePtr.read ptr
            ConversionMatrix = let ptr = backend.ConversionMatrix in NativePtr.read ptr
            DstTransferFunctionParameters = let ptr = backend.DstTransferFunctionParameters in NativePtr.read ptr
            DstAlphaMode = backend.DstAlphaMode
            InternalUsage = (backend.InternalUsage <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.CopyTextureForBrowserOptions>) = 
        use ptr = fixed &r
        CopyTextureForBrowserOptions.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        CopyTextureForBrowserOptions.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.CopyTextureForBrowserOptions>
type CreateComputePipelineAsyncCallback = delegate of IDisposable * status : CreatePipelineAsyncStatus * pipeline : ComputePipeline * message : string -> unit
type CreateComputePipelineAsyncCallbackInfo = 
    {
        Mode : CallbackMode
        Callback : CreateComputePipelineAsyncCallback
    }
    static member Null = Unchecked.defaultof<CreateComputePipelineAsyncCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable _callbackPtr = 0n
            if not (isNull (this.Callback :> obj)) then
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.CreateComputePipelineAsyncCallback>
                _callbackDel <- WebGPU.Raw.CreateComputePipelineAsyncCallback(fun status pipeline message userdata1 userdata2 ->
                    let _status = status
                    let _pipeline = new ComputePipeline(pipeline)
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _pipeline, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                _callbackPtr <- Marshal.GetFunctionPointerForDelegate(_callbackDel)
            let mutable value =
                new WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo(
                    nextInChain,
                    this.Mode,
                    _callbackPtr,
                    Unchecked.defaultof<_>,
                    Unchecked.defaultof<_>
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo>) = 
        use ptr = fixed &r
        CreateComputePipelineAsyncCallbackInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        CreateComputePipelineAsyncCallbackInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo>
type CreateRenderPipelineAsyncCallback = delegate of IDisposable * status : CreatePipelineAsyncStatus * pipeline : RenderPipeline * message : string -> unit
type CreateRenderPipelineAsyncCallbackInfo = 
    {
        Mode : CallbackMode
        Callback : CreateRenderPipelineAsyncCallback
    }
    static member Null = Unchecked.defaultof<CreateRenderPipelineAsyncCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable _callbackPtr = 0n
            if not (isNull (this.Callback :> obj)) then
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.CreateRenderPipelineAsyncCallback>
                _callbackDel <- WebGPU.Raw.CreateRenderPipelineAsyncCallback(fun status pipeline message userdata1 userdata2 ->
                    let _status = status
                    let _pipeline = new RenderPipeline(pipeline)
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _pipeline, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                _callbackPtr <- Marshal.GetFunctionPointerForDelegate(_callbackDel)
            let mutable value =
                new WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo(
                    nextInChain,
                    this.Mode,
                    _callbackPtr,
                    Unchecked.defaultof<_>,
                    Unchecked.defaultof<_>
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo>) = 
        use ptr = fixed &r
        CreateRenderPipelineAsyncCallbackInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        CreateRenderPipelineAsyncCallbackInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo>
type AHardwareBufferProperties = 
    {
        YCbCrInfo : YCbCrVkDescriptor
    }
    static member Null = Unchecked.defaultof<AHardwareBufferProperties>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.AHardwareBufferProperties> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            this.YCbCrInfo.Pin(device, fun _yCbCrInfoPtr ->
                let mutable value =
                    new WebGPU.Raw.AHardwareBufferProperties(
                        (if NativePtr.toNativeInt _yCbCrInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _yCbCrInfoPtr)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.AHardwareBufferProperties> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.AHardwareBufferProperties>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            YCbCrInfo = use pppp = fixed &backend.YCbCrInfo in YCbCrVkDescriptor.Read(device, pppp, relativePointers)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.AHardwareBufferProperties>) = 
        use ptr = fixed &r
        AHardwareBufferProperties.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        AHardwareBufferProperties.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.AHardwareBufferProperties>
type Device internal(handle : nativeint) as device =
    static let nullptr = new Device(Unchecked.defaultof<_>)
    let mutable runtime : Aardvark.Rendering.IRuntime = Unchecked.defaultof<_>
    let adapter =
        lazy (
            let relativePointers = false
            let mutable res = WebGPU.Raw.WebGPU.DeviceGetAdapter(handle)
            new Adapter(res)
        )
    let queue =
        lazy (
            let relativePointers = false
            let mutable res = WebGPU.Raw.WebGPU.DeviceGetQueue(handle)
            new Queue(device, res)
        )
    member x.Handle = handle
    override x.ToString() = $"Device(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? Device as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member x.Runtime
        with get() : Aardvark.Rendering.IRuntime = runtime
        and set (v : Aardvark.Rendering.IRuntime) = runtime <- v
    member x.EnqueueWait(f : Future) : unit =
        adapter.Value.Instance.EnqueueWait(f)
    member device.CreateBindGroup(descriptor : BindGroupDescriptor) : BindGroup =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateBindGroup(handle, _descriptorPtr)
            new BindGroup(device, res)
        )
    member device.CreateBindGroupLayout(descriptor : BindGroupLayoutDescriptor) : BindGroupLayout =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateBindGroupLayout(handle, _descriptorPtr)
            new BindGroupLayout(res)
        )
    member device.CreateBuffer(descriptor : BufferDescriptor) : Buffer =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateBuffer(handle, _descriptorPtr)
            new Buffer(device, res)
        )
    member device.CreateErrorBuffer(descriptor : BufferDescriptor) : Buffer =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateErrorBuffer(handle, _descriptorPtr)
            new Buffer(device, res)
        )
    member device.CreateCommandEncoder(descriptor : CommandEncoderDescriptor) : CommandEncoder =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateCommandEncoder(handle, _descriptorPtr)
            new CommandEncoder(device, res)
        )
    member device.CreateComputePipeline(descriptor : ComputePipelineDescriptor) : ComputePipeline =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateComputePipeline(handle, _descriptorPtr)
            new ComputePipeline(res)
        )
    member device.CreateComputePipelineAsync(descriptor : ComputePipelineDescriptor, callbackInfo : CreateComputePipelineAsyncCallbackInfo) : Future =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            callbackInfo.Pin(device, fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.DeviceCreateComputePipelineAsync(handle, _descriptorPtr, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
                use pppp = fixed &res in Future.Read(device, pppp, relativePointers)
            )
        )
    member device.CreateExternalTexture(externalTextureDescriptor : ExternalTextureDescriptor) : ExternalTexture =
        let relativePointers = false
        externalTextureDescriptor.Pin(device, fun _externalTextureDescriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateExternalTexture(handle, _externalTextureDescriptorPtr)
            new ExternalTexture(device, res)
        )
    member device.CreateErrorExternalTexture() : ExternalTexture =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.DeviceCreateErrorExternalTexture(handle)
        new ExternalTexture(device, res)
    member device.CreatePipelineLayout(descriptor : PipelineLayoutDescriptor) : PipelineLayout =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreatePipelineLayout(handle, _descriptorPtr)
            new PipelineLayout(device, res)
        )
    member device.CreateQuerySet(descriptor : QuerySetDescriptor) : QuerySet =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateQuerySet(handle, _descriptorPtr)
            new QuerySet(device, res)
        )
    member device.CreateRenderPipelineAsync(descriptor : RenderPipelineDescriptor, callbackInfo : CreateRenderPipelineAsyncCallbackInfo) : Future =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            callbackInfo.Pin(device, fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.DeviceCreateRenderPipelineAsync(handle, _descriptorPtr, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
                use pppp = fixed &res in Future.Read(device, pppp, relativePointers)
            )
        )
    member device.CreateRenderBundleEncoder(descriptor : RenderBundleEncoderDescriptor) : RenderBundleEncoder =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateRenderBundleEncoder(handle, _descriptorPtr)
            new RenderBundleEncoder(device, res)
        )
    member device.CreateRenderPipeline(descriptor : RenderPipelineDescriptor) : RenderPipeline =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateRenderPipeline(handle, _descriptorPtr)
            new RenderPipeline(res)
        )
    member device.CreateSampler(descriptor : SamplerDescriptor) : Sampler =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateSampler(handle, _descriptorPtr)
            new Sampler(device, res)
        )
    member device.CreateShaderModule(descriptor : ShaderModuleDescriptor) : ShaderModule =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateShaderModule(handle, _descriptorPtr)
            new ShaderModule(device, res)
        )
    member device.CreateErrorShaderModule(descriptor : ShaderModuleDescriptor, errorMessage : string) : ShaderModule =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let _errorMessageArr = if isNull errorMessage then null else Encoding.UTF8.GetBytes(errorMessage)
            use _errorMessagePtr = fixed _errorMessageArr
            try
                let _errorMessageLen = WebGPU.Raw.StringView(_errorMessagePtr, if isNull _errorMessageArr then 0un else unativeint _errorMessageArr.Length)
                let res = WebGPU.Raw.WebGPU.DeviceCreateErrorShaderModule(handle, _descriptorPtr, _errorMessageLen)
                new ShaderModule(device, res)
            finally
                ()
        )
    member device.CreateTexture(descriptor : TextureDescriptor) : Texture =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateTexture(handle, _descriptorPtr)
            new Texture(device, res)
        )
    member device.ImportSharedBufferMemory(descriptor : SharedBufferMemoryDescriptor) : SharedBufferMemory =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceImportSharedBufferMemory(handle, _descriptorPtr)
            new SharedBufferMemory(device, res)
        )
    member device.ImportSharedTextureMemory(descriptor : SharedTextureMemoryDescriptor) : SharedTextureMemory =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceImportSharedTextureMemory(handle, _descriptorPtr)
            new SharedTextureMemory(res)
        )
    member device.ImportSharedFence(descriptor : SharedFenceDescriptor) : SharedFence =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceImportSharedFence(handle, _descriptorPtr)
            new SharedFence(res)
        )
    member device.CreateErrorTexture(descriptor : TextureDescriptor) : Texture =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateErrorTexture(handle, _descriptorPtr)
            new Texture(device, res)
        )
    member device.Destroy() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.DeviceDestroy(handle)
        res
    member device.GetAHardwareBufferProperties(handle : nativeint, properties : byref<AHardwareBufferProperties>) : Status =
        let relativePointers = false
        let mutable propertiesCopy = properties
        try
            properties.Pin(device, fun _propertiesPtr ->
                if NativePtr.toNativeInt _propertiesPtr = 0n then
                    let mutable propertiesNative = Unchecked.defaultof<WebGPU.Raw.AHardwareBufferProperties>
                    use _propertiesPtr = fixed &propertiesNative
                    try
                        let res = WebGPU.Raw.WebGPU.DeviceGetAHardwareBufferProperties(handle, handle, _propertiesPtr)
                        let _ret = res
                        propertiesCopy <- AHardwareBufferProperties.Read(device, _propertiesPtr, relativePointers)
                        _ret
                    finally
                        ()
                else
                    let res = WebGPU.Raw.WebGPU.DeviceGetAHardwareBufferProperties(handle, handle, _propertiesPtr)
                    let _ret = res
                    propertiesCopy <- AHardwareBufferProperties.Read(device, _propertiesPtr, relativePointers)
                    _ret
                )
        finally
            properties <- propertiesCopy
    member device.Limits : Limits =
        let relativePointers = false
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        try
            let status = WebGPU.Raw.WebGPU.DeviceGetLimits(handle, ptr)
            if status <> Status.Success then failwith "GetLimits failed"
            use pppp = fixed &res in Limits.Read(device, pppp, relativePointers)
        finally
            ()
    member device.GetLostFuture() : Future =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.DeviceGetLostFuture(handle)
        use pppp = fixed &res in Future.Read(device, pppp, relativePointers)
    member device.HasFeature(feature : FeatureName) : bool =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.DeviceHasFeature(handle, feature)
        (res <> 0)
    member device.Features : SupportedFeatures =
        let relativePointers = false
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        try
            WebGPU.Raw.WebGPU.DeviceGetFeatures(handle, ptr)
            use pppp = fixed &res in SupportedFeatures.Read(device, pppp, relativePointers)
        finally
            ()
    member device.AdapterInfo : AdapterInfo =
        let relativePointers = false
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        try
            let status = WebGPU.Raw.WebGPU.DeviceGetAdapterInfo(handle, ptr)
            if status <> Status.Success then failwith "GetAdapterInfo failed"
            use pppp = fixed &res in AdapterInfo.Read(device, pppp, relativePointers)
        finally
            ()
    member device.Adapter : Adapter =
        adapter.Value
    member device.Queue : Queue =
        queue.Value
    member device.InjectError(typ : ErrorType, message : string) : unit =
        let relativePointers = false
        let _messageArr = if isNull message then null else Encoding.UTF8.GetBytes(message)
        use _messagePtr = fixed _messageArr
        try
            let _messageLen = WebGPU.Raw.StringView(_messagePtr, if isNull _messageArr then 0un else unativeint _messageArr.Length)
            let res = WebGPU.Raw.WebGPU.DeviceInjectError(handle, typ, _messageLen)
            res
        finally
            ()
    member device.ForceLoss(typ : DeviceLostReason, message : string) : unit =
        let relativePointers = false
        let _messageArr = if isNull message then null else Encoding.UTF8.GetBytes(message)
        use _messagePtr = fixed _messageArr
        try
            let _messageLen = WebGPU.Raw.StringView(_messagePtr, if isNull _messageArr then 0un else unativeint _messageArr.Length)
            let res = WebGPU.Raw.WebGPU.DeviceForceLoss(handle, typ, _messageLen)
            res
        finally
            ()
    member device.Tick() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.DeviceTick(handle)
        res
    member device.SetLoggingCallback(callbackInfo : LoggingCallbackInfo) : unit =
        let relativePointers = false
        callbackInfo.Pin(device, fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceSetLoggingCallback(handle, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
            res
        )
    member device.PushErrorScope(filter : ErrorFilter) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.DevicePushErrorScope(handle, filter)
        res
    member device.PopErrorScope(callbackInfo : PopErrorScopeCallbackInfo) : Future =
        let relativePointers = false
        callbackInfo.Pin(device, fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.DevicePopErrorScope(handle, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
            use pppp = fixed &res in Future.Read(device, pppp, relativePointers)
        )
    member device.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.DeviceSetLabel(handle, _labelLen)
            res
        finally
            ()
    member device.ValidateTextureDescriptor(descriptor : TextureDescriptor) : unit =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceValidateTextureDescriptor(handle, _descriptorPtr)
            res
        )
    member device.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.DeviceRelease(handle)
        res
    member device.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.DeviceAddRef(handle)
        res
    member x.Instance : Instance = x.Adapter.Instance
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type DeviceLostCallback = delegate of IDisposable * device : Device * reason : DeviceLostReason * message : string -> unit
type DeviceLostCallbackInfo = 
    {
        Mode : CallbackMode
        Callback : DeviceLostCallback
    }
    static member Null = Unchecked.defaultof<DeviceLostCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DeviceLostCallbackInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable _callbackPtr = 0n
            if not (isNull (this.Callback :> obj)) then
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.DeviceLostCallback>
                _callbackDel <- WebGPU.Raw.DeviceLostCallback(fun device reason message userdata1 userdata2 ->
                    let _device = let ptr = device in new Device(NativePtr.read ptr)
                    let _reason = reason
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _device, _reason, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                _callbackPtr <- Marshal.GetFunctionPointerForDelegate(_callbackDel)
            let mutable value =
                new WebGPU.Raw.DeviceLostCallbackInfo(
                    nextInChain,
                    this.Mode,
                    _callbackPtr,
                    Unchecked.defaultof<_>,
                    Unchecked.defaultof<_>
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DeviceLostCallbackInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DeviceLostCallbackInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DeviceLostCallbackInfo>) = 
        use ptr = fixed &r
        DeviceLostCallbackInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DeviceLostCallbackInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DeviceLostCallbackInfo>
type UncapturedErrorCallback = delegate of IDisposable * device : Device * typ : ErrorType * message : string -> unit
type UncapturedErrorCallbackInfo = 
    {
        Callback : UncapturedErrorCallback
    }
    static member Null = Unchecked.defaultof<UncapturedErrorCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.UncapturedErrorCallbackInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable _callbackPtr = 0n
            if not (isNull (this.Callback :> obj)) then
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.UncapturedErrorCallback>
                _callbackDel <- WebGPU.Raw.UncapturedErrorCallback(fun device typ message userdata1 userdata2 ->
                    let _device = let ptr = device in new Device(NativePtr.read ptr)
                    let _typ = typ
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _device, _typ, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                _callbackPtr <- Marshal.GetFunctionPointerForDelegate(_callbackDel)
            let mutable value =
                new WebGPU.Raw.UncapturedErrorCallbackInfo(
                    nextInChain,
                    _callbackPtr,
                    Unchecked.defaultof<_>,
                    Unchecked.defaultof<_>
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.UncapturedErrorCallbackInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.UncapturedErrorCallbackInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (next in chain, backend.NextInChain); (userdata1, backend.Userdata1); ... ]
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.UncapturedErrorCallbackInfo>) = 
        use ptr = fixed &r
        UncapturedErrorCallbackInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        UncapturedErrorCallbackInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.UncapturedErrorCallbackInfo>
type PopErrorScopeCallback = delegate of IDisposable * status : PopErrorScopeStatus * typ : ErrorType * message : string -> unit
type PopErrorScopeCallbackInfo = 
    {
        Mode : CallbackMode
        Callback : PopErrorScopeCallback
    }
    static member Null = Unchecked.defaultof<PopErrorScopeCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.PopErrorScopeCallbackInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable _callbackPtr = 0n
            if not (isNull (this.Callback :> obj)) then
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.PopErrorScopeCallback>
                _callbackDel <- WebGPU.Raw.PopErrorScopeCallback(fun status typ message userdata1 userdata2 ->
                    let _status = status
                    let _typ = typ
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _typ, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                _callbackPtr <- Marshal.GetFunctionPointerForDelegate(_callbackDel)
            let mutable value =
                new WebGPU.Raw.PopErrorScopeCallbackInfo(
                    nextInChain,
                    this.Mode,
                    _callbackPtr,
                    Unchecked.defaultof<_>,
                    Unchecked.defaultof<_>
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.PopErrorScopeCallbackInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.PopErrorScopeCallbackInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.PopErrorScopeCallbackInfo>) = 
        use ptr = fixed &r
        PopErrorScopeCallbackInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        PopErrorScopeCallbackInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.PopErrorScopeCallbackInfo>
type Limits = 
    {
        Next : ILimitsExtension
        MaxTextureDimension1D : int
        MaxTextureDimension2D : int
        MaxTextureDimension3D : int
        MaxTextureArrayLayers : int
        MaxBindGroups : int
        MaxBindGroupsPlusVertexBuffers : int
        MaxBindingsPerBindGroup : int
        MaxDynamicUniformBuffersPerPipelineLayout : int
        MaxDynamicStorageBuffersPerPipelineLayout : int
        MaxSampledTexturesPerShaderStage : int
        MaxSamplersPerShaderStage : int
        MaxStorageBuffersPerShaderStage : int
        MaxStorageTexturesPerShaderStage : int
        MaxUniformBuffersPerShaderStage : int
        MaxUniformBufferBindingSize : int64
        MaxStorageBufferBindingSize : int64
        MinUniformBufferOffsetAlignment : int
        MinStorageBufferOffsetAlignment : int
        MaxVertexBuffers : int
        MaxBufferSize : int64
        MaxVertexAttributes : int
        MaxVertexBufferArrayStride : int
        MaxInterStageShaderVariables : int
        MaxColorAttachments : int
        MaxColorAttachmentBytesPerSample : int
        MaxComputeWorkgroupStorageSize : int
        MaxComputeInvocationsPerWorkgroup : int
        MaxComputeWorkgroupSizeX : int
        MaxComputeWorkgroupSizeY : int
        MaxComputeWorkgroupSizeZ : int
        MaxComputeWorkgroupsPerDimension : int
        MaxImmediateSize : int
    }
    static member Null = Unchecked.defaultof<Limits>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.Limits> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable value =
                    new WebGPU.Raw.Limits(
                        nextInChain,
                        uint32(this.MaxTextureDimension1D),
                        uint32(this.MaxTextureDimension2D),
                        uint32(this.MaxTextureDimension3D),
                        uint32(this.MaxTextureArrayLayers),
                        uint32(this.MaxBindGroups),
                        uint32(this.MaxBindGroupsPlusVertexBuffers),
                        uint32(this.MaxBindingsPerBindGroup),
                        uint32(this.MaxDynamicUniformBuffersPerPipelineLayout),
                        uint32(this.MaxDynamicStorageBuffersPerPipelineLayout),
                        uint32(this.MaxSampledTexturesPerShaderStage),
                        uint32(this.MaxSamplersPerShaderStage),
                        uint32(this.MaxStorageBuffersPerShaderStage),
                        uint32(this.MaxStorageTexturesPerShaderStage),
                        uint32(this.MaxUniformBuffersPerShaderStage),
                        uint64(this.MaxUniformBufferBindingSize),
                        uint64(this.MaxStorageBufferBindingSize),
                        uint32(this.MinUniformBufferOffsetAlignment),
                        uint32(this.MinStorageBufferOffsetAlignment),
                        uint32(this.MaxVertexBuffers),
                        uint64(this.MaxBufferSize),
                        uint32(this.MaxVertexAttributes),
                        uint32(this.MaxVertexBufferArrayStride),
                        uint32(this.MaxInterStageShaderVariables),
                        uint32(this.MaxColorAttachments),
                        uint32(this.MaxColorAttachmentBytesPerSample),
                        uint32(this.MaxComputeWorkgroupStorageSize),
                        uint32(this.MaxComputeInvocationsPerWorkgroup),
                        uint32(this.MaxComputeWorkgroupSizeX),
                        uint32(this.MaxComputeWorkgroupSizeY),
                        uint32(this.MaxComputeWorkgroupSizeZ),
                        uint32(this.MaxComputeWorkgroupsPerDimension),
                        uint32(this.MaxImmediateSize)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.Limits> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.Limits>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ILimitsExtension> device relativePointers backend.NextInChain
            MaxTextureDimension1D = int(backend.MaxTextureDimension1D)
            MaxTextureDimension2D = int(backend.MaxTextureDimension2D)
            MaxTextureDimension3D = int(backend.MaxTextureDimension3D)
            MaxTextureArrayLayers = int(backend.MaxTextureArrayLayers)
            MaxBindGroups = int(backend.MaxBindGroups)
            MaxBindGroupsPlusVertexBuffers = int(backend.MaxBindGroupsPlusVertexBuffers)
            MaxBindingsPerBindGroup = int(backend.MaxBindingsPerBindGroup)
            MaxDynamicUniformBuffersPerPipelineLayout = int(backend.MaxDynamicUniformBuffersPerPipelineLayout)
            MaxDynamicStorageBuffersPerPipelineLayout = int(backend.MaxDynamicStorageBuffersPerPipelineLayout)
            MaxSampledTexturesPerShaderStage = int(backend.MaxSampledTexturesPerShaderStage)
            MaxSamplersPerShaderStage = int(backend.MaxSamplersPerShaderStage)
            MaxStorageBuffersPerShaderStage = int(backend.MaxStorageBuffersPerShaderStage)
            MaxStorageTexturesPerShaderStage = int(backend.MaxStorageTexturesPerShaderStage)
            MaxUniformBuffersPerShaderStage = int(backend.MaxUniformBuffersPerShaderStage)
            MaxUniformBufferBindingSize = int64(backend.MaxUniformBufferBindingSize)
            MaxStorageBufferBindingSize = int64(backend.MaxStorageBufferBindingSize)
            MinUniformBufferOffsetAlignment = int(backend.MinUniformBufferOffsetAlignment)
            MinStorageBufferOffsetAlignment = int(backend.MinStorageBufferOffsetAlignment)
            MaxVertexBuffers = int(backend.MaxVertexBuffers)
            MaxBufferSize = int64(backend.MaxBufferSize)
            MaxVertexAttributes = int(backend.MaxVertexAttributes)
            MaxVertexBufferArrayStride = int(backend.MaxVertexBufferArrayStride)
            MaxInterStageShaderVariables = int(backend.MaxInterStageShaderVariables)
            MaxColorAttachments = int(backend.MaxColorAttachments)
            MaxColorAttachmentBytesPerSample = int(backend.MaxColorAttachmentBytesPerSample)
            MaxComputeWorkgroupStorageSize = int(backend.MaxComputeWorkgroupStorageSize)
            MaxComputeInvocationsPerWorkgroup = int(backend.MaxComputeInvocationsPerWorkgroup)
            MaxComputeWorkgroupSizeX = int(backend.MaxComputeWorkgroupSizeX)
            MaxComputeWorkgroupSizeY = int(backend.MaxComputeWorkgroupSizeY)
            MaxComputeWorkgroupSizeZ = int(backend.MaxComputeWorkgroupSizeZ)
            MaxComputeWorkgroupsPerDimension = int(backend.MaxComputeWorkgroupsPerDimension)
            MaxImmediateSize = int(backend.MaxImmediateSize)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.Limits>) = 
        use ptr = fixed &r
        Limits.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        Limits.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.Limits>
type CompatibilityModeLimits = 
    {
        Next : ILimitsExtension
        MaxStorageBuffersInVertexStage : int
        MaxStorageTexturesInVertexStage : int
        MaxStorageBuffersInFragmentStage : int
        MaxStorageTexturesInFragmentStage : int
    }
    static member Null = Unchecked.defaultof<CompatibilityModeLimits>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.CompatibilityModeLimits> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.CompatibilityModeLimits
                let mutable value =
                    new WebGPU.Raw.CompatibilityModeLimits(
                        nextInChain,
                        sType,
                        uint32(this.MaxStorageBuffersInVertexStage),
                        uint32(this.MaxStorageTexturesInVertexStage),
                        uint32(this.MaxStorageBuffersInFragmentStage),
                        uint32(this.MaxStorageTexturesInFragmentStage)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ILimitsExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.CompatibilityModeLimits> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.CompatibilityModeLimits>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ILimitsExtension> device relativePointers backend.NextInChain
            MaxStorageBuffersInVertexStage = int(backend.MaxStorageBuffersInVertexStage)
            MaxStorageTexturesInVertexStage = int(backend.MaxStorageTexturesInVertexStage)
            MaxStorageBuffersInFragmentStage = int(backend.MaxStorageBuffersInFragmentStage)
            MaxStorageTexturesInFragmentStage = int(backend.MaxStorageTexturesInFragmentStage)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.CompatibilityModeLimits>) = 
        use ptr = fixed &r
        CompatibilityModeLimits.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        CompatibilityModeLimits.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.CompatibilityModeLimits>
type DawnTexelCopyBufferRowAlignmentLimits = 
    {
        Next : ILimitsExtension
        MinTexelCopyBufferRowAlignment : int
    }
    static member Null = Unchecked.defaultof<DawnTexelCopyBufferRowAlignmentLimits>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnTexelCopyBufferRowAlignmentLimits> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnTexelCopyBufferRowAlignmentLimits
                let mutable value =
                    new WebGPU.Raw.DawnTexelCopyBufferRowAlignmentLimits(
                        nextInChain,
                        sType,
                        uint32(this.MinTexelCopyBufferRowAlignment)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ILimitsExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnTexelCopyBufferRowAlignmentLimits> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnTexelCopyBufferRowAlignmentLimits>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ILimitsExtension> device relativePointers backend.NextInChain
            MinTexelCopyBufferRowAlignment = int(backend.MinTexelCopyBufferRowAlignment)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnTexelCopyBufferRowAlignmentLimits>) = 
        use ptr = fixed &r
        DawnTexelCopyBufferRowAlignmentLimits.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnTexelCopyBufferRowAlignmentLimits.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnTexelCopyBufferRowAlignmentLimits>
type DawnHostMappedPointerLimits = 
    {
        Next : ILimitsExtension
        HostMappedPointerAlignment : int
    }
    static member Null = Unchecked.defaultof<DawnHostMappedPointerLimits>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnHostMappedPointerLimits> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnHostMappedPointerLimits
                let mutable value =
                    new WebGPU.Raw.DawnHostMappedPointerLimits(
                        nextInChain,
                        sType,
                        uint32(this.HostMappedPointerAlignment)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ILimitsExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnHostMappedPointerLimits> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnHostMappedPointerLimits>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ILimitsExtension> device relativePointers backend.NextInChain
            HostMappedPointerAlignment = int(backend.HostMappedPointerAlignment)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnHostMappedPointerLimits>) = 
        use ptr = fixed &r
        DawnHostMappedPointerLimits.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnHostMappedPointerLimits.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnHostMappedPointerLimits>
type DynamicBindingArrayLimits = 
    {
        Next : ILimitsExtension
        MaxDynamicBindingArraySize : int
    }
    static member Null = Unchecked.defaultof<DynamicBindingArrayLimits>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DynamicBindingArrayLimits> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DynamicBindingArrayLimits
                let mutable value =
                    new WebGPU.Raw.DynamicBindingArrayLimits(
                        nextInChain,
                        sType,
                        uint32(this.MaxDynamicBindingArraySize)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ILimitsExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DynamicBindingArrayLimits> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DynamicBindingArrayLimits>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ILimitsExtension> device relativePointers backend.NextInChain
            MaxDynamicBindingArraySize = int(backend.MaxDynamicBindingArraySize)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DynamicBindingArrayLimits>) = 
        use ptr = fixed &r
        DynamicBindingArrayLimits.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DynamicBindingArrayLimits.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DynamicBindingArrayLimits>
type SupportedFeatures = 
    {
        Features : array<FeatureName>
    }
    static member Null = Unchecked.defaultof<SupportedFeatures>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SupportedFeatures> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            use featuresPtr = fixed (this.Features)
            try
                let featuresLen = unativeint this.Features.Length
                let mutable value =
                    new WebGPU.Raw.SupportedFeatures(
                        featuresLen,
                        featuresPtr
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SupportedFeatures> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SupportedFeatures>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if NativePtr.toNativeInt backend.Features <> 0n then
                backend.Features <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Features)
        {
            Features = let ptr = backend.Features in Array.init (int backend.FeatureCount) (fun i -> NativePtr.get ptr i)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SupportedFeatures>) = 
        use ptr = fixed &r
        SupportedFeatures.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SupportedFeatures.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SupportedFeatures>
type SupportedInstanceFeatures = 
    {
        Features : array<InstanceFeatureName>
    }
    static member Null = Unchecked.defaultof<SupportedInstanceFeatures>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SupportedInstanceFeatures> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            use featuresPtr = fixed (this.Features)
            try
                let featuresLen = unativeint this.Features.Length
                let mutable value =
                    new WebGPU.Raw.SupportedInstanceFeatures(
                        featuresLen,
                        featuresPtr
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SupportedInstanceFeatures> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SupportedInstanceFeatures>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if NativePtr.toNativeInt backend.Features <> 0n then
                backend.Features <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Features)
        {
            Features = let ptr = backend.Features in Array.init (int backend.FeatureCount) (fun i -> NativePtr.get ptr i)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SupportedInstanceFeatures>) = 
        use ptr = fixed &r
        SupportedInstanceFeatures.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SupportedInstanceFeatures.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SupportedInstanceFeatures>
type SupportedWGSLLanguageFeatures = 
    {
        Features : array<WGSLLanguageFeatureName>
    }
    static member Null = Unchecked.defaultof<SupportedWGSLLanguageFeatures>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SupportedWGSLLanguageFeatures> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            use featuresPtr = fixed (this.Features)
            try
                let featuresLen = unativeint this.Features.Length
                let mutable value =
                    new WebGPU.Raw.SupportedWGSLLanguageFeatures(
                        featuresLen,
                        featuresPtr
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SupportedWGSLLanguageFeatures> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SupportedWGSLLanguageFeatures>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if NativePtr.toNativeInt backend.Features <> 0n then
                backend.Features <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Features)
        {
            Features = let ptr = backend.Features in Array.init (int backend.FeatureCount) (fun i -> NativePtr.get ptr i)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SupportedWGSLLanguageFeatures>) = 
        use ptr = fixed &r
        SupportedWGSLLanguageFeatures.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SupportedWGSLLanguageFeatures.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SupportedWGSLLanguageFeatures>
type LoggingCallback = delegate of IDisposable * typ : LoggingType * message : string -> unit
type LoggingCallbackInfo = 
    {
        Callback : LoggingCallback
    }
    static member Null = Unchecked.defaultof<LoggingCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.LoggingCallbackInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable _callbackPtr = 0n
            if not (isNull (this.Callback :> obj)) then
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.LoggingCallback>
                _callbackDel <- WebGPU.Raw.LoggingCallback(fun typ message userdata1 userdata2 ->
                    let _typ = typ
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _typ, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                _callbackPtr <- Marshal.GetFunctionPointerForDelegate(_callbackDel)
            let mutable value =
                new WebGPU.Raw.LoggingCallbackInfo(
                    nextInChain,
                    _callbackPtr,
                    Unchecked.defaultof<_>,
                    Unchecked.defaultof<_>
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.LoggingCallbackInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.LoggingCallbackInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (next in chain, backend.NextInChain); (userdata1, backend.Userdata1); ... ]
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.LoggingCallbackInfo>) = 
        use ptr = fixed &r
        LoggingCallbackInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        LoggingCallbackInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.LoggingCallbackInfo>
type Extent2D = 
    {
        Width : int
        Height : int
    }
    static member Null = Unchecked.defaultof<Extent2D>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.Extent2D> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.Extent2D(
                    uint32(this.Width),
                    uint32(this.Height)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.Extent2D> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.Extent2D>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            Width = int(backend.Width)
            Height = int(backend.Height)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.Extent2D>) = 
        use ptr = fixed &r
        Extent2D.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        Extent2D.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.Extent2D>
type Extent3D = 
    {
        Width : int
        Height : int
        DepthOrArrayLayers : int
    }
    static member Null = Unchecked.defaultof<Extent3D>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.Extent3D> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.Extent3D(
                    uint32(this.Width),
                    uint32(this.Height),
                    uint32(this.DepthOrArrayLayers)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.Extent3D> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.Extent3D>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            Width = int(backend.Width)
            Height = int(backend.Height)
            DepthOrArrayLayers = int(backend.DepthOrArrayLayers)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.Extent3D>) = 
        use ptr = fixed &r
        Extent3D.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        Extent3D.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.Extent3D>
type ExternalTexture internal(device : Device, handle : nativeint) =
    static let nullptr = new ExternalTexture(Unchecked.defaultof<_>, Unchecked.defaultof<_>)
    member x.Handle = handle
    member x.Device = device
    override x.ToString() = $"ExternalTexture(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? ExternalTexture as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.ExternalTextureSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Destroy() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ExternalTextureDestroy(handle)
        res
    member this.Expire() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ExternalTextureExpire(handle)
        res
    member this.Refresh() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ExternalTextureRefresh(handle)
        res
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ExternalTextureRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ExternalTextureAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type ExternalTextureDescriptor = 
    {
        Label : string
        Plane0 : TextureView
        Plane1 : TextureView
        CropOrigin : Origin2D
        CropSize : Extent2D
        ApparentSize : Extent2D
        DoYuvToRgbConversionOnly : bool
        YuvToRgbConversionMatrix : float32
        SrcTransferFunctionParameters : float32
        DstTransferFunctionParameters : float32
        GamutConversionMatrix : float32
        Mirrored : bool
        Rotation : ExternalTextureRotation
    }
    static member Null = Unchecked.defaultof<ExternalTextureDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.ExternalTextureDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
            use _labelPtr = fixed _labelArr
            try
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                this.CropOrigin.Pin(device, fun _cropOriginPtr ->
                    this.CropSize.Pin(device, fun _cropSizePtr ->
                        this.ApparentSize.Pin(device, fun _apparentSizePtr ->
                            let mutable yuvToRgbConversionMatrixHandle = this.YuvToRgbConversionMatrix
                            use yuvToRgbConversionMatrixPtr = fixed (&yuvToRgbConversionMatrixHandle)
                            try
                                let mutable srcTransferFunctionParametersHandle = this.SrcTransferFunctionParameters
                                use srcTransferFunctionParametersPtr = fixed (&srcTransferFunctionParametersHandle)
                                try
                                    let mutable dstTransferFunctionParametersHandle = this.DstTransferFunctionParameters
                                    use dstTransferFunctionParametersPtr = fixed (&dstTransferFunctionParametersHandle)
                                    try
                                        let mutable gamutConversionMatrixHandle = this.GamutConversionMatrix
                                        use gamutConversionMatrixPtr = fixed (&gamutConversionMatrixHandle)
                                        try
                                            let mutable value =
                                                new WebGPU.Raw.ExternalTextureDescriptor(
                                                    nextInChain,
                                                    _labelLen,
                                                    this.Plane0.Handle,
                                                    this.Plane1.Handle,
                                                    (if NativePtr.toNativeInt _cropOriginPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _cropOriginPtr),
                                                    (if NativePtr.toNativeInt _cropSizePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _cropSizePtr),
                                                    (if NativePtr.toNativeInt _apparentSizePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _apparentSizePtr),
                                                    (if this.DoYuvToRgbConversionOnly then 1 else 0),
                                                    yuvToRgbConversionMatrixPtr,
                                                    srcTransferFunctionParametersPtr,
                                                    dstTransferFunctionParametersPtr,
                                                    gamutConversionMatrixPtr,
                                                    (if this.Mirrored then 1 else 0),
                                                    this.Rotation
                                                )
                                            use ptr = fixed &value
                                            try action ptr
                                            finally ()
                                        finally
                                            ()
                                    finally
                                        ()
                                finally
                                    ()
                            finally
                                ()
                        )
                    )
                )
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.ExternalTextureDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.ExternalTextureDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
            if NativePtr.toNativeInt backend.YuvToRgbConversionMatrix <> 0n then
                backend.YuvToRgbConversionMatrix <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.YuvToRgbConversionMatrix)
            if NativePtr.toNativeInt backend.SrcTransferFunctionParameters <> 0n then
                backend.SrcTransferFunctionParameters <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.SrcTransferFunctionParameters)
            if NativePtr.toNativeInt backend.DstTransferFunctionParameters <> 0n then
                backend.DstTransferFunctionParameters <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.DstTransferFunctionParameters)
            if NativePtr.toNativeInt backend.GamutConversionMatrix <> 0n then
                backend.GamutConversionMatrix <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.GamutConversionMatrix)
        {
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            Plane0 = new TextureView(backend.Plane0)
            Plane1 = new TextureView(backend.Plane1)
            CropOrigin = use pppp = fixed &backend.CropOrigin in Origin2D.Read(device, pppp, relativePointers)
            CropSize = use pppp = fixed &backend.CropSize in Extent2D.Read(device, pppp, relativePointers)
            ApparentSize = use pppp = fixed &backend.ApparentSize in Extent2D.Read(device, pppp, relativePointers)
            DoYuvToRgbConversionOnly = (backend.DoYuvToRgbConversionOnly <> 0)
            YuvToRgbConversionMatrix = let ptr = backend.YuvToRgbConversionMatrix in NativePtr.read ptr
            SrcTransferFunctionParameters = let ptr = backend.SrcTransferFunctionParameters in NativePtr.read ptr
            DstTransferFunctionParameters = let ptr = backend.DstTransferFunctionParameters in NativePtr.read ptr
            GamutConversionMatrix = let ptr = backend.GamutConversionMatrix in NativePtr.read ptr
            Mirrored = (backend.Mirrored <> 0)
            Rotation = backend.Rotation
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.ExternalTextureDescriptor>) = 
        use ptr = fixed &r
        ExternalTextureDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        ExternalTextureDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.ExternalTextureDescriptor>
type SharedBufferMemory internal(device : Device, handle : nativeint) =
    static let nullptr = new SharedBufferMemory(Unchecked.defaultof<_>, Unchecked.defaultof<_>)
    member x.Handle = handle
    member x.Device = device
    override x.ToString() = $"SharedBufferMemory(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? SharedBufferMemory as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.SharedBufferMemorySetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Properties : SharedBufferMemoryProperties =
        let relativePointers = false
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        try
            let status = WebGPU.Raw.WebGPU.SharedBufferMemoryGetProperties(handle, ptr)
            if status <> Status.Success then failwith "GetProperties failed"
            use pppp = fixed &res in SharedBufferMemoryProperties.Read(device, pppp, relativePointers)
        finally
            ()
    member this.CreateBuffer(descriptor : BufferDescriptor) : Buffer =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.SharedBufferMemoryCreateBuffer(handle, _descriptorPtr)
            new Buffer(device, res)
        )
    member this.BeginAccess(buffer : Buffer, descriptor : SharedBufferMemoryBeginAccessDescriptor) : Status =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.SharedBufferMemoryBeginAccess(handle, buffer.Handle, _descriptorPtr)
            res
        )
    member this.EndAccess(buffer : Buffer, descriptor : byref<SharedBufferMemoryEndAccessState>) : Status =
        let relativePointers = false
        let mutable descriptorCopy = descriptor
        try
            descriptor.Pin(device, fun _descriptorPtr ->
                if NativePtr.toNativeInt _descriptorPtr = 0n then
                    let mutable descriptorNative = Unchecked.defaultof<WebGPU.Raw.SharedBufferMemoryEndAccessState>
                    use _descriptorPtr = fixed &descriptorNative
                    try
                        let res = WebGPU.Raw.WebGPU.SharedBufferMemoryEndAccess(handle, buffer.Handle, _descriptorPtr)
                        let _ret = res
                        descriptorCopy <- SharedBufferMemoryEndAccessState.Read(device, _descriptorPtr, relativePointers)
                        _ret
                    finally
                        ()
                else
                    let res = WebGPU.Raw.WebGPU.SharedBufferMemoryEndAccess(handle, buffer.Handle, _descriptorPtr)
                    let _ret = res
                    descriptorCopy <- SharedBufferMemoryEndAccessState.Read(device, _descriptorPtr, relativePointers)
                    _ret
                )
        finally
            descriptor <- descriptorCopy
    member this.IsDeviceLost() : bool =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.SharedBufferMemoryIsDeviceLost(handle)
        (res <> 0)
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.SharedBufferMemoryRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.SharedBufferMemoryAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type SharedBufferMemoryProperties = 
    {
        Usage : BufferUsage
        Size : int64
    }
    static member Null = Unchecked.defaultof<SharedBufferMemoryProperties>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedBufferMemoryProperties> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable value =
                new WebGPU.Raw.SharedBufferMemoryProperties(
                    nextInChain,
                    this.Usage,
                    uint64(this.Size)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedBufferMemoryProperties> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedBufferMemoryProperties>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Usage = backend.Usage
            Size = int64(backend.Size)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedBufferMemoryProperties>) = 
        use ptr = fixed &r
        SharedBufferMemoryProperties.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedBufferMemoryProperties.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedBufferMemoryProperties>
type SharedBufferMemoryDescriptor = 
    {
        Label : string
    }
    static member Null = Unchecked.defaultof<SharedBufferMemoryDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedBufferMemoryDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
            use _labelPtr = fixed _labelArr
            try
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                let mutable value =
                    new WebGPU.Raw.SharedBufferMemoryDescriptor(
                        nextInChain,
                        _labelLen
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedBufferMemoryDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedBufferMemoryDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
        {
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedBufferMemoryDescriptor>) = 
        use ptr = fixed &r
        SharedBufferMemoryDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedBufferMemoryDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedBufferMemoryDescriptor>
type SharedTextureMemory internal(handle : nativeint) =
    static let device = Unchecked.defaultof<Device>
    static let nullptr = new SharedTextureMemory(Unchecked.defaultof<_>)
    member x.Handle = handle
    override x.ToString() = $"SharedTextureMemory(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? SharedTextureMemory as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.SharedTextureMemorySetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Properties : SharedTextureMemoryProperties =
        let relativePointers = false
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        try
            let status = WebGPU.Raw.WebGPU.SharedTextureMemoryGetProperties(handle, ptr)
            if status <> Status.Success then failwith "GetProperties failed"
            use pppp = fixed &res in SharedTextureMemoryProperties.Read(device, pppp, relativePointers)
        finally
            ()
    member this.CreateTexture(descriptor : TextureDescriptor) : Texture =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.SharedTextureMemoryCreateTexture(handle, _descriptorPtr)
            new Texture(device, res)
        )
    member this.BeginAccess(texture : Texture, descriptor : SharedTextureMemoryBeginAccessDescriptor) : Status =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.SharedTextureMemoryBeginAccess(handle, texture.Handle, _descriptorPtr)
            res
        )
    member this.EndAccess(texture : Texture, descriptor : byref<SharedTextureMemoryEndAccessState>) : Status =
        let relativePointers = false
        let mutable descriptorCopy = descriptor
        try
            descriptor.Pin(device, fun _descriptorPtr ->
                if NativePtr.toNativeInt _descriptorPtr = 0n then
                    let mutable descriptorNative = Unchecked.defaultof<WebGPU.Raw.SharedTextureMemoryEndAccessState>
                    use _descriptorPtr = fixed &descriptorNative
                    try
                        let res = WebGPU.Raw.WebGPU.SharedTextureMemoryEndAccess(handle, texture.Handle, _descriptorPtr)
                        let _ret = res
                        descriptorCopy <- SharedTextureMemoryEndAccessState.Read(device, _descriptorPtr, relativePointers)
                        _ret
                    finally
                        ()
                else
                    let res = WebGPU.Raw.WebGPU.SharedTextureMemoryEndAccess(handle, texture.Handle, _descriptorPtr)
                    let _ret = res
                    descriptorCopy <- SharedTextureMemoryEndAccessState.Read(device, _descriptorPtr, relativePointers)
                    _ret
                )
        finally
            descriptor <- descriptorCopy
    member this.IsDeviceLost() : bool =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.SharedTextureMemoryIsDeviceLost(handle)
        (res <> 0)
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.SharedTextureMemoryRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.SharedTextureMemoryAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type SharedTextureMemoryProperties = 
    {
        Next : ISharedTextureMemoryPropertiesExtension
        Usage : TextureUsage
        Size : Extent3D
        Format : TextureFormat
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryProperties>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedTextureMemoryProperties> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                this.Size.Pin(device, fun _sizePtr ->
                    let mutable value =
                        new WebGPU.Raw.SharedTextureMemoryProperties(
                            nextInChain,
                            this.Usage,
                            (if NativePtr.toNativeInt _sizePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _sizePtr),
                            this.Format
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                )
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedTextureMemoryProperties> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedTextureMemoryProperties>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryPropertiesExtension> device relativePointers backend.NextInChain
            Usage = backend.Usage
            Size = use pppp = fixed &backend.Size in Extent3D.Read(device, pppp, relativePointers)
            Format = backend.Format
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedTextureMemoryProperties>) = 
        use ptr = fixed &r
        SharedTextureMemoryProperties.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedTextureMemoryProperties.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedTextureMemoryProperties>
type SharedTextureMemoryAHardwareBufferProperties = 
    {
        Next : ISharedTextureMemoryPropertiesExtension
        YCbCrInfo : YCbCrVkDescriptor
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryAHardwareBufferProperties>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedTextureMemoryAHardwareBufferProperties> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedTextureMemoryAHardwareBufferProperties
                this.YCbCrInfo.Pin(device, fun _yCbCrInfoPtr ->
                    let mutable value =
                        new WebGPU.Raw.SharedTextureMemoryAHardwareBufferProperties(
                            nextInChain,
                            sType,
                            (if NativePtr.toNativeInt _yCbCrInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _yCbCrInfoPtr)
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                )
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryPropertiesExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedTextureMemoryAHardwareBufferProperties> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedTextureMemoryAHardwareBufferProperties>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryPropertiesExtension> device relativePointers backend.NextInChain
            YCbCrInfo = use pppp = fixed &backend.YCbCrInfo in YCbCrVkDescriptor.Read(device, pppp, relativePointers)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedTextureMemoryAHardwareBufferProperties>) = 
        use ptr = fixed &r
        SharedTextureMemoryAHardwareBufferProperties.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedTextureMemoryAHardwareBufferProperties.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedTextureMemoryAHardwareBufferProperties>
type SharedTextureMemoryDescriptor = 
    {
        Next : ISharedTextureMemoryDescriptorExtension
        Label : string
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedTextureMemoryDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                try
                    let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                    let mutable value =
                        new WebGPU.Raw.SharedTextureMemoryDescriptor(
                            nextInChain,
                            _labelLen
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedTextureMemoryDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedTextureMemoryDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryDescriptorExtension> device relativePointers backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedTextureMemoryDescriptor>) = 
        use ptr = fixed &r
        SharedTextureMemoryDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedTextureMemoryDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedTextureMemoryDescriptor>
type SharedBufferMemoryBeginAccessDescriptor = 
    {
        Initialized : bool
        Fences : array<SharedFence>
        SignaledValues : array<uint64>
    }
    static member Null = Unchecked.defaultof<SharedBufferMemoryBeginAccessDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedBufferMemoryBeginAccessDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let fencesHandles = this.Fences |> Array.map (fun a -> a.Handle)
            use fencesPtr = fixed (fencesHandles)
            try
                let fencesLen = unativeint this.Fences.Length
                use signaledValuesPtr = fixed (this.SignaledValues)
                try
                    let signaledValuesLen = unativeint this.SignaledValues.Length
                    let mutable value =
                        new WebGPU.Raw.SharedBufferMemoryBeginAccessDescriptor(
                            nextInChain,
                            (if this.Initialized then 1 else 0),
                            signaledValuesLen,
                            fencesPtr,
                            signaledValuesPtr
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                finally
                    ()
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedBufferMemoryBeginAccessDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedBufferMemoryBeginAccessDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Fences <> 0n then
                backend.Fences <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Fences)
            if NativePtr.toNativeInt backend.SignaledValues <> 0n then
                backend.SignaledValues <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.SignaledValues)
        {
            Initialized = (backend.Initialized <> 0)
            Fences = let ptr = backend.Fences in Array.init (int backend.FenceCount) (fun i -> new SharedFence(NativePtr.get ptr i))
            SignaledValues = let ptr = backend.SignaledValues in Array.init (int backend.FenceCount) (fun i -> NativePtr.get ptr i)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedBufferMemoryBeginAccessDescriptor>) = 
        use ptr = fixed &r
        SharedBufferMemoryBeginAccessDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedBufferMemoryBeginAccessDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedBufferMemoryBeginAccessDescriptor>
type SharedBufferMemoryEndAccessState = 
    {
        Initialized : bool
        Fences : array<SharedFence>
        SignaledValues : array<uint64>
    }
    static member Null = Unchecked.defaultof<SharedBufferMemoryEndAccessState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedBufferMemoryEndAccessState> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let fencesHandles = this.Fences |> Array.map (fun a -> a.Handle)
            use fencesPtr = fixed (fencesHandles)
            try
                let fencesLen = unativeint this.Fences.Length
                use signaledValuesPtr = fixed (this.SignaledValues)
                try
                    let signaledValuesLen = unativeint this.SignaledValues.Length
                    let mutable value =
                        new WebGPU.Raw.SharedBufferMemoryEndAccessState(
                            nextInChain,
                            (if this.Initialized then 1 else 0),
                            signaledValuesLen,
                            fencesPtr,
                            signaledValuesPtr
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                finally
                    ()
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedBufferMemoryEndAccessState> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedBufferMemoryEndAccessState>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Fences <> 0n then
                backend.Fences <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Fences)
            if NativePtr.toNativeInt backend.SignaledValues <> 0n then
                backend.SignaledValues <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.SignaledValues)
        {
            Initialized = (backend.Initialized <> 0)
            Fences = let ptr = backend.Fences in Array.init (int backend.FenceCount) (fun i -> new SharedFence(NativePtr.get ptr i))
            SignaledValues = let ptr = backend.SignaledValues in Array.init (int backend.FenceCount) (fun i -> NativePtr.get ptr i)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedBufferMemoryEndAccessState>) = 
        use ptr = fixed &r
        SharedBufferMemoryEndAccessState.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedBufferMemoryEndAccessState.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedBufferMemoryEndAccessState>
type SharedTextureMemoryVkDedicatedAllocationDescriptor = 
    {
        Next : ISharedTextureMemoryDescriptorExtension
        DedicatedAllocation : bool
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryVkDedicatedAllocationDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedTextureMemoryVkDedicatedAllocationDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedTextureMemoryVkDedicatedAllocationDescriptor
                let mutable value =
                    new WebGPU.Raw.SharedTextureMemoryVkDedicatedAllocationDescriptor(
                        nextInChain,
                        sType,
                        (if this.DedicatedAllocation then 1 else 0)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedTextureMemoryVkDedicatedAllocationDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedTextureMemoryVkDedicatedAllocationDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryDescriptorExtension> device relativePointers backend.NextInChain
            DedicatedAllocation = (backend.DedicatedAllocation <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedTextureMemoryVkDedicatedAllocationDescriptor>) = 
        use ptr = fixed &r
        SharedTextureMemoryVkDedicatedAllocationDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedTextureMemoryVkDedicatedAllocationDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedTextureMemoryVkDedicatedAllocationDescriptor>
type SharedTextureMemoryAHardwareBufferDescriptor = 
    {
        Next : ISharedTextureMemoryDescriptorExtension
        Handle : nativeint
        UseExternalFormat : bool
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryAHardwareBufferDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedTextureMemoryAHardwareBufferDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedTextureMemoryAHardwareBufferDescriptor
                let mutable value =
                    new WebGPU.Raw.SharedTextureMemoryAHardwareBufferDescriptor(
                        nextInChain,
                        sType,
                        this.Handle,
                        (if this.UseExternalFormat then 1 else 0)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedTextureMemoryAHardwareBufferDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedTextureMemoryAHardwareBufferDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryDescriptorExtension> device relativePointers backend.NextInChain
            Handle = backend.Handle
            UseExternalFormat = (backend.UseExternalFormat <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedTextureMemoryAHardwareBufferDescriptor>) = 
        use ptr = fixed &r
        SharedTextureMemoryAHardwareBufferDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedTextureMemoryAHardwareBufferDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedTextureMemoryAHardwareBufferDescriptor>
type SharedTextureMemoryDmaBufPlane = 
    {
        Fd : int
        Offset : int64
        Stride : int
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryDmaBufPlane>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedTextureMemoryDmaBufPlane> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.SharedTextureMemoryDmaBufPlane(
                    this.Fd,
                    uint64(this.Offset),
                    uint32(this.Stride)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedTextureMemoryDmaBufPlane> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedTextureMemoryDmaBufPlane>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            Fd = backend.Fd
            Offset = int64(backend.Offset)
            Stride = int(backend.Stride)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedTextureMemoryDmaBufPlane>) = 
        use ptr = fixed &r
        SharedTextureMemoryDmaBufPlane.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedTextureMemoryDmaBufPlane.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedTextureMemoryDmaBufPlane>
type SharedTextureMemoryDmaBufDescriptor = 
    {
        Next : ISharedTextureMemoryDescriptorExtension
        Size : Extent3D
        DrmFormat : int
        DrmModifier : int64
        Planes : array<SharedTextureMemoryDmaBufPlane>
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryDmaBufDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedTextureMemoryDmaBufDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedTextureMemoryDmaBufDescriptor
                this.Size.Pin(device, fun _sizePtr ->
                    WebGPU.Raw.Pinnable.pinArray device this.Planes (fun planesPtr ->
                        let planesLen = unativeint this.Planes.Length
                        let mutable value =
                            new WebGPU.Raw.SharedTextureMemoryDmaBufDescriptor(
                                nextInChain,
                                sType,
                                (if NativePtr.toNativeInt _sizePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _sizePtr),
                                uint32(this.DrmFormat),
                                uint64(this.DrmModifier),
                                planesLen,
                                planesPtr
                            )
                        use ptr = fixed &value
                        try action ptr
                        finally ()
                    )
                )
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedTextureMemoryDmaBufDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedTextureMemoryDmaBufDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Planes <> 0n then
                backend.Planes <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Planes)
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryDescriptorExtension> device relativePointers backend.NextInChain
            Size = use pppp = fixed &backend.Size in Extent3D.Read(device, pppp, relativePointers)
            DrmFormat = int(backend.DrmFormat)
            DrmModifier = int64(backend.DrmModifier)
            Planes = let ptr = backend.Planes in Array.init (int backend.PlaneCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in SharedTextureMemoryDmaBufPlane.Read(device, NativePtr.add ptr i, relativePointers))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedTextureMemoryDmaBufDescriptor>) = 
        use ptr = fixed &r
        SharedTextureMemoryDmaBufDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedTextureMemoryDmaBufDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedTextureMemoryDmaBufDescriptor>
type SharedTextureMemoryOpaqueFDDescriptor = 
    {
        Next : ISharedTextureMemoryDescriptorExtension
        VkImageCreateInfo : nativeint
        MemoryFD : int
        MemoryTypeIndex : int
        AllocationSize : int64
        DedicatedAllocation : bool
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryOpaqueFDDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedTextureMemoryOpaqueFDDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedTextureMemoryOpaqueFDDescriptor
                let mutable value =
                    new WebGPU.Raw.SharedTextureMemoryOpaqueFDDescriptor(
                        nextInChain,
                        sType,
                        this.VkImageCreateInfo,
                        this.MemoryFD,
                        uint32(this.MemoryTypeIndex),
                        uint64(this.AllocationSize),
                        (if this.DedicatedAllocation then 1 else 0)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedTextureMemoryOpaqueFDDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedTextureMemoryOpaqueFDDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryDescriptorExtension> device relativePointers backend.NextInChain
            VkImageCreateInfo = backend.VkImageCreateInfo
            MemoryFD = backend.MemoryFD
            MemoryTypeIndex = int(backend.MemoryTypeIndex)
            AllocationSize = int64(backend.AllocationSize)
            DedicatedAllocation = (backend.DedicatedAllocation <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedTextureMemoryOpaqueFDDescriptor>) = 
        use ptr = fixed &r
        SharedTextureMemoryOpaqueFDDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedTextureMemoryOpaqueFDDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedTextureMemoryOpaqueFDDescriptor>
type SharedTextureMemoryZirconHandleDescriptor = 
    {
        Next : ISharedTextureMemoryDescriptorExtension
        MemoryFD : int
        AllocationSize : int64
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryZirconHandleDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedTextureMemoryZirconHandleDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedTextureMemoryZirconHandleDescriptor
                let mutable value =
                    new WebGPU.Raw.SharedTextureMemoryZirconHandleDescriptor(
                        nextInChain,
                        sType,
                        uint32(this.MemoryFD),
                        uint64(this.AllocationSize)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedTextureMemoryZirconHandleDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedTextureMemoryZirconHandleDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryDescriptorExtension> device relativePointers backend.NextInChain
            MemoryFD = int(backend.MemoryFD)
            AllocationSize = int64(backend.AllocationSize)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedTextureMemoryZirconHandleDescriptor>) = 
        use ptr = fixed &r
        SharedTextureMemoryZirconHandleDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedTextureMemoryZirconHandleDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedTextureMemoryZirconHandleDescriptor>
type SharedTextureMemoryDXGISharedHandleDescriptor = 
    {
        Next : ISharedTextureMemoryDescriptorExtension
        Handle : nativeint
        UseKeyedMutex : bool
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryDXGISharedHandleDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedTextureMemoryDXGISharedHandleDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedTextureMemoryDXGISharedHandleDescriptor
                let mutable value =
                    new WebGPU.Raw.SharedTextureMemoryDXGISharedHandleDescriptor(
                        nextInChain,
                        sType,
                        this.Handle,
                        (if this.UseKeyedMutex then 1 else 0)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedTextureMemoryDXGISharedHandleDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedTextureMemoryDXGISharedHandleDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryDescriptorExtension> device relativePointers backend.NextInChain
            Handle = backend.Handle
            UseKeyedMutex = (backend.UseKeyedMutex <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedTextureMemoryDXGISharedHandleDescriptor>) = 
        use ptr = fixed &r
        SharedTextureMemoryDXGISharedHandleDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedTextureMemoryDXGISharedHandleDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedTextureMemoryDXGISharedHandleDescriptor>
type SharedTextureMemoryIOSurfaceDescriptor = 
    {
        Next : ISharedTextureMemoryDescriptorExtension
        IoSurface : nativeint
        AllowStorageBinding : bool
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryIOSurfaceDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedTextureMemoryIOSurfaceDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedTextureMemoryIOSurfaceDescriptor
                let mutable value =
                    new WebGPU.Raw.SharedTextureMemoryIOSurfaceDescriptor(
                        nextInChain,
                        sType,
                        this.IoSurface,
                        (if this.AllowStorageBinding then 1 else 0)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedTextureMemoryIOSurfaceDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedTextureMemoryIOSurfaceDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryDescriptorExtension> device relativePointers backend.NextInChain
            IoSurface = backend.IoSurface
            AllowStorageBinding = (backend.AllowStorageBinding <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedTextureMemoryIOSurfaceDescriptor>) = 
        use ptr = fixed &r
        SharedTextureMemoryIOSurfaceDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedTextureMemoryIOSurfaceDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedTextureMemoryIOSurfaceDescriptor>
type SharedTextureMemoryEGLImageDescriptor = 
    {
        Next : ISharedTextureMemoryDescriptorExtension
        Image : nativeint
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryEGLImageDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedTextureMemoryEGLImageDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedTextureMemoryEGLImageDescriptor
                let mutable value =
                    new WebGPU.Raw.SharedTextureMemoryEGLImageDescriptor(
                        nextInChain,
                        sType,
                        this.Image
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedTextureMemoryEGLImageDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedTextureMemoryEGLImageDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryDescriptorExtension> device relativePointers backend.NextInChain
            Image = backend.Image
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedTextureMemoryEGLImageDescriptor>) = 
        use ptr = fixed &r
        SharedTextureMemoryEGLImageDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedTextureMemoryEGLImageDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedTextureMemoryEGLImageDescriptor>
type SharedTextureMemoryBeginAccessDescriptor = 
    {
        Next : ISharedTextureMemoryBeginAccessDescriptorExtension
        ConcurrentRead : bool
        Initialized : bool
        Fences : array<SharedFence>
        SignaledValues : array<uint64>
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryBeginAccessDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedTextureMemoryBeginAccessDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let fencesHandles = this.Fences |> Array.map (fun a -> a.Handle)
                use fencesPtr = fixed (fencesHandles)
                try
                    let fencesLen = unativeint this.Fences.Length
                    use signaledValuesPtr = fixed (this.SignaledValues)
                    try
                        let signaledValuesLen = unativeint this.SignaledValues.Length
                        let mutable value =
                            new WebGPU.Raw.SharedTextureMemoryBeginAccessDescriptor(
                                nextInChain,
                                (if this.ConcurrentRead then 1 else 0),
                                (if this.Initialized then 1 else 0),
                                signaledValuesLen,
                                fencesPtr,
                                signaledValuesPtr
                            )
                        use ptr = fixed &value
                        try action ptr
                        finally ()
                    finally
                        ()
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedTextureMemoryBeginAccessDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedTextureMemoryBeginAccessDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Fences <> 0n then
                backend.Fences <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Fences)
            if NativePtr.toNativeInt backend.SignaledValues <> 0n then
                backend.SignaledValues <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.SignaledValues)
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryBeginAccessDescriptorExtension> device relativePointers backend.NextInChain
            ConcurrentRead = (backend.ConcurrentRead <> 0)
            Initialized = (backend.Initialized <> 0)
            Fences = let ptr = backend.Fences in Array.init (int backend.FenceCount) (fun i -> new SharedFence(NativePtr.get ptr i))
            SignaledValues = let ptr = backend.SignaledValues in Array.init (int backend.FenceCount) (fun i -> NativePtr.get ptr i)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedTextureMemoryBeginAccessDescriptor>) = 
        use ptr = fixed &r
        SharedTextureMemoryBeginAccessDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedTextureMemoryBeginAccessDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedTextureMemoryBeginAccessDescriptor>
type SharedTextureMemoryEndAccessState = 
    {
        Next : ISharedTextureMemoryEndAccessStateExtension
        Initialized : bool
        Fences : array<SharedFence>
        SignaledValues : array<uint64>
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryEndAccessState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedTextureMemoryEndAccessState> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let fencesHandles = this.Fences |> Array.map (fun a -> a.Handle)
                use fencesPtr = fixed (fencesHandles)
                try
                    let fencesLen = unativeint this.Fences.Length
                    use signaledValuesPtr = fixed (this.SignaledValues)
                    try
                        let signaledValuesLen = unativeint this.SignaledValues.Length
                        let mutable value =
                            new WebGPU.Raw.SharedTextureMemoryEndAccessState(
                                nextInChain,
                                (if this.Initialized then 1 else 0),
                                signaledValuesLen,
                                fencesPtr,
                                signaledValuesPtr
                            )
                        use ptr = fixed &value
                        try action ptr
                        finally ()
                    finally
                        ()
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedTextureMemoryEndAccessState> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedTextureMemoryEndAccessState>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Fences <> 0n then
                backend.Fences <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Fences)
            if NativePtr.toNativeInt backend.SignaledValues <> 0n then
                backend.SignaledValues <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.SignaledValues)
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryEndAccessStateExtension> device relativePointers backend.NextInChain
            Initialized = (backend.Initialized <> 0)
            Fences = let ptr = backend.Fences in Array.init (int backend.FenceCount) (fun i -> new SharedFence(NativePtr.get ptr i))
            SignaledValues = let ptr = backend.SignaledValues in Array.init (int backend.FenceCount) (fun i -> NativePtr.get ptr i)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedTextureMemoryEndAccessState>) = 
        use ptr = fixed &r
        SharedTextureMemoryEndAccessState.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedTextureMemoryEndAccessState.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedTextureMemoryEndAccessState>
type SharedTextureMemoryVkImageLayoutBeginState = 
    {
        Next : ISharedTextureMemoryBeginAccessDescriptorExtension
        OldLayout : int
        NewLayout : int
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryVkImageLayoutBeginState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedTextureMemoryVkImageLayoutBeginState> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedTextureMemoryVkImageLayoutBeginState
                let mutable value =
                    new WebGPU.Raw.SharedTextureMemoryVkImageLayoutBeginState(
                        nextInChain,
                        sType,
                        this.OldLayout,
                        this.NewLayout
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryBeginAccessDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedTextureMemoryVkImageLayoutBeginState> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedTextureMemoryVkImageLayoutBeginState>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryBeginAccessDescriptorExtension> device relativePointers backend.NextInChain
            OldLayout = backend.OldLayout
            NewLayout = backend.NewLayout
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedTextureMemoryVkImageLayoutBeginState>) = 
        use ptr = fixed &r
        SharedTextureMemoryVkImageLayoutBeginState.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedTextureMemoryVkImageLayoutBeginState.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedTextureMemoryVkImageLayoutBeginState>
type SharedTextureMemoryVkImageLayoutEndState = 
    {
        Next : ISharedTextureMemoryEndAccessStateExtension
        OldLayout : int
        NewLayout : int
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryVkImageLayoutEndState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedTextureMemoryVkImageLayoutEndState> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedTextureMemoryVkImageLayoutEndState
                let mutable value =
                    new WebGPU.Raw.SharedTextureMemoryVkImageLayoutEndState(
                        nextInChain,
                        sType,
                        this.OldLayout,
                        this.NewLayout
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryEndAccessStateExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedTextureMemoryVkImageLayoutEndState> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedTextureMemoryVkImageLayoutEndState>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryEndAccessStateExtension> device relativePointers backend.NextInChain
            OldLayout = backend.OldLayout
            NewLayout = backend.NewLayout
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedTextureMemoryVkImageLayoutEndState>) = 
        use ptr = fixed &r
        SharedTextureMemoryVkImageLayoutEndState.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedTextureMemoryVkImageLayoutEndState.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedTextureMemoryVkImageLayoutEndState>
type SharedTextureMemoryD3DSwapchainBeginState = 
    {
        Next : ISharedTextureMemoryBeginAccessDescriptorExtension
        IsSwapchain : bool
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryD3DSwapchainBeginState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedTextureMemoryD3DSwapchainBeginState> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedTextureMemoryD3DSwapchainBeginState
                let mutable value =
                    new WebGPU.Raw.SharedTextureMemoryD3DSwapchainBeginState(
                        nextInChain,
                        sType,
                        (if this.IsSwapchain then 1 else 0)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryBeginAccessDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedTextureMemoryD3DSwapchainBeginState> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedTextureMemoryD3DSwapchainBeginState>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryBeginAccessDescriptorExtension> device relativePointers backend.NextInChain
            IsSwapchain = (backend.IsSwapchain <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedTextureMemoryD3DSwapchainBeginState>) = 
        use ptr = fixed &r
        SharedTextureMemoryD3DSwapchainBeginState.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedTextureMemoryD3DSwapchainBeginState.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedTextureMemoryD3DSwapchainBeginState>
type SharedTextureMemoryD3D11BeginState = 
    {
        Next : ISharedTextureMemoryBeginAccessDescriptorExtension
        RequiresEndAccessFence : bool
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryD3D11BeginState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedTextureMemoryD3D11BeginState> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedTextureMemoryD3D11BeginState
                let mutable value =
                    new WebGPU.Raw.SharedTextureMemoryD3D11BeginState(
                        nextInChain,
                        sType,
                        (if this.RequiresEndAccessFence then 1 else 0)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryBeginAccessDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedTextureMemoryD3D11BeginState> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedTextureMemoryD3D11BeginState>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryBeginAccessDescriptorExtension> device relativePointers backend.NextInChain
            RequiresEndAccessFence = (backend.RequiresEndAccessFence <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedTextureMemoryD3D11BeginState>) = 
        use ptr = fixed &r
        SharedTextureMemoryD3D11BeginState.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedTextureMemoryD3D11BeginState.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedTextureMemoryD3D11BeginState>
type SharedFence internal(handle : nativeint) =
    static let device = Unchecked.defaultof<Device>
    static let nullptr = new SharedFence(Unchecked.defaultof<_>)
    member x.Handle = handle
    override x.ToString() = $"SharedFence(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? SharedFence as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.ExportInfo(info : byref<SharedFenceExportInfo>) : unit =
        let relativePointers = false
        let mutable infoCopy = info
        try
            info.Pin(device, fun _infoPtr ->
                if NativePtr.toNativeInt _infoPtr = 0n then
                    let mutable infoNative = Unchecked.defaultof<WebGPU.Raw.SharedFenceExportInfo>
                    use _infoPtr = fixed &infoNative
                    try
                        let res = WebGPU.Raw.WebGPU.SharedFenceExportInfo(handle, _infoPtr)
                        let _ret = res
                        infoCopy <- SharedFenceExportInfo.Read(device, _infoPtr, relativePointers)
                        _ret
                    finally
                        ()
                else
                    let res = WebGPU.Raw.WebGPU.SharedFenceExportInfo(handle, _infoPtr)
                    let _ret = res
                    infoCopy <- SharedFenceExportInfo.Read(device, _infoPtr, relativePointers)
                    _ret
                )
        finally
            info <- infoCopy
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.SharedFenceRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.SharedFenceAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type SharedFenceDescriptor = 
    {
        Next : ISharedFenceDescriptorExtension
        Label : string
    }
    static member Null = Unchecked.defaultof<SharedFenceDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedFenceDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                try
                    let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                    let mutable value =
                        new WebGPU.Raw.SharedFenceDescriptor(
                            nextInChain,
                            _labelLen
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedFenceDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedFenceDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
        {
            Next = ExtensionDecoder.decode<ISharedFenceDescriptorExtension> device relativePointers backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedFenceDescriptor>) = 
        use ptr = fixed &r
        SharedFenceDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedFenceDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedFenceDescriptor>
type SharedFenceVkSemaphoreOpaqueFDDescriptor = 
    {
        Next : ISharedFenceDescriptorExtension
        Handle : int
    }
    static member Null = Unchecked.defaultof<SharedFenceVkSemaphoreOpaqueFDDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedFenceVkSemaphoreOpaqueFDDescriptor
                let mutable value =
                    new WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDDescriptor(
                        nextInChain,
                        sType,
                        this.Handle
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedFenceDescriptorExtension> device relativePointers backend.NextInChain
            Handle = backend.Handle
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDDescriptor>) = 
        use ptr = fixed &r
        SharedFenceVkSemaphoreOpaqueFDDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedFenceVkSemaphoreOpaqueFDDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDDescriptor>
type SharedFenceSyncFDDescriptor = 
    {
        Next : ISharedFenceDescriptorExtension
        Handle : int
    }
    static member Null = Unchecked.defaultof<SharedFenceSyncFDDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedFenceSyncFDDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedFenceSyncFDDescriptor
                let mutable value =
                    new WebGPU.Raw.SharedFenceSyncFDDescriptor(
                        nextInChain,
                        sType,
                        this.Handle
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedFenceSyncFDDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedFenceSyncFDDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedFenceDescriptorExtension> device relativePointers backend.NextInChain
            Handle = backend.Handle
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedFenceSyncFDDescriptor>) = 
        use ptr = fixed &r
        SharedFenceSyncFDDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedFenceSyncFDDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedFenceSyncFDDescriptor>
type SharedFenceVkSemaphoreZirconHandleDescriptor = 
    {
        Next : ISharedFenceDescriptorExtension
        Handle : int
    }
    static member Null = Unchecked.defaultof<SharedFenceVkSemaphoreZirconHandleDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedFenceVkSemaphoreZirconHandleDescriptor
                let mutable value =
                    new WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleDescriptor(
                        nextInChain,
                        sType,
                        uint32(this.Handle)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedFenceDescriptorExtension> device relativePointers backend.NextInChain
            Handle = int(backend.Handle)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleDescriptor>) = 
        use ptr = fixed &r
        SharedFenceVkSemaphoreZirconHandleDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedFenceVkSemaphoreZirconHandleDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleDescriptor>
type SharedFenceDXGISharedHandleDescriptor = 
    {
        Next : ISharedFenceDescriptorExtension
        Handle : nativeint
    }
    static member Null = Unchecked.defaultof<SharedFenceDXGISharedHandleDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedFenceDXGISharedHandleDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedFenceDXGISharedHandleDescriptor
                let mutable value =
                    new WebGPU.Raw.SharedFenceDXGISharedHandleDescriptor(
                        nextInChain,
                        sType,
                        this.Handle
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedFenceDXGISharedHandleDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedFenceDXGISharedHandleDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedFenceDescriptorExtension> device relativePointers backend.NextInChain
            Handle = backend.Handle
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedFenceDXGISharedHandleDescriptor>) = 
        use ptr = fixed &r
        SharedFenceDXGISharedHandleDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedFenceDXGISharedHandleDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedFenceDXGISharedHandleDescriptor>
type SharedFenceMTLSharedEventDescriptor = 
    {
        Next : ISharedFenceDescriptorExtension
        SharedEvent : nativeint
    }
    static member Null = Unchecked.defaultof<SharedFenceMTLSharedEventDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedFenceMTLSharedEventDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedFenceMTLSharedEventDescriptor
                let mutable value =
                    new WebGPU.Raw.SharedFenceMTLSharedEventDescriptor(
                        nextInChain,
                        sType,
                        this.SharedEvent
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedFenceMTLSharedEventDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedFenceMTLSharedEventDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedFenceDescriptorExtension> device relativePointers backend.NextInChain
            SharedEvent = backend.SharedEvent
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedFenceMTLSharedEventDescriptor>) = 
        use ptr = fixed &r
        SharedFenceMTLSharedEventDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedFenceMTLSharedEventDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedFenceMTLSharedEventDescriptor>
type SharedFenceEGLSyncDescriptor = 
    {
        Next : ISharedFenceDescriptorExtension
        Sync : nativeint
    }
    static member Null = Unchecked.defaultof<SharedFenceEGLSyncDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedFenceEGLSyncDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedFenceEGLSyncDescriptor
                let mutable value =
                    new WebGPU.Raw.SharedFenceEGLSyncDescriptor(
                        nextInChain,
                        sType,
                        this.Sync
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedFenceEGLSyncDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedFenceEGLSyncDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedFenceDescriptorExtension> device relativePointers backend.NextInChain
            Sync = backend.Sync
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedFenceEGLSyncDescriptor>) = 
        use ptr = fixed &r
        SharedFenceEGLSyncDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedFenceEGLSyncDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedFenceEGLSyncDescriptor>
type DawnFakeBufferOOMForTesting = 
    {
        Next : IBufferDescriptorExtension
        FakeOOMAtWireClientMap : bool
        FakeOOMAtNativeMap : bool
        FakeOOMAtDevice : bool
    }
    static member Null = Unchecked.defaultof<DawnFakeBufferOOMForTesting>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnFakeBufferOOMForTesting> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnFakeBufferOOMForTesting
                let mutable value =
                    new WebGPU.Raw.DawnFakeBufferOOMForTesting(
                        nextInChain,
                        sType,
                        (if this.FakeOOMAtWireClientMap then 1 else 0),
                        (if this.FakeOOMAtNativeMap then 1 else 0),
                        (if this.FakeOOMAtDevice then 1 else 0)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IBufferDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnFakeBufferOOMForTesting> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnFakeBufferOOMForTesting>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IBufferDescriptorExtension> device relativePointers backend.NextInChain
            FakeOOMAtWireClientMap = (backend.FakeOOMAtWireClientMap <> 0)
            FakeOOMAtNativeMap = (backend.FakeOOMAtNativeMap <> 0)
            FakeOOMAtDevice = (backend.FakeOOMAtDevice <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnFakeBufferOOMForTesting>) = 
        use ptr = fixed &r
        DawnFakeBufferOOMForTesting.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnFakeBufferOOMForTesting.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnFakeBufferOOMForTesting>
type DawnFakeDeviceInitializeErrorForTesting = 
    {
        Next : IDeviceDescriptorExtension
    }
    static member Null = Unchecked.defaultof<DawnFakeDeviceInitializeErrorForTesting>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnFakeDeviceInitializeErrorForTesting> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnFakeDeviceInitializeErrorForTesting
                let mutable value =
                    new WebGPU.Raw.DawnFakeDeviceInitializeErrorForTesting(
                        nextInChain,
                        sType
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IDeviceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnFakeDeviceInitializeErrorForTesting> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnFakeDeviceInitializeErrorForTesting>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IDeviceDescriptorExtension> device relativePointers backend.NextInChain
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnFakeDeviceInitializeErrorForTesting>) = 
        use ptr = fixed &r
        DawnFakeDeviceInitializeErrorForTesting.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnFakeDeviceInitializeErrorForTesting.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnFakeDeviceInitializeErrorForTesting>
type SharedFenceExportInfo = 
    {
        Next : ISharedFenceExportInfoExtension
        Type : SharedFenceType
    }
    static member Null = Unchecked.defaultof<SharedFenceExportInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedFenceExportInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable value =
                    new WebGPU.Raw.SharedFenceExportInfo(
                        nextInChain,
                        this.Type
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedFenceExportInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedFenceExportInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedFenceExportInfoExtension> device relativePointers backend.NextInChain
            Type = backend.Type
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedFenceExportInfo>) = 
        use ptr = fixed &r
        SharedFenceExportInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedFenceExportInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedFenceExportInfo>
type SharedFenceVkSemaphoreOpaqueFDExportInfo = 
    {
        Next : ISharedFenceExportInfoExtension
        Handle : int
    }
    static member Null = Unchecked.defaultof<SharedFenceVkSemaphoreOpaqueFDExportInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDExportInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedFenceVkSemaphoreOpaqueFDExportInfo
                let mutable value =
                    new WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDExportInfo(
                        nextInChain,
                        sType,
                        this.Handle
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceExportInfoExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDExportInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDExportInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedFenceExportInfoExtension> device relativePointers backend.NextInChain
            Handle = backend.Handle
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDExportInfo>) = 
        use ptr = fixed &r
        SharedFenceVkSemaphoreOpaqueFDExportInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedFenceVkSemaphoreOpaqueFDExportInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDExportInfo>
type SharedFenceSyncFDExportInfo = 
    {
        Next : ISharedFenceExportInfoExtension
        Handle : int
    }
    static member Null = Unchecked.defaultof<SharedFenceSyncFDExportInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedFenceSyncFDExportInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedFenceSyncFDExportInfo
                let mutable value =
                    new WebGPU.Raw.SharedFenceSyncFDExportInfo(
                        nextInChain,
                        sType,
                        this.Handle
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceExportInfoExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedFenceSyncFDExportInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedFenceSyncFDExportInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedFenceExportInfoExtension> device relativePointers backend.NextInChain
            Handle = backend.Handle
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedFenceSyncFDExportInfo>) = 
        use ptr = fixed &r
        SharedFenceSyncFDExportInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedFenceSyncFDExportInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedFenceSyncFDExportInfo>
type SharedFenceVkSemaphoreZirconHandleExportInfo = 
    {
        Next : ISharedFenceExportInfoExtension
        Handle : int
    }
    static member Null = Unchecked.defaultof<SharedFenceVkSemaphoreZirconHandleExportInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleExportInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedFenceVkSemaphoreZirconHandleExportInfo
                let mutable value =
                    new WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleExportInfo(
                        nextInChain,
                        sType,
                        uint32(this.Handle)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceExportInfoExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleExportInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleExportInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedFenceExportInfoExtension> device relativePointers backend.NextInChain
            Handle = int(backend.Handle)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleExportInfo>) = 
        use ptr = fixed &r
        SharedFenceVkSemaphoreZirconHandleExportInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedFenceVkSemaphoreZirconHandleExportInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleExportInfo>
type SharedFenceDXGISharedHandleExportInfo = 
    {
        Next : ISharedFenceExportInfoExtension
        Handle : nativeint
    }
    static member Null = Unchecked.defaultof<SharedFenceDXGISharedHandleExportInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedFenceDXGISharedHandleExportInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedFenceDXGISharedHandleExportInfo
                let mutable value =
                    new WebGPU.Raw.SharedFenceDXGISharedHandleExportInfo(
                        nextInChain,
                        sType,
                        this.Handle
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceExportInfoExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedFenceDXGISharedHandleExportInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedFenceDXGISharedHandleExportInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedFenceExportInfoExtension> device relativePointers backend.NextInChain
            Handle = backend.Handle
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedFenceDXGISharedHandleExportInfo>) = 
        use ptr = fixed &r
        SharedFenceDXGISharedHandleExportInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedFenceDXGISharedHandleExportInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedFenceDXGISharedHandleExportInfo>
type SharedFenceMTLSharedEventExportInfo = 
    {
        Next : ISharedFenceExportInfoExtension
        SharedEvent : nativeint
    }
    static member Null = Unchecked.defaultof<SharedFenceMTLSharedEventExportInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedFenceMTLSharedEventExportInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedFenceMTLSharedEventExportInfo
                let mutable value =
                    new WebGPU.Raw.SharedFenceMTLSharedEventExportInfo(
                        nextInChain,
                        sType,
                        this.SharedEvent
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceExportInfoExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedFenceMTLSharedEventExportInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedFenceMTLSharedEventExportInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedFenceExportInfoExtension> device relativePointers backend.NextInChain
            SharedEvent = backend.SharedEvent
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedFenceMTLSharedEventExportInfo>) = 
        use ptr = fixed &r
        SharedFenceMTLSharedEventExportInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedFenceMTLSharedEventExportInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedFenceMTLSharedEventExportInfo>
type SharedFenceEGLSyncExportInfo = 
    {
        Next : ISharedFenceExportInfoExtension
        Sync : nativeint
    }
    static member Null = Unchecked.defaultof<SharedFenceEGLSyncExportInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SharedFenceEGLSyncExportInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedFenceEGLSyncExportInfo
                let mutable value =
                    new WebGPU.Raw.SharedFenceEGLSyncExportInfo(
                        nextInChain,
                        sType,
                        this.Sync
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceExportInfoExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SharedFenceEGLSyncExportInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SharedFenceEGLSyncExportInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISharedFenceExportInfoExtension> device relativePointers backend.NextInChain
            Sync = backend.Sync
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SharedFenceEGLSyncExportInfo>) = 
        use ptr = fixed &r
        SharedFenceEGLSyncExportInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SharedFenceEGLSyncExportInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SharedFenceEGLSyncExportInfo>
type DawnFormatCapabilities = 
    {
        Next : IDawnFormatCapabilitiesExtension
    }
    static member Null = Unchecked.defaultof<DawnFormatCapabilities>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnFormatCapabilities> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable value =
                    new WebGPU.Raw.DawnFormatCapabilities(
                        nextInChain
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnFormatCapabilities> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnFormatCapabilities>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IDawnFormatCapabilitiesExtension> device relativePointers backend.NextInChain
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnFormatCapabilities>) = 
        use ptr = fixed &r
        DawnFormatCapabilities.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnFormatCapabilities.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnFormatCapabilities>
type DawnDrmFormatCapabilities = 
    {
        Next : IDawnFormatCapabilitiesExtension
        Properties : array<DawnDrmFormatProperties>
    }
    static member Null = Unchecked.defaultof<DawnDrmFormatCapabilities>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnDrmFormatCapabilities> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnDrmFormatCapabilities
                WebGPU.Raw.Pinnable.pinArray device this.Properties (fun propertiesPtr ->
                    let propertiesLen = unativeint this.Properties.Length
                    let mutable value =
                        new WebGPU.Raw.DawnDrmFormatCapabilities(
                            nextInChain,
                            sType,
                            propertiesLen,
                            propertiesPtr
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                )
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IDawnFormatCapabilitiesExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnDrmFormatCapabilities> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnDrmFormatCapabilities>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Properties <> 0n then
                backend.Properties <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Properties)
        {
            Next = ExtensionDecoder.decode<IDawnFormatCapabilitiesExtension> device relativePointers backend.NextInChain
            Properties = let ptr = backend.Properties in Array.init (int backend.PropertiesCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in DawnDrmFormatProperties.Read(device, NativePtr.add ptr i, relativePointers))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnDrmFormatCapabilities>) = 
        use ptr = fixed &r
        DawnDrmFormatCapabilities.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnDrmFormatCapabilities.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnDrmFormatCapabilities>
type DawnDrmFormatProperties = 
    {
        Modifier : int64
        ModifierPlaneCount : int
    }
    static member Null = Unchecked.defaultof<DawnDrmFormatProperties>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnDrmFormatProperties> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.DawnDrmFormatProperties(
                    uint64(this.Modifier),
                    uint32(this.ModifierPlaneCount)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnDrmFormatProperties> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnDrmFormatProperties>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            Modifier = int64(backend.Modifier)
            ModifierPlaneCount = int(backend.ModifierPlaneCount)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnDrmFormatProperties>) = 
        use ptr = fixed &r
        DawnDrmFormatProperties.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnDrmFormatProperties.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnDrmFormatProperties>
type TexelCopyBufferInfo = 
    {
        Layout : TexelCopyBufferLayout
        Buffer : Buffer
    }
    static member Null = Unchecked.defaultof<TexelCopyBufferInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.TexelCopyBufferInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            this.Layout.Pin(device, fun _layoutPtr ->
                let mutable value =
                    new WebGPU.Raw.TexelCopyBufferInfo(
                        (if NativePtr.toNativeInt _layoutPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _layoutPtr),
                        this.Buffer.Handle
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.TexelCopyBufferInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.TexelCopyBufferInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            Layout = use pppp = fixed &backend.Layout in TexelCopyBufferLayout.Read(device, pppp, relativePointers)
            Buffer = new Buffer(device, backend.Buffer)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.TexelCopyBufferInfo>) = 
        use ptr = fixed &r
        TexelCopyBufferInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        TexelCopyBufferInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.TexelCopyBufferInfo>
type TexelCopyBufferLayout = 
    {
        Offset : int64
        BytesPerRow : int
        RowsPerImage : int
    }
    static member Null = Unchecked.defaultof<TexelCopyBufferLayout>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.TexelCopyBufferLayout> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.TexelCopyBufferLayout(
                    uint64(this.Offset),
                    uint32(this.BytesPerRow),
                    uint32(this.RowsPerImage)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.TexelCopyBufferLayout> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.TexelCopyBufferLayout>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            Offset = int64(backend.Offset)
            BytesPerRow = int(backend.BytesPerRow)
            RowsPerImage = int(backend.RowsPerImage)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.TexelCopyBufferLayout>) = 
        use ptr = fixed &r
        TexelCopyBufferLayout.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        TexelCopyBufferLayout.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.TexelCopyBufferLayout>
type TexelCopyTextureInfo = 
    {
        Texture : Texture
        MipLevel : int
        Origin : Origin3D
        Aspect : TextureAspect
    }
    static member Null = Unchecked.defaultof<TexelCopyTextureInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.TexelCopyTextureInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            this.Origin.Pin(device, fun _originPtr ->
                let mutable value =
                    new WebGPU.Raw.TexelCopyTextureInfo(
                        this.Texture.Handle,
                        uint32(this.MipLevel),
                        (if NativePtr.toNativeInt _originPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _originPtr),
                        this.Aspect
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.TexelCopyTextureInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.TexelCopyTextureInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            Texture = new Texture(device, backend.Texture)
            MipLevel = int(backend.MipLevel)
            Origin = use pppp = fixed &backend.Origin in Origin3D.Read(device, pppp, relativePointers)
            Aspect = backend.Aspect
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.TexelCopyTextureInfo>) = 
        use ptr = fixed &r
        TexelCopyTextureInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        TexelCopyTextureInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.TexelCopyTextureInfo>
type ImageCopyExternalTexture = 
    {
        ExternalTexture : ExternalTexture
        Origin : Origin3D
        NaturalSize : Extent2D
    }
    static member Null = Unchecked.defaultof<ImageCopyExternalTexture>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.ImageCopyExternalTexture> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            this.Origin.Pin(device, fun _originPtr ->
                this.NaturalSize.Pin(device, fun _naturalSizePtr ->
                    let mutable value =
                        new WebGPU.Raw.ImageCopyExternalTexture(
                            nextInChain,
                            this.ExternalTexture.Handle,
                            (if NativePtr.toNativeInt _originPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _originPtr),
                            (if NativePtr.toNativeInt _naturalSizePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _naturalSizePtr)
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                )
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.ImageCopyExternalTexture> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.ImageCopyExternalTexture>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            ExternalTexture = new ExternalTexture(device, backend.ExternalTexture)
            Origin = use pppp = fixed &backend.Origin in Origin3D.Read(device, pppp, relativePointers)
            NaturalSize = use pppp = fixed &backend.NaturalSize in Extent2D.Read(device, pppp, relativePointers)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.ImageCopyExternalTexture>) = 
        use ptr = fixed &r
        ImageCopyExternalTexture.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        ImageCopyExternalTexture.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.ImageCopyExternalTexture>
type Instance internal(handle : nativeint) =
    static let device = Unchecked.defaultof<Device>
    static let nullptr = new Instance(Unchecked.defaultof<_>)
    static let waitPoolCache = System.Collections.Generic.Dictionary<nativeint, WebGPU.Raw.FutureWaitPool>()
    static let getWaitPool (handle : nativeint)=
        lock waitPoolCache (fun () ->
            match waitPoolCache.TryGetValue handle with
            | (true, c) -> c
            | _ ->
                let c = WebGPU.Raw.FutureWaitPool(handle)
                waitPoolCache.[handle] <- c
                c
        )
    let waitPool = lazy (getWaitPool handle)
    member x.EnqueueWait(f : Future) : unit =
        waitPool.Value.Add(WebGPU.Raw.Future(uint64 f.Id))
    member x.Handle = handle
    override x.ToString() = $"Instance(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? Instance as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.CreateSurface(descriptor : SurfaceDescriptor) : Surface =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.InstanceCreateSurface(handle, _descriptorPtr)
            new Surface(res)
        )
    member this.ProcessEvents() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.InstanceProcessEvents(handle)
        res
    member this.WaitAny(futures : array<FutureWaitInfo>, timeoutNS : int64) : WaitStatus =
        let relativePointers = false
        WebGPU.Raw.Pinnable.pinArray device futures (fun futuresPtr ->
            let futuresLen = unativeint futures.Length
            let res = WebGPU.Raw.WebGPU.InstanceWaitAny(handle, futuresLen, futuresPtr, uint64(timeoutNS))
            res
        )
    member this.RequestAdapter(options : RequestAdapterOptions, callbackInfo : RequestAdapterCallbackInfo) : Future =
        let relativePointers = false
        options.Pin(device, fun _optionsPtr ->
            callbackInfo.Pin(device, fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.InstanceRequestAdapter(handle, _optionsPtr, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
                use pppp = fixed &res in Future.Read(device, pppp, relativePointers)
            )
        )
    member this.HasWGSLLanguageFeature(feature : WGSLLanguageFeatureName) : bool =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.InstanceHasWGSLLanguageFeature(handle, feature)
        (res <> 0)
    member this.WGSLLanguageFeatures : SupportedWGSLLanguageFeatures =
        let relativePointers = false
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        try
            WebGPU.Raw.WebGPU.InstanceGetWGSLLanguageFeatures(handle, ptr)
            use pppp = fixed &res in SupportedWGSLLanguageFeatures.Read(device, pppp, relativePointers)
        finally
            ()
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.InstanceRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.InstanceAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type Future = 
    {
        Id : int64
    }
    static member Null = Unchecked.defaultof<Future>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.Future> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.Future(
                    uint64(this.Id)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.Future> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.Future>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            Id = int64(backend.Id)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.Future>) = 
        use ptr = fixed &r
        Future.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        Future.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.Future>
type FutureWaitInfo = 
    {
        Future : Future
        Completed : bool
    }
    static member Null = Unchecked.defaultof<FutureWaitInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.FutureWaitInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            this.Future.Pin(device, fun _futurePtr ->
                let mutable value =
                    new WebGPU.Raw.FutureWaitInfo(
                        (if NativePtr.toNativeInt _futurePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _futurePtr),
                        (if this.Completed then 1 else 0)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.FutureWaitInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.FutureWaitInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            Future = use pppp = fixed &backend.Future in Future.Read(device, pppp, relativePointers)
            Completed = (backend.Completed <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.FutureWaitInfo>) = 
        use ptr = fixed &r
        FutureWaitInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        FutureWaitInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.FutureWaitInfo>
type InstanceLimits = 
    {
        TimedWaitAnyMaxCount : int64
    }
    static member Null = Unchecked.defaultof<InstanceLimits>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.InstanceLimits> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable value =
                new WebGPU.Raw.InstanceLimits(
                    nextInChain,
                    unativeint(this.TimedWaitAnyMaxCount)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.InstanceLimits> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.InstanceLimits>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            TimedWaitAnyMaxCount = int64(backend.TimedWaitAnyMaxCount)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.InstanceLimits>) = 
        use ptr = fixed &r
        InstanceLimits.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        InstanceLimits.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.InstanceLimits>
type InstanceDescriptor = 
    {
        Next : IInstanceDescriptorExtension
        RequiredFeatures : array<InstanceFeatureName>
        RequiredLimits : InstanceLimits
    }
    static member Null = Unchecked.defaultof<InstanceDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.InstanceDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                use requiredFeaturesPtr = fixed (this.RequiredFeatures)
                try
                    let requiredFeaturesLen = unativeint this.RequiredFeatures.Length
                    this.RequiredLimits.Pin(device, fun _requiredLimitsPtr ->
                        let mutable value =
                            new WebGPU.Raw.InstanceDescriptor(
                                nextInChain,
                                requiredFeaturesLen,
                                requiredFeaturesPtr,
                                _requiredLimitsPtr
                            )
                        use ptr = fixed &value
                        try action ptr
                        finally ()
                    )
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.InstanceDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.InstanceDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.RequiredFeatures <> 0n then
                backend.RequiredFeatures <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.RequiredFeatures)
            if NativePtr.toNativeInt backend.RequiredLimits <> 0n then
                backend.RequiredLimits <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.RequiredLimits)
        {
            Next = ExtensionDecoder.decode<IInstanceDescriptorExtension> device relativePointers backend.NextInChain
            RequiredFeatures = let ptr = backend.RequiredFeatures in Array.init (int backend.RequiredFeatureCount) (fun i -> NativePtr.get ptr i)
            RequiredLimits = InstanceLimits.Read(device, backend.RequiredLimits, relativePointers)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.InstanceDescriptor>) = 
        use ptr = fixed &r
        InstanceDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        InstanceDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.InstanceDescriptor>
type DawnWireWGSLControl = 
    {
        Next : IInstanceDescriptorExtension
        EnableExperimental : bool
        EnableUnsafe : bool
        EnableTesting : bool
    }
    static member Null = Unchecked.defaultof<DawnWireWGSLControl>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnWireWGSLControl> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnWireWGSLControl
                let mutable value =
                    new WebGPU.Raw.DawnWireWGSLControl(
                        nextInChain,
                        sType,
                        (if this.EnableExperimental then 1 else 0),
                        (if this.EnableUnsafe then 1 else 0),
                        (if this.EnableTesting then 1 else 0)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IInstanceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnWireWGSLControl> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnWireWGSLControl>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IInstanceDescriptorExtension> device relativePointers backend.NextInChain
            EnableExperimental = (backend.EnableExperimental <> 0)
            EnableUnsafe = (backend.EnableUnsafe <> 0)
            EnableTesting = (backend.EnableTesting <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnWireWGSLControl>) = 
        use ptr = fixed &r
        DawnWireWGSLControl.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnWireWGSLControl.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnWireWGSLControl>
type DawnInjectedInvalidSType = 
    {
        InvalidSType : SType
    }
    static member Null = Unchecked.defaultof<DawnInjectedInvalidSType>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnInjectedInvalidSType> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let sType = SType.DawnInjectedInvalidSType
            let mutable value =
                new WebGPU.Raw.DawnInjectedInvalidSType(
                    nextInChain,
                    sType,
                    this.InvalidSType
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnInjectedInvalidSType> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnInjectedInvalidSType>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            InvalidSType = backend.InvalidSType
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnInjectedInvalidSType>) = 
        use ptr = fixed &r
        DawnInjectedInvalidSType.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnInjectedInvalidSType.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnInjectedInvalidSType>
type VertexAttribute = 
    {
        Format : VertexFormat
        Offset : int64
        ShaderLocation : int
    }
    static member Null = Unchecked.defaultof<VertexAttribute>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.VertexAttribute> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable value =
                new WebGPU.Raw.VertexAttribute(
                    nextInChain,
                    this.Format,
                    uint64(this.Offset),
                    uint32(this.ShaderLocation)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.VertexAttribute> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.VertexAttribute>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Format = backend.Format
            Offset = int64(backend.Offset)
            ShaderLocation = int(backend.ShaderLocation)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.VertexAttribute>) = 
        use ptr = fixed &r
        VertexAttribute.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        VertexAttribute.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.VertexAttribute>
type VertexBufferLayout = 
    {
        StepMode : VertexStepMode
        ArrayStride : int64
        Attributes : array<VertexAttribute>
    }
    static member Null = Unchecked.defaultof<VertexBufferLayout>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.VertexBufferLayout> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            WebGPU.Raw.Pinnable.pinArray device this.Attributes (fun attributesPtr ->
                let attributesLen = unativeint this.Attributes.Length
                let mutable value =
                    new WebGPU.Raw.VertexBufferLayout(
                        nextInChain,
                        this.StepMode,
                        uint64(this.ArrayStride),
                        attributesLen,
                        attributesPtr
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.VertexBufferLayout> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.VertexBufferLayout>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Attributes <> 0n then
                backend.Attributes <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Attributes)
        {
            StepMode = backend.StepMode
            ArrayStride = int64(backend.ArrayStride)
            Attributes = let ptr = backend.Attributes in Array.init (int backend.AttributeCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in VertexAttribute.Read(device, NativePtr.add ptr i, relativePointers))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.VertexBufferLayout>) = 
        use ptr = fixed &r
        VertexBufferLayout.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        VertexBufferLayout.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.VertexBufferLayout>
type Origin3D = 
    {
        X : int
        Y : int
        Z : int
    }
    static member Null = Unchecked.defaultof<Origin3D>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.Origin3D> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.Origin3D(
                    uint32(this.X),
                    uint32(this.Y),
                    uint32(this.Z)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.Origin3D> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.Origin3D>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            X = int(backend.X)
            Y = int(backend.Y)
            Z = int(backend.Z)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.Origin3D>) = 
        use ptr = fixed &r
        Origin3D.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        Origin3D.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.Origin3D>
type Origin2D = 
    {
        X : int
        Y : int
    }
    static member Null = Unchecked.defaultof<Origin2D>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.Origin2D> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.Origin2D(
                    uint32(this.X),
                    uint32(this.Y)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.Origin2D> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.Origin2D>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            X = int(backend.X)
            Y = int(backend.Y)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.Origin2D>) = 
        use ptr = fixed &r
        Origin2D.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        Origin2D.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.Origin2D>
type PassTimestampWrites = 
    {
        QuerySet : QuerySet
        BeginningOfPassWriteIndex : int
        EndOfPassWriteIndex : int
    }
    static member Null = Unchecked.defaultof<PassTimestampWrites>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.PassTimestampWrites> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable value =
                new WebGPU.Raw.PassTimestampWrites(
                    nextInChain,
                    this.QuerySet.Handle,
                    uint32(this.BeginningOfPassWriteIndex),
                    uint32(this.EndOfPassWriteIndex)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.PassTimestampWrites> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.PassTimestampWrites>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            QuerySet = new QuerySet(device, backend.QuerySet)
            BeginningOfPassWriteIndex = int(backend.BeginningOfPassWriteIndex)
            EndOfPassWriteIndex = int(backend.EndOfPassWriteIndex)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.PassTimestampWrites>) = 
        use ptr = fixed &r
        PassTimestampWrites.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        PassTimestampWrites.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.PassTimestampWrites>
type PipelineLayout internal(device : Device, handle : nativeint) =
    static let nullptr = new PipelineLayout(Unchecked.defaultof<_>, Unchecked.defaultof<_>)
    member x.Handle = handle
    member x.Device = device
    override x.ToString() = $"PipelineLayout(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? PipelineLayout as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.PipelineLayoutSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.PipelineLayoutRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.PipelineLayoutAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type PipelineLayoutDescriptor = 
    {
        Next : IPipelineLayoutDescriptorExtension
        Label : string
        BindGroupLayouts : array<BindGroupLayout>
        ImmediateSize : int
    }
    static member Null = Unchecked.defaultof<PipelineLayoutDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.PipelineLayoutDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                try
                    let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                    let bindGroupLayoutsHandles = this.BindGroupLayouts |> Array.map (fun a -> a.Handle)
                    use bindGroupLayoutsPtr = fixed (bindGroupLayoutsHandles)
                    try
                        let bindGroupLayoutsLen = unativeint this.BindGroupLayouts.Length
                        let mutable value =
                            new WebGPU.Raw.PipelineLayoutDescriptor(
                                nextInChain,
                                _labelLen,
                                bindGroupLayoutsLen,
                                bindGroupLayoutsPtr,
                                uint32(this.ImmediateSize)
                            )
                        use ptr = fixed &value
                        try action ptr
                        finally ()
                    finally
                        ()
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.PipelineLayoutDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.PipelineLayoutDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
            if NativePtr.toNativeInt backend.BindGroupLayouts <> 0n then
                backend.BindGroupLayouts <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.BindGroupLayouts)
        {
            Next = ExtensionDecoder.decode<IPipelineLayoutDescriptorExtension> device relativePointers backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            BindGroupLayouts = let ptr = backend.BindGroupLayouts in Array.init (int backend.BindGroupLayoutCount) (fun i -> new BindGroupLayout(NativePtr.get ptr i))
            ImmediateSize = int(backend.ImmediateSize)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.PipelineLayoutDescriptor>) = 
        use ptr = fixed &r
        PipelineLayoutDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        PipelineLayoutDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.PipelineLayoutDescriptor>
type PipelineLayoutPixelLocalStorage = 
    {
        Next : IPipelineLayoutDescriptorExtension
        TotalPixelLocalStorageSize : int64
        StorageAttachments : array<PipelineLayoutStorageAttachment>
    }
    static member Null = Unchecked.defaultof<PipelineLayoutPixelLocalStorage>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.PipelineLayoutPixelLocalStorage> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.PipelineLayoutPixelLocalStorage
                WebGPU.Raw.Pinnable.pinArray device this.StorageAttachments (fun storageAttachmentsPtr ->
                    let storageAttachmentsLen = unativeint this.StorageAttachments.Length
                    let mutable value =
                        new WebGPU.Raw.PipelineLayoutPixelLocalStorage(
                            nextInChain,
                            sType,
                            uint64(this.TotalPixelLocalStorageSize),
                            storageAttachmentsLen,
                            storageAttachmentsPtr
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                )
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IPipelineLayoutDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.PipelineLayoutPixelLocalStorage> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.PipelineLayoutPixelLocalStorage>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.StorageAttachments <> 0n then
                backend.StorageAttachments <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.StorageAttachments)
        {
            Next = ExtensionDecoder.decode<IPipelineLayoutDescriptorExtension> device relativePointers backend.NextInChain
            TotalPixelLocalStorageSize = int64(backend.TotalPixelLocalStorageSize)
            StorageAttachments = let ptr = backend.StorageAttachments in Array.init (int backend.StorageAttachmentCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in PipelineLayoutStorageAttachment.Read(device, NativePtr.add ptr i, relativePointers))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.PipelineLayoutPixelLocalStorage>) = 
        use ptr = fixed &r
        PipelineLayoutPixelLocalStorage.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        PipelineLayoutPixelLocalStorage.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.PipelineLayoutPixelLocalStorage>
type PipelineLayoutStorageAttachment = 
    {
        Offset : int64
        Format : TextureFormat
    }
    static member Null = Unchecked.defaultof<PipelineLayoutStorageAttachment>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.PipelineLayoutStorageAttachment> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable value =
                new WebGPU.Raw.PipelineLayoutStorageAttachment(
                    nextInChain,
                    uint64(this.Offset),
                    this.Format
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.PipelineLayoutStorageAttachment> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.PipelineLayoutStorageAttachment>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Offset = int64(backend.Offset)
            Format = backend.Format
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.PipelineLayoutStorageAttachment>) = 
        use ptr = fixed &r
        PipelineLayoutStorageAttachment.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        PipelineLayoutStorageAttachment.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.PipelineLayoutStorageAttachment>
type ComputeState = 
    {
        Module : ShaderModule
        EntryPoint : string
        Constants : array<ConstantEntry>
    }
    static member Null = Unchecked.defaultof<ComputeState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.ComputeState> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let _entryPointArr = if isNull this.EntryPoint then null else Encoding.UTF8.GetBytes(this.EntryPoint)
            use _entryPointPtr = fixed _entryPointArr
            try
                let _entryPointLen = WebGPU.Raw.StringView(_entryPointPtr, if isNull _entryPointArr then 0un else unativeint _entryPointArr.Length)
                WebGPU.Raw.Pinnable.pinArray device this.Constants (fun constantsPtr ->
                    let constantsLen = unativeint this.Constants.Length
                    let mutable value =
                        new WebGPU.Raw.ComputeState(
                            nextInChain,
                            this.Module.Handle,
                            _entryPointLen,
                            constantsLen,
                            constantsPtr
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                )
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.ComputeState> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.ComputeState>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.EntryPoint.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.EntryPoint - NativePtr.toNativeInt &&backend
                backend.EntryPoint.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.EntryPoint.Data)
            if NativePtr.toNativeInt backend.Constants <> 0n then
                backend.Constants <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Constants)
        {
            Module = new ShaderModule(device, backend.Module)
            EntryPoint = let _entryPointPtr = NativePtr.toNativeInt(backend.EntryPoint.Data) in if _entryPointPtr = 0n then null else Marshal.PtrToStringUTF8(_entryPointPtr, int(backend.EntryPoint.Length))
            Constants = let ptr = backend.Constants in Array.init (int backend.ConstantCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in ConstantEntry.Read(device, NativePtr.add ptr i, relativePointers))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.ComputeState>) = 
        use ptr = fixed &r
        ComputeState.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        ComputeState.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.ComputeState>
type QuerySet internal(device : Device, handle : nativeint) =
    static let nullptr = new QuerySet(Unchecked.defaultof<_>, Unchecked.defaultof<_>)
    let typ =
        lazy (
            let relativePointers = false
            let mutable res = WebGPU.Raw.WebGPU.QuerySetGetType(handle)
            res
        )
    let count =
        lazy (
            let relativePointers = false
            let mutable res = WebGPU.Raw.WebGPU.QuerySetGetCount(handle)
            int(res)
        )
    member x.Handle = handle
    member x.Device = device
    override x.ToString() = $"QuerySet(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? QuerySet as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.QuerySetSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Type : QueryType =
        typ.Value
    member this.Count : int =
        count.Value
    member this.Destroy() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.QuerySetDestroy(handle)
        res
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.QuerySetRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.QuerySetAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type QuerySetDescriptor = 
    {
        Label : string
        Type : QueryType
        Count : int
    }
    static member Null = Unchecked.defaultof<QuerySetDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.QuerySetDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
            use _labelPtr = fixed _labelArr
            try
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                let mutable value =
                    new WebGPU.Raw.QuerySetDescriptor(
                        nextInChain,
                        _labelLen,
                        this.Type,
                        uint32(this.Count)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.QuerySetDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.QuerySetDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
        {
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            Type = backend.Type
            Count = int(backend.Count)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.QuerySetDescriptor>) = 
        use ptr = fixed &r
        QuerySetDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        QuerySetDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.QuerySetDescriptor>
type Queue internal(device : Device, handle : nativeint) =
    static let nullptr = new Queue(Unchecked.defaultof<_>, Unchecked.defaultof<_>)
    member x.Handle = handle
    member x.Device = device
    override x.ToString() = $"Queue(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? Queue as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.Submit(commands : array<CommandBuffer>) : System.Threading.Tasks.Task =
        let relativePointers = false
        let commandsHandles = commands |> Array.map (fun a -> a.Handle)
        use commandsPtr = fixed (commandsHandles)
        try
            let commandsLen = unativeint commands.Length
            let res = WebGPU.Raw.WebGPU.QueueSubmit(handle, commandsLen, commandsPtr)
            res
        finally
            ()
        let tcs = System.Threading.Tasks.TaskCompletionSource<unit>()
        this.OnSubmittedWorkDone { Mode = CallbackMode.WaitAnyOnly; Callback = QueueWorkDoneCallback(fun d _ _ -> d.Dispose(); tcs.SetResult()) } |> device.EnqueueWait
        task {
            do! tcs.Task
            for c in commands do do! c.RunCompleted()
        } :> System.Threading.Tasks.Task
    member this.OnSubmittedWorkDone(callbackInfo : QueueWorkDoneCallbackInfo) : Future =
        let relativePointers = false
        callbackInfo.Pin(device, fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.QueueOnSubmittedWorkDone(handle, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
            use pppp = fixed &res in Future.Read(device, pppp, relativePointers)
        )
    member this.WriteBuffer(buffer : Buffer, bufferOffset : int64, data : nativeint, size : int64) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.QueueWriteBuffer(handle, buffer.Handle, uint64(bufferOffset), data, unativeint(size))
        res
    member this.WriteTexture(destination : TexelCopyTextureInfo, data : nativeint, dataSize : int64, dataLayout : TexelCopyBufferLayout, writeSize : Extent3D) : unit =
        let relativePointers = false
        destination.Pin(device, fun _destinationPtr ->
            dataLayout.Pin(device, fun _dataLayoutPtr ->
                writeSize.Pin(device, fun _writeSizePtr ->
                    let res = WebGPU.Raw.WebGPU.QueueWriteTexture(handle, _destinationPtr, data, unativeint(dataSize), _dataLayoutPtr, _writeSizePtr)
                    res
                )
            )
        )
    member this.CopyTextureForBrowser(source : TexelCopyTextureInfo, destination : TexelCopyTextureInfo, copySize : Extent3D, options : CopyTextureForBrowserOptions) : unit =
        let relativePointers = false
        source.Pin(device, fun _sourcePtr ->
            destination.Pin(device, fun _destinationPtr ->
                copySize.Pin(device, fun _copySizePtr ->
                    options.Pin(device, fun _optionsPtr ->
                        let res = WebGPU.Raw.WebGPU.QueueCopyTextureForBrowser(handle, _sourcePtr, _destinationPtr, _copySizePtr, _optionsPtr)
                        res
                    )
                )
            )
        )
    member this.CopyExternalTextureForBrowser(source : ImageCopyExternalTexture, destination : TexelCopyTextureInfo, copySize : Extent3D, options : CopyTextureForBrowserOptions) : unit =
        let relativePointers = false
        source.Pin(device, fun _sourcePtr ->
            destination.Pin(device, fun _destinationPtr ->
                copySize.Pin(device, fun _copySizePtr ->
                    options.Pin(device, fun _optionsPtr ->
                        let res = WebGPU.Raw.WebGPU.QueueCopyExternalTextureForBrowser(handle, _sourcePtr, _destinationPtr, _copySizePtr, _optionsPtr)
                        res
                    )
                )
            )
        )
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.QueueSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.QueueRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.QueueAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type QueueDescriptor = 
    {
        Label : string
    }
    static member Null = Unchecked.defaultof<QueueDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.QueueDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
            use _labelPtr = fixed _labelArr
            try
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                let mutable value =
                    new WebGPU.Raw.QueueDescriptor(
                        nextInChain,
                        _labelLen
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.QueueDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.QueueDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
        {
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.QueueDescriptor>) = 
        use ptr = fixed &r
        QueueDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        QueueDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.QueueDescriptor>
type QueueWorkDoneCallback = delegate of IDisposable * status : QueueWorkDoneStatus * message : string -> unit
type QueueWorkDoneCallbackInfo = 
    {
        Mode : CallbackMode
        Callback : QueueWorkDoneCallback
    }
    static member Null = Unchecked.defaultof<QueueWorkDoneCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.QueueWorkDoneCallbackInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable _callbackPtr = 0n
            if not (isNull (this.Callback :> obj)) then
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.QueueWorkDoneCallback>
                _callbackDel <- WebGPU.Raw.QueueWorkDoneCallback(fun status message userdata1 userdata2 ->
                    let _status = status
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                _callbackPtr <- Marshal.GetFunctionPointerForDelegate(_callbackDel)
            let mutable value =
                new WebGPU.Raw.QueueWorkDoneCallbackInfo(
                    nextInChain,
                    this.Mode,
                    _callbackPtr,
                    Unchecked.defaultof<_>,
                    Unchecked.defaultof<_>
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.QueueWorkDoneCallbackInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.QueueWorkDoneCallbackInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.QueueWorkDoneCallbackInfo>) = 
        use ptr = fixed &r
        QueueWorkDoneCallbackInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        QueueWorkDoneCallbackInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.QueueWorkDoneCallbackInfo>
type RenderBundle internal(handle : nativeint) =
    static let device = Unchecked.defaultof<Device>
    static let nullptr = new RenderBundle(Unchecked.defaultof<_>)
    member x.Handle = handle
    override x.ToString() = $"RenderBundle(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? RenderBundle as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.RenderBundleSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderBundleRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderBundleAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type RenderBundleEncoder internal(device : Device, handle : nativeint) =
    static let nullptr = new RenderBundleEncoder(Unchecked.defaultof<_>, Unchecked.defaultof<_>)
    member x.Handle = handle
    member x.Device = device
    override x.ToString() = $"RenderBundleEncoder(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? RenderBundleEncoder as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.SetPipeline(pipeline : RenderPipeline) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderSetPipeline(handle, pipeline.Handle)
        res
    member this.SetBindGroup(groupIndex : int, group : BindGroup, dynamicOffsets : array<uint32>) : unit =
        let relativePointers = false
        use dynamicOffsetsPtr = fixed (dynamicOffsets)
        try
            let dynamicOffsetsLen = unativeint dynamicOffsets.Length
            let res = WebGPU.Raw.WebGPU.RenderBundleEncoderSetBindGroup(handle, uint32(groupIndex), group.Handle, dynamicOffsetsLen, dynamicOffsetsPtr)
            res
        finally
            ()
    member this.Draw(vertexCount : int, instanceCount : int, firstVertex : int, firstInstance : int) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderDraw(handle, uint32(vertexCount), uint32(instanceCount), uint32(firstVertex), uint32(firstInstance))
        res
    member this.DrawIndexed(indexCount : int, instanceCount : int, firstIndex : int, baseVertex : int, firstInstance : int) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderDrawIndexed(handle, uint32(indexCount), uint32(instanceCount), uint32(firstIndex), baseVertex, uint32(firstInstance))
        res
    member this.DrawIndirect(indirectBuffer : Buffer, indirectOffset : int64) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderDrawIndirect(handle, indirectBuffer.Handle, uint64(indirectOffset))
        res
    member this.DrawIndexedIndirect(indirectBuffer : Buffer, indirectOffset : int64) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderDrawIndexedIndirect(handle, indirectBuffer.Handle, uint64(indirectOffset))
        res
    member this.InsertDebugMarker(markerLabel : string) : unit =
        let relativePointers = false
        let _markerLabelArr = if isNull markerLabel then null else Encoding.UTF8.GetBytes(markerLabel)
        use _markerLabelPtr = fixed _markerLabelArr
        try
            let _markerLabelLen = WebGPU.Raw.StringView(_markerLabelPtr, if isNull _markerLabelArr then 0un else unativeint _markerLabelArr.Length)
            let res = WebGPU.Raw.WebGPU.RenderBundleEncoderInsertDebugMarker(handle, _markerLabelLen)
            res
        finally
            ()
    member this.PopDebugGroup() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderPopDebugGroup(handle)
        res
    member this.PushDebugGroup(groupLabel : string) : unit =
        let relativePointers = false
        let _groupLabelArr = if isNull groupLabel then null else Encoding.UTF8.GetBytes(groupLabel)
        use _groupLabelPtr = fixed _groupLabelArr
        try
            let _groupLabelLen = WebGPU.Raw.StringView(_groupLabelPtr, if isNull _groupLabelArr then 0un else unativeint _groupLabelArr.Length)
            let res = WebGPU.Raw.WebGPU.RenderBundleEncoderPushDebugGroup(handle, _groupLabelLen)
            res
        finally
            ()
    member this.SetVertexBuffer(slot : int, buffer : Buffer, offset : int64, size : int64) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderSetVertexBuffer(handle, uint32(slot), buffer.Handle, uint64(offset), uint64(size))
        res
    member this.SetIndexBuffer(buffer : Buffer, format : IndexFormat, offset : int64, size : int64) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderSetIndexBuffer(handle, buffer.Handle, format, uint64(offset), uint64(size))
        res
    member this.Finish(descriptor : RenderBundleDescriptor) : RenderBundle =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.RenderBundleEncoderFinish(handle, _descriptorPtr)
            new RenderBundle(res)
        )
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.RenderBundleEncoderSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.SetImmediateData(offset : int, data : nativeint, size : int64) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderSetImmediateData(handle, uint32(offset), data, unativeint(size))
        res
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type RenderBundleDescriptor = 
    {
        Label : string
    }
    static member Null = Unchecked.defaultof<RenderBundleDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.RenderBundleDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
            use _labelPtr = fixed _labelArr
            try
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                let mutable value =
                    new WebGPU.Raw.RenderBundleDescriptor(
                        nextInChain,
                        _labelLen
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.RenderBundleDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.RenderBundleDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
        {
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.RenderBundleDescriptor>) = 
        use ptr = fixed &r
        RenderBundleDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        RenderBundleDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.RenderBundleDescriptor>
type RenderBundleEncoderDescriptor = 
    {
        Label : string
        ColorFormats : array<TextureFormat>
        DepthStencilFormat : TextureFormat
        SampleCount : int
        DepthReadOnly : bool
        StencilReadOnly : bool
    }
    static member Null = Unchecked.defaultof<RenderBundleEncoderDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.RenderBundleEncoderDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
            use _labelPtr = fixed _labelArr
            try
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                use colorFormatsPtr = fixed (this.ColorFormats)
                try
                    let colorFormatsLen = unativeint this.ColorFormats.Length
                    let mutable value =
                        new WebGPU.Raw.RenderBundleEncoderDescriptor(
                            nextInChain,
                            _labelLen,
                            colorFormatsLen,
                            colorFormatsPtr,
                            this.DepthStencilFormat,
                            uint32(this.SampleCount),
                            (if this.DepthReadOnly then 1 else 0),
                            (if this.StencilReadOnly then 1 else 0)
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                finally
                    ()
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.RenderBundleEncoderDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.RenderBundleEncoderDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
            if NativePtr.toNativeInt backend.ColorFormats <> 0n then
                backend.ColorFormats <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.ColorFormats)
        {
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            ColorFormats = let ptr = backend.ColorFormats in Array.init (int backend.ColorFormatCount) (fun i -> NativePtr.get ptr i)
            DepthStencilFormat = backend.DepthStencilFormat
            SampleCount = int(backend.SampleCount)
            DepthReadOnly = (backend.DepthReadOnly <> 0)
            StencilReadOnly = (backend.StencilReadOnly <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.RenderBundleEncoderDescriptor>) = 
        use ptr = fixed &r
        RenderBundleEncoderDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        RenderBundleEncoderDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.RenderBundleEncoderDescriptor>
type RenderPassColorAttachment = 
    {
        Next : IRenderPassColorAttachmentExtension
        View : TextureView
        DepthSlice : int
        ResolveTarget : TextureView
        LoadOp : LoadOp
        StoreOp : StoreOp
        ClearValue : Color
    }
    static member Null = Unchecked.defaultof<RenderPassColorAttachment>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.RenderPassColorAttachment> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                this.ClearValue.Pin(device, fun _clearValuePtr ->
                    let mutable value =
                        new WebGPU.Raw.RenderPassColorAttachment(
                            nextInChain,
                            this.View.Handle,
                            uint32(this.DepthSlice),
                            this.ResolveTarget.Handle,
                            this.LoadOp,
                            this.StoreOp,
                            (if NativePtr.toNativeInt _clearValuePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _clearValuePtr)
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                )
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.RenderPassColorAttachment> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.RenderPassColorAttachment>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IRenderPassColorAttachmentExtension> device relativePointers backend.NextInChain
            View = new TextureView(backend.View)
            DepthSlice = int(backend.DepthSlice)
            ResolveTarget = new TextureView(backend.ResolveTarget)
            LoadOp = backend.LoadOp
            StoreOp = backend.StoreOp
            ClearValue = use pppp = fixed &backend.ClearValue in Color.Read(device, pppp, relativePointers)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.RenderPassColorAttachment>) = 
        use ptr = fixed &r
        RenderPassColorAttachment.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        RenderPassColorAttachment.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.RenderPassColorAttachment>
type DawnRenderPassColorAttachmentRenderToSingleSampled = 
    {
        Next : IRenderPassColorAttachmentExtension
        ImplicitSampleCount : int
    }
    static member Null = Unchecked.defaultof<DawnRenderPassColorAttachmentRenderToSingleSampled>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnRenderPassColorAttachmentRenderToSingleSampled> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnRenderPassColorAttachmentRenderToSingleSampled
                let mutable value =
                    new WebGPU.Raw.DawnRenderPassColorAttachmentRenderToSingleSampled(
                        nextInChain,
                        sType,
                        uint32(this.ImplicitSampleCount)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IRenderPassColorAttachmentExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnRenderPassColorAttachmentRenderToSingleSampled> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnRenderPassColorAttachmentRenderToSingleSampled>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IRenderPassColorAttachmentExtension> device relativePointers backend.NextInChain
            ImplicitSampleCount = int(backend.ImplicitSampleCount)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnRenderPassColorAttachmentRenderToSingleSampled>) = 
        use ptr = fixed &r
        DawnRenderPassColorAttachmentRenderToSingleSampled.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnRenderPassColorAttachmentRenderToSingleSampled.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnRenderPassColorAttachmentRenderToSingleSampled>
type RenderPassDepthStencilAttachment = 
    {
        View : TextureView
        DepthLoadOp : LoadOp
        DepthStoreOp : StoreOp
        DepthClearValue : float32
        DepthReadOnly : bool
        StencilLoadOp : LoadOp
        StencilStoreOp : StoreOp
        StencilClearValue : int
        StencilReadOnly : bool
    }
    static member Null = Unchecked.defaultof<RenderPassDepthStencilAttachment>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.RenderPassDepthStencilAttachment> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable value =
                new WebGPU.Raw.RenderPassDepthStencilAttachment(
                    nextInChain,
                    this.View.Handle,
                    this.DepthLoadOp,
                    this.DepthStoreOp,
                    this.DepthClearValue,
                    (if this.DepthReadOnly then 1 else 0),
                    this.StencilLoadOp,
                    this.StencilStoreOp,
                    uint32(this.StencilClearValue),
                    (if this.StencilReadOnly then 1 else 0)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.RenderPassDepthStencilAttachment> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.RenderPassDepthStencilAttachment>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            View = new TextureView(backend.View)
            DepthLoadOp = backend.DepthLoadOp
            DepthStoreOp = backend.DepthStoreOp
            DepthClearValue = backend.DepthClearValue
            DepthReadOnly = (backend.DepthReadOnly <> 0)
            StencilLoadOp = backend.StencilLoadOp
            StencilStoreOp = backend.StencilStoreOp
            StencilClearValue = int(backend.StencilClearValue)
            StencilReadOnly = (backend.StencilReadOnly <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.RenderPassDepthStencilAttachment>) = 
        use ptr = fixed &r
        RenderPassDepthStencilAttachment.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        RenderPassDepthStencilAttachment.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.RenderPassDepthStencilAttachment>
type RenderPassDescriptor = 
    {
        Next : IRenderPassDescriptorExtension
        Label : string
        ColorAttachments : array<RenderPassColorAttachment>
        DepthStencilAttachment : RenderPassDepthStencilAttachment
        OcclusionQuerySet : QuerySet
        TimestampWrites : PassTimestampWrites
    }
    static member Null = Unchecked.defaultof<RenderPassDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.RenderPassDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                try
                    let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                    WebGPU.Raw.Pinnable.pinArray device this.ColorAttachments (fun colorAttachmentsPtr ->
                        let colorAttachmentsLen = unativeint this.ColorAttachments.Length
                        this.DepthStencilAttachment.Pin(device, fun _depthStencilAttachmentPtr ->
                            this.TimestampWrites.Pin(device, fun _timestampWritesPtr ->
                                let mutable value =
                                    new WebGPU.Raw.RenderPassDescriptor(
                                        nextInChain,
                                        _labelLen,
                                        colorAttachmentsLen,
                                        colorAttachmentsPtr,
                                        _depthStencilAttachmentPtr,
                                        this.OcclusionQuerySet.Handle,
                                        _timestampWritesPtr
                                    )
                                use ptr = fixed &value
                                try action ptr
                                finally ()
                            )
                        )
                    )
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.RenderPassDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.RenderPassDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
            if NativePtr.toNativeInt backend.ColorAttachments <> 0n then
                backend.ColorAttachments <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.ColorAttachments)
            if NativePtr.toNativeInt backend.DepthStencilAttachment <> 0n then
                backend.DepthStencilAttachment <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.DepthStencilAttachment)
            if NativePtr.toNativeInt backend.TimestampWrites <> 0n then
                backend.TimestampWrites <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.TimestampWrites)
        {
            Next = ExtensionDecoder.decode<IRenderPassDescriptorExtension> device relativePointers backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            ColorAttachments = let ptr = backend.ColorAttachments in Array.init (int backend.ColorAttachmentCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in RenderPassColorAttachment.Read(device, NativePtr.add ptr i, relativePointers))
            DepthStencilAttachment = RenderPassDepthStencilAttachment.Read(device, backend.DepthStencilAttachment, relativePointers)
            OcclusionQuerySet = new QuerySet(device, backend.OcclusionQuerySet)
            TimestampWrites = PassTimestampWrites.Read(device, backend.TimestampWrites, relativePointers)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.RenderPassDescriptor>) = 
        use ptr = fixed &r
        RenderPassDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        RenderPassDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.RenderPassDescriptor>
type RenderPassDescriptorMaxDrawCount = RenderPassMaxDrawCount
type RenderPassMaxDrawCount = 
    {
        Next : IRenderPassDescriptorExtension
        MaxDrawCount : int64
    }
    static member Null = Unchecked.defaultof<RenderPassMaxDrawCount>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.RenderPassMaxDrawCount> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.RenderPassMaxDrawCount
                let mutable value =
                    new WebGPU.Raw.RenderPassMaxDrawCount(
                        nextInChain,
                        sType,
                        uint64(this.MaxDrawCount)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IRenderPassDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.RenderPassMaxDrawCount> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.RenderPassMaxDrawCount>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IRenderPassDescriptorExtension> device relativePointers backend.NextInChain
            MaxDrawCount = int64(backend.MaxDrawCount)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.RenderPassMaxDrawCount>) = 
        use ptr = fixed &r
        RenderPassMaxDrawCount.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        RenderPassMaxDrawCount.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.RenderPassMaxDrawCount>
type RenderPassDescriptorExpandResolveRect = 
    {
        Next : IRenderPassDescriptorExtension
        X : int
        Y : int
        Width : int
        Height : int
    }
    static member Null = Unchecked.defaultof<RenderPassDescriptorExpandResolveRect>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.RenderPassDescriptorExpandResolveRect> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.RenderPassDescriptorExpandResolveRect
                let mutable value =
                    new WebGPU.Raw.RenderPassDescriptorExpandResolveRect(
                        nextInChain,
                        sType,
                        uint32(this.X),
                        uint32(this.Y),
                        uint32(this.Width),
                        uint32(this.Height)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IRenderPassDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.RenderPassDescriptorExpandResolveRect> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.RenderPassDescriptorExpandResolveRect>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IRenderPassDescriptorExtension> device relativePointers backend.NextInChain
            X = int(backend.X)
            Y = int(backend.Y)
            Width = int(backend.Width)
            Height = int(backend.Height)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.RenderPassDescriptorExpandResolveRect>) = 
        use ptr = fixed &r
        RenderPassDescriptorExpandResolveRect.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        RenderPassDescriptorExpandResolveRect.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.RenderPassDescriptorExpandResolveRect>
type RenderPassDescriptorResolveRect = 
    {
        Next : IRenderPassDescriptorExtension
        ColorOffsetX : int
        ColorOffsetY : int
        ResolveOffsetX : int
        ResolveOffsetY : int
        Width : int
        Height : int
    }
    static member Null = Unchecked.defaultof<RenderPassDescriptorResolveRect>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.RenderPassDescriptorResolveRect> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.RenderPassDescriptorResolveRect
                let mutable value =
                    new WebGPU.Raw.RenderPassDescriptorResolveRect(
                        nextInChain,
                        sType,
                        uint32(this.ColorOffsetX),
                        uint32(this.ColorOffsetY),
                        uint32(this.ResolveOffsetX),
                        uint32(this.ResolveOffsetY),
                        uint32(this.Width),
                        uint32(this.Height)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IRenderPassDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.RenderPassDescriptorResolveRect> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.RenderPassDescriptorResolveRect>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IRenderPassDescriptorExtension> device relativePointers backend.NextInChain
            ColorOffsetX = int(backend.ColorOffsetX)
            ColorOffsetY = int(backend.ColorOffsetY)
            ResolveOffsetX = int(backend.ResolveOffsetX)
            ResolveOffsetY = int(backend.ResolveOffsetY)
            Width = int(backend.Width)
            Height = int(backend.Height)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.RenderPassDescriptorResolveRect>) = 
        use ptr = fixed &r
        RenderPassDescriptorResolveRect.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        RenderPassDescriptorResolveRect.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.RenderPassDescriptorResolveRect>
type RenderPassPixelLocalStorage = 
    {
        Next : IRenderPassDescriptorExtension
        TotalPixelLocalStorageSize : int64
        StorageAttachments : array<RenderPassStorageAttachment>
    }
    static member Null = Unchecked.defaultof<RenderPassPixelLocalStorage>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.RenderPassPixelLocalStorage> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.RenderPassPixelLocalStorage
                WebGPU.Raw.Pinnable.pinArray device this.StorageAttachments (fun storageAttachmentsPtr ->
                    let storageAttachmentsLen = unativeint this.StorageAttachments.Length
                    let mutable value =
                        new WebGPU.Raw.RenderPassPixelLocalStorage(
                            nextInChain,
                            sType,
                            uint64(this.TotalPixelLocalStorageSize),
                            storageAttachmentsLen,
                            storageAttachmentsPtr
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                )
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IRenderPassDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.RenderPassPixelLocalStorage> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.RenderPassPixelLocalStorage>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.StorageAttachments <> 0n then
                backend.StorageAttachments <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.StorageAttachments)
        {
            Next = ExtensionDecoder.decode<IRenderPassDescriptorExtension> device relativePointers backend.NextInChain
            TotalPixelLocalStorageSize = int64(backend.TotalPixelLocalStorageSize)
            StorageAttachments = let ptr = backend.StorageAttachments in Array.init (int backend.StorageAttachmentCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in RenderPassStorageAttachment.Read(device, NativePtr.add ptr i, relativePointers))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.RenderPassPixelLocalStorage>) = 
        use ptr = fixed &r
        RenderPassPixelLocalStorage.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        RenderPassPixelLocalStorage.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.RenderPassPixelLocalStorage>
type RenderPassStorageAttachment = 
    {
        Offset : int64
        Storage : TextureView
        LoadOp : LoadOp
        StoreOp : StoreOp
        ClearValue : Color
    }
    static member Null = Unchecked.defaultof<RenderPassStorageAttachment>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.RenderPassStorageAttachment> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            this.ClearValue.Pin(device, fun _clearValuePtr ->
                let mutable value =
                    new WebGPU.Raw.RenderPassStorageAttachment(
                        nextInChain,
                        uint64(this.Offset),
                        this.Storage.Handle,
                        this.LoadOp,
                        this.StoreOp,
                        (if NativePtr.toNativeInt _clearValuePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _clearValuePtr)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.RenderPassStorageAttachment> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.RenderPassStorageAttachment>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Offset = int64(backend.Offset)
            Storage = new TextureView(backend.Storage)
            LoadOp = backend.LoadOp
            StoreOp = backend.StoreOp
            ClearValue = use pppp = fixed &backend.ClearValue in Color.Read(device, pppp, relativePointers)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.RenderPassStorageAttachment>) = 
        use ptr = fixed &r
        RenderPassStorageAttachment.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        RenderPassStorageAttachment.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.RenderPassStorageAttachment>
type RenderPassEncoder internal(handle : nativeint) =
    static let device = Unchecked.defaultof<Device>
    static let nullptr = new RenderPassEncoder(Unchecked.defaultof<_>)
    member x.Handle = handle
    override x.ToString() = $"RenderPassEncoder(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? RenderPassEncoder as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.SetPipeline(pipeline : RenderPipeline) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetPipeline(handle, pipeline.Handle)
        res
    member this.SetBindGroup(groupIndex : int, group : BindGroup, dynamicOffsets : array<uint32>) : unit =
        let relativePointers = false
        use dynamicOffsetsPtr = fixed (dynamicOffsets)
        try
            let dynamicOffsetsLen = unativeint dynamicOffsets.Length
            let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetBindGroup(handle, uint32(groupIndex), group.Handle, dynamicOffsetsLen, dynamicOffsetsPtr)
            res
        finally
            ()
    member this.Draw(vertexCount : int, instanceCount : int, firstVertex : int, firstInstance : int) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderDraw(handle, uint32(vertexCount), uint32(instanceCount), uint32(firstVertex), uint32(firstInstance))
        res
    member this.DrawIndexed(indexCount : int, instanceCount : int, firstIndex : int, baseVertex : int, firstInstance : int) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderDrawIndexed(handle, uint32(indexCount), uint32(instanceCount), uint32(firstIndex), baseVertex, uint32(firstInstance))
        res
    member this.DrawIndirect(indirectBuffer : Buffer, indirectOffset : int64) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderDrawIndirect(handle, indirectBuffer.Handle, uint64(indirectOffset))
        res
    member this.DrawIndexedIndirect(indirectBuffer : Buffer, indirectOffset : int64) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderDrawIndexedIndirect(handle, indirectBuffer.Handle, uint64(indirectOffset))
        res
    member this.MultiDrawIndirect(indirectBuffer : Buffer, indirectOffset : int64, maxDrawCount : int, drawCountBuffer : Buffer, drawCountBufferOffset : int64) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderMultiDrawIndirect(handle, indirectBuffer.Handle, uint64(indirectOffset), uint32(maxDrawCount), drawCountBuffer.Handle, uint64(drawCountBufferOffset))
        res
    member this.MultiDrawIndexedIndirect(indirectBuffer : Buffer, indirectOffset : int64, maxDrawCount : int, drawCountBuffer : Buffer, drawCountBufferOffset : int64) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderMultiDrawIndexedIndirect(handle, indirectBuffer.Handle, uint64(indirectOffset), uint32(maxDrawCount), drawCountBuffer.Handle, uint64(drawCountBufferOffset))
        res
    member this.ExecuteBundles(bundles : array<RenderBundle>) : unit =
        let relativePointers = false
        let bundlesHandles = bundles |> Array.map (fun a -> a.Handle)
        use bundlesPtr = fixed (bundlesHandles)
        try
            let bundlesLen = unativeint bundles.Length
            let res = WebGPU.Raw.WebGPU.RenderPassEncoderExecuteBundles(handle, bundlesLen, bundlesPtr)
            res
        finally
            ()
    member this.InsertDebugMarker(markerLabel : string) : unit =
        let relativePointers = false
        let _markerLabelArr = if isNull markerLabel then null else Encoding.UTF8.GetBytes(markerLabel)
        use _markerLabelPtr = fixed _markerLabelArr
        try
            let _markerLabelLen = WebGPU.Raw.StringView(_markerLabelPtr, if isNull _markerLabelArr then 0un else unativeint _markerLabelArr.Length)
            let res = WebGPU.Raw.WebGPU.RenderPassEncoderInsertDebugMarker(handle, _markerLabelLen)
            res
        finally
            ()
    member this.PopDebugGroup() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderPopDebugGroup(handle)
        res
    member this.PushDebugGroup(groupLabel : string) : unit =
        let relativePointers = false
        let _groupLabelArr = if isNull groupLabel then null else Encoding.UTF8.GetBytes(groupLabel)
        use _groupLabelPtr = fixed _groupLabelArr
        try
            let _groupLabelLen = WebGPU.Raw.StringView(_groupLabelPtr, if isNull _groupLabelArr then 0un else unativeint _groupLabelArr.Length)
            let res = WebGPU.Raw.WebGPU.RenderPassEncoderPushDebugGroup(handle, _groupLabelLen)
            res
        finally
            ()
    member this.SetStencilReference(reference : int) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetStencilReference(handle, uint32(reference))
        res
    member this.SetBlendConstant(color : Color) : unit =
        let relativePointers = false
        color.Pin(device, fun _colorPtr ->
            let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetBlendConstant(handle, _colorPtr)
            res
        )
    member this.SetViewport(x : float32, y : float32, width : float32, height : float32, minDepth : float32, maxDepth : float32) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetViewport(handle, x, y, width, height, minDepth, maxDepth)
        res
    member this.SetScissorRect(x : int, y : int, width : int, height : int) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetScissorRect(handle, uint32(x), uint32(y), uint32(width), uint32(height))
        res
    member this.SetVertexBuffer(slot : int, buffer : Buffer, offset : int64, size : int64) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetVertexBuffer(handle, uint32(slot), buffer.Handle, uint64(offset), uint64(size))
        res
    member this.SetIndexBuffer(buffer : Buffer, format : IndexFormat, offset : int64, size : int64) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetIndexBuffer(handle, buffer.Handle, format, uint64(offset), uint64(size))
        res
    member this.BeginOcclusionQuery(queryIndex : int) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderBeginOcclusionQuery(handle, uint32(queryIndex))
        res
    member this.EndOcclusionQuery() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderEndOcclusionQuery(handle)
        res
    member this.WriteTimestamp(querySet : QuerySet, queryIndex : int) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderWriteTimestamp(handle, querySet.Handle, uint32(queryIndex))
        res
    member this.PixelLocalStorageBarrier() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderPixelLocalStorageBarrier(handle)
        res
    member this.End() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderEnd(handle)
        res
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.SetImmediateData(offset : int, data : nativeint, size : int64) : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetImmediateData(handle, uint32(offset), data, unativeint(size))
        res
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type RenderPipeline internal(handle : nativeint) =
    static let device = Unchecked.defaultof<Device>
    static let nullptr = new RenderPipeline(Unchecked.defaultof<_>)
    member x.Handle = handle
    override x.ToString() = $"RenderPipeline(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? RenderPipeline as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.GetBindGroupLayout(groupIndex : int) : BindGroupLayout =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPipelineGetBindGroupLayout(handle, uint32(groupIndex))
        new BindGroupLayout(res)
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.RenderPipelineSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPipelineRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.RenderPipelineAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type RequestDeviceCallback = delegate of IDisposable * status : RequestDeviceStatus * device : Device * message : string -> unit
type RequestDeviceCallbackInfo = 
    {
        Mode : CallbackMode
        Callback : RequestDeviceCallback
    }
    static member Null = Unchecked.defaultof<RequestDeviceCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.RequestDeviceCallbackInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable _callbackPtr = 0n
            if not (isNull (this.Callback :> obj)) then
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.RequestDeviceCallback>
                _callbackDel <- WebGPU.Raw.RequestDeviceCallback(fun status device message userdata1 userdata2 ->
                    let _status = status
                    let _device = new Device(device)
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _device, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                _callbackPtr <- Marshal.GetFunctionPointerForDelegate(_callbackDel)
            let mutable value =
                new WebGPU.Raw.RequestDeviceCallbackInfo(
                    nextInChain,
                    this.Mode,
                    _callbackPtr,
                    Unchecked.defaultof<_>,
                    Unchecked.defaultof<_>
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.RequestDeviceCallbackInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.RequestDeviceCallbackInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.RequestDeviceCallbackInfo>) = 
        use ptr = fixed &r
        RequestDeviceCallbackInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        RequestDeviceCallbackInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.RequestDeviceCallbackInfo>
type VertexState = 
    {
        Module : ShaderModule
        EntryPoint : string
        Constants : array<ConstantEntry>
        Buffers : array<VertexBufferLayout>
    }
    static member Null = Unchecked.defaultof<VertexState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.VertexState> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let _entryPointArr = if isNull this.EntryPoint then null else Encoding.UTF8.GetBytes(this.EntryPoint)
            use _entryPointPtr = fixed _entryPointArr
            try
                let _entryPointLen = WebGPU.Raw.StringView(_entryPointPtr, if isNull _entryPointArr then 0un else unativeint _entryPointArr.Length)
                WebGPU.Raw.Pinnable.pinArray device this.Constants (fun constantsPtr ->
                    let constantsLen = unativeint this.Constants.Length
                    WebGPU.Raw.Pinnable.pinArray device this.Buffers (fun buffersPtr ->
                        let buffersLen = unativeint this.Buffers.Length
                        let mutable value =
                            new WebGPU.Raw.VertexState(
                                nextInChain,
                                this.Module.Handle,
                                _entryPointLen,
                                constantsLen,
                                constantsPtr,
                                buffersLen,
                                buffersPtr
                            )
                        use ptr = fixed &value
                        try action ptr
                        finally ()
                    )
                )
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.VertexState> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.VertexState>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.EntryPoint.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.EntryPoint - NativePtr.toNativeInt &&backend
                backend.EntryPoint.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.EntryPoint.Data)
            if NativePtr.toNativeInt backend.Constants <> 0n then
                backend.Constants <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Constants)
            if NativePtr.toNativeInt backend.Buffers <> 0n then
                backend.Buffers <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Buffers)
        {
            Module = new ShaderModule(device, backend.Module)
            EntryPoint = let _entryPointPtr = NativePtr.toNativeInt(backend.EntryPoint.Data) in if _entryPointPtr = 0n then null else Marshal.PtrToStringUTF8(_entryPointPtr, int(backend.EntryPoint.Length))
            Constants = let ptr = backend.Constants in Array.init (int backend.ConstantCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in ConstantEntry.Read(device, NativePtr.add ptr i, relativePointers))
            Buffers = let ptr = backend.Buffers in Array.init (int backend.BufferCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in VertexBufferLayout.Read(device, NativePtr.add ptr i, relativePointers))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.VertexState>) = 
        use ptr = fixed &r
        VertexState.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        VertexState.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.VertexState>
type PrimitiveState = 
    {
        Topology : PrimitiveTopology
        StripIndexFormat : IndexFormat
        FrontFace : FrontFace
        CullMode : CullMode
        UnclippedDepth : bool
    }
    static member Null = Unchecked.defaultof<PrimitiveState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.PrimitiveState> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable value =
                new WebGPU.Raw.PrimitiveState(
                    nextInChain,
                    this.Topology,
                    this.StripIndexFormat,
                    this.FrontFace,
                    this.CullMode,
                    (if this.UnclippedDepth then 1 else 0)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.PrimitiveState> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.PrimitiveState>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Topology = backend.Topology
            StripIndexFormat = backend.StripIndexFormat
            FrontFace = backend.FrontFace
            CullMode = backend.CullMode
            UnclippedDepth = (backend.UnclippedDepth <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.PrimitiveState>) = 
        use ptr = fixed &r
        PrimitiveState.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        PrimitiveState.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.PrimitiveState>
type DepthStencilState = 
    {
        Format : TextureFormat
        DepthWriteEnabled : OptionalBool
        DepthCompare : CompareFunction
        StencilFront : StencilFaceState
        StencilBack : StencilFaceState
        StencilReadMask : int
        StencilWriteMask : int
        DepthBias : int
        DepthBiasSlopeScale : float32
        DepthBiasClamp : float32
    }
    static member Null = Unchecked.defaultof<DepthStencilState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DepthStencilState> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            this.StencilFront.Pin(device, fun _stencilFrontPtr ->
                this.StencilBack.Pin(device, fun _stencilBackPtr ->
                    let mutable value =
                        new WebGPU.Raw.DepthStencilState(
                            nextInChain,
                            this.Format,
                            this.DepthWriteEnabled,
                            this.DepthCompare,
                            (if NativePtr.toNativeInt _stencilFrontPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _stencilFrontPtr),
                            (if NativePtr.toNativeInt _stencilBackPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _stencilBackPtr),
                            uint32(this.StencilReadMask),
                            uint32(this.StencilWriteMask),
                            this.DepthBias,
                            this.DepthBiasSlopeScale,
                            this.DepthBiasClamp
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                )
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DepthStencilState> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DepthStencilState>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Format = backend.Format
            DepthWriteEnabled = backend.DepthWriteEnabled
            DepthCompare = backend.DepthCompare
            StencilFront = use pppp = fixed &backend.StencilFront in StencilFaceState.Read(device, pppp, relativePointers)
            StencilBack = use pppp = fixed &backend.StencilBack in StencilFaceState.Read(device, pppp, relativePointers)
            StencilReadMask = int(backend.StencilReadMask)
            StencilWriteMask = int(backend.StencilWriteMask)
            DepthBias = backend.DepthBias
            DepthBiasSlopeScale = backend.DepthBiasSlopeScale
            DepthBiasClamp = backend.DepthBiasClamp
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DepthStencilState>) = 
        use ptr = fixed &r
        DepthStencilState.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DepthStencilState.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DepthStencilState>
type MultisampleState = 
    {
        Count : int
        Mask : int
        AlphaToCoverageEnabled : bool
    }
    static member Null = Unchecked.defaultof<MultisampleState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.MultisampleState> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable value =
                new WebGPU.Raw.MultisampleState(
                    nextInChain,
                    uint32(this.Count),
                    uint32(this.Mask),
                    (if this.AlphaToCoverageEnabled then 1 else 0)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.MultisampleState> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.MultisampleState>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Count = int(backend.Count)
            Mask = int(backend.Mask)
            AlphaToCoverageEnabled = (backend.AlphaToCoverageEnabled <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.MultisampleState>) = 
        use ptr = fixed &r
        MultisampleState.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        MultisampleState.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.MultisampleState>
type FragmentState = 
    {
        Module : ShaderModule
        EntryPoint : string
        Constants : array<ConstantEntry>
        Targets : array<ColorTargetState>
    }
    static member Null = Unchecked.defaultof<FragmentState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.FragmentState> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let _entryPointArr = if isNull this.EntryPoint then null else Encoding.UTF8.GetBytes(this.EntryPoint)
            use _entryPointPtr = fixed _entryPointArr
            try
                let _entryPointLen = WebGPU.Raw.StringView(_entryPointPtr, if isNull _entryPointArr then 0un else unativeint _entryPointArr.Length)
                WebGPU.Raw.Pinnable.pinArray device this.Constants (fun constantsPtr ->
                    let constantsLen = unativeint this.Constants.Length
                    WebGPU.Raw.Pinnable.pinArray device this.Targets (fun targetsPtr ->
                        let targetsLen = unativeint this.Targets.Length
                        let mutable value =
                            new WebGPU.Raw.FragmentState(
                                nextInChain,
                                this.Module.Handle,
                                _entryPointLen,
                                constantsLen,
                                constantsPtr,
                                targetsLen,
                                targetsPtr
                            )
                        use ptr = fixed &value
                        try action ptr
                        finally ()
                    )
                )
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.FragmentState> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.FragmentState>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.EntryPoint.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.EntryPoint - NativePtr.toNativeInt &&backend
                backend.EntryPoint.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.EntryPoint.Data)
            if NativePtr.toNativeInt backend.Constants <> 0n then
                backend.Constants <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Constants)
            if NativePtr.toNativeInt backend.Targets <> 0n then
                backend.Targets <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Targets)
        {
            Module = new ShaderModule(device, backend.Module)
            EntryPoint = let _entryPointPtr = NativePtr.toNativeInt(backend.EntryPoint.Data) in if _entryPointPtr = 0n then null else Marshal.PtrToStringUTF8(_entryPointPtr, int(backend.EntryPoint.Length))
            Constants = let ptr = backend.Constants in Array.init (int backend.ConstantCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in ConstantEntry.Read(device, NativePtr.add ptr i, relativePointers))
            Targets = let ptr = backend.Targets in Array.init (int backend.TargetCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in ColorTargetState.Read(device, NativePtr.add ptr i, relativePointers))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.FragmentState>) = 
        use ptr = fixed &r
        FragmentState.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        FragmentState.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.FragmentState>
type ColorTargetState = 
    {
        Next : IColorTargetStateExtension
        Format : TextureFormat
        Blend : BlendState
        WriteMask : ColorWriteMask
    }
    static member Null = Unchecked.defaultof<ColorTargetState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.ColorTargetState> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                this.Blend.Pin(device, fun _blendPtr ->
                    let mutable value =
                        new WebGPU.Raw.ColorTargetState(
                            nextInChain,
                            this.Format,
                            _blendPtr,
                            this.WriteMask
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                )
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.ColorTargetState> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.ColorTargetState>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Blend <> 0n then
                backend.Blend <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Blend)
        {
            Next = ExtensionDecoder.decode<IColorTargetStateExtension> device relativePointers backend.NextInChain
            Format = backend.Format
            Blend = BlendState.Read(device, backend.Blend, relativePointers)
            WriteMask = backend.WriteMask
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.ColorTargetState>) = 
        use ptr = fixed &r
        ColorTargetState.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        ColorTargetState.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.ColorTargetState>
type ColorTargetStateExpandResolveTextureDawn = 
    {
        Next : IColorTargetStateExtension
        Enabled : bool
    }
    static member Null = Unchecked.defaultof<ColorTargetStateExpandResolveTextureDawn>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.ColorTargetStateExpandResolveTextureDawn> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.ColorTargetStateExpandResolveTextureDawn
                let mutable value =
                    new WebGPU.Raw.ColorTargetStateExpandResolveTextureDawn(
                        nextInChain,
                        sType,
                        (if this.Enabled then 1 else 0)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IColorTargetStateExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.ColorTargetStateExpandResolveTextureDawn> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.ColorTargetStateExpandResolveTextureDawn>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IColorTargetStateExtension> device relativePointers backend.NextInChain
            Enabled = (backend.Enabled <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.ColorTargetStateExpandResolveTextureDawn>) = 
        use ptr = fixed &r
        ColorTargetStateExpandResolveTextureDawn.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        ColorTargetStateExpandResolveTextureDawn.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.ColorTargetStateExpandResolveTextureDawn>
type BlendState = 
    {
        Color : BlendComponent
        Alpha : BlendComponent
    }
    static member Null = Unchecked.defaultof<BlendState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.BlendState> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            this.Color.Pin(device, fun _colorPtr ->
                this.Alpha.Pin(device, fun _alphaPtr ->
                    let mutable value =
                        new WebGPU.Raw.BlendState(
                            (if NativePtr.toNativeInt _colorPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _colorPtr),
                            (if NativePtr.toNativeInt _alphaPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _alphaPtr)
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                )
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.BlendState> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.BlendState>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            Color = use pppp = fixed &backend.Color in BlendComponent.Read(device, pppp, relativePointers)
            Alpha = use pppp = fixed &backend.Alpha in BlendComponent.Read(device, pppp, relativePointers)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.BlendState>) = 
        use ptr = fixed &r
        BlendState.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        BlendState.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.BlendState>
type RenderPipelineDescriptor = 
    {
        Label : string
        Layout : PipelineLayout
        Vertex : VertexState
        Primitive : PrimitiveState
        DepthStencil : DepthStencilState
        Multisample : MultisampleState
        Fragment : FragmentState
    }
    static member Null = Unchecked.defaultof<RenderPipelineDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.RenderPipelineDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
            use _labelPtr = fixed _labelArr
            try
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                this.Vertex.Pin(device, fun _vertexPtr ->
                    this.Primitive.Pin(device, fun _primitivePtr ->
                        this.DepthStencil.Pin(device, fun _depthStencilPtr ->
                            this.Multisample.Pin(device, fun _multisamplePtr ->
                                this.Fragment.Pin(device, fun _fragmentPtr ->
                                    let mutable value =
                                        new WebGPU.Raw.RenderPipelineDescriptor(
                                            nextInChain,
                                            _labelLen,
                                            this.Layout.Handle,
                                            (if NativePtr.toNativeInt _vertexPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _vertexPtr),
                                            (if NativePtr.toNativeInt _primitivePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _primitivePtr),
                                            _depthStencilPtr,
                                            (if NativePtr.toNativeInt _multisamplePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _multisamplePtr),
                                            _fragmentPtr
                                        )
                                    use ptr = fixed &value
                                    try action ptr
                                    finally ()
                                )
                            )
                        )
                    )
                )
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.RenderPipelineDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.RenderPipelineDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
            if NativePtr.toNativeInt backend.DepthStencil <> 0n then
                backend.DepthStencil <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.DepthStencil)
            if NativePtr.toNativeInt backend.Fragment <> 0n then
                backend.Fragment <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Fragment)
        {
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            Layout = new PipelineLayout(device, backend.Layout)
            Vertex = use pppp = fixed &backend.Vertex in VertexState.Read(device, pppp, relativePointers)
            Primitive = use pppp = fixed &backend.Primitive in PrimitiveState.Read(device, pppp, relativePointers)
            DepthStencil = DepthStencilState.Read(device, backend.DepthStencil, relativePointers)
            Multisample = use pppp = fixed &backend.Multisample in MultisampleState.Read(device, pppp, relativePointers)
            Fragment = FragmentState.Read(device, backend.Fragment, relativePointers)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.RenderPipelineDescriptor>) = 
        use ptr = fixed &r
        RenderPipelineDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        RenderPipelineDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.RenderPipelineDescriptor>
type Sampler internal(device : Device, handle : nativeint) =
    static let nullptr = new Sampler(Unchecked.defaultof<_>, Unchecked.defaultof<_>)
    member x.Handle = handle
    member x.Device = device
    override x.ToString() = $"Sampler(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? Sampler as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.SamplerSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.SamplerRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.SamplerAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type SamplerDescriptor = 
    {
        Next : ISamplerDescriptorExtension
        Label : string
        AddressModeU : AddressMode
        AddressModeV : AddressMode
        AddressModeW : AddressMode
        MagFilter : FilterMode
        MinFilter : FilterMode
        MipmapFilter : MipmapFilterMode
        LodMinClamp : float32
        LodMaxClamp : float32
        Compare : CompareFunction
        MaxAnisotropy : uint16
    }
    static member Null = Unchecked.defaultof<SamplerDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SamplerDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                try
                    let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                    let mutable value =
                        new WebGPU.Raw.SamplerDescriptor(
                            nextInChain,
                            _labelLen,
                            this.AddressModeU,
                            this.AddressModeV,
                            this.AddressModeW,
                            this.MagFilter,
                            this.MinFilter,
                            this.MipmapFilter,
                            this.LodMinClamp,
                            this.LodMaxClamp,
                            this.Compare,
                            this.MaxAnisotropy
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SamplerDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SamplerDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
        {
            Next = ExtensionDecoder.decode<ISamplerDescriptorExtension> device relativePointers backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            AddressModeU = backend.AddressModeU
            AddressModeV = backend.AddressModeV
            AddressModeW = backend.AddressModeW
            MagFilter = backend.MagFilter
            MinFilter = backend.MinFilter
            MipmapFilter = backend.MipmapFilter
            LodMinClamp = backend.LodMinClamp
            LodMaxClamp = backend.LodMaxClamp
            Compare = backend.Compare
            MaxAnisotropy = backend.MaxAnisotropy
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SamplerDescriptor>) = 
        use ptr = fixed &r
        SamplerDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SamplerDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SamplerDescriptor>
type ShaderModule internal(device : Device, handle : nativeint) =
    static let nullptr = new ShaderModule(Unchecked.defaultof<_>, Unchecked.defaultof<_>)
    member x.Handle = handle
    member x.Device = device
    override x.ToString() = $"ShaderModule(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? ShaderModule as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.GetCompilationInfo(callbackInfo : CompilationInfoCallbackInfo) : Future =
        let relativePointers = false
        callbackInfo.Pin(device, fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.ShaderModuleGetCompilationInfo(handle, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
            use pppp = fixed &res in Future.Read(device, pppp, relativePointers)
        )
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.ShaderModuleSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ShaderModuleRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.ShaderModuleAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type ShaderModuleDescriptor = 
    {
        Next : IShaderModuleDescriptorExtension
        Label : string
    }
    static member Null = Unchecked.defaultof<ShaderModuleDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.ShaderModuleDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                try
                    let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                    let mutable value =
                        new WebGPU.Raw.ShaderModuleDescriptor(
                            nextInChain,
                            _labelLen
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.ShaderModuleDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.ShaderModuleDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
        {
            Next = ExtensionDecoder.decode<IShaderModuleDescriptorExtension> device relativePointers backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.ShaderModuleDescriptor>) = 
        use ptr = fixed &r
        ShaderModuleDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        ShaderModuleDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.ShaderModuleDescriptor>
type ShaderModuleSPIRVDescriptor = ShaderSourceSPIRV
type ShaderSourceSPIRV = 
    {
        Next : IShaderModuleDescriptorExtension
        Code : array<uint32>
    }
    static member Null = Unchecked.defaultof<ShaderSourceSPIRV>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.ShaderSourceSPIRV> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.ShaderSourceSPIRV
                use codePtr = fixed (this.Code)
                try
                    let codeLen = uint32 this.Code.Length
                    let mutable value =
                        new WebGPU.Raw.ShaderSourceSPIRV(
                            nextInChain,
                            sType,
                            codeLen,
                            codePtr
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                finally
                    ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IShaderModuleDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.ShaderSourceSPIRV> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.ShaderSourceSPIRV>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Code <> 0n then
                backend.Code <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Code)
        {
            Next = ExtensionDecoder.decode<IShaderModuleDescriptorExtension> device relativePointers backend.NextInChain
            Code = let ptr = backend.Code in Array.init (int backend.CodeSize) (fun i -> NativePtr.get ptr i)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.ShaderSourceSPIRV>) = 
        use ptr = fixed &r
        ShaderSourceSPIRV.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        ShaderSourceSPIRV.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.ShaderSourceSPIRV>
type ShaderModuleWGSLDescriptor = ShaderSourceWGSL
type ShaderSourceWGSL = 
    {
        Next : IShaderModuleDescriptorExtension
        Code : string
    }
    static member Null = Unchecked.defaultof<ShaderSourceWGSL>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.ShaderSourceWGSL> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.ShaderSourceWGSL
                let _codeArr = if isNull this.Code then null else Encoding.UTF8.GetBytes(this.Code)
                use _codePtr = fixed _codeArr
                try
                    let _codeLen = WebGPU.Raw.StringView(_codePtr, if isNull _codeArr then 0un else unativeint _codeArr.Length)
                    let mutable value =
                        new WebGPU.Raw.ShaderSourceWGSL(
                            nextInChain,
                            sType,
                            _codeLen
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                finally
                    ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IShaderModuleDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.ShaderSourceWGSL> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.ShaderSourceWGSL>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Code.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Code - NativePtr.toNativeInt &&backend
                backend.Code.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Code.Data)
        {
            Next = ExtensionDecoder.decode<IShaderModuleDescriptorExtension> device relativePointers backend.NextInChain
            Code = let _codePtr = NativePtr.toNativeInt(backend.Code.Data) in if _codePtr = 0n then null else Marshal.PtrToStringUTF8(_codePtr, int(backend.Code.Length))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.ShaderSourceWGSL>) = 
        use ptr = fixed &r
        ShaderSourceWGSL.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        ShaderSourceWGSL.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.ShaderSourceWGSL>
type DawnShaderModuleSPIRVOptionsDescriptor = 
    {
        Next : IShaderModuleDescriptorExtension
        AllowNonUniformDerivatives : bool
    }
    static member Null = Unchecked.defaultof<DawnShaderModuleSPIRVOptionsDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnShaderModuleSPIRVOptionsDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnShaderModuleSPIRVOptionsDescriptor
                let mutable value =
                    new WebGPU.Raw.DawnShaderModuleSPIRVOptionsDescriptor(
                        nextInChain,
                        sType,
                        (if this.AllowNonUniformDerivatives then 1 else 0)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IShaderModuleDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnShaderModuleSPIRVOptionsDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnShaderModuleSPIRVOptionsDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IShaderModuleDescriptorExtension> device relativePointers backend.NextInChain
            AllowNonUniformDerivatives = (backend.AllowNonUniformDerivatives <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnShaderModuleSPIRVOptionsDescriptor>) = 
        use ptr = fixed &r
        DawnShaderModuleSPIRVOptionsDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnShaderModuleSPIRVOptionsDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnShaderModuleSPIRVOptionsDescriptor>
type ShaderModuleCompilationOptions = 
    {
        Next : IShaderModuleDescriptorExtension
        StrictMath : bool
    }
    static member Null = Unchecked.defaultof<ShaderModuleCompilationOptions>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.ShaderModuleCompilationOptions> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.ShaderModuleCompilationOptions
                let mutable value =
                    new WebGPU.Raw.ShaderModuleCompilationOptions(
                        nextInChain,
                        sType,
                        (if this.StrictMath then 1 else 0)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IShaderModuleDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.ShaderModuleCompilationOptions> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.ShaderModuleCompilationOptions>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IShaderModuleDescriptorExtension> device relativePointers backend.NextInChain
            StrictMath = (backend.StrictMath <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.ShaderModuleCompilationOptions>) = 
        use ptr = fixed &r
        ShaderModuleCompilationOptions.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        ShaderModuleCompilationOptions.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.ShaderModuleCompilationOptions>
type StencilFaceState = 
    {
        Compare : CompareFunction
        FailOp : StencilOperation
        DepthFailOp : StencilOperation
        PassOp : StencilOperation
    }
    static member Null = Unchecked.defaultof<StencilFaceState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.StencilFaceState> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.StencilFaceState(
                    this.Compare,
                    this.FailOp,
                    this.DepthFailOp,
                    this.PassOp
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.StencilFaceState> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.StencilFaceState>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            Compare = backend.Compare
            FailOp = backend.FailOp
            DepthFailOp = backend.DepthFailOp
            PassOp = backend.PassOp
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.StencilFaceState>) = 
        use ptr = fixed &r
        StencilFaceState.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        StencilFaceState.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.StencilFaceState>
type Surface internal(handle : nativeint) =
    static let device = Unchecked.defaultof<Device>
    static let nullptr = new Surface(Unchecked.defaultof<_>)
    member x.Handle = handle
    override x.ToString() = $"Surface(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? Surface as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.Configure(config : SurfaceConfiguration) : unit =
        let relativePointers = false
        config.Pin(device, fun _configPtr ->
            let res = WebGPU.Raw.WebGPU.SurfaceConfigure(handle, _configPtr)
            res
        )
    member this.GetCapabilities(adapter : Adapter, capabilities : byref<SurfaceCapabilities>) : Status =
        let relativePointers = false
        let mutable capabilitiesCopy = capabilities
        try
            capabilities.Pin(device, fun _capabilitiesPtr ->
                if NativePtr.toNativeInt _capabilitiesPtr = 0n then
                    let mutable capabilitiesNative = Unchecked.defaultof<WebGPU.Raw.SurfaceCapabilities>
                    use _capabilitiesPtr = fixed &capabilitiesNative
                    try
                        let res = WebGPU.Raw.WebGPU.SurfaceGetCapabilities(handle, adapter.Handle, _capabilitiesPtr)
                        let _ret = res
                        capabilitiesCopy <- SurfaceCapabilities.Read(device, _capabilitiesPtr, relativePointers)
                        _ret
                    finally
                        ()
                else
                    let res = WebGPU.Raw.WebGPU.SurfaceGetCapabilities(handle, adapter.Handle, _capabilitiesPtr)
                    let _ret = res
                    capabilitiesCopy <- SurfaceCapabilities.Read(device, _capabilitiesPtr, relativePointers)
                    _ret
                )
        finally
            capabilities <- capabilitiesCopy
    member this.CurrentTexture : SurfaceTexture =
        let relativePointers = false
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        try
            WebGPU.Raw.WebGPU.SurfaceGetCurrentTexture(handle, ptr)
            use pppp = fixed &res in SurfaceTexture.Read(device, pppp, relativePointers)
        finally
            ()
    member this.Present() : Status =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.SurfacePresent(handle)
        res
    member this.Unconfigure() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.SurfaceUnconfigure(handle)
        res
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.SurfaceSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.SurfaceRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.SurfaceAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type SurfaceDescriptor = 
    {
        Next : ISurfaceDescriptorExtension
        Label : string
    }
    static member Null = Unchecked.defaultof<SurfaceDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SurfaceDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                try
                    let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                    let mutable value =
                        new WebGPU.Raw.SurfaceDescriptor(
                            nextInChain,
                            _labelLen
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SurfaceDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SurfaceDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> device relativePointers backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SurfaceDescriptor>) = 
        use ptr = fixed &r
        SurfaceDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SurfaceDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SurfaceDescriptor>
type SurfaceDescriptorFromAndroidNativeWindow = SurfaceSourceAndroidNativeWindow
type SurfaceSourceAndroidNativeWindow = 
    {
        Next : ISurfaceDescriptorExtension
        Window : nativeint
    }
    static member Null = Unchecked.defaultof<SurfaceSourceAndroidNativeWindow>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SurfaceSourceAndroidNativeWindow> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SurfaceSourceAndroidNativeWindow
                let mutable value =
                    new WebGPU.Raw.SurfaceSourceAndroidNativeWindow(
                        nextInChain,
                        sType,
                        this.Window
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SurfaceSourceAndroidNativeWindow> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SurfaceSourceAndroidNativeWindow>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> device relativePointers backend.NextInChain
            Window = backend.Window
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SurfaceSourceAndroidNativeWindow>) = 
        use ptr = fixed &r
        SurfaceSourceAndroidNativeWindow.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SurfaceSourceAndroidNativeWindow.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SurfaceSourceAndroidNativeWindow>
type EmscriptenSurfaceSourceCanvasHTMLSelector = 
    {
        Next : ISurfaceDescriptorExtension
        Selector : string
    }
    static member Null = Unchecked.defaultof<EmscriptenSurfaceSourceCanvasHTMLSelector>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.EmscriptenSurfaceSourceCanvasHTMLSelector> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.EmscriptenSurfaceSourceCanvasHTMLSelector
                let _selectorArr = if isNull this.Selector then null else Encoding.UTF8.GetBytes(this.Selector)
                use _selectorPtr = fixed _selectorArr
                try
                    let _selectorLen = WebGPU.Raw.StringView(_selectorPtr, if isNull _selectorArr then 0un else unativeint _selectorArr.Length)
                    let mutable value =
                        new WebGPU.Raw.EmscriptenSurfaceSourceCanvasHTMLSelector(
                            nextInChain,
                            sType,
                            _selectorLen
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                finally
                    ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.EmscriptenSurfaceSourceCanvasHTMLSelector> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.EmscriptenSurfaceSourceCanvasHTMLSelector>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Selector.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Selector - NativePtr.toNativeInt &&backend
                backend.Selector.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Selector.Data)
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> device relativePointers backend.NextInChain
            Selector = let _selectorPtr = NativePtr.toNativeInt(backend.Selector.Data) in if _selectorPtr = 0n then null else Marshal.PtrToStringUTF8(_selectorPtr, int(backend.Selector.Length))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.EmscriptenSurfaceSourceCanvasHTMLSelector>) = 
        use ptr = fixed &r
        EmscriptenSurfaceSourceCanvasHTMLSelector.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        EmscriptenSurfaceSourceCanvasHTMLSelector.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.EmscriptenSurfaceSourceCanvasHTMLSelector>
type SurfaceDescriptorFromMetalLayer = SurfaceSourceMetalLayer
type SurfaceSourceMetalLayer = 
    {
        Next : ISurfaceDescriptorExtension
        Layer : nativeint
    }
    static member Null = Unchecked.defaultof<SurfaceSourceMetalLayer>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SurfaceSourceMetalLayer> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SurfaceSourceMetalLayer
                let mutable value =
                    new WebGPU.Raw.SurfaceSourceMetalLayer(
                        nextInChain,
                        sType,
                        this.Layer
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SurfaceSourceMetalLayer> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SurfaceSourceMetalLayer>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> device relativePointers backend.NextInChain
            Layer = backend.Layer
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SurfaceSourceMetalLayer>) = 
        use ptr = fixed &r
        SurfaceSourceMetalLayer.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SurfaceSourceMetalLayer.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SurfaceSourceMetalLayer>
type SurfaceDescriptorFromWindowsHWND = SurfaceSourceWindowsHWND
type SurfaceSourceWindowsHWND = 
    {
        Next : ISurfaceDescriptorExtension
        Hinstance : nativeint
        Hwnd : nativeint
    }
    static member Null = Unchecked.defaultof<SurfaceSourceWindowsHWND>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SurfaceSourceWindowsHWND> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SurfaceSourceWindowsHWND
                let mutable value =
                    new WebGPU.Raw.SurfaceSourceWindowsHWND(
                        nextInChain,
                        sType,
                        this.Hinstance,
                        this.Hwnd
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SurfaceSourceWindowsHWND> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SurfaceSourceWindowsHWND>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> device relativePointers backend.NextInChain
            Hinstance = backend.Hinstance
            Hwnd = backend.Hwnd
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SurfaceSourceWindowsHWND>) = 
        use ptr = fixed &r
        SurfaceSourceWindowsHWND.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SurfaceSourceWindowsHWND.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SurfaceSourceWindowsHWND>
type SurfaceDescriptorFromXcbWindow = SurfaceSourceXCBWindow
type SurfaceSourceXCBWindow = 
    {
        Next : ISurfaceDescriptorExtension
        Connection : nativeint
        Window : int
    }
    static member Null = Unchecked.defaultof<SurfaceSourceXCBWindow>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SurfaceSourceXCBWindow> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SurfaceSourceXCBWindow
                let mutable value =
                    new WebGPU.Raw.SurfaceSourceXCBWindow(
                        nextInChain,
                        sType,
                        this.Connection,
                        uint32(this.Window)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SurfaceSourceXCBWindow> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SurfaceSourceXCBWindow>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> device relativePointers backend.NextInChain
            Connection = backend.Connection
            Window = int(backend.Window)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SurfaceSourceXCBWindow>) = 
        use ptr = fixed &r
        SurfaceSourceXCBWindow.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SurfaceSourceXCBWindow.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SurfaceSourceXCBWindow>
type SurfaceDescriptorFromXlibWindow = SurfaceSourceXlibWindow
type SurfaceSourceXlibWindow = 
    {
        Next : ISurfaceDescriptorExtension
        Display : nativeint
        Window : int64
    }
    static member Null = Unchecked.defaultof<SurfaceSourceXlibWindow>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SurfaceSourceXlibWindow> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SurfaceSourceXlibWindow
                let mutable value =
                    new WebGPU.Raw.SurfaceSourceXlibWindow(
                        nextInChain,
                        sType,
                        this.Display,
                        uint64(this.Window)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SurfaceSourceXlibWindow> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SurfaceSourceXlibWindow>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> device relativePointers backend.NextInChain
            Display = backend.Display
            Window = int64(backend.Window)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SurfaceSourceXlibWindow>) = 
        use ptr = fixed &r
        SurfaceSourceXlibWindow.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SurfaceSourceXlibWindow.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SurfaceSourceXlibWindow>
type SurfaceDescriptorFromWaylandSurface = SurfaceSourceWaylandSurface
type SurfaceSourceWaylandSurface = 
    {
        Next : ISurfaceDescriptorExtension
        Display : nativeint
        Surface : nativeint
    }
    static member Null = Unchecked.defaultof<SurfaceSourceWaylandSurface>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SurfaceSourceWaylandSurface> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SurfaceSourceWaylandSurface
                let mutable value =
                    new WebGPU.Raw.SurfaceSourceWaylandSurface(
                        nextInChain,
                        sType,
                        this.Display,
                        this.Surface
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SurfaceSourceWaylandSurface> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SurfaceSourceWaylandSurface>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> device relativePointers backend.NextInChain
            Display = backend.Display
            Surface = backend.Surface
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SurfaceSourceWaylandSurface>) = 
        use ptr = fixed &r
        SurfaceSourceWaylandSurface.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SurfaceSourceWaylandSurface.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SurfaceSourceWaylandSurface>
type SurfaceDescriptorFromWindowsCoreWindow = 
    {
        Next : ISurfaceDescriptorExtension
        CoreWindow : nativeint
    }
    static member Null = Unchecked.defaultof<SurfaceDescriptorFromWindowsCoreWindow>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SurfaceDescriptorFromWindowsCoreWindow> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SurfaceDescriptorFromWindowsCoreWindow
                let mutable value =
                    new WebGPU.Raw.SurfaceDescriptorFromWindowsCoreWindow(
                        nextInChain,
                        sType,
                        this.CoreWindow
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SurfaceDescriptorFromWindowsCoreWindow> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SurfaceDescriptorFromWindowsCoreWindow>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> device relativePointers backend.NextInChain
            CoreWindow = backend.CoreWindow
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SurfaceDescriptorFromWindowsCoreWindow>) = 
        use ptr = fixed &r
        SurfaceDescriptorFromWindowsCoreWindow.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SurfaceDescriptorFromWindowsCoreWindow.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SurfaceDescriptorFromWindowsCoreWindow>
type SurfaceDescriptorFromWindowsUWPSwapChainPanel = 
    {
        Next : ISurfaceDescriptorExtension
        SwapChainPanel : nativeint
    }
    static member Null = Unchecked.defaultof<SurfaceDescriptorFromWindowsUWPSwapChainPanel>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SurfaceDescriptorFromWindowsUWPSwapChainPanel> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SurfaceDescriptorFromWindowsUWPSwapChainPanel
                let mutable value =
                    new WebGPU.Raw.SurfaceDescriptorFromWindowsUWPSwapChainPanel(
                        nextInChain,
                        sType,
                        this.SwapChainPanel
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SurfaceDescriptorFromWindowsUWPSwapChainPanel> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SurfaceDescriptorFromWindowsUWPSwapChainPanel>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> device relativePointers backend.NextInChain
            SwapChainPanel = backend.SwapChainPanel
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SurfaceDescriptorFromWindowsUWPSwapChainPanel>) = 
        use ptr = fixed &r
        SurfaceDescriptorFromWindowsUWPSwapChainPanel.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SurfaceDescriptorFromWindowsUWPSwapChainPanel.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SurfaceDescriptorFromWindowsUWPSwapChainPanel>
type SurfaceDescriptorFromWindowsWinUISwapChainPanel = 
    {
        Next : ISurfaceDescriptorExtension
        SwapChainPanel : nativeint
    }
    static member Null = Unchecked.defaultof<SurfaceDescriptorFromWindowsWinUISwapChainPanel>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SurfaceDescriptorFromWindowsWinUISwapChainPanel> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SurfaceDescriptorFromWindowsWinUISwapChainPanel
                let mutable value =
                    new WebGPU.Raw.SurfaceDescriptorFromWindowsWinUISwapChainPanel(
                        nextInChain,
                        sType,
                        this.SwapChainPanel
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SurfaceDescriptorFromWindowsWinUISwapChainPanel> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SurfaceDescriptorFromWindowsWinUISwapChainPanel>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> device relativePointers backend.NextInChain
            SwapChainPanel = backend.SwapChainPanel
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SurfaceDescriptorFromWindowsWinUISwapChainPanel>) = 
        use ptr = fixed &r
        SurfaceDescriptorFromWindowsWinUISwapChainPanel.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SurfaceDescriptorFromWindowsWinUISwapChainPanel.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SurfaceDescriptorFromWindowsWinUISwapChainPanel>
type SurfaceColorManagement = 
    {
        Next : ISurfaceDescriptorExtension
        ColorSpace : PredefinedColorSpace
        ToneMappingMode : ToneMappingMode
    }
    static member Null = Unchecked.defaultof<SurfaceColorManagement>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SurfaceColorManagement> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SurfaceColorManagement
                let mutable value =
                    new WebGPU.Raw.SurfaceColorManagement(
                        nextInChain,
                        sType,
                        this.ColorSpace,
                        this.ToneMappingMode
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SurfaceColorManagement> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SurfaceColorManagement>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> device relativePointers backend.NextInChain
            ColorSpace = backend.ColorSpace
            ToneMappingMode = backend.ToneMappingMode
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SurfaceColorManagement>) = 
        use ptr = fixed &r
        SurfaceColorManagement.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SurfaceColorManagement.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SurfaceColorManagement>
type SurfaceTexture = 
    {
        Texture : Texture
        Status : SurfaceGetCurrentTextureStatus
    }
    static member Null = Unchecked.defaultof<SurfaceTexture>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SurfaceTexture> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let mutable value =
                new WebGPU.Raw.SurfaceTexture(
                    nextInChain,
                    this.Texture.Handle,
                    this.Status
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SurfaceTexture> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SurfaceTexture>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Texture = new Texture(device, backend.Texture)
            Status = backend.Status
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SurfaceTexture>) = 
        use ptr = fixed &r
        SurfaceTexture.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SurfaceTexture.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SurfaceTexture>
type Texture internal(device : Device, handle : nativeint) =
    static let nullptr = new Texture(Unchecked.defaultof<_>, Unchecked.defaultof<_>)
    let width =
        lazy (
            let relativePointers = false
            let mutable res = WebGPU.Raw.WebGPU.TextureGetWidth(handle)
            int(res)
        )
    let height =
        lazy (
            let relativePointers = false
            let mutable res = WebGPU.Raw.WebGPU.TextureGetHeight(handle)
            int(res)
        )
    let depthOrArrayLayers =
        lazy (
            let relativePointers = false
            let mutable res = WebGPU.Raw.WebGPU.TextureGetDepthOrArrayLayers(handle)
            int(res)
        )
    let mipLevelCount =
        lazy (
            let relativePointers = false
            let mutable res = WebGPU.Raw.WebGPU.TextureGetMipLevelCount(handle)
            int(res)
        )
    let sampleCount =
        lazy (
            let relativePointers = false
            let mutable res = WebGPU.Raw.WebGPU.TextureGetSampleCount(handle)
            int(res)
        )
    let dimension =
        lazy (
            let relativePointers = false
            let mutable res = WebGPU.Raw.WebGPU.TextureGetDimension(handle)
            res
        )
    let format =
        lazy (
            let relativePointers = false
            let mutable res = WebGPU.Raw.WebGPU.TextureGetFormat(handle)
            res
        )
    let usage =
        lazy (
            let relativePointers = false
            let mutable res = WebGPU.Raw.WebGPU.TextureGetUsage(handle)
            res
        )
    member x.Handle = handle
    member x.Device = device
    override x.ToString() = $"Texture(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? Texture as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    interface Aardvark.Rendering.IBackendTexture with
        member x.Name
            with get() = null
            and set _ = ()
        member x.WantMipMaps = x.MipLevelCount > 1
        member x.Runtime = x.Device.Runtime
        member x.Dimension = Unchecked.defaultof<_>
        member x.Format = Unchecked.defaultof<_>
        member x.Samples = x.SampleCount
        member x.Count = x.DepthOrArrayLayers
        member x.MipMapLevels = x.MipLevelCount
        member x.Size = Aardvark.Base.V3i(x.Width, x.Height, x.DepthOrArrayLayers)
        member x.Handle = uint64 handle
    member this.CreateView(descriptor : TextureViewDescriptor) : TextureView =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.TextureCreateView(handle, _descriptorPtr)
            new TextureView(res)
        )
    member this.CreateErrorView(descriptor : TextureViewDescriptor) : TextureView =
        let relativePointers = false
        descriptor.Pin(device, fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.TextureCreateErrorView(handle, _descriptorPtr)
            new TextureView(res)
        )
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.TextureSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Width : int =
        width.Value
    member this.Height : int =
        height.Value
    member this.DepthOrArrayLayers : int =
        depthOrArrayLayers.Value
    member this.MipLevelCount : int =
        mipLevelCount.Value
    member this.SampleCount : int =
        sampleCount.Value
    member this.Dimension : TextureDimension =
        dimension.Value
    member this.Format : TextureFormat =
        format.Value
    member this.Usage : TextureUsage =
        usage.Value
    member this.Destroy() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.TextureDestroy(handle)
        res
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.TextureRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.TextureAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type TextureDescriptor = 
    {
        Next : ITextureDescriptorExtension
        Label : string
        Usage : TextureUsage
        Dimension : TextureDimension
        Size : Extent3D
        Format : TextureFormat
        MipLevelCount : int
        SampleCount : int
        ViewFormats : array<TextureFormat>
    }
    static member Null = Unchecked.defaultof<TextureDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.TextureDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                try
                    let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                    this.Size.Pin(device, fun _sizePtr ->
                        use viewFormatsPtr = fixed (this.ViewFormats)
                        try
                            let viewFormatsLen = unativeint this.ViewFormats.Length
                            let mutable value =
                                new WebGPU.Raw.TextureDescriptor(
                                    nextInChain,
                                    _labelLen,
                                    this.Usage,
                                    this.Dimension,
                                    (if NativePtr.toNativeInt _sizePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _sizePtr),
                                    this.Format,
                                    uint32(this.MipLevelCount),
                                    uint32(this.SampleCount),
                                    viewFormatsLen,
                                    viewFormatsPtr
                                )
                            use ptr = fixed &value
                            try action ptr
                            finally ()
                        finally
                            ()
                    )
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.TextureDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.TextureDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
            if NativePtr.toNativeInt backend.ViewFormats <> 0n then
                backend.ViewFormats <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.ViewFormats)
        {
            Next = ExtensionDecoder.decode<ITextureDescriptorExtension> device relativePointers backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            Usage = backend.Usage
            Dimension = backend.Dimension
            Size = use pppp = fixed &backend.Size in Extent3D.Read(device, pppp, relativePointers)
            Format = backend.Format
            MipLevelCount = int(backend.MipLevelCount)
            SampleCount = int(backend.SampleCount)
            ViewFormats = let ptr = backend.ViewFormats in Array.init (int backend.ViewFormatCount) (fun i -> NativePtr.get ptr i)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.TextureDescriptor>) = 
        use ptr = fixed &r
        TextureDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        TextureDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.TextureDescriptor>
type TextureBindingViewDimensionDescriptor = 
    {
        Next : ITextureDescriptorExtension
        TextureBindingViewDimension : TextureViewDimension
    }
    static member Null = Unchecked.defaultof<TextureBindingViewDimensionDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.TextureBindingViewDimensionDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.TextureBindingViewDimensionDescriptor
                let mutable value =
                    new WebGPU.Raw.TextureBindingViewDimensionDescriptor(
                        nextInChain,
                        sType,
                        this.TextureBindingViewDimension
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ITextureDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.TextureBindingViewDimensionDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.TextureBindingViewDimensionDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ITextureDescriptorExtension> device relativePointers backend.NextInChain
            TextureBindingViewDimension = backend.TextureBindingViewDimension
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.TextureBindingViewDimensionDescriptor>) = 
        use ptr = fixed &r
        TextureBindingViewDimensionDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        TextureBindingViewDimensionDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.TextureBindingViewDimensionDescriptor>
type TextureViewDescriptor = 
    {
        Next : ITextureViewDescriptorExtension
        Label : string
        Format : TextureFormat
        Dimension : TextureViewDimension
        BaseMipLevel : int
        MipLevelCount : int
        BaseArrayLayer : int
        ArrayLayerCount : int
        Aspect : TextureAspect
        Usage : TextureUsage
    }
    static member Null = Unchecked.defaultof<TextureViewDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.TextureViewDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                try
                    let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                    let mutable value =
                        new WebGPU.Raw.TextureViewDescriptor(
                            nextInChain,
                            _labelLen,
                            this.Format,
                            this.Dimension,
                            uint32(this.BaseMipLevel),
                            uint32(this.MipLevelCount),
                            uint32(this.BaseArrayLayer),
                            uint32(this.ArrayLayerCount),
                            this.Aspect,
                            this.Usage
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                finally
                    ()
            )
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.TextureViewDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.TextureViewDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
        {
            Next = ExtensionDecoder.decode<ITextureViewDescriptorExtension> device relativePointers backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            Format = backend.Format
            Dimension = backend.Dimension
            BaseMipLevel = int(backend.BaseMipLevel)
            MipLevelCount = int(backend.MipLevelCount)
            BaseArrayLayer = int(backend.BaseArrayLayer)
            ArrayLayerCount = int(backend.ArrayLayerCount)
            Aspect = backend.Aspect
            Usage = backend.Usage
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.TextureViewDescriptor>) = 
        use ptr = fixed &r
        TextureViewDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        TextureViewDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.TextureViewDescriptor>
type TexelBufferViewDescriptor = 
    {
        Label : string
        Format : TextureFormat
        Offset : int64
        Size : int64
    }
    static member Null = Unchecked.defaultof<TexelBufferViewDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.TexelBufferViewDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let nextInChain = 0n
            let _labelArr = if isNull this.Label then null else Encoding.UTF8.GetBytes(this.Label)
            use _labelPtr = fixed _labelArr
            try
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
                let mutable value =
                    new WebGPU.Raw.TexelBufferViewDescriptor(
                        nextInChain,
                        _labelLen,
                        this.Format,
                        uint64(this.Offset),
                        uint64(this.Size)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            finally
                ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.TexelBufferViewDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.TexelBufferViewDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Label.Data <> 0n then
                let offset = NativePtr.toNativeInt &&backend.Label - NativePtr.toNativeInt &&backend
                backend.Label.Data <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + offset + NativePtr.toNativeInt backend.Label.Data)
        {
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            Format = backend.Format
            Offset = int64(backend.Offset)
            Size = int64(backend.Size)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.TexelBufferViewDescriptor>) = 
        use ptr = fixed &r
        TexelBufferViewDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        TexelBufferViewDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.TexelBufferViewDescriptor>
type TextureComponentSwizzleDescriptor = 
    {
        Next : ITextureViewDescriptorExtension
        Swizzle : TextureComponentSwizzle
    }
    static member Null = Unchecked.defaultof<TextureComponentSwizzleDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.TextureComponentSwizzleDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.TextureComponentSwizzleDescriptor
                this.Swizzle.Pin(device, fun _swizzlePtr ->
                    let mutable value =
                        new WebGPU.Raw.TextureComponentSwizzleDescriptor(
                            nextInChain,
                            sType,
                            (if NativePtr.toNativeInt _swizzlePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _swizzlePtr)
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                )
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ITextureViewDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.TextureComponentSwizzleDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.TextureComponentSwizzleDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ITextureViewDescriptorExtension> device relativePointers backend.NextInChain
            Swizzle = use pppp = fixed &backend.Swizzle in TextureComponentSwizzle.Read(device, pppp, relativePointers)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.TextureComponentSwizzleDescriptor>) = 
        use ptr = fixed &r
        TextureComponentSwizzleDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        TextureComponentSwizzleDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.TextureComponentSwizzleDescriptor>
type TextureView internal(handle : nativeint) =
    static let device = Unchecked.defaultof<Device>
    static let nullptr = new TextureView(Unchecked.defaultof<_>)
    member x.Handle = handle
    override x.ToString() = $"TextureView(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? TextureView as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.TextureViewSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.TextureViewRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.TextureViewAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type TexelBufferView internal(handle : nativeint) =
    static let device = Unchecked.defaultof<Device>
    static let nullptr = new TexelBufferView(Unchecked.defaultof<_>)
    member x.Handle = handle
    override x.ToString() = $"TexelBufferView(0x%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? TexelBufferView as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = nullptr
    member this.SetLabel(label : string) : unit =
        let relativePointers = false
        let _labelArr = if isNull label then null else Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        try
            let _labelLen = WebGPU.Raw.StringView(_labelPtr, if isNull _labelArr then 0un else unativeint _labelArr.Length)
            let res = WebGPU.Raw.WebGPU.TexelBufferViewSetLabel(handle, _labelLen)
            res
        finally
            ()
    member this.Release() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.TexelBufferViewRelease(handle)
        res
    member this.AddRef() : unit =
        let relativePointers = false
        let res = WebGPU.Raw.WebGPU.TexelBufferViewAddRef(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type TextureComponentSwizzle = 
    {
        R : ComponentSwizzle
        G : ComponentSwizzle
        B : ComponentSwizzle
        A : ComponentSwizzle
    }
    static member Null = Unchecked.defaultof<TextureComponentSwizzle>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.TextureComponentSwizzle> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.TextureComponentSwizzle(
                    this.R,
                    this.G,
                    this.B,
                    this.A
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.TextureComponentSwizzle> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.TextureComponentSwizzle>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            R = backend.R
            G = backend.G
            B = backend.B
            A = backend.A
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.TextureComponentSwizzle>) = 
        use ptr = fixed &r
        TextureComponentSwizzle.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        TextureComponentSwizzle.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.TextureComponentSwizzle>
type YCbCrVkDescriptor = 
    {
        Next : ISamplerDescriptorExtension
        VkFormat : int
        VkYCbCrModel : int
        VkYCbCrRange : int
        VkComponentSwizzleRed : int
        VkComponentSwizzleGreen : int
        VkComponentSwizzleBlue : int
        VkComponentSwizzleAlpha : int
        VkXChromaOffset : int
        VkYChromaOffset : int
        VkChromaFilter : FilterMode
        ForceExplicitReconstruction : bool
        ExternalFormat : int64
    }
    static member Null = Unchecked.defaultof<YCbCrVkDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.YCbCrVkDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.YCbCrVkDescriptor
                let mutable value =
                    new WebGPU.Raw.YCbCrVkDescriptor(
                        nextInChain,
                        sType,
                        uint32(this.VkFormat),
                        uint32(this.VkYCbCrModel),
                        uint32(this.VkYCbCrRange),
                        uint32(this.VkComponentSwizzleRed),
                        uint32(this.VkComponentSwizzleGreen),
                        uint32(this.VkComponentSwizzleBlue),
                        uint32(this.VkComponentSwizzleAlpha),
                        uint32(this.VkXChromaOffset),
                        uint32(this.VkYChromaOffset),
                        this.VkChromaFilter,
                        (if this.ForceExplicitReconstruction then 1 else 0),
                        uint64(this.ExternalFormat)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISamplerDescriptorExtension
    interface ITextureViewDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.YCbCrVkDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.YCbCrVkDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ISamplerDescriptorExtension> device relativePointers backend.NextInChain
            VkFormat = int(backend.VkFormat)
            VkYCbCrModel = int(backend.VkYCbCrModel)
            VkYCbCrRange = int(backend.VkYCbCrRange)
            VkComponentSwizzleRed = int(backend.VkComponentSwizzleRed)
            VkComponentSwizzleGreen = int(backend.VkComponentSwizzleGreen)
            VkComponentSwizzleBlue = int(backend.VkComponentSwizzleBlue)
            VkComponentSwizzleAlpha = int(backend.VkComponentSwizzleAlpha)
            VkXChromaOffset = int(backend.VkXChromaOffset)
            VkYChromaOffset = int(backend.VkYChromaOffset)
            VkChromaFilter = backend.VkChromaFilter
            ForceExplicitReconstruction = (backend.ForceExplicitReconstruction <> 0)
            ExternalFormat = int64(backend.ExternalFormat)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.YCbCrVkDescriptor>) = 
        use ptr = fixed &r
        YCbCrVkDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        YCbCrVkDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.YCbCrVkDescriptor>
type DawnTextureInternalUsageDescriptor = 
    {
        Next : ITextureDescriptorExtension
        InternalUsage : TextureUsage
    }
    static member Null = Unchecked.defaultof<DawnTextureInternalUsageDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnTextureInternalUsageDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnTextureInternalUsageDescriptor
                let mutable value =
                    new WebGPU.Raw.DawnTextureInternalUsageDescriptor(
                        nextInChain,
                        sType,
                        this.InternalUsage
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ITextureDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnTextureInternalUsageDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnTextureInternalUsageDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ITextureDescriptorExtension> device relativePointers backend.NextInChain
            InternalUsage = backend.InternalUsage
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnTextureInternalUsageDescriptor>) = 
        use ptr = fixed &r
        DawnTextureInternalUsageDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnTextureInternalUsageDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnTextureInternalUsageDescriptor>
type DawnEncoderInternalUsageDescriptor = 
    {
        Next : ICommandEncoderDescriptorExtension
        UseInternalUsages : bool
    }
    static member Null = Unchecked.defaultof<DawnEncoderInternalUsageDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnEncoderInternalUsageDescriptor> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnEncoderInternalUsageDescriptor
                let mutable value =
                    new WebGPU.Raw.DawnEncoderInternalUsageDescriptor(
                        nextInChain,
                        sType,
                        (if this.UseInternalUsages then 1 else 0)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ICommandEncoderDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnEncoderInternalUsageDescriptor> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnEncoderInternalUsageDescriptor>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<ICommandEncoderDescriptorExtension> device relativePointers backend.NextInChain
            UseInternalUsages = (backend.UseInternalUsages <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnEncoderInternalUsageDescriptor>) = 
        use ptr = fixed &r
        DawnEncoderInternalUsageDescriptor.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnEncoderInternalUsageDescriptor.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnEncoderInternalUsageDescriptor>
type DawnAdapterPropertiesPowerPreference = 
    {
        Next : IAdapterInfoExtension
        PowerPreference : PowerPreference
    }
    static member Null = Unchecked.defaultof<DawnAdapterPropertiesPowerPreference>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnAdapterPropertiesPowerPreference> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnAdapterPropertiesPowerPreference
                let mutable value =
                    new WebGPU.Raw.DawnAdapterPropertiesPowerPreference(
                        nextInChain,
                        sType,
                        this.PowerPreference
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IAdapterInfoExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnAdapterPropertiesPowerPreference> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnAdapterPropertiesPowerPreference>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IAdapterInfoExtension> device relativePointers backend.NextInChain
            PowerPreference = backend.PowerPreference
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnAdapterPropertiesPowerPreference>) = 
        use ptr = fixed &r
        DawnAdapterPropertiesPowerPreference.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnAdapterPropertiesPowerPreference.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnAdapterPropertiesPowerPreference>
type MemoryHeapInfo = 
    {
        Properties : HeapProperty
        Size : int64
    }
    static member Null = Unchecked.defaultof<MemoryHeapInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.MemoryHeapInfo> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.MemoryHeapInfo(
                    this.Properties,
                    uint64(this.Size)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.MemoryHeapInfo> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.MemoryHeapInfo>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            Properties = backend.Properties
            Size = int64(backend.Size)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.MemoryHeapInfo>) = 
        use ptr = fixed &r
        MemoryHeapInfo.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        MemoryHeapInfo.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.MemoryHeapInfo>
type AdapterPropertiesMemoryHeaps = 
    {
        Next : IAdapterInfoExtension
        HeapInfo : array<MemoryHeapInfo>
    }
    static member Null = Unchecked.defaultof<AdapterPropertiesMemoryHeaps>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.AdapterPropertiesMemoryHeaps> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.AdapterPropertiesMemoryHeaps
                WebGPU.Raw.Pinnable.pinArray device this.HeapInfo (fun heapInfoPtr ->
                    let heapInfoLen = unativeint this.HeapInfo.Length
                    let mutable value =
                        new WebGPU.Raw.AdapterPropertiesMemoryHeaps(
                            nextInChain,
                            sType,
                            heapInfoLen,
                            heapInfoPtr
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                )
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IAdapterInfoExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.AdapterPropertiesMemoryHeaps> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.AdapterPropertiesMemoryHeaps>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.HeapInfo <> 0n then
                backend.HeapInfo <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.HeapInfo)
        {
            Next = ExtensionDecoder.decode<IAdapterInfoExtension> device relativePointers backend.NextInChain
            HeapInfo = let ptr = backend.HeapInfo in Array.init (int backend.HeapCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in MemoryHeapInfo.Read(device, NativePtr.add ptr i, relativePointers))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.AdapterPropertiesMemoryHeaps>) = 
        use ptr = fixed &r
        AdapterPropertiesMemoryHeaps.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        AdapterPropertiesMemoryHeaps.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.AdapterPropertiesMemoryHeaps>
type AdapterPropertiesD3D = 
    {
        Next : IAdapterInfoExtension
        ShaderModel : int
    }
    static member Null = Unchecked.defaultof<AdapterPropertiesD3D>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.AdapterPropertiesD3D> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.AdapterPropertiesD3D
                let mutable value =
                    new WebGPU.Raw.AdapterPropertiesD3D(
                        nextInChain,
                        sType,
                        uint32(this.ShaderModel)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IAdapterInfoExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.AdapterPropertiesD3D> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.AdapterPropertiesD3D>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IAdapterInfoExtension> device relativePointers backend.NextInChain
            ShaderModel = int(backend.ShaderModel)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.AdapterPropertiesD3D>) = 
        use ptr = fixed &r
        AdapterPropertiesD3D.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        AdapterPropertiesD3D.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.AdapterPropertiesD3D>
type AdapterPropertiesVk = 
    {
        Next : IAdapterInfoExtension
        DriverVersion : int
    }
    static member Null = Unchecked.defaultof<AdapterPropertiesVk>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.AdapterPropertiesVk> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.AdapterPropertiesVk
                let mutable value =
                    new WebGPU.Raw.AdapterPropertiesVk(
                        nextInChain,
                        sType,
                        uint32(this.DriverVersion)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IAdapterInfoExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.AdapterPropertiesVk> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.AdapterPropertiesVk>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IAdapterInfoExtension> device relativePointers backend.NextInChain
            DriverVersion = int(backend.DriverVersion)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.AdapterPropertiesVk>) = 
        use ptr = fixed &r
        AdapterPropertiesVk.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        AdapterPropertiesVk.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.AdapterPropertiesVk>
type DawnBufferDescriptorErrorInfoFromWireClient = 
    {
        Next : IBufferDescriptorExtension
        OutOfMemory : bool
    }
    static member Null = Unchecked.defaultof<DawnBufferDescriptorErrorInfoFromWireClient>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.DawnBufferDescriptorErrorInfoFromWireClient> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnBufferDescriptorErrorInfoFromWireClient
                let mutable value =
                    new WebGPU.Raw.DawnBufferDescriptorErrorInfoFromWireClient(
                        nextInChain,
                        sType,
                        (if this.OutOfMemory then 1 else 0)
                    )
                use ptr = fixed &value
                try action ptr
                finally ()
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IBufferDescriptorExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.DawnBufferDescriptorErrorInfoFromWireClient> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.DawnBufferDescriptorErrorInfoFromWireClient>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
        {
            Next = ExtensionDecoder.decode<IBufferDescriptorExtension> device relativePointers backend.NextInChain
            OutOfMemory = (backend.OutOfMemory <> 0)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.DawnBufferDescriptorErrorInfoFromWireClient>) = 
        use ptr = fixed &r
        DawnBufferDescriptorErrorInfoFromWireClient.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        DawnBufferDescriptorErrorInfoFromWireClient.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.DawnBufferDescriptorErrorInfoFromWireClient>
type SubgroupMatrixConfig = 
    {
        ComponentType : SubgroupMatrixComponentType
        ResultComponentType : SubgroupMatrixComponentType
        M : int
        N : int
        K : int
    }
    static member Null = Unchecked.defaultof<SubgroupMatrixConfig>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.SubgroupMatrixConfig> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.SubgroupMatrixConfig(
                    this.ComponentType,
                    this.ResultComponentType,
                    uint32(this.M),
                    uint32(this.N),
                    uint32(this.K)
                )
            use ptr = fixed &value
            try action ptr
            finally ()
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.SubgroupMatrixConfig> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.SubgroupMatrixConfig>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        {
            ComponentType = backend.ComponentType
            ResultComponentType = backend.ResultComponentType
            M = int(backend.M)
            N = int(backend.N)
            K = int(backend.K)
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.SubgroupMatrixConfig>) = 
        use ptr = fixed &r
        SubgroupMatrixConfig.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        SubgroupMatrixConfig.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.SubgroupMatrixConfig>
type AdapterPropertiesSubgroupMatrixConfigs = 
    {
        Next : IAdapterInfoExtension
        Configs : array<SubgroupMatrixConfig>
    }
    static member Null = Unchecked.defaultof<AdapterPropertiesSubgroupMatrixConfigs>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(device : Device, action : nativeptr<WebGPU.Raw.AdapterPropertiesSubgroupMatrixConfigs> -> 'r) : 'r = 
        let relativePointers = false
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.AdapterPropertiesSubgroupMatrixConfigs
                WebGPU.Raw.Pinnable.pinArray device this.Configs (fun configsPtr ->
                    let configsLen = unativeint this.Configs.Length
                    let mutable value =
                        new WebGPU.Raw.AdapterPropertiesSubgroupMatrixConfigs(
                            nextInChain,
                            sType,
                            configsLen,
                            configsPtr
                        )
                    use ptr = fixed &value
                    try action ptr
                    finally ()
                )
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(Unchecked.defaultof<_>, fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IAdapterInfoExtension
    interface WebGPU.Raw.IPinnable<Device, WebGPU.Raw.AdapterPropertiesSubgroupMatrixConfigs> with
        member x.Pin(device, action) = x.Pin(device, action)
    member x.CopyTo(dst : nativeint, aux : byref<nativeint>) =
        let mutable a = aux
        try
           x.Pin(Unchecked.defaultof<_>, fun src ->
               (NativePtr.read src).CopyTo(dst, &a)
           )
        finally
            aux <- a
    static member Read(device : Device, ptr : nativeptr<WebGPU.Raw.AdapterPropertiesSubgroupMatrixConfigs>, relativePointers : bool) = 
        let mutable backend = NativePtr.read ptr
        if relativePointers then
            if backend.NextInChain <> 0n then
                backend.NextInChain <- NativePtr.toNativeInt ptr + backend.NextInChain
            if NativePtr.toNativeInt backend.Configs <> 0n then
                backend.Configs <- NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + NativePtr.toNativeInt backend.Configs)
        {
            Next = ExtensionDecoder.decode<IAdapterInfoExtension> device relativePointers backend.NextInChain
            Configs = let ptr = backend.Configs in Array.init (int backend.ConfigCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in SubgroupMatrixConfig.Read(device, NativePtr.add ptr i, relativePointers))
        }
    static member Read(device : Device, r : inref<WebGPU.Raw.AdapterPropertiesSubgroupMatrixConfigs>) = 
        use ptr = fixed &r
        AdapterPropertiesSubgroupMatrixConfigs.Read(device, ptr, false)
    static member Read(device : Device, ptr : nativeint, ?relativePointers : bool) = 
        AdapterPropertiesSubgroupMatrixConfigs.Read(device, NativePtr.ofNativeInt ptr, defaultArg relativePointers true)
    static member SizeInBytes = nativeint sizeof<WebGPU.Raw.AdapterPropertiesSubgroupMatrixConfigs>
type BufferProxy(buffer : Buffer) =
    let size = buffer.Size
    
    let content = buffer.ToByteArray(0L, buffer.Size)
    member x.UInt8Array = content
    member x.UInt16Array = 
        use ptr = fixed content
        let ptr = NativePtr.ofNativeInt<uint16> (NativePtr.toNativeInt ptr)
        try Array.init (content.Length / 2) (fun i -> NativePtr.get ptr i) finally ()
    member x.UInt32Array = 
        use ptr = fixed content
        let ptr = NativePtr.ofNativeInt<uint32> (NativePtr.toNativeInt ptr)
        try Array.init (content.Length / 4) (fun i -> NativePtr.get ptr i) finally ()
    member x.UInt64Array = 
        use ptr = fixed content
        let ptr = NativePtr.ofNativeInt<uint64> (NativePtr.toNativeInt ptr)
        try Array.init (content.Length / 8) (fun i -> NativePtr.get ptr i) finally ()
    member x.Int8Array = 
        use ptr = fixed content
        let ptr = NativePtr.ofNativeInt<int8> (NativePtr.toNativeInt ptr)
        try Array.init (content.Length) (fun i -> NativePtr.get ptr i) finally ()
    member x.Int16Array = 
        use ptr = fixed content
        let ptr = NativePtr.ofNativeInt<int16> (NativePtr.toNativeInt ptr)
        try Array.init (content.Length / 2) (fun i -> NativePtr.get ptr i) finally ()
    member x.Int32Array = 
        use ptr = fixed content
        let ptr = NativePtr.ofNativeInt<int32> (NativePtr.toNativeInt ptr)
        try Array.init (content.Length / 4) (fun i -> NativePtr.get ptr i) finally ()
    member x.Int64Array = 
        use ptr = fixed content
        let ptr = NativePtr.ofNativeInt<int64> (NativePtr.toNativeInt ptr)
        try Array.init (content.Length / 8) (fun i -> NativePtr.get ptr i) finally ()
    member x.Float32Array = 
        use ptr = fixed content
        let ptr = NativePtr.ofNativeInt<float32> (NativePtr.toNativeInt ptr)
        try Array.init (content.Length / 4) (fun i -> NativePtr.get ptr i) finally ()
    member x.Float64Array = 
        use ptr = fixed content
        let ptr = NativePtr.ofNativeInt<double> (NativePtr.toNativeInt ptr)
        try Array.init (content.Length / 8) (fun i -> NativePtr.get ptr i) finally ()
