using SizeFS = System.Drawing.SizeF;

namespace FormProject.Classes.Emplacement
{
    internal class SizeF(float height, float width)
    {
        public float Width { get; set; } = width;
        public float Height { get; set; } = height;
        public SizeFS Struct => new(Width, Height);

        public SizeF(SizeFS size) : this(size.Width, size.Height) { }

        public void Resize(float x, float y)
        {
            Width += x;
            Height += y;
        }
        public void Resize(SizeF offset) => Resize(offset.Width, offset.Height);
    }
}
