using API.Data;
using API.Services.Abstractions;

namespace API.Services.One_off
{
    /// <summary>
    /// Another option for background tasks is to implement a class deriving from the
    /// BackgroundService abstract class, implementing the ExecuteAsync() method:
    /// As with the IHostedService interface, we cannot use a scoped service directly,
    /// so we inject an IServiceProvider instance.
    /// 
    /// </summary>
    /// <param name="worker"></param>
    /// <param name="serviceProvider"></param>
    public class InitializationBackgroundService(
    IWorker worker,
    IServiceProvider serviceProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using IServiceScope scope = serviceProvider.CreateScope();

            await using var context = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            await worker.SeedDatabaseAsync(context, stoppingToken);
        }
    }
}
