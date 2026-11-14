using System.Runtime.InteropServices;
using F3D.Native;

namespace F3D;

/// <summary>
/// Represents a video packet borrowed from an encoder callback. The packet is only valid
/// for the duration of that callback and must not be stored or disposed.
/// </summary>
public sealed unsafe class VideoPacket
{
    private f3d_video_packet_t* _handle;

    public f3d_video_packet_t* Handle
    {
        get
        {
            if (_handle == null)
                throw new ObjectDisposedException(nameof(VideoPacket));
            return _handle;
        }
    }

    public VideoPacket(f3d_video_packet_t* handle)
    {
        if (handle == null)
            throw new ArgumentNullException(nameof(handle));
        _handle = handle;
    }

    /// <summary>
    /// Get the data of the video packet.
    /// </summary>
    /// <returns>packet data</returns>
    public byte[] GetPacketData()
    {
        nuint size = NativeMethods.f3d_video_packet_get_packet_size(Handle);
        if (size == 0)
            return [];

        int length = checked((int)size);
        byte* data = NativeMethods.f3d_video_packet_get_packet_data(Handle);
        if (data == null)
            throw new F3DException("Could not retrieve video packet data.");

        byte[] result = new byte[length];
        Marshal.Copy((IntPtr)data, result, 0, length);
        return result;
    }

    /// <summary>
    /// Get the timestamp of the video packet.
    /// </summary>
    /// <returns>timestamp value</returns>
    public long GetTimestamp() => NativeMethods.f3d_video_packet_get_timestamp(Handle);

    /// <summary>
    /// Check if the video packet is a key frame.
    /// </summary>
    /// <returns>true if the packet is a key frame, false otherwise</returns>
    public bool IsKeyFrame() => NativeMethods.f3d_video_packet_is_key_frame(Handle) != 0;
}
