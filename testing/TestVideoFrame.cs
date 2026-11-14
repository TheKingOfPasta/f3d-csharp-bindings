using F3D;

static class TestVideoFrame
{
    public static void Run(string[] args)
    {
        using var engine = Engine.Create(true);

        var window = engine.Window ?? throw new Exception("Failed to get window");
        window.Render();

        TestGetVideoFrame(window);
        TestSetTimestamp(window);
        TestTimestampReachesPacket(window);
        TestDispose(window);
    }

    private static unsafe void TestGetVideoFrame(Window window)
    {
        using var frame1 = window.GetVideoFrame();
        using var frame2 = window.GetVideoFrame();

        Check(frame1.Handle != null, "Failed to get video frame");
        Check(frame1.Handle != frame2.Handle, "Each GetVideoFrame() call should give a new native frame");
    }

    private static void TestSetTimestamp(Window window)
    {
        using var frame = window.GetVideoFrame();

        Check(ReferenceEquals(frame.SetTimestamp(42), frame), "SetTimestamp should return this video frame");
        frame.SetTimestamp(0).SetTimestamp(long.MaxValue).SetTimestamp(-1);
    }

    private static void TestTimestampReachesPacket(Window window)
    {
        const long expected = 123;
        long ts = 0;

        var @params = new VideoEncoder.Params
        {
            Width = window.Width,
            Height = window.Height,
            Codec = VideoEncoder.Codec.H264,
            ExplicitCodecName = null,
            FrameRate = 30.0,
            Bitrate = 5.0,
            LowLatency = false,
        };

        using (var encoder = new VideoEncoder(@params))
        {
            // Called from the encoder background thread
            encoder.Listen(packet => Interlocked.Exchange(ref ts, packet.GetTimestamp()));

            using (var frame = window.GetVideoFrame())
            {
                frame.SetTimestamp(expected);
                encoder.Submit(frame);
            }

            encoder.Flush();
        }

        Check(Interlocked.Read(ref ts) == expected,
            $"Video packet callback was not called with the expected timestamp (got {Interlocked.Read(ref ts)})");
    }

    // C# only: Dispose can be called twice
    private static unsafe void TestDispose(Window window)
    {
        var frame = window.GetVideoFrame();
        frame.Dispose();
        frame.Dispose();

        CheckThrows<ObjectDisposedException>(() => _ = frame.Handle);
        CheckThrows<ObjectDisposedException>(() => frame.SetTimestamp(1));
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
