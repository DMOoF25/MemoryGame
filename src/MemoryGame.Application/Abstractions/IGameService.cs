using MemoryGame.Domain.Entities;

namespace MemoryGame.Application.Abstractions;

public interface IGameService
{
    IReadOnlyList<Card> Cards { get; }
    GameStats Stats { get; }

    Task StartNewGameAsync(string username, CancellationToken ct = default);
    Task FlipAsync(int cardId, CancellationToken ct = default);
}
