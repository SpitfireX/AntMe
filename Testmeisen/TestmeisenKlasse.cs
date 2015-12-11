using AntMe.Deutsch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AntMe.Simulation;

namespace AntMe.Player.Testmeisen
{
    /// <summary>
    /// Diese Datei enthält die Beschreibung für deine Ameise. Die einzelnen Code-Blöcke 
    /// (Beginnend mit "public override void") fassen zusammen, wie deine Ameise in den 
    /// entsprechenden Situationen reagieren soll. Welche Befehle du hier verwenden kannst, 
    /// findest du auf der Befehlsübersicht im Wiki (http://wiki.antme.net/de/API1:Befehlsliste).
    /// 
    /// Wenn du etwas Unterstützung bei der Erstellung einer Ameise brauchst, findest du
    /// in den AntMe!-Lektionen ein paar Schritt-für-Schritt Anleitungen.
    /// (http://wiki.antme.net/de/Lektionen)
    /// </summary>
    [Spieler(
        Volkname = "Testmeisen",   // Hier kannst du den Namen des Volkes festlegen
        Vorname = "",       // An dieser Stelle kannst du dich als Schöpfer der Ameise eintragen
        Nachname = ""       // An dieser Stelle kannst du dich als Schöpfer der Ameise eintragen
    )]

    /// Kasten stellen "Berufsgruppen" innerhalb deines Ameisenvolkes dar. Du kannst hier mit
    /// den Fähigkeiten einzelner Ameisen arbeiten. Wie genau das funktioniert kannst du der 
    /// Lektion zur Spezialisierung von Ameisen entnehmen (http://wiki.antme.net/de/Lektion7).
    [Kaste(
        Name = "Scout",                  // Name der Berufsgruppe
        AngriffModifikator = -1,             // Angriffsstärke einer Ameise
        DrehgeschwindigkeitModifikator = 0, // Drehgeschwindigkeit einer Ameise
        EnergieModifikator = -1,             // Lebensenergie einer Ameise
        GeschwindigkeitModifikator = 0,     // Laufgeschwindigkeit einer Ameise
        LastModifikator = -1,                // Tragkraft einer Ameise
        ReichweiteModifikator = +1,          // Ausdauer einer Ameise
        SichtweiteModifikator = +2           // Sichtweite einer Ameise
    )]

    [Kaste(
        Name = "Standard",                  // Name der Berufsgruppe
        AngriffModifikator = 0,             // Angriffsstärke einer Ameise
        DrehgeschwindigkeitModifikator = 0, // Drehgeschwindigkeit einer Ameise
        EnergieModifikator = 0,             // Lebensenergie einer Ameise
        GeschwindigkeitModifikator = 0,     // Laufgeschwindigkeit einer Ameise
        LastModifikator = 0,                // Tragkraft einer Ameise
        ReichweiteModifikator = 0,          // Ausdauer einer Ameise
        SichtweiteModifikator = 0           // Sichtweite einer Ameise
    )]
    public class TestmeisenKlasse : Basisameise
    {
        Dictionary<string, short> markerTypes = new Dictionary<string, short>
        {
            { "Scout", 0 },
            { "Sugar", 1 },
            { "Apple", 2 }
        };

        public int CombineMarker(short type, short message)
        {
            UInt32 t = (UInt32)type << 16;
            UInt32 m = (UInt32)message;
            return (int)(t | m);
        }

        public int CombineMarker(string type, short message)
        {
            UInt32 t = (UInt32)markerTypes[type] << 16;
            UInt32 m = (UInt32)message;
            return (int)(t | m);
        }

        public string MarkerType(int marker)
        {
            marker >>= 16;
            return markerTypes.FirstOrDefault(x => x.Value == marker).Key;
        }

        public short MarkerMessage(int marker)
        {
            marker <<= 16;
            return (short)(marker >> 16);
        }

