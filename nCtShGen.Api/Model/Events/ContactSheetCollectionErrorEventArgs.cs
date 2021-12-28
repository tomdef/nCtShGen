using System.Drawing;

namespace nCtShGen.Api.Model.Events;

public class ContactSheetCollectionErrorEventArgs : EventArgs
{
    public string Details { get; set; } = default!;
    public Exception Exception { get; set; } = default!;

    public ContactSheetCollectionErrorEventArgs(string details, Exception ex) : base()
    {
        this.Details = details;
        this.Exception = ex;
    }

    public ContactSheetCollectionErrorEventArgs(Exception ex) : base()
    {
        this.Details = ex.Message;
        this.Exception = ex;
    }
}