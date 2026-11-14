using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: GeneratedCode("ClangSharp", "23.1.5.0")]

namespace F3D.Native
{
    public partial struct f3d_animation_t
    {
    }

    [NativeTypeName("unsigned int")]
    public enum f3d_animation_direction_t : uint
    {
        F3D_ANIMATION_FORWARD,
        F3D_ANIMATION_BACKWARD,
    }

    public partial struct f3d_camera_t
    {
    }

    public partial struct f3d_camera_state_t
    {
        [NativeTypeName("f3d_point3_t")]
        public _position_e__FixedBuffer position;

        [NativeTypeName("f3d_point3_t")]
        public _focal_point_e__FixedBuffer focal_point;

        [NativeTypeName("f3d_vector3_t")]
        public _view_up_e__FixedBuffer view_up;

        [NativeTypeName("f3d_angle_deg_t")]
        public double view_angle;

        [InlineArray(3)]
        public partial struct _position_e__FixedBuffer
        {
            public double e0;
        }

        [InlineArray(3)]
        public partial struct _focal_point_e__FixedBuffer
        {
            public double e0;
        }

        [InlineArray(3)]
        public partial struct _view_up_e__FixedBuffer
        {
            public double e0;
        }
    }

    public partial struct f3d_context
    {
    }

    public partial struct f3d_engine_t
    {
    }

    public partial struct f3d_options_t
    {
    }

    public partial struct f3d_options_t
    {
    }

    public unsafe partial struct f3d_backend_info_t
    {
        [NativeTypeName("char *")]
        public sbyte* name;

        public int available;
    }

    public unsafe partial struct f3d_module_info_t
    {
        [NativeTypeName("char *")]
        public sbyte* name;

        public int available;
    }

    public unsafe partial struct f3d_lib_info_t
    {
        [NativeTypeName("char *")]
        public sbyte* version;

        [NativeTypeName("char *")]
        public sbyte* version_full;

        [NativeTypeName("char *")]
        public sbyte* build_date;

        [NativeTypeName("char *")]
        public sbyte* build_system;

        [NativeTypeName("char *")]
        public sbyte* compiler;

        public f3d_module_info_t* modules;

        [NativeTypeName("char *")]
        public sbyte* vtk_version;

        [NativeTypeName("char **")]
        public sbyte** copyrights;

        [NativeTypeName("char *")]
        public sbyte* license;
    }

    public unsafe partial struct f3d_reader_info_t
    {
        [NativeTypeName("char *")]
        public sbyte* name;

        [NativeTypeName("char *")]
        public sbyte* description;

        [NativeTypeName("char **")]
        public sbyte** extensions;

        [NativeTypeName("char **")]
        public sbyte** mime_types;

        [NativeTypeName("char *")]
        public sbyte* plugin_name;

        public int has_scene_reader;

        public int has_geometry_reader;
    }

    [NativeTypeName("unsigned int")]
    public enum f3d_image_save_format_t : uint
    {
        PNG,
        JPG,
        TIF,
        BMP,
    }

    [NativeTypeName("unsigned int")]
    public enum f3d_image_channel_type_t : uint
    {
        BYTE,
        SHORT,
        FLOAT,
    }

    public partial struct f3d_image
    {
    }

    public partial struct f3d_interactor_t
    {
    }

    [NativeTypeName("unsigned int")]
    public enum f3d_interaction_bind_modifier_keys_t : uint
    {
        F3D_INTERACTION_BIND_ANY = 0x80,
        F3D_INTERACTION_BIND_NONE = 0x0,
        F3D_INTERACTION_BIND_CTRL = 0x1,
        F3D_INTERACTION_BIND_SHIFT = 0x2,
        F3D_INTERACTION_BIND_CTRL_SHIFT = 0x3,
    }

    public partial struct f3d_interaction_bind_t
    {
        public f3d_interaction_bind_modifier_keys_t mod;

        [NativeTypeName("char[256]")]
        public _inter_e__FixedBuffer inter;

        [InlineArray(256)]
        public partial struct _inter_e__FixedBuffer
        {
            public sbyte e0;
        }
    }

    [NativeTypeName("unsigned int")]
    public enum f3d_interactor_binding_type_t : uint
    {
        F3D_INTERACTOR_BINDING_CYCLIC = 0,
        F3D_INTERACTOR_BINDING_NUMERICAL = 1,
        F3D_INTERACTOR_BINDING_TOGGLE = 2,
        F3D_INTERACTOR_BINDING_OTHER = 3,
    }

    [NativeTypeName("unsigned int")]
    public enum f3d_interactor_mouse_button_t : uint
    {
        F3D_INTERACTOR_MOUSE_LEFT,
        F3D_INTERACTOR_MOUSE_RIGHT,
        F3D_INTERACTOR_MOUSE_MIDDLE,
    }

    [NativeTypeName("unsigned int")]
    public enum f3d_interactor_wheel_direction_t : uint
    {
        F3D_INTERACTOR_WHEEL_FORWARD,
        F3D_INTERACTOR_WHEEL_BACKWARD,
        F3D_INTERACTOR_WHEEL_LEFT,
        F3D_INTERACTOR_WHEEL_RIGHT,
    }

    [NativeTypeName("unsigned int")]
    public enum f3d_interactor_input_action_t : uint
    {
        F3D_INTERACTOR_INPUT_PRESS,
        F3D_INTERACTOR_INPUT_RELEASE,
    }

    [NativeTypeName("unsigned int")]
    public enum f3d_interactor_input_modifier_t : uint
    {
        F3D_INTERACTOR_INPUT_NONE,
        F3D_INTERACTOR_INPUT_CTRL,
        F3D_INTERACTOR_INPUT_SHIFT,
        F3D_INTERACTOR_INPUT_CTRL_SHIFT,
    }

    [NativeTypeName("unsigned int")]
    public enum f3d_interactor_animation_direction_t : uint
    {
        F3D_INTERACTOR_ANIMATION_FORWARD,
        F3D_INTERACTOR_ANIMATION_BACKWARD,
    }

