using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using F3D.Native;

namespace F3D;

public sealed unsafe class Interactor
{
    /// <summary>
    /// Thrown when adding a command or binding that already exists.
    /// </summary>
    public class AlreadyExistsException : F3DException
    {
        public AlreadyExistsException(string message) : base(message) { }
    }

    /// <summary>
    /// Thrown when removing a command or binding that does not exist.
    /// </summary>
    public class DoesNotExistException : F3DException
    {
        public DoesNotExistException(string message) : base(message) { }
    }

    // Values match the C enums so they can be cast directly.

    public enum ModifierKeys : uint
    {
        Any = 0x80,
        None = 0x0,
        Ctrl = 0x1,
        Shift = 0x2,
        CtrlShift = 0x3,
    }

    public enum BindingType : uint
    {
        Cyclic = 0,
        Numerical = 1,
        Toggle = 2,
        Other = 3,
    }

    public enum MouseButton : uint
    {
        Left,
        Right,
        Middle,
    }

    public enum WheelDirection : uint
    {
        Forward,
        Backward,
        Left,
        Right,
    }

    public enum InputAction : uint
    {
        Press,
        Release,
    }

    public enum InputModifier : uint
    {
        None,
        Ctrl,
        Shift,
        CtrlShift,
    }

    public sealed record InteractionBind(ModifierKeys Mod, string Inter) : IComparable<InteractionBind>
    {
        public InteractionBind() : this(ModifierKeys.None, "") { }

        public string Format() => Mod switch
        {
            ModifierKeys.CtrlShift => "Ctrl+Shift+" + Inter,
            ModifierKeys.Ctrl => "Ctrl+" + Inter,
            ModifierKeys.Shift => "Shift+" + Inter,
            ModifierKeys.Any => "Any+" + Inter,
            _ => Inter,
        };

        public static InteractionBind Parse(string str)
        {
            ArgumentNullException.ThrowIfNull(str);
            int plus = str.LastIndexOf('+');
            if (plus == -1) return new(ModifierKeys.None, str);

            ModifierKeys mod = str[..plus] switch
            {
                "Ctrl+Shift" => ModifierKeys.CtrlShift,
                "Shift" => ModifierKeys.Shift,
                "Ctrl" => ModifierKeys.Ctrl,
                "Any" => ModifierKeys.Any,
                _ => ModifierKeys.None,
            };
            return new(mod, str[(plus + 1)..]);
        }

        public int CompareTo(InteractionBind? other)
        {
            if (other is null) return 1;
            if (Mod != other.Mod) return Mod.CompareTo(other.Mod);
            return string.CompareOrdinal(Inter, other.Inter);
        }

        public override string ToString() => Format();

        internal f3d_interaction_bind_t ToNative()
        {
            var bytes = Encoding.UTF8.GetBytes(Inter ?? "");
            if (bytes.Length > 255) throw new ArgumentException("The interaction is longer than 255 bytes.", nameof(Inter));

            f3d_interaction_bind_t native = default;
            native.mod = (f3d_interaction_bind_modifier_keys_t)Mod;
            bytes.CopyTo(new Span<byte>(&native.inter, 256));
            return native;
        }

        internal static InteractionBind FromNative(f3d_interaction_bind_t* native) =>
            new((ModifierKeys)native->mod, Marshal.PtrToStringUTF8((nint)(&native->inter)) ?? "");
    }

    public sealed record BindingDocumentation(string Documentation, string CurrentValue);

    public sealed class BindingParam
    {
        public BindingParam(InteractionBind bind, IEnumerable<string> commands)
        {
            Bind = bind ?? throw new ArgumentNullException(nameof(bind));
            Commands = (commands ?? throw new ArgumentNullException(nameof(commands))).ToList();
            if (Commands.Count == 0) throw new ArgumentException("A binding needs at least one command.", nameof(commands));
        }

        public InteractionBind Bind { get; }
        public IReadOnlyList<string> Commands { get; }
        public string Group { get; init; } = "";
        public BindingType Type { get; init; } = BindingType.Other;
        public bool Notify { get; init; } = true;
        public bool Repeat { get; init; }
    }

    readonly f3d_interactor_t* _handle;

