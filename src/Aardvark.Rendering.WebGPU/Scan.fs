namespace Aardvark.Rendering.WebGPU

open System.Runtime.CompilerServices
open Aardvark.Base
open WebGPU

module ScanKernels = 
    open FShade 
    
    [<Literal>]
    let scanSize = 128

    [<Literal>]
    let halfScanSize = 64

    [<LocalSize(X = halfScanSize, Y = 1)>]
    let scanKernel (inputData : int[]) (outputData : int[]) =
        compute {
            let inputOffset : int = uniform?Arguments?inputOffset
            let inputDelta : int = uniform?Arguments?inputDelta
            let inputSize : int = uniform?Arguments?inputSize
            let outputOffset : int = uniform?Arguments?outputOffset
            let outputDelta : int = uniform?Arguments?outputDelta
            let rowLength : int = uniform?Arguments?rowLength 
            
            let line = getGlobalId().Y
            let inputOffset = rowLength * line + inputOffset
            let outputOffset = rowLength * line + outputOffset
            
            
            let mem : int[] = allocateShared scanSize
            let gid = getGlobalId().X
            let group = getWorkGroupId().X

            let gid0 = gid
            let lid0 =  getLocalId().X

            let lai = lid0
            let lbi = lid0 + halfScanSize
            let ai  = 2 * gid0 - lid0 
            let bi  = ai + halfScanSize 


            if ai < inputSize then mem.[lai] <- inputData.[inputOffset + ai * inputDelta]
            if bi < inputSize then mem.[lbi] <- inputData.[inputOffset + bi * inputDelta]

            //if lgid < inputSize then mem.[llid] <- inputData.[inputOffset + lgid * inputDelta]
            //if rgid < inputSize then mem.[rlid] <- inputData.[inputOffset + rgid * inputDelta]

            let lgid = 2 * gid0
            let rgid = lgid + 1
            let llid = 2 * lid0
            let rlid = llid + 1

            let mutable offset = 1
            let mutable d = halfScanSize
            while d > 0 do
                barrier()
                if lid0 < d then
                    let ai = offset * (llid + 1) - 1
                    let bi = offset * (rlid + 1) - 1
                    mem.[bi] <- mem.[ai] + mem.[bi]
                d <- d >>> 1
                offset <- offset <<< 1

            d <- 2
            offset <- offset >>> 1

            while d < scanSize do
                offset <- offset >>> 1
                barrier()
                if lid0 < d - 1 then
                    let ai = offset*(llid + 2) - 1
                    let bi = offset*(rlid + 2) - 1

                    mem.[bi] <- mem.[bi] + mem.[ai]

                d <- d <<< 1
            barrier()

            if lgid < inputSize then
                outputData.[outputOffset + lgid * outputDelta] <- mem.[llid]
            if rgid < inputSize then
                outputData.[outputOffset + rgid * outputDelta] <- mem.[rlid]

        }


    [<LocalSize(X = halfScanSize)>]
    let scanKernelInPlace (data : int[]) =
        compute {
            let inputOffset : int = uniform?Arguments?inputOffset
            let inputDelta : int = uniform?Arguments?inputDelta
            let inputSize : int = uniform?Arguments?inputSize
            let outputOffset : int = uniform?Arguments?outputOffset
            let outputDelta : int = uniform?Arguments?outputDelta
            let rowLength : int = uniform?Arguments?rowLength 

            let line = getGlobalId().Y
            let inputOffset = rowLength * line + inputOffset
            let outputOffset = rowLength * line + outputOffset
            
            let mem : int[] = allocateShared scanSize
            let gid = getGlobalId().X
            let group = getWorkGroupId().X

            let gid0 = gid
            let lid0 =  getLocalId().X

            let lai = lid0
            let lbi = lid0 + halfScanSize
            let ai  = 2 * gid0 - lid0 
            let bi  = ai + halfScanSize 


            if ai < inputSize then mem.[lai] <- data.[inputOffset + ai * inputDelta]
            if bi < inputSize then mem.[lbi] <- data.[inputOffset + bi * inputDelta]

            //if lgid < inputSize then mem.[llid] <- inputData.[inputOffset + lgid * inputDelta]
            //if rgid < inputSize then mem.[rlid] <- inputData.[inputOffset + rgid * inputDelta]

            let lgid = 2 * gid0
            let rgid = lgid + 1
            let llid = 2 * lid0
            let rlid = llid + 1

            let mutable offset = 1
            let mutable d = halfScanSize
            while d > 0 do
                barrier()
                if lid0 < d then
                    let ai = offset * (llid + 1) - 1
                    let bi = offset * (rlid + 1) - 1
                    mem.[bi] <- mem.[ai] + mem.[bi]
                d <- d >>> 1
                offset <- offset <<< 1

            d <- 2
            offset <- offset >>> 1

            while d < scanSize do
                offset <- offset >>> 1
                barrier()
                if lid0 < d - 1 then
                    let ai = offset*(llid + 2) - 1
                    let bi = offset*(rlid + 2) - 1

                    mem.[bi] <- mem.[bi] + mem.[ai]

                d <- d <<< 1
            barrier()

            if lgid < inputSize then
                data.[outputOffset + lgid * outputDelta] <- mem.[llid]
            if rgid < inputSize then
                data.[outputOffset + rgid * outputDelta] <- mem.[rlid]

        }

    [<LocalSize(X = halfScanSize)>]
    let fixupKernelInPlace (data : int[]) =
        compute {
            let inputOffset : int = uniform?Arguments?inputOffset
            let inputDelta : int = uniform?Arguments?inputDelta
            let outputOffset : int = uniform?Arguments?outputOffset
            let outputDelta : int = uniform?Arguments?outputDelta
            let groupSize : int = uniform?Arguments?groupSize
            let count : int = uniform?Arguments?count
            let rowLength : int = uniform?Arguments?rowLength 

            let line = getGlobalId().Y
            let inputOffset = rowLength * line + inputOffset
            let outputOffset = rowLength * line + outputOffset
            
            let id = getGlobalId().X + groupSize

            if id < count then
                let block = id / groupSize - 1
              
                let iid = inputOffset + block * inputDelta
                let oid = outputOffset + id * outputDelta

                if id % groupSize <> groupSize - 1 then
                    data.[oid] <- data.[iid] + data.[oid]

        }

