using System.Drawing;

namespace nCtShGen.Api.Model;

public record ColorSchema
{
    public ColorSchemaName Name { get; set; } = default!;

    public Color ContactSheetBackground { get; set; } = default!;
    public Color ContactSheetTitle { get; set; } = default!;
    public Color ContactSheetDetails { get; set; } = default!;


    public Color ThumbnailTitle { get; set; } = default!;
    public Color ThumbnailImageExifInfo { get; set; } = default!;
    public Color ThumbnailBackground { get; set; } = default!;
    public Color ThumbnailFrame { get; set; } = default!;
    public Color ThumbnailImageFrame { get; set; } = default!;
}