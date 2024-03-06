using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Quizbot
{
    public class Test
    {
        public string Question { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string CorrectAnswer { get; set; }
    }

    public static class Testlar
    {
        public static List<Test> GetTest()
        {
            var stringTest = File.ReadAllText("/Users/otabek_coding/Desktop/Najot Ta'lim/Quizbot/Testlar.json");
            var tests = JsonSerializer.Deserialize<List<Test>>(stringTest);

            return tests;
        }

        public static async Task Create(List<string> list)
        {
            try
            {
                var filePath = "/Users/otabek_coding/Desktop/Najot Ta'lim/Quizbot/Testlar.json";

                var stringTest = await File.ReadAllTextAsync(filePath);
                var tests = JsonSerializer.Deserialize<List<Test>>(stringTest) ?? new List<Test>();

                Test test = new Test
                {
                    Question = list[0],
                    A = list[1],
                    B = list[2],
                    C = list[3],
                    D = list[4],
                    CorrectAnswer = list[5]
                };

                tests.Add(test);

                string json1 = JsonSerializer.Serialize(tests);
                await File.WriteAllTextAsync(filePath, json1);

                Console.WriteLine("Quiz added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static List<Test> GetBiologiya()
        {
            var stringTest = File.ReadAllText("/Users/otabek_coding/Desktop/Najot Ta'lim/Quizbot/General.json");
            var tests = JsonSerializer.Deserialize<List<Test>>(stringTest);

            return tests;
        }
    }
}
