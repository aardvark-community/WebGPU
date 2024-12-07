#!/bin/sh

OS=`uname -s`
ARCH_FLAGS=""
if [ "$OS" = "Darwin" ];
then
    if [ "$1" = "x86_64" ]; then
        ARCH="x86_64"
    elif [ "$1" = "arm64" ]; then
        ARCH="arm64"
    else
        ARCH=`uname -m | tail -1`
    fi

    ARCH_FLAGS="-DCMAKE_OSX_ARCHITECTURES=$ARCH"

fi
echo "$OS $ARCH"

cd src/WebGPUNative
rm -dfr build
mkdir build
cd build
cmake -S .. -B . -DCMAKE_BUILD_TYPE=Release $ARCH_FLAGS
make
make install