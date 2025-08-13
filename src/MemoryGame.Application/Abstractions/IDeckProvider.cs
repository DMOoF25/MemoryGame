using MemoryGame.Domain.Entities;

namespace MemoryGame.Application.Abstractions;

/// <summary>
/// Provides functionality to create a shuffled deck of cards for the memory game.
/// </summary>
public interface IDeckProvider
{
    /// <summary>
    /// Creates and returns a shuffled deck of 16 cards (8 pairs), all unflipped and unmatched.
    /// </summary>
    /// <returns>
    /// A list of <see cref="Card"/> objects representing a shuffled deck for the game.
    /// </returns>
    List<Card> CreateShuffledDeck();
}