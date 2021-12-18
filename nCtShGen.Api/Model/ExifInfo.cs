using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;

namespace nCtShGen.Api.Model;

public record ExifInfo
{
    public string Name { set; get; } = string.Empty;
    public string Path { set; get; } = string.Empty;
    public float FocalLength { get; set; }
    public short Iso { get; set; }
    public float Aperture { get; set; }
    public float ExposureTime { get; set; }
    public float ShutterSpeed { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Orientation { get; set; }
    public RotateFlipType RotateType { get; set; }
    public string GpsInfo { get; set; } = string.Empty;

    private string ShutterSpeedToString()
    {
        if (ExposureTime < 1)
            return string.Format(CultureInfo.InvariantCulture, "1/{0:0}", ShutterSpeed);
        else
            return string.Format(CultureInfo.InvariantCulture, "{0:0}", ExposureTime);
    }

    private string ApertureToString()
    {
        //return string.Format(CultureInfo.InvariantCulture, "{0:#}", Aperture);
        if (Aperture % 1 == 0)
            return Aperture.ToString("0", CultureInfo.InvariantCulture);
        else
            return Aperture.ToString("0.#", CultureInfo.InvariantCulture);
    }

    private string FocalLengthToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "{0:#}", FocalLength);
        // if (Aperture % 1 == 0)
        //     return Aperture.ToString("0", CultureInfo.InvariantCulture);
        // else
        //     return Aperture.ToString("0.#", CultureInfo.InvariantCulture);
    }

    public bool IsHorizontal()
    {
        return (this.Width >= this.Height);
    }

    public override string ToString()
    {
        if (Width == 0)
        {
            return " (no EXIF data) ";
        }

        return string.Format("ƒ{0} {1}s {2}mm Iso{3}",
            ApertureToString(),
            ShutterSpeedToString(),
            FocalLengthToString(),
            Iso);
    }
}