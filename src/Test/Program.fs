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
        
        Log.start "Features"
        
        let mutable unknown = ResizeArray()
        for f in adapter.Features.Features do
            if System.Enum.IsDefined(typeof<FeatureName>, f) then
                Log.line "%s" (string f)
            else
                unknown.Add (sprintf "0x%X" (int f))
        
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