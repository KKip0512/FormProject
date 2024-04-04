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

        public RectangleF Bounds => new(
            (float)-Scale.Width + Position.X, (float)Scale.Height + Position.Y, (float)Scale.Width * 2, (float)-Scale.Height * 2);

        public CoordinatePointConverter Converter { get; }

        public CoordinateSystem(Size sizeOfWindow)
        {
            _scale = new(new SizeF(10f, 10f));
            _position = new(new PointF(0f, 0f));
            _sizeOfWindow = new(sizeOfWindow);
            Converter = new CoordinatePointConverter(_sizeOfWindow, _scale, _position);
        }

        public Point[] GetPointsOfFunction(string expression)
        {
            int stride = 1;
            Point[] points = new Point[SizeOfWindow.Width / stride + 1];
            for (int x = 0; x < SizeOfWindow.Width + stride; x += stride)
            {
                float xCoord = Converter.PixelToCoordinate(new Point(x, 0)).X;
                float yCoord = (float)FunctionCompiler.GetY(expression, xCoord);
                if (float.IsNaN(yCoord)) yCoord = 0f;
                //if (systemY >= Scale) continue;
                points[x / stride] = Converter.CoordinateToPixel(new PointF(xCoord, yCoord));
            }
            return points;
        }

        public void MovePosition(int pixelX, int pixelY)
        {
            float multiplier = 1 / 500f * MathF.Min(Scale.Width, Scale.Height);
            Position = new(Position.X + pixelX * multiplier, Position.Y - pixelY * multiplier);
        }
        public void Rescale(float x, float y) => Scale = new(x, y);

        public LineSegment GetAbcissaSegment()
        {
            Point abscissaStart = Converter.CoordinateToPixel(new PointF(Bounds.Left, 0));
            Point abscissaEnd = Converter.CoordinateToPixel(new PointF(Bounds.Right, 0));
            return new LineSegment(abscissaStart, abscissaEnd);
        }
        public LineSegment GetOrdinateSegment()
        {
            Point ordinateStart = Converter.CoordinateToPixel(new PointF(0, Bounds.Bottom));
            Point ordinateEnd = Converter.CoordinateToPixel(new PointF(0, Bounds.Top));
            return new LineSegment(ordinateStart, ordinateEnd);
        }

        public LineSegment[] GetMeshXSegments(float scaleWidth, float scaleHeight)
        {
            int maxAmountOfSegmentsInOneHalf = 10;
            float distanceBetweenSegments = MathF.Max(scaleWidth, scaleHeight) / maxAmountOfSegmentsInOneHalf;

            float start = distanceBetweenSegments *
                (MathF.Round(Position.X / distanceBetweenSegments) - maxAmountOfSegmentsInOneHalf - 1);

            LineSegment[] meshSegments = new LineSegment[maxAmountOfSegmentsInOneHalf * 2 + 1];
            for (int i = 0; i < meshSegments.Length; i++)
            {
                float x = start + distanceBetweenSegments * (i + 1);
                LineSegment segment = new(new(x, Bounds.Bottom), new(x, Bounds.Top));
                meshSegments[i] = Converter.CoordinateToPixel(segment);
            }
            return meshSegments;
        }

        public LineSegment[] GetMeshYSegments(float scaleWidth, float scaleHeight)
        {
            int maxAmountOfSegmentsInOneHalf = 10;
            float distanceBetweenSegments = MathF.Max(scaleWidth, scaleHeight) / maxAmountOfSegmentsInOneHalf;

            float start = distanceBetweenSegments *
                (MathF.Round(Position.Y / distanceBetweenSegments) - maxAmountOfSegmentsInOneHalf - 1);

            LineSegment[] meshSegments = new LineSegment[maxAmountOfSegmentsInOneHalf * 2 + 1];
            for (int i = 0; i < meshSegments.Length; i++)
            {
                float y = start + distanceBetweenSegments * (i + 1);
                LineSegment segment = new(new(Bounds.Left, y), new(Bounds.Right, y));
                meshSegments[i] = Converter.CoordinateToPixel(segment);
            }
            return meshSegments;
        }

        public Point[] GetCoordinateDesignationXPoints()
        {
            PointF[] nums = new PointF[13];
            float leftSideCoord = MathF.Round(Position.X - Scale.Width);
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = new PointF(leftSideCoord, 0);
                leftSideCoord += MathF.Min(Scale.Width, Scale.Height) / 6;
            }
            Point[] convertedNums = new Point[nums.Length];
            for (int i = 0; i < nums.Length; i++)
                convertedNums[i] = Converter.CoordinateToPixel(nums[i]);
            return convertedNums;
        }
        public Point[] GetCoordinateDesignationYPoints()
        {
            PointF[] nums = new PointF[13];
            float bottomSideCoord = MathF.Round(Position.Y - Scale.Height);
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = new PointF(0, bottomSideCoord);
                bottomSideCoord += MathF.Min(Scale.Width, Scale.Height) / 6;
            }
            Point[] convertedNums = new Point[nums.Length];
            for (int i = 0; i < nums.Length; i++)
                convertedNums[i] = Converter.CoordinateToPixel(nums[i]);
            return convertedNums;
        }
    }
}
