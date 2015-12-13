using AntMe.Deutsch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Player.TeamA
{
    class MeineAmeise
    {
        protected Bau bau;                                  // Referenz auf den Ameisenbau
        protected TeamAKlasse ich;                          // aktuelle Ameise
        protected MeineKoordinaten meinePosition;           // aktuelle Koordinaten
        protected MeinZiel aktuellesZiel;                   // Aktuelles Ziel der Ameise
        protected bool sollWarten = false;
        protected bool sollZuZiel = true;
        protected string text;

        #region Kasten
        public MeineAmeise(TeamAKlasse ameise)
        {
            ich = ameise;
            bau = null;
        }

        /// <summary>
        /// Wenn die Ameise keinerlei Aufträge hat, wartet sie auf neue Aufgaben. Um dir das 
        /// mitzuteilen, wird diese Methode hier aufgerufen.
        /// Weitere Infos unter http://wiki.antme.net/de/API1:Wartet
        /// </summary>
        public virtual void Wartet()
        {
            text = "Ich warte!";
            if (ich.AktuelleLast == 0)
                sollZuZiel = true;

            BestimmeBau();

            if (!sollWarten)
            {
                if (aktuellesZiel == null)
                {
                    ich.GeheGeradeaus();
                }
                else
                {
                    if (ich.AktuelleLast != 0)
                        ich.GeheZuBau();
                    else
                        GeheZuZielOpt();
                }
            }
        }

        public void BestimmeBau()
        {
            if (bau == null)
            {
                ich.GeheZuBau();
                bau = (Bau)ich.Ziel;
                ich.BleibStehen();
                BestimmeKoordinaten();
            }
        }

        public void BestimmeKoordinaten()
        {
            if (bau != null)
            {
                int winkel = Koordinate.BestimmeRichtung(bau, ich);
                int radius = Koordinate.BestimmeEntfernung(bau, ich);
                if (meinePosition == null)
                    this.meinePosition = new MeineKoordinaten(radius, winkel);
                else
                {
                    this.meinePosition.SetzeNeueKoordinaten(radius, winkel);
                }
            }
        }

        /// <summary>
        /// Erreicht eine Ameise ein drittel ihrer Laufreichweite, wird diese Methode aufgerufen.
        /// Weitere Infos unter http://wiki.antme.net/de/API1:WirdM%C3%BCde
        /// </summary>
        public virtual void WirdMüde()
        {
        }

        /// <summary>
        /// Wenn eine Ameise stirbt, wird diese Methode aufgerufen. Man erfährt dadurch, wie 
        /// die Ameise gestorben ist. Die Ameise kann zu diesem Zeitpunkt aber keinerlei Aktion 
        /// mehr ausführen.
        /// Weitere Infos unter http://wiki.antme.net/de/API1:IstGestorben
        /// </summary>
        /// <param name="todesart">Art des Todes</param>
        public virtual void IstGestorben(Todesart todesart)
        {
        }

        /// <summary>
        /// Diese Methode wird in jeder Simulationsrunde aufgerufen - ungeachtet von zusätzlichen 
        /// Bedingungen. Dies eignet sich für Aktionen, die unter Bedingungen ausgeführt werden 
        /// sollen, die von den anderen Methoden nicht behandelt werden.
        /// Weitere Infos unter http://wiki.antme.net/de/API1:Tick
        /// </summary>
        public virtual void Tick()
        {
            BestimmeKoordinaten();

            if (aktuellesZiel != null && aktuellesZiel.Gegenstand != null)
            {
                ich.Denke(String.Format("Sprühe: ({0}, {1})",aktuellesZiel.Koordinaten.X, aktuellesZiel.Koordinaten.Y));
                ich.SprüheMarkierung(aktuellesZiel.InformationKodieren());
            } else if (aktuellesZiel != null && aktuellesZiel.Gegenstand == null)
                ich.Denke(String.Format("Rieche: ({0}, {1})", aktuellesZiel.Koordinaten.X, aktuellesZiel.Koordinaten.Y));

            //if (hatZiel)
            //    ameise.GeheGeradeaus();

            //if ((aktuellesZiel != null) && (Koordinate.BestimmeEntfernung(aktuellesZiel, ameise) < 30))
            //{
            //    ameise.GeheZuZiel(aktuellesZiel);
            //}
            //ameise.Denke(winkel + ", " + radius + "\n" + x + ", " + y);
        }

        protected void GeheZuBauOpt()
        {
            BestimmeBau();
            text = "Gehe zum Bau!";
            int distanz = Koordinate.BestimmeEntfernung(ich, bau);
            int winkel = Koordinate.BestimmeRichtung(ich, bau);
            ich.DreheInRichtung(winkel);
            if (distanz < ich.Sichtweite)
                ich.GeheZuBau();
            else
                ich.GeheGeradeaus(distanz);
        }

        protected void GeheZuZielOpt()
        {
            int distanz = 0, winkel = 0;
            text = "Gehe zum Ziel!";
            if (aktuellesZiel.Gegenstand == null)
            {
                MeineKoordinaten tmp = aktuellesZiel.BestimmeRichtung(meinePosition);
                distanz = (int)tmp.BestimmeBetrag();
                winkel = tmp.BestimmeRichtung();
            }
            else {
                distanz = Koordinate.BestimmeEntfernung(ich, aktuellesZiel.Gegenstand);
                winkel = Koordinate.BestimmeRichtung(ich, aktuellesZiel.Gegenstand);
            }
            ich.DreheInRichtung(winkel);
            if (distanz < ich.Sichtweite)
                if (this.aktuellesZiel.Gegenstand == null)
                    ich.GeheGeradeaus();
                else
                    ich.GeheZuZiel(this.aktuellesZiel.Gegenstand);
            else
                ich.GeheGeradeaus();
        }

        protected void GeheZuKoordinate(MeinZiel ziel)
        {
            //text = String.Format("Iche gehe zu Koordinate ({0},{1})!", ziel.Koordinaten.X, ziel.Koordinaten.Y);
            //int richtung = ziel.bestimmeRichtung(this.meinePosition);
            //if (richtung != ich.Richtung)
            //{
            //    ich.DreheInRichtung(richtung);
            //}
            //ich.GeheGeradeaus();
        }

        #endregion

        #region 

        /// <summary>
        /// Sobald eine Ameise innerhalb ihres Sichtradius einen Apfel erspäht wird 
        /// diese Methode aufgerufen. Als Parameter kommt das betroffene Stück Obst.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:Sieht(Obst)"
        /// </summary>
        /// <param name="obst">Das gesichtete Stück Obst</param>
        public virtual void Sieht(Obst obst)
        {
            //if (ameise.AktuelleLast == 0)
            //{
            //    myziel.ziel = obst;
            //    GeheZuZielOpt();
            //}
            //else
            //    GeheZuBauOpt();
        }

        /// <summary>
        /// Sobald eine Ameise innerhalb ihres Sichtradius einen Zuckerhügel erspäht wird 
        /// diese Methode aufgerufen. Als Parameter kommt der betroffene Zuckerghügel.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:Sieht(Zucker)"
        /// </summary>
        /// <param name="zucker">Der gesichtete Zuckerhügel</param>
        public virtual void Sieht(Zucker zucker)
        {
            if (sollZuZiel)
            {
                MeineKoordinaten ziel = new MeineKoordinaten(Koordinate.BestimmeEntfernung(bau, zucker), Koordinate.BestimmeRichtung(bau, zucker));
                aktuellesZiel = new MeinZiel(zucker, ziel);
                GeheZuZielOpt();
            }
            else
                GeheZuBauOpt();
        }

        /// <summary>
        /// Hat die Ameise ein Stück Obst als Ziel festgelegt, wird diese Methode aufgerufen, 
        /// sobald die Ameise ihr Ziel erreicht hat. Ab jetzt ist die Ameise nahe genug um mit 
        /// dem Ziel zu interagieren.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:ZielErreicht(Obst)"
        /// </summary>
        /// <param name="obst">Das erreichte Stück Obst</param>
        public virtual void ZielErreicht(Obst obst)
        {
            //if (bau == null)
            //{
            //    ameise.GeheZuBau();
            //    bau = ameise.Ziel as Bau;
            //}
            
            //ameise.Nimm(obst);
            //GeheZuBauOpt();
        }

        /// <summary>
        /// Hat die Ameise eine Zuckerhügel als Ziel festgelegt, wird diese Methode aufgerufen, 
        /// sobald die Ameise ihr Ziel erreicht hat. Ab jetzt ist die Ameise nahe genug um mit 
        /// dem Ziel zu interagieren.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:ZielErreicht(Zucker)"
        /// </summary>
        /// <param name="zucker">Der erreichte Zuckerhügel</param>
        public virtual void ZielErreicht(Zucker zucker)
        {

            ich.Nimm(zucker);
            sollZuZiel = false;
            GeheZuBauOpt();
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
        public virtual void RiechtFreund(Markierung markierung)
        {
            MeineKoordinaten k = new MeineKoordinaten(markierung.Information);
            if (aktuellesZiel == null)
            {
                aktuellesZiel = new MeinZiel(k);      
            }
        }

        /// <summary>
        /// So wie Ameisen unterschiedliche Nahrungsmittel erspähen können, entdecken Sie auch 
        /// andere Spielelemente. Entdeckt die Ameise eine Ameise aus dem eigenen Volk, so 
        /// wird diese Methode aufgerufen.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:SiehtFreund(Ameise)"
        /// </summary>
        /// <param name="ameise">Erspähte befreundete Ameise</param>
        public virtual void SiehtFreund(Ameise ameise)
        {
            //if (freund == null)
            //{
            //    if (ameise.AktuelleLast != 0 && this.ameise.AktuelleLast == 0)
            //    {
            //        aktuellesZiel = ameise;
            //        freund = ameise;
            //        freundID = ameise.Id;
            //    }
            //}
        }

        /// <summary>
        /// So wie Ameisen unterschiedliche Nahrungsmittel erspähen können, entdecken Sie auch 
        /// andere Spielelemente. Entdeckt die Ameise eine Ameise aus einem befreundeten Volk 
        /// (Völker im selben Team), so wird diese Methode aufgerufen.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:SiehtVerb%C3%BCndeten(Ameise)"
        /// </summary>
        /// <param name="ameise">Erspähte verbündete Ameise</param>
        public virtual void SiehtVerbündeten(Ameise ameise)
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
        public virtual void SiehtFeind(Ameise ameise)
        {
        }

        /// <summary>
        /// So wie Ameisen unterschiedliche Nahrungsmittel erspähen können, entdecken Sie auch 
        /// andere Spielelemente. Entdeckt die Ameise eine Wanze, so wird diese Methode aufgerufen.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:SiehtFeind(Wanze)"
        /// </summary>
        /// <param name="wanze">Erspähte Wanze</param>
        public virtual void SiehtFeind(Wanze wanze)
        {
            //int abstand = Koordinate.BestimmeEntfernung(ameise, wanze);
            //ameise.Denke(abstand.ToString());

            //if (abstand <= 10)
            //{
            //    ameise.BleibStehen();
            //    this.wanze = true;
            //}
        }

        /// <summary>
        /// Es kann vorkommen, dass feindliche Lebewesen eine Ameise aktiv angreifen. Sollte 
        /// eine feindliche Ameise angreifen, wird diese Methode hier aufgerufen und die 
        /// Ameise kann entscheiden, wie sie darauf reagieren möchte.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:WirdAngegriffen(Ameise)"
        /// </summary>
        /// <param name="ameise">Angreifende Ameise</param>
        public virtual void WirdAngegriffen(Ameise ameise)
        {
        }

        /// <summary>
        /// Es kann vorkommen, dass feindliche Lebewesen eine Ameise aktiv angreifen. Sollte 
        /// eine Wanze angreifen, wird diese Methode hier aufgerufen und die Ameise kann 
        /// entscheiden, wie sie darauf reagieren möchte.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:WirdAngegriffen(Wanze)"
        /// </summary>
        /// <param name="wanze">Angreifende Wanze</param>
        public virtual void WirdAngegriffen(Wanze wanze)
        {
        }
        #endregion
    }
}
