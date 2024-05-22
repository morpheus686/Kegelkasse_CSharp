using System;
using System.Collections.Generic;

namespace Strafenkatalog.Models;

public partial class Game
{
    public int Id { get; set; }

    public int Team { get; set; }

    public DateOnly Date { get; set; }

    public string? Vs { get; set; }

    public int? Gameday { get; set; }

    public virtual ICollection<GamePlayer> GamePlayers { get; set; } = new List<GamePlayer>();

    public virtual Team TeamNavigation { get; set; } = null!;
}
