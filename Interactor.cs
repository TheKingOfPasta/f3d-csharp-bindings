using System.Runtime.InteropServices;

namespace F3D;

public sealed partial class Interactor
{
    readonly nint _handle;

    internal Interactor(nint handle) => _handle = handle;

    // Opens the window. Returns when the window is closed.
    public void Start(double deltaTime = 1.0 / 30.0) => f3d_interactor_start(_handle, deltaTime);

    [LibraryImport(Engine.Lib)]
    private static partial void f3d_interactor_start(nint interactor, double deltaTime);
}
