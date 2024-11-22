
open WebGPU
open Aardvark.Base
open System.Threading
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

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
        
    let run (render : unit -> unit) (win : nativeptr<WindowHandle>) =
        let glfw = glfw.Value
        glfw.ShowWindow(win)
        let mutable run = true
        glfw.SetWindowCloseCallback(win, GlfwCallbacks.WindowCloseCallback(fun _ -> run <- false; glfw.PostEmptyEvent())) |> ignore
        while run do
            glfw.WaitEvents()
            render()

[<EntryPoint>]
let main argv =
    //
    // let lib = NativeLibrary.Load "/Users/schorsch/Library/Application Support/Aardvark/Cache/Native/WebGPU/a86276e2-ce19-133f-c75b-f3f45da7d698/mac/ARM64/libwebgpu_dawn.dylib"    
    // printfn "%A" lib
    // //
    
    Aardvark.Init()
    let instance = WebGPU.CreateInstance()
    
    
    let win = Window.create()
    
    let surf = instance.CreateGLFWSurface(NativePtr.toNativeInt win)
    printfn "%A" surf.Handle
   
   
    
    
    let start =
        ThreadStart(fun () ->
            while true do
                instance.ProcessEvents()
        )
    let thread = Thread(start)
    thread.Start()

    printfn "%0A" instance.WGSLLanguageFeatures


    let adapter = 
        instance.RequestAdapterAsync({
            Next = null
            CompatibleSurface = Surface.Null
            PowerPreference = PowerPreference.HighPerformance
            BackendType = BackendType.Undefined
            ForceFallbackAdapter = false
            CompatibilityMode = false
        }).Result
        
    
    printfn "%A" adapter.Info
    
    
    let device = 
        adapter.RequestDeviceAsync({
            Next = null
            DebugOutput = true
            Label = "Devon"
            RequiredFeatures = adapter.Features.Features
            RequiredLimits = { Next = null; Limits = adapter.Limits.Limits }
            DefaultQueue = { Next = null; Label = "Quentin" }
        }).Result
    let queue = device.GetQueue()
        
    //     
    // let cap = adapter.GetFormatCapabilities(TextureFormat.RGBA8UnormSrgb)
    // printfn "%A" cap
    let cap = surf.GetCapabilities(adapter)
    printfn "%A" cap
    surf.Configure {
        Next = null
        Device = device
        Format = TextureFormat.BGRA8UnormSrgb
        Usage = cap.Usages
        ViewFormats = [| TextureFormat.BGRA8Unorm |]
        AlphaMode = CompositeAlphaMode.Opaque
        Width = 1024
        Height = 768
        PresentMode = PresentMode.Fifo
    }
        
    printfn "%A" surf.CurrentTexture.Status
    printfn "%A" surf.CurrentTexture.Texture.Handle
    
    let layout = 
        device.CreatePipelineLayout {
            Next = null
            Label = "Peter"
            BindGroupLayouts  =
                [|
                    device.CreateBindGroupLayout {
                        Next = null
                        Label = "Ben"
                        Entries =
                            [||]
                        
                    }
                |]
            ImmediateDataRangeByteSize = 128
            
        }
        
    
    
    let rand = RandomSystem()
    win |> Window.run (fun () ->
        
        use enc =
            device.CreateRenderBundleEncoder {
                Next = null
                Label = "Randall"
                ColorFormats = [| TextureFormat.BGRA8Unorm |]
                DepthStencilFormat = TextureFormat.Undefined
                SampleCount = 1
                DepthReadOnly = true
                StencilReadOnly = true
            }
            
        let bundle = 
            enc.Finish {
                Next = null
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
        
        let c = rand.UniformC3d()
        let colorAtt =
            {
                Next = null
                View = colorView
                DepthSlice = -1
                ResolveTarget = TextureView.Null
                LoadOp = LoadOp.Clear
                StoreOp = StoreOp.Store
                ClearValue = { R = c.R; G = c.G; B = c.B; A = 1.0 }
            }
        
        use renc =
            enc.BeginRenderPass {
                Next = null
                Label = "Ronald"
                ColorAttachments = [| colorAtt |]
                DepthStencilAttachment = undefined
                OcclusionQuerySet = undefined
                TimestampWrites = undefined
            }
        renc.End()
        use cmd = 
            enc.Finish {
                Next = null
                Label = "Conrad"
            }
        queue.Submit [| cmd |]
        queue.Wait().Wait()
        surf.Present()
    )
    
    
    printfn "exit"
        
    0
