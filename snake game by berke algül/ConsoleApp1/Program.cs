using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        public enum Dir { left, right, up, down, stop };
        public enum Gamestatus { gameover, reset, paused,play };
        public ConsoleKeyInfo keypress = new ConsoleKeyInfo();
        public int headx, heady, foodx, foody, width, height, score, preX, preY, tems, speed, tems2, debuffx1, debuffx2, debuffy1, debuffy2;
        public bool isprinted,firststart,firstspawn,allspawn;
        public Dir CurrentDir,PreDir,TemDir;
        public Gamestatus CurrentStatus;
        public Random random = new Random();
        List<int> Bodyx = new List<int>();
        List<int> Bodyy = new List<int>();
  
        void InterFace()
        {
            Console.SetWindowSize(56, 22);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.CursorVisible = false;
            Console.WriteLine("#-----------------------------------------------------#");
            Console.WriteLine("#-----------------------------------------------------#");
            Console.WriteLine("#-----------------------------------------------------#");
            Console.WriteLine("#-------------------Snake Game------------------------#");
            Console.WriteLine("#-----------------------------------------------------#");
            Console.WriteLine("#-----------------By Berke Algül----------------------#");
            Console.WriteLine("#-----------------------------------------------------#");
            Console.WriteLine("#--------Press any key to start-----------------------#");
            Console.WriteLine("#---------------------Press Esc to exit---------------#");
            Console.WriteLine("#-----------------------------------------------------#");
            Console.WriteLine("#-----------------------------------------------------#");
            Console.WriteLine("#-----------------------------------------------------#");
            Console.WriteLine("#-----Control with \"W A S D\" buttons------------------#");
            Console.WriteLine("#-----------------------------------------------------#");
            Console.WriteLine("#--------Eat 'F'--&--Avoid 'X'------------------------#");
            Console.WriteLine("#-----------------------------------------------------#");
            Console.WriteLine("#-----------------------------------------------------#");
            keypress = Console.ReadKey(true);
            if (keypress.Key == ConsoleKey.Escape) { Environment.Exit(0); }
            firststart = false;
            Console.Clear();
            
        }

        void Start()
        {
            if (!firststart)
            {
                DefaultSettings();
                firststart = true;
            }
            Console.SetWindowSize(width + 6, height + 6);
            while (CurrentStatus == Gamestatus.play)
            {
                
                CollisionCheck();
                Input();
                Move();
                Render();
                ScoreCheck();
                DebuffDirector();
                Thread.Sleep(speed);
            }
            if (CurrentStatus == Gamestatus.reset)
            {
                Bodyx.RemoveRange(0, Bodyx.Count);
                Bodyy.RemoveRange(0, Bodyy.Count);
                CurrentStatus = Gamestatus.play;
                firststart = false;
                Start();
            }
        }

        void DefaultSettings()
        {
            Bodyx.Add(100);
            Bodyy.Add(100);
            PreDir = Dir.stop;
            debuffx1 = 100;
            debuffx2 = 100;
            width = 40;
            height = 24;
            firstspawn = false;
            allspawn = false;
            headx = width / 2;
            heady = height / 2;
            score = 0;
            tems = 0;
            tems2 = 0;
            CurrentDir = Dir.down;
            CurrentStatus = Gamestatus.play;
            Console.SetWindowSize(44, 26);
            foodx = random.Next(1, width - 2);
            foody = random.Next(1, height - 2);
            speed = 60;
        }

        void Render()
        {
            
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x == width - 1)
                    {
                        Console.WriteLine("|");
                    }
                    else if (x == 0)
                    {
                        Console.Write("|");
                    }
                    else if (x == headx && y == heady)
                    {
                        Console.Write("0");
                    }
                    else if ((y == 0 || y == height - 1) && (x != 0 || x != width - 1))
                    {
                        Console.Write("=");
                    }
                    else if (x == foodx && y == foody)
                    {
                        Console.Write("F");
                    }
                    else if((x == debuffx1 && y == debuffy1)||(x == debuffx2 && y == debuffy2))
                    {
                        Console.Write("X");
                    }
                    else
                    {
                        isprinted = false;
                        for (int a = 0; a < Bodyx.Count; a++)
                        {
                            if (x == Bodyx[a] && y == Bodyy[a])
                            {
                                Console.Write("o");
                                isprinted = true;
                            }
                        }
                        if (!isprinted)
                        {
                            Console.Write(" ");
                        }
                    }
                }
            }
            Console.WriteLine("Score:{0}",score);
            Console.WriteLine("Press P to pause");
        }

        void Move()
        {
            if(CurrentDir != Dir.stop)
            {
                if (CurrentDir == Dir.down)
                {
                    heady++;
                }
                else if (CurrentDir == Dir.up)
                {
                    heady--;
                }
                else if (CurrentDir == Dir.left)
                {
                    headx--;
                }
                else if (CurrentDir == Dir.right)
                {
                    headx++;
                }
                preX = Bodyx[0];
                preY = Bodyy[0];

                int tempx, tempy;
                for (int x = 0; x < Bodyx.Count; x++)
                {
                    if (x == 0)
                    {
                        Bodyx[x] = headx;
                        Bodyy[x] = heady;
                    }
                    else
                    {
                        tempx = Bodyx[x];
                        tempy = Bodyy[x];
                        Bodyx[x] = preX;
                        Bodyy[x] = preY;
                        preX = tempx;
                        preY = tempy;
                    }

                }
            }
        }

        void CollisionCheck()
        {
            if (headx == foodx && heady == foody) 
            {
                Eat();
            }
            else if ((headx == 0 || headx == width - 1) || (heady == 0 || heady == height - 1)) 
            {
                Die();
            }
            else if(((headx <= debuffx1+1&&headx >=debuffx1-1)&&(CurrentDir == Dir.left || CurrentDir == Dir.right)&&heady == debuffy1) || (headx == debuffx1&&(heady <= debuffy1 + 1 && heady >= debuffy1 - 1) && (CurrentDir == Dir.up || CurrentDir == Dir.down)))
            {
                debuffx1 = 100;
                debuffy1 = 100;
                score -= 50;
                for(int i = 0; i < 5; i++)
                {
                    int b = Bodyx.Count - 1;
                    Bodyx.RemoveAt(b);
                    Bodyy.RemoveAt(b);
                }
                
                firstspawn = false;
                
            }
            else if (((headx <= debuffx2 + 1 && headx >= debuffx2 - 1) && (CurrentDir == Dir.left || CurrentDir == Dir.right)&&heady ==debuffy2) || (headx ==debuffx2&&(heady <= debuffy2 + 1 && heady >= debuffy2 - 1) && (CurrentDir == Dir.up || CurrentDir == Dir.down)))
            {
                debuffx2 = 100;
                debuffy2 = 100;
                score -= 50;

                for (int i = 0; i < 5; i++)
                {
                    int a = Bodyx.Count - 1;
                    Bodyx.RemoveAt(a);
                    Bodyy.RemoveAt(a);
                }

                allspawn = false;
            }
            for (int i = 1; i < Bodyx.Count; i++)
            {
                if (headx == Bodyx[i] && heady == Bodyy[i])
                {
                    Die();
                }
            }
        }

        void Eat()
        {
            foodx = random.Next(1, width - 1);
            foody = random.Next(1, height - 1);
            Bodyx.Add(foodx);
            Bodyy.Add(foody);
            score += 10;
            tems += 10;
            tems2 += 10;
        }
        
        void Die()
        {
            CurrentStatus = Gamestatus.gameover;
            Console.Clear();
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("-----------------You Died-----------------");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("------------Final score: {0}----------------",score);
            Console.WriteLine("------------Press R to restart------------");
            Console.WriteLine("------------Press ESC key to exit---------");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("------------------------------------------");
            Thread.Sleep(2000);
            keypress = Console.ReadKey(true);
            if (keypress.Key == ConsoleKey.R)
            {
                Console.Clear();
                Start();
                CurrentStatus = Gamestatus.reset;
            }
            else if(keypress.Key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }

        } 

        void Input()
        {

            while(Console.KeyAvailable)
            {
                keypress = Console.ReadKey(true);
                if (keypress.Key == ConsoleKey.W && CurrentDir != Dir.down)
                {
                    TemDir = CurrentDir;
                    CurrentDir = Dir.up;
                    PreDir = TemDir;
                }
                else if (keypress.Key == ConsoleKey.S && CurrentDir != Dir.up)
                {
                    TemDir = CurrentDir;
                    CurrentDir = Dir.down;
                    PreDir = TemDir;
                }
                else if (keypress.Key == ConsoleKey.A && CurrentDir != Dir.right)
                {
                    TemDir = CurrentDir;
                    CurrentDir = Dir.left;
                    PreDir = TemDir;
                }
                else if (keypress.Key == ConsoleKey.D && CurrentDir != Dir.left)
                {
                    TemDir = CurrentDir;
                    CurrentDir = Dir.right;
                    PreDir = TemDir;
                }else if(keypress.Key == ConsoleKey.P)
                {
                    CurrentStatus = Gamestatus.paused;
                    Pause();
                }
            }
                
        }
        
        void Pause()
        {
            PreDir = CurrentDir;
            CurrentDir = Dir.stop;
            Console.Clear();
            Console.WriteLine("#################################################");
            Console.WriteLine("#################################################");
            Console.WriteLine("#-----------------Game Paused-------------------#");
            Console.WriteLine("#################################################");
            Console.WriteLine("#------------------Press P to Continue----------#");
            Console.WriteLine("#-----------------------------------------------#");
            Console.WriteLine("#################################################");
            Console.WriteLine("#################################################");
            Console.ReadKey(true);
            if(keypress.Key == ConsoleKey.P)
            {
                CurrentStatus = Gamestatus.play;
                CurrentDir = PreDir;
                Start();
            }
        }
        
        void ScoreCheck()
        {
            if(tems == 150)
            {
                if (speed <= 30)
                { speed -= 5; }
                tems = 0;
            }
            if(tems2 == 100)
            {
                    if (!firstspawn)
                    {
                        debuffx1 = width - 3;
                        debuffy1 = random.Next(height - 8,height-3);
                        firstspawn = true;
                    }
                    else if(firstspawn && !allspawn)
                    {
                        debuffx2 = 3;
                    debuffy2 = random.Next(3, 8);
                        allspawn = true;
                    }
                
                tems2 = 0;
            }
        }

        void DebuffDirector()
        {
            if (firstspawn) //sağ alt köşe sola kaycak
            {
                if(debuffx1 <= width - 3) 
                {
                    debuffx1--;
                    
                }
                if(debuffx1 == 3)
                {
                    debuffx1 = width - 3;
                    
                }
            }
            if (allspawn) // sol üst köşe sağa kaycak
            {
                if (debuffx2 >= 3)
                {
                    debuffx2++;
                    
                }
                if (debuffx2 == width-3)
                {
                    debuffx2 = 3;
                    
                }
            }
        }

        static void Main(string[] args)
        {
            Program snake = new Program();
            snake.InterFace();
            snake.Start();
        }    
    }
}
