module Demo.BinRasterizerTest


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

open BenchmarkDotNet.Attributes
open System.IO


type Marker = Marker

module Test = 

    let init (app: WebGPUApplication) (rasterizerType : string) (triangleCountPerBin : int) (actBlock : string) (mv : Trafo3d) (binSize : int)(maxSplits :int)=
        let shaders = BinRasterizer.BinRasterizer.compile app.Device

        let mutable rasterizer: Device -> string -> Rasterizer = 
            match rasterizerType with
            | "bin" -> BinRasterizer.BinRasterizer.run shaders
            | "default" -> fun d _ -> DefaultRasterizer.compile d
            | _ -> failwith $"Benchmark parameter \"rasterizerType\" has an invalid value {rasterizerType}"
        
        //let size = V2i(1920, 1280)
        let size = V2i(1024, 768)

        let proj =
            Frustum.perspective 90.0 0.1 100.0 (float size.X / float size.Y) |> Frustum.projTrafo

            
        //let vertices, normals, colors = ComputeRasterizerDemo.Obj.loadMesh "c:/Dev/VRVis/WebGPU/src/Demo/resources/exterior.obj"
        let vertices, normals, colors = ComputeRasterizerDemo.Obj.loadMesh "c:/Dev/VRVis/WebGPU/src/Demo/resources/sponza.obj"
        //let vertices, normals, colors = ComputeRasterizerDemo.Obj.loadMesh "c:/Dev/VRVis/WebGPU/src/Demo/resources/buddha.obj"
        //let vertices, normals, colors = ComputeRasterizerDemo.Obj.loadMesh "c:/Dev/VRVis/WebGPU/src/Demo/resources/sibenik.obj"
        //let vertices, normals, colors = ComputeRasterizerDemo.Obj.triangles size triangleCountPerBin
        
        let vertices = vertices |> Array.map (fun v -> V4f(v, 1.0f))
        let normals = normals |> Array.map (fun v -> V4f(v, 0.0f))
        
        let vertexBuffer = app.Device.CreateBuffer(BufferUsage.Vertex ||| BufferUsage.Storage, vertices).Result
        let normalsBuffer = app.Device.CreateBuffer(BufferUsage.Vertex ||| BufferUsage.Storage, normals).Result
        let colorBuffer = app.Device.CreateBuffer(BufferUsage.Vertex ||| BufferUsage.Storage, colors).Result
        
        
        let mutable texSize = V2i.Zero
        let mutable color = Unchecked.defaultof<Texture>
        let mutable depth = Unchecked.defaultof<Buffer>
        let csize = cval texSize
        let ctex = cval Unchecked.defaultof<ITexture>
        
        let rasterize = rasterizer app.Device

        if size <> texSize then
            if not (isNull (color :> obj)) then
                color.Dispose()
                depth.Dispose()
        
            color <- app.Device.CreateTexture(TextureFormat.R32ui, size)
            depth <-
                app.Device.CreateBuffer{
                    Next = null
                    Label = null
                    Usage = BufferUsage.Storage
                    Size = int64 size.X * int64 size.Y * 4L
                    MappedAtCreation = false
                }
            texSize <- size
            transact (fun () -> csize.Value <- size; ctex.Value <- color)


        let input =
            {
                Positions          = vertexBuffer
                Normals            = normalsBuffer
                Colors             = colorBuffer
                ColorTexture       = color
                DepthBuffer        = depth
                ModelViewTrafo     = mv
                ProjTrafo          = proj
                BinSize            = binSize
                MaxSplits          = maxSplits
            }

        let img = app.Device.DownloadPixImage(color).Result :?> PixImage<uint32>
        let rgbaImg = PixImage<byte>(Col.Format.RGBA, img.Size)

        rgbaImg.GetMatrix<C4b>().SetMap(img.GetChannel 0L, fun v ->
            C4b(byte v, byte (v >>> 8), byte (v >>> 16), byte (v >>> 24))
        ) |> ignore

        Directory.CreateDirectory("testResults") |> ignore
        Aardvark.Data.PixImageSharp.SaveImageSharp(rgbaImg, $"testResults/{rasterizerType}Rasterizer_ViewXXX.jpg")
    

        rasterize actBlock, input
        
        
    let run (rasterize : RasterizerInput -> Tasks.Task<unit>) (args : RasterizerInput)=
        let task = rasterize args
        task.Wait()

