namespace rec WebGPU
open System
open System.Text
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop
#nowarn "9"
#nowarn "26"
#nowarn "1182"
[<AllowNullLiteral>]
type IExtension =
    abstract member Pin<'r> : action : (nativeint -> 'r) -> 'r
[<AllowNullLiteral>]
type IAdapterInfoExtension = inherit IExtension
[<AllowNullLiteral>]
type IBindGroupEntryExtension = inherit IExtension
[<AllowNullLiteral>]
type IBindGroupLayoutEntryExtension = inherit IExtension
[<AllowNullLiteral>]
type IBufferDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type IColorTargetStateExtension = inherit IExtension
[<AllowNullLiteral>]
type ICommandEncoderDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type IComputePipelineDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type IDeviceDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type IFormatCapabilitiesExtension = inherit IExtension
[<AllowNullLiteral>]
type IInstanceDescriptorExtension = inherit IExtension
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
type ISupportedLimitsExtension = inherit IExtension
[<AllowNullLiteral>]
type ISurfaceDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type ITextureDescriptorExtension = inherit IExtension
[<AllowNullLiteral>]
type ITextureViewDescriptorExtension = inherit IExtension
module private ExtensionDecoder =
    let decode<'a when 'a :> IExtension> (ptr : nativeint) : 'a =
        if ptr = 0n then
            Unchecked.defaultof<'a>
        else
            let sType = NativePtr.read (NativePtr.ofNativeInt<SType> (ptr + nativeint sizeof<nativeint>))
            if typeof<'a> = typeof<IAdapterInfoExtension> then
                match sType with
                | SType.DawnAdapterPropertiesPowerPreference ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.DawnAdapterPropertiesPowerPreference> (ptr))
                    DawnAdapterPropertiesPowerPreference.Read(&rr) :> obj :?> 'a
                | SType.AdapterPropertiesMemoryHeaps ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.AdapterPropertiesMemoryHeaps> (ptr))
                    AdapterPropertiesMemoryHeaps.Read(&rr) :> obj :?> 'a
                | SType.AdapterPropertiesD3D ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.AdapterPropertiesD3D> (ptr))
                    AdapterPropertiesD3D.Read(&rr) :> obj :?> 'a
                | SType.AdapterPropertiesVk ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.AdapterPropertiesVk> (ptr))
                    AdapterPropertiesVk.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IBindGroupEntryExtension> then
                match sType with
                | SType.ExternalTextureBindingEntry ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.ExternalTextureBindingEntry> (ptr))
                    ExternalTextureBindingEntry.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IBindGroupLayoutEntryExtension> then
                match sType with
                | SType.StaticSamplerBindingLayout ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.StaticSamplerBindingLayout> (ptr))
                    StaticSamplerBindingLayout.Read(&rr) :> obj :?> 'a
                | SType.ExternalTextureBindingLayout ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.ExternalTextureBindingLayout> (ptr))
                    ExternalTextureBindingLayout.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IBufferDescriptorExtension> then
                match sType with
                | SType.BufferHostMappedPointer ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.BufferHostMappedPointer> (ptr))
                    BufferHostMappedPointer.Read(&rr) :> obj :?> 'a
                | SType.DawnBufferDescriptorErrorInfoFromWireClient ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.DawnBufferDescriptorErrorInfoFromWireClient> (ptr))
                    DawnBufferDescriptorErrorInfoFromWireClient.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IColorTargetStateExtension> then
                match sType with
                | SType.ColorTargetStateExpandResolveTextureDawn ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.ColorTargetStateExpandResolveTextureDawn> (ptr))
                    ColorTargetStateExpandResolveTextureDawn.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ICommandEncoderDescriptorExtension> then
                match sType with
                | SType.DawnEncoderInternalUsageDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.DawnEncoderInternalUsageDescriptor> (ptr))
                    DawnEncoderInternalUsageDescriptor.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IComputePipelineDescriptorExtension> then
                match sType with
                | SType.DawnComputePipelineFullSubgroups ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.DawnComputePipelineFullSubgroups> (ptr))
                    DawnComputePipelineFullSubgroups.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IDeviceDescriptorExtension> then
                match sType with
                | SType.DawnTogglesDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.DawnTogglesDescriptor> (ptr))
                    DawnTogglesDescriptor.Read(&rr) :> obj :?> 'a
                | SType.DawnCacheDeviceDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.DawnCacheDeviceDescriptor> (ptr))
                    DawnCacheDeviceDescriptor.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IFormatCapabilitiesExtension> then
                match sType with
                | SType.DrmFormatCapabilities ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.DrmFormatCapabilities> (ptr))
                    DrmFormatCapabilities.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IInstanceDescriptorExtension> then
                match sType with
                | SType.DawnTogglesDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.DawnTogglesDescriptor> (ptr))
                    DawnTogglesDescriptor.Read(&rr) :> obj :?> 'a
                | SType.DawnWGSLBlocklist ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.DawnWGSLBlocklist> (ptr))
                    DawnWGSLBlocklist.Read(&rr) :> obj :?> 'a
                | SType.DawnWireWGSLControl ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.DawnWireWGSLControl> (ptr))
                    DawnWireWGSLControl.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IPipelineLayoutDescriptorExtension> then
                match sType with
                | SType.PipelineLayoutPixelLocalStorage ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.PipelineLayoutPixelLocalStorage> (ptr))
                    PipelineLayoutPixelLocalStorage.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IRenderPassColorAttachmentExtension> then
                match sType with
                | SType.DawnRenderPassColorAttachmentRenderToSingleSampled ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.DawnRenderPassColorAttachmentRenderToSingleSampled> (ptr))
                    DawnRenderPassColorAttachmentRenderToSingleSampled.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IRenderPassDescriptorExtension> then
                match sType with
                | SType.RenderPassMaxDrawCount ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.RenderPassMaxDrawCount> (ptr))
                    RenderPassMaxDrawCount.Read(&rr) :> obj :?> 'a
                | SType.RenderPassDescriptorExpandResolveRect ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.RenderPassDescriptorExpandResolveRect> (ptr))
                    RenderPassDescriptorExpandResolveRect.Read(&rr) :> obj :?> 'a
                | SType.RenderPassPixelLocalStorage ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.RenderPassPixelLocalStorage> (ptr))
                    RenderPassPixelLocalStorage.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IRequestAdapterOptionsExtension> then
                match sType with
                | SType.DawnTogglesDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.DawnTogglesDescriptor> (ptr))
                    DawnTogglesDescriptor.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ISamplerDescriptorExtension> then
                match sType with
                | SType.YCbCrVkDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.YCbCrVkDescriptor> (ptr))
                    YCbCrVkDescriptor.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IShaderModuleDescriptorExtension> then
                match sType with
                | SType.ShaderSourceSPIRV ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.ShaderSourceSPIRV> (ptr))
                    ShaderSourceSPIRV.Read(&rr) :> obj :?> 'a
                | SType.ShaderSourceWGSL ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.ShaderSourceWGSL> (ptr))
                    ShaderSourceWGSL.Read(&rr) :> obj :?> 'a
                | SType.DawnShaderModuleSPIRVOptionsDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.DawnShaderModuleSPIRVOptionsDescriptor> (ptr))
                    DawnShaderModuleSPIRVOptionsDescriptor.Read(&rr) :> obj :?> 'a
                | SType.ShaderModuleCompilationOptions ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.ShaderModuleCompilationOptions> (ptr))
                    ShaderModuleCompilationOptions.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ISharedFenceDescriptorExtension> then
                match sType with
                | SType.SharedFenceVkSemaphoreOpaqueFDDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDDescriptor> (ptr))
                    SharedFenceVkSemaphoreOpaqueFDDescriptor.Read(&rr) :> obj :?> 'a
                | SType.SharedFenceSyncFDDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceSyncFDDescriptor> (ptr))
                    SharedFenceSyncFDDescriptor.Read(&rr) :> obj :?> 'a
                | SType.SharedFenceVkSemaphoreZirconHandleDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleDescriptor> (ptr))
                    SharedFenceVkSemaphoreZirconHandleDescriptor.Read(&rr) :> obj :?> 'a
                | SType.SharedFenceDXGISharedHandleDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceDXGISharedHandleDescriptor> (ptr))
                    SharedFenceDXGISharedHandleDescriptor.Read(&rr) :> obj :?> 'a
                | SType.SharedFenceMTLSharedEventDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceMTLSharedEventDescriptor> (ptr))
                    SharedFenceMTLSharedEventDescriptor.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ISharedFenceExportInfoExtension> then
                match sType with
                | SType.SharedFenceVkSemaphoreOpaqueFDExportInfo ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDExportInfo> (ptr))
                    SharedFenceVkSemaphoreOpaqueFDExportInfo.Read(&rr) :> obj :?> 'a
                | SType.SharedFenceSyncFDExportInfo ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceSyncFDExportInfo> (ptr))
                    SharedFenceSyncFDExportInfo.Read(&rr) :> obj :?> 'a
                | SType.SharedFenceVkSemaphoreZirconHandleExportInfo ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleExportInfo> (ptr))
                    SharedFenceVkSemaphoreZirconHandleExportInfo.Read(&rr) :> obj :?> 'a
                | SType.SharedFenceDXGISharedHandleExportInfo ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceDXGISharedHandleExportInfo> (ptr))
                    SharedFenceDXGISharedHandleExportInfo.Read(&rr) :> obj :?> 'a
                | SType.SharedFenceMTLSharedEventExportInfo ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedFenceMTLSharedEventExportInfo> (ptr))
                    SharedFenceMTLSharedEventExportInfo.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ISharedTextureMemoryBeginAccessDescriptorExtension> then
                match sType with
                | SType.SharedTextureMemoryVkImageLayoutBeginState ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryVkImageLayoutBeginState> (ptr))
                    SharedTextureMemoryVkImageLayoutBeginState.Read(&rr) :> obj :?> 'a
                | SType.SharedTextureMemoryD3DSwapchainBeginState ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryD3DSwapchainBeginState> (ptr))
                    SharedTextureMemoryD3DSwapchainBeginState.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ISharedTextureMemoryDescriptorExtension> then
                match sType with
                | SType.SharedTextureMemoryVkDedicatedAllocationDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryVkDedicatedAllocationDescriptor> (ptr))
                    SharedTextureMemoryVkDedicatedAllocationDescriptor.Read(&rr) :> obj :?> 'a
                | SType.SharedTextureMemoryAHardwareBufferDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryAHardwareBufferDescriptor> (ptr))
                    SharedTextureMemoryAHardwareBufferDescriptor.Read(&rr) :> obj :?> 'a
                | SType.SharedTextureMemoryDmaBufDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryDmaBufDescriptor> (ptr))
                    SharedTextureMemoryDmaBufDescriptor.Read(&rr) :> obj :?> 'a
                | SType.SharedTextureMemoryOpaqueFDDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryOpaqueFDDescriptor> (ptr))
                    SharedTextureMemoryOpaqueFDDescriptor.Read(&rr) :> obj :?> 'a
                | SType.SharedTextureMemoryZirconHandleDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryZirconHandleDescriptor> (ptr))
                    SharedTextureMemoryZirconHandleDescriptor.Read(&rr) :> obj :?> 'a
                | SType.SharedTextureMemoryDXGISharedHandleDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryDXGISharedHandleDescriptor> (ptr))
                    SharedTextureMemoryDXGISharedHandleDescriptor.Read(&rr) :> obj :?> 'a
                | SType.SharedTextureMemoryIOSurfaceDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryIOSurfaceDescriptor> (ptr))
                    SharedTextureMemoryIOSurfaceDescriptor.Read(&rr) :> obj :?> 'a
                | SType.SharedTextureMemoryEGLImageDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryEGLImageDescriptor> (ptr))
                    SharedTextureMemoryEGLImageDescriptor.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ISharedTextureMemoryEndAccessStateExtension> then
                match sType with
                | SType.SharedTextureMemoryVkImageLayoutEndState ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryVkImageLayoutEndState> (ptr))
                    SharedTextureMemoryVkImageLayoutEndState.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ISharedTextureMemoryPropertiesExtension> then
                match sType with
                | SType.SharedTextureMemoryAHardwareBufferProperties ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SharedTextureMemoryAHardwareBufferProperties> (ptr))
                    SharedTextureMemoryAHardwareBufferProperties.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ISupportedLimitsExtension> then
                match sType with
                | SType.DawnExperimentalSubgroupLimits ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.DawnExperimentalSubgroupLimits> (ptr))
                    DawnExperimentalSubgroupLimits.Read(&rr) :> obj :?> 'a
                | SType.DawnExperimentalImmediateDataLimits ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.DawnExperimentalImmediateDataLimits> (ptr))
                    DawnExperimentalImmediateDataLimits.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ISurfaceDescriptorExtension> then
                match sType with
                | SType.SurfaceSourceAndroidNativeWindow ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceSourceAndroidNativeWindow> (ptr))
                    SurfaceSourceAndroidNativeWindow.Read(&rr) :> obj :?> 'a
                | SType.SurfaceSourceCanvasHTMLSelector_Emscripten ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceSourceCanvasHTMLSelector_Emscripten> (ptr))
                    SurfaceSourceCanvasHTMLSelector_Emscripten.Read(&rr) :> obj :?> 'a
                | SType.SurfaceSourceMetalLayer ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceSourceMetalLayer> (ptr))
                    SurfaceSourceMetalLayer.Read(&rr) :> obj :?> 'a
                | SType.SurfaceSourceWindowsHWND ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceSourceWindowsHWND> (ptr))
                    SurfaceSourceWindowsHWND.Read(&rr) :> obj :?> 'a
                | SType.SurfaceSourceXCBWindow ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceSourceXCBWindow> (ptr))
                    SurfaceSourceXCBWindow.Read(&rr) :> obj :?> 'a
                | SType.SurfaceSourceXlibWindow ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceSourceXlibWindow> (ptr))
                    SurfaceSourceXlibWindow.Read(&rr) :> obj :?> 'a
                | SType.SurfaceSourceWaylandSurface ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceSourceWaylandSurface> (ptr))
                    SurfaceSourceWaylandSurface.Read(&rr) :> obj :?> 'a
                | SType.SurfaceDescriptorFromWindowsCoreWindow ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceDescriptorFromWindowsCoreWindow> (ptr))
                    SurfaceDescriptorFromWindowsCoreWindow.Read(&rr) :> obj :?> 'a
                | SType.SurfaceDescriptorFromWindowsSwapChainPanel ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.SurfaceDescriptorFromWindowsSwapChainPanel> (ptr))
                    SurfaceDescriptorFromWindowsSwapChainPanel.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ITextureDescriptorExtension> then
                match sType with
                | SType.TextureBindingViewDimensionDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.TextureBindingViewDimensionDescriptor> (ptr))
                    TextureBindingViewDimensionDescriptor.Read(&rr) :> obj :?> 'a
                | SType.DawnTextureInternalUsageDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.DawnTextureInternalUsageDescriptor> (ptr))
                    DawnTextureInternalUsageDescriptor.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<ITextureViewDescriptorExtension> then
                match sType with
                | SType.YCbCrVkDescriptor ->
                    let rr = NativePtr.toByRef (NativePtr.ofNativeInt<WebGPU.Raw.YCbCrVkDescriptor> (ptr))
                    YCbCrVkDescriptor.Read(&rr) :> obj :?> 'a
                | _ -> failwithf "bad s type: %A" sType
            elif typeof<'a> = typeof<IExtension> then
                Unchecked.defaultof<'a> // TODO
            else
                failwithf "bad extension type: %A" typeof<'a>
[<AbstractClass; Sealed>]
type private PinHelper() =
    static member inline PinNullable<'r>(x : IExtension, action : nativeint -> 'r) = 
        if isNull x then action 0n
        else x.Pin action
type INTERNAL__HAVE_EMDAWNWEBGPU_HEADER = 
    {
        Unused : bool
    }
    static member Null = Unchecked.defaultof<INTERNAL__HAVE_EMDAWNWEBGPU_HEADER>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.INTERNAL__HAVE_EMDAWNWEBGPU_HEADER> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.INTERNAL__HAVE_EMDAWNWEBGPU_HEADER(
                    (if this.Unused then 1 else 0)
                )
            use ptr = fixed &value
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.INTERNAL__HAVE_EMDAWNWEBGPU_HEADER> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.INTERNAL__HAVE_EMDAWNWEBGPU_HEADER>) = 
        {
            Unused = (backend.Unused <> 0)
        }
type Proc = delegate of IDisposable -> unit
type RequestAdapterOptions = 
    {
        Next : IRequestAdapterOptionsExtension
        CompatibleSurface : Surface
        PowerPreference : PowerPreference
        BackendType : BackendType
        ForceFallbackAdapter : bool
        CompatibilityMode : bool
    }
    static member Null = Unchecked.defaultof<RequestAdapterOptions>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RequestAdapterOptions> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable value =
                    new WebGPU.Raw.RequestAdapterOptions(
                        nextInChain,
                        this.CompatibleSurface.Handle,
                        this.PowerPreference,
                        this.BackendType,
                        (if this.ForceFallbackAdapter then 1 else 0),
                        (if this.CompatibilityMode then 1 else 0)
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RequestAdapterOptions> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RequestAdapterOptions>) = 
        {
            Next = ExtensionDecoder.decode<IRequestAdapterOptionsExtension> backend.NextInChain
            CompatibleSurface = new Surface(backend.CompatibleSurface)
            PowerPreference = backend.PowerPreference
            BackendType = backend.BackendType
            ForceFallbackAdapter = (backend.ForceFallbackAdapter <> 0)
            CompatibilityMode = (backend.CompatibilityMode <> 0)
        }
type RequestAdapterCallback = delegate of IDisposable * status : RequestAdapterStatus * adapter : Adapter * message : string -> unit
type RequestAdapterCallback2 = delegate of IDisposable * status : RequestAdapterStatus * adapter : Adapter * message : string -> unit
type RequestAdapterCallbackInfo = 
    {
        Next : IExtension
        Mode : CallbackMode
        Callback : RequestAdapterCallback
    }
    static member Null = Unchecked.defaultof<RequestAdapterCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RequestAdapterCallbackInfo> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.RequestAdapterCallback>
                _callbackDel <- WebGPU.Raw.RequestAdapterCallback(fun status adapter message userdata ->
                    let _status = status
                    let _adapter = new Adapter(adapter)
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _adapter, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.RequestAdapterCallbackInfo(
                        nextInChain,
                        this.Mode,
                        _callbackPtr,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RequestAdapterCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RequestAdapterCallbackInfo>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
type RequestAdapterCallbackInfo2 = 
    {
        Next : IExtension
        Mode : CallbackMode
        Callback : RequestAdapterCallback2
    }
    static member Null = Unchecked.defaultof<RequestAdapterCallbackInfo2>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RequestAdapterCallbackInfo2> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.RequestAdapterCallback2>
                _callbackDel <- WebGPU.Raw.RequestAdapterCallback2(fun status adapter message userdata1 userdata2 ->
                    let _status = status
                    let _adapter = new Adapter(adapter)
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _adapter, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.RequestAdapterCallbackInfo2(
                        nextInChain,
                        this.Mode,
                        _callbackPtr,
                        Unchecked.defaultof<_>,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RequestAdapterCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RequestAdapterCallbackInfo2>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
type Adapter internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"Adapter(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? Adapter as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new Adapter(0n)
    member this.Instance : Instance =
        let mutable res = WebGPU.Raw.WebGPU.AdapterGetInstance(handle)
        new Instance(res)
    member _.Limits : SupportedLimits =
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        let status = WebGPU.Raw.WebGPU.AdapterGetLimits(handle, ptr)
        if status <> Status.Success then failwith "GetLimits failed"
        SupportedLimits.Read(&res)
    member _.Info : AdapterInfo =
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        let status = WebGPU.Raw.WebGPU.AdapterGetInfo(handle, ptr)
        if status <> Status.Success then failwith "GetInfo failed"
        AdapterInfo.Read(&res)
    member _.HasFeature(feature : FeatureName) : bool =
        let res = WebGPU.Raw.WebGPU.AdapterHasFeature(handle, feature)
        (res <> 0)
    member _.Features : SupportedFeatures =
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        WebGPU.Raw.WebGPU.AdapterGetFeatures(handle, ptr)
        SupportedFeatures.Read(&res)
    member _.RequestDevice(descriptor : DeviceDescriptor, callback : RequestDeviceCallback) : unit =
        descriptor.Pin(fun _descriptorPtr ->
            let mutable _callbackGC = Unchecked.defaultof<GCHandle>
            let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.RequestDeviceCallback>
            _callbackDel <- WebGPU.Raw.RequestDeviceCallback(fun status device message userdata ->
                let _status = status
                let _device = new Device(device)
                let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _device, _message)
            )
            _callbackGC <- GCHandle.Alloc(_callbackDel)
            let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
            let res = WebGPU.Raw.WebGPU.AdapterRequestDevice(handle, _descriptorPtr, _callbackPtr, Unchecked.defaultof<_>)
            res
        )
    member _.RequestDeviceF(options : DeviceDescriptor, callbackInfo : RequestDeviceCallbackInfo) : Future =
        options.Pin(fun _optionsPtr ->
            callbackInfo.Pin(fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.AdapterRequestDeviceF(handle, _optionsPtr, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
                Future.Read(&res)
            )
        )
    member _.RequestDevice2(options : DeviceDescriptor, callbackInfo : RequestDeviceCallbackInfo2) : Future =
        options.Pin(fun _optionsPtr ->
            callbackInfo.Pin(fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.AdapterRequestDevice2(handle, _optionsPtr, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
                Future.Read(&res)
            )
        )
    member _.CreateDevice(descriptor : DeviceDescriptor) : Device =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.AdapterCreateDevice(handle, _descriptorPtr)
            new Device(res)
        )
    member _.GetFormatCapabilities(format : TextureFormat, capabilities : byref<FormatCapabilities>) : Status =
        let mutable capabilitiesCopy = capabilities
        try
            capabilities.Pin(fun _capabilitiesPtr ->
                if NativePtr.toNativeInt _capabilitiesPtr = 0n then
                    let mutable capabilitiesNative = Unchecked.defaultof<WebGPU.Raw.FormatCapabilities>
                    use _capabilitiesPtr = fixed &capabilitiesNative
                    let res = WebGPU.Raw.WebGPU.AdapterGetFormatCapabilities(handle, format, _capabilitiesPtr)
                    let _ret = res
                    capabilitiesCopy <- FormatCapabilities.Read(&capabilitiesNative)
                    _ret
                else
                    let res = WebGPU.Raw.WebGPU.AdapterGetFormatCapabilities(handle, format, _capabilitiesPtr)
                    let _ret = res
                    let capabilitiesResult = NativePtr.toByRef _capabilitiesPtr
                    capabilitiesCopy <- FormatCapabilities.Read(&capabilitiesResult)
                    _ret
            )
        finally
            capabilities <- capabilitiesCopy
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.AdapterRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
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
        CompatibilityMode : bool
    }
    static member Null = Unchecked.defaultof<AdapterInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.AdapterInfo> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _vendorArr = Encoding.UTF8.GetBytes(this.Vendor)
                use _vendorPtr = fixed _vendorArr
                let _vendorLen = WebGPU.Raw.StringView(_vendorPtr, unativeint _vendorArr.Length)
                let _architectureArr = Encoding.UTF8.GetBytes(this.Architecture)
                use _architecturePtr = fixed _architectureArr
                let _architectureLen = WebGPU.Raw.StringView(_architecturePtr, unativeint _architectureArr.Length)
                let _deviceArr = Encoding.UTF8.GetBytes(this.Device)
                use _devicePtr = fixed _deviceArr
                let _deviceLen = WebGPU.Raw.StringView(_devicePtr, unativeint _deviceArr.Length)
                let _descriptionArr = Encoding.UTF8.GetBytes(this.Description)
                use _descriptionPtr = fixed _descriptionArr
                let _descriptionLen = WebGPU.Raw.StringView(_descriptionPtr, unativeint _descriptionArr.Length)
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
                        (if this.CompatibilityMode then 1 else 0)
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.AdapterInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.AdapterInfo>) = 
        {
            Next = ExtensionDecoder.decode<IAdapterInfoExtension> backend.NextInChain
            Vendor = let _vendorPtr = NativePtr.toNativeInt(backend.Vendor.Data) in if _vendorPtr = 0n then null else Marshal.PtrToStringUTF8(_vendorPtr, int(backend.Vendor.Length))
            Architecture = let _architecturePtr = NativePtr.toNativeInt(backend.Architecture.Data) in if _architecturePtr = 0n then null else Marshal.PtrToStringUTF8(_architecturePtr, int(backend.Architecture.Length))
            Device = let _devicePtr = NativePtr.toNativeInt(backend.Device.Data) in if _devicePtr = 0n then null else Marshal.PtrToStringUTF8(_devicePtr, int(backend.Device.Length))
            Description = let _descriptionPtr = NativePtr.toNativeInt(backend.Description.Data) in if _descriptionPtr = 0n then null else Marshal.PtrToStringUTF8(_descriptionPtr, int(backend.Description.Length))
            BackendType = backend.BackendType
            AdapterType = backend.AdapterType
            VendorID = int(backend.VendorID)
            DeviceID = int(backend.DeviceID)
            CompatibilityMode = (backend.CompatibilityMode <> 0)
        }
type DeviceDescriptor = 
    {
        Next : IDeviceDescriptorExtension
        Label : string
        RequiredFeatures : array<FeatureName>
        RequiredLimits : RequiredLimits
        DefaultQueue : QueueDescriptor
        DeviceLostCallbackInfo : DeviceLostCallbackInfo
        UncapturedErrorCallbackInfo : UncapturedErrorCallbackInfo
        DeviceLostCallbackInfo2 : DeviceLostCallbackInfo2
        UncapturedErrorCallbackInfo2 : UncapturedErrorCallbackInfo2
    }
    static member Null = Unchecked.defaultof<DeviceDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DeviceDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                use requiredFeaturesPtr = fixed (this.RequiredFeatures)
                let requiredFeaturesLen = unativeint this.RequiredFeatures.Length
                this.RequiredLimits.Pin(fun _requiredLimitsPtr ->
                    this.DefaultQueue.Pin(fun _defaultQueuePtr ->
                        this.DeviceLostCallbackInfo.Pin(fun _deviceLostCallbackInfoPtr ->
                            this.UncapturedErrorCallbackInfo.Pin(fun _uncapturedErrorCallbackInfoPtr ->
                                this.DeviceLostCallbackInfo2.Pin(fun _deviceLostCallbackInfo2Ptr ->
                                    this.UncapturedErrorCallbackInfo2.Pin(fun _uncapturedErrorCallbackInfo2Ptr ->
                                        let mutable value =
                                            new WebGPU.Raw.DeviceDescriptor(
                                                nextInChain,
                                                _labelLen,
                                                requiredFeaturesLen,
                                                requiredFeaturesPtr,
                                                _requiredLimitsPtr,
                                                (if NativePtr.toNativeInt _defaultQueuePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _defaultQueuePtr),
                                                Unchecked.defaultof<_>,
                                                Unchecked.defaultof<_>,
                                                (if NativePtr.toNativeInt _deviceLostCallbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _deviceLostCallbackInfoPtr),
                                                (if NativePtr.toNativeInt _uncapturedErrorCallbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _uncapturedErrorCallbackInfoPtr),
                                                (if NativePtr.toNativeInt _deviceLostCallbackInfo2Ptr = 0n then Unchecked.defaultof<_> else NativePtr.read _deviceLostCallbackInfo2Ptr),
                                                (if NativePtr.toNativeInt _uncapturedErrorCallbackInfo2Ptr = 0n then Unchecked.defaultof<_> else NativePtr.read _uncapturedErrorCallbackInfo2Ptr)
                                            )
                                        use ptr = fixed &value
                                        action ptr
                                    )
                                )
                            )
                        )
                    )
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DeviceDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DeviceDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IDeviceDescriptorExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            RequiredFeatures = let ptr = backend.RequiredFeatures in Array.init (int backend.RequiredFeatureCount) (fun i -> NativePtr.get ptr i)
            RequiredLimits = let m = NativePtr.toByRef backend.RequiredLimits in RequiredLimits.Read(&m)
            DefaultQueue = QueueDescriptor.Read(&backend.DefaultQueue)
            DeviceLostCallbackInfo = DeviceLostCallbackInfo.Read(&backend.DeviceLostCallbackInfo)
            UncapturedErrorCallbackInfo = UncapturedErrorCallbackInfo.Read(&backend.UncapturedErrorCallbackInfo)
            DeviceLostCallbackInfo2 = DeviceLostCallbackInfo2.Read(&backend.DeviceLostCallbackInfo2)
            UncapturedErrorCallbackInfo2 = UncapturedErrorCallbackInfo2.Read(&backend.UncapturedErrorCallbackInfo2)
        }
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DawnTogglesDescriptor> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IInstanceDescriptorExtension
    interface IRequestAdapterOptionsExtension
    interface IDeviceDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnTogglesDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DawnTogglesDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IInstanceDescriptorExtension> backend.NextInChain
            EnabledToggleCount = int64(backend.EnabledToggleCount)
            EnabledToggles = backend.EnabledToggles
            DisabledToggleCount = int64(backend.DisabledToggleCount)
            DisabledToggles = backend.DisabledToggles
        }
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DawnCacheDeviceDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnCacheDeviceDescriptor
                let _isolationKeyArr = Encoding.UTF8.GetBytes(this.IsolationKey)
                use _isolationKeyPtr = fixed _isolationKeyArr
                let _isolationKeyLen = WebGPU.Raw.StringView(_isolationKeyPtr, unativeint _isolationKeyArr.Length)
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
                let _loadDataFunctionPtr = Marshal.GetFunctionPointerForDelegate(_loadDataFunctionDel)
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
                let _storeDataFunctionPtr = Marshal.GetFunctionPointerForDelegate(_storeDataFunctionDel)
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IDeviceDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnCacheDeviceDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DawnCacheDeviceDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IDeviceDescriptorExtension> backend.NextInChain
            IsolationKey = let _isolationKeyPtr = NativePtr.toNativeInt(backend.IsolationKey.Data) in if _isolationKeyPtr = 0n then null else Marshal.PtrToStringUTF8(_isolationKeyPtr, int(backend.IsolationKey.Length))
            LoadDataFunction = failwith "cannot read callbacks"//TODO2 map [(function userdata, backend.FunctionUserdata); (isolation key, backend.IsolationKey); (load data function, backend.LoadDataFunction); ... ]
            StoreDataFunction = failwith "cannot read callbacks"//TODO2 map [(function userdata, backend.FunctionUserdata); (isolation key, backend.IsolationKey); (load data function, backend.LoadDataFunction); ... ]
        }
