using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Labirent__Oluşturma_Çözme
{
    public partial class form1 : Form
    {
        public form1()
        {
            InitializeComponent();
        }

        private void form1_Load(object sender, EventArgs e)
        {
            label1.Text = "Hoşgeldiniz";
        }


        Random rnd = new Random();
        Pen p = new Pen(Color.Black, 2);
        SolidBrush sbg = new SolidBrush(Color.Green);
        SolidBrush sbr = new SolidBrush(Color.Red);
        SolidBrush sbb = new SolidBrush(Color.Blue);
        SolidBrush sbbl = new SolidBrush(Color.Black);
        Stopwatch sw = new Stopwatch();
        bool başarılı = false;

        private void button1_Click(object sender, EventArgs e)
        {
            if (!hesaplayıcı.IsBusy)
            {
                label1.Text = "İşleniyor...";
                button1.Text = "---";
                label3.Text = "__";
                hesaplayıcı.RunWorkerAsync();
            }   
        }

        private void hesaplayıcı_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            sw.Start();

            Graphics f = this.CreateGraphics();
            f.DrawRectangle(p, 20, 23, 502, 452);
            Graphics g = Maze.CreateGraphics();
            g.Clear(Color.Gray);
            int w = Maze.Width / 50;
            int h = Maze.Height / 50;
            List<Rectangle> dolu = new List<Rectangle>();
            List<Rectangle> boş = new List<Rectangle>();
            float c;
            Rectangle start = new Rectangle(0, 0, w, h);
            Rectangle finish = new Rectangle(Maze.Width - w, Maze.Height - h, w, h);
            g.FillRectangle(sbg, finish);

            for (int x = 0; x < Maze.Width; x += w)
            {
                for (int y = 0; y < Maze.Height; y += 2 * h)
                {
                    double num = rnd.NextDouble();

                    for (int i = 0; i < 2; i++)
                    {
                        Rectangle r = new Rectangle(x, y + h * i, w, h);
                        if ((x == 0 && y == 0) || (x == Maze.Width - w && y == Maze.Height - 2 * h))
                        {
                            g.DrawRectangle(p, r);
                            boş.Add(r);
                        }
                        else
                        {
                            if ((x > 20 * w && y > 20 * h) || (x < 10 * w && y < 10 * h))
                                c = 0.22f;
                            else
                                c = 0.35f;
                            if (num <= c)
                            {
                                dolu.Add(r);
                                g.FillRectangle(sbbl, r);
                            }
                            else
                            {
                                boş.Add(r);
                                g.DrawRectangle(p, r);
                            }
                        }
                    }
                }
            }  //creates a maze (it become a mine field but anyways)

            Rectangle current = start;
            List<Rectangle> önceki = new List<Rectangle>();
            List<double> uzaklık = new List<double>();
            List<Rectangle> aday = new List<Rectangle>();
            int ind;

            while (current != finish) //solves created maze
            {
                Rectangle r;
                g.FillRectangle(sbb, current);


                if (boş.Contains(r = new Rectangle(current.X, current.Y - h, w, h)) && !dolu.Contains(r = new Rectangle(current.X, current.Y - h, w, h)))
                {
                    uzaklık.Add(Math.Sqrt(Math.Pow(finish.X - r.X, 2) + Math.Pow(finish.Y - r.Y, 2)));
                    aday.Add(r);
                }
                if (boş.Contains(r = new Rectangle(current.X + w, current.Y, w, h)) && !dolu.Contains(r = new Rectangle(current.X + w, current.Y, w, h)))
                {
                    uzaklık.Add(Math.Sqrt(Math.Pow(finish.X - r.X, 2) + Math.Pow(finish.Y - r.Y, 2)));
                    aday.Add(r);
                }
                if (boş.Contains(r = new Rectangle(current.X, current.Y + h, w, h)) && !dolu.Contains(r = new Rectangle(current.X, current.Y + h, w, h)))
                {
                    uzaklık.Add(Math.Sqrt(Math.Pow(finish.X - r.X, 2) + Math.Pow(finish.Y - r.Y, 2)));
                    aday.Add(r);
                }
                if (boş.Contains(r = new Rectangle(current.X - w, current.Y, w, h)) && !dolu.Contains(r = new Rectangle(current.X - w, current.Y, w, h)))
                {
                    uzaklık.Add(Math.Sqrt(Math.Pow(finish.X - r.X, 2) + Math.Pow(finish.Y - r.Y, 2)));
                    aday.Add(r);
                }

                dolu.Add(current);

                if (aday.Count == 0)
                {
                    if (önceki.Count > 0)
                        current = önceki[önceki.Count - 1];
                    else
                        break;
                    önceki.RemoveAt(önceki.Count - 1);
                }
                else
                {
                    ind = 0;
                    for (int i = 0; i < uzaklık.Count; i++)
                    {
                        if (uzaklık[i] < uzaklık[ind])
                            ind = i;
                    }
                    current = aday[ind];
                    aday.Clear();
                    uzaklık.Clear();
                    önceki.Add(current);
                }

                g.FillRectangle(sbr, current);
                Thread.Sleep(50);
            }

            if (current == finish)
            {
                başarılı = true;
                g.FillRectangle(sbg, start);
                for (int i = 0; i < önceki.Count; i++)
                {
                    g.FillRectangle(sbg, önceki[i]);
                }
            }
            else
                başarılı = false;
            
        }

        private void hesaplayıcı_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            sw.Stop();
           
            if (başarılı)
                label1.Text = "Labirent Çözüldü";
            else
                label1.Text = "Yol Bulunamadı";

            button1.Text = "Tekrar";
            label3.Text = Convert.ToString((float)sw.ElapsedMilliseconds / 1000);
            sw.Reset();

        }
    }
}
