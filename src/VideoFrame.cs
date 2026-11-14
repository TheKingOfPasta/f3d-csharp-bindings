using F3D.Native;

namespace F3D;

/// <summary>
/// A video frame captured from a window, to be submitted to a video encoder.
/// </summary>
public sealed unsafe class VideoFrame : IDisposable
{
    /// <summary>Thrown when video frame allocation fails.</summary>
    public class InvalidFrameException : F3DException
    {
        public InvalidFrameException(string message) : base(message) { }
    }

    private f3d_video_frame_t* _handle;

    /// <summary>Native handle of the video frame.</summary>
    public f3d_video_frame_t* Handle
    {
        get
        {
            if (_handle == null)
                throw new ObjectDisposedException(nameof(VideoFrame));
            return _handle;
        }
    }

    /// <summary>
    /// Wrap a native video frame. Takes ownership of the handle.
    /// Should not be called directly, use Window.GetVideoFrame() instead.
    /// </summary>
    internal VideoFrame(f3d_video_frame_t* nativeHandle)
    {
        if (nativeHandle == null)
            throw new InvalidFrameException("Could not allocate video frame.");
        _handle = nativeHandle;
    }

    /// <summary>
    /// Wrap a native video frame from an IntPtr. Takes ownership of the handle.
    /// </summary>
    internal VideoFrame(IntPtr nativeHandle) : this((f3d_video_frame_t*)nativeHandle) { }

    /// <summary>Set the timestamp of the video frame.</summary>
    /// <param name="timestamp">Timestamp value.</param>
    /// <returns>This video frame for method chaining.</returns>
    public VideoFrame SetTimestamp(long timestamp)
    {
        NativeMethods.f3d_video_frame_set_timestamp(Handle, timestamp);
        return this;
    }

    /// <summary>
    /// Delete the video frame and free native resources.
    /// After calling this method, the video frame must not be used anymore.
    /// </summary>
    public void Dispose()
    {
        if (_handle == null) return;
        NativeMethods.f3d_video_frame_destroy(_handle);
        _handle = null;
        GC.SuppressFinalize(this);
    }

    ~VideoFrame()
    {
        if (_handle != null)
            NativeMethods.f3d_video_frame_destroy(_handle);
    }
}
