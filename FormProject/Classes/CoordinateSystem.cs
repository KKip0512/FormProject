using FormProject.Classes.TextToExpression;
using System.Collections.Generic;

namespace FormProject.Classes
{
    internal sealed class CoordinateSystem
    {
        private readonly Reference<Size> _sizeOfWindow;
        private readonly Reference<PointF> _position;
        private readonly Reference<SizeF> _scale;
        private readonly SizeF _maxScale;

        public Size SizeOfWindow { get => _sizeOfWindow.Value; private set => _sizeOfWindow.Value = value; }
        public PointF Position { get => _position.Value; private set => _position.Value = value; }
        public SizeF Scale { get => _scale.Value; private set => _scale.Value = value; }
        public SizeF MaxScale { get => _maxScale; }

        public RectangleF Bounds => new(
            (float)-Scale.Width + Position.X, (float)Scale.Height + Position.Y, (float)Scale.Width * 2, (float)-Scale.Height * 2);

        public CoordinatePointConverter Converter { get; }

        public CoordinateSystem(Size sizeOfWindow)
        {
            _maxScale = new SizeF(100000f, 100000f);
            _scale = new(new SizeF(10f, 10f));
            _position = new(new PointF(0f, 0f));
            _sizeOfWindow = new(sizeOfWindow);
            Converter = new CoordinatePointConverter(_sizeOfWindow, _scale, _position);
        }

        public List<List<PointF>> GetPointsOfFunction(string expression)
        {
            List<PointF> unformattedPoints = GetUnformattedPointsOfFunction(expression);
            return GetFormattedPointsOfFunction(unformattedPoints);
        }
        private List<PointF> GetUnformattedPointsOfFunction(string expression)
        {
            float offset = MathF.Min(Scale.Width, Scale.Height) / MathF.Max(SizeOfWindow.Width, SizeOfWindow.Height);
            //offset = 0.02f;
            List<PointF> points = [];

            for (float y, x = Bounds.Left - 1; x <= Bounds.Right + 1; x += offset)
            {
                x += offset; // perhaps some calculations are needed here
                y = (float)FunctionCompiler.GetY(expression, x);
                points.Add(new(x, y));
            }

            return points;
        }
        private List<List<PointF>> GetFormattedPointsOfFunction(List<PointF> points)
        {
            List<List<PointF>> result = [[]];
            float yInaccuracy = 10f;

            for (int i = 0; i < points.Count; i++)
                if (MathF.Abs(points[i].Y) > MaxScale.Height)
                    points[i] = new(points[i].X, MaxScale.Height * MathF.Sign(points[i].Y));

            for (int i = 1; i < points.Count - 1; i++)
            {
                if ((points[i].Y - points[i - 1].Y > yInaccuracy &&
                    points[i].Y - points[i + 1].Y > yInaccuracy) ||
                    (points[i].Y - points[i - 1].Y < -yInaccuracy &&
                    points[i].Y - points[i + 1].Y < -yInaccuracy))
                {
                    result.Last().Add(new(points[i].X, points[i].Y < 0 ? -MaxScale.Height : MaxScale.Height));
                    result.Add([new(points[i + 1].X, points[i + 1].Y < 0 ? -MaxScale.Height : MaxScale.Height)]);
                    i++;
                }
                else if (float.IsNaN(points[i].Y)) result.Add([]);
                else result.Last().Add(points[i]);
            }
            return result;
        }

        public void MovePosition(int pixelX, int pixelY)
        {
            float multiplier = 1 / 500f * MathF.Min(Scale.Width, Scale.Height);
            Position = new(Position.X + pixelX * multiplier, Position.Y - pixelY * multiplier);
        }
        public void Rescale(float wigth, float height) => Scale = new(wigth, height);

        public LineSegment GetAbcissaSegment()
        {
            PointF abscissaStart = new(Bounds.Left, 0);
            PointF abscissaEnd = new(Bounds.Right, 0);
            return new LineSegment(abscissaStart, abscissaEnd);
        }
        public LineSegment GetOrdinateSegment()
        {
            PointF ordinateStart = new(0, Bounds.Bottom);
            PointF ordinateEnd = new(0, Bounds.Top);
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
                meshSegments[i] = segment;
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
                meshSegments[i] = segment;
            }
            return meshSegments;
        }

        public PointF[] GetCoordinateDesignationXPoints(LineSegment[] meshXSegments)
        {
            PointF[] points = new PointF[meshXSegments.Length];
            for (int i = 0; i < meshXSegments.Length; i++)
                points[i] = new(meshXSegments[i].Start.X, new PointF(0f, 0f).Y);

            return points;
        }

        public PointF[] GetCoordinateDesignationYPoints(LineSegment[] meshYSegments)
        {
            PointF[] points = new PointF[meshYSegments.Length];
            for (int i = 0; i < meshYSegments.Length; i++)
                points[i] = new(new PointF(0f, 0f).X, meshYSegments[i].Start.Y);

            return points;
        }
    }
}
