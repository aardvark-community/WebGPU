module Demo.BinRasterizer

open Aardvark.Base
open WebGPU
open Aardvark.Rendering.WebGPU


let ceilDiv (a : int) (b : int) =
    if a % b = 0 then a / b
    else 1 + a / b


module Heat = 
     let heatMapColors =
         let fromInt (i : int) =
             C4b(
                 byte ((i >>> 16) &&& 0xFF),
                 byte ((i >>> 8) &&& 0xFF),
                 byte (i &&& 0xFF),
                 255uy
             ).ToC4f().ToV4f()

         Array.map fromInt [|
             0x1639fa
             0x2050fa
             0x3275fb
             0x459afa
             0x55bdfb
             0x67e1fc
             0x72f9f4
             0x72f8d3
             0x72f7ad
             0x71f787
             0x71f55f
             0x70f538
             0x74f530
             0x86f631
             0x9ff633
             0xbbf735
             0xd9f938
             0xf7fa3b
             0xfae238
             0xf4be31
             0xf29c2d
             0xee7627
             0xec5223
             0xeb3b22
         |]

     [<ReflectedDefinition>]
     let heat (tc : float32) =
         let tc = clamp 0.0f 1.0f tc
         let fid = tc * float32 24 - 0.5f

         let id = int (floor fid)
         if id < 0 then 
             heatMapColors.[0]
         elif id >= 24 - 1 then
             heatMapColors.[24 - 1]
         else
             let c0 = heatMapColors.[id]
             let c1 = heatMapColors.[id + 1]
             let t = fid - float32 id
             (c0 * (1.0f - t) + c1 * t)