    public unsafe partial struct f3d_binding_params_t
    {
        public f3d_interaction_bind_t bind;

        [NativeTypeName("const char **")]
        public sbyte** commands;

        public int command_count;

        [NativeTypeName("char *")]
        public sbyte* group;

        public f3d_interactor_binding_type_t type;

        public int notify;

        public int repeat;
    }

    public partial struct f3d_binding_documentation_t
    {
        [NativeTypeName("char[512]")]
        public _doc_e__FixedBuffer doc;

        [NativeTypeName("char[256]")]
        public _value_e__FixedBuffer value;

        [InlineArray(512)]
        public partial struct _doc_e__FixedBuffer
        {
            public sbyte e0;
        }

        [InlineArray(256)]
        public partial struct _value_e__FixedBuffer
        {
            public sbyte e0;
        }
    }

    [NativeTypeName("unsigned int")]
    public enum f3d_log_verbose_level_t : uint
    {
        F3D_LOG_DEBUG = 0,
        F3D_LOG_INFO,
        F3D_LOG_WARN,
        F3D_LOG_ERROR,
        F3D_LOG_QUIET,
    }

    [NativeTypeName("unsigned int")]
    public enum f3d_domain_style_t : uint
    {
        F3D_DOMAIN_STYLE_NONE = 0,
        F3D_DOMAIN_STYLE_RANGE = 1,
        F3D_DOMAIN_STYLE_ENUM = 2,
        F3D_DOMAIN_STYLE_INDEX = 3,
    }

    [NativeTypeName("unsigned int")]
    public enum f3d_option_type_t : uint
    {
        F3D_OPTION_TYPE_INVALID,
        F3D_OPTION_TYPE_BOOL,
        F3D_OPTION_TYPE_INT,
        F3D_OPTION_TYPE_DOUBLE,
        F3D_OPTION_TYPE_RATIO,
        F3D_OPTION_TYPE_STRING,
        F3D_OPTION_TYPE_PATH,
        F3D_OPTION_TYPE_COLOR,
        F3D_OPTION_TYPE_DIRECTION,
        F3D_OPTION_TYPE_COLORMAP,
        F3D_OPTION_TYPE_TRANSFORM2D,
        F3D_OPTION_TYPE_DOUBLE_VECTOR,
        F3D_OPTION_TYPE_INT_VECTOR,
    }

    public partial struct f3d_scene_t
    {
    }

    public partial struct f3d_ratio_t
    {
        public double value;
    }

    public partial struct f3d_color_t
    {
        [NativeTypeName("double[3]")]
        public _data_e__FixedBuffer data;

        [InlineArray(3)]
        public partial struct _data_e__FixedBuffer
        {
            public double e0;
        }
    }

    public partial struct f3d_direction_t
    {
        [NativeTypeName("double[3]")]
        public _data_e__FixedBuffer data;

        [InlineArray(3)]
        public partial struct _data_e__FixedBuffer
        {
            public double e0;
        }
    }

    public partial struct f3d_transform2d_t
    {
        [NativeTypeName("double[9]")]
        public _data_e__FixedBuffer data;

        [InlineArray(9)]
        public partial struct _data_e__FixedBuffer
        {
            public double e0;
        }
    }

    public unsafe partial struct f3d_colormap_t
    {
        public double* data;

        [NativeTypeName("size_t")]
        public nuint count;
    }

    public unsafe partial struct f3d_mesh_t
    {
        public float* points;

        [NativeTypeName("size_t")]
        public nuint points_count;

        public float* normals;

        [NativeTypeName("size_t")]
        public nuint normals_count;

        public float* texture_coordinates;

        [NativeTypeName("size_t")]
        public nuint texture_coordinates_count;

        [NativeTypeName("unsigned int *")]
        public uint* face_sides;

        [NativeTypeName("size_t")]
        public nuint face_sides_count;

        [NativeTypeName("unsigned int *")]
        public uint* face_indices;

        [NativeTypeName("size_t")]
        public nuint face_indices_count;
    }

    [NativeTypeName("unsigned int")]
    public enum f3d_mesh_data_type_t : uint
    {
        F3D_MESH_DATA_U8 = 0,
        F3D_MESH_DATA_I8,
        F3D_MESH_DATA_U16,
        F3D_MESH_DATA_I16,
        F3D_MESH_DATA_U32,
        F3D_MESH_DATA_I32,
        F3D_MESH_DATA_U64,
        F3D_MESH_DATA_I64,
        F3D_MESH_DATA_F32,
        F3D_MESH_DATA_F64,
    }

    public unsafe partial struct f3d_data_array_t
    {
        [NativeTypeName("const char *")]
        public sbyte* name;

        public f3d_mesh_data_type_t type;

        [NativeTypeName("const void *")]
        public void* data;

        [NativeTypeName("size_t")]
        public nuint components;

        [NativeTypeName("size_t")]
        public nuint stride;

        public int time_dependent;
    }

    public partial struct f3d_cell_array_t
    {
        [NativeTypeName("size_t")]
        public nuint offset_count;

        public f3d_data_array_t offsets;

        [NativeTypeName("size_t")]
        public nuint index_count;

        public f3d_data_array_t indices;
    }

    public unsafe partial struct f3d_memory_view_t
    {
        [NativeTypeName("size_t")]
        public nuint point_count;

        public f3d_data_array_t points;

        public f3d_data_array_t normals;

        public f3d_data_array_t texture_coordinates;

        public f3d_cell_array_t vertices;

        public f3d_cell_array_t lines;

        public f3d_cell_array_t polygons;

        [NativeTypeName("const f3d_data_array_t *")]
        public f3d_data_array_t* point_scalars;

        [NativeTypeName("size_t")]
        public nuint point_scalars_count;

        [NativeTypeName("const f3d_data_array_t *")]
        public f3d_data_array_t* cell_scalars;

