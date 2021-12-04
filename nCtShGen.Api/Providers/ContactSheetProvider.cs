using System.Drawing;
using System.Drawing.Imaging;
using nCtShGen.Api.Model;
using nCtShGen.Api.Model.Events;

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
    private Brush brushCsTitleBackground = default!;
    private Font fontCsTitle = default!;
    private Font fontCsDetails = default!;

    // ------------------------------------------------------------------------

    private readonly ConfigurationItem configurationItem = default!;
    private readonly ThumbnailProvider thumbnailProvider = default!;
    private readonly ColorSchema colorSchema = default!;

    // ------------------------------------------------------------------------

    public event EventHandler<ContactSheetEventArgs> OnStartGenerateContactSheet = default!;
    public event EventHandler<ContactSheetEventArgs> OnFinishGenerateContactSheet = default!;
    public event EventHandler<ContactSheetItemEventArgs> OnAddContactSheetItem = default!;

    // ------------------------------------------------------------------------

    public ContactSheetProvider(ConfigurationItem configurationItem, ColorSchemaName colorSchemaName)
    {
        this.configurationItem = configurationItem;
        this.thumbnailProvider = new(configurationItem.Thumbnail);
        this.colorSchema = ColorSchemaProvider.Get(colorSchemaName);

        CreateUISettings();
    }

    private void CreateUISettings()
    {
        // fonts, brushes & pens
        fontCsiTitle = new("Consolas", 10f, FontStyle.Bold);
        fontCsiExifInfo = new("Consolas", 8f, FontStyle.Regular);
        brushCsiTitle = new SolidBrush(colorSchema.ThumbnailTitle);
        brushCsiExifInfo = new SolidBrush(colorSchema.ThumbnailImageExifInfo);
        brushCsiFill = new SolidBrush(colorSchema.ThumbnailBackground);
        penCsi = new(colorSchema.ThumbnailFrame, 1);
        penThumb = new(colorSchema.ThumbnailImageFrame, 1);

        brushCsFill = new SolidBrush(colorSchema.ContactSheetBackground);
        brushCsTitle = new SolidBrush(colorSchema.ContactSheetTitle);
        brushCsDetails = new SolidBrush(colorSchema.ContactSheetDetails);
        brushCsTitleBackground = new SolidBrush(colorSchema.ThumbnailBackground);
        fontCsTitle = new("Consolas", 11f, FontStyle.Bold);
        fontCsDetails = new("Consolas", 10f, FontStyle.Regular);
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

        // draw additional information
        if (!string.IsNullOrEmpty(exifInfo.GpsInfo))
        {
            string iconPath = string.Format("{0}.Icons.gps.png", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            var iconResource = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(iconPath);
            if (iconResource != null)
            {
                Bitmap iconGps = new(iconResource);

                //create a color matrix object  
                ColorMatrix matrix = new()
                {
                    Matrix33 = 0.3f
                };

                ImageAttributes iconAttributes = new();
                iconAttributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                //now draw the image  
                if (iconGps != null)
                {
                    Rectangle destRect = new(image.Width - 16, image.Height - 5, iconGps.Width, iconGps.Height);
                    gs.DrawImage(iconGps, destRect, 0, 0, iconGps.Width, iconGps.Height, GraphicsUnit.Pixel, iconAttributes);
                }
            }
        }

        return (Image)(csItem);
    }

    public Image GenerateContactSheet(string title, string folderPath, string filter = "*.*")
    {
        OnStartGenerateContactSheet?.Invoke(this, new ContactSheetEventArgs(folderPath, 0));

        int currYInit = contactSheetTitleHeight + contactSheetTopMargin;
        int currXInit = margin;
        int currX = currXInit;
        int currY = currYInit;
        int csMinimumWidth = this.configurationItem.Thumbnail.MaxWidth * 2;

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
                currX = currXInit;
                currY += maxHeightInRow + margin;
                maxHeightInRow = img.Height;
            }

            ImageWithPosition newItem = new(filePath, img, new Point(currX, currY));
            positions.Add(newItem);

            tmp = (currX + img.Width + margin);
            currX = tmp;

            if (tmp > csWidth)
                csWidth = tmp;

            tmp = (currY + img.Height + margin);

            if (tmp > csHeight)
                csHeight = tmp;
        }

        if (csWidth < csMinimumWidth)
        {
            csWidth = csMinimumWidth;
        }

        Bitmap csImage = new(csWidth, csHeight);
        Graphics gs = Graphics.FromImage(csImage);

        // frame
        gs.FillRectangle(brushCsFill, 0, 0, csImage.Width, csImage.Height);

        Rectangle contactSheetTitleRectangle = new(0, 0, csImage.Width - 1, contactSheetTitleHeight);
        gs.FillRectangle(brushCsTitleBackground, contactSheetTitleRectangle);
        gs.DrawRectangle(penCsi, contactSheetTitleRectangle);

        // title
        string contactSheetTitle = string.Format("[{0}]", title);
        string contactSheetDetails = string.Format("[{0}] ● [{1}] image(s)", folderPath, positions.Count);

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
            OnAddContactSheetItem?.Invoke(this, new ContactSheetItemEventArgs(iwp.FileName));
        }

        OnFinishGenerateContactSheet?.Invoke(this, new ContactSheetEventArgs(folderPath, positions.Count));

        return (Image)csImage;
    }


}