using F3D.Native;

namespace F3D;

public sealed unsafe class Transform2D
{
    public double[] Data { get; }

    public Transform2D()
    {
        Data = new double[] { 1, 0, 0, 0, 1, 0, 0, 0, 1 };
    }

    public Transform2D(double[] matrix)
    {
        ArgumentNullException.ThrowIfNull(matrix);
        if (matrix.Length != 9)
            throw new ArgumentException("Matrix array must have exactly 9 elements", nameof(matrix));
        Data = matrix;
    }

    /// <summary>Create a 2D transform from scale, translate and angle.</summary>
    /// <param name="scale">array of 2 doubles for x and y scale factors</param>
    /// <param name="translate">array of 2 doubles for x and y translation</param>
    /// <param name="angleDeg">rotation angle in degrees</param>
    public static Transform2D Create(double[] scale, double[] translate, double angleDeg)
    {
        ArgumentNullException.ThrowIfNull(scale);
        ArgumentNullException.ThrowIfNull(translate);
        if (scale.Length != 2)
            throw new ArgumentException("Scale array must have exactly 2 elements", nameof(scale));
        if (translate.Length != 2)
            throw new ArgumentException("Translate array must have exactly 2 elements", nameof(translate));

        f3d_transform2d_t native;
        NativeMethods.f3d_transform2d_create(&native, scale[0], scale[1], translate[0], translate[1], angleDeg);
        return FromNative(native);
    }

    internal static Transform2D FromNative(f3d_transform2d_t native)
    {
        var data = new double[9];
        for (int i = 0; i < 9; i++)
            data[i] = native.data[i];
        return new Transform2D(data);
    }

    internal f3d_transform2d_t ToNative()
    {
        f3d_transform2d_t native = default;
        for (int i = 0; i < 9; i++)
            native.data[i] = Data[i];
        return native;
    }
}
