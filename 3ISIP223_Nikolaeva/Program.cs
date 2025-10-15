using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }

        class Player
        {
            public double HP { get; set; }
            public string Name { get; set; }
            public double AttackWeapon { get; set; }
            public double Armor { get; set; }
            public bool IsEvade { get; set; }
            public bool IsBlock { get; set; }
            public bool IsFrozen { get; set; }
            public bool IsAlive => HP > 0;
            public int CountMurders { get; set; }


            public Player(string name)
            {
                HP = 100;
                AttackWeapon = 20;
                Armor = 40;
                Name = name;
                CountMurders = 0;
                IsEvade = false;
                IsBlock = false;
                IsFrozen = false;
            }

            public void InfoWeapon()
            {

            }
            public void InfoAfterDeath()
            {

            }
            public void InfoArmor()
            {

            }
        }

        class Enemy
        {
            public string Name { get; set; }
            public double HP { get; set; }
            public double Attack { get; set; }
            public double Defense { get; set; }
            public bool HaveDefense => Defense > 0;
            public bool IsAlive => HP > 0;
            public Enemy(string name, double attack, double defense)
            {
                HP = 100.0;
                Name = name;
                Attack = attack;
                Defense = defense;
            }

            public virtual void AttackInfo()
            {
                Console.WriteLine("VragAtrackuet");
            }
        }

        class Goblin : Enemy
        {
            public Random random = new Random();
            public double ProcentKritAttack { get; set; }
            public bool KritAttack => random.Next(0, 101) <= ProcentKritAttack;

            public Goblin() : base("Гоблин", 40, 20)
            {
                ProcentKritAttack = 15;
            }
            public override void AttackInfo()
            {
                Console.WriteLine($"Goblin attackuet. {(KritAttack ? "KritAttack" : "ObichnAttack")}.");
            }
        }
        class Skeleton : Enemy
        {
            public Skeleton() : base("Скелет", 35, 30)
            {

            }

            public override void AttackInfo()
            {
                Console.WriteLine("Skelet attackuet. Ignor zashitu.");
            }
        }
        class Magician : Enemy
        {
            public Random random = new Random();
            public double ProcentFrozen { get; set; }
            public bool Frozen => random.Next(0, 101) <= ProcentFrozen;
            public Magician() : base("Маг", 40, 15)
            {
                ProcentFrozen = 25;
            }
            public override void AttackInfo()
            {
                if (!Frozen) Console.WriteLine("Маг атакует вас! Он бросает ледяную стрелу...");
                else Console.WriteLine("Маг пытается заморозить вас... У него получилось! Вы пропустите следующий ход.");
            }
        }
    }
}
