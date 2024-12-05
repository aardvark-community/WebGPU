cmake_minimum_required(VERSION 3.9)
include("cmake/Aardvark.cmake")

project(WebGPU)


# set(CMAKE_PLATFORM_TOOLSET "v141")
set(CMAKE_BUILD_TYPE, "Release")
if(UNIX)
    set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -O3 -ffunction-sections -fdata-sections -fvisibility=hidden")
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -O3 -ffunction-sections -fdata-sections -fvisibility=hidden")
endif()

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(FILES WebGPU.cpp)
set(DAWN_LIBRARY_NAME "")
if(APPLE)
    set(CMAKE_SHARED_LINKER_FLAGS "-O3 -flto=thin -dead_strip -framework CoreFoundation -framework QuartzCore -framework Metal")
    set(FILES WebGPU.cpp utils_metal.mm)
    set(DAWN_LIBRARY_NAME "libwebgpu_dawn.dylib")
elseif(UNIX)
    set(CMAKE_SHARED_LINKER_FLAGS "-O3 -Wl,--gc-sections -Wl,--strip-all")
    set(DAWN_LIBRARY_NAME "libwebgpu_dawn.so")
elseif(WIN32)
    add_compile_definitions(NOMINMAX)
    set(DAWN_LIBRARY_NAME "webgpu_dawn.dll")
endif()

add_definitions(-D_DISABLE_CONSTEXPR_MUTEX_CONSTRUCTOR )
link_directories(${CMAKE_CURRENT_SOURCE_DIR}/../../libs/Native/WebGPU/${OS}/${ARCH}/)


add_library(WebGPU SHARED ${FILES})

include_directories(${CMAKE_CURRENT_SOURCE_DIR}/../../include/dawn)
target_link_libraries(WebGPU PRIVATE  ${DAWN_LIBRARY_NAME})

SET(CMAKE_INSTALL_PREFIX "${CMAKE_CURRENT_SOURCE_DIR}/../../libs/Native/WebGPU/") 
install(TARGETS WebGPU DESTINATION ${OS}/${ARCH})