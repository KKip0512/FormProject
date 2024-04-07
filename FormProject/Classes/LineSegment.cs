namespace FormProject.Classes
{
    internal struct LineSegment
    {
        public PointF Start { get; set; }

        public PointF End{ get; set; }

        public readonly float XProjectionLength => MathF.Abs(Start.X - End.X);

        public readonly float YProjectionLenth => MathF.Abs(Start.Y - End.Y);

        public readonly float Length => MathF.Sqrt(MathF.Pow(XProjectionLength, 2) + MathF.Pow(YProjectionLenth, 2));

        public readonly float Angle => MathF.Atan2(YProjectionLenth, XProjectionLength);

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