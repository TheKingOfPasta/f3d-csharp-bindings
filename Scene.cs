using System.Runtime.InteropServices;

namespace F3D;

public sealed partial class Scene
{
    readonly nint _handle;

    internal Scene(nint handle) => _handle = handle;

    public Scene Add(string filePath)
    {
        if (f3d_scene_add(_handle, filePath) == 0)
            throw new F3DException($"Could not load: {filePath}");
        return this;
    }

    [LibraryImport(Engine.Lib, StringMarshalling = StringMarshalling.Utf8)]
    private static partial int f3d_scene_add(nint scene, string filePath); // 1 = success, 0 = failure
}