    // Callbacks given to F3D. The GCHandles keep them alive while F3D can call them
    readonly Dictionary<string, GCHandle> _commands = new();
    GCHandle _eventLoopCallback;
    GCHandle _notificationCallback;

    internal Interactor(f3d_interactor_t* handle) => _handle = handle;

    /// <summary>
    /// Remove all existing commands and add all default commands.
    /// </summary>
    /// <returns>this interactor for method chaining</returns>
    public Interactor InitCommands()
    {
        NativeMethods.f3d_interactor_init_commands(_handle);
        foreach (var handle in _commands.Values) handle.Free();
        _commands.Clear();
        return this;
    }

    /// <summary>
    /// Add a command to be called using triggerCommand.
    /// </summary>
    /// <param name="action">action name for the command</param>
    /// <param name="callback">callback to execute when command is triggered</param>
    /// <returns>this interactor for method chaining</returns>
    public Interactor AddCommand(string action, Action<IReadOnlyList<string>> callback)
    {
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(callback);
        if (GetCommandActions().Contains(action))
            throw new AlreadyExistsException($"Command already exists: {action}");

        var handle = GCHandle.Alloc(callback);
        NativeMethods.f3d_interactor_add_command(_handle, action, &OnCommand, (void*)GCHandle.ToIntPtr(handle));
        _commands[action] = handle;
        return this;
    }

    /// <summary>
    /// Remove a command for provided action.
    /// </summary>
    /// <param name="action">action name to remove</param>
    /// <returns>this interactor for method chaining</returns>
    public Interactor RemoveCommand(string action)
    {
        ArgumentNullException.ThrowIfNull(action);
        NativeMethods.f3d_interactor_remove_command(_handle, action);
        if (_commands.Remove(action, out var handle)) handle.Free();
        return this;
    }

    /// <summary>
    /// Return a list containing all currently defined actions of commands.
    /// </summary>
    /// <returns>list of command actions</returns>
    public List<string> GetCommandActions()
    {
        int count = 0;
        var array = NativeMethods.f3d_interactor_get_command_actions(_handle, &count);
        return ToStringList(array, count);
    }

    /// <summary>
    /// Trigger provided command.
    /// </summary>
    /// <param name="command">command string to execute</param>
    /// <param name="keepComments">if true, comments with # are supported</param>
    /// <returns>true if command succeeded, false otherwise</returns>
    public bool TriggerCommand(string command, bool keepComments = true)
    {
        ArgumentNullException.ThrowIfNull(command);
        return NativeMethods.f3d_interactor_trigger_command(_handle, command, keepComments ? 1 : 0) != 0;
    }

    /// <summary>
    /// Remove all existing interaction commands and add all default bindings.
    /// </summary>
    /// <returns>this interactor for method chaining</returns>
    public Interactor InitBindings()
    {
        NativeMethods.f3d_interactor_init_bindings(_handle);
        return this;
    }

    /// <summary>
    /// Add binding to trigger commands for a specific bind.
    /// </summary>
    /// <param name="binding">The binding definition to register with the interactor.</param>
    /// <returns>this interactor for method chaining</returns>
    public Interactor AddBinding(BindingParam binding)
    {
        ArgumentNullException.ThrowIfNull(binding);
        if (GetBinds().Contains(binding.Bind))
            throw new AlreadyExistsException($"Binding already exists: {binding.Bind}");

        // The commands and the group are copied to native memory for the duration of the call
        int count = binding.Commands.Count;
        var commands = stackalloc sbyte*[count];
        sbyte* group = null;
        try
        {
            for (int i = 0; i < count; i++)
                commands[i] = (sbyte*)Marshal.StringToCoTaskMemUTF8(binding.Commands[i]);
            group = (sbyte*)Marshal.StringToCoTaskMemUTF8(binding.Group ?? "");

            var native = new f3d_binding_params_t
            {
                bind = binding.Bind.ToNative(),
                commands = commands,
                command_count = count,
                group = group,
                type = (f3d_interactor_binding_type_t)binding.Type,
                notify = binding.Notify ? 1 : 0,
                repeat = binding.Repeat ? 1 : 0,
            };
            NativeMethods.f3d_interactor_add_binding(_handle, native);
        }
        finally
        {
            for (int i = 0; i < count; i++) Marshal.FreeCoTaskMem((nint)commands[i]);
            Marshal.FreeCoTaskMem((nint)group);
        }
        return this;
    }

