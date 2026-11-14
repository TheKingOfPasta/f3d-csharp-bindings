using F3D;

static class TestOptions
{
    public static void Run(string[] args)
    {
        using (var standaloneOptions = new Options()) { }

        using var engine = Engine.Create(true);
        var options = engine.Options;

        options.SetAsBool("scene.camera.orthographic", true);
        options.SetAsInt("model.scivis.component", 2);
        options.SetAsDouble("render.line_width", 3.5);
        options.SetAsString("render.effect.final_shader", "test.glsl");
        options.SetAsDoubleVector("render.background.color", [1.0, 2.0, 3.0]);
        options.SetAsIntVector("scene.animation.indices", [1, 2]);

        _ = options.GetAsBool("scene.camera.orthographic");
        _ = options.GetAsInt("model.scivis.component");
        _ = options.GetAsDouble("render.line_width");
        _ = options.GetAsString("render.effect.final_shader");
        _ = options.GetAsDoubleVector("render.background.color");
        _ = options.GetAsIntVector("scene.animation.indices");

        options.Toggle("scene.camera.orthographic");

        using (var engine2 = Engine.Create(true))
        {
            var options2 = engine2.Options;
            _ = options.IsSame(options2, "scene.camera.orthographic");
            options2.Copy(options, "scene.camera.orthographic");
        }

        _ = options.HasValue("scene.camera.orthographic");
        _ = Options.GetAllNames();
        _ = options.GetNames();
        _ = options.IsOptional("render.show_edges");
        _ = options.GetOptionType("render.show_edges");

        if (options.GetOptionType("dummy") != Options.OptionType.Invalid)
            throw new Exception("GetOptionType should return Invalid for a non-existent option");

        options.Reset("scene.camera.orthographic");
        options.RemoveValue("render.show_edges");

        _ = options.HasDomain("scene.animation.speed_factor");
        _ = options.GetDomainStyle("scene.animation.speed_factor");

        // Test GetRangeDomain

        var range = options.GetRangeDomainDouble("render.line_width")
            ?? throw new Exception("GetRangeDomainDouble should succeed for render.line_width");
        if (range[0] != 0.0 || range[1] != 10.0 || range[2] != 0.1)
            throw new Exception("GetRangeDomainDouble returned unexpected values for render.line_width");

        var intRange = options.GetRangeDomainInt("render.raytracing.samples")
            ?? throw new Exception("GetRangeDomainInt should succeed for render.raytracing.samples");
        if (intRange[0] != 1 || intRange[1] != 50 || intRange[2] != 1)
            throw new Exception("GetRangeDomainInt returned unexpected values for render.raytracing.samples");

        // Test GetRangeDomain error code path

        if (options.GetRangeDomainDouble("render.raytracing.samples") != null)
            throw new Exception("GetRangeDomainDouble should fail for an int range domain");
        if (options.GetRangeDomainDouble("dummy") != null)
            throw new Exception("GetRangeDomainDouble should fail for a non-existent option");

        if (options.GetRangeDomainInt("render.line_width") != null)
            throw new Exception("GetRangeDomainInt should fail for a double range domain");
        if (options.GetRangeDomainInt("dummy") != null)
            throw new Exception("GetRangeDomainInt should fail for a non-existent option");

        // Test GetEnumDomain

        if (options.GetEnumDomainString("interactor.style").Count == 0)
            throw new Exception("GetEnumDomainString failed to recover an existing domain");

        // Test GetEnumDomain error code path

        if (options.GetEnumDomainString("scene.animation.speed_factor").Count != 0)
            throw new Exception("GetEnumDomainString should fail to recover a domain for an incompatible option");
        if (options.GetEnumDomainString("inexistent").Count != 0)
            throw new Exception("GetEnumDomainString should fail to recover a domain for an inexistent option");

        // Test GetIndexDomain
        // scene.camera.index has an index domain without a max value set, so null is expected
        if (options.GetIndexDomain("scene.camera.index") != null)
            throw new Exception("GetIndexDomain should return null for scene.camera.index");
        if (options.GetIndexDomain("render.raytracing.samples") != null)
            throw new Exception("GetIndexDomain should fail for an incompatible option");
        if (options.GetIndexDomain("dummy") != null)
            throw new Exception("GetIndexDomain should fail for a non-existent option");

        options.Increase("scene.animation.speed_factor");
        options.Decrease("scene.animation.speed_factor");
        options.Cycle("interactor.style");

        _ = options.GetAsStringRepresentation("render.line_width");
        options.SetAsStringRepresentation("render.line_width", "5.0");

        _ = options.GetClosestOption("render.line_wdth");

        _ = Options.ParseBool("true");
        _ = Options.ParseInt("42");
        _ = Options.ParseDouble("3.14");
        _ = Options.ParseString("test");
        _ = Options.ParseDoubleVector("1.0,2.0,3.0");
        _ = Options.ParseIntVector("1,2,3");

        _ = Options.FormatBool(true);
        _ = Options.FormatInt(42);
        _ = Options.FormatDouble(3.14);
        _ = Options.FormatString("test");
        _ = Options.FormatDoubleVector([1.0, 2.0, 3.0]);
        _ = Options.FormatIntVector([1, 2, 3]);
    }
}
