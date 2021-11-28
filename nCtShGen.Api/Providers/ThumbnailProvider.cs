using System.Drawing;
using nCtShGen.Api.Model;

namespace nCtShGen.Api.Providers;

public class ThumbnailProvider
{
    public static Image Get(ExifInfo exifInfo, ConfigurationThumbnailItem configurationItem)
    {
        // int maxWidth = configurationItem.MaxWidth;
        // int maxHeight = configurationItem.MaxHeight;

        // Image image = Image.FromFile(exifInfo.Path);

        // return image.GetThumbnailImage(120, 120, () => false, IntPtr.Zero);
        return null;
    }
}