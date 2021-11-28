using NUnit.Framework;
using nCtShGen.Api.Providers;
using nCtShGen.Api.Model;
using ExifPhotoReader;
using System.Drawing;
using System.IO;

namespace nCtShGen.Tests.Tests;

public class ThumbnailProviderTests
{
    private ConfigurationThumbnailItem cti = default!;
    private ThumbnailProvider tp = default!;

    [SetUp]
    public void Setup()
    {
        cti = new()
        {
            MaxWidth = 300,
            MaxHeight = 300
        };

        tp = new(cti);
    }

    [Category("Providers")]
    [TestCase(1000, 500, true, 300, 150)]
    [TestCase(500, 1000, false, 150, 300)]
    public void ThumbnailProviderTests_GetThumbnailSize(int x, int y, bool isHorizontal, int expectedWidth, int expectedHeight)
    {
        var (width, height) = tp.GetThumbnailSize(x, y, isHorizontal);
        Assert.AreEqual(expectedWidth, width);
        Assert.AreEqual(expectedHeight, height);
    }

    [TestCase(".\\TestData\\test001.jpg", 200, 300)]
    [TestCase(".\\TestData\\test002.jpg", 300, 200)]
    public void ThumbnailProviderTests_GetThumbnail(string filePath, int expectedWidth, int expectedHeight)
    {
        var (_, img) = tp.GetThumbnail(filePath);

        Assert.AreEqual(expectedWidth, img.Width);
        Assert.AreEqual(expectedHeight, img.Height);

        img.Save(string.Format("d:\\temp\\_{0}.jpg", Path.GetFileName(filePath)));
    }
}