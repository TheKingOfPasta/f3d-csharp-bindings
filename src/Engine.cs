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

    public static implicit operator f3d_engine_t*(Engine e)
    {
        return e._handle;
    }

    public static Engine Create(bool offscreen = false) => new(NativeMethods.f3d_engine_create(offscreen ? 1 : 0));
    public static Engine CreateGLX(bool offscreen = false) => new(NativeMethods.f3d_engine_create_glx(offscreen ? 1 : 0));
    public static Engine CreateWGL(bool offscreen = false) => new(NativeMethods.f3d_engine_create_wgl(offscreen ? 1 : 0));
    public static Engine CreateEGL() => new(NativeMethods.f3d_engine_create_egl());
    public static Engine CreateOSMesa() => new(NativeMethods.f3d_engine_create_osmesa());
    //public static Engine CreateExternal(Delegate del) => new(NativeMethods.f3d_engine_create_external(del));// TODO: Tester celui la (type faux)
    public static Engine CreateExternalGLX() => new(NativeMethods.f3d_engine_create_external_glx());
    public static Engine CreateExternalWGL() => new(NativeMethods.f3d_engine_create_external_wgl());
    public static Engine CreateExternalCOCOA() => new(NativeMethods.f3d_engine_create_external_cocoa());
    public static Engine CreateExternalEGL() => new(NativeMethods.f3d_engine_create_external_egl());
    public static Engine CreateExternalOSMesa() => new(NativeMethods.f3d_engine_create_external_osmesa());
    public int SetCachePath(string cache_path) => NativeMethods.f3d_engine_set_cache_path(this, cache_path);
    public string GetCachePath() => NativeMethods.f3d_engine_get_cache_path(this);// TODO: Tester celui la
    public void SetOptions(Options opt) => NativeMethods.f3d_engine_get_options(this);
    public string DumpToString() => NativeMethods.f3d_engine_dump_to_string(this);
    public int DumpToFile(string file_path) => NativeMethods.f3d_engine_dump_to_file(this, file_path);
    public int DumpToClipboard() => NativeMethods.f3d_engine_dump_to_clipboard(this);
    public int LoadFromString(string content) => NativeMethods.f3d_engine_load_from_string(this, content);
    public int LoadFromFile(string file_path) => NativeMethods.f3d_engine_load_from_file(this, file_path);
    public int LoadFromClipboard() => NativeMethods.f3d_engine_load_from_clipboard(this);
    public static void DestroyString(string str) => NativeMethods.f3d_engine_destroy_string(str);
    public Options GetOptions() => new(NativeMethods.f3d_engine_get_options(this));
    //public Window GetWindow() => new(NativeMethods.f3d_engine_get_window(this));
    public Scene GetScene() => new(NativeMethods.f3d_engine_get_scene(this));
    public Interactor GetInteractor() => new(NativeMethods.f3d_engine_get_interactor(this));
    //public static BackendInfo GetRenderingBackendList(out int count) => NativeMethods.f3d_engine_get_rendering_backend_list(out count);// TODO: int* count => ??? in cs
    public static int LoadPlugin(string path_or_name) => NativeMethods.f3d_engine_load_plugin(path_or_name);
    public static void AutoloadPlugins() => NativeMethods.f3d_engine_autoload_plugins();
    public string[] GetPluginsList(string plugin_path) => NativeMethods.f3d_engine_get_plugins_list(plugin_path);// TODO : sbyte** => ??? in cs
    public static string[] GetAllReaderOptionNames() => NativeMethods.f3d_engine_get_all_reader_option_names();//TODO: same as above
    public static int SetReaderOption(string name, string val) => NativeMethods.f3d_engine_set_reader_option(name, val);
    //public static void DestroyBackendList(BackendInfo backends) => NativeMethods.f3d_engine_destroy_backend_list(backends);
    //public static LibInfo GetLibInfo() => NativeMethods.f3d_engine_get_lib_info();
    //public static void DestroyLibInfo(LibInfo info) => NativeMethods.f3d_engine_destroy_lib_info(info);
    //public static ReaderInfo GetReadersInfo(out int count) => NativeMethods.f3d_engine_get_readers_info(out count);
    //public static void DestroyReadersInfo(ReaderInfo readers) => NativeMethods.f3d_engine_destroy_readers_info(readers);
    public static void DestroyStringArray(string[] array) => NativeMethods.f3d_engine_destroy_string_array(array);

    public Options Options { get; }
    public Scene Scene { get; }
    public Interactor Interactor { get; }

    public void Dispose()
    {
        if (_handle == null) return;
        NativeMethods.f3d_engine_destroy(_handle);
        _handle = null;
        Interactor.ReleaseCallbacks(); // F3D can no longer call them
    }
}
