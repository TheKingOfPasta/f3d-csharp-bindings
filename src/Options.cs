using System.Runtime.InteropServices;
using F3D.Native;

namespace F3D;

public sealed unsafe class Options : IDisposable
{
    /// <summary>
    /// Enumeration of the possible types of an option.
    /// </summary>
    public enum OptionType
    {
        Invalid,
        Bool,
        Int,
        Double,
        Ratio,
        String,
        Path,
        Color,
        Direction,
        Colormap,
        Transform2D,
        DoubleVector,
        IntVector,
    }

    /// <summary>
    /// Enumeration of supported domain style.
    /// </summary>
    public enum DomainStyle
    {
        None = 0,
        Range = 1,
        Enum = 2,
        Index = 3
    }

    const int MaxVectorSize = 64; // TODO: discard when the C api is update to be able to fetch the size

    /// <summary>
    /// Native pointer to the underlying f3d::options object.
    /// </summary>
    f3d_options_t* _handle;
    readonly bool _owned;

    /// <summary>
    /// Package-private constructor for creating an Options instance from a native pointer.
    /// </summary>
    /// <param name="handle">the native memory address of the underlying f3d::options object</param>
    internal Options(f3d_options_t* handle) => _handle = handle;

    /// <summary>
    /// Create a new options object.
    ///
    /// The native object is released when this instance is disposed.
    /// </summary>
    public Options()
    {
        _handle = NativeMethods.f3d_options_create();
        _owned = true;
    }

    /// <summary>
    /// Native pointer to the underlying f3d::options object.
    /// </summary>
    internal f3d_options_t* Handle => _handle;

    /// <summary>
    /// Delete the underlying options object if it was created by this instance.
    /// Options owned by an engine are left untouched.
    /// </summary>
    public void Dispose()
    {
        if (!_owned || _handle == null) return;
        NativeMethods.f3d_options_destroy(_handle);
        _handle = null;
    }

    /// <summary>
    /// Sets a boolean option value.
    /// </summary>
    /// <param name="name">the name of the option to set</param>
    /// <param name="value">the boolean value to set</param>
    /// <exception cref="ArgumentException">if the option name does not exist or is not a boolean type</exception>
    public void SetAsBool(string name, bool value) => NativeMethods.f3d_options_set_as_bool(_handle, name, value ? 1 : 0);

    /// <summary>
    /// Sets an integer option value.
    /// </summary>
    /// <param name="name">the name of the option to set</param>
    /// <param name="value">the integer value to set</param>
    /// <exception cref="ArgumentException">if the option name does not exist or is not an integer type</exception>
    public void SetAsInt(string name, int value) => NativeMethods.f3d_options_set_as_int(_handle, name, value);

    /// <summary>
    /// Sets a double option value.
    /// </summary>
    /// <param name="name">the name of the option to set</param>
    /// <param name="value">the double value to set</param>
    /// <exception cref="ArgumentException">if the option name does not exist or is not a double type</exception>
    public void SetAsDouble(string name, double value) => NativeMethods.f3d_options_set_as_double(_handle, name, value);

    /// <summary>
    /// Sets a string option value.
    /// </summary>
    /// <param name="name">the name of the option to set</param>
    /// <param name="value">the string value to set</param>
    /// <exception cref="ArgumentException">if the option name does not exist or is not a string type</exception>
    public void SetAsString(string name, string value) => NativeMethods.f3d_options_set_as_string(_handle, name, value);

    /// <summary>
    /// Sets a double vector option value.
    /// </summary>
    /// <param name="name">the name of the option to set</param>
    /// <param name="values">the array of double values to set</param>
    /// <exception cref="ArgumentException">if the option name does not exist or is not a double vector type</exception>
    public void SetAsDoubleVector(string name, double[] values) { fixed (double* v = values) NativeMethods.f3d_options_set_as_double_vector(_handle, name, v, (nuint)values.Length); }

    /// <summary>
    /// Sets an integer vector option value.
    /// </summary>
    /// <param name="name">the name of the option to set</param>
    /// <param name="values">the array of integer values to set</param>
    /// <exception cref="ArgumentException">if the option name does not exist or is not an integer vector type</exception>
    public void SetAsIntVector(string name, int[] values) { fixed (int* v = values) NativeMethods.f3d_options_set_as_int_vector(_handle, name, v, (nuint)values.Length); }

