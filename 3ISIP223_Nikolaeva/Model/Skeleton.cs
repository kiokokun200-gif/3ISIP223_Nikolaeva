using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class Skeleton : Enemy
    {
        public Skeleton() : base("Скелет", Raaandom.GetRandomInt(20, 26), Raaandom.GetRandomInt(7, 12), Raaandom.GetRandomInt(1, 7), EnemyType.Skeleton)
        {
            IgnoreDefense = true;
        }
    }

}
