using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class FactoryCreate : Factory
    {
        public List<Factory> mob;
        public List<Factory> boss;
        public FactoryCreate() {
            mob = new List<Factory>();
            mob.Add(new FactorySlug());
            mob.Add(new MageFactory());
            mob.Add(new SkeletonFactory());
            mob.Add(new GoblinFactory());
        }

        //public override Enemy CreateEnemy()
        //{
        //    //return mob; //mob[n]
        //}
    }
}
