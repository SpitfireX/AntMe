using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Player.TeamA
{
    class Helfer : MeineAmeise
    {
        public Helfer(TeamAKlasse ameise):base(ameise) { }
        public override string ToString()
        {
            return "Helfer";
        }
    }
}
