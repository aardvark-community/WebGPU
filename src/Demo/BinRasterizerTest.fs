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

module Obj = 
    open Aardvark.Data.Wavefront
    
    let loadMesh (file : System.IO.Stream) : V3f[] * V3f[] * C4b[] =
        
        let tmp = System.IO.Path.GetTempFileName() + ".obj"
        do
            use s = System.IO.File.OpenWrite tmp
            file.CopyTo(s)
            
        let mesh = Aardvark.Data.Wavefront.ObjParser.Load tmp
        //System.IO.File.Delete tmp
        
        let positions = 
            match mesh.Vertices with
            | :? System.Collections.Generic.IList<V3f> as v -> v.ToArray(v.Count)
            | :? System.Collections.Generic.IList<V3d> as v -> v.ToArray(v.Count) |> Array.map V3f
            | :? System.Collections.Generic.IList<V4f> as v -> v.ToArray(v.Count) |> Array.map Vec.xyz
            | :? System.Collections.Generic.IList<V4d> as v -> v.ToArray(v.Count) |> Array.map V3f
            | _ -> failwith ""
            
        let bounds = Box3f positions |> Box3d
            
        let trafo =
            Trafo3d.Translation(-bounds.Center) *
            Trafo3d.Scale(2.0 / bounds.Size.NormMax)
        match mesh.Normals with
        | null ->
            [||], [||], [||]
            
        | normals ->
            let positions = positions |> Array.map (fun p -> trafo.Forward.TransformPos (V3d p) |> V3f)
            let normals = normals.ToArray(normals.Count) |> Array.map (fun n -> trafo.Backward.TransposedTransformDir (V3d n) |> Vec.normalize |> V3f)
            
            let colors =
                match mesh.VertexColors with
                | null ->  Array.create positions.Length C4b.White
                | cs -> cs.ToArray(cs.Count) |> Array.map (fun c -> c.ToC3b().ToC4b())
            
            
            let ps = ResizeArray()
            let ns = ResizeArray()
            let cs = ResizeArray()
            
            for set in mesh.FaceSets do
                
                let iPos = set.VertexIndices
                let iNormals =
                    if isNull set.NormalIndices then iPos
                    else set.NormalIndices
                let iColors = set.VertexIndices
                
                for ti in 0 .. set.ElementCount - 1 do
                    let fi = set.FirstIndices.[ti]
                    let cnt = set.FirstIndices.[ti+1] - fi
                
                    if cnt = 3 then
                        cs.Add colors.[iColors.[fi + 0]]
                        cs.Add colors.[iColors.[fi + 1]]
                        cs.Add colors.[iColors.[fi + 2]]
                        
                        ps.Add positions.[iPos.[fi + 0]]
                        ps.Add positions.[iPos.[fi + 1]]
                        ps.Add positions.[iPos.[fi + 2]]
                        
                        ns.Add normals.[iNormals.[fi + 0]]
                        ns.Add normals.[iNormals.[fi + 1]]
                        ns.Add normals.[iNormals.[fi + 2]]
            
            ps.ToArray(), ns.ToArray(), cs.ToArray()
    
    let readObjFromResources (fileName : string) =
        let names = typeof<Marker>.Assembly.GetManifestResourceNames()
        match names |> Array.tryFind (fun str -> str.EndsWith fileName) with
        | Some objName ->
            typeof<Marker>.Assembly.GetManifestResourceStream(objName)
        | None ->
            failwith $"Could not find {fileName} in resources"
    let bunny() =
        use objStream = readObjFromResources("stanford-bunny.obj")
        loadMesh objStream


    let beetle() =
        use s = System.IO.File.OpenRead "C:/Users/Simon/Desktop/stanford-bunny.obj"
        loadMesh s

    let triangles(size : V2i) (xAmount : int32) =
        let cameraXDistance = -1 |> float32
        let fov = 1.57079632679f
        let alpha = fov / 2.0f;

        let aspectRatio = (float32 size.X / float32 size.Y)

        let binSizePx = 32
        let binCountX = size.X / binSizePx
        let binCountY = size.Y / binSizePx

        let yScale = float32 binSizePx / float32 size.X
        let zScale = float32 binSizePx / float32 size.Y

        let yHalfViewPortSize = cameraXDistance * tan(alpha)
        let zHalfViewPortSize = cameraXDistance * tan(alpha) / aspectRatio

        let binSizeX = 2.0f * yHalfViewPortSize / float32 binCountX
        let binSizeY = 2.0f * zHalfViewPortSize / float32 binCountY

        let ps = ResizeArray()
        let ns = ResizeArray()
        let cs = ResizeArray()

        let margin = 0.1f

        for i in 0 .. binCountX * binCountY - 1 do
            for j in 0 .. xAmount - 1 do
                let xBin = i % binCountX
                let yBin = i / binCountX
                
                let yTrans = float32 xBin * binSizeX - yHalfViewPortSize + binSizeX / 2.0f
                let zTrans = float32 yBin * binSizeY - zHalfViewPortSize + binSizeY / 2.0f

                ps.Add ((Trafo3f.Scale(1.0f, yScale, zScale) * Trafo3f.Translation(0.0f, yTrans, zTrans)).TransformPos(V3f(0.0f, -yHalfViewPortSize + yHalfViewPortSize * margin, zHalfViewPortSize - zHalfViewPortSize * margin)))
                ps.Add ((Trafo3f.Scale(1.0f, yScale, zScale) * Trafo3f.Translation(0.0f, yTrans, zTrans)).TransformPos(V3f(0.0f, 0.0f, -zHalfViewPortSize + zHalfViewPortSize * margin)))
                ps.Add ((Trafo3f.Scale(1.0f, yScale, zScale) * Trafo3f.Translation(0.0f, yTrans, zTrans)).TransformPos(V3f(0.0f, yHalfViewPortSize - yHalfViewPortSize * margin, zHalfViewPortSize - zHalfViewPortSize * margin)))
                

                ns.Add (V3f(1, 0, 0))
                ns.Add (V3f(1, 0, 0))
                ns.Add (V3f(1, 0, 0))

                cs.Add (V3f(1, 1, 1))
                cs.Add (V3f(1, 1, 1))
                cs.Add (V3f(1, 1, 1))
        
        printf $"Triangle Count: {ps.Count}\n"
        ps.ToArray(), ns.ToArray(), cs.ToArray()


    //let buddha() =
    //    use objStream = readObjFromResources("buddha.obj")
    //    loadMesh objStream

    //let powerplant() =
    //    use objStream = readObjFromResources("powerplant.obj")
    //    loadMesh objStream

    let amazon_lumberyard_bistro() =
        //use objStream = readObjFromResources("amazon_lumberyard_bistro.obj")
        //use objStream = System.IO.File.OpenRead "c:/Dev/VRVis/WebGPU/src/Demo/resources/amazon_lumberyard_bistro.obj"
        
        //use objStream = System.IO.File.OpenRead "c:/Dev/VRVis/WebGPU/src/Demo/resources/exterior.obj"
        //loadMesh objStream
        let tmp = "c:/Dev/VRVis/WebGPU/src/Demo/resources/exterior.obj"
        let mesh = Aardvark.Data.Wavefront.ObjParser.Load tmp
        //System.IO.File.Delete tmp
        
        let positions = 
            match mesh.Vertices with
            | :? System.Collections.Generic.IList<V3f> as v -> v.ToArray(v.Count)
            | :? System.Collections.Generic.IList<V3d> as v -> v.ToArray(v.Count) |> Array.map V3f
            | :? System.Collections.Generic.IList<V4f> as v -> v.ToArray(v.Count) |> Array.map Vec.xyz
            | :? System.Collections.Generic.IList<V4d> as v -> v.ToArray(v.Count) |> Array.map V3f
            | _ -> failwith ""
            
        let bounds = Box3f positions |> Box3d
            
        let trafo =
            Trafo3d.Translation(-bounds.Center) *
            Trafo3d.Scale(2.0 / bounds.Size.NormMax)
        match mesh.Normals with
        | null ->
            [||], [||], [||]
            
        | normals ->
            let positions = positions |> Array.map (fun p -> trafo.Forward.TransformPos (V3d p) |> V3f)
            let normals = normals.ToArray(normals.Count) |> Array.map (fun n -> trafo.Backward.TransposedTransformDir (V3d n) |> Vec.normalize |> V3f)
            
            let colors =
                match mesh.VertexColors with
                | null ->  Array.create positions.Length C4b.White
                | cs -> cs.ToArray(cs.Count) |> Array.map (fun c -> c.ToC3b().ToC4b())
            
            
            let ps = ResizeArray()
            let ns = ResizeArray()
            let cs = ResizeArray()
            
            for set in mesh.FaceSets do
                
                let iPos = set.VertexIndices
                let iNormals =
                    if isNull set.NormalIndices then iPos
                    else set.NormalIndices
                let iColors = set.VertexIndices
                
                for ti in 0 .. set.ElementCount - 1 do
                    let fi = set.FirstIndices.[ti]
                    let cnt = set.FirstIndices.[ti+1] - fi
                
                    if cnt = 3 then
                        cs.Add colors.[iColors.[fi + 0]]
                        cs.Add colors.[iColors.[fi + 1]]
                        cs.Add colors.[iColors.[fi + 2]]
                        
                        ps.Add positions.[iPos.[fi + 0]]
                        ps.Add positions.[iPos.[fi + 1]]
                        ps.Add positions.[iPos.[fi + 2]]
                        
                        ns.Add normals.[iNormals.[fi + 0]]
                        ns.Add normals.[iNormals.[fi + 1]]
                        ns.Add normals.[iNormals.[fi + 2]]
            
            ps.ToArray(), ns.ToArray(), cs.ToArray()



    
    let ofIndexedGeometry (ig : IndexedGeometry) =
        let pos =
            match ig.IndexedAttributes.[DefaultSemantic.Positions] with
            | :? array<V3f> as v -> v
            | :? array<V3d> as v -> v |> Array.map V3f
            | :? array<V4f> as v -> v |> Array.map Vec.xyz
            | :? array<V4d> as v -> v |> Array.map V3f
            | p -> failwithf "bad positions: %A" p
            
        let ns =
            match ig.IndexedAttributes.[DefaultSemantic.Normals] with
            | :? array<V3f> as v -> v
            | :? array<V3d> as v -> v |> Array.map V3f
            | :? array<V4f> as v -> v |> Array.map Vec.xyz
            | :? array<V4d> as v -> v |> Array.map V3f
            | p -> failwithf "bad positions: %A" p
            
        if isNull ig.IndexArray then
            pos, ns, Array.create pos.Length C4b.White
        else
            let index = ig.IndexArray :?> int[]

            let pos = index |> Array.map (fun i -> V3f pos.[i])
            let ns = index |> Array.map (fun i -> V3f ns.[i])
            let colors = index |> Array.map (fun _ -> C4b.White)
            pos, ns, colors
            
    let box() =
        let box = Aardvark.SceneGraph.IndexedGeometryPrimitives.Box.solidBox (Box3d(-V3d.III, V3d.III)) C4b.White
        ofIndexedGeometry box
        
    let testy() =
        let pos =
            [|
                V3f.Zero; V3f.IOO; V3f.OIO
                V3f.OIO; V3f.IOO; V3f.IIO
            |]
        let ns = Array.create pos.Length V3f.OOI
        let cs = Array.create pos.Length C4b.White
        pos, ns, cs
        

