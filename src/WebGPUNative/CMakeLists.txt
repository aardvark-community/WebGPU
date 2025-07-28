cmake_minimum_required(VERSION 3.9)
include("cmake/Aardvark.cmake")

project(WebGPUNative)


# set(CMAKE_PLATFORM_TOOLSET "v141")
set(CMAKE_BUILD_TYPE, "Release")
if(UNIX)
    set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -O3 -ffunction-sections -fdata-sections -fvisibility=hidden")
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -O3 -ffunction-sections -fdata-sections -fvisibility=hidden")
endif()

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(FILES WebGPU.cpp ShaderTrans.cpp)
set(DAWN_LIBRARY_NAME "")
if(APPLE)
    set(CMAKE_SHARED_LINKER_FLAGS "-O3 -flto=thin -dead_strip -framework CoreFoundation -framework QuartzCore -framework Metal")
    set(FILES WebGPU.cpp ShaderTrans.cpp utils_metal.mm)
    set(DAWN_LIBRARY_NAME "libwebgpu_dawn.dylib")
elseif(UNIX)
    set(CMAKE_SHARED_LINKER_FLAGS "-O3 -Wl,--gc-sections -Wl,--strip-all")
    set(DAWN_LIBRARY_NAME "libwebgpu_dawn.so")
elseif(WIN32)
    add_compile_definitions(NOMINMAX)
    set(DAWN_LIBRARY_NAME "webgpu_dawn.dll")
endif()

add_definitions(-D_DISABLE_CONSTEXPR_MUTEX_CONSTRUCTOR )
link_directories(${CMAKE_CURRENT_SOURCE_DIR}/../../libs/Native/WebGPU/${OS}/${ARCH}/ ${CMAKE_CURRENT_SOURCE_DIR}/../../libs/${OS}/${ARCH}/)

SET(ADDITIONAL_LIBS "")
if(WIN32)
set(ADDITIONAL_LIBS SPIRV-Tools.lib SPIRV-Tools-opt.lib tint_api.lib tint_api_common.lib tint_lang_core.lib tint_lang_core_constant.lib tint_lang_core_intrinsic.lib tint_lang_core_ir.lib tint_lang_core_ir_analysis.lib tint_lang_core_ir_transform.lib tint_lang_core_ir_type.lib tint_lang_core_type.lib tint_lang_hlsl.lib tint_lang_hlsl_intrinsic.lib tint_lang_hlsl_ir.lib tint_lang_hlsl_type.lib tint_lang_hlsl_writer.lib tint_lang_hlsl_writer_common.lib tint_lang_hlsl_writer_printer.lib tint_lang_hlsl_writer_raise.lib tint_lang_spirv.lib tint_lang_spirv_intrinsic.lib tint_lang_spirv_ir.lib tint_lang_spirv_reader.lib tint_lang_spirv_reader_ast_lower.lib tint_lang_spirv_reader_ast_parser.lib tint_lang_spirv_reader_common.lib tint_lang_spirv_reader_lower.lib tint_lang_spirv_reader_parser.lib tint_lang_spirv_type.lib tint_lang_spirv_validate.lib tint_lang_spirv_writer.lib tint_lang_spirv_writer_common.lib tint_lang_spirv_writer_printer.lib tint_lang_spirv_writer_raise.lib tint_lang_wgsl.lib tint_lang_wgsl_ast.lib tint_lang_wgsl_inspector.lib tint_lang_wgsl_intrinsic.lib tint_lang_wgsl_ir.lib tint_lang_wgsl_program.lib tint_lang_wgsl_reader.lib tint_lang_wgsl_reader_lower.lib tint_lang_wgsl_reader_parser.lib tint_lang_wgsl_reader_program_to_ir.lib tint_lang_wgsl_resolver.lib tint_lang_wgsl_sem.lib tint_lang_wgsl_writer.lib tint_lang_wgsl_writer_ast_printer.lib tint_lang_wgsl_writer_ir_to_program.lib tint_lang_wgsl_writer_raise.lib tint_lang_wgsl_writer_syntax_tree_printer.lib tint_utils.lib tint_utils_containers.lib tint_utils_diagnostic.lib tint_utils_ice.lib tint_utils_macros.lib tint_utils_math.lib tint_utils_memory.lib tint_utils_rtti.lib tint_utils_strconv.lib tint_utils_symbol.lib tint_utils_system.lib tint_utils_text.lib tint_utils_text_generator.lib)
endif()

add_library(WebGPUNative SHARED ${FILES})
set(CMAKE_INSTALL_RPATH_USE_LINK_PATH TRUE)
if(APPLE)
    set(CMAKE_INSTALL_RPATH "@loader_path")
    set_target_properties(WebGPUNative PROPERTIES INSTALL_RPATH "@loader_path")
elseif(UNIX)
    set(CMAKE_INSTALL_RPATH "$ORIGIN")
    set_target_properties(WebGPUNative PROPERTIES INSTALL_RPATH "$ORIGIN")
endif()

include_directories(${CMAKE_CURRENT_SOURCE_DIR}/../../include)
target_link_libraries(WebGPUNative PRIVATE  ${DAWN_LIBRARY_NAME} ${ADDITIONAL_LIBS})


SET(CMAKE_INSTALL_PREFIX "${CMAKE_CURRENT_SOURCE_DIR}/../../libs/Native/WebGPU/") 
install(TARGETS WebGPUNative DESTINATION ${OS}/${ARCH})