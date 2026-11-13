using System.Text;
using F3D.Native;

namespace F3D;

public sealed unsafe class Scene
{
    readonly f3d_scene_t* _handle;

    internal Scene(f3d_scene_t* handle) => _handle = handle;

    public Scene Add(string filePath)
    {
        fixed (byte* path = Encoding.UTF8.GetBytes(filePath + '\0'))
        {
            if (NativeMethods.f3d_scene_add(_handle, (sbyte*)path) == 0) // 1 = success, 0 = failure
                throw new F3DException($"Could not load: {filePath}");
        }
        return this;
    }
}
