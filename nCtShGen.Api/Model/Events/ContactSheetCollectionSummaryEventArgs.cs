using System.Drawing;

namespace nCtShGen.Api.Model.Events;

public class ContactSheetCollectionSummaryEventArgs : EventArgs
{
    public int AllContactSheets { get; set; } = 0;

    public TimeSpan Duration { get; set; } = new TimeSpan();

    public ContactSheetCollectionSummaryEventArgs(int allContactSheets, TimeSpan duration) : base()
    {
        this.AllContactSheets = allContactSheets;
        this.Duration = duration;
    }
}