namespace WebGPU

open Microsoft.FSharp.NativeInterop
open System
open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open System.Threading.Tasks

[<AutoOpen>]
module Extensions =
    let str (str : string) =
        {
            Data = str
            Length = unativeint str.Length
        }


#nowarn "9"
[<AbstractClass; Sealed; Extension>]
type WebGPU private() =
    
    static member CreateInstance() =
        match RuntimeInformation.ProcessArchitecture with
        | Architecture.Wasm ->
            Instance(0n)
        | _ -> 
            let mutable options = Unchecked.defaultof<WebGPU.Raw.InstanceDescriptor>
            use ptr = fixed &options
            let handle = WebGPU.Raw.WebGPU.CreateInstance(ptr)
            Instance(handle)
    
    [<Extension>]
    static member RequestDeviceAsync(this : Adapter, options : DeviceDescriptor) =
        let tcs = TaskCompletionSource<Device>()
        this.RequestDevice(options, RequestDeviceCallback(fun status device message _ ->
            match status with
            | RequestDeviceStatus.Success -> tcs.SetResult device
            | _ -> tcs.SetException(Exception $"could not create device: {message}")
        ))
        tcs.Task

    [<Extension>]
    static member RequestAdapterAsync(this : Instance, options : RequestAdapterOptions) =
        let tcs = TaskCompletionSource<Adapter>()
        this.RequestAdapter(options, RequestAdapterCallback(fun status adapter message _ ->
            match status with
            | RequestAdapterStatus.Success -> tcs.SetResult adapter
            | _ -> tcs.SetException(Exception $"could not create adapter: {message}")
        ))
        tcs.Task
        
 