using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace CerealSquad
{
    class Timer
    {
        public Time Time { get; set; }
        public Time Current {  get {
                if (_Start && TimerClock.ElapsedTime + _PausedTime <= Time)
                        return TimerClock.ElapsedTime + _PausedTime;
                if (TimerClock.ElapsedTime + _PausedTime >= Time)
                    return Time;
                return Time.Zero;
            } }

        private Clock TimerClock = new Clock();
        private bool _Start = false;
        private bool _Pause = false;
        private Time _PausedTime = Time.Zero;

        public bool Pause { get { return _Pause; } set { setPause(value); _Pause = value; } }

        public bool Started { get { return _Start; } }

        public Timer(Time time)
        {
            Time = time;
            TimerClock.Restart();
            _Start = false;
        }

        public void Start()
        {
            _Start = true;
            TimerClock.Restart();
        }

        public void setPause(bool _pause)
        {
            if (_pause && !_Pause)
                _PausedTime += TimerClock.ElapsedTime;
        }

        public bool IsTimerOver()
        {
            if (!_Start)
                return true;

            if (TimerClock.ElapsedTime + _PausedTime >= Time)
            {
                _PausedTime = Time.Zero;
                return true;
            }

            return false;
        }
    }
}
