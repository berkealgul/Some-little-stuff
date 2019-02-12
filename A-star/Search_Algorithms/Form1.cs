using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Search_Algorithms
{
    public partial class Form1 : Form
    {
        //Load Components
        public Form1()
        {
            InitializeComponent();

            graph = canvas.CreateGraphics();

            Bluesb = new SolidBrush(Color.Blue);
            Redsb = new SolidBrush(Color.IndianRed);
            Greensb = new SolidBrush(Color.LightGreen);
            Graysb = new SolidBrush(Color.DarkGray);
            Blacksb = new SolidBrush(Color.Black);
            Yellowsb = new SolidBrush(Color.Yellow);

            BlueP = new Pen(Color.Blue, 3);

            w = canvas.Width / Cell.Size;
            h = canvas.Height / Cell.Size;

            SetGrid();
            Walls = new List<Cell>();
            Gravel = new List<Cell>();

            sw = new Stopwatch();
        }

        //Control methods
        private void start_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (backgroundWorker1.IsBusy)
                return;

            if (NeedRes)
            {
                NeedRes= false;
                RestartScreen();
            }

            int x = e.X / Cell.Size;
            int y = e.Y / Cell.Size;
            Cell cell = grid[x, y];
            //Add-remove squares 
            switch (e.Button)
            {
                case MouseButtons.Middle: //adjust starting point
                    if (cell != Start && !Walls.Contains(cell))
                    {
                        graph.FillRectangle(Graysb, Start.rec);
                        Start = cell;
                        graph.FillRectangle(Greensb, Start.rec);
                    }
                    break;
                case MouseButtons.Right: //adjust finish point
                    if (cell != Finish && !Walls.Contains(cell))
                    {
                        graph.FillRectangle(Graysb, Finish.rec);
                        Finish = cell;
                        graph.FillRectangle(Redsb, Finish.rec);
                    }
                    break;
                case MouseButtons.Left:
                    if (cell != Finish && cell != Start)
                    {
                        if (wall.Checked)
                        {
                            if (Gravel.Contains(cell))
                                break;

                            if (!Walls.Contains(cell))  //add or remove wall
                            {
                                graph.FillRectangle(Blacksb, cell.rec);
                                Walls.Add(cell);
                            }
                            else
                            {
                                graph.FillRectangle(Graysb, cell.rec);
                                Walls.Remove(cell);
                            }
                        }
                        else
                        {
                            if (Walls.Contains(cell))
                                break;

                            if (!Gravel.Contains(cell))  //add or remove gravel
                            {
                                graph.FillRectangle(Yellowsb, cell.rec);
                                Gravel.Add(cell);
                            }
                            else
                            {
                                graph.FillRectangle(Graysb, cell.rec);
                                Gravel.Remove(cell);
                            }
                        }
                    }
                    break;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rec = new Rectangle(canvas.Location.X - 2, canvas.Location.Y - 2, canvas.Width + 4, canvas.Height + 4);
            Pen p = new Pen(Color.Black, 3);
            e.Graphics.DrawRectangle(p,rec);

            Rectangle rec2 = new Rectangle(label1.Location.X -5,label1.Location.Y - 5,120,200);
            e.Graphics.DrawRectangle(p, rec2);
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.DarkGray);

            Pen p = new Pen(Color.Black, 3);

            for (int i = 0; i < w; i++)
            {
                Point pt1 = new Point(i * Cell.Size, 0);
                Point pt2 = new Point(i * Cell.Size, h * Cell.Size);
                e.Graphics.DrawLine(p, pt1, pt2);
            }
            for (int i = 0; i < h; i++)
            {
                Point pt1 = new Point(0, i * Cell.Size);
                Point pt2 = new Point(w * Cell.Size, i * Cell.Size);
                e.Graphics.DrawLine(p, pt1, pt2);
            }

            e.Graphics.FillRectangle(Greensb, Start.rec);
            e.Graphics.FillRectangle(Redsb, Finish.rec);

            foreach (Cell item in Walls)
            {
                graph.FillRectangle(Blacksb, item.rec);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Form infoF = new Info(this.Location.X, this.Location.Y);
            infoF.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
                return;

            Walls.Clear();
            Gravel.Clear();
            RestartScreen();
        }

        //Variables
        Graphics graph;
        Pen BlueP;
        SolidBrush Bluesb, Redsb, Greensb, Graysb, Blacksb, Yellowsb;

        int w, h;
        bool NeedRes;

        Cell[,] grid;
        List<Cell> Walls,Gravel, OpenSet, ClosedSet;
        Cell Start, Finish,Current;

        Stopwatch sw;

        // Main method
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            OpenSet = new List<Cell>();
            ClosedSet = new List<Cell>();

            Start.g = 0;
            Start.h = dis(Start, Finish);
            Start.f = Start.g + Start.f;

            OpenSet.Add(Start);

            sw.Start();
            while (OpenSet.Count > 0)
            {
                int ind = 0;
                for (int i = 0; i < OpenSet.Count; i++)
                {
                    if (OpenSet[ind].f > OpenSet[i].f)
                        ind = i;
                }
                Current = OpenSet[ind];

                if (Current == Finish)
                    break;

                OpenSet.Remove(Current);
                ClosedSet.Add(Current);

                AddNeighbors(ref Current);

                for (int i = 0; i < Current.Neighbors.Count; i++)
                {
                    Cell Neighbor = Current.Neighbors[i];
                    double temG = Current.g + dis(Current,Neighbor) * (Gravel.Contains(Neighbor) ? 2 : 1);

                    if (ClosedSet.Contains(Neighbor))
                        continue;

                    if (!OpenSet.Contains(Neighbor))
                    {
                        Neighbor.h = dis(Current, Finish);
                        Neighbor.g = temG;
                        Neighbor.f = Neighbor.g + Neighbor.h;
                        Neighbor.Parent = Current;
                        OpenSet.Add(Neighbor);
                        continue;
                    }

                    if (temG < Neighbor.g)
                    {
                        Neighbor.Parent = Current;
                        Neighbor.g = temG;
                        Neighbor.f = Neighbor.g + Neighbor.h;
                    }
                }
            }

            sw.Stop();

            if (Current == Finish)
                Display();

            UIupdate();

            NeedRes = true;
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    grid[x, y].Neighbors.Clear();
        }

        //sub methods
        void Display() 
        {
            Point[] Path = GeneratePath();
            graph.DrawLines(BlueP,Path);

            Rectangle recS = new Rectangle(Path[0].X - 10, Path[0].Y - 10, 20, 20);
            Rectangle recF = new Rectangle(Path[Path.Length-1].X - 10, Path[Path.Length-1].Y - 10, 20, 20);
            graph.FillEllipse(Bluesb, recS);
            graph.FillEllipse(Bluesb, recF);
        }

        void RestartScreen()
        {
            graph.Clear(Color.DarkGray);

            Pen p = new Pen(Color.Black, 3);

            for (int i = 0; i < w; i++)
            {
                Point pt1 = new Point(i * Cell.Size, 0);
                Point pt2 = new Point(i * Cell.Size, h * Cell.Size);
                graph.DrawLine(p, pt1, pt2);
            }
            for (int i = 0; i < h; i++)
            {
                Point pt1 = new Point(0, i * Cell.Size);
                Point pt2 = new Point(w * Cell.Size, i * Cell.Size);
                graph.DrawLine(p, pt1, pt2);
            }

            graph.FillRectangle(Greensb, Start.rec);
            graph.FillRectangle(Redsb, Finish.rec);

            foreach (Cell cell in Walls)
                graph.FillRectangle(Blacksb, cell.rec);
            foreach (Cell cell in Gravel)
                graph.FillRectangle(Yellowsb, cell.rec);
        }

        void SetGrid()
        {
            grid = new Cell[w, h];

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    grid[x, y] = new Cell(x, y);

            Start = grid[1, h / 2];
            Finish = grid[w - 2, h / 2];
        }

        void UIupdate()
        {
            Invoke(new Action(() =>
            {
                double time = (double)sw.ElapsedMilliseconds / 1000;
                sw.Reset();

                if (Current == Finish)
                {
                    status.ForeColor = Color.LightGreen;
                    status.Text = "Başarılı";
                }
                else
                {
                    status.ForeColor = Color.OrangeRed;
                    status.Text = "Başarısız";
                }
                string timeS = Convert.ToString(time);
                timeL.Text = timeS + " sn";           
            }));
        }

        void AddNeighbors(ref Cell cell)
        {
            for (int i = -1; i < 2; i++)
                for (int j = -1; j < 2; j++)
                {
                    int x = cell.X + i;
                    int y = cell.Y + j;

                    if (x < 0 || x >= w || y < 0 || y >= h)
                        continue;
                    if (!Walls.Contains(grid[x, y]))
                        cell.Neighbors.Add(grid[x, y]);
                }
        }

        double dis(Cell c1,Cell c2)
        {
            return Math.Sqrt(Math.Pow(c1.X - c2.X,2) + Math.Pow(c1.Y - c2.Y,2));
        }

        Point[] GeneratePath()
        {
            List<Point> Path = new List<Point>();
            Cell cell = Finish;
            
            while(cell != Start)
            {
                Path.Add(cell.MassC);
                cell = cell.Parent;
            }
            Path.Add(Start.MassC);

            return Path.ToArray();
        }
    }
}
