namespace WebGPU.Raw

#nowarn "9"

type FutureWaitPool(instance : nativeint, threadCount : int) =
    
    let futures = new System.Collections.Concurrent.BlockingCollection<WebGPU.Raw.Future>()
    
    let run() =
        for f in futures.GetConsumingEnumerable() do
            let wait = WebGPU.Raw.FutureWaitInfo(f, 0)
            use ptr = fixed &wait
            let mutable status = WebGPU.Raw.WebGPU.InstanceWaitAny(instance, 1un, ptr, System.UInt64.MaxValue)
            if status <> WebGPU.WaitStatus.Success then
                eprintfn "wait for 0x%X failed" f.Id
            
    let threads =
        Array.init threadCount (fun _ ->
            let thread = System.Threading.Thread(System.Threading.ThreadStart(run), IsBackground = true)
            thread.Start()
            thread
        )
                
    member x.Add(f : WebGPU.Raw.Future) =
        futures.Add f
        
        
module Label =
    let table = System.Collections.Concurrent.ConcurrentDictionary<string, string>()
    let rx = System.Text.RegularExpressions.Regex @"\$\$\$[^\$]+\$\$\$"
    
    let randomString() =
        let g = System.Guid.NewGuid()
        let str = g.ToByteArray() |> System.Convert.ToBase64String
        $"$$${str}$$$"
    
    let processMessage (msg : string) =
        #if DEBUG
        rx.Replace(msg, fun m ->
            match table.TryGetValue m.Value with
            | (true, sf) ->
                sf
            | _ ->
                m.Value
        )
        #else
        msg
        #endif
    
    let nolabel() =
        #if DEBUG
        let f = System.Diagnostics.StackTrace(1)
        let id = randomString()
        table.TryAdd(id, string f) |> ignore
        id
        #else
        null
        #endif
        
        