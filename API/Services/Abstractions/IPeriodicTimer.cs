namespace API.Services.Abstractions
{
    public interface IPeriodicTimer : IDisposable
    {
        ValueTask<bool> WaitForNextTickAsync(CancellationToken cancellationToken = default);
    }
}
