
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Player.TeamA
{
    class Scout : MeineAmeise
    {
        public Scout(TeamAKlasse ameise) : base(ameise) { }

        public override string ToString()
        {
            return "Scout";
        }
    }
}
