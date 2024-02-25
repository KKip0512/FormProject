using PointFS = System.Drawing.PointF;

namespace FormProject.Classes.Emplacement
{
    internal class PointF(float x, float y)
    {
        public float X { get; set; } = x;
        public float Y { get; set; } = y;
        public PointFS Struct => new(X, Y);

        public PointF(PointFS point) : this(point.X, point.Y) { }

        public void Offset(float x, float y)
        {
            X += x;
            Y += y;
        }
        public void Offset(SizeF offset) => Offset(offset.Width, offset.Height);
    }
}
