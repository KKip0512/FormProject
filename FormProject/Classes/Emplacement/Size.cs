using SizeS = System.Drawing.Size;

namespace FormProject.Classes.Emplacement
{
    internal class Size(int height, int width)
    {
        public int Width { get; set; } = width;
        public int Height { get; set; } = height;
        public SizeS Struct => new(Width, Height);

        public Size(SizeS size) : this(size.Width, size.Height) { }

        public void Resize(int x, int y)
        {
            Width += x;
            Height += y;
        }
        public void Resize(Size offset) => Resize(offset.Width, offset.Height);
    }
}

