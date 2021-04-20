using System;
using System.Linq;
using DR.Packages.Timer.Consumer;
using DR.Packages.Timer.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DR.Packages.Timer.ConsumerFactory
{
    public static class TimerConsumerFactoryServiceCollectionExtensions
    {
        public static IServiceCollection AddTimerConsumer<T>(this IServiceCollection services, string name, Action<TimerConsumer<T>> configureConsumer)
            where T : ITimer
        {
            if (!services.Any(x => x.ServiceType.Equals(typeof(TimerConsumer<T>))))
                services.AddTransient<TimerConsumer<T>>();

            if (!services.Any(x => x.ServiceType.Equals(typeof(TimerConsumerFactory<T>))))
                services.AddTransient(
                    x => new TimerConsumerFactory<T>(x.GetServices<NamedTimerConsumer<T>>()));

            services.AddTransient(x =>
            {
                var timerConsumer = x.GetService<TimerConsumer<T>>();
                configureConsumer(timerConsumer);
                return new NamedTimerConsumer<T>
                {
                    Name = name,
                    TimerConsumer = timerConsumer
                };
            });

            return services;
        }
    }
}
