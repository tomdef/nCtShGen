using ExifPhotoReader;
using nCtShGen.Api.Model;

namespace nCtShGen.Api.Providers;

public class ExifInfoProvider
{
    public static ExifInfo Read(string imageFileName)
    {
        ExifImageProperties exif = ExifPhoto.GetExifDataPhoto(imageFileName);

        return new ExifInfo()
        {
            Name = Path.GetFileName(imageFileName),
            Path = Path.GetFullPath(imageFileName),
            FocalLength = exif.FocalLength,
            Iso = exif.ISOSpeedRatings,
            Aperture = exif.FNumber,
            ExposureTime = exif.ExposureTime,
            ShutterSpeed = (float)System.Math.Round(exif.ShutterSpeedValue, 2),
            Width = exif.ExifImageWidth,
            Height = exif.ExifImageHeight,
            Orientation = (int)exif.Orientation
        };
    }
}