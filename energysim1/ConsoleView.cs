using System;

namespace PvSimulation
{
    public class ConsoleView
    {
        public void Zeichne(DateTime zeit, PvAnlage anlage, int schrittweite, int tempo, bool autoModus, bool pausiert)
        {
            // Setzt den Cursor nach oben links, anstatt Console.Clear() zu rufen
            // Das verhindert das störende Flackern bei schnellen Aktualisierungen
            Console.SetCursorPosition(0, 0);

            // PadRight füllt die Zeile mit Leerzeichen auf. Das überschreibt alte Zeichen aus dem vorherigen Frame
            Console.WriteLine($"=== PV-Anlage | {(autoModus ? "AUTO   " : "MANUELL")} ===".PadRight(80));
            Console.WriteLine($"Simulationszeit: {zeit:yyyy-MM-dd HH:mm} | Schritt: {schrittweite} min | Tempo: {tempo} ms".PadRight(80));
            Console.WriteLine("".PadRight(80));

            // Drei passenden Zustandswerte des Geräts anzeigen
            Console.WriteLine($"Betriebszustand: {anlage.Zustand,-10} | Wetter: {anlage.AktuellesWetter,-10}".PadRight(80));

            // Logik für den grafischen Balken
            double prozent = anlage.NennleistungKw > 0 ? (anlage.AktuelleLeistungKw / anlage.NennleistungKw) : 0;
            int balkenLaenge = (int)(prozent * 20);

            // Erzeugt einen String aus Rauten '#' und füllt den Rest mit Bindestrichen '-' auf
            string balken = new string('#', Math.Max(0, balkenLaenge)).PadRight(20, '-');

            Console.WriteLine($"Leistung:       {anlage.AktuelleLeistungKw,5:0.0} kW  [{balken}] {(prozent * 100),3:0}%".PadRight(80));
            Console.WriteLine($"Tagesenergie:   {anlage.TagesenergieKwh,5:0.0} kWh".PadRight(80));
            Console.WriteLine($"Einstrahlung:   {anlage.Einstrahlung,5:0} W/m^2".PadRight(80));
            Console.WriteLine($"Wechselrichter: {anlage.InverterStatus,-10}".PadRight(80));
            Console.WriteLine("".PadRight(80));

            // Menü für die Steuerung der Anlage
            Console.WriteLine("Steuerung: [P] Pause | [E] Ein/Aus | [F] Fehler | [A] Auto-Modus".PadRight(80));
            Console.WriteLine("Parameter: [T] Tempo | [S] Schrittweite | [W] Wetter (nur manuell)".PadRight(80));

            // Anzeigen, wenn pausiert wurde
            if (pausiert)
                Console.WriteLine("\n*** SIMULATION PAUSIERT ***".PadRight(80));
            else
                Console.WriteLine("\n                           ".PadRight(80)); // Platzhalter überschreiben
        }
    }
}
