using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MemoryGame.Application.Abstractions;
using MemoryGame.UI.Commands;

namespace MemoryGame.UI.ViewModels;

public class GameViewModel : INotifyPropertyChanged
{
    private readonly IGameService _gameService;
    private bool _busy;

    public ObservableCollection<CardViewModel> Cards { get; } = new();
    public string Username { get; set; } = "Player";
    public int Moves => _gameService.Stats.MoveCount;
    public TimeSpan TimeElapsed => _gameService.Stats.TimeElapsed;

    public RelayCommand StartGameCommand { get; }
    public RelayCommand FlipBlockedCommand { get; } // no-op when busy resolving

    public GameViewModel(IGameService gameService)
    {
        _gameService = gameService;
        _gameService.StateChanged += (_, __) => RefreshFromService();

        StartGameCommand = new RelayCommand(StartGame);
        FlipBlockedCommand = new RelayCommand(() => { });

        StartGame(); // optional auto-start
    }

    private void StartGame()
    {
        _gameService.StartNewGame(Username);
        Cards.Clear();
        foreach (var c in _gameService.Cards)
        {
            var vm = new CardViewModel
            {
                Id = c.Id,
                Symbol = c.Symbol
            };
            vm.FlipCommand = new RelayCommand(async () => await Flip(vm));
            Cards.Add(vm);
        }
        OnPropertyChanged(nameof(Moves));
        OnPropertyChanged(nameof(TimeElapsed));
        RefreshFromService(); // sync initial state
    }

    private async Task Flip(CardViewModel vm)
    {
        if (_busy) return;
        _busy = true;
        try
        {
            await _gameService.FlipCardAsync(vm.Id);
        }
        finally
        {
            _busy = false;
        }
    }

    private void RefreshFromService()
    {
        // update card VMs
        foreach (var vm in Cards)
        {
            var card = _gameService.Cards[vm.Id];
            vm.IsFlipped = card.IsFlipped;
            vm.IsMatched = card.IsMatched;
            vm.Symbol = card.Symbol;
        }
        OnPropertyChanged(nameof(Moves));
        OnPropertyChanged(nameof(TimeElapsed));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
