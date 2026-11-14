using F3D;

static class TestContext
{
    public static void Run(string[] args)
    {
        // none of the functions should throw exception
        // the C# factories throw F3DException, so only that exception is accepted here
        TryCreate(Context.Glx);
        TryCreate(Context.Wgl);
        TryCreate(Context.Cocoa);
        TryCreate(Context.Egl);
        TryCreate(Context.Osmesa);

        // C# only (GetSymbol refuses null arguments)
        try
        {
            Context.GetSymbol(null!, "func");
            throw new Exception("GetSymbol with a null lib should throw");
        }
        catch (ArgumentNullException) { }

        try
        {
            Context.GetSymbol("lib", null!);
            throw new Exception("GetSymbol with a null func should throw");
        }
        catch (ArgumentNullException) { }
    }

    static void TryCreate(Func<Context> factory)
    {
        try
        {
            using var context = factory();
        }
        catch (F3DException) { }
    }
}
