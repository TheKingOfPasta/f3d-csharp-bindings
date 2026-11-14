using System.Runtime.InteropServices;
using System.Text;
using F3D.Native;

namespace F3D;

public sealed unsafe class Scene
{
    readonly f3d_scene_t* _handle;

    public Animation Animation { get; }

    internal Scene(f3d_scene_t* handle)
    {
        _handle = handle;
        Animation = new Animation(NativeMethods.f3d_scene_get_animation(handle));
    }

    public static implicit operator f3d_scene_t*(Scene s)
    {
        return s._handle;
    }

    /// <summary>
    /// Adds and loads a file into the scene.
    /// </summary>
    /// <param name="filePath">file path to add</param>
    /// <returns>this scene for method chaining</returns>
    public Scene Add(string filePath)
    {
        if (NativeMethods.f3d_scene_add(_handle, filePath) == 0) // 1 = success, 0 = failure
            throw new F3DException($"Could not load: {filePath}");

        return this;
    }

    /// <summary>
    /// Add and load multiple files into the scene.
    /// </summary>
    /// <param name="file_paths">list of file paths to add</param>
    /// <param name="count">number of files to add</param>
    /// <returns>this scene for method chaining</returns>
    public Scene AddMultiple(string[] file_paths, nuint count)
    {
        if (NativeMethods.f3d_scene_add_multiple(this, file_paths, count) == 0) throw new F3DException("add_multiple function returned 0");
        return this;
    }

    /// <summary>
    /// Add and load a mesh into the scene.
    /// </summary>
    /// <param name="mesh">mesh to add</param>
    /// <returns>this scene for method chaining</returns>
    public Scene AddMesh(f3d_mesh_t* mesh)
    {
        if (NativeMethods.f3d_scene_add_mesh(this, mesh) == 0) throw new F3DException("add_mesh function returned 0");
        return this;
    }
    public Scene AddMeshView(f3d_mesh_view_t* mesh_view)
    {
        if (NativeMethods.f3d_scene_add_mesh_view(this, mesh_view) == 0) throw new F3DException("add_mesh_view function returned 0");
        return this;
    }

    /// <summary>
    /// Add and load a buffer containing a file into the scene.
    /// </summary>
    /// <param name="buffer">Memory buffer to load</param>
    /// <param name="size">Size of the buffer</param>
    /// <returns>this scene for method chaining</returns>
    public Scene AddBuffer(byte[] buffer, nuint size)
    {
        fixed (byte* buffBytes = buffer)
        {
            if (NativeMethods.f3d_scene_add_buffer(this, buffBytes, size) == 0) throw new F3DException("add_buffer function returned 0");

            return this;
        }
    }

    public Scene Add(string[] file_paths, nuint count) => AddMultiple(file_paths, count);
    public Scene Add(string[] file_paths) => AddMultiple(file_paths, (nuint)file_paths.Length);
    public Scene Add(f3d_mesh_t* mesh) => AddMesh(mesh);
    public Scene Add(f3d_mesh_view_t* mesh_view) => AddMeshView(mesh_view);
    public Scene Add(byte[] buffer, nuint size) => AddBuffer(buffer, size);
    public Scene Add(byte[] buffer) => AddBuffer(buffer, (nuint)buffer.Length);


    /// <summary>
    /// Clear the scene of all added files.
    /// </summary>
    /// <returns>this scene for method chaining</returns>
    public Scene Clear()
    {
        NativeMethods.f3d_scene_clear(this);
        return this;
    }

    /// <summary>
    /// Get the list of files currently added to the scene.
    /// </summary>
    /// <param name="count">number of files in the returned list</param>
    /// <returns>list of added file paths</returns>
    public string[] GetAddedFiles(out uint count)
    {
        uint c = 0;
        sbyte** resAsBytes = NativeMethods.f3d_scene_get_added_files(this, &c);
        string[] res = new string[c];
        for (int i = 0; i < c; i++)
        {
            string? tmp = Marshal.PtrToStringUTF8((IntPtr)resAsBytes[i]);
            if (tmp == null)
                continue;

            res[i] = tmp!;
        }

        count = c;

        return res;
    }

    /// <summary>
    /// Destroy the list of added files.
    /// </summary>
    /// <param name="files">list of added file paths</param>
    /// <param name="count">number of files in the list</param>
    public static void DestroyAddedFiles(string[] files, uint count)
    {
        sbyte** result = (sbyte**)NativeMemory.Alloc((nuint)count, (nuint)sizeof(sbyte*));

        for (int i = 0; i < count; i++)
            result[i] = (sbyte*)Marshal.StringToCoTaskMemUTF8(files[i]);

        NativeMethods.f3d_scene_destroy_added_files(result, count);

        for (int i = 0; i < count; i++)
            Marshal.FreeCoTaskMem((IntPtr)result[i]);

        NativeMemory.Free(result);
    }