    /// <summary>
    /// Sets an option value from its string representation.
    /// </summary>
    /// <param name="name">the name of the option to set</param>
    /// <param name="str">the string representation to parse and set</param>
    /// <exception cref="ArgumentException">if the option name does not exist or the string cannot be parsed</exception>
    public void SetAsStringRepresentation(string name, string str) => NativeMethods.f3d_options_set_as_string_representation(_handle, name, str);

    /// <summary>
    /// Gets a boolean option value.
    /// </summary>
    /// <param name="name">the name of the option to retrieve</param>
    /// <returns>the boolean value of the option</returns>
    /// <exception cref="ArgumentException">if the option name does not exist or is not a boolean type</exception>
    public bool GetAsBool(string name) => NativeMethods.f3d_options_get_as_bool(_handle, name) != 0;

    /// <summary>
    /// Gets an integer option value.
    /// </summary>
    /// <param name="name">the name of the option to retrieve</param>
    /// <returns>the integer value of the option</returns>
    /// <exception cref="ArgumentException">if the option name does not exist or is not an integer type</exception>
    public int GetAsInt(string name) => NativeMethods.f3d_options_get_as_int(_handle, name);

    /// <summary>
    /// Gets a double option value.
    /// </summary>
    /// <param name="name">the name of the option to retrieve</param>
    /// <returns>the double value of the option</returns>
    /// <exception cref="ArgumentException">if the option name does not exist or is not a double type</exception>
    public double GetAsDouble(string name) => NativeMethods.f3d_options_get_as_double(_handle, name);

    /// <summary>
    /// Gets a string option value.
    /// </summary>
    /// <param name="name">the name of the option to retrieve</param>
    /// <returns>the string value of the option</returns>
    /// <exception cref="ArgumentException">if the option name does not exist or is not a string type</exception>
    public string? GetAsString(string name) => TakeString(NativeMethods.f3d_options_get_as_string(_handle, name));

    /// <summary>
    /// Gets the string representation of an option value regardless of its underlying type.
    /// </summary>
    /// <param name="name">the name of the option to retrieve</param>
    /// <returns>the string representation of the option value</returns>
    /// <exception cref="ArgumentException">if the option name does not exist</exception>
    public string? GetAsStringRepresentation(string name) => TakeString(NativeMethods.f3d_options_get_as_string_representation(_handle, name));

    /// <summary>
    /// Gets a double vector option value.
    /// </summary>
    /// <param name="name">the name of the option to retrieve</param>
    /// <returns>the array of double values</returns>
    /// <exception cref="ArgumentException">if the option name does not exist or is not a double vector type</exception>
    public double[] GetAsDoubleVector(string name)
    {
        double* values = stackalloc double[MaxVectorSize];
        nuint count = 0;
        NativeMethods.f3d_options_get_as_double_vector(_handle, name, values, &count);
        return new Span<double>(values, (int)count).ToArray();
    }

    /// <summary>
    /// Gets an integer vector option value.
    /// </summary>
    /// <param name="name">the name of the option to retrieve</param>
    /// <returns>the array of integer values</returns>
    /// <exception cref="ArgumentException">if the option name does not exist or is not an integer vector type</exception>
    public int[] GetAsIntVector(string name)
    {
        int* values = stackalloc int[MaxVectorSize];
        nuint count = 0;
        NativeMethods.f3d_options_get_as_int_vector(_handle, name, values, &count);
        return new Span<int>(values, (int)count).ToArray();
    }

    /// <summary>
    /// Toggles a boolean option value.
    /// </summary>
    /// <param name="name">the name of the boolean option to toggle</param>
    /// <exception cref="ArgumentException">if the option name does not exist or is not a boolean type</exception>
    public void Toggle(string name) => NativeMethods.f3d_options_toggle(_handle, name);

    /// <summary>
    /// Resets the specified option to its default value.
    /// </summary>
    /// <param name="name">the name of the option to reset</param>
    /// <exception cref="ArgumentException">if the option name does not exist</exception>
    public void Reset(string name) => NativeMethods.f3d_options_reset(_handle, name);

