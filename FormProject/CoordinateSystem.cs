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
            Scale = 3f;
            Position = new(0);
            SizeOfWindow = sizeOfWindow;
        }

        public Point[] GetPointsOfFunction(Func<float, float> function)
        {
            int stride = 2;
            Point[] points = new Point[SizeOfWindow.Width / stride + 1];
            for (int x = 0; x < SizeOfWindow.Width + stride; x+=stride)
            {
                float systemX = ConvertPixelFromAnyAxisToSystem(x, SizeOfWindow.Width);
                float systemY = function(systemX);
                if (systemY > Scale) continue;
                points[x] = ConvertSystemCoordToPixel(new PointF(systemX, systemY));
            }

            return points;
        }

        public void DrawAxes(Graphics graphics, Pen pen)
        {
            graphics.DrawLine(pen, 0, SizeOfWindow.Height/2, SizeOfWindow.Width, SizeOfWindow.Height/2);
            graphics.DrawLine(pen, SizeOfWindow.Width/2, SizeOfWindow.Height, SizeOfWindow.Width / 2, 0);
        }
        public void DrawMeshAndNums(Graphics graphics, Pen pen)
        {
            Point zeroPos = ConvertSystemCoordToPixel(new(0, 0));
            zeroPos.Offset(6, 6);
            graphics.DrawString(0.ToString(), SystemFonts.DefaultFont,
                Brushes.Black, zeroPos);

            for (float xNegative = -0.5f; xNegative > -Scale; xNegative -= 0.5f)
            {
                graphics.DrawLine(pen,
                    ConvertSystemCoordToPixel(new(xNegative, Scale)), ConvertSystemCoordToPixel(new(xNegative, -Scale)));

                Point numPos = ConvertSystemCoordToPixel(new(xNegative, 0));
                numPos.Offset(6, 6);

                graphics.DrawString(xNegative.ToString(), SystemFonts.DefaultFont,
                    Brushes.Black, numPos);
            }
            for (float xPositive = 0.5f; xPositive < Scale; xPositive += 0.5f)
            {
                graphics.DrawLine(pen,
                    ConvertSystemCoordToPixel(new(xPositive, Scale)), ConvertSystemCoordToPixel(new(xPositive, -Scale)));

                Point numPos = ConvertSystemCoordToPixel(new(xPositive, 0));
                numPos.Offset(6, 6);

                graphics.DrawString(xPositive.ToString(), SystemFonts.DefaultFont,
                    Brushes.Black, numPos);
            }

            for (float yNegative = -0.5f; yNegative > -Scale; yNegative -= 0.5f)
            {
                graphics.DrawLine(pen,
                    ConvertSystemCoordToPixel(new(Scale, yNegative)), ConvertSystemCoordToPixel(new(-Scale, yNegative)));

                Point numPos = ConvertSystemCoordToPixel(new(0, yNegative));
                numPos.Offset(6, 6);

                graphics.DrawString(yNegative.ToString(), SystemFonts.DefaultFont,
                    Brushes.Black, numPos);
            }
            for (float yPositive = 0.5f; yPositive < Scale; yPositive += 0.5f)
            {
                graphics.DrawLine(pen,
                    ConvertSystemCoordToPixel(new(Scale, yPositive)), ConvertSystemCoordToPixel(new(-Scale, yPositive)));

                Point numPos = ConvertSystemCoordToPixel(new(0, yPositive));
                numPos.Offset(6, 6);

                graphics.DrawString(yPositive.ToString(), SystemFonts.DefaultFont,
                    Brushes.Black, numPos);
            }
        }

        private int ConvertSystemFromAnyAxisToPixel(float systemPos, int maxPixels)
        {
            return (int)MathF.Round((systemPos + Scale) / 2 * maxPixels / Scale);
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
