using System.Text;
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

    public static Context Glx() => new(NativeMethods.f3d_context_glx());

    public static Context Wgl() => new(NativeMethods.f3d_context_wgl());

    public static Context Cocoa() => new(NativeMethods.f3d_context_cocoa());

    public static Context Egl() => new(NativeMethods.f3d_context_egl());

    public static Context Osmesa() => new(NativeMethods.f3d_context_osmesa());

    public static Context GetSymbol(string lib, string func)
    {
        if (lib == null) throw new ArgumentNullException(nameof(lib));
        if (func == null) throw new ArgumentNullException(nameof(func));
        fixed (byte* libPtr = Encoding.UTF8.GetBytes(lib + '\0'))
        {
            fixed (byte* funcPtr = Encoding.UTF8.GetBytes(func + '\0'))
            {
                return new(NativeMethods.f3d_context_get_symbol((sbyte*)libPtr, (sbyte*)funcPtr));
            }
        }
    }

    public void Dispose()
    {
        if (_handle == null) return;
        NativeMethods.f3d_context_destroy(_handle);
        _handle = null;
    }
}
