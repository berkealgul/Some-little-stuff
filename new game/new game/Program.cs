using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

namespace new_game
{
    class Program
    {
        int width, height, score, ammo, spawnrate, charge, count, sleeprate;
        Stopwatch watch;
        long time;
        bool isprinted, activated, visible;
        string ShieldCharge, Ammo;
        ConsoleKeyInfo keypress = new ConsoleKeyInfo();
        Random random = new Random();
        Player p;
        List<Astreoid> Astreoids = new List<Astreoid>();



        static void Main(string[] args)
        {
            Program game = new Program();
            game.Interface();
            game.DefaultSettings();
            game.GameManagement();
        }

        void Interface()
        {
            Console.SetWindowSize(65, 19);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "Welcome Good Luck";
            Console.WriteLine("#################################################################");
            Console.WriteLine("#################################################################");
            Console.WriteLine("##--------THE GAME-------------BY---BERKE ALGÜL----------------##");
            Console.WriteLine("##-------------------------------------------------------------##");
            Console.WriteLine("##------------Hit W to rise  &  Hit S to dive------------------##");
            Console.WriteLine("##-------------------------------------------------------------##");
            Console.WriteLine("##----Space to activate shield---------------------------------##");
            Console.WriteLine("##----F to fire but care you cant get ammo replenishment-------##");
            Console.WriteLine("##-------------------------------------------------------------##");
            Console.WriteLine("##-------------------------------------------------------------##");
            Console.WriteLine("##-------------------------------------------------------------##");
            Console.WriteLine("##--------Avoid 'X' in other words astreoids-------------------##");
            Console.WriteLine("##--------Also avert game border-------------------------------##");
            Console.WriteLine("##-------------------------------------------------------------##");
            Console.WriteLine("##---------Press Esc to Exit-----------------------------------##");
            Console.WriteLine("##-----------------------Press other key to start--------------##");
            Console.WriteLine("#################################################################");
            Console.WriteLine("#################################################################");
            keypress = Console.ReadKey(true);
            if (keypress.Key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }
        }

        void DefaultSettings()
        {
            Console.CursorVisible = false;
            Console.SetWindowPosition(10, 10);
            width = 80;
            height = 20;
            score = 0;
            spawnrate = 16;
            sleeprate = 25;
            ammo = 3;
            charge = 0;
            visible = true;
            ShieldCharge = ShieldChargeDisplay(0);
            Ammo = AmmoDisplay(ammo);
            p = new Player((width / 4) - 5, height / 2);
            Console.SetWindowSize(width, height + 3);
            watch = Stopwatch.StartNew();
            Console.Title = "";
        }

        void GameManagement()
        {
            while (true)
            {
                Input();
                Astreoid();
                p.Move();
                Screen();
                ScoreManagement();
                ColliderCheck();
                FPS();

                Thread.Sleep(sleeprate);
            }
        }

