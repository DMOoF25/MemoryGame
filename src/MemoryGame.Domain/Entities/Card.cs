namespace MemoryGame.Domain.Entities;

public class Card
{
    public int Id { get; init; }
    public string Symbol { get; init; } = "";
    public bool IsFlipped { get; set; }
    public bool IsMatched { get; set; }
}