    /// <summary>
    /// Removes the value of the specified option, making it unset.
    /// </summary>
    /// <param name="name">the name of the option to remove the value from</param>
    /// <exception cref="ArgumentException">if the option name does not exist</exception>
    public void RemoveValue(string name) => NativeMethods.f3d_options_remove_value(_handle, name);

    /// <summary>
    /// Increase the specified option if it has a range or index domain
    /// </summary>
    /// <param name="name">the name of the option to increase</param>
    /// <exception cref="ArgumentException">if the option name does not exist or doesn't have the expected domain</exception>
    public void Increase(string name) => NativeMethods.f3d_options_increase(_handle, name);

    /// <summary>
    /// Decrease the specified option if it has a range or index domain
    /// </summary>
    /// <param name="name">the name of the option to decrease</param>
    /// <exception cref="ArgumentException">if the option name does not exist or doesn't have the expected domain</exception>
    public void Decrease(string name) => NativeMethods.f3d_options_decrease(_handle, name);

    /// <summary>
    /// Cycle the specified option if it has an enum or index domain
    /// </summary>
    /// <param name="name">the name of the option to cycle</param>
    /// <exception cref="ArgumentException">if the option name does not exist or doesn't have the expected domain</exception>
    public void Cycle(string name) => NativeMethods.f3d_options_cycle(_handle, name);

    /// <summary>
    /// Copies the value of an option from another Options instance to this instance.
    /// </summary>
    /// <param name="other">the Options instance to copy from</param>
    /// <param name="name">the name of the option to copy</param>
    /// <exception cref="ArgumentException">if the option name does not exist</exception>
    public void Copy(Options other, string name) => NativeMethods.f3d_options_copy(_handle, other._handle, name);

    /// <summary>
    /// Checks if the specified option has the same value in this instance and another Options instance.
    /// </summary>
    /// <param name="other">the other Options instance to compare with</param>
    /// <param name="name">the name of the option to compare</param>
    /// <returns>true if both options have the same value, false otherwise</returns>
    /// <exception cref="ArgumentException">if the option name does not exist</exception>
    public bool IsSame(Options other, string name) => NativeMethods.f3d_options_is_same(_handle, other._handle, name) != 0;

    /// <summary>
    /// Checks if the specified option has a value set.
    /// </summary>
    /// <param name="name">the name of the option to check</param>
    /// <returns>true if the option has a value, false otherwise</returns>
    /// <exception cref="ArgumentException">if the option name does not exist</exception>
    public bool HasValue(string name) => NativeMethods.f3d_options_has_value(_handle, name) != 0;

    /// <summary>
    /// Checks if the specified option is optional.
    /// </summary>
    /// <param name="name">the name of the option to check</param>
    /// <returns>true if the option is optional, false if it is required</returns>
    /// <exception cref="ArgumentException">if the option name does not exist</exception>
    public bool IsOptional(string name) => NativeMethods.f3d_options_is_optional(_handle, name) != 0;

    /// <summary>
    /// Gets the type of the specified option.
    /// </summary>
    /// <param name="name">the name of the option to check</param>
    /// <returns>the type of the option</returns>
    /// <exception cref="ArgumentException">if the option name does not exist</exception>
    public OptionType GetOptionType(string name) => (OptionType)NativeMethods.f3d_options_get_type(_handle, name);

    /// <summary>
    /// Gets a list of option names that have values set in this Options instance.
    /// </summary>
    /// <returns>a list containing names of options with values</returns>
    public List<string> GetNames()
    {
        nuint count = 0;
        sbyte** names = NativeMethods.f3d_options_get_names(_handle, &count);
        return TakeNames(names, count);
    }

    /// <summary>
    /// Gets a list of all available option names in F3D.
    /// </summary>
    /// <returns>a list containing all option names</returns>
    public static List<string> GetAllNames()
    {
        nuint count = 0;
        sbyte** names = NativeMethods.f3d_options_get_all_names(&count);
        return TakeNames(names, count);
    }

