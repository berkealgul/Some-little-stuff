
using System.Drawing;
using System.Windows.Forms;

namespace Search_Algorithms
{
    public partial class Info : Form
    {
        int X, Y;
        public Info(int X,int Y)
        {
            InitializeComponent();
            this.X = X + 200;
            this.Y = Y + 100;
        }
        
        private void Info_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rec = new Rectangle(10, 10, this.Width - 40, this.Height - 55);
            Pen p = new Pen(Color.Black,4);
            e.Graphics.DrawEllipse(p, rec);
        }

        private void Info_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void label5_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void Info_Load(object sender, System.EventArgs e)
        {
            this.Location = new Point(X, Y);
        }        
    }
}
