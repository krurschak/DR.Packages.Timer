using System;
using System.Linq;
using DR.Packages.Timer.ConsumerFactory;
using DR.Packages.Timer.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DR.Packages.Timer
{
    public static class Extensions
    {
        /// <summary>
        /// Adds a new Timer with timerName as typeof(T).Name which must equal one timers option name
        /// </summary>
        /// <typeparam name="T">Implementing ITimer class which Consumes the scheduled background task</typeparam>
        /// <returns>IServiceCollection with registered Timer consumer</returns>
        public static IServiceCollection AddTimer<T>(this IServiceCollection services, TimerConsumersOptions options)
            where T : class, ITimer
            => services.AddTimer<T>(typeof(T).Name, options);

        /// <summary>
        /// Adds a new Timer
        /// </summary>
        /// <typeparam name="T">Implementing ITimer class which handles the scheduled background task</typeparam>
        /// <param name="timerName">Name of the Timer (Must equal one timers option name)</param>
        /// <returns>IServiceCollection with registered Timer</returns>
        public static IServiceCollection AddTimer<T>(this IServiceCollection services, string timerName, TimerConsumersOptions options)
            where T : class, ITimer
        {
            // Register implemented Timer
            services.AddScoped<T>();

            // Register Timer Consumer and add it to Consumer Factory with Options by timerName
            services.AddTimerConsumer<T>(timerName, consumer =>
            {
                var timer = options.Timers.SingleOrDefault(s => s.Name.Equals(timerName,
                    StringComparison.InvariantCultureIgnoreCase));
                if (timer is null)
                {
                    throw new ArgumentNullException(timerName,
                        $"Timer option: '{timerName}' was not found.");
                }

                consumer.PeriodMinutes = timer.PeriodMinutes;
                consumer.Active = timer.Active;
            });

            // Add Timed hosted Services
            services.AddHostedService(x => x.GetService<TimerConsumerFactory<T>>().GetConsumer(timerName));

            return services;
        }
    }
}
