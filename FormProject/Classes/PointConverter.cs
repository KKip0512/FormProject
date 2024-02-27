namespace FormProject.Classes
{
    internal class PointConverter(Reference<Size> sizeOfWindow, Reference<SizeF> scale, Reference<PointF> systemCenterCoordinate)
    {
        public Size SizeOfWindow { get => sizeOfWindow.Value; }
        public SizeF Scale { get => scale.Value; }
        public PointF SystemCenterCoordinate { get => systemCenterCoordinate.Value; }

        public Point CoordinateToPixelPosition(PointF coordinate)
        {
            int x = (int)Math.Round(
                (coordinate.X - SystemCenterCoordinate.X + Scale.Width) / 2 * SizeOfWindow.Width / Scale.Width);

            int y = (int)(SizeOfWindow.Height - Math.Round(
                (coordinate.Y - SystemCenterCoordinate.Y + Scale.Height) / 2 * SizeOfWindow.Height / Scale.Height));

            return new Point(x, y);
        }

        public PointF PixelPositionToCoordinate(Point pixelPosition)
        {
            float x = SystemCenterCoordinate.X + ((float)pixelPosition.X / SizeOfWindow.Width * Scale.Width * 2 - Scale.Width);
            float y = SizeOfWindow.Height - SystemCenterCoordinate.Y +
                ((float)pixelPosition.Y / SizeOfWindow.Height * Scale.Height * 2 - Scale.Height);

            return new PointF(x, y);
        }
    }
}
