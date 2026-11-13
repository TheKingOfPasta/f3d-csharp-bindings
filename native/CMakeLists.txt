# Finds an installed F3D (3.4.1+) and copies its libraries into native/lib,
# where the C# project picks them up.
#
#   cmake -S native -B native/build -Df3d_DIR=<f3d install>/lib/cmake/f3d
#   cmake --build native/build
#
# Leave out -Df3d_DIR if F3D is installed in a standard location.

cmake_minimum_required(VERSION 3.21)
project(f3d_native NONE)

find_package(f3d REQUIRED COMPONENTS c_api)

# Copy the whole folder that holds the C library: it also contains
# libf3d and, in official F3D packages, the VTK libraries it depends on.
add_custom_target(copy_f3d ALL
  COMMAND ${CMAKE_COMMAND} -E copy_directory
          $<TARGET_FILE_DIR:f3d::c_api>
          ${CMAKE_CURRENT_SOURCE_DIR}/lib
  COMMENT "Copying F3D libraries to native/lib")
