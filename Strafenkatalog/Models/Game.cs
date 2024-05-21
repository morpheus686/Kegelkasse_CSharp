using System;
using System.Collections.Generic;

namespace Strafenkatalog.Models;

public partial class Game
{
    public int Id { get; set; }

    public int Team { get; set; }

    public string Date { get; set; } = null!;

    public virtual ICollection<GamePlayer> GamePlayers { get; set; } = new List<GamePlayer>();

    public virtual Team TeamNavigation { get; set; } = null!;
}
