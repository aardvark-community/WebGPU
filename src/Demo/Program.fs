open Aardvark.Rendering.WebGPU
open Demo

open Aardvark.Base
open WebGPU
open Aardvark.Application.Slim

module Shader =
    open FShade
    
    [<LocalSize(X = 8, Y = 8)>]
    let bla (img : Image2d<Formats.rgba8>) =
        compute {
            img.[V2i(0, 0)] <- V4f.IIII
        }
    
let writeRGBA() =
    Aardvark.Init()
    WebGPUShaderExtensions.ShaderCaching <- false
    WebGPUShaderExtensions.PrintShaders <- true
    let app = WebGPUApplication.Create(true).Result
    
    let sh = app.Device.CompileCompute Shader.bla
    let tex = app.Device.CreateTexture(TextureUsage.StorageBinding, TextureFormat.RGBA8Unorm, V2i(1024, 1024))
    
    sh.Run(V2i(128, 128), [
        "img", tex.CreateView(TextureUsage.StorageBinding) :> obj
    ]).Wait()


[<ReflectedDefinition>]
module AtomicQueue =
    
    open FShade
    
    let dequeueHeadIndex = 0
    let queueCount = 1
    let enqueueTailIndex = 2
    let offset = 3
    
    
    
    let invalid = 0xDEADBEEF
    
    [<GLSLIntrinsic("atomicAdd({0}, {1})")>]
    let atomicAdd (r : ref<int>) (e : int) : int = onlyInShaderCode ""
    
    [<GLSLIntrinsic("atomicCompSwap({0}, {1}, {2})")>]
    let atomicCompSwap (r : ref<int>) (cmp : int) (e : int) : int = onlyInShaderCode ""
    
    [<GLSLIntrinsic("atomicExchange({0}, {1})")>]
    let atomicExchange (r : ref<int>) (e : int) : int = onlyInShaderCode ""
    
    [<GLSLIntrinsic("atomicExchange({0}, {1})")>]
    let atomicWrite (r : ref<int>) (e : int) : unit = onlyInShaderCode ""
    
    [<GLSLIntrinsic("atomicAdd({0}, 0)")>]
    let atomicRead (r : ref<int>) : int = onlyInShaderCode ""
    
    type UniformScope with
        member x.QueueBuffer : int[] = uniform?StorageBuffer?QueueBuffer
        member x.QueueCapacity : int = uniform?QueueCapacity
        
    
    let tryEnqueue (queueOffset : int) (value : int) =
        let realEnqueueTailIndex = queueOffset + enqueueTailIndex
        let realQueueCount = queueOffset + queueCount
        let realOffset = queueOffset + offset
        
        let mutable r = -1
        let mutable t = invalid
        while t <> r do
            if t = invalid then
                t <- atomicRead &&uniform.QueueBuffer.[realEnqueueTailIndex]
            else
                r <- atomicCompSwap &&uniform.QueueBuffer.[realEnqueueTailIndex] t invalid
                if r <> t then
                    t <- r
                    r <- -1
        
        let ti = t % uniform.QueueCapacity
        if atomicRead &&uniform.QueueBuffer.[realQueueCount] >= uniform.QueueCapacity then
            atomicWrite &&uniform.QueueBuffer.[realEnqueueTailIndex] ti
            false
        else
            let newTail = ti+1
            atomicWrite &&uniform.QueueBuffer.[realOffset + ti] value
            atomicAdd &&uniform.QueueBuffer.[realQueueCount] 1 |> ignore
            atomicWrite &&uniform.QueueBuffer.[realEnqueueTailIndex] newTail
            true
 
    let tryDequeue (queueOffset : int) (value : ref<int>) =
        let realDequeueHeadIndex = queueOffset + dequeueHeadIndex
        let realQueueCount = queueOffset + queueCount
        let realOffset = queueOffset + offset
        
        let mutable r = -1
        let mutable h = uniform.QueueBuffer.[realDequeueHeadIndex]
        while h <> r do
            if h = invalid then
                h <- atomicRead &&uniform.QueueBuffer.[realDequeueHeadIndex]
            else
                r <- atomicCompSwap &&uniform.QueueBuffer.[realDequeueHeadIndex] h invalid
                if r <> h then
                    h <- r
                    r <- -1
        
        let hi = h % uniform.QueueCapacity
        if atomicRead &&uniform.QueueBuffer.[realQueueCount] = 0 then
            atomicWrite &&uniform.QueueBuffer.[realDequeueHeadIndex] hi
            false
        else
            let newHead = hi + 1
            value.Value <- atomicRead &&uniform.QueueBuffer.[realOffset + hi]
            atomicAdd &&uniform.QueueBuffer.[realQueueCount] -1 |> ignore
            atomicWrite &&uniform.QueueBuffer.[realDequeueHeadIndex] newHead
            true


    [<LocalSize(X = 1, Y = 1)>]
    let test (enqueue : int) (result : int[]) =
        compute {
            let id = getGlobalId().XY
            
            if enqueue = 0 then
                let mutable value = 0
                let mutable worked = false
                //while not worked do
                worked <- tryDequeue 0 &&value
                result.[id.X] <- value
            else
                let value = id.X
                let mutable worked = false
                //while not worked do
                worked <- tryEnqueue 0 value
        }

    let run() =
        Aardvark.Init()
        let app = WebGPUApplication.Create(true).Result
        WebGPUShaderExtensions.ShaderCaching <- false
        WebGPUShaderExtensions.PrintShaders <- true
        let device = app.Device
        let computer = device.CompileCompute test
        
        
        let capacity = 128
        let groups = 8
        
        
        let queueBuffer =
            device.CreateBuffer {
                Next = null
                Label = "QueueBuffer"
                Usage = BufferUsage.Storage
                Size = int64 sizeof<int> * int64 (capacity + 3)
                MappedAtCreation = false
            }
        
        let result =
            device.CreateBuffer {
                Next = null
                Label = "QueueBuffer"
                Usage = BufferUsage.Storage
                Size = int64 sizeof<int> * int64 groups * int64 computer.LocalSize.X
                MappedAtCreation = false
            }
            
        queueBuffer.Fill(0u).Wait()
        result.Fill(uint32 -1).Wait()
        
        computer.Run(V2i(groups, 1), [
            "QueueBuffer", queueBuffer :> obj
            "QueueCapacity", capacity
            "result", result
            "enqueue", 1
        ]).Wait()
        printfn "enqueued"
               
        computer.Run(V2i(groups, 1), [
            "QueueBuffer", queueBuffer :> obj
            "QueueCapacity", capacity
            "result", result
            "enqueue", 0
        ]).Wait()
        
        let arr = device.Download<int>(result).Result |> Array.sort
        printfn "%A" arr
                


[<EntryPoint>]
let main _argv =
    AtomicQueue.run()
    //writeRGBA()
    //ComputeRasterizerDemo.run()
    //RenderDemo.run()
        
    0
