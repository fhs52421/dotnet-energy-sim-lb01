# PV-Anlage - Simulator (LB01)

Dieser Simulator zeigt Verhalten einer Photovoltaikanlage als Teil eines zukünftigen Energiesystems. Die Simulation bildet einen Zeitraum von 24 Stunden ab, wobei die Zeit gegenüber der Echtzeit beschleunigt abläuft.

## Voraussetzungen
* Eine kompatible Entwicklungsumgebung (z.B. Visual Studio, JetBrains Rider) und .NET 10.0.

## Starthinweise
1. Lade die Datei von GitHub in deine Entwicklungsumgebung.
2. Starte das Programm über den Play-Button in der IDE.
3. Die Simulation startet sofort und schreibt fortlaufend Statusdaten in die Datei `pv_log.csv`.

## Bedienhinweise
Während die Simulation läuft, können folgende Parameter über die Tastatur dynamisch verändert werden, um manuelle Eingriffe zu testen:

* **[P]** Simulation pausieren / fortsetzen
* **[E]** Anlage manuell Ein- / Ausschalten
* **[F]** Wechselrichter-Störung auslösen / beheben
* **[A]** Auto-Modus (Wetterautomatik) aktivieren / deaktivieren
* **[W]** Wetter manuell ändern (nur möglich, wenn Auto-Modus deaktiviert ist)
* **[T]** Simulationsgeschwindigkeit anpassen (Normal: 1000ms, Schnell: 100ms)
* **[S]** Detailgrad der Simulation (Schrittweite) anpassen (15 Minuten oder 60 Minuten)

## Modellbeschreibung und wichtige Annahmen
Das Programm zeigt ein dynamisches Verhalten über die simulierte Zeit:
* **Tageszeit:** Die Anlage erzeugt abhängig von der Uhrzeit Energie; nachts (20:00 Uhr bis 06:00 Uhr) wird keine Energie erzeugt.
* **Einstrahlung:** Die Basis-Sonneneinstrahlung steigt bis zum theoretischen Höchststand um 13:00 Uhr (1000 W/m²) linear an und fällt danach wieder ab.
* **Einflussgrößen:** Das aktuelle Wetter agiert als prozentualer Faktor auf die Einstrahlung (Sonnig = 100%, Bewölkt = 50%, Regen = 20%). Im Auto-Modus variieren diese Einflussgrößen zufällig.
* **Leistungsberechnung:** Die aktuelle Leistung ergibt sich aus dem Verhältnis der berechneten Einstrahlung zur maximalen Einstrahlung multipliziert mit der Anlagen-Nennleistung.
* **Fehlerzustände:** Ein ausgelöster Fehler im Wechselrichter oder eine manuelle Abschaltung reduzieren die Erzeugung sofort auf 0 kW.

## Beispielparameter und Konfiguration
Die Kapazität der Anlage ist konfigurierbar, sodass derselbe Simulator später mit unterschiedlichen Parametern mehrfach gestartet werden kann.
// Beispiel für eine Anlage mit geringerer Leistung:
var anlage = new PvAnlage(4.5);
