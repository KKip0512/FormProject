using System.Drawing.Drawing2D;
using FormProject.Classes;

namespace FormProject
{
    public partial class MyForm : Form
    {
        private readonly Pen _graphOfFunctionPen = new(Color.Black, 3f);
        private readonly Pen _axisPen;
        private readonly Pen _meshPen;

        private readonly CoordinateSystem _system;
        private readonly Bitmap _bitmap;
        private readonly Graphics _graphics;
        private string? _expression;

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

            //_system.DrawMeshAndNums(_graphics, _meshPen);
            foreach (LineSegment line in _system.GetMeshXSegments())
                _graphics.DrawLine(_meshPen, line);
            foreach (LineSegment line in _system.GetMeshYSegments())
                _graphics.DrawLine(_meshPen, line);

            _graphics.DrawLine(_axisPen, _system.GetAbcissaSegment());
            _graphics.DrawLine(_axisPen, _system.GetOrdinateSegment());

            /*foreach (Point position in _system.GetCoordinateDesignationXPoints())
                _graphics.DrawString("0", SystemFonts.DefaultFont, Brushes.DarkRed, position);
            foreach (Point position in _system.GetCoordinateDesignationYPoints())
                _graphics.DrawString("0", SystemFonts.DefaultFont, Brushes.DarkRed, position);*/

            //Pow(Abs(Min(Abs(Sin(x * PI/2)), 1-Abs(x))), 0.5)
            if (_expression != null)
            {
                try
                {
                    Point[] points = _system.GetPointsOfFunction(_expression);
                    _graphics.DrawLines(_graphOfFunctionPen, points);
                }
                catch { }
            }

            GraphDrawingField.Image = _bitmap;
        }

        private void ZoomIn_Click(object sender, EventArgs e)
        {
            if (MathF.Max(_system.Scale.Width, _system.Scale.Height) <= 1f)
                _system.Rescale(_system.Scale.Width / 2f, _system.Scale.Height / 2f);
            else
                _system.Rescale(_system.Scale.Width - 1f, _system.Scale.Height - 1f);
            GraphDrawingField.Invalidate();
        }
        private void ZoomOut_Click(object sender, EventArgs e)
        {
            if (MathF.Max(_system.Scale.Width, _system.Scale.Height) <= 1f)
                _system.Rescale(_system.Scale.Width * 2f, _system.Scale.Height * 2f);
            else
                _system.Rescale(_system.Scale.Width + 1f, _system.Scale.Height + 1f);
            GraphDrawingField.Invalidate();
        }

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


        private bool _isMouseDown = false;
        private void GraphDrawingField_MouseDown(object sender, MouseEventArgs e)
        {
            _isMouseDown = true;
        }

        private void GraphDrawingField_MouseUp(object sender, MouseEventArgs e)
        {
            _isMouseDown = false;
        }
        private int _oldX;
        private int _oldY;
        private void GraphDrawingField_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown)
                _system.MovePosition(_oldX - e.X, _oldY - e.Y);

            _oldX = e.X;
            _oldY = e.Y;
        }
    }
}
