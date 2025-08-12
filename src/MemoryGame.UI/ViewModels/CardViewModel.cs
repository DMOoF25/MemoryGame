using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MemoryGame.UI.ViewModels;

public class CardViewModel : INotifyPropertyChanged
{
    private int _id;
    private string _symbol = "";
    private bool _isFlipped;
    private bool _isMatched;

    public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }
    public string Symbol { get => _symbol; set { _symbol = value; OnPropertyChanged(); } }
    public bool IsFlipped { get => _isFlipped; set { _isFlipped = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsInteractive)); } }
    public bool IsMatched { get => _isMatched; set { _isMatched = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsInteractive)); } }

    public bool IsInteractive => !IsMatched && !IsFlipped;

    public ICommand? FlipCommand { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
