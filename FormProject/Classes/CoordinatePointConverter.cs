namespace FormProject.Classes
{
    internal class CoordinatePointConverter(Reference<Size> sizeOfWindow, Reference<SizeF> scale, Reference<PointF> systemCenterCoordinate)
    {
        public Size SizeOfWindow { get => sizeOfWindow.Value; }
        public SizeF Scale { get => scale.Value; }
        public PointF SystemCenterCoordinate { get => systemCenterCoordinate.Value; }

        public Point CoordinateToPixel(PointF coordinate)
        {
            int x = (int)Math.Round(
                (coordinate.X - SystemCenterCoordinate.X + Scale.Width) / 2 * SizeOfWindow.Width / Scale.Width);

            int y = (int)(SizeOfWindow.Height - Math.Round(
                (coordinate.Y - SystemCenterCoordinate.Y + Scale.Height) / 2 * SizeOfWindow.Height / Scale.Height));

            return new Point(x, y);
        }

        public PointF PixelToCoordinate(Point pixelPosition)
        {
            float x = SystemCenterCoordinate.X + ((float)pixelPosition.X / SizeOfWindow.Width * Scale.Width * 2 - Scale.Width);
            float y = SizeOfWindow.Height - SystemCenterCoordinate.Y +
                ((float)pixelPosition.Y / SizeOfWindow.Height * Scale.Height * 2 - Scale.Height);

            return new PointF(x, y);
        }

        public LineSegment CoordinateToPixel(LineSegment coordinateSegment)
        {
            PointF start = CoordinateToPixel(coordinateSegment.Start);
            PointF end = CoordinateToPixel(coordinateSegment.End);
            return new LineSegment(start, end);
        }

        public LineSegment PixelToCoordinate(LineSegment pixelSegment)
        {
            PointF start = PixelToCoordinate(new Point((int)pixelSegment.Start.X, (int)pixelSegment.Start.Y));
            PointF end = PixelToCoordinate(new Point((int)pixelSegment.End.X, (int)pixelSegment.End.Y));
            return new LineSegment(start, end);
        }
    }
}
