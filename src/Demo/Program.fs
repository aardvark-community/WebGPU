open Aardvark.Rendering.WebGPU
open Demo

open Aardvark.Base
open WebGPU
open Aardvark.Application.Slim

module Shader =
    open FShade
    
    [<LocalSize(X = 8, Y = 8)>]
    let bla (img : Image2d<Formats.rgba8>) =
        compute {
            img.[V2i(0, 0)] <- V4f.IIII
        }
    
let writeRGBA() =
    Aardvark.Init()
    let app = WebGPUApplication.Create(true).Result
    
    let sh = app.Device.CompileCompute Shader.bla
    let tex = app.Device.CreateTexture(TextureUsage.StorageBinding, TextureFormat.RGBA8Unorm, V2i(1024, 1024))
    
    sh.Run(V2i(128, 128), [
        "img", tex.CreateView(TextureUsage.StorageBinding) :> obj
    ]).Wait()


[<EntryPoint>]
let main _argv =
    //writeRGBA()
    ComputeRasterizerDemo.run()
    //RenderDemo.run()
        
    0
