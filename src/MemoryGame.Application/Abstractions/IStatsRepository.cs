using MemoryGame.Domain.Entities;

namespace MemoryGame.Application.Abstractions;

/// <summary>
/// Defines a contract for persisting game statistics.
/// </summary>
public interface IStatsRepository
{
    /// <summary>
    /// Saves the specified game statistics asynchronously.
    /// </summary>
    /// <param name="stats">The <see cref="GameStats"/> instance to save.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task SaveAsync(GameStats stats, CancellationToken ct = default);
}