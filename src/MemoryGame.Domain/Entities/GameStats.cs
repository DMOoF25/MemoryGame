namespace MemoryGame.Domain.Entities;

public class GameStats
{
    public string Username { get; set; } = "";
    public int MoveCount { get; set; }
    public TimeSpan TimeElapsed { get; set; }
}
