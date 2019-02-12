using System;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using Matris;

namespace tetris
{
    public partial class Tetris : Form
    {
        public Tetris()
        {
            InitializeComponent();
        }

        private void Tetris_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.DrawRectangle(new Pen(Color.Black, 2), new Rectangle(panel1.Location.X, panel1.Location.Y, panel1.Width, panel1.Height));
            g.DrawRectangle(new Pen(Color.Black, 2), new Rectangle(panel2.Location.X, panel2.Location.Y, panel2.Width, panel2.Height));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            if (!GameLogic.IsBusy)
                GameLogic.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Visible = false;
            LoadComponents();
            GameLogic.RunWorkerAsync();
        }

        private void Tetris_KeyDown(object sender, KeyEventArgs e)
        {
            if (GameLogic.IsBusy)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        keypressed = true;
                        dir = -1;
                        break;
                    case Keys.D:
                        keypressed = true;
                        dir = 1;
                        break;
                    case Keys.R:
                        keypressed = true;
                        rotate = true;
                        break;
                    case Keys.W:
                        keypressed = true;
                        fall = true;
                        break;
                    case Keys.F1:
                        _break = true;
                        break;
                }
            }
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void Tetris_Load(object sender, EventArgs e)
        {
            LoadComponents();
        }

        void LoadComponents()
        {
            g = panel1.CreateGraphics();
            g2 = panel2.CreateGraphics();
            p = new Pen(Color.Black, 2);
            r = new Random();
            w = panel1.Width / 20;
            h = panel1.Height / 20;
            clrs = new Color[w, h];
            filled = new bool[w, h];
            started = true;
            dir = 0;
            score = 0;
            sb = new SolidBrush(Color.Black);
            sleeprate = 130;
            matris = new int[,]{ { 0, -1 },
                                 { 1,  0 } };
            _break = false;
            initiate = false;
            fall = false;
            rotate = false;
            keypressed = false;
            seated = false;
        }

        bool started, initiate, fall, rotate, _break, seated, keypressed;
        int[,] matris;
        bool[,] filled;
        Color[,] clrs;
        int w, h, dir, score, sleeprate;
        Graphics g, g2;
        Pen p;
        Random r;
        Shape CurrentShp, NextShp;
        SolidBrush sb;
        delegate void UIUpdater();

        private void GameLogic_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SoundPlayer sp = new SoundPlayer(@"C:\Users\samed\source\repos\tetris\tetris\Ses\You Failed TF2 Sound Effect.wav"); // @"C:\Users\samed\source\repos\tetris\tetris\Ses\You Failed TF2 Sound Effect.wav"
            try
            {
                sp.Play();
                puan.Text = "00000";
            }
            catch
            {

            }
            MessageBox.Show("Kaybettiniz \nPuanınız: \n" + score);
            Refresh();
            button2.Visible = true;

        } // executed when game is over


        private void GameLogic_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    clrs[x, y] = Color.Gray;
                }
            }
            Initialization();

            while (true)
            {
            Movements:
                InstantFall();
                MoveLR();
                Rotate();
                MoveDown();
                Render();

                Thread.Sleep(sleeprate);

                if (seated)
                {
                    Thread.Sleep(50);
                    if (keypressed)
                    {
                        keypressed = false;
                        seated = false;
                        goto Movements;
                    }
                    for (int i = 0; i < CurrentShp.points.Length; i++)
                    {
                        filled[CurrentShp.points[i].X / 20, CurrentShp.points[i].Y / 20] = true;
                    }
                    initiate = true;
                    seated = false;
                }
                if (initiate)
                {
                    initiate = false;
                    Initialization();
                }
                if (_break)
                    break;
                if (!keypressed)
                    keypressed = false;
                ControlForCollapse();
            }
        } //main loop

        void ControlForCollapse()
        {
            int collapseCombo = 0;
            int num = 0;
            int index = 0;

            for (int y = h - 1; y >= 0; y--)
            {
                for (int x = 0; x < w; x++)
                {
                    if (filled[x, y])
                        num++;
                }

                if (num == w)
                {
                    collapseCombo++;
                    index = y;
                }
                num = 0;
            }

            if (collapseCombo != 0)
            {
                for (int y = index; y >= 0; y--)
                {
                    for (int x = 0; x < w; x++)
                    {
                        if (y - collapseCombo < 0)
                        {
                            clrs[x, y] = Color.Gray;
                            filled[x, y] = false;
                        }
                        else
                        {
                            clrs[x, y] = clrs[x, y - 1];
                            filled[x, y] = filled[x, y -1];
                        }
                    }
                }
                score += 100 * collapseCombo;
                if (score % 500 == 0 && sleeprate >= 50)
                    sleeprate -= 10;
                UIUpdater scoreUpdate = new UIUpdater(UpdateScoreBoard);
                Invoke(scoreUpdate);
            }
        }

        void Render()
        {
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (clrs[x, y] == Color.Gray)
                        g.FillRectangle(new SolidBrush(clrs[x, y]), new Rectangle(x * 20, y * 20, 20, 20));
                    else
                    {
                        g.FillRectangle(new SolidBrush(clrs[x, y]), new Rectangle(x * 20, y * 20, 20, 20));
                        g.DrawRectangle(p, new Rectangle(x * 20, y * 20, 20, 20));
                    }
                }
            }
        }

        void MoveLR()
        {
            int CanMove = 0;

            if (dir == 0)
                return;

            for (int i = 0; i < CurrentShp.points.Length; i++)
            {
                if ((CurrentShp.points[i].X == 0 && dir == -1) || (CurrentShp.points[i].X == (w - 1) * 20 && dir == 1))
                    break;
                if (!filled[(CurrentShp.points[i].X + (20 * dir)) / 20, CurrentShp.points[i].Y / 20])
                    CanMove++;
            }

            if (CanMove == 4)
            {
                for (int i = 0; i < CurrentShp.points.Length; i++)
                {
                    clrs[CurrentShp.points[i].X / 20, CurrentShp.points[i].Y / 20] = Color.Gray;
                    CurrentShp.points[i].X += 20 * dir;
                }

                for (int i = 0; i < CurrentShp.points.Length; i++)
                {
                    clrs[CurrentShp.points[i].X / 20, CurrentShp.points[i].Y / 20] = CurrentShp.clr;
                }
                CurrentShp.massC.X += 20 * dir;
            }
            dir = 0;
        }

        void MoveDown()
        {
            int count = 0; ;

            for (int i = 0; i < CurrentShp.points.Length; i++)
            {
                if ((CurrentShp.points[i].Y + 20) / 20 != h)
                {
                    if (!filled[CurrentShp.points[i].X / 20, (CurrentShp.points[i].Y + 20) / 20])
                        count++;
                }
            }

            if (count == 4)
            {
                for (int i = 0; i < CurrentShp.points.Length; i++)
                {
                    clrs[CurrentShp.points[i].X / 20, CurrentShp.points[i].Y / 20] = Color.Gray;
                    CurrentShp.points[i].Y += 20;
                }

                for (int i = 0; i < CurrentShp.points.Length; i++)
                {
                    clrs[CurrentShp.points[i].X / 20, CurrentShp.points[i].Y / 20] = CurrentShp.clr;
                }
                CurrentShp.massC.Y += 20;
            }
            else
            {
                seated = true;
            }
        }

        void InstantFall()
        {
            if (!fall)
                return;

            int c = 0;
            bool done = false;

            for (int i = 0; i < CurrentShp.points.Length; i++)
                clrs[CurrentShp.points[i].X / 20, CurrentShp.points[i].Y / 20] = Color.Gray;

            while (!done)
            {
                for (int i = 0; i < CurrentShp.points.Length; i++)
                {
                    if ((CurrentShp.points[i].Y + 20) / 20 != h)
                    {
                        if (!filled[CurrentShp.points[i].X / 20, (CurrentShp.points[i].Y + 20) / 20])
                            c++;
                    }
                }

                if (c == 4)
                {
                    for (int i = 0; i < CurrentShp.points.Length; i++)
                        CurrentShp.points[i].Y += 20;
                }
                else
                {
                    for (int i = 0; i < CurrentShp.points.Length; i++)
                    {
                        filled[CurrentShp.points[i].X / 20, CurrentShp.points[i].Y / 20] = true;
                        clrs[CurrentShp.points[i].X / 20, CurrentShp.points[i].Y / 20] = CurrentShp.clr;
                    }
                    done = true;
                }
                c = 0;
            }
            fall = false;
        }

        void Rotate()
        {
            if (!rotate || CurrentShp.clr == Color.Yellow)
                return;

            int CanRotate = 0;
            Point[] temp = new Point[4];

            Point pbase = CurrentShp.massC;

            for (int i = 0; i < CurrentShp.points.Length; i++)
            {
                temp[i] = MatrisÇarpma(new Point((CurrentShp.points[i].X - pbase.X), (CurrentShp.points[i].Y - pbase.Y)), matris);
                temp[i].X += pbase.X;
                temp[i].Y += pbase.Y;

                if (temp[i].X / 20 < 0 || temp[i].X / 20 >= w || temp[i].Y / 20 < 0 || temp[i].Y / 20 >= h || filled[temp[i].X / 20, temp[i].Y / 20])
                    continue;
                else
                    CanRotate++;
            }

            if (CanRotate == 4)
            {
                for (int i = 0; i < CurrentShp.points.Length; i++)
                {
                    clrs[CurrentShp.points[i].X / 20, CurrentShp.points[i].Y / 20] = Color.Gray;
                    CurrentShp.points[i] = temp[i];
                }

                for (int i = 0; i < CurrentShp.points.Length; i++)
                {
                    clrs[CurrentShp.points[i].X / 20, CurrentShp.points[i].Y / 20] = CurrentShp.clr;
                }
            }
            rotate = false;
        }

        void Initialization()
        {
            if (started)
            {
                CurrentShp = ChoseShp(r);
                NextShp = ChoseShp(r);
                Add(CurrentShp);
                NextP();
                started = false;
            }
            else
            {
                CurrentShp = NextShp;
                Add(CurrentShp);
                NextShp = ChoseShp(r);
                NextP();
            }
        }

        void Add(Shape shp)
        {
            int count = 0;
            for (int i = 0; i < shp.points.Length; i++)
            {
                if (!filled[shp.points[i].X / 20, shp.points[i].Y / 20])
                    count++;
            }

            if (count == 4)
            {
                for (int i = 0; i < shp.points.Length; i++)
                {
                    clrs[shp.points[i].X / 20, shp.points[i].Y / 20] = shp.clr;
                }
            }
            else
            {
                _break = true;
            }

        }

        void NextP()
        {
            g2.Clear(Color.Gray);
            for (int i = 0; i < NextShp.points.Length; i++)
            {
                g2.FillRectangle(new SolidBrush(NextShp.clr), NextShp.points[i].X - 130, NextShp.points[i].Y + 20, 20, 20);
                g2.DrawRectangle(p, NextShp.points[i].X - 130, NextShp.points[i].Y + 20, 20, 20);
            }
        }

        void UpdateScoreBoard()
        {
            puan.Text = Convert.ToString(score);
        }

        Shape ChoseShp(Random r)
        {
            switch (r.Next(0, 7))
            {
                case 0:
                    return new O();
                case 1:
                    return new S();
                case 2:
                    return new T();
                case 3:
                    return new L();
                case 4:
                    return new I();
                case 5:
                    return new J();
                case 6:
                    return new Z();
            }
            return null;
        }

        public Point MatrisÇarpma(Point p, int[,] m)
        {
            Point p2 = new Point();

            p2.X = (p.X * m[0, 0]) + (p.Y * m[1, 0]);
            p2.Y = (p.X * m[0, 1]) + (p.Y * m[1, 1]);

            return p2;
        }

    }

    sealed class O : Shape
    {
        public O()
        {
            points[0] = Base;
            points[1] = new Point(Base.X + 20, Base.Y);
            points[2] = new Point(Base.X + 20, Base.Y + 20);
            points[3] = new Point(Base.X, Base.Y + 20);
            massC = new Point(Base.X, Base.Y);
            clr = Color.Yellow;
        }
    }

    sealed class I : Shape
    {
        public I()
        {
            points[0] = Base;
            points[1] = new Point(Base.X, Base.Y + 20);
            points[2] = new Point(Base.X, Base.Y + 40);
            points[3] = new Point(Base.X, Base.Y + 60);
            massC = new Point(Base.X, Base.Y + 20);
            clr = Color.Green;
        }
    }

    sealed class L : Shape
    {
        public L()
        {
            points[0] = Base;
            points[1] = new Point(Base.X, Base.Y + 20);
            points[2] = new Point(Base.X, Base.Y + 40);
            points[3] = new Point(Base.X + 20, Base.Y + 40);
            massC = new Point(Base.X, Base.Y + 40);
            clr = Color.Red;
        }
    }

    sealed class J : Shape
    {
        public J()
        {
            points[0] = Base;
            points[1] = new Point(Base.X, Base.Y + 20);
            points[2] = new Point(Base.X, Base.Y + 40);
            points[3] = new Point(Base.X - 20, Base.Y + 40);
            massC = new Point(Base.X, Base.Y + 40);
            clr = Color.Orange;
        }
    }

    sealed class T : Shape
    {
        public T()
        {
            points[0] = Base;
            points[1] = new Point(Base.X - 20, Base.Y + 20);
            points[2] = new Point(Base.X, Base.Y + 20);
            points[3] = new Point(Base.X + 20, Base.Y + 20);
            massC = new Point(Base.X, Base.Y + 20);
            clr = Color.Aqua;
        }
    }

    sealed class S : Shape
    {
        public S()
        {
            points[0] = Base;
            points[1] = new Point(Base.X + 20, Base.Y);
            points[2] = new Point(Base.X, Base.Y + 20);
            points[3] = new Point(Base.X - 20, Base.Y + 20);
            massC = new Point(Base.X, Base.Y + 20);
            clr = Color.Magenta;
        }
    }

    sealed class Z : Shape
    {
        public Z()
        {
            points[0] = Base;
            points[1] = new Point(Base.X - 20, Base.Y);
            points[2] = new Point(Base.X, Base.Y + 20);
            points[3] = new Point(Base.X + 20, Base.Y + 20);
            massC = new Point(Base.X, Base.Y + 20);
            clr = Color.Blue;
        }
    }

    class Shape
    {
        public Point[] points = new Point[4];
        static public Point Base = new Point(160, 0);
        public Point massC;
        public Color clr;
    }
}