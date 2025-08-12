using MemoryGame.Application.Abstractions;
using MemoryGame.Domain.Entities;

namespace MemoryGame.Infrastructure.Deck;

public class EmojiDeckProvider : IDeckProvider
{
    private static readonly string[] DefaultSymbols =
    {
        "🐶","🐱","🐭","🐹","🐰","🦊","🐻","🐼"
    };

    public IList<Card> CreateShuffledDeck(int pairCount)
    {
        var symbols = DefaultSymbols.Take(pairCount).ToArray();
        var deck = new List<Card>();
        foreach (var s in symbols)
        {
            deck.Add(new Card { Symbol = s });
            deck.Add(new Card { Symbol = s });
        }

        var rnd = new Random();
        deck = deck.OrderBy(_ => rnd.Next()).ToList();
        for (int i = 0; i < deck.Count; i++)
            deck[i].Id = i;

        return deck;
    }
}
