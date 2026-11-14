using System.Runtime.InteropServices;
using System.Text;
using F3D.Native;

namespace F3D;

using SaveFormat = F3D.Native.f3d_image_save_format_t;
using ChannelType = F3D.Native.f3d_image_channel_type_t;

public sealed unsafe class Image : IDisposable, IEquatable<Image>
{
    /// <summary>
    /// Thrown when an image file cannot be read (unsupported format, file not found, etc.).
    /// </summary>
    public class ReadException : F3DException
    {
        public ReadException(string message) : base(message) { }
    }

    /// <summary>
    /// Thrown when an image cannot be written/saved.
    /// </summary>
    public class WriteException : F3DException
    {
        public WriteException(string message) : base(message) { }
    }

    /// <summary>
    /// Thrown when a metadata key does not exist on the image.
    /// </summary>
    public class MetadataException : F3DException
    {
        public MetadataException(string message) : base(message) { }
    }

    private f3d_image* _handle;

    public f3d_image* Handle
    {
        get
        {
            if (_handle == null)
                throw new ObjectDisposedException(nameof(Image));
            return _handle;
        }
    }

    /// <summary>
    /// Read provided file path into a new image instance.
    /// </summary>
    /// <param name="filePath">Path to image file.</param>
    public Image(string filePath)
    {
        ArgumentNullException.ThrowIfNull(filePath);
        fixed (byte* pPath = Encoding.UTF8.GetBytes(filePath + '\0'))
        {
            _handle = NativeMethods.f3d_image_create_path((sbyte*)pPath);
        }

        if (_handle == null)
            throw new ReadException($"Could not read image file: '{filePath}'");
    }

    /// <summary>
    /// Create an image from given width, height, and channel count.
    /// </summary>
    /// <param name="width">Image width.</param>
    /// <param name="height">Image height.</param>
    /// <param name="channelCount">Number of channels.</param>
    /// <param name="type">Channel type.</param>
    public Image(uint width, uint height, uint channelCount, ChannelType type)
    {
        _handle = NativeMethods.f3d_image_create_params(width, height, channelCount, type);
        if (_handle == null)
            throw new F3DException($"Could not create image ({width}x{height}, {channelCount} channels).");
    }

    /// <summary>
    /// Create an image from given width, height, and channel count with BYTE type.
    /// </summary>
    /// <param name="width">Image width.</param>
    /// <param name="height">Image height.</param>
    /// <param name="channelCount">Number of channels.</param>
    public Image(uint width, uint height, uint channelCount) 
        : this(width, height, channelCount, ChannelType.BYTE)
    {
    }

    /// <summary>
    /// Create an image from given stream buffer.
    /// </summary>
    /// <param name="buffer">Stream buffer.</param>
    public Image(ReadOnlySpan<byte> buffer)
    {
        fixed (byte* pBuf = buffer)
        {
            _handle = NativeMethods.f3d_image_create_stream(pBuf, (uint)buffer.Length);
        }

        if (_handle == null)
            throw new ReadException("Could not create image from stream buffer.");
    }

    /// <summary>
    /// Create an empty image instance.
    /// </summary>
    public Image()
    {
        _handle = NativeMethods.f3d_image_create_empty();
        if (_handle == null)
            throw new F3DException("Could not create empty image.");
    }

    internal Image(f3d_image* handle)
    {
        if (handle == null)
            throw new ArgumentNullException(nameof(handle));
        _handle = handle;
    }

    /// <summary>
    /// Get image width.
    /// </summary>
    public uint Width => NativeMethods.f3d_image_get_width(Handle);

    /// <summary>
    /// Get image height.
    /// </summary>
    public uint Height => NativeMethods.f3d_image_get_height(Handle);

    /// <summary>
    /// Get image channel count.
    /// </summary>
    public uint ChannelCount => NativeMethods.f3d_image_get_channel_count(Handle);

