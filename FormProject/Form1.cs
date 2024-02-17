namespace FormProject
{
    public partial class Form : System.Windows.Forms.Form
    {
        private readonly Pen _pen = new(Color.Black, 3f);
        private readonly CoordinateSystem _system;
        private readonly Bitmap _bitmap;
        private readonly Graphics _graphics;

        public Form()
        {
            InitializeComponent();
            _system = new(GraphPanel.Size);
            _bitmap = new(GraphPanel.Width, GraphPanel.Height);
            _graphics = Graphics.FromImage(_bitmap);
        }

        private void GraphPanel_Paint(object sender, PaintEventArgs e)
        {
            // MathF.Pow(MathF.Abs(MathF.Min(MathF.Abs(MathF.Sin(x * MathF.PI / 2)), 1f - MathF.Abs(x))), 0.5f)

            Point[] points = _system.GetPointsOfFunction(x =>
                MathF.Pow(MathF.Abs(MathF.Min(MathF.Abs(MathF.Sin(x * MathF.PI / 2)), 1f - MathF.Abs(x))), 0.5f));

            _graphics.DrawLines(_pen, points);
            GraphPanel.BackgroundImage = _bitmap;
        }
    }
}