type DawnWGSLBlocklist = 
    {
        Next : IInstanceDescriptorExtension
        BlocklistedFeatureCount : int64
        BlocklistedFeatures : nativeptr<nativeptr<byte>>
    }
    static member Null = Unchecked.defaultof<DawnWGSLBlocklist>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DawnWGSLBlocklist> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IInstanceDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnWGSLBlocklist> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DawnWGSLBlocklist>) = 
        {
            Next = ExtensionDecoder.decode<IInstanceDescriptorExtension> backend.NextInChain
            BlocklistedFeatureCount = int64(backend.BlocklistedFeatureCount)
            BlocklistedFeatures = backend.BlocklistedFeatures
        }
type BindGroup internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"BindGroup(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? BindGroup as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new BindGroup(0n)
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.BindGroupSetLabel(handle, _labelLen)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.BindGroupRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BindGroupEntry> -> 'r) : 'r = 
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
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BindGroupEntry> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BindGroupEntry>) = 
        {
            Next = ExtensionDecoder.decode<IBindGroupEntryExtension> backend.NextInChain
            Binding = int(backend.Binding)
            Buffer = new Buffer(backend.Buffer)
            Offset = int64(backend.Offset)
            Size = int64(backend.Size)
            Sampler = new Sampler(backend.Sampler)
            TextureView = new TextureView(backend.TextureView)
        }
type BindGroupDescriptor = 
    {
        Next : IExtension
        Label : string
        Layout : BindGroupLayout
        Entries : array<BindGroupEntry>
    }
    static member Null = Unchecked.defaultof<BindGroupDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BindGroupDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                WebGPU.Raw.Pinnable.pinArray this.Entries (fun entriesPtr ->
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
                    action ptr
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BindGroupDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BindGroupDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            Layout = new BindGroupLayout(backend.Layout)
            Entries = let ptr = backend.Entries in Array.init (int backend.EntryCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in BindGroupEntry.Read(&r))
        }
type BindGroupLayout internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"BindGroupLayout(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? BindGroupLayout as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new BindGroupLayout(0n)
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.BindGroupLayoutSetLabel(handle, _labelLen)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.BindGroupLayoutRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type BufferBindingLayout = 
    {
        Next : IExtension
        Type : BufferBindingType
        HasDynamicOffset : bool
        MinBindingSize : int64
    }
    static member Null = Unchecked.defaultof<BufferBindingLayout>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BufferBindingLayout> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable value =
                    new WebGPU.Raw.BufferBindingLayout(
                        nextInChain,
                        this.Type,
                        (if this.HasDynamicOffset then 1 else 0),
                        uint64(this.MinBindingSize)
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BufferBindingLayout> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BufferBindingLayout>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Type = backend.Type
            HasDynamicOffset = (backend.HasDynamicOffset <> 0)
            MinBindingSize = int64(backend.MinBindingSize)
        }
type SamplerBindingLayout = 
    {
        Next : IExtension
        Type : SamplerBindingType
    }
    static member Null = Unchecked.defaultof<SamplerBindingLayout>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SamplerBindingLayout> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable value =
                    new WebGPU.Raw.SamplerBindingLayout(
                        nextInChain,
                        this.Type
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SamplerBindingLayout> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SamplerBindingLayout>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Type = backend.Type
        }
type StaticSamplerBindingLayout = 
    {
        Next : IBindGroupLayoutEntryExtension
        Sampler : Sampler
        SampledTextureBinding : int
    }
    static member Null = Unchecked.defaultof<StaticSamplerBindingLayout>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.StaticSamplerBindingLayout> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IBindGroupLayoutEntryExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.StaticSamplerBindingLayout> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.StaticSamplerBindingLayout>) = 
        {
            Next = ExtensionDecoder.decode<IBindGroupLayoutEntryExtension> backend.NextInChain
            Sampler = new Sampler(backend.Sampler)
            SampledTextureBinding = int(backend.SampledTextureBinding)
        }
type TextureBindingLayout = 
    {
        Next : IExtension
        SampleType : TextureSampleType
        ViewDimension : TextureViewDimension
        Multisampled : bool
    }
    static member Null = Unchecked.defaultof<TextureBindingLayout>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.TextureBindingLayout> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable value =
                    new WebGPU.Raw.TextureBindingLayout(
                        nextInChain,
                        this.SampleType,
                        this.ViewDimension,
                        (if this.Multisampled then 1 else 0)
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.TextureBindingLayout> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.TextureBindingLayout>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            SampleType = backend.SampleType
            ViewDimension = backend.ViewDimension
            Multisampled = (backend.Multisampled <> 0)
        }
type SurfaceCapabilities = 
    {
        Next : IExtension
        Usages : TextureUsage
        Formats : array<TextureFormat>
        PresentModes : array<PresentMode>
        AlphaModes : array<CompositeAlphaMode>
    }
    static member Null = Unchecked.defaultof<SurfaceCapabilities>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SurfaceCapabilities> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                use formatsPtr = fixed (this.Formats)
                let formatsLen = unativeint this.Formats.Length
                use presentModesPtr = fixed (this.PresentModes)
                let presentModesLen = unativeint this.PresentModes.Length
                use alphaModesPtr = fixed (this.AlphaModes)
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
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceCapabilities> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SurfaceCapabilities>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Usages = backend.Usages
            Formats = let ptr = backend.Formats in Array.init (int backend.FormatCount) (fun i -> NativePtr.get ptr i)
            PresentModes = let ptr = backend.PresentModes in Array.init (int backend.PresentModeCount) (fun i -> NativePtr.get ptr i)
            AlphaModes = let ptr = backend.AlphaModes in Array.init (int backend.AlphaModeCount) (fun i -> NativePtr.get ptr i)
        }
type SurfaceConfiguration = 
    {
        Next : IExtension
        Device : Device
        Format : TextureFormat
        Usage : TextureUsage
        ViewFormats : array<TextureFormat>
        AlphaMode : CompositeAlphaMode
        Width : int
        Height : int
        PresentMode : PresentMode
    }
    static member Null = Unchecked.defaultof<SurfaceConfiguration>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SurfaceConfiguration> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                use viewFormatsPtr = fixed (this.ViewFormats)
                let viewFormatsLen = unativeint this.ViewFormats.Length
                let mutable value =
                    new WebGPU.Raw.SurfaceConfiguration(
                        nextInChain,
                        this.Device.Handle,
                        this.Format,
                        this.Usage,
                        viewFormatsLen,
                        viewFormatsPtr,
                        this.AlphaMode,
                        uint32(this.Width),
                        uint32(this.Height),
                        this.PresentMode
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceConfiguration> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SurfaceConfiguration>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Device = new Device(backend.Device)
            Format = backend.Format
            Usage = backend.Usage
            ViewFormats = let ptr = backend.ViewFormats in Array.init (int backend.ViewFormatCount) (fun i -> NativePtr.get ptr i)
            AlphaMode = backend.AlphaMode
            Width = int(backend.Width)
            Height = int(backend.Height)
            PresentMode = backend.PresentMode
        }
type ExternalTextureBindingEntry = 
    {
        Next : IBindGroupEntryExtension
        ExternalTexture : ExternalTexture
    }
    static member Null = Unchecked.defaultof<ExternalTextureBindingEntry>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ExternalTextureBindingEntry> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IBindGroupEntryExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ExternalTextureBindingEntry> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ExternalTextureBindingEntry>) = 
        {
            Next = ExtensionDecoder.decode<IBindGroupEntryExtension> backend.NextInChain
            ExternalTexture = new ExternalTexture(backend.ExternalTexture)
        }
type ExternalTextureBindingLayout = 
    {
        Next : IBindGroupLayoutEntryExtension
    }
    static member Null = Unchecked.defaultof<ExternalTextureBindingLayout>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ExternalTextureBindingLayout> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IBindGroupLayoutEntryExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ExternalTextureBindingLayout> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ExternalTextureBindingLayout>) = 
        {
            Next = ExtensionDecoder.decode<IBindGroupLayoutEntryExtension> backend.NextInChain
        }
type StorageTextureBindingLayout = 
    {
        Next : IExtension
        Access : StorageTextureAccess
        Format : TextureFormat
        ViewDimension : TextureViewDimension
    }
    static member Null = Unchecked.defaultof<StorageTextureBindingLayout>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.StorageTextureBindingLayout> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable value =
                    new WebGPU.Raw.StorageTextureBindingLayout(
                        nextInChain,
                        this.Access,
                        this.Format,
                        this.ViewDimension
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.StorageTextureBindingLayout> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.StorageTextureBindingLayout>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Access = backend.Access
            Format = backend.Format
            ViewDimension = backend.ViewDimension
        }
type BindGroupLayoutEntry = 
    {
        Next : IBindGroupLayoutEntryExtension
        Binding : int
        Visibility : ShaderStage
        Buffer : BufferBindingLayout
        Sampler : SamplerBindingLayout
        Texture : TextureBindingLayout
        StorageTexture : StorageTextureBindingLayout
    }
    static member Null = Unchecked.defaultof<BindGroupLayoutEntry>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BindGroupLayoutEntry> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                this.Buffer.Pin(fun _bufferPtr ->
                    this.Sampler.Pin(fun _samplerPtr ->
                        this.Texture.Pin(fun _texturePtr ->
                            this.StorageTexture.Pin(fun _storageTexturePtr ->
                                let mutable value =
                                    new WebGPU.Raw.BindGroupLayoutEntry(
                                        nextInChain,
                                        uint32(this.Binding),
                                        this.Visibility,
                                        (if NativePtr.toNativeInt _bufferPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _bufferPtr),
                                        (if NativePtr.toNativeInt _samplerPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _samplerPtr),
                                        (if NativePtr.toNativeInt _texturePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _texturePtr),
                                        (if NativePtr.toNativeInt _storageTexturePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _storageTexturePtr)
                                    )
                                use ptr = fixed &value
                                action ptr
                            )
                        )
                    )
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BindGroupLayoutEntry> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BindGroupLayoutEntry>) = 
        {
            Next = ExtensionDecoder.decode<IBindGroupLayoutEntryExtension> backend.NextInChain
            Binding = int(backend.Binding)
            Visibility = backend.Visibility
            Buffer = BufferBindingLayout.Read(&backend.Buffer)
            Sampler = SamplerBindingLayout.Read(&backend.Sampler)
            Texture = TextureBindingLayout.Read(&backend.Texture)
            StorageTexture = StorageTextureBindingLayout.Read(&backend.StorageTexture)
        }
type BindGroupLayoutDescriptor = 
    {
        Next : IExtension
        Label : string
        Entries : array<BindGroupLayoutEntry>
    }
    static member Null = Unchecked.defaultof<BindGroupLayoutDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BindGroupLayoutDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                WebGPU.Raw.Pinnable.pinArray this.Entries (fun entriesPtr ->
                    let entriesLen = unativeint this.Entries.Length
                    let mutable value =
                        new WebGPU.Raw.BindGroupLayoutDescriptor(
                            nextInChain,
                            _labelLen,
                            entriesLen,
                            entriesPtr
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BindGroupLayoutDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BindGroupLayoutDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            Entries = let ptr = backend.Entries in Array.init (int backend.EntryCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in BindGroupLayoutEntry.Read(&r))
        }
type BlendComponent = 
    {
        Operation : BlendOperation
        SrcFactor : BlendFactor
        DstFactor : BlendFactor
    }
    static member Null = Unchecked.defaultof<BlendComponent>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BlendComponent> -> 'r) : 'r = 
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
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BlendComponent> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BlendComponent>) = 
        {
            Operation = backend.Operation
            SrcFactor = backend.SrcFactor
            DstFactor = backend.DstFactor
        }
type StringView = 
    {
        Data : string
        Length : int64
    }
    static member Null = Unchecked.defaultof<StringView>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.StringView> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            use _dataPtr = fixed (Encoding.UTF8.GetBytes(this.Data))
            let mutable value =
                new WebGPU.Raw.StringView(
                    _dataPtr,
                    unativeint(this.Length)
                )
            use ptr = fixed &value
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.StringView> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.StringView>) = 
        {
            Data = Marshal.PtrToStringAnsi(NativePtr.toNativeInt backend.Data)
            Length = int64(backend.Length)
        }
