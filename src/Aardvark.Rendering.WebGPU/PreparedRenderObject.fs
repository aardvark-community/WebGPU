namespace Aardvark.Rendering.WebGPU

open System.Runtime.CompilerServices
open FSharp.Data.Adaptive
open Aardvark.Rendering
open global.WebGPU
open Aardvark.Rendering.WebGPU

type PreparedRenderObject =
    {
        Original        : RenderObject
        Pipeline        : AdaptiveResource<RenderPipeline>
        VertexBuffers   : array<int * int64 * AdaptiveResource<Buffer>>
        IndexBuffer     : option<AdaptiveResource<Buffer> * IndexFormat * int64>
        BindGroups      : AdaptiveResource<array<BindGroup>>
        Draw            : aval<list<DrawCallInfo>>
        IsActive        : aval<bool>
        Activation      : System.IDisposable
    }
    
    interface System.IDisposable with
        member x.Dispose() = x.Release()
    
    interface IRenderObject with
        member x.Id = x.Original.Id
        member x.AttributeScope = x.Original.AttributeScope
        member x.RenderPass = x.Original.RenderPass
    
    interface IPreparedRenderObject with
        member x.Original = Some x.Original
        member x.Update(token : AdaptiveToken, _) =
            ()
    
    
    member x.Acquire() =
        x.Pipeline.Acquire()
        for _, _, vbo in x.VertexBuffers do vbo.Acquire()
        x.BindGroups.Acquire()
        x.IndexBuffer |> Option.iter (fun (ib, _, _) -> ib.Acquire())
        
    member x.Release() =
        x.Pipeline.Release()
        for _, _, vbo in x.VertexBuffers do vbo.Release()
        x.BindGroups.Release()
        x.IndexBuffer |> Option.iter (fun (ib, _, _) -> ib.Release())
        x.Activation.Dispose()
      
      
type PreparedMultiRenderObject(inner : list<PreparedRenderObject>) =
    member x.Inner = inner
    
    interface System.IDisposable with
        member x.Dispose() = inner |> List.iter (fun i -> i.Release())
    
    interface IRenderObject with
        member x.Id = inner.Head.Original.Id
        member x.AttributeScope = inner.Head.Original.AttributeScope
        member x.RenderPass = inner.Head.Original.RenderPass
    
    interface IPreparedRenderObject with
        member x.Original = None
        member x.Update(token : AdaptiveToken, _) =
            ()
    
    member x.Acquire() =
        inner |> List.iter (fun i -> i.Acquire())
        
    member x.Release() =
        inner |> List.iter (fun i -> i.Release())
        
[<AbstractClass; Sealed>]
type PreparedRenderObjectExtensions private() =
    
    [<Extension>]
    static member PrepareRenderObject(this : ResourceManager, o : RenderObject, signature : FramebufferSignature) =
        let desc = AdaptiveRenderPipelineDescription.ofRenderObject signature o
        let pipeline = this.CreateRenderPipeline desc
        
        let indexBuffer =
            match o.Indices with
            | Some view ->
                let buffer = this.CreateBuffer(BufferUsage.Index, view.Buffer)
                
                let indexFormat =
                    if view.ElementType = typeof<int> then IndexFormat.Uint32
                    elif view.ElementType = typeof<uint32> then IndexFormat.Uint32
                    elif view.ElementType = typeof<uint16> then IndexFormat.Uint16
                    else failwith "unsupported index buffer element type"
                
                Some (buffer, indexFormat, int64 view.Offset)
            | None ->
                None
            
        let bindGroups = this.CreateBindGroups(desc.Shader, o.Uniforms)
        
        let vertexBuffers = this.CreateVertexBuffers(desc.Shader, o.VertexAttributes, o.InstanceAttributes)
        
        let draw =
            match o.DrawCalls with
            | DrawCalls.Direct draw -> draw
            | _ -> failwith "indirect draw calls are not supported"
        
        {
            Original = o
            Pipeline = pipeline
            VertexBuffers = vertexBuffers
            IndexBuffer = indexBuffer
            BindGroups = bindGroups
            Draw = draw
            IsActive = o.IsActive
            Activation = o.Activate()
        }
    
    [<Extension>]
    static member PrepareRenderObject(this : ResourceManager, o : IRenderObject, signature : FramebufferSignature) =
        match o with
        | :? RenderObject as o ->
            new PreparedMultiRenderObject([this.PrepareRenderObject(o, signature)])
        | :? MultiRenderObject as o ->
            let objects = o.Children |> List.collect (fun o -> this.PrepareRenderObject(o, signature).Inner)
            new PreparedMultiRenderObject(objects)
        | :? PreparedRenderObject as o ->
            new PreparedMultiRenderObject([o])
        | :? PreparedMultiRenderObject as o ->
            o
        | _ ->
                failwith "unsupported render object type"
    
    [<Extension>]
    static member Render(this : RenderPassEncoder, o : PreparedRenderObject, cmd : CommandEncoder, token : AdaptiveToken) =
        if o.IsActive.GetValue token then
            this.SetPipeline (o.Pipeline.GetHandle(cmd, token))
            
            let groups = o.BindGroups.GetHandle(cmd, token)
            for i in 0 .. groups.Length - 1 do
                this.SetBindGroup(i, groups.[i], [||])
            
            for i in 0 .. o.VertexBuffers.Length - 1 do
                let slot, offset, vbo = o.VertexBuffers.[i]
                let buffer = vbo.GetHandle(cmd, token)
                this.SetVertexBuffer(slot, buffer, offset, buffer.Size)

            match o.IndexBuffer with
            | Some (index, format, offset) ->
                let index = index.GetHandle(cmd, token)
                this.SetIndexBuffer(index, format, offset, index.Size - offset)
                
                
                for draw in o.Draw.GetValue token do
                    this.DrawIndexed(draw.FaceVertexCount, draw.InstanceCount, draw.FirstIndex, draw.BaseVertex, draw.FirstInstance)
                
            | None ->
                //this.SetIndexBuffer(Buffer.Null, IndexFormat.Undefined, 0L, 0L)
                
                for draw in o.Draw.GetValue token do
                    this.Draw(draw.FaceVertexCount, draw.InstanceCount, draw.FirstIndex, draw.FirstInstance)
   
    [<Extension>]
    static member Render(this : RenderPassEncoder, o : PreparedMultiRenderObject, cmd : CommandEncoder, token : AdaptiveToken) =
        for inner in o.Inner do
            this.Render(inner, cmd, token)