        bool scoutSuccess, foundSugar, foundApple, walk;
        Spielobjekt festesZiel = null;
        string dMsg = "";

        #region Kasten

        /// <summary>
        /// Jedes mal, wenn eine neue Ameise geboren wird, muss ihre Berufsgruppe
        /// bestimmt werden. Das kannst du mit Hilfe dieses Rückgabewertes dieser 
        /// Methode steuern.
        /// Weitere Infos unter http://wiki.antme.net/de/API1:BestimmeKaste
        /// </summary>
        /// <param name="anzahl">Anzahl Ameisen pro Kaste</param>
        /// <returns>Name der Kaste zu der die geborene Ameise gehören soll</returns>
        public override string BestimmeKaste(Dictionary<string, int> anzahl)
        {
            // Gibt den Namen der betroffenen Kaste zurück.
            if (anzahl["Scout"] < 10)
            {
                return "Scout";
            }
            else
            {
                return "Standard";
            }
        }

        #endregion

        #region Fortbewegung

        /// <summary>
        /// Wenn die Ameise keinerlei Aufträge hat, wartet sie auf neue Aufgaben. Um dir das 
        /// mitzuteilen, wird diese Methode hier aufgerufen.
        /// Weitere Infos unter http://wiki.antme.net/de/API1:Wartet
        /// </summary>
        public override void Wartet()
        {
            if (Kaste == "Scout")
            {
                if (scoutSuccess && Angekommen && EntfernungZuBau == 0)
                {
                    Denke("Bau");
                    if (foundSugar)
                    {
                        SprüheMarkierung(CombineMarker(markerTypes["Sugar"], (short)(Richtung - 180)), 100);
                    }
                    else
                    {
                        SprüheMarkierung(CombineMarker(markerTypes["Apple"], (short)(Richtung - 180)), 100);
                    }

                    foundSugar = false;
                    foundApple = false;
                    scoutSuccess = false;
                    GeheGeradeaus();
                }
                else
                {
                    GeheGeradeaus();
                    dMsg = "";
                }
            }
            else
            {
                if (festesZiel != null)
                {
                    GeheZuZiel(festesZiel);
                }
                else
                {
                    if (walk)
                        GeheGeradeaus();
                }
            }
        }

        /// <summary>
        /// Erreicht eine Ameise ein drittel ihrer Laufreichweite, wird diese Methode aufgerufen.
        /// Weitere Infos unter http://wiki.antme.net/de/API1:WirdM%C3%BCde
        /// </summary>
        public override void WirdMüde()
        {
            GeheZuBau();
        }

        /// <summary>
        /// Wenn eine Ameise stirbt, wird diese Methode aufgerufen. Man erfährt dadurch, wie 
        /// die Ameise gestorben ist. Die Ameise kann zu diesem Zeitpunkt aber keinerlei Aktion 
        /// mehr ausführen.
        /// Weitere Infos unter http://wiki.antme.net/de/API1:IstGestorben
        /// </summary>
        /// <param name="todesart">Art des Todes</param>
        public override void IstGestorben(Todesart todesart)
        {
        }

        /// <summary>
        /// Diese Methode wird in jeder Simulationsrunde aufgerufen - ungeachtet von zusätzlichen 
        /// Bedingungen. Dies eignet sich für Aktionen, die unter Bedingungen ausgeführt werden 
        /// sollen, die von den anderen Methoden nicht behandelt werden.
        /// Weitere Infos unter http://wiki.antme.net/de/API1:Tick
        /// </summary>
        public override void Tick()
        {
            Denke(Kaste + " > "+ Ziel + ": " + dMsg);

            if (festesZiel != null)
            {
                if (festesZiel is Zucker)
                {
                    if (((Zucker) festesZiel).Menge == 0)
                    {
                        festesZiel = null;
                        GeheZuBau();
                    }
                }
            }

            if (Kaste == "Scout")
            {
                if (!scoutSuccess)
                {
                    SprüheMarkierung(CombineMarker(markerTypes["Scout"], 0), 2);
                }
                else
                {
                    if (foundSugar)
                    {
                        SprüheMarkierung(CombineMarker(markerTypes["Sugar"], (short)(Richtung - 180)), 2);
                    }
                    else
                    {
                        SprüheMarkierung(CombineMarker(markerTypes["Apple"], (short)(Richtung - 180)), 2);
                    }
                }
            }
            else
            {
                if (AktuelleLast > 0 && GetragenesObst == null)
                {
                    SprüheMarkierung(CombineMarker(markerTypes["Sugar"], (short)(Richtung - 180)), 2);
                }
                else if (AktuelleLast > 0)
                {
                    SprüheMarkierung(CombineMarker(markerTypes["Apple"], (short)(Richtung - 180)), 2);
                }
            }
        }

