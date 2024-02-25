using FormProject.Classes.TextToExpression;
using Size = FormProject.Classes.Emplacement.Size;
using SizeF = FormProject.Classes.Emplacement.SizeF;
using PointF = FormProject.Classes.Emplacement.PointF;
using PointFS = System.Drawing.PointF;
using System.Drawing;

namespace FormProject.Classes
{
    internal sealed class CoordinateSystem
    {
        public Size SizeOfWindow { get; private set; }
        public PointF Position { get; private set; }
        public SizeF Scale { get; private set; }

        public PointConverter PointConverter { get; }
        public RectangleF Bounds => new((float)-Scale.Width + Position.X, (float)Scale.Height + Position.Y, (float)Scale.Width * 2, (float)-Scale.Height * 2);

        public CoordinateSystem(System.Drawing.Size sizeOfWindow)
        {
            Scale = new SizeF(3f, 3f);
            Position = new PointF(0f, 0f);
            SizeOfWindow = new(sizeOfWindow);
            PointConverter = new PointConverter(SizeOfWindow, Scale, Position);
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

        public Point[] GetPointsOfFunction(string expression)
        {
            int stride = 1;
            Point[] points = new Point[SizeOfWindow.Width / stride + 1];
            try
            {
                for (int x = 0; x < SizeOfWindow.Width + stride; x += stride)
                {
                    float xCoord = PixelPositionToCoordinate(new Point(x, 0)).X;
                    float yCoord = (float)FunctionCompiler.GetY(expression, xCoord);
                    //if (systemY >= Scale) continue;
                    points[x / stride] = CoordinateToPixelPosition(new PointFS(xCoord, yCoord));
                }
            }
            catch { }
            return points;
        }




        public Point CoordinateToPixelPosition(PointFS coordinate)
        {
            int x = (int)Math.Round(
                (coordinate.X - Position.X + Scale.Width) / 2 * SizeOfWindow.Width / Scale.Width);

            int y = (int)(SizeOfWindow.Height - Math.Round(
                (coordinate.Y - Position.Y + Scale.Height) / 2 * SizeOfWindow.Height / Scale.Height));

            return new Point(x, y);
        }

        // PixelPositionToCoordinate != ConvertPixelCoordToSystem И Я НЕ ПОНИМАЮ ПОЧЕМУ
        public PointFS PixelPositionToCoordinate(Point pixelPosition)
        {
            float x = (pixelPosition.X / SizeOfWindow.Width * Scale.Width * 2 - Scale.Width) + Position.X;
            float y = (SizeOfWindow.Height - (pixelPosition.Y / SizeOfWindow.Height * Scale.Height * 2 - Scale.Height)) + Position.Y;

            return new PointFS(x, y);
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
        }







        public void Rescale(float x ,float y) => Scale = new(x, y);

        public void DrawAxes(Graphics graphics, Pen pen)
        {
            Point abscissaStart = PointConverter.CoordinateToPixelPosition(new PointFS(Bounds.Left, 0));
            Point abscissaEnd = PointConverter.CoordinateToPixelPosition(new PointFS(Bounds.Right, 0));
            Point ordinateStart = PointConverter.CoordinateToPixelPosition(new PointFS(0, Bounds.Bottom));
            Point ordinateEnd = PointConverter.CoordinateToPixelPosition(new PointFS(0, Bounds.Top));
            graphics.DrawLine(pen, abscissaStart, abscissaEnd);
            graphics.DrawLine(pen, ordinateStart, ordinateEnd);
        }
        public void DrawMeshAndNums(Graphics graphics, Pen pen)
        {
            Point centerPos = PointConverter.CoordinateToPixelPosition(new(0, 0));
            centerPos.Offset(6, 6);
            graphics.DrawString(0.ToString(), SystemFonts.DefaultFont,
                Brushes.Black, centerPos);

            for (float xNegative = Position.X; xNegative > Bounds.Left; xNegative -= 0.5f)
            {
                graphics.DrawLine(pen,
                    PointConverter.CoordinateToPixelPosition(new(xNegative, Bounds.Top)),
                    PointConverter.CoordinateToPixelPosition(new(xNegative, Bounds.Bottom)));

                Point numPos = PointConverter.CoordinateToPixelPosition(new(xNegative, 0));
                numPos.Offset(6, 6);

                graphics.DrawString(xNegative.ToString("0.##"), SystemFonts.DefaultFont,
                    Brushes.Black, numPos);
            }
            for (float xPositive = Position.X; xPositive < Bounds.Right; xPositive += 0.5f)
            {
                graphics.DrawLine(pen,
                    PointConverter.CoordinateToPixelPosition(new(xPositive, Bounds.Top)),
                    PointConverter.CoordinateToPixelPosition(new(xPositive, Bounds.Bottom)));

                Point numPos = PointConverter.CoordinateToPixelPosition(new(xPositive, 0));
                numPos.Offset(6, 6);

                graphics.DrawString(xPositive.ToString("0.##"), SystemFonts.DefaultFont,
                    Brushes.Black, numPos);
            }

            for (float yNegative = Position.Y; yNegative > Bounds.Bottom; yNegative -= 0.5f)
            {
                graphics.DrawLine(pen,
                    PointConverter.CoordinateToPixelPosition(new(Bounds.Left, yNegative)),
                    PointConverter.CoordinateToPixelPosition(new(Bounds.Right, yNegative)));

                Point numPos = PointConverter.CoordinateToPixelPosition(new(0, yNegative));
                numPos.Offset(6, 6);

                graphics.DrawString(yNegative.ToString("0.##"), SystemFonts.DefaultFont,
                    Brushes.Black, numPos);
            }
            for (float yPositive = Position.Y; yPositive < Bounds.Top; yPositive += 0.5f)
            {
                graphics.DrawLine(pen,
                    PointConverter.CoordinateToPixelPosition(new(Bounds.Left, yPositive)),
                    PointConverter.CoordinateToPixelPosition(new(Bounds.Right, yPositive)));

                Point numPos = PointConverter.CoordinateToPixelPosition(new(0, yPositive));
                numPos.Offset(6, 6);

                graphics.DrawString(yPositive.ToString("0.##"), SystemFonts.DefaultFont,
                    Brushes.Black, numPos);
            }
        }
    }
}
