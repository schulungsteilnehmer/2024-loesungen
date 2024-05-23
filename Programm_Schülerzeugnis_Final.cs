using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ZeugnisStufe2
{
	class Program
	{
		
		public static void Main(string[] args){
			
			Console.WriteLine("Zeugnismanagement-Programm 'RaaPrint'");
			Console.WriteLine("-----------------------------------");
			Console.WriteLine();
			Console.WriteLine("+ Prozessschritt: Relevante Daten einlesen +");
			Console.WriteLine();
			
			string[] faecher = {"Mathe", "Deutsch", "Englisch", "Biologie", "Sport", "Kunst", "Geschichte", "Informatik"};
			int[] noten = new int[faecher.Length];
			
			//Möglichkeit (Regex) um ungültige Benutzereingaben abzufangen. (Mächtige Methode, aber komplex & schwer verständlich)
			Regex rgxName = new Regex("^[A-Z a-z]+$");
			String name;
			bool validName = true;
			do
			{
				Console.WriteLine("Geben Sie den Namen des Schülers an: ");
				name = Console.ReadLine();
				validName = rgxName.IsMatch(name);
				if (!validName)
				{
					Console.WriteLine();
					Console.WriteLine("Hinweis: Das Namensfeld darf nur Buchstaben der Form [a-z] u. [A-Z] enthalten,");
					Console.WriteLine("Sonderzeichen und Kommas sind nicht zulässig!");
					Console.WriteLine();
				}
			} while (!validName);
			Console.WriteLine();

			//Möglichkeit (Regex) um ungültige Benutzereingaben abzufangen. (Mächtige Methode, aber komplex & schwer verständlich)
			Regex regexDatum = new Regex(@"^(3[01]|[12][0-9]|0?[1-9])\.(1[012]|0?[1-9])\.((?:19|20)\d{2})$");
			String datum = "";
			bool validDatum = true;
			do
			{
				Console.WriteLine("Geben Sie das Ausstellungsdatum ein (Leer lassen, für heutiges Datum): ");
				datum = Console.ReadLine();
				if (datum.Length == 0)
				{
					datum = DateTime.Now.Date.ToString("dd.MM.yyyy");
				}

				validDatum = regexDatum.IsMatch(datum);
				if (!validDatum)
				{
					Console.WriteLine("Sie müssen ein korrektes Datumsformat tt.mm.jjjj angeben!");
					Console.WriteLine();
				}
			} while (!validDatum);
			Console.WriteLine();

			bool validLeistungskurse = false;
			int leistungskursIndex1 = 0;
			int leistungskursIndex2 = 0;
			do
			{	
				Console.WriteLine("Angebotene Schulfächer:");
				Console.WriteLine("-----------------------");
				
				for (int i = 0; i < faecher.Length; i++)
				{
					Console.WriteLine("{0} - {1}", i + 1, faecher[i]);
				}
				Console.WriteLine();

				// 2. Möglichkeit (try - catch) um ungültige Benutzereingaben abzufangen. (Einfachere Methode, die leichter verständlich ist)
				try
				{
					Console.Write("Geben Sie Ihren 1. belegten Leistungskurs an: ");
					leistungskursIndex1 = Convert.ToInt32(Console.ReadLine());
					Console.Write("Geben Sie Ihren 2. belegten Leistungskurs an: ");
					leistungskursIndex2 = Convert.ToInt32(Console.ReadLine());
					if (leistungskursIndex1 >= 9 || leistungskursIndex1 <= 0 || leistungskursIndex2 >= 9 ||
					    leistungskursIndex2 <= 0)
					{
						validLeistungskurse = false;
					}
					else
					{
						validLeistungskurse = true;
					}

					if (leistungskursIndex1 == leistungskursIndex2)
					{
						Console.WriteLine("Sie müssen zwei unterschiedliche Leistungskurse wählen!");
						validLeistungskurse = false;
					}
				}
				catch (FormatException ex)
				{
					Console.WriteLine("{0} Sie müssen eine Zahl von 1-8 eingeben!", ex.Message);
				}
			} while (!validLeistungskurse);
			Console.WriteLine();

			faecher[leistungskursIndex1 - 1] += "(LK)";
			faecher[leistungskursIndex2 - 1] += "(LK)";

			bool validNotepunkte;
			int notenpunkte;
			for (int i = 0; i < faecher.Length; i++)
			{
				do
				{
					validNotepunkte = false;
					notenpunkte = 0;
					try
					{
						Console.Write("Geben Sie Ihre Notenpunkte für das Fach {0} an: ", faecher[i]);
						notenpunkte = Convert.ToInt32(Console.ReadLine());
						if (notenpunkte <= 15 && notenpunkte >= 0)
						{
							validNotepunkte = true;
						}
						else
						{
							Console.WriteLine("Sie müssen eine Zahl von 0-15 eingeben!");
						}
					}
					catch (FormatException ex)
					{
						Console.WriteLine("{0} Sie müssen eine Zahl von 0-15 eingeben!", ex.Message);
					}
				} while (!validNotepunkte);

				noten[i] = notenpunkte;
			}
			Console.WriteLine();
			
			Console.WriteLine("Wieviele Fehltage?");
			int fehltage = Convert.ToInt32(Console.ReadLine());
			Console.WriteLine("Davon Entschuldigt?");
			int entschuldigteFehltage = Convert.ToInt32(Console.ReadLine());
			
			int unentschuldigteFehltage = fehltage - entschuldigteFehltage;
			
			string antwort;
			bool validAntwort;
			do {
				Console.WriteLine("Soll das Zeugnis in die Datei geschrieben werden? (ja|nein)");
				antwort = Console.ReadLine().ToLower();
				validAntwort = antwort.Equals("ja") || antwort.Equals("nein");
				if(!validAntwort) {
					Console.WriteLine("Bitte geben Sie 'ja' oder 'nein' ein!");
				}
			} while(!validAntwort);
			
			if (antwort.Equals("ja")) {
				printZeugnisInDatei(name, datum, faecher, noten, fehltage, unentschuldigteFehltage, durchschnittBerechnen(noten, leistungskursIndex1, leistungskursIndex2));
			}
			
			printZeugnis(name, datum, faecher, noten, fehltage, unentschuldigteFehltage, durchschnittBerechnen(noten, leistungskursIndex1, leistungskursIndex2));
			Console.Write("Drücke eine beliebige Taste . . . ");
			Console.ReadKey(true);
		}
		
		public static void printZeugnis(String name, String datum, string[] faecher, int[] noten, int fehltage, int unentschuldigteFehltage, double durchschnitt) {
			Console.WriteLine("===============Zeugnis===============");
			Console.WriteLine("Name: {0}", name);
			Console.WriteLine("Datum: {0}", datum);
			Console.WriteLine("=====================================");
			Console.WriteLine();
			for(int i=0; i<8; i++){
				Console.WriteLine("{0, -15}: {1,4}", faecher[i], noten[i]);
			}
			Console.WriteLine();
			Console.WriteLine("Durchschnittsnote: {0:F1}", durchschnitt);
			Console.WriteLine("=====================================");
			Console.WriteLine("Fehltage: {0}", fehltage);
			Console.WriteLine("Davon Unentschuldigt: {0}", unentschuldigteFehltage);
			Console.WriteLine();
			if(wirdVersetzt(noten, unentschuldigteFehltage)) {
				Console.WriteLine("Der Schüler wird versetzt.");
			} else {
				Console.WriteLine("Der Schüler wird nicht versetzt.");
			}
		}
		
		public static void printZeugnisInDatei(String name, String datum, string[] faecher, int[] noten, int fehltage, int unentschuldigteFehltage, double durchschnitt) {
			StreamWriter writer = new StreamWriter("Zeugnis.txt");
			writer.WriteLine("===============Zeugnis===============");
			writer.WriteLine("Name: {0}", name);
			writer.WriteLine("Datum: {0}", datum);
			writer.WriteLine("=====================================");
			writer.WriteLine();
			for(int i=0; i<8; i++){
				writer.WriteLine("{0, -15}: {1,4}", faecher[i], noten[i]);
			}
			writer.WriteLine();
			writer.WriteLine("Durchschnittsnote: {0:F1}", durchschnitt);
			writer.WriteLine("=====================================");
			writer.WriteLine("Fehltage: {0}", fehltage);
			writer.WriteLine("Davon Unentschuldigt: {0}", unentschuldigteFehltage);
			writer.WriteLine();
			if(wirdVersetzt(noten, unentschuldigteFehltage)) {
				writer.WriteLine("Der Schüler wird versetzt.");
			} else {
				writer.WriteLine("Der Schüler wird nicht versetzt.");
			}
			writer.Close();
		}
		
		public static bool wirdVersetzt(int[] noten, int unentschuldigteFehltage)
		{
			int counter = 0;
			for (int i = 0; i < noten.Length; i++) {
				if (noten[i] < 5) {
					counter++;
				}
			}
			
			return unentschuldigteFehltage < 30 && counter <= 2;
		}
		
		public static double durchschnittBerechnen(int[] noten, int leistungskursIndex1, int leistungskursIndex2) {
			double durchschnitt = 0;
			for (int i = 0; i < noten.Length; i++) {
				if (i == leistungskursIndex1 || i == leistungskursIndex2) {
					durchschnitt += noten[i] * 2;
				} else {
					durchschnitt += noten[i];
				}
			}
			durchschnitt /= (noten.Length + 2);
			return Math.Round((17 - durchschnitt) / 3, 1); // Transformation von Notenpunkten in Schulnoten (1-6)
		}
	}
}