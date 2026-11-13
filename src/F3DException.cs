namespace F3D;

/// <summary>Base class for all F3D errors, like F3DException in the Java bindings.</summary>
public class F3DException : Exception
{
    public F3DException(string message) : base(message) { }
}
