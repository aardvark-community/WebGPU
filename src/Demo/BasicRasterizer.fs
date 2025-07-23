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
        member x.TriangleCount : int = uniform?TriangleCount
        member x.Offset : V2i = uniform?Offset
        member x.Size : V2i = uniform?Size
        member x.ViewportSize : V2i = uniform?ViewportSize
        member x.ModelViewTrafo : M44f = uniform?ModelViewTrafo
        member x.ProjTrafo : M44f = uniform?ProjTrafo
        
    [<LocalSize(X = 8, Y = 8)>]
    let rasterize (vertices : V4f[]) (normals : V4f[]) (colors : uint32[]) (color : UIntImage2d<Formats.r32ui>) (depth : int[]) =
        compute {
            
            let id = getGlobalId().XY
            if id.X < uniform.Size.X && id.Y < uniform.Size.Y then
                let id = id + uniform.Offset
                for tri in 0 .. uniform.TriangleCount - 1 do
                    let triOff = 3*tri
                    let vp0 = uniform.ModelViewTrafo * vertices.[triOff + 0]
                    let vp1 = uniform.ModelViewTrafo * vertices.[triOff + 1]
                    let vp2 = uniform.ModelViewTrafo * vertices.[triOff + 2]
                    
                    let p0 = uniform.ProjTrafo * vp0
                    let p1 = uniform.ProjTrafo * vp1
                    let p2 = uniform.ProjTrafo * vp2
                    
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
                    let c = (1.0f - a - b)
                    
                    if a >= 0.0f && b >= 0.0f && c >= 0.0f && a <= 1.0f && b <= 1.0f && c <= 1.0f then
                        let pos = a*p0 + b*p1 + c*p2
                        if pos.Z >= -pos.W && pos.Z <= pos.W then
                            let col0 = unpackUnorm4x8(colors.[triOff + 0]).ZYXW
                            let col1 = unpackUnorm4x8(colors.[triOff + 1]).ZYXW
                            let col2 = unpackUnorm4x8(colors.[triOff + 2]).ZYXW
                            
                            let n0 = normals.[triOff + 0].XYZ
                            let n1 = normals.[triOff + 1].XYZ
                            let n2 = normals.[triOff + 2].XYZ
                            
                            let projected = pos.XYZ / pos.W
                            let vp = a*vp0 + b*vp1 + c*vp2
                            let n = a*n0 + b*n1 + c*n2
                            let col = a*col0 + b*col1 + c*col2
                            
                            let vn = uniform.ModelViewTrafo * V4f(n, 0.0f) |> Vec.xyz |> Vec.normalize
                            
                            let light = V3f.Zero
                            let lightDir = Vec.normalize (light - vp.XYZ)
                            let diffuse = Vec.dot lightDir vn |> abs
                            
                            let light = 0.2f + 0.8f*diffuse
                            
                            let c = V4f(col.XYZ * light, 1.0f)
                            
                            let di = id.X + id.Y * uniform.ViewportSize.X
                            let newDepth = projected.Z * 16777215.0f |> int
                            if newDepth <= depth.[di] then
                                depth.[di] <- newDepth
                                color.[id] <- V4ui(packUnorm4x8 c)
                           
            
        }


let compile (device : Device) : Rasterizer =
    
    let shader = device.CompileCompute Shader.rasterize
    
    fun (input : RasterizerInput) ->
        
        task {
            let size = V2i(input.ColorTexture.Width, input.ColorTexture.Height)
            let color = input.ColorTexture
            let depth = input.DepthBuffer
            
            let vertexCount = input.Positions.Size / int64 sizeof<V4f> |> int
            let triangleCount = vertexCount / 3
            
            use colorView = color.CreateView(TextureUsage.StorageBinding ||| TextureUsage.TextureBinding)
            let groups = V2i(ceilDiv size.X shader.LocalSize.X, ceilDiv size.Y shader.LocalSize.Y)
            
            do! color.Clear(0xFF000000u)
            do! depth.Fill(16777215)
            do! shader.Run(V3i(groups, 1), [
                "TriangleCount", triangleCount :> obj
                "vertices", input.Positions
                "colors", input.Colors
                "normals", input.Normals
                "color", colorView
                "depth", depth
                "Offset", V2i.Zero 
                "Size", size
                "ViewportSize", size
                "ModelViewTrafo", M44f input.ModelViewTrafo.Forward
                "ProjTrafo", M44f input.ProjTrafo.Forward
            ])
        }
    
        