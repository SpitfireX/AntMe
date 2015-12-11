using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Player.TeamA
{
    class KriegerObst : Krieger
    {
        public KriegerObst(TeamAKlasse ameise) : base(ameise) { }

        public override string ToString()
        {
            return base.ToString() + "Obst";
        }
    }
}
