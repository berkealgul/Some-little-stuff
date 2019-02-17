using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class DNA
    {
        public float fGeneF, fGeneP, fGene3, fScore,fFoodAR,fPoisonAR;
        Random r;

        static float fMutationRate = 0.05f;

        public DNA(Random r, bool bRandom)
        {
            this.r = r;

            if (bRandom)
            {
                fGeneF = (float)r.NextDouble() * r.Next(-11,11);
                fGeneP = (float)r.NextDouble() * r.Next(-11,11);
                fGene3 = (float)r.NextDouble() * r.Next(-11,11);
                fFoodAR = r.Next(10,400);
                fPoisonAR = r.Next(10,400);
            }                                    
        }

        public DNA Reproduction(DNA parent2)
        {
            DNA child = new DNA(r, false);

            if (r.NextDouble() <= fMutationRate)
            {
                child.fGeneF = (float)r.NextDouble() * r.Next(-11, 11);
                child.fGeneP = (float)r.NextDouble() * r.Next(-11, 11);
                child.fGene3 = (float)r.NextDouble() * r.Next(-11, 11);
                fFoodAR = r.Next(10, 400);
                fPoisonAR = r.Next(10, 400);
            }
            else
            {
                child.fGeneF = (fGeneF + parent2.fGeneF) / 2;
                child.fGeneP = (fGeneP + parent2.fGeneP) / 2;
                child.fGene3 = (fGene3 + parent2.fGene3) / 2;
                child.fFoodAR = (fFoodAR + parent2.fFoodAR) / 2;
                child.fPoisonAR = (fPoisonAR + parent2.fPoisonAR) / 2;
            }

            return child;
        }
    }
}
