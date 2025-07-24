@echo off

powershell -command "Expand-Archive -Force libs/windows/AMD64/libs.zip libs/windows/AMD64/"


pushd src\WebGPUNative
rd /s /q  build
mkdir build
pushd build
cmake -S .. -B . -DCMAKE_BUILD_TYPE=Release
cmake --build . --config Release --target INSTALL
popd
popd
