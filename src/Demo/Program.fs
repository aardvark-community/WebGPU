
open System.Diagnostics
open Aardvark.Application
open Aardvark.Application.Slim
open Aardvark.Base
open Aardvark.Data
open Aardvark.Rendering
open System.Threading
open Aardvark.Rendering.DefaultSemantic
open Aardvark.Rendering.WebGPU
open Microsoft.FSharp.NativeInterop
open global.WebGPU
open FSharp.Data.Adaptive

#nowarn "9"
type Blitter(device : Device, format : TextureFormat) =
    
    static let localSizeX = 8
    static let localSizeY = 8
    
    static let ceilDiv (a : int) (b : int) =
        if a % b = 0 then a / b
        else 1 + a / b
    
    
    let code =
        let wgslfmt = string(format).ToLower()
        $"""
        @group(0) @binding(0) var src : texture_2d<f32>;
        @group(0) @binding(1) var sam : sampler;
        @group(0) @binding(2) var dst : texture_storage_2d<{wgslfmt}, write>;
        
        @compute @workgroup_size({localSizeX}, {localSizeY})
        fn main(@builtin(global_invocation_id) GlobalInvocationID : vec3u) {{
            let size = textureDimensions(dst);
            let tc = (vec2f(GlobalInvocationID.xy) + vec2f(0.5, 0.5)) / vec2f(size);
            textureStore(dst, GlobalInvocationID.xy, textureSampleLevel(src, sam, tc, 0.0));
        }}
        """
        
    let shader =
        device.CreateShaderModule {
            Next = { Next = null; Code = code }
            Label = "Blitter"
        }
    
    let groupLayout =
        device.CreateBindGroupLayout {
            Label = "BlitterGroupLayout"
            Entries =
                [|
                    BindGroupLayoutEntry.Texture(
                        0, ShaderStage.Compute, {
                            SampleType = TextureSampleType.Float
                            ViewDimension = TextureViewDimension.D2D
                            Multisampled = false
                        }
                    )
                       
                    BindGroupLayoutEntry.Sampler(
                        1, ShaderStage.Compute,
                        SamplerBindingType.Filtering
                    )
             
                    BindGroupLayoutEntry.StorageTexture(
                        2, ShaderStage.Compute, {
                            Access = StorageTextureAccess.WriteOnly
                            Format = format
                            ViewDimension = TextureViewDimension.D2D
                        }
                    )
                    
                |]
        }
    
    let layout =
        device.CreatePipelineLayout {
            Next = null
            Label = "BlitterLayout"
            BindGroupLayouts = [|groupLayout |]
            ImmediateDataRangeByteSize = 0
        }
    
    let pipeline =
        device.CreateComputePipeline {
            Label = "BlitterPipeline"
            Layout = layout
            Compute =
                {
                    Module = shader
                    EntryPoint = "main"
                    Constants = [||]
                }
        }
    
    let sampler =
        device.CreateSampler {
            Next = null
            Label = null
            AddressModeU = AddressMode.ClampToEdge
            AddressModeV = AddressMode.ClampToEdge
            AddressModeW = AddressMode.ClampToEdge
            MagFilter = FilterMode.Linear
            MinFilter = FilterMode.Linear
            MipmapFilter = MipmapFilterMode.Nearest
            LodMinClamp = 0.0f
            LodMaxClamp = 1000.0f
            Compare = CompareFunction.Undefined
            MaxAnisotropy = 1us
        }
    
    
    member x.Run(input : Texture, inputLevel : int, output : Texture, outputLevel : int) =
        
        let outputSize =
            let f = 1 <<< outputLevel
            V2i(max 1 (output.Width / f), max 1 (output.Height / f))
        
        use output = output.CreateView(TextureUsage.StorageBinding, outputLevel)
        use input = input.CreateView(TextureUsage.TextureBinding, inputLevel)
        
        use enc = device.CreateCommandEncoder { Label = null; Next = null }
        
        use cenc = enc.BeginComputePass { Label = null; TimestampWrites = undefined }
    
        
        use group =
            device.CreateBindGroup {
                Label = null
                Layout = groupLayout
                Entries =
                    [|
                        BindGroupEntry.TextureView(0, input)
                        BindGroupEntry.Sampler(1, sampler)
                        BindGroupEntry.TextureView(2, output)
                    |]
            }
        
        cenc.SetPipeline pipeline
        cenc.SetBindGroup(0, group, [||])
        cenc.DispatchWorkgroups(ceilDiv outputSize.X localSizeX, ceilDiv outputSize.Y localSizeY, 1)
        cenc.End()
        
        use buf = enc.Finish { Label = null }
        device.Queue.Submit [| buf |]

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
            [<Position>] pos : V4d
            [<TexCoord>] tc : V2d
            [<Normal>] n : V3d
            [<Color>] col : V4d
        }
        

    let tex =
        sampler2dShadow {
            texture uniform?DiffuseColorTexture
            addressU WrapMode.Wrap
            addressV WrapMode.Wrap
            filter Filter.MinMagMipLinear
        }
        
        
    [<ReflectedDefinition>]
    let sammy (s : Sampler2dShadow) (tc : V2d) =
        s.Sample(tc, 0.5)
 

    let env =
        samplerCube {
            texture uniform?EnvMap
            addressU WrapMode.Wrap
            addressV WrapMode.Wrap
            addressW WrapMode.Wrap
            filter Filter.MinMagMipLinear
        }

    let envMap (v : Effects.Vertex) =
        fragment {
            let vp = uniform.ProjTrafoInv * V4d(v.pos.X, v.pos.Y, -1.0, 1.0)
            let vp = vp.XYZ / vp.W
            let dir = (uniform.ViewTrafoInv * V4d(vp, 0.0)).XYZ |> Vec.normalize
            return env.Sample(dir)
        }

    let reverseTrafo (v : Effects.Vertex) =
        vertex {
            let wp = uniform.ViewProjTrafoInv * v.pos
            return { v with wp = wp / wp.W }
        }
           
    let fragment (v : Vertex) =
        fragment {
            return V4d(1.0, 0.0, 0.0, 1.0) //Vec.normalize v.n * 0.5 + V3d.Half, 1.0)
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
    
    let tryCompileWGSL (code : string) =
        
        Report.BeginTimed "shader compile"
        
        for l in lineRx.Split code do
            Log.line "%s" l
        
        withDevice (fun device ->
            let shader =
                device.CreateShaderModule {
                    Label = null
                    Next = { ShaderSourceWGSL.Next = null; ShaderSourceWGSL.Code = code }
                }
                
            let info = shader.GetCompilationInfo().Result
            if info.Messages |> Array.exists (fun m -> m.Type <= CompilationMessageType.Warning) then
                    
                Log.start "code"
                let lines = lineRx.Split code
                
                let w = log10 (float lines.Length) |> ceil |> int
                let fmt = System.String('0', w)
                for i, l in Array.indexed lines do
                   // let l = l.Trim("\r".[0]).Trim('\n')
                    Report.Line("{0}| {1}", (i + 1).ToString(fmt), l)
                Log.stop()
                
                for m in info.Messages do
                    let str = sprintf "@%d,%d: %s" m.LineNum m.LinePos m.Message
                    for line in lineRx.Split str do
                        match m.Type with
                        | CompilationMessageType.Error -> Report.ErrorNoPrefix("{0}", line)
                        | CompilationMessageType.Warning -> Report.WarnNoPrefix("{0}", line)
                        | _ -> Report.Line("{0}", line)
            
        )
        Report.End() |> ignore
        ()
    
    let compileCompute (func : 'a -> 'b) =
        let code = 
            let c = ComputeShader.ofFunction (V3i(1024, 1024, 1024)) func
            ComputeShader.toModule c
            |> ModuleCompiler.compileWGSL
        tryCompileWGSL code.code
        
    let compileWGSL (effects : list<Effect>) =
        let effect = Effect.compose effects
        
        let config =
            {
                depthRange = Range1d(-1.0, 1.0)
                flipHandedness = false
                lastStage = ShaderStage.Fragment
                outputs = Map.ofList ["Colors", (typeof<V4d>, 0)]
            }
        
        let code = 
            effect
            |> Effect.toModule config
            |> ModuleCompiler.compileWGSL
        tryCompileWGSL code.code
        code


    let createPipelineLayout (device : Device) (iface : WGSL.WGSLProgramInterface) =
        
        let mutable groupDescriptors = MapExt.empty
        
        let stages = WebGPU.ShaderStage.Vertex ||| WebGPU.ShaderStage.Fragment
        for KeyValue(_, b) in iface.images do
            let sampleType =
                match b.imageType.valueType with
                | WGSL.WGSLType.Int(true,_) -> TextureSampleType.Sint
                | WGSL.WGSLType.Int(false,_) -> TextureSampleType.Uint
                | WGSL.WGSLType.Float _ -> TextureSampleType.Float
                | _ -> TextureSampleType.Undefined
            
            let viewDimension =
                match b.imageType.dimension with
                | SamplerDimension.Sampler1d -> TextureViewDimension.D1D
                | SamplerDimension.Sampler2d -> TextureViewDimension.D2D
                | SamplerDimension.Sampler3d -> TextureViewDimension.D3D
                | SamplerDimension.SamplerCube -> TextureViewDimension.Cube
                | _ -> TextureViewDimension.Undefined
                
            groupDescriptors <-
                groupDescriptors |> MapExt.alter b.imageGroup (fun g ->
                    let g = 
                        match g with
                        | Some g -> g
                        | None -> MapExt.empty
                    g |> MapExt.add b.imageBinding (
                        BindGroupLayoutEntry.Texture(b.imageBinding, stages, {
                            TextureBindingLayout.Multisampled = b.imageType.isMS
                            TextureBindingLayout.SampleType = sampleType
                            TextureBindingLayout.ViewDimension = viewDimension
                        })
                    ) |> Some
                )
               
        for KeyValue(_, b) in iface.samplers do
            groupDescriptors <-
                groupDescriptors |> MapExt.alter b.samplerGroup (fun g ->
                    let g = 
                        match g with
                        | Some g -> g
                        | None -> MapExt.empty
                    g |> MapExt.add b.samplerBinding (
                        BindGroupLayoutEntry.Sampler(b.samplerBinding, stages, SamplerBindingType.Filtering)
                    ) |> Some
                )
                
        for KeyValue(_, t) in iface.textures do
            let sampleType =
                match t.textureType.valueType with
                | WGSL.WGSLType.Int(true,_) -> TextureSampleType.Sint
                | WGSL.WGSLType.Int(false,_) -> TextureSampleType.Uint
                | WGSL.WGSLType.Float _ -> TextureSampleType.Float
                | _ -> TextureSampleType.Undefined
            
            let viewDimension =
                match t.textureType.dimension with
                | SamplerDimension.Sampler1d -> TextureViewDimension.D1D
                | SamplerDimension.Sampler2d -> TextureViewDimension.D2D
                | SamplerDimension.Sampler3d -> TextureViewDimension.D3D
                | SamplerDimension.SamplerCube -> TextureViewDimension.Cube
                | _ -> TextureViewDimension.Undefined
                
            groupDescriptors <-
                groupDescriptors |> MapExt.alter t.textureGroup (fun g ->
                    let g = 
                        match g with
                        | Some g -> g
                        | None -> MapExt.empty
                    g |> MapExt.add t.textureBinding (
                        BindGroupLayoutEntry.Texture(t.textureBinding, stages, {
                            TextureBindingLayout.Multisampled = t.textureType.isMS
                            TextureBindingLayout.SampleType = sampleType
                            TextureBindingLayout.ViewDimension = viewDimension
                        })
                    ) |> Some
                )
               
        for KeyValue(_, b) in iface.storageBuffers do
            groupDescriptors <-
                groupDescriptors |> MapExt.alter b.ssbGroup (fun g ->
                    let g = 
                        match g with
                        | Some g -> g
                        | None -> MapExt.empty
                    g |> MapExt.add b.ssbBinding (
                        BindGroupLayoutEntry.Buffer(b.ssbBinding, stages, {
                            BufferBindingLayout.Type = BufferBindingType.Storage
                            BufferBindingLayout.HasDynamicOffset = false
                            BufferBindingLayout.MinBindingSize = 0L
                        })
                    ) |> Some
                )
        
        for KeyValue(_, b) in iface.uniformBuffers do
            groupDescriptors <-
                groupDescriptors |> MapExt.alter b.ubGroup (fun g ->
                    let g = 
                        match g with
                        | Some g -> g
                        | None -> MapExt.empty
                    g |> MapExt.add b.ubBinding (
                        BindGroupLayoutEntry.Buffer(b.ubBinding, stages, {
                            BufferBindingLayout.Type = BufferBindingType.Uniform
                            BufferBindingLayout.HasDynamicOffset = false
                            BufferBindingLayout.MinBindingSize = int64 b.ubSize
                        })
                    ) |> Some
                )
        
       
        let groupLayouts =
            groupDescriptors |> MapExt.map (fun gi bindings ->
                device.CreateBindGroupLayout {
                    Label = null
                    Entries = bindings |> MapExt.values |> Array.ofSeq
                }    
            )
            
        let maxKey = MapExt.tryMax groupLayouts
            
        let groupLayouts =
            match maxKey with
            | Some maxIndex ->
                let arr = Array.zeroCreate (maxIndex + 1)
                groupLayouts |> MapExt.iter (fun k v -> arr.[k] <- v)
                arr
            | None ->
                [||]
            
        device.CreatePipelineLayout {
            Next = null
            Label = null
            BindGroupLayouts = groupLayouts
            ImmediateDataRangeByteSize = 0
        }

module Scene =
    open FSharp.Data.Adaptive
    open Aardvark.SceneGraph
    open Aardvark.Rendering.WebGPU
    
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
                let names = typeof<Marker>.Assembly.GetManifestResourceNames()
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
    
    
    let scene (size : aval<V2i>) (view : aval<Trafo3d>)  =
        Sg.ofList [
            Sg.farPlaneQuad
            |> Sg.texture "EnvMap" (skybox "miramar_$.png")
            |> Sg.shader {
                do! Shader.reverseTrafo
                do! Shader.envMap
            }
        
        
            Sg.box' C4b.Green Box3d.Unit
            |> Sg.shader {
                do! DefaultSurfaces.trafo
                do! Shader.sammm
                //do! DefaultSurfaces.constantColor C4f.White
                //do! DefaultSurfaces.simpleLighting
            }
            |> Sg.diffuseFileTexture "/Users/schorsch/Desktop/GettyImages-121786088-58a4cc5a5f9b58a3c955c5fb.jpg" true
        ]
        |> Sg.viewTrafo view
        |> Sg.projTrafo (size |> AVal.map (fun s -> Frustum.perspective 90.0 0.1 100.0 (float s.X / float s.Y) |> Frustum.projTrafo))
    
    let renderTask (size : aval<V2i>) (view : aval<Trafo3d>) (device : Device) =
        //let rt = Runtime(device) 
        let signature = device.CreateFramebufferSignature(1, Map.ofList [0, { Name = DefaultSemantic.Colors; Format = TextureFormat.Bgra8 }], Some TextureFormat.Depth24Stencil8)
        let objs = Aardvark.SceneGraph.Semantics.RenderObjectSemantics.Semantic.renderObjects Ag.Scope.Root (scene size view)
        //let objs = objs |> ASet.force |> ASet.ofSeq)
        let task = new RenderTask(device, signature, objs)
        task

[<EntryPoint>]
let main _argv =
    Aardvark.Init()
    
    let app = WebGPUApplication.Create(true).Result
    let win = app.CreateGameWindow(vsync = true)
    
    let cam =
        CameraView.lookAt (V3d(4,3,2)) V3d.Zero V3d.OOI
        |> DefaultCameraController.control win.Mouse win.Keyboard win.Time
        |> AVal.map CameraView.viewTrafo
    
    let task = 
        RenderTask.ofList [
            new ClearTask(app.Device, win.FramebufferSignature, AVal.constant (clear { color C4f.Red; depth 1.0; stencil 0 }))
            Scene.renderTask win.Sizes cam app.Device
        ]
        
    win.RenderTask <- task
    win.Run()
        
    // let wgsl = 
    //     Shader.compileWGSL [
    //         FShade.Effect.ofFunction DefaultSurfaces.trafo
    //         FShade.Effect.ofFunction DefaultSurfaces.simpleLighting
    //     ]
    // let layout = Shader.createPipelineLayout device wgsl.iface
    //
//     let win = Window.create()
//     let surf =
//         instance.CreateGLFWSurface {
//             Label = "Siegfried"
//             Window = win
//         }
//     
//     let blitter = Blitter(device, TextureFormat.RGBA8Unorm)
//         
//     let cap = surf.GetCapabilities(adapter)
//     
//     surf.Configure {
//         Device = device
//         Format = TextureFormat.BGRA8Unorm
//         Usage = cap.Usages
//         ViewFormats = [| TextureFormat.BGRA8Unorm |]
//         AlphaMode = CompositeAlphaMode.Opaque
//         Width = 1024
//         Height = 768
//         PresentMode = PresentMode.Fifo
//     }
// //        
// //     let code =
// //         """
// //             struct VertexInput {
// //                 @location(0) pos : vec4f,
// //                 @location(1) color : vec4f,
// //                 @location(2) tc : vec2f
// //             }
// //             struct VertexOutput {
// //                   @builtin(position) pos : vec4f,
// //                   @location(0) tc : vec2<f32>
// //             }
// //             
// //             struct UBO {
// //                 mvp : mat4x4<f32>
// //             }
// //             
// //              @group(0) @binding(0) var<uniform> ubo : UBO;
// //              @group(0) @binding(1) var tex : texture_2d<f32>;
// //              @group(0) @binding(2) var sam : sampler;
// //             
// //             fn sample(tc : vec2f) -> vec4f {
// //                 return textureSample(tex, sam, tc);
// //             }
// //             
// //             @vertex
// //             fn vs(input : VertexInput) -> VertexOutput {
// //               var res : VertexOutput;
// //               res.pos = input.pos * ubo.mvp;
// //               res.tc = input.tc;
// //               return res;
// //             }
// //
// //             @fragment
// //             fn fs(@location(0) tc : vec2f) -> @location(0) vec4f {
// //               return sample(tc);
// //             }
// //
// //         """
// //        
// //        
// //        
// //     let bgl =
// //         device.CreateBindGroupLayout {
// //             Label = "Paul"
// //             Entries =
// //                 [|
// //                     BindGroupLayoutEntry.Buffer(
// //                         0, ShaderStage.Vertex ||| ShaderStage.Fragment, {
// //                             Type = BufferBindingType.Uniform
// //                             HasDynamicOffset = false
// //                             MinBindingSize = 64L
// //                         }
// //                     )
// //                     
// //                     BindGroupLayoutEntry.Texture(
// //                         1, ShaderStage.Vertex ||| ShaderStage.Fragment, {
// //                             SampleType = TextureSampleType.Float
// //                             ViewDimension = TextureViewDimension.D2D
// //                             Multisampled = false
// //                         }
// //                     )
// //                     
// //                     BindGroupLayoutEntry.Sampler(
// //                         2, ShaderStage.Vertex ||| ShaderStage.Fragment,
// //                         SamplerBindingType.Filtering
// //                     )
// //                     
// //                 |]
// //         }
// //         
// //     let layoutDesc =
// //         {
// //             Next = null
// //             Label = "Peter"
// //             BindGroupLayouts  = [| bgl |]
// //             ImmediateDataRangeByteSize = 0
// //         }
// //         
// //         
// //     let layout = 
// //         device.CreatePipelineLayout {
// //             Next = null
// //             Label = "Peter"
// //             BindGroupLayouts  = [| bgl |]
// //             ImmediateDataRangeByteSize = 0
// //         }
// //     let shader = device.CompileShader(code)
// //     
// //     
// //     let pipeline =
// //         device.CreateRenderPipeline {
// //             Label = "Peter"
// //             Layout = layout
// //             Primitive =
// //                 {
// //                     Topology = PrimitiveTopology.TriangleList
// //                     StripIndexFormat = IndexFormat.Undefined
// //                     FrontFace = FrontFace.CCW
// //                     CullMode = CullMode.None
// //                     UnclippedDepth = false
// //                 }
// //             Multisample =
// //                 {
// //                     Count = 1
// //                     Mask = 1
// //                     AlphaToCoverageEnabled = false
// //                 }
// //             Vertex = {
// //                 Module = shader
// //                 EntryPoint = "vs"
// //                 Constants = [||]
// //                 Buffers =
// //                     [|
// //                         {
// //                             ArrayStride = 12L
// //                             StepMode = VertexStepMode.Vertex
// //                             Attributes =
// //                                 [|
// //                                     {
// //                                         Format = VertexFormat.Float32x3
// //                                         Offset = 0L
// //                                         ShaderLocation = 0
// //                                     }
// //                                 |]
// //                         }
// //                         {
// //                             ArrayStride = 4L
// //                             StepMode = VertexStepMode.Vertex
// //                             Attributes =
// //                                 [|
// //                                     {
// //                                         Format = VertexFormat.Unorm8x4BGRA
// //                                         Offset = 0L
// //                                         ShaderLocation = 1
// //                                     }
// //                                 |]
// //                         }
// //                         {
// //                             ArrayStride = 8L
// //                             StepMode = VertexStepMode.Vertex
// //                             Attributes =
// //                                 [|
// //                                     {
// //                                         Format = VertexFormat.Float32x2
// //                                         Offset = 0L
// //                                         ShaderLocation = 2
// //                                     }
// //                                 |]
// //                         }
// //                     |]
// //             }
// //             Fragment =
// //                 {
// //                     Module = shader
// //                     EntryPoint = "fs"
// //                     Constants = [||]
// //                     Targets =
// //                         [|
// //                             {
// //                                 Next = null
// //                                 Format = TextureFormat.BGRA8Unorm
// //                                 Blend = BlendState.Null
// //                                 WriteMask = ColorWriteMask.All
// //                             }
// //                         |]
// //                 }
// //             
// //             DepthStencil = DepthStencilState.Null
// //         }
// //     
// //     
// //     let pos = device.CreateBuffer(BufferUsage.Vertex, [| V3f(-0.5, -0.5, 0.0); V3f(0.5, -0.5, 0.0); V3f(0.0, 0.5, 0.0) |]).Result
// //     let color = device.CreateBuffer(BufferUsage.Vertex, [| C4b.Red; C4b.Green; C4b.Blue |]).Result
// //     let tc = device.CreateBuffer(BufferUsage.Vertex, [| V2f.OO; V2f.IO; V2f(0.5, 1.0) |]).Result
// //     let ubo = device.CreateBuffer(BufferUsage.Uniform, [| M44f.Identity |]).Result
// //     
// //     let bla = tc.[8 .. ]
// //     
// //     let img = PixImageSharp.Create "/Users/schorsch/Downloads/brick_texture3452.jpg"
// //
// //     let tex =
// //         device.CreateTexture {
// //             Next = null
// //             Label = "Timmy"
// //             Usage = TextureUsage.CopyDst ||| TextureUsage.StorageBinding ||| TextureUsage.TextureBinding
// //             Dimension = TextureDimension.D2D
// //             Size = { Width = img.Size.X; Height = img.Size.Y; DepthOrArrayLayers = 1 }
// //             Format = TextureFormat.RGBA8Unorm
// //             MipLevelCount = mipMapLevels2d img.Size
// //             SampleCount = 1
// //             ViewFormats = [|TextureFormat.RGBA8Unorm|]
// //         }
// //         
// //     do
// //         use enc = device.CreateCommandEncoder { Label = null; Next = null }
// //         enc.CopyImageToTexture(img, tex)
// //         
// //         use cmd = enc.Finish { Label = null }
// //         device.Queue.Submit [| cmd |]
// //         
// //         for l in 1 .. tex.MipLevelCount - 1 do
// //             blitter.Run(tex, l - 1, tex, l)
// //             
// //         device.Queue.Wait().Wait()
// //     //     
// //     //
// //     // do
// //     //     
// //     //     let bpr = img.Size.X * 4
// //     //     let fakebpr =
// //     //         if bpr % 256 = 0 then bpr
// //     //         else (bpr / 256 + 1) * 256
// //     //     
// //     //     let tmp =
// //     //         device.CreateBuffer {
// //     //             Next = null
// //     //             Label = null
// //     //             Usage = BufferUsage.CopySrc ||| BufferUsage.MapWrite
// //     //             Size = int64 fakebpr * int64 img.Size.Y
// //     //             MappedAtCreation = true
// //     //         }
// //     //         
// //     //     let tmpPtr = tmp.GetMappedRange(0L, tmp.Size)
// //     //     
// //     //     
// //     //     let fakeWidth = fakebpr / 4
// //     //     let dstVolume = NativeVolume<byte>(NativePtr.ofNativeInt tmpPtr, VolumeInfo(0L, V3l(fakeWidth, img.Size.Y, 4), V3l(4L, 4L*int64 fakeWidth, 1L)))
// //     //         
// //     //     NativeVolume.using img.Volume (fun srcVolume ->
// //     //        NativeVolume.copy srcVolume (dstVolume.SubVolume(V3l.Zero, V3l(img.Size.X, img.Size.Y, 4)))
// //     //     )
// //     //     tmp.Unmap()
// //     //     
// //     //     use enc = device.CreateCommandEncoder { Label = null; Next = null }
// //     //     let src : ImageCopyBuffer =
// //     //         {
// //     //             Layout =
// //     //                 {
// //     //                     Next = null
// //     //                     Offset = 0L
// //     //                     BytesPerRow = fakebpr
// //     //                     RowsPerImage = img.Size.Y
// //     //                 }
// //     //             Buffer = tmp
// //     //         }
// //     //     let dst : ImageCopyTexture =
// //     //         {
// //     //             Texture = tex
// //     //             Origin = { X = 0; Y = 0; Z = 0 }
// //     //             Aspect = TextureAspect.All
// //     //             MipLevel = 0
// //     //         }
// //     //         
// //     //     enc.CopyBufferToTexture(src, dst, { Width = img.Size.X; Height = img.Size.Y; DepthOrArrayLayers = 1 })
// //     //     
// //     //     use cmd = enc.Finish { Label = null; Next = null }
// //     //     device.Queue.Submit [| cmd |]
// //     //     device.Queue.Wait().Wait()
// //     //    
// //         
// //     
// //      
// //     
// //     let view = tex.CreateView TextureUsage.TextureBinding
// //         
// //     let sam =
// //         device.CreateSampler {
// //             Next = null
// //             Label = null
// //             AddressModeU = AddressMode.Repeat
// //             AddressModeV = AddressMode.Repeat
// //             AddressModeW = AddressMode.Repeat
// //             MagFilter = FilterMode.Linear
// //             MinFilter = FilterMode.Linear
// //             MipmapFilter = MipmapFilterMode.Linear
// //             LodMinClamp = 0.0f
// //             LodMaxClamp = 1000.0f
// //             Compare = CompareFunction.Undefined
// //             MaxAnisotropy = 1us
// //         }
// //     
// //     
// //     let bg =
// //         device.CreateBindGroup {
// //             Label = "Peggy"
// //             Layout = bgl
// //             Entries =
// //                 [|
// //                     BindGroupEntry.Buffer(0, ubo)
// //                     BindGroupEntry.TextureView(1, view)
// //                     BindGroupEntry.Sampler(2, sam)
// //                 |]
// //         }
// //     
// //     let bundle = 
// //         use benc =
// //             device.CreateRenderBundleEncoder {
// //                 Label = null
// //                 ColorFormats = [| TextureFormat.BGRA8Unorm |]
// //                 DepthStencilFormat = TextureFormat.Undefined
// //                 SampleCount = 1
// //                 DepthReadOnly = true
// //                 StencilReadOnly = true
// //             }
// //         benc.SetPipeline pipeline
// //         benc.SetBindGroup(0, bg, [||])
// //         benc.SetVertexBuffer(0, pos, 0, pos.Size)
// //         benc.SetVertexBuffer(1, color, 0, color.Size)
// //         benc.SetVertexBuffer(2, tc, 0, tc.Size)
// //         benc.Draw(3, 1, 0, 0)
// //         benc.Finish {
// //             Label = null
// //         }
//         
//
//     let size = cval V2i.II
//     let task = Scene.renderTask size device
//         
//     let rand = RandomSystem()
//     let sw = Stopwatch.StartNew()
//     
//     let mutable depthTex : option<Texture> = None
//     
//     
//     
//     
//     win |> Window.run (fun viewport ->
//         transact (fun () -> size.Value <- viewport)
//         
//         use colorTex = surf.CurrentTexture.Texture
//         
//         let depthTex =
//             match depthTex with
//             | Some t when t.Width = colorTex.Width && t.Height = colorTex.Height -> t
//             | _ ->
//                 match depthTex with
//                 | Some t -> t.Dispose()
//                 | _ -> ()
//                 let depth =
//                     device.CreateTexture {
//                         Next = null
//                         Label = null
//                         Usage = TextureUsage.RenderAttachment
//                         Dimension = TextureDimension.D2D
//                         Size = { Width = viewport.X; Height = viewport.Y; DepthOrArrayLayers = 1 }
//                         Format = WebGPU.TextureFormat.Depth24PlusStencil8
//                         MipLevelCount = 1
//                         SampleCount = 1
//                         ViewFormats = [| TextureFormat.Depth24PlusStencil8 |]
//                     }
//                 depthTex <- Some depth
//                 depth
//         
//         use colorView =
//             colorTex.CreateView {
//                 Next = null
//                 Label = "Vernon"
//                 Format = TextureFormat.BGRA8Unorm
//                 Dimension = TextureViewDimension.D2D
//                 BaseMipLevel = 0
//                 MipLevelCount = 1
//                 BaseArrayLayer = 0
//                 ArrayLayerCount = 1
//                 Aspect = TextureAspect.Undefined
//                 Usage = cap.Usages
//             }
//         
//         use depthView =
//             depthTex.CreateView {
//                 Next = null
//                 Label = "Vernon2"
//                 Format = TextureFormat.Depth24PlusStencil8
//                 Dimension = TextureViewDimension.D2D
//                 BaseMipLevel = 0
//                 MipLevelCount = 1
//                 BaseArrayLayer = 0
//                 ArrayLayerCount = 1
//                 Aspect = WebGPU.TextureAspect.All
//                 Usage = TextureUsage.RenderAttachment
//             }
//        
//         let fbo = new Aardvark.Rendering.WebGPU.Framebuffer(task.FramebufferSignature, V2i(viewport.X, viewport.Y), [| colorView |], Some depthView)
//         task.Run(FSharp.Data.Adaptive.AdaptiveToken.Top, OutputDescription.ofFramebuffer fbo)
//         
//         surf.Present()
//     )
//     
    
    printfn "exit"
        
    0
