
open Microsoft.FSharp.NativeInterop
open System
open WebGPU

[<EntryPoint>]
let main args =
    
    let run =
        task {
            let! adapter =
                Adapter.Request {
                    PowerPreference = PowerPreference.High
                    ForceFallbackAdapter = false 
                }
                
            printfn "Vendor: %A" adapter.Vendor
            printfn "Fallback: %A" adapter.IsFallback
            printfn "Architecture: %A" adapter.Architecture
            printfn "Device: %A" adapter.Device
            printfn "Description: %A" adapter.Description
            printfn "Handle: %A" adapter.Handle
            
            printfn "Features:"
            for f in Enum.GetValues<Features>() |> Array.filter (fun f -> adapter.Features.HasFlag f) do
                printfn "  %A" f
                
            printfn "Limits:"
            printfn "  MaxUniformBufferBindingSize: %A" adapter.Limits.MaxUniformBufferBindingSize
            printfn "  MaxStorageBufferBindingSize: %A" adapter.Limits.MaxStorageBufferBindingSize
            printfn "  MaxBufferSize: %A" adapter.Limits.MaxBufferSize
            printfn "  MaxTextureDimension1D: %A" adapter.Limits.MaxTextureDimension1D
            printfn "  MaxTextureDimension2D: %A" adapter.Limits.MaxTextureDimension2D
            printfn "  MaxTextureDimension3D: %A" adapter.Limits.MaxTextureDimension3D
            printfn "  MaxTextureArrayLayers: %A" adapter.Limits.MaxTextureArrayLayers
            printfn "  MaxBindGroups: %A" adapter.Limits.MaxBindGroups
            printfn "  MaxBindGroupsPlusVertexBuffers: %A" adapter.Limits.MaxBindGroupsPlusVertexBuffers
            printfn "  MaxBindingsPerBindGroup: %A" adapter.Limits.MaxBindingsPerBindGroup
            printfn "  MaxDynamicUniformBuffersPerPipelineLayout: %A" adapter.Limits.MaxDynamicUniformBuffersPerPipelineLayout
            printfn "  MaxDynamicStorageBuffersPerPipelineLayout: %A" adapter.Limits.MaxDynamicStorageBuffersPerPipelineLayout
            printfn "  MaxSampledTexturesPerShaderStage: %A" adapter.Limits.MaxSampledTexturesPerShaderStage
            printfn "  MaxSamplersPerShaderStage: %A" adapter.Limits.MaxSamplersPerShaderStage
            printfn "  MaxStorageBuffersPerShaderStage: %A" adapter.Limits.MaxStorageBuffersPerShaderStage
            printfn "  MaxStorageTexturesPerShaderStage: %A" adapter.Limits.MaxStorageTexturesPerShaderStage
            printfn "  MaxUniformBuffersPerShaderStage: %A" adapter.Limits.MaxUniformBuffersPerShaderStage
            printfn "  MinUniformBufferOffsetAlignment: %A" adapter.Limits.MinUniformBufferOffsetAlignment
            printfn "  MinStorageBufferOffsetAlignment: %A" adapter.Limits.MinStorageBufferOffsetAlignment
            printfn "  MaxVertexBuffers: %A" adapter.Limits.MaxVertexBuffers
            printfn "  MaxVertexAttributes: %A" adapter.Limits.MaxVertexAttributes
            printfn "  MaxVertexBufferArrayStride: %A" adapter.Limits.MaxVertexBufferArrayStride
            printfn "  MaxInterStageShaderVariables: %A" adapter.Limits.MaxInterStageShaderVariables
            printfn "  MaxColorAttachments: %A" adapter.Limits.MaxColorAttachments
            printfn "  MaxColorAttachmentBytesPerSample: %A" adapter.Limits.MaxColorAttachmentBytesPerSample
            printfn "  MaxComputeWorkgroupStorageSize: %A" adapter.Limits.MaxComputeWorkgroupStorageSize
            printfn "  MaxComputeInvocationsPerWorkgroup: %A" adapter.Limits.MaxComputeInvocationsPerWorkgroup
            printfn "  MaxComputeWorkgroupSizeX: %A" adapter.Limits.MaxComputeWorkgroupSizeX
            printfn "  MaxComputeWorkgroupSizeY: %A" adapter.Limits.MaxComputeWorkgroupSizeY
            printfn "  MaxComputeWorkgroupSizeZ: %A" adapter.Limits.MaxComputeWorkgroupSizeZ
            
            
            let! dev = adapter.RequestDevice()
            printfn "Device: %A" dev.Handle
            printfn "Queue: %A" dev.Queue.Handle
        }
    //
    // let del = Delegate.CreateDelegate(typeof<WebGPURaw.WGPUDelegate>, typeof<WebGPURaw.WGPUCallbacks>.GetMethod "Callback")
    // let gc = GCHandle.Alloc(del)
    // let ptr = Marshal.GetFunctionPointerForDelegate(del)
    // printfn "c %A" ptr
    // printfn "HI %A" (WebGPURaw.wgpuSupported())
    //
    // let cb =
    //     WebGPURaw.WGPUCallbacks.RegisterCallback(fun a ->
    //         printfn "HAHAHAHAHAHAHAHA: %A" a
    //     )
    // let mutable info = { WebGPURaw.PowerPreference = WebGPURaw.GPUPowerPreference.High; WebGPURaw.ForceFallbackAdapter = 0 }
    // let a = WebGPURaw.my_navigator_gpu_request_adapter_async(&info, ptr, cb)
    // printfn "d: %A" a
    
    0