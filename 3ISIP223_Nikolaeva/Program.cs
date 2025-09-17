class Product
{
    enum ProductCategory
    {
        Electronics,
        Clothing,
        Food
    }

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

List<Product> products = new()
{
    new Product("Ноутбук HP", 45000, 5, Product.ProductCategory.Electronics),
    new Product("Футболка", 1500, 20, Product.ProductCategory.Clothing),
    new Product("Яблоки", 120, 100, Product.ProductCategory.Food),
    new Product("Айфон 17 Ультра про макс", 200000, 15, Product.ProductCategory.Electronics),
    new Product("Творог", 100, 8, Product.ProductCategory.Food)
};


char da = 'y';
do
{
    console.writeline("1. Добавить товар\n2. Удалить товар\n3. Заказать поставку товара\n4. Продать товар\n5. Поиск товара по коду\n6. Поиск по названию\n7. Поиск по категории\n8. Показать все\n9. Выход");
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
} while (da == 'y')

void AddProduct()
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
 
void RemoveProduct()
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

void OrderSupply()
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



void SellProduct() 
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
        foundProduct.Quantity -= amount;
        Console.WriteLine("Продажа совершена");
    }
    else
    {
        Console.WriteLine("Не найден");
    }
}


void SearchByID()
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

void SearchByName()
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

void SearchByCategory()
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

void ShowAllProducts()
{
    ShowProducts(products);
}

void ShowProducts(List<Product> productsList)
{
    foreach (var product in productsList)
    {
        Console.WriteLine($"ID: {product.ID}, Название: {product.Name}, Цена: {product.Price}, Количество: {product.Quantity}, Категория: {product.Category}");
    }
}
