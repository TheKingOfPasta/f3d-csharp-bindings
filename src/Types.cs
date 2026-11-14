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
    public sealed unsafe class LightState : IEquatable<LightState>
    {
        public LightType Type { get; set; } = LightType.SceneLight;
        public double[] Position { get; set; } = [0.0, 0.0, 0.0];
        public double[] Color { get; set; } = [1.0, 1.0, 1.0];
        public double[] Direction { get; set; } = [1.0, 0.0, 0.0];
        public bool PositionalLight { get; set; }
        public double Intensity { get; set; } = 1.0;
        public bool SwitchState { get; set; } = true;
        public bool CastShadows { get; set; } = true;

        public static implicit operator LightState(f3d_light_state_t* ls)
        {
            LightState l = new LightState();

            l.Type = (LightType)ls->type;
            l.Position = [ ls->position[0], ls->position[1], ls->position[2] ];
            l.Color = [ ls->color.data[0], ls->color.data[1], ls->color.data[2]];
            l.Direction = [ ls->direction[0], ls->direction[1], ls->direction[2] ];
            l.PositionalLight = ls->positional_light == 1;
            l.Intensity = ls->intensity;
            l.SwitchState = ls->switch_state == 1;
            l.CastShadows = ls->cast_shadows == 1;

            return l;
        }

        public static implicit operator f3d_light_state_t*(LightState l)
        {
            f3d_light_state_t* ls = (f3d_light_state_t*)NativeMemory.Alloc((nuint)sizeof(f3d_light_state_t));

            ls->type = (f3d_light_type_t)l.Type;
            ls->position[0] = l.Position[0];
            ls->position[1] = l.Position[1];
            ls->position[2] = l.Position[2];
            ls->color.data[0] = l.Color[0];
            ls->color.data[1] = l.Color[1];
            ls->color.data[2] = l.Color[2];
            ls->direction[0] = l.Direction[0];
            ls->direction[1] = l.Direction[1];
            ls->direction[2] = l.Direction[2];
            ls->positional_light = l.PositionalLight ? 1 : 0;
            ls->intensity = l.Intensity;
            ls->switch_state = l.SwitchState ? 1 : 0;
            ls->cast_shadows = l.CastShadows ? 1 : 0;

            return ls;
        }

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
    public sealed unsafe class NodeState
    {
        public int Id { get; set; } = -1;
        public int ParentId { get; set; } = -1;
        public int Level { get; set; }
        public string Label { get; set; } = string.Empty;
        public bool Visible { get; set; } = true;
        public bool HasChildren { get; set; }
        public bool Collapsed { get; set; }

        public static implicit operator NodeState(f3d_node_state_t* n)
        {
            NodeState node = new NodeState();

            node.Id = n->id;
            node.ParentId = n->parent_id;
            node.Level = n->level;
            node.Label = "";
            for (int i = 0; n->label[i] != 0; i++)
                node.Label += n->label[i];

            node.Visible = n->visible == 1;
            node.HasChildren = n->has_children == 1;
            node.Collapsed = n->collapsed == 1;

            return node;
        }

        public static implicit operator f3d_node_state_t*(NodeState n)
        {
            f3d_node_state_t* ptr = (f3d_node_state_t*)NativeMemory.Alloc(
                (nuint)sizeof(f3d_node_state_t));

            ptr->id = n.Id;
            ptr->parent_id = n.ParentId;
            ptr->level = n.Level;

            ptr->label = (sbyte*)NativeMemory.Alloc((nuint)(sizeof(sbyte) * n.Label.Length));
            for (int i = 0; i < n.Label.Length; i++)
                ptr->label[i] = (sbyte)n.Label[i];

            ptr->visible = n.Visible ? 1 : 0;
            ptr->has_children = n.HasChildren ? 1 : 0;
            ptr->collapsed = n.Collapsed ? 1 : 0;

            return ptr;
        }
    }

    /// <summary>
    /// Describes the contents of a scene.
    /// NumberOfFiles includes files, meshes, and buffers added to the scene.
    /// </summary>
    public sealed unsafe class SceneInfo
    {
        public int NumberOfFiles { get; set; }
        public int NumberOfActors { get; set; }
        public long NumberOfPoints { get; set; }
        public long NumberOfCells { get; set; }

        public static implicit operator f3d_scene_info_t*(SceneInfo s)
        {
            f3d_scene_info_t* ptr = (f3d_scene_info_t*)NativeMemory.Alloc(
                (nuint)sizeof(f3d_scene_info_t));

            ptr->number_of_files = s.NumberOfFiles;
            ptr->number_of_actors = s.NumberOfActors;
            ptr->number_of_points = s.NumberOfPoints;
            ptr->number_of_cells = s.NumberOfCells;

            return ptr;
        }
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

        public static implicit operator f3d_mesh_t*(Mesh m)
        {
            f3d_mesh_t* ptr = (f3d_mesh_t*)NativeMemory.Alloc(
                (nuint)sizeof(f3d_mesh_t));

            ptr->points = (float*)NativeMemory.Alloc((nuint)(sizeof(float) * m.Points.Length));
            for (int i = 0; i < m.Points.Length; i++)
                ptr->points[i] = m.Points[i];
            ptr->points_count = (nuint)m.Points.Length;

            ptr->normals = (float*)NativeMemory.Alloc((nuint)(sizeof(float) * m.Normals.Length));
            for (int i = 0; i < m.Normals.Length; i++)
                ptr->normals[i] = m.Normals[i];
            ptr->normals_count = (nuint)m.Normals.Length;

            ptr->texture_coordinates = (float*)NativeMemory.Alloc((nuint)(sizeof(float) * m.TextureCoordinates.Length));
            for (int i = 0; i < m.TextureCoordinates.Length; i++)
                ptr->texture_coordinates[i] = m.TextureCoordinates[i];
            ptr->texture_coordinates_count = (nuint)m.TextureCoordinates.Length;

            ptr->face_sides = (uint*)NativeMemory.Alloc((nuint)(sizeof(uint) * m.FaceSides.Length));
            for (int i = 0; i < m.FaceSides.Length; i++)
                ptr->face_sides[i] = m.FaceSides[i];
            ptr->face_sides_count = (nuint)m.FaceSides.Length;

            ptr->face_indices = (uint*)NativeMemory.Alloc((nuint)(sizeof(uint) * m.FaceIndices.Length));
            for (int i = 0; i < m.FaceIndices.Length; i++)
                ptr->face_indices[i] = m.FaceIndices[i];
            ptr->face_indices_count = (nuint)m.FaceIndices.Length;

            return ptr;
        }

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
