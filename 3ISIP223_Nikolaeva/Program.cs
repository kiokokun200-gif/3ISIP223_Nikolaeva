using System;
using System.Collections.Generic;

namespace _ISIP223_Nikolaeva
{
    internal class Program
    {
        // Структура для хранения статистики по тексту
        struct TextStatistics
        {
            public int WordCount;
            public string ShortestWord;
            public int SentenceCount;
            public int VowelCount;
            public int ConsonantCount;
            public string LongestWord;
            public Dictionary<char, int> LetterFrequency;
        }

        private static List<TextStatistics> allStatistics = new List<TextStatistics>();

        static void Main(string[] args)
        {
            Console.WriteLine("=== Анализатор текста ===");

            bool continueWorking = true;

            while (continueWorking)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1 - Анализ нового текста");
                Console.WriteLine("2 - Просмотр статистики по прошлым текстам");
                Console.WriteLine("3 - Выход");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AnalyzeNewText();
                        break;
                    case "2":
                        ShowPreviousStatistics();
                        break;
                    case "3":
                        continueWorking = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }

        
        }

        // Метод для анализа нового текста
        static void AnalyzeNewText()
        {
            Console.WriteLine("\nВведите текст:");
            string text = Console.ReadLine();

        
            if (text == null) return;

            // Создание структуры для хранения статистики
            TextStatistics stats = new TextStatistics();

            // Выполнение всех анализов
            stats.WordCount = CountWords(text);
            stats.ShortestWord = FindShortestWord(text);
            stats.SentenceCount = CountSentences(text);
            CountVowelsAndConsonants(text, out int vowelsCount, out int consonantsCount);
            stats.VowelCount = vowelsCount;
            stats.ConsonantCount = consonantsCount;
            stats.LongestWord = FindLongestWord(text);
            stats.LetterFrequency = CalculateLetterFrequency(text);

            // Добавление статистики в общий список
            allStatistics.Add(stats);

            // Вывод результатов текущего анализа
            DisplayCurrentStatistics(stats);
        }

        // Подсчет количества слов в тексте (простая версия)
        static int CountWords(string text)
        {
            int wordCount = 1;

            foreach (char c in text)
            {
                if (c == ' ') wordCount++;
            }

            return wordCount;
        }

        // Поиск самого короткого слова
        static string FindShortestWord(string text)
        {
            string[] words = text.Split(' ');
            string shortestWord = words[0];

            foreach (string word in words)
            {
                if (word.Length < shortestWord.Length && word.Length > 0)
                {
                    shortestWord = word;
                }
            }

            return shortestWord;
        }

        // Подсчет количества предложений
        static int CountSentences(string text)
        {
            int sentenceCount = 0;

            foreach (char c in text)
            {
                if (c == '.' || c == '!' || c == '?')
                {
                    sentenceCount++;
                }
            }

            return sentenceCount;
        }

        // Подсчет гласных и согласных букв
        static void CountVowelsAndConsonants(string text, out int vowelCount, out int consonantCount)
        {
            vowelCount = 0;
            consonantCount = 0;

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    char lowerC = char.ToLower(c);

                    // Простая проверка на гласные
                    if (lowerC == 'а' || lowerC == 'е' || lowerC == 'ё' || lowerC == 'и' || lowerC == 'о' ||
                        lowerC == 'у' || lowerC == 'ы' || lowerC == 'э' || lowerC == 'ю' || lowerC == 'я' ||
                        lowerC == 'a' || lowerC == 'e' || lowerC == 'i' || lowerC == 'o' || lowerC == 'u')
                    {
                        vowelCount++;
                    }
                    else
                    {
                        consonantCount++;
                    }
                }
            }
        } 

        // Поиск самого длинного слова
        static string FindLongestWord(string text)
        {
            string[] words = text.Split(' ');
            string longestWord = "";

            foreach (string word in words)
            {
                if (word.Length > longestWord.Length)
                {
                    longestWord = word;
                }
            }

            return longestWord;
        }

        // Создание статистики по частоте встречаемости букв
        static Dictionary<char, int> CalculateLetterFrequency(string text)
        {
            Dictionary<char, int> frequency = new Dictionary<char, int>();

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    char lowerC = char.ToLower(c);

                    if (frequency.ContainsKey(lowerC))
                    {
                        frequency[lowerC]++;
                    }
                    else
                    {
                        frequency[lowerC] = 1;
                    }
                } 
            }

            return frequency;
        }
         
        // Вывод статистики по текущему тексту
        static void DisplayCurrentStatistics(TextStatistics stats)
        {
            Console.WriteLine("\n=== РЕЗУЛЬТАТЫ АНАЛИЗА ===");
            Console.WriteLine($"Количество слов: {stats.WordCount}");
            Console.WriteLine($"Самое короткое слово: \"{stats.ShortestWord}\"");
            Console.WriteLine($"Количество предложений: {stats.SentenceCount}");
            Console.WriteLine($"Гласных букв: {stats.VowelCount}");
            Console.WriteLine($"Согласных букв: {stats.ConsonantCount}");
            Console.WriteLine($"Самое длинное слово: \"{stats.LongestWord}\"");

            Console.WriteLine("\nЧастота букв:");
            foreach (var pair in stats.LetterFrequency)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }
        }

        // Вывод статистики по прошлым текстам
        static void ShowPreviousStatistics()
        {
            if (allStatistics.Count == 0)
            {
                Console.WriteLine("Статистика по прошлым текстам отсутствует.");
                return;
            }

            Console.WriteLine($"\n=== СТАТИСТИКА ПО {allStatistics.Count} ТЕКСТАМ ===");

            for (int i = 0; i < allStatistics.Count; i++)
            {
                Console.WriteLine($"\n--- Текст #{i + 1} ---");
                Console.WriteLine($"Количество слов: {allStatistics[i].WordCount}");
                Console.WriteLine($"Количество предложений: {allStatistics[i].SentenceCount}");
                Console.WriteLine($"Гласные/Согласные: {allStatistics[i].VowelCount}/{allStatistics[i].ConsonantCount}");
                Console.WriteLine($"Самое короткое слово: \"{allStatistics[i].ShortestWord}\"");
                Console.WriteLine($"Самое длинное слово: \"{allStatistics[i].LongestWord}\"");
               
            }
        }
    }
}