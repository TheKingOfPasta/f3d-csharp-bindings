using F3D;

static class TestOptions
{
    public static void Run(string[] args)
    {
        using var engine = Engine.Create(true);
        engine.Options.SetAsBool("render.grid.enable", true);
    }
}
