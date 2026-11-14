using F3D;

static class TestTypes
{
    public static void Run(string[] args)
    {
        TestColormap();
        TestLightType();
        TestLightStateDefaultsAndEquality();
        TestLightStateConstructor();
        TestNodeStateDefaults();
        TestSceneInfoDefaults();
        TestMeshDefaultsAndConstructor();
        TestMeshValidation();
        TestValidationResult();
    }

    private static void TestColormap()
    {
        var empty = new Types.Colormap();
        Check(empty.Data.Length == 0, "A new colormap should be empty.");

        double[] values = [0.0, 1.0, 0.0, 0.0];
        var colormap = new Types.Colormap(values);
        Check(ReferenceEquals(colormap.Data, values), "Colormap should retain its data.");
        CheckThrows<ArgumentNullException>(() => new Types.Colormap(null!));
    }

    private static void TestLightType()
    {
        Check((int)Types.LightType.Headlight == 1, "Headlight enum value changed.");
        Check((int)Types.LightType.CameraLight == 2, "CameraLight enum value changed.");
        Check((int)Types.LightType.SceneLight == 3, "SceneLight enum value changed.");
    }

    private static void TestLightStateDefaultsAndEquality()
    {
        var light = new Types.LightState();
        Check(light.Type == Types.LightType.SceneLight, "Unexpected default light type.");
        Check(light.Position.SequenceEqual([0.0, 0.0, 0.0]), "Unexpected default position.");
        Check(light.Color.SequenceEqual([1.0, 1.0, 1.0]), "Unexpected default color.");
        Check(light.Direction.SequenceEqual([1.0, 0.0, 0.0]), "Unexpected default direction.");
        Check(!light.PositionalLight, "A light should default to non-positional.");
        Check(light.Intensity == 1.0, "Unexpected default intensity.");
        Check(light.SwitchState, "A light should default to switched on.");
        Check(light.CastShadows, "A light should default to casting shadows.");

        var equal = new Types.LightState();
        Check(light.Equals(equal), "Equivalent light states should compare equal.");
        Check(light.Equals((object)equal), "Object equality should compare light states.");
        Check(light.GetHashCode() == equal.GetHashCode(), "Equal light states need matching hashes.");
        double[] changedPosition = equal.Position;
        changedPosition[0] = 4.0;
        Check(!light.Equals(equal), "Different light state data should not compare equal.");
    }

    private static void TestLightStateConstructor()
    {
        double[] position = [1.0, 2.0, 3.0];
        double[] color = [0.2, 0.4, 0.6];
        double[] direction = [0.0, 0.0, -1.0];
        var light = new Types.LightState(
            Types.LightType.CameraLight,
            position,
            color,
            direction,
            positionalLight: true,
            intensity: 2.5,
            switchState: false,
            castShadows: false);

        Check(light.Type == Types.LightType.CameraLight, "Constructor did not set the type.");
        Check(ReferenceEquals(light.Position, position), "Constructor did not set the position.");
        Check(ReferenceEquals(light.Color, color), "Constructor did not set the color.");
        Check(ReferenceEquals(light.Direction, direction), "Constructor did not set the direction.");
        Check(light.PositionalLight && light.Intensity == 2.5, "Constructor did not set light properties.");
        Check(!light.SwitchState && !light.CastShadows, "Constructor did not set boolean properties.");
        CheckThrows<ArgumentNullException>(() => new Types.LightState(
            Types.LightType.Headlight, null!, color, direction, false, 1.0, true, true));
        CheckThrows<ArgumentNullException>(() => new Types.LightState(
            Types.LightType.Headlight, position, null!, direction, false, 1.0, true, true));
        CheckThrows<ArgumentNullException>(() => new Types.LightState(
            Types.LightType.Headlight, position, color, null!, false, 1.0, true, true));
    }

    private static void TestNodeStateDefaults()
    {
        var node = new Types.NodeState();
        Check(node.Id == -1 && node.ParentId == -1, "Unexpected default node identifiers.");
        Check(node.Level == 0, "Unexpected default node level.");
        Check(node.Label == string.Empty, "Unexpected default node label.");
        Check(node.Visible, "A node should default to visible.");
        Check(!node.HasChildren && !node.Collapsed, "Unexpected default hierarchy flags.");
    }

    private static void TestSceneInfoDefaults()
    {
        var info = new Types.SceneInfo();
        Check(info.NumberOfFiles == 0 && info.NumberOfActors == 0, "Unexpected scene count defaults.");
        Check(info.NumberOfPoints == 0 && info.NumberOfCells == 0, "Unexpected geometry count defaults.");
    }

    private static void TestMeshDefaultsAndConstructor()
    {
        var empty = new Types.Mesh();
        Check(empty.Points.Length == 0, "Default mesh points should be empty.");
        Check(empty.Normals.Length == 0, "Default mesh normals should be empty.");
        Check(empty.TextureCoordinates.Length == 0, "Default mesh texture coordinates should be empty.");
        Check(empty.FaceSides.Length == 0, "Default mesh face sides should be empty.");
        Check(empty.FaceIndices.Length == 0, "Default mesh face indices should be empty.");

        float[] points = [0.0f, 0.0f, 0.0f];
        uint[] faceSides = [1];
        var mesh = new Types.Mesh(points, null, null, faceSides, null);
        Check(ReferenceEquals(mesh.Points, points), "Mesh should retain its points.");
        Check(mesh.Normals.Length == 0, "Null normals should become an empty array.");
        Check(mesh.TextureCoordinates.Length == 0, "Null texture coordinates should become an empty array.");
        Check(ReferenceEquals(mesh.FaceSides, faceSides), "Mesh should retain its face sides.");
        Check(mesh.FaceIndices.Length == 0, "Null face indices should become an empty array.");
    }

    private static void TestMeshValidation()
    {
        var validMesh = new Types.Mesh(
            points: [0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f],
            normals: null,
            textureCoordinates: null,
            faceSides: [3],
            faceIndices: [0, 1, 2]);
        Types.ValidationResult valid = validMesh.IsValid();
        Check(valid.Valid, $"Triangle mesh should be valid. Native error: {valid.ErrorMessage}");
        Check(valid.ErrorMessage == string.Empty, "A valid mesh should have no error message.");

        Types.ValidationResult invalid = new Types.Mesh().IsValid();
        Check(!invalid.Valid, "An empty mesh should be invalid.");
        Check(!string.IsNullOrWhiteSpace(invalid.ErrorMessage), "Invalid mesh should include an error message.");

        var nullArrayMesh = new Types.Mesh { Points = null! };
        CheckThrows<ArgumentNullException>(() => nullArrayMesh.IsValid());
    }

    private static void TestValidationResult()
    {
        var result = new Types.ValidationResult(false, "invalid mesh");
        Check(!result.Valid && result.ErrorMessage == "invalid mesh", "Validation result values were not retained.");
        CheckThrows<ArgumentNullException>(() => new Types.ValidationResult(false, null!));
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
