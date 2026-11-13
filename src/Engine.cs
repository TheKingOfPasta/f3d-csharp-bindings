using System.Runtime.InteropServices;

namespace F3D;

public sealed partial class Engine : IDisposable
{
    // libf3d_c_api.so / libf3d_c_api.dylib / f3d_c_api.dll
    internal const string Lib = "f3d_c_api";

    nint _handle;

    Engine(nint handle)
    {
        if (handle == 0) throw new F3DException("Could not create the F3D engine.");
        _handle = handle;
        Options = new Options(f3d_engine_get_options(handle));
        Scene = new Scene(f3d_engine_get_scene(handle));
        Interactor = new Interactor(f3d_engine_get_interactor(handle));
    }

    public static Engine Create(bool offscreen = false) => new(f3d_engine_create(offscreen ? 1 : 0));

    public static void AutoloadPlugins() => f3d_engine_autoload_plugins();

    public Options Options { get; }
    public Scene Scene { get; }
    public Interactor Interactor { get; }

    public void Dispose()
    {
        if (_handle == 0) return;
        f3d_engine_destroy(_handle);
        _handle = 0;
    }

    [LibraryImport(Lib)] private static partial nint f3d_engine_create(int offscreen);
    [LibraryImport(Lib)] private static partial void f3d_engine_destroy(nint engine);
    [LibraryImport(Lib)] private static partial void f3d_engine_autoload_plugins();
    [LibraryImport(Lib)] private static partial nint f3d_engine_get_options(nint engine);
    [LibraryImport(Lib)] private static partial nint f3d_engine_get_scene(nint engine);
    [LibraryImport(Lib)] private static partial nint f3d_engine_get_interactor(nint engine);
}
