using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace csharpPrac1grade2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Налаштування кодування для коректного відображення кирилиці
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("=== Аналізатор Тексту ===");
            Console.WriteLine("Введіть повний шлях до вашого .txt файлу (або перетягніть файл у вікно консолі):");

            // Читання шляху та видалення зайвих лапок (якщо файл перетягнули мишкою)
            string filePath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                Console.WriteLine("Помилка: Файл не знайдено. Перевірте правильність шляху.");
                return;
            }

            try
            {
                // 1. Зчитування всього тексту з файлу
                string text = File.ReadAllText(filePath).ToLower();

                // 2. Витягування лише слів (ігноруємо пробіли, крапки, коми тощо)
                // \b - межа слова, \w+ - одне або більше буквено-цифрових символів
                var words = Regex.Matches(text, @"\b\w+\b")
                                 .Cast<Match>()
                                 .Select(m => m.Value)
                                 .ToList();

                if (words.Count == 0)
                {
                    Console.WriteLine("У файлі не знайдено жодного слова для аналізу.");
                    return;
                }

                // 3. Аналіз слів за допомогою LINQ
                var wordStatistics = words
                    .GroupBy(word => word)
                    .Select(group => new { Word = group.Key, Count = group.Count() })
                    .OrderByDescending(x => x.Count)
                    .ToList();

                int totalWords = words.Count;
                int uniqueWords = wordStatistics.Count;

                // 4. Виведення результатів
                Console.WriteLine("\n--- Статистика файлу ---");
                Console.WriteLine($"Загальна кількість слів: {totalWords}");
                Console.WriteLine($"Кількість унікальних слів: {uniqueWords}");
                Console.WriteLine("\n--- Топ-10 найпопулярніших слів ---");

                var top10 = wordStatistics.Take(10);
                foreach (var item in top10)
                {
                    Console.WriteLine($"- {item.Word}: {item.Count}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при обробці файлу: {ex.Message}");
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
            Console.ReadKey();
        }
    }
}