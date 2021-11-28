using System;

namespace nCtShGen.Api.Model;

public record ConfigurationItem
{
    public string? RootPhotoFolder { set; get; }
    public byte FolderDeepLevel { set; get; }
    public ConfigurationThumbnailItem Thumbnail { get; set; }
}