using F3D;

static class TestVideoEncoder
{
    public static void Run(string[] args)
    {
        using var engine = Engine.Create(true);

        var window = engine.Window ?? throw new Exception("Failed to get window");
        window.Render();

        // get available encoders
        var encoders = VideoEncoder.GetAvailableEncoders();
        for (int i = 0; i < encoders.Count; ++i)
            Console.WriteLine($"Encoder {i}: {encoders[i].First} - {encoders[i].Second}");

        // video
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

        // The constructor throws CodecException if the encoder cannot be created
        long ts = 0;
        using (var encoder = new VideoEncoder(@params))
        {
            // Called from the encoder background thread
            encoder.Listen(packet =>
            {
                _ = packet.GetPacketData();
                Interlocked.Exchange(ref ts, packet.GetTimestamp());
            });

            using (var videoFrame = window.GetVideoFrame())
            {
                videoFrame.SetTimestamp(42);
                encoder.Submit(videoFrame);
            }

            encoder.Flush();

            // C# only: Flush ends the stream, so submitting again throws
            using (var videoFrame = window.GetVideoFrame())
            {
                try
                {
                    encoder.Submit(videoFrame);
                    throw new Exception("Submitting a frame after Flush should throw");
                }
                catch (VideoEncoder.TransportException) { }
            }
        }

        if (Interlocked.Read(ref ts) != 42)
            throw new Exception("Video packet callback was not called with the expected timestamp");
    }
}
