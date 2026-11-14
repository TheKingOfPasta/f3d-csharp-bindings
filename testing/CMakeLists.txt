# One ctest entry per Test<Name>.cs, like java/testing/CMakeLists.txt.
# Tests.dll is built by F3D.sln (see ../CMakeLists.txt).

set(F3D_CSHARP_TESTING_DATA_DIR "" CACHE PATH "F3D testing folder (testing/ in the F3D source tree)")

set(tests_dll ${CMAKE_CURRENT_SOURCE_DIR}/bin/Debug/net8.0/Tests.dll)

list(APPEND csharpf3dTests_list
  Engine
  Interactor
  Options
  Scene
  Image
  VideoPacket
  Utils
  )

foreach(test_name ${csharpf3dTests_list})
  string(TOLOWER ${test_name} TName)
  add_test(NAME libf3d_bindings_csharp::test_${TName}
    COMMAND ${DOTNET_EXECUTABLE} ${tests_dll}
      Test${test_name} ${F3D_CSHARP_TESTING_DATA_DIR}/ ${CMAKE_BINARY_DIR}/Testing/Temporary/
    )

  set_tests_properties(libf3d_bindings_csharp::test_${TName} PROPERTIES
    LABELS "libf3d;bindings;csharp"
    TIMEOUT 180
  )
endforeach()
