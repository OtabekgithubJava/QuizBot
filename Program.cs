// using Newtonsoft.Json;

using System.Text;
using System.Text.Json;
using Quizbot;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;


public class Program
{
    public static int TestNumber=0;
    public static int TestNumber1;
    public static int correctAnswer=0;
    public static string PhoneNumber;
    public static string adminText;
    public static int questionNum;
    public static string question;
    public static string answer1;
    public static string answer2;
    public static string answer3;
    public static string answer4;
    public static string correctanswer;
    public static int Status = 0;
    public static List<string> list = new List<string>();
    public static int update_int = 0;

    
    // public  enum TestType
    // {
    //     BiologyTest,
    //     GeneralTest,
    //     EnglishTest,
    // }


    
    public static async Task Main(string[] args)
    {
        var botClient = new TelegramBotClient("X"); // gotta edit hide the token
        PdfDownload pdfDownload = new PdfDownload();
        ContactPdf contactPdf = new ContactPdf();
        Class1 class1 = new Class1();
        QuestionAnswerPdf questionAnswerPdf = new QuestionAnswerPdf();
        using CancellationTokenSource cts = new();
        
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
        };
        try
        {

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
        }
        catch( Exception ex ) 
        {
            Console.WriteLine(ex.Message);
        }
            
        var me = await botClient.GetMeAsync();

        Console.WriteLine($"Start listening for @{me.Username}");
        Console.ReadLine();

        // Send cancellation request to stop bot
        
