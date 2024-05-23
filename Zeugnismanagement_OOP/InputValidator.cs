using System;
using System.Text.RegularExpressions;

namespace Zeugnismanagement_OOP
{
    public static class InputValidator
        {
            public static string GetValidName()
            {
                Regex rgxName = new Regex("^[A-Z a-z]+$");
                string name;
                do
                {
                    Console.WriteLine("Geben Sie den Namen des Schülers an: ");
                    name = Console.ReadLine();
                    if (!rgxName.IsMatch(name))
                    {
                        Console.WriteLine(
                            "Hinweis: Das Namensfeld darf nur Buchstaben der Form [a-z] u. [A-Z] enthalten, Sonderzeichen und Kommas sind nicht zulässig!");
                    }
                } while (!rgxName.IsMatch(name));

                return name;
            }

            public static string GetValidDate()
            {
                Regex regexDatum = new Regex(@"^(3[01]|[12][0-9]|0?[1-9])\.(1[012]|0?[1-9])\.((?:19|20)\d{2})$");
                string datum;
                do
                {
                    Console.WriteLine("Geben Sie das Ausstellungsdatum ein (Leer lassen, für heutiges Datum): ");
                    datum = Console.ReadLine();
                    if (string.IsNullOrEmpty(datum))
                    {
                        datum = DateTime.Now.ToString("dd.MM.yyyy");
                    }

                    if (!regexDatum.IsMatch(datum))
                    {
                        Console.WriteLine("Sie müssen ein korrektes Datumsformat tt.mm.jjjj angeben!");
                    }
                } while (!regexDatum.IsMatch(datum));

                return datum;
            }

            public static Fach[] GetSubjectsWithGrades()
            {
                string[] faecherNamen =
                    {"Mathe", "Deutsch", "Englisch", "Biologie", "Sport", "Kunst", "Geschichte", "Informatik"};
                Fach[] faecher = new Fach[faecherNamen.Length];

                for (int i = 0; i < faecherNamen.Length; i++)
                {
                    faecher[i] = new Fach {Name = faecherNamen[i]};
                }

                int leistungskursIndex1, leistungskursIndex2;
                do
                {
                    Console.WriteLine("Angebotene Schulfächer:");
                    for (int i = 0; i < faecherNamen.Length; i++)
                    {
                        Console.WriteLine($"{i + 1} - {faecherNamen[i]}");
                    }

                    leistungskursIndex1 = GetValidInt("Geben Sie Ihren 1. belegten Leistungskurs an: ") - 1;
                    leistungskursIndex2 = GetValidInt("Geben Sie Ihren 2. belegten Leistungskurs an: ") - 1;

                    if (leistungskursIndex1 == leistungskursIndex2 || leistungskursIndex1 < 0 ||
                        leistungskursIndex1 >= faecherNamen.Length || leistungskursIndex2 < 0 ||
                        leistungskursIndex2 >= faecherNamen.Length)
                    {
                        Console.WriteLine("Sie müssen zwei unterschiedliche und gültige Leistungskurse wählen!");
                    }
                    else
                    {
                        faecher[leistungskursIndex1].IsAdvancedCourse = true;
                        faecher[leistungskursIndex2].IsAdvancedCourse = true;
                    }
                } while (leistungskursIndex1 == leistungskursIndex2 || leistungskursIndex1 < 0 ||
                         leistungskursIndex1 >= faecherNamen.Length || leistungskursIndex2 < 0 ||
                         leistungskursIndex2 >= faecherNamen.Length);

                for (int i = 0; i < faecher.Length; i++)
                {
                    faecher[i].Grade = GetValidInt($"Geben Sie Ihre Notenpunkte für das Fach {faecher[i].Name} an: ",
                        0, 15);
                }

                return faecher;
            }

            public static int GetValidInt(string message, int min = int.MinValue, int max = int.MaxValue)
            {
                int value;
                do
                {
                    Console.WriteLine(message);
                    if (int.TryParse(Console.ReadLine(), out value) && value >= min && value <= max)
                    {
                        return value;
                    }
                    else
                    {
                        Console.WriteLine($"Bitte geben Sie eine gültige Zahl zwischen {min} und {max} ein.");
                    }
                } while (true);
            }

            public static bool GetValidYesNo(string message)
            {
                string response;
                do
                {
                    Console.WriteLine(message);
                    response = Console.ReadLine().ToLower();
                    if (response == "ja" || response == "nein")
                    {
                        return response == "ja";
                    }
                    else
                    {
                        Console.WriteLine("Bitte geben Sie 'ja' oder 'nein' ein!");
                    }
                } while (true);
            }
        }
}