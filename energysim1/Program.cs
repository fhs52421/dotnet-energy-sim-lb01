using System;
using System.Threading.Tasks;

namespace PvSimulation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Objekte werden hier erzeugt
            var anlage = new PvAnlage(10.0);
            var logger = new CsvLogger("pv_log.csv");
            var view = new ConsoleView();

            // Objekt in Controller
            var engine = new SimulationEngine(anlage, logger, view);

            // Start nach 24h Ablauf
            await engine.RunAsync();

            Console.Clear();
            Console.WriteLine("24-Stunden-Simulation erfolgreich beendet. Daten exportiert nach pv_log.csv");
            Console.WriteLine("Drücke eine beliebige Taste zum Beenden...");
            Console.ReadKey();
        }
    }
}
