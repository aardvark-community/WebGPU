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
    let binSize = 32
    
    [<Literal>]
    let binLength = 1024
    
    [<Literal>]
    let doubleBinLength = 2048
    
    [<Literal>]
    let halfBinLength = 512
    
    type UniformScope with
        member x.BinCount : V2i = uniform?BinCount
        member x.ViewportSize : V2i = uniform?ViewportSize
        member x.TriangleCount : int = uniform?TriangleCount
        member x.TriangleChunkSize : int = uniform?TriangleChunkSize
        member x.LoopOffset : int = uniform?LoopOffset
        member x.ModelViewTrafo : M44f = uniform?ModelViewTrafo
        member x.ProjTrafo : M44f = uniform?ProjTrafo
        member x.VertexCount : int = uniform?VertexCount
    
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
        let mutable vertexCount = 0
        
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
            
            //let mutable tMin = min vertices.[0] vertices.[1]
            //let mutable tMax = max vertices.[0] vertices.[1]
            //tMin <- min tMin vertices.[2]
            //tMax <- max tMax vertices.[2]
            //
            //tMin.X <= bMax.X && tMax.X >= bMin.X &&
            //tMin.Y <= bMax.Y && tMax.Y >= bMin.Y &&
            //tMin.Z <= bMax.Z && tMax.Z >= bMin.Z

            vertexCount <- 3
            
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

            //let mutable tMin = min vertices.[0] vertices.[1]
            //let mutable tMax = max vertices.[0] vertices.[1]
            //tMin <- min tMin vertices.[2]
            //tMax <- max tMax vertices.[2]
            //tMin <- min tMin vertices.[3]
            //tMax <- max tMax vertices.[3]
            //
            //tMin.X <= bMax.X && tMax.X >= bMin.X &&
            //tMin.Y <= bMax.Y && tMax.Y >= bMin.Y &&
            //tMin.Z <= bMax.Z && tMax.Z >= bMin.Z
            
            vertexCount <- 4
            
        elif v0.W >= eps then
            let t = (eps - v0.W) / (v2.W - v0.W)
            let p02 = v0 + t * (v2 - v0)
            let t = (eps - v0.W) / (v1.W - v0.W)
            let p01 = v0 + t * (v1 - v0)
            
            vertices.[0] <- v0.XYZ / v0.W
            vertices.[1] <- p01.XYZ / p01.W
            vertices.[2] <- p02.XYZ / p02.W
            
            //let mutable tMin = min vertices.[0] vertices.[1]
            //let mutable tMax = max vertices.[0] vertices.[1]
            //tMin <- min tMin vertices.[2]
            //tMax <- max tMax vertices.[2]
            //
            //tMin.X <= bMax.X && tMax.X >= bMin.X &&
            //tMin.Y <= bMax.Y && tMax.Y >= bMin.Y &&
            //tMin.Z <= bMax.Z && tMax.Z >= bMin.Z
            
            vertexCount <- 3
        else
            //false
            vertexCount <- 0
           
            
        if vertexCount > 0 then
            let mutable tMin = vertices.[0]
            let mutable tMax = vertices.[0]
                
            for i in 1 .. vertexCount - 1 do
                tMin <- min tMin vertices.[i]
                tMax <- max tMax vertices.[i]
                
            tMin.X <= bMax.X && tMax.X >= bMin.X &&
            tMin.Y <= bMax.Y && tMax.Y >= bMin.Y &&
            tMin.Z <= bMax.Z && tMax.Z >= bMin.Z
  
        else
            false
                    
                    
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

    [<LocalSize(X = 256, Y = 1)>]
    let binTriangles (positions : V4f[]) (triangleMask : int[]) =
        compute {
            let gId = getGlobalId()
            let binIndex = gId.Y


            let binId = V2i(binIndex % uniform.BinCount.X, binIndex / uniform.BinCount.X)

            let offset = binId * binSize
            let minTc = (V2f offset + V2f.Half) / V2f uniform.ViewportSize
            let maxTc = (V2f (offset + V2i(binSize, binSize)) - V2f.Half) / V2f uniform.ViewportSize
            let minNdc = 2.0f * minTc - V2f.II
            let maxNdc = 2.0f * maxTc - V2f.II
            let bMin = V3f(minNdc, -1.0f)
            let bMax = V3f(maxNdc, 1.0f)
            
            let tid = 3 * (gId.X + uniform.LoopOffset * uniform.TriangleChunkSize)
            let p0 = positions.[tid]
            let p1 = positions.[tid + 1]
            let p2 = positions.[tid + 2]

            triangleMask.[gId.X + binIndex * uniform.TriangleChunkSize] <- if boxTriangle2 bMin bMax p0 p1 p2 then 1 else 0
        }

    
    [<LocalSize(X = 256, Y = 1)>]
    let compactPrefixSum (triangleMask : int[]) (prefixSum : int[])  (compactedTriangleMask : int[]) =
        compute {
            let gId = getGlobalId()
            let binId = gId.Y
            let tid = gId.X + uniform.LoopOffset * uniform.TriangleChunkSize
            
            if triangleMask.[gId.X + binId * uniform.TriangleChunkSize] = 1 then
                compactedTriangleMask.[prefixSum.[gId.X + binId * uniform.TriangleChunkSize] + binId * uniform.TriangleChunkSize - 1] <- tid
        }


    [<LocalSize(X = binLength, Y = 1)>]
    let rasterize (color : UIntImage2d<Formats.r32ui>) (depth : int[]) (positions : V4f[]) (viewPositions : V4f[]) (viewNormals : V4f[]) =
        compute {
            let triangleMask = allocateShared<int> doubleBinLength
            let tids = allocateShared<int> doubleBinLength
            
            //let triangleMat = allocateShared<V2f> (3 * doubleBinLength)
            
            let localId = getLocalId().X
            let binIndex = getGlobalId().Y
            
            let binId = V2i(binIndex % uniform.BinCount.X, binIndex / uniform.BinCount.X)
            
            let px = binId * binSize + V2i(localId % binSize, localId / binSize)
            let di = px.X + px.Y * uniform.ViewportSize.X
            
            let offset = binId * binSize
            let minTc = (V2f offset + V2f.Half) / V2f uniform.ViewportSize
            let maxTc = (V2f (offset + V2i(binSize, binSize)) - V2f.Half) / V2f uniform.ViewportSize
            let minNdc = 2.0f * minTc - V2f.II
            let maxNdc = 2.0f * maxTc - V2f.II
            let mutable triangleOffset = 0
            
            let mutable finalColor = V4f.Zero
            let mutable finalDepth = 1.0f
            
            while triangleOffset < uniform.TriangleCount do
                let tid0 = triangleOffset + getLocalId().X * 2
                let tid1 = tid0 + 1
                let mutable intersects0 = 0
                let mutable intersects1 = 0
                
                let bMin = V3f(minNdc, -1.0f)
                let bMax = V3f(maxNdc, 1.0f)
                if tid0 < uniform.TriangleCount then
                    let oo = tid0 * 3
                    let p0 = positions.[oo + 0]
                    let p1 = positions.[oo + 1]
                    let p2 = positions.[oo + 2]
                    
                    intersects0 <- if boxTriangle2 bMin bMax p0 p1 p2 then 1 else 0
                    
                if tid1 < uniform.TriangleCount then
                    let oo = tid1 * 3
                    let p0 = positions.[oo + 0]
                    let p1 = positions.[oo + 1]
                    let p2 = positions.[oo + 2]
                
                    intersects1 <- if boxTriangle2 bMin bMax p0 p1 p2 then 1 else 0
                    
                let lid0 = 2*localId
                let lid1 = lid0 + 1
                    
                triangleMask.[lid0] <- intersects0
                triangleMask.[lid1] <- intersects1
                barrier()
                
                // scan the triangle-mask
                let mutable s = 1
                let mutable d = 2
                
                let mutable nThreads = binLength
                
                while nThreads >= 1 do
                    if localId < nThreads then
                        let ri = d * localId + d - 1
                        let li = ri - s
                        triangleMask.[ri] <- triangleMask.[ri] + triangleMask.[li]
                        
                    //
                    // if lid % d = d-1 then
                    //     triangleMask.[lid] <- triangleMask.[lid] + triangleMask.[lid - s]
                    barrier()
                    nThreads <- nThreads / 2
                    s <- s * 2
                    d <- d * 2
                    
                s <- s / 4
                d <- d / 4
                nThreads <- 2
                
                while d > 1 do
                    
                    if localId < nThreads - 1 then
                        let li = d * localId + d - 1
                        let ri = li + s
                        triangleMask.[ri] <- triangleMask.[ri] + triangleMask.[li]
                    //     
                    // if lid % d = d-1 && lid + s < binLength then
                    //     triangleMask.[lid + s] <- triangleMask.[lid + s] + triangleMask.[lid] 
                    barrier()
                    s <- s / 2
                    d <- d / 2
                    nThreads <- nThreads * 2
                    
                
                    
                // compact the triangle-ids into tids
                if intersects0 <> 0 then
                    //let oo = tid0 * 3
                    //let p0 = positions.[oo + 0]
                    //let p1 = positions.[oo + 1]
                    //let p2 = positions.[oo + 2]
                    //
                    //let v0 = p0.XY / p0.W
                    //let v1 = p1.XY / p1.W
                    //let v2 = p2.XY / p2.W
                    //
                    //let u = v1 - v0
                    //let v = v2 - v0
                    //
                    //let M = M22f.FromCols(u, v).Inverse
                    //let k = -M * v0
                    
                    let index = if lid0 > 0 then triangleMask.[lid0-1] else 0
                    
                    //triangleMat.[3*index] <- M.R0
                    //triangleMat.[3*index + 1] <- M.R1
                    //triangleMat.[3*index + 2] <- k
                
                    tids.[index] <- tid0
                if intersects1 <> 0 then
                    //let oo = tid1 * 3
                    //let p0 = positions.[oo + 0]
                    //let p1 = positions.[oo + 1]
                    //let p2 = positions.[oo + 2]
                    //
                    //let v0 = p0.XY / p0.W
                    //let v1 = p1.XY / p1.W
                    //let v2 = p2.XY / p2.W
                    //
                    //let u = v1 - v0
                    //let v = v2 - v0
                    //
                    //let M = M22f.FromCols(u, v).Inverse
                    //let k = -M * v0
                    
                    let index = triangleMask.[lid1-1]
                    
                    //triangleMat.[3*index] <- M.R0
                    //triangleMat.[3*index + 1] <- M.R1
                    //triangleMat.[3*index + 2] <- k
                
                    tids.[index] <- tid1
                barrier()
                
                
                // each thread is now a pixel
                let triangleCount = triangleMask.[doubleBinLength - 1]
                
                
                //color.[px] <- V4ui(packUnorm4x8 (Heat.heat (float32 triangleCount / float32 (min uniform.TriangleCount binLength))))
                //finalColor <- Heat.heat (float32 triangleCount / float32 (min uniform.TriangleCount binLength))
                
                //color.[px] <- V4ui(packUnorm4x8 (V4f(V2f px / V2f uniform.ViewportSize, 1.0f, 1.0f)))
                
                //color.[px] <- V4ui(255, 0, 0, 1)
                //finalColor <- V4f(triangleCount / uniform.TriangleCount, 1, 1, 1)
                //let di = px.X + px.Y * uniform.ViewportSize.X
                //depth.[di] <- 1
            
                let tc = (V2f px + V2f.Half) / V2f uniform.ViewportSize
                let ndc = 2.0f * tc - V2f.II
                
                //finalColor <- V4f(triangleMask.[doubleBinLength - 1], triangleMask.[doubleBinLength - 1], 1, 1)
            
                //finalColor <- V4f(float32(binId.X + uniform.BinCount.X * binId.Y) / float32(uniform.BinCount.X * uniform.BinCount.Y), 0.0f, 0.0f, 1.0f)
                //color.[px] <- V4ui(packUnorm4x8 (V4f(V2f px / V2f uniform.ViewportSize, 1.0f, 1.0f)))
                
                // rasterize the pixel for each triangle
                for i in 0 .. triangleCount-1 do
                    let tid = tids.[i]
                    let vi0 = 3*tid + 0
                    let vi1 = vi0 + 1
                    let vi2 = vi0 + 2
                    
                    let p0 = positions.[vi0]
                    let p1 = positions.[vi1]
                    let p2 = positions.[vi2]
                    
                    let f0 = p0.XY - ndc*p0.W
                    let f1 = p1.XY - ndc*p1.W
                    let f2 = p2.XY - ndc*p2.W
                    
                    let c0 = f0 - f2
                    let c1 = f1 - f2
                    
                    let det = c0.X*c1.Y - c0.Y*c1.X
                    let r0 = V2f(c1.Y / det, -c1.X / det)
                    let r1 = V2f(-c0.Y / det, c0.X / det)
                    
                    let a = -Vec.dot r0 f2
                    let b = -Vec.dot r1 f2
                    let c = (1.0f - a - b)
                
                
                    //let x = triangleMat.[3 * i].Dot(ndc)
                    //let y = triangleMat.[3 * i + 1].Dot(ndc)
                    //let k = triangleMat.[3 * i + 2]
                    
                    //let lambda = V2f(x, y) + k
                    //if lambda.X >= 0.0f && lambda.Y >= 0.0f && lambda.X + lambda.Y <= 1.0f then
                    //if (contains positions.[vi0].XY positions.[vi1].XY positions.[vi2].XY ndc) then
                    //    finalColor <- V4f(1, 1, 1, 1)
                    if a >= 0.0f && b >= 0.0f && c >= 0.0f && a <= 1.0f && b <= 1.0f && c <= 1.0f then
                        let pos = a*p0 + b*p1 + c*p2
                        if pos.Z >= -pos.W && pos.Z <= pos.W then
                            let projected = pos.XYZ / pos.W
                         
                            //let newDepth = projected.Z * 16777215.0f |> int
                            let newDepth = projected.Z
                            //let mutable tmp = 0
                            //tmp <- newDepth
                    
                            //if newDepth <= depth.[di] then
                            if newDepth <= finalDepth then
                                //let mutable tmp = 0
                                //tmp <- newDepth
                                //depth.[di] <- newDepth
                                let vp = a*viewPositions.[vi0] + b*viewPositions.[vi1] + c*viewPositions.[vi2]
                                let vn = (a*viewNormals.[vi0] + b*viewNormals.[vi1] + c*viewNormals.[vi2]).XYZ |> Vec.normalize
                                 
                                let light = V3f.Zero
                                let lightDir = Vec.normalize (light - vp.XYZ)
                                let diffuse = Vec.dot lightDir vn |> abs
                         
                                let light = 0.2f + 0.8f*diffuse
                                 
                                //depth.[di] <- newDepth
                                finalDepth <- newDepth
                                //color.[px] <- V4ui (packUnorm4x8(V4f(V3f.III * light, 1.0f)))
                                finalColor <- V4f(V3f.III * light, 1.0f)
                
                
                triangleOffset <- triangleOffset + binLength * 2
            
            //let di = px.X + px.Y * uniform.ViewportSize.X
            //
            depth.[di] <- finalDepth * 16777215.0f |> int
            color.[px] <- V4ui(packUnorm4x8(finalColor))
        }
    


    [<LocalSize(X = 8, Y = 8)>]
    let rasterize2 (color : UIntImage2d<Formats.r32ui>) (depth : int[]) (positions : V4f[]) (viewPositions : V4f[]) (viewNormals : V4f[]) (prefixSum : int[]) (tids : int[]) =
        compute {
            //let localId = getLocalId().X
            //let binIndex = getGlobalId().Y
            //
            
            //
            //let px = binId * binSize + V2i(localId % binSize, localId / binSize)
            let px = getGlobalId().XY
            let di = px.X + px.Y * uniform.ViewportSize.X

            let binId = px.X / binSize + (px.Y / binSize) * uniform.BinCount.X //(uniform.ViewportSize.X / binSize)
            

            let triangleCount = prefixSum.[uniform.TriangleChunkSize + uniform.TriangleChunkSize * binId - 1]
            
            let tc = (V2f px + V2f.Half) / V2f uniform.ViewportSize
            let ndc = 2.0f * tc - V2f.II
            //color.[px] <- V4ui (packUnorm4x8(Heat.heat (float32 (triangleCount) / 10.0f)))
            // rasterize the pixel for each triangle
            for i in 0 .. triangleCount-1 do
                let tid = tids.[i + uniform.TriangleChunkSize * binId]
                let vi0 = 3*tid + 0
                let vi1 = vi0 + 1
                let vi2 = vi0 + 2
                
                let p0 = positions.[vi0]
                let p1 = positions.[vi1]
                let p2 = positions.[vi2]
                
                let f0 = p0.XY - ndc*p0.W
                let f1 = p1.XY - ndc*p1.W
                let f2 = p2.XY - ndc*p2.W
                
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
                        let projected = pos.XYZ / pos.W
                        let newDepth = projected.Z * 16777215.0f |> int
                
                        if newDepth <= depth.[di] then
                            let vp = a*viewPositions.[vi0] + b*viewPositions.[vi1] + c*viewPositions.[vi2]
                            let vn = (a*viewNormals.[vi0] + b*viewNormals.[vi1] + c*viewNormals.[vi2]).XYZ |> Vec.normalize
                             
                            let light = V3f.Zero
                            let lightDir = Vec.normalize (light - vp.XYZ)
                            let diffuse = Vec.dot lightDir vn |> abs
                     
                            let light = 0.2f + 0.8f*diffuse
                             
                            depth.[di] <- newDepth
                            color.[px] <- V4ui (packUnorm4x8(V4f(V3f.III * light, 1.0f)))
        }
    
    
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
    
