#!/bin/sh

cd src/WebGPUNative

rm -dfr tmp
mkdir -p tmp
cd tmp

git clone https://github.com/google/dawn.git
cd dawn

python tools/fetch_dawn_dependencies.py --use-test-deps

mkdir -p out/Release
cd out/Release

cmake ../.. -DCMAKE_BUILD_TYPE=Release -DCMAKE_OSX_ARCHITECTURES=arm64
make -j

if [[ "$OSTYPE" == "darwin"* ]]; then
  cp ./src/dawn/native/libwebgpu_dawn.dylib ../../../../../../libs/Native/WebGPU/mac/ARM64/
  cp ./src/dawn/glfw/libdawn_glfw.a ../../../../../../libs/Native/WebGPU/mac/ARM64/
elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
  cp ./src/dawn/native/libwebgpu_dawn.so ../../../../../../libs/Native/WebGPU/linux/x64/
  cp ./src/dawn/glfw/libdawn_glfw.a ../../../../../../libs/Native/WebGPU/linux/x64/
fi
cp ./gen/include/dawn/webgpu.h ../../../../../../include/dawn/webgpu
cp ./gen/include/dawn/webgpu_cpp.h ../../../../../../include/dawn
cp ./gen/src/emdawnwebgpu/include/webgpu/webgpu_cpp_chained_struct.h ../../../../../../include/dawn/webgpu
cp ../../include/webgpu/webgpu_enum_class_bitmasks.h ../../../../../../include/dawn/webgpu
cp ./gen/webgpu-headers/webgpu.h ../../../../../../include/webgpu.h
cp ../../src/dawn/dawn.json ../../../../../../

cd ../../../../../../

dotnet fsi Generator.fsx

./buildnative.sh
