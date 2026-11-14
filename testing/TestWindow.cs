using System.Diagnostics;
using System.Runtime.InteropServices;
using F3D;

// Port of f3d/c/testing/test_window.c
static class TestWindow
{
    public static void Run(string[] args)
    {
        using var engine = Engine.Create(true);

        Window window = engine.Window
            ?? throw new Exception("[ERROR] Failed to get window");

        window.SetUseHDRICache(true);
        double dpi_scale = window.DPIScale;
        Debug.Assert(dpi_scale >= 1.0);
        var type = window.Type;
        bool offscreen = window.IsOffscreen;

        Camera camera = window.Camera;

        window.Render();
        using var img = window.RenderToImage();

        window.SetSize(800,600);
        int width = window.Width;
        int height = window.Height;
        var (sizeWidth, sizeHeight) = window.Size;

        Debug.Assert(sizeWidth == width);
        Debug.Assert(sizeHeight == height);

        window.SetPosition(100,100);
        window.Render();
        var (posX, posY) = window.Position;
        var (posX2, posY2) = window.Position;

        Debug.Assert(posX == posX2);
        Debug.Assert(posY == posY2);

        Debug.Assert(window.Left == posX);
        Debug.Assert(window.Top == posY);

        Byte[] b = { 0xFF, 0xFF, 0xFF, 0xFF };

        ReadOnlySpan<Byte> icon = b;
        window.SetIcon(icon);
        window.SetWindowName("Test Window");

        double[] display_point = { 400.0, 300.0, 0.0 };
        double[] world_point = window.GetWorldFromDisplay(display_point);

        double[] test_world = { 0.0, 0.0, 0.0 };
        double[] display_out = window.GetDisplayFromWorld(test_world);
    }
}
