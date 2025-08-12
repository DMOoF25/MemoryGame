using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryGame.Domain;

namespace MemoryGame.Application;

public sealed class GameEngine
{
    private List<Card> _deck = new();
    private DateTime startTimer;
    private DateTime endTimer;

    public ReadOnlyCollection<Card> Deck => _deck.AsReadOnly();
    public string userName  { get; set; } = string.Empty;

    public int Moves { get; set; }

    public TimeSpan ElapsedTime { get; set; }

    public bool IsRunning { get; set; }

}
