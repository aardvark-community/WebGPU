#!/bin/sh

OS=""

if [[ "$OSTYPE" == "darwin"* ]]; then
  OS="mac"
  ARCH=`uname -m`
elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
  OS="linux"
  ARCH=`uname -m`
fi

echo "$OS $ARCH"

cd src/WebGPUNative
rm -dfr build
mkdir build
cd build
cmake .. -DCMAKE_BUILD_TYPE=Release -DCMAKE_OSX_ARCHITECTURES=arm64
make
make install