using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class Mage : Enemy
    {
        public Mage() : base("Маг", 20, 12, 1, EnemyType.Mage)
        {
            FreezeChance = 0.25;
        }
    }
}
