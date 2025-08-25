module Demo.ComputeRasterizerDemo

open System.Runtime.CompilerServices
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

    let beetle() =
        use s = System.IO.File.OpenRead "/Users/schorsch/Desktop/stanford-bunny.obj"
        loadMesh s
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
    
    let vertices, normals, colors = Obj.beetle()
    
    
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
        
    let vertexBuffer = device.CreateBuffer(BufferUsage.Vertex ||| BufferUsage.Storage, vertices).Result
    let normalsBuffer = device.CreateBuffer(BufferUsage.Vertex ||| BufferUsage.Storage, normals).Result
    let colorBuffer = device.CreateBuffer(BufferUsage.Vertex ||| BufferUsage.Storage, colors).Result
    
    
    let mutable texSize = V2i.Zero
    let mutable color = Unchecked.defaultof<Texture>
    let mutable depth = Unchecked.defaultof<Buffer>
    let csize = cval texSize
    let ctex = cval Unchecked.defaultof<ITexture>
    
    let resolve = Resolve.compileRender signature ctex
    
    let rasterize = compile device
    
    let sw = System.Diagnostics.Stopwatch.StartNew()
    let mutable sum = 0.0
    let mutable cnt = 0
    
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
            
        
        let t0 = sw.Elapsed.TotalSeconds        
        let task = 
            rasterize {
                Positions = vertexBuffer
                Normals = normalsBuffer
                Colors = colorBuffer
                ModelViewTrafo = Trafo3d.RotationX(Constant.PiHalf) * mv
                ProjTrafo = proj
                ColorTexture = color
                DepthBuffer = depth
            }
        task.Wait()
        
        
        let dt = sw.Elapsed.TotalSeconds - t0
        sum <- sum + dt
        cnt <- cnt + 1
        if cnt > 30 then
            Log.line "render: %.3fms" (1000.0 * sum / float cnt)
            sum <- 0.0
            cnt <- 0
        
        resolve.Run(o)
    )


