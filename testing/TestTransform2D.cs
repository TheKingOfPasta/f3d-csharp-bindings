using F3D;

static class TestTransform2D
{
    public static void Run(string[] args)
    {
        TestDefaultIdentity();
        TestMatrixConstructor();
        TestCreateWithoutRotation();
        TestCreateWithRotation();
        TestInvalidArguments();
        TestOptionType();
    }

    private static void TestDefaultIdentity()
    {
        var transform = new Transform2D();
        Check(transform.Data.SequenceEqual([1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0]),
            "A default transform should be the 3x3 identity matrix.");
    }

    private static void TestMatrixConstructor()
    {
        double[] matrix = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0];
        var transform = new Transform2D(matrix);

        Check(ReferenceEquals(transform.Data, matrix), "The matrix constructor should retain the provided array.");
        Check(transform.Data.SequenceEqual(matrix), "The matrix constructor should preserve all matrix values.");

        matrix[0] = 10.0;
        Check(transform.Data[0] == 10.0, "Transform data should reflect changes to the retained matrix array.");
    }

    private static void TestCreateWithoutRotation()
    {
        var identity = Transform2D.Create([1.0, 1.0], [0.0, 0.0], 0.0);
        Check(identity.Data.SequenceEqual([1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0]),
            "Creating a unit transform should produce the identity matrix.");

        var transform = Transform2D.Create([2.0, 3.0], [4.0, 5.0], 0.0);
        CheckMatrixNear(transform.Data, [2.0, 0.0, 4.0, 0.0, 3.0, 5.0, 0.0, 0.0, 1.0],
            "Scale and translation were not represented correctly.");
    }

    private static void TestCreateWithRotation()
    {
        var transform = Transform2D.Create([2.0, 2.0], [4.0, 5.0], 90.0);
        double[] matrix = transform.Data;

        Check(matrix.Length == 9, "A created transform should contain nine matrix values.");
        CheckNear(matrix[0], 0.0, "Unexpected first matrix component after a 90 degree rotation.");
        CheckNear(Math.Abs(matrix[1]), 2.0, "Unexpected rotated scale on the first row.");
        CheckNear(Math.Abs(matrix[3]), 2.0, "Unexpected rotated scale on the second row.");
        CheckNear(matrix[4], 0.0, "Unexpected diagonal component after a 90 degree rotation.");
        CheckNear(matrix[2], 4.0, "X translation should be preserved by rotation.");
        CheckNear(matrix[5], 5.0, "Y translation should be preserved by rotation.");
        CheckNear(matrix[6], 0.0, "Unexpected third-row first component.");
        CheckNear(matrix[7], 0.0, "Unexpected third-row second component.");
        CheckNear(matrix[8], 1.0, "The homogeneous matrix component should be one.");
    }

    private static void TestInvalidArguments()
    {
        CheckThrows<ArgumentNullException>(() => new Transform2D(null!));
        CheckThrows<ArgumentException>(() => new Transform2D([]));
        CheckThrows<ArgumentException>(() => new Transform2D(new double[8]));
        CheckThrows<ArgumentException>(() => new Transform2D(new double[10]));

        CheckThrows<ArgumentNullException>(() => Transform2D.Create(null!, [0.0, 0.0], 0.0));
        CheckThrows<ArgumentNullException>(() => Transform2D.Create([1.0, 1.0], null!, 0.0));
        CheckThrows<ArgumentException>(() => Transform2D.Create([1.0], [0.0, 0.0], 0.0));
        CheckThrows<ArgumentException>(() => Transform2D.Create([1.0, 1.0, 1.0], [0.0, 0.0], 0.0));
        CheckThrows<ArgumentException>(() => Transform2D.Create([1.0, 1.0], [0.0], 0.0));
        CheckThrows<ArgumentException>(() => Transform2D.Create([1.0, 1.0], [0.0, 0.0, 0.0], 0.0));
    }

    private static void TestOptionType()
    {
        Check((int)Options.OptionType.Transform2D == 10, "Transform2D option type value changed.");
    }

    private static void CheckMatrixNear(double[] actual, double[] expected, string message)
    {
        Check(actual.Length == expected.Length, message);
        for (int i = 0; i < actual.Length; i++)
            CheckNear(actual[i], expected[i], $"{message} Matrix element {i} differed.");
    }

    private static void CheckNear(double actual, double expected, string message)
    {
        Check(Math.Abs(actual - expected) < 1e-10, $"{message} Expected {expected}, got {actual}.");
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
            throw new Exception(message);
    }

    private static void CheckThrows<TException>(Action action)
        where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            return;
        }

        throw new Exception($"Expected {typeof(TException).Name} to be thrown.");
    }
}