using F3D;

static class TestInteractor
{
    public static void Run(string[] args)
    {
        using var engine = Engine.Create(true);
        _ = engine.Interactor; // Start() opens a window and blocks, so it is not called here
    }
}
