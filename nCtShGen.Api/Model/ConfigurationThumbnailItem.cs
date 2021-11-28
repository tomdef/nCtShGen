using System;

namespace nCtShGen.Api.Model;

public record ConfigurationThumbnailItem
{
    public int MaxWidth { set; get; }
    public int MaxHeight { set; get; }

}