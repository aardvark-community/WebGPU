module Demo.DefaultRasterizer

open FShade
open Aardvark.Base
open Aardvark.Rendering.WebGPU
open WebGPU

module Shader =
    
    open FShade
    
    type Vertex =
        {
            [<Position>] pos : V4f
            [<Semantic("Normals")>] n : V4f
            [<Semantic("ViewPos")>] vp : V4f
            [<Color>] c : V4f
        }
    
    type UniformScope with
        member x.ProjTrafo : M44f = uniform?ProjTrafo
        member x.ModelViewTrafo : M44f = uniform?ModelViewTrafo
    
    let vertex (v : Vertex) =
        vertex {
            let vp = uniform.ModelViewTrafo * v.pos
            return {
                pos = uniform.ProjTrafo * vp
                vp = vp
                n = uniform.ModelViewTrafo * v.n
                c = v.c
            }
        }
    
    
    let fragment (v : Vertex) =
        fragment {
            let vn = Vec.normalize v.n.XYZ
            let light = V3f.Zero
            let lightDir = Vec.normalize (light - v.vp.XYZ)
            let diffuse = Vec.dot lightDir vn |> abs
            
            let light = 0.2f + 0.8f*diffuse
            
            let c = V4f(v.c.XYZ * light, 1.0f)
            
            return V4ui(packUnorm4x8 c)
        }
        

