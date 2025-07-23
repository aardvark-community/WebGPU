module Demo.ComputeRasterizerDemo


open Aardvark.Application
open Aardvark.Application.Slim
open Aardvark.Base
open Aardvark.Rendering
open System.Threading
open Aardvark.Rendering.WebGPU
open global.WebGPU
open FSharp.Data.Adaptive
open Microsoft.FSharp.NativeInterop
open Demo

module Resolve =
    open FShade
    open Aardvark.SceneGraph
    
    type UniformScope with
        member x.Input : UIntImage2d<Formats.r32ui> = uniform?Input
    
    let blit (v : Effects.Vertex) =
        fragment {
            let value = uniform.Input.[V2i(v.tc * V2f uniform.ViewportSize)].X
            return unpackUnorm4x8 value
        }

    let compileRender (signature : IFramebufferSignature) (inputTex : aval<ITexture>) =
        let size = inputTex |> AVal.map (fun t -> (t :?> IBackendTexture).Size.XY)
        let sg = 
            Sg.fullScreenQuad
            |> Sg.shader {
                do! blit
            }
            |> Sg.uniform "Input" inputTex
            |> Sg.uniform "ViewportSize" size
            
        let objs = Aardvark.SceneGraph.Semantics.RenderObjectSemantics.Semantic.renderObjects Ag.Scope.Root sg
        //let objs = objs |> ASet.force |> ASet.ofSeq)
        let rt = signature.Runtime :?> Runtime
        let task = new RenderTask(rt.Device, signature :?> FramebufferSignature, objs)
        task

let computeRasterizerTask (signature : IFramebufferSignature) (mvp : aval<Trafo3d>) (device : Device) (compile : Device -> Rasterizer) =
    let vertices =
        [|
            V4f(0.0f, 0.0f, 0.0f, 1.0f); V4f(0.5f, 0.0f, 0.0f, 1.0f); V4f(0.0f, 0.5f, 0.0f, 1.0f)
            V4f(0.0f, 0.5f, 0.0f, 1.0f); V4f(0.5f, 0.0f, 0.0f, 1.0f); V4f(0.5f, 0.5f, 0.0f, 1.0f); 
        |]
    
    let colors =
        [|
            C4b.Red; C4b.Green; C4b.Blue;
            C4b.Blue; C4b.Green; C4b.White;
        |]
        
    let vertexBuffer = device.CreateBuffer(BufferUsage.Storage, vertices).Result
    let colorBuffer = device.CreateBuffer(BufferUsage.Storage, colors).Result
    
    
    let mutable texSize = V2i.Zero
    let mutable color = Unchecked.defaultof<Texture>
    let mutable depth = Unchecked.defaultof<Texture>
    let csize = cval texSize
    let ctex = cval Unchecked.defaultof<ITexture>
    
    let resolve = Resolve.compileRender signature ctex
    
    let rasterize = compile device
    
    RenderTask.custom (fun (t, _, o) ->
        let size = o.viewport.Size
        let mvp = mvp.GetValue t
        
        if size <> texSize then
            if not (isNull (color :> obj)) then
                color.Dispose()
                depth.Dispose()
        
            color <- device.CreateTexture(TextureFormat.R32ui, size)
            depth <- device.CreateTexture(TextureFormat.R32f, size)
            texSize <- size
            transact (fun () -> csize.Value <- size; ctex.Value <- color)
            
        let task = 
            rasterize {
                Positions = vertexBuffer
                Colors = colorBuffer
                ModelViewProj = mvp
                ColorTexture = color
                DepthTexture = depth
            }
        task.Wait()
        resolve.Run(o)
    )
    

let run() =
    Aardvark.Init()
     
     
    let rasterizer = BasicRasterizer.compile
     
    let app = WebGPUApplication.Create(true).Result
    let win = app.CreateGameWindow(vsync = true)
    
    let cam =
        CameraView.lookAt (V3d(4,3,2)) V3d.Zero V3d.OOI
        |> DefaultCameraController.control win.Mouse win.Keyboard win.Time
        |> AVal.map CameraView.viewTrafo
    
    let frustum =
        win.Sizes |> AVal.map (fun s ->
            Frustum.perspective 90.0 0.1 100.0 (float s.X / float s.Y) |> Frustum.projTrafo
        )
    
    let mvp = AVal.map2 (*) cam frustum
  
    
    
    
    let task = computeRasterizerTask win.FramebufferSignature mvp app.Device rasterizer

    win.RenderTask <- task
    win.Run()
