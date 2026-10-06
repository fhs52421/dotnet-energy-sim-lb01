using System;

namespace PvSimulation
{
    public class PvAnlage
    {
        public double NennleistungKw { get; private set; }

        // Aktuelle Zustände der Anlage mit Standardwerten
        public Betriebszustand Zustand { get; set; } = Betriebszustand.EIN;
        public WechselrichterStatus InverterStatus { get; set; } = WechselrichterStatus.OK;
        public Wetter AktuellesWetter { get; set; } = Wetter.Nacht;

        // Berechnete Ausgabewerte für die Anzeige und das Logging
        public double AktuelleLeistungKw { get; private set; }
        public double TagesenergieKwh { get; private set; }
        public double Einstrahlung { get; private set; }

        // Konstruktor: Setzt die Basis-Kapazität der Anlage
        public PvAnlage(double nennleistungKw)
        {
            NennleistungKw = nennleistungKw;
        }

        // Diese Methode wird in jedem Simulationsschritt aufgerufen, um das Modell zu aktualisieren
        public void Update(DateTime zeit, int schrittweiteMin, bool autoModus)
        {
            int stunde = zeit.Hour;

            // Wenn Anlage manuell ausgeschaltet wurde oder ein Fehler vorliegt, wird sofort abgebrochen und die Leistung auf 0
            if (Zustand == Betriebszustand.AUS || InverterStatus == WechselrichterStatus.Fehler)
            {
                AktuelleLeistungKw = 0;
                Einstrahlung = 0;
                return;
            }

            // In der Nacht erzeugt eine PV-Anlage keine Energie
            if (stunde < 6 || stunde > 20)
            {
                AktuellesWetter = Wetter.Nacht;
                Einstrahlung = 0;
                AktuelleLeistungKw = 0;
                return;
            }

            // Im Auto-Modus variieren die Einflussgrößen (hier das Wetter)
            if (autoModus)
            {
                Random rnd = new Random();
                AktuellesWetter = (Wetter)rnd.Next(0, 3); // 0=Sonnig, 1=Bewoelkt, 2=Regen
            }

            // Basis-Einstrahlung berechnen: Peak ist um 13 Uhr (1000 W/m^2)
            // Je weiter die Uhrzeit von 13 Uhr entfernt ist, desto geringer die Einstrahlung
            double basis = 1000 - Math.Abs(13 - stunde) * 100;
            if (basis < 0) basis = 0;

            // Das aktuelle Wetter reduziert die maximal mögliche Sonneneinstrahlung
            double faktor = AktuellesWetter switch
            {
                Wetter.Sonnig => 1.0,
                Wetter.Bewoelkt => 0.5,   
                Wetter.Regen => 0.2,      
                _ => 0
            };

            Einstrahlung = basis * faktor;

            // Die tatsächliche Leistung ergibt sich aus der Einstrahlung im Verhältnis zur Nennleistung
            AktuelleLeistungKw = (Einstrahlung / 1000.0) * NennleistungKw;

            // Die erzeugte Energie wird über die Zeit aufsummiert (Leistung * Zeit in Stunden)
            TagesenergieKwh += AktuelleLeistungKw * (schrittweiteMin / 60.0);
        }

        // Hilfsmethode für den manuellen Eingriff
        public void ToggleFehler()
        {
            InverterStatus = InverterStatus == WechselrichterStatus.OK ? WechselrichterStatus.Fehler : WechselrichterStatus.OK;
        }

        // Hilfsmethode für den manuellen Eingriff: Ändert das Wetter zyklisch
        public void WechsleWetterManuell()
        {
            if (AktuellesWetter == Wetter.Nacht) return;
            int naechstes = ((int)AktuellesWetter + 1) % 3;
            AktuellesWetter = (Wetter)naechstes;
        }
    }
}