type Buffer internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"Buffer(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? Buffer as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new Buffer(0n)
    member _.MapAsync(mode : MapMode, offset : int64, size : int64, callback : BufferMapCallback) : unit =
        let mutable _callbackGC = Unchecked.defaultof<GCHandle>
        let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.BufferMapCallback>
        _callbackDel <- WebGPU.Raw.BufferMapCallback(fun status userdata ->
            let _status = status
            callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status)
        )
        _callbackGC <- GCHandle.Alloc(_callbackDel)
        let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
        let res = WebGPU.Raw.WebGPU.BufferMapAsync(handle, mode, unativeint(offset), unativeint(size), _callbackPtr, Unchecked.defaultof<_>)
        res
    member _.MapAsyncF(mode : MapMode, offset : int64, size : int64, callbackInfo : BufferMapCallbackInfo) : Future =
        callbackInfo.Pin(fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.BufferMapAsyncF(handle, mode, unativeint(offset), unativeint(size), (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
            Future.Read(&res)
        )
    member _.MapAsync2(mode : MapMode, offset : int64, size : int64, callbackInfo : BufferMapCallbackInfo2) : Future =
        callbackInfo.Pin(fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.BufferMapAsync2(handle, mode, unativeint(offset), unativeint(size), (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
            Future.Read(&res)
        )
    member _.GetMappedRange(offset : int64, size : int64) : nativeint =
        let res = WebGPU.Raw.WebGPU.BufferGetMappedRange(handle, unativeint(offset), unativeint(size))
        res
    member _.GetConstMappedRange(offset : int64, size : int64) : nativeint =
        let res = WebGPU.Raw.WebGPU.BufferGetConstMappedRange(handle, unativeint(offset), unativeint(size))
        res
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.BufferSetLabel(handle, _labelLen)
        res
    member this.Usage : BufferUsage =
        let mutable res = WebGPU.Raw.WebGPU.BufferGetUsage(handle)
        res
    member this.Size : int64 =
        let mutable res = WebGPU.Raw.WebGPU.BufferGetSize(handle)
        int64(res)
    member this.MapState : BufferMapState =
        let mutable res = WebGPU.Raw.WebGPU.BufferGetMapState(handle)
        res
    member _.Unmap() : unit =
        let res = WebGPU.Raw.WebGPU.BufferUnmap(handle)
        res
    member _.Destroy() : unit =
        let res = WebGPU.Raw.WebGPU.BufferDestroy(handle)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.BufferRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BufferDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                let mutable value =
                    new WebGPU.Raw.BufferDescriptor(
                        nextInChain,
                        _labelLen,
                        this.Usage,
                        uint64(this.Size),
                        (if this.MappedAtCreation then 1 else 0)
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BufferDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BufferDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IBufferDescriptorExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            Usage = backend.Usage
            Size = int64(backend.Size)
            MappedAtCreation = (backend.MappedAtCreation <> 0)
        }
type BufferHostMappedPointer = 
    {
        Next : IBufferDescriptorExtension
        Pointer : nativeint
        DisposeCallback : Callback
    }
    static member Null = Unchecked.defaultof<BufferHostMappedPointer>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BufferHostMappedPointer> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.BufferHostMappedPointer
                let mutable _disposeCallbackGC = Unchecked.defaultof<GCHandle>
                let mutable _disposeCallbackDel = Unchecked.defaultof<WebGPU.Raw.Callback>
                _disposeCallbackDel <- WebGPU.Raw.Callback(fun userdata ->
                    this.DisposeCallback.Invoke({ new IDisposable with member __.Dispose() = _disposeCallbackGC.Free() })
                )
                _disposeCallbackGC <- GCHandle.Alloc(_disposeCallbackDel)
                let _disposeCallbackPtr = Marshal.GetFunctionPointerForDelegate(_disposeCallbackDel)
                let mutable value =
                    new WebGPU.Raw.BufferHostMappedPointer(
                        nextInChain,
                        sType,
                        this.Pointer,
                        _disposeCallbackPtr,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IBufferDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BufferHostMappedPointer> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BufferHostMappedPointer>) = 
        {
            Next = ExtensionDecoder.decode<IBufferDescriptorExtension> backend.NextInChain
            Pointer = backend.Pointer
            DisposeCallback = failwith "cannot read callbacks"//TODO2 map [(dispose callback, backend.DisposeCallback); (next in chain, backend.NextInChain); (pointer, backend.Pointer); ... ]
        }
type Callback = delegate of IDisposable -> unit
type BufferMapCallback = delegate of IDisposable * status : BufferMapAsyncStatus -> unit
type BufferMapCallback2 = delegate of IDisposable * status : MapAsyncStatus * message : string -> unit
type BufferMapCallbackInfo = 
    {
        Next : IExtension
        Mode : CallbackMode
        Callback : BufferMapCallback
    }
    static member Null = Unchecked.defaultof<BufferMapCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BufferMapCallbackInfo> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.BufferMapCallback>
                _callbackDel <- WebGPU.Raw.BufferMapCallback(fun status userdata ->
                    let _status = status
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.BufferMapCallbackInfo(
                        nextInChain,
                        this.Mode,
                        _callbackPtr,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BufferMapCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BufferMapCallbackInfo>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
type BufferMapCallbackInfo2 = 
    {
        Next : IExtension
        Mode : CallbackMode
        Callback : BufferMapCallback2
    }
    static member Null = Unchecked.defaultof<BufferMapCallbackInfo2>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BufferMapCallbackInfo2> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.BufferMapCallback2>
                _callbackDel <- WebGPU.Raw.BufferMapCallback2(fun status message userdata1 userdata2 ->
                    let _status = status
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.BufferMapCallbackInfo2(
                        nextInChain,
                        this.Mode,
                        _callbackPtr,
                        Unchecked.defaultof<_>,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BufferMapCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BufferMapCallbackInfo2>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
type Color = 
    {
        R : double
        G : double
        B : double
        A : double
    }
    static member Null = Unchecked.defaultof<Color>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.Color> -> 'r) : 'r = 
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
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.Color> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.Color>) = 
        {
            R = backend.R
            G = backend.G
            B = backend.B
            A = backend.A
        }
type ConstantEntry = 
    {
        Next : IExtension
        Key : string
        Value : double
    }
    static member Null = Unchecked.defaultof<ConstantEntry>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ConstantEntry> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _keyArr = Encoding.UTF8.GetBytes(this.Key)
                use _keyPtr = fixed _keyArr
                let _keyLen = WebGPU.Raw.StringView(_keyPtr, unativeint _keyArr.Length)
                let mutable value =
                    new WebGPU.Raw.ConstantEntry(
                        nextInChain,
                        _keyLen,
                        this.Value
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ConstantEntry> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ConstantEntry>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Key = let _keyPtr = NativePtr.toNativeInt(backend.Key.Data) in if _keyPtr = 0n then null else Marshal.PtrToStringUTF8(_keyPtr, int(backend.Key.Length))
            Value = backend.Value
        }
type CommandBuffer internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"CommandBuffer(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? CommandBuffer as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new CommandBuffer(0n)
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.CommandBufferSetLabel(handle, _labelLen)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.CommandBufferRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type CommandBufferDescriptor = 
    {
        Next : IExtension
        Label : string
    }
    static member Null = Unchecked.defaultof<CommandBufferDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CommandBufferDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                let mutable value =
                    new WebGPU.Raw.CommandBufferDescriptor(
                        nextInChain,
                        _labelLen
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CommandBufferDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CommandBufferDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
        }
type CommandEncoder internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"CommandEncoder(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? CommandEncoder as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new CommandEncoder(0n)
    member _.Finish(descriptor : CommandBufferDescriptor) : CommandBuffer =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.CommandEncoderFinish(handle, _descriptorPtr)
            new CommandBuffer(res)
        )
    member _.BeginComputePass(descriptor : ComputePassDescriptor) : ComputePassEncoder =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.CommandEncoderBeginComputePass(handle, _descriptorPtr)
            new ComputePassEncoder(res)
        )
    member _.BeginRenderPass(descriptor : RenderPassDescriptor) : RenderPassEncoder =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.CommandEncoderBeginRenderPass(handle, _descriptorPtr)
            new RenderPassEncoder(res)
        )
    member _.CopyBufferToBuffer(source : Buffer, sourceOffset : int64, destination : Buffer, destinationOffset : int64, size : int64) : unit =
        let res = WebGPU.Raw.WebGPU.CommandEncoderCopyBufferToBuffer(handle, source.Handle, uint64(sourceOffset), destination.Handle, uint64(destinationOffset), uint64(size))
        res
    member _.CopyBufferToTexture(source : ImageCopyBuffer, destination : ImageCopyTexture, copySize : Extent3D) : unit =
        source.Pin(fun _sourcePtr ->
            destination.Pin(fun _destinationPtr ->
                copySize.Pin(fun _copySizePtr ->
                    let res = WebGPU.Raw.WebGPU.CommandEncoderCopyBufferToTexture(handle, _sourcePtr, _destinationPtr, _copySizePtr)
                    res
                )
            )
        )
    member _.CopyTextureToBuffer(source : ImageCopyTexture, destination : ImageCopyBuffer, copySize : Extent3D) : unit =
        source.Pin(fun _sourcePtr ->
            destination.Pin(fun _destinationPtr ->
                copySize.Pin(fun _copySizePtr ->
                    let res = WebGPU.Raw.WebGPU.CommandEncoderCopyTextureToBuffer(handle, _sourcePtr, _destinationPtr, _copySizePtr)
                    res
                )
            )
        )
    member _.CopyTextureToTexture(source : ImageCopyTexture, destination : ImageCopyTexture, copySize : Extent3D) : unit =
        source.Pin(fun _sourcePtr ->
            destination.Pin(fun _destinationPtr ->
                copySize.Pin(fun _copySizePtr ->
                    let res = WebGPU.Raw.WebGPU.CommandEncoderCopyTextureToTexture(handle, _sourcePtr, _destinationPtr, _copySizePtr)
                    res
                )
            )
        )
    member _.ClearBuffer(buffer : Buffer, offset : int64, size : int64) : unit =
        let res = WebGPU.Raw.WebGPU.CommandEncoderClearBuffer(handle, buffer.Handle, uint64(offset), uint64(size))
        res
    member _.InjectValidationError(message : string) : unit =
        let _messageArr = Encoding.UTF8.GetBytes(message)
        use _messagePtr = fixed _messageArr
        let _messageLen = WebGPU.Raw.StringView(_messagePtr, unativeint _messageArr.Length)
        let res = WebGPU.Raw.WebGPU.CommandEncoderInjectValidationError(handle, _messageLen)
        res
    member _.InsertDebugMarker(markerLabel : string) : unit =
        let _markerLabelArr = Encoding.UTF8.GetBytes(markerLabel)
        use _markerLabelPtr = fixed _markerLabelArr
        let _markerLabelLen = WebGPU.Raw.StringView(_markerLabelPtr, unativeint _markerLabelArr.Length)
        let res = WebGPU.Raw.WebGPU.CommandEncoderInsertDebugMarker(handle, _markerLabelLen)
        res
    member _.PopDebugGroup() : unit =
        let res = WebGPU.Raw.WebGPU.CommandEncoderPopDebugGroup(handle)
        res
    member _.PushDebugGroup(groupLabel : string) : unit =
        let _groupLabelArr = Encoding.UTF8.GetBytes(groupLabel)
        use _groupLabelPtr = fixed _groupLabelArr
        let _groupLabelLen = WebGPU.Raw.StringView(_groupLabelPtr, unativeint _groupLabelArr.Length)
        let res = WebGPU.Raw.WebGPU.CommandEncoderPushDebugGroup(handle, _groupLabelLen)
        res
    member _.ResolveQuerySet(querySet : QuerySet, firstQuery : int, queryCount : int, destination : Buffer, destinationOffset : int64) : unit =
        let res = WebGPU.Raw.WebGPU.CommandEncoderResolveQuerySet(handle, querySet.Handle, uint32(firstQuery), uint32(queryCount), destination.Handle, uint64(destinationOffset))
        res
    member _.WriteBuffer(buffer : Buffer, bufferOffset : int64, data : array<uint8>, size : int64) : unit =
        use dataPtr = fixed (data)
        let dataLen = uint64 data.Length
        let res = WebGPU.Raw.WebGPU.CommandEncoderWriteBuffer(handle, buffer.Handle, uint64(bufferOffset), dataPtr, uint64(size))
        res
    member _.WriteTimestamp(querySet : QuerySet, queryIndex : int) : unit =
        let res = WebGPU.Raw.WebGPU.CommandEncoderWriteTimestamp(handle, querySet.Handle, uint32(queryIndex))
        res
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.CommandEncoderSetLabel(handle, _labelLen)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.CommandEncoderRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type CommandEncoderDescriptor = 
    {
        Next : ICommandEncoderDescriptorExtension
        Label : string
    }
    static member Null = Unchecked.defaultof<CommandEncoderDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CommandEncoderDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                let mutable value =
                    new WebGPU.Raw.CommandEncoderDescriptor(
                        nextInChain,
                        _labelLen
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CommandEncoderDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CommandEncoderDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ICommandEncoderDescriptorExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
        }
type CompilationInfo = 
    {
        Next : IExtension
        Messages : array<CompilationMessage>
    }
    static member Null = Unchecked.defaultof<CompilationInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CompilationInfo> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                WebGPU.Raw.Pinnable.pinArray this.Messages (fun messagesPtr ->
                    let messagesLen = unativeint this.Messages.Length
                    let mutable value =
                        new WebGPU.Raw.CompilationInfo(
                            nextInChain,
                            messagesLen,
                            messagesPtr
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CompilationInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CompilationInfo>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Messages = let ptr = backend.Messages in Array.init (int backend.MessageCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in CompilationMessage.Read(&r))
        }
type CompilationInfoCallback = delegate of IDisposable * status : CompilationInfoRequestStatus * compilationInfo : CompilationInfo -> unit
type CompilationInfoCallback2 = delegate of IDisposable * status : CompilationInfoRequestStatus * compilationInfo : CompilationInfo -> unit
type CompilationInfoCallbackInfo = 
    {
        Next : IExtension
        Mode : CallbackMode
        Callback : CompilationInfoCallback
    }
    static member Null = Unchecked.defaultof<CompilationInfoCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CompilationInfoCallbackInfo> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.CompilationInfoCallback>
                _callbackDel <- WebGPU.Raw.CompilationInfoCallback(fun status compilationInfo userdata ->
                    let _status = status
                    let _compilationInfo = let m = NativePtr.toByRef compilationInfo in CompilationInfo.Read(&m)
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _compilationInfo)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.CompilationInfoCallbackInfo(
                        nextInChain,
                        this.Mode,
                        _callbackPtr,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CompilationInfoCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CompilationInfoCallbackInfo>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
type CompilationInfoCallbackInfo2 = 
    {
        Next : IExtension
        Mode : CallbackMode
        Callback : CompilationInfoCallback2
    }
    static member Null = Unchecked.defaultof<CompilationInfoCallbackInfo2>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CompilationInfoCallbackInfo2> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.CompilationInfoCallback2>
                _callbackDel <- WebGPU.Raw.CompilationInfoCallback2(fun status compilationInfo userdata1 userdata2 ->
                    let _status = status
                    let _compilationInfo = let m = NativePtr.toByRef compilationInfo in CompilationInfo.Read(&m)
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _compilationInfo)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.CompilationInfoCallbackInfo2(
                        nextInChain,
                        this.Mode,
                        _callbackPtr,
                        Unchecked.defaultof<_>,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CompilationInfoCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CompilationInfoCallbackInfo2>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
type CompilationMessage = 
    {
        Next : IExtension
        Message : string
        Type : CompilationMessageType
        LineNum : int64
        LinePos : int64
        Offset : int64
        Length : int64
        Utf16LinePos : int64
        Utf16Offset : int64
        Utf16Length : int64
    }
    static member Null = Unchecked.defaultof<CompilationMessage>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CompilationMessage> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _messageArr = Encoding.UTF8.GetBytes(this.Message)
                use _messagePtr = fixed _messageArr
                let _messageLen = WebGPU.Raw.StringView(_messagePtr, unativeint _messageArr.Length)
                let mutable value =
                    new WebGPU.Raw.CompilationMessage(
                        nextInChain,
                        _messageLen,
                        this.Type,
                        uint64(this.LineNum),
                        uint64(this.LinePos),
                        uint64(this.Offset),
                        uint64(this.Length),
                        uint64(this.Utf16LinePos),
                        uint64(this.Utf16Offset),
                        uint64(this.Utf16Length)
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CompilationMessage> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CompilationMessage>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Message = let _messagePtr = NativePtr.toNativeInt(backend.Message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(backend.Message.Length))
            Type = backend.Type
            LineNum = int64(backend.LineNum)
            LinePos = int64(backend.LinePos)
            Offset = int64(backend.Offset)
            Length = int64(backend.Length)
            Utf16LinePos = int64(backend.Utf16LinePos)
            Utf16Offset = int64(backend.Utf16Offset)
            Utf16Length = int64(backend.Utf16Length)
        }
type ComputePassDescriptor = 
    {
        Next : IExtension
        Label : string
        TimestampWrites : ComputePassTimestampWrites
    }
    static member Null = Unchecked.defaultof<ComputePassDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ComputePassDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                this.TimestampWrites.Pin(fun _timestampWritesPtr ->
                    let mutable value =
                        new WebGPU.Raw.ComputePassDescriptor(
                            nextInChain,
                            _labelLen,
                            _timestampWritesPtr
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ComputePassDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ComputePassDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            TimestampWrites = let m = NativePtr.toByRef backend.TimestampWrites in ComputePassTimestampWrites.Read(&m)
        }
type ComputePassEncoder internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"ComputePassEncoder(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? ComputePassEncoder as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new ComputePassEncoder(0n)
    member _.InsertDebugMarker(markerLabel : string) : unit =
        let _markerLabelArr = Encoding.UTF8.GetBytes(markerLabel)
        use _markerLabelPtr = fixed _markerLabelArr
        let _markerLabelLen = WebGPU.Raw.StringView(_markerLabelPtr, unativeint _markerLabelArr.Length)
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderInsertDebugMarker(handle, _markerLabelLen)
        res
    member _.PopDebugGroup() : unit =
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderPopDebugGroup(handle)
        res
    member _.PushDebugGroup(groupLabel : string) : unit =
        let _groupLabelArr = Encoding.UTF8.GetBytes(groupLabel)
        use _groupLabelPtr = fixed _groupLabelArr
        let _groupLabelLen = WebGPU.Raw.StringView(_groupLabelPtr, unativeint _groupLabelArr.Length)
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderPushDebugGroup(handle, _groupLabelLen)
        res
    member _.SetPipeline(pipeline : ComputePipeline) : unit =
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderSetPipeline(handle, pipeline.Handle)
        res
    member _.SetBindGroup(groupIndex : int, group : BindGroup, dynamicOffsets : array<uint32>) : unit =
        use dynamicOffsetsPtr = fixed (dynamicOffsets)
        let dynamicOffsetsLen = unativeint dynamicOffsets.Length
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderSetBindGroup(handle, uint32(groupIndex), group.Handle, dynamicOffsetsLen, dynamicOffsetsPtr)
        res
    member _.WriteTimestamp(querySet : QuerySet, queryIndex : int) : unit =
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderWriteTimestamp(handle, querySet.Handle, uint32(queryIndex))
        res
    member _.DispatchWorkgroups(workgroupCountX : int, workgroupCountY : int, workgroupCountZ : int) : unit =
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderDispatchWorkgroups(handle, uint32(workgroupCountX), uint32(workgroupCountY), uint32(workgroupCountZ))
        res
    member _.DispatchWorkgroupsIndirect(indirectBuffer : Buffer, indirectOffset : int64) : unit =
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderDispatchWorkgroupsIndirect(handle, indirectBuffer.Handle, uint64(indirectOffset))
        res
    member _.End() : unit =
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderEnd(handle)
        res
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderSetLabel(handle, _labelLen)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.ComputePassEncoderRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type ComputePassTimestampWrites = 
    {
        QuerySet : QuerySet
        BeginningOfPassWriteIndex : int
        EndOfPassWriteIndex : int
    }
    static member Null = Unchecked.defaultof<ComputePassTimestampWrites>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ComputePassTimestampWrites> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.ComputePassTimestampWrites(
                    this.QuerySet.Handle,
                    uint32(this.BeginningOfPassWriteIndex),
                    uint32(this.EndOfPassWriteIndex)
                )
            use ptr = fixed &value
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ComputePassTimestampWrites> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ComputePassTimestampWrites>) = 
        {
            QuerySet = new QuerySet(backend.QuerySet)
            BeginningOfPassWriteIndex = int(backend.BeginningOfPassWriteIndex)
            EndOfPassWriteIndex = int(backend.EndOfPassWriteIndex)
        }
type ComputePipeline internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"ComputePipeline(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? ComputePipeline as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new ComputePipeline(0n)
    member _.GetBindGroupLayout(groupIndex : int) : BindGroupLayout =
        let res = WebGPU.Raw.WebGPU.ComputePipelineGetBindGroupLayout(handle, uint32(groupIndex))
        new BindGroupLayout(res)
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.ComputePipelineSetLabel(handle, _labelLen)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.ComputePipelineRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type ComputePipelineDescriptor = 
    {
        Next : IComputePipelineDescriptorExtension
        Label : string
        Layout : PipelineLayout
        Compute : ProgrammableStageDescriptor
    }
    static member Null = Unchecked.defaultof<ComputePipelineDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ComputePipelineDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                this.Compute.Pin(fun _computePtr ->
                    let mutable value =
                        new WebGPU.Raw.ComputePipelineDescriptor(
                            nextInChain,
                            _labelLen,
                            this.Layout.Handle,
                            (if NativePtr.toNativeInt _computePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _computePtr)
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ComputePipelineDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ComputePipelineDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IComputePipelineDescriptorExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            Layout = new PipelineLayout(backend.Layout)
            Compute = ProgrammableStageDescriptor.Read(&backend.Compute)
        }
type DawnComputePipelineFullSubgroups = 
    {
        Next : IComputePipelineDescriptorExtension
        RequiresFullSubgroups : bool
    }
    static member Null = Unchecked.defaultof<DawnComputePipelineFullSubgroups>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DawnComputePipelineFullSubgroups> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnComputePipelineFullSubgroups
                let mutable value =
                    new WebGPU.Raw.DawnComputePipelineFullSubgroups(
                        nextInChain,
                        sType,
                        (if this.RequiresFullSubgroups then 1 else 0)
                    )
                use ptr = fixed &value
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IComputePipelineDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnComputePipelineFullSubgroups> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DawnComputePipelineFullSubgroups>) = 
        {
            Next = ExtensionDecoder.decode<IComputePipelineDescriptorExtension> backend.NextInChain
            RequiresFullSubgroups = (backend.RequiresFullSubgroups <> 0)
        }
type CopyTextureForBrowserOptions = 
    {
        Next : IExtension
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CopyTextureForBrowserOptions> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable srcTransferFunctionParametersHandle = this.SrcTransferFunctionParameters
                use srcTransferFunctionParametersPtr = fixed (&srcTransferFunctionParametersHandle)
                let mutable conversionMatrixHandle = this.ConversionMatrix
                use conversionMatrixPtr = fixed (&conversionMatrixHandle)
                let mutable dstTransferFunctionParametersHandle = this.DstTransferFunctionParameters
                use dstTransferFunctionParametersPtr = fixed (&dstTransferFunctionParametersHandle)
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
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CopyTextureForBrowserOptions> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CopyTextureForBrowserOptions>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            FlipY = (backend.FlipY <> 0)
            NeedsColorSpaceConversion = (backend.NeedsColorSpaceConversion <> 0)
            SrcAlphaMode = backend.SrcAlphaMode
            SrcTransferFunctionParameters = let ptr = backend.SrcTransferFunctionParameters in NativePtr.read ptr
            ConversionMatrix = let ptr = backend.ConversionMatrix in NativePtr.read ptr
            DstTransferFunctionParameters = let ptr = backend.DstTransferFunctionParameters in NativePtr.read ptr
            DstAlphaMode = backend.DstAlphaMode
            InternalUsage = (backend.InternalUsage <> 0)
        }
type CreateComputePipelineAsyncCallback = delegate of IDisposable * status : CreatePipelineAsyncStatus * pipeline : ComputePipeline * message : string -> unit
type CreateComputePipelineAsyncCallback2 = delegate of IDisposable * status : CreatePipelineAsyncStatus * pipeline : ComputePipeline * message : string -> unit
type CreateComputePipelineAsyncCallbackInfo = 
    {
        Next : IExtension
        Mode : CallbackMode
        Callback : CreateComputePipelineAsyncCallback
    }
    static member Null = Unchecked.defaultof<CreateComputePipelineAsyncCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.CreateComputePipelineAsyncCallback>
                _callbackDel <- WebGPU.Raw.CreateComputePipelineAsyncCallback(fun status pipeline message userdata ->
                    let _status = status
                    let _pipeline = new ComputePipeline(pipeline)
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _pipeline, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo(
                        nextInChain,
                        this.Mode,
                        _callbackPtr,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
type CreateComputePipelineAsyncCallbackInfo2 = 
    {
        Next : IExtension
        Mode : CallbackMode
        Callback : CreateComputePipelineAsyncCallback2
    }
    static member Null = Unchecked.defaultof<CreateComputePipelineAsyncCallbackInfo2>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo2> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.CreateComputePipelineAsyncCallback2>
                _callbackDel <- WebGPU.Raw.CreateComputePipelineAsyncCallback2(fun status pipeline message userdata1 userdata2 ->
                    let _status = status
                    let _pipeline = new ComputePipeline(pipeline)
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _pipeline, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo2(
                        nextInChain,
                        this.Mode,
                        _callbackPtr,
                        Unchecked.defaultof<_>,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CreateComputePipelineAsyncCallbackInfo2>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
type CreateRenderPipelineAsyncCallback = delegate of IDisposable * status : CreatePipelineAsyncStatus * pipeline : RenderPipeline * message : string -> unit
type CreateRenderPipelineAsyncCallback2 = delegate of IDisposable * status : CreatePipelineAsyncStatus * pipeline : RenderPipeline * message : string -> unit
type CreateRenderPipelineAsyncCallbackInfo = 
    {
        Next : IExtension
        Mode : CallbackMode
        Callback : CreateRenderPipelineAsyncCallback
    }
    static member Null = Unchecked.defaultof<CreateRenderPipelineAsyncCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.CreateRenderPipelineAsyncCallback>
                _callbackDel <- WebGPU.Raw.CreateRenderPipelineAsyncCallback(fun status pipeline message userdata ->
                    let _status = status
                    let _pipeline = new RenderPipeline(pipeline)
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _pipeline, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo(
                        nextInChain,
                        this.Mode,
                        _callbackPtr,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
type CreateRenderPipelineAsyncCallbackInfo2 = 
    {
        Next : IExtension
        Mode : CallbackMode
        Callback : CreateRenderPipelineAsyncCallback2
    }
    static member Null = Unchecked.defaultof<CreateRenderPipelineAsyncCallbackInfo2>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo2> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.CreateRenderPipelineAsyncCallback2>
                _callbackDel <- WebGPU.Raw.CreateRenderPipelineAsyncCallback2(fun status pipeline message userdata1 userdata2 ->
                    let _status = status
                    let _pipeline = new RenderPipeline(pipeline)
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _pipeline, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo2(
                        nextInChain,
                        this.Mode,
                        _callbackPtr,
                        Unchecked.defaultof<_>,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.CreateRenderPipelineAsyncCallbackInfo2>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
type AHardwareBufferProperties = 
    {
        YCbCrInfo : YCbCrVkDescriptor
    }
    static member Null = Unchecked.defaultof<AHardwareBufferProperties>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.AHardwareBufferProperties> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            this.YCbCrInfo.Pin(fun _yCbCrInfoPtr ->
                let mutable value =
                    new WebGPU.Raw.AHardwareBufferProperties(
                        (if NativePtr.toNativeInt _yCbCrInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _yCbCrInfoPtr)
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.AHardwareBufferProperties> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.AHardwareBufferProperties>) = 
        {
            YCbCrInfo = YCbCrVkDescriptor.Read(&backend.YCbCrInfo)
        }
type Device internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"Device(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? Device as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new Device(0n)
    member _.CreateBindGroup(descriptor : BindGroupDescriptor) : BindGroup =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateBindGroup(handle, _descriptorPtr)
            new BindGroup(res)
        )
    member _.CreateBindGroupLayout(descriptor : BindGroupLayoutDescriptor) : BindGroupLayout =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateBindGroupLayout(handle, _descriptorPtr)
            new BindGroupLayout(res)
        )
    member _.CreateBuffer(descriptor : BufferDescriptor) : Buffer =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateBuffer(handle, _descriptorPtr)
            new Buffer(res)
        )
    member _.CreateErrorBuffer(descriptor : BufferDescriptor) : Buffer =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateErrorBuffer(handle, _descriptorPtr)
            new Buffer(res)
        )
    member _.CreateCommandEncoder(descriptor : CommandEncoderDescriptor) : CommandEncoder =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateCommandEncoder(handle, _descriptorPtr)
            new CommandEncoder(res)
        )
    member _.CreateComputePipeline(descriptor : ComputePipelineDescriptor) : ComputePipeline =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateComputePipeline(handle, _descriptorPtr)
            new ComputePipeline(res)
        )
    member _.CreateComputePipelineAsync(descriptor : ComputePipelineDescriptor, callback : CreateComputePipelineAsyncCallback) : unit =
        descriptor.Pin(fun _descriptorPtr ->
            let mutable _callbackGC = Unchecked.defaultof<GCHandle>
            let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.CreateComputePipelineAsyncCallback>
            _callbackDel <- WebGPU.Raw.CreateComputePipelineAsyncCallback(fun status pipeline message userdata ->
                let _status = status
                let _pipeline = new ComputePipeline(pipeline)
                let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _pipeline, _message)
            )
            _callbackGC <- GCHandle.Alloc(_callbackDel)
            let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
            let res = WebGPU.Raw.WebGPU.DeviceCreateComputePipelineAsync(handle, _descriptorPtr, _callbackPtr, Unchecked.defaultof<_>)
            res
        )
    member _.CreateComputePipelineAsyncF(descriptor : ComputePipelineDescriptor, callbackInfo : CreateComputePipelineAsyncCallbackInfo) : Future =
        descriptor.Pin(fun _descriptorPtr ->
            callbackInfo.Pin(fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.DeviceCreateComputePipelineAsyncF(handle, _descriptorPtr, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
                Future.Read(&res)
            )
        )
    member _.CreateComputePipelineAsync2(descriptor : ComputePipelineDescriptor, callbackInfo : CreateComputePipelineAsyncCallbackInfo2) : Future =
        descriptor.Pin(fun _descriptorPtr ->
            callbackInfo.Pin(fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.DeviceCreateComputePipelineAsync2(handle, _descriptorPtr, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
                Future.Read(&res)
            )
        )
    member _.CreateExternalTexture(externalTextureDescriptor : ExternalTextureDescriptor) : ExternalTexture =
        externalTextureDescriptor.Pin(fun _externalTextureDescriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateExternalTexture(handle, _externalTextureDescriptorPtr)
            new ExternalTexture(res)
        )
    member _.CreateErrorExternalTexture() : ExternalTexture =
        let res = WebGPU.Raw.WebGPU.DeviceCreateErrorExternalTexture(handle)
        new ExternalTexture(res)
    member _.CreatePipelineLayout(descriptor : PipelineLayoutDescriptor) : PipelineLayout =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreatePipelineLayout(handle, _descriptorPtr)
            new PipelineLayout(res)
        )
    member _.CreateQuerySet(descriptor : QuerySetDescriptor) : QuerySet =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateQuerySet(handle, _descriptorPtr)
            new QuerySet(res)
        )
    member _.CreateRenderPipelineAsync(descriptor : RenderPipelineDescriptor, callback : CreateRenderPipelineAsyncCallback) : unit =
        descriptor.Pin(fun _descriptorPtr ->
            let mutable _callbackGC = Unchecked.defaultof<GCHandle>
            let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.CreateRenderPipelineAsyncCallback>
            _callbackDel <- WebGPU.Raw.CreateRenderPipelineAsyncCallback(fun status pipeline message userdata ->
                let _status = status
                let _pipeline = new RenderPipeline(pipeline)
                let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _pipeline, _message)
            )
            _callbackGC <- GCHandle.Alloc(_callbackDel)
            let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
            let res = WebGPU.Raw.WebGPU.DeviceCreateRenderPipelineAsync(handle, _descriptorPtr, _callbackPtr, Unchecked.defaultof<_>)
            res
        )
    member _.CreateRenderPipelineAsyncF(descriptor : RenderPipelineDescriptor, callbackInfo : CreateRenderPipelineAsyncCallbackInfo) : Future =
        descriptor.Pin(fun _descriptorPtr ->
            callbackInfo.Pin(fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.DeviceCreateRenderPipelineAsyncF(handle, _descriptorPtr, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
                Future.Read(&res)
            )
        )
    member _.CreateRenderPipelineAsync2(descriptor : RenderPipelineDescriptor, callbackInfo : CreateRenderPipelineAsyncCallbackInfo2) : Future =
        descriptor.Pin(fun _descriptorPtr ->
            callbackInfo.Pin(fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.DeviceCreateRenderPipelineAsync2(handle, _descriptorPtr, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
                Future.Read(&res)
            )
        )
    member _.CreateRenderBundleEncoder(descriptor : RenderBundleEncoderDescriptor) : RenderBundleEncoder =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateRenderBundleEncoder(handle, _descriptorPtr)
            new RenderBundleEncoder(res)
        )
    member _.CreateRenderPipeline(descriptor : RenderPipelineDescriptor) : RenderPipeline =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateRenderPipeline(handle, _descriptorPtr)
            new RenderPipeline(res)
        )
    member _.CreateSampler(descriptor : SamplerDescriptor) : Sampler =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateSampler(handle, _descriptorPtr)
            new Sampler(res)
        )
    member _.CreateShaderModule(descriptor : ShaderModuleDescriptor) : ShaderModule =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateShaderModule(handle, _descriptorPtr)
            new ShaderModule(res)
        )
    member _.CreateErrorShaderModule(descriptor : ShaderModuleDescriptor, errorMessage : string) : ShaderModule =
        descriptor.Pin(fun _descriptorPtr ->
            let _errorMessageArr = Encoding.UTF8.GetBytes(errorMessage)
            use _errorMessagePtr = fixed _errorMessageArr
            let _errorMessageLen = WebGPU.Raw.StringView(_errorMessagePtr, unativeint _errorMessageArr.Length)
            let res = WebGPU.Raw.WebGPU.DeviceCreateErrorShaderModule(handle, _descriptorPtr, _errorMessageLen)
            new ShaderModule(res)
        )
    member _.CreateTexture(descriptor : TextureDescriptor) : Texture =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateTexture(handle, _descriptorPtr)
            new Texture(res)
        )
    member _.ImportSharedBufferMemory(descriptor : SharedBufferMemoryDescriptor) : SharedBufferMemory =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceImportSharedBufferMemory(handle, _descriptorPtr)
            new SharedBufferMemory(res)
        )
    member _.ImportSharedTextureMemory(descriptor : SharedTextureMemoryDescriptor) : SharedTextureMemory =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceImportSharedTextureMemory(handle, _descriptorPtr)
            new SharedTextureMemory(res)
        )
    member _.ImportSharedFence(descriptor : SharedFenceDescriptor) : SharedFence =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceImportSharedFence(handle, _descriptorPtr)
            new SharedFence(res)
        )
    member _.CreateErrorTexture(descriptor : TextureDescriptor) : Texture =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceCreateErrorTexture(handle, _descriptorPtr)
            new Texture(res)
        )
    member _.Destroy() : unit =
        let res = WebGPU.Raw.WebGPU.DeviceDestroy(handle)
        res
    member _.GetAHardwareBufferProperties(handle : nativeint, properties : byref<AHardwareBufferProperties>) : Status =
        let mutable propertiesCopy = properties
        try
            properties.Pin(fun _propertiesPtr ->
                if NativePtr.toNativeInt _propertiesPtr = 0n then
                    let mutable propertiesNative = Unchecked.defaultof<WebGPU.Raw.AHardwareBufferProperties>
                    use _propertiesPtr = fixed &propertiesNative
                    let res = WebGPU.Raw.WebGPU.DeviceGetAHardwareBufferProperties(handle, handle, _propertiesPtr)
                    let _ret = res
                    propertiesCopy <- AHardwareBufferProperties.Read(&propertiesNative)
                    _ret
                else
                    let res = WebGPU.Raw.WebGPU.DeviceGetAHardwareBufferProperties(handle, handle, _propertiesPtr)
                    let _ret = res
                    let propertiesResult = NativePtr.toByRef _propertiesPtr
                    propertiesCopy <- AHardwareBufferProperties.Read(&propertiesResult)
                    _ret
            )
        finally
            properties <- propertiesCopy
    member _.Limits : SupportedLimits =
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        let status = WebGPU.Raw.WebGPU.DeviceGetLimits(handle, ptr)
        if status <> Status.Success then failwith "GetLimits failed"
        SupportedLimits.Read(&res)
    member this.LostFuture : Future =
        let mutable res = WebGPU.Raw.WebGPU.DeviceGetLostFuture(handle)
        Future.Read(&res)
    member _.HasFeature(feature : FeatureName) : bool =
        let res = WebGPU.Raw.WebGPU.DeviceHasFeature(handle, feature)
        (res <> 0)
    member _.Features : SupportedFeatures =
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        WebGPU.Raw.WebGPU.DeviceGetFeatures(handle, ptr)
        SupportedFeatures.Read(&res)
    member _.AdapterInfo : AdapterInfo =
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        let status = WebGPU.Raw.WebGPU.DeviceGetAdapterInfo(handle, ptr)
        if status <> Status.Success then failwith "GetAdapterInfo failed"
        AdapterInfo.Read(&res)
    member this.Adapter : Adapter =
        let mutable res = WebGPU.Raw.WebGPU.DeviceGetAdapter(handle)
        new Adapter(res)
    member this.Queue : Queue =
        let mutable res = WebGPU.Raw.WebGPU.DeviceGetQueue(handle)
        new Queue(res)
    member _.InjectError(typ : ErrorType, message : string) : unit =
        let _messageArr = Encoding.UTF8.GetBytes(message)
        use _messagePtr = fixed _messageArr
        let _messageLen = WebGPU.Raw.StringView(_messagePtr, unativeint _messageArr.Length)
        let res = WebGPU.Raw.WebGPU.DeviceInjectError(handle, typ, _messageLen)
        res
    member _.ForceLoss(typ : DeviceLostReason, message : string) : unit =
        let _messageArr = Encoding.UTF8.GetBytes(message)
        use _messagePtr = fixed _messageArr
        let _messageLen = WebGPU.Raw.StringView(_messagePtr, unativeint _messageArr.Length)
        let res = WebGPU.Raw.WebGPU.DeviceForceLoss(handle, typ, _messageLen)
        res
    member _.Tick() : unit =
        let res = WebGPU.Raw.WebGPU.DeviceTick(handle)
        res
    member _.SetUncapturedErrorCallback(callback : ErrorCallback) : unit =
        let mutable _callbackGC = Unchecked.defaultof<GCHandle>
        let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.ErrorCallback>
        _callbackDel <- WebGPU.Raw.ErrorCallback(fun typ message userdata ->
            let _typ = typ
            let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
            callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _typ, _message)
        )
        _callbackGC <- GCHandle.Alloc(_callbackDel)
        let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
        let res = WebGPU.Raw.WebGPU.DeviceSetUncapturedErrorCallback(handle, _callbackPtr, Unchecked.defaultof<_>)
        res
    member _.SetLoggingCallback(callback : LoggingCallback) : unit =
        let mutable _callbackGC = Unchecked.defaultof<GCHandle>
        let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.LoggingCallback>
        _callbackDel <- WebGPU.Raw.LoggingCallback(fun typ message userdata ->
            let _typ = typ
            let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
            callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _typ, _message)
        )
        _callbackGC <- GCHandle.Alloc(_callbackDel)
        let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
        let res = WebGPU.Raw.WebGPU.DeviceSetLoggingCallback(handle, _callbackPtr, Unchecked.defaultof<_>)
        res
    member _.PushErrorScope(filter : ErrorFilter) : unit =
        let res = WebGPU.Raw.WebGPU.DevicePushErrorScope(handle, filter)
        res
    member _.PopErrorScope(oldCallback : ErrorCallback) : unit =
        let mutable _oldCallbackGC = Unchecked.defaultof<GCHandle>
        let mutable _oldCallbackDel = Unchecked.defaultof<WebGPU.Raw.ErrorCallback>
        _oldCallbackDel <- WebGPU.Raw.ErrorCallback(fun typ message userdata ->
            let _typ = typ
            let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
            oldCallback.Invoke({ new IDisposable with member __.Dispose() = _oldCallbackGC.Free() }, _typ, _message)
        )
        _oldCallbackGC <- GCHandle.Alloc(_oldCallbackDel)
        let _oldCallbackPtr = Marshal.GetFunctionPointerForDelegate(_oldCallbackDel)
        let res = WebGPU.Raw.WebGPU.DevicePopErrorScope(handle, _oldCallbackPtr, Unchecked.defaultof<_>)
        res
    member _.PopErrorScopeF(callbackInfo : PopErrorScopeCallbackInfo) : Future =
        callbackInfo.Pin(fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.DevicePopErrorScopeF(handle, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
            Future.Read(&res)
        )
    member _.PopErrorScope2(callbackInfo : PopErrorScopeCallbackInfo2) : Future =
        callbackInfo.Pin(fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.DevicePopErrorScope2(handle, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
            Future.Read(&res)
        )
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.DeviceSetLabel(handle, _labelLen)
        res
    member _.ValidateTextureDescriptor(descriptor : TextureDescriptor) : unit =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.DeviceValidateTextureDescriptor(handle, _descriptorPtr)
            res
        )
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.DeviceRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type DeviceLostCallback = delegate of IDisposable * reason : DeviceLostReason * message : string -> unit
type DeviceLostCallbackNew = delegate of IDisposable * device : Device * reason : DeviceLostReason * message : string -> unit
type DeviceLostCallback2 = delegate of IDisposable * device : Device * reason : DeviceLostReason * message : string -> unit
type DeviceLostCallbackInfo = 
    {
        Next : IExtension
        Mode : CallbackMode
        Callback : DeviceLostCallbackNew
    }
    static member Null = Unchecked.defaultof<DeviceLostCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DeviceLostCallbackInfo> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.DeviceLostCallbackNew>
                _callbackDel <- WebGPU.Raw.DeviceLostCallbackNew(fun device reason message userdata ->
                    let _device = let ptr = device in new Device(NativePtr.read ptr)
                    let _reason = reason
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _device, _reason, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.DeviceLostCallbackInfo(
                        nextInChain,
                        this.Mode,
                        _callbackPtr,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DeviceLostCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DeviceLostCallbackInfo>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
type DeviceLostCallbackInfo2 = 
    {
        Next : IExtension
        Mode : CallbackMode
        Callback : DeviceLostCallback2
    }
    static member Null = Unchecked.defaultof<DeviceLostCallbackInfo2>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DeviceLostCallbackInfo2> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.DeviceLostCallback2>
                _callbackDel <- WebGPU.Raw.DeviceLostCallback2(fun device reason message userdata1 userdata2 ->
                    let _device = let ptr = device in new Device(NativePtr.read ptr)
                    let _reason = reason
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _device, _reason, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.DeviceLostCallbackInfo2(
                        nextInChain,
                        this.Mode,
                        _callbackPtr,
                        Unchecked.defaultof<_>,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DeviceLostCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DeviceLostCallbackInfo2>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
type ErrorCallback = delegate of IDisposable * typ : ErrorType * message : string -> unit
type UncapturedErrorCallback = delegate of IDisposable * device : Device * typ : ErrorType * message : string -> unit
type UncapturedErrorCallbackInfo = 
    {
        Next : IExtension
        Callback : ErrorCallback
    }
    static member Null = Unchecked.defaultof<UncapturedErrorCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.UncapturedErrorCallbackInfo> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.ErrorCallback>
                _callbackDel <- WebGPU.Raw.ErrorCallback(fun typ message userdata ->
                    let _typ = typ
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _typ, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.UncapturedErrorCallbackInfo(
                        nextInChain,
                        _callbackPtr,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.UncapturedErrorCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.UncapturedErrorCallbackInfo>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (next in chain, backend.NextInChain); (userdata, backend.Userdata)]
        }
type UncapturedErrorCallbackInfo2 = 
    {
        Next : IExtension
        Callback : UncapturedErrorCallback
    }
    static member Null = Unchecked.defaultof<UncapturedErrorCallbackInfo2>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.UncapturedErrorCallbackInfo2> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.UncapturedErrorCallback>
                _callbackDel <- WebGPU.Raw.UncapturedErrorCallback(fun device typ message userdata1 userdata2 ->
                    let _device = let ptr = device in new Device(NativePtr.read ptr)
                    let _typ = typ
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _device, _typ, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.UncapturedErrorCallbackInfo2(
                        nextInChain,
                        _callbackPtr,
                        Unchecked.defaultof<_>,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.UncapturedErrorCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.UncapturedErrorCallbackInfo2>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (next in chain, backend.NextInChain); (userdata1, backend.Userdata1); ... ]
        }
type PopErrorScopeCallback = delegate of IDisposable * status : PopErrorScopeStatus * typ : ErrorType * message : string -> unit
type PopErrorScopeCallback2 = delegate of IDisposable * status : PopErrorScopeStatus * typ : ErrorType * message : string -> unit
type PopErrorScopeCallbackInfo = 
    {
        Next : IExtension
        Mode : CallbackMode
        Callback : PopErrorScopeCallback
        OldCallback : ErrorCallback
    }
    static member Null = Unchecked.defaultof<PopErrorScopeCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.PopErrorScopeCallbackInfo> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.PopErrorScopeCallback>
                _callbackDel <- WebGPU.Raw.PopErrorScopeCallback(fun status typ message userdata ->
                    let _status = status
                    let _typ = typ
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _typ, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable _oldCallbackGC = Unchecked.defaultof<GCHandle>
                let mutable _oldCallbackDel = Unchecked.defaultof<WebGPU.Raw.ErrorCallback>
                _oldCallbackDel <- WebGPU.Raw.ErrorCallback(fun typ message userdata ->
                    let _typ = typ
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.OldCallback.Invoke({ new IDisposable with member __.Dispose() = _oldCallbackGC.Free() }, _typ, _message)
                )
                _oldCallbackGC <- GCHandle.Alloc(_oldCallbackDel)
                let _oldCallbackPtr = Marshal.GetFunctionPointerForDelegate(_oldCallbackDel)
                let mutable value =
                    new WebGPU.Raw.PopErrorScopeCallbackInfo(
                        nextInChain,
                        this.Mode,
                        _callbackPtr,
                        _oldCallbackPtr,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.PopErrorScopeCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.PopErrorScopeCallbackInfo>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
            OldCallback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
type PopErrorScopeCallbackInfo2 = 
    {
        Next : IExtension
        Mode : CallbackMode
        Callback : PopErrorScopeCallback2
    }
    static member Null = Unchecked.defaultof<PopErrorScopeCallbackInfo2>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.PopErrorScopeCallbackInfo2> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.PopErrorScopeCallback2>
                _callbackDel <- WebGPU.Raw.PopErrorScopeCallback2(fun status typ message userdata1 userdata2 ->
                    let _status = status
                    let _typ = typ
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _typ, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.PopErrorScopeCallbackInfo2(
                        nextInChain,
                        this.Mode,
                        _callbackPtr,
                        Unchecked.defaultof<_>,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.PopErrorScopeCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.PopErrorScopeCallbackInfo2>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
type Limits = 
    {
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
        MaxInterStageShaderComponents : int
        MaxInterStageShaderVariables : int
        MaxColorAttachments : int
        MaxColorAttachmentBytesPerSample : int
        MaxComputeWorkgroupStorageSize : int
        MaxComputeInvocationsPerWorkgroup : int
        MaxComputeWorkgroupSizeX : int
        MaxComputeWorkgroupSizeY : int
        MaxComputeWorkgroupSizeZ : int
        MaxComputeWorkgroupsPerDimension : int
    }
    static member Null = Unchecked.defaultof<Limits>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.Limits> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.Limits(
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
                    uint32(this.MaxInterStageShaderComponents),
                    uint32(this.MaxInterStageShaderVariables),
                    uint32(this.MaxColorAttachments),
                    uint32(this.MaxColorAttachmentBytesPerSample),
                    uint32(this.MaxComputeWorkgroupStorageSize),
                    uint32(this.MaxComputeInvocationsPerWorkgroup),
                    uint32(this.MaxComputeWorkgroupSizeX),
                    uint32(this.MaxComputeWorkgroupSizeY),
                    uint32(this.MaxComputeWorkgroupSizeZ),
                    uint32(this.MaxComputeWorkgroupsPerDimension)
                )
            use ptr = fixed &value
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.Limits> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.Limits>) = 
        {
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
            MaxInterStageShaderComponents = int(backend.MaxInterStageShaderComponents)
            MaxInterStageShaderVariables = int(backend.MaxInterStageShaderVariables)
            MaxColorAttachments = int(backend.MaxColorAttachments)
            MaxColorAttachmentBytesPerSample = int(backend.MaxColorAttachmentBytesPerSample)
            MaxComputeWorkgroupStorageSize = int(backend.MaxComputeWorkgroupStorageSize)
            MaxComputeInvocationsPerWorkgroup = int(backend.MaxComputeInvocationsPerWorkgroup)
            MaxComputeWorkgroupSizeX = int(backend.MaxComputeWorkgroupSizeX)
            MaxComputeWorkgroupSizeY = int(backend.MaxComputeWorkgroupSizeY)
            MaxComputeWorkgroupSizeZ = int(backend.MaxComputeWorkgroupSizeZ)
            MaxComputeWorkgroupsPerDimension = int(backend.MaxComputeWorkgroupsPerDimension)
        }
type DawnExperimentalSubgroupLimits = 
    {
        Next : ISupportedLimitsExtension
        MinSubgroupSize : int
        MaxSubgroupSize : int
    }
    static member Null = Unchecked.defaultof<DawnExperimentalSubgroupLimits>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DawnExperimentalSubgroupLimits> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnExperimentalSubgroupLimits
                let mutable value =
                    new WebGPU.Raw.DawnExperimentalSubgroupLimits(
                        nextInChain,
                        sType,
                        uint32(this.MinSubgroupSize),
                        uint32(this.MaxSubgroupSize)
                    )
                use ptr = fixed &value
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISupportedLimitsExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnExperimentalSubgroupLimits> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DawnExperimentalSubgroupLimits>) = 
        {
            Next = ExtensionDecoder.decode<ISupportedLimitsExtension> backend.NextInChain
            MinSubgroupSize = int(backend.MinSubgroupSize)
            MaxSubgroupSize = int(backend.MaxSubgroupSize)
        }
type DawnExperimentalImmediateDataLimits = 
    {
        Next : ISupportedLimitsExtension
        MaxImmediateDataRangeByteSize : int
    }
    static member Null = Unchecked.defaultof<DawnExperimentalImmediateDataLimits>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DawnExperimentalImmediateDataLimits> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DawnExperimentalImmediateDataLimits
                let mutable value =
                    new WebGPU.Raw.DawnExperimentalImmediateDataLimits(
                        nextInChain,
                        sType,
                        uint32(this.MaxImmediateDataRangeByteSize)
                    )
                use ptr = fixed &value
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISupportedLimitsExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnExperimentalImmediateDataLimits> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DawnExperimentalImmediateDataLimits>) = 
        {
            Next = ExtensionDecoder.decode<ISupportedLimitsExtension> backend.NextInChain
            MaxImmediateDataRangeByteSize = int(backend.MaxImmediateDataRangeByteSize)
        }
type RequiredLimits = 
    {
        Next : IExtension
        Limits : Limits
    }
    static member Null = Unchecked.defaultof<RequiredLimits>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RequiredLimits> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                this.Limits.Pin(fun _limitsPtr ->
                    let mutable value =
                        new WebGPU.Raw.RequiredLimits(
                            nextInChain,
                            (if NativePtr.toNativeInt _limitsPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _limitsPtr)
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RequiredLimits> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RequiredLimits>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Limits = Limits.Read(&backend.Limits)
        }
type SupportedLimits = 
    {
        Next : ISupportedLimitsExtension
        Limits : Limits
    }
    static member Null = Unchecked.defaultof<SupportedLimits>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SupportedLimits> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                this.Limits.Pin(fun _limitsPtr ->
                    let mutable value =
                        new WebGPU.Raw.SupportedLimits(
                            nextInChain,
                            (if NativePtr.toNativeInt _limitsPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _limitsPtr)
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SupportedLimits> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SupportedLimits>) = 
        {
            Next = ExtensionDecoder.decode<ISupportedLimitsExtension> backend.NextInChain
            Limits = Limits.Read(&backend.Limits)
        }
type SupportedFeatures = 
    {
        Features : array<FeatureName>
    }
    static member Null = Unchecked.defaultof<SupportedFeatures>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SupportedFeatures> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            use featuresPtr = fixed (this.Features)
            let featuresLen = unativeint this.Features.Length
            let mutable value =
                new WebGPU.Raw.SupportedFeatures(
                    featuresLen,
                    featuresPtr
                )
            use ptr = fixed &value
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SupportedFeatures> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SupportedFeatures>) = 
        {
            Features = let ptr = backend.Features in Array.init (int backend.FeatureCount) (fun i -> NativePtr.get ptr i)
        }
type LoggingCallback = delegate of IDisposable * typ : LoggingType * message : string -> unit
type Extent2D = 
    {
        Width : int
        Height : int
    }
    static member Null = Unchecked.defaultof<Extent2D>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.Extent2D> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.Extent2D(
                    uint32(this.Width),
                    uint32(this.Height)
                )
            use ptr = fixed &value
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.Extent2D> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.Extent2D>) = 
        {
            Width = int(backend.Width)
            Height = int(backend.Height)
        }
type Extent3D = 
    {
        Width : int
        Height : int
        DepthOrArrayLayers : int
    }
    static member Null = Unchecked.defaultof<Extent3D>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.Extent3D> -> 'r) : 'r = 
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
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.Extent3D> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.Extent3D>) = 
        {
            Width = int(backend.Width)
            Height = int(backend.Height)
            DepthOrArrayLayers = int(backend.DepthOrArrayLayers)
        }
type ExternalTexture internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"ExternalTexture(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? ExternalTexture as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new ExternalTexture(0n)
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.ExternalTextureSetLabel(handle, _labelLen)
        res
    member _.Destroy() : unit =
        let res = WebGPU.Raw.WebGPU.ExternalTextureDestroy(handle)
        res
    member _.Expire() : unit =
        let res = WebGPU.Raw.WebGPU.ExternalTextureExpire(handle)
        res
    member _.Refresh() : unit =
        let res = WebGPU.Raw.WebGPU.ExternalTextureRefresh(handle)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.ExternalTextureRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type ExternalTextureDescriptor = 
    {
        Next : IExtension
        Label : string
        Plane0 : TextureView
        Plane1 : TextureView
        VisibleOrigin : Origin2D
        VisibleSize : Extent2D
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ExternalTextureDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                this.VisibleOrigin.Pin(fun _visibleOriginPtr ->
                    this.VisibleSize.Pin(fun _visibleSizePtr ->
                        this.CropOrigin.Pin(fun _cropOriginPtr ->
                            this.CropSize.Pin(fun _cropSizePtr ->
                                this.ApparentSize.Pin(fun _apparentSizePtr ->
                                    let mutable yuvToRgbConversionMatrixHandle = this.YuvToRgbConversionMatrix
                                    use yuvToRgbConversionMatrixPtr = fixed (&yuvToRgbConversionMatrixHandle)
                                    let mutable srcTransferFunctionParametersHandle = this.SrcTransferFunctionParameters
                                    use srcTransferFunctionParametersPtr = fixed (&srcTransferFunctionParametersHandle)
                                    let mutable dstTransferFunctionParametersHandle = this.DstTransferFunctionParameters
                                    use dstTransferFunctionParametersPtr = fixed (&dstTransferFunctionParametersHandle)
                                    let mutable gamutConversionMatrixHandle = this.GamutConversionMatrix
                                    use gamutConversionMatrixPtr = fixed (&gamutConversionMatrixHandle)
                                    let mutable value =
                                        new WebGPU.Raw.ExternalTextureDescriptor(
                                            nextInChain,
                                            _labelLen,
                                            this.Plane0.Handle,
                                            this.Plane1.Handle,
                                            (if NativePtr.toNativeInt _visibleOriginPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _visibleOriginPtr),
                                            (if NativePtr.toNativeInt _visibleSizePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _visibleSizePtr),
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
                                    action ptr
                                )
                            )
                        )
                    )
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ExternalTextureDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ExternalTextureDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            Plane0 = new TextureView(backend.Plane0)
            Plane1 = new TextureView(backend.Plane1)
            VisibleOrigin = Origin2D.Read(&backend.VisibleOrigin)
            VisibleSize = Extent2D.Read(&backend.VisibleSize)
            CropOrigin = Origin2D.Read(&backend.CropOrigin)
            CropSize = Extent2D.Read(&backend.CropSize)
            ApparentSize = Extent2D.Read(&backend.ApparentSize)
            DoYuvToRgbConversionOnly = (backend.DoYuvToRgbConversionOnly <> 0)
            YuvToRgbConversionMatrix = let ptr = backend.YuvToRgbConversionMatrix in NativePtr.read ptr
            SrcTransferFunctionParameters = let ptr = backend.SrcTransferFunctionParameters in NativePtr.read ptr
            DstTransferFunctionParameters = let ptr = backend.DstTransferFunctionParameters in NativePtr.read ptr
            GamutConversionMatrix = let ptr = backend.GamutConversionMatrix in NativePtr.read ptr
            Mirrored = (backend.Mirrored <> 0)
            Rotation = backend.Rotation
        }
type SharedBufferMemory internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"SharedBufferMemory(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? SharedBufferMemory as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new SharedBufferMemory(0n)
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.SharedBufferMemorySetLabel(handle, _labelLen)
        res
    member _.Properties : SharedBufferMemoryProperties =
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        let status = WebGPU.Raw.WebGPU.SharedBufferMemoryGetProperties(handle, ptr)
        if status <> Status.Success then failwith "GetProperties failed"
        SharedBufferMemoryProperties.Read(&res)
    member _.CreateBuffer(descriptor : BufferDescriptor) : Buffer =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.SharedBufferMemoryCreateBuffer(handle, _descriptorPtr)
            new Buffer(res)
        )
    member _.BeginAccess(buffer : Buffer, descriptor : SharedBufferMemoryBeginAccessDescriptor) : Status =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.SharedBufferMemoryBeginAccess(handle, buffer.Handle, _descriptorPtr)
            res
        )
    member _.EndAccess(buffer : Buffer, descriptor : byref<SharedBufferMemoryEndAccessState>) : Status =
        let mutable descriptorCopy = descriptor
        try
            descriptor.Pin(fun _descriptorPtr ->
                if NativePtr.toNativeInt _descriptorPtr = 0n then
                    let mutable descriptorNative = Unchecked.defaultof<WebGPU.Raw.SharedBufferMemoryEndAccessState>
                    use _descriptorPtr = fixed &descriptorNative
                    let res = WebGPU.Raw.WebGPU.SharedBufferMemoryEndAccess(handle, buffer.Handle, _descriptorPtr)
                    let _ret = res
                    descriptorCopy <- SharedBufferMemoryEndAccessState.Read(&descriptorNative)
                    _ret
                else
                    let res = WebGPU.Raw.WebGPU.SharedBufferMemoryEndAccess(handle, buffer.Handle, _descriptorPtr)
                    let _ret = res
                    let descriptorResult = NativePtr.toByRef _descriptorPtr
                    descriptorCopy <- SharedBufferMemoryEndAccessState.Read(&descriptorResult)
                    _ret
            )
        finally
            descriptor <- descriptorCopy
    member _.IsDeviceLost() : bool =
        let res = WebGPU.Raw.WebGPU.SharedBufferMemoryIsDeviceLost(handle)
        (res <> 0)
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.SharedBufferMemoryRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type SharedBufferMemoryProperties = 
    {
        Next : IExtension
        Usage : BufferUsage
        Size : int64
    }
    static member Null = Unchecked.defaultof<SharedBufferMemoryProperties>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedBufferMemoryProperties> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable value =
                    new WebGPU.Raw.SharedBufferMemoryProperties(
                        nextInChain,
                        this.Usage,
                        uint64(this.Size)
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedBufferMemoryProperties> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedBufferMemoryProperties>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Usage = backend.Usage
            Size = int64(backend.Size)
        }
type SharedBufferMemoryDescriptor = 
    {
        Next : IExtension
        Label : string
    }
    static member Null = Unchecked.defaultof<SharedBufferMemoryDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedBufferMemoryDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                let mutable value =
                    new WebGPU.Raw.SharedBufferMemoryDescriptor(
                        nextInChain,
                        _labelLen
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedBufferMemoryDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedBufferMemoryDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
        }
type SharedTextureMemory internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"SharedTextureMemory(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? SharedTextureMemory as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new SharedTextureMemory(0n)
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.SharedTextureMemorySetLabel(handle, _labelLen)
        res
    member _.Properties : SharedTextureMemoryProperties =
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        let status = WebGPU.Raw.WebGPU.SharedTextureMemoryGetProperties(handle, ptr)
        if status <> Status.Success then failwith "GetProperties failed"
        SharedTextureMemoryProperties.Read(&res)
    member _.CreateTexture(descriptor : TextureDescriptor) : Texture =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.SharedTextureMemoryCreateTexture(handle, _descriptorPtr)
            new Texture(res)
        )
    member _.BeginAccess(texture : Texture, descriptor : SharedTextureMemoryBeginAccessDescriptor) : Status =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.SharedTextureMemoryBeginAccess(handle, texture.Handle, _descriptorPtr)
            res
        )
    member _.EndAccess(texture : Texture, descriptor : byref<SharedTextureMemoryEndAccessState>) : Status =
        let mutable descriptorCopy = descriptor
        try
            descriptor.Pin(fun _descriptorPtr ->
                if NativePtr.toNativeInt _descriptorPtr = 0n then
                    let mutable descriptorNative = Unchecked.defaultof<WebGPU.Raw.SharedTextureMemoryEndAccessState>
                    use _descriptorPtr = fixed &descriptorNative
                    let res = WebGPU.Raw.WebGPU.SharedTextureMemoryEndAccess(handle, texture.Handle, _descriptorPtr)
                    let _ret = res
                    descriptorCopy <- SharedTextureMemoryEndAccessState.Read(&descriptorNative)
                    _ret
                else
                    let res = WebGPU.Raw.WebGPU.SharedTextureMemoryEndAccess(handle, texture.Handle, _descriptorPtr)
                    let _ret = res
                    let descriptorResult = NativePtr.toByRef _descriptorPtr
                    descriptorCopy <- SharedTextureMemoryEndAccessState.Read(&descriptorResult)
                    _ret
            )
        finally
            descriptor <- descriptorCopy
    member _.IsDeviceLost() : bool =
        let res = WebGPU.Raw.WebGPU.SharedTextureMemoryIsDeviceLost(handle)
        (res <> 0)
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.SharedTextureMemoryRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedTextureMemoryProperties> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                this.Size.Pin(fun _sizePtr ->
                    let mutable value =
                        new WebGPU.Raw.SharedTextureMemoryProperties(
                            nextInChain,
                            this.Usage,
                            (if NativePtr.toNativeInt _sizePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _sizePtr),
                            this.Format
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedTextureMemoryProperties> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedTextureMemoryProperties>) = 
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryPropertiesExtension> backend.NextInChain
            Usage = backend.Usage
            Size = Extent3D.Read(&backend.Size)
            Format = backend.Format
        }
type SharedTextureMemoryAHardwareBufferProperties = 
    {
        Next : ISharedTextureMemoryPropertiesExtension
        YCbCrInfo : YCbCrVkDescriptor
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryAHardwareBufferProperties>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedTextureMemoryAHardwareBufferProperties> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedTextureMemoryAHardwareBufferProperties
                this.YCbCrInfo.Pin(fun _yCbCrInfoPtr ->
                    let mutable value =
                        new WebGPU.Raw.SharedTextureMemoryAHardwareBufferProperties(
                            nextInChain,
                            sType,
                            (if NativePtr.toNativeInt _yCbCrInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _yCbCrInfoPtr)
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryPropertiesExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedTextureMemoryAHardwareBufferProperties> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedTextureMemoryAHardwareBufferProperties>) = 
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryPropertiesExtension> backend.NextInChain
            YCbCrInfo = YCbCrVkDescriptor.Read(&backend.YCbCrInfo)
        }
type SharedTextureMemoryDescriptor = 
    {
        Next : ISharedTextureMemoryDescriptorExtension
        Label : string
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedTextureMemoryDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                let mutable value =
                    new WebGPU.Raw.SharedTextureMemoryDescriptor(
                        nextInChain,
                        _labelLen
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedTextureMemoryDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedTextureMemoryDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryDescriptorExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
        }
type SharedBufferMemoryBeginAccessDescriptor = 
    {
        Next : IExtension
        Initialized : bool
        Fences : array<SharedFence>
        SignaledValues : array<uint64>
    }
    static member Null = Unchecked.defaultof<SharedBufferMemoryBeginAccessDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedBufferMemoryBeginAccessDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let fencesHandles = this.Fences |> Array.map (fun a -> a.Handle)
                use fencesPtr = fixed (fencesHandles)
                let fencesLen = unativeint this.Fences.Length
                use signaledValuesPtr = fixed (this.SignaledValues)
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
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedBufferMemoryBeginAccessDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedBufferMemoryBeginAccessDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Initialized = (backend.Initialized <> 0)
            Fences = let ptr = backend.Fences in Array.init (int backend.FenceCount) (fun i -> new SharedFence(NativePtr.get ptr i))
            SignaledValues = let ptr = backend.SignaledValues in Array.init (int backend.FenceCount) (fun i -> NativePtr.get ptr i)
        }
type SharedBufferMemoryEndAccessState = 
    {
        Next : IExtension
        Initialized : bool
        Fences : array<SharedFence>
        SignaledValues : array<uint64>
    }
    static member Null = Unchecked.defaultof<SharedBufferMemoryEndAccessState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedBufferMemoryEndAccessState> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let fencesHandles = this.Fences |> Array.map (fun a -> a.Handle)
                use fencesPtr = fixed (fencesHandles)
                let fencesLen = unativeint this.Fences.Length
                use signaledValuesPtr = fixed (this.SignaledValues)
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
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedBufferMemoryEndAccessState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedBufferMemoryEndAccessState>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Initialized = (backend.Initialized <> 0)
            Fences = let ptr = backend.Fences in Array.init (int backend.FenceCount) (fun i -> new SharedFence(NativePtr.get ptr i))
            SignaledValues = let ptr = backend.SignaledValues in Array.init (int backend.FenceCount) (fun i -> NativePtr.get ptr i)
        }
type SharedTextureMemoryVkDedicatedAllocationDescriptor = 
    {
        Next : ISharedTextureMemoryDescriptorExtension
        DedicatedAllocation : bool
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryVkDedicatedAllocationDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedTextureMemoryVkDedicatedAllocationDescriptor> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedTextureMemoryVkDedicatedAllocationDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedTextureMemoryVkDedicatedAllocationDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryDescriptorExtension> backend.NextInChain
            DedicatedAllocation = (backend.DedicatedAllocation <> 0)
        }
type SharedTextureMemoryAHardwareBufferDescriptor = 
    {
        Next : ISharedTextureMemoryDescriptorExtension
        Handle : nativeint
        UseExternalFormat : bool
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryAHardwareBufferDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedTextureMemoryAHardwareBufferDescriptor> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedTextureMemoryAHardwareBufferDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedTextureMemoryAHardwareBufferDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryDescriptorExtension> backend.NextInChain
            Handle = backend.Handle
            UseExternalFormat = (backend.UseExternalFormat <> 0)
        }
type SharedTextureMemoryDmaBufPlane = 
    {
        Fd : int
        Offset : int64
        Stride : int
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryDmaBufPlane>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedTextureMemoryDmaBufPlane> -> 'r) : 'r = 
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
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedTextureMemoryDmaBufPlane> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedTextureMemoryDmaBufPlane>) = 
        {
            Fd = backend.Fd
            Offset = int64(backend.Offset)
            Stride = int(backend.Stride)
        }
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedTextureMemoryDmaBufDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedTextureMemoryDmaBufDescriptor
                this.Size.Pin(fun _sizePtr ->
                    WebGPU.Raw.Pinnable.pinArray this.Planes (fun planesPtr ->
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
                        action ptr
                    )
                )
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedTextureMemoryDmaBufDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedTextureMemoryDmaBufDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryDescriptorExtension> backend.NextInChain
            Size = Extent3D.Read(&backend.Size)
            DrmFormat = int(backend.DrmFormat)
            DrmModifier = int64(backend.DrmModifier)
            Planes = let ptr = backend.Planes in Array.init (int backend.PlaneCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in SharedTextureMemoryDmaBufPlane.Read(&r))
        }
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedTextureMemoryOpaqueFDDescriptor> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedTextureMemoryOpaqueFDDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedTextureMemoryOpaqueFDDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryDescriptorExtension> backend.NextInChain
            VkImageCreateInfo = backend.VkImageCreateInfo
            MemoryFD = backend.MemoryFD
            MemoryTypeIndex = int(backend.MemoryTypeIndex)
            AllocationSize = int64(backend.AllocationSize)
            DedicatedAllocation = (backend.DedicatedAllocation <> 0)
        }
type SharedTextureMemoryZirconHandleDescriptor = 
    {
        Next : ISharedTextureMemoryDescriptorExtension
        MemoryFD : int
        AllocationSize : int64
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryZirconHandleDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedTextureMemoryZirconHandleDescriptor> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedTextureMemoryZirconHandleDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedTextureMemoryZirconHandleDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryDescriptorExtension> backend.NextInChain
            MemoryFD = int(backend.MemoryFD)
            AllocationSize = int64(backend.AllocationSize)
        }
type SharedTextureMemoryDXGISharedHandleDescriptor = 
    {
        Next : ISharedTextureMemoryDescriptorExtension
        Handle : nativeint
        UseKeyedMutex : bool
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryDXGISharedHandleDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedTextureMemoryDXGISharedHandleDescriptor> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedTextureMemoryDXGISharedHandleDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedTextureMemoryDXGISharedHandleDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryDescriptorExtension> backend.NextInChain
            Handle = backend.Handle
            UseKeyedMutex = (backend.UseKeyedMutex <> 0)
        }
type SharedTextureMemoryIOSurfaceDescriptor = 
    {
        Next : ISharedTextureMemoryDescriptorExtension
        IoSurface : nativeint
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryIOSurfaceDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedTextureMemoryIOSurfaceDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SharedTextureMemoryIOSurfaceDescriptor
                let mutable value =
                    new WebGPU.Raw.SharedTextureMemoryIOSurfaceDescriptor(
                        nextInChain,
                        sType,
                        this.IoSurface
                    )
                use ptr = fixed &value
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedTextureMemoryIOSurfaceDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedTextureMemoryIOSurfaceDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryDescriptorExtension> backend.NextInChain
            IoSurface = backend.IoSurface
        }
type SharedTextureMemoryEGLImageDescriptor = 
    {
        Next : ISharedTextureMemoryDescriptorExtension
        Image : nativeint
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryEGLImageDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedTextureMemoryEGLImageDescriptor> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedTextureMemoryEGLImageDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedTextureMemoryEGLImageDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryDescriptorExtension> backend.NextInChain
            Image = backend.Image
        }
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedTextureMemoryBeginAccessDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let fencesHandles = this.Fences |> Array.map (fun a -> a.Handle)
                use fencesPtr = fixed (fencesHandles)
                let fencesLen = unativeint this.Fences.Length
                use signaledValuesPtr = fixed (this.SignaledValues)
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
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedTextureMemoryBeginAccessDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedTextureMemoryBeginAccessDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryBeginAccessDescriptorExtension> backend.NextInChain
            ConcurrentRead = (backend.ConcurrentRead <> 0)
            Initialized = (backend.Initialized <> 0)
            Fences = let ptr = backend.Fences in Array.init (int backend.FenceCount) (fun i -> new SharedFence(NativePtr.get ptr i))
            SignaledValues = let ptr = backend.SignaledValues in Array.init (int backend.FenceCount) (fun i -> NativePtr.get ptr i)
        }
