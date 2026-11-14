using F3D;

// C# port of f3d's c/testing/test_animation.c
static class TestAnimation
{
    public static void Run(string[] args)
    {
        Engine.AutoloadPlugins();
        using var engine = Engine.Create(true);

        // Add an animated file (throws F3DException on failure)
        engine.Scene.Add(Path.Combine(args[0], "data", "soldier_animations.mdl"));

        // Test the animation API
        Animation anim = engine.Scene.Animation;

        anim.LoadTime(0.5);

        var (minTime, maxTime) = anim.TimeRange;
        Check(minTime == 0 && Math.Abs(maxTime - 0.8) < 1e-10,
            "Failed to recover expected time range");

        Check(anim.Count == 10, "Failed to recover expected animation count");

        double[] keyFrames = anim.KeyFrames;
        Check(keyFrames.Length == 9 && Math.Abs(keyFrames[1] - 0.1) < 1e-10,
            "Failed to recover expected animation key_frames");

        Check(anim.CurrentName == "stand", "Failed to recover expected current animation name");

        Check(anim.GetName(1) == "dead", "Failed to recover expected animation name");

        IReadOnlyList<string> names = anim.Names;
        Check(names.Count == 10 && names[1] == "dead", "Failed to recover expected animation names");

        anim.Toggle(AnimationDirection.Forward);
        Check(anim.IsPlaying, "Failed to recover expected playing value after toggle");

        Check(anim.Direction == AnimationDirection.Forward,
            "Failed to recover expected direction value after toggle forward");

        anim.Stop();
        Check(!anim.IsPlaying, "Failed to recover expected playing value after stop");

        anim.Start(AnimationDirection.Backward);
        Check(anim.IsPlaying, "Failed to recover expected playing value after start");

        Check(anim.Direction == AnimationDirection.Backward,
            $"Failed to recover expected direction value after toggle backward ({anim.Direction})");
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
            throw new Exception(message);
    }
}
