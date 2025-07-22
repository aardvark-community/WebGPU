
#define TINT_BUILD_SPV_READER 1
#define TINT_BUILD_WGSL_WRITER 1

#include "dllexport.h"
#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include "dawn/webgpu_cpp.h"
#include "dawn/webgpu.h"
#include "dawn/native/DawnNative.h"
#include <iostream>
#include "tint/tint.h"


using namespace std;
// #include "./ShaderTranspiler/include/ShaderTranspiler/ShaderTranspiler.hpp"

// #include "./ShaderTranspiler/deps/tint/include/tint/tint.h"


// struct MemoryCompileTask {
// 	const std::string source;
// 	const std::string sourceFileName;
// 	const ShaderStage stage;
// 	const std::vector<std::filesystem::path> includePaths;	// optional
// };
static std::mutex tintInitMtx;
static bool tintInit = false;

DllExport(int) transpileSpirV(const uint32_t* spv, int spvLength, char** wgsl, size_t* wgslSize) {
    tintInitMtx.lock();
	if (!tintInit) {
		tint::Initialize();
		tintInit = true;
	}
	tintInitMtx.unlock();
	tint::spirv::reader::Options options;
	options.allow_non_uniform_derivatives = true;
	options.allowed_features = tint::wgsl::AllowedFeatures::Everything();

	auto bin = std::vector<uint32_t>(spv, spv + spvLength);
	auto m = tint::spirv::reader::Read(bin, options);
	if(!m.IsValid()) {
		auto err = m.Diagnostics().Str();

		auto res = new char[err.size() + 1];
		strcpy(res, err.c_str());
		*wgsl = res;
		*wgslSize = err.size();
		
		return -1;
	}


	auto result = tint::wgsl::writer::Generate(m, {});

	auto wgslStr = result->wgsl;


	auto res = new char[wgslStr.size() + 1];
	strcpy(res, wgslStr.c_str());
	*wgsl = res;
	*wgslSize = wgslStr.size();

	return 0;
	// cout << "spv parsed" << bin.size() << endl;
	// auto tintprogram = tint::reader::spirv::Parse(bin, {
	// 	.allow_non_uniform_derivatives = true	
	// });

	// cout << "Tint parsed" << endl;
	// if (tintprogram.Diagnostics().contains_errors()) {
	// 	throw runtime_error(tintprogram.Diagnostics().str());
	// }
	// auto result = tint::writer::wgsl::Generate(&tintprogram, {});
	// if (!result.success) {
	// 	throw std::runtime_error(result.error);
	// }

	
}
DllExport(int) freeWGSL(char* wgsl) {
	if (wgsl) {
		delete[] wgsl;
	}
	return 0;
}