# Builds the C# bindings against an installed F3D, like java/CMakeLists.txt in F3D.
#
#   cmake -S . -B build -Df3d_DIR=<f3d install>/lib/cmake/f3d [-DBUILD_TESTING=ON]
#   cmake --build build
#   ctest --test-dir build

cmake_minimum_required(VERSION 3.21)
project(f3d_csharp NONE)

find_package(f3d REQUIRED COMPONENTS c_api)
find_program(DOTNET_EXECUTABLE dotnet REQUIRED)

# Builds F3D.sln. F3DLibDir tells F3D.csproj where to copy the F3D libraries from.
add_custom_target(f3d-csharp ALL
  COMMAND ${DOTNET_EXECUTABLE} build ${CMAKE_CURRENT_SOURCE_DIR}/F3D.sln
    -p:F3DLibDir=$<TARGET_FILE_DIR:f3d::c_api>
  VERBATIM)

option(BUILD_TESTING "Build the tests" OFF)
if(BUILD_TESTING)
  enable_testing()
  add_subdirectory(testing)
endif()
