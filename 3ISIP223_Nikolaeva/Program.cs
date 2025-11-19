using System;
using System.Collections.Generic;
using _3ISIP223_Nikolaeva.Model;

namespace _3ISIP223_Nikolaeva
{
    // Перечисление типов врагов

    public class Game
    {
        private Raaandom random = new Raaandom();

        // Игрок
        public int PlayerHP { get; set; } = 100;
        public int MaxPlayerHP { get; set; } = 100;
        public Item CurrentWeapon { get; set; }
        public Item CurrentArmor { get; set; }

        // Статистика
        public int Turn { get; set; } = 0;
        public bool IsFrozen { get; set; } = false;

        // Предметы для сундуков
        private List<Item> weapons = new List<Item>
        {
            new Item("Небесная ось", 5, 0),
            new Item("Волчья погибель", 10, 0),
            new Item("Аква Симулякрум", 15, 0),
            new Item("Нефритовый коршун", 12, 3),
            new Item("Посох Хомы", 20, 5)
        };

        private List<Item> armors = new List<Item>
        {
            new Item("Кожаная броня", 0, 5),
            new Item("Кольчуга", 0, 10),
            new Item("Латные доспехи", 0, 15),
            new Item("Волшебная мантия", 3, 8),
            new Item("Легендарные доспехи", 5, 20)
        };

        public Game()
        {
            // Начальная экипировка
            CurrentWeapon = new Item("Дубина переговоров", 2, 0);
            CurrentArmor = new Item("Одежда", 0, 1);
        }

