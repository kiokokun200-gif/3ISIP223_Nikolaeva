using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class BossVvg : Enemy
    {
        public BossVvg() : base("ВВГ", Raaandom.GetRandomInt(12, 18), Raaandom.GetRandomInt(9, 12), Raaandom.GetRandomInt(1, 4), EnemyType.BossVvg)
        {
            CriticalChance = 0.3;
        }
    }
}
