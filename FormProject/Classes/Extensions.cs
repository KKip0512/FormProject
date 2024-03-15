namespace FormProject.Classes
{
    internal static class GraphicsExtension
    {
        public static void DrawLine(this Graphics graphics, Pen pen, LineSegment segment)
        {
            graphics.DrawLine(pen, segment.Start, segment.End);
        }
    }
}