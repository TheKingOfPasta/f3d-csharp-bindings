using F3D.Native;

namespace F3D;

public unsafe class Camera
{
    public class CameraState
    {
        /// <summary>
        /// Camera position [x, y, z].
        /// </summary>
        public double[] Position;

        /// <summary>
        /// Camera focal point [x, y, z].
        /// </summary>
        public double[] FocalPoint;

        /// <summary>
        /// Camera view up vector [x, y, z].
        /// </summary>
        public double[] ViewUp;

        /// <summary>
        /// Camera view angle in degrees.
        /// </summary>
        public double ViewAngle;

        /// <summary>
        /// Create a camera state with default values.
        /// </summary>
        public CameraState()
        {
            Position = new[] { 0.0, 0.0, 1.0 };
            FocalPoint = new[] { 0.0, 0.0, 0.0 };
            ViewUp = new[] { 0.0, 1.0, 0.0 };
            ViewAngle = 30.0;
        }

        /// <summary>Create a camera state with specified values.</summary>
        /// <param name="position">camera position [x, y, z]</param>
        /// <param name="focalPoint">camera focal point [x, y, z]</param>
        /// <param name="viewUp">camera view up vector [x, y, z]</param>
        /// <param name="viewAngle">camera view angle in degrees</param>
        public CameraState(double[] position, double[] focalPoint, double[] viewUp, double viewAngle)
        {
            Position = position;
            FocalPoint = focalPoint;
            ViewUp = viewUp;
            ViewAngle = viewAngle;
        }
    }

    private readonly f3d_camera_t* _camera;

    /// <summary>
    /// Wrap an existing native camera pointer (f3d_camera_t*).
    /// </summary>
    public Camera(IntPtr nativeHandle)
    {
        if (nativeHandle == IntPtr.Zero)
            throw new ArgumentNullException(nameof(nativeHandle));
        _camera = (f3d_camera_t*)nativeHandle;
    }

    /// <summary>
    /// Native pointer to the underlying f3d_camera_t.
    /// </summary>
    public IntPtr Handle => (IntPtr)_camera;

    /// <summary>Set the position of the camera.</summary>
    /// <param name="pos">array of 3 doubles [x, y, z]</param>
    public Camera SetPosition(double[] pos)
    {
        CheckVec3(pos, nameof(pos));
        fixed (double* p = pos)
            NativeMethods.f3d_camera_set_position(_camera, p);
        return this;
    }

    /// <summary>Get the position of the camera.</summary>
    /// <returns>array of 3 doubles [x, y, z]</returns>
    public double[] GetPosition()
    {
        var pos = new double[3];
        fixed (double* p = pos)
            NativeMethods.f3d_camera_get_position(_camera, p);
        return pos;
    }

    /// <summary>Set the focal point of the camera.</summary>
    /// <param name="foc">array of 3 doubles [x, y, z]</param>
    public Camera SetFocalPoint(double[] foc)
    {
        CheckVec3(foc, nameof(foc));
        fixed (double* p = foc)
            NativeMethods.f3d_camera_set_focal_point(_camera, p);
        return this;
    }

    /// <summary>Get the focal point of the camera.</summary>
    /// <returns>array of 3 doubles [x, y, z]</returns>
    public double[] GetFocalPoint()
    {
        var foc = new double[3];
        fixed (double* p = foc)
            NativeMethods.f3d_camera_get_focal_point(_camera, p);
        return foc;
    }

    /// <summary>Set the view up vector of the camera.</summary>
    /// <param name="up">array of 3 doubles [x, y, z]</param>
    public Camera SetViewUp(double[] up)
    {
        CheckVec3(up, nameof(up));
        fixed (double* p = up)
            NativeMethods.f3d_camera_set_view_up(_camera, p);
        return this;
    }

    /// <summary>Get the view up vector of the camera.</summary>
    /// <returns>array of 3 doubles [x, y, z]</returns>
    public double[] GetViewUp()
    {
        var up = new double[3];
        fixed (double* p = up)
            NativeMethods.f3d_camera_get_view_up(_camera, p);
        return up;
    }

    /// <summary>Set the view angle in degrees of the camera.</summary>
    /// <param name="angle">view angle in degrees</param>
    public Camera SetViewAngle(double angle)
    {
        NativeMethods.f3d_camera_set_view_angle(_camera, angle);
        return this;
    }

    /// <summary>
    /// Get the view angle in degrees of the camera.
    /// </summary>
    public double GetViewAngle() => NativeMethods.f3d_camera_get_view_angle(_camera);