type private Scanner(device : Device) =
    static let ceilDiv a b =
        if a % b = 0 then a / b
        else a / b + 1
    
    let scan = device.CompileCompute ScanKernels.scanKernel
    let scanInPlace = device.CompileCompute ScanKernels.scanKernelInPlace
    let fixup = device.CompileCompute ScanKernels.fixupKernelInPlace
    
    member x.Run(rows : int, columns : int, src : Buffer, dst : Buffer) =
        let rowLength = columns
        let rec run (src : Buffer) (srcOffset : int) (srcStride : int) (srcCount : int) (dst : Buffer) (dstOffset : int) (dstStride : int) (dstCount : int) =
            if srcCount > 1 then
                
                let kernel =
                    if src = dst then scanInPlace
                    else scan
                
                
                
                kernel.Run(V2i(ceilDiv srcCount ScanKernels.scanSize, rows), [
                    "rowLength", rowLength :> obj
                    "inputOffset", srcOffset :> obj
                    "inputDelta", srcStride :> obj
                    "inputSize", srcCount :> obj
                    "inputData", src :> obj
                    "outputOffset", dstOffset :> obj
                    "outputDelta", dstStride :> obj
                    "outputData", dst :> obj
                    "data", src :> obj
                ]).Wait()
        
                //let oSums = output.Skip(Kernels.scanSize - 1).Strided(Kernels.scanSize)
    
                let oSumsOffset = dstOffset + (ScanKernels.scanSize - 1) * dstStride
                let oSumsStride = dstStride * ScanKernels.scanSize
                let oSumsCount =
                    let lastIndex = dstOffset + (dstCount - 1) * dstStride
                    
                    // n <= (lastIndex - oSumsOffset) / oSumsStride
                    
                    (lastIndex - oSumsOffset) / oSumsStride + 1
    
                if oSumsCount > 0 then
                    run dst oSumsOffset oSumsStride oSumsCount dst oSumsOffset oSumsStride oSumsCount
                    if dstCount > ScanKernels.scanSize then
                        fixup.Run(V2i(ceilDiv (dstCount - ScanKernels.scanSize) ScanKernels.halfScanSize, rows), [
                            "rowLength", rowLength :> obj
                            "data", dst :> obj
                            "inputOffset", oSumsOffset :> obj
                            "inputDelta", oSumsStride :> obj
                            "outputOffset", dstOffset :> obj
                            "outputDelta", dstStride :> obj
                            "count", dstCount :> obj
                            "groupSize", ScanKernels.scanSize :> obj
                        ]).Wait()

        run src 0 1 columns dst 0 1 columns

    member x.Dispose() =
        scan.Dispose()
        scanInPlace.Dispose()
        fixup.Dispose()
    
    interface System.IDisposable with
        member x.Dispose() = x.Dispose()

[<AbstractClass; Sealed>]
type DeviceScanExtensions private() =
    static let cache = Dict<Device, Scanner>()
    
    [<Extension>]
    static member ScanRows(device : Device, rows : int, columns : int, src : Buffer, dst : Buffer) =
        let scan = lock cache (fun () -> cache.GetOrCreate(device, fun d -> new Scanner(d)))
        scan.Run(rows, columns, src, dst)
    
    [<Extension>]
    static member Scan(device : Device, src : Buffer, dst : Buffer) =
        let scan = lock cache (fun () -> cache.GetOrCreate(device, fun d -> new Scanner(d)))
        scan.Run(1, int (src.Size / int64 sizeof<int>), src, dst)
    
    
    

