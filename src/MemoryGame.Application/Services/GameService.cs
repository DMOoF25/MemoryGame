using MemoryGame.Application.Abstractions;
using MemoryGame.Domain.Entities;

namespace MemoryGame.Application.Services;

public class GameService : IGameService
{
    private readonly IDeckProvider _deckProvider;
    private readonly IStatsRepository _statsRepo;

    private List<Card> _cards = [];
    private Card? _first;
    private bool _lock; // prevents flipping during evaluation

    public bool Lock { get => _lock; set => _lock = value; }

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
            // We made a mistake here, we need to show both cards flipped for a moment so we pu the delay is inside GameViewModel instead
            // Return the IDs of the two cards to be unflipped after delay - Wrong approach
            // Instead, we should handle the UI update in the ViewModel after this method returns. So we return the IDs of the cards to be unflipped.
            var firstId = _first.Id;
            var secondId = card.Id;
            _first = null;
            //_lock = false;
            return (false, firstId, secondId);
        }
    }

    //public async Task FlipAsync(int cardId, CancellationToken ct = default)
    //{
    //    if (_lock) return;

    //    var card = _cards.FirstOrDefault(c => c.Id == cardId);
    //    if (card is null || card.IsMatched ) return;

    //    // Every flip counts as a move
    //    Stats.Moves++;

    //    card.IsFlipped = true;

    //    if (_first is null)
    //    {
    //        _first = card;
    //        return;
    //    }

    //    // Second flip — evaluate
    //    _lock = true;

    //    if (_first.Symbol == card.Symbol)
    //    {
    //        _first.IsMatched = true;
    //        card.IsMatched = true;
    //        _first = null;
    //        _lock = false;

    //        if (_cards.All(c => c.IsMatched))
    //        {
    //            Stats.EndedAt = DateTimeOffset.UtcNow;
    //            await _statsRepo.SaveAsync(Stats, ct);
    //        }
    //    }
    //    else
    //    {
    //        // Here I need to update the UI to show both cards flipped for a moment
    //        // Brief delay so the player can see the second card
    //        try
    //        {
    //            await Task.Delay(700, ct);
    //        }
    //        catch (TaskCanceledException) { /* ignore */ }

    //        _first.IsFlipped = false;
    //        card.IsFlipped = false;
    //        _first = null;
    //        _lock = false;
    //    }
    //}
}