module ScanKernels = 
    open FShade 
    
    [<Literal>]
    let scanSize = 128

    [<Literal>]
    let halfScanSize = 64

    [<LocalSize(X = halfScanSize, Y = 1)>]
    let scanKernel (inputData : int[]) (outputData : int[]) =
        compute {
            let inputOffset : int = uniform?Arguments?inputOffset
            let inputDelta : int = uniform?Arguments?inputDelta
            let inputSize : int = uniform?Arguments?inputSize
            let outputOffset : int = uniform?Arguments?outputOffset
            let outputDelta : int = uniform?Arguments?outputDelta
            let rowLength : int = uniform?Arguments?rowLength 
            
            let line = getGlobalId().Y
            let inputOffset = rowLength * line + inputOffset
            let outputOffset = rowLength * line + outputOffset
            
            
            let mem : int[] = allocateShared scanSize
            let gid = getGlobalId().X
            let group = getWorkGroupId().X

            let gid0 = gid
            let lid0 =  getLocalId().X

            let lai = lid0
            let lbi = lid0 + halfScanSize
            let ai  = 2 * gid0 - lid0 
            let bi  = ai + halfScanSize 


            if ai < inputSize then mem.[lai] <- inputData.[inputOffset + ai * inputDelta]
            if bi < inputSize then mem.[lbi] <- inputData.[inputOffset + bi * inputDelta]

            //if lgid < inputSize then mem.[llid] <- inputData.[inputOffset + lgid * inputDelta]
            //if rgid < inputSize then mem.[rlid] <- inputData.[inputOffset + rgid * inputDelta]

            let lgid = 2 * gid0
            let rgid = lgid + 1
            let llid = 2 * lid0
            let rlid = llid + 1

            let mutable offset = 1
            let mutable d = halfScanSize
            while d > 0 do
                barrier()
                if lid0 < d then
                    let ai = offset * (llid + 1) - 1
                    let bi = offset * (rlid + 1) - 1
                    mem.[bi] <- mem.[ai] + mem.[bi]
                d <- d >>> 1
                offset <- offset <<< 1

            d <- 2
            offset <- offset >>> 1

            while d < scanSize do
                offset <- offset >>> 1
                barrier()
                if lid0 < d - 1 then
                    let ai = offset*(llid + 2) - 1
                    let bi = offset*(rlid + 2) - 1

                    mem.[bi] <- mem.[bi] + mem.[ai]

                d <- d <<< 1
            barrier()

            if lgid < inputSize then
                outputData.[outputOffset + lgid * outputDelta] <- mem.[llid]
            if rgid < inputSize then
                outputData.[outputOffset + rgid * outputDelta] <- mem.[rlid]

        }


    [<LocalSize(X = halfScanSize)>]
    let scanKernelInPlace (data : int[]) =
        compute {
            let inputOffset : int = uniform?Arguments?inputOffset
            let inputDelta : int = uniform?Arguments?inputDelta
            let inputSize : int = uniform?Arguments?inputSize
            let outputOffset : int = uniform?Arguments?outputOffset
            let outputDelta : int = uniform?Arguments?outputDelta
            let rowLength : int = uniform?Arguments?rowLength 

            let line = getGlobalId().Y
            let inputOffset = rowLength * line + inputOffset
            let outputOffset = rowLength * line + outputOffset
            
            let mem : int[] = allocateShared scanSize
            let gid = getGlobalId().X
            let group = getWorkGroupId().X

            let gid0 = gid
            let lid0 =  getLocalId().X

            let lai = lid0
            let lbi = lid0 + halfScanSize
            let ai  = 2 * gid0 - lid0 
            let bi  = ai + halfScanSize 


            if ai < inputSize then mem.[lai] <- data.[inputOffset + ai * inputDelta]
            if bi < inputSize then mem.[lbi] <- data.[inputOffset + bi * inputDelta]

            //if lgid < inputSize then mem.[llid] <- inputData.[inputOffset + lgid * inputDelta]
            //if rgid < inputSize then mem.[rlid] <- inputData.[inputOffset + rgid * inputDelta]

            let lgid = 2 * gid0
            let rgid = lgid + 1
            let llid = 2 * lid0
            let rlid = llid + 1

            let mutable offset = 1
            let mutable d = halfScanSize
            while d > 0 do
                barrier()
                if lid0 < d then
                    let ai = offset * (llid + 1) - 1
                    let bi = offset * (rlid + 1) - 1
                    mem.[bi] <- mem.[ai] + mem.[bi]
                d <- d >>> 1
                offset <- offset <<< 1

            d <- 2
            offset <- offset >>> 1

            while d < scanSize do
                offset <- offset >>> 1
                barrier()
                if lid0 < d - 1 then
                    let ai = offset*(llid + 2) - 1
                    let bi = offset*(rlid + 2) - 1

                    mem.[bi] <- mem.[bi] + mem.[ai]

                d <- d <<< 1
            barrier()

            if lgid < inputSize then
                data.[outputOffset + lgid * outputDelta] <- mem.[llid]
            if rgid < inputSize then
                data.[outputOffset + rgid * outputDelta] <- mem.[rlid]

        }

    [<LocalSize(X = halfScanSize)>]
    let fixupKernelInPlace (data : int[]) =
        compute {
            let inputOffset : int = uniform?Arguments?inputOffset
            let inputDelta : int = uniform?Arguments?inputDelta
            let outputOffset : int = uniform?Arguments?outputOffset
            let outputDelta : int = uniform?Arguments?outputDelta
            let groupSize : int = uniform?Arguments?groupSize
            let count : int = uniform?Arguments?count
            let rowLength : int = uniform?Arguments?rowLength 

            let line = getGlobalId().Y
            let inputOffset = rowLength * line + inputOffset
            let outputOffset = rowLength * line + outputOffset
            
            let id = getGlobalId().X + groupSize

            if id < count then
                let block = id / groupSize - 1
              
                let iid = inputOffset + block * inputDelta
                let oid = outputOffset + id * outputDelta

                if id % groupSize <> groupSize - 1 then
                    data.[oid] <- data.[iid] + data.[oid]

        }

