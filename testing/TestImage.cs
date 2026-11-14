using System.Diagnostics;
using F3D;

using SaveFormat = F3D.Native.f3d_image_save_format_t;
using ChannelType = F3D.Native.f3d_image_channel_type_t;

static class TestImage
{
    public static void Run(string[] args)
    {
        TestCreationAndProperties();
        TestBufferAndPixels();
        TestMetadata();
        TestComparisonAndEquality();
        TestSaveAndReloadBuffer();
        TestEmptyStream();
        TestExceptions();
        TestSupportedFormats();
    }

    private static void TestCreationAndProperties()
    {
        using var img = new Image(80, 60, 3, ChannelType.BYTE);

        Debug.Assert(img.Width == 80);
        Debug.Assert(img.Height == 60);
        Debug.Assert(img.ChannelCount == 3);
        Debug.Assert(img.ChannelType == ChannelType.BYTE);
        Debug.Assert(img.ChannelTypeSize == 1);
    }

    private static void TestBufferAndPixels()
    {
        using var img = new Image(10, 10, 3);

        byte[] content = img.GetContent();
        Debug.Assert(content.Length == 10 * 10 * 3);

        for (int i = 0; i < content.Length; i += 3)
        {
            content[i] = 255;
            content[i + 1] = 0;
            content[i + 2] = 0;
        }
        img.SetContent(content);

        double[] pixel = img.GetNormalizedPixel(0, 0);
        Debug.Assert(pixel.Length == 3);
        Debug.Assert(Math.Abs(pixel[0] - 1.0) < 1e-4);
        Debug.Assert(Math.Abs(pixel[1] - 0.0) < 1e-4);
    }

    private static void TestMetadata()
    {
        using var img = new Image(10, 10, 3);

        img.SetMetadata("Author", "HackathonTeam");
        img.SetMetadata("Engine", "F3D");

        Debug.Assert(img.GetMetadata("Author") == "HackathonTeam");
        Debug.Assert(img.GetMetadata("Engine") == "F3D");

        List<string> keys = img.AllMetadata();
        Debug.Assert(keys.Contains("Author"));
        Debug.Assert(keys.Contains("Engine"));
    }

    private static void TestComparisonAndEquality()
    {
        using var img1 = new Image(20, 20, 3);
        using var img2 = new Image(20, 20, 3);

        byte[] initialData = new byte[20 * 20 * 3];
        img1.SetContent(initialData);
        img2.SetContent(initialData);

        Debug.Assert(img1 == img2);
        Debug.Assert(img1.Equals(img2));
        Debug.Assert(img1.Compare(img2) == 0.0);

        byte[] buffer = img2.GetContent();
        buffer[0] = 255;
        img2.SetContent(buffer);

        Debug.Assert(img1 != img2);
        Debug.Assert(img1.Compare(img2) > 0.0);
    }

    private static void TestSaveAndReloadBuffer()
    {
        using var original = new Image(800, 600, 3, ChannelType.BYTE);
        byte[] pngBytes = original.SaveBuffer(SaveFormat.PNG);

        Debug.Assert(pngBytes.Length > 0);

        using var reloaded = new Image(pngBytes);
        Debug.Assert(reloaded.Width == 800);
        Debug.Assert(reloaded.Height == 600);
        Debug.Assert(original.Compare(reloaded) == 0.0);
    }

    private static void TestEmptyStream()
    {
        bool readExceptionThrown = false;
        try
        {
            using var invalid = new Image(ReadOnlySpan<byte>.Empty);
        }
        catch (Image.ReadException)
        {
            readExceptionThrown = true;
        }

        Debug.Assert(readExceptionThrown);
    }

    private static void TestExceptions()
    {
        bool readExceptionThrown = false;
        try
        {
            using var invalid = new Image("/invalid/path/to/nonexistent/image.png");
        }
        catch (Image.ReadException)
        {
            readExceptionThrown = true;
        }
        Debug.Assert(readExceptionThrown);

        bool metadataExceptionThrown = false;
        using var img = new Image(5, 5, 3);
        try
        {
            img.GetMetadata("NonExistentKey");
        }
        catch (Image.MetadataException)
        {
            metadataExceptionThrown = true;
        }
        Debug.Assert(metadataExceptionThrown);
    }

    private static void TestSupportedFormats()
    {
        List<string> formats = Image.GetSupportedFormats();
        Debug.Assert(formats != null && formats.Count > 0);
        Debug.Assert(formats.Contains("png") || formats.Contains(".png"));
    }
}