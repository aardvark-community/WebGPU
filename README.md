# WebGPU

This project provides WebGPU/Dawn bindings for .NET

Currently it only supports running with [Dawn](https://dawn.googlesource.com/dawn/) in a native environment, but our goal is to have the identical wrapper for Blazor WASM projects.
Sadly this cannot be done until .NET updates their emscripten compiler.

All the Wrapper code is auto-generated (from dawn.json) and some extensions make it easier to use dawn from F#.

## Building

If you only want to run the examples all you need is:

* `dotnet tool restore`
* `dotnet paket restore`
* `dotnet build`

For building the native dependencies you need to have `cmake`, `python` and an appropriate C++ compiler for your platform installed.

`./pulldawn.sh` (Linux/Mac) or `pulldawn.cmd` (Windows) clones the dawn repository, generates the wrapper code and builds all native dependencies for your platform.

After building native dependencies be sure to clean all dotnet outputs since the native deps will be embedded in your dotnet dlls.
(`git clean -xdf` helps)


