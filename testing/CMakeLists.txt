# One ctest entry per Test<Name>.cs, like java/testing/CMakeLists.txt

set(F3D_CSHARP_TESTING_DATA_DIR "" CACHE PATH "F3D testing folder (testing/ in the F3D source tree)")

list(APPEND csharpf3dTests_list
  Engine
  Interactor
  Options
  Scene
  )

set(csharp_tests_dir ${CMAKE_CURRENT_BINARY_DIR}/bin)

add_custom_target(f3d-csharp-tests ALL
  COMMAND ${DOTNET_EXECUTABLE} build ${CMAKE_CURRENT_SOURCE_DIR}/Tests.csproj
    --nologo -c ${F3D_CSHARP_CONFIG} -o ${csharp_tests_dir}
    -p:F3DLibDir=${F3D_CSHARP_LIB_DIR}
  COMMENT "Building F3D C# tests"
  VERBATIM)
add_dependencies(f3d-csharp-tests f3d-csharp)

foreach(test_name ${csharpf3dTests_list})
  string(TOLOWER ${test_name} TName)
  add_test(NAME libf3d_bindings_csharp::test_${TName}
    WORKING_DIRECTORY ${csharp_tests_dir}
    COMMAND ${DOTNET_EXECUTABLE} ${csharp_tests_dir}/Tests.dll
      Test${test_name} ${F3D_CSHARP_TESTING_DATA_DIR}/ ${CMAKE_BINARY_DIR}/Testing/Temporary/
    )

  set_tests_properties(libf3d_bindings_csharp::test_${TName} PROPERTIES
    LABELS "libf3d;bindings;csharp"
    TIMEOUT 180
  )
endforeach()
