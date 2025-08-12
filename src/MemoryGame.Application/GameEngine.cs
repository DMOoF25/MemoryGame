using System.Collections.ObjectModel;
using MemoryGame.Domain;

namespace MemoryGame.Application;

// Represents a position on the game board
public readonly struct Position
{
    public int X { get; }
    public int Y { get; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}


public sealed class GameEngine
{
    private List<Card> _deck = new();
    private DateTime startTimer;
    private DateTime endTimer;

    public ReadOnlyCollection<Card> Deck => _deck.AsReadOnly();
    public string userName { get; set; } = string.Empty;

    public int Moves { get; set; }

    public TimeSpan ElapsedTime { get; set; }

    public bool IsRunning { get; set; }

    // Returns a shuffled deck with positions for a 4x4 grid (8 pairs)
    public Dictionary<Position, Card> ShuffleDeckToGrid()
    {
        // Ensure there are at least 8 unique pairs (16 cards)
        var pairs = _deck
            .GroupBy(card => card.Symbol)
            .Where(g => g.Count() >= 2)
            .Take(8)
            .SelectMany(g => g.Take(2))
            .ToList();

        if (pairs.Count < 16)
            throw new InvalidOperationException("Deck must contain at least 8 pairs (16 cards) for a 4x4 grid.");

        // Shuffle the 16 cards (8 pairs)
        var rng = new Random();
        var shuffled = pairs.OrderBy(_ => rng.Next()).ToList();

        var grid = new Dictionary<Position, Card>(16);
        int index = 0;
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                grid[new Position(x, y)] = shuffled[index++];
            }
        }
        return grid;
    }

    // Starts the game timer
    public void StartTimer()
    {
        startTimer = DateTime.Now;
        IsRunning = true;
    }

    // Stops the game timer and calculates elapsed time
    public void StopTimer()
    {
        if (!IsRunning)
            throw new InvalidOperationException("Game timer is not running.");
        endTimer = DateTime.Now;
        ElapsedTime = endTimer - startTimer;
        IsRunning = false;
    }

    // Resets the game state
    public void ResetGame()
    {
        _deck.Clear();
        Moves = 0;
        ElapsedTime = TimeSpan.Zero;
        IsRunning = false;
        startTimer = DateTime.MinValue;
        endTimer = DateTime.MinValue;
    }

    // Compares two cards by their positions
    public bool CompareCards(Position pos1, Position pos2, Dictionary<Position, Card> grid)
    {
        if (!grid.ContainsKey(pos1) || !grid.ContainsKey(pos2))
            throw new ArgumentException("Invalid card positions provided.");
        var card1 = grid[pos1];
        var card2 = grid[pos2];
        if (card1.IsMatched || card2.IsMatched)
            return false; // Cannot compare matched cards
        Moves++;
        return card1.Symbol == card2.Symbol;
    }

    // End the game and return the final score
    public string EndGame()
    {
        if (IsRunning)
            StopTimer();
        return $"Game result! {userName} made {Moves} moves in {ElapsedTime.TotalSeconds} seconds.";
    }
}
