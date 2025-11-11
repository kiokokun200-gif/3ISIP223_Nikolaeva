using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace пр7
{
    public class Game
    {
        public List<Client> clients { get; private set; }
        public List<Detail> details { get; private set; }
        public Random rand { get; private set; }
        public double balance { get; set; }
        public double refuseClient { get; set; }
        public double Fine { get; set; }

        public void Initialize()
        {
            clients = Core.Context.Client.ToList();
            details = Core.Context.Detail.ToList();
            rand = new Random();
            balance = 1000;
            refuseClient = 50;
            Fine = 100;
        }

        public void ShowDetails()
        {
            Console.Clear();
            Console.WriteLine("ДЕТАЛИ НА СКЛАДЕ");
            Console.WriteLine("=================");
            foreach (var detail in details)
            {
                Console.WriteLine($"{detail.Name}");
                Console.WriteLine($"Количество: {detail.Quantity} шт.");
                Console.WriteLine("-----------------");
            }
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        public void ServeClient()
        {
            Console.Clear();
            Client currentClient = clients[rand.Next(clients.Count)];
            Detail brokenDetail = details[rand.Next(details.Count)];

            bool clientServed = false;
            while (!clientServed)
            {
                Console.Clear();
                Console.WriteLine("ОБСЛУЖИВАНИЕ КЛИЕНТА");
                Console.WriteLine("====================");
                Console.WriteLine($"Клиент: {currentClient.Name}");
                Console.WriteLine($"Сломалась: {brokenDetail.Name}");
                Console.WriteLine($"Стоимость ремонта: {brokenDetail.SellPrice} руб.");
                Console.WriteLine("-----------------");
                Console.WriteLine("1. Обслужить клиента");
                Console.WriteLine("2. Проверить наличие детали на складе");
                Console.WriteLine("3. Отказать клиенту");
                Console.WriteLine("4. Вернуться в главное меню");
                Console.Write("Выберите действие: ");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Repair(brokenDetail, currentClient);
                        clientServed = true;
                        break;
                    case 2:
                        if (!CheckDetail(brokenDetail))
                        {
                            Console.WriteLine("\n-----------------");
                            Console.WriteLine("1. Заказать деталь");
                            Console.WriteLine("2. Отказать клиенту");
                            Console.WriteLine("3. Продолжить обслуживание этого клиента");
                            Console.Write("Выберите действие: ");
                            int choice2 = int.Parse(Console.ReadLine());
                            switch (choice2)
                            {
                                case 1:
                                    PurchaseDetail(brokenDetail);
                                    break;
                                case 2:
                                    RefuseClient(currentClient);
                                    clientServed = true;
                                    break;
                                case 3:
                                    // Остаемся с тем же клиентом
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                        }
                        break;
                    case 3:
                        RefuseClient(currentClient);
                        clientServed = true;
                        break;
                    case 4:
                        clientServed = true;
                        break;
                }
            }

            UpdatePurchases();
        }

        public void RefuseClient(Client client)
        {
            Console.WriteLine($"Отказ в обслуживании клиента {client.Name}. Штраф: {refuseClient} руб.");
            balance -= refuseClient;

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        public bool CheckDetail(Detail detail)
        {
            if (detail.Quantity > 0)
            {
                Console.WriteLine($"\nДеталь '{detail.Name}' доступна в количестве {detail.Quantity} шт.");
                return true;
            }
            else
            {
                Console.WriteLine($"\nДеталь '{detail.Name}' недоступна на складе!");
                return false;
            }
        }

        public void PurchaseDetail(Detail detail)
        {
            Console.Clear();
            Console.WriteLine("ЗАКУПКА ДЕТАЛЕЙ");
            Console.WriteLine("================");
            Console.WriteLine($"Деталь: {detail.Name}");
            Console.WriteLine($"Цена за штуку: {detail.PurchasePrice} руб.");
            Console.Write("Укажите количество деталей: ");
            int quantity = int.Parse(Console.ReadLine());

            double total_coast = detail.PurchasePrice * quantity;
            balance -= total_coast;
            int newPurchaseId = Core.Context.Purchase.Any() ? Core.Context.Purchase.Max(p => p.ID_Purchase) + 1 : 1;
            Purchase purchase = new Purchase
            {
                ID_Purchase = newPurchaseId,
                ID_Detail = detail.ID_Detail,
                Quantity = quantity,
                Total_price = total_coast,
                Days_to_arrival = 2
            };
            Core.Context.Purchase.Add(purchase);
            Core.Context.SaveChanges();

            Console.WriteLine($"\nЗаказ оформлен! Доставка через 2 клиента.");
            Console.WriteLine($"Списано: {total_coast} руб.");
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        public void PurchaseDetail()
        {
            Console.Clear();
            Console.WriteLine("ЗАКУПКА ДЕТАЛЕЙ");
            Console.WriteLine("================");
            Console.WriteLine("Список доступных деталей:");
            foreach (var detail in details)
            {
                Console.WriteLine($"ID: {detail.ID_Detail} - {detail.Name}");
                Console.WriteLine($"Цена: {detail.PurchasePrice} руб.");
                Console.WriteLine("-----------------");
            }

            Console.Write("Введите ID детали для покупки: ");
            int ID = int.Parse(Console.ReadLine());
            var detailToBuy = details.FirstOrDefault(d => d.ID_Detail == ID);

            if (detailToBuy != null)
            {
                Console.WriteLine($"\nДеталь: {detailToBuy.Name}");
                Console.WriteLine($"Цена за штуку: {detailToBuy.PurchasePrice} руб.");
                Console.Write("Укажите количество деталей: ");
                int quantity = int.Parse(Console.ReadLine());

                double total_coast = detailToBuy.PurchasePrice * quantity;
                balance -= total_coast;
                int newPurchaseId = Core.Context.Purchase.Any() ? Core.Context.Purchase.Max(p => p.ID_Purchase) + 1 : 1;
                Purchase purchase = new Purchase
                {
                    ID_Purchase = newPurchaseId,
                    ID_Detail = detailToBuy.ID_Detail,
                    Quantity = quantity,
                    Total_price = total_coast,
                    Days_to_arrival = 2
                };
                Core.Context.Purchase.Add(purchase);
                Core.Context.SaveChanges();

                Console.WriteLine($"\nЗаказ оформлен! Доставка через 2 клиента.");
                Console.WriteLine($"Списано: {total_coast} руб.");
            }
            else
            {
                Console.WriteLine("Деталь с таким ID не найдена!");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        public void Repair(Detail brokendetail, Client client)
        {
            Console.Clear();
            Console.WriteLine("РЕМОНТ ДЕТАЛИ");
            Console.WriteLine("==============");

            var repairdetail = details.FirstOrDefault(d => d.ID_Detail == brokendetail.ID_Detail);
            bool correctdetail;

            if (repairdetail != null && repairdetail.Quantity > 0)
            {
                Console.WriteLine("Ремонт успешно прошел!");
                balance += brokendetail.SellPrice;
                repairdetail.Quantity--;
                correctdetail = true;
                Console.WriteLine($"Получено: {brokendetail.SellPrice} руб.");
            }
            else
            {
                var availableDetails = details.Where(d => d.Quantity > 0).ToList();
                if (availableDetails.Count > 0)
                {
                    repairdetail = availableDetails[rand.Next(availableDetails.Count)];
                    Console.WriteLine("Ремонт выполнен с неверной деталью!");
                    Console.WriteLine($"Использована: {repairdetail.Name}");
                    Console.WriteLine($"Штраф: {Fine} руб.");
                    balance -= Fine;
                    repairdetail.Quantity--;
                    correctdetail = false;
                }
                else
                {
                    Console.WriteLine("На складе нет деталей! Штраф за отказ.");
                    balance -= Fine;
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    return;
                }
            }

            int newOrderId = Core.Context.Order.Any() ? Core.Context.Order.Max(o => o.ID_Order) + 1 : 1;

            Order order = new Order
            {
                ID_Order = newOrderId,
                ID_Client = client.ID_Client,
                ID_Detail = repairdetail.ID_Detail,
                Price = correctdetail ? repairdetail.SellPrice : -Fine,
            };

            Core.Context.Order.Add(order);
            Core.Context.SaveChanges();

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
         
        public void UpdatePurchases()
        {
            var purchases = Core.Context.Purchase.ToList();
            bool deliveryArrived = false;

            foreach (var purchase in purchases)
            {
                if (purchase.Days_to_arrival > 0)
                {
                    purchase.Days_to_arrival--;
                    if (purchase.Days_to_arrival == 0)
                    {
                        var detail = details.FirstOrDefault(d => d.ID_Detail == purchase.ID_Detail);
                        if (detail != null)
                        {
                            detail.Quantity += purchase.Quantity;
                            Console.WriteLine($"\nДоставка прибыла: {detail.Name} в количестве {purchase.Quantity} шт.");
                            deliveryArrived = true;
                        }
                    }
                }
            }

            Core.Context.SaveChanges();

            if (deliveryArrived)
            {
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Initialize();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("МАШИННЫЙ САЛОН");
                Console.WriteLine("==============");
                Console.WriteLine($"Баланс: {game.balance} руб.");
                Console.WriteLine("-----------------");
                Console.WriteLine("1. Детали на складе");
                Console.WriteLine("2. Обслужить клиента");
                Console.WriteLine("3. Заказать поставку");
                Console.WriteLine("0. Выход");
                Console.WriteLine("-----------------");
                Console.Write("Выберите действие: ");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 0: return;
                    case 1: game.ShowDetails(); break;
                    case 2:
                        game.ServeClient();
                        break;
                    case 3:
                        game.PurchaseDetail();
                        break;

                }
            }
        }
    }
}