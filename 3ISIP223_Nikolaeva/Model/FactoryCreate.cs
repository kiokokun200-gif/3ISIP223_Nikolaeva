using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class FactoryCreate
    {
        public List<Factory> mob;
        public List<Factory> boss;
        public FactoryCreate() {
            mob = new List<Factory>();
            mob.Add(new FactorySlug());
            mob.Add(new MageFactory());
            mob.Add(new SkeletonFactory());
            mob.Add(new GoblinFactory());

            boss = new List<Factory>();
            boss.Add(new FactoryBossArchmage());
            boss.Add(new FactoryBossKovalsky());
            boss.Add(new FactoryBossPestov());
            boss.Add(new FactoryBossVvg());

        }

        public Factory CreateMob(int n)
        {
            return mob[n]; //mob[n]
        }

        public Factory CreateBoss(int n) { return boss[n]; }
    }
}
