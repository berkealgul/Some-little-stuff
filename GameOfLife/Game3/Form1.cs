using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game3
{
    public partial class AnaSayfa : Form
    {
        public AnaSayfa()
        {
            InitializeComponent();

        }

        private void AnaSayfa_Load(object sender, EventArgs e)
        {
            g = panel1.CreateGraphics();
            k = new Pen(Color.Black, 2);
            popülasyon = new bool[panel1.Width / 10,panel1.Height / 10];
            sonrakiP = new bool[panel1.Width / 10, panel1.Height / 10];
            r = new Random();
            sbs = new SolidBrush(Color.Black);
            sbg = new SolidBrush(Color.Gray);
            komşu = new List<bool>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!hesaplayıcı.IsBusy)
                hesaplayıcı.RunWorkerAsync();
        }

        Graphics g;
        bool[,] popülasyon,sonrakiP; //[x,y]
        Pen k;
        Random r;
        SolidBrush sbs, sbg;
        List<bool> komşu;

        private void hesaplayıcı_DoWork(object sender, DoWorkEventArgs e)
        {
            using (Graphics f = CreateGraphics())
            {
                f.DrawRectangle(k, panel1.Location.X - 1, panel1.Location.Y - 1, panel1.Width + 2, panel1.Height + 2);
            }
            for (int i = 10; i < panel1.Width; i += 10)
            {
                g.DrawLine(k, i, 0, i, panel1.Height);
            }
            for (int i = 10; i < panel1.Height; i += 10)
            {
                g.DrawLine(k, 0, i, panel1.Width, i);
            }


            for (int x = 0; x < panel1.Width / 10; x++)
            {
                for (int y = 0; y < panel1.Height / 10; y++)
                {
                    if (r.Next(0,2) == 1)
                    {
                        popülasyon[x, y] = true;
                        g.FillRectangle(sbs, new Rectangle(x * 10, y * 10, 9, 9));
                    }
                    else
                    {
                        g.FillRectangle(sbg, new Rectangle(x * 10, y * 10, 9, 9));
                        popülasyon[x, y] = false;
                    }
                }
            }


            while (true)
            {
                for (int x = 0; x < panel1.Width / 10; x++)
                {
                    for (int y = 0; y < panel1.Height / 10; y++)
                    {
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                if (i == 0 && j == 0)
                                    continue;
                                try
                                {
                                    if (popülasyon[x + i, y + j])
                                        komşu.Add(true);
                                }
                                catch
                                {
                                    continue;
                                }
                                
                            }
                        }

                        if (!popülasyon[x, y])
                        {
                            if (komşu.Count == 3)
                                sonrakiP[x, y] = true;
                        }
                        else
                        {
                            if(komşu.Count < 2 || komşu.Count > 3)
                                sonrakiP[x, y] = false;
                        }

                        komşu.Clear();
                    }
                }

                for (int x = 0; x < panel1.Width / 10; x++)
                {
                    for (int y = 0; y < panel1.Height / 10; y++)
                    {
                        if (sonrakiP[x, y])
                            g.FillRectangle(sbs, new Rectangle(x * 10, y * 10, 9, 9));
                        else
                            g.FillRectangle(sbg, new Rectangle(x * 10, y * 10, 9, 9));
                    }
                }

                popülasyon = sonrakiP;
            }
        }
    }
}
