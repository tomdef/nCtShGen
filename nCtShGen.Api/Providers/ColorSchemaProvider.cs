using System.Drawing;
using Microsoft.Extensions.Configuration;
using nCtShGen.Api.Model;

namespace nCtShGen.Api.Providers;

public class ColorSchemaProvider
{
    private readonly List<ColorSchema> colorSchemaCollection;

    public ColorSchemaProvider()
    {
        colorSchemaCollection = new List<ColorSchema>() {
            new ColorSchema() {
                        Name = ColorSchemaName.Dark,
                        ContactSheetBackground = Color.FromKnownColor(KnownColor.Black),
                        ContactSheetDetails = Color.FromKnownColor(KnownColor.DarkSlateGray),
                        ContactSheetTitle = Color.FromKnownColor(KnownColor.DarkGray),
                        ThumbnailBackground = Color.FromKnownColor(KnownColor.Black),
                        ThumbnailFrame = Color.FromKnownColor(KnownColor.DarkSlateGray),
                        ThumbnailImageFrame = Color.FromKnownColor(KnownColor.DarkGray),
                        ThumbnailImageExifInfo = Color.FromKnownColor(KnownColor.DarkSlateGray),
                        ThumbnailTitle = Color.FromKnownColor(KnownColor.DarkGray)
            },
             new ColorSchema() {
                        Name = ColorSchemaName.Light,
                        ContactSheetBackground = Color.FromKnownColor(KnownColor.Window),
                        ContactSheetDetails = Color.FromKnownColor(KnownColor.WindowText),
                        ContactSheetTitle = Color.FromKnownColor(KnownColor.WindowText),
                        ThumbnailBackground = Color.FromKnownColor(KnownColor.ControlLight),
                        ThumbnailFrame = Color.FromKnownColor(KnownColor.ActiveCaption),
                        ThumbnailImageFrame = Color.FromKnownColor(KnownColor.WindowFrame),
                        ThumbnailImageExifInfo = Color.FromKnownColor(KnownColor.ActiveCaptionText),
                        ThumbnailTitle = Color.FromKnownColor(KnownColor.ActiveCaptionText)
             }
        };
    }

    public ColorSchema Get(ColorSchemaName colorSchemaName)
    {
        return colorSchemaCollection.First(c => c.Name == colorSchemaName);
    }

    public ColorSchema Get()
    {
        return colorSchemaCollection.First(c => c.Name == ColorSchemaName.Dark);
    }
}