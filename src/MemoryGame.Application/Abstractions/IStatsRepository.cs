using MemoryGame.Domain.Entities;

namespace MemoryGame.Application.Abstractions;

public interface IStatsRepository
{
    Task SaveAsync(GameStats stats, CancellationToken ct = default);
}