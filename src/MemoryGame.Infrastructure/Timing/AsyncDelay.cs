using MemoryGame.Application.Abstractions;

namespace MemoryGame.Infrastructure.Timing;

public class AsyncDelay : IAsyncDelay
{
    public Task Delay(TimeSpan delay, CancellationToken ct = default)
        => Task.Delay(delay, ct);
}
