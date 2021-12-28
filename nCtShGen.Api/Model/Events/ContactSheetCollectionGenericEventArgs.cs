using System.Drawing;

namespace nCtShGen.Api.Model.Events;

public class ContactSheetCollectionGenericEventArgs : EventArgs
{
    public string Details { get; set; } = default!;
    public string Reason { get; set; } = default!;

    public ContactSheetCollectionGenericEventArgs(string details, string reason) : base()
    {
        this.Details = details;
        this.Reason = reason;
    }

    public ContactSheetCollectionGenericEventArgs(string details) : this(details, "") { }
}