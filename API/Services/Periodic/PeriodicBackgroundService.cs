using API.Data;
using API.Services.Abstractions;

namespace API.Services.Periodic
{
    /// <summary>
    /// We create the PeriodicBackgroundService and make it implement the BackgroundService class.
    /// We have the same logic as with our PeriodicHostedService class,
    /// but here it is located inside the ExecuteAsync() method.
    /// </summary>
    /// <param name="worker"></param>
    /// <param name="timer"></param>
    /// <param name="serviceProvider"></param>
    public class PeriodicBackgroundService(
    IWorker worker,
    IPeriodicTimer timer,
    IServiceProvider serviceProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using IServiceScope scope = serviceProvider.CreateScope();

            await using var context = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            while (!stoppingToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(stoppingToken))
            {
                await worker.ArchiveOldClientsAsync(context, stoppingToken);
            }
        }
    }
}
