REM @echo off

pushd tmp
pushd dawn
pushd out 
pushd Release

xcopy /I /s /Y .\gen\include\dawn\*.h ..\..\..\..\..\..\include\dawn
xcopy /s /I /Y ..\..\include\  ..\..\..\..\..\..\include\
xcopy /s /I /Y ..\..\src\tint\*.h  ..\..\..\..\..\..\include\src\tint\
xcopy /s /I /Y ..\..\src\utils\*.h  ..\..\..\..\..\..\include\src\utils\
xcopy /-I /Y .\gen\include\dawn\webgpu.h ..\..\..\..\..\..\include\dawn\webgpu\webgpu.h
xcopy /-I /Y .\gen\include\dawn\webgpu_cpp.h ..\..\..\..\..\..\include\dawn\webgpu_cpp.h
xcopy /-I /Y .\gen\src\emdawnwebgpu\include\webgpu\webgpu_cpp_chained_struct.h ..\..\..\..\..\..\include\dawn\webgpu\webgpu_cpp_chained_struct.h
xcopy /-I /Y ..\..\include\webgpu\webgpu_enum_class_bitmasks.h ..\..\..\..\..\..\include\dawn\webgpu\webgpu_enum_class_bitmasks.h
xcopy /-I /Y .\gen\webgpu-headers\webgpu.h ..\..\..\..\..\..\include\dawn\webgpu.h



REM xcopy /s /Y .\gen\include\*.h ..\..\..\..\..\..\include\
REM xcopy /s /Y .\gen\webgpu-headers\*.h ..\..\..\..\..\..\include\webgpu\
REM xcopy /s /Y .\gen\src\emdawnwebgpu\include\webgpu\*.h ..\..\..\..\..\..\include\dawn\webgpu\
REM xcopy /Y ..\..\include\webgpu\webgpu_enum_class_bitmasks.h ..\..\..\..\..\..\include\dawn\webgpu\

popd 
popd 
popd 
popd 