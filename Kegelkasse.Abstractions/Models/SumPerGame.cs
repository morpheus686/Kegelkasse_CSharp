using System;
using System.Collections.Generic;

namespace Kegelkasse.Base.Models;

public partial class SumPerGame
{
    public int? GameId { get; set; }

    public double? PenaltySum { get; set; }
}
