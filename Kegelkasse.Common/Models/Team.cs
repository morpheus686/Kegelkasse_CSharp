using System;
using System.Collections.Generic;

namespace Kegelkasse.Common.Models;

public partial class Team
{
    public int Id { get; set; }

    public string TeamName { get; set; } = null!;

    public virtual ICollection<DefaultTeamPlayer> DefaultTeamPlayers { get; set; } = new List<DefaultTeamPlayer>();

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<TeamPenalty> TeamPenalties { get; set; } = new List<TeamPenalty>();
}
