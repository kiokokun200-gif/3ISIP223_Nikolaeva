using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal static class Raaandom
    {
        static Random random = new Random();

        static public int GetRandomInt(int min, int max)
        {
            return random.Next(min, max);
        }

        static public double GetRandomDouble()
        {
            return random.NextDouble();
        }
    }
}
