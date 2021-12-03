using System.Drawing;
using Microsoft.Extensions.Configuration;
using nCtShGen.Api.Model;

namespace nCtShGen.Api.Providers;

public class ColorSchemaProvider
{
    public static ColorSchema Get(ColorSchemaName colorSchemaName)
    {
        switch (colorSchemaName)
        {
            case ColorSchemaName.Light:
                {
                    return new()
                    {
                        ContactSheetBackground = Color.FromKnownColor(KnownColor.White),
                        ContactSheetDetails = Color.FromKnownColor(KnownColor.Gray),
                        ContactSheetTitle = Color.FromKnownColor(KnownColor.DarkSlateGray),
                        ThumbnailBackground = Color.FromKnownColor(KnownColor.AntiqueWhite),
                        ThumbnailFrame = Color.FromKnownColor(KnownColor.Silver),
                        ThumbnailImageFrame = Color.FromKnownColor(KnownColor.Gray),
                        ThumbnailImageExifInfo = Color.FromKnownColor(KnownColor.DarkSlateGray),
                        ThumbnailTitle = Color.FromKnownColor(KnownColor.Black)
                    };
                }
            case ColorSchemaName.Dark:
                {
                    return new()
                    {
                        ContactSheetBackground = Color.FromKnownColor(KnownColor.Black),
                        ContactSheetDetails = Color.FromKnownColor(KnownColor.DarkSlateGray),
                        ContactSheetTitle = Color.FromKnownColor(KnownColor.Gray),
                        ThumbnailBackground = Color.FromArgb(10, 10, 10),
                        ThumbnailFrame = Color.FromKnownColor(KnownColor.DarkSlateGray),
                        ThumbnailImageFrame = Color.FromKnownColor(KnownColor.Gray),
                        ThumbnailImageExifInfo = Color.FromKnownColor(KnownColor.DarkSlateGray),
                        ThumbnailTitle = Color.FromKnownColor(KnownColor.Gray)
                    };
                }
            default:
                {
                    return new()
                    {
                        ContactSheetBackground = Color.FromKnownColor(KnownColor.Window),
                        ContactSheetDetails = Color.FromKnownColor(KnownColor.WindowText),
                        ContactSheetTitle = Color.FromKnownColor(KnownColor.WindowText),
                        ThumbnailBackground = Color.FromKnownColor(KnownColor.ControlLight),
                        ThumbnailFrame = Color.FromKnownColor(KnownColor.ActiveCaption),
                        ThumbnailImageFrame = Color.FromKnownColor(KnownColor.WindowFrame),
                        ThumbnailImageExifInfo = Color.FromKnownColor(KnownColor.ActiveCaptionText),
                        ThumbnailTitle = Color.FromKnownColor(KnownColor.ActiveCaptionText)
                    };
                }
        }
    }
}