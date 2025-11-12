using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp2
{
    class Program
    { 
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Маркетплейс ===");
                Console.WriteLine("1. Просмотр товаров");
                Console.WriteLine("2. Регистрация");
                Console.WriteLine("3. Вход в аккаунт");
                Console.WriteLine("4. Выход");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewProducts();
                        break;
                    case "2":
                        RegisterUser();
                        break;
                    case "3":
                        LoginUser();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void ViewProducts()
        {
            Console.Clear();
            Console.WriteLine("=== ВСЕ ТОВАРЫ ===");

            var products = Core.Context.Products.ToList();

            if (!products.Any())
            {
                Console.WriteLine("Товары не найдены!");
                Console.ReadKey();
                return;
            }

            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.ProductID}");
                Console.WriteLine($"Название: {product.Name}");
                Console.WriteLine($"Описание: {product.Description}");
                Console.WriteLine($"Цена: {product.Price} руб.");
                Console.WriteLine($"В наличии: {product.StockQuantity} шт.");
                Console.WriteLine($"Категория: {product.Categories.Name}");
                Console.WriteLine("-----------------------------------");
            }

            Console.WriteLine("Нажмите любую клавишу для возврата...");
            Console.ReadKey();
        }

        // Регистрация пользователя
        static void RegisterUser()
        {
            Console.Clear();
            Console.WriteLine("=== РЕГИСТРАЦИЯ ===");

            Console.Write("Введите логин: ");
            var login = Console.ReadLine();

            var existingUser = Core.Context.Users.FirstOrDefault(u => u.Login == login);
            if (existingUser != null)
            {
                Console.WriteLine("Пользователь с таким логином уже существует!");
                Console.ReadKey();
                return;
            }
            Console.Write("Введите пароль: ");
            var password = Console.ReadLine();

            Console.Write("Подтвердите пароль: ");
            var confirmPassword = Console.ReadLine();

            if (password != confirmPassword)
            {
                Console.WriteLine("Пароли не совпадают!");
                Console.ReadKey();
                return;
            }
            Console.Write("Введите ваше имя: ");
            var name = Console.ReadLine();

            // Проверки
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Все поля обязательны для заполнения!");
                Console.ReadKey();
                return;
            }

            try
            {
                // Создаем пользователя
                var newUser = new Users
                {
                    Login = login,
                    Password = password,
                    Name = name
                };

                Core.Context.Users.Add(newUser);
                Core.Context.SaveChanges();

                Console.WriteLine("Регистрация прошла успешно! Теперь вы можете войти в систему.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при регистрации: {ex.Message}");
            }

            Console.ReadKey();
        }

        // Вход в аккаунт
        static void LoginUser()
        {
            Console.Clear();
            Console.WriteLine("=== ВХОД В АККАУНТ ===");

            Console.Write("Логин: ");
            var login = Console.ReadLine();

            Console.Write("Пароль: ");
            var password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Логин и пароль обязательны!");
                Console.ReadKey();
                return;
            }

            // Ищем пользователя
            var user = Core.Context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            if (user == null)
            {
                Console.WriteLine("Неверный логин или пароль!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Добро пожаловать, {user.Name}!");
            Console.ReadKey();

            // Переходим в личный кабинет
            UserMenu(user);
        }

        // Меню авторизованного пользователя
        static void UserMenu(Users user)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== ЛИЧНЫЙ КАБИНЕТ ({user.Name}) ===");
                Console.WriteLine("1. Просмотр товаров");
                Console.WriteLine("2. Моя корзина");
                Console.WriteLine("3. Мои заказы");
                Console.WriteLine("4. Выйти из аккаунта");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewProductsForUser(user);
                        break;
                    case "2":
                        ViewCart(user);
                        break;
                    case "3":
                        ViewOrders(user);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void ViewProductsForUser(Users user)
        {
            Console.Clear();
            Console.WriteLine("=== ТОВАРЫ ===");

            var products = Core.Context.Products.ToList();

            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.ProductID}");
                Console.WriteLine($"Название: {product.Name}");
                Console.WriteLine($"Цена: {product.Price} руб.");
                Console.WriteLine($"Категория: {product.Categories.Name}");
                Console.WriteLine($"В наличии: {product.StockQuantity} шт.");
                Console.WriteLine("-----------------------------------");
            }

            Console.WriteLine("\n1. Добавить товар в корзину");
            Console.WriteLine("2. Вернуться в меню");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();
            if (choice == "1")
            {
                AddToCart(user);
            }
        }

        // Добавление товара в корзину
        static void AddToCart(Users user)
        {
            Console.Write("Введите ID товара: ");
            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.WriteLine("Неверный ID товара!");
                Console.ReadKey();
                return;
            }

            Console.Write("Введите количество: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Неверное количество!");
                Console.ReadKey();
                return;
            }

            // Проверяем существование товара
            var product = Core.Context.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product == null)
            {
                Console.WriteLine("Товар не найден!");
                Console.ReadKey();
                return;
            }

            // Проверяем наличие на складе
            if (product.StockQuantity < quantity)
            {
                Console.WriteLine("Недостаточно товара на складе!");
                Console.ReadKey();
                return;
            }

            try
            {
                // Ищем активную корзину пользователя
                var cart = Core.Context.Carts.FirstOrDefault(c => c.UserID == user.UserID);

                // Если корзины нет - создаем новую
                if (cart == null)
                {
                    cart = new Carts
                    {
                        UserID = user.UserID,
                        CreatedAt = DateTime.Now
                    };
                    Core.Context.Carts.Add(cart);
                    Core.Context.SaveChanges();
                }

                // Проверяем, есть ли уже этот товар в корзине
                var existingCartItem = Core.Context.CartItems
                    .FirstOrDefault(ci => ci.CartID == cart.CartID && ci.ProductID == productId);

                if (existingCartItem != null)
                {
                    // Если товар уже есть - увеличиваем количество
                    existingCartItem.Quantity += quantity;
                }
                else
                {
                    // Если товара нет - добавляем новый
                    var cartItem = new CartItems
                    {
                        CartID = cart.CartID,
                        ProductID = productId,
                        Quantity = quantity
                    };
                    Core.Context.CartItems.Add(cartItem);
                }

                Core.Context.SaveChanges();
                Console.WriteLine($"Товар '{product.Name}' добавлен в корзину!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении в корзину: {ex.Message}");
            }

            Console.ReadKey();
        }

        // Просмотр корзины с выводом ID
        // Просмотр корзины с выводом ID
        static void ViewCart(Users user)
        {
            Console.Clear();
            Console.WriteLine("=== МОЯ КОРЗИНА ===");

            var cart = Core.Context.Carts.FirstOrDefault(c => c.UserID == user.UserID);

            if (cart == null)
            {
                Console.WriteLine("Ваша корзина пуста!");
                Console.ReadKey();
                return;
            }

            var cartItems = Core.Context.CartItems.Where(ci => ci.CartID == cart.CartID).ToList();

            if (!cartItems.Any())
            {
                Console.WriteLine("Ваша корзина пуста!");
                Console.ReadKey();
                return;
            }

            decimal totalAmount = 0;
            int itemNumber = 1;

            foreach (var item in cartItems)
            {
                var itemTotal = item.Quantity * item.Products.Price;
                totalAmount += itemTotal;

                Console.WriteLine($"{itemNumber}. Товар: {item.Products.Name}");
                Console.WriteLine($"   Цена: {item.Products.Price} руб.");
                Console.WriteLine($"   Количество: {item.Quantity}");
                Console.WriteLine($"   Сумма: {itemTotal} руб.");
                Console.WriteLine($"   ID: {item.CartItemID}");
                Console.WriteLine("-----------------------------------");
                itemNumber++;
            }

            Console.WriteLine($"ОБЩАЯ СУММА: {totalAmount} руб.");
            Console.WriteLine("\n1. Купить все товары из корзины");
            Console.WriteLine("2. Купить один товар");
            Console.WriteLine("3. Удалить товар из корзины");
            Console.WriteLine("4. Вернуться в меню");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    CreateOrder(user, cart, cartItems); // Покупка всей корзины
                    break;
                case "2":
                    BuySingleItem(user, cart, cartItems); // Покупка одного товара
                    break;
                case "3":
                    RemoveFromCart(user);
                    break;
                case "4":
                    // Просто возвращаемся в меню
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    Console.ReadKey();
                    break;
            }
        }

        // Покупка одного товара из корзины
        static void BuySingleItem(Users user, Carts cart, List<CartItems> cartItems)
        {
            if (!cartItems.Any())
            {
                Console.WriteLine("Корзина пуста!");
                Console.ReadKey();
                return;
            }

            Console.Write("Введите номер товара для покупки: ");
            if (!int.TryParse(Console.ReadLine(), out int itemNumber) || itemNumber < 1 || itemNumber > cartItems.Count)
            {
                Console.WriteLine("Неверный номер товара!");
                Console.ReadKey();
                return;
            }

            var selectedCartItem = cartItems[itemNumber - 1];

            Console.Clear();
            Console.WriteLine("=== ПОКУПКА ОДНОГО ТОВАРА ===");
            Console.WriteLine($"Товар: {selectedCartItem.Products.Name}");
            Console.WriteLine($"Количество: {selectedCartItem.Quantity}");
            Console.WriteLine($"Сумма: {selectedCartItem.Quantity * selectedCartItem.Products.Price} руб.");

            // Показываем доступные пункты выдачи
            var pickupPoints = Core.Context.PickupPoints.ToList();
            Console.WriteLine("\nДоступные пункты выдачи:");
            foreach (var point in pickupPoints)
            {
                Console.WriteLine($"ID: {point.PickupPointID} - {point.Description} ({point.Address})");
            }

            Console.Write("Выберите ID пункта выдачи: ");
            if (!int.TryParse(Console.ReadLine(), out int pickupPointId))
            {
                Console.WriteLine("Неверный ID пункта выдачи!");
                Console.ReadKey();
                return;
            }

            var selectedPoint = Core.Context.PickupPoints.FirstOrDefault(p => p.PickupPointID == pickupPointId);
            if (selectedPoint == null)
            {
                Console.WriteLine("Пункт выдачи не найден!");
                Console.ReadKey();
                return;
            }

            try
            {
                // Создаем заказ для одного товара
                var order = new Orders
                {
                    UserID = user.UserID,
                    PickupPointID = pickupPointId,
                    OrderDate = DateTime.Now,
                    TotalAmount = selectedCartItem.Quantity * selectedCartItem.Products.Price
                };
                Core.Context.Orders.Add(order);
                Core.Context.SaveChanges();

                // Создаем элемент заказа
                var orderItem = new OrderItems
                {
                    OrderID = order.OrderID,
                    ProductID = selectedCartItem.ProductID,
                    Quantity = selectedCartItem.Quantity,
                    UnitPrice = selectedCartItem.Products.Price
                };
                Core.Context.OrderItems.Add(orderItem);

                // Уменьшаем количество товара на складе
                selectedCartItem.Products.StockQuantity -= selectedCartItem.Quantity;


                Console.WriteLine($"Товар '{selectedCartItem.Products.Name}' успешно куплен!");
                Console.WriteLine($"Сумма: {order.TotalAmount} руб.");
                Console.WriteLine($"Пункт выдачи: {selectedPoint.Description}");
                // Удаляем только этот товар из корзины
                Core.Context.CartItems.Remove(selectedCartItem);
                Core.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при покупке товара: {ex.Message}");
            }

            Console.ReadKey();
        }

        static void CreateOrder(Users user, Carts cart, System.Collections.Generic.List<CartItems> cartItems)
        {
            Console.Clear();
            Console.WriteLine("=== ПОКУПКА ВСЕЙ КОРЗИНЫ ===");

            decimal totalAmount = cartItems.Sum(item => item.Quantity * item.Products.Price);
            Console.WriteLine("Состав заказа:");
            foreach (var item in cartItems)
            {
                Console.WriteLine($"- {item.Products.Name} x {item.Quantity} = {item.Quantity * item.Products.Price} руб.");
            }
            Console.WriteLine($"ОБЩАЯ СУММА: {totalAmount} руб.");

            var pickupPoints = Core.Context.PickupPoints.ToList();
            Console.WriteLine("\nДоступные пункты выдачи:");
            foreach (var point in pickupPoints)
            {
                Console.WriteLine($"ID: {point.PickupPointID} - {point.Description} ({point.Address})");
            }

            Console.Write("Выберите ID пункта выдачи: ");
            if (!int.TryParse(Console.ReadLine(), out int pickupPointId))
            {
                Console.WriteLine("Неверный ID пункта выдачи!");
                Console.ReadKey();
                return;
            }

            var selectedPoint = Core.Context.PickupPoints.FirstOrDefault(p => p.PickupPointID == pickupPointId);
            if (selectedPoint == null)
            {
                Console.WriteLine("Пункт выдачи не найден!");
                Console.ReadKey();
                return;
            }

            try
            {
                var order = new Orders
                {
                    UserID = user.UserID,
                    PickupPointID = pickupPointId,
                    OrderDate = DateTime.Now,
                    TotalAmount = totalAmount
                };
                Core.Context.Orders.Add(order);
                Core.Context.SaveChanges();

                foreach (var cartItem in cartItems)
                {
                    var orderItem = new OrderItems
                    {
                        OrderID = order.OrderID,
                        ProductID = cartItem.ProductID,
                        Quantity = cartItem.Quantity,
                        UnitPrice = cartItem.Products.Price
                    };
                    Core.Context.OrderItems.Add(orderItem);

                    cartItem.Products.StockQuantity -= cartItem.Quantity;
                }

                Core.Context.CartItems.RemoveRange(cartItems);
                Core.Context.SaveChanges();

                Console.WriteLine($"Заказ успешно оформлен!");
                Console.WriteLine($"Общая сумма: {totalAmount} руб.");
                Console.WriteLine($"Пункт выдачи: {selectedPoint.Description}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при оформлении заказа: {ex.Message}");
            }

            Console.ReadKey();
        }
        // Удаление товара из корзины по CartItemID
        static void RemoveFromCart(Users user)
        {
            Console.Write("Введите ID товара из корзины для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int cartItemId))
            {
                Console.WriteLine("Неверный ID!");
                Console.ReadKey();
                return;
            }

            var cart = Core.Context.Carts.FirstOrDefault(c => c.UserID == user.UserID);
            if (cart == null)
            {
                Console.WriteLine("Корзина не найдена!");
                Console.ReadKey();
                return;
            }

            var cartItem = Core.Context.CartItems
                .FirstOrDefault(ci => ci.CartItemID == cartItemId && ci.CartID == cart.CartID);

            if (cartItem == null)
            {
                Console.WriteLine("Товар с таким ID не найден в вашей корзине!");
                Console.ReadKey();
                return;
            }

            try
            {
                string productName = cartItem.Products.Name;
                Core.Context.CartItems.Remove(cartItem);
                Core.Context.SaveChanges();

                Console.WriteLine($"Товар '{productName}' удален из корзины!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении: {ex.Message}");
            }

            Console.ReadKey();
        }
        // Создание заказа
        

        // Просмотр заказов
        static void ViewOrders(Users user)
        {
            Console.Clear();
            Console.WriteLine("=== МОИ ЗАКАЗЫ ===");

            var orders = Core.Context.Orders
                .Where(o => o.UserID == user.UserID)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            if (!orders.Any())
            {
                Console.WriteLine("У вас еще нет заказов!");
                Console.ReadKey();
                return;
            }

            foreach (var order in orders)
            {
                Console.WriteLine($"Заказ от {order.OrderDate:dd.MM.yyyy HH:mm}");
                Console.WriteLine($"Сумма: {order.TotalAmount} руб.");
                Console.WriteLine($"Пункт выдачи: {order.PickupPoints.Description}");

                var orderItems = Core.Context.OrderItems.Where(oi => oi.OrderID == order.OrderID).ToList();
                Console.WriteLine("Товары:");
                foreach (var item in orderItems)
                {
                    Console.WriteLine($"  - {item.Products.Name} x {item.Quantity} по {item.UnitPrice} руб.");
                }
                Console.WriteLine("===================================");
            }

            Console.WriteLine("Нажмите любую клавишу для возврата...");
            Console.ReadKey();
        }
    }
}