using System;
using System.Collections.Generic;
using System.Text;

namespace dfg
{
    internal class Program
    {
        // Структура для хранения статистики по тексту
        struct TextStatistics
        {
            public string Text;
            public int WordCount;
            public string ShortestWord;
            public int SentenceCount;
            public int VowelCount;
            public int ConsonantCount;
            public string LongestWord;
            public Dictionary<char, int> LetterFrequency;
            public DateTime AnalysisTime;
        }

        // Список для хранения статистики по всем текстам
        private static List<TextStatistics> allStatistics = new List<TextStatistics>();

        // Наборы гласных букв (русский и английский алфавиты)
        private static readonly HashSet<char> vowels = new HashSet<char>("аеёиоуыэюяaeiou");
        private static readonly HashSet<char> consonants = new HashSet<char>("бвгджзйклмнпрстфхцчшщbcdfghjklmnpqrstvwxyz");

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

            Console.WriteLine("Программа завершена. До свидания!");
            Console.ReadKey();
        }

        // Метод для анализа нового текста
        static void AnalyzeNewText()
        {
            Console.WriteLine("\nВведите текст (минимум 100 символов):");
            string text = Console.ReadLine();

            // Проверка минимальной длины текста
            while (text != null && text.Length < 100)
            {
                Console.WriteLine($"Текст слишком короткий! Введено {text.Length} символов. Нужно минимум 100.");
                Console.WriteLine("Пожалуйста, введите текст еще раз:");
                text = Console.ReadLine();
            }

            if (text == null) return;

            // Создание структуры для хранения статистики
            TextStatistics stats = new TextStatistics();
            stats.Text = text;
            stats.AnalysisTime = DateTime.Now;

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

        // Подсчет количества слов в тексте
        static int CountWords(string text)
        {
            int wordCount = 0;
            bool inWord = false;

            foreach (char c in text)
            {
                // Проверяем, является ли символ частью слова (буква или апостроф)
                if (char.IsLetter(c) || c == '\'')
                {
                    if (!inWord)
                    {
                        wordCount++;
                        inWord = true;
                    }
                }
                else
                {
                    inWord = false;
                }
            }

            return wordCount;
        }

        // Поиск самого короткого слова
        static string FindShortestWord(string text)
        {
            string shortestWord = "";
            int shortestLength = int.MaxValue;
            StringBuilder currentWord = new StringBuilder();

            foreach (char c in text)
            {
                if (char.IsLetter(c) || c == '\'')
                {
                    currentWord.Append(c);
                }
                else
                {
                    if (currentWord.Length > 0)
                    {
                        if (currentWord.Length < shortestLength)
                        {
                            shortestLength = currentWord.Length;
                            shortestWord = currentWord.ToString();
                        }
                        currentWord.Clear();
                    }
                }
            }

            // Проверяем последнее слово
            if (currentWord.Length > 0 && currentWord.Length < shortestLength)
            {
                shortestWord = currentWord.ToString();
            }

            return shortestWord;
        }

        // Подсчет количества предложений
        static int CountSentences(string text)
        {
            int sentenceCount = 0;
            bool inSentence = false;

            foreach (char c in text)
            {
                if (char.IsLetter(c) || char.IsDigit(c))
                {
                    if (!inSentence)
                    {
                        inSentence = true;
                    }
                }
                else if (c == '.' || c == '!' || c == '?' || c == ';')
                {
                    if (inSentence)
                    {
                        sentenceCount++;
                        inSentence = false;
                    }
                }
            }

            // Если текст заканчивается без знака препинания
            if (inSentence)
            {
                sentenceCount++;
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
                char lowerC = char.ToLower(c);

                if (vowels.Contains(lowerC))
                {
                    vowelCount++;
                }
                else if (consonants.Contains(lowerC))
                {
                    consonantCount++;
                }
            }
        }

        // Поиск самого длинного слова
        static string FindLongestWord(string text)
        {
            string longestWord = "";
            int longestLength = 0;
            StringBuilder currentWord = new StringBuilder();

            foreach (char c in text)
            {
                if (char.IsLetter(c) || c == '\'')
                {
                    currentWord.Append(c);
                }
                else
                {
                    if (currentWord.Length > longestLength)
                    {
                        longestLength = currentWord.Length;
                        longestWord = currentWord.ToString();
                    }
                    currentWord.Clear();
                }
            }

            // Проверяем последнее слово
            if (currentWord.Length > longestLength)
            {
                longestWord = currentWord.ToString();
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

            Console.WriteLine($"\nВремя анализа: {stats.AnalysisTime}");
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
                Console.WriteLine($"Время анализа: {allStatistics[i].AnalysisTime}");
                Console.WriteLine($"Количество слов: {allStatistics[i].WordCount}");
                Console.WriteLine($"Количество предложений: {allStatistics[i].SentenceCount}");
                Console.WriteLine($"Гласные/Согласные: {allStatistics[i].VowelCount}/{allStatistics[i].ConsonantCount}");
                Console.WriteLine($"Самое короткое слово: \"{allStatistics[i].ShortestWord}\"");
                Console.WriteLine($"Самое длинное слово: \"{allStatistics[i].LongestWord}\"");

                // Для экономии места не выводим полную статистику по буквам для всех текстов
                Console.WriteLine($"Уникальных букв: {allStatistics[i].LetterFrequency.Count}");
            }
        }
    } 
}
