namespace WebGPU

open Microsoft.FSharp.NativeInterop
open System
open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open System.Threading.Tasks
open Aardvark.Base
open System.Text.RegularExpressions
open WebGPU
// [<AutoOpen>]
// module Extensions =
//     let str (str : string) =
//         {
//             Data = str
//             Length = unativeint str.Length
//         }

type FrontendDeviceDescriptor = 
    {
        Next : IDeviceDescriptorExtension
        Label : string
        DebugOutput : bool
        RequiredFeatures : array<FeatureName>
        RequiredLimits : RequiredLimits
        DefaultQueue : QueueDescriptor
    }


#nowarn "9"
[<AbstractClass; Sealed; Extension>]
type WebGPU private() =
    static do
        match RuntimeInformation.ProcessArchitecture with
        | Architecture.Wasm -> ()
        | _ ->
            Aardvark.LoadLibrary(typeof<WebGPU>.Assembly, "webgpu_dawn") |> ignore
            let ptr = Aardvark.LoadLibrary(typeof<WebGPU>.Assembly, "libglfw.3.dylib")
            printfn "%A" ptr
        
    static let enumRx = Regex @"([a-zA-Z_0-9]+)::"
    
    static member CreateInstance() =
        match RuntimeInformation.ProcessArchitecture with
        | Architecture.Wasm ->
            new Instance(0n)
        | _ -> 
            let mutable options = Unchecked.defaultof<WebGPU.Raw.InstanceDescriptor>
            use ptr = fixed &options
            let handle = WebGPU.Raw.WebGPU.CreateInstance(ptr)
            new Instance(handle)
    
    [<Extension>]
    static member CreateGLFWSurface(this : Instance, window : nativeint) =
        let handle = WebGPU.Raw.WebGPU.InstanceCreateGLFWSurface(this.Handle, window)
        new Surface(handle)
        
    [<Extension>]
    static member RequestDeviceAsync(this : Adapter, options : FrontendDeviceDescriptor) =
        let tcs = TaskCompletionSource<Device>()
        
        let err =
            if options.DebugOutput then
                {
                    UncapturedErrorCallbackInfo.Next = null
                    Callback =
                        ErrorCallback(fun _disp typ message ->
                            let t = System.Diagnostics.StackTrace(4) |> string
                            let message = enumRx.Replace(message, "$1.") + "\n" + t
                            let lines = message.Split('\n')
                            Report.ErrorNoPrefix($"{typ} ERROR:")
                            for l in lines do
                                Report.ErrorNoPrefix($"  {l}")
                        )
                }
            else
                UncapturedErrorCallbackInfo.Null
        
        let realOptions =
            {
                Next = options.Next
                Label = options.Label
                RequiredFeatures = options.RequiredFeatures
                RequiredLimits = options.RequiredLimits
                DefaultQueue = options.DefaultQueue
                DeviceLostCallbackInfo = DeviceLostCallbackInfo.Null
                UncapturedErrorCallbackInfo = err
                DeviceLostCallbackInfo2 = DeviceLostCallbackInfo2.Null
                UncapturedErrorCallbackInfo2 = UncapturedErrorCallbackInfo2.Null
            }
        
        this.RequestDevice(realOptions, RequestDeviceCallback(fun disp status device message ->
            disp.Dispose()
            match status with
            | RequestDeviceStatus.Success -> tcs.SetResult device
            | _ -> tcs.SetException(Exception $"could not create device: {message}")
        ))
        task {
            let! dev = tcs.Task
            
            if options.DebugOutput then
                dev.SetLoggingCallback(LoggingCallback(fun _ t str ->
                    let lines = str.Split("\n")
                    for line in lines do 
                        match t with
                        | LoggingType.Error -> Report.ErrorNoPrefix("{0}", line)
                        | LoggingType.Warning -> Report.WarnNoPrefix("{0}", line)
                        | LoggingType.Info -> Report.Line("{0}", line)
                        | _ -> Report.Line(4, "{0}", line)
                ))
            
            return dev   
        }

    [<Extension>]
    static member GetFormatCapabilities(this : Adapter, format : TextureFormat) =
         let mutable res = Unchecked.defaultof<WebGPU.Raw.FormatCapabilities>
         use ptr = fixed &res
         let status = WebGPU.Raw.WebGPU.AdapterGetFormatCapabilities(this.Handle, format, ptr)
         FormatCapabilities.Read(&res)
         
    [<Extension>]
    static member GetCapabilities(this : Surface, adapter : Adapter) =
        let mutable res = Unchecked.defaultof<WebGPU.Raw.SurfaceCapabilities>
        use ptr = fixed &res
        let status = WebGPU.Raw.WebGPU.SurfaceGetCapabilities(this.Handle, adapter.Handle, ptr)
        SurfaceCapabilities.Read(&res)
    
    [<Extension>]
    static member RequestAdapterAsync(this : Instance, options : RequestAdapterOptions) =
        let tcs = TaskCompletionSource<Adapter>()
        this.RequestAdapter(options, RequestAdapterCallback(fun disp status adapter message ->
            disp.Dispose()
            match status with
            | RequestAdapterStatus.Success -> tcs.SetResult adapter
            | _ -> tcs.SetException(Exception $"could not create adapter: {message}")
        ))
        tcs.Task
   
    [<Extension>]
    static member GetWGSLLanguageFeatures(this : Instance) =
        let cnt = WebGPU.Raw.WebGPU.InstanceEnumerateWGSLLanguageFeatures(this.Handle, NativePtr.ofNativeInt 0n)
        let arr = Array.zeroCreate (int cnt)
        use ptr = fixed arr
        WebGPU.Raw.WebGPU.InstanceEnumerateWGSLLanguageFeatures(this.Handle, ptr) |> ignore
        arr
        
    [<Extension>]
    static member Mapped<'r>(buffer : Buffer, instance : Instance,  mode : MapMode, offset : nativeint, size : nativeint, action : nativeint -> 'r) =
        match buffer.GetMapState() with
        | BufferMapState.Mapped ->
            let ptr =
                match mode with
                | MapMode.Write -> buffer.GetMappedRange(unativeint offset, unativeint size)
                | _ -> buffer.GetConstMappedRange(unativeint offset, unativeint size)
            try action ptr |> Task.FromResult
            finally buffer.Unmap()
        | _ ->
            let tcs = TaskCompletionSource<_>()
            
            let info : BufferMapCallbackInfo2 =
                {
                    Next = null
                    Mode = CallbackMode.AllowProcessEvents
                    Callback = BufferMapCallback2(fun d status msg ->
                        d.Dispose()
                        match status with
                        | MapAsyncStatus.Success ->
                            let ptr = 
                                match mode with
                                | MapMode.Write -> buffer.GetMappedRange(unativeint offset, unativeint size)
                                | _ -> buffer.GetConstMappedRange(unativeint offset, unativeint size)
                            let res = action ptr
                            buffer.Unmap()
                            tcs.SetResult res
                        | s ->
                            tcs.SetException (Exception (sprintf "could not map buffer: %A" s))  
                    )
                }
            
            let f = buffer.MapAsync2(mode, unativeint offset, unativeint size, info)
            // let s = instance.WaitAny([|{ FutureWaitInfo.Future = f; Completed = false }|], System.UInt64.MaxValue)
            // printfn "STATUS: %A" s
            // buffer.MapAsync(mode, unativeint offset, unativeint size, BufferMapCallback (fun d s ->
            //     d.Dispose()
            //     match s with
            //     | BufferMapAsyncStatus.Success ->
            //         let ptr = buffer.GetMappedRange(unativeint offset, unativeint size)
            //         let res = action ptr
            //         buffer.Unmap()
            //         tcs.SetResult res
            //     | s ->
            //         tcs.SetException (Exception (sprintf "could not map buffer: %A" s))    
            // ))
            
            tcs.Task
    [<Extension>]
    static member Mapped<'r>(buffer : Buffer, instance, mode : MapMode, action : nativeint -> 'r) =
        buffer.Mapped(instance, mode, 0n, nativeint (buffer.GetSize()), action)
    
    [<Extension>]
    static member Wait(this : Queue) =
        let tcs = TaskCompletionSource()
        this.OnSubmittedWorkDone2 {
            QueueWorkDoneCallbackInfo2.Next = null
            Mode = CallbackMode.AllowProcessEvents
            Callback =
                QueueWorkDoneCallback2(fun d s ->
                    match s with
                    | QueueWorkDoneStatus.Success -> tcs.SetResult()
                    | _ -> tcs.SetException(Exception (sprintf "could not wait for queue: %A" s))
                )
        } |> ignore
        tcs.Task
        
[<AutoOpen>]
module ``F# Extensions`` =
    
    type Instance with
        member this.WGSLLanguageFeatures = WebGPU.GetWGSLLanguageFeatures(this)
        
    let inline RenderBundleEncoderDescriptor (e : RenderBundleEncoderDescriptor) = e
        
    let inline undefined<'a when 'a : (static member Null : 'a)> : 'a =
        'a.Null
        
    // type Adapter with
    //     member this.Info = WebGPU.GetInfo(this)
    //     member this.Limits = WebGPU.GetLimits(this)
    //     
    // type Device with
    //     member this.Queue = this.GetQueue()
    //     member this.Limits = WebGPU.GetLimits(this)