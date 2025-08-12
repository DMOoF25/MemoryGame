using System.Text.Json;
using MemoryGame.Application.Abstractions;
using MemoryGame.Domain.Entities;

namespace MemoryGame.Infrastructure.Stats;

public class FileStatsRepository : IStatsRepository
{
    private readonly string _filePath;

    public FileStatsRepository(string filePath)
    {
        _filePath = filePath;
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
    }

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