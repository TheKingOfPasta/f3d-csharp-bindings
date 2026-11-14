using F3D;

// Port of f3d/c/testing/test_camera.c
static class TestCamera
{
    public static void Run(string[] args)
    {
        using var engine = Engine.Create(true);

        Window window = engine.Window
            ?? throw new Exception("[ERROR] Failed to get window");

        Camera camera = window.Camera
            ?? throw new Exception("[ERROR] Failed to get camera");

        camera.SetPosition(new[] { 1.0, 2.0, 3.0 });
        double[] getPos = camera.GetPosition();

        camera.SetFocalPoint(new[] { 0.0, 0.0, 0.0 });
        double[] getFocal = camera.GetFocalPoint();

        camera.SetViewUp(new[] { 0.0, 1.0, 0.0 });
        double[] getViewUp = camera.GetViewUp();

        camera.SetViewAngle(30.0);
        double angle = camera.GetViewAngle();

        var state = new Camera.CameraState(
            new[] { 5.0, 5.0, 5.0 },
            new[] { 0.0, 0.0, 0.0 },
            new[] { 0.0, 1.0, 0.0 },
            45.0);
        camera.SetState(state);
        Camera.CameraState getState = camera.GetState();

        camera.Dolly(1.5);
        camera.Pan(0.1, 0.2, 0.3);
        camera.Zoom(0.9);
        camera.Roll(10.0);
        camera.Azimuth(15.0);
        camera.Yaw(20.0);
        camera.Elevation(25.0);
        camera.Pitch(30.0);

        camera.SetCurrentAsDefault();
        camera.ResetToDefault();
        camera.ResetToBounds(0.9);
    }
}
