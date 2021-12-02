using NUnit.Framework;
using System;
using nCtShGen.Api.Providers;
using nCtShGen.Api.Model;
using System.Drawing;
using System.IO;

namespace nCtShGen.Tests.Tests;

public class ContactSheetProviderTests
{
    private ContactSheetProvider contactSheetProvider = default!;
    private ConfigurationItem configurationItem = default!;

    [SetUp]
    public void Setup()
    {
        configurationItem = new ConfigurationItem()
        {
            FolderDeepLevel = 0,
            RootPhotoFolder = "",
            Thumbnail = new ConfigurationThumbnailItem()
            {
                MaxWidth = 300,
                MaxHeight = 300
            }
        };

        contactSheetProvider = new ContactSheetProvider(configurationItem);
    }

    [Category("Providers")]
    [TestCase(".\\TestData\\test001.jpg")]
    [TestCase(".\\TestData\\test002.jpg")]
    public void ContactSheetProvider_GenerateContactSheetItem(string filePath)
    {
        Image image = contactSheetProvider.GenerateContactSheetItem(filePath);
        Assert.NotNull(image);

        string name = Path.GetFileName(filePath);
        image.Save(string.Format("d:\\temp\\_test_{0}.jpg", name));
    }

    [Category("Providers")]
    [Test]
    public void ContactSheetProvider_GenerateContactSheet()
    {
        string title = "20202001";
        Image image = contactSheetProvider.GenerateContactSheet(title, @"d:\Temp\TEST_22\", "*.jpg");
        Assert.NotNull(image);
        image.Save("d:\\temp\\_contactSheet.jpg");
    }
}