using Newtonsoft.Json;
using System;
namespace Quizbot;

public class Class1
{
    public string Question { get; set; }
    public string A { get; set; }
    public string B { get; set; }
    public string C { get; set; }
    public string D { get; set; }
    public string CorrectAnswer { get; set; }


    public void GetData(string quest,string ans1,string ans2,string ans3, string ans4,string cur)
    {
        List<Class1> savollar = new List<Class1>();
        
            var class1 = new Class1
            {
                Question = quest,
                A = ans1,
                B = ans2,
                C =ans3,
                D = ans4,
                CorrectAnswer=cur
            };

            savollar.Add(class1);
        

        SaveQuestionsToJson(savollar);
        UpdateQuestion(savollar);

        Console.WriteLine("JSON saved.");
        Console.WriteLine("\nRemaining questions:");
        foreach (var savol in savollar)
        {
            Console.WriteLine($"{savol.Question}\nA. {savol.A}\nB. {savol.B}\nC. {savol.C}\nD. {savol.D}\n");
        }
    }


    public void DeleteQuestion(List<Class1> savollar)
    {
        Console.WriteLine("Choose which question you wanna delete (1 dan {0} gacha):", savollar.Count);
        int ochirishIndeksi = Convert.ToInt32(Console.ReadLine()) - 1;

        if (ochirishIndeksi >= 0 && ochirishIndeksi < savollar.Count)
        {
            savollar.RemoveAt(ochirishIndeksi);
            Console.WriteLine("Question deleted!");
        }
        else
        {
            Console.WriteLine("Invalid input, question not deleted.");
        }
    }

    static void SaveQuestionsToJson(List<Class1> savollar)
    {
        string json = JsonConvert.SerializeObject(savollar, Formatting.Indented);
        File.WriteAllText("/Users/otabek_coding/Desktop/Najot Ta'lim/Quizbot/jsconfig2.json", json);
        Console.WriteLine("JSON saved to file!");
    }


    static void UpdateQuestion(List<Class1> savollar)
    {
        Console.WriteLine("Choose which question you wanna delete (1 dan {0} gacha):", savollar.Count);
        int yangilashIndeksi = Convert.ToInt32(Console.ReadLine()) - 1;

        if (yangilashIndeksi >= 0 && yangilashIndeksi < savollar.Count)
        {
            Console.WriteLine("Enter new data:");

            savollar[yangilashIndeksi].Question = Console.ReadLine();
            savollar[yangilashIndeksi].A = Console.ReadLine();
            savollar[yangilashIndeksi].B = Console.ReadLine();
            savollar[yangilashIndeksi].C = Console.ReadLine();
            savollar[yangilashIndeksi].D = Console.ReadLine();

            SaveQuestionsToJson(savollar); 
            Console.WriteLine("Question updated");
        }
        else
        {
            Console.WriteLine("Invalid input, question not updated.");
        }
    }
}