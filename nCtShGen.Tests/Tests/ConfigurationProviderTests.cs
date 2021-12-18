using NUnit.Framework;
using System;
using nCtShGen.Api.Providers;

namespace nCtShGen.Tests.Tests;

public class ConfigurationProviderTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test, Category("Providers")]
    public void ConfigurationProvider_ReadTest()
    {
        var cp = ConfigurationProvider.Read(@".\TestData\appsettings.test.json");

        Assert.AreEqual(@"d:\Temp\Test", cp.RootPhotoFolder, "Invalid RootPhotoFolder value");
        Assert.AreEqual(0, cp.ContactSheetRootFolderOnLevel, "Invalid FolderDeepLevel value");
        Assert.AreEqual(3, cp.ContactSheetSubfolderDeepLevel, "Invalid FolderDeepLevel value");
    }
}