[<MemoryDiagnoser>]
//[<InvocationCount(10)>]
type RasterizerBenchmark() =

    static let app = WebGPUApplication.Create(true).Result

    static let mutable old = []

    [<DefaultValue>]
    val mutable rasterize : RasterizerInput -> Tasks.Task<unit>
    
    [<DefaultValue>]
    val mutable args : RasterizerInput
    
    [<DefaultValue>]
    val mutable mv :Trafo3d
    
    //[<DefaultValue; Params(3)>]
    [<DefaultValue; Params(0, 1, 2)>]
    //[<DefaultValue; Params(1)>]
    val mutable sceneView : int

    //[<DefaultValue; Params("bin", "default")>]
    [<DefaultValue; Params("bin")>]
    //[<DefaultValue; Params("default")>]
    val mutable rasterizerType : string

    [<DefaultValue; Params(32, 64, 128)>]
    val mutable binSize : int
    
    [<DefaultValue; Params(0, 1, 2)>]
    val mutable maxSplits : int

    [<DefaultValue; Params(1)>]
    //[<DefaultValue; Params(1, 10, 100, 1000)>]
    val mutable triangleCountPerBin : int

    [<DefaultValue; Params("all")>]
    val mutable actBlock : string

    [<GlobalSetup>]
    member x.Init() =
        if (x.sceneView = 0) then
            // Some bins
            x.mv <- Trafo3d.RotationX(-Constant.PiHalf) * (CameraView.lookAt (V3d(3,2,-1)) V3d.Zero V3d.OOI |> CameraView.viewTrafo)
        else if (x.sceneView = 1) then
            // All bins
            x.mv <- Trafo3d.RotationX(-Constant.PiHalf) * (CameraView.lookAt (V3d(-0.4, 0.1, -0.1)) (V3d(0, -0.5, 0.75)) V3d.OOI |> CameraView.viewTrafo)
        else if (x.sceneView = 2) then
            // Just one bin
            x.mv <- Trafo3d.RotationX(-Constant.PiHalf) * Trafo3d.Translation(0, 1.5, 1) * (CameraView.lookAt (V3d(70,0,0)) V3d.Zero V3d.OOI |> CameraView.viewTrafo)
        else if (x.sceneView = 3) then
            // For triangle test
            x.mv <- Trafo3d.RotationX(Constant.Pi) * (CameraView.lookAt (V3d(-1.0f, 0.0f, 0.0f)) (V3d(0.5, 0, 0)) V3d.OOI |> CameraView.viewTrafo)
        else
            raise (System.Exception "Unhandeled value for the sceneView benchmark parameter")

        let (rasterize, args) = Test.init app x.rasterizerType x.triangleCountPerBin x.actBlock x.mv x.binSize x.maxSplits

        old <- (x.rasterize, x.args) :: old

        x.rasterize <- rasterize
        x.args <- args


    [<Benchmark(Description = "Rasterize")>]
    member x.Rasterize() =
        Test.run x.rasterize { x.args with ModelViewTrafo = x.mv }

    [<GlobalCleanup>]
    member x.Cleanup() = 
        if x.actBlock = "all" then
            let img = app.Device.DownloadPixImage(x.args.ColorTexture).Result :?> PixImage<uint32>
            let rgbaImg = PixImage<byte>(Col.Format.RGBA, img.Size)
    
            rgbaImg.GetMatrix<C4b>().SetMap(img.GetChannel 0L, fun v ->
                C4b(byte v, byte (v >>> 8), byte (v >>> 16), byte (v >>> 24))
            ) |> ignore
    
            Directory.CreateDirectory("testResults") |> ignore
            Aardvark.Data.PixImageSharp.SaveImageSharp(rgbaImg, $"testResults/{x.rasterizerType}Rasterizer_View{x.sceneView}.jpg")

module Benchmarks = 
    open BenchmarkDotNet.Running;
    open BenchmarkDotNet.Configs
    open BenchmarkDotNet.Jobs
    open BenchmarkDotNet.Toolchains

    let runBenchmark (_argv : string[]) =
        Aardvark.Init()
        //WebGPUShaderExtensions.ShaderCaching <- true
        WebGPUConfig.shaderCaching <- true

        let cfg =
            let job = Job.ShortRun.WithToolchain(InProcess.Emit.InProcessEmitToolchain.Instance)
            ManualConfig.Create(DefaultConfig.Instance).WithOptions(ConfigOptions.DisableOptimizationsValidator).AddJob(job)
        
        //BenchmarkSwitcher.FromAssembly(typeof<RasterizerBenchmark>.Assembly).Run(_argv, cfg)
        BenchmarkRunner.Run(typeof<RasterizerBenchmark>.Assembly, cfg, _argv)

