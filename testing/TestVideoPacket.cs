using F3D;

static class TestVideoPacket
{
    public static void Run(string[] args)
    {
        TestHandle();
        TestNullHandle();
    }

    private static unsafe void TestHandle()
    {
        var nativeHandle = (F3D.Native.f3d_video_packet_t*)1;
        var packet = new VideoPacket(nativeHandle);
        if (packet.Handle != nativeHandle)
            throw new Exception("VideoPacket should return its native handle.");
    }

    private static unsafe void TestNullHandle()
    {
        try
        {
            _ = new VideoPacket(null);
            throw new Exception("Creating a video packet with a null handle should throw.");
        }
        catch (ArgumentNullException) { }
    }
}
