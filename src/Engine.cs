using System.Runtime.InteropServices;
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

    /// <summary>
    /// Create an engine with automatic window selection.
    /// The engine will choose the best available windowing system for the current platform.
    /// If `offscreen` is set to true, the window will be hidden and rendering will be done offscreen.
    /// </summary>
    /// <param name="offscreen">if true, window will be hidden</param>
    /// <returns>new Engine instance</returns>
    public static Engine Create(bool offscreen = false) => new(NativeMethods.f3d_engine_create(offscreen ? 1 : 0));

    /// <summary>
    /// Create an engine with a GLX window (Linux only)
    /// </summary>
    /// <param name="offscreen">if true, window will be hidden</param>
    /// <returns>new Engine instance</returns>
    public static Engine CreateGLX(bool offscreen = false) => new(NativeMethods.f3d_engine_create_glx(offscreen ? 1 : 0));

    /// <summary>
    /// Create an engine with a WGL window (Windows only)
    /// </summary>
    /// <param name="offscreen">if true, window will be hidden</param>
    /// <returns>new Engine instance</returns>
    public static Engine CreateWGL(bool offscreen = false) => new(NativeMethods.f3d_engine_create_wgl(offscreen ? 1 : 0));

    /// <summary>
    /// Create an engine with an offscreen EGL window
    /// </summary>
    /// <returns>new Engine instance</returns>
    public static Engine CreateEGL() => new(NativeMethods.f3d_engine_create_egl());

    /// <summary>
    /// Create an engine with an offscreen OSMesa window
    /// </summary>
    /// <returns>new Engine instance</returns>
    public static Engine CreateOSMesa() => new(NativeMethods.f3d_engine_create_osmesa());
    //public static Engine CreateExternal(Delegate del) => new(NativeMethods.f3d_engine_create_external(del));// TODO: Tester celui la (type faux)

    /// <summary>
    /// Create an engine with an external GLX context (Linux only)
    /// </summary>
    /// <returns>new Engine instance</returns>
    public static Engine CreateExternalGLX() => new(NativeMethods.f3d_engine_create_external_glx());

    /// <summary>
    /// Create an engine with an external WGL context (Windows only)
    /// </summary>
    /// <returns>new Engine instance</returns>
    public static Engine CreateExternalWGL() => new(NativeMethods.f3d_engine_create_external_wgl());

    /// <summary>
    /// Create an engine with an external COCOA context (macOS only)
    /// </summary>
    /// <returns>new Engine instance</returns>
    public static Engine CreateExternalCOCOA() => new(NativeMethods.f3d_engine_create_external_cocoa());

    /// <summary>
    /// Create an engine with an external EGL context
    /// </summary>
    /// <returns>new Engine instance</returns>
    public static Engine CreateExternalEGL() => new(NativeMethods.f3d_engine_create_external_egl());

    /// <summary>
    /// Create an engine with an external OSMesa context
    /// </summary>
    /// <returns>new Engine instance</returns>
    public static Engine CreateExternalOSMesa() => new(NativeMethods.f3d_engine_create_external_osmesa());

    /// <summary>
    /// Set the cache path directory
    /// </summary>
    /// <param name="cachePath">path to cache directory</param>
    /// <returns>0 on success, -1 on failure</returns>
    public int SetCachePath(string cache_path) => NativeMethods.f3d_engine_set_cache_path(this, cache_path);

    /// <summary>
    /// Get the cache path directory
    /// </summary>
    /// <returns>path to cache directory</returns>
    public string GetCachePath()
    {
        sbyte* resBytes = NativeMethods.f3d_engine_get_cache_path(this);

        string res = "";

        for (int i = 0; resBytes[i] != 0; i++)
            res += resBytes[i];

        return res;
    }

    /// <summary>
    /// Set options for this engine
    /// </summary>
    /// <param name="options">the options to set</param>
    public void SetOptions(Options opt) => NativeMethods.f3d_engine_get_options(this);
    public string DumpToString()
    {
        sbyte* resBytes = NativeMethods.f3d_engine_dump_to_string(this);

        string res = "";

        for (int i = 0; resBytes[i] != 0; i++)
            res += resBytes[i];

        return res;
    }
    public int DumpToFile(string file_path) => NativeMethods.f3d_engine_dump_to_file(this, file_path);
    public int DumpToClipboard() => NativeMethods.f3d_engine_dump_to_clipboard(this);
    public int LoadFromString(string content) => NativeMethods.f3d_engine_load_from_string(this, content);
    public int LoadFromFile(string file_path) => NativeMethods.f3d_engine_load_from_file(this, file_path);
    public int LoadFromClipboard() => NativeMethods.f3d_engine_load_from_clipboard(this);

    //public static BackendInfo GetRenderingBackendList(out int count) => NativeMethods.f3d_engine_get_rendering_backend_list(out count);// TODO: int* count => ??? in cs

    /// <summary>
    /// Load a plugin by path or name
    /// </summary>
    /// <param name="plugin">path or name of the plugin</param>
    /// <returns>0 on success, -1 on failure</returns>
    public static int LoadPlugin(string path_or_name) => NativeMethods.f3d_engine_load_plugin(path_or_name);

    /// <summary>
    /// Automatically load all static plugins
    /// </summary>
    public static void AutoloadPlugins() => NativeMethods.f3d_engine_autoload_plugins();

    /// <summary>
    /// List plugins available in the given directory
    /// </summary>
    /// <param name="pluginPath">path to search for plugins</param>
    /// <returns>list of available plugin names</returns>
    public string[] GetPluginsList(string plugin_path)
    {
        sbyte** resAsBytes = NativeMethods.f3d_engine_get_plugins_list(plugin_path);
        List<string> resList = new List<string>();
        for (int i = 0; resAsBytes[i] != null; i++)
        {
            string? tmp = Marshal.PtrToStringUTF8((IntPtr)resAsBytes[i]);
            if (tmp == null)
                continue;

            resList.Add(tmp!);
        }

        return resList.ToArray();
    }

    /// <summary>
    /// Get all reader option names
    /// </summary>
    /// <returns>list of option names</returns>
    public static string[] GetAllReaderOptionNames()
    {
        sbyte** resAsBytes = NativeMethods.f3d_engine_get_all_reader_option_names();
        List<string> resList = new List<string>();
        for (int i = 0; resAsBytes[i] != null; i++)
        {
            string? tmp = Marshal.PtrToStringUTF8((IntPtr)resAsBytes[i]);
            if (tmp == null)
                continue;

            resList.Add(tmp!);
        }

        return resList.ToArray();
    }

    /// <summary>
    /// Set a reader option value
    /// </summary>
    /// <param name="name">option name</param>
    /// <param name="val">option value</param>
    /// <returns>0 on success, -1 on failure</returns>
    public static int SetReaderOption(string name, string val) => NativeMethods.f3d_engine_set_reader_option(name, val);
    //public static void DestroyBackendList(BackendInfo backends) => NativeMethods.f3d_engine_destroy_backend_list(backends);
    //public static LibInfo GetLibInfo() => NativeMethods.f3d_engine_get_lib_info();
    //public static void DestroyLibInfo(LibInfo info) => NativeMethods.f3d_engine_destroy_lib_info(info);
    //public static ReaderInfo GetReadersInfo(out int count) => NativeMethods.f3d_engine_get_readers_info(out count);
    //public static void DestroyReadersInfo(ReaderInfo readers) => NativeMethods.f3d_engine_destroy_readers_info(readers);

    public Options Options { get => new(NativeMethods.f3d_engine_get_options(this)); }
    public Scene Scene { get => new(NativeMethods.f3d_engine_get_scene(this)); }
    public Interactor Interactor { get => new(NativeMethods.f3d_engine_get_interactor(this)); }

    public void Dispose()
    {
        if (_handle == null) return;
        NativeMethods.f3d_engine_destroy(_handle);
        _handle = null;
        Interactor.ReleaseCallbacks(); // F3D can no longer call them
    }
}
