using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using F3D.Native;

namespace F3D;

/// <summary>
/// Enumeration of verbose levels.
/// </summary>
public enum LogVerboseLevel : uint
{
    Debug = f3d_log_verbose_level_t.F3D_LOG_DEBUG,
    Info = f3d_log_verbose_level_t.F3D_LOG_INFO,
    Warn = f3d_log_verbose_level_t.F3D_LOG_WARN,
    Error = f3d_log_verbose_level_t.F3D_LOG_ERROR,
    Quiet = f3d_log_verbose_level_t.F3D_LOG_QUIET,
}

/// <summary>
/// Callback interface for log forwarding.
/// </summary>
/// <remarks>
/// Called when a log message is forwarded.
/// </remarks>
/// <param name="level">verbose level of the message</param>
/// <param name="message">log message</param>
public delegate void LogForwardCallback(LogVerboseLevel level, string message);

public static unsafe class Log
{
    // Managed callback invoked by the native forwarder; null when forwarding is disabled.
    static volatile LogForwardCallback? _callback;


    /// <summary>
    /// Convert a .NET string to a null-terminated UTF-8 buffer and pass it to a native function.
    /// </summary>
    /// <param name="message">message string</param>
    /// <param name="nativeLog">native log function</param>
    static void Send(string message, delegate*<sbyte*, void> nativeLog)
    {
        ArgumentNullException.ThrowIfNull(message);
        fixed (byte* str = Encoding.UTF8.GetBytes(message + '\0'))
        {
            nativeLog((sbyte*)str);
        }
    }

    /// <summary>
    /// Log a message at the specified verbose level.
    /// </summary>
    /// <param name="level">verbose level for the message</param>
    /// <param name="message">message string</param>
    public static void Print(LogVerboseLevel level, string message)
    {
        ArgumentNullException.ThrowIfNull(message);
        fixed (byte* str = Encoding.UTF8.GetBytes(message + '\0'))
        {
            NativeMethods.f3d_log_print((f3d_log_verbose_level_t)level, (sbyte*)str);
        }
    }

    /// <summary>
    /// Log a debug message.
    /// </summary>
    /// <param name="message">message string</param>
    public static void Debug(string message) => Send(message, &NativeMethods.f3d_log_debug);

    /// <summary>
    /// Log an info message.
    /// </summary>
    /// <param name="message">message string</param>
    public static void Info(string message) => Send(message, &NativeMethods.f3d_log_info);

    /// <summary>
    /// Log a warning message.
    /// </summary>
    /// <param name="message">message string</param>
    public static void Warn(string message) => Send(message, &NativeMethods.f3d_log_warn);

    /// <summary>
    /// Log an error message.
    /// </summary>
    /// <param name="message">message string</param>
    public static void Error(string message) => Send(message, &NativeMethods.f3d_log_error);

    /// <summary>
    /// Set the coloring usage.
    /// </summary>
    /// <param name="use">if true, coloring will be used</param>
    public static void SetUseColoring(bool use) => NativeMethods.f3d_log_set_use_coloring(use ? 1 : 0);

    /// <summary>
    /// Set the verbose level.
    /// </summary>
    /// <param name="level">verbose level to set</param>
    /// <param name="forceStdErr">if true, all messages are written to stderr</param>
    public static void SetVerboseLevel(LogVerboseLevel level, bool forceStdErr = false)
    {
        NativeMethods.f3d_log_set_verbose_level((f3d_log_verbose_level_t)level, forceStdErr ? 1 : 0);
    }

    /// <summary>
    /// Get the current verbose level.
    /// </summary>
    /// <value>current verbose level</value>
    public static LogVerboseLevel VerboseLevel => (LogVerboseLevel)NativeMethods.f3d_log_get_verbose_level();

    /// <summary>
    /// Bridge between f3d and user C# callback
    /// f3d_log_forward() needs a C function pointer, not a C# delegate: this method is that C function.
    /// Call chain : f3d (C++) -> Forwarder (C#) -> _callback (user C# delegate).
    /// Static => address never moves and never collected by the GC, unlike a delegate.
    /// </summary>
    /// <param name="level">log level</param>
    /// <param name="message">log message</param>

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    static void Forwarder(f3d_log_verbose_level_t level, sbyte* message)
    {
        // An exception must never leave this method: the caller is C++ code, which cannot handle .NET exceptions
        try {_callback?.Invoke((LogVerboseLevel)level, Marshal.PtrToStringUTF8((nint)message) ?? "");}
        catch (Exception ex)
        {
            // Nested try: even writing to stderr can throw
            try {Console.Error.WriteLine($"[F3D] Log forward callback threw an exception: {ex}");}
            catch {}
        }
    }

    /// <summary>
    /// Set a callback function to forward log messages.
    /// </summary>
    /// <param name="callback">callback function, or null to disable forwarding</param>
    public static void Forward(LogForwardCallback? callback)
    {
        if (callback != null)
        {
            _callback = callback;
            NativeMethods.f3d_log_forward(&Forwarder);
        }
        else
        {
            NativeMethods.f3d_log_forward(null);
            _callback = null;
        }
    }
}
