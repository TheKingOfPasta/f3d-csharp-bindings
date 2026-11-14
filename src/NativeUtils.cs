using System.Runtime.InteropServices;
using System.Text;
using F3D.Native;

namespace F3D;

internal static unsafe class NativeUtils
{
    internal static byte[] EncodeNullTerminated(string value)
    {
        byte[] encoded = Encoding.UTF8.GetBytes(value);
        Array.Resize(ref encoded, encoded.Length + 1);
        return encoded;
    }

    internal static string TakeString(sbyte* value, string errorMessage)
    {
        if (value == null)
            throw new F3DException(errorMessage);

        try
        {
            return Marshal.PtrToStringUTF8((IntPtr)value)
                ?? throw new F3DException(errorMessage);
        }
        finally
        {
            NativeMethods.f3d_utils_string_destroy(value);
        }
    }
}
