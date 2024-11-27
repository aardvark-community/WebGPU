namespace WebGPU.Raw

open Microsoft.FSharp.NativeInterop

#nowarn "9"

type IPinnable<'d, 'a when 'a : unmanaged> =
    abstract member Pin<'r> : device : 'd * action : (nativeptr<'a> -> 'r) -> 'r

module Pinnable = 
    let pinArray (d : 'd) (arr : #IPinnable<'d, 'a>[]) (action : nativeptr<'a> -> 'r) =
        let rec pin (acc : 'a[]) (i : int) =
            if i >= arr.Length then
                use ptr = fixed acc
                action ptr
            else
                (arr.[i] :> IPinnable<_, 'a>).Pin<'r>(d, fun ptr ->
                    acc.[i] <- NativePtr.read ptr
                    pin acc (i + 1)
                )
        let ptrs = Array.zeroCreate arr.Length
        pin ptrs 0
  
[<AutoOpen>]
module internal GeneratorHelpers =
    let inline sum (n : int) (f : int -> 'a) =
        let mutable res = LanguagePrimitives.GenericZero<'a>
        for i in 0 .. n - 1 do
            res <- res + f i
        res
    
type NativeStream(ptr : nativeint) =
    static let alignMask = 7n
    let mutable current = ptr
    
    member x.BasePointer = ptr
    
    member x.Value<'a when 'a : unmanaged>(value : 'a) =
        let ptr1 = current
        NativePtr.write (NativePtr.ofNativeInt current) value
        current <- current + nativeint sizeof<'a>
        
        let rem = current &&& alignMask
        if rem <> 0n then current <- current + (8n - rem)
        
        ptr1 |> NativePtr.ofNativeInt<'a>
        
    member x.Array<'a when 'a : unmanaged>(values : 'a[]) =
        let ptr1 = current
        for value in values do
            NativePtr.write (NativePtr.ofNativeInt current) value
            current <- current + nativeint sizeof<'a>
            
        let rem = current &&& alignMask
        if rem <> 0n then current <- current + (8n - rem)
        ptr1 |> NativePtr.ofNativeInt<'a>
        
    member x.WriteableArray<'a, 'b when 'a :> INativeWriteable and 'b : unmanaged >(values : 'a[]) =
        let ptr1 = current
        for value in values do
            value.WriteTo(x) |> ignore
        let rem = current &&& alignMask
        if rem <> 0n then current <- current + (8n - rem)
        ptr1 |> NativePtr.ofNativeInt<'b>
        
    member x.Pointer
        with get() = current
        and set v = current <- v
        
and [<AllowNullLiteral>] INativeWriteable =
    abstract member WriteTo : NativeStream -> nativeint
