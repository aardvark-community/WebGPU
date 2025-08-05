namespace Aardvark.Rendering.WebGPU

open FSharp.Data.Adaptive
open WebGPU
open Aardvark.Rendering


type RenderTask(device : Device, signature : IFramebufferSignature, objects : aset<IRenderObject>) =
    inherit AdaptiveObject()
    let signature = signature :?> FramebufferSignature
    let manager = ResourceManager(device)
    let set = objects |> ASet.map (fun o -> manager.PrepareRenderObject(o, signature))
    let prepared = set.GetReader()
    let id = RenderTaskId.New()
    let all = System.Collections.Generic.HashSet<PreparedMultiRenderObject>()
    let mutable frameId = 0UL
    let mutable name : string = null
    
    member x.FramebufferSignature = signature
        
    member x.Run(token : AdaptiveToken, output : OutputDescription) =
        let fbo = output.framebuffer :?> Framebuffer
        x.EvaluateAlways token (fun token ->
            for o in prepared.GetChanges(token) do
                match o with
                | Add(_, o) ->
                    printfn "ADD"
                    if all.Add o then
                        o.Acquire()
                | Rem(_, o) ->
                    printfn "REM"
                    if all.Remove o then
                        o.Release()
           
            use update = device.CreateCommandEncoder { Label = null; Next = null }
            use cmd = device.CreateCommandEncoder { Label = null; Next = null }
            
            
            
            
            let depth =
                match fbo.DepthStencil with
                | Some depth ->
                    {
                        View = depth
                        DepthLoadOp = LoadOp.Load
                        DepthStoreOp = StoreOp.Store
                        DepthClearValue = 1.0f
                        DepthReadOnly = false
                        StencilLoadOp = LoadOp.Load
                        StencilStoreOp = StoreOp.Store
                        StencilClearValue = 0
                        StencilReadOnly = false
                    }
                | None ->
                    {
                        View = TextureView.Null
                        DepthLoadOp = LoadOp.Undefined
                        DepthStoreOp = StoreOp.Undefined
                        DepthClearValue = 0.0f
                        DepthReadOnly = true
                        StencilLoadOp = LoadOp.Undefined
                        StencilStoreOp = StoreOp.Undefined
                        StencilClearValue = 0
                        StencilReadOnly = true
                    }
                    
            use renc = 
                cmd.BeginRenderPass {
                    Label = null
                    Next = null
                    ColorAttachments =
                        fbo.ColorAttachments |> Array.map (fun tex ->
                            {
                                Next = null
                                DepthSlice = -1 // TODO
                                ResolveTarget = TextureView.Null
                                View = tex
                                LoadOp = LoadOp.Load
                                StoreOp = StoreOp.Store
                                ClearValue = { R = 1.0; G = 1.0; B = 1.0; A = 1.0 }
                            }    
                        )
                    DepthStencilAttachment = depth
                    OcclusionQuerySet = QuerySet.Null
                    TimestampWrites = PassTimestampWrites.Null
                }
            for o in all do
                renc.Render(o, update, token)
                  
            use update = update.Finish { Label = null}
            renc.End()
            use render = cmd.Finish { Label = null}
            
            let t = device.Queue.Submit [| update; render |]
            t.Wait()
            frameId <- frameId + 1UL
        )
    
    interface IRenderTask with
        member this.Name
            with get() = name
            and set v = name <- v
        member this.Dispose() = ()
        member this.FrameId = frameId
        member this.FramebufferSignature = Some (signature :> IFramebufferSignature)
        member this.Id = id
        member this.Run(var0,var1,var2) = this.Run(var0, var2)
        member this.Runtime = Some device.Runtime
        member this.Update(var0,var1) = ()
        member this.Use(var0) = var0()
    
    
type ClearTask(device : Device, signature : IFramebufferSignature, values : aval<ClearValues>) =
    inherit AdaptiveObject()
    
    let id = RenderTaskId.New()
    let mutable frameId = 0UL
    let mutable name : string = null
    
    member x.FramebufferSignature = signature
        
    member x.Run(token : AdaptiveToken, output : OutputDescription) =
        x.EvaluateAlways token (fun token ->
            let fbo = output.framebuffer :?> Framebuffer
            
            use enc = device.CreateCommandEncoder { Label = null; Next = null }
            
            let values = values.GetValue(token)
            
            let depth =
                match fbo.DepthStencil with
                | Some depth ->
                    
                    let depthOp, depthValue =
                        match values.Depth with
                        | Some v -> LoadOp.Clear, v
                        | None -> LoadOp.Load, 0.0f
                    
                    let stencilOp, stencilValue =
                        match values.Stencil with
                        | Some v -> LoadOp.Clear, v
                        | None -> LoadOp.Load, 0u
                    
                    {
                        View = depth
                        DepthLoadOp = depthOp
                        DepthStoreOp = StoreOp.Store
                        DepthClearValue = depthValue
                        DepthReadOnly = false
                        StencilLoadOp = stencilOp
                        StencilStoreOp = StoreOp.Store
                        StencilClearValue = int stencilValue
                        StencilReadOnly = false
                    }
                | None ->
                    {
                        View = TextureView.Null
                        DepthLoadOp = LoadOp.Undefined
                        DepthStoreOp = StoreOp.Undefined
                        DepthClearValue = 0.0f
                        DepthReadOnly = true
                        StencilLoadOp = LoadOp.Undefined
                        StencilStoreOp = StoreOp.Undefined
                        StencilClearValue = 0
                        StencilReadOnly = true
                    }
                    
            use renc = 
                enc.BeginRenderPass {
                    Label = null
                    Next = null
                    ColorAttachments =
                        fbo.ColorAttachments |> Array.mapi (fun i tex ->
                            let color = 
                                match Map.tryFind i signature.ColorAttachments with
                                | Some s ->
                                    match Map.tryFind s.Name values.Colors with
                                    | Some c -> Some c
                                    | None -> values.Color
                                | None ->
                                    values.Color
                                    
                            match color with
                            | Some color -> 
                                {
                                    Next = null
                                    DepthSlice = -1 // TODO
                                    ResolveTarget = TextureView.Null
                                    View = tex
                                    LoadOp = LoadOp.Clear
                                    StoreOp = StoreOp.Store
                                    ClearValue = { R = float color.Float.X; G = float color.Float.Y; B = float color.Float.Z; A = float color.Float.W }
                                }
                            | None ->
                                {
                                    Next = null
                                    DepthSlice = -1 // TODO
                                    ResolveTarget = TextureView.Null
                                    View = tex
                                    LoadOp = LoadOp.Load
                                    StoreOp = StoreOp.Discard
                                    ClearValue = { R = 1.0; G = 1.0; B = 1.0; A = 1.0 }
                                }
                        )
                    DepthStencilAttachment = depth
                    OcclusionQuerySet = QuerySet.Null
                    TimestampWrites = PassTimestampWrites.Null
                }
            
            renc.End()
            use cmd = enc.Finish { Label = null}
            let t = device.Queue.Submit [| cmd |]
            t.Wait()
            
            frameId <- frameId + 1UL
        )
        
    interface IRenderTask with
        member this.Dispose() = ()
        member this.FrameId = frameId
        member this.FramebufferSignature = Some signature
        member this.Id = id
        member this.Run(var0,var1,var2) = this.Run(var0, var2)
        member this.Runtime = Some device.Runtime
        member this.Update(var0,var1) = ()
        member this.Use(var0) = var0()
        member this.Name
            with get() = name
            and set v = name <- v
    
    
        
        


