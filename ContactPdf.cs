using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Infrastructure;
using System.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Helpers;
using static QuestPDF.Helpers.Colors;
using Telegram.Bot.Types;

namespace Quizbot
{
    public class ContactPdf
    {
        List<string> list = new List<string>();
        
        public void GetMethod(string number)
        {
            list.Add(number);
            
            DirectoryInfo projectDirectoryInfo =
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent;

            Console.WriteLine(projectDirectoryInfo.FullName);

            string pdfsFolder = Directory.CreateDirectory(
                 Path.Combine(projectDirectoryInfo.FullName, "contact")).FullName;

            QuestPDF.Settings.License = LicenseType.Community;
            QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header()
                      .Text("Hello PDF!")
                      .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                    page.Content()
                      .PaddingVertical(1, Unit.Centimetre)
                      .Column(x =>
                      {
                          foreach (var item in list)
                          {
                              x.Spacing(20);
                              x.Item().Text($"User phone number {item} ");
                          }
                      });

                    page.Footer()
                      .AlignCenter()
                      .Text(x =>
                      {
                          x.Span("Page ");
                          x.CurrentPageNumber();
                      });
                });
            })
            .GeneratePdf(Path.Combine(pdfsFolder, "hello-from-contact.pdf"));
        }
    }
}