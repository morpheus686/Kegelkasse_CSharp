using System;
using System.Collections.Generic;

namespace Kegelkasse.Base.Models;

public partial class Player
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DefaultTeamPlayer> DefaultTeamPlayers { get; set; } = new List<DefaultTeamPlayer>();

    public virtual ICollection<GamePlayer> GamePlayers { get; set; } = new List<GamePlayer>();
}
