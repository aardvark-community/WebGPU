namespace WebGPU

open Microsoft.FSharp.NativeInterop
open System.Runtime.InteropServices

#nowarn "9"

module ShaderTranspiler =

    [<DllImport("WebGPUNative")>]
    extern int private transpileSpirV(byte* spirV, int spirVLength, nativeint* wgsl, int* wgslSize)    

    [<DllImport("WebGPUNative")>]
    extern int private freeWGSL(byte* ptr)
    
    let toWGSL (stage : ShaderStage) (entryPoint : string) (glsl : string) =
        let glslStage =
            match stage with
            | ShaderStage.Compute -> GLSLang.ShaderStage.Compute
            | ShaderStage.Vertex -> GLSLang.ShaderStage.Vertex
            | _ -> GLSLang.ShaderStage.Fragment
        
        let def = string glslStage
        
        let spv, err = GLSLang.GLSLang.tryCompileWithTarget GLSLang.Target.SPIRV_1_3 glslStage entryPoint true [def] glsl
        
        match spv with
        | Some spv2 -> 
            use ptr = fixed [| 0n  |]
            use pLen = fixed [| 0 |]
            use pSpv = fixed spv2
            if transpileSpirV(pSpv, spv2.Length / 4, ptr, pLen) = 0 then
                let pStr = NativePtr.read ptr |> NativePtr.ofNativeInt<byte>
                try
                    Marshal.PtrToStringAnsi(NativePtr.toNativeInt pStr, NativePtr.read pLen)
                finally
                    freeWGSL pStr |> ignore
            else
                failwith "bad transpile"
        | _ ->
            failwithf "bad transpile: %A" err