using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Domain;

public class GameStats
{
    public string PlayerName { get; init; }
    public int Moves { get; init; }
    public TimeSpan GameTime { get; init; }
    public DateTime CompletedAt { get; init; }

}