    /// <summary>Set the complete state of the camera.</summary>
    /// <param name="state">camera state to set</param>
    public Camera SetState(CameraState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        CheckVec3(state.Position, nameof(state.Position));
        CheckVec3(state.FocalPoint, nameof(state.FocalPoint));
        CheckVec3(state.ViewUp, nameof(state.ViewUp));

        var native = new f3d_camera_state_t { view_angle = state.ViewAngle };
        state.Position.CopyTo((Span<double>)native.position);
        state.FocalPoint.CopyTo((Span<double>)native.focal_point);
        state.ViewUp.CopyTo((Span<double>)native.view_up);

        NativeMethods.f3d_camera_set_state(_camera, &native);
        return this;
    }

    /// <summary>Get the complete state of the camera.</summary>
    /// <returns>current camera state</returns>
    public CameraState GetState()
    {
        f3d_camera_state_t native;
        NativeMethods.f3d_camera_get_state(_camera, &native);
        ReadOnlySpan<double> pos = native.position;
        ReadOnlySpan<double> fp = native.focal_point;
        ReadOnlySpan<double> up = native.view_up;
        return new CameraState(pos.ToArray(), fp.ToArray(), up.ToArray(), native.view_angle);
    }

    /// <summary>Divide the camera's distance from the focal point by the given value.</summary>
    /// <param name="val">dolly factor</param>
    public Camera Dolly(double val)
    {
        NativeMethods.f3d_camera_dolly(_camera, val);
        return this;
    }

    /// <summary>Move the camera along its horizontal, vertical, and forward axes.</summary>
    /// <param name="right">distance to move right</param>
    /// <param name="up">distance to move up</param>
    /// <param name="forward">distance to move forward (default 0)</param>
    public Camera Pan(double right, double up, double forward = 0.0)
    {
        NativeMethods.f3d_camera_pan(_camera, right, up, forward);
        return this;
    }

    /// <summary>Decrease the view angle (or the parallel scale in parallel mode) by the specified factor.</summary>
    /// <param name="factor">zoom factor</param>
    public Camera Zoom(double factor)
    {
        NativeMethods.f3d_camera_zoom(_camera, factor);
        return this;
    }

    /// <summary>Rotate the camera about its forward axis.</summary>
    /// <param name="angle">rotation angle in degrees</param>
    public Camera Roll(double angle)
    {
        NativeMethods.f3d_camera_roll(_camera, angle);
        return this;
    }

    /// <summary>Rotate the camera about its vertical axis, centered at the focal point.</summary>
    /// <param name="angle">rotation angle in degrees</param>
    public Camera Azimuth(double angle)
    {
        NativeMethods.f3d_camera_azimuth(_camera, angle);
        return this;
    }

    /// <summary>Rotate the camera about its vertical axis, centered at the camera's position.</summary>
    /// <param name="angle">rotation angle in degrees</param>
    public Camera Yaw(double angle)
    {
        NativeMethods.f3d_camera_yaw(_camera, angle);
        return this;
    }

    /// <summary>Rotate the camera about its horizontal axis, centered at the focal point.</summary>
    /// <param name="angle">rotation angle in degrees</param>
    public Camera Elevation(double angle)
    {
        NativeMethods.f3d_camera_elevation(_camera, angle);
        return this;
    }

    /// <summary>Rotate the camera about its horizontal axis, centered at the camera's position.</summary>
    /// <param name="angle">rotation angle in degrees</param>
    public Camera Pitch(double angle)
    {
        NativeMethods.f3d_camera_pitch(_camera, angle);
        return this;
    }

    /// <summary>
    /// Store the current camera configuration as default.
    /// </summary>
    public Camera SetCurrentAsDefault()
    {
        NativeMethods.f3d_camera_set_current_as_default(_camera);
        return this;
    }

    /// <summary>
    /// Reset the camera to the stored default camera configuration.
    /// </summary>
    public Camera ResetToDefault()
    {
        NativeMethods.f3d_camera_reset_to_default(_camera);
        return this;
    }

    /// <summary>
    /// Reset the camera using the bounds of actors in the scene.
    /// Provided zoomFactor will be used to position the camera.
    /// A value of 1 corresponds to the bounds roughly aligned to the edges of the window.
    /// </summary>
    /// <param name="zoomFactor">zoom factor (default 0.9)</param>
    public Camera ResetToBounds(double zoomFactor = 0.9)
    {
        NativeMethods.f3d_camera_reset_to_bounds(_camera, zoomFactor);
        return this;
    }

    private static void CheckVec3(double[] v, string name)
    {
        ArgumentNullException.ThrowIfNull(v, name);
        if (v.Length != 3)
            throw new ArgumentException("Expected an array of 3 doubles [x, y, z].", name);
    }
}