    /// <summary>
    /// Remove binding corresponding to provided bind.
    /// </summary>
    /// <param name="bind">binding to remove</param>
    /// <returns>this interactor for method chaining</returns>
    public Interactor RemoveBinding(InteractionBind bind)
    {
        ArgumentNullException.ThrowIfNull(bind);
        if (!GetBinds().Contains(bind))
            throw new DoesNotExistException($"Binding does not exist: {bind}");

        var native = bind.ToNative();
        NativeMethods.f3d_interactor_remove_binding(_handle, &native);
        return this;
    }

    /// <summary>
    /// Return a vector of available bind groups, in order of addition.
    /// </summary>
    /// <returns>list of bind group names</returns>
    public List<string> GetBindGroups()
    {
        int count = 0;
        var array = NativeMethods.f3d_interactor_get_bind_groups(_handle, &count);
        return ToStringList(array, count);
    }

    /// <summary>
    /// Return a vector of bind for the specified group, in order of addition.
    /// </summary>
    /// <param name="group">group name</param>
    /// <returns>list of binds for the group</returns>
    public List<InteractionBind> GetBindsForGroup(string group)
    {
        ArgumentNullException.ThrowIfNull(group);
        int count = 0;
        return ToBindList(NativeMethods.f3d_interactor_get_binds_for_group(_handle, group, &count), count);
    }

    /// <summary>
    /// Return a vector of all binds, in order of addition.
    /// </summary>
    /// <returns>list of all binds</returns>
    public List<InteractionBind> GetBinds()
    {
        int count = 0;
        return ToBindList(NativeMethods.f3d_interactor_get_binds(_handle, &count), count);
    }

    /// <summary>
    /// Get documentation for a binding.
    /// </summary>
    /// <param name="bind">binding to get documentation for</param>
    /// <returns>documentation pair (description, current value)</returns>
    public BindingDocumentation GetBindingDocumentation(InteractionBind bind)
    {
        ArgumentNullException.ThrowIfNull(bind);
        var native = bind.ToNative();
        f3d_binding_documentation_t doc = default;
        NativeMethods.f3d_interactor_get_binding_documentation(_handle, &native, &doc);
        return new(
            Marshal.PtrToStringUTF8((nint)(&doc.doc)) ?? "",
            Marshal.PtrToStringUTF8((nint)(&doc.value)) ?? "");
    }

    /// <summary>
    /// Get the type of a binding.
    /// </summary>
    /// <param name="bind">binding to get type for</param>
    /// <returns>binding type</returns>
    public BindingType GetBindingType(InteractionBind bind)
    {
        ArgumentNullException.ThrowIfNull(bind);
        var native = bind.ToNative();
        return (BindingType)NativeMethods.f3d_interactor_get_binding_type(_handle, &native);
    }

    /// <summary>
    /// Enable camera movement.
    /// </summary>
    /// <returns>this interactor for method chaining</returns>
    public Interactor EnableCameraMovement()
    {
        NativeMethods.f3d_interactor_enable_camera_movement(_handle);
        return this;
    }

    /// <summary>
    /// Disable camera movement.
    /// </summary>
    /// <returns>this interactor for method chaining</returns>
    public Interactor DisableCameraMovement()
    {
        NativeMethods.f3d_interactor_disable_camera_movement(_handle);
        return this;
    }

    /// <summary>
    /// Trigger a modifier update.
    /// </summary>
    /// <param name="mod">modifier state</param>
    /// <returns>this interactor for method chaining</returns>
    public Interactor TriggerModUpdate(InputModifier mod)
    {
        NativeMethods.f3d_interactor_trigger_mod_update(_handle, (f3d_interactor_input_modifier_t)mod);
        return this;
    }

    /// <summary>
    /// Trigger a mouse button event.
    /// </summary>
    /// <param name="action">press or release action</param>
    /// <param name="button">mouse button</param>
    /// <returns>this interactor for method chaining</returns>
    public Interactor TriggerMouseButton(InputAction action, MouseButton button)
    {
        NativeMethods.f3d_interactor_trigger_mouse_button(_handle,
            (f3d_interactor_input_action_t)action, (f3d_interactor_mouse_button_t)button);
        return this;
    }

