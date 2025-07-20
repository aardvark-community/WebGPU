namespace Aardvark.Rendering.WebGPU

open WebGPU
open Aardvark.Base
open Aardvark.Rendering

type Framebuffer (signature : IFramebufferSignature, size : V2i, colorAttachments : TextureView[], depth : option<TextureView>) =
    member x.ColorAttachments = colorAttachments
    member x.DepthStencil = depth
    
    member x.Dispose() =
        ()
    
    interface System.IDisposable with
        member x.Dispose() = x.Dispose()
    
    interface IFramebuffer with
        member x.Handle = 0UL
        member x.Signature = signature
        member x.Attachments = Map.empty
        member x.Size = size