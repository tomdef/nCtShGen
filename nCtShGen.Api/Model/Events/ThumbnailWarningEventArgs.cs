using System.Drawing;

namespace nCtShGen.Api.Model.Events;

public class ThumbnailWarningEventArgs : EventArgs
{
    public string FileName { get; set; } = default!;
    public string Details { get; set; } = default!;

    public ThumbnailWarningEventArgs(string fileName, string details) : base()
    {
        this.FileName = fileName;
        this.Details = details;
    }
}