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

            _system = new(DrawingField.Size);
            _bitmap = new(DrawingField.Width, DrawingField.Height);
            _graphics = Graphics.FromImage(_bitmap);
            _graphics.SmoothingMode = SmoothingMode.AntiAlias;

            _axisPen = GetAxisPen();
            _meshPen = GetMeshPen();
        }

        private LineSegment[] _meshXSegments;
        private LineSegment[] _meshYSegments;

        private float _sx = 0;
        private float _sy = 0;
        private int _sf = 0;

        private void GraphPanel_Paint(object sender, PaintEventArgs e)
        {
            _graphics.Clear(Color.White);

            //_system.DrawMeshAndNums(_graphics, _meshPen);

            if (_sf % 5 == 0)
            {
                _sx = _system.Scale.Width * 2f;
                _sy = _system.Scale.Height * 2f;
            }

            _meshXSegments = _system.GetMeshXSegments(_sx, _sy);
            _meshYSegments = _system.GetMeshYSegments(_sx, _sy);

            foreach (LineSegment line in _meshXSegments)
                _graphics.DrawLine(_meshPen, line);
            foreach (LineSegment line in _meshYSegments)
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
                catch (Exception) { }
            }

            DrawingField.Image = _bitmap;
        }

        private readonly float _rescalingStep = MathF.Pow(2, 0.2f);

        private void ZoomIn_Click(object sender, EventArgs e)
        {
            _system.Rescale(_system.Scale.Width / _rescalingStep, _system.Scale.Height / _rescalingStep);
            _sf -= 1;
            DrawingField.Invalidate();
        }
        private void ZoomOut_Click(object sender, EventArgs e)
        {
            _system.Rescale(_system.Scale.Width * _rescalingStep, _system.Scale.Height * _rescalingStep);
            _sf += 1;
            DrawingField.Invalidate();
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
