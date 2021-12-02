using System;

namespace nCtShGen.Api.Model;

public record ConfigurationItem
{
    public string RootPhotoFolder { get; set; } = default!;
    public byte FolderDeepLevel { get; set; } = default!;
    public int MaxContactSheetWidth { get; set; } = 1900;
    public ConfigurationThumbnailItem Thumbnail { get; set; } = default!;
}