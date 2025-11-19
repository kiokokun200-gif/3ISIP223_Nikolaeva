using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class BossArchmage : Enemy
    {
        public BossArchmage() : base("Архимаг C++", rando, 19, 1, EnemyType.BossArchmage)
        {
            FreezeChance = 0.35;
        }
    }
}