    /// <summary>
    /// Get image channel type.
    /// </summary>
    public ChannelType ChannelType => (ChannelType)NativeMethods.f3d_image_get_channel_type(Handle);

    /// <summary>
    /// Get image channel type size in bytes.
    /// </summary>
    public uint ChannelTypeSize => NativeMethods.f3d_image_get_channel_type_size(Handle);

    /// <summary>
    /// Read one specific pixel and return all channel normalized values.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <returns>Normalized pixel values [0, 1].</returns>
    public double[] GetNormalizedPixel(int x, int y)
    {
        uint channels = ChannelCount;
        double[] pixel = new double[channels];

        fixed (double* p = pixel)
        {
            int res = NativeMethods.f3d_image_get_normalized_pixel(Handle, x, y, p);
            if (res == 0)
                throw new F3DException($"Invalid pixel coordinates: ({x}, {y})");
        }

        return pixel;
    }

    /// <summary>
    /// Get image buffer data.
    /// </summary>
    /// <returns>Byte array containing image data.</returns>
    public byte[] GetContent()
    {
        void* pData = NativeMethods.f3d_image_get_content(Handle);
        if (pData == null)
            return Array.Empty<byte>();

        int size = (int)(Width * Height * ChannelCount * ChannelTypeSize);
        byte[] result = new byte[size];
        Marshal.Copy((IntPtr)pData, result, 0, size);
        return result;
    }

    /// <summary>
    /// Set image buffer data.
    /// </summary>
    /// <param name="buffer">Byte buffer containing image data.</param>
    /// <returns>This image for method chaining.</returns>
    public Image SetContent(ReadOnlySpan<byte> buffer)
    {
        fixed (byte* pBuf = buffer)
        {
            NativeMethods.f3d_image_set_content(Handle, pBuf);
        }
        return this;
    }

    /// <summary>
    /// Get the list of supported image format extensions.
    /// </summary>
    /// <returns>List of supported formats.</returns>
    public static List<string> GetSupportedFormats()
    {
        uint count = NativeMethods.f3d_image_get_supported_formats_count();
        sbyte** formats = NativeMethods.f3d_image_get_supported_formats();
        var list = new List<string>((int)count);

        for (int i = 0; i < count; i++)
        {
            string? ext = Marshal.PtrToStringUTF8((IntPtr)formats[i]);
            if (ext != null) list.Add(ext);
        }

        return list;
    }

    /// <summary>
    /// Convert to colored text using ANSI escape sequences.
    /// </summary>
    /// <returns>ANSI colored text representation.</returns>
    public string ToTerminalText()
    {
        sbyte* pStr = NativeMethods.f3d_image_to_terminal_text_string(Handle);
        return Marshal.PtrToStringUTF8((IntPtr)pStr) ?? string.Empty;
    }

    /// <summary>
    /// Save an image to the provided file path in the specified format.
    /// </summary>
    /// <param name="filePath">Path to save image.</param>
    /// <param name="format">Save format.</param>
    /// <returns>This image for method chaining.</returns>
    public Image Save(string filePath, SaveFormat format)
    {
        ArgumentNullException.ThrowIfNull(filePath);
        fixed (byte* pPath = Encoding.UTF8.GetBytes(filePath + '\0'))
        {
            int res = NativeMethods.f3d_image_save(Handle, (sbyte*)pPath, format);
            if (res == 0)
                throw new WriteException($"Failed to save image to '{filePath}'");
        }
        return this;
    }

    /// <summary>
    /// Save an image to the provided file path in PNG format.
    /// </summary>
    /// <param name="filePath">Path to save image.</param>
    /// <returns>This image for method chaining.</returns>
    public Image Save(string filePath) => Save(filePath, SaveFormat.PNG);

