using FormProject.Classes.TextToExpression;

namespace FormProject.Classes
{
    internal sealed class CoordinateSystem
    {
        public Size SizeOfWindow { get; private set; }

        public double Scale { get; set; }
        public Point Position { get; set; }

        public CoordinateSystem(Size sizeOfWindow)
        {
            Scale = 3f;
            Position = new(0);
            SizeOfWindow = sizeOfWindow;
        }

        public Point[] GetPointsOfFunction(Func<double, double> expression)
        {
            int stride = 2;
            Point[] points = new Point[SizeOfWindow.Width / stride + 1];
            for (int x = 0; x < SizeOfWindow.Width + stride; x += stride)
            {
                double systemX = ConvertPixelFromAnyAxisToSystem(x, SizeOfWindow.Width);
                double systemY = expression(systemX);
                //if (systemY >= Scale) continue;
                points[x / stride] = ConvertSystemCoordToPixel(new PointF((float)systemX, (float)systemY));
            }

            return points;
        }
        public Point[] GetPointsOfFunction(string expression)
        {
            int stride = 1;
            Point[] points = new Point[SizeOfWindow.Width / stride + 1];
            try
            {
                for (int x = 0; x < SizeOfWindow.Width + stride; x += stride)
                {
                    double systemX = ConvertPixelFromAnyAxisToSystem(x, SizeOfWindow.Width);
                    double systemY = FunctionCompiler.GetY(expression, systemX);
                    //if (systemY >= Scale) continue;
                    points[x / stride] = ConvertSystemCoordToPixel(new PointF((float)systemX, (float)systemY));
                }
            }
            catch
            { }
            return points;
        }

        public void DrawAxes(Graphics graphics, Pen pen)
        {
            graphics.DrawLine(pen, 0, SizeOfWindow.Height / 2, SizeOfWindow.Width, SizeOfWindow.Height / 2);
            graphics.DrawLine(pen, SizeOfWindow.Width / 2, SizeOfWindow.Height, SizeOfWindow.Width / 2, 0);
        }
        public void DrawMeshAndNums(Graphics graphics, Pen pen)
        {
            Point zeroPos = ConvertSystemCoordToPixel(new(0, 0));
            zeroPos.Offset(6, 6);
            graphics.DrawString(0.ToString(), SystemFonts.DefaultFont,
                Brushes.Black, zeroPos);

            for (double xNegative = -0.5f; xNegative > -Scale; xNegative -= 0.5f)
            {
                graphics.DrawLine(pen,
                    ConvertSystemCoordToPixel(new((float)xNegative, (float)Scale)),
                    ConvertSystemCoordToPixel(new((float)xNegative, (float)-Scale)));

                Point numPos = ConvertSystemCoordToPixel(new((float)xNegative, 0));
                numPos.Offset(6, 6);

                graphics.DrawString(xNegative.ToString(), SystemFonts.DefaultFont,
                    Brushes.Black, numPos);
            }
            for (double xPositive = 0.5f; xPositive < Scale; xPositive += 0.5f)
            {
                graphics.DrawLine(pen,
                    ConvertSystemCoordToPixel(new((float)xPositive, (float)Scale)),
                    ConvertSystemCoordToPixel(new((float)xPositive, (float)-Scale)));

                Point numPos = ConvertSystemCoordToPixel(new((float)xPositive, 0));
                numPos.Offset(6, 6);

                graphics.DrawString(xPositive.ToString(), SystemFonts.DefaultFont,
                    Brushes.Black, numPos);
            }

            for (double yNegative = -0.5f; yNegative > -Scale; yNegative -= 0.5f)
            {
                graphics.DrawLine(pen,
                    ConvertSystemCoordToPixel(new((float)Scale, (float)yNegative)),
                    ConvertSystemCoordToPixel(new((float)-Scale, (float)yNegative)));

                Point numPos = ConvertSystemCoordToPixel(new(0, (float)yNegative));
                numPos.Offset(6, 6);

                graphics.DrawString(yNegative.ToString(), SystemFonts.DefaultFont,
                    Brushes.Black, numPos);
            }
            for (double yPositive = 0.5f; yPositive < Scale; yPositive += 0.5f)
            {
                graphics.DrawLine(pen,
                    ConvertSystemCoordToPixel(new((float)Scale, (float)yPositive)),
                    ConvertSystemCoordToPixel(new((float)-Scale, (float)yPositive)));

                Point numPos = ConvertSystemCoordToPixel(new(0, (float)yPositive));
                numPos.Offset(6, 6);

                graphics.DrawString(yPositive.ToString(), SystemFonts.DefaultFont,
                    Brushes.Black, numPos);
            }
        }

        private int ConvertSystemFromAnyAxisToPixel(double systemPos, int maxPixels)
        {
            return (int)Math.Round((systemPos + Scale) / 2 * maxPixels / Scale);
        }
        private Point ConvertSystemCoordToPixel(PointF systemCoord)
        {
            int x = ConvertSystemFromAnyAxisToPixel(systemCoord.X, SizeOfWindow.Width);
            int y = SizeOfWindow.Height - ConvertSystemFromAnyAxisToPixel(systemCoord.Y, SizeOfWindow.Height);

            return new Point(x, y);
        }

        private double ConvertPixelFromAnyAxisToSystem(int pixelPos, int maxPixels)
        {
            return (double)pixelPos / maxPixels * Scale * 2 - Scale;
        }
        /// <summary>
        /// Мб не работает
        /// </summary>
        /// <param name="pixelCoord"></param>
        /// <returns></returns>
        private PointF ConvertPixelCoordToSystem(Point pixelCoord)
        {
            double x = ConvertPixelFromAnyAxisToSystem(pixelCoord.X, SizeOfWindow.Width);
            double y = SizeOfWindow.Height - ConvertPixelFromAnyAxisToSystem(pixelCoord.Y, SizeOfWindow.Height);

            return new PointF((float)x, (float)y);
        }
    }
}
