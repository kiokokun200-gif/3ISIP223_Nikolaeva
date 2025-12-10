using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class Skeleton : Enemy
    {
        public Skeleton() : base("Скелет", Raaandom.GetRandomInt(10, 15), Raaandom.GetRandomInt(7, 12), Raaandom.GetRandomInt(1, 2), EnemyType.Skeleton)
        {
            IgnoreDefense = true;
        }
    }

}
