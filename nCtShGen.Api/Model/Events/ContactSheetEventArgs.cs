using System.Drawing;

namespace nCtShGen.Api.Model.Events;

public class ContactSheetEventArgs : EventArgs
{
    public string Folder { get; set; } = default!;

    public int AllItems { get; set; } = 0;

    public ContactSheetEventArgs(string folder, int allItems) : base()
    {
        this.Folder = folder;
        this.AllItems = allItems;
    }
}