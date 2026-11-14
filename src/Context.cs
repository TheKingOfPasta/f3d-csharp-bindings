using F3D.Native;

namespace F3D;

public sealed unsafe class Context : IDisposable
{
    f3d_context* _handle;

    Context(f3d_context* handle)
    {
        if (handle == null) throw new F3DException("Could not create the F3D context.");
        _handle = handle;
    }

    /// <summary>
    /// Create a GLX context.
    /// </summary>
    /// <returns>context handle (must be deleted with <see cref="Dispose"/>)</returns>
    public static Context Glx() => new(NativeMethods.f3d_context_glx());

    /// <summary>
    /// Create a WGL context.
    /// </summary>
    /// <returns>context handle (must be deleted with <see cref="Dispose"/>)</returns>
    public static Context Wgl() => new(NativeMethods.f3d_context_wgl());

    /// <summary>
    /// Create a COCOA context.
    /// </summary>
    /// <returns>context handle (must be deleted with <see cref="Dispose"/>)</returns>
    public static Context Cocoa() => new(NativeMethods.f3d_context_cocoa());

    /// <summary>
    /// Create an EGL context.
    /// </summary>
    /// <returns>context handle (must be deleted with <see cref="Dispose"/>)</returns>
    public static Context Egl() => new(NativeMethods.f3d_context_egl());

    /// <summary>
    /// Create an OSMesa context.
    /// </summary>
    /// <returns>context handle (must be deleted with <see cref="Dispose"/>)</returns>
    public static Context Osmesa() => new(NativeMethods.f3d_context_osmesa());

    /// <summary>
    /// Create a context from a library name and a function name.
    /// </summary>
    /// <param name="lib">the library name (without prefix or extension)</param>
    /// <param name="func">the function name to resolve</param>
    /// <returns>context handle (must be deleted with <see cref="Dispose"/>)</returns>
    public static Context GetSymbol(string lib, string func)
    {
        if (lib == null) throw new ArgumentNullException(nameof(lib));
        if (func == null) throw new ArgumentNullException(nameof(func));
        return new(NativeMethods.f3d_context_get_symbol(lib, func));
    }

    /// <summary>
    /// Delete a context and free associated resources.
    /// </summary>
    public void Dispose()
    {
        if (_handle == null) return;
        NativeMethods.f3d_context_destroy(_handle);
        _handle = null;
    }
}
