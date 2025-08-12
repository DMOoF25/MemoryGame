namespace MemoryGame.Application.Abstractions;

public interface ITimerService
{
    event EventHandler<TimeSpan>? Tick;
    void Start();
    void Stop();
    TimeSpan Elapsed { get; }
    void Reset();
}
