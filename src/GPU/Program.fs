
open System.Buffers
open Microsoft.FSharp.NativeInterop
open System
open WebGPU


type IPinnable<'a when 'a : unmanaged> =
    abstract member Pin<'r> : action : (nativeptr<'a> -> 'r) -> 'r

let pinArray (arr : #IPinnable<'a>[]) (action : nativeptr<'a>[] -> 'r) =
    let rec pin (acc : nativeptr<'a>[]) (i : int) =
        if i >= arr.Length then
            action acc
        else
            (arr.[i] :> IPinnable<'a>).Pin<'r>(fun ptr ->
                acc.[i] <- ptr
                pin acc (i + 1)
            )
    let ptrs = Array.zeroCreate arr.Length
    pin ptrs 0

type Hans() =
    interface IPinnable<int> with
        member this.Pin<'r> action =
            let ptr = NativePtr.ofNativeInt<int> 0n
            action ptr

let test =
    pinArray [|Hans(); Hans()|] (fun ptr ->
        ptr
    )


[<EntryPoint>]
let main args =
    
    let run =
        task {
            let mutable options =
                {
                    WebGPURaw.RequestAdapterOptions.CompatibleSurface = 0n
                    WebGPURaw.RequestAdapterOptions.PowerPreference = PowerPreference.High
                    WebGPURaw.RequestAdapterOptions.ForceFallbackAdapter = 0
                    WebGPURaw.RequestAdapterOptions.NextInChain = 0n 
                }
            let cbid = WebGPURaw.WGPUCallbacks.RegisterCallback(fun a b c -> printfn "%A %A %A" a b c)
            let ptr = WebGPURaw.gpuInstanceRequestAdapter(0n, &options, WebGPURaw.delegatePtr2, cbid)
            printfn "%A" ptr
            
            
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
            dev.Lost.Add(fun (reason, message) ->
                printfn "device lost %A: %s" reason message
            )
            
            dev.UncapturedError.Add(fun (code, message) ->
                printfn "ERROR: %A: %s" code message  
            )
            
            printfn "Device: %A" dev.Handle
            printfn "Queue: %A" dev.Queue.Handle
            let buffer = 
                dev.CreateBuffer {
                    Size = 16UL
                    Usage = BufferUsage.Vertex ||| BufferUsage.CopyDst
                    MappedAtCreation = false 
                }
            printfn "Buffer: %A" buffer.Handle
            dev.Queue.WriteBuffer([|1;2;3;4|], buffer, 0)
            printfn "write submitted"
            do! dev.Queue.Wait()
            printfn "write done"
            dev.Dispose()
            
            // dev.Queue.WriteBuffer(ReadOnlyMemory([|1;2;3|], 1, 2), Buffer(0), 10)
            
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