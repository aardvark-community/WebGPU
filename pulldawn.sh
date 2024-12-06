#!/bin/sh

cd src/WebGPUNative

rm -dfr tmp
mkdir -p tmp
cd tmp

git clone https://github.com/google/dawn.git
cd dawn
git checkout 2d08f945c77094a754bed83d2821cd60dbf81c6c

python tools/fetch_dawn_dependencies.py --use-test-deps

mkdir -p out/Release
cd out/Release

cmake -S ../.. -B . -DCMAKE_BUILD_TYPE=Release -DCMAKE_OSX_ARCHITECTURES=arm64 -DCMAKE_INSTALL_PREFIX=./blabber
make -j

if [[ "$OSTYPE" == "darwin"* ]]; then
  mkdir -p ../../../../../../libs/Native/WebGPU/mac/ARM64/
  cp ./src/dawn/native/libwebgpu_dawn.dylib ../../../../../../libs/Native/WebGPU/mac/ARM64/
elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
  mkdir -p ../../../../../../libs/Native/WebGPU/linux/AMD64/
  cp ./src/dawn/native/libwebgpu_dawn.so ../../../../../../libs/Native/WebGPU/linux/AMD64/
fi
cp -r ./gen/include/ ../../../../../../include/dawn
cp -r ../../include/ ../../../../../../include/dawn
cp ../../src/dawn/dawn.json ../../../../../../
cd ../../../../../../

dotnet fsi Generator.fsx

./buildnative.sh