    /// <summary>
    /// Finds the closest matching option name to the provided string.
    /// </summary>
    /// <param name="option">the option name to find a match for</param>
    /// <returns>a ClosestOptionResult containing the closest match name and edit distance</returns>
    public (string Name, uint Distance) GetClosestOption(string option)
    {
        sbyte* closest = null;
        uint distance = 0;
        NativeMethods.f3d_options_get_closest_option(_handle, option, &closest, &distance);
        return (TakeString(closest) ?? "", distance);
    }

    /// <summary>
    /// Return true if an option as a domain, false otherwise.
    /// </summary>
    /// <param name="name">the name of the option to check if it has domain from</param>
    /// <exception cref="ArgumentException">if the option name does not exist</exception>
    public bool HasDomain(string name) => NativeMethods.f3d_options_has_domain(_handle, name) != 0;

    /// <summary>
    /// Return the domain style of the provided option.
    /// </summary>
    /// <param name="name">the name of the option to get the domain style of</param>
    /// <exception cref="ArgumentException">if the option name does not exist or it doesn't have a domain</exception>
    public DomainStyle GetDomainStyle(string name) => (DomainStyle)NativeMethods.f3d_options_get_domain_style(_handle, name);

    /// <summary>
    /// Return the range domain of the option as a DomainRange of Double.
    /// Matches double and ratio domains, as ratio does not exist in Java.
    /// </summary>
    /// <param name="name">the name of the option to get the range domain from</param>
    /// <returns>the range domain of the option</returns>
    /// <exception cref="ArgumentException">if the option name does not exist, doesn't have a range domain, or if the domain is not a double or ratio domain</exception>
    public double[]? GetRangeDomainDouble(string name)
    {
        double* range = stackalloc double[3];
        return NativeMethods.f3d_options_get_range_domain_double(_handle, name, range) != 0 ? new Span<double>(range, 3).ToArray() : null;
    }

    /// <summary>
    /// Return the range domain of the option as a DomainRange of Integer.
    /// </summary>
    /// <param name="name">the name of the option to get the range domain from</param>
    /// <returns>the range domain of the option</returns>
    /// <exception cref="ArgumentException">if the option name does not exist, doesn't have a range domain, or if the domain is not an int domain</exception>
    public int[]? GetRangeDomainInt(string name)
    {
        int* range = stackalloc int[3];
        return NativeMethods.f3d_options_get_range_domain_int(_handle, name, range) != 0 ? new Span<int>(range, 3).ToArray() : null;
    }

    /// <summary>
    /// Return a vector of string containing the enumeration for the option
    /// </summary>
    /// <param name="name">the name of the option to get the enum domain of</param>
    /// <returns>enumeration of values</returns>
    /// <exception cref="ArgumentException">if the option name does not exist or doesn't have an enum domain</exception>
    public List<string> GetEnumDomainString(string name)
    {
        int count = 0;
        sbyte** values = NativeMethods.f3d_options_get_enum_domain_string(_handle, name, &count);
        var list = ReadStrings(values, count);
        if (values != null) NativeMethods.f3d_options_destroy_string_array(values, count);
        return list;
    }

    /// <summary>
    /// Return the max value of the index domain for the option, or null if not set
    /// </summary>
    /// <param name="name">the name of the option to get the index domain of</param>
    /// <returns>max value of the index domain or null if not set</returns>
    /// <exception cref="ArgumentException">if the option name does not exist or doesn't have an index domain</exception>
    public uint? GetIndexDomain(string name)
    {
        uint max = 0;
        return NativeMethods.f3d_options_get_index_domain(_handle, name, &max) == 1 ? max : null;
    }

    /// <summary>
    /// Parse a string as a boolean.
    /// </summary>
    /// <param name="str">String to parse.</param>
    /// <returns>The parsed boolean. false if the string cannot be parsed.</returns>
    public static bool ParseBool(string str) => NativeMethods.f3d_options_parse_bool(str) != 0;

    /// <summary>
    /// Parse a string as an integer.
    /// </summary>
    /// <param name="str">String to parse.</param>
    /// <returns>Parsed integer value. 0 if the string cannot be parsed.</returns>
    public static int ParseInt(string str) => NativeMethods.f3d_options_parse_int(str);

