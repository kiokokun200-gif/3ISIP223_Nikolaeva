using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class FactoryBossArchmage : Factory
    {
        public override Enemy CreateEnemy()
        {
            return new BossArchmage();
        }
    
    }
}
