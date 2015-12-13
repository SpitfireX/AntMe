using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Player.TeamA
{
    class MeineKoordinaten
    {
        public int X { get; private set; }      // X-Achse in kartesischen Koordinaten
        public int Y { get; private set; }      // Y-Achse in kartesischen Koordinaten

        public MeineKoordinaten(int dist, int wink)
        {
            X = (int)(dist * Math.Cos(Math.PI * wink / 180.0));
            Y = (int)(dist * Math.Sin(Math.PI * wink / 180.0));
        }

        public MeineKoordinaten(int information)
        {
            sbyte y = (sbyte)(information % 256);
            sbyte x = (sbyte)((information >> 8) % 256);
            X = x * 6;
            Y = y * 6;
        }

        public MeineKoordinaten(MeineKoordinaten k)
        {
            X = k.X;
            Y = k.Y;
        }

        public MeineKoordinaten(MeineKoordinaten start, MeineKoordinaten ziel)
        {
            X = ziel.X - start.X;
            Y = ziel.Y - start.Y;
        }

        /// <summary>
        /// bestimmt die neue aktuelle Position aus den Polarkoordinaten
        /// </summary>
        /// <param name="dist">Entfernung</param>
        /// <param name="wink">winkel</param>
        public void SetzeNeueKoordinaten(int dist, int wink)
        {
            X = (int)(dist * Math.Cos(Math.PI * wink / 180.0));
            Y = (int)(dist * Math.Sin(Math.PI * wink / 180.0));
        }  
        
        public double BestimmeBetrag()
        {
            return Math.Sqrt((X * X) + (Y * Y));
        }

        public int BestimmeRichtung()
        {
            if (Y < 0)
            {
                return (int)((-Math.Acos(X / BestimmeBetrag())*180)/Math.PI);
            }
            else
            {
                return (int)((Math.Acos(X / BestimmeBetrag())*180)/Math.PI);
            }
        }      
    }
}
