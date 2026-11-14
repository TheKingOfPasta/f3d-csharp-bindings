using System.Runtime.InteropServices;
using F3D.Native;

namespace F3D;

public enum AnimationDirection : uint
{
    Forward = f3d_animation_direction_t.F3D_ANIMATION_FORWARD,
    Backward = f3d_animation_direction_t.F3D_ANIMATION_BACKWARD,
}

public sealed unsafe class Animation
{
    readonly f3d_animation_t* _handle;

    internal Animation(f3d_animation_t* handle) => _handle = handle;

    /// <summary>
    /// Load files in the scene at provided time value if they contain any animation.
    /// </summary>
    /// <param name="timeValue">time value to load</param>
    /// <returns>this animation for method chaining</returns>
    public Animation LoadTime(double timeValue)
    {
        NativeMethods.f3d_animation_load_time(_handle, timeValue);
        return this;
    }

    /// <summary>
    /// Get animation time range of files currently in the scene.
    /// </summary>
    /// <value>array of 2 doubles [min_time, max_time]</value>
    public (double Min, double Max) TimeRange
    {
        get
        {
            double min, max;
            NativeMethods.f3d_animation_get_time_range(_handle, &min, &max);
            return (min, max);
        }
    }

    /// <summary>
    /// Get animation key frame's time of files currently in the scene.
    /// </summary>
    /// <value>list of double</value>
    public double[] KeyFrames
    {
        get
        {
            uint count;
            double* frames = NativeMethods.f3d_animation_get_key_frames(_handle, &count);
            if (frames == null)
                return [];
            try
            {
                return new ReadOnlySpan<double>(frames, (int)count).ToArray();
            }
            finally
            {
                NativeMethods.f3d_animation_destroy_key_frames(frames);
            }
        }
    }

    /// <summary>
    /// Return the number of animations available in the files currently in the scene.
    /// </summary>
    /// <value>number of available animations</value>
    public int Count => (int)NativeMethods.f3d_animation_count(_handle);

    /// <summary>
    /// Get the current animation name, if any.
    /// </summary>
    /// <value>animation names or string error</value>
    public string? CurrentName => TakeString(NativeMethods.f3d_animation_get_current_name(_handle));

    /// <summary>
    /// Get the animation name of a given animation index, if any.
    /// </summary>
    /// <param name="index">animation index</param>
    /// <returns>animation name or string error</returns>
    public string GetName(int index)
        => TakeString(NativeMethods.f3d_animation_get_name(_handle, index))
           ?? throw new F3DException($"No animation at index {index}");

    /// <summary>
    /// Get all of the animation names, if any.
    /// </summary>
    /// <value>list of animation names</value>
    public IReadOnlyList<string> Names
    {
        get
        {
            int count;
            sbyte** names = NativeMethods.f3d_animation_get_names(_handle, &count);
            if (names == null)
                return [];
            try
            {
                var result = new string[count];
                for (int i = 0; i < count; i++)
                    result[i] = Marshal.PtrToStringUTF8((nint)names[i]) ?? "";
                return result;
            }
            finally
            {
                NativeMethods.f3d_animation_destroy_string_array(names, count);
            }
        }
    }

    /// <summary>
    /// Toggle animation state.
    /// </summary>
    /// <remarks>
    /// Toggle animation state with default forward direction.
    /// </remarks>
    /// <param name="direction">animation direction</param>
    /// <returns>this animation for method chaining</returns>
    public Animation Toggle(AnimationDirection direction = AnimationDirection.Forward)
    {
        NativeMethods.f3d_animation_toggle(_handle, (f3d_animation_direction_t)direction);
        return this;
    }

    /// <summary>
    /// Start animation.
    /// </summary>
    /// <remarks>
    /// Start animation with default forward direction.
    /// </remarks>
    /// <param name="direction">animation direction</param>
    /// <returns>this animation for method chaining</returns>
    public Animation Start(AnimationDirection direction = AnimationDirection.Forward)
    {
        NativeMethods.f3d_animation_start(_handle, (f3d_animation_direction_t)direction);
        return this;
    }

    /// <summary>
    /// Stop animation.
    /// </summary>
    /// <returns>this animation for method chaining</returns>
    public Animation Stop()
    {
        NativeMethods.f3d_animation_stop(_handle);
        return this;
    }

    /// <summary>
    /// Check if animation is playing.
    /// </summary>
    /// <value>true if playing, false otherwise</value>
    public bool IsPlaying => NativeMethods.f3d_animation_is_playing(_handle) != 0;

    /// <summary>
    /// Get the current animation direction.
    /// </summary>
    /// <value>animation direction</value>
    public AnimationDirection Direction
        => (AnimationDirection)NativeMethods.f3d_animation_get_direction(_handle);

    // Copy string to .NET then free C allocation
    static string? TakeString(sbyte* str)
    {
        if (str == null)
            return null;
        try
        {
            return Marshal.PtrToStringUTF8((nint)str);
        }
        finally
        {
            NativeMethods.f3d_animation_destroy_string(str);
        }
    }
}
