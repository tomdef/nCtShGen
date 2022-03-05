using System.Drawing;
using ExifPhotoReader;
using nCtShGen.Api.Model;

namespace nCtShGen.Api.Providers;

public class ExifInfoProvider
{
    public static ExifInfo Read(string imageFileName)
    {
        try
        {
            ExifImageProperties exif = ExifPhoto.GetExifDataPhoto(imageFileName);

            string gpsInfo = ((exif.GPSInfo.Latitude > 0) && (exif.GPSInfo.Longitude > 0))
                ? string.Format("{0} {1}", exif.GPSInfo.Latitude, exif.GPSInfo.Longitude)
                : string.Empty;

            RotateFlipType rotateType = RotateFlipType.RotateNoneFlipNone;

            switch (exif.Orientation)
            {
                case Orientation.Rotate270:
                    {
                        rotateType = RotateFlipType.Rotate270FlipNone;
                        break;
                    }
                case Orientation.Rotate90:
                    {
                        rotateType = RotateFlipType.Rotate90FlipNone;
                        break;
                    }
            };

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
                Orientation = (int)exif.Orientation,
                RotateType = rotateType,
                GpsInfo = gpsInfo,
                IsValid = ((exif.FocalLength > 0f) && (exif.ISOSpeedRatings > 0) && (exif.FNumber > 0f) && (exif.ExposureTime > 0f)),
                ErrorMessage = string.Empty
            };
        }
        catch (Exception ex)
        {
            return new ExifInfo()
            {
                Name = Path.GetFileName(imageFileName),
                Path = Path.GetFullPath(imageFileName),
                FocalLength = 0,
                Iso = 0,
                Aperture = 0,
                ExposureTime = 0,
                ShutterSpeed = 0,
                Width = 0,
                Height = 0,
                Orientation = 0,
                RotateType = RotateFlipType.RotateNoneFlipNone,
                GpsInfo = string.Empty,
                IsValid = false,
                ErrorMessage = ex.Message
            };
        }
    }
}