    /// <summary>
    /// Save an image to a memory buffer in the specified format.
    /// </summary>
    /// <param name="format">Save format.</param>
    /// <returns>Byte array containing saved image.</returns>
    public byte[] SaveBuffer(SaveFormat format)
    {
        uint size = 0;
        byte* pBuf = NativeMethods.f3d_image_save_buffer(Handle, format, &size);

        if (pBuf == null || size == 0)
            throw new WriteException("Failed to save image to memory buffer.");

        try
        {
            byte[] data = new byte[size];
            Marshal.Copy((IntPtr)pBuf, data, 0, (int)size);
            return data;
        }
        finally
        {
            NativeMethods.f3d_image_destroy_buffer(pBuf);
        }
    }

    /// <summary>
    /// Save an image to a memory buffer in PNG format.
    /// </summary>
    /// <returns>Byte array containing saved image.</returns>
    public byte[] SaveBuffer() => SaveBuffer(SaveFormat.PNG);

    /// <summary>
    /// Set the value for a metadata key.
    /// </summary>
    /// <param name="key">Metadata key.</param>
    /// <param name="value">Metadata value.</param>
    /// <returns>This image for method chaining.</returns>
    public Image SetMetadata(string key, string value)
    {
        fixed (byte* pKey = Encoding.UTF8.GetBytes(key + '\0'))
        fixed (byte* pVal = Encoding.UTF8.GetBytes(value + '\0'))
        {
            NativeMethods.f3d_image_set_metadata(Handle, (sbyte*)pKey, (sbyte*)pVal);
        }
        return this;
    }

    /// <summary>
    /// Get the value for a metadata key.
    /// </summary>
    /// <param name="key">Metadata key.</param>
    /// <returns>Metadata value.</returns>
    public string GetMetadata(string key)
    {
        fixed (byte* pKey = Encoding.UTF8.GetBytes(key + '\0'))
        {
            sbyte* pVal = NativeMethods.f3d_image_get_metadata(Handle, (sbyte*)pKey);
            if (pVal == null)
                throw new MetadataException($"Metadata key not found: '{key}'");

            return Marshal.PtrToStringUTF8((IntPtr)pVal) ?? string.Empty;
        }
    }

    /// <summary>
    /// List all metadata keys which have a value set.
    /// </summary>
    /// <returns>List of metadata keys.</returns>
    public List<string> AllMetadata()
    {
        uint count = 0;
        sbyte** pKeys = NativeMethods.f3d_image_all_metadata(Handle, &count);
        if (pKeys == null || count == 0)
            return new List<string>();

        try
        {
            var keys = new List<string>((int)count);
            for (int i = 0; i < count; i++)
            {
                string? k = Marshal.PtrToStringUTF8((IntPtr)pKeys[i]);
                if (k != null) keys.Add(k);
            }
            return keys;
        }
        finally
        {
            NativeMethods.f3d_image_destroy_metadata_keys(pKeys, count);
        }
    }

    /// <summary>
    /// Compare current image to a reference.
    /// </summary>
    /// <param name="reference">Reference image.</param>
    /// <returns>Error value (0.0 = identical, 1.0 = completely different).</returns>
    public double Compare(Image reference)
    {
        ArgumentNullException.ThrowIfNull(reference);
        return NativeMethods.f3d_image_compare(Handle, reference.Handle);
    }

    /// <summary>
    /// Compare images for equality using a threshold of 1e-14.
    /// </summary>
    /// <param name="other">Reference image.</param>
    /// <returns>True if images are equal within threshold, false otherwise.</returns>
    public bool Equals(Image? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return NativeMethods.f3d_image_equals(Handle, other.Handle) != 0;
    }

    public override bool Equals(object? obj) => obj is Image other && Equals(other);

    public override int GetHashCode() => ((IntPtr)Handle).GetHashCode();

    public static bool operator ==(Image? left, Image? right) => left?.Equals(right) ?? right is null;

    public static bool operator !=(Image? left, Image? right) => !(left == right);

    /// <summary>
    /// Delete the image and free native resources.
    /// After calling this method, the image should not be used anymore.
    /// </summary>
    public void Dispose()
    {
        if (_handle == null) return;
        NativeMethods.f3d_image_destroy(_handle);
        _handle = null;
    }
}