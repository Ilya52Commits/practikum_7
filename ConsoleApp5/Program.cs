﻿using System;
using System.Collections.Generic;
using System.IO;

namespace practice_7
{
    internal class Program
    {
        struct Ingredient
        {
            public string name;
            public int quantity;
            public string measure;
        }

        static Dictionary<string, List<Ingredient>> FileData()
        {
            string path = "dishes.txt";

            var cook_book = new Dictionary<string, List<Ingredient>> { };

            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var name = reader.ReadLine();

                    if (string.IsNullOrEmpty(name)) 
                        continue;

                    var quantity = int.Parse(reader.ReadLine());
                    var ingredients = new List<Ingredient> { };

                    for (int i = 0; i < quantity; i++)
                    {
                        var ingredient = reader.ReadLine().Split('|');

                        ingredients.Add(new Ingredient
                        {
                            name = ingredient[0].Trim(),
                            quantity = int.Parse(ingredient[1].Trim()),
                            measure = ingredient[2].Trim()
                        });
                    }

                    cook_book.Add(name, ingredients);
                }

                reader.Close();
            }

            return cook_book;
        }

        struct dishesPersons
        {
            public int quan;
            public string meas;
        }

        static Dictionary<string, List<dishesPersons>> get_shop_list_by_dishes(List<string> dishes, int person_count)
        {
            var cook_book = FileData();
            var dishesPersons = new Dictionary<string, List<dishesPersons>> { };
            var k = 0;

            foreach (var kyeDishes in cook_book)
            {
                foreach (var stringDishes in dishes)
                {
                    if (stringDishes == kyeDishes.Key)
                    {
                        foreach (var items in kyeDishes.Value)
                        {
                            var nams = items.name;
                            var r = string.Empty;                           
                            var ingredient = new List<dishesPersons> { };

                            foreach (var i in dishesPersons)
                            {
                                if (i.Key == nams)
                                {
                                    foreach (var j in i.Value)
                                    {
                                        k = j.quan;
                                        r = nams;
                                    }
                                }
                            }

                            dishesPersons.Remove(r);
                            ingredient.Add(new dishesPersons
                            {
                                quan = items.quantity * person_count + k,
                                meas = items.measure,
                            });

                            k = 0;

                            dishesPersons.Add(nams, ingredient);
                        }
                    }
                }
            }

            return dishesPersons;
        }

        static void FileSort()
        {
            var CountItems = new List<int>();
            var name = new List<string>();

            File.Delete("Finish.txt");

            DirectoryInfo files = new DirectoryInfo("files");

            for (int i = 1; i <= files.GetFiles().Length; i++)
            {
                int linesCount = 1;
                int nextLine = '\n';

                using (var streamReader = new StreamReader(
                    new BufferedStream(
                        File.OpenRead("files\\" + i + ".txt"), 10 * 1024 * 1024)))
                {
                    while (!streamReader.EndOfStream)
                        if (streamReader.Read() == nextLine) 
                            linesCount++;
                }

                CountItems.Add(linesCount);

                name.Add(i + ".txt");
            }

            for (int i = 0; i < CountItems.Count; i++)
            {
                for (int j = i + 1; j < CountItems.Count; j++)
                {
                    if (CountItems[i] > CountItems[j])
                    {
                        int tmp = CountItems[i];

                        CountItems[i] = CountItems[j];
                        CountItems[j] = tmp;

                        string tmp1 = name[i];

                        name[i] = name[j];
                        name[j] = tmp1;
                    }
                }
            }

            for (int i = 0; i < name.Count; i++)
            {
                File.AppendAllText("Finish.txt", name[i]);
                File.AppendAllText("Finish.txt", "\n");
                File.AppendAllText("Finish.txt", CountItems[i].ToString());
                File.AppendAllText("Finish.txt", "\n");

                string tmp = File.ReadAllText("files\\" + name[i]);

                File.AppendAllText("Finish.txt", tmp);
                File.AppendAllText("Finish.txt", "\n");
            }
        }

        static void Main(string[] args)
        {
            var cook_book = FileData();

            foreach (var ttt in cook_book)
            {
                Console.WriteLine(ttt.Key);
                Console.WriteLine(ttt.Value.Count);

                foreach (var ppp in ttt.Value)
                    Console.WriteLine($"{ppp.name} | {ppp.quantity} | {ppp.measure}");

                Console.WriteLine();
            }

            var dishes = get_shop_list_by_dishes(new List<string> { "Омлет", "Фахитос", "Утка по-пекински" }, 2);

            foreach (var ttt in dishes)
            {
                Console.Write($"{ttt.Key} | ");

                foreach (var ppp in ttt.Value)
                    Console.WriteLine($"{ppp.quan} | {ppp.meas}");
            }

            FileSort();
        }
    }
}