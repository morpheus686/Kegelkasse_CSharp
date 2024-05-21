using System;
using System.Collections.Generic;

namespace Strafenkatalog.Models;

public partial class Penalty
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public int Type { get; set; }

    public int Penalty1 { get; set; }

    public int? LowerLimit { get; set; }

    public int? UpperLimit { get; set; }

    public virtual ICollection<PlayerPenalty> PlayerPenalties { get; set; } = new List<PlayerPenalty>();

    public virtual ICollection<TeamPenalty> TeamPenalties { get; set; } = new List<TeamPenalty>();

    public virtual PenaltyKind TypeNavigation { get; set; } = null!;
}
