using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ShopZapchast shop = new ShopZapchast
            {
                ID_ShopZapchast = 4,
                Name = "kldsjf",
                Price = 12,
                Count = 1
            };

            Core.Context.ShopZapchast.Add(shop);
            Core.Context.SaveChanges();



            
        }
    }
}