    /// <summary>
    /// Parse a string as a double.
    /// </summary>
    /// <param name="str">String to parse.</param>
    /// <returns>Parsed double value. 0.0 if the string cannot be parsed.</returns>
    public static double ParseDouble(string str) => NativeMethods.f3d_options_parse_double(str);

    /// <summary>
    /// Parse a string as a string (returns a copy).
    /// </summary>
    /// <param name="str">String to parse.</param>
    /// <returns>Parsed string. null if the string cannot be parsed.</returns>
    public static string? ParseString(string str) => TakeString(NativeMethods.f3d_options_parse_string(str));

    /// <summary>
    /// Parse a string as a double vector.
    /// </summary>
    /// <param name="str">String to parse.</param>
    /// <returns>The parsed double values. Empty if the string cannot be parsed.</returns>
    public static double[] ParseDoubleVector(string str)
    {
        double* values = stackalloc double[MaxVectorSize];
        nuint count = 0;
        NativeMethods.f3d_options_parse_double_vector(str, values, &count);
        return new Span<double>(values, (int)count).ToArray();
    }

    /// <summary>
    /// Parse a string as an integer vector.
    /// </summary>
    /// <param name="str">String to parse.</param>
    /// <returns>The parsed integer values. Empty if the string cannot be parsed.</returns>
    public static int[] ParseIntVector(string str)
    {
        int* values = stackalloc int[MaxVectorSize];
        nuint count = 0;
        NativeMethods.f3d_options_parse_int_vector(str, values, &count);
        return new Span<int>(values, (int)count).ToArray();
    }

    /// <summary>
    /// Format a boolean as a string.
    /// </summary>
    /// <param name="value">Boolean value.</param>
    /// <returns>Formatted string.</returns>
    public static string FormatBool(bool value) => TakeString(NativeMethods.f3d_options_format_bool(value ? 1 : 0)) ?? "";

    /// <summary>
    /// Format an integer as a string.
    /// </summary>
    /// <param name="value">Integer value.</param>
    /// <returns>Formatted string.</returns>
    public static string FormatInt(int value) => TakeString(NativeMethods.f3d_options_format_int(value)) ?? "";

    /// <summary>
    /// Format a double as a string.
    /// </summary>
    /// <param name="value">Double value.</param>
    /// <returns>Formatted string.</returns>
    public static string FormatDouble(double value) => TakeString(NativeMethods.f3d_options_format_double(value)) ?? "";

    /// <summary>
    /// Format a string (returns a copy).
    /// </summary>
    /// <param name="value">String value.</param>
    /// <returns>Formatted string.</returns>
    public static string FormatString(string value) => TakeString(NativeMethods.f3d_options_format_string(value)) ?? "";

    /// <summary>
    /// Format a double vector as a string.
    /// </summary>
    /// <param name="values">Array of double values.</param>
    /// <returns>Formatted string.</returns>
    public static string FormatDoubleVector(double[] values) { fixed (double* v = values) return TakeString(NativeMethods.f3d_options_format_double_vector(v, (nuint)values.Length)) ?? ""; }

    /// <summary>
    /// Format an integer vector as a string.
    /// </summary>
    /// <param name="values">Array of integer values.</param>
    /// <returns>Formatted string.</returns>
    public static string FormatIntVector(int[] values) { fixed (int* v = values) return TakeString(NativeMethods.f3d_options_format_int_vector(v, (nuint)values.Length)) ?? ""; }

    static string? TakeString(sbyte* str)
    {
        if (str == null) return null;
        string? result = Marshal.PtrToStringUTF8((nint)str);
        NativeMethods.f3d_options_destroy_string(str);
        return result;
    }

    static List<string> ReadStrings(sbyte** array, long count)
    {
        var list = new List<string>();
        for (long i = 0; array != null && i < count; i++) list.Add(Marshal.PtrToStringUTF8((nint)array[i]) ?? "");
        return list;
    }

    static List<string> TakeNames(sbyte** names, nuint count)
    {
        var list = ReadStrings(names, (long)count);
        if (names != null) NativeMethods.f3d_options_destroy_names(names, count);
        return list;
    }
}
