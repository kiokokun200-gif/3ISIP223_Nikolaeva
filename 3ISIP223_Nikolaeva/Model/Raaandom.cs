using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal class Raaandom
    {
        Random random = new Random();

        public int GetRand(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}
