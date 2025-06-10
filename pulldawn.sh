#!/bin/sh

OS=`uname -s`
ARCH_FLAGS=""
ARCH_NAME="AMD64"
if [ "$OS" = "Darwin" ];
then
    if [ "$1" = "x86_64" ]; then
        ARCH="x86_64"
    elif [ "$1" = "arm64" ]; then
        ARCH="arm64"
        ARCH_NAME="ARM64"
    else
        ARCH=`uname -m | tail -1`
    fi

    if [ "$ARCH" = "arm64" ]; then
        ARCH_NAME="ARM64"
    fi

    ARCH_FLAGS="-DCMAKE_OSX_ARCHITECTURES=$ARCH"

fi
echo "$OS $ARCH $ARCH_NAME"

cd src/WebGPUNative

rm -dfr tmp
mkdir -p tmp
cd tmp

# git clone https://github.com/google/dawn.git
mkdir dawn
cd dawn
git init
git remote add origin https://github.com/google/dawn.git
git fetch --depth 1 origin 3d47c8a32f07bdc91840ae56d94c247a66b6c47f
git reset --hard FETCH_HEAD

# git checkout 3d47c8a32f07bdc91840ae56d94c247a66b6c47f

python tools/fetch_dawn_dependencies.py --use-test-deps

mkdir -p out/Release
cd out/Release

cmake -S ../.. -B . -DCMAKE_BUILD_TYPE=Release $ARCH_FLAGS -DCMAKE_INSTALL_PREFIX=./blabber || { echo 'cmake failed' ; exit 1; }
make webgpu_dawn

if [ "$OS" = "Darwin" ]; then
  mkdir -p ../../../../../../libs/Native/WebGPU/mac/$ARCH_NAME/
  cp ./src/dawn/native/libwebgpu_dawn.dylib ../../../../../../libs/Native/WebGPU/mac/$ARCH_NAME/ || { echo 'copy failed' ; exit 1; }
else
  mkdir -p ../../../../../../libs/Native/WebGPU/linux/$ARCH_NAME/
  cp ./src/dawn/native/libwebgpu_dawn.so ../../../../../../libs/Native/WebGPU/linux/$ARCH_NAME/ || { echo 'copy failed' ; exit 1; }
fi
cp -r ./gen/include/ ../../../../../../include/dawn
cp -r ../../include/ ../../../../../../include/dawn
cp ../../src/dawn/dawn.json ../../../../../../
cd ../../../../../../

dotnet fsi Generator.fsx

./buildnative.sh $ARCH
