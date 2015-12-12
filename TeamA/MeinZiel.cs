using AntMe.Deutsch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Player.TeamA
{
    class MeinZiel
    {
        public Spielobjekt ziel { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public MeinZiel()
        {
            ziel = null;
        }

    }
}
