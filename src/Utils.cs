using System.Runtime.InteropServices;
using F3D.Native;

namespace F3D;

/// <summary>
/// Utility functions provided by the native F3D C API.
/// </summary>
public static unsafe class Utils
{
    /// <summary>
    /// Enumeration of supported Windows known folders.
    /// </summary>
    public enum KnownFolder
    {
        RoamingAppData = 0,
        LocalAppData = 1,
        Pictures = 2
    }

    /// <summary>
    /// Compute the Levenshtein distance between two strings.
    /// </summary>
    /// <param name="strA">first string</param>
    /// <param name="strB">second string</param>
    /// <returns>Levenshtein distance between the two strings</returns>
    public static uint TextDistance(string strA, string strB)
    {
        ArgumentNullException.ThrowIfNull(strA);
        ArgumentNullException.ThrowIfNull(strB);
        return NativeMethods.f3d_utils_text_distance(strA, strB);
    }

    /// <summary>
    /// Tokenize a string using the same logic as bash.
    /// </summary>
    /// <param name="str">input string to tokenize</param>
    /// <param name="keepComments">if true, keep comments with '#'</param>
    /// <returns>list of tokens</returns>
    public static List<string> Tokenize(string str, bool keepComments)
    {
        ArgumentNullException.ThrowIfNull(str);

        nuint count = 0;
        sbyte** tokens = NativeMethods.f3d_utils_tokenize(str, keepComments ? 1 : 0, &count);
        if (tokens == null)
            throw new F3DException("Could not tokenize the input string.");

        try
        {
            var result = new List<string>(checked((int)count));
            for (nuint i = 0; i < count; i++)
            {
                string? token = Marshal.PtrToStringUTF8((IntPtr)tokens[i]);
                if (token is null)
                    throw new F3DException($"Native tokenizer returned a null token at index {i}.");
                result.Add(token);
            }

            return result;
        }
        finally
        {
            NativeMethods.f3d_utils_tokens_destroy(tokens, count);
        }
    }

    /// <summary>
    /// Tokenize a string with keepComments enabled.
    /// </summary>
    /// <param name="str">input string to tokenize</param>
    /// <returns>list of tokens</returns>
    public static List<string> Tokenize(string str) => Tokenize(str, keepComments: true);

    /// <summary>
    /// Collapse a filesystem path.
    /// </summary>
    /// <param name="path">input path</param>
    /// <param name="baseDirectory">base directory for relative paths</param>
    /// <returns>collapsed absolute path string</returns>
    public static string CollapsePath(string path, string baseDirectory)
    {
        ArgumentNullException.ThrowIfNull(path);
        ArgumentNullException.ThrowIfNull(baseDirectory);

        sbyte* result = NativeMethods.f3d_utils_collapse_path(path, baseDirectory);
        return NativeUtils.TakeString(result, "Could not collapse the path.");
    }

    /// <summary>
    /// Collapse a filesystem path using current directory as base.
    /// </summary>
    /// <param name="path">input path</param>
    /// <returns>collapsed absolute path string</returns>
    public static string CollapsePath(string path) => CollapsePath(path, string.Empty);

    /// <summary>
    /// Converts a glob expression to a regular expression.
    /// </summary>
    /// <param name="glob">glob expression</param>
    /// <param name="pathSeparator">path separator character</param>
    /// <returns>regular expression string</returns>
    public static string? GlobToRegex(string glob, char pathSeparator)
    {
        ArgumentNullException.ThrowIfNull(glob);
        if (pathSeparator > sbyte.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(pathSeparator), "The native API accepts an 8-bit path separator.");

        sbyte* result = NativeMethods.f3d_utils_glob_to_regex(glob, (sbyte)pathSeparator);
        return result == null ? null : NativeUtils.TakeString(result, "Could not convert the glob expression.");
    }

    /// <summary>
    /// Converts a glob expression to a regular expression with default separator '/'.
    /// </summary>
    /// <param name="glob">glob expression</param>
    /// <returns>regular expression string</returns>
    public static string? GlobToRegex(string glob) => GlobToRegex(glob, '/');

    /// <summary>
    /// Get the value of an environment variable.
    /// </summary>
    /// <param name="env">environment variable name</param>
    /// <returns>value of the environment variable, or null if not set</returns>
    public static string? GetEnv(string env)
    {
        ArgumentNullException.ThrowIfNull(env);

        sbyte* result = NativeMethods.f3d_utils_get_env(env);
        return result == null ? null : NativeUtils.TakeString(result, $"Could not read environment variable '{env}'.");
    }

    /// <summary>
    /// Get a Windows known folder.
    /// </summary>
    /// <param name="knownFolder">known folder identifier</param>
    /// <returns>folder path, or null on non-Windows platforms or error</returns>
    public static string? GetKnownFolder(KnownFolder knownFolder)
    {
        if (!Enum.IsDefined(knownFolder))
            throw new ArgumentOutOfRangeException(nameof(knownFolder), knownFolder, "Unknown known-folder value.");

        sbyte* result = NativeMethods.f3d_utils_get_known_folder((f3d_utils_known_folder_t)knownFolder);
        return result == null ? null : NativeUtils.TakeString(result, "Could not retrieve the known folder.");
    }
}
