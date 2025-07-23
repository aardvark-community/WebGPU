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

type Marker = Marker

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

    let bunny() =
        use bunnyStream =
            let names = typeof<Marker>.Assembly.GetManifestResourceNames()
            match names |> Array.tryFind (fun str -> str.EndsWith "stanford-bunny.obj") with
            | Some bunnyName ->
                typeof<Marker>.Assembly.GetManifestResourceStream(bunnyName)
            | None ->
                failwith "Could not find stanford-bunny.obj in resources"
        loadMesh bunnyStream

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
        

let computeRasterizerTask (signature : IFramebufferSignature) (mv : aval<Trafo3d>) (proj : aval<Trafo3d>) (device : Device) (compile : Device -> Rasterizer) =
    
    let vertices, normals, colors = Obj.bunny()
    
    
    let vertices = vertices |> Array.map (fun v -> V4f(v, 1.0f))
    let normals = normals |> Array.map (fun v -> V4f(v, 0.0f))
    //
    // let vertices = 
    //     [|
    //         V4f(0.0f, 0.0f, 0.0f, 1.0f); V4f(0.5f, 0.0f, 0.0f, 1.0f); V4f(0.0f, 0.5f, 0.0f, 1.0f)
    //         V4f(0.0f, 0.5f, 0.0f, 1.0f); V4f(0.5f, 0.0f, 0.0f, 1.0f); V4f(0.5f, 0.5f, 0.0f, 1.0f); 
    //     |]
    //
    // let colors =
    //     [|
    //         C4b.Red; C4b.Green; C4b.Blue;
    //         C4b.Blue; C4b.Green; C4b.White;
    //     |]
    //     
    // let normals =
    //     [|
    //         V4f.OOIO; V4f.OOIO; V4f.OOIO
    //         V4f.OOIO; V4f.OOIO; V4f.OOIO
    //     |]
        
    let vertexBuffer = device.CreateBuffer(BufferUsage.Storage, vertices).Result
    let normalsBuffer = device.CreateBuffer(BufferUsage.Storage, normals).Result
    let colorBuffer = device.CreateBuffer(BufferUsage.Storage, colors).Result
    
    
    let mutable texSize = V2i.Zero
    let mutable color = Unchecked.defaultof<Texture>
    let mutable depth = Unchecked.defaultof<Buffer>
    let csize = cval texSize
    let ctex = cval Unchecked.defaultof<ITexture>
    
    let resolve = Resolve.compileRender signature ctex
    
    let rasterize = compile device
    
    RenderTask.custom (fun (t, _, o) ->
        let size = o.viewport.Size
        let mv = mv.GetValue t
        let proj = proj.GetValue t
        
        if size <> texSize then
            if not (isNull (color :> obj)) then
                color.Dispose()
                depth.Dispose()
        
            color <- device.CreateTexture(TextureFormat.R32ui, size)
            depth <-
                device.CreateBuffer{
                    Next = null
                    Label = null
                    Usage = BufferUsage.Storage
                    Size = int64 size.X * int64 size.Y * 4L
                    MappedAtCreation = false
                }
            texSize <- size
            transact (fun () -> csize.Value <- size; ctex.Value <- color)
            
        let task = 
            rasterize {
                Positions = vertexBuffer
                Normals = normalsBuffer
                Colors = colorBuffer
                ModelViewTrafo = mv
                ProjTrafo = proj
                ColorTexture = color
                DepthBuffer = depth
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
    
    
    
    let task = computeRasterizerTask win.FramebufferSignature cam frustum app.Device rasterizer

    win.RenderTask <- task
    win.Run()
