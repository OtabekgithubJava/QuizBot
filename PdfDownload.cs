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

namespace Quizbot
{
    public class PdfDownload
    {
        List<int> list = new List<int>();
        public void GetMethod(int num)
        {
            list.Add(num);

            DirectoryInfo projectDirectoryInfo =
          Directory.GetParent(Environment.CurrentDirectory).Parent.Parent;

            Console.WriteLine(projectDirectoryInfo.FullName);

            string pdfsFolder = Directory.CreateDirectory(
                 Path.Combine(projectDirectoryInfo.FullName, "pdfs")).FullName;

            QuestPDF.Settings.License = LicenseType.Community;
            // code in my main method
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
                              x.Item().Text($"Otabek {item} ");
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
            .GeneratePdf(Path.Combine(pdfsFolder, "hello-from-otabek.pdf"));
        }
        internal void GetMethod(string phoneNumber)
        {
            throw new NotImplementedException();
        }
    }
}