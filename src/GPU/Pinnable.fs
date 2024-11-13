namespace WebGPU.Raw

open Microsoft.FSharp.NativeInterop

#nowarn "9"

type IPinnable<'a when 'a : unmanaged> =
    abstract member Pin<'r> : action : (nativeptr<'a> -> 'r) -> 'r

module Pinnable = 
    let pinArray (arr : #IPinnable<'a>[]) (action : nativeptr<'a> -> 'r) =
        let rec pin (acc : 'a[]) (i : int) =
            if i >= arr.Length then
                use ptr = fixed acc
                action ptr
            else
                (arr.[i] :> IPinnable<'a>).Pin<'r>(fun ptr ->
                    acc.[i] <- NativePtr.read ptr
                    pin acc (i + 1)
                )
        let ptrs = Array.zeroCreate arr.Length
        pin ptrs 0