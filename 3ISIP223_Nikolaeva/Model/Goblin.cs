using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class Goblin : Enemy
    {
        public Goblin() : base("Гоблин", rando  , 8, 2, EnemyType.Goblin)
        {
            CriticalChance = 0.2;
        }
    }
}
