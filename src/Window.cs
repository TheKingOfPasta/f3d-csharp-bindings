using F3D.Native;

namespace F3D;

public enum WindowType : uint
{
    None = f3d_window_type_t.F3D_WINDOW_NONE,
    External = f3d_window_type_t.F3D_WINDOW_EXTERNAL,
    Glx = f3d_window_type_t.F3D_WINDOW_GLX,
    Wgl = f3d_window_type_t.F3D_WINDOW_WGL,
    Cocoa = f3d_window_type_t.F3D_WINDOW_COCOA,
    Egl = f3d_window_type_t.F3D_WINDOW_EGL,
    OsMesa = f3d_window_type_t.F3D_WINDOW_OSMESA,
    Wasm = f3d_window_type_t.F3D_WINDOW_WASM,
    Unknown = f3d_window_type_t.F3D_WINDOW_UNKNOWN,
}

public sealed unsafe class Window
{
    readonly f3d_window_t* _handle;
    Camera? _camera;

    internal Window(f3d_window_t* handle) => _handle = handle;

    /// <summary>
    /// Get the type of the window.
    /// </summary>
    /// <value>window type</value>
    public WindowType Type => (WindowType)NativeMethods.f3d_window_get_type(_handle);

    /// <summary>
    /// Check if the window is offscreen.
    /// </summary>
    /// <value>true if offscreen, false otherwise</value>
    public bool IsOffscreen => NativeMethods.f3d_window_is_offscreen(_handle) != 0;

    /// <summary>
    /// Get the camera provided by the window.
    /// </summary>
    /// <value>camera instance</value>
    public Camera Camera => _camera ??= new Camera((IntPtr)NativeMethods.f3d_window_get_camera(_handle));

    /// <summary>
    /// Perform a render of the window to the screen.
    /// All dynamic options are updated if needed.
    /// </summary>
    /// <returns>true on success, false otherwise</returns>
    public bool Render() => NativeMethods.f3d_window_render(_handle) != 0;

    /// <summary>
    /// Perform a render of the window to the screen and save the result in an image.
    /// Set noBackground to true to have a transparent background.
    /// </summary>
    /// <remarks>
    /// The returned image must be disposed (e.g. <c>using var image = window.RenderToImage();</c>).
    /// </remarks>
    /// <param name="noBackground">if true, background will be transparent</param>
    /// <returns>resulting image</returns>
    public Image RenderToImage(bool noBackground = false)
    {
        f3d_image* image = NativeMethods.f3d_window_render_to_image(_handle, noBackground ? 1 : 0);
        if (image == null)
            throw new F3DException("Could not render the window to an image.");
        return new Image(image);
    }

    /// <summary>
    /// Get the current video frame of the window.
    /// </summary>
    /// <remarks>
    /// The returned video frame must be disposed (e.g. <c>using var frame = window.GetVideoFrame();</c>).
    /// </remarks>
    /// <returns>video frame</returns>
    public VideoFrame GetVideoFrame()
    {
        f3d_video_frame_t* frame = NativeMethods.f3d_window_get_video_frame(_handle);
        if (frame == null)
            throw new F3DException("Could not get the video frame of the window.");
        return new VideoFrame(frame);
    }

    /// <summary>
    /// Set the size of the window.
    /// </summary>
    /// <param name="width">window width</param>
    /// <param name="height">window height</param>
    /// <returns>this window for method chaining</returns>
    public Window SetSize(int width, int height)
    {
        NativeMethods.f3d_window_set_size(_handle, width, height);
        return this;
    }

    /// <summary>
    /// Get the size of the window as an array {width, height}.
    /// </summary>
    /// <value>window size as {width, height}</value>
    public (int Width, int Height) Size
    {
        get
        {
            int width, height;
            NativeMethods.f3d_window_get_size(_handle, &width, &height);
            return (width, height);
        }
    }

    /// <summary>
    /// Get the width of the window.
    /// </summary>
    /// <value>window width</value>
    public int Width => NativeMethods.f3d_window_get_width(_handle);

    /// <summary>
    /// Get the height of the window.
    /// </summary>
    /// <value>window height</value>
    public int Height => NativeMethods.f3d_window_get_height(_handle);

