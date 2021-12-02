using System.Drawing;
using nCtShGen.Api.Model;

namespace nCtShGen.Api.Providers;

public class ContactSheetProvider
{

    #region ConstsForContactSheetItem

    private const int csiWidthMargin = 12;
    private const int csiTopMargin = 18;
    private const int csiBottomMargin = 18;

    #endregion ConstsForContactSheetItem

    #region ConstsForContactSheet

    private const int margin = 5;
    private const int contactSheetTitleHeight = 25;
    private const int contactSheetTopMargin = 25;

    #endregion ConstsForContactSheet

    // ------------------------------------------------------------------------

    private readonly ConfigurationItem configurationItem = default!;
    private readonly ThumbnailProvider thumbnailProvider = default!;

    // ------------------------------------------------------------------------

    // private Color colorTitle = default!;
    // private Color colorExifInfo = default!;
    // private Color colorContactSheetBackground = default!;
    // private Color colorBackground = default!;
    // private Color colorFrame = default!;
    // private Color colorFrameImage = default!;

    // private Color colorCsTitle = default!;
    // private Color colorCsDetails = default!;

    // ------------------------------------------------------------------------

    private Font fontCsiTitle = default!;
    private Font fontCsiExifInfo = default!;
    private Brush brushCsiTitle = default!;
    private Brush brushCsiExifInfo = default!;
    private Brush brushCsiFill = default!;
    private Pen penCsi = default!;
    private Pen penThumb = default!;


    private Brush brushCsFill = default!;
    private Brush brushCsTitle = default!;
    private Brush brushCsDetails = default!;
    private Font fontCsTitle = default!;
    private Font fontCsDetails = default!;

    private ColorSchema colorSchema = default!;


    // ------------------------------------------------------------------------

    public ContactSheetProvider(ConfigurationItem configurationItem, ColorSchemaName colorSchemaName)
    {
        this.configurationItem = configurationItem;
        this.thumbnailProvider = new(configurationItem.Thumbnail);
        this.colorSchema = ColorSchemaProvider.Get(colorSchemaName);

        CreateUISettings();
    }

    // private void CreateColors()
    // {
    //     colorContactSheetBackground = Color.FromKnownColor(KnownColor.Black);

    //     colorTitle = Color.FromKnownColor(KnownColor.Gray);
    //     colorExifInfo = Color.FromKnownColor(KnownColor.DarkSlateGray);

    //     //colorBackground = Color.FromArgb(10, 10, 10);
    //     colorBackground = Color.FromArgb(255, 255, 255);
    //     colorFrame = Color.FromKnownColor(KnownColor.DarkSlateGray);
    //     colorFrameImage = Color.FromKnownColor(KnownColor.Gray);

    //     colorCsTitle = Color.FromKnownColor(KnownColor.Gray);
    //     colorCsDetails = Color.FromKnownColor(KnownColor.DarkSlateGray);
    // }

    private void CreateUISettings()
    {
        // fonts, brushes & pens
        fontCsiTitle = new("Consolas", 10f, FontStyle.Bold);
        fontCsiExifInfo = new("Consolas", 9f, FontStyle.Regular);
        brushCsiTitle = new SolidBrush(colorSchema.ThumbnailTitle);
        brushCsiExifInfo = new SolidBrush(colorSchema.ThumbnailImageExifInfo);
        brushCsiFill = new SolidBrush(colorSchema.ThumbnailBackground);
        penCsi = new(colorSchema.ThumbnailFrame, 1);
        penThumb = new(colorSchema.ThumbnailImageFrame, 1);

        brushCsFill = new SolidBrush(colorSchema.ContactSheetBackground);
        brushCsTitle = new SolidBrush(colorSchema.ContactSheetTitle);
        brushCsDetails = new SolidBrush(colorSchema.ContactSheetDetails);
        fontCsTitle = new("Consolas", 14f, FontStyle.Bold);
        fontCsDetails = new("Consolas", 12f, FontStyle.Regular);
    }

