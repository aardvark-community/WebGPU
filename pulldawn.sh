#!/bin/sh

DAWNCOMMIT=`cat dawn.commit`
echo "using dawn commit $DAWNCOMMIT"

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

cd src/WebGPUNative || exit 1

rm -dfr tmp
mkdir -p tmp
cd tmp || exit 1

# git clone https://github.com/google/dawn.git
mkdir dawn
cd dawn || exit 1
git init
git remote add origin https://github.com/google/dawn.git
git fetch --depth 1 origin $DAWNCOMMIT
git reset --hard FETCH_HEAD

# git checkout $DAWNCOMMIT

python tools/fetch_dawn_dependencies.py --use-test-deps

mkdir -p out/Release
cd out/Release || exit 1

cmake -S ../.. -B . -DCMAKE_BUILD_TYPE=Release $ARCH_FLAGS -DCMAKE_INSTALL_PREFIX=./blabber -DTINT_BUILD_SPV_READER=1 -DTINT_BUILD_WGSL_WRITER=1 || { echo 'cmake failed' ; exit 1; }
make webgpu_dawn

# Copy dawn.json
cp ../../src/dawn/dawn.json ../../../../../../

# Copy Libs
if [ "$OS" = "Darwin" ]; then
  mkdir -p ../../../../../../libs/Native/WebGPU/mac/$ARCH_NAME/
  cp ./src/dawn/native/libwebgpu_dawn.dylib ../../../../../../libs/Native/WebGPU/mac/$ARCH_NAME/ || { echo 'copy failed' ; exit 1; }
else
  mkdir -p ../../../../../../libs/Native/WebGPU/linux/$ARCH_NAME/
  cp ./src/dawn/native/libwebgpu_dawn.so ../../../../../../libs/Native/WebGPU/linux/$ARCH_NAME/ || { echo 'copy failed' ; exit 1; }
fi

# Copy Headers
mkdir -p ../../../../../../include/dawn/webgpu
mkdir -p ../../../../../../include/src/tint
mkdir -p ../../../../../../include/src/utils
rsync -ar --include='*/' --include='*.h' --exclude='*' gen/include/dawn/ ../../../../../../include/dawn
rsync -ar --include='*/' --include='*.h' --exclude='*' ../../include/  ../../../../../../include
rsync -a --include='*/' --include='*.h' --exclude='*' ../../src/tint/  ../../../../../../include/src/tint/
rsync -ar --include='*/' --include='*.h' --exclude='*' ../../src/utils/  ../../../../../../include/src/utils/
cp ./gen/include/dawn/webgpu.h ../../../../../../include/dawn/webgpu/webgpu.h
cp ./gen/include/dawn/webgpu_cpp.h ../../../../../../include/dawn/webgpu_cpp.h
cp ./gen/src/emdawnwebgpu/include/webgpu/webgpu_cpp_chained_struct.h ../../../../../../include/dawn/webgpu/webgpu_cpp_chained_struct.h
cp ../../include/webgpu/webgpu_enum_class_bitmasks.h ../../../../../../include/dawn/webgpu/webgpu_enum_class_bitmasks.h
cp ./gen/webgpu-headers/webgpu.h ../../../../../../include/dawn/webgpu.h



cd ../../../../../../

dotnet fsi Generator.fsx

./buildnative.sh $ARCH
