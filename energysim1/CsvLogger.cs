using System;
using System.IO;

namespace PvSimulation
{
    public class CsvLogger
    {
        private readonly string _dateipfad;

        public CsvLogger(string dateipfad)
        {
            _dateipfad = dateipfad;
            // Erstellt die Datei oder überschreibt sie und schreibt die Spaltenüberschriften
            File.WriteAllText(_dateipfad, "Zeit,Leistung_kW,Energie_kWh,Einstrahlung,Wetter,Inverter\n");
        }

        // Engine Aufruf, um den aktuellen Momentaufnahmewert anzuhängen
        public void Log(DateTime zeit, PvAnlage anlage)
        {
            // F2 formatiert auf zwei Nachkommastellen, F0 auf ganze Zahlen
            string line = $"{zeit:yyyy-MM-dd HH:mm},{anlage.AktuelleLeistungKw:F2},{anlage.TagesenergieKwh:F2},{anlage.Einstrahlung:F0},{anlage.AktuellesWetter},{anlage.InverterStatus}\n";
            File.AppendAllText(_dateipfad, line);
        }
    }
}
