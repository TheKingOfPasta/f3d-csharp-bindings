using F3D;

static class TestEngine
{
    public static void Run(string[] args)
    {
        Engine.AutoloadPlugins();

        using var engine = Engine.Create(true);
        if (engine.Options is null || engine.Scene is null || engine.Interactor is null)
            throw new Exception("Engine should give access to options, scene and interactor");
    }
}
