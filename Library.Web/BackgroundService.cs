using Library.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Web
{
    public class BackgroundService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public BackgroundService(ILogger<BackgroundService> logger, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var systemService = scope.ServiceProvider.GetRequiredService<ILibrarySystem>();

                await systemService.CheckForOverdueBooks();
                await systemService.CheckForOverdueMemberships();
                await systemService.CheckForSoonOverdueMemberships();
                await systemService.CheckForOverdueReservations();
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}