using MemoryGame.Application.Abstractions;
using MemoryGame.Domain.Entities;

namespace MemoryGame.Application.Services;

/// <summary>
/// Provides the core logic for the memory game, including card management, game state, and move evaluation.
/// </summary>
public class GameService : IGameService
{
    private readonly IDeckProvider _deckProvider;
    private readonly IStatsRepository _statsRepo;

    private List<Card> _cards = [];
    private Card? _first;
    private bool _lock; // prevents flipping during evaluation

    /// <inheritdoc/>
    public bool Lock { get => _lock; set => _lock = value; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GameService"/> class.
    /// </summary>
    /// <param name="deckProvider">The deck provider used to create shuffled decks.</param>
    /// <param name="statsRepo">The repository used to persist game statistics.</param>
    public GameService(IDeckProvider deckProvider, IStatsRepository statsRepo)
    {
        _deckProvider = deckProvider;
        _statsRepo = statsRepo;
        Stats = new GameStats();
    }

    /// <inheritdoc/>
    public IReadOnlyList<Card> Cards => _cards;

    /// <inheritdoc/>
    public GameStats Stats { get; private set; }

    /// <inheritdoc/>
    public Task StartNewGameAsync(string username, CancellationToken ct = default)
    {
        _cards = _deckProvider.CreateShuffledDeck();
        _first = null;
        _lock = false;

        Stats = new GameStats
        {
            Username = username,
            Moves = 0,
            StartedAt = DateTimeOffset.UtcNow,
            EndedAt = null
        };
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// If a match is found, both cards are marked as matched. If not, the method returns the IDs of the two cards to be unflipped after a delay.
    /// </remarks>
    public async Task<(bool IsMatch, int? FirstId, int? SecondId)> FlipAsync(int cardId, CancellationToken ct = default)
    {
        if (_lock) return (false, null, null);

        var card = _cards.FirstOrDefault(c => c.Id == cardId);
        if (card is null || card.IsMatched) return (false, null, null);

        Stats.Moves++;
        card.IsFlipped = true;

        if (_first is null)
        {
            _first = card;
            return (false, null, null);
        }

        _lock = true;

        if (_first.Symbol == card.Symbol)
        {
            _first.IsMatched = true;
            card.IsMatched = true;
            _first = null;
            _lock = false;

            if (_cards.All(c => c.IsMatched))
            {
                Stats.EndedAt = DateTimeOffset.UtcNow;
                await _statsRepo.SaveAsync(Stats, ct);
            }
            return (true, null, null);
        }
        else
        {
            // We made a mistake here, we need to show both cards flipped for a moment so we put the delay inside GameViewModel instead
            // Return the IDs of the two cards to be unflipped after delay
            var firstId = _first.Id;
            var secondId = card.Id;
            _first = null;
            //_lock = false;
            return (false, firstId, secondId);
        }
    }
}