    /// <summary>
    /// Add a light based on a light state.
    /// </summary>
    /// <param name="light_state">light state to add</param>
    /// <returns>index of the added light</returns>
    public int AddLight(f3d_light_state_t* light_state) => NativeMethods.f3d_scene_add_light(this, light_state);

    /// <summary>
    /// Get the number of lights in the scene.
    /// </summary>
    /// <returns>number of lights in the scene</returns>
    public int GetLightCount() => NativeMethods.f3d_scene_get_light_count(this);

    /// <summary>
    /// Get the light state at provided index.
    /// </summary>
    /// <param name="index">index of the light</param>
    /// <returns>light state</returns>
    public f3d_light_state_t* GetLight(int index) => NativeMethods.f3d_scene_get_light(this, index);

    /// <summary>
    /// Update a light at provided index with the provided light state.
    /// </summary>
    /// <param name="index">index of the light to update</param>
    /// <param name="light_state">new light state</param>
    /// <returns>this scene for method chaining</returns>
    public Scene UpdateLight(int index, f3d_light_state_t* light_state)
    {
        if (NativeMethods.f3d_scene_update_light(this, index, light_state) == 0) throw new F3DException("update_light function returned 0");
        return this;
    }

    /// <summary>
    /// Remove a light at provided index.
    /// </summary>
    /// <param name="index">index of the light to remove</param>
    /// <returns>this scene for method chaining</returns>
    public Scene RemoveLight(int index)
    {
        if (NativeMethods.f3d_scene_remove_light(this, index) == 0) throw new F3DException("remove_light function returned 0");
        return this;
    }

    /// <summary>
    /// Remove all lights from the scene.
    /// </summary>
    /// <returns>this scene for method chaining</returns>
    public Scene RemoveAllLights()
    {
        NativeMethods.f3d_scene_remove_all_lights(this);
        return this;
    }

    /// <summary>
    /// Get the scene hierarchy of all added files, in depth-first pre-order, so that a parent
    /// node always precedes its children.
    /// </summary>
    /// <param name="count">number of nodes in the returned list</param>
    /// <returns>the list of scene hierarchy nodes</returns>
    public f3d_node_state_t* GetSceneHierarchy(out uint count)
    {
        uint c = 0;
        var res = NativeMethods.f3d_scene_get_scene_hierarchy(this, &c);
        count = c;

        return res;
    }
    public static void DestroySceneHierarchy(f3d_node_state_t* nodes, uint count) => NativeMethods.f3d_scene_destroy_scene_hierarchy(nodes, count);

    /// <summary>
    /// Set the visibility of a scene hierarchy node and of all the nodes in its subtree.
    /// </summary>
    /// <param name="node_id">index of the node</param>
    /// <param name="visible">visibility to set</param>
    /// <returns>this scene for method chaining</returns>
    public Scene SetNodeVisibility(int node_id, int visible)
    {
        if (NativeMethods.f3d_scene_set_node_visibility(this, node_id, visible) == 0) throw new F3DException("set_node_visibility function returned 0");
        return this;
    }

    /// <summary>
    /// Get information about the contents of the scene, eg. its number of points and cells.
    /// All the counters are zero when the scene is empty.
    /// </summary>
    /// <param name="info">pointer to the scene information structure</param>
    /// <returns>0 on success, non-zero on failure</returns>
    public int GetSceneInfo(f3d_scene_info_t* info) => NativeMethods.f3d_scene_get_scene_info(this, info);

    /// <summary>
    /// Check if a file path is supported by the scene.
    /// </summary>
    /// <param name="file_path">file path to check</param>
    /// <returns>file availability</returns>
    public int Supports(string file_path) => NativeMethods.f3d_scene_supports(this, file_path);

    /// <summary>
    /// Check if a memory buffer is supported by the scene.
    /// 
    /// throws IllegalArgumentException if buffer is null
    /// </summary>
    /// <param name="buffer">memory buffer to check</param>
    /// <param name="size">size of the memory buffer</param>
    /// <returns>file availability</returns>
    public int SupportsBuffer(byte[] buffer, nuint size)
    {
        fixed (byte* buffBytes = buffer)
        {
            return NativeMethods.f3d_scene_supports_buffer(this, buffBytes, size);
        }
    }
    public int Supports(byte[] buffer, nuint size) => SupportsBuffer(buffer, size);
    public int Supports(byte[] buffer) => SupportsBuffer(buffer, (nuint)buffer.Length);

    /// <summary>
    /// Get the animation
    /// </summary>
    /// <returns>Animation instance</returns>
    public f3d_animation_t* GetAnimation() => NativeMethods.f3d_scene_get_animation(this);
}