type SharedTextureMemoryEndAccessState = 
    {
        Next : ISharedTextureMemoryEndAccessStateExtension
        Initialized : bool
        Fences : array<SharedFence>
        SignaledValues : array<uint64>
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryEndAccessState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedTextureMemoryEndAccessState> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let fencesHandles = this.Fences |> Array.map (fun a -> a.Handle)
                use fencesPtr = fixed (fencesHandles)
                let fencesLen = unativeint this.Fences.Length
                use signaledValuesPtr = fixed (this.SignaledValues)
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
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedTextureMemoryEndAccessState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedTextureMemoryEndAccessState>) = 
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryEndAccessStateExtension> backend.NextInChain
            Initialized = (backend.Initialized <> 0)
            Fences = let ptr = backend.Fences in Array.init (int backend.FenceCount) (fun i -> new SharedFence(NativePtr.get ptr i))
            SignaledValues = let ptr = backend.SignaledValues in Array.init (int backend.FenceCount) (fun i -> NativePtr.get ptr i)
        }
type SharedTextureMemoryVkImageLayoutBeginState = 
    {
        Next : ISharedTextureMemoryBeginAccessDescriptorExtension
        OldLayout : int
        NewLayout : int
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryVkImageLayoutBeginState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedTextureMemoryVkImageLayoutBeginState> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryBeginAccessDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedTextureMemoryVkImageLayoutBeginState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedTextureMemoryVkImageLayoutBeginState>) = 
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryBeginAccessDescriptorExtension> backend.NextInChain
            OldLayout = backend.OldLayout
            NewLayout = backend.NewLayout
        }
