using System.Drawing.Drawing2D;

namespace FormProject
{
    public partial class MyForm : Form
    {
        private readonly Pen _pen = new(Color.Black, 3f);

        private readonly Pen _axisPen;
        private readonly Pen _meshPen;

        private readonly CoordinateSystem _system;
        private readonly Bitmap _bitmap;
        private readonly Graphics _graphics;

        public MyForm()
        {
            InitializeComponent();

            DoubleBuffered = true;

            _system = new(GraphDrawingField.Size);
            _bitmap = new(GraphDrawingField.Width, GraphDrawingField.Height);
            _graphics = Graphics.FromImage(_bitmap);
            _graphics.SmoothingMode = SmoothingMode.AntiAlias;

            _axisPen = GetAxisPen();
            _meshPen = GetMeshPen();
        }

        private void GraphPanel_Paint(object sender, PaintEventArgs e)
        {
            _graphics.Clear(Color.White);

            // MathF.Pow(MathF.Abs(MathF.Min(MathF.Abs(MathF.Sin(x * MathF.PI / 2)), 1f - MathF.Abs(x))), 0.5f)
            Point[] points = _system.GetPointsOfFunction(x =>
                MathF.Sin(x * MathF.PI));

            _system.DrawAxes(_graphics, _axisPen);
            _system.DrawMeshAndNums(_graphics, _meshPen);
            _graphics.DrawCurve(_pen, points);

            GraphDrawingField.Image = _bitmap;
        }

        private void ZoomIn_Click(object sender, EventArgs e)
        {
            if (_system.Scale <= 1f) _system.Scale /= 2f;
            else _system.Scale -= 1f;
            GraphDrawingField.Invalidate();
        }
        private void ZoomOut_Click(object sender, EventArgs e)
        {
            if (_system.Scale <= 1f) _system.Scale *= 2f;
            else _system.Scale += 1f;
            GraphDrawingField.Invalidate();
        }

        private void FunctionTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private static Pen GetAxisPen()
        {
            Pen pen;

            GraphicsPath graphicsEndPath = new();
            graphicsEndPath.AddLine(0, 0, -2, -2);
            graphicsEndPath.AddLine(0, 0, 2, -2);

            pen = new(Color.Black, 3f)
            {
                CustomEndCap = new(null, graphicsEndPath)
            };

            return pen;
        }
        private static Pen GetMeshPen()
        {
            Pen pen = new(Color.DarkGray, 1f);
            return pen;
        }
    }
}
