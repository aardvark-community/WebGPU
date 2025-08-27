namespace WebGPU.Raw

#nowarn "9"

type internal FutureWaitPool(instance : nativeint) =
    let futures = new System.Collections.Concurrent.BlockingCollection<struct(WebGPU.Raw.Future * System.Diagnostics.StackTrace)>()
    
    let run() =
        for struct(f, stack) in futures.GetConsumingEnumerable() do
            let wait = WebGPU.Raw.FutureWaitInfo(f, 0)
            use ptr = fixed &wait
            let mutable status = WebGPU.Raw.WebGPU.InstanceWaitAny(instance, 1un, ptr, System.UInt64.MaxValue)
            if status <> WebGPU.WaitStatus.Success then
                eprintfn "wait for 0x%X failed: %A" f.Id status
                if WebGPU.WebGPUConfig.captureStackTraces then eprintfn "stack: %A" stack
            
    let threads =
        Array.init WebGPU.WebGPUConfig.futureWaitThreads (fun _ ->
            let thread = System.Threading.Thread(System.Threading.ThreadStart(run), IsBackground = true)
            thread.Start()
            thread
        )
                
    member x.Add(f : WebGPU.Raw.Future) =
        let stack =
            if WebGPU.WebGPUConfig.captureStackTraces then System.Diagnostics.StackTrace(1)
            else null
        futures.Add(struct(f, stack))
        
        
module Label =
    let mutable private currentId = 0
    let private table = System.Collections.Concurrent.ConcurrentDictionary<string, int * string>()
    let private rx = System.Text.RegularExpressions.Regex @"\$\$\$[^\$]+\$\$\$"
    
    let private randomString() =
        let g = System.Guid.NewGuid()
        let str = g.ToByteArray() |> System.Convert.ToBase64String
        $"$$${str}$$$"
    
    let processMessage (msg : string) =
        if WebGPU.WebGPUConfig.captureStackTraces then
            let stacks = System.Collections.Generic.Dictionary<string, string>()
            
            let newMsg = 
                rx.Replace(msg, fun m ->
                    match table.TryGetValue m.Value with
                    | (true, (index, sf)) ->
                        let name = sprintf "%03d" index
                        stacks.[name] <- sf
                        name
                    | _ ->
                        m.Value
                )
                
            let res = System.Text.StringBuilder()
            res.AppendLine newMsg|> ignore
            
            for KeyValue(name, stack) in stacks do
                res.AppendLine $"--- stack for {name} ---" |> ignore
                res.AppendLine stack |> ignore
            res.ToString()
        else
            msg
    
    let nolabel() =
        if WebGPU.WebGPUConfig.captureStackTraces then
            let index = System.Threading.Interlocked.Increment &currentId
            let f = System.Diagnostics.StackTrace(1, true)
            let fStr = string f
            
            
            
            
            let id = randomString()
            table.TryAdd(id, (index, fStr)) |> ignore
            id
        else
            null
        
        