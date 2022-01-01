using System;

namespace nCtShGen.Api.Model;

public record ConfigurationItem
{
    public string RootPhotoFolder { get; set; } = default!;
    public string ExcludeFolders { get; set; } = default!;
    public string ContactSheetFolder { get; set; } = default!;
    public string ContactSheetFileNameTemplate { get; set; } = default!;
    public byte ContactSheetRootFolderOnLevel { get; set; } = default!;
    public byte ContactSheetSubfolderDeepLevel { get; set; } = default!;
    public int MaxContactSheetWidth { get; set; } = 1900;
    public ExistsAction ContactSheetExistsAction { get; set; } = ExistsAction.Skip;
    public ConfigurationThumbnailItem Thumbnail { get; set; } = default!;
}