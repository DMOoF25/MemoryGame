using MemoryGame.Application.Abstractions;

namespace MemoryGame.Infrastructure.Timing;

public class DispatcherTimerService : ITimerService
{
    private readonly System.Timers.Timer _timer = new(1000);
    private DateTime _started;
    private TimeSpan _elapsedBeforePause;

    public DispatcherTimerService()
    {
        _timer.Elapsed += (_, __) =>
        {
            Elapsed = _elapsedBeforePause + (DateTime.Now - _started);
            Tick?.Invoke(this, Elapsed);
        };
        _timer.AutoReset = true;
    }

    public event EventHandler<TimeSpan>? Tick;
    public TimeSpan Elapsed { get; private set; }

    public void Start()
    {
        _started = DateTime.Now;
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
        _elapsedBeforePause = Elapsed;
    }

    public void Reset()
    {
        _timer.Stop();
        _elapsedBeforePause = TimeSpan.Zero;
        Elapsed = TimeSpan.Zero;
    }
}
