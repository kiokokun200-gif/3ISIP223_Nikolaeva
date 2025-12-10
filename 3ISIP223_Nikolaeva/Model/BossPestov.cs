using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class BossPestov : Enemy
    {
        public BossPestov() : base("Пестов С--", Raaandom.GetRandomInt(12, 20), Raaandom.GetRandomInt(12, 15), Raaandom.GetRandomInt(1, 4), EnemyType.BossPestov)
        {
            IgnoreDefense = true;
            FreezeChance = 0.4;
        }
    }

}
