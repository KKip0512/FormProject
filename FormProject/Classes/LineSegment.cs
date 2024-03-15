namespace FormProject.Classes
{
    internal struct LineSegment
    {
        public PointF Start { get; set; }

        public PointF End{ get; set; }

        public readonly float XProjection => MathF.Abs(Start.X - End.X);

        public readonly float YProjection => MathF.Abs(Start.Y - End.Y);

        public readonly float Length => MathF.Sqrt(MathF.Pow(XProjection, 2) + MathF.Pow(YProjection, 2));

        public readonly float Angle => MathF.Atan2(YProjection, XProjection);

        public LineSegment(PointF start, PointF end)
        {
            Start = start;
            End = end;
        }
        public LineSegment(PointF start, float length, float angleInRadians)
        {
            Start = start;
            End = new(start.X + length * MathF.Acos(angleInRadians),
                      start.Y + length * MathF.Asin(angleInRadians));
        }
    }
}