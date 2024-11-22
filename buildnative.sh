#!/bin/sh
cd src/WebGPUNative
rm -dfr build
mkdir build
cd build
cmake .. -DCMAKE_BUILD_TYPE=Release -DCMAKE_OSX_ARCHITECTURES=arm64
make
make install