type SharedTextureMemoryVkImageLayoutEndState = 
    {
        Next : ISharedTextureMemoryEndAccessStateExtension
        OldLayout : int
        NewLayout : int
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryVkImageLayoutEndState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedTextureMemoryVkImageLayoutEndState> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryEndAccessStateExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedTextureMemoryVkImageLayoutEndState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedTextureMemoryVkImageLayoutEndState>) = 
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryEndAccessStateExtension> backend.NextInChain
            OldLayout = backend.OldLayout
            NewLayout = backend.NewLayout
        }
type SharedTextureMemoryD3DSwapchainBeginState = 
    {
        Next : ISharedTextureMemoryBeginAccessDescriptorExtension
        IsSwapchain : bool
    }
    static member Null = Unchecked.defaultof<SharedTextureMemoryD3DSwapchainBeginState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedTextureMemoryD3DSwapchainBeginState> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedTextureMemoryBeginAccessDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedTextureMemoryD3DSwapchainBeginState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedTextureMemoryD3DSwapchainBeginState>) = 
        {
            Next = ExtensionDecoder.decode<ISharedTextureMemoryBeginAccessDescriptorExtension> backend.NextInChain
            IsSwapchain = (backend.IsSwapchain <> 0)
        }
type SharedFence internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"SharedFence(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? SharedFence as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new SharedFence(0n)
    member _.ExportInfo(info : byref<SharedFenceExportInfo>) : unit =
        let mutable infoCopy = info
        try
            info.Pin(fun _infoPtr ->
                if NativePtr.toNativeInt _infoPtr = 0n then
                    let mutable infoNative = Unchecked.defaultof<WebGPU.Raw.SharedFenceExportInfo>
                    use _infoPtr = fixed &infoNative
                    let res = WebGPU.Raw.WebGPU.SharedFenceExportInfo(handle, _infoPtr)
                    let _ret = res
                    infoCopy <- SharedFenceExportInfo.Read(&infoNative)
                    _ret
                else
                    let res = WebGPU.Raw.WebGPU.SharedFenceExportInfo(handle, _infoPtr)
                    let _ret = res
                    let infoResult = NativePtr.toByRef _infoPtr
                    infoCopy <- SharedFenceExportInfo.Read(&infoResult)
                    _ret
            )
        finally
            info <- infoCopy
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.SharedFenceRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type SharedFenceDescriptor = 
    {
        Next : ISharedFenceDescriptorExtension
        Label : string
    }
    static member Null = Unchecked.defaultof<SharedFenceDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedFenceDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                let mutable value =
                    new WebGPU.Raw.SharedFenceDescriptor(
                        nextInChain,
                        _labelLen
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedFenceDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedFenceDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISharedFenceDescriptorExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
        }
type SharedFenceVkSemaphoreOpaqueFDDescriptor = 
    {
        Next : ISharedFenceDescriptorExtension
        Handle : int
    }
    static member Null = Unchecked.defaultof<SharedFenceVkSemaphoreOpaqueFDDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDDescriptor> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISharedFenceDescriptorExtension> backend.NextInChain
            Handle = backend.Handle
        }
type SharedFenceSyncFDDescriptor = 
    {
        Next : ISharedFenceDescriptorExtension
        Handle : int
    }
    static member Null = Unchecked.defaultof<SharedFenceSyncFDDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedFenceSyncFDDescriptor> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedFenceSyncFDDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedFenceSyncFDDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISharedFenceDescriptorExtension> backend.NextInChain
            Handle = backend.Handle
        }
type SharedFenceVkSemaphoreZirconHandleDescriptor = 
    {
        Next : ISharedFenceDescriptorExtension
        Handle : int
    }
    static member Null = Unchecked.defaultof<SharedFenceVkSemaphoreZirconHandleDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleDescriptor> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISharedFenceDescriptorExtension> backend.NextInChain
            Handle = int(backend.Handle)
        }
type SharedFenceDXGISharedHandleDescriptor = 
    {
        Next : ISharedFenceDescriptorExtension
        Handle : nativeint
    }
    static member Null = Unchecked.defaultof<SharedFenceDXGISharedHandleDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedFenceDXGISharedHandleDescriptor> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedFenceDXGISharedHandleDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedFenceDXGISharedHandleDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISharedFenceDescriptorExtension> backend.NextInChain
            Handle = backend.Handle
        }
type SharedFenceMTLSharedEventDescriptor = 
    {
        Next : ISharedFenceDescriptorExtension
        SharedEvent : nativeint
    }
    static member Null = Unchecked.defaultof<SharedFenceMTLSharedEventDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedFenceMTLSharedEventDescriptor> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedFenceMTLSharedEventDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedFenceMTLSharedEventDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISharedFenceDescriptorExtension> backend.NextInChain
            SharedEvent = backend.SharedEvent
        }
type SharedFenceExportInfo = 
    {
        Next : ISharedFenceExportInfoExtension
        Type : SharedFenceType
    }
    static member Null = Unchecked.defaultof<SharedFenceExportInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedFenceExportInfo> -> 'r) : 'r = 
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
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedFenceExportInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedFenceExportInfo>) = 
        {
            Next = ExtensionDecoder.decode<ISharedFenceExportInfoExtension> backend.NextInChain
            Type = backend.Type
        }
type SharedFenceVkSemaphoreOpaqueFDExportInfo = 
    {
        Next : ISharedFenceExportInfoExtension
        Handle : int
    }
    static member Null = Unchecked.defaultof<SharedFenceVkSemaphoreOpaqueFDExportInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDExportInfo> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceExportInfoExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDExportInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedFenceVkSemaphoreOpaqueFDExportInfo>) = 
        {
            Next = ExtensionDecoder.decode<ISharedFenceExportInfoExtension> backend.NextInChain
            Handle = backend.Handle
        }
type SharedFenceSyncFDExportInfo = 
    {
        Next : ISharedFenceExportInfoExtension
        Handle : int
    }
    static member Null = Unchecked.defaultof<SharedFenceSyncFDExportInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedFenceSyncFDExportInfo> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceExportInfoExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedFenceSyncFDExportInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedFenceSyncFDExportInfo>) = 
        {
            Next = ExtensionDecoder.decode<ISharedFenceExportInfoExtension> backend.NextInChain
            Handle = backend.Handle
        }
type SharedFenceVkSemaphoreZirconHandleExportInfo = 
    {
        Next : ISharedFenceExportInfoExtension
        Handle : int
    }
    static member Null = Unchecked.defaultof<SharedFenceVkSemaphoreZirconHandleExportInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleExportInfo> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceExportInfoExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleExportInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedFenceVkSemaphoreZirconHandleExportInfo>) = 
        {
            Next = ExtensionDecoder.decode<ISharedFenceExportInfoExtension> backend.NextInChain
            Handle = int(backend.Handle)
        }
type SharedFenceDXGISharedHandleExportInfo = 
    {
        Next : ISharedFenceExportInfoExtension
        Handle : nativeint
    }
    static member Null = Unchecked.defaultof<SharedFenceDXGISharedHandleExportInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedFenceDXGISharedHandleExportInfo> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceExportInfoExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedFenceDXGISharedHandleExportInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedFenceDXGISharedHandleExportInfo>) = 
        {
            Next = ExtensionDecoder.decode<ISharedFenceExportInfoExtension> backend.NextInChain
            Handle = backend.Handle
        }
type SharedFenceMTLSharedEventExportInfo = 
    {
        Next : ISharedFenceExportInfoExtension
        SharedEvent : nativeint
    }
    static member Null = Unchecked.defaultof<SharedFenceMTLSharedEventExportInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SharedFenceMTLSharedEventExportInfo> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISharedFenceExportInfoExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SharedFenceMTLSharedEventExportInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SharedFenceMTLSharedEventExportInfo>) = 
        {
            Next = ExtensionDecoder.decode<ISharedFenceExportInfoExtension> backend.NextInChain
            SharedEvent = backend.SharedEvent
        }
type FormatCapabilities = 
    {
        Next : IFormatCapabilitiesExtension
    }
    static member Null = Unchecked.defaultof<FormatCapabilities>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.FormatCapabilities> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable value =
                    new WebGPU.Raw.FormatCapabilities(
                        nextInChain
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.FormatCapabilities> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.FormatCapabilities>) = 
        {
            Next = ExtensionDecoder.decode<IFormatCapabilitiesExtension> backend.NextInChain
        }