        #endregion

        #region Nahrung

        /// <summary>
        /// Sobald eine Ameise innerhalb ihres Sichtradius einen Apfel erspäht wird 
        /// diese Methode aufgerufen. Als Parameter kommt das betroffene Stück Obst.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:Sieht(Obst)"
        /// </summary>
        /// <param name="obst">Das gesichtete Stück Obst</param>
        public override void Sieht(Obst obst)
        {
            if (Kaste == "Scout")
            {
                if (!scoutSuccess)
                {
                    GeheZuZiel(obst);
                }
            }
            else
            {
                if (AktuelleLast == 0 && Ziel == null)
                {
                    if (BrauchtNochTräger(obst))
                    {
                        GeheZuZiel(obst);
                    }
                    else
                    {
                        GeheZuBau();
                    }
                }
            }
        }

        /// <summary>
        /// Sobald eine Ameise innerhalb ihres Sichtradius einen Zuckerhügel erspäht wird 
        /// diese Methode aufgerufen. Als Parameter kommt der betroffene Zuckerghügel.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:Sieht(Zucker)"
        /// </summary>
        /// <param name="zucker">Der gesichtete Zuckerhügel</param>
        public override void Sieht(Zucker zucker)
        {
            if (Kaste == "Scout")
            {
                if (!scoutSuccess)
                {
                    GeheZuZiel(zucker);
                }
            }
            else
            {
                if (AktuelleLast == 0 && Ziel == null)
                {
                    GeheZuZiel(zucker);
                }
            }
        }

        /// <summary>
        /// Hat die Ameise ein Stück Obst als Ziel festgelegt, wird diese Methode aufgerufen, 
        /// sobald die Ameise ihr Ziel erreicht hat. Ab jetzt ist die Ameise nahe genug um mit 
        /// dem Ziel zu interagieren.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:ZielErreicht(Obst)"
        /// </summary>
        /// <param name="obst">Das erreichte Stück Obst</param>
        public override void ZielErreicht(Obst obst)
        {
            foundApple = true;

            if (Kaste == "Scout")
            {
                dMsg = "Obst gefunden";
                scoutSuccess = true;
            }
            else
            {
                Nimm(obst);
            }

            GeheZuBau();
        }

        /// <summary>
        /// Hat die Ameise eine Zuckerhügel als Ziel festgelegt, wird diese Methode aufgerufen, 
        /// sobald die Ameise ihr Ziel erreicht hat. Ab jetzt ist die Ameise nahe genug um mit 
        /// dem Ziel zu interagieren.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:ZielErreicht(Zucker)"
        /// </summary>
        /// <param name="zucker">Der erreichte Zuckerhügel</param>
        public override void ZielErreicht(Zucker zucker)
        {
            foundSugar = true;

            if (Kaste == "Scout")
            {
                dMsg = "Zucker gefunden";
                scoutSuccess = true;
            }
            else
            {
                festesZiel = zucker;
                Nimm(zucker);
            }

            GeheZuBau();
        }

        #endregion

        #region Kommunikation

