using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva.Model
{
    internal public class Game
    {
        // Игрок
        public int PlayerHP { get; set; } = 100;
        public int MaxPlayerHP { get; set; } = 100;
        public Item CurrentWeapon { get; set; }
        public Item CurrentArmor { get; set; }

        // Статистика
        public int Turn { get; set; } = 0;
        public bool IsFrozen { get; set; } = false;

        // Фабрика для создания врагов
        private FactoryCreate factoryCreate = new FactoryCreate();

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
                    if (Raaandom.GetRandomInt(0, 1) == 0)
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
            // Выбираем случайную фабрику из списка обычных мобов
            int randomIndex = Raaandom.GetRandomInt(0, factoryCreate.mob.Count - 1);
            Factory selectedFactory = factoryCreate.CreateMob(randomIndex);

            // Используем фабрику для создания врага
            Enemy enemy = selectedFactory.CreateEnemy();
            Console.WriteLine($"Появился: {enemy.Name} (HP: {enemy.MaxHP}, Атака: {enemy.Attack}, Защита: {enemy.Defense})");
            return enemy;
        }

        private Enemy GenerateBoss()
        {
            // Выбираем случайную фабрику из списка боссов
            int randomIndex = Raaandom.GetRandomInt(0, factoryCreate.boss.Count - 1);
            Factory selectedFactory = factoryCreate.CreateBoss(randomIndex);

            // Используем фабрику для создания босса
            Enemy boss = selectedFactory.CreateEnemy();
            Console.WriteLine($"!!! БОСС {boss.Name} !!! (HP: {boss.MaxHP}, Атака: {boss.Attack}, Защита: {boss.Defense})");
            return boss;
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
                enemy.TakeDamage(damage);
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

            int damage = enemy.CalculateDamage(CurrentArmor.Defense);
            bool dodged = false;

            // Проверка уклонения при защите
            if (defending && Raaandom.GetRandomDouble() < 0.4)
            {
                dodged = true;
                Console.WriteLine("Вы успешно уклонились от атаки!");
            }

            if (!dodged)
            {
                // Блокирование урона
                if (defending)
                {
                    double blockPercent = 0.7 + (Raaandom.GetRandomDouble() * 0.3); // 70-100%
                    int blockedDamage = (int)(damage * blockPercent);
                    damage -= blockedDamage;
                    Console.WriteLine($"Вы заблокировали {blockedDamage} урона!");
                }

                PlayerHP -= damage;
                Console.WriteLine($"{enemy.Name} наносит вам {damage} урона!");

                // Проверка заморозки
                if (enemy.TryFreeze())
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
            int chestContent = Raaandom.GetRandomInt(0, 2);

            switch (chestContent)
            {
                case 0: // Зелье здоровья
                    PlayerHP = MaxPlayerHP;
                    Console.WriteLine("Вы нашли зелье здоровья! Ваше HP полностью восстановлено!");
                    break;

                case 1: // Оружие
                    Item newWeapon = weapons[Raaandom.GetRandomInt(0, weapons.Count - 1)];
                    Console.WriteLine("Вы нашли новое оружие:");
                    newWeapon.DisplayStats();
                    Console.WriteLine("Ваше текущее оружие:");
                    CurrentWeapon.DisplayStats();
                    OfferItem(newWeapon, true);
                    break;

                case 2: // Доспехи
                    Item newArmor = armors[Raaandom.GetRandomInt(0, armors.Count - 1)];
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
}
