using System;
using System.Diagnostics.Contracts;

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
    public string GpsInfo { get; set; } = string.Empty;

    private string ShutterSpeedToString()
    {
        if (ExposureTime < 1)
            return string.Format("1/{0}", ShutterSpeed);
        else
            return string.Format("{0}", ExposureTime);
    }

    private string ApertureToString()
    {
        if (Aperture % 1 == 0)
            return Aperture.ToString("0", System.Globalization.CultureInfo.InvariantCulture);
        else
            return Aperture.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);
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

        return string.Format("ƒ{0} {1}s {2}mm ISO{3}",
            ApertureToString(),
            ShutterSpeedToString(),
            FocalLength,
            Iso);
    }
}