    /// <summary>
    /// Trigger a mouse new position event.
    /// </summary>
    /// <param name="xpos">x position in pixels</param>
    /// <param name="ypos">y position in pixels</param>
    /// <returns>this interactor for method chaining</returns>
    public Interactor TriggerMousePosition(double xpos, double ypos)
    {
        NativeMethods.f3d_interactor_trigger_mouse_position(_handle, xpos, ypos);
        return this;
    }

    /// <summary>
    /// Trigger a mouse wheel event.
    /// </summary>
    /// <param name="direction">wheel direction</param>
    /// <returns>this interactor for method chaining</returns>
    public Interactor TriggerMouseWheel(WheelDirection direction)
    {
        NativeMethods.f3d_interactor_trigger_mouse_wheel(_handle, (f3d_interactor_wheel_direction_t)direction);
        return this;
    }

    /// <summary>
    /// Trigger a keyboard key event.
    /// </summary>
    /// <param name="action">press or release action</param>
    /// <param name="keySym">key symbol (e.g., "A", "Left", "Space")</param>
    /// <returns>this interactor for method chaining</returns>
    public Interactor TriggerKeyboardKey(InputAction action, string keySym)
    {
        ArgumentNullException.ThrowIfNull(keySym);
        NativeMethods.f3d_interactor_trigger_keyboard_key(_handle, (f3d_interactor_input_action_t)action, keySym);
        return this;
    }

    /// <summary>
    /// Trigger a text character input event.
    /// </summary>
    /// <param name="codepoint">Unicode codepoint</param>
    /// <returns>this interactor for method chaining</returns>
    public Interactor TriggerTextCharacter(uint codepoint)
    {
        NativeMethods.f3d_interactor_trigger_text_character(_handle, codepoint);
        return this;
    }

    /// <summary>
    /// Manually trigger the event loop.
    /// </summary>
    /// <param name="deltaTime">time delta in seconds (must be positive)</param>
    /// <returns>this interactor for method chaining</returns>
    public Interactor TriggerEventLoop(double deltaTime)
    {
        NativeMethods.f3d_interactor_trigger_event_loop(_handle, deltaTime);
        return this;
    }

    /// <summary>
    /// Set the event loop user callback called automatically on each event loop iteration.
    /// </summary>
    /// <param name="callback">callback to be called on each event loop iteration</param>
    /// <returns>this interactor for method chaining</returns>
    public Interactor SetEventLoopUserCallback(Action? callback)
    {
        var handle = callback is null ? default : GCHandle.Alloc(callback);
        NativeMethods.f3d_interactor_set_event_loop_user_callback(_handle, &OnEventLoop,
            callback is null ? null : (void*)GCHandle.ToIntPtr(handle));
        Replace(ref _eventLoopCallback, handle);
        return this;
    }

    /// <summary>
    /// Set notification callback. If null, only standard notifications will be displayed.
    /// </summary>
    public Interactor SetNotificationCallback(Func<string, string, string, double, bool>? callback)
    {
        if (callback is null)
        {
            NativeMethods.f3d_interactor_set_notification_callback(_handle, null, null);
            Replace(ref _notificationCallback, default);
            return this;
        }

        var handle = GCHandle.Alloc(callback);
        NativeMethods.f3d_interactor_set_notification_callback(_handle, &OnNotification, (void*)GCHandle.ToIntPtr(handle));
        Replace(ref _notificationCallback, handle);
        return this;
    }

    /// <summary>
    /// Play a VTK interaction file.
    /// </summary>
    /// <param name="file">path to interaction file</param>
    /// <param name="deltaTime">time delta in seconds</param>
    /// <returns>true on success, false otherwise</returns>
    public bool PlayInteraction(string file, double deltaTime = 1.0 / 30.0)
    {
        ArgumentNullException.ThrowIfNull(file);
        return NativeMethods.f3d_interactor_play_interaction(_handle, file, deltaTime) != 0;
    }

    /// <summary>
    /// Start interaction and record it in a VTK interaction file.
    /// </summary>
    /// <param name="file">path to save interaction file</param>
    /// <returns>true on success, false otherwise</returns>
    public bool RecordInteraction(string file)
    {
        ArgumentNullException.ThrowIfNull(file);
        return NativeMethods.f3d_interactor_record_interaction(_handle, file) != 0;
    }

