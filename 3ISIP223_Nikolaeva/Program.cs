using System;
using System.Collections.Generic;
using System.Linq;

namespace _3ISIP223_Nikolaeva
{
    public enum Book_genre
    {
        Nonfiction,
        Romance,
        Classic
    }

    class Book
    {
        public static int NextID = 1;
        public int ID;
        public string Name;
        public string Author;
        public int YearPublication;
        public Book_genre Genre;
        public float Price;

        public Book(string name, string author, int yearPublication, Book_genre genre, float price)
        {
            ID = NextID;
            NextID++;
            Name = name;
            Author = author;
            YearPublication = yearPublication;
            Genre = genre;
            Price = price;
        }
    }

    internal class Program
    {
        static List<Book> Books = new List<Book>()
        {
            new Book("Война и мир", "Лев Толстой", 1869, Book_genre.Classic, 1500),
            new Book("1984", "Джордж Оруэлл", 1949, Book_genre.Nonfiction, 800),
            new Book("Гордость и предубеждение", "Джейн Остин", 1813, Book_genre.Romance, 950),
            new Book("Мастер и Маргарита", "Михаил Булгаков", 1967, Book_genre.Classic, 1200),
            new Book("Маленькие женщины", "Луиза Мэй Олкотт", 1868, Book_genre.Romance, 700)
        };

        static void Main(string[] args)
        {
            char da = 'y';
            do
            {
                Console.WriteLine("1. Добавить книгу");
                Console.WriteLine("2. Удалить книгу");
                Console.WriteLine("3. Поиск книги по названию");
                Console.WriteLine("4. Поиск книг по автору");
                Console.WriteLine("5. Поиск книги по жанру");
                Console.WriteLine("6. Сортировка по названию");
                Console.WriteLine("7. Сортировка по году");
                Console.WriteLine("8. Вывести самую дорогую и самую дешёвую книгу");
                Console.WriteLine("9. Сгруппировать книги по авторам и вывести количество книг каждого автора");
                Console.WriteLine("10. Показать все книги");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите команду: ");

                int n = Convert.ToInt32(Console.ReadLine());
                switch (n)
                {
                    case 1: AddBook(); break;
                    case 2: RemoveBook(); break;
                    case 3: SearchByName(); break;
                    case 4: SearchByAuthor(); break;
                    case 5: SearchByGenre(); break;
                    case 6: SortByName(); break;
                    case 7: SortByYear(); break;
                    case 8: Cheapest_Expensive_Book(); break;
                    case 9: GroupByAuthor(); break;
                    case 10: ShowAllBooks(); break;
                    case 0: return;
                }

                Console.WriteLine("Продолжить? (y/n)");
                da = Convert.ToChar(Console.ReadLine());
            } while (da == 'y' || da == 'Y');
        }

        static void AddBook()
        {
            Console.Write("Название: ");
            string name = Console.ReadLine();

            Console.Write("Автор: ");
            string author = Console.ReadLine();

            Console.Write("Год издания: ");
            int yearPublication = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Жанр: 0 - Nonfiction, 1 - Romance, 2 - Classic");
            Book_genre genre = (Book_genre)Convert.ToInt32(Console.ReadLine());

            Console.Write("Цена: ");
            float price = Convert.ToSingle(Console.ReadLine());

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(author))
            {
                Console.WriteLine("Ошибка: название и автор не могут быть пустыми");
                return;
            }

            if (yearPublication <= 0 || price < 0)
            {
                Console.WriteLine("Ошибка: год должен быть положительным, цена не может быть отрицательной");
                return;
            }

            Books.Add(new Book(name, author, yearPublication, genre, price));
            Console.WriteLine("Книга добавлена");
        }

        static void RemoveBook()
        {
            Console.Write("ID книги: ");
            int id = Convert.ToInt32(Console.ReadLine());

            var foundBook = Books.FirstOrDefault(b => b.ID == id);

            if (foundBook != null)
            {
                Books.Remove(foundBook);
                Console.WriteLine("Книга удалена");
            }
            else
            {
                Console.WriteLine("Не найдена");
            }
        }

        static void SearchByName()
        {
            Console.Write("Название: ");
            string name = Console.ReadLine();

            var result = Books.Where(b => b.Name.ToLower().Contains(name.ToLower())).ToList();
            if (result.Count != 0)
            {
                ShowBooks(result);
            }
            else
            {
                Console.WriteLine("Не найдено");
            }
        }

        static void SearchByAuthor()
        {
            Console.Write("Автор: ");
            string author = Console.ReadLine();

            var result = Books.Where(b => b.Author.ToLower().Contains(author.ToLower())).ToList();
            if (result.Count != 0)
            {
                ShowBooks(result);
            }
            else
            {
                Console.WriteLine("Не найдено");
            }
        }

        static void SearchByGenre()
        {
            Console.WriteLine("Жанр: 0 - Nonfiction, 1 - Romance, 2 - Classic");
            Book_genre genre = (Book_genre)Convert.ToInt32(Console.ReadLine());

            var result = Books.Where(b => b.Genre == genre).ToList();
            if (result.Count != 0)
            {
                ShowBooks(result);
            }
            else
            {
                Console.WriteLine("Не найдено");
            }
        }

        static void SortByName()
        {
            var sortedbyname = Books.OrderBy(b => b.Name).ToList();
            ShowBooks(sortedbyname);
        }

        static void SortByYear()
        {
            var sortedbyyear = Books.OrderBy(b => b.YearPublication).ToList();
            ShowBooks(sortedbyyear);
        }

        static void Cheapest_Expensive_Book()
        {
            if (Books.Count == 0)
            {
                Console.WriteLine("Нет книг в библиотеке");
                return;
            }

            var cheapest = Books.OrderBy(b => b.Price).First();
            var expensive = Books.OrderByDescending(b => b.Price).First();

            Console.WriteLine("Самая дешевая книга:");
            Console.WriteLine($"ID: {cheapest.ID}, Название: {cheapest.Name}, Автор: {cheapest.Author}, Жанр: {cheapest.Genre}, Год издания: {cheapest.YearPublication}, Цена: {cheapest.Price}");

            Console.WriteLine("Самая дорогая книга:");
            Console.WriteLine($"ID: {expensive.ID}, Название: {expensive.Name}, Автор: {expensive.Author}, Жанр: {expensive.Genre}, Год издания: {expensive.YearPublication}, Цена: {expensive.Price}");
        }

        static void GroupByAuthor()
        {
            var groupedBooks = Books.GroupBy(b => b.Author);

            foreach (var group in groupedBooks)
            {
                Console.WriteLine($"Автор: {group.Key}, Количество книг: {group.Count()}");
            }
        }

        static void ShowAllBooks()
        {
            ShowBooks(Books);
        }

        static void ShowBooks(List<Book> BooksList)
        {
            foreach (var book in BooksList)
            {
                Console.WriteLine($"ID: {book.ID}, Название: {book.Name}, Автор: {book.Author}, Жанр: {book.Genre}, Год издания: {book.YearPublication}, Цена: {book.Price}");
            }
        }
    }
}