        [NativeTypeName("size_t")]
        public nuint cell_scalars_count;
    }

    public unsafe partial struct f3d_mesh_view_t
    {
        public double time_min;

        public double time_max;

        [NativeTypeName("const char *")]
        public sbyte* name;

        [NativeTypeName("const f3d_memory_view_t *(*)(double, void *)")]
        public delegate* unmanaged[Cdecl]<double, void*, f3d_memory_view_t*> get_memory_view;

        [NativeTypeName("int (*)(void *)")]
        public delegate* unmanaged[Cdecl]<void*, int> buffers_need_update;

        [NativeTypeName("void (*)(void *)")]
        public delegate* unmanaged[Cdecl]<void*, void> buffers_updated;

        public void* opaque;
    }

    [NativeTypeName("unsigned int")]
    public enum f3d_light_type_t : uint
    {
        F3D_LIGHT_TYPE_HEADLIGHT = 1,
        F3D_LIGHT_TYPE_CAMERA_LIGHT = 2,
        F3D_LIGHT_TYPE_SCENE_LIGHT = 3,
    }

    public partial struct f3d_light_state_t
    {
        public f3d_light_type_t type;

        [NativeTypeName("f3d_point3_t")]
        public _position_e__FixedBuffer position;

        public f3d_color_t color;

        [NativeTypeName("f3d_vector3_t")]
        public _direction_e__FixedBuffer direction;

        public int positional_light;

        public double intensity;

        public int switch_state;

        public int cast_shadows;

        [InlineArray(3)]
        public partial struct _position_e__FixedBuffer
        {
            public double e0;
        }

        [InlineArray(3)]
        public partial struct _direction_e__FixedBuffer
        {
            public double e0;
        }
    }

    public unsafe partial struct f3d_node_state_t
    {
        public int id;

        public int parent_id;

        public int level;

        [NativeTypeName("char *")]
        public sbyte* label;

        public int visible;

        public int has_children;

        public int collapsed;
    }

    public partial struct f3d_scene_info_t
    {
        public int number_of_files;

        public int number_of_actors;

        [NativeTypeName("int64_t")]
        public long number_of_points;

        [NativeTypeName("int64_t")]
        public long number_of_cells;
    }

    [NativeTypeName("unsigned int")]
    public enum f3d_utils_known_folder_t : uint
    {
        F3D_UTILS_KNOWN_FOLDER_ROAMINGAPPDATA,
        F3D_UTILS_KNOWN_FOLDER_LOCALAPPDATA,
        F3D_UTILS_KNOWN_FOLDER_PICTURES,
    }

    public partial struct f3d_video_frame_t
    {
    }

    public partial struct f3d_video_packet_t
    {
    }

    public partial struct f3d_video_encoder_t
    {
    }

    [NativeTypeName("unsigned int")]
    public enum f3d_video_encoder_codec_t : uint
    {
        F3D_VIDEO_ENCODER_CODEC_H264,
        F3D_VIDEO_ENCODER_CODEC_HEVC,
        F3D_VIDEO_ENCODER_CODEC_VP8,
        F3D_VIDEO_ENCODER_CODEC_VP9,
        F3D_VIDEO_ENCODER_CODEC_AV1,
        F3D_VIDEO_ENCODER_CODEC_EXPLICIT,
    }

    public unsafe partial struct f3d_video_encoder_params_t
    {
        public f3d_video_encoder_codec_t codec;

        [NativeTypeName("const char *")]
        public sbyte* explicit_codec_name;

        public int width;

        public int height;

        public double frame_rate;

        public double bitrate;

        public int low_latency;
    }

    public partial struct f3d_window_t
    {
    }

    [NativeTypeName("unsigned int")]
    public enum f3d_window_type_t : uint
    {
        F3D_WINDOW_NONE,
        F3D_WINDOW_EXTERNAL,
        F3D_WINDOW_GLX,
        F3D_WINDOW_WGL,
        F3D_WINDOW_COCOA,
        F3D_WINDOW_EGL,
        F3D_WINDOW_OSMESA,
        F3D_WINDOW_WASM,
        F3D_WINDOW_UNKNOWN,
    }

