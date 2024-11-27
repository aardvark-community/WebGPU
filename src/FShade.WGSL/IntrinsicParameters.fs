namespace FShade.WGSL.Utilities

open Aardvark.Base
open FShade

[<AutoOpen>]
module BuiltInParameters =


    let builtInInputs =
        Dictionary.ofList [
            ShaderStage.Vertex,
                Map.ofList [
                    Intrinsics.VertexId, "vertex_index"
                    Intrinsics.InstanceId, "instance_index"
                ]

            ShaderStage.Fragment,
                Map.ofList [
                    Intrinsics.FragCoord, "position"
                    //Intrinsics.PointCoord, "gl_PointCoord"
                    Intrinsics.FrontFacing, "front_facing"
                    Intrinsics.SampleId, "sample_index"
                    //Intrinsics.SamplePosition, "gl_SamplePosition"
                    Intrinsics.SampleMask, "sample_mask"
                    Intrinsics.ClipDistance, "clip_distances"
                    //Intrinsics.PrimitiveId, "gl_PrimitiveID"
                    //Intrinsics.Layer, "gl_Layer"
                    //Intrinsics.ViewportIndex, "gl_ViewportIndex"
                ]

            ShaderStage.Compute, Map.empty

        ]


    let builtInOutputs =
        Dictionary.ofList [
            ShaderStage.Vertex,
                Map.ofList [
                    //Intrinsics.PointSize, "gl_PointSize"
                    Intrinsics.ClipDistance, "clip_distances"
                ]


            ShaderStage.Fragment,
                Map.ofList [
                    Intrinsics.Depth, "frag_depth"
                    Intrinsics.SampleMask, "sample_mask"
                ]

            ShaderStage.Compute, Map.empty
        ]