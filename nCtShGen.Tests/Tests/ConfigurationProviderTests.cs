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
        var cp = ConfigurationProvider.Read(@"TestData\appsettings.test.json");

        Assert.AreEqual(@"d:\Temp\T2", cp.RootPhotoFolder, "Invalid RootPhotoFolder value");
        Assert.AreEqual(5, cp.FolderDeepLevel, "Invalid FolderDeepLevel value");
    }
}