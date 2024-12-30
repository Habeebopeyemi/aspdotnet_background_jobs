using API.Data;
using API.Services.Abstractions;

namespace API.Services.One_off
{
    /// <summary>
    /// The new IHostedLifecycleService interface implements the already familiar IHostedService interface.
    /// However, with four additional methods,
    /// the interface provides us more control over when to run something in the background of our application.
    /// ====>>>
    /// First in line is the StartingAsync() method that executes while our application is starting,
    /// before the StartAsync() method.
    /// The StartedAsync() method is triggered after our application has been started.
    /// The StoppingAsync() method runs once we start shutting down our application.
    /// Finally, the StoppedAsync() is executed once our application stops,
    /// and after the StopAsync() method.
    /// ====>>>
    /// </summary>
    /// <param name="worker"></param>
    /// <param name="serviceProvider"></param>
    public class InitializationHostedLifecycleService(
    IWorker worker,
    IServiceProvider serviceProvider) : IHostedLifecycleService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = serviceProvider.CreateScope();

            await using var context = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            await worker.SeedDatabaseAsync(context, cancellationToken);
        }

        public Task StartedAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        public Task StartingAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        public Task StoppedAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        public Task StoppingAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
