REM @echo off

set /p DAWNCOMMIT=<dawn.commit

pushd src\WebGPUNative

rd /s /q tmp
mkdir tmp
pushd tmp

mkdir dawn
REM git clone https://github.com/google/dawn.git
pushd dawn
git init
git remote add origin https://github.com/google/dawn.git
git fetch --depth 1 origin %DAWNCOMMIT%
git reset --hard FETCH_HEAD

REM git checkout %DAWNCOMMIT%

python tools\fetch_dawn_dependencies.py --use-test-deps

mkdir out
pushd out 
mkdir Release
pushd Release

cmake ..\.. -G "Visual Studio 17 2022" -A x64 -DCMAKE_CXX_STANDARD=17 -DDAWN_BUILD_TESTS=OFF -DTINT_BUILD_TESTS=OFF -DTINT_BUILD_CMD_TOOLS=OFF -DCMAKE_BUILD_TYPE=Release -DTINT_BUILD_SPV_READER=1 -DTINT_BUILD_WGSL_WRITER=1
cmake --build . --config Release --target ALL_BUILD -- /m

REM Copy dawn.json
xcopy /Y ..\..\src\dawn\dawn.json ..\..\..\..\..\..\

REM Copy Libs
xcopy /Y .\Release\webgpu_dawn.dll ..\..\..\..\..\..\libs\Native\WebGPU\windows\AMD64\
xcopy /Y .\src\dawn\native\Release\webgpu_dawn.lib ..\..\..\..\..\..\libs\Native\WebGPU\windows\AMD64\
xcopy /Y .\third_party\spirv-tools\source\Release\SPIRV-Tools.lib ..\..\..\..\..\..\libs\windows\AMD64\
xcopy /Y .\third_party\spirv-tools\source\opt\Release\SPIRV-Tools-opt.lib ..\..\..\..\..\..\libs\windows\AMD64\
xcopy .\src\tint\Release\*.lib ..\..\..\..\..\..\libs\windows\AMD64 /Y
echo %DAWNCOMMIT% > ..\..\..\..\..\..\libs\Native\WebGPU\windows\AMD64\dawn.commit

powershell -command "Compress-Archive -Force ../../../../../../libs/windows/AMD64/*.lib ../../../../../../libs/windows/AMD64/libs.zip"

REM Copy Includes
xcopy /I /s /Y .\gen\include\dawn\*.h ..\..\..\..\..\..\include\dawn
xcopy /s /I /Y ..\..\include\  ..\..\..\..\..\..\include\
xcopy /s /I /Y ..\..\src\tint\*.h  ..\..\..\..\..\..\include\src\tint\
xcopy /s /I /Y ..\..\src\utils\*.h  ..\..\..\..\..\..\include\src\utils\
xcopy /Y .\gen\include\dawn\webgpu.h ..\..\..\..\..\..\include\dawn\webgpu\webgpu.h
xcopy /Y .\gen\include\dawn\webgpu_cpp.h ..\..\..\..\..\..\include\dawn\webgpu_cpp.h
xcopy /Y .\gen\src\emdawnwebgpu\include\webgpu\webgpu_cpp_chained_struct.h ..\..\..\..\..\..\include\dawn\webgpu\webgpu_cpp_chained_struct.h
xcopy /Y ..\..\include\webgpu\webgpu_enum_class_bitmasks.h ..\..\..\..\..\..\include\dawn\webgpu\webgpu_enum_class_bitmasks.h
xcopy /Y .\gen\webgpu-headers\webgpu.h ..\..\..\..\..\..\include\dawn\webgpu.h

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