using AntMe.Deutsch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Player.TeamA
{
    class Wache : MeineAmeise
    {
        private string state;
        private int entfernung = 80;
        private int strecke;
        private Wanze ziel;

        public Wache(TeamAKlasse ameise) : base(ameise) { }

        public override void Wartet()
        {
            BestimmeBau();

            if (state == null)
                state = "BezieheStellung";

            switch (state)
            {
                case "BezieheStellung":
                    ich.GeheGeradeaus();
                    break;
            }
        }

        public override void Tick()
        {
            BestimmeKoordinaten();

            switch (state)
            {
                case "GeheHeim":
                    if (ich.EntfernungZuBau > 0)
                    {
                        GeheZuBauOpt();
                    }
                    else
                    {
                        strecke = 0;
                        state = "BezieheStellung";
                    }
                    break;
                case "BezieheStellung":
                    if (ich.EntfernungZuBau >= entfernung)
                    {
                        ich.BleibStehen();
                        ich.DreheUmWinkel(90);
                        entfernung = ich.EntfernungZuBau + 32; // Radius Ameisenbau = 32
                        strecke += entfernung;
                        state = "Bewachen";
                    }
                    break;
                case "Bewachen":
                    if (ich.ZurückgelegteStrecke < ich.Reichweite - ich.EntfernungZuBau)
                    {
                        if (strecke <= entfernung * 2)
                        {
                            if (ich.RestWinkel == 0)
                            {
                                strecke += ich.AktuelleGeschwindigkeit;
                                ich.GeheGeradeaus();
                            }
                        }
                        else
                        {
                            ich.BleibStehen();
                            ich.DreheUmWinkel(90);
                            state = "BewachenDrehen";
                        }
                    } 
                    else
                    {
                        state = "GeheHeim";
                    }
                    break;
                case "BewachenDrehen":
                    if (ich.RestWinkel == 0) 
                    {
                        strecke = 0;
                        state = "Bewachen";
                    }
                    break;
                case "Angriff":
                        if (ziel != null && Koordinate.BestimmeEntfernung(ich, ziel) <= ich.Sichtweite / 2)
                            ich.GreifeAn(ziel);
                        else
                            state = "GeheHeim";
                    break;
            }

            ich.Denke(
                "s: " + state
            );
        }

        public override void SiehtFeind(Wanze wanze)
        {
            if (state != "GeheHeim")
            {
                state = "Angriff";
                ziel = wanze;
                ich.GreifeAn(ziel);
            }
        }

        public override void Sieht(Obst obst) { }

        public override void Sieht(Zucker zucker) { }

        public override string ToString()
        {
            return "Wache";
        }
    }
}
