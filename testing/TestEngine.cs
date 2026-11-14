using F3D;

static class TestEngine
{
    public static void Run(string[] args)
    {
        Engine.AutoloadPlugins();

        string temporaryDirectory = Path.Combine(Path.GetTempPath(), $"f3d-engine-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);

        try
        {
            TestEngineAccessAndCachePath(temporaryDirectory);
            TestPluginsAndReaderOptions();
            TestStateFileRoundTrip(temporaryDirectory);
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    private static void TestEngineAccessAndCachePath(string temporaryDirectory)
    {
        using var engine = Engine.Create(true);
        if (engine.Options is null || engine.Scene is null || engine.Interactor is null || engine.Window is null)
            throw new Exception("Engine should give access to options, scene, window and interactor.");

        string cachePath = Path.Combine(temporaryDirectory, "cache");
        if (engine.SetCachePath(cachePath) != 1)
            throw new Exception("Setting the cache path should succeed.");
        string actualCachePath = engine.GetCachePath();
        if (actualCachePath != cachePath)
            throw new Exception($"GetCachePath returned '{actualCachePath}' instead of '{cachePath}'.");

        using var options = new Options();
        options.SetAsBool("ui.scalar_bar", true);
        engine.SetOptions(options);
        if (!engine.Options.GetAsBool("ui.scalar_bar"))
            throw new Exception("SetOptions should copy option values into the engine.");
    }

    private static void TestPluginsAndReaderOptions()
    {
        Engine.LoadPlugin("native");
        Engine.LoadPlugin("inexistent_plugin");

        using var engine = Engine.Create(true);
        string[] plugins = engine.GetPluginsList("inexistent");
        if (plugins.Length != 0)
            throw new Exception("An invalid plugin directory should not return plugins.");

        Engine.SetReaderOption("invalid.option", "value");
        Engine.SetReaderOption("QuakeMDL.skin_index", "0");

        _ = Engine.GetAllReaderOptionNames();
    }

    private static void TestStateFileRoundTrip(string temporaryDirectory)
    {
        string meshPath = Path.Combine(temporaryDirectory, "engine_test.obj");
        File.WriteAllText(meshPath, """
            o Triangle
            v 0 0 0
            v 1 0 0
            v 0 1 0
            f 1 2 3
            """);

        string stateFilePath = Path.Combine(temporaryDirectory, "engine_state.json");
        using var engine = Engine.Create(true);
        engine.Options.SetAsBool("ui.scalar_bar", true);
        engine.Scene.Add(meshPath);

        string state = engine.DumpToString();
        if (string.IsNullOrWhiteSpace(state))
            throw new Exception("DumpToString should return the engine state.");

        using (var restoredFromString = Engine.Create(true))
        {
            if (restoredFromString.LoadFromString(state) != 1)
                throw new Exception("Loading a valid state string should succeed.");
            if (!restoredFromString.Options.GetAsBool("ui.scalar_bar"))
                throw new Exception("Options should be restored from the state string.");
            if (!restoredFromString.DumpToString().Contains(Path.GetFileName(meshPath), StringComparison.Ordinal))
                throw new Exception("Scene should be restored from the state string.");
        }

        if (engine.DumpToFile(stateFilePath) != 1)
            throw new Exception("DumpToFile should succeed.");

        using (var restoredFromFile = Engine.Create(true))
        {
            if (restoredFromFile.LoadFromFile(stateFilePath) != 1)
                throw new Exception("Loading a valid state file should succeed.");
            if (!restoredFromFile.Options.GetAsBool("ui.scalar_bar"))
                throw new Exception("Options should be restored from the state file.");
            if (!restoredFromFile.DumpToString().Contains(Path.GetFileName(meshPath), StringComparison.Ordinal))
                throw new Exception("Scene should be restored from the state file.");
        }

        if (engine.LoadFromFile(Path.Combine(temporaryDirectory, "missing-state.json")) != 0)
            throw new Exception("Loading a missing state file should fail.");
        if (engine.LoadFromString("{ not valid json") != 0)
            throw new Exception("Loading invalid state content should fail.");
    }

}