let compile (device : Device) : Rasterizer =
    // Todo
    let triangleChunkSize = 65536
    let binCount = 768
    
    //let shader = device.CompileCompute Shader.rasterize
    let binning = device.CompileCompute Shader.binTriangles
    let compact = device.CompileCompute Shader.compactPrefixSum
    let vertex = device.CompileCompute Shader.transform
    let raster = device.CompileCompute Shader.rasterize2
    let mutable vps, pps, ns = createTempBuffers 11 device
        
    let mutable tm = device.CreateBuffer {
                Next = null
                Label = null
                Usage = BufferUsage.Storage ||| BufferUsage.CopySrc
                Size = int64 sizeof<int> * int64 triangleChunkSize * int64 binCount
                MappedAtCreation = false
            }

    let mutable prefixSum =
        device.CreateBuffer {
            Next = null
            Label = null
            Usage = BufferUsage.Storage ||| BufferUsage.CopySrc
            Size = int64 sizeof<int> * int64 triangleChunkSize * int64 binCount
            MappedAtCreation = false
        }

    let mutable ctm =
        device.CreateBuffer {
            Next = null
            Label = null
            Usage = BufferUsage.Storage ||| BufferUsage.CopySrc
            Size = int64 sizeof<int> * int64 triangleChunkSize * int64 binCount
            MappedAtCreation = false
        }

    fun (input : RasterizerInput) ->
        
            task {

            let size = V2i(input.ColorTexture.Width, input.ColorTexture.Height)
            let color = input.ColorTexture
            let depth = input.DepthBuffer
            
            let vertexCount = input.Positions.Size / int64 sizeof<V4f> |> int
            let triangleCount = vertexCount / 3
            
            use colorView = color.CreateView(TextureUsage.StorageBinding ||| TextureUsage.TextureBinding)
            //let groups = V2i(ceilDiv size.X shader.LocalSize.X, ceilDiv size.Y shader.LocalSize.Y)
            
            let binCount = V2i(ceilDiv size.X Shader.binSize, ceilDiv size.Y Shader.binSize)
            
            //printf $"{binCount}\n"

            if vps.Size <> input.Positions.Size then
                vps.Dispose()
                pps.Dispose()
                ns.Dispose()
                let (a,b,c) = createTempBuffers vertexCount device
                vps <- a
                pps <- b
                ns <- c

            do! vertex.Run(ceilDiv vertexCount vertex.LocalSize.X, [
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
                let mutable workGroupsX = triangleChunkSize
                if remainingTriangles < triangleChunkSize then
                    workGroupsX <- remainingTriangles
            
                do! binning.Run(V3i(ceilDiv workGroupsX binning.LocalSize.X, binCount.X * binCount.Y, 1), [
                    "TriangleCount", triangleCount :> obj
                    "TriangleChunkSize", triangleChunkSize
                    "LoopOffset", i
                    "ViewportSize", size
                    "BinCount", binCount
                    "positions", pps
                    "triangleMask", tm
                ])
            
                device.ScanRows(binCount.X * binCount.Y, triangleChunkSize, tm, prefixSum)
                
                do! compact.Run(V3i(ceilDiv workGroupsX binning.LocalSize.X, binCount.X * binCount.Y, 1), [
                    "TriangleChunkSize", triangleChunkSize :> obj
                    "LoopOffset", i
                    "triangleMask", tm
                    "prefixSum", prefixSum
                    "compactedTriangleMask", ctm
                ])

                do! raster.Run(V3i(ceilDiv size.X raster.LocalSize.X, ceilDiv size.Y raster.LocalSize.Y, 1), [
                    "color", colorView :> obj
                    "depth", depth
                    "positions", pps
                    "viewPositions", vps
                    "viewNormals", ns
                    "TriangleChunkSize", triangleChunkSize
                    "prefixSum", prefixSum
                    "tids", ctm
                    "BinCount", binCount
                    "ViewportSize", size
                ])
            
                remainingTriangles <- remainingTriangles - triangleChunkSize
            //let triangleMask = device.Download<int>(tm).Result |> Array.chunkBySize triangleChunkSize
            //printf "asdf\n"



            //do! shader.Run(V3i(1, binCount.X * binCount.Y, 1), [
            //    "TriangleCount", triangleCount :> obj
            //    "positions", pps
            //    "viewPositions", vps
            //    "viewNormals", ns
            //    //"colors", input.Colors
            //    //"normals", input.Normals
            //    "BinCount", binCount
            //    "color", colorView
            //    "depth", depth
            //    "ViewportSize", size
            //    "ModelViewProjTrafo", M44f (input.ModelViewTrafo * input.ProjTrafo).Forward
            //])

                        
        }