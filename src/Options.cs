using System.Text;
using F3D.Native;

namespace F3D;

public sealed unsafe class Options
{
    readonly f3d_options_t* _handle;

    internal Options(f3d_options_t* handle) => _handle = handle;

    public void SetAsBool(string name, bool value)
    {
        NativeMethods.f3d_options_set_as_bool(_handle, name, value ? 1 : 0);
    }

    public static implicit operator f3d_options_t*(Options opt)
    {
        return opt._handle;
    }

}
