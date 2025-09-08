using System;
using System.Collections.Generic;

namespace Kegelkasse.Models;

public partial class TeamPenalty
{
    public int Id { get; set; }

    public int Team { get; set; }

    public int Penalty { get; set; }

    public virtual Penalty PenaltyNavigation { get; set; } = null!;

    public virtual Team TeamNavigation { get; set; } = null!;
}
