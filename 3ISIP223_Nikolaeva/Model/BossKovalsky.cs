using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class BossKovalsky : Enemy
    {
        public BossKovalsky() : base("Ковальский", 63, 13, 4, EnemyType.BossKovalsky)
        {
            IgnoreDefense = true;
        }
    }

}
