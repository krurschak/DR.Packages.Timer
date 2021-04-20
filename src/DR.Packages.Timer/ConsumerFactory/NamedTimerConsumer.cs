using DR.Packages.Timer.Consumer;
using DR.Packages.Timer.Interfaces;

namespace DR.Packages.Timer.ConsumerFactory
{
    public class NamedTimerConsumer<T> where T : ITimer
    {
        public NamedTimerConsumer() {}

        public NamedTimerConsumer(string name, TimerConsumer<T> timerConsumer)
        {
            Name = name;
            TimerConsumer = timerConsumer;
        }

        public string Name { get; set; }
        public TimerConsumer<T> TimerConsumer { get; set; }
    }
}
