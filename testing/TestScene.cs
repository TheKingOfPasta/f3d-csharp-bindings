using F3D;

static class TestScene
{
    public static void Run(string[] args)
    {
        Engine.AutoloadPlugins();
        using var engine = Engine.Create(true);

        try
        {
            engine.Scene.Add("invalid/path/to/file.obj");
            throw new Exception("Adding a missing file should throw");
        }
        catch (F3DException) { }
    }
}
