using System;
using System.Collections.Generic;

namespace Kegelkasse.Base.Models;

public partial class Penalty
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public int PenaltyTypeId { get; set; }

    public double Penalty1 { get; set; }

    public int? LowerLimit { get; set; }

    public int? UpperLimit { get; set; }

    public int? GetsValueByParent { get; set; }

    public virtual PenaltyKind PenaltyType { get; set; } = null!;

    public virtual ICollection<PlayerPenalty> PlayerPenalties { get; set; } = new List<PlayerPenalty>();

    public virtual ICollection<TeamPenalty> TeamPenalties { get; set; } = new List<TeamPenalty>();
}
