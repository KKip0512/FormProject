using FormProject.Classes.TextToExpression;

namespace FormProject.Classes
{
    internal sealed class CoordinateSystem
    {
        public Size SizeOfWindow { get; private set; }

        public double Scale { get; set; }
        public PointF Position { get; private set; }
        //public RectangleF Bounds => new((float)-Scale + Position.X, (float)Scale + Position.Y, (float)Scale, (float)Scale);
        public RectangleF Bounds => new((float)-Scale + Position.X, (float)Scale + Position.Y, (float)Scale * 2, (float)-Scale * 2);

        public CoordinateSystem(Size sizeOfWindow)
        {
            Scale = 3d;
            Position = new PointF(0f, 0f);
            SizeOfWindow = sizeOfWindow;
        }

        public void MovePosition(int pixelX, int pixelY)
        {
            float multiplier = 1 / 100f;
            PointF a = new(pixelX * multiplier, pixelY * multiplier);
            PointF oldPositon = Position;
            oldPositon.X += a.X;
            oldPositon.Y -= a.Y;
            Position = oldPositon;
        }

        /*public Point[] GetPointsOfFunction(Func<double, double> expression)
        {
            int stride = 2;
            Point[] points = new Point[SizeOfWindow.Width / stride + 1];
            for (int x = 0; x < SizeOfWindow.Width + stride; x += stride)
            {
                double systemX = ConvertPixelAxisToSystem(x, SizeOfWindow.Width);
                double systemY = expression(systemX);
                //if (systemY >= Scale) continue;
                points[x / stride] = ConvertSystemCoordToPixel(new PointF((float)systemX, (float)systemY));
            }

            return points;
        }*/
        public Point[] GetPointsOfFunction(string expression)
        {
            int stride = 1;
            Point[] points = new Point[SizeOfWindow.Width / stride + 1];
            try
            {
                for (int x = 0; x < SizeOfWindow.Width + stride; x += stride)
                {
                    double systemX = ConvertPixelCoordToSystem(new Point(x, 0)).X;
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
            Point abscissaStart = ConvertSystemCoordToPixel(new PointF(Bounds.Left, 0));
            Point abscissaEnd = ConvertSystemCoordToPixel(new PointF(Bounds.Right, 0));
            Point ordinateStart = ConvertSystemCoordToPixel(new PointF(0, Bounds.Bottom));
            Point ordinateEnd = ConvertSystemCoordToPixel(new PointF(0, Bounds.Top));
            graphics.DrawLine(pen, abscissaStart, abscissaEnd);
            graphics.DrawLine(pen, ordinateStart, ordinateEnd);
        }
        public void DrawMeshAndNums(Graphics graphics, Pen pen)
        {
            Point centerPos = ConvertSystemCoordToPixel(new(0, 0));
            centerPos.Offset(6, 6);
            graphics.DrawString(0.ToString(), SystemFonts.DefaultFont,
                Brushes.Black, centerPos);

            for (float xNegative = Position.X; xNegative > Bounds.Left; xNegative -= 0.5f)
            {
                graphics.DrawLine(pen,
                    ConvertSystemCoordToPixel(new(xNegative, Bounds.Top)),
                    ConvertSystemCoordToPixel(new(xNegative, Bounds.Bottom)));

                Point numPos = ConvertSystemCoordToPixel(new(xNegative, 0));
                numPos.Offset(6, 6);

                graphics.DrawString(xNegative.ToString("0.##"), SystemFonts.DefaultFont,
                    Brushes.Black, numPos);
            }
            for (float xPositive = Position.X; xPositive < Bounds.Right; xPositive += 0.5f)
            {
                graphics.DrawLine(pen,
                    ConvertSystemCoordToPixel(new(xPositive, Bounds.Top)),
                    ConvertSystemCoordToPixel(new(xPositive, Bounds.Bottom)));

                Point numPos = ConvertSystemCoordToPixel(new(xPositive, 0));
                numPos.Offset(6, 6);

                graphics.DrawString(xPositive.ToString("0.##"), SystemFonts.DefaultFont,
                    Brushes.Black, numPos);
            }

            for (float yNegative = Position.Y; yNegative > Bounds.Bottom; yNegative -= 0.5f)
            {
                graphics.DrawLine(pen,
                    ConvertSystemCoordToPixel(new(Bounds.Left, yNegative)),
                    ConvertSystemCoordToPixel(new(Bounds.Right, yNegative)));

                Point numPos = ConvertSystemCoordToPixel(new(0, yNegative));
                numPos.Offset(6, 6);

                graphics.DrawString(yNegative.ToString("0.##"), SystemFonts.DefaultFont,
                    Brushes.Black, numPos);
            }
            for (float yPositive = Position.Y; yPositive < Bounds.Top; yPositive += 0.5f)
            {
                graphics.DrawLine(pen,
                    ConvertSystemCoordToPixel(new(Bounds.Left, yPositive)),
                    ConvertSystemCoordToPixel(new(Bounds.Right, yPositive)));

                Point numPos = ConvertSystemCoordToPixel(new(0, yPositive));
                numPos.Offset(6, 6);

                graphics.DrawString(yPositive.ToString("0.##"), SystemFonts.DefaultFont,
                    Brushes.Black, numPos);
            }
        }

        private Point ConvertSystemCoordToPixel(PointF systemCoord)
        {
            int x = ConvertSystemAxisToPixel(systemCoord.X - Position.X, SizeOfWindow.Width);
            int y = SizeOfWindow.Height - ConvertSystemAxisToPixel(systemCoord.Y - Position.Y, SizeOfWindow.Height);

            return new Point(x, y);
        }
        private int ConvertSystemAxisToPixel(double systemPos, int maxPixels)
        {
            return (int)Math.Round((systemPos + Scale) / 2 * maxPixels / Scale);
        }

        /// <summary>
        /// Мб не работает
        /// </summary>
        /// <param name="pixelCoord"></param>
        /// <returns></returns>
        private PointF ConvertPixelCoordToSystem(Point pixelCoord)
        {
            float x = (float)ConvertPixelAxisToSystem(pixelCoord.X, SizeOfWindow.Width) + Position.X;
            float y = (float)(SizeOfWindow.Height - ConvertPixelAxisToSystem(pixelCoord.Y, SizeOfWindow.Height) + Position.Y);

            return new PointF(x, y);
        }
        private double ConvertPixelAxisToSystem(int pixelPos, int maxPixels)
        {
            return (double)pixelPos / maxPixels * Scale * 2 - Scale;
        }
    }
}
