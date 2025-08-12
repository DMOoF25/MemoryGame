namespace MemoryGame.Application.Abstractions;

public interface IAsyncDelay
{
    Task Delay(TimeSpan delay, CancellationToken ct = default);
}