    public static unsafe partial class NativeMethods
    {
        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_animation_load_time(f3d_animation_t* animation, double time_value);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern double* f3d_animation_get_key_frames(f3d_animation_t* animation, [NativeTypeName("unsigned int *")] uint* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_animation_destroy_key_frames(double* key_frames);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_animation_get_time_range(f3d_animation_t* animation, double* min_time, double* max_time);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint f3d_animation_count([NativeTypeName("const f3d_animation_t *")] f3d_animation_t* animation);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char *")]
        public static extern sbyte* f3d_animation_get_current_name([NativeTypeName("const f3d_animation_t *")] f3d_animation_t* animation);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char *")]
        public static extern sbyte* f3d_animation_get_name([NativeTypeName("const f3d_animation_t *")] f3d_animation_t* animation, int index);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char **")]
        public static extern sbyte** f3d_animation_get_names([NativeTypeName("const f3d_animation_t *")] f3d_animation_t* animation, int* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_animation_toggle(f3d_animation_t* animation, f3d_animation_direction_t direction);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_animation_start(f3d_animation_t* animation, f3d_animation_direction_t direction);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_animation_stop(f3d_animation_t* animation);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_animation_is_playing(f3d_animation_t* animation);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_animation_direction_t f3d_animation_get_direction(f3d_animation_t* animation);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_animation_destroy_string([NativeTypeName("const char *")] sbyte* str);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_animation_destroy_string_array([NativeTypeName("char **")] sbyte** array, int count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_set_position(f3d_camera_t* camera, [NativeTypeName("const f3d_point3_t")] double* pos);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_get_position([NativeTypeName("const f3d_camera_t *")] f3d_camera_t* camera, [NativeTypeName("f3d_point3_t")] double* pos);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_set_focal_point(f3d_camera_t* camera, [NativeTypeName("const f3d_point3_t")] double* focal_point);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_get_focal_point([NativeTypeName("const f3d_camera_t *")] f3d_camera_t* camera, [NativeTypeName("f3d_point3_t")] double* focal_point);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_set_view_up(f3d_camera_t* camera, [NativeTypeName("const f3d_vector3_t")] double* view_up);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_get_view_up([NativeTypeName("const f3d_camera_t *")] f3d_camera_t* camera, [NativeTypeName("f3d_vector3_t")] double* view_up);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_set_view_angle(f3d_camera_t* camera, [NativeTypeName("f3d_angle_deg_t")] double angle);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("f3d_angle_deg_t")]
        public static extern double f3d_camera_get_view_angle([NativeTypeName("const f3d_camera_t *")] f3d_camera_t* camera);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_set_state(f3d_camera_t* camera, [NativeTypeName("const f3d_camera_state_t *")] f3d_camera_state_t* state);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_get_state([NativeTypeName("const f3d_camera_t *")] f3d_camera_t* camera, f3d_camera_state_t* state);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_dolly(f3d_camera_t* camera, double val);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_pan(f3d_camera_t* camera, double right, double up, double forward);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_zoom(f3d_camera_t* camera, double factor);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_roll(f3d_camera_t* camera, [NativeTypeName("f3d_angle_deg_t")] double angle);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_azimuth(f3d_camera_t* camera, [NativeTypeName("f3d_angle_deg_t")] double angle);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_yaw(f3d_camera_t* camera, [NativeTypeName("f3d_angle_deg_t")] double angle);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_elevation(f3d_camera_t* camera, [NativeTypeName("f3d_angle_deg_t")] double angle);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_pitch(f3d_camera_t* camera, [NativeTypeName("f3d_angle_deg_t")] double angle);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_set_current_as_default(f3d_camera_t* camera);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_reset_to_default(f3d_camera_t* camera);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_camera_reset_to_bounds(f3d_camera_t* camera, double zoom_factor);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("f3d_context_t *")]
        public static extern f3d_context* f3d_context_glx();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("f3d_context_t *")]
        public static extern f3d_context* f3d_context_wgl();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("f3d_context_t *")]
        public static extern f3d_context* f3d_context_cocoa();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("f3d_context_t *")]
        public static extern f3d_context* f3d_context_egl();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("f3d_context_t *")]
        public static extern f3d_context* f3d_context_osmesa();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("f3d_context_t *")]
        public static extern f3d_context* f3d_context_get_symbol([MarshalAs(UnmanagedType.LPUTF8Str)] string lib, [MarshalAs(UnmanagedType.LPUTF8Str)] string func);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_context_destroy([NativeTypeName("f3d_context_t *")] f3d_context* ctx);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_engine_t* f3d_engine_create(int offscreen);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_engine_t* f3d_engine_create_none();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_engine_t* f3d_engine_create_glx(int offscreen);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_engine_t* f3d_engine_create_wgl(int offscreen);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_engine_t* f3d_engine_create_egl();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_engine_t* f3d_engine_create_osmesa();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_engine_t* f3d_engine_create_external([NativeTypeName("f3d_context_function_t")] delegate* unmanaged[Cdecl]<sbyte*, delegate* unmanaged[Cdecl]<void>> get_proc_address);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_engine_t* f3d_engine_create_external_glx();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_engine_t* f3d_engine_create_external_wgl();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_engine_t* f3d_engine_create_external_cocoa();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_engine_t* f3d_engine_create_external_egl();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_engine_t* f3d_engine_create_external_osmesa();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_engine_destroy(f3d_engine_t* engine);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_engine_set_cache_path(f3d_engine_t* engine, [MarshalAs(UnmanagedType.LPUTF8Str)] string cache_path);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* f3d_engine_get_cache_path(f3d_engine_t* engine);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_engine_set_options(f3d_engine_t* engine, f3d_options_t* options);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* f3d_engine_dump_to_string(f3d_engine_t* engine);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_engine_dump_to_file(f3d_engine_t* engine, [MarshalAs(UnmanagedType.LPUTF8Str)] string file_path);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_engine_dump_to_clipboard(f3d_engine_t* engine);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_engine_load_from_string(f3d_engine_t* engine, [MarshalAs(UnmanagedType.LPUTF8Str)] string content);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_engine_load_from_file(f3d_engine_t* engine, [MarshalAs(UnmanagedType.LPUTF8Str)] string file_path);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_engine_load_from_clipboard(f3d_engine_t* engine);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_engine_destroy_string([NativeTypeName("const char *")] sbyte* str);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_options_t* f3d_engine_get_options(f3d_engine_t* engine);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_window_t* f3d_engine_get_window(f3d_engine_t* engine);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_scene_t* f3d_engine_get_scene(f3d_engine_t* engine);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_interactor_t* f3d_engine_get_interactor(f3d_engine_t* engine);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_backend_info_t* f3d_engine_get_rendering_backend_list(int* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_engine_load_plugin([MarshalAs(UnmanagedType.LPUTF8Str)] string path_or_name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_engine_autoload_plugins();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char **")]
        public static extern sbyte** f3d_engine_get_plugins_list([MarshalAs(UnmanagedType.LPUTF8Str)] string plugin_path);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char **")]
        public static extern sbyte** f3d_engine_get_all_reader_option_names();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_engine_set_reader_option([MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_engine_destroy_backend_list(f3d_backend_info_t* backends);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_lib_info_t* f3d_engine_get_lib_info();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_engine_destroy_lib_info(f3d_lib_info_t* info);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_reader_info_t* f3d_engine_get_readers_info(int* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_engine_destroy_readers_info(f3d_reader_info_t* readers);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_engine_destroy_string_array([NativeTypeName("char **")] sbyte** array);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("f3d_image_t *")]
        public static extern f3d_image* f3d_image_create_empty();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("f3d_image_t *")]
        public static extern f3d_image* f3d_image_create_params([NativeTypeName("unsigned int")] uint width, [NativeTypeName("unsigned int")] uint height, [NativeTypeName("unsigned int")] uint channelCount, f3d_image_channel_type_t channelType);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("f3d_image_t *")]
        public static extern f3d_image* f3d_image_create_path([MarshalAs(UnmanagedType.LPUTF8Str)] string path);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("f3d_image_t *")]
        public static extern f3d_image* f3d_image_create_stream([NativeTypeName("unsigned char *")] byte* buffer, [NativeTypeName("unsigned int")] uint size);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_image_destroy([NativeTypeName("f3d_image_t *")] f3d_image* img);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_image_equals([NativeTypeName("f3d_image_t *")] f3d_image* img, [NativeTypeName("f3d_image_t *")] f3d_image* reference);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_image_not_equals([NativeTypeName("f3d_image_t *")] f3d_image* img, [NativeTypeName("f3d_image_t *")] f3d_image* reference);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_image_get_normalized_pixel([NativeTypeName("f3d_image_t *")] f3d_image* img, int x, int y, double* pixel);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint f3d_image_get_supported_formats_count();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char **")]
        public static extern sbyte** f3d_image_get_supported_formats();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint f3d_image_get_width([NativeTypeName("f3d_image_t *")] f3d_image* img);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint f3d_image_get_height([NativeTypeName("f3d_image_t *")] f3d_image* img);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint f3d_image_get_channel_count([NativeTypeName("f3d_image_t *")] f3d_image* img);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_image_get_channel_type([NativeTypeName("f3d_image_t *")] f3d_image* img);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint f3d_image_get_channel_type_size([NativeTypeName("f3d_image_t *")] f3d_image* img);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_image_set_content([NativeTypeName("f3d_image_t *")] f3d_image* img, void* buffer);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void* f3d_image_get_content([NativeTypeName("f3d_image_t *")] f3d_image* img);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern double f3d_image_compare([NativeTypeName("f3d_image_t *")] f3d_image* img, [NativeTypeName("f3d_image_t *")] f3d_image* reference);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_image_save([NativeTypeName("f3d_image_t *")] f3d_image* img, [MarshalAs(UnmanagedType.LPUTF8Str)] string path, f3d_image_save_format_t format);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("unsigned char *")]
        public static extern byte* f3d_image_save_buffer([NativeTypeName("f3d_image_t *")] f3d_image* img, f3d_image_save_format_t format, [NativeTypeName("unsigned int *")] uint* size);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_image_destroy_buffer([NativeTypeName("unsigned char *")] byte* buffer);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_image_to_terminal_text([NativeTypeName("f3d_image_t *")] f3d_image* img, void* stream);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* f3d_image_to_terminal_text_string([NativeTypeName("f3d_image_t *")] f3d_image* img);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_image_set_metadata([NativeTypeName("f3d_image_t *")] f3d_image* img, [MarshalAs(UnmanagedType.LPUTF8Str)] string key, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* f3d_image_get_metadata([NativeTypeName("f3d_image_t *")] f3d_image* img, [MarshalAs(UnmanagedType.LPUTF8Str)] string key);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char **")]
        public static extern sbyte** f3d_image_all_metadata([NativeTypeName("f3d_image_t *")] f3d_image* img, [NativeTypeName("unsigned int *")] uint* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_image_destroy_metadata_keys([NativeTypeName("char **")] sbyte** keys, [NativeTypeName("unsigned int")] uint count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interaction_bind_format([NativeTypeName("const f3d_interaction_bind_t *")] f3d_interaction_bind_t* bind, [NativeTypeName("char *")] sbyte* output, int output_size);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interaction_bind_parse([MarshalAs(UnmanagedType.LPUTF8Str)] string str, f3d_interaction_bind_t* bind);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_interaction_bind_less_than([NativeTypeName("const f3d_interaction_bind_t *")] f3d_interaction_bind_t* lhs, [NativeTypeName("const f3d_interaction_bind_t *")] f3d_interaction_bind_t* rhs);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_interaction_bind_equals([NativeTypeName("const f3d_interaction_bind_t *")] f3d_interaction_bind_t* lhs, [NativeTypeName("const f3d_interaction_bind_t *")] f3d_interaction_bind_t* rhs);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_init_commands(f3d_interactor_t* interactor);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_add_command(f3d_interactor_t* interactor, [MarshalAs(UnmanagedType.LPUTF8Str)] string action, [NativeTypeName("f3d_interactor_command_callback_t")] delegate* unmanaged[Cdecl]<sbyte**, int, void*, void> callback, void* user_data);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_remove_command(f3d_interactor_t* interactor, [MarshalAs(UnmanagedType.LPUTF8Str)] string action);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char **")]
        public static extern sbyte** f3d_interactor_get_command_actions(f3d_interactor_t* interactor, int* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_interactor_trigger_command(f3d_interactor_t* interactor, [MarshalAs(UnmanagedType.LPUTF8Str)] string command, int keep_comments);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_init_bindings(f3d_interactor_t* interactor);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_add_binding(f3d_interactor_t* interactor, f3d_binding_params_t binding);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_remove_binding(f3d_interactor_t* interactor, [NativeTypeName("const f3d_interaction_bind_t *")] f3d_interaction_bind_t* bind);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char **")]
        public static extern sbyte** f3d_interactor_get_bind_groups(f3d_interactor_t* interactor, int* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_interaction_bind_t* f3d_interactor_get_binds_for_group(f3d_interactor_t* interactor, [MarshalAs(UnmanagedType.LPUTF8Str)] string group, int* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_interaction_bind_t* f3d_interactor_get_binds(f3d_interactor_t* interactor, int* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_get_binding_documentation(f3d_interactor_t* interactor, [NativeTypeName("const f3d_interaction_bind_t *")] f3d_interaction_bind_t* bind, f3d_binding_documentation_t* doc);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_interactor_binding_type_t f3d_interactor_get_binding_type(f3d_interactor_t* interactor, [NativeTypeName("const f3d_interaction_bind_t *")] f3d_interaction_bind_t* bind);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_enable_camera_movement(f3d_interactor_t* interactor);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_disable_camera_movement(f3d_interactor_t* interactor);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_trigger_mod_update(f3d_interactor_t* interactor, f3d_interactor_input_modifier_t mod);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_trigger_mouse_button(f3d_interactor_t* interactor, f3d_interactor_input_action_t action, f3d_interactor_mouse_button_t button);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_trigger_mouse_position(f3d_interactor_t* interactor, double xpos, double ypos);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_trigger_mouse_wheel(f3d_interactor_t* interactor, f3d_interactor_wheel_direction_t direction);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_trigger_keyboard_key(f3d_interactor_t* interactor, f3d_interactor_input_action_t action, [MarshalAs(UnmanagedType.LPUTF8Str)] string key_sym);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_trigger_text_character(f3d_interactor_t* interactor, [NativeTypeName("unsigned int")] uint codepoint);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_trigger_event_loop(f3d_interactor_t* interactor, double delta_time);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_interactor_play_interaction(f3d_interactor_t* interactor, [MarshalAs(UnmanagedType.LPUTF8Str)] string file_path, double delta_time);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_interactor_record_interaction(f3d_interactor_t* interactor, [MarshalAs(UnmanagedType.LPUTF8Str)] string file_path);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_start(f3d_interactor_t* interactor, double delta_time);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_set_event_loop_user_callback(f3d_interactor_t* interactor, [NativeTypeName("f3d_interactor_callback_t")] delegate* unmanaged[Cdecl]<void*, void> callback, void* user_data);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_set_notification_callback(f3d_interactor_t* interactor, [NativeTypeName("f3d_interactor_notification_callback_t")] delegate* unmanaged[Cdecl]<sbyte*, sbyte*, sbyte*, double, void*, int> callback, void* user_data);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_stop(f3d_interactor_t* interactor);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_request_render(f3d_interactor_t* interactor);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_request_stop(f3d_interactor_t* interactor);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_trigger_notification(f3d_interactor_t* interactor, [MarshalAs(UnmanagedType.LPUTF8Str)] string desc, [MarshalAs(UnmanagedType.LPUTF8Str)] string value, double duration);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_destroy_string_array([NativeTypeName("char **")] sbyte** array, int count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_interactor_destroy_bind_array(f3d_interaction_bind_t* array);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_log_print(f3d_log_verbose_level_t level, [MarshalAs(UnmanagedType.LPUTF8Str)] string message);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_log_debug([MarshalAs(UnmanagedType.LPUTF8Str)] string message);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_log_info([MarshalAs(UnmanagedType.LPUTF8Str)] string message);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_log_warn([MarshalAs(UnmanagedType.LPUTF8Str)] string message);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_log_error([MarshalAs(UnmanagedType.LPUTF8Str)] string message);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_log_set_use_coloring(int use);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_log_set_verbose_level(f3d_log_verbose_level_t level, int force_std_err);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_log_verbose_level_t f3d_log_get_verbose_level();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_log_forward([NativeTypeName("f3d_log_forward_fn_t")] delegate* unmanaged[Cdecl]<f3d_log_verbose_level_t, sbyte*, void> callback);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_options_t* f3d_options_create();

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_destroy(f3d_options_t* options);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_set_as_bool(f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, int value);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_set_as_int(f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, int value);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_set_as_double(f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, double value);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_set_as_string(f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string value);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_set_as_double_vector(f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [NativeTypeName("const double *")] double* values, [NativeTypeName("size_t")] nuint count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_set_as_int_vector(f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [NativeTypeName("const int *")] int* values, [NativeTypeName("size_t")] nuint count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_options_get_as_bool([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_options_get_as_int([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern double f3d_options_get_as_double([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* f3d_options_get_as_string([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* f3d_options_get_as_string_representation([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_set_as_string_representation(f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string str);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_destroy_string([NativeTypeName("const char *")] sbyte* str);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_destroy_string_array([NativeTypeName("char **")] sbyte** array, int count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_get_as_double_vector([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, double* values, [NativeTypeName("size_t *")] nuint* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_get_as_int_vector([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, int* values, [NativeTypeName("size_t *")] nuint* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_toggle(f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_options_is_same([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [NativeTypeName("const f3d_options_t *")] f3d_options_t* other, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_options_has_value([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_copy(f3d_options_t* options, [NativeTypeName("const f3d_options_t *")] f3d_options_t* other, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char **")]
        public static extern sbyte** f3d_options_get_all_names([NativeTypeName("size_t *")] nuint* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char **")]
        public static extern sbyte** f3d_options_get_names([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [NativeTypeName("size_t *")] nuint* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_get_closest_option([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string option, [NativeTypeName("char **")] sbyte** closest, [NativeTypeName("unsigned int *")] uint* distance);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_destroy_names([NativeTypeName("char **")] sbyte** names, [NativeTypeName("size_t")] nuint count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_options_is_optional([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_reset(f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_remove_value(f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_options_has_domain([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_domain_style_t f3d_options_get_domain_style([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_options_get_range_domain_double([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [NativeTypeName("double[3]")] double* range);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_options_get_range_domain_int([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [NativeTypeName("int[3]")] int* range);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char **")]
        public static extern sbyte** f3d_options_get_enum_domain_string([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, int* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_options_get_index_domain([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [NativeTypeName("unsigned int *")] uint* max);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_increase(f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_decrease(f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_cycle(f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_option_type_t f3d_options_get_type([NativeTypeName("const f3d_options_t *")] f3d_options_t* options, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_options_parse_bool([MarshalAs(UnmanagedType.LPUTF8Str)] string str);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_options_parse_int([MarshalAs(UnmanagedType.LPUTF8Str)] string str);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern double f3d_options_parse_double([MarshalAs(UnmanagedType.LPUTF8Str)] string str);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* f3d_options_parse_string([MarshalAs(UnmanagedType.LPUTF8Str)] string str);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_parse_double_vector([MarshalAs(UnmanagedType.LPUTF8Str)] string str, double* values, [NativeTypeName("size_t *")] nuint* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_options_parse_int_vector([MarshalAs(UnmanagedType.LPUTF8Str)] string str, int* values, [NativeTypeName("size_t *")] nuint* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* f3d_options_format_bool(int value);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* f3d_options_format_int(int value);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* f3d_options_format_double(double value);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* f3d_options_format_string([MarshalAs(UnmanagedType.LPUTF8Str)] string value);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* f3d_options_format_double_vector([NativeTypeName("const double *")] double* values, [NativeTypeName("size_t")] nuint count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* f3d_options_format_int_vector([NativeTypeName("const int *")] int* values, [NativeTypeName("size_t")] nuint count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_scene_add(f3d_scene_t* scene, [MarshalAs(UnmanagedType.LPUTF8Str)] string file_path);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_scene_add_multiple(f3d_scene_t* scene, [NativeTypeName("const char **")] sbyte** file_paths, [NativeTypeName("size_t")] nuint count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_scene_add_mesh(f3d_scene_t* scene, [NativeTypeName("const f3d_mesh_t *")] f3d_mesh_t* mesh);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_scene_add_mesh_view(f3d_scene_t* scene, [NativeTypeName("const f3d_mesh_view_t *")] f3d_mesh_view_t* mesh_view);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_scene_add_buffer(f3d_scene_t* scene, void* buffer, [NativeTypeName("size_t")] nuint size);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_scene_clear(f3d_scene_t* scene);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char **")]
        public static extern sbyte** f3d_scene_get_added_files([NativeTypeName("const f3d_scene_t *")] f3d_scene_t* scene, [NativeTypeName("unsigned int *")] uint* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_scene_destroy_added_files([NativeTypeName("char **")] sbyte** files, [NativeTypeName("unsigned int")] uint count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_scene_add_light([NativeTypeName("const f3d_scene_t *")] f3d_scene_t* scene, [NativeTypeName("const f3d_light_state_t *")] f3d_light_state_t* light_state);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_scene_get_light_count([NativeTypeName("const f3d_scene_t *")] f3d_scene_t* scene);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_light_state_t* f3d_scene_get_light([NativeTypeName("const f3d_scene_t *")] f3d_scene_t* scene, int index);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_scene_update_light(f3d_scene_t* scene, int index, [NativeTypeName("const f3d_light_state_t *")] f3d_light_state_t* light_state);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_scene_remove_light(f3d_scene_t* scene, int index);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_scene_remove_all_lights(f3d_scene_t* scene);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_node_state_t* f3d_scene_get_scene_hierarchy([NativeTypeName("const f3d_scene_t *")] f3d_scene_t* scene, [NativeTypeName("unsigned int *")] uint* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_scene_destroy_scene_hierarchy(f3d_node_state_t* nodes, [NativeTypeName("unsigned int")] uint count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_scene_set_node_visibility(f3d_scene_t* scene, int node_id, int visible);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_scene_get_scene_info([NativeTypeName("const f3d_scene_t *")] f3d_scene_t* scene, f3d_scene_info_t* info);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_scene_supports(f3d_scene_t* scene, [MarshalAs(UnmanagedType.LPUTF8Str)] string file_path);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_scene_supports_buffer(f3d_scene_t* scene, [NativeTypeName("const void *")] void* buffer, [NativeTypeName("size_t")] nuint size);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_animation_t* f3d_scene_get_animation(f3d_scene_t* scene);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern double f3d_color_r([NativeTypeName("const f3d_color_t *")] f3d_color_t* color);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern double f3d_color_g([NativeTypeName("const f3d_color_t *")] f3d_color_t* color);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern double f3d_color_b([NativeTypeName("const f3d_color_t *")] f3d_color_t* color);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_color_set(f3d_color_t* color, double r, double g, double b);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern double f3d_direction_x([NativeTypeName("const f3d_direction_t *")] f3d_direction_t* dir);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern double f3d_direction_y([NativeTypeName("const f3d_direction_t *")] f3d_direction_t* dir);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern double f3d_direction_z([NativeTypeName("const f3d_direction_t *")] f3d_direction_t* dir);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_direction_set(f3d_direction_t* dir, double x, double y, double z);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_transform2d_create(f3d_transform2d_t* transform, double scale_x, double scale_y, double translate_x, double translate_y, [NativeTypeName("f3d_angle_deg_t")] double angle_deg);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_colormap_destroy(f3d_colormap_t* colormap);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_mesh_is_valid([NativeTypeName("const f3d_mesh_t *")] f3d_mesh_t* mesh, [NativeTypeName("char **")] sbyte** error_message);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_light_state_destroy(f3d_light_state_t* light_state);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_light_state_equal([NativeTypeName("const f3d_light_state_t *")] f3d_light_state_t* a, [NativeTypeName("const f3d_light_state_t *")] f3d_light_state_t* b);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint f3d_utils_text_distance([MarshalAs(UnmanagedType.LPUTF8Str)] string str_a, [MarshalAs(UnmanagedType.LPUTF8Str)] string str_b);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char **")]
        public static extern sbyte** f3d_utils_tokenize([MarshalAs(UnmanagedType.LPUTF8Str)] string str, int keep_comments, [NativeTypeName("size_t *")] nuint* out_count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_utils_tokens_destroy([NativeTypeName("char **")] sbyte** tokens, [NativeTypeName("size_t")] nuint count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char *")]
        public static extern sbyte* f3d_utils_collapse_path([MarshalAs(UnmanagedType.LPUTF8Str)] string path, [MarshalAs(UnmanagedType.LPUTF8Str)] string base_directory);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char *")]
        public static extern sbyte* f3d_utils_glob_to_regex([MarshalAs(UnmanagedType.LPUTF8Str)] string glob, [NativeTypeName("char")] sbyte path_separator);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char *")]
        public static extern sbyte* f3d_utils_get_env([MarshalAs(UnmanagedType.LPUTF8Str)] string env);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char *")]
        public static extern sbyte* f3d_utils_get_known_folder(f3d_utils_known_folder_t known_folder);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_utils_string_destroy([NativeTypeName("char *")] sbyte* str);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_video_frame_destroy(f3d_video_frame_t* frame);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_video_frame_set_timestamp(f3d_video_frame_t* frame, [NativeTypeName("int64_t")] long timestamp);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("size_t")]
        public static extern nuint f3d_video_packet_get_packet_size([NativeTypeName("const f3d_video_packet_t *")] f3d_video_packet_t* packet);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const unsigned char *")]
        public static extern byte* f3d_video_packet_get_packet_data([NativeTypeName("const f3d_video_packet_t *")] f3d_video_packet_t* packet);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int64_t")]
        public static extern long f3d_video_packet_get_timestamp([NativeTypeName("const f3d_video_packet_t *")] f3d_video_packet_t* packet);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_video_packet_is_key_frame([NativeTypeName("const f3d_video_packet_t *")] f3d_video_packet_t* packet);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_video_encoder_get_available_encoders([NativeTypeName("char ***")] sbyte*** names, [NativeTypeName("char ***")] sbyte*** descriptions, int* count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_video_encoder_free_available_encoders([NativeTypeName("char **")] sbyte** names, [NativeTypeName("char **")] sbyte** descriptions, int count);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_video_encoder_t* f3d_video_encoder_create([NativeTypeName("const f3d_video_encoder_params_t *")] f3d_video_encoder_params_t* @params);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_video_encoder_destroy(f3d_video_encoder_t* encoder);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_video_encoder_get_width([NativeTypeName("const f3d_video_encoder_t *")] f3d_video_encoder_t* encoder);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_video_encoder_get_height([NativeTypeName("const f3d_video_encoder_t *")] f3d_video_encoder_t* encoder);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_video_encoder_listen(f3d_video_encoder_t* encoder, [NativeTypeName("f3d_video_packet_callback_t")] delegate* unmanaged[Cdecl]<f3d_video_packet_t*, void*, void> callback, void* user_data);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_video_encoder_submit(f3d_video_encoder_t* encoder, f3d_video_frame_t* frame);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_video_encoder_flush(f3d_video_encoder_t* encoder);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_window_type_t f3d_window_get_type(f3d_window_t* window);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_window_is_offscreen(f3d_window_t* window);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_camera_t* f3d_window_get_camera(f3d_window_t* window);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_window_render(f3d_window_t* window);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("f3d_image_t *")]
        public static extern f3d_image* f3d_window_render_to_image(f3d_window_t* window, int no_background);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_window_set_size(f3d_window_t* window, int width, int height);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_window_get_size([NativeTypeName("const f3d_window_t *")] f3d_window_t* window, int* width, int* height);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_window_get_width([NativeTypeName("const f3d_window_t *")] f3d_window_t* window);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_window_get_height([NativeTypeName("const f3d_window_t *")] f3d_window_t* window);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_window_set_position(f3d_window_t* window, int x, int y);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_window_get_position([NativeTypeName("const f3d_window_t *")] f3d_window_t* window, int* x, int* y);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_window_get_left([NativeTypeName("const f3d_window_t *")] f3d_window_t* window);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int f3d_window_get_top([NativeTypeName("const f3d_window_t *")] f3d_window_t* window);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_window_set_icon(f3d_window_t* window, [NativeTypeName("const unsigned char *")] byte* icon, [NativeTypeName("size_t")] nuint icon_size);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_window_set_window_name(f3d_window_t* window, [MarshalAs(UnmanagedType.LPUTF8Str)] string window_name);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_window_get_world_from_display([NativeTypeName("const f3d_window_t *")] f3d_window_t* window, [NativeTypeName("const f3d_point3_t")] double* display_point, [NativeTypeName("f3d_point3_t")] double* world_point);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_window_get_display_from_world([NativeTypeName("const f3d_window_t *")] f3d_window_t* window, [NativeTypeName("const f3d_point3_t")] double* world_point, [NativeTypeName("f3d_point3_t")] double* display_point);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern double f3d_window_get_dpi_scale(f3d_window_t* window);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern f3d_video_frame_t* f3d_window_get_video_frame(f3d_window_t* window);

        [DllImport("libf3d_c_api", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void f3d_window_set_use_hdri_cache(f3d_window_t* window, int use_hdri_cache);
    }

    /// <summary>Defines the type of a member as it was used in the native signature.</summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false, Inherited = true)]
    [Conditional("DEBUG")]
    internal sealed partial class NativeTypeNameAttribute : Attribute
    {
        private readonly string _name;

        /// <summary>Initializes a new instance of the <see cref="NativeTypeNameAttribute" /> class.</summary>
        /// <param name="name">The name of the type that was used in the native signature.</param>
        public NativeTypeNameAttribute(string name)
        {
            _name = name;
        }

        /// <summary>Gets the name of the type that was used in the native signature.</summary>
        public string Name => _name;
    }

    /// <summary>Defines the annotation found in a native declaration.</summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
    [Conditional("DEBUG")]
    internal sealed partial class NativeAnnotationAttribute : Attribute
    {
        private readonly string _annotation;

        /// <summary>Initializes a new instance of the <see cref="NativeAnnotationAttribute" /> class.</summary>
        /// <param name="annotation">The annotation that was used in the native declaration.</param>
        public NativeAnnotationAttribute(string annotation)
        {
            _annotation = annotation;
        }

        /// <summary>Gets the annotation that was used in the native declaration.</summary>
        public string Annotation => _annotation;
    }
}
