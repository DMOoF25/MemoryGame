using MemoryGame.Application.Abstractions;
using MemoryGame.Domain.Entities;

namespace MemoryGame.Application.Services;

public class GameService : IGameService
{
    private readonly IDeckProvider _deckProvider;
    private readonly IStatsRepository _statsRepo;

    private List<Card> _cards = new();
    private Card? _first;
    private bool _lock; // prevents flipping during evaluation

    public GameService(IDeckProvider deckProvider, IStatsRepository statsRepo)
    {
        _deckProvider = deckProvider;
        _statsRepo = statsRepo;
        Stats = new GameStats();
    }

    public IReadOnlyList<Card> Cards => _cards;
    public GameStats Stats { get; private set; }

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

    public async Task FlipAsync(int cardId, CancellationToken ct = default)
    {
        if (_lock) return;

        var card = _cards.FirstOrDefault(c => c.Id == cardId);
        if (card is null || card.IsMatched || card.IsFlipped) return;

        // Every flip counts as a move
        Stats.Moves++;

        card.IsFlipped = true;

        if (_first is null)
        {
            _first = card;
            return;
        }

        // Second flip — evaluate
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
        }
        else
        {
            // Brief delay so the player can see the second card
            try
            {
                await Task.Delay(700, ct);
            }
            catch (TaskCanceledException) { /* ignore */ }

            _first.IsFlipped = false;
            card.IsFlipped = false;
            _first = null;
            _lock = false;
        }
    }
}