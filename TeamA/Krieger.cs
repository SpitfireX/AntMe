using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Player.TeamA
{
    class Krieger : MeineAmeise
    {
        public Krieger(TeamAKlasse ameise) : base(ameise) { }

        public override string ToString()
        {
            return "Krieger";
        }
    }
}
