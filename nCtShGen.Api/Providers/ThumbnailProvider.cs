using System.Collections;
using System.Diagnostics;
using System.Drawing;
using ExifPhotoReader;
using Microsoft.Extensions.FileProviders;
using nCtShGen.Api.Model;
using nCtShGen.Api.Model.Events;

namespace nCtShGen.Api.Providers;

public class ThumbnailProvider
{
    private readonly ConfigurationThumbnailItem configurationItem = default!;

    public event EventHandler<ThumbnailWarningEventArgs> OnWarningThumbnail = default!;

    public ThumbnailProvider(ConfigurationThumbnailItem configurationItem)
    {
        this.configurationItem = configurationItem;
    }

    public Image GetThumbnail(ExifInfo exifInfo)
    {
        Image image = Image.FromFile(exifInfo.Path);
        var (width, height) = GetThumbnailSize(exifInfo.Width, exifInfo.Height, exifInfo.IsHorizontal());

        image = (Image)(new Bitmap(image, new Size(width, height)));

        if (exifInfo.RotateType != RotateFlipType.RotateNoneFlipNone)
        {
            image.RotateFlip(exifInfo.RotateType);
        }

        return image;
    }

    public (ExifInfo, Image?) GetThumbnail(string filePath)
    {
        ExifInfo exifInfo = ExifInfoProvider.Read(filePath);

        if ((exifInfo.Width == 0) || (exifInfo.Height == 0))
        {
            Image tmpImg = Image.FromFile(filePath);
            if (tmpImg != null)
            {

                exifInfo.Width = tmpImg.Width;
                exifInfo.Height = tmpImg.Height;

                string msg = $"Image size not exists in exif and it is taken directly from image size ({exifInfo.Width}x{exifInfo.Height})";
                OnWarningThumbnail?.Invoke(this, new ThumbnailWarningEventArgs(filePath, msg));

                if ((exifInfo.Width == 0) || (exifInfo.Height == 0))
                {
                    return (exifInfo, null);
                }
            }
        }

        Image image = this.GetThumbnail(exifInfo);

        return (exifInfo, image);
    }

    public (int, int) GetThumbnailSize(int width, int height, bool isHorizontal)
    {
        int size = configurationItem.MaxWidth;

        float scale = (isHorizontal) ? (size / (float)width) : (size / (float)height);

        width = (int)(Math.Round(scale * width));
        height = (int)(Math.Round(scale * height));

        return (width, height);
    }
}