type DrmFormatCapabilities = 
    {
        Next : IFormatCapabilitiesExtension
        Properties : array<DrmFormatProperties>
    }
    static member Null = Unchecked.defaultof<DrmFormatCapabilities>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DrmFormatCapabilities> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.DrmFormatCapabilities
                WebGPU.Raw.Pinnable.pinArray this.Properties (fun propertiesPtr ->
                    let propertiesLen = unativeint this.Properties.Length
                    let mutable value =
                        new WebGPU.Raw.DrmFormatCapabilities(
                            nextInChain,
                            sType,
                            propertiesLen,
                            propertiesPtr
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IFormatCapabilitiesExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DrmFormatCapabilities> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DrmFormatCapabilities>) = 
        {
            Next = ExtensionDecoder.decode<IFormatCapabilitiesExtension> backend.NextInChain
            Properties = let ptr = backend.Properties in Array.init (int backend.PropertiesCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in DrmFormatProperties.Read(&r))
        }
type DrmFormatProperties = 
    {
        Modifier : int64
        ModifierPlaneCount : int
    }
    static member Null = Unchecked.defaultof<DrmFormatProperties>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DrmFormatProperties> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.DrmFormatProperties(
                    uint64(this.Modifier),
                    uint32(this.ModifierPlaneCount)
                )
            use ptr = fixed &value
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DrmFormatProperties> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DrmFormatProperties>) = 
        {
            Modifier = int64(backend.Modifier)
            ModifierPlaneCount = int(backend.ModifierPlaneCount)
        }
type ImageCopyBuffer = 
    {
        Layout : TextureDataLayout
        Buffer : Buffer
    }
    static member Null = Unchecked.defaultof<ImageCopyBuffer>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ImageCopyBuffer> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            this.Layout.Pin(fun _layoutPtr ->
                let mutable value =
                    new WebGPU.Raw.ImageCopyBuffer(
                        (if NativePtr.toNativeInt _layoutPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _layoutPtr),
                        this.Buffer.Handle
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ImageCopyBuffer> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ImageCopyBuffer>) = 
        {
            Layout = TextureDataLayout.Read(&backend.Layout)
            Buffer = new Buffer(backend.Buffer)
        }
type ImageCopyTexture = 
    {
        Texture : Texture
        MipLevel : int
        Origin : Origin3D
        Aspect : TextureAspect
    }
    static member Null = Unchecked.defaultof<ImageCopyTexture>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ImageCopyTexture> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            this.Origin.Pin(fun _originPtr ->
                let mutable value =
                    new WebGPU.Raw.ImageCopyTexture(
                        this.Texture.Handle,
                        uint32(this.MipLevel),
                        (if NativePtr.toNativeInt _originPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _originPtr),
                        this.Aspect
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ImageCopyTexture> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ImageCopyTexture>) = 
        {
            Texture = new Texture(backend.Texture)
            MipLevel = int(backend.MipLevel)
            Origin = Origin3D.Read(&backend.Origin)
            Aspect = backend.Aspect
        }
type ImageCopyExternalTexture = 
    {
        Next : IExtension
        ExternalTexture : ExternalTexture
        Origin : Origin3D
        NaturalSize : Extent2D
    }
    static member Null = Unchecked.defaultof<ImageCopyExternalTexture>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ImageCopyExternalTexture> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                this.Origin.Pin(fun _originPtr ->
                    this.NaturalSize.Pin(fun _naturalSizePtr ->
                        let mutable value =
                            new WebGPU.Raw.ImageCopyExternalTexture(
                                nextInChain,
                                this.ExternalTexture.Handle,
                                (if NativePtr.toNativeInt _originPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _originPtr),
                                (if NativePtr.toNativeInt _naturalSizePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _naturalSizePtr)
                            )
                        use ptr = fixed &value
                        action ptr
                    )
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ImageCopyExternalTexture> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ImageCopyExternalTexture>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            ExternalTexture = new ExternalTexture(backend.ExternalTexture)
            Origin = Origin3D.Read(&backend.Origin)
            NaturalSize = Extent2D.Read(&backend.NaturalSize)
        }
type Instance internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"Instance(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? Instance as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new Instance(0n)
    member _.CreateSurface(descriptor : SurfaceDescriptor) : Surface =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.InstanceCreateSurface(handle, _descriptorPtr)
            new Surface(res)
        )
    member _.ProcessEvents() : unit =
        let res = WebGPU.Raw.WebGPU.InstanceProcessEvents(handle)
        res
    member _.WaitAny(futures : array<FutureWaitInfo>, timeoutNS : int64) : WaitStatus =
        WebGPU.Raw.Pinnable.pinArray futures (fun futuresPtr ->
            let futuresLen = unativeint futures.Length
            let res = WebGPU.Raw.WebGPU.InstanceWaitAny(handle, futuresLen, futuresPtr, uint64(timeoutNS))
            res
        )
    member _.RequestAdapter(options : RequestAdapterOptions, callback : RequestAdapterCallback) : unit =
        options.Pin(fun _optionsPtr ->
            let mutable _callbackGC = Unchecked.defaultof<GCHandle>
            let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.RequestAdapterCallback>
            _callbackDel <- WebGPU.Raw.RequestAdapterCallback(fun status adapter message userdata ->
                let _status = status
                let _adapter = new Adapter(adapter)
                let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _adapter, _message)
            )
            _callbackGC <- GCHandle.Alloc(_callbackDel)
            let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
            let res = WebGPU.Raw.WebGPU.InstanceRequestAdapter(handle, _optionsPtr, _callbackPtr, Unchecked.defaultof<_>)
            res
        )
    member _.RequestAdapterF(options : RequestAdapterOptions, callbackInfo : RequestAdapterCallbackInfo) : Future =
        options.Pin(fun _optionsPtr ->
            callbackInfo.Pin(fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.InstanceRequestAdapterF(handle, _optionsPtr, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
                Future.Read(&res)
            )
        )
    member _.RequestAdapter2(options : RequestAdapterOptions, callbackInfo : RequestAdapterCallbackInfo2) : Future =
        options.Pin(fun _optionsPtr ->
            callbackInfo.Pin(fun _callbackInfoPtr ->
                let res = WebGPU.Raw.WebGPU.InstanceRequestAdapter2(handle, _optionsPtr, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
                Future.Read(&res)
            )
        )
    member _.HasWGSLLanguageFeature(feature : WGSLFeatureName) : bool =
        let res = WebGPU.Raw.WebGPU.InstanceHasWGSLLanguageFeature(handle, feature)
        (res <> 0)
    member _.EnumerateWGSLLanguageFeatures(features : WGSLFeatureName) : int64 =
        let mutable featuresHandle = features
        use featuresPtr = fixed (&featuresHandle)
        let res = WebGPU.Raw.WebGPU.InstanceEnumerateWGSLLanguageFeatures(handle, featuresPtr)
        int64(res)
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.InstanceRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type Future = 
    {
        Id : int64
    }
    static member Null = Unchecked.defaultof<Future>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.Future> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.Future(
                    uint64(this.Id)
                )
            use ptr = fixed &value
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.Future> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.Future>) = 
        {
            Id = int64(backend.Id)
        }
type FutureWaitInfo = 
    {
        Future : Future
        Completed : bool
    }
    static member Null = Unchecked.defaultof<FutureWaitInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.FutureWaitInfo> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            this.Future.Pin(fun _futurePtr ->
                let mutable value =
                    new WebGPU.Raw.FutureWaitInfo(
                        (if NativePtr.toNativeInt _futurePtr = 0n then Unchecked.defaultof<_> else NativePtr.read _futurePtr),
                        (if this.Completed then 1 else 0)
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.FutureWaitInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.FutureWaitInfo>) = 
        {
            Future = Future.Read(&backend.Future)
            Completed = (backend.Completed <> 0)
        }
type InstanceFeatures = 
    {
        Next : IExtension
        TimedWaitAnyEnable : bool
        TimedWaitAnyMaxCount : int64
    }
    static member Null = Unchecked.defaultof<InstanceFeatures>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.InstanceFeatures> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable value =
                    new WebGPU.Raw.InstanceFeatures(
                        nextInChain,
                        (if this.TimedWaitAnyEnable then 1 else 0),
                        unativeint(this.TimedWaitAnyMaxCount)
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.InstanceFeatures> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.InstanceFeatures>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            TimedWaitAnyEnable = (backend.TimedWaitAnyEnable <> 0)
            TimedWaitAnyMaxCount = int64(backend.TimedWaitAnyMaxCount)
        }
type InstanceDescriptor = 
    {
        Next : IInstanceDescriptorExtension
        Features : InstanceFeatures
    }
    static member Null = Unchecked.defaultof<InstanceDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.InstanceDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                this.Features.Pin(fun _featuresPtr ->
                    let mutable value =
                        new WebGPU.Raw.InstanceDescriptor(
                            nextInChain,
                            (if NativePtr.toNativeInt _featuresPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _featuresPtr)
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.InstanceDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.InstanceDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IInstanceDescriptorExtension> backend.NextInChain
            Features = InstanceFeatures.Read(&backend.Features)
        }
type DawnWireWGSLControl = 
    {
        Next : IInstanceDescriptorExtension
        EnableExperimental : bool
        EnableUnsafe : bool
        EnableTesting : bool
    }
    static member Null = Unchecked.defaultof<DawnWireWGSLControl>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DawnWireWGSLControl> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IInstanceDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnWireWGSLControl> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DawnWireWGSLControl>) = 
        {
            Next = ExtensionDecoder.decode<IInstanceDescriptorExtension> backend.NextInChain
            EnableExperimental = (backend.EnableExperimental <> 0)
            EnableUnsafe = (backend.EnableUnsafe <> 0)
            EnableTesting = (backend.EnableTesting <> 0)
        }
type VertexAttribute = 
    {
        Format : VertexFormat
        Offset : int64
        ShaderLocation : int
    }
    static member Null = Unchecked.defaultof<VertexAttribute>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.VertexAttribute> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.VertexAttribute(
                    this.Format,
                    uint64(this.Offset),
                    uint32(this.ShaderLocation)
                )
            use ptr = fixed &value
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.VertexAttribute> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.VertexAttribute>) = 
        {
            Format = backend.Format
            Offset = int64(backend.Offset)
            ShaderLocation = int(backend.ShaderLocation)
        }
type VertexBufferLayout = 
    {
        ArrayStride : int64
        StepMode : VertexStepMode
        Attributes : array<VertexAttribute>
    }
    static member Null = Unchecked.defaultof<VertexBufferLayout>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.VertexBufferLayout> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            WebGPU.Raw.Pinnable.pinArray this.Attributes (fun attributesPtr ->
                let attributesLen = unativeint this.Attributes.Length
                let mutable value =
                    new WebGPU.Raw.VertexBufferLayout(
                        uint64(this.ArrayStride),
                        this.StepMode,
                        attributesLen,
                        attributesPtr
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.VertexBufferLayout> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.VertexBufferLayout>) = 
        {
            ArrayStride = int64(backend.ArrayStride)
            StepMode = backend.StepMode
            Attributes = let ptr = backend.Attributes in Array.init (int backend.AttributeCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in VertexAttribute.Read(&r))
        }
type Origin3D = 
    {
        X : int
        Y : int
        Z : int
    }
    static member Null = Unchecked.defaultof<Origin3D>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.Origin3D> -> 'r) : 'r = 
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
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.Origin3D> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.Origin3D>) = 
        {
            X = int(backend.X)
            Y = int(backend.Y)
            Z = int(backend.Z)
        }
type Origin2D = 
    {
        X : int
        Y : int
    }
    static member Null = Unchecked.defaultof<Origin2D>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.Origin2D> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.Origin2D(
                    uint32(this.X),
                    uint32(this.Y)
                )
            use ptr = fixed &value
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.Origin2D> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.Origin2D>) = 
        {
            X = int(backend.X)
            Y = int(backend.Y)
        }
type PipelineLayout internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"PipelineLayout(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? PipelineLayout as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new PipelineLayout(0n)
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.PipelineLayoutSetLabel(handle, _labelLen)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.PipelineLayoutRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type PipelineLayoutDescriptor = 
    {
        Next : IPipelineLayoutDescriptorExtension
        Label : string
        BindGroupLayouts : array<BindGroupLayout>
        ImmediateDataRangeByteSize : int
    }
    static member Null = Unchecked.defaultof<PipelineLayoutDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.PipelineLayoutDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                let bindGroupLayoutsHandles = this.BindGroupLayouts |> Array.map (fun a -> a.Handle)
                use bindGroupLayoutsPtr = fixed (bindGroupLayoutsHandles)
                let bindGroupLayoutsLen = unativeint this.BindGroupLayouts.Length
                let mutable value =
                    new WebGPU.Raw.PipelineLayoutDescriptor(
                        nextInChain,
                        _labelLen,
                        bindGroupLayoutsLen,
                        bindGroupLayoutsPtr,
                        uint32(this.ImmediateDataRangeByteSize)
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.PipelineLayoutDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.PipelineLayoutDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IPipelineLayoutDescriptorExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            BindGroupLayouts = let ptr = backend.BindGroupLayouts in Array.init (int backend.BindGroupLayoutCount) (fun i -> new BindGroupLayout(NativePtr.get ptr i))
            ImmediateDataRangeByteSize = int(backend.ImmediateDataRangeByteSize)
        }
type PipelineLayoutPixelLocalStorage = 
    {
        Next : IPipelineLayoutDescriptorExtension
        TotalPixelLocalStorageSize : int64
        StorageAttachments : array<PipelineLayoutStorageAttachment>
    }
    static member Null = Unchecked.defaultof<PipelineLayoutPixelLocalStorage>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.PipelineLayoutPixelLocalStorage> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.PipelineLayoutPixelLocalStorage
                WebGPU.Raw.Pinnable.pinArray this.StorageAttachments (fun storageAttachmentsPtr ->
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
                    action ptr
                )
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IPipelineLayoutDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.PipelineLayoutPixelLocalStorage> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.PipelineLayoutPixelLocalStorage>) = 
        {
            Next = ExtensionDecoder.decode<IPipelineLayoutDescriptorExtension> backend.NextInChain
            TotalPixelLocalStorageSize = int64(backend.TotalPixelLocalStorageSize)
            StorageAttachments = let ptr = backend.StorageAttachments in Array.init (int backend.StorageAttachmentCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in PipelineLayoutStorageAttachment.Read(&r))
        }
type PipelineLayoutStorageAttachment = 
    {
        Next : IExtension
        Offset : int64
        Format : TextureFormat
    }
    static member Null = Unchecked.defaultof<PipelineLayoutStorageAttachment>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.PipelineLayoutStorageAttachment> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable value =
                    new WebGPU.Raw.PipelineLayoutStorageAttachment(
                        nextInChain,
                        uint64(this.Offset),
                        this.Format
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.PipelineLayoutStorageAttachment> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.PipelineLayoutStorageAttachment>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Offset = int64(backend.Offset)
            Format = backend.Format
        }
