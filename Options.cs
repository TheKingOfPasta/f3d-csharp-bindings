using System.Runtime.InteropServices;

namespace F3D;

public sealed partial class Options
{
    readonly nint _handle;

    internal Options(nint handle) => _handle = handle;

    public void SetAsBool(string name, bool value) => f3d_options_set_as_bool(_handle, name, value ? 1 : 0);

    [LibraryImport(Engine.Lib, StringMarshalling = StringMarshalling.Utf8)]
    private static partial void f3d_options_set_as_bool(nint options, string name, int value);
}
