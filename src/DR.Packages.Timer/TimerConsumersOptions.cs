using System;
using System.Collections.Generic;
using System.Text;

namespace DR.Packages.Timer
{
    public class TimerConsumersOptions
    {
        public IEnumerable<Timer> Timers { get; set; }

        public class Timer
        {
            public string Name { get; set; }
            public int PeriodMinutes { get; set; }
            public bool Active { get; set; } = true;
        }
    }
}
