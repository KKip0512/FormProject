namespace FormProject
{
    public partial class Form1 : Form
    {
        private Graphics g;
        private Point _click1;
        private bool _flag = true;

        public Form1()
        {
            InitializeComponent();
            g = CreateGraphics();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            g.Clear(Color.Magenta);
            g.DrawEllipse(Pens.LimeGreen, 0, 0, Width, Height);
            g.DrawLine(Pens.Tan, 100, 100, 400, 400);
            //g.DrawPolygon();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (_flag)
            {
                _click1 = e.Location;
                _flag = false;
            }
            else
            {
                g.DrawRectangle(Pens.White, _click1.X, _click1.Y, e.Location.X, e.Location.Y);
                _flag = true;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
