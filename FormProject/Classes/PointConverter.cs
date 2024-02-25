using Size = FormProject.Classes.Emplacement.Size;
using SizeF = FormProject.Classes.Emplacement.SizeF;
using PointF = FormProject.Classes.Emplacement.PointF;
using PointFS = System.Drawing.PointF;

namespace FormProject.Classes
{
    internal class PointConverter(Size sizeOfWindow, SizeF scale, PointF systemCenterCoordinate)
    {
        public Size SizeOfWindow { get; } = sizeOfWindow;
        public SizeF Scale { get; } = scale;
        public PointF SystemCenterCoordinate { get; } = systemCenterCoordinate;



        public Point CoordinateToPixelPosition(PointFS coordinate)
        {
            int x = (int)Math.Round(
                (coordinate.X - SystemCenterCoordinate.X + Scale.Width) / 2 * SizeOfWindow.Width / Scale.Width);

            int y = (int)(SizeOfWindow.Height - Math.Round(
                (coordinate.Y - SystemCenterCoordinate.Y + Scale.Height) / 2 * SizeOfWindow.Height / Scale.Height));

            return new Point(x, y);
        }

        public PointFS PixelPositionToCoordinate(Point pixelPosition)
        {
            float x = SystemCenterCoordinate.X + (pixelPosition.X / SizeOfWindow.Width * Scale.Width * 2 - Scale.Width);
            float y = SizeOfWindow.Height - SystemCenterCoordinate.Y +
                (pixelPosition.Y / SizeOfWindow.Height * Scale.Height * 2 - Scale.Height);

            return new PointFS(x, y);
        }

        /*private Point ConvertSystemCoordToPixel(PointFS systemCoord)
        {
            int x = ConvertSystemAxisToPixel(systemCoord.X - Position.X, SizeOfWindow.Width);
            int y = SizeOfWindow.Height - ConvertSystemAxisToPixel(systemCoord.Y - Position.Y, SizeOfWindow.Height);

            return new Point(x, y);
        }
        private int ConvertSystemAxisToPixel(double systemPos, int maxPixels)
        {
            return (int)Math.Round((systemPos + Scale.Width) / 2 * maxPixels / Scale.Width);
        }

        private PointFS ConvertPixelCoordToSystem(Point pixelCoord)
        {
            float x = (float)ConvertPixelAxisToSystem(pixelCoord.X, SizeOfWindow.Width) + Position.X;
            float y = (float)(SizeOfWindow.Height - ConvertPixelAxisToSystem(pixelCoord.Y, SizeOfWindow.Height) + Position.Y);

            return new PointFS(x, y);
        }
        private double ConvertPixelAxisToSystem(int pixelPos, int maxPixels)
        {
            return (double)pixelPos / maxPixels * Scale.Width * 2 - Scale.Width;
        }*/
    }
}
