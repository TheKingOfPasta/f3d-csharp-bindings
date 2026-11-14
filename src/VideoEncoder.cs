using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using F3D.Native;

namespace F3D;

public sealed unsafe class VideoEncoder : IDisposable
{
    /// <summary>
    /// Thrown when codec initialization fails.
    /// </summary>
    public class CodecException : F3DException
    {
        public CodecException(string message) : base(message) { }
    }

    /// <summary>
    /// Thrown when data transport fails.
    /// </summary>
    public class TransportException : F3DException
    {
        public TransportException(string message) : base(message) { }
    }

    /// <summary>
    /// Enumeration of codec types.
    /// Supported codecs include H264, HEVC, VP8, VP9, AV1, and EXPLICIT for specifying a codec name.
    /// H264 is widely supported and commonly used for video encoding.
    /// HEVC (also called H265) offers better compression efficiency than H264 but may have less
    /// widespread support.
    /// VP8 and VP9 are commonly used for web-based video applications.
    /// AV1 is a modern codec with high compression efficiency but may require
    /// more computational resources.
    /// </summary>
    public enum Codec : uint
    {
        H264,
        HEVC,
        VP8,
        VP9,
        AV1,
        Explicit,
    }

    /// <summary>
    /// Parameters for configuring the video stream construction.
    /// ExplicitCodecName is used when the codec is set to EXPLICIT.
    /// Use getAvailableEncoders() to retrieve the list of available codec names.
    /// Bitrate is specified in Mbps.
    /// </summary>
    public class Params
    {
        public Codec Codec = Codec.H264;
        public string? ExplicitCodecName;
        public int Width;
        public int Height;
        public double FrameRate = 30;
        public double Bitrate = 5;
        public bool LowLatency = false;
    }

    /// <summary>
    /// Represents a pair of strings, used for codec name and description.
    /// </summary>
    public record StringPair(string First, string Second);

    f3d_video_encoder_t* _handle;

    // Listeners given to F3D. All of them are kept alive until the encoder is destroyed
    readonly List<GCHandle> _listeners = new();

    /// <summary>
    /// Create a video encoder with the given parameters.
    /// Throws CodecException if the codec cannot be initialized.
    /// </summary>
    /// <param name="params">encoder parameters</param>
    public VideoEncoder(Params @params)
    {
        ArgumentNullException.ThrowIfNull(@params);

        // explicit_codec_name is a struct field, so it is not converted by the generated bindings.
        var name = @params.ExplicitCodecName is null ? 0 : Marshal.StringToCoTaskMemUTF8(@params.ExplicitCodecName);
        try
        {
            var native = new f3d_video_encoder_params_t
            {
                codec = (f3d_video_encoder_codec_t)@params.Codec,
                explicit_codec_name = (sbyte*)name,
                width = @params.Width,
                height = @params.Height,
                frame_rate = @params.FrameRate,
                bitrate = @params.Bitrate,
                low_latency = @params.LowLatency ? 1 : 0,
            };
            _handle = NativeMethods.f3d_video_encoder_create(&native);
        }
        finally
        {
            Marshal.FreeCoTaskMem(name);
        }

        if (_handle == null) throw new CodecException("Could not initialize the video codec.");
    }

    /// <summary>
    /// Get the list of available codecs.
    /// Returns a list of pair of String (name, description) representing the available codecs.
    /// </summary>
    public static List<StringPair> GetAvailableEncoders()
    {
        sbyte** names = null;
        sbyte** descriptions = null;
        int count = 0;
        NativeMethods.f3d_video_encoder_get_available_encoders(&names, &descriptions, &count);

        var list = new List<StringPair>(count);
        if (names == null || descriptions == null) return list;
        for (int i = 0; i < count; i++)
            list.Add(new(
                Marshal.PtrToStringUTF8((nint)names[i]) ?? "",
                Marshal.PtrToStringUTF8((nint)descriptions[i]) ?? ""));
        NativeMethods.f3d_video_encoder_free_available_encoders(names, descriptions, count);
        return list;
    }

    /// <summary>
    /// Get the width of the video stream.
    /// </summary>
    /// <returns>width</returns>
    public int GetWidth() => NativeMethods.f3d_video_encoder_get_width(_handle);

    /// <summary>
    /// Get the height of the video stream.
    /// </summary>
    /// <returns>height</returns>
    public int GetHeight() => NativeMethods.f3d_video_encoder_get_height(_handle);

    /// <summary>
    /// Asynchronously listen for encoded video packets and invoke the provided listener for each
    /// packet. Thread safety is ensured, allowing concurrent calls to submit and listen.
    /// </summary>
    /// <param name="listener">packet listener</param>
    /// <returns>this video encoder for method chaining</returns>
    public VideoEncoder Listen(Action<VideoPacket> listener)
    {
        ArgumentNullException.ThrowIfNull(listener);
        var handle = GCHandle.Alloc(listener);
        lock (_listeners) _listeners.Add(handle);
        NativeMethods.f3d_video_encoder_listen(_handle, &OnPacket, (void*)GCHandle.ToIntPtr(handle));
        return this;
    }

    /// <summary>
    /// Send a frame to the video stream for encoding.
    /// Returns true if the frame was successfully submitted, false if the encoder is busy and
    /// cannot accept new frames at the moment. Throws TransportException if there is an error
    /// submitting the frame to the encoder.
    /// </summary>
    /// <param name="frame">video frame to submit</param>
    /// <returns>true if submitted, false otherwise</returns>
    public bool Submit(VideoFrame frame)
    {
        ArgumentNullException.ThrowIfNull(frame);
        int result = NativeMethods.f3d_video_encoder_submit(_handle, frame.Handle);
        if (result < 0) throw new TransportException("Could not submit the frame to the video encoder.");
        return result == 1;
    }

    /// <summary>
    /// Flush the video encoder, ensuring all encoded frames are processed.
    /// Signals the end of the stream to the encoder, allowing it to output any remaining packets.
    /// </summary>
    /// <returns>this video encoder for method chaining</returns>
    public VideoEncoder Flush()
    {
        NativeMethods.f3d_video_encoder_flush(_handle);
        return this;
    }

    /// <summary>
    /// Delete the video encoder and free native resources.
    /// After calling this method, the video encoder should not be used anymore.
    /// </summary>
    public void Dispose()
    {
        if (_handle == null) return;
        NativeMethods.f3d_video_encoder_destroy(_handle);
        _handle = null;

        // F3D can no longer call the listeners.
        lock (_listeners)
        {
            foreach (var handle in _listeners) handle.Free();
            _listeners.Clear();
        }
    }

    // Called from the encoder background thread. Exceptions are logged as F3D errors
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    static void OnPacket(f3d_video_packet_t* packet, void* userData)
    {
        try
        {
            var listener = (Action<VideoPacket>)GCHandle.FromIntPtr((nint)userData).Target!;
            listener(new VideoPacket(packet));
        }
        catch (Exception e)
        {
            NativeMethods.f3d_log_error($"Exception in video packet listener: {e}");
        }
    }
}
