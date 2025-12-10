using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class Enemy

    {
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public EnemyType Type { get; set; }

        public double CriticalChance { get; set; } = 0;
        public double FreezeChance { get; set; } = 0;
        public bool IgnoreDefense { get; set; } = false;

        public Enemy(string name, int hp, int attack, int defense, EnemyType type)
        {
            Name = name;
            MaxHP = hp;
            CurrentHP = hp;
            Attack = attack;
            Defense = defense;
            Type = type;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"{Name} - HP: {CurrentHP}/{MaxHP}, Атака: {Attack}, Защита: {Defense}");
        }

        public virtual int CalculateDamage(int playerDefense)
        {
            int damage = Attack;

            // Критический удар
            if (Raaandom.GetRandomDouble() < CriticalChance)
            {
                damage *= 2;
                Console.WriteLine("Критический удар!");
            }

            // Игнор защиты
            if (IgnoreDefense)
            {
                return damage;
            }

            // Учет защиты игрока
            int actualDefense = Math.Min(playerDefense, damage);
            damage -= actualDefense;

            return Math.Max(1, damage);
        }

        public virtual bool TryFreeze()
        {
            return Raaandom.GetRandomDouble() < FreezeChance;
        }

        public virtual void TakeDamage(int damage)
        {
            CurrentHP -= damage;
        }
    }
}