type ProgrammableStageDescriptor = 
    {
        Next : IExtension
        Module : ShaderModule
        EntryPoint : string
        Constants : array<ConstantEntry>
    }
    static member Null = Unchecked.defaultof<ProgrammableStageDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ProgrammableStageDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _entryPointArr = Encoding.UTF8.GetBytes(this.EntryPoint)
                use _entryPointPtr = fixed _entryPointArr
                let _entryPointLen = WebGPU.Raw.StringView(_entryPointPtr, unativeint _entryPointArr.Length)
                WebGPU.Raw.Pinnable.pinArray this.Constants (fun constantsPtr ->
                    let constantsLen = unativeint this.Constants.Length
                    let mutable value =
                        new WebGPU.Raw.ProgrammableStageDescriptor(
                            nextInChain,
                            this.Module.Handle,
                            _entryPointLen,
                            constantsLen,
                            constantsPtr
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ProgrammableStageDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ProgrammableStageDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Module = new ShaderModule(backend.Module)
            EntryPoint = let _entryPointPtr = NativePtr.toNativeInt(backend.EntryPoint.Data) in if _entryPointPtr = 0n then null else Marshal.PtrToStringUTF8(_entryPointPtr, int(backend.EntryPoint.Length))
            Constants = let ptr = backend.Constants in Array.init (int backend.ConstantCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in ConstantEntry.Read(&r))
        }
type QuerySet internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"QuerySet(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? QuerySet as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new QuerySet(0n)
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.QuerySetSetLabel(handle, _labelLen)
        res
    member this.Type : QueryType =
        let mutable res = WebGPU.Raw.WebGPU.QuerySetGetType(handle)
        res
    member this.Count : int =
        let mutable res = WebGPU.Raw.WebGPU.QuerySetGetCount(handle)
        int(res)
    member _.Destroy() : unit =
        let res = WebGPU.Raw.WebGPU.QuerySetDestroy(handle)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.QuerySetRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type QuerySetDescriptor = 
    {
        Next : IExtension
        Label : string
        Type : QueryType
        Count : int
    }
    static member Null = Unchecked.defaultof<QuerySetDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.QuerySetDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                let mutable value =
                    new WebGPU.Raw.QuerySetDescriptor(
                        nextInChain,
                        _labelLen,
                        this.Type,
                        uint32(this.Count)
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.QuerySetDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.QuerySetDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            Type = backend.Type
            Count = int(backend.Count)
        }
type Queue internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"Queue(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? Queue as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new Queue(0n)
    member _.Submit(commands : array<CommandBuffer>) : unit =
        let commandsHandles = commands |> Array.map (fun a -> a.Handle)
        use commandsPtr = fixed (commandsHandles)
        let commandsLen = unativeint commands.Length
        let res = WebGPU.Raw.WebGPU.QueueSubmit(handle, commandsLen, commandsPtr)
        res
    member _.OnSubmittedWorkDone(callback : QueueWorkDoneCallback) : unit =
        let mutable _callbackGC = Unchecked.defaultof<GCHandle>
        let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.QueueWorkDoneCallback>
        _callbackDel <- WebGPU.Raw.QueueWorkDoneCallback(fun status userdata ->
            let _status = status
            callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status)
        )
        _callbackGC <- GCHandle.Alloc(_callbackDel)
        let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
        let res = WebGPU.Raw.WebGPU.QueueOnSubmittedWorkDone(handle, _callbackPtr, Unchecked.defaultof<_>)
        res
    member _.OnSubmittedWorkDoneF(callbackInfo : QueueWorkDoneCallbackInfo) : Future =
        callbackInfo.Pin(fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.QueueOnSubmittedWorkDoneF(handle, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
            Future.Read(&res)
        )
    member _.OnSubmittedWorkDone2(callbackInfo : QueueWorkDoneCallbackInfo2) : Future =
        callbackInfo.Pin(fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.QueueOnSubmittedWorkDone2(handle, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
            Future.Read(&res)
        )
    member _.WriteBuffer(buffer : Buffer, bufferOffset : int64, data : nativeint, size : int64) : unit =
        let res = WebGPU.Raw.WebGPU.QueueWriteBuffer(handle, buffer.Handle, uint64(bufferOffset), data, unativeint(size))
        res
    member _.WriteTexture(destination : ImageCopyTexture, data : nativeint, dataSize : int64, dataLayout : TextureDataLayout, writeSize : Extent3D) : unit =
        destination.Pin(fun _destinationPtr ->
            dataLayout.Pin(fun _dataLayoutPtr ->
                writeSize.Pin(fun _writeSizePtr ->
                    let res = WebGPU.Raw.WebGPU.QueueWriteTexture(handle, _destinationPtr, data, unativeint(dataSize), _dataLayoutPtr, _writeSizePtr)
                    res
                )
            )
        )
    member _.CopyTextureForBrowser(source : ImageCopyTexture, destination : ImageCopyTexture, copySize : Extent3D, options : CopyTextureForBrowserOptions) : unit =
        source.Pin(fun _sourcePtr ->
            destination.Pin(fun _destinationPtr ->
                copySize.Pin(fun _copySizePtr ->
                    options.Pin(fun _optionsPtr ->
                        let res = WebGPU.Raw.WebGPU.QueueCopyTextureForBrowser(handle, _sourcePtr, _destinationPtr, _copySizePtr, _optionsPtr)
                        res
                    )
                )
            )
        )
    member _.CopyExternalTextureForBrowser(source : ImageCopyExternalTexture, destination : ImageCopyTexture, copySize : Extent3D, options : CopyTextureForBrowserOptions) : unit =
        source.Pin(fun _sourcePtr ->
            destination.Pin(fun _destinationPtr ->
                copySize.Pin(fun _copySizePtr ->
                    options.Pin(fun _optionsPtr ->
                        let res = WebGPU.Raw.WebGPU.QueueCopyExternalTextureForBrowser(handle, _sourcePtr, _destinationPtr, _copySizePtr, _optionsPtr)
                        res
                    )
                )
            )
        )
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.QueueSetLabel(handle, _labelLen)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.QueueRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type QueueDescriptor = 
    {
        Next : IExtension
        Label : string
    }
    static member Null = Unchecked.defaultof<QueueDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.QueueDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                let mutable value =
                    new WebGPU.Raw.QueueDescriptor(
                        nextInChain,
                        _labelLen
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.QueueDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.QueueDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
        }
type QueueWorkDoneCallback = delegate of IDisposable * status : QueueWorkDoneStatus -> unit
type QueueWorkDoneCallback2 = delegate of IDisposable * status : QueueWorkDoneStatus -> unit
type QueueWorkDoneCallbackInfo = 
    {
        Next : IExtension
        Mode : CallbackMode
        Callback : QueueWorkDoneCallback
    }
    static member Null = Unchecked.defaultof<QueueWorkDoneCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.QueueWorkDoneCallbackInfo> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.QueueWorkDoneCallback>
                _callbackDel <- WebGPU.Raw.QueueWorkDoneCallback(fun status userdata ->
                    let _status = status
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.QueueWorkDoneCallbackInfo(
                        nextInChain,
                        this.Mode,
                        _callbackPtr,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.QueueWorkDoneCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.QueueWorkDoneCallbackInfo>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
type QueueWorkDoneCallbackInfo2 = 
    {
        Next : IExtension
        Mode : CallbackMode
        Callback : QueueWorkDoneCallback2
    }
    static member Null = Unchecked.defaultof<QueueWorkDoneCallbackInfo2>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.QueueWorkDoneCallbackInfo2> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.QueueWorkDoneCallback2>
                _callbackDel <- WebGPU.Raw.QueueWorkDoneCallback2(fun status userdata1 userdata2 ->
                    let _status = status
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.QueueWorkDoneCallbackInfo2(
                        nextInChain,
                        this.Mode,
                        _callbackPtr,
                        Unchecked.defaultof<_>,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.QueueWorkDoneCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.QueueWorkDoneCallbackInfo2>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
type RenderBundle internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"RenderBundle(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? RenderBundle as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new RenderBundle(0n)
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.RenderBundleSetLabel(handle, _labelLen)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.RenderBundleRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type RenderBundleEncoder internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"RenderBundleEncoder(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? RenderBundleEncoder as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new RenderBundleEncoder(0n)
    member _.SetPipeline(pipeline : RenderPipeline) : unit =
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderSetPipeline(handle, pipeline.Handle)
        res
    member _.SetBindGroup(groupIndex : int, group : BindGroup, dynamicOffsets : array<uint32>) : unit =
        use dynamicOffsetsPtr = fixed (dynamicOffsets)
        let dynamicOffsetsLen = unativeint dynamicOffsets.Length
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderSetBindGroup(handle, uint32(groupIndex), group.Handle, dynamicOffsetsLen, dynamicOffsetsPtr)
        res
    member _.Draw(vertexCount : int, instanceCount : int, firstVertex : int, firstInstance : int) : unit =
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderDraw(handle, uint32(vertexCount), uint32(instanceCount), uint32(firstVertex), uint32(firstInstance))
        res
    member _.DrawIndexed(indexCount : int, instanceCount : int, firstIndex : int, baseVertex : int, firstInstance : int) : unit =
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderDrawIndexed(handle, uint32(indexCount), uint32(instanceCount), uint32(firstIndex), baseVertex, uint32(firstInstance))
        res
    member _.DrawIndirect(indirectBuffer : Buffer, indirectOffset : int64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderDrawIndirect(handle, indirectBuffer.Handle, uint64(indirectOffset))
        res
    member _.DrawIndexedIndirect(indirectBuffer : Buffer, indirectOffset : int64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderDrawIndexedIndirect(handle, indirectBuffer.Handle, uint64(indirectOffset))
        res
    member _.InsertDebugMarker(markerLabel : string) : unit =
        let _markerLabelArr = Encoding.UTF8.GetBytes(markerLabel)
        use _markerLabelPtr = fixed _markerLabelArr
        let _markerLabelLen = WebGPU.Raw.StringView(_markerLabelPtr, unativeint _markerLabelArr.Length)
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderInsertDebugMarker(handle, _markerLabelLen)
        res
    member _.PopDebugGroup() : unit =
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderPopDebugGroup(handle)
        res
    member _.PushDebugGroup(groupLabel : string) : unit =
        let _groupLabelArr = Encoding.UTF8.GetBytes(groupLabel)
        use _groupLabelPtr = fixed _groupLabelArr
        let _groupLabelLen = WebGPU.Raw.StringView(_groupLabelPtr, unativeint _groupLabelArr.Length)
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderPushDebugGroup(handle, _groupLabelLen)
        res
    member _.SetVertexBuffer(slot : int, buffer : Buffer, offset : int64, size : int64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderSetVertexBuffer(handle, uint32(slot), buffer.Handle, uint64(offset), uint64(size))
        res
    member _.SetIndexBuffer(buffer : Buffer, format : IndexFormat, offset : int64, size : int64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderSetIndexBuffer(handle, buffer.Handle, format, uint64(offset), uint64(size))
        res
    member _.Finish(descriptor : RenderBundleDescriptor) : RenderBundle =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.RenderBundleEncoderFinish(handle, _descriptorPtr)
            new RenderBundle(res)
        )
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderSetLabel(handle, _labelLen)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.RenderBundleEncoderRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type RenderBundleDescriptor = 
    {
        Next : IExtension
        Label : string
    }
    static member Null = Unchecked.defaultof<RenderBundleDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderBundleDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                let mutable value =
                    new WebGPU.Raw.RenderBundleDescriptor(
                        nextInChain,
                        _labelLen
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderBundleDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderBundleDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
        }
type RenderBundleEncoderDescriptor = 
    {
        Next : IExtension
        Label : string
        ColorFormats : array<TextureFormat>
        DepthStencilFormat : TextureFormat
        SampleCount : int
        DepthReadOnly : bool
        StencilReadOnly : bool
    }
    static member Null = Unchecked.defaultof<RenderBundleEncoderDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderBundleEncoderDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                use colorFormatsPtr = fixed (this.ColorFormats)
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
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderBundleEncoderDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderBundleEncoderDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            ColorFormats = let ptr = backend.ColorFormats in Array.init (int backend.ColorFormatCount) (fun i -> NativePtr.get ptr i)
            DepthStencilFormat = backend.DepthStencilFormat
            SampleCount = int(backend.SampleCount)
            DepthReadOnly = (backend.DepthReadOnly <> 0)
            StencilReadOnly = (backend.StencilReadOnly <> 0)
        }
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderPassColorAttachment> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                this.ClearValue.Pin(fun _clearValuePtr ->
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
                    action ptr
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPassColorAttachment> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderPassColorAttachment>) = 
        {
            Next = ExtensionDecoder.decode<IRenderPassColorAttachmentExtension> backend.NextInChain
            View = new TextureView(backend.View)
            DepthSlice = int(backend.DepthSlice)
            ResolveTarget = new TextureView(backend.ResolveTarget)
            LoadOp = backend.LoadOp
            StoreOp = backend.StoreOp
            ClearValue = Color.Read(&backend.ClearValue)
        }
type DawnRenderPassColorAttachmentRenderToSingleSampled = 
    {
        Next : IRenderPassColorAttachmentExtension
        ImplicitSampleCount : int
    }
    static member Null = Unchecked.defaultof<DawnRenderPassColorAttachmentRenderToSingleSampled>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DawnRenderPassColorAttachmentRenderToSingleSampled> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IRenderPassColorAttachmentExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnRenderPassColorAttachmentRenderToSingleSampled> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DawnRenderPassColorAttachmentRenderToSingleSampled>) = 
        {
            Next = ExtensionDecoder.decode<IRenderPassColorAttachmentExtension> backend.NextInChain
            ImplicitSampleCount = int(backend.ImplicitSampleCount)
        }
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderPassDepthStencilAttachment> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.RenderPassDepthStencilAttachment(
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
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPassDepthStencilAttachment> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderPassDepthStencilAttachment>) = 
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
type RenderPassDescriptor = 
    {
        Next : IRenderPassDescriptorExtension
        Label : string
        ColorAttachments : array<RenderPassColorAttachment>
        DepthStencilAttachment : RenderPassDepthStencilAttachment
        OcclusionQuerySet : QuerySet
        TimestampWrites : RenderPassTimestampWrites
    }
    static member Null = Unchecked.defaultof<RenderPassDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderPassDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                WebGPU.Raw.Pinnable.pinArray this.ColorAttachments (fun colorAttachmentsPtr ->
                    let colorAttachmentsLen = unativeint this.ColorAttachments.Length
                    this.DepthStencilAttachment.Pin(fun _depthStencilAttachmentPtr ->
                        this.TimestampWrites.Pin(fun _timestampWritesPtr ->
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
                            action ptr
                        )
                    )
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPassDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderPassDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IRenderPassDescriptorExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            ColorAttachments = let ptr = backend.ColorAttachments in Array.init (int backend.ColorAttachmentCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in RenderPassColorAttachment.Read(&r))
            DepthStencilAttachment = let m = NativePtr.toByRef backend.DepthStencilAttachment in RenderPassDepthStencilAttachment.Read(&m)
            OcclusionQuerySet = new QuerySet(backend.OcclusionQuerySet)
            TimestampWrites = let m = NativePtr.toByRef backend.TimestampWrites in RenderPassTimestampWrites.Read(&m)
        }
type RenderPassDescriptorMaxDrawCount = RenderPassMaxDrawCount
type RenderPassMaxDrawCount = 
    {
        Next : IRenderPassDescriptorExtension
        MaxDrawCount : int64
    }
    static member Null = Unchecked.defaultof<RenderPassMaxDrawCount>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderPassMaxDrawCount> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IRenderPassDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPassMaxDrawCount> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderPassMaxDrawCount>) = 
        {
            Next = ExtensionDecoder.decode<IRenderPassDescriptorExtension> backend.NextInChain
            MaxDrawCount = int64(backend.MaxDrawCount)
        }
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderPassDescriptorExpandResolveRect> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IRenderPassDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPassDescriptorExpandResolveRect> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderPassDescriptorExpandResolveRect>) = 
        {
            Next = ExtensionDecoder.decode<IRenderPassDescriptorExtension> backend.NextInChain
            X = int(backend.X)
            Y = int(backend.Y)
            Width = int(backend.Width)
            Height = int(backend.Height)
        }
type RenderPassPixelLocalStorage = 
    {
        Next : IRenderPassDescriptorExtension
        TotalPixelLocalStorageSize : int64
        StorageAttachments : array<RenderPassStorageAttachment>
    }
    static member Null = Unchecked.defaultof<RenderPassPixelLocalStorage>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderPassPixelLocalStorage> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.RenderPassPixelLocalStorage
                WebGPU.Raw.Pinnable.pinArray this.StorageAttachments (fun storageAttachmentsPtr ->
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
                    action ptr
                )
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IRenderPassDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPassPixelLocalStorage> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderPassPixelLocalStorage>) = 
        {
            Next = ExtensionDecoder.decode<IRenderPassDescriptorExtension> backend.NextInChain
            TotalPixelLocalStorageSize = int64(backend.TotalPixelLocalStorageSize)
            StorageAttachments = let ptr = backend.StorageAttachments in Array.init (int backend.StorageAttachmentCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in RenderPassStorageAttachment.Read(&r))
        }
type RenderPassStorageAttachment = 
    {
        Next : IExtension
        Offset : int64
        Storage : TextureView
        LoadOp : LoadOp
        StoreOp : StoreOp
        ClearValue : Color
    }
    static member Null = Unchecked.defaultof<RenderPassStorageAttachment>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderPassStorageAttachment> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                this.ClearValue.Pin(fun _clearValuePtr ->
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
                    action ptr
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPassStorageAttachment> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderPassStorageAttachment>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Offset = int64(backend.Offset)
            Storage = new TextureView(backend.Storage)
            LoadOp = backend.LoadOp
            StoreOp = backend.StoreOp
            ClearValue = Color.Read(&backend.ClearValue)
        }
type RenderPassEncoder internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"RenderPassEncoder(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? RenderPassEncoder as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new RenderPassEncoder(0n)
    member _.SetPipeline(pipeline : RenderPipeline) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetPipeline(handle, pipeline.Handle)
        res
    member _.SetBindGroup(groupIndex : int, group : BindGroup, dynamicOffsets : array<uint32>) : unit =
        use dynamicOffsetsPtr = fixed (dynamicOffsets)
        let dynamicOffsetsLen = unativeint dynamicOffsets.Length
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetBindGroup(handle, uint32(groupIndex), group.Handle, dynamicOffsetsLen, dynamicOffsetsPtr)
        res
    member _.Draw(vertexCount : int, instanceCount : int, firstVertex : int, firstInstance : int) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderDraw(handle, uint32(vertexCount), uint32(instanceCount), uint32(firstVertex), uint32(firstInstance))
        res
    member _.DrawIndexed(indexCount : int, instanceCount : int, firstIndex : int, baseVertex : int, firstInstance : int) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderDrawIndexed(handle, uint32(indexCount), uint32(instanceCount), uint32(firstIndex), baseVertex, uint32(firstInstance))
        res
    member _.DrawIndirect(indirectBuffer : Buffer, indirectOffset : int64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderDrawIndirect(handle, indirectBuffer.Handle, uint64(indirectOffset))
        res
    member _.DrawIndexedIndirect(indirectBuffer : Buffer, indirectOffset : int64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderDrawIndexedIndirect(handle, indirectBuffer.Handle, uint64(indirectOffset))
        res
    member _.MultiDrawIndirect(indirectBuffer : Buffer, indirectOffset : int64, maxDrawCount : int, drawCountBuffer : Buffer, drawCountBufferOffset : int64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderMultiDrawIndirect(handle, indirectBuffer.Handle, uint64(indirectOffset), uint32(maxDrawCount), drawCountBuffer.Handle, uint64(drawCountBufferOffset))
        res
    member _.MultiDrawIndexedIndirect(indirectBuffer : Buffer, indirectOffset : int64, maxDrawCount : int, drawCountBuffer : Buffer, drawCountBufferOffset : int64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderMultiDrawIndexedIndirect(handle, indirectBuffer.Handle, uint64(indirectOffset), uint32(maxDrawCount), drawCountBuffer.Handle, uint64(drawCountBufferOffset))
        res
    member _.ExecuteBundles(bundles : array<RenderBundle>) : unit =
        let bundlesHandles = bundles |> Array.map (fun a -> a.Handle)
        use bundlesPtr = fixed (bundlesHandles)
        let bundlesLen = unativeint bundles.Length
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderExecuteBundles(handle, bundlesLen, bundlesPtr)
        res
    member _.InsertDebugMarker(markerLabel : string) : unit =
        let _markerLabelArr = Encoding.UTF8.GetBytes(markerLabel)
        use _markerLabelPtr = fixed _markerLabelArr
        let _markerLabelLen = WebGPU.Raw.StringView(_markerLabelPtr, unativeint _markerLabelArr.Length)
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderInsertDebugMarker(handle, _markerLabelLen)
        res
    member _.PopDebugGroup() : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderPopDebugGroup(handle)
        res
    member _.PushDebugGroup(groupLabel : string) : unit =
        let _groupLabelArr = Encoding.UTF8.GetBytes(groupLabel)
        use _groupLabelPtr = fixed _groupLabelArr
        let _groupLabelLen = WebGPU.Raw.StringView(_groupLabelPtr, unativeint _groupLabelArr.Length)
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderPushDebugGroup(handle, _groupLabelLen)
        res
    member _.SetStencilReference(reference : int) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetStencilReference(handle, uint32(reference))
        res
    member _.SetBlendConstant(color : Color) : unit =
        color.Pin(fun _colorPtr ->
            let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetBlendConstant(handle, _colorPtr)
            res
        )
    member _.SetViewport(x : float32, y : float32, width : float32, height : float32, minDepth : float32, maxDepth : float32) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetViewport(handle, x, y, width, height, minDepth, maxDepth)
        res
    member _.SetScissorRect(x : int, y : int, width : int, height : int) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetScissorRect(handle, uint32(x), uint32(y), uint32(width), uint32(height))
        res
    member _.SetVertexBuffer(slot : int, buffer : Buffer, offset : int64, size : int64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetVertexBuffer(handle, uint32(slot), buffer.Handle, uint64(offset), uint64(size))
        res
    member _.SetIndexBuffer(buffer : Buffer, format : IndexFormat, offset : int64, size : int64) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetIndexBuffer(handle, buffer.Handle, format, uint64(offset), uint64(size))
        res
    member _.BeginOcclusionQuery(queryIndex : int) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderBeginOcclusionQuery(handle, uint32(queryIndex))
        res
    member _.EndOcclusionQuery() : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderEndOcclusionQuery(handle)
        res
    member _.WriteTimestamp(querySet : QuerySet, queryIndex : int) : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderWriteTimestamp(handle, querySet.Handle, uint32(queryIndex))
        res
    member _.PixelLocalStorageBarrier() : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderPixelLocalStorageBarrier(handle)
        res
    member _.End() : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderEnd(handle)
        res
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderSetLabel(handle, _labelLen)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.RenderPassEncoderRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type RenderPassTimestampWrites = 
    {
        QuerySet : QuerySet
        BeginningOfPassWriteIndex : int
        EndOfPassWriteIndex : int
    }
    static member Null = Unchecked.defaultof<RenderPassTimestampWrites>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderPassTimestampWrites> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.RenderPassTimestampWrites(
                    this.QuerySet.Handle,
                    uint32(this.BeginningOfPassWriteIndex),
                    uint32(this.EndOfPassWriteIndex)
                )
            use ptr = fixed &value
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPassTimestampWrites> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderPassTimestampWrites>) = 
        {
            QuerySet = new QuerySet(backend.QuerySet)
            BeginningOfPassWriteIndex = int(backend.BeginningOfPassWriteIndex)
            EndOfPassWriteIndex = int(backend.EndOfPassWriteIndex)
        }
type RenderPipeline internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"RenderPipeline(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? RenderPipeline as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new RenderPipeline(0n)
    member _.GetBindGroupLayout(groupIndex : int) : BindGroupLayout =
        let res = WebGPU.Raw.WebGPU.RenderPipelineGetBindGroupLayout(handle, uint32(groupIndex))
        new BindGroupLayout(res)
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.RenderPipelineSetLabel(handle, _labelLen)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.RenderPipelineRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type RequestDeviceCallback = delegate of IDisposable * status : RequestDeviceStatus * device : Device * message : string -> unit
type RequestDeviceCallback2 = delegate of IDisposable * status : RequestDeviceStatus * device : Device * message : string -> unit
type RequestDeviceCallbackInfo = 
    {
        Next : IExtension
        Mode : CallbackMode
        Callback : RequestDeviceCallback
    }
    static member Null = Unchecked.defaultof<RequestDeviceCallbackInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RequestDeviceCallbackInfo> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.RequestDeviceCallback>
                _callbackDel <- WebGPU.Raw.RequestDeviceCallback(fun status device message userdata ->
                    let _status = status
                    let _device = new Device(device)
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _device, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.RequestDeviceCallbackInfo(
                        nextInChain,
                        this.Mode,
                        _callbackPtr,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RequestDeviceCallbackInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RequestDeviceCallbackInfo>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
type RequestDeviceCallbackInfo2 = 
    {
        Next : IExtension
        Mode : CallbackMode
        Callback : RequestDeviceCallback2
    }
    static member Null = Unchecked.defaultof<RequestDeviceCallbackInfo2>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RequestDeviceCallbackInfo2> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable _callbackGC = Unchecked.defaultof<GCHandle>
                let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.RequestDeviceCallback2>
                _callbackDel <- WebGPU.Raw.RequestDeviceCallback2(fun status device message userdata1 userdata2 ->
                    let _status = status
                    let _device = new Device(device)
                    let _message = let _messagePtr = NativePtr.toNativeInt(message.Data) in if _messagePtr = 0n then null else Marshal.PtrToStringUTF8(_messagePtr, int(message.Length))
                    this.Callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _device, _message)
                )
                _callbackGC <- GCHandle.Alloc(_callbackDel)
                let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
                let mutable value =
                    new WebGPU.Raw.RequestDeviceCallbackInfo2(
                        nextInChain,
                        this.Mode,
                        _callbackPtr,
                        Unchecked.defaultof<_>,
                        Unchecked.defaultof<_>
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RequestDeviceCallbackInfo2> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RequestDeviceCallbackInfo2>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Mode = backend.Mode
            Callback = failwith "cannot read callbacks"//TODO2 map [(callback, backend.Callback); (mode, backend.Mode); (next in chain, backend.NextInChain); ... ]
        }
type VertexState = 
    {
        Next : IExtension
        Module : ShaderModule
        EntryPoint : string
        Constants : array<ConstantEntry>
        Buffers : array<VertexBufferLayout>
    }
    static member Null = Unchecked.defaultof<VertexState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.VertexState> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _entryPointArr = Encoding.UTF8.GetBytes(this.EntryPoint)
                use _entryPointPtr = fixed _entryPointArr
                let _entryPointLen = WebGPU.Raw.StringView(_entryPointPtr, unativeint _entryPointArr.Length)
                WebGPU.Raw.Pinnable.pinArray this.Constants (fun constantsPtr ->
                    let constantsLen = unativeint this.Constants.Length
                    WebGPU.Raw.Pinnable.pinArray this.Buffers (fun buffersPtr ->
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
                        action ptr
                    )
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.VertexState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.VertexState>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Module = new ShaderModule(backend.Module)
            EntryPoint = let _entryPointPtr = NativePtr.toNativeInt(backend.EntryPoint.Data) in if _entryPointPtr = 0n then null else Marshal.PtrToStringUTF8(_entryPointPtr, int(backend.EntryPoint.Length))
            Constants = let ptr = backend.Constants in Array.init (int backend.ConstantCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in ConstantEntry.Read(&r))
            Buffers = let ptr = backend.Buffers in Array.init (int backend.BufferCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in VertexBufferLayout.Read(&r))
        }
type PrimitiveState = 
    {
        Next : IExtension
        Topology : PrimitiveTopology
        StripIndexFormat : IndexFormat
        FrontFace : FrontFace
        CullMode : CullMode
        UnclippedDepth : bool
    }
    static member Null = Unchecked.defaultof<PrimitiveState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.PrimitiveState> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
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
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.PrimitiveState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.PrimitiveState>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Topology = backend.Topology
            StripIndexFormat = backend.StripIndexFormat
            FrontFace = backend.FrontFace
            CullMode = backend.CullMode
            UnclippedDepth = (backend.UnclippedDepth <> 0)
        }
type DepthStencilState = 
    {
        Next : IExtension
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DepthStencilState> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                this.StencilFront.Pin(fun _stencilFrontPtr ->
                    this.StencilBack.Pin(fun _stencilBackPtr ->
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
                        action ptr
                    )
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DepthStencilState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DepthStencilState>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Format = backend.Format
            DepthWriteEnabled = backend.DepthWriteEnabled
            DepthCompare = backend.DepthCompare
            StencilFront = StencilFaceState.Read(&backend.StencilFront)
            StencilBack = StencilFaceState.Read(&backend.StencilBack)
            StencilReadMask = int(backend.StencilReadMask)
            StencilWriteMask = int(backend.StencilWriteMask)
            DepthBias = backend.DepthBias
            DepthBiasSlopeScale = backend.DepthBiasSlopeScale
            DepthBiasClamp = backend.DepthBiasClamp
        }
type MultisampleState = 
    {
        Next : IExtension
        Count : int
        Mask : int
        AlphaToCoverageEnabled : bool
    }
    static member Null = Unchecked.defaultof<MultisampleState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.MultisampleState> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable value =
                    new WebGPU.Raw.MultisampleState(
                        nextInChain,
                        uint32(this.Count),
                        uint32(this.Mask),
                        (if this.AlphaToCoverageEnabled then 1 else 0)
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.MultisampleState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.MultisampleState>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Count = int(backend.Count)
            Mask = int(backend.Mask)
            AlphaToCoverageEnabled = (backend.AlphaToCoverageEnabled <> 0)
        }
type FragmentState = 
    {
        Next : IExtension
        Module : ShaderModule
        EntryPoint : string
        Constants : array<ConstantEntry>
        Targets : array<ColorTargetState>
    }
    static member Null = Unchecked.defaultof<FragmentState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.FragmentState> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _entryPointArr = Encoding.UTF8.GetBytes(this.EntryPoint)
                use _entryPointPtr = fixed _entryPointArr
                let _entryPointLen = WebGPU.Raw.StringView(_entryPointPtr, unativeint _entryPointArr.Length)
                WebGPU.Raw.Pinnable.pinArray this.Constants (fun constantsPtr ->
                    let constantsLen = unativeint this.Constants.Length
                    WebGPU.Raw.Pinnable.pinArray this.Targets (fun targetsPtr ->
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
                        action ptr
                    )
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.FragmentState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.FragmentState>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Module = new ShaderModule(backend.Module)
            EntryPoint = let _entryPointPtr = NativePtr.toNativeInt(backend.EntryPoint.Data) in if _entryPointPtr = 0n then null else Marshal.PtrToStringUTF8(_entryPointPtr, int(backend.EntryPoint.Length))
            Constants = let ptr = backend.Constants in Array.init (int backend.ConstantCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in ConstantEntry.Read(&r))
            Targets = let ptr = backend.Targets in Array.init (int backend.TargetCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in ColorTargetState.Read(&r))
        }
type ColorTargetState = 
    {
        Next : IColorTargetStateExtension
        Format : TextureFormat
        Blend : BlendState
        WriteMask : ColorWriteMask
    }
    static member Null = Unchecked.defaultof<ColorTargetState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ColorTargetState> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                this.Blend.Pin(fun _blendPtr ->
                    let mutable value =
                        new WebGPU.Raw.ColorTargetState(
                            nextInChain,
                            this.Format,
                            _blendPtr,
                            this.WriteMask
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ColorTargetState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ColorTargetState>) = 
        {
            Next = ExtensionDecoder.decode<IColorTargetStateExtension> backend.NextInChain
            Format = backend.Format
            Blend = let m = NativePtr.toByRef backend.Blend in BlendState.Read(&m)
            WriteMask = backend.WriteMask
        }
type ColorTargetStateExpandResolveTextureDawn = 
    {
        Next : IColorTargetStateExtension
        Enabled : bool
    }
    static member Null = Unchecked.defaultof<ColorTargetStateExpandResolveTextureDawn>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ColorTargetStateExpandResolveTextureDawn> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IColorTargetStateExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ColorTargetStateExpandResolveTextureDawn> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ColorTargetStateExpandResolveTextureDawn>) = 
        {
            Next = ExtensionDecoder.decode<IColorTargetStateExtension> backend.NextInChain
            Enabled = (backend.Enabled <> 0)
        }
type BlendState = 
    {
        Color : BlendComponent
        Alpha : BlendComponent
    }
    static member Null = Unchecked.defaultof<BlendState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.BlendState> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            this.Color.Pin(fun _colorPtr ->
                this.Alpha.Pin(fun _alphaPtr ->
                    let mutable value =
                        new WebGPU.Raw.BlendState(
                            (if NativePtr.toNativeInt _colorPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _colorPtr),
                            (if NativePtr.toNativeInt _alphaPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _alphaPtr)
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.BlendState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.BlendState>) = 
        {
            Color = BlendComponent.Read(&backend.Color)
            Alpha = BlendComponent.Read(&backend.Alpha)
        }
type RenderPipelineDescriptor = 
    {
        Next : IExtension
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.RenderPipelineDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                this.Vertex.Pin(fun _vertexPtr ->
                    this.Primitive.Pin(fun _primitivePtr ->
                        this.DepthStencil.Pin(fun _depthStencilPtr ->
                            this.Multisample.Pin(fun _multisamplePtr ->
                                this.Fragment.Pin(fun _fragmentPtr ->
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
                                    action ptr
                                )
                            )
                        )
                    )
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.RenderPipelineDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.RenderPipelineDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            Layout = new PipelineLayout(backend.Layout)
            Vertex = VertexState.Read(&backend.Vertex)
            Primitive = PrimitiveState.Read(&backend.Primitive)
            DepthStencil = let m = NativePtr.toByRef backend.DepthStencil in DepthStencilState.Read(&m)
            Multisample = MultisampleState.Read(&backend.Multisample)
            Fragment = let m = NativePtr.toByRef backend.Fragment in FragmentState.Read(&m)
        }
type Sampler internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"Sampler(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? Sampler as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new Sampler(0n)
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.SamplerSetLabel(handle, _labelLen)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.SamplerRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SamplerDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
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
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SamplerDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SamplerDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISamplerDescriptorExtension> backend.NextInChain
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
type ShaderModule internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"ShaderModule(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? ShaderModule as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new ShaderModule(0n)
    member _.GetCompilationInfo(callback : CompilationInfoCallback) : unit =
        let mutable _callbackGC = Unchecked.defaultof<GCHandle>
        let mutable _callbackDel = Unchecked.defaultof<WebGPU.Raw.CompilationInfoCallback>
        _callbackDel <- WebGPU.Raw.CompilationInfoCallback(fun status compilationInfo userdata ->
            let _status = status
            let _compilationInfo = let m = NativePtr.toByRef compilationInfo in CompilationInfo.Read(&m)
            callback.Invoke({ new IDisposable with member __.Dispose() = _callbackGC.Free() }, _status, _compilationInfo)
        )
        _callbackGC <- GCHandle.Alloc(_callbackDel)
        let _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callbackDel)
        let res = WebGPU.Raw.WebGPU.ShaderModuleGetCompilationInfo(handle, _callbackPtr, Unchecked.defaultof<_>)
        res
    member _.GetCompilationInfoF(callbackInfo : CompilationInfoCallbackInfo) : Future =
        callbackInfo.Pin(fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.ShaderModuleGetCompilationInfoF(handle, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
            Future.Read(&res)
        )
    member _.GetCompilationInfo2(callbackInfo : CompilationInfoCallbackInfo2) : Future =
        callbackInfo.Pin(fun _callbackInfoPtr ->
            let res = WebGPU.Raw.WebGPU.ShaderModuleGetCompilationInfo2(handle, (if NativePtr.toNativeInt _callbackInfoPtr = 0n then Unchecked.defaultof<_> else NativePtr.read _callbackInfoPtr))
            Future.Read(&res)
        )
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.ShaderModuleSetLabel(handle, _labelLen)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.ShaderModuleRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type ShaderModuleDescriptor = 
    {
        Next : IShaderModuleDescriptorExtension
        Label : string
    }
    static member Null = Unchecked.defaultof<ShaderModuleDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ShaderModuleDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                let mutable value =
                    new WebGPU.Raw.ShaderModuleDescriptor(
                        nextInChain,
                        _labelLen
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ShaderModuleDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ShaderModuleDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IShaderModuleDescriptorExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
        }
type ShaderModuleSPIRVDescriptor = ShaderSourceSPIRV
type ShaderSourceSPIRV = 
    {
        Next : IShaderModuleDescriptorExtension
        Code : array<uint32>
    }
    static member Null = Unchecked.defaultof<ShaderSourceSPIRV>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ShaderSourceSPIRV> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.ShaderSourceSPIRV
                use codePtr = fixed (this.Code)
                let codeLen = uint32 this.Code.Length
                let mutable value =
                    new WebGPU.Raw.ShaderSourceSPIRV(
                        nextInChain,
                        sType,
                        codeLen,
                        codePtr
                    )
                use ptr = fixed &value
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IShaderModuleDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ShaderSourceSPIRV> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ShaderSourceSPIRV>) = 
        {
            Next = ExtensionDecoder.decode<IShaderModuleDescriptorExtension> backend.NextInChain
            Code = let ptr = backend.Code in Array.init (int backend.CodeSize) (fun i -> NativePtr.get ptr i)
        }
type ShaderModuleWGSLDescriptor = ShaderSourceWGSL
type ShaderSourceWGSL = 
    {
        Next : IShaderModuleDescriptorExtension
        Code : string
    }
    static member Null = Unchecked.defaultof<ShaderSourceWGSL>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ShaderSourceWGSL> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.ShaderSourceWGSL
                let _codeArr = Encoding.UTF8.GetBytes(this.Code)
                use _codePtr = fixed _codeArr
                let _codeLen = WebGPU.Raw.StringView(_codePtr, unativeint _codeArr.Length)
                let mutable value =
                    new WebGPU.Raw.ShaderSourceWGSL(
                        nextInChain,
                        sType,
                        _codeLen
                    )
                use ptr = fixed &value
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IShaderModuleDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ShaderSourceWGSL> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ShaderSourceWGSL>) = 
        {
            Next = ExtensionDecoder.decode<IShaderModuleDescriptorExtension> backend.NextInChain
            Code = let _codePtr = NativePtr.toNativeInt(backend.Code.Data) in if _codePtr = 0n then null else Marshal.PtrToStringUTF8(_codePtr, int(backend.Code.Length))
        }
type DawnShaderModuleSPIRVOptionsDescriptor = 
    {
        Next : IShaderModuleDescriptorExtension
        AllowNonUniformDerivatives : bool
    }
    static member Null = Unchecked.defaultof<DawnShaderModuleSPIRVOptionsDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DawnShaderModuleSPIRVOptionsDescriptor> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IShaderModuleDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnShaderModuleSPIRVOptionsDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DawnShaderModuleSPIRVOptionsDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<IShaderModuleDescriptorExtension> backend.NextInChain
            AllowNonUniformDerivatives = (backend.AllowNonUniformDerivatives <> 0)
        }
type ShaderModuleCompilationOptions = 
    {
        Next : IShaderModuleDescriptorExtension
        StrictMath : bool
    }
    static member Null = Unchecked.defaultof<ShaderModuleCompilationOptions>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.ShaderModuleCompilationOptions> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IShaderModuleDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.ShaderModuleCompilationOptions> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.ShaderModuleCompilationOptions>) = 
        {
            Next = ExtensionDecoder.decode<IShaderModuleDescriptorExtension> backend.NextInChain
            StrictMath = (backend.StrictMath <> 0)
        }
type StencilFaceState = 
    {
        Compare : CompareFunction
        FailOp : StencilOperation
        DepthFailOp : StencilOperation
        PassOp : StencilOperation
    }
    static member Null = Unchecked.defaultof<StencilFaceState>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.StencilFaceState> -> 'r) : 'r = 
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
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.StencilFaceState> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.StencilFaceState>) = 
        {
            Compare = backend.Compare
            FailOp = backend.FailOp
            DepthFailOp = backend.DepthFailOp
            PassOp = backend.PassOp
        }
type Surface internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"Surface(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? Surface as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new Surface(0n)
    member _.Configure(config : SurfaceConfiguration) : unit =
        config.Pin(fun _configPtr ->
            let res = WebGPU.Raw.WebGPU.SurfaceConfigure(handle, _configPtr)
            res
        )
    member _.GetCapabilities(adapter : Adapter, capabilities : byref<SurfaceCapabilities>) : Status =
        let mutable capabilitiesCopy = capabilities
        try
            capabilities.Pin(fun _capabilitiesPtr ->
                if NativePtr.toNativeInt _capabilitiesPtr = 0n then
                    let mutable capabilitiesNative = Unchecked.defaultof<WebGPU.Raw.SurfaceCapabilities>
                    use _capabilitiesPtr = fixed &capabilitiesNative
                    let res = WebGPU.Raw.WebGPU.SurfaceGetCapabilities(handle, adapter.Handle, _capabilitiesPtr)
                    let _ret = res
                    capabilitiesCopy <- SurfaceCapabilities.Read(&capabilitiesNative)
                    _ret
                else
                    let res = WebGPU.Raw.WebGPU.SurfaceGetCapabilities(handle, adapter.Handle, _capabilitiesPtr)
                    let _ret = res
                    let capabilitiesResult = NativePtr.toByRef _capabilitiesPtr
                    capabilitiesCopy <- SurfaceCapabilities.Read(&capabilitiesResult)
                    _ret
            )
        finally
            capabilities <- capabilitiesCopy
    member _.CurrentTexture : SurfaceTexture =
        let mutable res = Unchecked.defaultof<_>
        let ptr = fixed &res
        WebGPU.Raw.WebGPU.SurfaceGetCurrentTexture(handle, ptr)
        SurfaceTexture.Read(&res)
    member _.Present() : unit =
        let res = WebGPU.Raw.WebGPU.SurfacePresent(handle)
        res
    member _.Unconfigure() : unit =
        let res = WebGPU.Raw.WebGPU.SurfaceUnconfigure(handle)
        res
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.SurfaceSetLabel(handle, _labelLen)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.SurfaceRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type SurfaceDescriptor = 
    {
        Next : ISurfaceDescriptorExtension
        Label : string
    }
    static member Null = Unchecked.defaultof<SurfaceDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SurfaceDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                let mutable value =
                    new WebGPU.Raw.SurfaceDescriptor(
                        nextInChain,
                        _labelLen
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SurfaceDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
        }
type SurfaceDescriptorFromAndroidNativeWindow = SurfaceSourceAndroidNativeWindow
type SurfaceSourceAndroidNativeWindow = 
    {
        Next : ISurfaceDescriptorExtension
        Window : nativeint
    }
    static member Null = Unchecked.defaultof<SurfaceSourceAndroidNativeWindow>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SurfaceSourceAndroidNativeWindow> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceSourceAndroidNativeWindow> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SurfaceSourceAndroidNativeWindow>) = 
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> backend.NextInChain
            Window = backend.Window
        }
type SurfaceDescriptorFromCanvasHTMLSelector = SurfaceSourceCanvasHTMLSelector_Emscripten
type SurfaceSourceCanvasHTMLSelector_Emscripten = 
    {
        Next : ISurfaceDescriptorExtension
        Selector : string
    }
    static member Null = Unchecked.defaultof<SurfaceSourceCanvasHTMLSelector_Emscripten>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SurfaceSourceCanvasHTMLSelector_Emscripten> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SurfaceSourceCanvasHTMLSelector_Emscripten
                let _selectorArr = Encoding.UTF8.GetBytes(this.Selector)
                use _selectorPtr = fixed _selectorArr
                let _selectorLen = WebGPU.Raw.StringView(_selectorPtr, unativeint _selectorArr.Length)
                let mutable value =
                    new WebGPU.Raw.SurfaceSourceCanvasHTMLSelector_Emscripten(
                        nextInChain,
                        sType,
                        _selectorLen
                    )
                use ptr = fixed &value
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceSourceCanvasHTMLSelector_Emscripten> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SurfaceSourceCanvasHTMLSelector_Emscripten>) = 
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> backend.NextInChain
            Selector = let _selectorPtr = NativePtr.toNativeInt(backend.Selector.Data) in if _selectorPtr = 0n then null else Marshal.PtrToStringUTF8(_selectorPtr, int(backend.Selector.Length))
        }
type SurfaceDescriptorFromMetalLayer = SurfaceSourceMetalLayer
type SurfaceSourceMetalLayer = 
    {
        Next : ISurfaceDescriptorExtension
        Layer : nativeint
    }
    static member Null = Unchecked.defaultof<SurfaceSourceMetalLayer>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SurfaceSourceMetalLayer> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceSourceMetalLayer> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SurfaceSourceMetalLayer>) = 
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> backend.NextInChain
            Layer = backend.Layer
        }
type SurfaceDescriptorFromWindowsHWND = SurfaceSourceWindowsHWND
type SurfaceSourceWindowsHWND = 
    {
        Next : ISurfaceDescriptorExtension
        Hinstance : nativeint
        Hwnd : nativeint
    }
    static member Null = Unchecked.defaultof<SurfaceSourceWindowsHWND>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SurfaceSourceWindowsHWND> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceSourceWindowsHWND> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SurfaceSourceWindowsHWND>) = 
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> backend.NextInChain
            Hinstance = backend.Hinstance
            Hwnd = backend.Hwnd
        }
