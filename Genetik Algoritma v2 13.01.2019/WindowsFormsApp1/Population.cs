using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class Population
    {
        public List<DNA> DNAs;
        public int Generation;
        public int Population_Size;

        float fTop_score;
        Random rnd;

        public Population(Random rnd,int Population_Size)
        {
            this.rnd = rnd;
            this.Population_Size = Population_Size;
            DNAs = new List<DNA>();
            Generation = 1;

            for (int i = 0; i < Population_Size; i++)
            {
                DNAs.Add(new DNA(rnd, true));
            }
        }

        public void Next_Generation()
        {
            if(DNAs.Count == 0) //if nobody survived
            {
                for (int i = 0; i < Population_Size; i++)
                {
                    DNAs.Add(new DNA(rnd, true));
                }
                return;
            }

            List<DNA> New_Generation = new List<DNA>();
            List<DNA> LuckWheel = new List<DNA>();

            Fitness_Evaluate();
            foreach(DNA dna in DNAs)
            {
                for (int j = 0; j < dna.fScore * 100; j++)
                {
                    LuckWheel.Add(dna);
                }
            }

            while (New_Generation.Count < Population_Size)
            {
                foreach (DNA dna in DNAs)
                {
                    New_Generation.Add(dna.Reproduction(Choose_Parent(LuckWheel)));
                    if (New_Generation.Count == Population_Size)
                        break;
                }
            }

            DNAs = New_Generation;
            Generation++;
        }

        void Fitness_Evaluate()
        {
            fTop_score = 0.0f;
            for (int i = 0; i < DNAs.Count; i++)
            {
                if (DNAs[i].fScore > fTop_score)
                    fTop_score = DNAs[i].fScore;
            }

            for (int i = 0; i < DNAs.Count; i++)
            {
                DNAs[i].fScore /= fTop_score;
            }
        }

        DNA Choose_Parent(List<DNA> LuckWheel)
        {
            return LuckWheel[rnd.Next(0, LuckWheel.Count)];
        }
    }
}