module Shader =
    open FShade
    
    [<Literal>]
    let binSize = 128
    
    //[<Literal>]
    //let binLength = 4096
    //
    //[<Literal>]
    //let doubleBinLength = 8192
    //
    //[<Literal>]
    //let halfBinLength = 2048
    
    type UniformScope with
        member x.BinCount : V2i = uniform?BinCount
        member x.ViewportSize : V2i = uniform?ViewportSize
        member x.TriangleCount : int = uniform?TriangleCount
        member x.TriangleChunkSize : int = uniform?TriangleChunkSize
        member x.TriangleOffset : int = uniform?TriangleOffset
        member x.ModelViewTrafo : M44f = uniform?ModelViewTrafo
        member x.ProjTrafo : M44f = uniform?ProjTrafo
        member x.VertexCount : int = uniform?VertexCount
        member x.TotalBinCount : int = uniform?TotalBinCount
        member x.BinIdOffset : int = uniform?BinIdOffset
        member x.BinSize : int = uniform?BinSize
        
    let plane (i : int) (bMin : V3f) (bMax : V3f) =
        match i with
        | 0 -> V4f(1.0f, 0.0f, 0.0f, -bMax.X)
        | 1 -> V4f(-1.0f, 0.0f, 0.0f, bMin.X)
        | 2 -> V4f(0.0f, 1.0f, 0.0f, -bMax.Y)
        | 3 -> V4f(0.0f, -1.0f, 0.0f, bMin.Y)
        | 4 -> V4f(0.0f, 0.0f, 1.0f, -bMax.Z)
        | _ -> V4f(0.0f, 0.0f, -1.0f, bMin.Z)
       
    
    let boxLine (bMin : V3f) (bMax : V3f) (p0 : V4f) (p1 : V4f) =

        let mutable found = false
        let mutable pi = 0
        while pi < 6 && not found do
            let pl = plane pi bMin bMax
            
            let t = -Vec.dot pl p0 / Vec.dot pl (p1 - p0)
            if t >= 0.0f && t <= 1.0f then
                let pt = p0 + t * (p1 - p0)
                
                let mutable inside = true
                for oi in 0 .. 5 do
                    if oi <> pi then
                        let h = Vec.dot pt (plane oi bMin bMax)
                        if h >= 0.0f then inside <- false
                        
                        
                if inside then
                    found <- true
            pi <- pi + 1
        
        found
        
    [<ReflectedDefinition>]
    let triangleRay3d (p0 : V3f) (p1 : V3f) (p2 : V3f) (o : V3f) (d : V3f) (uv : ref<V2f>) =

        let u = p1 - p0
        let v = p2 - p0
        let k = o - p0
        // p0 + l*u + m*v = o + t*d
        // l*u + m*v - t*d = k

        let mutable r0 = V3f(u.X, v.X, -d.X)
        let mutable r1 = V3f(u.Y, v.Y, -d.Y)
        let mutable r2 = V3f(u.Z, v.Z, -d.Z)
        let mutable rhs = k


        if abs r1.X > abs r0.X then
            let t = r0
            r0 <- r1
            r1 <- t
            rhs <- rhs.YXZ

        if abs r2.X > abs r0.X then
            let t = r0
            r0 <- r2
            r2 <- t
            rhs <- rhs.ZYX

        r1 <- r1 - (r1.X/r0.X) * r0
        r2 <- r2 - (r2.X/r0.X) * r0


        if abs r2.Y > abs r1.Y then
            let t = r1
            r1 <- r2
            r2 <- t
            rhs <- rhs.XZY

        r2 <- r2 - (r2.Y/r1.Y)*r1

        let t = rhs.Z / r2.Z
        if t >= 0.0f then
            let m = (rhs.Y - t*r1.Z) / r1.Y
            let l = (rhs.X - r0.Y*m - r0.Z*t) / r0.X
            if m >= 0.0f && l >= 0.0f && l + m <= 1.0f then
                uv.Value <- V2f(l, m)
                true
            else
                false
        else
            false





    let triangleRay (p0 : V4f) (p1 : V4f) (p2 : V4f) (o : V3f) (d : V3f) =
        
        
        
        
        // u := p1 - p0
        // v := p2 - p0
        // (p0.XYZ + l*u.XYZ + m*v.XYZ) / (p0.W + l*u.W + m*v.W) = o + t*d
        // p0.XYZ + l*u.XYZ + m*v.XYZ = (o + t*d) * (p0.W + l*u.W + m*v.W)
        
        
        // 
        
        
        // p0.XYZ + l*u.XYZ + m*v.XYZ = o*p0.W + l*o*u.W + m*o*v.W + t*d*p0.W + l*t*d*u.W + m*t*d*v.W
        
        // 0 =  t*d*p0.W 
        
        // (p0.XYZ - o*p0.W) + l*(u.XYZ - o*u.W - t*d*u.W) + m*(v.XYZ - o*v.W - t*d*v.W) - t*d*p0.W = 0
        
        
        
        // (p0.X - o.X*p0.W) + l*(u.X - o.X*u.W - t*d.X*u.W) + m*(v.X - o.X*v.W - t*d.X*v.W) - t*d.X*p0.W = 0
        // (p0.Y - o.Y*p0.W) + l*(u.Y - o.Y*u.W - t*d.Y*u.W) + m*(v.Y - o.Y*v.W - t*d.Y*v.W) - t*d.Y*p0.W = 0
        // (p0.Z - o.Z*p0.W) + l*(u.Z - o.Z*u.W - t*d.Z*u.W) + m*(v.Z - o.Z*v.W - t*d.Z*v.W) - t*d.Z*p0.W = 0
        
        
        // A[i] = p0[i] - o[i]*p0.W
        // B[i] = u[i] - o[i]*u.W
        // C[i] = d[i]*u.W
        // D[i] = v[i] - o[i]*v.W
        // E[i] = d[i]*v.W
        // F[i] = d[i]*p0.W
        
        // A1 + l*(B1 - t*C1) + m*(D1 - t*E1) - t*F1 = 0
        // A2 + l*(B2 - t*C2) + m*(D2 - t*E2) - t*F2 = 0
        // A3 + l*(B3 - t*C3) + m*(D3 - t*E3) - t*F3 = 0
        
        
        
        // A1*(B2 - t*C2) + l*(B1 - t*C1)*(B2 - t*C2) + m*(D1 - t*E1)*(B2 - t*C2) - t*F1*(B2 - t*C2) = 0
        // -A2*(B1 - t*C1) - l*(B1 - t*C1)*(B2 - t*C2) - m*(B1 - t*C1)*(D2 - t*E2) + t*F2*(B1 - t*C1) = 0
        
        
        // A1*(B2 - t*C2) + l*(B1 - t*C1)*(B2 - t*C2) + m*(D1 - t*E1)*(B2 - t*C2) - t*F1*(B2 - t*C2) = 0
        // -A2*(B1 - t*C1) - l*(B1 - t*C1)*(B2 - t*C2) - m*(B1 - t*C1)*(D2 - t*E2) + t*F2*(B1 - t*C1) = 0
    
        
        
        
        // A1*(B2 - t*C2) - A2*(B1 - t*C1) + m*[(D1 - t*E1)*(B2 - t*C2) - (B1 - t*C1)*(D2 - t*E2)] + t*[F2*(B1 - t*C1) - F1*(B2 - t*C2)] = 0
        // A1*(B3 - t*C3) - A3*(B1 - t*C1) + m*[(D1 - t*E1)*(B3 - t*C3) - (B1 - t*C1)*(D3 - t*E3)] + t*[F3*(B1 - t*C1) - F1*(B3 - t*C3)] = 0
        
        
        
        // R[i](t) := [(D1 - t*E1)*(B[i] - t*C[i]) - (B1 - t*C1)*(D[i] - t*E[i])]
        
        // A1*(B2 - t*C2) - A2*(B1 - t*C1) + m*R2(t) + t*[F2*(B1 - t*C1) - F1*(B2 - t*C2)] = 0
        // A1*(B3 - t*C3) - A3*(B1 - t*C1) + m*R3(t) + t*[F3*(B1 - t*C1) - F1*(B3 - t*C3)] = 0
        
        
        // A1*(B2 - t*C2)*R3(t) - A2*(B1 - t*C1)*R3(t) + m*R2(t)*R3(t) + t*[F2*(B1 - t*C1) - F1*(B2 - t*C2)]*R3(t) = 0
        // -A1*(B3 - t*C3)*R2(t) + A3*(B1 - t*C1)*R2(t) - m*R2(t)*R3(t) - t*[F3*(B1 - t*C1) - F1*(B3 - t*C3)]*R2(t) = 0
        
        // A1*(B2 - t*C2)*R3(t) - A1*(B3 - t*C3)*R2(t) - A2*(B1 - t*C1)*R3(t) + A3*(B1 - t*C1)*R2(t) + t*[F2*(B1 - t*C1) - F1*(B2 - t*C2)]*R3(t) - t*[F3*(B1 - t*C1) - F1*(B3 - t*C3)]*R2(t) = 0
        
        // R2 = (D1 - t*E1)*(B2 - t*C2) - (B1 - t*C1)*(D2 - t*E2)
        // R3 = (D1 - t*E1)*(B3 - t*C3) - (B1 - t*C1)*(D3 - t*E3)
        
        
        // A1*(B2 - t*C2)*(D1 - t*E1)*(B3 - t*C3) - (B1 - t*C1)*(D3 - t*E3) - A1*(B3 - t*C3)*(D1 - t*E1)*(B2 - t*C2) - (B1 - t*C1)*(D2 - t*E2) -
        // A2*(B1 - t*C1)*(D1 - t*E1)*(B3 - t*C3) - (B1 - t*C1)*(D3 - t*E3) + A3*(B1 - t*C1)*(D1 - t*E1)*(B2 - t*C2) - (B1 - t*C1)*(D2 - t*E2) +
        // t*[F2*(B1 - t*C1) - F1*(B2 - t*C2)]*(D1 - t*E1)*(B3 - t*C3) - (B1 - t*C1)*(D3 - t*E3) -
        // t*[F3*(B1 - t*C1) - F1*(B3 - t*C3)]*(D1 - t*E1)*(B2 - t*C2) - (B1 - t*C1)*(D2 - t*E2) = 0
        //
        
        
        // x*p[i].X + y*p[i].Y + z*p[i].Z + w*p[i].W = 0
        
        
        
        // x*p0.X + y*p0.Y + z*p0.Z + w*p0.W = 0
        // x*p1.X + y*p1.Y + z*p1.Z + w*p1.W = 0
        // x*p2.X + y*p2.Y + z*p2.Z + w*p2.W = 0
        
        
        
        
        // p0.X  p0.Y  p0.Z  p0.W
        // p1.X  p1.Y  p1.Z  p1.W
        // p2.X  p2.Y  p2.Z  p1.W
            
        
        
        let mutable r0 = p0
        let mutable r1 = p1
        let mutable r2 = p2
        
        if abs r0.X > abs r1.X then
            if abs r0.X > abs r2.X then ()
            else let t = r0 in r0 <- r2; r2 <- t
        else
            if abs r1.X > abs r2.X then let t = r0 in r0 <- r1; r1 <- t
            else let t = r0 in r0 <- r2; r2 <- t
            
        if abs r1.X > 1E-6f then
            r1 <- r1 - (r1.X / r0.X) * r0
            
        if abs r2.X > 1E-6f then
            r2 <- r2 - (r2.X / r0.X) * r0
        
        if abs r1.Y < abs r2.Y then
            let t = r1
            r1 <- r2
            r2 <- t
        
        if abs r2.Y > 1E-6f then
            r2 <- r2 - (r2.Y / r1.Y) * r1
        
        let z = r2.W
        let w = -r2.Z
        let y = -(r1.Z * z + r1.W * w) / r1.Y
        let x = -(y*r0.Y + z*r0.Z + w*r0.W) / r0.X
        
        
        let p = V4f(x,y,z,w)
        let t = -(Vec.dot p.XYZ o + p.W) / Vec.dot p.XYZ d 
        
        if t >= 0.0f && t <= 1.0f then
            let pt = o + t*d
            
            // p0 + a*u + b*v = pt
            
            
            
            let f0 = p0.XYZ - pt*p0.W
            let f1 = p1.XYZ - pt*p1.W
            let f2 = p2.XYZ - pt*p2.W
            
            let m =
                M33f.FromRows(
                    p0.XYZ - pt*p0.W,
                    p1.XYZ - pt*p1.W,
                    p2.XYZ - pt*p2.W
                )
            
            let abc = m.Inverse * pt
            
            abc.X >= 0.0f && abc.X <= 1.0f &&
            abc.Y >= 0.0f && abc.Y <= 1.0f &&
            abc.Z >= 0.0f && abc.Z <= 1.0f &&
            abc.X + abc.Y + abc.Z <= 1.0f
        else
            false

    let flags (pt : V4f) (bMin : V3f) (bMax : V3f) =
        let mutable f = Box.Flags.None
        if pt.X > bMax.X*pt.W then f <- f ||| Box.Flags.MaxX
        if pt.X < bMin.X*pt.W then f <- f ||| Box.Flags.MinX
        if pt.Y > bMax.Y*pt.W then f <- f ||| Box.Flags.MaxY
        if pt.Y < bMin.Y*pt.W then f <- f ||| Box.Flags.MinY
        if pt.Z > bMax.Z*pt.W then f <- f ||| Box.Flags.MaxZ
        if pt.Z < bMin.Z*pt.W then f <- f ||| Box.Flags.MinZ
        f
    
    let boxTriangle (bMin : V3f) (bMax : V3f) (p0 : V4f) (p1 : V4f) (p2 : V4f) =
        let f0 = flags p0 bMin bMax
        let f1 = flags p0 bMin bMax
        let f2 = flags p0 bMin bMax
        
        
        if f0 = Box.Flags.None || f1 = Box.Flags.None || f2 = Box.Flags.None then
            true
        elif (f0 &&& f1 &&& f2) <> Box.Flags.None then
            false
        elif boxLine bMin bMax p0 p1 || boxLine bMin bMax p1 p2 || boxLine bMin bMax p2 p0  then
            true
        else
            let size = bMax - bMin
            let mutable o = bMin
            let mutable d = size
            if triangleRay p0 p1 p2 o d then
                true
            else
                o.X <- bMax.X
                d.X <- -size.X
                if triangleRay p0 p1 p2 o d then
                    true
                else
                    o.Y <- bMax.Y
                    d.Y <- -size.Y
                    if triangleRay p0 p1 p2 o d then
                        true
                    else
                        o.X <- bMin.X
                        d.X <- size.X
                        if triangleRay p0 p1 p2 o d then
                            true
                        else
                            false

    [<ReflectedDefinition>]
    let contains (p0 : V2f) (p1 : V2f) (p2 : V2f) (pos : V2f) =
        let e1 = p2 - p0
        let e2 = p0 - p1
        let e3 = p1 - p2
        
        let n1 = V2f(e1.Y, -e1.X)
        let n2 = V2f(e2.Y, -e2.X)
        let n3 = V2f(e3.Y, -e3.X)
        
        let eps = 0f

        n1.Dot(pos - p0) >= eps && n2.Dot(pos - p1) >= eps && n3.Dot(pos - p2) >= eps




    [<ReflectedDefinition>]
    let boxTriangle2 (bMin : V3f) (bMax : V3f) (p0 : V4f) (p1 : V4f) (p2 : V4f) =
        let eps = 1E-5f

        let vertices : Arr<4 N, V3f> = Unchecked.defaultof<_>
        //let mutable vertexCount = 0
        
        let mutable v0 = V4f.Zero
        let mutable v1 = V4f.Zero
        let mutable v2 = V4f.Zero
        
        if p0.W > p1.W then
            if p1.W > p2.W then v0 <- p0; v1 <- p1; v2 <- p2
            elif p0.W > p2.W then v0 <- p0; v1 <- p2; v2 <- p1
            else v0 <- p2; v1 <- p0; v2 <- p1
        elif p1.W > p2.W then
            if p0.W > p2.W then v0 <- p1; v1 <- p0; v2 <- p2
            else v0 <- p1; v1 <- p2; v2 <- p0
        else
            v0 <- p2; v1 <- p1; v2 <- p0

        if v2.W >= eps then
            vertices.[0] <- v0.XYZ / v0.W
            vertices.[1] <- v1.XYZ / v1.W
            vertices.[2] <- v2.XYZ / v2.W
            
            let mutable tMin = min vertices.[0] vertices.[1]
            let mutable tMax = max vertices.[0] vertices.[1]
            tMin <- min tMin vertices.[2]
            tMax <- max tMax vertices.[2]
            
            tMin.X <= bMax.X && tMax.X >= bMin.X &&
            tMin.Y <= bMax.Y && tMax.Y >= bMin.Y &&
            tMin.Z <= bMax.Z && tMax.Z >= bMin.Z

            //vertexCount <- 3
            
        elif v1.W >= eps then
            // p0.W + t * (p2.W - p0.W) = eps
            let t = (eps - v0.W) / (v2.W - v0.W)
            let p02 = v0 + t * (v2 - v0)
            let t = (eps - v1.W) / (v2.W - v1.W)
            let p12 = v1 + t * (v2 - v1)
            
            vertices.[0] <- v0.XYZ / v0.W
            vertices.[1] <- v1.XYZ / v1.W
            vertices.[2] <- p12.XYZ / p12.W
            vertices.[3] <- p02.XYZ / p02.W

            let mutable tMin = min vertices.[0] vertices.[1]
            let mutable tMax = max vertices.[0] vertices.[1]
            tMin <- min tMin vertices.[2]
            tMax <- max tMax vertices.[2]
            tMin <- min tMin vertices.[3]
            tMax <- max tMax vertices.[3]
            
            tMin.X <= bMax.X && tMax.X >= bMin.X &&
            tMin.Y <= bMax.Y && tMax.Y >= bMin.Y &&
            tMin.Z <= bMax.Z && tMax.Z >= bMin.Z
            
            //vertexCount <- 4
            
        elif v0.W >= eps then
            let t = (eps - v0.W) / (v2.W - v0.W)
            let p02 = v0 + t * (v2 - v0)
            let t = (eps - v0.W) / (v1.W - v0.W)
            let p01 = v0 + t * (v1 - v0)
            
            vertices.[0] <- v0.XYZ / v0.W
            vertices.[1] <- p01.XYZ / p01.W
            vertices.[2] <- p02.XYZ / p02.W
            
            let mutable tMin = min vertices.[0] vertices.[1]
            let mutable tMax = max vertices.[0] vertices.[1]
            tMin <- min tMin vertices.[2]
            tMax <- max tMax vertices.[2]
            
            tMin.X <= bMax.X && tMax.X >= bMin.X &&
            tMin.Y <= bMax.Y && tMax.Y >= bMin.Y &&
            tMin.Z <= bMax.Z && tMax.Z >= bMin.Z
            
            //vertexCount <- 3
        else
            false
            //vertexCount <- 0
           
            
        //if vertexCount > 0 then
        //    let mutable tMin = vertices.[0]
        //    let mutable tMax = vertices.[0]
        //        
        //    for i in 1 .. vertexCount - 1 do
        //        tMin <- min tMin vertices.[i]
        //        tMax <- max tMax vertices.[i]
        //        
        //    tMin.X <= bMax.X && tMax.X >= bMin.X &&
        //    tMin.Y <= bMax.Y && tMax.Y >= bMin.Y &&
        //    tMin.Z <= bMax.Z && tMax.Z >= bMin.Z
        //
        //else
        //    false
              

    [<ReflectedDefinition>]
    let lineLine (p0 : V2f) (p1 : V2f) (l0 : V2f) (l1 : V2f) =
        let u = p1 - p0
        let v = l1 - l0
        let k = l0 - p0


        // p0 + t*u = l0 + s*v
        // u*t - v*s = l0 - p0

        // u.X    -v.X



        

        let mutable r0 = V2f(u.X, -v.X)
        let mutable r1 = V2f(u.Y, -v.Y)
        let mutable rhs = k

        if abs r0.X < abs r1.X then
            let t = r0
            r0 <- r1
            r1 <- t
            rhs <- V2f(rhs.Y, rhs.X)

        if abs r0.X < 1E-8f then
            // l0 + s*v = p0
            if abs v.X > abs v.Y then
                let s = -k.X / v.X
                s >= 0.0f && s <= 1.0f
            else
                let s = -k.Y / v.Y
                s >= 0.0f && s <= 1.0f
            
        else
            r1 <- r1 - (r1.X / r0.X) * r0
            
            if abs r1.Y < 1E-8f then
                
                true

                // p0 + t*u = l0
                // p0 + t*u = l1

            else
                // r1.Y = 0 => r1 = f * r0

                // r0.X * t + r0.Y * s = rhs.X
                // t = (rhs.X - r0.Y*s) / r0.X
                // r1.Y*s = rhs.Y

                let s = rhs.Y / r1.Y
                let t = (rhs.X - r0.Y*s) / r0.X
                s >= 0.0f && s <= 1.0f && t >= 0.0f && t <= 1.0f
              
    [<ReflectedDefinition>]
    let boxLineNotContained (bMin : V2f) (bMax : V2f) (p0 : V2f) (p1 : V2f) (f0 : Box.Flags) (f1 : Box.Flags) =

        if (f0 &&& f1) <> Box.Flags.None then
            false
        else
            lineLine p0 p1 (V2f(bMin.X, bMin.Y)) (V2f(bMax.X, bMin.Y)) ||
            lineLine p0 p1 (V2f(bMax.X, bMin.Y)) (V2f(bMax.X, bMax.Y)) ||
            lineLine p0 p1 (V2f(bMax.X, bMax.Y)) (V2f(bMin.X, bMax.Y)) ||
            lineLine p0 p1 (V2f(bMin.X, bMax.Y)) (V2f(bMin.X, bMin.Y))
                       

    [<ReflectedDefinition>]
    let boxLineNotContained2 (bMin : V2f) (bMax : V2f) (p0 : V2f) (p1 : V2f) (f0 : Box.Flags) (f1 : Box.Flags) =
        let mutable res = false
        let mutable fin = false
        let bf = f0 ||| f1
        if bf &&& Box.Flags.X <> Box.Flags.None then
            let dx = p1.X - p0.X
            if bf &&& Box.Flags.MinX <> Box.Flags.None then
                if dx = 0.0f && p0.X < bMin.X then
                    fin <- true
                else
                    let t = (bMin.X - p0.X) / dx
                    let pt = p0 + t * (p1 - p0)
                    if pt.Y >= bMin.Y && pt.Y <= bMax.Y then
                        res <- true
                        fin <- true

            if not fin && bf &&& Box.Flags.MaxX <> Box.Flags.None then
                if dx = 0.0f && p0.X > bMax.X then
                    fin <- true
                else
                    let t = (bMax.X - p0.X) / dx
                    let pt = p0 + t * (p1 - p0)
                    if pt.Y >= bMin.Y && pt.Y <= bMax.Y then
                        res <- true
                        fin <- true

        if not fin && bf &&& Box.Flags.Y <> Box.Flags.None then
            let dx = p1.Y - p0.Y
            if bf &&& Box.Flags.MinY <> Box.Flags.None then
                if dx = 0.0f && p0.Y < bMin.Y then
                    fin <- true
                else
                    let t = (bMin.Y - p0.Y) / dx
                    let pt = p0 + t * (p1 - p0)
                    if pt.X >= bMin.X && pt.X <= bMax.X then
                        res <- true
                        fin <- true

            if not fin && bf &&& Box.Flags.MaxY <> Box.Flags.None then
                if dx = 0.0f && p0.Y > bMax.Y then
                    fin <- true
                else
                    let t = (bMax.Y - p0.Y) / dx
                    let pt = p0 + t * (p1 - p0)
                    if pt.X >= bMin.X && pt.X <= bMax.X then
                        res <- true
                        fin <- true

        res

    [<ReflectedDefinition>]
    let boxTriangle2d (bMin : V2f) (bMax : V2f) (p0 : V2f) (p1 : V2f) (p2 : V2f) =
        let f0 = 
            let mutable f = Box.Flags.None
            if p0.X > bMax.X then f <- f ||| Box.Flags.MaxX
            elif p0.X < bMin.X then f <- f ||| Box.Flags.MinX
            if p0.Y > bMax.Y then f <- f ||| Box.Flags.MaxY
            elif p0.Y < bMin.Y then f <- f ||| Box.Flags.MinY
            f
            
        let f1 = 
            let mutable f = Box.Flags.None
            if p1.X > bMax.X then f <- f ||| Box.Flags.MaxX
            elif p1.X < bMin.X then f <- f ||| Box.Flags.MinX
            if p1.Y > bMax.Y then f <- f ||| Box.Flags.MaxY
            elif p1.Y < bMin.Y then f <- f ||| Box.Flags.MinY
            f
            
        let f2 = 
            let mutable f = Box.Flags.None
            if p2.X > bMax.X then f <- f ||| Box.Flags.MaxX
            elif p2.X < bMin.X then f <- f ||| Box.Flags.MinX
            if p2.Y > bMax.Y then f <- f ||| Box.Flags.MaxY
            elif p2.Y < bMin.Y then f <- f ||| Box.Flags.MinY
            f
        
        if f0 = Box.Flags.None || f1 = Box.Flags.None || f2 = Box.Flags.None then
            true
        elif (f0 &&& f1 &&& f2) <> Box.Flags.None then 
            false
        elif boxLineNotContained2 bMin bMax p0 p1 f0 f1 ||
                boxLineNotContained2 bMin bMax p1 p2 f1 f2 || 
                boxLineNotContained2 bMin bMax p2 p0 f2 f0 then
                true
        else
            let u = p1 - p0
            let v = p2 - p0

            // ux  vx   * (t0)   =   a
            // uy  vy     (t1)

            // d = ux*vy - uy*vx


            //  vy  -vx * a
            // -uy   ux


            // vy*ax - vx*ay
            // ux*ay - uy*ax

            let cross = u.X * v.Y - u.Y * v.X
            if abs cross < 1E-8f then
                false
            else
                let a = bMin - p0
                let invCross = 1.0f / cross
                let t0 = (a.Y * u.X - a.X * u.Y) * invCross
                let t1 = (a.X * v.Y - a.Y * v.X) * invCross

                t0 >= 0.0f && t1 >= 0.0f && t0 + t1 <= 1.0f

    [<ReflectedDefinition>]
    let boxTriangle3 (bMin : V3f) (bMax : V3f) (p0 : V4f) (p1 : V4f) (p2 : V4f) =
        let eps = 1E-5f

        let mutable v0 = V4f.Zero
        let mutable v1 = V4f.Zero
        let mutable v2 = V4f.Zero
        
        if p0.W > p1.W then
            if p1.W > p2.W then v0 <- p0; v1 <- p1; v2 <- p2
            elif p0.W > p2.W then v0 <- p0; v1 <- p2; v2 <- p1
            else v0 <- p2; v1 <- p0; v2 <- p1
        elif p1.W > p2.W then
            if p0.W > p2.W then v0 <- p1; v1 <- p0; v2 <- p2
            else v0 <- p1; v1 <- p2; v2 <- p0
        else
            v0 <- p2; v1 <- p1; v2 <- p0

        if v2.W >= eps then
            let x0 = v0.XY / v0.W
            let x1 = v1.XY / v1.W
            let x2 = v2.XY / v2.W

            boxTriangle2d bMin.XY bMax.XY x0 x1 x2
            
        elif v1.W >= eps then

            let t = (eps - v0.W) / (v2.W - v0.W)
            let p02 = v0 + t * (v2 - v0)
            let t = (eps - v1.W) / (v2.W - v1.W)
            let p12 = v1 + t * (v2 - v1)
            
            let x0 = v0.XY / v0.W
            let x1 = v1.XY / v1.W
            let x2 = p12.XY / p12.W
            let x3 = p02.XY / p02.W
            boxTriangle2d bMin.XY bMax.XY x0 x1 x2 ||
            boxTriangle2d bMin.XY bMax.XY x0 x2 x3
            
        elif v0.W >= eps then
            let t = (eps - v0.W) / (v2.W - v0.W)
            let p02 = v0 + t * (v2 - v0)
            let t = (eps - v0.W) / (v1.W - v0.W)
            let p01 = v0 + t * (v1 - v0)
            
            let x0 = v0.XY / v0.W
            let x1 = p01.XY / p01.W
            let x2 = p02.XY / p02.W
            boxTriangle2d bMin.XY bMax.XY x0 x1 x2
        else
            false
           
           
    // vec4 cross4(vec4 a, vec4 b, vec4 c) {
    //     float x = dot(a.yzw, cross(b.yzw, c.yzw));
    //     float y = -dot(a.xzw, cross(b.xzw, c.xzw));
    //     float z = dot(a.xyw, cross(b.xyw, c.xyw));
    //     float w = -dot(a.xyz, cross(b.xyz, c.xyz));
    //     return vec4(x, y, z, w);
    // }

    [<ReflectedDefinition>]
    let cross4 (a : V4f) (b : V4f) (c : V4f) =
        V4f(
            Vec.dot a.YZW (Vec.cross b.YZW c.YZW),
            -Vec.dot a.XZW (Vec.cross b.XZW c.XZW),
            Vec.dot a.XYW (Vec.cross b.XYW c.XYW),
            -Vec.dot a.XYZ (Vec.cross b.XYZ c.XYZ)
        )
    
    [<ReflectedDefinition>]
    let boxTriangle4 (bMin : V3f) (bMax : V3f) (p0 : V4f) (p1 : V4f) (p2 : V4f) =
        let tri = cross4 p0 p1 p2
        let n = Vec.normalize (tri.XYZ)
        
        let l : Arr<7 N, V4f> = Unchecked.defaultof<_>
        l.[0] <- V4f(V3f.NOO, bMax.X)
        l.[1] <- V4f(V3f.IOO, -bMin.X)
        l.[2] <- V4f(V3f.ONO, bMax.Y)
        l.[3] <- V4f(V3f.OIO, -bMin.Y)
        l.[4] <- cross4 p0 p1 (p0 + n.XYZO)
        l.[5] <- cross4 p1 p2 (p1 + n.XYZO)
        l.[6] <- cross4 p2 p0 (p2 + n.XYZO)
        
        
        let mutable i = 0
        let mutable j = 0
        let mutable res = false
        while i < 7 do
            j <- i + 1
            while j < 7 do
                let r = cross4 tri l.[i] l.[j]
                let mutable sum = 0.0f
                for k in 0 .. 6 do sum <- sum + sqrt (Vec.dot l.[k] r)
                if not (System.Single.IsNaN sum) then
                    res <- true
                    i <- 7
                    j <- 7
                    
                j <- j + 1
            i <- i + 1
        
        res
        
        
        
        
        


    [<LocalSize(X = 256, Y = 1)>]
    let transform (vertices : V4f[]) (normals : V4f[]) (pp : V4f[]) (vp : V4f[]) (vn : V4f[]) =
        compute {
            let id = getGlobalId().X
            if id < uniform.VertexCount then
                let p = vertices.[id]
                let n = normals.[id]
                
                let vv = uniform.ModelViewTrafo * p

                vp.[id] <- vv
                pp.[id] <- uniform.ProjTrafo * vv
                vn.[id] <- uniform.ModelViewTrafo * V4f(n.XYZ, 0.0f) |> Vec.normalize

        }
        
    [<ReflectedDefinition>]
    let bb (p0 : V4f) (p1 : V4f) (p2 : V4f) =
        let eps = 1E-5f

        let mutable v0 = V4f.Zero
        let mutable v1 = V4f.Zero
        let mutable v2 = V4f.Zero
        
        if p0.W > p1.W then
            if p1.W > p2.W then v0 <- p0; v1 <- p1; v2 <- p2
            elif p0.W > p2.W then v0 <- p0; v1 <- p2; v2 <- p1
            else v0 <- p2; v1 <- p0; v2 <- p1
        elif p1.W > p2.W then
            if p0.W > p2.W then v0 <- p1; v1 <- p0; v2 <- p2
            else v0 <- p1; v1 <- p2; v2 <- p0
        else
            v0 <- p2; v1 <- p1; v2 <- p0

        if v2.W >= eps then
            let x0 = v0.XY / v0.W
            let x1 = v1.XY / v1.W
            let x2 = v2.XY / v2.W

            let bMin = min x0 (min x1 x2)
            let bMax = max x0 (max x1 x2)
            
            V4f(bMin, bMax)

        elif v1.W >= eps then

            let t = (eps - v0.W) / (v2.W - v0.W)
            let p02 = v0 + t * (v2 - v0)
            let t = (eps - v1.W) / (v2.W - v1.W)
            let p12 = v1 + t * (v2 - v1)
            
            let x0 = v0.XY / v0.W
            let x1 = v1.XY / v1.W
            let x2 = p12.XY / p12.W
            let x3 = p02.XY / p02.W

            let bMin = min x0 (min x1 (min x2 x3))
            let bMax = max x0 (max x1 (max x2 x3))
            
            V4f(bMin, bMax)
            
        elif v0.W >= eps then
            let t = (eps - v0.W) / (v2.W - v0.W)
            let p02 = v0 + t * (v2 - v0)
            let t = (eps - v0.W) / (v1.W - v0.W)
            let p01 = v0 + t * (v1 - v0)
            
            let x0 = v0.XY / v0.W
            let x1 = p01.XY / p01.W
            let x2 = p02.XY / p02.W

            let bMin = min x0 (min x1 x2)
            let bMax = max x0 (max x1 x2)
            
            V4f(bMin, bMax)
        else
            V4f(10000, 10000, -10000, -10000)


    [<LocalSize(X = 256, Y = 1)>]
    let boundingBox (vertices : V4f[]) (boundingBoxes : V4f[]) =
        compute {
            let id = getGlobalId().X
            if id < uniform.TriangleChunkSize then
                let tid = 3 * (id + uniform.TriangleOffset)
                let p0 = vertices.[tid]
                let p1 = vertices.[tid + 1]
                let p2 = vertices.[tid + 2]

                boundingBoxes.[id] <- bb p0 p1 p2
        }
        
    
    [<GLSLIntrinsic("atomicAdd({0}, {1})")>]
    let atomicAdd (r : ref<int>) (e : int) : int = onlyInShaderCode ""
    

    // type Job =
    //     {
    //         Min : V2f
    //         Max : V2f
    //         Offset : int
    //         Count : int
    //     }
    //
    
    [<LocalSize(X = 64, Y = 1)>]
    let bin (counter : int[]) (jobBounds : V4f[]) (jobBoundsOffset : int) (jobOffsets : int[]) (jobOffsetOffset : int) (scannedJobCounts : int[]) (jobCount : int) (indices : int[]) (positions : V4f[]) (outputMask : int[]) (outputTriangleIds : int[]) =
        compute {
            let totalCount = scannedJobCounts.[jobCount - 1]
            
            let mutable id = atomicAdd &&counter.[0] 1
            while id < totalCount do
                let mutable l = 0
                let mutable r = jobCount - 1
                while l <= r do
                    let m = (l + r) / 2
                    let v = scannedJobCounts.[m]
                    if id >= v then
                        l <- m + 1
                    else
                        r <- m - 1
                
                let jobId = l
                let triangleId =
                    if jobId = 0 then id
                    else id - scannedJobCounts.[jobId - 1]
                
                
                let indexOffset = jobOffsets.[jobId + jobOffsetOffset]
                
                let globalTriangleIndex =
                    if indexOffset < 0 then triangleId
                    else indices.[indexOffset + triangleId]
                let vi0 = 3*globalTriangleIndex
                let p0 = positions.[vi0]
                let p1 = positions.[vi0+1]
                let p2 = positions.[vi0+2]
                
                let bMin = jobBounds.[jobId + jobBoundsOffset].XY
                let bMax = jobBounds.[jobId + jobBoundsOffset].ZW
                
                let res = if boxTriangle4 (V3f(bMin, -1.0f)) (V3f(bMax, 1.0f)) p0 p1 p2 then 1 else 0
                
                
                let outIndex = jobId * uniform.TriangleChunkSize + triangleId
                outputMask.[outIndex] <- res
                outputTriangleIds.[outIndex] <- globalTriangleIndex
                id <- atomicAdd &&counter.[0] 1
        }

    [<LocalSize(X = 64, Y = 1)>]
    let compact (positions : V4f[]) (scannedMaskBuffer : int[]) (globalTriangleIds : int[]) (dstIndices : int[]) (dstIndexOffset : int) (dstCounts : int[]) (dstCountOffset : int) =
        compute {
            let triangleId = getGlobalId().X
            let jobId = getGlobalId().Y
            
            let index = jobId * uniform.TriangleChunkSize + triangleId
            
            let scanValue = scannedMaskBuffer.[index]
            if (triangleId = 0 && scanValue > 0) || (scanValue > scannedMaskBuffer.[index - 1]) then
                let denseIndex = scanValue - 1
                let triIndex = globalTriangleIds.[index]
                
                // let vi0 = 3*triIndex
                // let p0 = positions.[vi0]
                // let p1 = positions.[vi0+1]
                // let p2 = positions.[vi0+2]
                //
                
                let dstIndex = jobId * uniform.TriangleChunkSize + denseIndex
                dstIndices.[dstIndexOffset + dstIndex] <- triIndex
                //dstBounds.[dstBoundsOffset + dstIndex] <- bb p0 p1 p2
                
            if triangleId = 0 then
                let index = jobId * uniform.TriangleChunkSize + uniform.TriangleChunkSize - 1
                dstCounts.[dstCountOffset + jobId] <- scannedMaskBuffer.[index]
            
        }
    
    
    [<LocalSize(X = 8, Y = 8)>]
    let createJobs (jobBounds : V4f[]) (jobOffsets : int[]) (jobCounts : int[]) =
        compute {
            let id = getGlobalId().XY
            if id.X < uniform.BinCount.X && id.Y < uniform.BinCount.Y then
                
                let pxMin = uniform.BinSize * id
                let pxMaxEx = pxMin + V2i(uniform.BinSize, uniform.BinSize)
                
                let tcMin = (V2f pxMin + V2f.Half) / V2f uniform.ViewportSize
                let tcMax = (V2f pxMaxEx - V2f.Half) / V2f uniform.ViewportSize
                
                let ndcMin  = 2.0f * tcMin - V2f.II
                let ndcMax  = 2.0f * tcMax - V2f.II
                
                let index = id.Y * uniform.BinCount.X + id.X
                jobBounds.[index] <- V4f(ndcMin, ndcMax)
                jobOffsets.[index] <- -1
                jobCounts.[index] <- uniform.TriangleCount
        }
    
    
    [<LocalSize(X = 64, Y = 1)>]
    let binTriangles (counter : int[]) (positions : V4f[]) (triangleMask : int[]) (binBounds : V4f[]) =
        compute {
            //let gId = getGlobalId()

            //let binIndex = gId.Y
            //let triangleIndex = gId.X 

            let mutable id = atomicAdd &&counter.[0] 1
            let totalBins = uniform.TotalBinCount
            let totalThreads = totalBins * uniform.TriangleChunkSize
            while id < totalThreads do
                let binIndex = id % totalBins
                let triangleIndex = id / totalBins 
                
                let mutable res = 0
                if triangleIndex < uniform.TriangleCount then
                    let binId = V2i(binIndex % uniform.BinCount.X, binIndex / uniform.BinCount.X)
        
                    let offset = binId * binSize
                    let minTc = (V2f offset + V2f.Half) / V2f uniform.ViewportSize
                    let maxTc = (V2f (offset + V2i(binSize, binSize)) - V2f.Half) / V2f uniform.ViewportSize
                    let minNdc = 2.0f * minTc - V2f.II
                    let maxNdc = 2.0f * maxTc - V2f.II
                    let bMin = V3f(minNdc, -1.0f)
                    let bMax = V3f(maxNdc, 1.0f)
                    if triangleIndex = 0 then binBounds.[binIndex] <- V4f(minNdc, maxNdc)
                    
                    let tid = 3 * (triangleIndex + uniform.TriangleOffset)
                    let p0 = positions.[tid]
                    let p1 = positions.[tid + 1]
                    let p2 = positions.[tid + 2]
        
                    res <- if boxTriangle3 bMin bMax p0 p1 p2 then 1 else 0
                    
                triangleMask.[triangleIndex + binIndex * uniform.TriangleChunkSize] <- res
                id <- atomicAdd &&counter.[0] 1
        }

    
    [<LocalSize(X = 64, Y = 1)>]
    let compactPrefixSum (overflow : int[]) (prefixSum : int[]) (compactedTriangleMask : int[]) =
        compute {
            let gId = getGlobalId()
            let localBinId = gId.Y
            let globalBinId = localBinId + uniform.BinIdOffset
            let tid = gId.X
            
            let index = gId.X + localBinId * uniform.TriangleChunkSize
            let prefix = prefixSum.[index]
            
            if (index = 0 && prefix > 0) || prefix > prefixSum.[index - 1]  then
                compactedTriangleMask.[prefix - 1 + globalBinId * uniform.TriangleChunkSize] <- tid
        
            if gId.X = uniform.TriangleChunkSize - 1 then
                let mutable cnt = 0
                if prefix > 500 then
                    cnt <- prefix
                
                overflow.[localBinId] <- cnt
                overflow.[localBinId + uniform.TotalBinCount] <- globalBinId
        }



    //[<LocalSize(X = binLength, Y = 1)>]
    //let rasterize (color : UIntImage2d<Formats.r32ui>) (depth : int[]) (positions : V4f[]) (viewPositions : V4f[]) (viewNormals : V4f[]) =
    //    compute {
    //        let triangleMask = allocateShared<int> doubleBinLength
    //        let tids = allocateShared<int> doubleBinLength
            
    //        //let triangleMat = allocateShared<V2f> (3 * doubleBinLength)
            
    //        let localId = getLocalId().X
    //        let binIndex = getGlobalId().Y
            
    //        let binId = V2i(binIndex % uniform.BinCount.X, binIndex / uniform.BinCount.X)
            
    //        let px = binId * binSize + V2i(localId % binSize, localId / binSize)
    //        let di = px.X + px.Y * uniform.ViewportSize.X
            
    //        let offset = binId * binSize
    //        let minTc = (V2f offset + V2f.Half) / V2f uniform.ViewportSize
    //        let maxTc = (V2f (offset + V2i(binSize, binSize)) - V2f.Half) / V2f uniform.ViewportSize
    //        let minNdc = 2.0f * minTc - V2f.II
    //        let maxNdc = 2.0f * maxTc - V2f.II
    //        let mutable triangleOffset = 0
            
    //        let mutable finalColor = V4f.Zero
    //        let mutable finalDepth = 1.0f
            
    //        while triangleOffset < uniform.TriangleCount do
    //            let tid0 = triangleOffset + getLocalId().X * 2
    //            let tid1 = tid0 + 1
    //            let mutable intersects0 = 0
    //            let mutable intersects1 = 0
                
    //            let bMin = V3f(minNdc, -1.0f)
    //            let bMax = V3f(maxNdc, 1.0f)
    //            if tid0 < uniform.TriangleCount then
    //                let oo = tid0 * 3
    //                let p0 = positions.[oo + 0]
    //                let p1 = positions.[oo + 1]
    //                let p2 = positions.[oo + 2]
                    
    //                intersects0 <- if boxTriangle2 bMin bMax p0 p1 p2 then 1 else 0
                    
    //            if tid1 < uniform.TriangleCount then
    //                let oo = tid1 * 3
    //                let p0 = positions.[oo + 0]
    //                let p1 = positions.[oo + 1]
    //                let p2 = positions.[oo + 2]
                
    //                intersects1 <- if boxTriangle2 bMin bMax p0 p1 p2 then 1 else 0
                    
    //            let lid0 = 2*localId
    //            let lid1 = lid0 + 1
                    
    //            triangleMask.[lid0] <- intersects0
    //            triangleMask.[lid1] <- intersects1
    //            barrier()
                
    //            // scan the triangle-mask
    //            let mutable s = 1
    //            let mutable d = 2
                
    //            let mutable nThreads = binLength
                
    //            while nThreads >= 1 do
    //                if localId < nThreads then
    //                    let ri = d * localId + d - 1
    //                    let li = ri - s
    //                    triangleMask.[ri] <- triangleMask.[ri] + triangleMask.[li]
                        
    //                //
    //                // if lid % d = d-1 then
    //                //     triangleMask.[lid] <- triangleMask.[lid] + triangleMask.[lid - s]
    //                barrier()
    //                nThreads <- nThreads / 2
    //                s <- s * 2
    //                d <- d * 2
                    
    //            s <- s / 4
    //            d <- d / 4
    //            nThreads <- 2
                
    //            while d > 1 do
                    
    //                if localId < nThreads - 1 then
    //                    let li = d * localId + d - 1
    //                    let ri = li + s
    //                    triangleMask.[ri] <- triangleMask.[ri] + triangleMask.[li]
    //                //     
    //                // if lid % d = d-1 && lid + s < binLength then
    //                //     triangleMask.[lid + s] <- triangleMask.[lid + s] + triangleMask.[lid] 
    //                barrier()
    //                s <- s / 2
    //                d <- d / 2
    //                nThreads <- nThreads * 2
                    
                
                    
    //            // compact the triangle-ids into tids
    //            if intersects0 <> 0 then
    //                //let oo = tid0 * 3
    //                //let p0 = positions.[oo + 0]
    //                //let p1 = positions.[oo + 1]
    //                //let p2 = positions.[oo + 2]
    //                //
    //                //let v0 = p0.XY / p0.W
    //                //let v1 = p1.XY / p1.W
    //                //let v2 = p2.XY / p2.W
    //                //
    //                //let u = v1 - v0
    //                //let v = v2 - v0
    //                //
    //                //let M = M22f.FromCols(u, v).Inverse
    //                //let k = -M * v0
                    
    //                let index = if lid0 > 0 then triangleMask.[lid0-1] else 0
                    
    //                //triangleMat.[3*index] <- M.R0
    //                //triangleMat.[3*index + 1] <- M.R1
    //                //triangleMat.[3*index + 2] <- k
                
    //                tids.[index] <- tid0
    //            if intersects1 <> 0 then
    //                //let oo = tid1 * 3
    //                //let p0 = positions.[oo + 0]
    //                //let p1 = positions.[oo + 1]
    //                //let p2 = positions.[oo + 2]
    //                //
    //                //let v0 = p0.XY / p0.W
    //                //let v1 = p1.XY / p1.W
    //                //let v2 = p2.XY / p2.W
    //                //
    //                //let u = v1 - v0
    //                //let v = v2 - v0
    //                //
    //                //let M = M22f.FromCols(u, v).Inverse
    //                //let k = -M * v0
                    
    //                let index = triangleMask.[lid1-1]
                    
    //                //triangleMat.[3*index] <- M.R0
    //                //triangleMat.[3*index + 1] <- M.R1
    //                //triangleMat.[3*index + 2] <- k
                
    //                tids.[index] <- tid1
    //            barrier()
                
                
    //            // each thread is now a pixel
    //            let triangleCount = triangleMask.[doubleBinLength - 1]
                
                
    //            //color.[px] <- V4ui(packUnorm4x8 (Heat.heat (float32 triangleCount / float32 (min uniform.TriangleCount binLength))))
    //            //finalColor <- Heat.heat (float32 triangleCount / float32 (min uniform.TriangleCount binLength))
                
    //            //color.[px] <- V4ui(packUnorm4x8 (V4f(V2f px / V2f uniform.ViewportSize, 1.0f, 1.0f)))
                
    //            //color.[px] <- V4ui(255, 0, 0, 1)
    //            //finalColor <- V4f(triangleCount / uniform.TriangleCount, 1, 1, 1)
    //            //let di = px.X + px.Y * uniform.ViewportSize.X
    //            //depth.[di] <- 1
            
    //            let tc = (V2f px + V2f.Half) / V2f uniform.ViewportSize
    //            let ndc = 2.0f * tc - V2f.II
                
    //            //finalColor <- V4f(triangleMask.[doubleBinLength - 1], triangleMask.[doubleBinLength - 1], 1, 1)
            
    //            //finalColor <- V4f(float32(binId.X + uniform.BinCount.X * binId.Y) / float32(uniform.BinCount.X * uniform.BinCount.Y), 0.0f, 0.0f, 1.0f)
    //            //color.[px] <- V4ui(packUnorm4x8 (V4f(V2f px / V2f uniform.ViewportSize, 1.0f, 1.0f)))
                
    //            // rasterize the pixel for each triangle
    //            for i in 0 .. triangleCount-1 do
    //                let tid = tids.[i]
    //                let vi0 = 3*tid + 0
    //                let vi1 = vi0 + 1
    //                let vi2 = vi0 + 2
                    
    //                let p0 = positions.[vi0]
    //                let p1 = positions.[vi1]
    //                let p2 = positions.[vi2]
                    
    //                let f0 = p0.XY - ndc*p0.W
    //                let f1 = p1.XY - ndc*p1.W
    //                let f2 = p2.XY - ndc*p2.W
                    
    //                let c0 = f0 - f2
    //                let c1 = f1 - f2
                    
    //                let det = c0.X*c1.Y - c0.Y*c1.X
    //                let r0 = V2f(c1.Y / det, -c1.X / det)
    //                let r1 = V2f(-c0.Y / det, c0.X / det)
                    
    //                let a = -Vec.dot r0 f2
    //                let b = -Vec.dot r1 f2
    //                let c = (1.0f - a - b)
                
                
    //                //let x = triangleMat.[3 * i].Dot(ndc)
    //                //let y = triangleMat.[3 * i + 1].Dot(ndc)
    //                //let k = triangleMat.[3 * i + 2]
                    
    //                //let lambda = V2f(x, y) + k
    //                //if lambda.X >= 0.0f && lambda.Y >= 0.0f && lambda.X + lambda.Y <= 1.0f then
    //                //if (contains positions.[vi0].XY positions.[vi1].XY positions.[vi2].XY ndc) then
    //                //    finalColor <- V4f(1, 1, 1, 1)
    //                if a >= 0.0f && b >= 0.0f && c >= 0.0f && a <= 1.0f && b <= 1.0f && c <= 1.0f then
    //                    let pos = a*p0 + b*p1 + c*p2
    //                    if pos.Z >= -pos.W && pos.Z <= pos.W then
    //                        let projected = pos.XYZ / pos.W
                         
    //                        //let newDepth = projected.Z * 16777215.0f |> int
    //                        let newDepth = projected.Z
    //                        //let mutable tmp = 0
    //                        //tmp <- newDepth
                    
    //                        //if newDepth <= depth.[di] then
    //                        if newDepth <= finalDepth then
    //                            //let mutable tmp = 0
    //                            //tmp <- newDepth
    //                            //depth.[di] <- newDepth
    //                            let vp = a*viewPositions.[vi0] + b*viewPositions.[vi1] + c*viewPositions.[vi2]
    //                            let vn = (a*viewNormals.[vi0] + b*viewNormals.[vi1] + c*viewNormals.[vi2]).XYZ |> Vec.normalize
                                 
    //                            let light = V3f.Zero
    //                            let lightDir = Vec.normalize (light - vp.XYZ)
    //                            let diffuse = Vec.dot lightDir vn |> abs
                         
    //                            let light = 0.2f + 0.8f*diffuse
                                 
    //                            //depth.[di] <- newDepth
    //                            finalDepth <- newDepth
    //                            //color.[px] <- V4ui (packUnorm4x8(V4f(V3f.III * light, 1.0f)))
    //                            finalColor <- V4f(V3f.III * light, 1.0f)
                
                
    //            triangleOffset <- triangleOffset + binLength * 2
            
    //        //let di = px.X + px.Y * uniform.ViewportSize.X
    //        //
    //        depth.[di] <- finalDepth * 16777215.0f |> int
    //        color.[px] <- V4ui(packUnorm4x8(finalColor))
    //    }
    

    [<LocalSize(X = 64)>]
    let rasterize2 (counter : int[]) (color : UIntImage2d<Formats.r32ui>) (depth : int[]) (positions : V4f[]) (viewPositions : V4f[]) (viewNormals : V4f[]) (jobCounts : int[]) (tids : int[]) (triangleMask : int[]) =
        compute {
            let totalBins = uniform.BinCount.X * uniform.BinCount.Y
            let pixelCount = uniform.ViewportSize.X * uniform.ViewportSize.Y
            let mutable di = atomicAdd &&counter.[0] 1
            let triangleOffset = uniform.TriangleOffset
            //while di < total do
            while di < pixelCount do
                let px = V2i(di % uniform.ViewportSize.X, di / uniform.ViewportSize.X) //getGlobalId().XY
                //let di = px.X + px.Y * uniform.ViewportSize.X

                let binId = px.X / binSize + (px.Y / binSize) * uniform.BinCount.X //(uniform.ViewportSize.X / binSize)
                let binOffset = uniform.TriangleChunkSize * binId

                let triangleCount = jobCounts.[binId]
            
                let tc = (V2f px + V2f.Half) / V2f uniform.ViewportSize
                let ndc = 2.0f * tc - V2f.II
            
                let mutable finalColor = unpackUnorm4x8 (color.[px].X)
                let mutable finalDepth = float32 depth.[di] / 16777215.0f
                //color.[px] <- V4ui (packUnorm4x8(Heat.heat (float32 (triangleCount) / 10.0f)))
                // rasterize the pixel for each triangle

                //let o = V3f(ndc, -1.0f)
                //let d = V3f(0.0f, 0.0f, 1.0f)

               
                for i in 0 .. triangleCount-1 do
                    let tidInGroup = tids.[i + binOffset]
                    let tid = tidInGroup + triangleOffset

                    let vi0 = 3*tid
                    let vi1 = vi0 + 1
                    let vi2 = vi1 + 1
                    let p0 = positions.[vi0]
                    let p1 = positions.[vi1]
                    let p2 = positions.[vi2]
            
                    let f0 = p0.XY - ndc*p0.W
                    let f1 = p1.XY - ndc*p1.W
                    let f2 = ndc*p2.W - p2.XY
                    
                    //let mutable r0 = V2f(f0.X + f2.X, f1.X + f2.X)
                    //let mutable r1 = V2f(f0.Y + f2.Y, f1.Y + f2.Y)
                    //let mutable rhs = f2
                    //if abs r1.X > abs r0.X then
                    //    let t = r0
                    //    r0 <- r1
                    //    r1 <- t
                    //    rhs <- rhs.YX

                    //let f = (r1.X / r0.X)
                    //let r1y = r1.Y - f*r0.Y
                    //rhs.Y <- rhs.Y - f*rhs.X
                    //let b = rhs.Y / r1y 
                    //let a = (rhs.X - r0.Y * b) / r0.X
                    //let c = (1.0f - a - b)

                    let c0 = f0 + f2
                    let c1 = f1 + f2
                    let det = 1.0f / (c0.X*c1.Y - c0.Y*c1.X)
                    let r0 = V2f(c1.Y * det, -c1.X * det)
                    let r1 = V2f(-c0.Y * det, c0.X * det)
                    
                    let a = Vec.dot r0 f2
                    let b = Vec.dot r1 f2
                    let c = (1.0f - a - b)
                    
                    if a >= 0.0f && b >= 0.0f && c >= 0.0f then
                        let pos = a*p0 + b*p1 + c*p2
                        if pos.Z >= -pos.W && pos.Z <= pos.W then
                            let projected = pos.XYZ / pos.W
                            let newDepth = projected.Z 

                            if newDepth <= finalDepth then
                                let vp = a*viewPositions.[vi0] + b*viewPositions.[vi1] + c*viewPositions.[vi2]
                                let vn = (a*viewNormals.[vi0] + b*viewNormals.[vi1] + c*viewNormals.[vi2]).XYZ |> Vec.normalize
                         
                                let light = V3f.Zero
                                let lightDir = Vec.normalize (light - vp.XYZ)
                                let diffuse = Vec.dot lightDir vn |> abs
                 
                                let light = 0.2f + 0.8f*diffuse
                         
                                finalDepth <- newDepth
                                finalColor <- V4f(V3f.III * light, 1.0f)
                        
                depth.[di] <- finalDepth * 16777215.0f |> int
                color.[px] <- V4ui (packUnorm4x8(finalColor))

          
                di <- atomicAdd &&counter.[0] 1
        }
    

