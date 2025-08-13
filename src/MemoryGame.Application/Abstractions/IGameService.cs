using MemoryGame.Domain.Entities;

namespace MemoryGame.Application.Abstractions;

/// <summary>
/// Defines the contract for the memory game service, providing game state, statistics, and game actions.
/// </summary>
public interface IGameService
{
    /// <summary>
    /// Gets the current list of cards in the game.
    /// </summary>
    IReadOnlyList<Card> Cards { get; }

    /// <summary>
    /// Gets the current game statistics.
    /// </summary>
    GameStats Stats { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the game is currently locked (prevents flipping during evaluation).
    /// </summary>
    bool Lock { get; set; }

    /// <summary>
    /// Starts a new game for the specified username.
    /// </summary>
    /// <param name="username">The username of the player.</param>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task StartNewGameAsync(string username, CancellationToken ct = default);

    /// <summary>
    /// Flips a card with the specified identifier and evaluates the game state.
    /// </summary>
    /// <param name="cardId">The identifier of the card to flip.</param>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>
    /// A task that returns a tuple indicating whether the flip resulted in a match,
    /// and the identifiers of the first and second cards involved in a mismatch (if any).
    /// </returns>
    Task<(bool IsMatch, int? FirstId, int? SecondId)> FlipAsync(int cardId, CancellationToken ct = default);
}
