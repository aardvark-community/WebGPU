namespace Aardvark.Rendering.WebGPU

open WebGPU
open Aardvark.Base
open Aardvark.Rendering
open Aardvark.Rendering.WebGPU
type Runtime(device : Device) as this =
    do device.Runtime <- this
    
    member x.Device = device
    
    interface IRuntime with
        member this.DebugLabelsEnabled = false
        member this.Blit(src,srcRegion,dst,dstRegion) = failwith "todo"
        member this.Clear(fbo: IFramebuffer,values: ClearValues): unit = failwith "todo"
        member this.Clear(texture: IBackendTexture,values: ClearValues): unit = failwith "todo"
        member this.CompileClear(signature,values) = failwith "todo"
        member this.CompileCompute(commands) = failwith "todo"
        member this.CompileRender(signature,objects) = failwith "todo"
        member this.CompileTrace(pipeline,commands) = failwith "todo"
        member this.ContextLock = failwith "todo"
        member this.Copy(src,dst) = failwith "todo"
        member this.Copy(src,srcBaseSlice,srcBaseLevel,dst,dstBaseSlice,dstBaseLevel,slices,levels) = failwith "todo"
        member this.Copy(src,srcOffset,dst,dstOffset,sizeInBytes) = failwith "todo"
        member this.CreateBuffer(sizeInBytes,usage,storage) = failwith "todo"
        member this.CreateComputeShader(shader) = failwith "todo"
        member this.CreateFramebuffer(signature,attachments) = failwith "todo"
        member this.CreateFramebufferSignature(colorAttachments,depthStencilAttachment,samples,layers,perLayerUniforms) = failwith "todo"
        member this.CreateInputBinding(shader,inputs) = failwith "todo"
        member this.CreateOcclusionQuery(precise) = failwith "todo"
        member this.CreatePipelineQuery(statistics) = failwith "todo"
        member this.CreateRenderbuffer(size,format,samples) = failwith "todo"
        member this.CreateSparseTexture(size,levels,slices,dimension,format,brickSize,maxMemory) = failwith "todo"
        member this.CreateStreamingTexture(mipMaps) = failwith "todo"
        member this.CreateTexture(size,dimension,format,levels,samples) = failwith "todo"
        member this.CreateTextureArray(size,dimension,format,levels,samples,count) = failwith "todo"
        member this.CreateTextureView(texture,levels,slices,isArray) = failwith "todo"
        member this.CreateAccelerationStructure(geometry,usage,allowUpdate) = failwith "todo"
        member this.CreateLodRenderer(config,data) = failwith "todo"
        member this.CreateGeometryPool(var0) = failwith "todo"
        member this.CreateTimeQuery() =
            { new ITimeQuery with
                member _.Dispose() = ()
                member _.HasResult() = false
                member _.TryGetResult(a,c) = None
                member this.Begin() = ()
                member this.End() = ()
                member this.Reset() = ()
                member this.GetResult(_, _) = MicroTime.Zero
            }
        member this.DebugConfig = failwith "todo"
        member this.DeviceCount = 1
        member this.Download(texture,target,format,offset,size) = failwith "todo"
        member this.Download(src,srcOffset,dst,sizeInBytes) = failwith "todo"
        member this.DownloadAsync(src,srcOffset,dst,sizeInBytes) = failwith "todo"
        member this.DownloadDepth(texture,target,level,slice,offset) = failwith "todo"
        member this.DownloadStencil(texture,target,level,slice,offset) = failwith "todo"
        member this.GenerateMipMaps(var0) = failwith "todo"
        member this.MaxLocalSize = failwith "todo"
        member this.MaxRayRecursionDepth = failwith "todo"
        member this.OnDispose = failwith "todo"
        member this.PrepareBuffer(data,usage,storage) = failwith "todo"
        member this.PrepareEffect(signature,effect,topology) = failwith "todo"
        member this.PrepareRenderObject(var0,var1) = failwith "todo"
        member this.PrepareTexture(texture) = failwith "todo"
        member this.ReadPixels(src,attachment,offset,size) = failwith "todo"
        member this.ShaderCachePath = failwith "todo"
        member this.ShaderCachePath with set value = failwith "todo"
        member this.ShaderDepthRange = Range1f(-1.0f, 1.0f)
        member this.SupportedPipelineStatistics = Set.empty
        member this.SupportsLayeredShaderInputs = false
        member this.SupportsRaytracing = false
        member this.TryUpdateAccelerationStructure(handle,geometry) = failwith "todo"
        member this.Upload(texture,source,format,offset,size) = failwith "todo"
        member this.Upload(src,dst,dstOffset,sizeInBytes) = failwith "todo"