module BinRasterizer = 
    
    type TriangleBinner(device : Device, triangleChunkSize : int, maxJobCount : int) =
        
        let bin = device.CompileCompute(Shader.bin)
        let compact = device.CompileCompute(Shader.compact)
        let createJobs = device.CompileCompute(Shader.createJobs)
        let counter = device.CreateBuffer(BufferUsage.Storage ||| BufferUsage.CopyDst, Array.zeroCreate<int> 32)
        
        let scannedJobCounts =
            device.CreateBuffer {
                Next = null
                Label = nolabel()
                Usage = BufferUsage.Storage ||| BufferUsage.CopySrc
                Size = int64 sizeof<int> * int64 maxJobCount
                MappedAtCreation = false
            }
            
        
        let maskBuffer =
            device.CreateBuffer {
                Next = null
                Label = nolabel()
                Usage = BufferUsage.Storage ||| BufferUsage.CopySrc ||| BufferUsage.CopyDst
                Size = int64 sizeof<int> * int64 triangleChunkSize * int64 maxJobCount
                MappedAtCreation = false
            }
        
        
        let triangleBuffer =
            device.CreateBuffer {
                Next = null
                Label = nolabel()
                Usage = BufferUsage.Storage ||| BufferUsage.CopySrc
                Size = int64 sizeof<int> * int64 triangleChunkSize * int64 maxJobCount
                MappedAtCreation = false
            }
            
        
        member x.Run(jobBounds4f : BufferRange, jobOffsets1i : BufferRange, jobCounts1i : BufferRange, indices1i : Buffer, positions4f : Buffer, dstIndices1i : BufferRange, dstCounts1i : BufferRange) =
            task {
                let! counter = counter
                do! counter.Fill(0)
                //  let bin (counter : int[]) (jobBounds : V4f[]) (jobBoundsOffset : int) (jobOffsets : int[]) (jobOffsetOffset : int) (scannedJobCounts : int[]) (jobCount : int) (indices : int[]) (positions : V4f[]) (outputMask : int[]) =
        
                let jobCount = int (jobCounts1i.Size / int64 sizeof<int>)
                let maskSize = jobCount * triangleChunkSize
                
                use enc = device.CreateCommandEncoder { Label = nolabel(); Next = null }
                enc.ClearBuffer(maskBuffer, 0L, int64 maskSize * int64 sizeof<int>)
                use cmd = enc.Finish { Label = nolabel() }
                do! device.Queue.Submit [| cmd |]
                
                device.Scan(jobCounts1i, scannedJobCounts.[..jobCounts1i.Size-1L])
                do! bin.Run(ceilDiv 8192 bin.LocalSize.X, [
                    "counter", counter :> obj
                    "jobBounds", jobBounds4f.Buffer
                    "jobBoundsOffset", int (jobBounds4f.Offset / int64 sizeof<V4f>)
                    "jobOffsets", jobOffsets1i.Buffer
                    "jobOffsetOffset", int (jobOffsets1i.Offset / int64 sizeof<int>)
                    "jobCount", jobCount
                    "indices", indices1i
                    "positions", positions4f
                    "outputMask", maskBuffer
                    "outputTriangleIds", triangleBuffer
                    "scannedJobCounts", scannedJobCounts
                    "TriangleChunkSize", triangleChunkSize
                ])
                
                device.ScanRows(jobCount, triangleChunkSize, maskBuffer, maskBuffer)
                
                do! compact.Run(V2i(ceilDiv triangleChunkSize compact.LocalSize.X, jobCount), [
                    "scannedMaskBuffer", maskBuffer :> obj
                    "globalTriangleIds", triangleBuffer
                    "dstIndices", dstIndices1i.Buffer
                    "dstIndexOffset", int (dstIndices1i.Offset / int64 sizeof<int>)
                    "TriangleChunkSize", triangleChunkSize
                    "dstCounts", dstCounts1i.Buffer
                    "dstCountOffset", int (dstCounts1i.Offset / int64 sizeof<int>)
                ])
                
            }
            
            
        member x.CreateJobs(viewportSize : V2i, binSize : int, triangleCount : int, jobBounds4f : Buffer, jobOffsets1i : Buffer, jobCounts1i : Buffer) =
            task {
                let bins = V2i(ceilDiv viewportSize.X binSize, ceilDiv viewportSize.Y binSize)
                do! createJobs.Run(V2i(ceilDiv bins.X createJobs.LocalSize.X, ceilDiv bins.Y createJobs.LocalSize.Y), [
                    "ViewportSize", viewportSize :> obj
                    "jobBounds", jobBounds4f
                    "jobOffsets", jobOffsets1i
                    "jobCounts", jobCounts1i
                    "BinCount", bins
                    "BinSize", binSize
                    "TriangleCount", triangleCount
                ])
            }
            
            
        
        
        
    
    
    type Shaders =
        {
            vertex  : ComputeShader
            computeBoundingBoxes  : ComputeShader
            binning  : ComputeShader
            compact  : ComputeShader
            raster  : ComputeShader
        }

    let compile (device : Device) =
        //let shader = device.CompileCompute Shader.rasterize

        let shaders =
            {
                vertex = device.CompileCompute Shader.transform
                computeBoundingBoxes = device.CompileCompute Shader.boundingBox
                binning = device.CompileCompute Shader.binTriangles
                compact = device.CompileCompute Shader.compactPrefixSum
                raster = device.CompileCompute Shader.rasterize2
            }
        
        shaders

    
    let mutable windowSize = V2i(1024, 768)
    let triangleChunkSize = 65536
    
  
    let createTempBuffers (vertexCount : int) (device : Device) =
        
        let vps =
            device.CreateBuffer {
                Next = null
                Label = nolabel()
                Usage = BufferUsage.Storage ||| BufferUsage.CopySrc
                Size = int64 sizeof<V4f> * int64 vertexCount
                MappedAtCreation = false
            }
            
        let pps =
            device.CreateBuffer {
                Next = null
                Label = nolabel()
                Usage = BufferUsage.Storage ||| BufferUsage.CopySrc
                Size = int64 sizeof<V4f> * int64 vertexCount
                MappedAtCreation = false
            }
            
        let ns =
            device.CreateBuffer {
                Next = null
                Label = nolabel()
                Usage = BufferUsage.Storage ||| BufferUsage.CopySrc
                Size = int64 sizeof<V4f> * int64 vertexCount
                MappedAtCreation = false
            }
        vps, pps, ns
    
    let createBinningBuffers (triangleChunkSize : int) (binCount : int) (device : Device) =
        let tm = device.CreateBuffer {
                    Next = null
                    Label = null
                    Usage = BufferUsage.Storage ||| BufferUsage.CopySrc
                    Size = int64 sizeof<int> * int64 triangleChunkSize * int64 binCount
                    MappedAtCreation = false
                }
    
        let prefixSum =
            device.CreateBuffer {
                Next = null
                Label = null
                Usage = BufferUsage.Storage ||| BufferUsage.CopySrc
                Size = int64 sizeof<int> * int64 triangleChunkSize * int64 binCount
                MappedAtCreation = false
            }
    
        let ctm =
            device.CreateBuffer {
                Next = null
                Label = null
                Usage = BufferUsage.Storage ||| BufferUsage.CopySrc
                Size = int64 sizeof<int> * int64 triangleChunkSize * int64 binCount
                MappedAtCreation = false
            }
        tm, prefixSum, ctm
    
    let run (shaders : Shaders) (device : Device) : string -> Rasterizer =

        
        let tmpBinCount = ceilDiv windowSize.X Shader.binSize * ceilDiv windowSize.Y Shader.binSize

        let mutable vps, pps, ns = createTempBuffers 11 device
        let mutable tm, _prefixSum, ctm = createBinningBuffers triangleChunkSize tmpBinCount device
        
        let bbb = device.CreateBuffer {
                Next = null
                Label = null
                Usage = BufferUsage.Storage ||| BufferUsage.CopySrc
                Size = int64 sizeof<V4f> * int64 triangleChunkSize
                MappedAtCreation = false
            }

        let counter = device.CreateBuffer(BufferUsage.CopyDst ||| BufferUsage.Storage, [|0|]).Result

        let maxBinCount = 4096
        let overflowBuffer =
            device.CreateBuffer {
                Next = null
                Label = null
                Usage = BufferUsage.Storage ||| BufferUsage.CopySrc
                Size = int64 sizeof<int> * int64 maxBinCount
                MappedAtCreation = false
            }
            
        let binBounds =
            device.CreateBuffer {
                Next = null
                Label = null
                Usage = BufferUsage.Storage ||| BufferUsage.CopySrc
                Size = int64 sizeof<V4f> * int64 maxBinCount
                MappedAtCreation = false
            }
            
        let binOffsets = 
            device.CreateBuffer {
                Next = null
                Label = null
                Usage = BufferUsage.Storage ||| BufferUsage.CopySrc
                Size = int64 sizeof<int> * int64 maxBinCount
                MappedAtCreation = false
            }
            
        let binCounts = 
            device.CreateBuffer {
                Next = null
                Label = null
                Usage = BufferUsage.Storage ||| BufferUsage.CopySrc
                Size = int64 sizeof<int> * int64 maxBinCount
                MappedAtCreation = false
            }
            
        let jobOutputCounts = 
            device.CreateBuffer {
                Next = null
                Label = null
                Usage = BufferUsage.Storage ||| BufferUsage.CopySrc
                Size = int64 sizeof<int> * int64 maxBinCount
                MappedAtCreation = false
            }
            
        let binner = TriangleBinner(device, triangleChunkSize, maxBinCount)
        
        
        fun (actBlock : string) (input : RasterizerInput) ->
            task {
                let size = V2i(input.ColorTexture.Width, input.ColorTexture.Height)
                let color = input.ColorTexture
                let depth = input.DepthBuffer
                
                let vertexCount = input.Positions.Size / int64 sizeof<V4f> |> int
                let triangleCount = vertexCount / 3
                
                
                use colorView = color.CreateView(TextureUsage.StorageBinding ||| TextureUsage.TextureBinding)
                
                let binCount = V2i(ceilDiv size.X Shader.binSize, ceilDiv size.Y Shader.binSize)
                let totalBins = binCount.X * binCount.Y
                
                if windowSize <> size then
                    windowSize <- size
                    tm.Dispose()
                    _prefixSum.Dispose()
                    ctm.Dispose()
                    let (a,b,c) = createBinningBuffers triangleChunkSize (binCount.X * binCount.Y) device
                    tm <- a
                    _prefixSum <- b
                    ctm <- c


                if vps.Size <> input.Positions.Size then
                    vps.Dispose()
                    pps.Dispose()
                    ns.Dispose()
                    let (a,b,c) = createTempBuffers vertexCount device
                    vps <- a
                    pps <- b
                    ns <- c
    
                if actBlock = "vertex" || actBlock = "all" then
                    do! shaders.vertex.Run(ceilDiv vertexCount shaders.vertex.LocalSize.X, [
                        "VertexCount", vertexCount :> obj
                        "vertices", input.Positions
                        "normals", input.Normals
                        "ModelViewTrafo", input.ModelViewTrafo
                        "ProjTrafo", input.ProjTrafo
                        "pp", pps :> obj
                        "vp", vps :> obj
                        "vn", ns :> obj
                    ])            
                
                do! color.Clear(0xFF000000u)
                do! depth.Fill(16777215)
                let mutable remainingTriangles = triangleCount
                for i in 0 .. triangleCount / triangleChunkSize do
                    //let workGroupsX = triangleChunkSize
                    
                    let triangleOffset = i * triangleChunkSize
                    let binTriangleCount = min triangleChunkSize (triangleCount - triangleOffset)
                    
                    do! binner.CreateJobs(size, Shader.binSize, binTriangleCount, binBounds, binOffsets, binCounts)
                    do! binner.Run(binBounds.Sub 0L, binOffsets.Sub 0L, binCounts.Sub 0L, ctm, pps, ctm.Sub 0L, jobOutputCounts.Sub 0L)
                    
                    // //if i = 0 && (actBlock = "binning" || actBlock = "all") then
                    // if actBlock = "binning" || actBlock = "all" then
                    //     do! counter.Fill(0)
                    //     do! shaders.computeBoundingBoxes.Run(ceilDiv triangleChunkSize shaders.computeBoundingBoxes.LocalSize.X, [
                    //         "TriangleChunkSize", triangleChunkSize :> obj
                    //         "TriangleOffset", triangleOffset
                    //         "vertices", pps
                    //         "boundingBoxes", bbb
                    //     ])
                    //     do! shaders.binning.Run(ceilDiv 8192 shaders.binning.LocalSize.X, [
                    //         "TriangleCount", binTriangleCount :> obj
                    //         "TriangleChunkSize", triangleChunkSize
                    //         "TriangleOffset", triangleOffset
                    //         "ViewportSize", size
                    //         "BinCount", binCount
                    //         "TotalBinCount", totalBins
                    //         "positions", pps
                    //         "triangleMask", tm
                    //         "counter", counter
                    //         "binBounds", binBounds
                    //     ])
                    //
                    // if actBlock = "scan" || actBlock = "all" then
                    //     device.ScanRows(binCount.X * binCount.Y, triangleChunkSize, tm, prefixSum)
                    //
                    // if actBlock = "compact" || actBlock = "all" then
                    //     do! shaders.compact.Run(V3i(ceilDiv triangleChunkSize shaders.compact.LocalSize.X, binCount.X * binCount.Y, 1), [
                    //         "TriangleChunkSize", triangleChunkSize :> obj
                    //         "TriangleOffset", triangleOffset
                    //         "triangleMask", tm
                    //         "prefixSum", prefixSum
                    //         "compactedTriangleMask", ctm
                    //         "overflow", overflowBuffer
                    //         "TotalBinCount", totalBins
                    //         "BinIdOffset", 0
                    //     ])
                    //     
                    //     let o = totalBins*4
                    //     let overflowCounts = overflowBuffer.[0..o-1]
                    //     let overflowBinIds = overflowBuffer.[o..2*o-1]
                    //     device.Scan(overflowCounts, overflowCounts)
                    //     
                    //     
                    //     let! overflowCounts = device.Download<int>(overflowCounts)
                    //     let! overflowBinIds = device.Download<int>(overflowBinIds)
                    //     printfn "%A %A" overflowCounts overflowBinIds
    
                    if actBlock = "raster" || actBlock = "all" then
                        do! counter.Fill(0)
                        do! shaders.raster.Run(ceilDiv 8192 shaders.raster.LocalSize.X, [
                            "color", colorView :> obj
                            "depth", depth
                            "positions", pps
                            "viewPositions", vps
                            "viewNormals", ns
                            "TriangleChunkSize", triangleChunkSize
                            "jobCounts", jobOutputCounts
                            "tids", ctm
                            "BinCount", binCount
                            "ViewportSize", size
                            "counter", counter
                            //"boundingBoxes", bbb
                            "TriangleOffset", triangleOffset
                            "triangleMask", tm
                        ])
                
                    remainingTriangles <- remainingTriangles - triangleChunkSize
            }

    let compileAndRun (actBlock : string) (device : Device)  : Rasterizer = 
        let shaders = compile device
        run shaders device actBlock 