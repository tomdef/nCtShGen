using NUnit.Framework;
using nCtShGen.Api.Providers;
using nCtShGen.Api.Model;

namespace nCtShGen.Tests.Tests;

public class ExifInfoProviderTests
{
    [Test, Category("Providers")]
    public void ExifInfoProvider_ReadExif()
    {
        // horizontal image
        var ti = ExifInfoProvider.Read(@".\TestData\test001.jpg");

        Assert.NotNull(ti);
        Assert.AreEqual("test001.jpg", ti.Name);
        Assert.AreEqual(18.0f, ti.FocalLength);
        Assert.AreEqual(5.6f, ti.Aperture);
        Assert.AreEqual(800, ti.Iso);
        Assert.AreEqual(80.0f, ti.ShutterSpeed);
        Assert.AreEqual(1080, ti.Width);
        Assert.AreEqual(1620, ti.Height);
        Assert.AreEqual(false, ti.IsHorizontal());

        // vertical image
        ti = ExifInfoProvider.Read(@".\TestData\test002.jpg");

        Assert.NotNull(ti);
        Assert.AreEqual("test002.jpg", ti.Name);
        Assert.AreEqual(16.0f, ti.FocalLength);
        Assert.AreEqual(5.6f, ti.Aperture);
        Assert.AreEqual(1600, ti.Iso);
        Assert.AreEqual(30.0f, ti.ShutterSpeed);
        Assert.AreEqual(1080, ti.Width);
        Assert.AreEqual(720, ti.Height);
        Assert.AreEqual(true, ti.IsHorizontal());
    }
}