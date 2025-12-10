using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class Goblin : Enemy
    {
        public Goblin() : base("Гоблин", Raaandom.GetRandomInt(10, 15), Raaandom.GetRandomInt(5, 8), Raaandom.GetRandomInt(1, 4), EnemyType.Goblin)
        {
            CriticalChance = 0.2;
        }
    }
}
