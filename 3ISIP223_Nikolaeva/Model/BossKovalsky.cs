using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class BossKovalsky : Enemy
    {
        public BossKovalsky() : base("Ковальский", Raaandom.GetRandomInt(12, 20), Raaandom.GetRandomInt(8, 12), Raaandom.GetRandomInt(1, 4), EnemyType.BossKovalsky)
        {
            IgnoreDefense = true;
        }
    }

}
