using FormProject.Classes.TextToExpression;

namespace FormProject.Classes
{
    internal sealed class CoordinateSystem
    {
        private readonly Reference<Size> _sizeOfWindow;
        private readonly Reference<PointF> _position;
        private readonly Reference<SizeF> _scale;

        public Size SizeOfWindow { get => _sizeOfWindow.Value; private set => _sizeOfWindow.Value = value; }
        public PointF Position { get => _position.Value; private set => _position.Value = value; }
        public SizeF Scale { get => _scale.Value; private set => _scale.Value = value; }

        public PointConverter PointConverter { get; }
        public RectangleF Bounds => new(
            (float)-Scale.Width + Position.X, (float)Scale.Height + Position.Y, (float)Scale.Width * 2, (float)-Scale.Height * 2);

        public CoordinateSystem(Size sizeOfWindow)
        {
            _scale = new(new SizeF(3f, 3f));
            _position = new(new PointF(0f, 0f));
            _sizeOfWindow = new(sizeOfWindow);
            PointConverter = new PointConverter(_sizeOfWindow, _scale, _position);
        }

        public Point[] GetPointsOfFunction(string expression)
        {
            int stride = 1;
            Point[] points = new Point[SizeOfWindow.Width / stride + 1];
            for (int x = 0; x < SizeOfWindow.Width + stride; x += stride)
            {
                float xCoord = PointConverter.PixelPositionToCoordinate(new Point(x, 0)).X;
                float yCoord = (float)FunctionCompiler.GetY(expression, xCoord);
                //if (systemY >= Scale) continue;
                points[x / stride] = PointConverter.CoordinateToPixelPosition(new PointF(xCoord, yCoord));
            }
            return points;
        }

        public void MovePosition(int pixelX, int pixelY)
        {
            float multiplier = 1 / 100f;
            Position = new(Position.X + pixelX * multiplier, Position.Y - pixelY * multiplier);
        }
        public void Rescale(float x, float y) => Scale = new(x, y);

        public (Point, Point) GetAbcissaPoints()
        {
            Point abscissaStart = PointConverter.CoordinateToPixelPosition(new PointF(Bounds.Left, 0));
            Point abscissaEnd = PointConverter.CoordinateToPixelPosition(new PointF(Bounds.Right, 0));
            return (abscissaStart, abscissaEnd);
        }
        public (Point, Point) GetOrdinatePoints()
        {
            Point ordinateStart = PointConverter.CoordinateToPixelPosition(new PointF(0, Bounds.Bottom));
            Point ordinateEnd = PointConverter.CoordinateToPixelPosition(new PointF(0, Bounds.Top));
            return (ordinateStart, ordinateEnd);
        }

        public (Point, Point)[] GetMeshXPoints()
        {

        }
        public (Point, Point)[] GetMeshYPoints()
        {

        }
        public Point[] GetCoordinateDesignationXPoints()
        {
            Point[] points = new Point[Scale.Width * 2];
        }
        public Point[] GetCoordinateDesignationYPoints()
        {
            Point[] points = new Point[Scale * 2];
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
