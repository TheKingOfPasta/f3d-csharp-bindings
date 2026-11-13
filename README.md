# f3d-csharp-bindings

Minimal C# bindings for F3D, using libf3d's C API. Laid out like F3D's Java bindings
(`java/` in the F3D repository).

## Files

- `src/`: the library (`F3D.csproj`, builds `F3D.dll`)
  - `Engine.cs`, `Options.cs`, `Scene.cs`, `Interactor.cs`: one class per F3D class, each with its own `[LibraryImport]` calls
  - `F3DException.cs`: base class for F3D errors
- `F3D.sln`: builds the library, the tests and the viewer
- `CMakeLists.txt`: finds F3D and builds `F3D.sln`
- `testing/`: one `Test<Class>.cs` per class, run through ctest
- `examples/Viewer/`: opens a 3D file in an F3D window

## Requirements

- .NET 8 SDK
- CMake 3.21+
- F3D 4.0 (nightly) with the C library (`-DF3D_BINDINGS_C=ON`). Release 3.x names some
  functions differently (e.g. `f3d_engine_delete` instead of `f3d_engine_destroy`).

Distribution packages (e.g. Fedora's `f3d`) don't include the C library, so build F3D yourself
and install it. The CMake files in the F3D build folder only work once installed:

```sh
cmake --install ../f3d/build --prefix ../f3d-install
```

## Build and test

```sh
cmake -S . -B build -Df3d_DIR=<f3d install>/lib64/cmake/f3d -DBUILD_TESTING=ON
cmake --build build
ctest --test-dir build
```

Use `lib` instead of `lib64` if that is where your install put it. If F3D was built against
your system VTK, VTK is loaded from the system and is not copied.

CMake runs `dotnet build F3D.sln`. Without CMake, pass the folder holding `libf3d_c_api`
yourself. Its libraries are copied next to each program:

```sh
dotnet build F3D.sln -p:F3DLibDir=<f3d install>/lib64
```

## Run the viewer

```sh
dotnet run --project examples/Viewer -- path/to/model.obj
```
