using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using MemoryGame.Application.Abstractions;
using MemoryGame.UI.Common;

namespace MemoryGame.UI.ViewModels;

public class GameViewModel : INotifyPropertyChanged
{
    private readonly IGameService _game;
    private readonly DispatcherTimer _timer;

    private string _username = "Player";
    private TimeSpan _elapsed = TimeSpan.Zero;
    private bool _isRunning;

    public GameViewModel(IGameService gameService)
    {
        _game = gameService;

        Cards = new ObservableCollection<CardViewModel>();
        StartCommand = new RelayCommand(_ => _ = StartAsync());

        FlipCommand = new RelayCommand(
            p => _ = FlipAsync((int)p!),
            p => _isRunning && Cards.Any(c => c.Id == (int)p! && !c.IsFlipped));

        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
        _timer.Tick += (_, __) =>
        {
            if (_isRunning) Elapsed = _game.Stats.Elapsed;
        };
    }

    public ObservableCollection<CardViewModel> Cards { get; }
    public RelayCommand StartCommand { get; }
    public RelayCommand FlipCommand { get; }

    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(); }
    }

    public int Moves => _game.Stats.Moves;

    public TimeSpan Elapsed
    {
        get => _elapsed;
        private set { _elapsed = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private async Task StartAsync()
    {
        await _game.StartNewGameAsync(Username);
        Cards.Clear();
        foreach (var c in _game.Cards)
            Cards.Add(new CardViewModel(c));

        _isRunning = true;
        _timer.Start();
        OnPropertyChanged(nameof(Moves));
        Elapsed = TimeSpan.Zero;
    }

    private async Task FlipAsync(int cardId)
    {
        //bool isMatch = false;
        //int? firstId = null, secondId = null;
        var (isMatch, firstId, secondId) = await _game.FlipAsync(cardId);

        // Update all CardViewModels to reflect the current state
        foreach (var vm in Cards)
        {
            var card = _game.Cards.First(c => c.Id == vm.Id);
            vm.IsFlipped = card.IsFlipped;
            vm.IsMatched = card.IsMatched;
        }

        OnPropertyChanged(nameof(Moves));

        // If not a match, show both cards for a moment, then unflip
        if (!isMatch && firstId.HasValue && secondId.HasValue)
        {
            await Task.Delay(700);

            var firstVm = Cards.First(c => c.Id == firstId.Value);
            var secondVm = Cards.First(c => c.Id == secondId.Value);

            firstVm.IsFlipped = false;
            secondVm.IsFlipped = false;
            _game.Lock = false; // Unlock the game for the next move
        }

        if (_game.Stats.IsCompleted)
        {
            _isRunning = false;
            _timer.Stop();
            Elapsed = _game.Stats.Elapsed;
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}