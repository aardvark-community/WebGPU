REM @echo off

pushd src\WebGPUNative

rd /s /q tmp
mkdir tmp
pushd tmp

mkdir dawn
REM git clone https://github.com/google/dawn.git
pushd dawn
git init
git remote add origin https://github.com/google/dawn.git
git fetch --depth 1 origin 3d47c8a32f07bdc91840ae56d94c247a66b6c47f
git reset --hard FETCH_HEAD

REM git checkout 3d47c8a32f07bdc91840ae56d94c247a66b6c47f

python tools\fetch_dawn_dependencies.py --use-test-deps

mkdir out
pushd out 
mkdir Release
pushd Release

cmake ..\.. -DCMAKE_BUILD_TYPE=Release
cmake --build . --config Release --target ALL_BUILD 
xcopy /Y .\Release\webgpu_dawn.dll ..\..\..\..\..\..\libs\Native\WebGPU\windows\AMD64\
xcopy /Y .\src\dawn\native\Release\webgpu_dawn.lib ..\..\..\..\..\..\libs\Native\WebGPU\windows\AMD64\
xcopy /Y .\gen\include\dawn\webgpu.h ..\..\..\..\..\..\include\dawn\webgpu
xcopy /Y .\gen\include\dawn\webgpu_cpp.h ..\..\..\..\..\..\include\dawn
xcopy /Y .\gen\src\emdawnwebgpu\include\webgpu\webgpu_cpp_chained_struct.h ..\..\..\..\..\..\include\dawn\webgpu
xcopy /Y ..\..\include\webgpu\webgpu_enum_class_bitmasks.h ..\..\..\..\..\..\include\dawn\webgpu
xcopy /Y .\gen\webgpu-headers\webgpu.h ..\..\..\..\..\..\include\webgpu.h
xcopy /Y ..\..\src\dawn\dawn.json ..\..\..\..\..\..\

popd 
popd 
popd 
popd 
popd 

dotnet fsi Generator.fsx

CALL buildnative.cmd
goto :EOF

:error
echo Failed with error #%errorlevel%.
exit /b %errorlevel%