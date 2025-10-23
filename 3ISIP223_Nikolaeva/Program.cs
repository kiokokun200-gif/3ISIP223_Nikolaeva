using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoServiceGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== АВТОСЕРВИС 'ПРОФЕССИОНАЛ' ===");
            
            try
            {
                GameManager.StartGame();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
                Console.ReadLine();
            }
        }
    }

    public static class GameManager
    {
        private static Service service;
        private static List<Part> parts;
        private static Random random = new Random();

        public static void StartGame()
        {
            LoadGameData();
            MainGameLoop();
        }

        private static void LoadGameData()
        {
            using (var context = Core.Context)
            {
                service = context.Service.First();
                parts = context.Part.ToList();
                
                Console.WriteLine("Данные успешно загружены!");
                Console.WriteLine($"Баланс: {service.Balance:C0}");
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        private static void MainGameLoop()
        {
            while (true)
            {
                Console.Clear();
                ShowMainMenu();
                
                var choice = ReadPositiveInt("Выберите действие: ");
                
                switch (choice)
                {
                    case 1:
                        ServeNextCustomer();
                        break;
                    case 2:
                        ShowPurchaseMenu();
                        break;
                    case 3:
                        ShowInventory();
                        break;
                    case 4:
                        Console.WriteLine("Выход из игры...");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void ShowMainMenu()
        {
            Console.WriteLine($"\n=== АВТОСЕРВИС ===");
            Console.WriteLine($"День: {service.Current_day} | Баланс: {service.Balance:C0}");
            Console.WriteLine("1. Обслужить следующего клиента");
            Console.WriteLine("2. Закупить запчасти");
            Console.WriteLine("3. Показать склад");
            Console.WriteLine("4. Выйти из игры");
        }

        private static void ServeNextCustomer()
        {
            Console.WriteLine("\n=== НОВЫЙ КЛИЕНТ ===");
            
            using (var context = new nico_carServiceEntities())
            {
                var brokenPart = parts[random.Next(parts.Count)];
                int repairCost = brokenPart.Sell_price;
                
                Console.WriteLine($"У клиента сломался: {brokenPart.Name}");
                Console.WriteLine($"Стоимость ремонта: {repairCost:C0}");
                Console.WriteLine($"На складе есть: {GetPartQuantity(brokenPart.Part_ID)} шт.");
                
                Console.WriteLine("\n1. Принять заказ");
                Console.WriteLine("2. Отказать");
                
                var choice = ReadPositiveInt("Ваш выбор: ");
                
                if (choice == 1)
                {
                    AcceptOrder(context, brokenPart, repairCost);
                }
                else if (choice == 2)
                {
                    RefuseOrder(context);
                }
                else
                {
                    Console.WriteLine("Неверный выбор!");
                }
                
                context.SaveChanges();
                UpdateGameDay();
                
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        private static int GetPartQuantity(int partId)
        {
            using (var context = new nico_carServiceEntities())
            {
                return context.Part_Inventorry
                    .Where(pi => pi.Part_ID == partId && pi.Quantity > 0)
                    .Sum(pi => pi.Quantity);
            }
        }

        private static void AcceptOrder(nico_carServiceEntities context, Part brokenPart, int repairCost)
        {
            int availableQuantity = GetPartQuantity(brokenPart.Part_ID);
            
            if (availableQuantity > 0)
            {
                UsePartFromInventory(brokenPart.Part_ID);
                service.Balance += repairCost;
                service.Count_clients++;
                
                Console.WriteLine($"Ремонт выполнен успешно! Клиент заплатил {repairCost:C0}");
            }
            else
            {
                Console.WriteLine("Нужной детали нет на складе! Производим замену другой деталью...");
                PerformWrongReplacement(context, brokenPart, repairCost);
            }
        }

        private static void UsePartFromInventory(int partId)
        {
            using (var context = new nico_carServiceEntities())
            {
                var partInventory = context.Part_Inventorry
                    .FirstOrDefault(pi => pi.Part_ID == partId && pi.Quantity > 0);
                    
                if (partInventory != null)
                {
                    partInventory.Quantity--;
                    if (partInventory.Quantity == 0)
                    {
                        context.Part_Inventorry.Remove(partInventory);
                    }
                    context.SaveChanges();
                }
            }
        }

        private static void PerformWrongReplacement(nico_carServiceEntities context, Part brokenPart, int repairCost)
        {
            var availablePart = context.Part_Inventorry
                .Where(pi => pi.Quantity > 0 && pi.Part_ID != brokenPart.Part_ID)
                .Select(pi => pi.Part)
                .FirstOrDefault();
                
            if (availablePart != null)
            {
                UsePartFromInventory(availablePart.Part_ID);
                
                int penalty = repairCost * 2;
                service.Balance -= penalty;
                
                Console.WriteLine($"Клиент возмущен! Вы поставили {availablePart.Name} вместо {brokenPart.Name}");
                Console.WriteLine($"Выплачен штраф: {penalty:C0}");
            }
            else
            {
                Console.WriteLine("На складе нет вообще никаких деталей! Штраф увеличен.");
                int penalty = repairCost * 3;
                service.Balance -= penalty;
                Console.WriteLine($"Выплачен штраф: {penalty:C0}");
            }
        }

        private static void RefuseOrder(nico_carServiceEntities context)
        {
            int fine = 50;
            service.Balance -= fine;
            Console.WriteLine($"Вы отказали клиенту. Штраф: {fine:C0}");
        }

        private static void UpdateGameDay()
        {
            service.Current_day++;
            
            using (var context = new nico_carServiceEntities())
            {
                var currentService = context.Service.First();
                currentService.Current_day = service.Current_day;
                currentService.Balance = service.Balance;
                currentService.Count_clients = service.Count_clients;
                
                var arrivingOrders = context.Purchase_Queue
                    .Where(pq => pq.Days_to_arrive <= 0)
                    .ToList();
                    
                foreach (var order in arrivingOrders)
                {
                    AddPartToInventory(order.Part_ID, order.Quantity);
                    context.Purchase_Queue.Remove(order);
                    Console.WriteLine($"Поставка прибыла: {order.Quantity} шт. {GetPartName(order.Part_ID)}");
                }
                
                var otherOrders = context.Purchase_Queue.ToList();
                foreach (var order in otherOrders)
                {
                    order.Days_to_arrive--;
                }
                
                context.SaveChanges();
            }
        }

        private static void AddPartToInventory(int partId, int quantity)
        {
            using (var context = new nico_carServiceEntities())
            {
                var existingInventory = context.Part_Inventorry
                    .FirstOrDefault(pi => pi.Part_ID == partId);
                    
                if (existingInventory != null)
                {
                    existingInventory.Quantity += quantity;
                }
                else
                {
                    var newInventory = new Inventory
                    {
                        PurchaseDate = DateTime.Now,
                        Cost = 0
                    };
                    context.Inventory.Add(newInventory);
                    context.SaveChanges();
                    
                    var partInventory = new Part_Inventorry
                    {
                        Part_ID = partId,
                        Inventory_ID = newInventory.Inventory_ID,
                        Quantity = quantity
                    };
                    context.Part_Inventorry.Add(partInventory);
                }
                
                context.SaveChanges();
            }
        }

        private static string GetPartName(int partId)
        {
            using (var context = new nico_carServiceEntities())
            {
                return context.Part.First(p => p.Part_ID == partId).Name;
            }
        }

        private static void ShowPurchaseMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ЗАКУПКА ЗАПЧАСТЕЙ ===");
                Console.WriteLine($"Баланс: {service.Balance:C0}\n");
                
                using (var context = new nico_carServiceEntities())
                {
                    for (int i = 0; i < parts.Count; i++)
                    {
                        var part = parts[i];
                        int inStock = GetPartQuantity(part.Part_ID);
                        Console.WriteLine($"{i + 1}. {part.Name} | Цена: {part.Puchase_price:C0} | На складе: {inStock} шт.");
                    }
                    
                    Console.WriteLine($"\n{parts.Count + 1}. Назад");
                    
                    var choice = ReadPositiveInt("\nВыберите деталь для закупки: ");
                    
                    if (choice == parts.Count + 1)
                        break;
                    
                    if (choice > 0 && choice <= parts.Count)
                    {
                        var selectedPart = parts[choice - 1];
                        PurchasePart(context, selectedPart);
                    }
                    else
                    {
                        Console.WriteLine("Неверный выбор!");
                        Console.ReadKey();
                    }
                }
            }
        }

        private static void PurchasePart(nico_carServiceEntities context, Part part)
        {
            Console.WriteLine($"\nЗакупка: {part.Name}");
            Console.WriteLine($"Цена за штуку: {part.Puchase_price:C0}");
            Console.WriteLine($"Ваш баланс: {service.Balance:C0}");
            
            int quantity = ReadPositiveInt("Сколько штук закупить? ");
            int totalCost = part.Puchase_price * quantity;
            
            if (totalCost > service.Balance)
            {
                Console.WriteLine("Недостаточно денег для закупки!");
                Console.ReadKey();
                return;
            }
            
            service.Balance -= totalCost;
            
            var purchaseOrder = new Purchase_Queue
            {
                Part_ID = part.Part_ID,
                Quantity = quantity,
                Days_to_arrive = 2
            };
            context.Purchase_Queue.Add(purchaseOrder);
            
            // Обновляем баланс в базе данных
            var currentService = context.Service.First();
            currentService.Balance = service.Balance;
            
            context.SaveChanges();
            
            Console.WriteLine($"Заказ оформлен! Поставка прибудет через 2 клиента. Списано: {totalCost:C0}");
            Console.ReadKey();
        }

        private static void ShowInventory()
        {
            Console.Clear();
            Console.WriteLine("=== СКЛАД ===");
            
            using (var context = new nico_carServiceEntities())
            {
                var inventory = context.Part_Inventorry
                    .Where(pi => pi.Quantity > 0)
                    .GroupBy(pi => pi.Part)
                    .Select(g => new { Part = g.Key, Total = g.Sum(pi => pi.Quantity) })
                    .ToList();

                if (!inventory.Any())
                {
                    Console.WriteLine("Склад пуст!");
                }
                else
                {
                    foreach (var item in inventory)
                    {
                        Console.WriteLine($"  {item.Part.Name}: {item.Total} шт.");
                    }
                }
                
                var pendingOrders = context.Purchase_Queue.ToList();
                if (pendingOrders.Any())
                {
                    Console.WriteLine("\nОжидаются поставки:");
                    foreach (var order in pendingOrders)
                    {
                        var part = context.Part.First(p => p.Part_ID == order.Part_ID);
                        Console.WriteLine($"  {part.Name}: {order.Quantity} шт. (через {order.Days_to_arrive} клиента(ов))");
                    }
                }
            }
            
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        public static int ReadPositiveInt(string message)
        {
            while (true)
            {
                Console.Write(message);
                if (int.TryParse(Console.ReadLine(), out int result) && result > 0)
                    return result;
                Console.WriteLine("Ошибка! Введите положительное число.");
            }
        }
    }

    public static class Core
    {
        public static nico_carServiceEntities Context { get; } = new nico_carServiceEntities();
    }
}