open Aardvark.Base
open WebGPU


let run() =
    task {
        use instance = WebGPU.CreateInstance()
        use! adapter = instance.CreateAdapter()
        
        Log.start "Adapter"
        Log.line "Vendor:            %s" adapter.Info.Vendor
        Log.line "Architecture:      %s" adapter.Info.Architecture
        Log.line "Device:            %s" adapter.Info.Device
        Log.line "Description:       %s" adapter.Info.Description
        Log.line "BackendType:       %A" adapter.Info.BackendType
        Log.line "AdapterType:       %A" adapter.Info.AdapterType
        Log.line "VendorID:          0x%08X" adapter.Info.VendorID
        Log.line "DeviceID:          0x%08X" adapter.Info.DeviceID
        Log.line "CompatibilityMode: %b" adapter.Info.CompatibilityMode
        
        Log.start "Features"
        
        let mutable unknown = ResizeArray()
        for f in adapter.Features.Features do
            let str = string f
            match System.Int32.TryParse str with
            | (true, v) -> unknown.Add (sprintf "0x%X" v)
            | _ -> Log.line "%s" str
        
        if unknown.Count > 0 then
            Log.line "+%d Unknown Features" unknown.Count
        
        Log.stop()
        
        Log.stop()
        
    }
    
    
[<EntryPoint>]
let main _args =
    Aardvark.Init()
    run().Result
    0