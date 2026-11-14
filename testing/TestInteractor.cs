using F3D;
using F3D.Native;
using static F3D.Interactor;

static unsafe class TestInteractor
{
    public static void Run(string[] args)
    {
        using var engine = Engine.Create(true);

        engine.Window?.Render();

        var interactor = engine.Interactor ?? throw new Exception("Failed to get interactor");

        interactor.EnableCameraMovement();
        interactor.DisableCameraMovement();

        interactor.TriggerModUpdate(InputModifier.Ctrl);
        interactor.TriggerMouseButton(InputAction.Press, MouseButton.Left);
        interactor.TriggerMouseButton(InputAction.Release, MouseButton.Right);
        interactor.TriggerMousePosition(100.0, 200.0);
        interactor.TriggerMouseWheel(WheelDirection.Forward);
        interactor.TriggerMouseWheel(WheelDirection.Backward);
        interactor.TriggerKeyboardKey(InputAction.Press, "a");
        interactor.TriggerKeyboardKey(InputAction.Release, "b");
        interactor.TriggerTextCharacter(65);

        interactor.TriggerEventLoop(0.016);

        interactor.TriggerNotification("foo", "bar", 3.0);
        interactor.SetNotificationCallback(null);

        interactor.PlayInteraction("/nonexistent.log", 1.0 / 30.0);
        interactor.RecordInteraction(Path.Combine(args[1], "test_interaction.log"));

        interactor.RequestRender();
        interactor.RequestStop();

        interactor.InitCommands();
        interactor.RemoveCommand("test_action");
        interactor.TriggerCommand("print Test", false);
        interactor.TriggerCommand("print Test # comment", true);

        interactor.InitBindings();

        // The C test passes a NULL callback, which C# refuses (use an empty one)
        interactor.AddCommand("test_action", _ => { });
        _ = interactor.GetCommandActions();

        // C# only: adding an existing command throws
        try
        {
            interactor.AddCommand("test_action", _ => { });
            throw new Exception("Adding an existing command should throw");
        }
        catch (AlreadyExistsException) { }

        var bind = new InteractionBind(ModifierKeys.None, "t");
        _ = bind.Format();

        var ctrlBind = new InteractionBind(ModifierKeys.Ctrl, "A");
        _ = ctrlBind.Format();

        var repeatBind = new InteractionBind(ModifierKeys.None, "r");
        _ = repeatBind.Format();

        var parsedBind = InteractionBind.Parse("Shift+B");

        _ = ctrlBind.Equals(parsedBind);
        _ = ctrlBind.CompareTo(parsedBind) < 0;

        // C# only: InteractionBind reimplements format, parse, equals, less_than (so it compares to C API)
        CheckSameAsNative(bind, ctrlBind, repeatBind, parsedBind);

        interactor.AddBinding(new BindingParam(bind, ["test_action"])
        {
            Group = "test_group",
            Type = BindingType.Cyclic,
            Notify = true,
            Repeat = false,
        });

        interactor.AddBinding(new BindingParam(repeatBind, ["test_action_repeat"])
        {
            Group = "test_group",
            Type = BindingType.Numerical,
            Notify = true,
            Repeat = true,
        });

        // C# only: adding an existing binding throws
        try
        {
            interactor.AddBinding(new BindingParam(bind, ["test_action"]) { Group = "test_group" });
            throw new Exception("Adding an existing binding should throw");
        }
        catch (AlreadyExistsException) { }

        _ = interactor.GetBindGroups();
        _ = interactor.GetBindsForGroup("test_group");
        _ = interactor.GetBinds();

        _ = interactor.GetBindingDocumentation(bind);
        _ = interactor.GetBindingType(bind);
        interactor.RemoveBinding(bind);

        _ = interactor.GetBindingDocumentation(repeatBind);
        _ = interactor.GetBindingType(repeatBind);
        interactor.RemoveBinding(repeatBind);

        // C# only: removing a missing binding throws
        try
        {
            interactor.RemoveBinding(repeatBind);
            throw new Exception("Removing a missing binding should throw");
        }
        catch (DoesNotExistException) { }

        interactor.SetEventLoopUserCallback(() => interactor.RequestStop());
        interactor.Start(0.01);
    }

    static void CheckSameAsNative(params InteractionBind[] binds)
    {
        var output = stackalloc sbyte[512];
        foreach (var bind in binds)
        {
            var native = bind.ToNativeForTest();
            NativeMethods.f3d_interaction_bind_format(&native, output, 512);
            var expected = new string(output);
            if (bind.Format() != expected)
                throw new Exception($"Format gives \"{bind.Format()}\", the C API gives \"{expected}\"");

            f3d_interaction_bind_t parsed = default;
            NativeMethods.f3d_interaction_bind_parse(expected, &parsed);
            if (InteractionBind.Parse(expected) != FromNativeForTest(&parsed))
                throw new Exception($"Parse(\"{expected}\") differs from the C API");

            foreach (var other in binds)
            {
                var nativeOther = other.ToNativeForTest();
                if (bind.Equals(other) != (NativeMethods.f3d_interaction_bind_equals(&native, &nativeOther) != 0))
                    throw new Exception($"Equals({bind}, {other}) differs from the C API");
                if ((bind.CompareTo(other) < 0) != (NativeMethods.f3d_interaction_bind_less_than(&native, &nativeOther) != 0))
                    throw new Exception($"CompareTo({bind}, {other}) differs from the C API");
            }
        }
    }

    // InteractionBind.ToNative and FromNative are internal, so the conversion is redone here
    static f3d_interaction_bind_t ToNativeForTest(this InteractionBind bind)
    {
        f3d_interaction_bind_t native = default;
        native.mod = (f3d_interaction_bind_modifier_keys_t)bind.Mod;
        System.Text.Encoding.UTF8.GetBytes(bind.Inter).CopyTo(new Span<byte>(&native.inter, 256));
        return native;
    }

    static InteractionBind FromNativeForTest(f3d_interaction_bind_t* native) =>
        new((ModifierKeys)native->mod, new string((sbyte*)&native->inter));
}
