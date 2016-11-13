using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.Debug
{
    public enum Type
    {
        [Description("INFO")]
        Info,
        [Description("WARNING")]
        Warning,
        [Description("CRITICAL")]
        Critical,
        [Description("DEBUG")]
        Debug
    }

    public class Time
    {
        #region Singleton
        private Time() { }

        public static Time Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested() { }

            internal static readonly Time instance = new Time();
        }
        #endregion

        private Object _Locker = new Object();

        private Dictionary<string, DebugFunction> _Timers = new Dictionary<string, DebugFunction>();
        private List<Type> debugLevel = new List<Type>();

        private bool ShallDisplay(Type type)
        {
            return debugLevel.Contains(type);
        }

        public void DebugMode(Type type, bool display = true)
        {
            lock (_Locker)
            {
                if (display == false && debugLevel.Contains(type))
                    debugLevel.Remove(type);
                else if (display == true && !debugLevel.Contains(type))
                    debugLevel.Add(type);
            }
        }

        public void StartTimer(string name, Type type = Type.Debug, bool displayStart = false)
        {
            lock(_Locker)
            {
                if (_Timers.Keys.Contains(name))
                    throw new ArgumentException("Debug Name already exist");
                DebugFunction _current = new DebugFunction(name, type);
                _Timers.Add(name, _current);
                if (displayStart && ShallDisplay(type))
                    System.Diagnostics.Debug.WriteLine(_current.DisplayName() + " (START!)");
            }
        }

        public void Display(string name)
        {
            lock (_Locker)
            {
                if (_Timers.Keys.Contains(name))
                {
                    DebugFunction _current = _Timers[name];
                    if (ShallDisplay(_current.Type))
                        System.Diagnostics.Debug.WriteLine(_current.ToString());
                }
            }
        }

        public void StopTimer(string name)
        {
            lock(_Locker)
            {
                if (!_Timers.Keys.Contains(name))
                    throw new ArgumentOutOfRangeException("Debug name doesn't exist");
                DebugFunction _current = _Timers[name];
                if (ShallDisplay(_current.Type))
                {
                    System.Diagnostics.Debug.WriteLine(_current.ToString());
                    _current.Stop();
                }
                _Timers.Remove(name);
            }
        }

        private class DebugFunction
        {
            public Type Type { get; }
            public string Name { get; }
            public System.Diagnostics.Stopwatch Timer { get; private set; }

            private DebugFunction() { }
            public DebugFunction(string name, Type type = Type.Info)
            {
                Name = name;
                Type = type;

                Timer = new System.Diagnostics.Stopwatch();
                Timer.Start();
            }

            public void Stop()
            {
                if (Timer.IsRunning)
                    Timer.Stop();
            }

            public string DisplayName()
            {
                var type = typeof(Type);
                var memInfo = type.GetMember(Type.ToString());
                var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                var description = ((DescriptionAttribute)attributes[0]).Description;
                return "[" + description + "] " + Name;
            }

            public override string ToString()
            {
                return DisplayName() + " (" + Timer.ElapsedMilliseconds + "ms)";
            }
        }
    }
}
