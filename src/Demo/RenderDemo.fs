module Demo.RenderDemo


open Aardvark.Application
open Aardvark.Application.Slim
open Aardvark.Base
open Aardvark.Rendering
open System.Threading
open Aardvark.Rendering.WebGPU
open global.WebGPU
open FSharp.Data.Adaptive
open Microsoft.FSharp.NativeInterop


module Shader =
    open FShade
    
    let sammy2 =
        sampler2d {
            texture uniform?DiffuseColorTexture
            addressU WrapMode.Wrap
            addressV WrapMode.Wrap
            filter Filter.Anisotropic
            maxAnisotropy 16
        }
    
    let sammm (v : Effects.Vertex) =
        fragment {
            let dx = ddx v.tc
            let dy = ddy v.tc
            return sammy2.SampleGrad(v.tc,dx, dy)
        }
    
    type Vertex =
        {
            [<Position>] pos : V4f
            [<TexCoord>] tc : V2f
            [<Normal>] n : V3f
            [<Color>] col : V4f
        }
        

    let tex =
        sampler2dShadow {
            texture uniform?DiffuseColorTexture
            addressU WrapMode.Wrap
            addressV WrapMode.Wrap
            filter Filter.MinMagMipLinear
        }
        
        
    [<ReflectedDefinition>]
    let sammy (s : Sampler2dShadow) (tc : V2f) =
        s.Sample(tc, 0.5f)
 

    let env =
        samplerCube {
            texture uniform?EnvMap
            addressU WrapMode.Wrap
            addressV WrapMode.Wrap
            addressW WrapMode.Wrap
            filter Filter.MinMagMipLinear
        }

    type UniformScope with
        member x.Hansi : V4f[] = uniform?StorageBuffer?Hansi
        member x.HansiIndex : int = uniform?HansiIndex
    
    let envMap (v : Effects.Vertex) =
        fragment {
            let vp = uniform.ProjTrafoInv * V4f(v.pos.X, v.pos.Y, -1.0f, 1.0f)
            let vp = vp.XYZ / vp.W
            let dir = (uniform.ViewTrafoInv * V4f(vp, 0.0f)).XYZ |> Vec.normalize
            return env.Sample(dir) * uniform.Hansi.[uniform.HansiIndex]
        }

    let reverseTrafo (v : Effects.Vertex) =
        vertex {
            let wp = uniform.ViewProjTrafoInv * v.pos
            return { v with wp = wp / wp.W }
        }
           
    let fragment (v : Vertex) =
        fragment {
            return V4f(1.0, 0.0, 0.0, 1.0) //Vec.normalize v.n * 0.5 + V3d.Half, 1.0)
        }

    
    
    [<LocalSize(X = 64)>]
    let computer (x : int) (a : int[]) (b : int[]) (c : int[]) =
        compute {
            let id = getGlobalId().X
            let s = getWorkGroupSize()
            c.[id] <- a.[id] + b.[id] + x - s.X
        }

    let lineRx = System.Text.RegularExpressions.Regex @"\r\n|\n|\r"
    
    let withDevice (action : Device -> 'r) =
        let mutable running = true
        use instance =
            WebGPU.CreateInstance()
            
        let start =
            ThreadStart(fun () ->
                while running do
                    instance.ProcessEvents()
            )
        let thread = Thread(start, IsBackground = true)
        thread.Start()

        use adapter = 
            instance.CreateAdapter().Result
     
        use device = 
            adapter.RequestDeviceAsync({
                Next = null
                DebugOutput = true
                Label = "Devon"
                RequiredFeatures = adapter.Features.Features
                RequiredLimits = adapter.Limits
                DefaultQueue = { Label = "Quentin" }
            }).Result
        
        
        try
            action device
        finally 
            running <- false
            thread.Join()

module Scene =
    open Aardvark.SceneGraph
    
    type Marker = Marker
    
    let getImage =
        let names = typeof<Marker>.Assembly.GetManifestResourceNames()
        let load (name : string) =
            let name = names |> Array.find (fun str -> str.EndsWith name)
            use s = typeof<Marker>.Assembly.GetManifestResourceStream(name)
            PixImage.Load(s)

        load
    
    let skybox (name : string) =
        AVal.custom (fun _ ->
            let env =
                let trafo t (img : PixImage) = img.TransformedPixImage t

                PixCube [|
                    PixImageMipMap(
                        getImage (name.Replace("$", "rt"))
                        |> trafo ImageTrafo.Rot90
                    )
                    PixImageMipMap(
                        getImage (name.Replace("$", "lf"))
                        |> trafo ImageTrafo.Rot270
                    )
                
                    PixImageMipMap(
                        getImage (name.Replace("$", "bk"))
                    )
                    PixImageMipMap(
                        getImage (name.Replace("$", "ft"))
                        |> trafo ImageTrafo.Rot180
                    )
                
                    PixImageMipMap(
                        getImage (name.Replace("$", "up"))
                        |> trafo ImageTrafo.Rot90
                    )
                    PixImageMipMap(
                        getImage (name.Replace("$", "dn"))
                        |> trafo ImageTrafo.Rot90
                    )
                |]

            PixTextureCube(env, TextureParams.empty) :> ITexture
        )
    
    
    let scene (hansiIndex : aval<int>) (view : aval<Trafo3d>) (proj : aval<Trafo3d>)  =
        
        
        
        Sg.ofList [
            Sg.farPlaneQuad
            |> Sg.texture "EnvMap" (skybox "miramar_$.png")
            |> Sg.shader {
                do! Shader.reverseTrafo
                do! Shader.envMap
            }
            |> Sg.uniform "HansiIndex" hansiIndex
            |> Sg.uniform' "Hansi" [| V4f.IIII; V4f.IOOI; V4f.OIOI; V4f.OOII |]
        
        
            Sg.box' C4b.Green Box3d.Unit
            |> Sg.shader {
                do! DefaultSurfaces.trafo
                do! Shader.sammm
            }
            |> Sg.diffuseFileTexture "/Users/schorsch/Desktop/stuff/GettyImages-121786088-58a4cc5a5f9b58a3c955c5fb.jpg" true
        ]
        |> Sg.viewTrafo view
        |> Sg.projTrafo proj
    
    let renderTask (hansiIndex : aval<int>)(view : aval<Trafo3d>) (proj : aval<Trafo3d>) (device : Device) =
        //let rt = Runtime(device) 
        let signature = device.CreateFramebufferSignature(1, Map.ofList [0, { Name = DefaultSemantic.Colors; Format = TextureFormat.Bgra8 }], Some TextureFormat.Depth24Stencil8)
        let objs = Aardvark.SceneGraph.Semantics.RenderObjectSemantics.Semantic.renderObjects Ag.Scope.Root (scene hansiIndex view proj)
        //let objs = objs |> ASet.force |> ASet.ofSeq)
        let task = new RenderTask(device, signature, objs)
        task

let run() =
    Aardvark.Init()
    let app = WebGPUApplication.Create(true).Result
    let win = app.CreateGameWindow(vsync = true)
    
    let index = cval 0
    win.Keyboard.DownWithRepeats.Values.Add (fun k ->
        match k with
        | Keys.Space ->
            transact (fun () -> index.Value <- (index.Value + 1) % 4)
        | _ ->
            ()
    )
    
    
    let cam =
        CameraView.lookAt (V3d(4,3,2)) V3d.Zero V3d.OOI
        |> DefaultCameraController.control win.Mouse win.Keyboard win.Time
        |> AVal.map CameraView.viewTrafo
    
    let frustum =
        win.Sizes |> AVal.map (fun s ->
            Frustum.perspective 90.0 0.1 100.0 (float s.X / float s.Y) |> Frustum.projTrafo
        )
        
    let task =
        RenderTask.ofList [
            new ClearTask(app.Device, win.FramebufferSignature, AVal.constant (clear { color C4f.Black; depth 1.0; stencil 0 }))
            Scene.renderTask index cam frustum app.Device
        ]
    win.RenderTask <- task
    win.Run()
