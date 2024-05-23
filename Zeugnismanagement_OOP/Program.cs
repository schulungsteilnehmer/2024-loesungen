using System;

namespace Zeugnismanagement_OOP
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Zeugnismanagement-Programm 'RaaPrint'");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine();
            Console.WriteLine("+ Prozessschritt: Relevante Daten einlesen +");
            Console.WriteLine();

            Student student = new Student();
            student.Name = InputValidator.GetValidName();
            student.Date = InputValidator.GetValidDate();
            student.Faecher = InputValidator.GetSubjectsWithGrades();

            student.AbsenceDays = InputValidator.GetValidInt("Wieviele Fehltage?");
            student.ExcusedAbsenceDays = InputValidator.GetValidInt("Davon Entschuldigt?");
            
            bool saveToFile = InputValidator.GetValidYesNo("Soll das Zeugnis in die Datei geschrieben werden? (ja|nein)");

            ReportCard reportCard = new ReportCard(student);
            if (saveToFile)
            {
                reportCard.PrintToFile();
            }
            Console.WriteLine();
            reportCard.PrintToConsole();

            Console.Write("Drücke eine beliebige Taste . . . ");
            Console.ReadKey(true);
        }
    }
}