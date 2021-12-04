using System.Collections;
using System.Drawing;
using ExifPhotoReader;
using nCtShGen.Api.Model;

namespace nCtShGen.Api.Providers;

public class ThumbnailProvider
{
    private readonly ConfigurationThumbnailItem configurationItem = default!;

    public ThumbnailProvider(ConfigurationThumbnailItem configurationItem)
    {
        this.configurationItem = configurationItem;
    }

    public Image GetThumbnail(ExifInfo exifInfo)
    {
        var (width, height) = GetThumbnailSize(exifInfo.Width, exifInfo.Height, exifInfo.IsHorizontal());

        Image image = Image.FromFile(exifInfo.Path);
        image = (Image)(new Bitmap(image, new Size(width, height)));

        if (exifInfo.RotateType != RotateFlipType.RotateNoneFlipNone)
        {
            image.RotateFlip(exifInfo.RotateType);
        }

        // if ((exifInfo.Width == 0) || (exifInfo.Height == 0) ||
        //     (exifInfo.RotateType != RotateFlipType.RotateNoneFlipNone))
        // {
        //     width = image.Width;
        //     height = image.Height;
        // }

        return image;
    }

    public (ExifInfo, Image) GetThumbnail(string filePath)
    {
        ExifInfo exifInfo = ExifInfoProvider.Read(filePath);
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