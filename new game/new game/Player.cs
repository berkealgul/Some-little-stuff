using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace new_game
{
    class Player
    {
        public int headX, headY, preY, temY, x;
        public bool fired,finished;
        public List<int> TailX = new List<int>();
        public List<int> TailY = new List<int>();
        public int[,] Blast = new int[5, 2];
        public int[,] Body = new int[7, 2];
        public int[,] Shield = new int[19, 2];


        public Player(int X, int Y)
        {
            headX = X;
            headY = Y;
            x = headX - 1; 
            for (int i = 0; i < 7; i++)
            {
                TailX.Add(x);
                TailY.Add(headY);
                x--;
                if (i < 5)
                {
                    Blast[i, 0] = 150;
                    Blast[i, 1] = 150;
                }
            }
            Body[0, 0] = headX;
            Body[0, 1] = headY + 1;
            Body[1, 0] = headX;
            Body[1, 1] = headY - 1;
            Body[2, 0] = headX + 1;
            Body[2, 1] = headY + 1;
            Body[3, 0] = headX + 1;
            Body[3, 1] = headY;
            Body[4, 0] = headX + 1;
            Body[4, 1] = headY - 1;
            Body[5, 0] = headX + 2;
            Body[5, 1] = headY;
            Body[6, 0] = headX + 3;
            Body[6, 1] = headY;

            int offset = 0;
            for (int i = 0; i < 17; i++)
            {
                if(i < 7)
                {
                    Shield[i, 0] = headX + offset;
                    Shield[i, 1] = headY - 2;
                    offset++;
                }else if(i < 11)
                {
                    offset = 1;
                    Shield[i, 0] = Shield[i - 1, 0];
                    Shield[i, 1] = Shield[i-1,1] + offset;
                    offset++;
                }else if(i < 17)
                {
                    offset = -1;
                    Shield[i, 0] = Shield[i - 1, 0] + offset;
                    Shield[i, 1] = Shield[i - 1, 1];
                    offset--;
                }
               
            }
        }

        public void Fire(int hx,int hy)
        {
            fired = true;
            finished = false;
            int a = 2;
            for (int i = 0; i < 5; i++)
            {
                Blast[i, 0] = hx + 4;
                Blast[i, 1] = hy + a;
                a--;
            }
   
        }

        public void PlayerMove(int x)
        {
            headY += x;
            for (int i = 0; i < 19; i++)
            {
                Shield[i, 1] += x;
                if(i < 7)
                Body[i, 1] += x;
            }
        }

        public void Move()
        {
            preY = headY;
            for (int i = 0; i < TailY.Count; i++)
            {
                temY = TailY[i];
                TailY[i] = preY;
                preY = temY;
            }
            if (fired)
            {
                for (int i = 0; i < 5; i++)
                {
                    Blast[i, 0]++;
                }
                if (Blast[1, 0] == 78)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Blast[i, 0] = 150;
                    }
                    fired = false;
                    finished = true;
                }
            }
        }
    }
}
