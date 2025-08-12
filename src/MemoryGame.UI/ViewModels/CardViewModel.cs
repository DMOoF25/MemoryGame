using System.ComponentModel;
using System.Runtime.CompilerServices;
using MemoryGame.Domain.Entities;

namespace MemoryGame.UI.ViewModels;

public class CardViewModel : INotifyPropertyChanged
{
    private readonly Card _card;
    public CardViewModel(Card card) => _card = card;

    public int Id => _card.Id;
    public string Symbol => _card.Symbol;

    public bool IsFlipped
    {
        get => _card.IsFlipped;
        set { if (_card.IsFlipped != value) { _card.IsFlipped = value; OnPropertyChanged(); } }
    }

    public bool IsMatched
    {
        get => _card.IsMatched;
        set { if (_card.IsMatched != value) { _card.IsMatched = value; OnPropertyChanged(); } }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}