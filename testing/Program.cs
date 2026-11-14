// Runs one test: Tests <TestName> <data dir> <output dir>
// A test fails by throwing, like the Java tests.

var tests = new Dictionary<string, Action<string[]>>
{
    ["TestEngine"] = TestEngine.Run,
    ["TestInteractor"] = TestInteractor.Run,
    ["TestOptions"] = TestOptions.Run,
    ["TestScene"] = TestScene.Run,
    ["TestImage"] = TestImage.Run,
    ["TestVideoPacket"] = TestVideoPacket.Run,
    ["TestUtils"] = TestUtils.Run,
    ["TestTypes"] = TestTypes.Run,
    ["TestWindow"] = TestWindow.Run,
};

if (args.Length < 1 || !tests.TryGetValue(args[0], out var test))
{
    Console.Error.WriteLine($"usage: Tests <{string.Join("|", tests.Keys)}> [data dir] [output dir]");
    return 1;
}

test(args[1..]);
return 0;
