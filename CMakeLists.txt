# Builds the C# bindings against an installed F3D, like java/CMakeLists.txt in F3D.
#
#   cmake -S . -B build -Df3d_DIR=<f3d install>/lib/cmake/f3d [-DBUILD_TESTING=ON]
#   cmake --build build
#   ctest --test-dir build

cmake_minimum_required(VERSION 3.21)
project(f3d_csharp NONE)

find_package(f3d REQUIRED COMPONENTS c_api)
find_program(DOTNET_EXECUTABLE dotnet REQUIRED)

# Generates src/F3D/Native/NativeMethods.g.cs from include/f3d/*.h with scripts/generate_bindings.sh.
# Runs again when a header, the script or the tool version changes.
set(F3D_CSHARP_NATIVE_METHODS ${CMAKE_CURRENT_SOURCE_DIR}/src/F3D/Native/NativeMethods.g.cs)
file(GLOB F3D_CSHARP_HEADERS ${CMAKE_CURRENT_SOURCE_DIR}/include/f3d/*.h)

add_custom_command(OUTPUT ${F3D_CSHARP_NATIVE_METHODS}
  COMMAND ${DOTNET_EXECUTABLE} tool restore
  COMMAND sh scripts/generate_bindings.sh
  DEPENDS
    ${CMAKE_CURRENT_SOURCE_DIR}/scripts/generate_bindings.sh
    ${CMAKE_CURRENT_SOURCE_DIR}/dotnet-tools.json
    ${F3D_CSHARP_HEADERS}
  WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
  COMMENT "Generating C# bindings"
  VERBATIM)

# Builds F3D.sln. F3DLibDir tells F3D.csproj where to copy the F3D libraries from.
add_custom_target(f3d-csharp ALL
  COMMAND ${DOTNET_EXECUTABLE} build ${CMAKE_CURRENT_SOURCE_DIR}/F3D.sln
    -p:F3DLibDir=$<TARGET_FILE_DIR:f3d::c_api>
  DEPENDS ${F3D_CSHARP_NATIVE_METHODS}
  VERBATIM)

option(BUILD_TESTING "Build the tests" OFF)
if(BUILD_TESTING)
  enable_testing()
  add_subdirectory(testing)
endif()
