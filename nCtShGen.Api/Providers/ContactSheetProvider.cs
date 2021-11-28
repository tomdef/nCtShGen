using System.Drawing;
using System.Runtime.ExceptionServices;
using nCtShGen.Api.Model;

namespace nCtShGen.Api.Providers;

public class ContactSheetProvider
{
    private readonly ConfigurationItem configurationItem = default!;
    private readonly ThumbnailProvider thumbnailProvider = default!;

    public ContactSheetProvider(ConfigurationItem configurationItem)
    {
        this.configurationItem = configurationItem;
        this.thumbnailProvider = new(configurationItem.Thumbnail);
    }

    public Image GenerateContactSheetItem(string filePath)
    {
        var (exifInfo, image) = thumbnailProvider.GetThumbnail(filePath);

        // margins
        int csiWidthMargin = 10;
        int csiTopMargin = 15;
        int csiBottomMargin = 15;

        // ContactSheet item size
        int csiWidth = image.Width + csiWidthMargin;
        int csiHeight = image.Height + (csiTopMargin + csiBottomMargin);

        // image position in ContactSheet item
        int csiImageX = (csiWidthMargin / 2);
        int csiImageY = csiTopMargin;

        // fonts & brushes


        Font fontName = new(FontFamily.GenericSansSerif, 7f, FontStyle.Bold);
        //Brush brushName = new SolidBrush(Color.FromArgb(100, 100, 100));
        Brush brushName = new SolidBrush(Color.FromKnownColor(KnownColor.ControlText));
        //Brush fillBrush = new SolidBrush(Color.FromArgb(40, 40, 40));
        Brush fillBrush = new SolidBrush(Color.FromKnownColor(KnownColor.Control));
        Pen penCsi = new(Color.FromArgb(50, 50, 50), 1);
        Pen penThumb = new(Color.FromArgb(90, 90, 90), 1);

        // draw frames and titles
        Bitmap csItem = new(csiWidth, csiHeight);
        Graphics gs = Graphics.FromImage(csItem);


        gs.FillRectangle(fillBrush, 0, 0, csiWidth - 1, csiHeight - 1);
        gs.DrawImage(image, csiImageX, csiImageY);

        gs.DrawRectangle(penCsi, 0, 0, csiWidth - 1, csiHeight - 1);
        gs.DrawRectangle(penThumb, csiImageX, csiImageY, image.Width - 1, image.Height - 1);

        gs.DrawString(exifInfo.Name, fontName, brushName, csiImageX, 1);
        gs.DrawString(exifInfo.ToString(), fontName, brushName, csiImageX, csiHeight - csiBottomMargin);

        return (Image)(csItem);
    }
}