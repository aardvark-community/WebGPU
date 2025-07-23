namespace Demo

open System
open System.Threading.Tasks
open Aardvark.Base
open WebGPU


type RasterizerInput =
    {
        Positions           : Buffer
        Colors              : Buffer
        
        ColorTexture        : Texture
        DepthTexture        : Texture
        
        ModelViewProj       : Trafo3d
    }


type Rasterizer = RasterizerInput -> Task<unit>