using NUnit.Framework;
using System;
using nCtShGen.Api.Providers;
using nCtShGen.Api.Model;
using System.Drawing;

namespace nCtShGen.Tests.Tests;

public class ContactSheetProviderTests
{
    private ConfigurationItem configurationItem = default!;

    [SetUp]
    public void Setup()
    {
        configurationItem = new ConfigurationItem()
        {
            Thumbnail = new ConfigurationThumbnailItem()
            {
                MaxWidth = 300,
                MaxHeight = 300
            }
        };
    }

    [Category("Providers")]
    [TestCase(@".\TestData\test001.jpg")]
    [TestCase(@".\TestData\test002.jpg")]
    public void ContactSheetProvider_GenerateContactSheetItem(string filePath)
    {
        ContactSheetProvider contactSheetProvider = new(configurationItem, ColorSchemaName.Light);
        Image? image = contactSheetProvider.GenerateContactSheetItem(filePath);
        Assert.NotNull(image);
    }

    [Category("Providers")]
    [Test]
    public void ContactSheetProvider_GenerateContactSheet()
    {
        ContactSheetProvider contactSheetProvider = new(configurationItem, ColorSchemaName.Dark);

        contactSheetProvider.OnAddContactSheetItem += (a, e) => Console.WriteLine("OnAddContactSheetItem : {0}", e.FileName);
        contactSheetProvider.OnStartGenerateContactSheet += (a, e) => Console.WriteLine("OnStartGenerateContactSheet : {0} [{1}]", e.Folder, e.AllItems);
        contactSheetProvider.OnFinishGenerateContactSheet += (a, e) => Console.WriteLine("OnFinishGenerateContactSheet : {0} [{1}]", e.Folder, e.AllItems);
        contactSheetProvider.OnWarningContactSheetItem += (a, e) => Console.WriteLine("OnWarningContactSheetItem : {0} [{1}]", e.FileName, e.Details);

        Image? image = contactSheetProvider.GenerateContactSheet("20202001", @"TestData");
        Assert.NotNull(image);
    }
}