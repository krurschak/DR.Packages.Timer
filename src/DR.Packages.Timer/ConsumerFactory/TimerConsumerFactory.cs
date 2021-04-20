using System;
using System.Collections.Generic;
using System.Linq;
using DR.Packages.Timer.Consumer;
using DR.Packages.Timer.Interfaces;

namespace DR.Packages.Timer.ConsumerFactory
{

    public class TimerConsumerFactory<T> where T : ITimer
    {
        private readonly IDictionary<string, TimerConsumer<T>> timerConsumers;

        public TimerConsumerFactory(IEnumerable<NamedTimerConsumer<T>> namedTimerConsumers)
        {
            timerConsumers = namedTimerConsumers.ToDictionary(n => n.Name, n => n.TimerConsumer);
        }

        public TimerConsumer<T> GetConsumer(string name)
        {
            if (timerConsumers.TryGetValue(name, out var timerConsumer))
                return timerConsumer;

            throw new ArgumentException($"Timer Consumer '{name}' not found");
        }
    }
}
