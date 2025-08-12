using MemoryGame.Domain.Entities;

namespace MemoryGame.Application.Abstractions;

public interface IGameService
{
    IReadOnlyList<Card> Cards { get; }
    GameStats Stats { get; }

    void StartNewGame(string username);
    Task FlipCardAsync(int cardId, CancellationToken ct = default);
    event EventHandler? StateChanged; // Notify UI when cards/stats change
}
