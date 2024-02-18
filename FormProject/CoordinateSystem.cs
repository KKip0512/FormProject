using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormProject
{
    internal sealed class CoordinateSystem
    {
        public Size SizeOfWindow { get; private set; }

        public float Scale { get; set; }
        public Point Position { get; set; }

        public CoordinateSystem(Size sizeOfWindow)
        {
            Scale = 10f;
            Position = new(0);
            SizeOfWindow = sizeOfWindow;
        }

        public Point[] GetPointsOfFunction(Func<float, float> function)
        {
            Point[] points = new Point[SizeOfWindow.Width];
            for (int x = 0; x < SizeOfWindow.Width; x++)
            {
                float systemX = ConvertPixelFromAnyAxisToSystem(x, SizeOfWindow.Width);
                float systemY = function(systemX);
                if (systemY > Scale) continue;
                points[x] = ConvertSystemCoordToPixel(new PointF(systemX, systemY));
            }

            return points;
        }

        private int ConvertSystemFromAnyAxisToPixel(float systemPos, int maxPixels)
        {
            return (int)((systemPos + Scale) / 2 * maxPixels / Scale);
        }
        private Point ConvertSystemCoordToPixel(PointF systemCoord)
        {
            int x = ConvertSystemFromAnyAxisToPixel(systemCoord.X, SizeOfWindow.Width);
            int y = SizeOfWindow.Height - ConvertSystemFromAnyAxisToPixel(systemCoord.Y, SizeOfWindow.Height);

            return new Point(x, y);
        }

        private float ConvertPixelFromAnyAxisToSystem(int pixelPos, int maxPixels)
        {
            return (float)pixelPos / maxPixels * Scale * 2 - Scale;
        }
        /// <summary>
        /// Мб не работает
        /// </summary>
        /// <param name="pixelCoord"></param>
        /// <returns></returns>
        private PointF ConvertPixelCoordToSystem(Point pixelCoord)
        {
            float x = ConvertPixelFromAnyAxisToSystem(pixelCoord.X, SizeOfWindow.Width);
            float y = SizeOfWindow.Height - ConvertPixelFromAnyAxisToSystem(pixelCoord.Y, SizeOfWindow.Height);

            return new PointF(x, y);
        }
    }
}
