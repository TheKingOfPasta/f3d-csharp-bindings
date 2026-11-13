using F3D.Native;

namespace F3D;

public sealed unsafe class Interactor
{
    readonly f3d_interactor_t* _handle;

    internal Interactor(f3d_interactor_t* handle) => _handle = handle;

    // Opens the window. Returns when the window is closed.
    public void Start(double deltaTime = 1.0 / 30.0) => NativeMethods.f3d_interactor_start(_handle, deltaTime);
}
