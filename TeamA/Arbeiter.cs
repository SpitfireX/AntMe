
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Player.TeamA
{
    class Arbeiter : MeineAmeise
    {
        public Arbeiter(TeamAKlasse ameise) : base(ameise) { }

        public override string ToString()
        {
            return "Arbeiter";
        }
    }
}
