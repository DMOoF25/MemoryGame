using MemoryGame.Domain.Entities;

namespace MemoryGame.Application.Abstractions;

public interface IDeckProvider
{
    // Returns 16 cards (8 pairs), unflipped, unmatched, shuffled.
    List<Card> CreateShuffledDeck();
}