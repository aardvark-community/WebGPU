namespace Aardvark.Rendering.WebGPU

open System.Runtime.CompilerServices
open WebGPU
open Aardvark.Base
open Aardvark.Rendering

type FramebufferSignature(device : Device, samples : int, attachments : Map<int, AttachmentSignature>, depthAttachment : option<TextureFormat>) =
    let mutable runtime : IRuntime = Unchecked.defaultof<_>
    
    member x.Device = device
    
        
    member this.ColorAttachments = attachments
    member this.DepthStencilAttachment = depthAttachment
    member this.Dispose() = ()
    member this.LayerCount = 1
    member this.PerLayerUniforms = Set.empty
    member this.Samples = samples
    
    interface IFramebufferSignature with
        member this.ColorAttachments = attachments
        member this.DepthStencilAttachment = depthAttachment
        member this.Dispose() = ()
        member this.LayerCount = 1
        member this.PerLayerUniforms = Set.empty
        member this.Runtime = device.Runtime
        member this.Samples = samples

[<AbstractClass; Sealed>]
type DeviceFramebufferSignatureExtensions private() =
    [<Extension>]
    static member CreateFramebufferSignature (device : Device, samples : int, attachments : Map<int, AttachmentSignature>, depthAttachment : option<TextureFormat>) =
        new FramebufferSignature(device, samples, attachments, depthAttachment)

