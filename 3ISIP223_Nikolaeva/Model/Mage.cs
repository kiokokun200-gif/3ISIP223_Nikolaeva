using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class Mage : Enemy
    {
        public Mage() : base("Маг", Raaandom.GetRandomInt(13, 20), Raaandom.GetRandomInt(10, 15), Raaandom.GetRandomInt(1, 4), EnemyType.Mage)
        {
            FreezeChance = 0.25;
        }
    }
}
