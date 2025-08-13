using MemoryGame.Domain.Entities;

namespace MemoryGame.Application.Abstractions;

public interface IGameService
{
    IReadOnlyList<Card> Cards { get; }
    GameStats Stats { get; }

    bool Lock { get; set; }

    Task StartNewGameAsync(string username, CancellationToken ct = default);
    Task<(bool IsMatch, int? FirstId, int? SecondId)> FlipAsync(int cardId, CancellationToken ct = default);

}
