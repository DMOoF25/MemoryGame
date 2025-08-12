namespace MemoryGame.Domain.Entities;

public class Card
{
    public int Id { get; set; }
    public string Symbol { get; set; } = "";
    public bool IsFlipped { get; set; }
    public bool IsMatched { get; set; }
}
