using F3D;

if (args.Length != 1)
{
    Console.WriteLine("usage: dotnet run --project examples/Viewer -- <path/to/model>");
    return;
}

Engine.AutoloadPlugins();
using var engine = Engine.Create();
engine.Options.SetAsBool("render.grid.enable", true);
engine.Options.SetAsBool("ui.axis", true);
engine.Scene.Add(args[0]);
engine.Interactor.Start();
