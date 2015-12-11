﻿using AntMe.Deutsch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Player.TeamA
{
    class MeineAmeise
    {
        protected Bau bau;
        protected Spielobjekt ziel;
        protected TeamAKlasse ameise;

        protected int radius, winkel, x, y;
        protected bool wanze;

        #region Kasten
        public MeineAmeise(TeamAKlasse ameise)
        {
            this.ameise = ameise;
            ziel = null;
            bau = null;
        }

        /// <summary>
        /// Wenn die Ameise keinerlei Aufträge hat, wartet sie auf neue Aufgaben. Um dir das 
        /// mitzuteilen, wird diese Methode hier aufgerufen.
        /// Weitere Infos unter http://wiki.antme.net/de/API1:Wartet
        /// </summary>
        public virtual void Wartet()
        {
            if (bau == null)
            {
                ameise.GeheZuBau();
                bau = (Bau)ameise.Ziel;
                ameise.BleibStehen();
            }

            if (wanze)
            {
                wanze = false;
            }
            else
            {
                if (ziel != null)
                    ameise.GeheZuZiel(ziel);
                else
                    ameise.GeheGeradeaus();
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
            winkel = Koordinate.BestimmeRichtung(bau, ameise);
            radius = Koordinate.BestimmeEntfernung(bau, ameise);

            x = (int)(radius * Math.Cos(winkel));
            y = (int)(radius * Math.Sin(winkel));

            //ameise.Denke(winkel + ", " + radius + "\n" + x + ", " + y);
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
            //    this.ziel = ameise.Ziel as Obst;
            //    ameise.GeheZuZiel(ziel);
            //}
            //else
            //    ameise.GeheZuZiel(bau);
        }

        /// <summary>
        /// Sobald eine Ameise innerhalb ihres Sichtradius einen Zuckerhügel erspäht wird 
        /// diese Methode aufgerufen. Als Parameter kommt der betroffene Zuckerghügel.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:Sieht(Zucker)"
        /// </summary>
        /// <param name="zucker">Der gesichtete Zuckerhügel</param>
        public virtual void Sieht(Zucker zucker)
        {
            if (ameise.AktuelleLast == 0)
            {
                this.ziel = zucker;
                ameise.GeheZuZiel(ziel);
            }
            else
                ameise.GeheZuZiel(bau);
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
            //ameise.SprüheMarkierung(10, 30);

            //if (bau == null)
            //{
            //    ameise.GeheZuBau();
            //    bau = ameise.Ziel as Bau;
            //}

            //ameise.Nimm(obst);
            //ameise.GeheZuZiel(bau);
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
            ameise.Nimm(zucker);
            if (bau == null)
            {
                ameise.GeheZuBau();
                bau = ameise.Ziel as Bau;
            }
            ameise.GeheZuZiel(bau);
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
            //if (markierung.Information == 10 && ameise.AktuelleLast == 0)
            //    ameise.GeheZuZiel(markierung);
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
            int abstand = Koordinate.BestimmeEntfernung(ameise, wanze);
            ameise.Denke(abstand.ToString());

            if (abstand <= 10)
            {
                ameise.BleibStehen();
                this.wanze = true;
            }
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