type private Scanner(device : Device) =
    static let ceilDiv a b =
        if a % b = 0 then a / b
        else a / b + 1
    
    let scan = device.CompileCompute ScanKernels.scanKernel
    let scanInPlace = device.CompileCompute ScanKernels.scanKernelInPlace
    let fixup = device.CompileCompute ScanKernels.fixupKernelInPlace
    
    member x.Run(rows : int, columns : int, src : Buffer, dst : Buffer) =
        let rowLength = columns
        let rec run (src : Buffer) (srcOffset : int) (srcStride : int) (srcCount : int) (dst : Buffer) (dstOffset : int) (dstStride : int) (dstCount : int) =
            if srcCount > 1 then
                
                let kernel =
                    if src = dst then scanInPlace
                    else scan
                
                
                
                kernel.Run(V2i(ceilDiv srcCount ScanKernels.scanSize, rows), [
                    "rowLength", rowLength :> obj
                    "inputOffset", srcOffset :> obj
                    "inputDelta", srcStride :> obj
                    "inputSize", srcCount :> obj
                    "inputData", src :> obj
                    "outputOffset", dstOffset :> obj
                    "outputDelta", dstStride :> obj
                    "outputData", dst :> obj
                    "data", src :> obj
                ]).Wait()
        
                //let oSums = output.Skip(Kernels.scanSize - 1).Strided(Kernels.scanSize)
    
                let oSumsOffset = dstOffset + (ScanKernels.scanSize - 1) * dstStride
                let oSumsStride = dstStride * ScanKernels.scanSize
                let oSumsCount =
                    let lastIndex = dstOffset + (dstCount - 1) * dstStride
                    
                    // n <= (lastIndex - oSumsOffset) / oSumsStride
                    
                    (lastIndex - oSumsOffset) / oSumsStride + 1
    
                if oSumsCount > 0 then
                    run dst oSumsOffset oSumsStride oSumsCount dst oSumsOffset oSumsStride oSumsCount
                    if dstCount > ScanKernels.scanSize then
                        fixup.Run(V2i(ceilDiv (dstCount - ScanKernels.scanSize) ScanKernels.halfScanSize, rows), [
                            "rowLength", rowLength :> obj
                            "data", dst :> obj
                            "inputOffset", oSumsOffset :> obj
                            "inputDelta", oSumsStride :> obj
                            "outputOffset", dstOffset :> obj
                            "outputDelta", dstStride :> obj
                            "count", dstCount :> obj
                            "groupSize", ScanKernels.scanSize :> obj
                        ]).Wait()

        run src 0 1 columns dst 0 1 columns

    member x.Dispose() =
        scan.Dispose()
        scanInPlace.Dispose()
        fixup.Dispose()
    
    interface System.IDisposable with
        member x.Dispose() = x.Dispose()

[<AbstractClass; Sealed>]
type DeviceScanExtensions private() =
    static let cache = Dict<Device, Scanner>()
    
    [<Extension>]
    static member ScanRows(device : Device, rows : int, columns : int, src : Buffer, dst : Buffer) =
        let scan = lock cache (fun () -> cache.GetOrCreate(device, fun d -> new Scanner(d)))
        scan.Run(rows, columns, src, dst)
    
    
    


let scanTest() =
    Aardvark.Init()
    WebGPUShaderExtensions.ShaderCaching <- true
     
    let rasterizer = DefaultRasterizer.compile
     
    let app = WebGPUApplication.Create(true).Result
    let win = app.CreateGameWindow(vsync = true)
    
    
    let device = app.Device

    let r = RandomSystem()
    
    let rows = 1234
    let columns = 4321
    
    let arr = Array.init (rows * columns) (fun i -> r.UniformInt(100))
    let a = device.CreateBuffer(BufferUsage.Storage ||| BufferUsage.CopyDst, arr).Result
    let b = device.CreateBuffer(BufferUsage.Storage ||| BufferUsage.CopySrc, arr).Result
    device.ScanRows(rows, columns, a, b)
    
    
    let prefix = device.Download<int>(b).Result |> Array.chunkBySize columns
    
    let reff =
        arr |> Array.chunkBySize columns |> Array.map (fun arr ->
            let reff = Array.zeroCreate arr.Length
            let mutable sum = 0
            for i in 0 .. arr.Length - 1 do
                sum <- sum + arr.[i]
                reff.[i] <- sum
            reff
        )
    
    printfn "(%A)" (reff = prefix) 

let run() =
    Aardvark.Init()
    WebGPUShaderExtensions.ShaderCaching <- true
     
    let rasterizer = DefaultRasterizer.compile
     
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
    win.RenderAsFastAsPossible <- true
    win.Run()
