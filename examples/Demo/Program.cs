using F3D;
using static F3D.Interactor;

if (args.Length != 1)
{
    Console.WriteLine("usage: dotnet run --project examples/Demo -- <path/to/model>");
    return;
}

// Create the engine with it owns the options, the window, the scene and the interactor
Engine.AutoloadPlugins();
using var engine = Engine.Create();

// Options are set by name like in the f3d application
engine.Options.SetAsBool("render.grid.enable", true);
engine.Options.SetAsBool("ui.axis", true);
engine.Options.SetAsBool("ui.notifications.enable", true);
// curl -L -o ./assets/soldier_animations.mdl https://media.githubusercontent.com/media/f3d-app/f3d/master/testing/data/soldier_animations.mdl
engine.Options.SetAsStringRepresentation("scene.up_direction", "+Z"); // used by Quake .mdl models that are Z-up

engine.Window.SetWindowName("F3D C# Demo").SetSize(1280, 720);

// Load the model and play its animation
engine.Scene.Add(args[0]);
Console.WriteLine($"Animations: {string.Join(", ", engine.Scene.Animation.Names)}");
engine.Scene.Animation.Start();

// A new command written in C#, bound to Ctrl+S (Cmd+S on macOS)
var interactor = engine.Interactor;
interactor.AddCommand("save_screenshot", _ =>
{
    using var image = engine.Window.RenderToImage();
    image.Save("screenshot.png");
    interactor.TriggerNotification("Screenshot saved", "screenshot.png");
});
interactor.AddBinding(new BindingParam(new InteractionBind(ModifierKeys.Ctrl, "S"), ["save_screenshot"]));

// Open the window and wait until it is closed
interactor.Start();
