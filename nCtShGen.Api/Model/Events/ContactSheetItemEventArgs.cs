using System.Drawing;

namespace nCtShGen.Api.Model.Events;

public class ContactSheetItemEventArgs : EventArgs
{
    public string FileName { get; set; } = default!;

    public ContactSheetItemEventArgs(string fileName) : base()
    {
        this.FileName = fileName;
    }
}