namespace WebGPU

open System
open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open System.Threading.Tasks

#nowarn "9"
[<AbstractClass; Sealed>]
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