        void ColliderCheck()
        {
            if (p.Body[1, 1] == 0 || p.Body[0, 1] == height - 1)
            {
                Die();
            }

            if (p.fired)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < Astreoids.Count; j++)
                    {
                        if ((p.Blast[i, 0] == Astreoids[j].x || p.Blast[i, 0] == Astreoids[j].x - 1 || p.Blast[i, 0] == Astreoids[j].x - 2 || p.Blast[i, 0] == Astreoids[j].x - 3 || p.Blast[i, 0] == Astreoids[j].x - 4 || p.Blast[i, 0] == Astreoids[j].x - 5 || p.Blast[i, 0] == Astreoids[j].x - 6) && p.Blast[i, 1] == Astreoids[j].y)
                            Astreoids.RemoveAt(j);
                    }
                }
            }

            if (activated)
            {
                for (int j = 0; j < Astreoids.Count; j++)
                {
                    for (int i = 0; i < 17; i++)
                    {
                        if ((p.Shield[i, 0] == Astreoids[j].x || p.Shield[i, 0] == Astreoids[j].x - 1 || p.Shield[i, 0] == Astreoids[j].x - 2 || p.Shield[i, 0] == Astreoids[j].x - 3 || p.Shield[i, 0] == Astreoids[j].x - 4 || p.Shield[i, 0] == Astreoids[j].x - 5 || p.Shield[i, 0] == Astreoids[j].x - 6) && p.Shield[i, 1] == Astreoids[j].y)
                        {
                            Astreoids.RemoveAt(j);
                            j++;
                        }
                    }
                }
            }

            if (!activated)
            {
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < Astreoids.Count; j++)
                    {
                        if (p.Body[i, 0] == Astreoids[j].x && p.Body[i, 1] == Astreoids[j].y && i != 3) 
                           Die();
                    }
                }
            }
        }

        void Screen()
        {
            List<char> screen = new List<char>();
            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y == 0 || y == height - 1)
                        screen.Add('#');
                    else if (x == p.headX && y == p.headY)
                        screen.Add('>');
                    else
                    {
                        isprinted = false;
                        for (int i = 0; i < 17; i++)
                        {
                            if (x == p.Shield[i, 0] && y == p.Shield[i, 1] && activated && visible)
                            {
                                if (i < 5)
                                    screen.Add('-');
                                else if (i == 5)
                                    screen.Add('\\');
                                else if (i == 6)
                                    screen.Add(' ');
                                else if (i < 10)
                                    screen.Add('|');
                                else if (i == 10)
                                    screen.Add(' ');
                                else if (i == 11)
                                    screen.Add('/');
                                else
                                    screen.Add('-');

                                isprinted = true;
                            }

                            if (i < 7 && x == p.TailX[i] && y == p.TailY[i] && !isprinted)
                            {
                                screen.Add('>');
                                isprinted = true;
                            }

                            if (i < 7 && !isprinted)
                            {
                                if (x == p.Body[i, 0] && y == p.Body[i, 1])
                                {
                                    screen.Add('0');
                                    isprinted = true;
                                }
                            }

                            if (i < 5 && p.fired && !isprinted)
                            {
                                if (x == p.Blast[i, 0] && y == p.Blast[i, 1])
                                {
                                    screen.Add(')');
                                    isprinted = true;
                                }
                            }
                        }

                        for (int i = 0; i < Astreoids.Count; i++)
                        {
                            if (x == Astreoids[i].x && y == Astreoids[i].y && !isprinted)
                            {
                                screen.Add('X');
                                isprinted = true;
                            }
                        }
                        if (!isprinted)
                            screen.Add(' ');
                    }
                    if (x == width - 1)
                        screen.Add('\n');
                }
            }
            char[] Screen = screen.ToArray();
            Console.WriteLine(Screen);
            Console.Write("Score: " + score + "           Blast Charges: " + Ammo);
            Console.WriteLine("    Shield Battery ({0})", ShieldCharge);
            Console.Write("Press P to pause");
        }

        void Input()
        {
            while (Console.KeyAvailable)
            {
                keypress = Console.ReadKey(true);
                if (keypress.Key == ConsoleKey.W)
                {
                    p.PlayerMove(-1);
                }
                else if (keypress.Key == ConsoleKey.S)
                {
                    p.PlayerMove(1);
                }
                else if (keypress.Key == ConsoleKey.F && !p.fired && ammo > 0)
                {
                    p.Fire(p.headX, p.headY);
                    ammo--;
                    Ammo = AmmoDisplay(ammo);
                }
                else if (keypress.Key == ConsoleKey.P)
                {
                    Pause();
                }
                else if (keypress.Key == ConsoleKey.Spacebar && charge == 100)
                {
                    charge = 0;
                    ShieldCharge = ShieldChargeDisplay(charge);
                    count = score;
                    activated = true;
                }
            }
        }

        void Astreoid()
        {
            if (score % spawnrate == 0)
            {
                double r;
                bool add = false;
                Astreoid a = new Astreoid(random, true);
                Astreoids.Add(a);
                int x = Astreoids.Count - 1;
                int c = random.Next(0, 7);
                for (int i = 0; i < c; i++)
                {
                    Astreoid b = new Astreoid(random, false);

                    r = random.NextDouble();
                    if (r <= 0.3)
                        b.x = a.x + 1;
                    else if (r <= 0.6)
                        b.x = a.x;
                    else
                        b.x = a.x - 1;
                    r = random.NextDouble();
                    if (r <= 0.3)
                        b.y = a.y - 1;
                    else if (r <= 0.6)
                        b.y = a.y;
                    else
                        b.y = a.y + 1;

                    for (int j = x; j < Astreoids.Count; j++)
                    {
                        if (Astreoids[j].x == b.x && Astreoids[j].y == b.y)
                            add = false;
                        else
                            add = true;
                    }
                    if (add)
                        Astreoids.Add(b);
                }
            }
            for (int i = 0; i < Astreoids.Count; i++)
            {
                Astreoids[i].x--;
                if (Astreoids[i].x == -1)
                    Astreoids.RemoveAt(i);
            }
        }

        void Restart()
        {
            Astreoids.Clear();
            DefaultSettings();
            GameManagement();
        }

        void Die()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("                ---------------------------------------------");
            Console.WriteLine("                ---------------------------------------------");
            Console.WriteLine("                ---------------------------------------------");
            Console.WriteLine("                -------------------You Dİed------------------");
            Console.WriteLine("                ---------------------------------------------");
            Console.WriteLine("                ---------------Your final score: " + score + "--------");
            Console.WriteLine("                ---------------------------------------------");
            Console.WriteLine("                ----------Press R to restart-----------------");
            Console.WriteLine("                ----------------Press Esc to exit------------");
            Console.WriteLine("                ---------------------------------------------");
            Console.WriteLine("                ---------------------------------------------");
            Console.Title = "GG";
            keypress = Console.ReadKey(true);
            if (keypress.Key == ConsoleKey.R)
                Restart();
            else if (keypress.Key == ConsoleKey.Escape)
                Environment.Exit(0);
            else
                Die();
        }

        void Pause()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("                ---------------------------------------------");
            Console.WriteLine("                ---------------------------------------------");
            Console.WriteLine("                ---------------------------------------------");
            Console.WriteLine("                ----------------Game Paused------------------");
            Console.WriteLine("                ---------------------------------------------");
            Console.WriteLine("                ----------Press any key to continue----------");
            Console.WriteLine("                ---------------------------------------------");
            Console.WriteLine("                ---------------------------------------------");
            Console.WriteLine("                ---------------------------------------------");
            Thread.Sleep(1000);
            Console.ReadKey();
            GameManagement();
        }

        void FPS()
        {
            watch.Stop();
            time = watch.ElapsedMilliseconds - sleeprate;
            Console.Write("    FPS: " + 1000 / time);
            watch.Restart();
        }

        void ScoreManagement()
        {
            score++;
            if (score % 300 == 0 && spawnrate != 6)
            {
                spawnrate -= 2;
            }
            if (score % 7 == 0 && charge < 100 && !activated)
            {
                charge++;
                if (charge % 10 == 0)
                {
                    ShieldCharge = ShieldChargeDisplay(charge / 10);
                }
            }
            if (activated && score - count >= 300)
            {
                if (score % 5 == 0)
                {
                    if (visible)
                        visible = false;
                    else
                        visible = true;
                }

                if (score - count == 350)
                    activated = false;
            }
        }

        string AmmoDisplay(int x)
        {
            switch (x)
            {
                case 0:
                    return "_ _ _";
                case 1:
                    return "A _ _";
                case 2:
                    return "A A _";
                case 3:
                    return "A A A";
            }
            return null;
        }

        string ShieldChargeDisplay(int z)
        {
            string x = null;
            for (int i = 0; i < z; i++)
            {
                x = x + 'D';
            }
            for (int i = 0; i < 10 - z; i++)
            {
                x = x + '_';
            }
            return x;
        }


        //dev note: there is annoying glict in movement of astreoids which are close to players y axis. The source of problem still not found :(
    }
}


  