    /// <summary>
    /// Start the interactor event loop.
    /// </summary>
    /// <param name="deltaTime">time delta in seconds (must be positive)</param>
    /// <returns>this interactor for method chaining</returns>
    public Interactor Start(double deltaTime = 1.0 / 30.0)
    {
        NativeMethods.f3d_interactor_start(_handle, deltaTime);
        return this;
    }

    /// <summary>
    /// Stop the interactor.
    /// </summary>
    /// <returns>this interactor for method chaining</returns>
    public Interactor Stop()
    {
        NativeMethods.f3d_interactor_stop(_handle);
        return this;
    }

    /// <summary>
    /// Request a render to be done on the next event loop.
    /// </summary>
    /// <returns>this interactor for method chaining</returns>
    public Interactor RequestRender()
    {
        NativeMethods.f3d_interactor_request_render(_handle);
        return this;
    }

    /// <summary>
    /// Request the interactor to stop on the next event loop.
    /// </summary>
    /// <returns>this interactor for method chaining</returns>
    public Interactor RequestStop()
    {
        NativeMethods.f3d_interactor_request_stop(_handle);
        return this;
    }

    /// <summary>
    /// Trigger a single text line notification with text desc for duration seconds.
    /// </summary>
    /// <param name="desc">Text description</param>
    /// <param name="value">Text value</param>
    /// <param name="duration">Duration of notification in seconds</param>
    public Interactor TriggerNotification(string desc, string value = "", double duration = 3.0)
    {
        ArgumentNullException.ThrowIfNull(desc);
        NativeMethods.f3d_interactor_trigger_notification(_handle, desc, value ?? "", duration);
        return this;
    }

    // Frees the callbacks. Called by Engine.Dispose once F3D can no longer call them
    internal void ReleaseCallbacks()
    {
        foreach (var handle in _commands.Values) handle.Free();
        _commands.Clear();
        Replace(ref _eventLoopCallback, default);
        Replace(ref _notificationCallback, default);
    }

    static void Replace(ref GCHandle field, GCHandle value)
    {
        if (field.IsAllocated) field.Free();
        field = value;
    }

    static List<string> ToStringList(sbyte** array, int count)
    {
        var list = new List<string>(count);
        if (array == null) return list;
        for (int i = 0; i < count; i++)
            list.Add(Marshal.PtrToStringUTF8((nint)array[i]) ?? "");
        NativeMethods.f3d_interactor_destroy_string_array(array, count);
        return list;
    }

    static List<InteractionBind> ToBindList(f3d_interaction_bind_t* array, int count)
    {
        var list = new List<InteractionBind>(count);
        if (array == null) return list;
        for (int i = 0; i < count; i++)
            list.Add(InteractionBind.FromNative(&array[i]));
        NativeMethods.f3d_interactor_destroy_bind_array(array);
        return list;
    }

    // Exceptions are logged as F3D errors
    static void LogError(string where, Exception e)
    {
        NativeMethods.f3d_log_error($"Exception in {where} callback: {e}");
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    static void OnCommand(sbyte** args, int count, void* userData)
    {
        try
        {
            var callback = (Action<IReadOnlyList<string>>)GCHandle.FromIntPtr((nint)userData).Target!;
            var list = new List<string>(count);
            for (int i = 0; i < count; i++)
                list.Add(Marshal.PtrToStringUTF8((nint)args[i]) ?? "");
            callback(list);
        }
        catch (Exception e) { LogError("command", e); }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    static void OnEventLoop(void* userData)
    {
        if (userData == null) return;
        try { ((Action)GCHandle.FromIntPtr((nint)userData).Target!)(); }
        catch (Exception e) { LogError("event loop", e); }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    static int OnNotification(sbyte* desc, sbyte* value, sbyte* bind, double duration, void* userData)
    {
        try
        {
            var callback = (Func<string, string, string, double, bool>)GCHandle.FromIntPtr((nint)userData).Target!;
            return callback(
                Marshal.PtrToStringUTF8((nint)desc) ?? "",
                Marshal.PtrToStringUTF8((nint)value) ?? "",
                Marshal.PtrToStringUTF8((nint)bind) ?? "",
                duration) ? 1 : 0;
        }
        catch (Exception e)
        {
            LogError("notification", e);
            return 1;
        }
    }
}