        cts.Cancel();

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {


            var handler = update.Type switch
            {
                UpdateType.Message => HandleMessageAsync(botClient, update, cancellationToken),
                UpdateType.CallbackQuery => HandleCallBackQueryAsync(botClient, update, cancellationToken)
            };

            try
            {
                await handler;
            }catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Error: {ex.Message}");
            }
            
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] { new KeyboardButton("Admin"), new KeyboardButton("General") },
                new KeyboardButton[] { new KeyboardButton("Start") },
            });


            ReplyKeyboardMarkup replyKeyboardMarkup2 = new(new[]
            {
                new KeyboardButton[] { "Users List", new KeyboardButton("Subject CRUD")},
                new KeyboardButton[] { "Back ", new KeyboardButton("Back " ) }
            }

            );
            ReplyKeyboardMarkup replyKeyboardMarkup3 = new(new[]
            {
                new KeyboardButton[] { "Create", new KeyboardButton("Update") },
                new KeyboardButton[] { "Delete", new KeyboardButton("Start") },
            }
            );

            if (update.Message is not { } message)
                return;

            if (message.Text is not { } messageText)
                return;
            
            var chatId = message.Chat.Id;
            
            if (messageText == "/start" || messageText.ToLower() == "start")
            {
                await SendStartMessageAsync(chatId, botClient, cancellationToken);
            }
            else if (messageText == "Users List")
            {
                await using Stream stream = System.IO.File.OpenRead("/Users/otabek_coding/Desktop/Najot Ta'lim/Quizbot/contact/hello-from-contact.pdf");
                await botClient.SendDocumentAsync(
                    chatId: update.Message.Chat.Id,
                    document: InputFile.FromStream(stream: stream, fileName: $"All_users2.pdf"),
                    caption: "All user info"
                    );
                stream.Dispose();
            }
            else if (messageText.ToLower() == "share contact")
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    // P/s: Faqat meni no'merimga ishledi
                    text: "Please send you phone number to get the code to be an admin!\n",
                    cancellationToken: cancellationToken);
            }
            else if (messageText.StartsWith("+9989"))
            {
                PhoneNumber = messageText;
                TwilioName twilioName = new TwilioName(PhoneNumber);
                // twilioName.GetMethod();
                // contactPdf.GetMethod(PhoneNumber);
                
                await botClient.SendTextMessageAsync(
                   chatId: chatId,
                   text: "Now enter the code:",
                   replyMarkup: replyKeyboardMarkup,
                   cancellationToken: cancellationToken);
            }
            else if (messageText == "Send")
            {
              //  class1.GetData(question,answer1,answer2,answer3,answer4,correctanswer);
              
            }
            else if (messageText == "User")
            {
                await UserMessageAsync(chatId, botClient, cancellationToken);
            }
            
            else if (messageText == "Back")
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "choose",
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken);
            }
            else if (messageText == "Admin")
            {
                await AdminRegisterationMessageAsync(chatId, botClient, cancellationToken);
            }
            else if (messageText == "Subject CRUD" || messageText == "CRUD")
            {
                await CRUDMessageAsync(chatId, botClient, cancellationToken);
            }
            else if (messageText.StartsWith("34267"))
            {
                adminText = messageText;
                await botClient.SendTextMessageAsync(
               chatId: chatId,
              text: "Choose",
               replyMarkup: replyKeyboardMarkup2,
              cancellationToken: cancellationToken);
            }
            else if (messageText == "BACK")
            {
                adminText = messageText;
                await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: "Choose",
               replyMarkup: replyKeyboardMarkup2,
               cancellationToken: cancellationToken);
            }
            else if (messageText == "1")
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "The following question has been deleted:" +
                          "[\n  {\n    \"Question\": \"How many years does a baobab tree live?\",\n    \"A\": \"100\",\n    \"B\": \"1000\",\n    \"C\": \"2000\",\n    \"D\": \"4000\",\n    \"CorrectAnswer\": \"D\"\n  },\n  {\n    \"Question\": \"Which plants are resistant to camels?\",\n    \"A\": \"Ajriq\",\n    \"B\": \"Namatak\",\n    \"C\": \"Makkajoxori\",\n    \"D\": \"Bugdoy\",\n    \"CorrectAnswer\": \"B\"\n  },\n  {\n    \"Question\": \"How many types of plastids are there?\",\n    \"A\": \"3\",\n    \"B\": \"2\",\n    \"C\": \"4\",\n    \"D\": \"0\",\n    \"CorrectAnswer\": \"A\"\n  },\n  {\n    \"Question\": \"Find the star-shaped flowers.\",\n    \"A\": \"Lavlagi\",\n    \"B\": \"Karam\",\n    \"C\": \"Bugdoy\",\n    \"D\": \"Mosh\",\n    \"CorrectAnswer\": \"A\"\n  },\n  {\n    \"Question\": \"Identify an annual plant.\",\n    \"A\": \"Bugdoy\",\n    \"B\": \"Ajriq\",\n    \"C\": \"Namatak\",\n    \"D\": \"Yantoq\",\n    \"CorrectAnswer\": \"A\"\n  }\n]",
                    replyMarkup: replyKeyboardMarkup2,
                    cancellationToken: cancellationToken);
            }
            else if (messageText == "2")
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "The following question has been updated:" +
                          "[\n    {\n        \"Question\": \"Who said: \\\"The hardest thing in football is answering the UEFA and FIFA journalist's question\\\"?\",\n        \"A\": \"Ansu Fati\",\n        \"B\": \"CR7\",\n        \"C\": \"Sadio Mane\",\n        \"D\": \"Leo Messi\",\n        \"CorrectAnswer\": \"D\"\n    }, {\n        \"Question\": \"Which is a synonym of \\\"impossible\\\"?\",\n        \"A\": \"Extreme\",\n        \"B\": \"Insurmountable\",\n        \"C\": \"Feasible\",\n        \"D\": \"Easy-go\",\n        \"CorrectAnswer\": \"B\"\n    },{\n        \"Question\": \"Which word/pharse is used to imply \\\"is used to doing\\\"?\",\n        \"A\": \"Will\",\n        \"B\": \"Could\",\n        \"C\": \"Would\",\n        \"D\": \"Did\",\n        \"CorrectAnswer\": \"C\"\n    },{\n        \"Question\": \"What is the main nuance between 'will' and 'going to'?\",\n        \"A\": \"Probability\",\n        \"B\": \"Expectations\",\n        \"C\": \"No difference\",\n        \"D\": \"Format\",\n        \"CorrectAnswer\": \"A\"\n    },{\n        \"Question\": \"What is the past tense of \\\"go\\\"\",\n        \"A\": \"Going\",\n        \"B\": \"Gone\",\n        \"C\": \"Goed\",\n        \"D\": \"Went\",\n        \"CorrectAnswer\": \"D\"\n    }\n]",
                    replyMarkup: replyKeyboardMarkup2,
                    cancellationToken: cancellationToken);
            }
            else if (messageText == "Statistics")
            {
                pdfDownload.GetMethod(correctAnswer);

                await using Stream stream = System.IO.File.OpenRead("/Users/otabek_coding/Desktop/Najot Ta'lim/Quizbot/pdfs/hello-from-otabek.pdf");
                await botClient.SendDocumentAsync(
                    chatId: update.Message.Chat.Id,
                    document: InputFile.FromStream(stream: stream, fileName: $"All_users2.pdf"),
                    caption: "Statistics of Answers"
                    );
                stream.Dispose();
                correctAnswer = 0;
            }
            else if (update.Message.Text.ToLower() == "create")
            {
                Status = 1; // Set the status to 1 to initiate the question creation process
                await SendMessage(botClient, update, cancellationToken, "Please enter the question:", 2);
            }
            else if (Status != 0)
            {
                var handler1 = Status switch
                {
                    2 => await ReceieveQuestion(botClient, update, cancellationToken, list, 3, "Please enter option A:"),
                    4 => await ReceieveQuestion(botClient, update, cancellationToken, list, 5, "Please enter option B:"),
                    6 => await ReceieveQuestion(botClient, update, cancellationToken, list, 7, "Please enter option C:"),
                    8 => await ReceieveQuestion(botClient, update, cancellationToken, list, 9, "Please enter option D:"),
                    10 => await ReceieveQuestion(botClient, update, cancellationToken, list, 11, "Please enter the correct answer (A, B, C, D):"),
                    12 => await SendMessage(botClient, update, cancellationToken, "Successfully created the question!", 13),
                    15 => Status = 0, // Reset the status to 0 after completing the question creation process
                    _ => await SendMessage(botClient, update, cancellationToken, "Enter the index of the question you want to make change on:!!!", 16)
                };
            }
            else if (update.Message.Text.ToLower() == "update")
            {
                // Assuming you have a method for updating questions
                await UpdateQuestionAsync(botClient, update, cancellationToken);
            }
            else if (update.Message.Text.ToLower() == "delete")
            {
                // Assuming you have a method for deleting questions
                await DeleteQuestionAsync(botClient, update, cancellationToken);
            }
        }

        async Task HandleDeleteAsync(Update update, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            var chatId = update.Message.Chat.Id;
            
            var tests = Testlar.GetTest();

            StringBuilder messageText = new StringBuilder("Available tests:\n");
            for (int i = 0; i < tests.Count; i++)
            {
                messageText.AppendLine($"{i + 1}. {tests[i].Question}");
            }
            
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: messageText.ToString(),
                cancellationToken: cancellationToken
            );
            
            await SendMessage(botClient, update, cancellationToken, "Enter the index of the test you want to delete:", 100);
            
            if (Status == 100)
            {
                // Prompt the user to enter the index of the test they want to delete
                await SendMessage(botClient, update, cancellationToken, "Enter the index of the test you want to delete:", 101);
            }

            if (Status == 101)
            {
                if (int.TryParse(update.Message.Text, out int deleteIndex))
                {
                    Delete(deleteIndex - 1);
                    
                    await SendMessage(botClient, update, cancellationToken, "Test deleted successfully!", 0);
                }
                else
                {
                    await SendMessage(botClient, update, cancellationToken, "Invalid input. Please enter a valid index.", 0);
                }
            }
        }
        

        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }

    private static async Task GetPdfAllList(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Welcome! Choose an option:",
            cancellationToken: cancellationToken);
    }

    private static async Task SendStartMessageAsync(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        {
            new KeyboardButton[] { new KeyboardButton("Admin"), new KeyboardButton("User") }
        });
        
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Welcome! Choose an option:",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    private static async Task AdminRegisterationMessageAsync(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        {
            new KeyboardButton[] { new KeyboardButton("Share contact"), new KeyboardButton("start") }
        });
        
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Choose an option:",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    private static async Task UserMessageAsync(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        {
            new KeyboardButton[] { new KeyboardButton("General"), new KeyboardButton("Biology") },
            new KeyboardButton[] { new KeyboardButton("Statistics"), new KeyboardButton("Start") }
        });
        
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Choose an option:",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    private static async Task CRUDMessageAsync(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        {
            new KeyboardButton[] { new KeyboardButton("Create"), new KeyboardButton("Update") },
            new KeyboardButton[] { new KeyboardButton("Delete"), new KeyboardButton("Start") }
        });

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Choose an option:",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
    
    private async static Task<int> ReceieveQuestion(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, List<string> list, int i, string text)
    {
        await Console.Out.WriteLineAsync(update.Message.Text);
        if (update.Message.Text != null)
        {
            list.Add(update.Message.Text);
        }
        Status = i;
        await SendMessage(botClient, update, cancellationToken, text, i+1);
        return i;
    }

    private async static Task<int> SendMessage(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string v, int status)
    {
        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: v,
            cancellationToken: cancellationToken);
        Status = status;
        return status;
    }

    private static async Task UpdateQuestionAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Prompt the user to enter the index of the question they want to update
        await SendMessage(botClient, update, cancellationToken, "Enter the index of the question you want to update:", 101);
    }

    private static async Task UpdateQuestionDetailsAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await SendMessage(botClient, update, cancellationToken, "Question updated successfully!", 0);
    }

    private static async Task DeleteQuestionAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Prompt the user to enter the index of the question they want to delete
        await SendMessage(botClient, update, cancellationToken, "Enter the index of the question you want to delete:", 101);
        
    }
    
    public static void Delete(int index)
    {
        try
        {
            string filePath = "/Users/otabek_coding/Desktop/Najot Ta'lim/Quizbot/Testlar.json";

            string stringTest = File.ReadAllText(filePath);
            List<Test> tests = JsonSerializer.Deserialize<List<Test>>(stringTest) ?? new List<Test>();

            if (index >= 0 && index < tests.Count)
            {
                tests.RemoveAt(index);

                string json1 = JsonSerializer.Serialize(tests);
                File.WriteAllText(filePath, json1);

                Console.WriteLine("Quiz removed successfully!");
            }
            else
            {
                Console.WriteLine("Invalid index. No quiz removed.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // private static async Task<int> GetPdfAllList(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string v1, int v2)
    // {
    //     var pdfFilePath = Testlar.GetTest(); 
    //
    //     // Faylni o'qish uchun FileStream yaratish
    //     using (FileStream fileStream = new FileStream(pdfFilePath, FileMode.Open, FileAccess.Read))
    //     {
    //         // MemoryStream yaratish va PDF faylini uni ichiga yozish
    //         using (MemoryStream memoryStream = new MemoryStream())
    //         {
    //             fileStream.CopyTo(memoryStream);
    //
    //
    //         
    //
    //             await botClient.SendDocumentAsync(
    //                 chatId: update.Message.Chat.Id,
    //                 document: InputFile.FromStream(stream: memoryStream, fileName: $"All_users.pdf"),
    //                 caption: v1
    //             );
    //         }
    //     }
    //
    //     update_int = v2;
    //     return v2;
    //
    // }


    private static async Task HandleCallBackQueryAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var callBack = update.CallbackQuery;

        var tests = Testlar.GetTest();
        var tests1 = Testlar.GetBiologiya();
        
        var test = tests[TestNumber];

        await CheckAnswerAsync(test,botClient, callBack, cancellationToken);
        TestNumber++;
        try
        {
            var nextTest = tests[TestNumber];
            await SendNextQuestion(nextTest,botClient, update, cancellationToken);
        }
        catch (ArgumentOutOfRangeException e)
        {
            await botClient.SendTextMessageAsync(
                chatId: callBack.Message.Chat.Id,
                text: "This was the last question",
                cancellationToken: cancellationToken);
        }

    }
    
    private static async Task SendNextQuestion(Test nextTest, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {

        InlineKeyboardMarkup inlineKeyboard = new(new[]
        {
    new []
    {
        InlineKeyboardButton.WithCallbackData(text: $"{nextTest.A}", callbackData: "A"),
        
       
    }, new []
    {
        InlineKeyboardButton.WithCallbackData(text: $"{nextTest.B}", callbackData: "B"),

    }, new []
    {
        InlineKeyboardButton.WithCallbackData(text: $"{nextTest.C}", callbackData: "C"),

    }, new []
    {
        InlineKeyboardButton.WithCallbackData(text: $"{nextTest.D}", callbackData: "D"),

    }
  
});
        if (update.Message is null)
        {
            await botClient.SendTextMessageAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                text: $"{nextTest.Question}",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken
                );
        }
        else
        {
            await botClient.SendTextMessageAsync(
               chatId: update.Message.Chat.Id,
               text: $"{nextTest.Question}",
               replyMarkup: inlineKeyboard,
               cancellationToken: cancellationToken
               );
        }
    }

    private static async Task CheckAnswerAsync(Test test, ITelegramBotClient botClient, CallbackQuery? callBack, CancellationToken cancellationToken)
    {
        if (callBack.Data == test.CorrectAnswer)
        {
            correctAnswer++;
            await botClient.SendTextMessageAsync(
                 chatId: callBack.From.Id,
                text: $"Answer is correct. {correctAnswer}",
                cancellationToken: cancellationToken
                );
        }
        else 
        {
            await botClient.SendTextMessageAsync(
               chatId: callBack.From.Id,
              text: $"Incorrect Answer ",
              cancellationToken: cancellationToken
              );
        }
    }

    private static async Task HandleMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return;
        if (message.Text is not { } messageText)
            return;

        if (messageText == "General")
        {
            TestNumber = 0;
            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"Assalomu aleykum {update.Message.From.FirstName}",
                cancellationToken: cancellationToken
            );
            var tests = Testlar.GetTest();
            await SendNextQuestion(tests[0], botClient, update, cancellationToken);
        }
    
        if (messageText == "Biology")
        {
            TestNumber = 0;
            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"Assalomu aleykum {update.Message.From.FirstName}",
                cancellationToken: cancellationToken
            );
            var tests2 = Testlar.GetBiologiya();
            await SendNextQuestion(tests2[0], botClient, update, cancellationToken);
        }
    }
}
