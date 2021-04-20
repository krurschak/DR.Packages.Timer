using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DR.Packages.Timer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DR.Packages.Timer.Consumer
{
    public class TimerConsumer<T> : IHostedService, IDisposable where T : ITimer
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger logger;
        private System.Threading.Timer timer;

        public int PeriodMinutes { get; set; }
        public bool Active { get; set; }

        public TimerConsumer(IServiceScopeFactory serviceScopeFactory,
            ILogger<TimerConsumer<T>> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
            
            // Set Default values
            PeriodMinutes = 60;
            Active = true;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (Active)
            {
                timer = new System.Threading.Timer(TimerCallback, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(PeriodMinutes));
                logger.LogInformation("Timed Background Service: '" + typeof(T).Name + "' repeats every " + PeriodMinutes + " minutes");
            }
            else
            {
                logger.LogInformation("Timed Background Service '" + typeof(T).Name + "' not active");
            }

            return Task.CompletedTask;
        }

        public async void TimerCallback(object state)
        {
            logger.LogInformation("Timed Background Service '" + typeof(T).Name + " triggered");
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var taskHandler = scope.ServiceProvider.GetRequiredService<T>();
                await taskHandler.ConsumeAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Timed Background Service '" + typeof(T).Name + "' stopped");

            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