type SurfaceDescriptorFromXcbWindow = SurfaceSourceXCBWindow
type SurfaceSourceXCBWindow = 
    {
        Next : ISurfaceDescriptorExtension
        Connection : nativeint
        Window : int
    }
    static member Null = Unchecked.defaultof<SurfaceSourceXCBWindow>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SurfaceSourceXCBWindow> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceSourceXCBWindow> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SurfaceSourceXCBWindow>) = 
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> backend.NextInChain
            Connection = backend.Connection
            Window = int(backend.Window)
        }
type SurfaceDescriptorFromXlibWindow = SurfaceSourceXlibWindow
type SurfaceSourceXlibWindow = 
    {
        Next : ISurfaceDescriptorExtension
        Display : nativeint
        Window : int64
    }
    static member Null = Unchecked.defaultof<SurfaceSourceXlibWindow>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SurfaceSourceXlibWindow> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceSourceXlibWindow> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SurfaceSourceXlibWindow>) = 
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> backend.NextInChain
            Display = backend.Display
            Window = int64(backend.Window)
        }
type SurfaceDescriptorFromWaylandSurface = SurfaceSourceWaylandSurface
type SurfaceSourceWaylandSurface = 
    {
        Next : ISurfaceDescriptorExtension
        Display : nativeint
        Surface : nativeint
    }
    static member Null = Unchecked.defaultof<SurfaceSourceWaylandSurface>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SurfaceSourceWaylandSurface> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceSourceWaylandSurface> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SurfaceSourceWaylandSurface>) = 
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> backend.NextInChain
            Display = backend.Display
            Surface = backend.Surface
        }
type SurfaceDescriptorFromWindowsCoreWindow = 
    {
        Next : ISurfaceDescriptorExtension
        CoreWindow : nativeint
    }
    static member Null = Unchecked.defaultof<SurfaceDescriptorFromWindowsCoreWindow>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SurfaceDescriptorFromWindowsCoreWindow> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceDescriptorFromWindowsCoreWindow> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SurfaceDescriptorFromWindowsCoreWindow>) = 
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> backend.NextInChain
            CoreWindow = backend.CoreWindow
        }
type SurfaceDescriptorFromWindowsSwapChainPanel = 
    {
        Next : ISurfaceDescriptorExtension
        SwapChainPanel : nativeint
    }
    static member Null = Unchecked.defaultof<SurfaceDescriptorFromWindowsSwapChainPanel>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SurfaceDescriptorFromWindowsSwapChainPanel> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.SurfaceDescriptorFromWindowsSwapChainPanel
                let mutable value =
                    new WebGPU.Raw.SurfaceDescriptorFromWindowsSwapChainPanel(
                        nextInChain,
                        sType,
                        this.SwapChainPanel
                    )
                use ptr = fixed &value
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISurfaceDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceDescriptorFromWindowsSwapChainPanel> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SurfaceDescriptorFromWindowsSwapChainPanel>) = 
        {
            Next = ExtensionDecoder.decode<ISurfaceDescriptorExtension> backend.NextInChain
            SwapChainPanel = backend.SwapChainPanel
        }
type SurfaceTexture = 
    {
        Texture : Texture
        Suboptimal : bool
        Status : SurfaceGetCurrentTextureStatus
    }
    static member Null = Unchecked.defaultof<SurfaceTexture>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.SurfaceTexture> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.SurfaceTexture(
                    this.Texture.Handle,
                    (if this.Suboptimal then 1 else 0),
                    this.Status
                )
            use ptr = fixed &value
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.SurfaceTexture> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.SurfaceTexture>) = 
        {
            Texture = new Texture(backend.Texture)
            Suboptimal = (backend.Suboptimal <> 0)
            Status = backend.Status
        }
type Texture internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"Texture(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? Texture as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new Texture(0n)
    member _.CreateView(descriptor : TextureViewDescriptor) : TextureView =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.TextureCreateView(handle, _descriptorPtr)
            new TextureView(res)
        )
    member _.CreateErrorView(descriptor : TextureViewDescriptor) : TextureView =
        descriptor.Pin(fun _descriptorPtr ->
            let res = WebGPU.Raw.WebGPU.TextureCreateErrorView(handle, _descriptorPtr)
            new TextureView(res)
        )
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.TextureSetLabel(handle, _labelLen)
        res
    member this.Width : int =
        let mutable res = WebGPU.Raw.WebGPU.TextureGetWidth(handle)
        int(res)
    member this.Height : int =
        let mutable res = WebGPU.Raw.WebGPU.TextureGetHeight(handle)
        int(res)
    member this.DepthOrArrayLayers : int =
        let mutable res = WebGPU.Raw.WebGPU.TextureGetDepthOrArrayLayers(handle)
        int(res)
    member this.MipLevelCount : int =
        let mutable res = WebGPU.Raw.WebGPU.TextureGetMipLevelCount(handle)
        int(res)
    member this.SampleCount : int =
        let mutable res = WebGPU.Raw.WebGPU.TextureGetSampleCount(handle)
        int(res)
    member this.Dimension : TextureDimension =
        let mutable res = WebGPU.Raw.WebGPU.TextureGetDimension(handle)
        res
    member this.Format : TextureFormat =
        let mutable res = WebGPU.Raw.WebGPU.TextureGetFormat(handle)
        res
    member this.Usage : TextureUsage =
        let mutable res = WebGPU.Raw.WebGPU.TextureGetUsage(handle)
        res
    member _.Destroy() : unit =
        let res = WebGPU.Raw.WebGPU.TextureDestroy(handle)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.TextureRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
type TextureDataLayout = 
    {
        Next : IExtension
        Offset : int64
        BytesPerRow : int
        RowsPerImage : int
    }
    static member Null = Unchecked.defaultof<TextureDataLayout>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.TextureDataLayout> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let mutable value =
                    new WebGPU.Raw.TextureDataLayout(
                        nextInChain,
                        uint64(this.Offset),
                        uint32(this.BytesPerRow),
                        uint32(this.RowsPerImage)
                    )
                use ptr = fixed &value
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.TextureDataLayout> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.TextureDataLayout>) = 
        {
            Next = ExtensionDecoder.decode<IExtension> backend.NextInChain
            Offset = int64(backend.Offset)
            BytesPerRow = int(backend.BytesPerRow)
            RowsPerImage = int(backend.RowsPerImage)
        }
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.TextureDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
                this.Size.Pin(fun _sizePtr ->
                    use viewFormatsPtr = fixed (this.ViewFormats)
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
                    action ptr
                )
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.TextureDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.TextureDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ITextureDescriptorExtension> backend.NextInChain
            Label = let _labelPtr = NativePtr.toNativeInt(backend.Label.Data) in if _labelPtr = 0n then null else Marshal.PtrToStringUTF8(_labelPtr, int(backend.Label.Length))
            Usage = backend.Usage
            Dimension = backend.Dimension
            Size = Extent3D.Read(&backend.Size)
            Format = backend.Format
            MipLevelCount = int(backend.MipLevelCount)
            SampleCount = int(backend.SampleCount)
            ViewFormats = let ptr = backend.ViewFormats in Array.init (int backend.ViewFormatCount) (fun i -> NativePtr.get ptr i)
        }
type TextureBindingViewDimensionDescriptor = 
    {
        Next : ITextureDescriptorExtension
        TextureBindingViewDimension : TextureViewDimension
    }
    static member Null = Unchecked.defaultof<TextureBindingViewDimensionDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.TextureBindingViewDimensionDescriptor> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ITextureDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.TextureBindingViewDimensionDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.TextureBindingViewDimensionDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ITextureDescriptorExtension> backend.NextInChain
            TextureBindingViewDimension = backend.TextureBindingViewDimension
        }
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.TextureViewDescriptor> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let _labelArr = Encoding.UTF8.GetBytes(this.Label)
                use _labelPtr = fixed _labelArr
                let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
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
                action ptr
            )
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.TextureViewDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.TextureViewDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ITextureViewDescriptorExtension> backend.NextInChain
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
type TextureView internal(handle : nativeint) =
    member x.Handle = handle
    override x.ToString() = $"TextureView(%08X{handle})"
    override x.GetHashCode() = hash handle
    override x.Equals(obj) =
        match obj with
        | :? TextureView as other -> other.Handle = x.Handle
        | _ -> false
    static member Null = new TextureView(0n)
    member _.SetLabel(label : string) : unit =
        let _labelArr = Encoding.UTF8.GetBytes(label)
        use _labelPtr = fixed _labelArr
        let _labelLen = WebGPU.Raw.StringView(_labelPtr, unativeint _labelArr.Length)
        let res = WebGPU.Raw.WebGPU.TextureViewSetLabel(handle, _labelLen)
        res
    member _.Release() : unit =
        let res = WebGPU.Raw.WebGPU.TextureViewRelease(handle)
        res
    member private x.Dispose(disposing : bool) =
        if disposing then System.GC.SuppressFinalize(x)
        x.Release()
    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)
    interface System.IDisposable with
        member x.Dispose() = x.Dispose(true)
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
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.YCbCrVkDescriptor> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ISamplerDescriptorExtension
    interface ITextureViewDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.YCbCrVkDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.YCbCrVkDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ISamplerDescriptorExtension> backend.NextInChain
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
type DawnTextureInternalUsageDescriptor = 
    {
        Next : ITextureDescriptorExtension
        InternalUsage : TextureUsage
    }
    static member Null = Unchecked.defaultof<DawnTextureInternalUsageDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DawnTextureInternalUsageDescriptor> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ITextureDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnTextureInternalUsageDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DawnTextureInternalUsageDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ITextureDescriptorExtension> backend.NextInChain
            InternalUsage = backend.InternalUsage
        }
type DawnEncoderInternalUsageDescriptor = 
    {
        Next : ICommandEncoderDescriptorExtension
        UseInternalUsages : bool
    }
    static member Null = Unchecked.defaultof<DawnEncoderInternalUsageDescriptor>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DawnEncoderInternalUsageDescriptor> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface ICommandEncoderDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnEncoderInternalUsageDescriptor> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DawnEncoderInternalUsageDescriptor>) = 
        {
            Next = ExtensionDecoder.decode<ICommandEncoderDescriptorExtension> backend.NextInChain
            UseInternalUsages = (backend.UseInternalUsages <> 0)
        }
type DawnAdapterPropertiesPowerPreference = 
    {
        Next : IAdapterInfoExtension
        PowerPreference : PowerPreference
    }
    static member Null = Unchecked.defaultof<DawnAdapterPropertiesPowerPreference>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DawnAdapterPropertiesPowerPreference> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IAdapterInfoExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnAdapterPropertiesPowerPreference> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DawnAdapterPropertiesPowerPreference>) = 
        {
            Next = ExtensionDecoder.decode<IAdapterInfoExtension> backend.NextInChain
            PowerPreference = backend.PowerPreference
        }
type MemoryHeapInfo = 
    {
        Properties : HeapProperty
        Size : int64
    }
    static member Null = Unchecked.defaultof<MemoryHeapInfo>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.MemoryHeapInfo> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            let mutable value =
                new WebGPU.Raw.MemoryHeapInfo(
                    this.Properties,
                    uint64(this.Size)
                )
            use ptr = fixed &value
            action ptr
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.MemoryHeapInfo> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.MemoryHeapInfo>) = 
        {
            Properties = backend.Properties
            Size = int64(backend.Size)
        }
type AdapterPropertiesMemoryHeaps = 
    {
        Next : IAdapterInfoExtension
        HeapInfo : array<MemoryHeapInfo>
    }
    static member Null = Unchecked.defaultof<AdapterPropertiesMemoryHeaps>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.AdapterPropertiesMemoryHeaps> -> 'r) : 'r = 
        if isNull (this :> obj) then
            action (NativePtr.ofNativeInt 0n)
        else
            PinHelper.PinNullable(this.Next, fun nextInChain ->
                let sType = SType.AdapterPropertiesMemoryHeaps
                WebGPU.Raw.Pinnable.pinArray this.HeapInfo (fun heapInfoPtr ->
                    let heapInfoLen = unativeint this.HeapInfo.Length
                    let mutable value =
                        new WebGPU.Raw.AdapterPropertiesMemoryHeaps(
                            nextInChain,
                            sType,
                            heapInfoLen,
                            heapInfoPtr
                        )
                    use ptr = fixed &value
                    action ptr
                )
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IAdapterInfoExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.AdapterPropertiesMemoryHeaps> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.AdapterPropertiesMemoryHeaps>) = 
        {
            Next = ExtensionDecoder.decode<IAdapterInfoExtension> backend.NextInChain
            HeapInfo = let ptr = backend.HeapInfo in Array.init (int backend.HeapCount) (fun i -> let r = NativePtr.toByRef (NativePtr.add ptr i) in MemoryHeapInfo.Read(&r))
        }
type AdapterPropertiesD3D = 
    {
        Next : IAdapterInfoExtension
        ShaderModel : int
    }
    static member Null = Unchecked.defaultof<AdapterPropertiesD3D>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.AdapterPropertiesD3D> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IAdapterInfoExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.AdapterPropertiesD3D> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.AdapterPropertiesD3D>) = 
        {
            Next = ExtensionDecoder.decode<IAdapterInfoExtension> backend.NextInChain
            ShaderModel = int(backend.ShaderModel)
        }
type AdapterPropertiesVk = 
    {
        Next : IAdapterInfoExtension
        DriverVersion : int
    }
    static member Null = Unchecked.defaultof<AdapterPropertiesVk>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.AdapterPropertiesVk> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IAdapterInfoExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.AdapterPropertiesVk> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.AdapterPropertiesVk>) = 
        {
            Next = ExtensionDecoder.decode<IAdapterInfoExtension> backend.NextInChain
            DriverVersion = int(backend.DriverVersion)
        }
type DawnBufferDescriptorErrorInfoFromWireClient = 
    {
        Next : IBufferDescriptorExtension
        OutOfMemory : bool
    }
    static member Null = Unchecked.defaultof<DawnBufferDescriptorErrorInfoFromWireClient>
    [<CompilationRepresentation(CompilationRepresentationFlags.Static)>]
    member this.Pin<'r>(action : nativeptr<WebGPU.Raw.DawnBufferDescriptorErrorInfoFromWireClient> -> 'r) : 'r = 
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
                action ptr
            )
    interface IExtension with
        member x.Pin<'r>(action : nativeint -> 'r) = x.Pin(fun ptr -> action(NativePtr.toNativeInt ptr))
    interface IBufferDescriptorExtension
    interface WebGPU.Raw.IPinnable<WebGPU.Raw.DawnBufferDescriptorErrorInfoFromWireClient> with
        member x.Pin(action) = x.Pin(action)
    static member Read(backend : inref<WebGPU.Raw.DawnBufferDescriptorErrorInfoFromWireClient>) = 
        {
            Next = ExtensionDecoder.decode<IBufferDescriptorExtension> backend.NextInChain
            OutOfMemory = (backend.OutOfMemory <> 0)
        }
