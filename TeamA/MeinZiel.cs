using AntMe.Deutsch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Player.TeamA
{
    class MeinZiel
    {
        public Spielobjekt Gegenstand { get; private set; }
        public MeineKoordinaten Koordinaten { get; private set; }

        public MeinZiel(Spielobjekt gegenstand, MeineKoordinaten koordinaten)
        {
            this.Gegenstand = gegenstand;
            this.Koordinaten = new MeineKoordinaten(koordinaten);
        }

        public MeinZiel(MeineKoordinaten k)
        {
            Gegenstand = null;
            this.Koordinaten = new MeineKoordinaten(k);
        }

        public int InformationKodieren()
        {
            int info = 0;
            sbyte x = (sbyte)(Koordinaten.X / 6);
            sbyte y = (sbyte)(Koordinaten.Y / 6);
            sbyte gegenstand = 0;
            gegenstand += (sbyte)((Gegenstand is Zucker) ? 1 : 0);
            gegenstand += (sbyte)((Gegenstand is Zucker) ? 2 : 0);
            gegenstand += (sbyte)((Gegenstand is Ameise) ? 3 : 0);
            gegenstand += (sbyte)((Gegenstand is Wanze) ? 4 : 0);

            info += (gegenstand << 16);
            info += (x << 8);
            info += y;
            return info;
        }

        public MeineKoordinaten BestimmeRichtung(MeineKoordinaten aktuellePosition)
        {
            return new MeineKoordinaten(aktuellePosition, Koordinaten);
        }
    }
}
