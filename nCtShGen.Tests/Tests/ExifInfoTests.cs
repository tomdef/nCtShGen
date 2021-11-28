using NUnit.Framework;
using nCtShGen.Api.Providers;
using nCtShGen.Api.Model;
using System.Collections.Generic;

namespace nCtShGen.Tests.Tests;

public class ExifInfoTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test, Category("Providers")]
    public void ExifInfo_ToString()
    {
        List<KeyValuePair<string, ExifInfo>> testItems = new()
        {
            new KeyValuePair<string, ExifInfo>("ƒ5.6 1s 16mm ISO400",
                new()
                {
                    Aperture = 5.6f,
                    ExposureTime = 1f,
                    FocalLength = 16,
                    Height = 1080,
                    Iso = 400,
                    Name = "test.jpg",
                    Path = "c:\\test\\test.jpg",
                    ShutterSpeed = 1f,
                    Width = 1920
                }
            ),
            new KeyValuePair<string, ExifInfo>("ƒ4 1/80s 35mm ISO100",
                new()
                {
                    Aperture = 4f,
                    ExposureTime = 80f,
                    FocalLength = 35,
                    Height = 1080,
                    Iso = 100,
                    Name = "test.jpg",
                    Path = "c:\\test\\test.jpg",
                    ShutterSpeed = 0.0125f,
                    Width = 1920
                }
            )
        };

        foreach (var item in testItems)
        {
            string s = item.Value.ToString();
            Assert.AreEqual(item.Key, s);
        }
    }
}