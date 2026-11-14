using F3D;

// C# port of f3d's c/testing/test_log.c
static class TestLog
{
    public static void Run(string[] args)
    {
        LogVerboseLevel initialLevel = Log.VerboseLevel;

        Log.SetVerboseLevel(LogVerboseLevel.Debug);
        Check(Log.VerboseLevel == LogVerboseLevel.Debug, "Expected LogVerboseLevel.Debug");

        Log.SetVerboseLevel(LogVerboseLevel.Quiet);
        Check(Log.VerboseLevel == LogVerboseLevel.Quiet, "Expected LogVerboseLevel.Quiet");

        Log.SetVerboseLevel(LogVerboseLevel.Info, forceStdErr: true);
        Check(Log.VerboseLevel == LogVerboseLevel.Info, "Expected LogVerboseLevel.Info with forceStdErr");

        Log.SetVerboseLevel(initialLevel);

        Log.SetUseColoring(true);
        Log.SetUseColoring(false);

        LogVerboseLevel savedLevel = Log.VerboseLevel;
        Log.SetVerboseLevel(LogVerboseLevel.Quiet);

        Log.Print(LogVerboseLevel.Debug, "Test debug message");
        Log.Print(LogVerboseLevel.Info, "Test info message");
        Log.Print(LogVerboseLevel.Warn, "Test warning message");
        Log.Print(LogVerboseLevel.Error, "Test error message");

        Log.Debug("Test debug message");
        Log.Info("Test info message");
        Log.Warn("Test warning message");
        Log.Error("Test error message");

        // Forwarding: callback always called
        int callbackCount = 0;
        LogVerboseLevel lastLevel = LogVerboseLevel.Info;
        string lastMessage = "";

        Log.Forward((level, message) =>
        {
            callbackCount++;
            lastLevel = level;
            lastMessage = message;
        });
        Log.Print(LogVerboseLevel.Warn, "Test callback message");

        Check(callbackCount == 1, "Callback should have been called once");
        Check(lastLevel == LogVerboseLevel.Warn, "Expected callback level LogVerboseLevel.Warn");
        Check(lastMessage == "Test callback message", "Expected callback message 'Test callback message'");

        // Each helper forwards with its own level
        Log.Error("Test error callback");
        Check(callbackCount == 2 && lastLevel == LogVerboseLevel.Error && lastMessage == "Test error callback",
            "Expected Log.Error to be forwarded with LogVerboseLevel.Error");

        // A throwing callback must not crash the process (caught in Log.Forwarder)
        Log.Forward((_, _) => throw new InvalidOperationException("Test exception from callback"));
        Log.Info("Test throwing callback");

        // Disabling forwarding: the previous callback is no longer called
        Log.Forward(null);
        int countBefore = callbackCount;
        Log.Warn("Test message after forward disabled");
        Check(callbackCount == countBefore, "Callback should not be called after Forward(null)");

        Log.SetVerboseLevel(savedLevel);

        // The C test passes NULL messages; the C# wrappers reject null instead
        CheckThrows<ArgumentNullException>(() => Log.Debug(null!));
        CheckThrows<ArgumentNullException>(() => Log.Info(null!));
        CheckThrows<ArgumentNullException>(() => Log.Warn(null!));
        CheckThrows<ArgumentNullException>(() => Log.Error(null!));
        CheckThrows<ArgumentNullException>(() => Log.Print(LogVerboseLevel.Info, null!));
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
            throw new Exception(message);
    }

    private static void CheckThrows<TException>(Action action)
        where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            return;
        }
        throw new Exception($"Expected {typeof(TException).Name}");
    }
}
