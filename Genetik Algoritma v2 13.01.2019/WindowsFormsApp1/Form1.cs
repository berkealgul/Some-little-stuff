using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            g = panel1.CreateGraphics();
            Greensb = new SolidBrush(Color.Green);
            Bluesb = new SolidBrush(Color.Blue);
            Redsb = new SolidBrush(Color.Red);
            Graysb = new SolidBrush(Color.Gray);

            r = new Random();
            Population_Size = 45;
            LifeTime = 150;
            fTreshHold = 10.0f;
  
            Poisons = new List<Poison>();
            Foods = new List<Food>();
            newF = new List<RectangleF>();
            newP = new List<RectangleF>();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Gray);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 3), new Rectangle(panel1.Location.X - 3, panel1.Location.Y - 3, panel1.Width + 6, panel1.Height + 6));
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!CalculatorThread.IsBusy || !bOnline)
                return;

            if (e.Button == MouseButtons.Left)
            {
                Food f = new Food(e.X, e.Y);
                Foods.Add(f);
                newF.Add(f.Rec);
            }
                
            else if (e.Button == MouseButtons.Right)
            {
                Poison p = new Poison(e.X, e.Y);
                Poisons.Add(p);
                newP.Add(p.Rec);
            }
                
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!CalculatorThread.IsBusy)
                CalculatorThread.RunWorkerAsync();
        }

        int Population_Size;
        int LifeTime;
        float EatenF;
        float EatenP;
        bool bOnline;

        float fTreshHold;

        Graphics g;
        Random r;

        List<Food> Foods;
        List<Poison> Poisons;
        List<RectangleF> newF;
        List<RectangleF> newP;

        SolidBrush Greensb;
        SolidBrush Bluesb;
        SolidBrush Redsb;
        SolidBrush Graysb;

        private void CalculatorThread_DoWork(object sender, DoWorkEventArgs e)
        {
            var p = new Population(r, Population_Size);
            while (true)
            {
                UIupdate(p.Generation,-1,-1);

                Life_Similation(ref p);
                p.Next_Generation();

                UIupdate(p.Generation, -1, EatenF / EatenP);
            }
        }

        void Life_Similation(ref Population p)
        {
            List<Agent> Agents = new List<Agent>();
            List<DNA> Survived_DNAs = new List<DNA>();
            List<RectangleF> oldR = new List<RectangleF>();

            EatenP = 0;
            EatenF = 0;

            bOnline = true;

            for (int i = 0; i < 40; i++)
            {
                Foods.Add(new Food(r.Next(0, panel1.Width), r.Next(r.Next(0, panel1.Height))));
                Poisons.Add(new Poison(r.Next(0, panel1.Width), r.Next(r.Next(0, panel1.Height))));
                g.FillEllipse(Bluesb, Foods[Foods.Count - 1].Rec);
                g.FillEllipse(Redsb, Poisons[Poisons.Count - 1].Rec);
                if (i < Population_Size)
                {
                    Agents.Add(new Agent(r.Next(0, panel1.Width), r.Next(0, panel1.Height), p.DNAs[i], r));
                }
            }

            for (int y = 0; y < LifeTime; y++)
            {
                for (int i = 0; i < Agents.Count; i++)
                {
                    oldR.Add(Agents[i].Rec);

                    Agents[i].SpotTarget(Foods, Poisons);
                    Agents[i].Move();
                    Agents[i].HealthP--;

                    for(int x = 0; x<Foods.Count;x++)
                        if (Math.Abs(Foods[x].fX - Agents[i].fX) < fTreshHold && Math.Abs(Foods[x].fY - Agents[i].fY) < fTreshHold)
                        {
                            oldR.Add(Foods[x].Rec);

                            Foods.Remove(Foods[x]);
                            Agents[i].HealthP += 10;
                            Foods.Add(new Food(r.Next(0, panel1.Width), r.Next(r.Next(0, panel1.Height))));
                            EatenF++;

                            newF.Add(Foods[Foods.Count - 1].Rec);
                        }

                    for (int x = 0; x < Poisons.Count; x++)
                        if (Math.Abs(Poisons[x].fX - Agents[i].fX) < fTreshHold && Math.Abs(Poisons[x].fY - Agents[i].fY) < fTreshHold)
                        {
                            oldR.Add(Poisons[x].Rec);

                            Poisons.Remove(Poisons[x]);
                            Agents[i].HealthP -= 20;
                            Poisons.Add(new Poison(r.Next(0, panel1.Width), r.Next(r.Next(0, panel1.Height))));
                            EatenP++;

                            newP.Add(Poisons[Poisons.Count - 1].Rec);
                        }

                    if (Agents[i].HealthP <= 0)
                        Agents.Remove(Agents[i]);
                }
                Render(Agents, ref oldR, ref newF, ref newP);
                Thread.Sleep(30);
                UIupdate(-1, LifeTime - y,-1);
            }

            for (int i = 0; i < Agents.Count; i++)
            {
                Agents[i].dna.fScore = Agents[i].HealthP;
                Survived_DNAs.Add(Agents[i].dna);
            }
            p.DNAs = Survived_DNAs;

            g.Clear(Color.Gray);
            Foods.Clear();
            Poisons.Clear();
            newF.Clear();
            newP.Clear();
            bOnline = false;
        }

        void Render(List<Agent> Agents, ref List<RectangleF> oldR, ref List<RectangleF> newF, ref List<RectangleF> newP)
        {
            foreach (RectangleF rec in oldR)
            {
                g.FillEllipse(Graysb, rec);
            }
            try
            {
                foreach (RectangleF rec in newF)
                {
                    g.FillEllipse(Bluesb, rec);
                }
                foreach (RectangleF rec in newP)
                {
                    g.FillEllipse(Redsb, rec);
                }
            }
            catch
            {

            }

            oldR.Clear();
            newF.Clear();
            newP.Clear();
            foreach (Agent item in Agents)
            {
                g.FillEllipse(Greensb, item.Rec);
            }
        }

        void UIupdate(int gen,int count,float FdP)
        {
            try
            {
                if (gen != -1)
                {
                    this.Invoke(new Action(() =>
                    {
                        Gen.Text = Convert.ToString(gen);
                    }
                ));
                }

                if (count != -1)
                {
                    this.Invoke(new Action(() =>
                    {
                        label1.Text = Convert.ToString(count);
                    }
                    ));
                    this.Invoke(new Action(() =>
                    {
                        label4.Text = Convert.ToString(EatenF / EatenP);
                    }
                    ));
                }

                if (FdP != -1)
                {
                    this.Invoke(new Action(() =>
                    {
                        listBox1.Items.Add("Nesil "+ (gen-1) + "->"+ FdP);
                    }
                  ));
                }
            }
            catch { }
        }
    }


    class Agent
    {
        public static int Size = 15;

        public Food targetFood;
        public Poison targetPoison;
        public RectangleF Rec;
        public DNA dna;
        public float fX, fY;
        public int HealthP = 100;

        Random rnd;

        public Agent(float fX, float fY ,DNA dna,Random rnd)
        {
            this.rnd = rnd;
            this.fX = fX;
            this.fY = fY;
            this.dna = dna;
            Rec = new RectangleF(fX - Size/2, fY- Size / 2, Size, Size);
        }

        public void SpotTarget(List<Food> Foods, List<Poison> Poisons)
        {
            targetFood = null;
            targetPoison = null;

            double dClosest = 1000;
            int index = 0;

            for (int i = 0; i < Foods.Count; i++)
            {
                double dis = Math.Sqrt(Math.Pow(Foods[i].fX - fX, 2) + Math.Pow(Foods[i].fY - fY, 2));
                if (dis < dClosest && dis <= dna.fFoodAR)
                {
                    dClosest = dis;
                    index = i;
                }
            }
            targetFood = Foods[index];


            index = 0;
            dClosest = 1000;

            for (int i = 0; i < Poisons.Count; i++)
            {
                double dis = Math.Sqrt(Math.Pow(Poisons[i].fX - fX, 2) + Math.Pow(Poisons[i].fY - fY, 2));
                if (dis < dClosest && dis <= dna.fPoisonAR)
                {
                    dClosest = dis;
                    index = i;
                }
            }
            targetPoison = Poisons[index];
        }

        public void Move()
        {
            float oldX = fX;
            float oldY = fY;
            float fDirSin;
            float fDirCos;

            if (targetFood != null)
            {
                fDirSin = (targetFood.fY - fY) / (float)Math.Sqrt(Math.Pow(targetFood.fY - fY, 2) + Math.Pow(targetFood.fX - fX, 2));
                fDirCos = (targetFood.fX - fX) / (float)Math.Sqrt(Math.Pow(targetFood.fY - fY, 2) + Math.Pow(targetFood.fX - fX, 2));
                //go for food
                fX += fDirCos * dna.fGeneF;
                fY += fDirSin * dna.fGeneF;
            }

            if (targetPoison != null)
            {
                fDirSin = (targetPoison.fY - fY) / (float)Math.Sqrt(Math.Pow(targetPoison.fY - fY, 2) + Math.Pow(targetPoison.fX - fX, 2));
                fDirCos = (targetPoison.fX - fX) / (float)Math.Sqrt(Math.Pow(targetPoison.fY - fY, 2) + Math.Pow(targetPoison.fX - fX, 2));
                //go for poison
                fX += fDirCos * dna.fGeneP;
                fY += fDirSin * dna.fGeneP;
            }

            //go for arbitary direction
            fX += (float)Math.Cos(rnd.NextDouble() * Math.PI * 2) * dna.fGene3;
            fY += (float)Math.Sin(rnd.NextDouble() * Math.PI * 2) * dna.fGene3;

           Rec.X = fX - Size / 2;
           Rec.Y = fY - Size / 2;
        }

        PointF RotatePoint(PointF p)
        {
            PointF newP = new PointF(-1 * p.Y, p.X);
            return newP;
        }
    }

    class Food
    {
        public static float Size = 8.0f;

        public float fX, fY;
        public RectangleF Rec;

        public Food(float fX, float fY)
        {
            this.fX = fX;
            this.fY = fY;
            Rec = new RectangleF(fX - Size / 2, fY - Size / 2, Size, Size);
        }
    }

    class Poison
    {
        public static float Size = 8.0f;

        public float fX, fY;
        public RectangleF Rec;

        public Poison(float fX, float fY)
        {
            this.fX = fX;
            this.fY = fY;
            Rec = new RectangleF(fX - Size / 2, fY - Size / 2, Size, Size);
        }
    }
}