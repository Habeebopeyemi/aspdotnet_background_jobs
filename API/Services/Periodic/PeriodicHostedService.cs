using API.Data;
using API.Services.Abstractions;

namespace API.Services.Periodic
{
    /// <summary>
    /// The first thing we do is to create the PeriodicHostedService class and implement the IHostedService
    /// interface. This time we inject an IPeriodicTimer instance alongside the IWorker and IServiceProvider
    /// instances. Next, inside the StartAsync() method, we get an instance of our ApplicationDbContext via
    /// the already familiar approach.
    /// </summary>
    /// <param name="worker"></param>
    /// <param name="timer"></param>
    /// <param name="serviceProvider"></param>
    public class PeriodicHostedService(
    IWorker worker,
    IPeriodicTimer timer,
    IServiceProvider serviceProvider) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = serviceProvider.CreateScope();

            await using var context = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            while (!cancellationToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(cancellationToken))
            {
                await worker.ArchiveOldClientsAsync(context, cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
