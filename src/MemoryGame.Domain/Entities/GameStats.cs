namespace MemoryGame.Domain.Entities;

/// <summary>
/// Represents the statistics for a memory game session.
/// </summary>
public class GameStats
{
    /// <summary>
    /// Gets or sets the username of the player.
    /// </summary>
    public string Username { get; set; } = "";

    /// <summary>
    /// Gets or sets the number of moves made in the game.
    /// </summary>
    public int Moves { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the game started.
    /// </summary>
    public DateTimeOffset StartedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the game ended, or <c>null</c> if the game is still in progress.
    /// </summary>
    public DateTimeOffset? EndedAt { get; set; }

    /// <summary>
    /// Gets the elapsed time for the game. If the game is still in progress, returns the time since <see cref="StartedAt"/> until now.
    /// </summary>
    public TimeSpan Elapsed =>
        (EndedAt ?? DateTimeOffset.UtcNow) - StartedAt;

    /// <summary>
    /// Gets a value indicating whether the game has been completed.
    /// </summary>
    public bool IsCompleted => EndedAt.HasValue;
}
