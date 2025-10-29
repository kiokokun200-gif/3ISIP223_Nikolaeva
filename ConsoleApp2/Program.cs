using ConsoleApp2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp2
{
    class Program
    {
        private MarketplaceApp app;

        public Program()
        {
            app = new MarketplaceApp();
        }

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();
        }

        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Добро пожаловать в GMWOG||GG.MOW||WONGG Маркетплейс!");

            while (true)
            {
                if (app.CurrentUser == null)
                {
                    app.ShowMainMenu();
                }
                else
                {
                    app.ShowUserMenu();
                }
            }
        }
    }

    public class MarketplaceApp
    {
        public Users CurrentUser { get; private set; }

        public void ShowMainMenu()
        {
            Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine("1. Просмотр товаров");
            Console.WriteLine("2. Регистрация");
            Console.WriteLine("3. Вход в аккаунт");
            Console.WriteLine("4. Выход");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ViewProducts();
                    break;
                case "2":
                    Register();
                    break;
                case "3":
                    Login();
                    break;
                case "4":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }

        public void ShowUserMenu()
        {
            Console.WriteLine($"\n=== ДОБРО ПОЖАЛОВАТЬ, {CurrentUser.Login.ToUpper()} ===");
            Console.WriteLine("1. Просмотр товаров");
            Console.WriteLine("2. Корзина");
            Console.WriteLine("3. Мои заказы");
            Console.WriteLine("4. Выйти из аккаунта");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ViewProducts();
                    break;
                case "2":
                    ShowCart();
                    break;
                case "3":
                    ViewOrders();
                    break;
                case "4":
                    CurrentUser = null;
                    Console.WriteLine("Вы вышли из аккаунта.");
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }

        public void ViewProducts()
        {
            Console.WriteLine("\n=== КАТАЛОГ ТОВАРОВ ===");

            try
            {
                var products = Core.Context.Products
                    .Where(p => p.IsActive && p.StockQuantity > 0)
                    .ToList();

                if (!products.Any())
                {
                    Console.WriteLine("Товары отсутствуют.");
                    return;
                }

                foreach (var product in products)
                {
                    var category = Core.Context.Categories.FirstOrDefault(c => c.CategoryID == product.CategoryID);
                    Console.WriteLine($"{product.ProductID}. {product.ProductName}");
                    Console.WriteLine($"   Описание: {product.Description}");
                    Console.WriteLine($"   Цена: {product.Price} руб.");
                    Console.WriteLine($"   В наличии: {product.StockQuantity} шт.");
                    Console.WriteLine($"   Категория: {category?.CategoryName}");
                    Console.WriteLine("   " + new string('-', 40));
                }

                if (CurrentUser != null)
                {
                    Console.Write("Хотите добавить товар в корзину? (y/n): ");
                    string addToCart = Console.ReadLine();
                    if (addToCart?.ToLower() == "y")
                    {
                        AddToCart();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке товаров: {ex.Message}");
            }
        }

        public void Register()
        {
            Console.WriteLine("\n=== РЕГИСТРАЦИЯ ===");

            Console.Write("Логин: ");
            string login = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(login))
            {
                Console.WriteLine("Логин не может быть пустым!");
                return;
            }

            if (Core.Context.Users.Any(u => u.Login == login))
            {
                Console.WriteLine("Пользователь с таким логином уже существует!");
                return;
            }

            Console.Write("Пароль: ");
            string password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Пароль не может быть пустым!");
                return;
            }

            Console.Write("Подтвердите пароль: ");
            string confirmPassword = Console.ReadLine();

            if (password != confirmPassword)
            {
                Console.WriteLine("Пароли не совпадают!");
                return;
            }

            Console.Write("Введите имя: ");
            string name = Console.ReadLine();


            try
            {
                var newUser = new Users
                {
                    Name = name,
                    Login = login,
                    Password = password,
                    RegistrationDate = DateTime.Now
                };

                Core.Context.Users.Add(newUser);
                Core.Context.SaveChanges();

                Console.WriteLine("Регистрация прошла успешно! Теперь вы можете войти в аккаунт.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при регистрации: {ex.Message}");
            }
        }

        public void Login()
        {
            Console.WriteLine("\n=== ВХОД В АККАУНТ ===");

            Console.Write("Логин: ");
            string login = Console.ReadLine();

            Console.Write("Пароль: ");
            string password = Console.ReadLine();

            try
            {
                var user = Core.Context.Users
                    .FirstOrDefault(u => u.Login == login && u.Password == password);

                if (user != null)
                {
                    CurrentUser = user;
                    Console.WriteLine($"Успешный вход! Добро пожаловать, {user.Login}!");
                }
                else
                {
                    Console.WriteLine("Неверный логин или пароль!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при входе: {ex.Message}");
            }
        }

        public void AddToCart()
        {
            if (CurrentUser == null)
            {
                Console.WriteLine("Необходимо войти в аккаунт!");
                return;
            }

            Console.Write("Введите ID товара для добавления в корзину: ");
            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                try
                {
                    var product = Core.Context.Products.FirstOrDefault(p => p.ProductID == productId && p.IsActive);
                    if (product == null)
                    {
                        Console.WriteLine("Товар не найден!");
                        return;
                    }

                    Console.Write("Количество: ");
                    if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                    {
                        if (quantity > product.StockQuantity)
                        {
                            Console.WriteLine($"Недостаточно товара на складе! Доступно: {product.StockQuantity} шт.");
                            return;
                        }

                        var existingCartItem = Core.Context.CartItems
                            .FirstOrDefault(c => c.UserID == CurrentUser.UserID && c.ProductID == productId);

                        if (existingCartItem != null)
                        {
                            existingCartItem.Quantity += quantity;
                            Console.WriteLine("Количество товара в корзине обновлено!");
                        }
                        else
                        {
                            var cartItem = new CartItems
                            {
                                UserID = CurrentUser.UserID,
                                ProductID = productId,
                                Quantity = quantity,
                                AddedDate = DateTime.Now
                            };
                            Core.Context.CartItems.Add(cartItem);
                            Console.WriteLine("Товар добавлен в корзину!");
                        }

                        Core.Context.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("Неверное количество!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при добавлении в корзину: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Неверный ID товара!");
            }
        }

        public void ShowCart()
        {
            if (CurrentUser == null)
            {
                Console.WriteLine("Необходимо войти в аккаунт!");
                return;
            }

            Console.WriteLine("\n=== КОРЗИНА ===");

            try
            {
                var cartItems = Core.Context.CartItems
                    .Where(c => c.UserID == CurrentUser.UserID)
                    .ToList();

                if (!cartItems.Any())
                {
                    Console.WriteLine("Корзина пуста.");
                    return;
                }

                decimal totalAmount = 0;
                int itemNumber = 1;

                foreach (var item in cartItems)
                {
                    var product = Core.Context.Products.FirstOrDefault(p => p.ProductID == item.ProductID);
                    if (product != null)
                    {
                        decimal itemTotal = product.Price * item.Quantity;
                        totalAmount += itemTotal;

                        Console.WriteLine($"{itemNumber}. {product.ProductName}");
                        Console.WriteLine($"   Цена: {product.Price} руб. × {item.Quantity} = {itemTotal} руб.");
                        Console.WriteLine($"   ID в корзине: {item.CartItemID}");
                        Console.WriteLine();
                        itemNumber++;
                    }
                }

                Console.WriteLine($"Общая сумма: {totalAmount} руб.");

                Console.WriteLine("\n1. Оформить заказ на всю корзину");
                Console.WriteLine("2. Удалить товар из корзины");
                Console.WriteLine("3. Очистить корзину");
                Console.WriteLine("4. Вернуться в меню");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Checkout();
                        break;
                    case "2":
                        RemoveFromCart();
                        break;
                    case "3":
                        ClearCart();
                        break;
                    case "4":
                        break;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке корзины: {ex.Message}");
            }
        }

        public void RemoveFromCart()
        {
            Console.Write("Введите ID товара в корзине для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int cartItemId))
            {
                try
                {
                    var cartItem = Core.Context.CartItems
                        .FirstOrDefault(c => c.CartItemID == cartItemId && c.UserID == CurrentUser.UserID);

                    if (cartItem != null)
                    {
                        Core.Context.CartItems.Remove(cartItem);
                        Core.Context.SaveChanges();
                        Console.WriteLine("Товар удален из корзины!");
                    }
                    else
                    {
                        Console.WriteLine("Товар не найден в корзине!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при удалении из корзины: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Неверный ID товара!");
            }
        }

        public void ClearCart()
        {
            try
            {
                var cartItems = Core.Context.CartItems
                    .Where(c => c.UserID == CurrentUser.UserID)
                    .ToList();

                if (cartItems.Any())
                {
                    Core.Context.CartItems.RemoveRange(cartItems);
                    Core.Context.SaveChanges();
                    Console.WriteLine("Корзина очищена!");
                }
                else
                {
                    Console.WriteLine("Корзина уже пуста!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при очистке корзины: {ex.Message}");
            }
        }

        public void Checkout()
        {
            if (CurrentUser == null)
            {
                Console.WriteLine("Необходимо войти в аккаунт!");
                return;
            }

            try
            {
                var cartItems = Core.Context.CartItems
                    .Where(c => c.UserID == CurrentUser.UserID)
                    .ToList();

                if (!cartItems.Any())
                {
                    Console.WriteLine("Корзина пуста!");
                    return;
                }

                // Проверяем доступность всех товаров
                foreach (var item in cartItems)
                {
                    var product = Core.Context.Products.FirstOrDefault(p => p.ProductID == item.ProductID);
                    if (product == null || !product.IsActive || product.StockQuantity < item.Quantity)
                    {
                        Console.WriteLine($"Товар '{product?.ProductName}' недоступен в нужном количестве!");
                        return;
                    }
                }

                // Показываем доступные ПВЗ
                Console.WriteLine("\n=== ВЫБОР ПУНКТА ВЫДАЧИ ===");
                var pickupPoints = Core.Context.PickupPoints.ToList();
                foreach (var point in pickupPoints)
                {
                    Console.WriteLine($"{point.PointID}. {point.PointName}");
                    Console.WriteLine($"   Адрес: {point.Address}");
                    Console.WriteLine($"   Телефон: {point.PhoneNumber}");
                    Console.WriteLine();
                }

                Console.Write("Выберите ПВЗ: ");
                if (int.TryParse(Console.ReadLine(), out int pointId) &&
                    pickupPoints.Any(p => p.PointID == pointId))
                {
                    // Считаем общую сумму
                    decimal totalAmount = 0;
                    foreach (var item in cartItems)
                    {
                        var product = Core.Context.Products.First(p => p.ProductID == item.ProductID);
                        totalAmount += product.Price * item.Quantity;
                    }

                    // Создаем заказ
                    var order = new Orders
                    {
                        UserID = CurrentUser.UserID,
                        OrderDate = DateTime.Now,
                        TotalAmount = totalAmount,
                        PointID = pointId
                    };

                    Core.Context.Orders.Add(order);
                    Core.Context.SaveChanges();

                    // Создаем элементы заказа и обновляем склад
                    foreach (var item in cartItems)
                    {
                        var product = Core.Context.Products.First(p => p.ProductID == item.ProductID);

                        var orderItem = new OrderItems
                        {
                            OrderID = order.OrderID,
                            ProductID = item.ProductID,
                            Quantity = item.Quantity,
                            Price = product.Price
                        };
                        Core.Context.OrderItems.Add(orderItem);

                        // Обновляем количество на складе
                        product.StockQuantity -= item.Quantity;
                    }

                    // Очищаем корзину
                    Core.Context.CartItems.RemoveRange(cartItems);
                    Core.Context.SaveChanges();

                    var selectedPoint = pickupPoints.First(p => p.PointID == pointId);

                    Console.WriteLine("\n=== ЗАКАЗ УСПЕШНО ОФОРМЛЕН ===");
                    Console.WriteLine($"Номер заказа: {order.OrderID}");
                    Console.WriteLine($"Общая сумма: {totalAmount} руб.");
                    Console.WriteLine($"ПВЗ: {selectedPoint.PointName}");
                    Console.WriteLine($"Адрес: {selectedPoint.Address}");
                    Console.WriteLine($"Телефон: {selectedPoint.PhoneNumber}");
                    Console.WriteLine($"Дата заказа: {order.OrderDate:dd.MM.yyyy HH:mm}");
                }
                else
                {
                    Console.WriteLine("Неверный выбор ПВЗ!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при оформлении заказа: {ex.Message}");
            }
        }

        public void ViewOrders()
        {
            if (CurrentUser == null)
            {
                Console.WriteLine("Необходимо войти в аккаунт!");
                return;
            }

            Console.WriteLine("\n=== МОИ ЗАКАЗЫ ===");

            try
            {
                var orders = Core.Context.Orders
                    .Where(o => o.UserID == CurrentUser.UserID)
                    .OrderByDescending(o => o.OrderDate)
                    .ToList();

                if (!orders.Any())
                {
                    Console.WriteLine("У вас пока нет заказов.");
                    return;
                }

                foreach (var order in orders)
                {
                    var pickupPoint = Core.Context.PickupPoints.FirstOrDefault(p => p.PointID == order.PointID);
                    Console.WriteLine($"Заказ №{order.OrderID}");
                    Console.WriteLine($"   Дата: {order.OrderDate:dd.MM.yyyy HH:mm}");
                    Console.WriteLine($"   Сумма: {order.TotalAmount} руб.");
                    Console.WriteLine($"   ПВЗ: {pickupPoint?.PointName}");
                    Console.WriteLine($"   Адрес: {pickupPoint?.Address}");

                    // Показываем товары в заказе
                    var orderItems = Core.Context.OrderItems
                        .Where(oi => oi.OrderID == order.OrderID)
                        .ToList();

                    Console.WriteLine("   Товары:");
                    foreach (var item in orderItems)
                    {
                        var product = Core.Context.Products.FirstOrDefault(p => p.ProductID == item.ProductID);
                        if (product != null)
                        {
                            Console.WriteLine($"     - {product.ProductName} × {item.Quantity} = {item.Price * item.Quantity} руб.");
                        }
                    }
                    Console.WriteLine(new string('=', 50));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке заказов: {ex.Message}");
            }
        }
    }
}