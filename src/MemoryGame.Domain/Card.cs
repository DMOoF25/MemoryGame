using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Domain; 

public class Card
{
    public int Id { get; init; }
    public string Symbol { get; init; }
    public bool IsFlipped { get; init; }
    public bool IsMatched { get; init; }

}
