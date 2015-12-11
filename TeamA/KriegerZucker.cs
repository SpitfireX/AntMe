using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Player.TeamA
{
    class KriegerZucker : Krieger
    {
        public KriegerZucker(TeamAKlasse ameise) : base(ameise) { }

        public override string ToString()
        {
            return base.ToString() + "Zucker";
        }
    }
}
