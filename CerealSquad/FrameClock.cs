using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace CerealSquad
{
    class FrameClock
    {
        Clock frameClock = new Clock();

        public Time Restart()
        {
           return frameClock.Restart();
        }
    }
}
