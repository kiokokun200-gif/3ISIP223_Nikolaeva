using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class Item
    {
        public string Name { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }

        public Item(string name, int attack, int defense)
        {
            Name = name;
            Attack = attack;
            Defense = defense;
        }

        public void DisplayStats()
        {
            Console.WriteLine($"{Name} (Атака: {Attack}, Защита: {Defense})");
        }
    }
}
