namespace MemoryGame.Domain.Entities;

/// <summary>
/// Represents a single card in the memory game.
/// </summary>
public class Card
{
    /// <summary>
    /// Gets or sets the unique identifier for the card.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets or sets the symbol displayed on the card.
    /// </summary>
    public string Symbol { get; init; } = "";

    /// <summary>
    /// Gets or sets a value indicating whether the card is currently flipped face up.
    /// </summary>
    public bool IsFlipped { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the card has been matched.
    /// </summary>
    public bool IsMatched { get; set; }
}