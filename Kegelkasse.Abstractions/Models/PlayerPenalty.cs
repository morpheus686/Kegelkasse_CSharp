using System;
using System.Collections.Generic;

namespace Kegelkasse.Base.Models;

public partial class PlayerPenalty
{
    public int Id { get; set; }

    public int GamePlayer { get; set; }

    public int Penalty { get; set; }

    public int Value { get; set; }

    public virtual GamePlayer GamePlayerNavigation { get; set; } = null!;

    public virtual Penalty PenaltyNavigation { get; set; } = null!;
}
