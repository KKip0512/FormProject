using System.Drawing.Drawing2D;
using System.Globalization;

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

        public static readonly NumberFormatInfo numberFormatInfo = new()
        {
            NumberDecimalSeparator = "."
        };

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

            // Math.Pow(Math.Abs(Math.Min(Math.Abs(Math.Sin(x * Math.PI / 2)), 1f - Math.Abs(x))), 0.5f)

            //_expression = "Pow(Abs(Min(Abs(Sin(x * 3.14 / 2)), 1 - Abs(x))), 0.5)";
            //_expression = "2 * sin(x)";
            _system.DrawMeshAndNums(_graphics, _meshPen);
            _system.DrawAxes(_graphics, _axisPen);

            Point[] points;
            if (_expression != null)
            {
                points = _system.GetPointsOfFunction(_expression);
                _graphics.DrawLines(_pen, points);
            }
            //_graphics.DrawString(_expression, SystemFonts.DefaultFont, Brushes.Violet, new Point(10, 10));
            try
            {

            _graphics.DrawString(MathExpression.Calculate(FunctionTextBox.Text).ToString(), SystemFonts.DefaultFont, Brushes.Violet, new Point(10, 10));
            }
            catch {}

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

        private string? _expression;
        private void FunctionTextBox_TextChanged(object sender, EventArgs e)
        {
            _expression = FunctionTextBox.Text;
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
