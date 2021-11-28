using System;

namespace nCtShGen.Api.Model;

public record ConfigurationItem
{
    public string RootPhotoFolder { set; get; } = default!;
    public byte FolderDeepLevel { set; get; } = default!;
    public ConfigurationThumbnailItem Thumbnail { get; set; } = default!;
}