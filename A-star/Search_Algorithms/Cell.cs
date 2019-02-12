using System.Collections.Generic;
using System.Drawing;

namespace Search_Algorithms
{
    class Cell
    {
        public static int Size = 30;

        public int X;
        public int Y;

        public double h;
        public double g;
        public double f;

        public List<Cell> Neighbors;
        public Cell Parent;

        public Point MassC;
        public Rectangle rec;

        public Cell(int X, int Y)
        {
            Neighbors = new List<Cell>();

            int g = int.MaxValue;
            int f = int.MaxValue;

            this.X = X;
            this.Y = Y;

            MassC = new Point(X * Size + Size / 2, Y * Size + Size / 2);
            rec = new Rectangle(X * Size + 2, Y * Size + 2, Size - 4, Size - 4);
        }
    }
}
