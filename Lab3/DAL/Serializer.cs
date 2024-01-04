using Lab3.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Text.Json;
using System.IO;
using System.Xml.Serialization;
using System.Data.SQLite;
using System.Diagnostics;
using Microsoft.Data.Sqlite;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Lab3.DAL
{
    public static class Serializer
    {
        public static void SaveToJson(string path, Dictionary dict)
        {
            var dictDto = GetDictionaryDto(dict);

            var options = new JsonSerializerOptions();
            string jsonString = JsonSerializer.Serialize(dictDto);

            File.WriteAllText(path, jsonString);
            Console.WriteLine("Данные успешно сохранены");
        }

        public static void FromJson(string path)
        {
            string jsonString = File.ReadAllText(path);
            var dictDto = JsonSerializer.Deserialize<DictionaryDto>(jsonString);

            foreach (var word in dictDto.Words)
            {
                Console.WriteLine(word.SplittedWord);
            }
        }

        public static void SaveToXml(string path, Dictionary dict)
        {
            var dictDto = GetDictionaryDto(dict);
            XmlSerializer x = new XmlSerializer(dictDto.GetType());

            StreamWriter myWriter = new StreamWriter(path);
            x.Serialize(myWriter, dictDto);
            myWriter.Close();
        }

        public static void FromXml(string path, Dictionary dict)
        {
            var dictDto = GetDictionaryDto(dict);
            var mySerializer = new XmlSerializer(dictDto.GetType());
            var myFileStream = new FileStream(path, FileMode.Open);
            var myObject = (DictionaryDto)mySerializer.Deserialize(myFileStream);

            foreach (var word in myObject.Words)
            {
                Console.WriteLine(word.SplittedWord);
            }

            myFileStream.Close();

            Console.WriteLine("Данные успешно сохранены");
        }



        public static void SaveToBD(Dictionary dict)
        {
            var dictDto = GetDictionaryDto(dict);
            using (var conn = new SQLiteConnection("Data Source=dictionary.db"))
            {
                conn.Open();

                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "CREATE TABLE IF NOT EXISTS Dictionary (Prefix TEXT, Root TEXT, Suffix TEXT, Ending TEXT)";
                    command.ExecuteNonQuery();

                    foreach (WordDto word in dictDto.Words)
                    {
                        command.CommandText = "INSERT INTO Dictionary (Prefix, Root, Suffix, Ending) VALUES (:prefix, :root, :suffix, :ending)";
                        command.Parameters.Clear();

                        command.Parameters.AddWithValue("prefix", word.Prefix);
                        command.Parameters.AddWithValue("root", word.Root);
                        command.Parameters.AddWithValue("suffix", word.Suffix);
                        command.Parameters.AddWithValue("ending", word.Ending);

                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine("Данные успешно загружены");
                }
            }
        }

        public static void FromBD()
        {
            using (var conn = new SQLiteConnection("Data Source=dictionary.db;Version=3;"))
            {
                Dictionary dict = new Dictionary();

                conn.Open();

                using (var cmd = new SQLiteCommand("SELECT * FROM Dictionary", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string prefix = reader["Prefix"].ToString();
                            string root = reader["Root"].ToString();
                            string suffix = reader["Suffix"].ToString();
                            string ending = reader["Ending"].ToString();
                            dict.AddWord(new Word(prefix, root, suffix, ending));
                        }
                    }
                }
                foreach (var word in dict.Words)
                {
                    Console.WriteLine(word.SplittedWord);
                }
            }
        }

        private static DictionaryDto GetDictionaryDto(Dictionary dict)
        {
            return new DictionaryDto
            {
                Words = dict.Words.Select(w => new WordDto
                {
                    Ending = w.Ending,
                    FullWord = w.FullWord,
                    Prefix = w.Prefix,
                    Root = w.Root,
                    SplittedWord = w.SplittedWord,
                    Suffix = w.Suffix
                }).ToList()
            };
        }

        private static WordDto GetWordDto(Word word)
        {
            return new WordDto
            {
                Prefix = word.Prefix,
                Root = word.Root,
                Suffix = word.Suffix,
                Ending = word.Ending,
                FullWord = word.FullWord,
                SplittedWord = word.SplittedWord,
                Count = word.Count
            };
        }
    }
}
