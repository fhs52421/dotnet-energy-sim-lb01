using System;
using System.Threading.Tasks;

namespace PvSimulation
{ 
    public class SimulationEngine
    {
        // Abhängigkeiten
        private PvAnlage _anlage;
        private CsvLogger _logger;
        private ConsoleView _view;

        // Simulationszeitraum 
        private DateTime _simulierteZeit = new DateTime(2026, 7, 15, 0, 0, 0);
        private DateTime _endZeit = new DateTime(2026, 7, 16, 0, 0, 0);

        // Standardwerte für die konfigurierbaren Parameter
        private int _schrittweiteMin = 15;
        private int _tempoMs = 1000;
        private bool _autoModus = true;
        private bool _pausiert = false;

        // Konstruktor: Nimmt benötigten Objekte entgegen
        public SimulationEngine(PvAnlage anlage, CsvLogger logger, ConsoleView view)
        {
            _anlage = anlage;
            _logger = logger;
            _view = view;
        }

        // Hauptschleife
        public async Task RunAsync()
        {
            Console.Clear(); // Konsole einmal initial leeren

            // 24 Stunden Laufzeit
            while (_simulierteZeit <= _endZeit)
            {
                // Prüft ob taste gedrückt
                if (Console.KeyAvailable)
                {
                    HandleInput(Console.ReadKey(true).Key);
                }

                // Wenn nicht pausiert ist, wird die Zeit vorangetrieben
                if (!_pausiert)
                {
                    _anlage.Update(_simulierteZeit, _schrittweiteMin, _autoModus);
                    _logger.Log(_simulierteZeit, _anlage);
                    _simulierteZeit = _simulierteZeit.AddMinutes(_schrittweiteMin);
                }

                // UI in jedem Durchlauf aktualisieren
                _view.Zeichne(_simulierteZeit, _anlage, _schrittweiteMin, _tempoMs, _autoModus, _pausiert);
                await Task.Delay(_tempoMs);
            }
        }

        // Verarbeitet die manuellen Eingriffe des Benutzers
        private void HandleInput(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.P:
                    _pausiert = !_pausiert;
                    break;
                case ConsoleKey.E:
                    // Anlage ein- oder ausschalten
                    _anlage.Zustand = _anlage.Zustand == Betriebszustand.EIN ? Betriebszustand.AUS : Betriebszustand.EIN;
                    break;
                case ConsoleKey.F:
                    _anlage.ToggleFehler(); // Simuliert einen Defekt
                    break;
                case ConsoleKey.A:
                    _autoModus = !_autoModus; // Aktiviert/Deaktiviert automatisches Wetter
                    break;
                case ConsoleKey.W:
                    // Manuelle Wetteränderung funktioniert nur, wenn die Automatik aus ist
                    if (!_autoModus) _anlage.WechsleWetterManuell();
                    break;
                case ConsoleKey.T:
                    // Wechselt zwischen zwei Geschwindigkeiten (500ms und 100ms pro Tick)
                    _tempoMs = _tempoMs == 500 ? 100 : 500;
                    break;
                case ConsoleKey.S:
                    // Wechselt den Detailgrad der Berechnung (15 Minuten vs 60 Minuten Sprünge)
                    _schrittweiteMin = _schrittweiteMin == 15 ? 60 : 15;
                    break;
            }
        }
    }
}
