module Demo.BasicRasterizer

open Aardvark.Base
open WebGPU
open Aardvark.Rendering.WebGPU


let ceilDiv (a : int) (b : int) =
    if a % b = 0 then a / b
    else 1 + a / b


module Shader =
    open FShade
    
    type UniformScope with
        member x.Offset : V2i = uniform?Offset
        member x.Size : V2i = uniform?Size
        member x.ViewportSize : V2i = uniform?ViewportSize
        member x.ModelViewProjTrafo : M44f = uniform?ModelViewProjTrafo
        
    [<LocalSize(X = 8, Y = 8)>]
    let rasterize (vertices : V4f[]) (colors : uint32[]) (color : UIntImage2d<Formats.r32ui>) (depth : Image2d<Formats.r32f>) =
        compute {
            let tri = getGlobalId().Z
            let triOff = 3*tri
            let id = getGlobalId().XY
            if id.X < uniform.Size.X && id.Y < uniform.Size.Y then
                let id = id + uniform.Offset
                
                let p0 = uniform.ModelViewProjTrafo * vertices.[triOff + 0]
                let p1 = uniform.ModelViewProjTrafo * vertices.[triOff + 1]
                let p2 = uniform.ModelViewProjTrafo * vertices.[triOff + 2]
                
                let col0 = unpackUnorm4x8(colors.[triOff + 0]).ZYXW
                let col1 = unpackUnorm4x8(colors.[triOff + 1]).ZYXW
                let col2 = unpackUnorm4x8(colors.[triOff + 2]).ZYXW
                
                let tc = (V2f id + V2f.Half) / V2f uniform.ViewportSize
                let ndc = 2.0f * tc - V2f.II
                // (a*p0.XY + b*p1.XY + c*p2.XY) / (a*p0.W + b*p1.W + c*p2.W) = ndc
                // (a*p0.XY + b*p1.XY + c*p2.XY) = ndc * (a*p0.W + b*p1.W + c*p2.W)
                
                
                // a*p0.XY + b*p1.XY + c*p2.XY = a*ndc*p0.W + b*ndc*p1.W + c*ndc*p2.W
                // a*(p0.XY - ndc*p0.W) + b*(p1.XY - ndc*p1.W) + c*(p2.XY - ndc*p2.W) = 0
                
                let f0 = p0.XY - ndc*p0.W
                let f1 = p1.XY - ndc*p1.W
                let f2 = p2.XY - ndc*p2.W
                
                // a*f0 + b*f1 + c*f2 = 0
                // a + b + c = 1
                
                // a*f0 + b*f1 + f2-a*f2-b*f2 = 0
                // a*(f0 - f2) + b*(f1 - f2) = -f2
                
                let c0 = f0 - f2
                let c1 = f1 - f2
                
                let det = c0.X*c1.Y - c0.Y*c1.X
                let r0 = V2f(c1.Y / det, -c1.X / det)
                let r1 = V2f(-c0.Y / det, c0.X / det)
                
                let a = -Vec.dot r0 f2
                let b = -Vec.dot r1 f2
                let c = 1.0f - a - b
                
                if a >= 0.0f && b >= 0.0f && c >= 0.0f && a <= 1.0f && b <= 1.0f && c <= 1.0f then
                    let pos = a*p0 + b*p1 + c*p2
                    if pos.X >= -pos.W && pos.X <= pos.W &&
                       pos.Y >= -pos.W && pos.Y <= pos.W &&
                       pos.Z >= -pos.W && pos.Z <= pos.W then
                            let c = a*col0 + b*col1 + c*col2
                            let projected = pos.XYZ / pos.W
                            
                            //let c = V4d.IOOI
                            color.[id] <- V4ui(packUnorm4x8 c)
                            depth.[id] <- V4f(projected.Z)
                        
                
                
                // a*(f0x - f2x) + b*(f1x - f2x) = -f2x
                // a*(f0y - f2y) + b*(f1y - f2y) = -f2y
                
                // a*(f0x - f2x)*(f0y - f2y) + b*(f1x - f2x)*(f0y - f2y) = -f2x*(f0y - f2y)
                // -a*(f0x - f2x)*(f0y - f2y) - b*(f0x - f2x)*(f1y - f2y) = +f2y*(f0x - f2x)
                
                // b*[(f1x - f2x)*(f0y - f2y) - (f0x - f2x)*(f1y - f2y)] = [f2y*(f0x - f2x) - f2x*(f0y - f2y)]
                // b*[(f1x - f2x)*(f0y - f2y) - (f0x - f2x)*(f1y - f2y)] = [f2y*(f0x - f2x) - f2x*(f0y - f2y)]
                ()
            
        }


let compile (device : Device) : Rasterizer =
    
    let shader = device.CompileCompute Shader.rasterize
    
    fun (input : RasterizerInput) ->
        
        task {
            let size = V2i(input.ColorTexture.Width, input.ColorTexture.Height)
            let color = input.ColorTexture
            let depth = input.DepthTexture
            
            let vertexCount = input.Positions.Size / int64 sizeof<V4f> |> int
            let triangleCount = vertexCount / 3
            
            use colorView = color.CreateView(TextureUsage.StorageBinding ||| TextureUsage.TextureBinding)
            use depthView = depth.CreateView(TextureUsage.StorageBinding)
            let groups = V2i(ceilDiv size.X shader.LocalSize.X, ceilDiv size.Y shader.LocalSize.Y)
            
            do! color.Clear(0xFF000000u)
            do! shader.Run(V3i(groups, triangleCount), [
                "vertices", input.Positions :> obj
                "colors", input.Colors
                "Offset", V2i.Zero 
                "Size", size
                "ViewportSize", size
                "color", colorView
                "depth", depthView
                "ModelViewProjTrafo", M44f input.ModelViewProj.Forward
            ])
        }
    
        