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
        let glsl = glsl.Replace("#extension NV_gpu_shader5 : enable", "").Replace("#extension GL_ARB_gpu_shader_fp64 : enable", "").Replace("#extension GL_EXT_shader_explicit_arithmetic_types_float64 : enable", "")
        
        let spv, err = GLSLang.GLSLang.tryCompileWithTarget GLSLang.Target.SPIRV_1_0 glslStage entryPoint true [def] glsl
         
        match spv with
        | Some spv2 -> 
            use ptr = fixed [| 0n  |]
            use pLen = fixed [| 0 |]
            use pSpv = fixed spv2
            let ret =  transpileSpirV(pSpv, spv2.Length / 4, ptr, pLen)
            let str =
                let pStr = NativePtr.read ptr |> NativePtr.ofNativeInt<byte>
                let len = NativePtr.read pLen
                try
                    Marshal.PtrToStringAnsi(NativePtr.toNativeInt pStr, len)
                finally
                    freeWGSL pStr |> ignore
            
            
            if ret = 0 then str
            else
                Aardvark.Base.Log.warn "%s" glsl
                failwithf "transpileSpirV failed with code %d: %s" ret str
                
            
        | _ ->
            failwithf "bad transpile: %A" err