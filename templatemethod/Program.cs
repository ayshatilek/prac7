using System;

namespace ReportTemplate
{
    public abstract class ReportGenerator
    {
        public void GenerateReport()
        {
            Log("Начало генерации");
            CreateHeader();
            FormatData();
            CreateBody();
            if (CustomerWantsSave())
                SaveReport();
            if (CustomerWantsEmail())
                SendByEmail();
            Log("Отчет создан\n");
        }

        protected abstract void CreateHeader();
        protected abstract void FormatData();
        protected abstract void CreateBody();

        protected virtual void SaveReport()
        {
            Console.WriteLine("Сохранение отчета...");
        }

        protected virtual bool CustomerWantsSave()
        {
            Console.Write("Сохранить отчет? (y/n): ");
            string input = Console.ReadLine()?.ToLower();
            return input == "y";
        }

        protected virtual bool CustomerWantsEmail()
        {
            Console.Write("Отправить по email? (y/n): ");
            string input = Console.ReadLine()?.ToLower();
            return input == "y";
        }

        protected virtual void SendByEmail()
        {
            Console.WriteLine("Отчет отправлен по email");
        }

        protected void Log(string message)
        {
            Console.WriteLine("[LOG] " + message);
        }
    }

    public class PdfReport : ReportGenerator
    {
        protected override void CreateHeader() => Console.WriteLine("PDF: Создание заголовка");
        protected override void FormatData() => Console.WriteLine("PDF: Форматирование данных");
        protected override void CreateBody() => Console.WriteLine("PDF: Создание тела");
    }

    public class ExcelReport : ReportGenerator
    {
        protected override void CreateHeader() => Console.WriteLine("Excel: Создание таблицы");
        protected override void FormatData() => Console.WriteLine("Excel: Форматирование ячеек");
        protected override void CreateBody() => Console.WriteLine("Excel: Заполнение листа");

        protected override void SaveReport()
        {
            Console.WriteLine("Excel файл сохранен корректно");
        }
    }

    public class HtmlReport : ReportGenerator
    {
        protected override void CreateHeader() => Console.WriteLine("HTML: <head>");
        protected override void FormatData() => Console.WriteLine("HTML: Форматирование CSS");
        protected override void CreateBody() => Console.WriteLine("HTML: <body>");
    }

    public class CsvReport : ReportGenerator
    {
        protected override void CreateHeader() => Console.WriteLine("CSV: Заголовки столбцов");
        protected override void FormatData() => Console.WriteLine("CSV: Разделение через запятую");
        protected override void CreateBody() => Console.WriteLine("CSV: Запись строк");
    }

    class Program
    {
        static void Main()
        {
            ReportGenerator report = new ExcelReport();
            report.GenerateReport();

            report = new PdfReport();
            report.GenerateReport();
        }
    }
}
