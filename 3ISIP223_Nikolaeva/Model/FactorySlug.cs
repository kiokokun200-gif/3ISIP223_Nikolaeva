using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class FactorySlug : Factory
    {
        public override Enemy CreateEnemy()
        {
            return new Slug();
        }
    }
}
