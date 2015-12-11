using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Player.TeamA
{
    class Wache : MeineAmeise
    {
        public Wache(TeamAKlasse ameise) : base(ameise) { }

        public override string ToString()
        {
            return "Wache";
        }
    }
}
