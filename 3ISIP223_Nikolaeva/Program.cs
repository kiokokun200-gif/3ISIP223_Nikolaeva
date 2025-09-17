using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    // Enum должен быть публичным и вне класса Product
    public enum ProductCategory
    {
        Electronics,
        Clothing,
        Food
    }

    class Product
    {
        public static int NextID = 1;
        public int ID;
        public string Name;
        public float Price;
        public int Quantity;
        public bool Availability => Quantity > 0;
        public ProductCategory Category;

        public Product(string name, float price, int quantity, ProductCategory category)
        {
            ID = NextID;
            NextID++;
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
        }
    }

    internal class Program
    {
        static List<Product> products = new List<Product>()
        {
            new Product("Ноутбук HP", 45000, 5, ProductCategory.Electronics),
            new Product("Футболка", 1500, 20, ProductCategory.Clothing),
            new Product("Яблоки", 120, 100, ProductCategory.Food),
            new Product("Айфон 17 Ультра про макс", 200000, 15, ProductCategory.Electronics),
            new Product("Творог", 100, 8, ProductCategory.Food)
        };

        static void Main(string[] args)
        {
            char da = 'y';
            do
            {
                Console.WriteLine("1. Добавить товар\n2. Удалить товар\n3. Заказать поставку товара\n4. Продать товар\n5. Поиск товара по коду\n6. Поиск по названию\n7. Поиск по категории\n8. Показать все\n9. Выход");
                int n = Convert.ToInt32(Console.ReadLine());
                switch (n)
                {
                    case 1: AddProduct(); break;
                    case 2: RemoveProduct(); break;
                    case 3: OrderSupply(); break;
                    case 4: SellProduct(); break;
                    case 5: SearchByID(); break;
                    case 6: SearchByName(); break;
                    case 7: SearchByCategory(); break;
                    case 8: ShowAllProducts(); break;
                    case 9: return;
                }

                Console.WriteLine("Продолжить? (y, n)");
                da = Convert.ToChar(Console.ReadLine());
            } while (da == 'y');
        }

        static void AddProduct()
        {
            Console.Write("Название: ");
            string name = Console.ReadLine();

            Console.Write("Цена: ");
            float price = Convert.ToSingle(Console.ReadLine());

            Console.Write("Количество: ");
            int quantity = Convert.ToInt32(Console.ReadLine());

            Console.Write("Категория (0-2): ");
            ProductCategory category = (ProductCategory)Convert.ToInt32(Console.ReadLine());

            products.Add(new Product(name, price, quantity, category));
            Console.WriteLine("Товар добавлен");
        }

        static void RemoveProduct()
        {
            Console.Write("ID товара: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Product foundProduct = null;
            foreach (var product in products)
            {
                if (product.ID == id)
                {
                    foundProduct = product;
                    break;
                }
            }

            if (foundProduct != null)
            {
                products.Remove(foundProduct);
                Console.WriteLine("Товар удален");
            }
            else
            {
                Console.WriteLine("Не найден");
            }
        }

        static void OrderSupply()
        {
            Console.Write("ID товара: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Количество: ");
            int amount = Convert.ToInt32(Console.ReadLine());

            Product foundProduct = null;
            foreach (var product in products)
            {
                if (product.ID == id)
                {
                    foundProduct = product;
                    break;
                }
            }

            if (foundProduct != null)
            {
                foundProduct.Quantity += amount;
                Console.WriteLine("Поставка добавлена!");
            }
            else
            {
                Console.WriteLine("Не найден");
            }
        }

        static void SellProduct()
        {
            Console.Write("ID товара: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Количество: ");
            int amount = Convert.ToInt32(Console.ReadLine());

            Product foundProduct = null;
            foreach (var product in products)
            {
                if (product.ID == id)
                {
                    foundProduct = product;
                    break;
                }
            }

            if (foundProduct != null)
            {
                if (foundProduct.Quantity >= amount)
                {
                    foundProduct.Quantity -= amount;
                    Console.WriteLine("Продажа совершена");
                    if (foundProduct.Quantity == 0) products.Remove(foundProduct);
                }
                else
                {
                    Console.WriteLine("Недостаточно товара на складе");
                }
            }
            else
            {
                Console.WriteLine("Не найден");
            }
        }

        static void SearchByID()
        {
            Console.Write("ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            var result = new List<Product>();
            foreach (var product in products)
            {
                if (product.ID == id)
                {
                    result.Add(product);
                }
            }
            ShowProducts(result);
        }

        static void SearchByName()
        {
            Console.Write("Название: ");
            string name = Console.ReadLine();

            var result = new List<Product>();
            foreach (var product in products)
            {
                if (product.Name.Contains(name))
                {
                    result.Add(product);
                }
            }
            ShowProducts(result);
        }

        static void SearchByCategory()
        {
            Console.Write("Категория (0-2): ");
            ProductCategory category = (ProductCategory)Convert.ToInt32(Console.ReadLine());

            var result = new List<Product>();
            foreach (var product in products)
            {
                if (product.Category == category)
                {
                    result.Add(product);
                }
            }
            ShowProducts(result);
        }

        static void ShowAllProducts()
        {
            ShowProducts(products);
        }

        static void ShowProducts(List<Product> productsList)
        {
            foreach (var product in productsList)
            {
                Console.WriteLine($"ID: {product.ID}, Название: {product.Name}, Цена: {product.Price}, Количество: {product.Quantity}, Категория: {product.Category}, В наличии: {product.Availability}");
            }
        }
    }
}