@echo off

pushd src\WebGPUNative
rd /s /q  build
mkdir build
pushd build
cmake -S .. -B . -DCMAKE_BUILD_TYPE=Release
cmake --build . --config Release --target INSTALL
popd
popd
