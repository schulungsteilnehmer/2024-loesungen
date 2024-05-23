using System;
using System.IO;

namespace Zeugnismanagement_OOP
{
    public class ReportCard
    {
        private Student _student;

            public ReportCard(Student student)
            {
                _student = student;
            }

            public void PrintToConsole()
            {
                Console.WriteLine("===============Zeugnis===============");
                Console.WriteLine($"Name: {_student.Name}");
                Console.WriteLine($"Datum: {_student.Date}");
                Console.WriteLine("=====================================");
                Console.WriteLine();
                foreach (var subject in _student.Faecher)
                {
                    string subjectName = subject.IsAdvancedCourse ? $"{subject.Name} (LK)" : subject.Name;
                    Console.WriteLine($"{subjectName, -15}: {subject.Grade,4}");
                }

                Console.WriteLine();
                Console.WriteLine($"Durchschnittsnote: {CalculateAverage():F1}");
                Console.WriteLine("=====================================");
                Console.WriteLine($"Fehltage: {_student.AbsenceDays}");
                Console.WriteLine($"Davon Unentschuldigt: {_student.UnexcusedAbsenceDays}");
                Console.WriteLine();
                if (IsStudentPromoted())
                {
                    Console.WriteLine("Der Schüler wird versetzt.");
                }
                else
                {
                    Console.WriteLine("Der Schüler wird nicht versetzt.");
                }
            }

            public void PrintToFile()
            {
                using (StreamWriter writer = new StreamWriter("Zeugnis.txt"))
                {
                    writer.WriteLine("===============Zeugnis===============");
                    writer.WriteLine($"Name: {_student.Name}");
                    writer.WriteLine($"Datum: {_student.Date}");
                    writer.WriteLine("=====================================");
                    writer.WriteLine();
                    foreach (var subject in _student.Faecher)
                    {
                        string subjectName = subject.IsAdvancedCourse ? $"{subject.Name} (LK)" : subject.Name;
                        Console.WriteLine($"{subjectName, -15}: {subject.Grade,4}");
                    }

                    writer.WriteLine();
                    writer.WriteLine($"Durchschnittsnote: {CalculateAverage():F1}");
                    writer.WriteLine("=====================================");
                    writer.WriteLine($"Fehltage: {_student.AbsenceDays}");
                    writer.WriteLine($"Davon Unentschuldigt: {_student.UnexcusedAbsenceDays}");
                    writer.WriteLine();
                    if (IsStudentPromoted())
                    {
                        writer.WriteLine("Der Schüler wird versetzt.");
                    }
                    else
                    {
                        writer.WriteLine("Der Schüler wird nicht versetzt.");
                    }
                }
            }

            private double CalculateAverage()
            {
                double gesamtPunktzahl = 0;
                int anzahlFaecher = _student.Faecher.Length;
                foreach (var fach in _student.Faecher)
                {
                    gesamtPunktzahl += fach.Grade * (fach.IsAdvancedCourse ? 2 : 1);
                }

                double averagePoints = gesamtPunktzahl / (anzahlFaecher + 2);
                return Math.Round((17 - averagePoints) / 3, 1);
            }

            private bool IsStudentPromoted()
            {
                int unterKurse = 0;
                foreach (var subject in _student.Faecher)
                {
                    if (subject.Grade < 5)
                    {
                        unterKurse++;
                    }
                }
                return _student.UnexcusedAbsenceDays < 30 && unterKurse <= 2;
            }
        }
}