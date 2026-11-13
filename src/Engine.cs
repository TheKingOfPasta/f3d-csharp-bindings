using F3D.Native;

namespace F3D;

public sealed unsafe class Engine : IDisposable
{
    f3d_engine_t* _handle;

    Engine(f3d_engine_t* handle)
    {
        if (handle == null) throw new F3DException("Could not create the F3D engine.");
        _handle = handle;
        Options = new Options(NativeMethods.f3d_engine_get_options(handle));
        Scene = new Scene(NativeMethods.f3d_engine_get_scene(handle));
        Interactor = new Interactor(NativeMethods.f3d_engine_get_interactor(handle));
    }

    public static Engine Create(bool offscreen = false) => new(NativeMethods.f3d_engine_create(offscreen ? 1 : 0));

    public static void AutoloadPlugins() => NativeMethods.f3d_engine_autoload_plugins();

    public Options Options { get; }
    public Scene Scene { get; }
    public Interactor Interactor { get; }

    public void Dispose()
    {
        if (_handle == null) return;
        NativeMethods.f3d_engine_destroy(_handle);
        _handle = null;
    }
}
