using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using System.Timers;

namespace CerealSquad
{
    class EntityTimer
    {
        public Time Time { get; set; }
        public Time Current {  get {
                if (_Start && Time.FromMilliseconds((int)TimerCSharp.ElapsedMilliseconds) + _PausedTime >= Time)
                        return Time.FromMilliseconds((int)TimerCSharp.ElapsedMilliseconds) + _PausedTime;
                if (Time.FromMilliseconds((int)TimerCSharp.ElapsedMilliseconds) + _PausedTime >= Time)
                    return Time;
                return Time.Zero;
            } }

        private System.Diagnostics.Stopwatch TimerCSharp;
        private bool _Start = false;
        private bool _Pause = false;
        private Time _PausedTime = Time.Zero;

        public bool Pause { get { return _Pause; } set { setPause(value); _Pause = value; } }

        public bool Started { get { return _Start; } }

        public EntityTimer(Time time)
        {
            Time = time;
            _Start = false;
            TimerCSharp = new System.Diagnostics.Stopwatch();
        }

        public void Start()
        {
            TimerCSharp.Restart();
            _Start = true;
        }

        public void setPause(bool _pause)
        {
            if (_pause && !_Pause)
                _PausedTime += Time.FromMilliseconds((int)TimerCSharp.ElapsedMilliseconds);
            _Pause = _pause;
        }

        public bool IsTimerOver()
        {
            if (!_Start || !TimerCSharp.IsRunning)
            {
                return true;
            }
            
            if (Time.FromMilliseconds((int)TimerCSharp.ElapsedMilliseconds) + _PausedTime >= Time)
            {
                _PausedTime = Time.Zero;
                TimerCSharp.Stop();
                return true;
            }

            return false;
        }
    }
}
