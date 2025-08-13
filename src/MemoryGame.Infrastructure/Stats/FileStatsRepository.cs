using System.Text.Json;
using MemoryGame.Application.Abstractions;
using MemoryGame.Domain.Entities;

namespace MemoryGame.Infrastructure.Stats;

/// <summary>
/// A file-based implementation of <see cref="IStatsRepository"/> that saves game statistics as JSON lines.
/// </summary>
public class FileStatsRepository : IStatsRepository
{
    private readonly string _filePath;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileStatsRepository"/> class.
    /// Ensures the directory for the file path exists.
    /// </summary>
    /// <param name="filePath">The path to the file where statistics will be saved.</param>
    public FileStatsRepository(string filePath)
    {
        _filePath = filePath;
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
    }

    /// <summary>
    /// Saves the specified <see cref="GameStats"/> instance to the file as a JSON line.
    /// </summary>
    /// <param name="stats">The game statistics to save.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task SaveAsync(GameStats stats, CancellationToken ct = default)
    {
        var entry = new
        {
            stats.Username,
            stats.Moves,
            StartedAt = stats.StartedAt,
            EndedAt = stats.EndedAt,
            ElapsedSeconds = stats.Elapsed.TotalSeconds
        };

        // Append as JSON lines for simplicity
        await using var sw = new StreamWriter(_filePath, append: true);
        await sw.WriteLineAsync(JsonSerializer.Serialize(entry));
    }
}