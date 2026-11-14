using System.Text.RegularExpressions;
using F3D;

static class TestUtils
{
    public static void Run(string[] args)
    {
        TestKnownFolderValues();
        TestTextDistance();
        TestTokenize();
        TestCollapsePath();
        TestGlobToRegex();
        TestEnvironmentVariable();
        TestKnownFolder();
    }

    private static void TestKnownFolderValues()
    {
        Check((int)Utils.KnownFolder.RoamingAppData == 0, "Unexpected roaming app data value.");
        Check((int)Utils.KnownFolder.LocalAppData == 1, "Unexpected local app data value.");
        Check((int)Utils.KnownFolder.Pictures == 2, "Unexpected pictures value.");
        CheckThrows<ArgumentOutOfRangeException>(() => Utils.GetKnownFolder((Utils.KnownFolder)99));
    }

    private static void TestTextDistance()
    {
        Check(Utils.TextDistance("kitten", "sitting") == 3, "Incorrect Levenshtein distance.");
        Check(Utils.TextDistance("same", "same") == 0, "Equal strings should have distance zero.");
        Check(Utils.TextDistance(string.Empty, "abc") == 3, "Incorrect distance for an empty string.");
        Check(Utils.TextDistance("café", "café") == 0, "UTF-8 strings should be handled correctly.");
        CheckThrows<ArgumentNullException>(() => Utils.TextDistance(null!, "text"));
        CheckThrows<ArgumentNullException>(() => Utils.TextDistance("text", null!));
    }

    private static void TestTokenize()
    {
        List<string> tokens = Utils.Tokenize("one 'two words' \"three\"");
        Check(tokens.SequenceEqual(["one", "two words", "three"]), "Incorrect shell-style tokenization.");

        List<string> commandTokens = Utils.Tokenize("one two \"three four\" # comment", keepComments: true);
        Check(commandTokens.SequenceEqual(["one", "two", "three four"]),
            "Tokenizing a command with a trailing comment returned unexpected tokens.");

        List<string> withoutComments = Utils.Tokenize("# this is a comment", keepComments: false);
        Check(withoutComments.SequenceEqual(["#", "this", "is", "a", "comment"]),
            "The '#' character should be treated as normal input when keepComments is false.");

        List<string> withComments = Utils.Tokenize("# this is a comment");
        Check(withComments.Count == 0, "The default overload should ignore comment-only input.");

        CheckThrows<F3DException>(() => Utils.Tokenize("'unterminated"));
        CheckThrows<ArgumentNullException>(() => Utils.Tokenize(null!));
    }

    private static void TestCollapsePath()
    {
        string baseDirectory = Path.Combine(Path.GetTempPath(), "f3d-utils-test");
        string expected = Path.GetFullPath(Path.Combine(baseDirectory, "file.txt"));
        string actual = Utils.CollapsePath("folder/../file.txt", baseDirectory);
        Check(Path.GetFullPath(actual) == expected, "Relative path was not collapsed against the base directory.");

        string currentDirectoryResult = Utils.CollapsePath(".");
        Check(!string.IsNullOrEmpty(currentDirectoryResult) && Path.IsPathFullyQualified(currentDirectoryResult),
            "Collapsing the current directory should return a non-empty absolute path.");
        CheckThrows<ArgumentNullException>(() => Utils.CollapsePath(null!, baseDirectory));
        CheckThrows<ArgumentNullException>(() => Utils.CollapsePath("file.txt", null!));
    }

    private static void TestGlobToRegex()
    {
        string? regex = Utils.GlobToRegex("*.txt");
        Check(!string.IsNullOrEmpty(regex), "A valid glob should produce a non-empty regular expression.");
        Check(Regex.IsMatch("document.txt", regex!), "Generated regex should match a glob-matching filename.");
        Check(!Regex.IsMatch("document.jpg", regex!), "Generated regex should reject a non-matching filename.");

        string? windowsRegex = Utils.GlobToRegex("folder/*.png", '\\');
        Check(windowsRegex is not null, "A glob with a Windows separator should be converted.");
        CheckThrows<ArgumentOutOfRangeException>(() => Utils.GlobToRegex("*.png", '\u0100'));
        CheckThrows<ArgumentNullException>(() => Utils.GlobToRegex(null!));
    }

    private static void TestEnvironmentVariable()
    {
        Check(Utils.GetEnv("PATH") is not null, "The PATH environment variable should be available.");
        Check(Utils.GetEnv("F3D_CSHARP_UTILS_VARIABLE_NOT_SET") is null,
            "An unset environment variable should return null.");
        CheckThrows<ArgumentNullException>(() => Utils.GetEnv(null!));
    }

    private static void TestKnownFolder()
    {
        string? path = Utils.GetKnownFolder(Utils.KnownFolder.RoamingAppData);
        if (OperatingSystem.IsWindows())
            Check(!string.IsNullOrEmpty(path), "Roaming AppData should be a non-empty path on Windows.");
        else
            Check(path is null, "Known folders should be unavailable on non-Windows platforms.");
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
            throw new Exception(message);
    }

    private static void CheckThrows<TException>(Action action)
        where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            return;
        }

        throw new Exception($"Expected {typeof(TException).Name} to be thrown.");
    }
}
