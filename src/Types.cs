using System.Runtime.InteropServices;
using F3D.Native;

namespace F3D;

public static class Types
{
    /// <summary>
    /// Describes a colormap as repeated value, red, green, and blue components.
    /// </summary>
    public sealed class Colormap
    {
        public double[] Data { get; set; } = [];

        public Colormap()
        {
        }

        public Colormap(double[] data)
        {
            ArgumentNullException.ThrowIfNull(data);
            Data = data;
        }
    }

    /// <summary>
    /// Light type enumeration.
    /// </summary>
    public enum LightType
    {
        Headlight = 1,
        CameraLight = 2,
        SceneLight = 3
    }

    /// <summary>
    /// Describes the state of a light.
    /// </summary>
    public sealed class LightState : IEquatable<LightState>
    {
        public LightType Type { get; set; } = LightType.SceneLight;
        public double[] Position { get; set; } = [0.0, 0.0, 0.0];
        public double[] Color { get; set; } = [1.0, 1.0, 1.0];
        public double[] Direction { get; set; } = [1.0, 0.0, 0.0];
        public bool PositionalLight { get; set; }
        public double Intensity { get; set; } = 1.0;
        public bool SwitchState { get; set; } = true;
        public bool CastShadows { get; set; } = true;

        public LightState()
        {
        }

        public LightState(
            LightType type,
            double[] position,
            double[] color,
            double[] direction,
            bool positionalLight,
            double intensity,
            bool switchState,
            bool castShadows)
        {
            ArgumentNullException.ThrowIfNull(position);
            ArgumentNullException.ThrowIfNull(color);
            ArgumentNullException.ThrowIfNull(direction);

            Type = type;
            Position = position;
            Color = color;
            Direction = direction;
            PositionalLight = positionalLight;
            Intensity = intensity;
            SwitchState = switchState;
            CastShadows = castShadows;
        }

        public bool Equals(LightState? other) =>
            other is not null &&
            Type == other.Type &&
            Position.AsSpan().SequenceEqual(other.Position) &&
            Color.AsSpan().SequenceEqual(other.Color) &&
            Direction.AsSpan().SequenceEqual(other.Direction) &&
            PositionalLight == other.PositionalLight &&
            Intensity == other.Intensity &&
            SwitchState == other.SwitchState &&
            CastShadows == other.CastShadows;

        public override bool Equals(object? obj) => obj is LightState other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Type);
            AddValues(ref hash, Position);
            AddValues(ref hash, Color);
            AddValues(ref hash, Direction);
            hash.Add(PositionalLight);
            hash.Add(Intensity);
            hash.Add(SwitchState);
            hash.Add(CastShadows);
            return hash.ToHashCode();
        }

        private static void AddValues(ref HashCode hash, double[] values)
        {
            foreach (double value in values)
                hash.Add(value);
        }
    }

    /// <summary>
    /// Describes a node in the scene hierarchy.
    /// </summary>
    public sealed class NodeState
    {
        public int Id { get; set; } = -1;
        public int ParentId { get; set; } = -1;
        public int Level { get; set; }
        public string Label { get; set; } = string.Empty;
        public bool Visible { get; set; } = true;
        public bool HasChildren { get; set; }
        public bool Collapsed { get; set; }
    }

    /// <summary>
    /// Describes the contents of a scene.
    /// NumberOfFiles includes files, meshes, and buffers added to the scene.
    /// </summary>
    public sealed class SceneInfo
    {
        public int NumberOfFiles { get; set; }
        public int NumberOfActors { get; set; }
        public long NumberOfPoints { get; set; }
        public long NumberOfCells { get; set; }
    }

    /// <summary>
    /// Describes a 3D surfacic mesh.
    /// </summary>
    public sealed unsafe class Mesh
    {
        public float[] Points { get; set; } = [];
        public float[] Normals { get; set; } = [];
        public float[] TextureCoordinates { get; set; } = [];
        public uint[] FaceSides { get; set; } = [];
        public uint[] FaceIndices { get; set; } = [];

        public Mesh()
        {
        }

        public Mesh(
            float[]? points,
            float[]? normals,
            float[]? textureCoordinates,
            uint[]? faceSides,
            uint[]? faceIndices)
        {
            Points = points ?? [];
            Normals = normals ?? [];
            TextureCoordinates = textureCoordinates ?? [];
            FaceSides = faceSides ?? [];
            FaceIndices = faceIndices ?? [];
        }

        /// <summary>
        /// Validates the mesh using the native F3D C API.
        /// </summary>
        public ValidationResult IsValid()
        {
            ArgumentNullException.ThrowIfNull(Points);
            ArgumentNullException.ThrowIfNull(Normals);
            ArgumentNullException.ThrowIfNull(TextureCoordinates);
            ArgumentNullException.ThrowIfNull(FaceSides);
            ArgumentNullException.ThrowIfNull(FaceIndices);

            fixed (float* points = Points)
            fixed (float* normals = Normals)
            fixed (float* textureCoordinates = TextureCoordinates)
            fixed (uint* faceSides = FaceSides)
            fixed (uint* faceIndices = FaceIndices)
            {
                var nativeMesh = new f3d_mesh_t
                {
                    points = points,
                    points_count = (nuint)Points.Length,
                    normals = normals,
                    normals_count = (nuint)Normals.Length,
                    texture_coordinates = textureCoordinates,
                    texture_coordinates_count = (nuint)TextureCoordinates.Length,
                    face_sides = faceSides,
                    face_sides_count = (nuint)FaceSides.Length,
                    face_indices = faceIndices,
                    face_indices_count = (nuint)FaceIndices.Length
                };

                sbyte* errorMessage = null;
                int isValid = NativeMethods.f3d_mesh_is_valid(&nativeMesh, &errorMessage);
                try
                {
                    string message = errorMessage == null
                        ? string.Empty
                        : Marshal.PtrToStringUTF8((IntPtr)errorMessage) ?? string.Empty;
                    return new ValidationResult(isValid != 0, message);
                }
                finally
                {
                    if (errorMessage != null)
                        NativeMethods.f3d_utils_string_destroy(errorMessage);
                }
            }
        }
    }

    /// <summary>
    /// Result of a mesh validation.
    /// </summary>
    public sealed class ValidationResult
    {
        public bool Valid { get; }
        public string ErrorMessage { get; }

        public ValidationResult(bool valid, string errorMessage)
        {
            Valid = valid;
            ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
        }
    }
}
