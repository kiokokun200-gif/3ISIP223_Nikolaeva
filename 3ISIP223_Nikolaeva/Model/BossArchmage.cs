using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class BossArchmage : Enemy
    {
        public BossArchmage() : base("Архимаг C++", Raaandom.GetRandomInt(12, 20), Raaandom.GetRandomInt(5, 15), Raaandom.GetRandomInt(1, 4), EnemyType.BossArchmage)
        {
            FreezeChance = 0.35;
        }
    }
}
