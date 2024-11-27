
open System.Diagnostics
open Aardvark.Base
open Aardvark.Data
open Aardvark.Rendering
open System.Threading
open Aardvark.Rendering.DefaultSemantic
open Microsoft.FSharp.NativeInterop
open WebGPU

#nowarn "9"

type MyGLFW() =

    inherit Silk.NET.GLFW.Glfw(MyGLFW.Context)

    static let ctx =
        lazy (
            let lib = Aardvark.LoadLibrary(typeof<Instance>.Assembly, "libglfw.3.dylib")
            {
                new Silk.NET.Core.Contexts.INativeContext with
                    member x.Dispose() = ()
                    member this.GetProcAddress(proc,slot) = Aardvark.GetProcAddress(lib, proc)
                    member this.TryGetProcAddress(proc,addr,slot) =
                        let ptr = Aardvark.GetProcAddress(lib, proc)
                        if ptr = 0n then false
                        else
                            addr <- ptr
                            true
            }
        )
    static member Context = ctx.Value

module Window =
    open Silk.NET.GLFW
    
    let glfw =
        lazy (
            let glfw = new MyGLFW() :> Glfw
            glfw.Init() |> ignore
            glfw
        )
    
    let create () =
        let glfw = glfw.Value
        glfw.DefaultWindowHints()
        // let retina =
        //     if RuntimeInformation.IsOSPlatform OSPlatform.OSX then true
        //     else false
        //glfw.WindowHint(unbox<WindowHintBool> 0x00023001, retina)
        glfw.WindowHint(WindowHintBool.TransparentFramebuffer, false)
        glfw.WindowHint(WindowHintBool.Visible, false)
        glfw.WindowHint(WindowHintBool.Resizable, true)
        glfw.WindowHint(WindowHintInt.RefreshRate, 0)
        glfw.WindowHint(WindowHintBool.FocusOnShow, true)
        glfw.WindowHint(WindowHintClientApi.ClientApi, ClientApi.NoApi)
        let win = glfw.CreateWindow(1024, 768, "Winnie", NativePtr.ofNativeInt 0n, NativePtr.ofNativeInt 0n)
        if NativePtr.toNativeInt win = 0n then
            let description = "Could not create window"

            let msg = $"[GLFW] {description}"
            Report.Error msg
            failwith msg
        win
        
    let run (render : V2i -> unit) (win : nativeptr<WindowHandle>) =
        let glfw = glfw.Value
        glfw.ShowWindow(win)
        let mutable run = true
        glfw.SetWindowCloseCallback(win, GlfwCallbacks.WindowCloseCallback(fun _ ->
            run <- false
            glfw.HideWindow(win)
            glfw.PostEmptyEvent()
        )) |> ignore
        while run do
            glfw.PollEvents()
            let (w, h) = glfw.GetWindowSize(win)
            render(V2i(w,h))


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
            Next = null
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
    
    
    type Vertex =
        {
            [<Position>] pos : V4d
            [<TexCoord>] tc : V2d
            [<Color>] col : V4d
        }
        
    let vertex (v : Vertex) =
        vertex {
            return { v with pos =  uniform.ModelViewProjTrafo * v.pos }
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
        
    let fragment (v : Vertex) =
        fragment {
            return V4d.IIII * sammy tex v.tc
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
            WebGPU.CreateInstance {
                Next = null
                Features = WebGPU.InstanceFeatures
            }
            
        let start =
            ThreadStart(fun () ->
                while running do
                    instance.ProcessEvents()
            )
        let thread = Thread(start, IsBackground = true)
        thread.Start()

        use adapter = 
            instance.RequestAdapterAsync({
                Next = null
                CompatibleSurface = Surface.Null
                PowerPreference = PowerPreference.HighPerformance
                BackendType = BackendType.Undefined
                ForceFallbackAdapter = false
                CompatibilityMode = false
            }).Result
     
        use device = 
            adapter.RequestDeviceAsync({
                Next = null
                DebugOutput = true
                Label = "Devon"
                RequiredFeatures = adapter.Features.Features
                RequiredLimits = { Limits = adapter.Limits.Limits }
                DefaultQueue = { Label = "Quentin" }
            }).Result
        
        
        try
            action device
        finally 
            running <- false
            thread.Join()
    
    let tryCompileWGSL (code : string) =
        
        Report.BeginTimed "shader compile"
        
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
        let signature =
            { new IFramebufferSignature with
                member x.Dispose() = ()
                member x.ColorAttachments = Map.ofList [0, { Name = DefaultSemantic.Colors; Format = TextureFormat.Rgba8 }]
                member x.DepthStencilAttachment = Some TextureFormat.Depth24Stencil8
                member x.LayerCount = 1
                member x.Samples = 1
                member x.PerLayerUniforms = Set.empty
                member x.Runtime = Unchecked.defaultof<_>
            }
        
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

let inline bla< ^a, ^b when ^a : (static member (+) : ^a -> ^a -> ^a)> (a : ^a) (b : ^a) =
    a + b

[<EntryPoint>]
let main _argv =
    Aardvark.Init()
    
    Shader.compileCompute Shader.computer
    Shader.compileWGSL [FShade.Effect.ofFunction Shader.vertex; FShade.Effect.ofFunction Shader.fragment]
    exit 0
   
    
    let instance =
        WebGPU.CreateInstance {
            Next = null
            Features = WebGPU.InstanceFeatures
        }
        
    let start =
        ThreadStart(fun () ->
            while true do
                instance.ProcessEvents()
        )
    let thread = Thread(start, IsBackground = true)
    thread.Start()

    let adapter = 
        instance.RequestAdapterAsync({
            Next = null
            CompatibleSurface = Surface.Null
            PowerPreference = PowerPreference.HighPerformance
            BackendType = BackendType.Undefined
            ForceFallbackAdapter = false
            CompatibilityMode = false
        }).Result
 
    let device = 
        adapter.RequestDeviceAsync({
            Next = null
            DebugOutput = true
            Label = "Devon"
            RequiredFeatures = adapter.Features.Features
            RequiredLimits = { Limits = adapter.Limits.Limits }
            DefaultQueue = { Label = "Quentin" }
        }).Result
        
    
    let win = Window.create()
    let surf =
        instance.CreateGLFWSurface {
            Label = "Siegfried"
            Window = win
        }
    
    let blitter = Blitter(device, TextureFormat.RGBA8Unorm)
        
    let cap = surf.GetCapabilities(adapter)
    
    surf.Configure {
        Device = device
        Format = TextureFormat.BGRA8Unorm
        Usage = cap.Usages
        ViewFormats = [| TextureFormat.BGRA8Unorm |]
        AlphaMode = CompositeAlphaMode.Opaque
        Width = 1024
        Height = 768
        PresentMode = PresentMode.Fifo
    }
       
    let code =
        """
            struct VertexInput {
                @location(0) pos : vec4f,
                @location(1) color : vec4f,
                @location(2) tc : vec2f
            }
            struct VertexOutput {
                  @builtin(position) pos : vec4f,
                  @location(0) tc : vec2<f32>
            }
            
            struct UBO {
                mvp : mat4x4<f32>
            }
            
             @group(0) @binding(0) var<uniform> ubo : UBO;
             @group(0) @binding(1) var tex : texture_2d<f32>;
             @group(0) @binding(2) var sam : sampler;
            
            fn sample(tc : vec2f) -> vec4f {
                return textureSample(tex, sam, tc);
            }
            
            @vertex
            fn vs(input : VertexInput) -> VertexOutput {
              var res : VertexOutput;
              res.pos = input.pos * ubo.mvp;
              res.tc = input.tc;
              return res;
            }

            @fragment
            fn fs(@location(0) tc : vec2f) -> @location(0) vec4f {
              return sample(tc);
            }

        """
       
    let bgl =
        device.CreateBindGroupLayout {
            Label = "Paul"
            Entries =
                [|
                    BindGroupLayoutEntry.Buffer(
                        0, ShaderStage.Vertex ||| ShaderStage.Fragment, {
                            Type = BufferBindingType.Uniform
                            HasDynamicOffset = false
                            MinBindingSize = 64L
                        }
                    )
                    
                    BindGroupLayoutEntry.Texture(
                        1, ShaderStage.Vertex ||| ShaderStage.Fragment, {
                            SampleType = TextureSampleType.Float
                            ViewDimension = TextureViewDimension.D2D
                            Multisampled = false
                        }
                    )
                    
                    BindGroupLayoutEntry.Sampler(
                        2, ShaderStage.Vertex ||| ShaderStage.Fragment,
                        SamplerBindingType.Filtering
                    )
                    
                |]
        }
        
    let layoutDesc =
        {
            Next = null
            Label = "Peter"
            BindGroupLayouts  = [| bgl |]
            ImmediateDataRangeByteSize = 0
        }
        
        
    let layout = 
        device.CreatePipelineLayout {
            Next = null
            Label = "Peter"
            BindGroupLayouts  = [| bgl |]
            ImmediateDataRangeByteSize = 0
        }
    let shader = device.CompileShader(code)
    
    
    let pipeline =
        device.CreateRenderPipeline {
            Label = "Peter"
            Layout = layout
            Primitive =
                {
                    Topology = PrimitiveTopology.TriangleList
                    StripIndexFormat = IndexFormat.Undefined
                    FrontFace = FrontFace.CCW
                    CullMode = CullMode.None
                    UnclippedDepth = false
                }
            Multisample =
                {
                    Count = 1
                    Mask = 1
                    AlphaToCoverageEnabled = false
                }
            Vertex = {
                Module = shader
                EntryPoint = "vs"
                Constants = [||]
                Buffers =
                    [|
                        {
                            ArrayStride = 12L
                            StepMode = VertexStepMode.Vertex
                            Attributes =
                                [|
                                    {
                                        Format = VertexFormat.Float32x3
                                        Offset = 0L
                                        ShaderLocation = 0
                                    }
                                |]
                        }
                        {
                            ArrayStride = 4L
                            StepMode = VertexStepMode.Vertex
                            Attributes =
                                [|
                                    {
                                        Format = VertexFormat.Unorm8x4BGRA
                                        Offset = 0L
                                        ShaderLocation = 1
                                    }
                                |]
                        }
                        {
                            ArrayStride = 8L
                            StepMode = VertexStepMode.Vertex
                            Attributes =
                                [|
                                    {
                                        Format = VertexFormat.Float32x2
                                        Offset = 0L
                                        ShaderLocation = 2
                                    }
                                |]
                        }
                    |]
            }
            Fragment =
                {
                    Module = shader
                    EntryPoint = "fs"
                    Constants = [||]
                    Targets =
                        [|
                            {
                                Next = null
                                Format = TextureFormat.BGRA8Unorm
                                Blend = BlendState.Null
                                WriteMask = ColorWriteMask.All
                            }
                        |]
                }
            
            DepthStencil = DepthStencilState.Null
        }
    
    
    let pos = device.CreateBuffer(BufferUsage.Vertex, [| V3f(-0.5, -0.5, 0.0); V3f(0.5, -0.5, 0.0); V3f(0.0, 0.5, 0.0) |]).Result
    let color = device.CreateBuffer(BufferUsage.Vertex, [| C4b.Red; C4b.Green; C4b.Blue |]).Result
    let tc = device.CreateBuffer(BufferUsage.Vertex, [| V2f.OO; V2f.IO; V2f(0.5, 1.0) |]).Result
    let ubo = device.CreateBuffer(BufferUsage.Uniform, [| M44f.Identity |]).Result
    
    let bla = tc.[8 .. ]
    
    let img = PixImageSharp.Create "/Users/schorsch/Downloads/brick_texture3452.jpg"

    let tex =
        device.CreateTexture {
            Next = null
            Label = "Timmy"
            Usage = TextureUsage.CopyDst ||| TextureUsage.StorageBinding ||| TextureUsage.TextureBinding
            Dimension = TextureDimension.D2D
            Size = { Width = img.Size.X; Height = img.Size.Y; DepthOrArrayLayers = 1 }
            Format = TextureFormat.RGBA8Unorm
            MipLevelCount = mipMapLevels img.Size
            SampleCount = 1
            ViewFormats = [|TextureFormat.RGBA8Unorm|]
        }
        
    do
        use enc = device.CreateCommandEncoder { Label = null; Next = null }
        enc.CopyImageToTexture(img, tex)
        
        use cmd = enc.Finish { Label = null }
        device.Queue.Submit [| cmd |]
        
        for l in 1 .. tex.MipLevelCount - 1 do
            blitter.Run(tex, l - 1, tex, l)
            
        device.Queue.Wait().Wait()
    //     
    //
    // do
    //     
    //     let bpr = img.Size.X * 4
    //     let fakebpr =
    //         if bpr % 256 = 0 then bpr
    //         else (bpr / 256 + 1) * 256
    //     
    //     let tmp =
    //         device.CreateBuffer {
    //             Next = null
    //             Label = null
    //             Usage = BufferUsage.CopySrc ||| BufferUsage.MapWrite
    //             Size = int64 fakebpr * int64 img.Size.Y
    //             MappedAtCreation = true
    //         }
    //         
    //     let tmpPtr = tmp.GetMappedRange(0L, tmp.Size)
    //     
    //     
    //     let fakeWidth = fakebpr / 4
    //     let dstVolume = NativeVolume<byte>(NativePtr.ofNativeInt tmpPtr, VolumeInfo(0L, V3l(fakeWidth, img.Size.Y, 4), V3l(4L, 4L*int64 fakeWidth, 1L)))
    //         
    //     NativeVolume.using img.Volume (fun srcVolume ->
    //        NativeVolume.copy srcVolume (dstVolume.SubVolume(V3l.Zero, V3l(img.Size.X, img.Size.Y, 4)))
    //     )
    //     tmp.Unmap()
    //     
    //     use enc = device.CreateCommandEncoder { Label = null; Next = null }
    //     let src : ImageCopyBuffer =
    //         {
    //             Layout =
    //                 {
    //                     Next = null
    //                     Offset = 0L
    //                     BytesPerRow = fakebpr
    //                     RowsPerImage = img.Size.Y
    //                 }
    //             Buffer = tmp
    //         }
    //     let dst : ImageCopyTexture =
    //         {
    //             Texture = tex
    //             Origin = { X = 0; Y = 0; Z = 0 }
    //             Aspect = TextureAspect.All
    //             MipLevel = 0
    //         }
    //         
    //     enc.CopyBufferToTexture(src, dst, { Width = img.Size.X; Height = img.Size.Y; DepthOrArrayLayers = 1 })
    //     
    //     use cmd = enc.Finish { Label = null; Next = null }
    //     device.Queue.Submit [| cmd |]
    //     device.Queue.Wait().Wait()
    //    
        
    
     
    
    let view = tex.CreateView TextureUsage.TextureBinding
        
    let sam =
        device.CreateSampler {
            Next = null
            Label = null
            AddressModeU = AddressMode.Repeat
            AddressModeV = AddressMode.Repeat
            AddressModeW = AddressMode.Repeat
            MagFilter = FilterMode.Linear
            MinFilter = FilterMode.Linear
            MipmapFilter = MipmapFilterMode.Linear
            LodMinClamp = 0.0f
            LodMaxClamp = 1000.0f
            Compare = CompareFunction.Undefined
            MaxAnisotropy = 1us
        }
    
    
    let bg =
        device.CreateBindGroup {
            Label = "Peggy"
            Layout = bgl
            Entries =
                [|
                    BindGroupEntry.UniformBuffer(0, ubo)
                    BindGroupEntry.TextureView(1, view)
                    BindGroupEntry.Sampler(2, sam)
                |]
        }
    
    let bundle = 
        use benc =
            device.CreateRenderBundleEncoder {
                Label = null
                ColorFormats = [| TextureFormat.BGRA8Unorm |]
                DepthStencilFormat = TextureFormat.Undefined
                SampleCount = 1
                DepthReadOnly = true
                StencilReadOnly = true
            }
        benc.SetPipeline pipeline
        benc.SetBindGroup(0, bg, [||])
        benc.SetVertexBuffer(0, pos, 0, pos.Size)
        benc.SetVertexBuffer(1, color, 0, color.Size)
        benc.SetVertexBuffer(2, tc, 0, tc.Size)
        benc.Draw(3, 1, 0, 0)
        benc.Finish {
            Label = null
        }
        
    let rand = RandomSystem()
    let sw = Stopwatch.StartNew()
    win |> Window.run (fun viewport ->
        
        use enc =
            device.CreateRenderBundleEncoder {
                Label = "Randall"
                ColorFormats = [| TextureFormat.BGRA8Unorm |]
                DepthStencilFormat = TextureFormat.Undefined
                SampleCount = 1
                DepthReadOnly = true
                StencilReadOnly = true
            }
            
        let _bundle = 
            enc.Finish {
                Label = "Randy"
            }
        
            
        use enc = device.CreateCommandEncoder { Label = "Conan"; Next = null }
        
        use colorView =
            surf.CurrentTexture.Texture.CreateView {
                Next = null
                Label = "Vernon"
                Format = TextureFormat.BGRA8Unorm
                Dimension = TextureViewDimension.D2D
                BaseMipLevel = 0
                MipLevelCount = 1
                BaseArrayLayer = 0
                ArrayLayerCount = 1
                Aspect = TextureAspect.Undefined
                Usage = cap.Usages
            }
        
        let colorAtt =
            {
                Next = null
                View = colorView
                DepthSlice = -1
                ResolveTarget = TextureView.Null
                LoadOp = LoadOp.Clear
                StoreOp = StoreOp.Store
                ClearValue = { R = 0.0; G = 0.0; B = 0.0; A = 1.0 }
            }
        
        let phi = 0.5 * sw.Elapsed.TotalSeconds
        let pos = 1.0 * V3d(cos phi, sin phi, 0.5).Normalized
        let view = CameraView.lookAt pos V3d.Zero V3d.OOI |> CameraView.viewTrafo
        let proj = Frustum.perspective 90.0 0.1 100.0 (float viewport.X / float viewport.Y) |> Frustum.projTrafo
        let vp = view * proj
        
        
        device.Queue.WriteBuffer(ubo, System.Span [| M44f vp.Forward |])
        use renc =
            enc.BeginRenderPass {
                Next = null
                Label = "Ronald"
                ColorAttachments = [| colorAtt |]
                DepthStencilAttachment = undefined
                OcclusionQuerySet = undefined
                TimestampWrites = undefined
            }
            
        renc.ExecuteBundles [| bundle |]
            
        renc.End()
        use cmd = 
            enc.Finish {
                Label = "Conrad"
            }
        device.Queue.Submit [| cmd |]
        device.Queue.Wait().Wait()
        surf.Present()
    )
    
    
    printfn "exit"
        
    0