module Test = 

    let init (app: WebGPUApplication) (rasterizerType : string) (triangleCountPerBin : int) (actBlock : string)=

        let mutable rasterizer: Device -> Rasterizer = 
            match rasterizerType with
            | "bin" -> BinRasterizer.compile actBlock
            | "default" -> DefaultRasterizer.compile
            | _ -> failwith $"Benchmark parameter \"rasterizerType\" has an invalid value {rasterizerType}"

        let mv =
            CameraView.lookAt (V3d(4,3,2)) V3d.Zero V3d.OOI
            |> CameraView.viewTrafo
        

        //let size = V2i(1920, 1280)
        let size = V2i(1024, 768)
        //let size = V2i(640, 480)
        let proj =
            Frustum.perspective 90.0 0.1 100.0 (float size.X / float size.Y) |> Frustum.projTrafo

            
        //let vertices, normals, colors = Obj.beetle()
        //let vertices, normals, colors = Obj.buddha()
        //let vertices, normals, colors = Obj.powerplant()
        let vertices, normals, colors = Obj.amazon_lumberyard_bistro()
        //let vertices, normals, colors = Obj.triangles size triangleCountPerBin
        
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
        rasterize, {
            Positions          = vertexBuffer
            Normals            = normalsBuffer
            Colors             = colorBuffer
            ColorTexture       = color
            DepthBuffer        = depth
            ModelViewTrafo     = mv
            ProjTrafo          = proj
        }
        
        
    let run (rasterize : RasterizerInput -> Tasks.Task<unit>) (args : RasterizerInput)=
        let task = rasterize args
        task.Wait()

[<MemoryDiagnoser>]
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
    //[<DefaultValue; Params(0, 1, 2)>]
    [<DefaultValue; Params(1)>]
    val mutable sceneView : int

    //[<DefaultValue; Params("bin", "default")>]
    [<DefaultValue; Params("bin")>]
    //[<DefaultValue; Params("default")>]
    val mutable rasterizerType : string

    [<DefaultValue; Params(1)>]
    //[<DefaultValue; Params(1, 10, 100, 1000)>]
    val mutable triangleCountPerBin : int

    [<DefaultValue; Params("all", "binning", "scan", "compact", "raster")>]
    val mutable actBlock : string

    [<GlobalSetup>]
    member x.Init() =
        let (rasterize, args) = Test.init app x.rasterizerType x.triangleCountPerBin x.actBlock

        old <- (x.rasterize, x.args) :: old

        x.rasterize <- rasterize
        x.args <- args

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

