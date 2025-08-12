using MemoryGame.Domain.Entities;

namespace MemoryGame.Application.Abstractions;

public interface IDeckProvider
{
    IList<Card> CreateShuffledDeck(int pairCount);
}