        /// <summary>
        /// Markierungen, die von anderen Ameisen platziert werden, können von befreundeten Ameisen 
        /// gewittert werden. Diese Methode wird aufgerufen, wenn eine Ameise zum ersten Mal eine 
        /// befreundete Markierung riecht.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:RiechtFreund(Markierung)"
        /// </summary>
        /// <param name="markierung">Die gerochene Markierung</param>
        public override void RiechtFreund(Markierung markierung)
        {
            string type = MarkerType(markierung.Information);
            short message = MarkerMessage(markierung.Information);

            if (Kaste == "Scout")
            {
                if (type == "Scout" && !scoutSuccess)
                {
                    DreheUmWinkel(10);
                }
            }
            else
            {
                if (this. AktuelleLast == 0 && (type == "Sugar" || type == "Apple"))
                {
                    DreheZuZiel(markierung);
                    walk = true;
                }
            }
        }

        /// <summary>
        /// So wie Ameisen unterschiedliche Nahrungsmittel erspähen können, entdecken Sie auch 
        /// andere Spielelemente. Entdeckt die Ameise eine Ameise aus dem eigenen Volk, so 
        /// wird diese Methode aufgerufen.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:SiehtFreund(Ameise)"
        /// </summary>
        /// <param name="ameise">Erspähte befreundete Ameise</param>
        public override void SiehtFreund(Ameise ameise)
        {
        }

        /// <summary>
        /// So wie Ameisen unterschiedliche Nahrungsmittel erspähen können, entdecken Sie auch 
        /// andere Spielelemente. Entdeckt die Ameise eine Ameise aus einem befreundeten Volk 
        /// (Völker im selben Team), so wird diese Methode aufgerufen.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:SiehtVerb%C3%BCndeten(Ameise)"
        /// </summary>
        /// <param name="ameise">Erspähte verbündete Ameise</param>
        public override void SiehtVerbündeten(Ameise ameise)
        {
        }

        #endregion

        #region Kampf
        /// <summary>
        /// So wie Ameisen unterschiedliche Nahrungsmittel erspähen können, entdecken Sie auch 
        /// andere Spielelemente. Entdeckt die Ameise eine Ameise aus einem feindlichen Volk, 
        /// so wird diese Methode aufgerufen.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:SiehtFeind(Ameise)"
        /// </summary>
        /// <param name="ameise">Erspähte feindliche Ameise</param>
        public override void SiehtFeind(Ameise ameise)
        {
            BleibStehen();
        }

        /// <summary>
        /// So wie Ameisen unterschiedliche Nahrungsmittel erspähen können, entdecken Sie auch 
        /// andere Spielelemente. Entdeckt die Ameise eine Wanze, so wird diese Methode aufgerufen.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:SiehtFeind(Wanze)"
        /// </summary>
        /// <param name="wanze">Erspähte Wanze</param>
        public override void SiehtFeind(Wanze wanze)
        {
            BleibStehen();
        }

        /// <summary>
        /// Es kann vorkommen, dass feindliche Lebewesen eine Ameise aktiv angreifen. Sollte 
        /// eine feindliche Ameise angreifen, wird diese Methode hier aufgerufen und die 
        /// Ameise kann entscheiden, wie sie darauf reagieren möchte.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:WirdAngegriffen(Ameise)"
        /// </summary>
        /// <param name="ameise">Angreifende Ameise</param>
        public override void WirdAngegriffen(Ameise ameise)
        {
        }

        /// <summary>
        /// Es kann vorkommen, dass feindliche Lebewesen eine Ameise aktiv angreifen. Sollte 
        /// eine Wanze angreifen, wird diese Methode hier aufgerufen und die Ameise kann 
        /// entscheiden, wie sie darauf reagieren möchte.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:WirdAngegriffen(Wanze)"
        /// </summary>
        /// <param name="wanze">Angreifende Wanze</param>
        public override void WirdAngegriffen(Wanze wanze)
        {
        }

        #endregion
    }
}
