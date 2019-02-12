using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace new_game
{
    class Astreoid
    {
        public int x, y;
      
        public Astreoid(Random random, bool randomed)
        {
            x = 85;
            if(randomed)
            y = random.Next(2, 18);
        }
    }
}
