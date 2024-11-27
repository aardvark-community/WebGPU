namespace FShade

open System
open Aardvark.Base
open FShade.Imperative
open FShade.WGSL

[<AutoOpen>]
module Backends =
    let wgsl =
        Backend.Create {
            createUniformBuffers    = true
            bindingMode             = BindingMode.Global
            createDescriptorSets    = true
            stepDescriptorSets      = false
            createInputLocations    = true
            createOutputLocations   = true
            createPassingLocations  = true
            createPerStageUniforms  = false
            reverseMatrixLogic      = true
            depthWriteMode          = true
            useInOut                = true
            doubleAsFloat           = true 
        }

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module ModuleCompiler =

        let private containsCompute (m : Module) =
            m.Entries |> List.exists (fun e -> e.decorations |> List.exists (function EntryDecoration.Stages ShaderStageDescription.Compute -> true | _ -> false))

        let compileWGSLInternal (cfg : Backend) (module_ : Module) =
            let cfg =
                if containsCompute module_ then
                    Backend.Create cfg.Config
                else
                    cfg

            module_ 
                |> ModuleCompiler.compile cfg 
                |> Assembler.assemble cfg
                
        let compileWGSL (module_ : Module) =
            compileWGSLInternal wgsl module_
