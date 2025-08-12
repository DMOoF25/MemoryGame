using MemoryGame.Application.Abstractions;
using MemoryGame.Domain.Entities;

namespace MemoryGame.Infrastructure.Decks;

public class EmojiDeckProvider : IDeckProvider
{
    private static readonly string[] BaseSymbols =
    {
        "🍎","🐶","🚗","⭐","🎈","🌙","⚽","🎵"
    };

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