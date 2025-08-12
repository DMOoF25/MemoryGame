namespace MemoryGame.Domain.Entities;

public class GameStats
{
    public string Username { get; set; } = "";
    public int Moves { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset? EndedAt { get; set; }

    public TimeSpan Elapsed =>
        (EndedAt ?? DateTimeOffset.UtcNow) - StartedAt;

    public bool IsCompleted => EndedAt.HasValue;
}
