namespace Demo

open System
open System.Threading.Tasks
open Aardvark.Base
open WebGPU


type RasterizerInput =
    {
        Positions           : Buffer
        Normals             : Buffer
        Colors              : Buffer
        
        ColorTexture        : Texture
        DepthBuffer         : Buffer
        
        ModelViewTrafo      : Trafo3d
        ProjTrafo           : Trafo3d
    }


type Rasterizer = RasterizerInput -> Task<unit>