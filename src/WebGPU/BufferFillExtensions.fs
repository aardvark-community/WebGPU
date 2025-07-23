namespace WebGPU

open System.Runtime.CompilerServices
open Microsoft.FSharp.NativeInterop
open System.Threading.Tasks
open Aardvark.Base

#nowarn "9"
   
[<AbstractClass; Sealed>]
type BufferFillExtensions private() =
        
    static let cache32 = Dict<Device, uint32 -> BufferRange -> Task>()
    
    static let fillBuffer32 =
        """
        struct Parameters {
          value: u32,
          count: u32
        }

        @group(0) @binding(0) var<storage, read_write> data: array<u32>;
        @group(0) @binding(1) var<uniform> parameters: Parameters;
 
        @compute @workgroup_size(64) fn main(@builtin(global_invocation_id) id: vec3<u32>) {
            let i = id.x;
            if(i < parameters.count) {
                data[i] = parameters.value;
            }
        }
        """
        
    static let compileFill32 (device : Device) =
        cache32.GetOrCreate(device, System.Func<_,_>(fun device ->
            let m =
                device.CreateShaderModule {
                    Next = { ShaderSourceWGSL.Code = fillBuffer32; Next = null }
                    Label = "fillBuffer32"
                }
            let layout =
                device.CreateBindGroupLayout {
                    Label = "fillBuffer32Layout"
                    Entries =
                        [|
                            BindGroupLayoutEntry.Buffer(0, ShaderStage.Compute, {
                                Type = BufferBindingType.Storage
                                HasDynamicOffset = false
                                MinBindingSize = 0L
                            })
                            BindGroupLayoutEntry.Buffer(1, ShaderStage.Compute, {
                                Type = BufferBindingType.Uniform
                                HasDynamicOffset = false
                                MinBindingSize = 8L
                            })
                        |]
                }
            let pipeLayout =
                device.CreatePipelineLayout {
                    Next = null
                    ImmediateSize = 0
                    Label = "fillBuffer32PipeLayout"
                    BindGroupLayouts = [| layout |]
                }
                
            let pipeline =
                device.CreateComputePipeline {
                    Label = "fillBuffer32Pipeline"
                    Layout = pipeLayout
                    Compute = { Module = m; EntryPoint = "main"; Constants = [||] }
                }
                
            let ceilDiv a b =
                if a % b = 0 then a / b
                else 1 + a / b
                
            let ub =
                device.CreateBuffer {
                    Next = null
                    Label = "fillBuffer32Uniform"
                    Size = 8L // 2 * uint32
                    Usage = BufferUsage.Uniform ||| BufferUsage.CopyDst
                    MappedAtCreation = false
                }
                
            let ubMem = [| 0u; 0u |]
                
            let run (value : uint32) (buffer : BufferRange) =
                let cnt = int (buffer.Size / 4L)
                
                ubMem.[0] <- value
                ubMem.[1] <- uint32 cnt
                
                use group =
                    device.CreateBindGroup {
                        Label = "fillBuffer32BindGroup"
                        Layout = layout
                        Entries =
                            [|
                                { Next = null; Binding = 0; Buffer = buffer.Buffer; Offset = buffer.Offset; Size = buffer.Size; Sampler = Sampler.Null; TextureView = TextureView.Null }
                                { Next = null; Binding = 1; Buffer = ub; Offset = 0L; Size = ub.Size; Sampler = Sampler.Null; TextureView = TextureView.Null }
                            |]
                    }
                use enc = device.CreateCommandEncoder { Label = "fillBuffer32Encoder"; Next = null }
                enc.Upload(System.ReadOnlySpan<uint32> ubMem, ub, 0L)
                use comp = enc.BeginComputePass { Label = null; TimestampWrites = PassTimestampWrites.Null }
                comp.SetPipeline(pipeline)
                comp.SetBindGroup(0, group, [||])
                comp.DispatchWorkgroups(ceilDiv cnt 64, 1, 1)
                comp.End()
                use cmd = enc.Finish { Label = "fillBuffer32Command" }
                device.Queue.Submit [| cmd |]
            run
        ))
  
    [<Extension>]
    static member Fill(this : Buffer, value : uint32) =
        compileFill32 this.Device value this.[0..]
        
    [<Extension>]
    static member Fill(this : Buffer, value : int) =
        compileFill32 this.Device (uint32 value) this.[0..]
        
    [<Extension>]
    static member Fill(this : Buffer, value : float32) =
        let mutable value = value
        use ptr = fixed &value
        let uintValue = NativePtr.read (NativePtr.cast ptr)
        compileFill32 this.Device uintValue this.[0..]
        