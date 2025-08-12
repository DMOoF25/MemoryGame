using MemoryGame.Application.Abstractions;
using MemoryGame.Domain.Entities;

namespace MemoryGame.Application.Services;

public class GameService : IGameService
{
    private readonly IDeckProvider _deckProvider;
    private readonly IAsyncDelay _delay;
    private readonly ITimerService _timer;

    private readonly List<Card> _cards = new();
    private readonly GameStats _stats = new();
    private Card? _first;
    private Card? _second;
    private bool _resolvingPair;

    public GameService(IDeckProvider deckProvider, IAsyncDelay delay, ITimerService timer)
    {
        _deckProvider = deckProvider;
        _delay = delay;
        _timer = timer;
        _timer.Tick += (_, elapsed) =>
        {
            _stats.TimeElapsed = elapsed;
            StateChanged?.Invoke(this, EventArgs.Empty);
        };
    }

    public IReadOnlyList<Card> Cards => _cards;
    public GameStats Stats => _stats;
    public event EventHandler? StateChanged;

    public void StartNewGame(string username)
    {
        _timer.Stop();
        _timer.Reset();

        _cards.Clear();
        foreach (var c in _deckProvider.CreateShuffledDeck(pairCount: 8))
            _cards.Add(c);

        _stats.Username = username;
        _stats.MoveCount = 0;
        _stats.TimeElapsed = TimeSpan.Zero;

        _first = null;
        _second = null;
        _resolvingPair = false;

        _timer.Start();
        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task FlipCardAsync(int cardId, CancellationToken ct = default)
    {
        if (_resolvingPair) return;

        var card = _cards.FirstOrDefault(c => c.Id == cardId);
        if (card is null) return;
        if (card.IsMatched || card.IsFlipped) return;

        card.IsFlipped = true;
        _stats.MoveCount++; // every flip counts
        StateChanged?.Invoke(this, EventArgs.Empty);

        if (_first is null)
        {
            _first = card;
            return;
        }

        _second = card;
        _resolvingPair = true;

        if (_first.Symbol == _second.Symbol)
        {
            _first.IsMatched = _second.IsMatched = true;
            _first = _second = null;
            _resolvingPair = false;
            StateChanged?.Invoke(this, EventArgs.Empty);
            CheckIfGameOver();
        }
        else
        {
            // brief delay before flipping back
            await _delay.Delay(TimeSpan.FromMilliseconds(700), ct);
            if (!ct.IsCancellationRequested)
            {
                _first.IsFlipped = false;
                _second.IsFlipped = false;
                _first = _second = null;
                _resolvingPair = false;
                StateChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void CheckIfGameOver()
    {
        if (_cards.All(c => c.IsMatched))
        {
            _timer.Stop(); // freeze time on win
        }
    }
}
