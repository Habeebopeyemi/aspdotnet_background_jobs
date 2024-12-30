using API.Data;
using API.Services.Abstractions;

namespace API.Services.One_off
{
    /// <summary>
    /// One-off Background Tasks With IHostedService
    /// The interface has two methods – StartAsync() and StopAsync().
    /// The first one runs once when our application starts; meanwhile, the second one runs when it stops.
    /// We cannot inject our DbContext directly as it’s registered as a scoped service, and we can only inject transient or singleton services when implementing the IHostedService interface.
    /// </summary>
    /// <param name="worker"></param>
    /// <param name="serviceProvider"></param>
    public class InitializationHostedService(
    IWorker worker,
    IServiceProvider serviceProvider) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = serviceProvider.CreateScope();

            await using var context = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            await worker.SeedDatabaseAsync(context, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
