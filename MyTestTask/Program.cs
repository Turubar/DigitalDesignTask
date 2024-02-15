using System.Text;
using System;
using System.Diagnostics;

namespace MyTestTask
{
    internal class Program
    {
        // путь к файлу с тестовыми данными
        public static string pathToFile = "../../../test_data.txt";

        static void Main(string[] args)
        {
            // проверяем существование тестового файла (он требуется для корректной работы приложения)
            if (!File.Exists(pathToFile))
            {
                Console.WriteLine("Что-то пошло не так, тестовый файл отсутствует!");
                Environment.Exit(1);
            }

            // проверяем передаваемые параметры, чтобы можно было использовать приложение через консоль
            if(args.Length == 1)
            {
                if (File.Exists(args[0]))
                {
                    if (Path.GetExtension(args[0]) != ".txt")
                    {
                        Console.WriteLine("Некорректное расширение файла, файл должен иметь расширение (.txt)!");
                        Console.WriteLine("По умолчанию будет использован тестовый файл из папки проекта ('../../test_data.txt')");
                    }
                    else pathToFile = args[0];
                }
                else
                {
                    Console.WriteLine("Некорректный путь, файл не найден!");
                    Console.WriteLine("По умолчанию будет использован тестовый файл из папки проекта ('../../test_data.txt')");
                }
            }
            else
            {
                Console.WriteLine("Вы не указали путь к входному файлу (.txt)!");
                Console.WriteLine("По умолчанию будет использован тестовый файл из папки проекта ('../../test_data.txt')");
            }

            // создаем словарь <string, int>, в key(string) будем записывать слова, в value(int) будем хранить кол-во повторений 
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            // создаем массив символов, которые должны быть удалены из строк
            string[] symbols = new string[] { " " ," ", ",", ".", "!", "?", ":", ";", "-", "–", "(", ")", "…", "«", "»", "[", "]", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};

            // создаем цикл, в котором построчно будет читать файл
            foreach (var line in File.ReadLines(pathToFile))
            {
                // разбиваем строку с помощью ненужных символов, при этом удаляя пропуски
                var words = line.Split(symbols, StringSplitOptions.RemoveEmptyEntries);

                // создаем цикл, в котором будем проходится по полученному массиву строк
                foreach (var word in words)
                {
                    // удаляем ненужные пробелы (если они есть)
                    string clearWord = word.Trim();

                    // проверяем слово на уникальность, если в словаре его нет, то добавляем его, иначе увеличиваем счетчик повторений
                    if (!dictionary.ContainsKey(clearWord)) dictionary.Add(clearWord, 1);
                    else dictionary[clearWord]++;
                }
            }

            // путь к файлу с результатом работы (в той же папке)
            string pathToResult = new FileInfo(pathToFile).Directory.FullName + "\\result.txt";

            using (var sw = new StreamWriter(pathToResult))
            {
                // проходимся по словарю (отсорт. по убыванию value) и записываем результат в файл
                foreach (var item in dictionary.OrderByDescending(x => x.Value))
                    sw.WriteLine($"{item.Key,-35} {item.Value}");
            }

            Console.WriteLine("Файл с результатом работы вы сможете найти по пути: " + pathToResult);
        }
    }
}