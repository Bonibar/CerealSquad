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
        private Time save = Time.Zero;
        private System.Diagnostics.Stopwatch frameClock = new System.Diagnostics.Stopwatch();

        public FrameClock()
        {
            frameClock.Start();
        }

        public Time Restart()
        {
            Time result = Time.FromMilliseconds((int)frameClock.ElapsedMilliseconds);

            frameClock.Restart();
            save = Time.Zero;

            return result;
        }

        public Time GetElapsedTime()
        {
            save += Time.FromMilliseconds((int)frameClock.ElapsedMilliseconds);

            return save;
        }
    }
}
