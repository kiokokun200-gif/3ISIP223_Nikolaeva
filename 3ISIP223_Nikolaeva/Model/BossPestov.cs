using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class BossPestov : Enemy
    {
        public BossPestov() : base("Пестов С--", 26, 22, 1, EnemyType.BossPestov)
        {
            IgnoreDefense = true;
            FreezeChance = 0.4;
        }
    }

}
