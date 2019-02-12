using System.Drawing;

namespace Matris
{
    public static class Matrisİşlem
    {
        public static Point Çarpma(Point p, int[,] m)
        {
            Point p2 = new Point();

            p2.X = (p.X * m[0, 0]) + (p.Y * m[1, 0]);
            p2.Y = (p.X * m[0, 1]) + (p.Y * m[1, 1]);

            return p2;
        }
    }
}
