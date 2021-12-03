using System.Drawing;

namespace nCtShGen.Api.Model;

internal class ImageWithPosition
{
    public string FileName { get; private set; } = default!;
    public Image Image { get; private set; } = default!;
    public Point Position { get; private set; } = default!;

    public int MaxPositionX
    {
        get { return Position.X + Image.Width; }
    }

    public int MaxPositionY
    {
        get { return Position.Y + Image.Height; }
    }

    public ImageWithPosition(string fileName, Image image, Point position)
    {
        this.FileName = fileName;
        this.Image = image;
        this.Position = position;
    }
}