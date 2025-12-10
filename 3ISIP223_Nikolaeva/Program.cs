using System;
using System.Collections.Generic;
using _3ISIP223_Nikolaeva.Model;

namespace _3ISIP223_Nikolaeva
{
    

    // Главный класс программы
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Game game = new Game();
                game.StartGame();

                Console.WriteLine();
                Console.Write("Хотите сыграть еще раз? (д/н): ");
                string choice = Console.ReadLine().ToLower();

                if (choice != "д" && choice != "y")
                {
                    break;
                }

                Console.Clear();
            }
        }
    }
}