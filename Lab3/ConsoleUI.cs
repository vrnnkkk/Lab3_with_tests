using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
using System.Collections;
using Lab3.DAL;
//using Newtonsoft.Json;
using System.IO;
using Lab3.DAL.DTOs;

namespace Lab3
{
    public class ConsoleUI
    {
        Word same_word;
        Dictionary dict = new Dictionary();

        public void Begin()
        {
            do
            {
                Start();
            } while (true);
        }

        public void Start()
        {
            PrintConsoleMenu();

            ConsoleKey Key = Console.ReadKey().Key;

            switch (Key)
            {
                case ConsoleKey.D1:
                    {
                        Console.WriteLine();
                        ReadWord();
                        break;
                    }

                case ConsoleKey.D2:
                    {
                        Console.WriteLine();
                        Serializer.SaveToJson("lab3.json", dict);
                        break;
                    }

                case ConsoleKey.D3:
                    {
                        Console.WriteLine();
                        Serializer.FromJson("lab3.json");
                        break;
                    }

                case ConsoleKey.D4:
                    {
                        Console.WriteLine();
                        Serializer.SaveToXml("lab3.xml", dict);
                        break;
                    }

                case ConsoleKey.D5:
                    {
                        Console.WriteLine();
                        Serializer.FromXml("lab3.xml", dict);
                        break;
                    }

                case ConsoleKey.D6:
                    {
                        Console.WriteLine();
                        Serializer.SaveToBD(dict);
                        break;
                    }

                case ConsoleKey.D7:
                    {
                        Console.WriteLine();
                        Serializer.FromBD();
                        break;
                    }

                case ConsoleKey.D8:
                    {
                        Console.WriteLine();
                        Environment.Exit(0);
                        break;
                    }

                case ConsoleKey.Q:
                    {
                        Console.WriteLine();
                        Environment.Exit(0);
                        break;
                    }

                default:
                    {
                        Console.WriteLine();
                        Console.WriteLine("Пункт меню не найден\n");
                        break;
                    }
            }
        }

        public void ReadWord()
        {
            Console.Write("Введите слово: ");
            String new_word = Console.ReadLine().ToLower();

            if (new_word.Equals("q"))
            {
                Environment.Exit(0);
            }

            if (string.IsNullOrEmpty(new_word))
            {
                Console.WriteLine("Вы не ввели слово :(\n");
                ReadWord();
            }

            var word = dict.GetWordByText(new_word);

            if (word != null)
            {
                Console.WriteLine("Это слово есть в словаре. Список слов с таким корнем:");
                PrintCognate(word);
                Console.Write("\n");
            }
            else
            {
                Console.WriteLine("Неизвестное слово. Хотите добавить его в словарь? (y/n/q - вернуться в меню)");
                String answer = Console.ReadLine().ToLower();
                if (answer.Equals("y"))
                {
                    ReadNewWord();
                    Check(new_word);
                }
                else if (answer.Equals("n"))
                {
                    ReadWord();
                }
                else if (answer.Equals("q"))
                {
                    Start();
                }
                else
                {
                    Console.WriteLine("Неккоректный ответ :(\n");
                    ReadWord();
                }
            }
        }

        public void ReadNewWord()
        {
            Console.Write("Введите префикс: ");
            string prefix = Console.ReadLine().ToLower();

            Console.Write("Введите корень: ");
            string root = Console.ReadLine().ToLower();

            Console.Write("Введите суффиксы: ");
            string suffix = Console.ReadLine().ToLower();

            Console.Write("Введите окончание: ");
            string ending = Console.ReadLine().ToLower();

            same_word = new Word(prefix, root, suffix, ending);
        }

        public void Check(String new_word)
        {
            if (same_word.CompareWord(new_word))
            {
                dict.AddWord(same_word);

                Console.WriteLine($"Слово '{same_word.FullWord}' успешно добавлено в словарь.\n");
            }
            else
            {
                Console.WriteLine("Слова не совпали. Попробуйте снова.\n");
                ReadNewWord();
                Check(new_word);
            }
        }

        public void PrintCognate(Word enteredWord)
        {
            foreach (Word existingWord in dict.Words)
            {
                if (existingWord.HasTheSameRoot(enteredWord))
                {
                    PrintWord(existingWord);
                }
            }
        }

        public void PrintWord(Word w)
        {
            Console.WriteLine(w.SplittedWord);
        }

        static void PrintConsoleMenu()
        {
            var menuItems = new[]
            {
                "1) Ввести слово",
                "2) Сохранить в JSON",
                "3) Загрузить из JSON",
                "4) Сохранить в XML",
                "5) Загрузить из XML",
                "6) Сохранить в SQLite",
                "7) Загрузить из SQLite",
                "8) Выход (q)"
            };

            foreach (var item in menuItems)
            {
                Console.WriteLine(item);
            }
        }
    }
}