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
            Y = (int)(wink * Math.Sin(Math.PI * wink / 180.0));
        }

        public MeineKoordinaten(int information)
        {
            sbyte y = (sbyte)(information % 256);
            sbyte x = (sbyte)((information >> 8) % 256);
            X = x * 6;
            Y = y * 6;
        }

        /// <summary>
        /// bestimmt die neue aktuelle Position aus den Polarkoordinaten
        /// </summary>
        /// <param name="dist">Entfernung</param>
        /// <param name="wink">winkel</param>
        public void SetzeNeueKoordinaten(int dist, int wink)
        {
            X = (int)(dist * Math.Cos(Math.PI * wink / 180.0));
            Y = (int)(wink * Math.Sin(Math.PI * wink / 180.0));
        }        
    }
}
