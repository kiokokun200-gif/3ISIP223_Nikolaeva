using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class Slug : Enemy
    {
        public Slug() : base("Слизень", 35, 5, 3, EnemyType.Slug)
        {

        }

  
        public override void TakeDamage(int damage)
        {
            int reducedDamage = damage - 2;
            CurrentHP -= Math.Max(1, reducedDamage); 
            Console.WriteLine($"Слизень поглотил часть урона! Получено: {Math.Max(1, reducedDamage)} урона");
        }
    }
}
