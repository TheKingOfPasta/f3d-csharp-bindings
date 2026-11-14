using System.Text;
using F3D.Native;

namespace F3D;

public sealed unsafe class Scene
{
    readonly f3d_scene_t* _handle;

    internal Scene(f3d_scene_t* handle) => _handle = handle;

    public Scene Add(string filePath)
    {
        {
        if (NativeMethods.f3d_scene_add(_handle, filePath) == 0) // 1 = success, 0 = failure
            throw new F3DException($"Could not load: {filePath}");
        }
        return this;
    }
    public Scene AddMultiple(string[] filePaths)
    {
        {
        if (NativeMethods.f3d_scene_add_multiple(_handle, filePaths, (nuint)filePaths.Length) == 0) // 1 = success, 0 = failure
            throw new F3DException($"Could not load: {filePaths}");
        }
        return this;
    }

    public static implicit operator f3d_scene_t*(Scene s)
    {
        return s._handle;
    }
}