    /// <summary>
    /// Set the position of the window.
    /// </summary>
    /// <param name="x">x position</param>
    /// <param name="y">y position</param>
    /// <returns>this window for method chaining</returns>
    public Window SetPosition(int x, int y)
    {
        NativeMethods.f3d_window_set_position(_handle, x, y);
        return this;
    }

    /// <summary>
    /// Get the position of the window as an array {x, y}.
    /// </summary>
    /// <value>window position as {x, y}</value>
    public (int X, int Y) Position
    {
        get
        {
            int x, y;
            NativeMethods.f3d_window_get_position(_handle, &x, &y);
            return (x, y);
        }
    }

    /// <summary>
    /// Get the position of the left border of the window.
    /// </summary>
    /// <value>window left border position</value>
    public int Left => NativeMethods.f3d_window_get_left(_handle);

    /// <summary>
    /// Get the position of the top border of the window.
    /// </summary>
    /// <value>window top border position</value>
    public int Top => NativeMethods.f3d_window_get_top(_handle);

    /// <summary>
    /// Set the icon to be shown by a window manager.
    /// </summary>
    /// <param name="icon">icon data as byte array</param>
    /// <returns>this window for method chaining</returns>
    public Window SetIcon(ReadOnlySpan<byte> icon)
    {
        fixed (byte* data = icon)
        {
            NativeMethods.f3d_window_set_icon(_handle, data, (nuint)icon.Length);
        }
        return this;
    }

    /// <summary>
    /// Set the window name to be shown by a window manager.
    /// </summary>
    /// <param name="windowName">window name</param>
    /// <returns>this window for method chaining</returns>
    public Window SetWindowName(string windowName)
    {
        ArgumentNullException.ThrowIfNull(windowName);
        NativeMethods.f3d_window_set_window_name(_handle, windowName);
        return this;
    }

    /// <summary>
    /// C API reads/writes exactly 3 doubles
    /// </summary>
    /// <param name="point">double table</param>
    /// <param name="paramName"></param>
    /// <exception cref="ArgumentException"></exception>
    static void CheckPoint3(double[] point, string paramName)
    {
        ArgumentNullException.ThrowIfNull(point, paramName);
        if (point.Length != 3)
            throw new ArgumentException("Expected an array of 3 doubles [x, y, z].", paramName);
    }

    /// <summary>
    /// Convert a point in display coordinate to world coordinate.
    /// </summary>
    /// <param name="displayPoint">array of 3 doubles [x, y, z] in display coordinates</param>
    /// <returns>array of 3 doubles [x, y, z] in world coordinates</returns>
    public double[] GetWorldFromDisplay(double[] displayPoint)
    {
        CheckPoint3(displayPoint, nameof(displayPoint));
        var worldPoint = new double[3];
        fixed (double* input = displayPoint)
        fixed (double* output = worldPoint)
        {
            NativeMethods.f3d_window_get_world_from_display(_handle, input, output);
        }
        return worldPoint;
    }

    /// <summary>
    /// Convert a point in world coordinate to display coordinate.
    /// </summary>
    /// <param name="worldPoint">array of 3 doubles [x, y, z] in world coordinates</param>
    /// <returns>array of 3 doubles [x, y, z] in display coordinates</returns>
    public double[] GetDisplayFromWorld(double[] worldPoint)
    {
        CheckPoint3(worldPoint, nameof(worldPoint));
        var displayPoint = new double[3];
        fixed (double* input = worldPoint)
        fixed (double* output = displayPoint)
        {
            NativeMethods.f3d_window_get_display_from_world(_handle, input, output);
        }
        return displayPoint;
    }

    /// <summary>
    /// Get the DPI scale value of the window.
    /// Returns 1.0 on platforms where DPI scaling is not supported.
    /// </summary>
    /// <value>DPI scale.</value>
    public double DPIScale => NativeMethods.f3d_window_get_dpi_scale(_handle);

    /// <summary>
    /// Enable or disable the use of HDRI cache for the window.
    /// </summary>
    /// <param name="useHDRICache">true to enable HDRI cache, false to disable</param>
    /// <returns>this window for method chaining</returns>
    public Window SetUseHDRICache(bool useHDRICache)
    {
        NativeMethods.f3d_window_set_use_hdri_cache(_handle, useHDRICache ? 1 : 0);
        return this;
    }
}
