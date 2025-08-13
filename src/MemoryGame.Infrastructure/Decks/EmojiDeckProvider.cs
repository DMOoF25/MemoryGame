using MemoryGame.Application.Abstractions;
using MemoryGame.Domain.Entities;

namespace MemoryGame.Infrastructure.Decks;

/// <summary>
/// Provides a deck of emoji cards for the memory game.
/// </summary>
public class EmojiDeckProvider : IDeckProvider
{
    /// <summary>
    /// The base set of emoji symbols used to create card pairs.
    /// </summary>
    private static readonly string[] BaseSymbols =
    {
        "🍎","🐶","🚗","⭐","🎈","🌙","⚽","🎵"
    };

    /// <summary>
    /// Creates and returns a shuffled deck of 16 cards (8 pairs), all unflipped and unmatched.
    /// </summary>
    /// <returns>
    /// A list of <see cref="Card"/> objects representing a shuffled deck for the game.
    /// </returns>
    public List<Card> CreateShuffledDeck()
    {
        var symbols = BaseSymbols.SelectMany(s => new[] { s, s }).ToList();
        var rng = new Random();
        symbols = symbols.OrderBy(_ => rng.Next()).ToList();

        var cards = symbols.Select((sym, i) => new Card
        {
            Id = i,
            Symbol = sym,
            IsFlipped = false,
            IsMatched = false
        }).ToList();

        return cards;
    }
}