        public void StartGame()
        {
            Console.WriteLine("Добро пожаловать в текстовый рогалик!");
            Console.WriteLine("Ваша цель - выживать как можно дольше.");
            Console.WriteLine();

            while (PlayerHP > 0)
            {
                Turn++;
                Console.WriteLine($"=== Ход {Turn} ===");
                Console.WriteLine($"Ваше HP: {PlayerHP}/{MaxPlayerHP}");
                Console.WriteLine($"Оружие: {CurrentWeapon.Name} (Атака: {CurrentWeapon.Attack})");
                Console.WriteLine($"Доспехи: {CurrentArmor.Name} (Защита: {CurrentArmor.Defense})");
                Console.WriteLine();

                // Каждые 10 ходов - босс
                if (Turn % 10 == 0)
                {
                    Console.WriteLine("!!! Появляется БОСС !!!");
                    Enemy boss = GenerateBoss();
                    Combat(boss);
                }
                else
                {
                    // Случайное событие: 50% враг, 50% сундук
                    if (random.Next(2) == 0)
                    {
                        Enemy enemy = GenerateEnemy();
                        Combat(enemy);
                    }
                    else
                    {
                        OpenChest();
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }

            GameOver();
        }

        private Enemy GenerateEnemy()
        {
            int enemyType = random.Next(3);
            switch (enemyType)
            {
                case 0:
                    return new Goblin();
                case 1:
                    return new Skeleton();
                case 2:
                    return new Mage();
                default:
                    return new Goblin();
            }
        }

        private Enemy GenerateBoss()
        {
            int bossType = random.Next(4);
            switch (bossType)
            {
                case 0:
                    return new BossVvg();
                case 1:
                    return new BossKovalsky();
                case 2:
                    return new BossArchmage();
                case 3:
                    return new BossPestov();
                default:
                    return new BossVvg();
            }
        }

        private void Combat(Enemy enemy)
        {
            Console.WriteLine($"Вы встретили: {enemy.Name}");
            enemy.DisplayInfo();
            Console.WriteLine();

            bool playerTurn = true;
            bool defending = false;

            while (enemy.CurrentHP > 0 && PlayerHP > 0)
            {
                if (playerTurn)
                {
                    if (IsFrozen)
                    {
                        Console.WriteLine("Вы заморожены и пропускаете ход!");
                        IsFrozen = false;
                        playerTurn = false;
                        continue;
                    }

                    PlayerTurn(enemy, ref defending);
                }
                else
                {
                    EnemyTurn(enemy, ref defending);
                }

                playerTurn = !playerTurn;
            }

            if (PlayerHP > 0)
            {
                Console.WriteLine($"Вы победили {enemy.Name}!");
            }
        }

        private void PlayerTurn(Enemy enemy, ref bool defending)
        {
            Console.WriteLine("Ваш ход:");
            Console.WriteLine("1 - Атаковать");
            Console.WriteLine("2 - Защищаться");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                int damage = CurrentWeapon.Attack;
                enemy.CurrentHP -= damage;
                Console.WriteLine($"Вы нанесли {damage} урона {enemy.Name}!");
                defending = false;
            }
            else if (choice == "2")
            {
                Console.WriteLine("Вы готовитесь к защите...");
                defending = true;
            }
            else
            {
                Console.WriteLine("Неверный выбор, вы пропускаете ход!");
            }
        }

        private void EnemyTurn(Enemy enemy, ref bool defending)
        {
            Console.WriteLine($"Ход {enemy.Name}:");

            int damage = enemy.CalculateDamage(random, CurrentArmor.Defense);
            bool dodged = false;

            // Проверка уклонения при защите
            if (defending && random.NextDouble() < 0.4)
            {
                dodged = true;
                Console.WriteLine("Вы успешно уклонились от атаки!");
            }

            if (!dodged)
            {
                // Блокирование урона
                if (defending)
                {
                    double blockPercent = 0.7 + (random.NextDouble() * 0.3); // 70-100%
                    int blockedDamage = (int)(damage * blockPercent);
                    damage -= blockedDamage;
                    Console.WriteLine($"Вы заблокировали {blockedDamage} урона!");
                }

                PlayerHP -= damage;
                Console.WriteLine($"{enemy.Name} наносит вам {damage} урона!");

                // Проверка заморозки
                if (enemy.TryFreeze(random))
                {
                    IsFrozen = true;
                    Console.WriteLine("Враг заморозил вас! Вы пропустите следующий ход.");
                }
            }

            defending = false;
        }

        private void OpenChest()
        {
            Console.WriteLine("Вы нашли сундук!");
            int chestContent = random.Next(3);

            switch (chestContent)
            {
                case 0: // Зелье здоровья
                    PlayerHP = MaxPlayerHP;
                    Console.WriteLine("Вы нашли зелье здоровья! Ваше HP полностью восстановлено!");
                    break;

                case 1: // Оружие
                    Item newWeapon = weapons[random.Next(weapons.Count)];
                    Console.WriteLine("Вы нашли новое оружие:");
                    newWeapon.DisplayStats();
                    Console.WriteLine("Ваше текущее оружие:");
                    CurrentWeapon.DisplayStats();
                    OfferItem(newWeapon, true);
                    break;

                case 2: // Доспехи
                    Item newArmor = armors[random.Next(armors.Count)];
                    Console.WriteLine("Вы нашли новые доспехи:");
                    newArmor.DisplayStats();
                    Console.WriteLine("Ваши текущие доспехи:");
                    CurrentArmor.DisplayStats();
                    OfferItem(newArmor, false);
                    break;
            }
        }

        private void OfferItem(Item newItem, bool isWeapon)
        {
            Console.Write("Хотите взять этот предмет? (д/н): ");
            string choice = Console.ReadLine().ToLower();

            if (choice == "д" || choice == "y")
            {
                if (isWeapon)
                {
                    CurrentWeapon = newItem;
                    Console.WriteLine($"Вы экипировали {newItem.Name}!");
                }
                else
                {
                    CurrentArmor = newItem;
                    Console.WriteLine($"Вы экипировали {newItem.Name}!");
                }
            }
            else
            {
                Console.WriteLine("Вы оставили предмет в сундуке.");
            }
        }

        private void GameOver()
        {
            Console.Clear();
            Console.WriteLine("=== ИГРА ОКОНЧЕНА ===");
            Console.WriteLine($"Вы продержались {Turn} ходов!");
            Console.WriteLine("Спасибо за игру!");
        }
    }


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