    public Image GenerateContactSheetItem(string filePath)
    {
        var (exifInfo, image) = thumbnailProvider.GetThumbnail(filePath);

        // ContactSheet item size
        int csiWidth = image.Width + csiWidthMargin;
        int csiHeight = image.Height + (csiTopMargin + csiBottomMargin);

        // image position in ContactSheet item
        int csiImageX = (csiWidthMargin / 2);
        int csiImageY = csiTopMargin;

        // draw frames and titles
        Bitmap csItem = new(csiWidth, csiHeight);
        Graphics gs = Graphics.FromImage(csItem);

        gs.FillRectangle(brushCsiFill, 0, 0, csiWidth - 1, csiHeight - 1);
        gs.DrawImage(image, csiImageX, csiImageY);

        gs.DrawRectangle(penCsi, 0, 0, csiWidth - 1, csiHeight - 1);
        gs.DrawRectangle(penThumb, csiImageX, csiImageY, image.Width - 1, image.Height - 1);

        gs.DrawString(exifInfo.Name, fontCsiTitle, brushCsiTitle, csiImageX, 1);
        gs.DrawString(exifInfo.ToString(), fontCsiExifInfo, brushCsiExifInfo, csiImageX, (csiHeight - csiBottomMargin) + 1);

        return (Image)(csItem);
    }

    public Image GenerateContactSheet(string title, string folderPath, string filter = "*.*")
    {
        int currYInit = contactSheetTitleHeight + contactSheetTopMargin;
        int currX = 0;
        int currY = currYInit;

        int csWidth = 0;
        int csHeight = 0;
        int maxHeightInRow = 0;
        int tmp;

        var options = new EnumerationOptions()
        {
            MaxRecursionDepth = configurationItem.FolderDeepLevel
        };

        List<ImageWithPosition> positions = new();
        string[] files = Directory.GetFiles(folderPath, filter, options);

        // add thumbnails to collection and calculate contactsheet size
        foreach (string filePath in files)
        {
            Image img = GenerateContactSheetItem(filePath);

            if (img.Height > maxHeightInRow)
            {
                maxHeightInRow = img.Height;
            }

            // check if current thumbnail fit to max width of contactsheet
            tmp = (currX + img.Width + margin);

            if (tmp > configurationItem.MaxContactSheetWidth)
            {
                currX = 0;
                currY += maxHeightInRow + margin;
                maxHeightInRow = img.Height;
            }

            ImageWithPosition newItem = new(img, new Point(currX, currY));
            positions.Add(newItem);

            tmp = (currX + img.Width + margin);
            currX = tmp;

            if (tmp > csWidth)
                csWidth = tmp;

            tmp = (currY + img.Height + margin);

            if (tmp > csHeight)
                csHeight = tmp;
        }

        Bitmap csImage = new(csWidth, csHeight);
        Graphics gs = Graphics.FromImage(csImage);

        // frame
        gs.FillRectangle(brushCsFill, 0, 0, csImage.Width - 1, csImage.Height - 1);
        Rectangle contactSheetTitleRectangle = new(0, 0, csImage.Width - 1, contactSheetTitleHeight - 1);
        gs.DrawRectangle(penCsi, 0, 0, csImage.Width - 1, contactSheetTitleHeight - 1);

        // title
        string contactSheetTitle = string.Format("[{0}]", title);
        string contactSheetDetails = string.Format("Folder [{0}] contains [{1}] image(s)", folderPath, positions.Count);

        StringFormat sfTitle = new()
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Near
        };
        StringFormat sfDetails = new()
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Far
        };
        gs.DrawString(contactSheetTitle, fontCsTitle, brushCsTitle, contactSheetTitleRectangle, sfTitle);
        gs.DrawString(contactSheetDetails, fontCsDetails, brushCsDetails, contactSheetTitleRectangle, sfDetails);

        // add thumbnails

        foreach (ImageWithPosition iwp in positions)
        {
            gs.DrawImage(iwp.Image, iwp.Position);
        }

        return (Image)csImage;
    }


}