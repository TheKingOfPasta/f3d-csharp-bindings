# Builds the C# bindings against an installed F3D, like java/CMakeLists.txt in F3D.
#
#   cmake -S . -B build -Df3d_DIR=<f3d install>/lib/cmake/f3d [-DBUILD_TESTING=ON]
#   cmake --build build
#   ctest --test-dir build

cmake_minimum_required(VERSION 3.21)
project(f3d_csharp NONE)

find_package(f3d REQUIRED COMPONENTS c_api)
find_program(DOTNET_EXECUTABLE dotnet REQUIRED)
message(STATUS "Bindings: C#: dotnet found: ${DOTNET_EXECUTABLE}")

set(F3D_CSHARP_CONFIG $<IF:$<CONFIG:Debug>,Debug,Release>)
set(F3D_CSHARP_LIB_DIR $<TARGET_FILE_DIR:f3d::c_api>)
set(F3D_CSHARP_OUTPUT_DIR ${CMAKE_BINARY_DIR}/bin)

# Builds F3D.dll and copies the F3D libraries next to it
add_custom_target(f3d-csharp ALL
  COMMAND ${DOTNET_EXECUTABLE} build ${CMAKE_CURRENT_SOURCE_DIR}/F3D.csproj
    --nologo -c ${F3D_CSHARP_CONFIG} -o ${F3D_CSHARP_OUTPUT_DIR}
    -p:F3DLibDir=${F3D_CSHARP_LIB_DIR}
  COMMENT "Building F3D C# bindings"
  VERBATIM)

# Testing
option(BUILD_TESTING "Build the tests" OFF)
if(BUILD_TESTING)
  enable_testing()
  add_subdirectory(testing)
endif()