let compile (device : Device) : Rasterizer =
    
    let effect =
        Effect.compose [
            Effect.ofFunction Shader.vertex
            Effect.ofFunction Shader.fragment
        ]
        
    let signature =
        new Aardvark.Rendering.WebGPU.FramebufferSignature(
            device, 1, Map.ofList [0, { Format = Aardvark.Rendering.TextureFormat.R32ui; Name = Symbol.Create "Colors" }], Some Aardvark.Rendering.TextureFormat.Depth24Stencil8)
        
    let program = device.CreateShaderProgram(effect, signature)
    
    let blend : BlendState =
        {
            Color = { Operation = BlendOperation.Undefined; SrcFactor = BlendFactor.One; DstFactor = BlendFactor.Zero }
            Alpha = { Operation = BlendOperation.Undefined; SrcFactor = BlendFactor.One; DstFactor = BlendFactor.Zero }
        }
        
    let lPos = program.ShaderCode.iface.inputs |> List.pick (fun i -> if i.paramSemantic = "Positions" then Some i.paramLocation else None)
    let lNorm = program.ShaderCode.iface.inputs |> List.pick (fun i -> if i.paramSemantic = "Normals" then Some i.paramLocation else None)
    let lCol = program.ShaderCode.iface.inputs |> List.pick (fun i -> if i.paramSemantic = "Colors" then Some i.paramLocation else None)
    
    let buffers =
        [|
            {
                StepMode = VertexStepMode.Vertex
                ArrayStride = int64 sizeof<V4f>
                Attributes = [| { Format = VertexFormat.Float32x4; Offset = 0; ShaderLocation = lPos } |]
            }
            
            {
                StepMode = VertexStepMode.Vertex
                ArrayStride = int64 sizeof<V4f>
                Attributes = [| { Format = VertexFormat.Float32x4; Offset = 0; ShaderLocation = lNorm } |]
            }
            
            {
                StepMode = VertexStepMode.Vertex
                ArrayStride = int64 sizeof<C4b>
                Attributes = [| { Format = VertexFormat.Unorm8x4BGRA; Offset = 0; ShaderLocation = lCol } |]
            }
        |]
    
    let pipe =
        device.CreateRenderPipeline {
            Label = null
            Layout = program.PipelineLayout
            Vertex = {
                Module = program.ShaderModules.[FShade.ShaderStage.Vertex]
                EntryPoint = "main"
                Buffers = buffers
                Constants = [||]
            }
            Fragment = {
                Module = program.ShaderModules.[FShade.ShaderStage.Fragment]
                EntryPoint = "main"
                Targets = [| { Next = null; Format = TextureFormat.R32Uint; Blend = BlendState.Null; WriteMask = ColorWriteMask.All } |]
                Constants = [||]
            }
            Primitive = {
                Topology = PrimitiveTopology.TriangleList
                StripIndexFormat = IndexFormat.Undefined
                FrontFace = FrontFace.CCW
                CullMode = CullMode.None
                UnclippedDepth = false
            }
            DepthStencil = {
                Format = TextureFormat.Depth24PlusStencil8
                DepthWriteEnabled = OptionalBool.True
                DepthCompare = CompareFunction.LessEqual //CompareFunction.LessEqual
                StencilFront = StencilFaceState.Null
                // {
                //     Compare = CompareFunction.Always
                //     FailOp = StencilOperation.Keep
                //     PassOp = StencilOperation.Keep
                //     DepthFailOp = StencilOperation.Keep
                // }
                StencilBack = StencilFaceState.Null
                // {
                //     Compare = CompareFunction.Always
                //     FailOp = StencilOperation.Keep
                //     PassOp = StencilOperation.Keep
                //     DepthFailOp = StencilOperation.Keep
                // }
                StencilReadMask = 0
                StencilWriteMask = 0
                DepthBias = 0
                DepthBiasSlopeScale = 0.0f
                DepthBiasClamp = 0.0f
            }
            Multisample = {
                Count = 1
                Mask = 0xFFFFFFFF
                AlphaToCoverageEnabled = false
            }
        }
    
    
    let _, uu = program.ShaderCode.iface.uniformBuffers |> MapExt.toSeq |> Seq.head
    
    let uboMem = Array.zeroCreate<M44f> 2
    let ubo =
        device.CreateBuffer(BufferUsage.Uniform ||| BufferUsage.CopyDst, uboMem).Result
    
    let g0 = 
        device.CreateBindGroup {
            Label = null
            Layout = program.BindGroupLayouts.[0]
            Entries =
                [|
                    BindGroupEntry.Buffer(0, ubo)
                |]
        }
    
    let fMV = uu.ubFields |> List.find (fun f -> f.ufName = "ModelViewTrafo") 
    let fProj = uu.ubFields |> List.find (fun f -> f.ufName = "ProjTrafo") 
    
    let mutable depthTex = device.CreateTexture(TextureUsage.RenderAttachment, TextureFormat.Depth24PlusStencil8, V2i.II)
    
    let repairProj = Trafo3d.Scale(1.0, -1.0, 1.0)
    fun (input : RasterizerInput) ->
        task {     
            if input.ColorTexture.Width <> depthTex.Width || input.ColorTexture.Height <> depthTex.Height then
                depthTex.Dispose()
                depthTex <- device.CreateTexture(TextureUsage.RenderAttachment, TextureFormat.Depth24PlusStencil8, V2i(input.ColorTexture.Width, input.ColorTexture.Height))
            
            uboMem.[fMV.ufOffset / sizeof<M44f>] <- M44f input.ModelViewTrafo.Forward
            uboMem.[fProj.ufOffset / sizeof<M44f>] <- M44f (input.ProjTrafo * repairProj).Forward
            use cView = input.ColorTexture.CreateView(TextureUsage.RenderAttachment)
            use dView = depthTex.CreateView(TextureUsage.RenderAttachment)
            use enc = device.CreateCommandEncoder { Label = null; Next = null }
            enc.Upload(uboMem, ubo)
            use renc = 
                enc.BeginRenderPass {
                    Label = null
                    Next = null
                    ColorAttachments =
                        [|
                            {
                                Next = null
                                View = cView
                                DepthSlice = -1
                                ResolveTarget = TextureView.Null
                                LoadOp = LoadOp.Clear
                                StoreOp = StoreOp.Store
                                ClearValue = { R = 0.0; G = 0.0; B = 0.0; A = 1.0}
                            }
                            
                        |]
                    DepthStencilAttachment =
                        {
                            View = dView
                            DepthLoadOp = LoadOp.Clear
                            DepthStoreOp = StoreOp.Store
                            DepthClearValue = 1.0f
                            DepthReadOnly = false
                            StencilLoadOp = LoadOp.Clear
                            StencilStoreOp = StoreOp.Store
                            StencilClearValue = 0
                            StencilReadOnly = false
                        }
                    OcclusionQuerySet = QuerySet.Null
                    TimestampWrites = PassTimestampWrites.Null
                }
                
            renc.SetPipeline(pipe)
            renc.SetBindGroup(0, g0, [||])
            renc.SetVertexBuffer(0, input.Positions, 0L, input.Positions.Size)
            renc.SetVertexBuffer(1, input.Normals, 0L, input.Normals.Size)
            renc.SetVertexBuffer(2, input.Colors, 0L, input.Colors.Size)
            renc.Draw(int input.Positions.Size / sizeof<V4f>, 1, 0, 0)
            renc.End()
            
            use cmd = enc.Finish { Label = null }
            
            do! device.Queue.Submit [| cmd |]
        }