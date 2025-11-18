using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class Skeleton : Enemy
    {
        public Skeleton() : base("Скелет", 25, 10, 3, EnemyType.Skeleton)
        {
            IgnoreDefense = true;
        }
    }

}
