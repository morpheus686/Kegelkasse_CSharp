using System;
using System.Collections.Generic;

namespace Kegelkasse.Common.Models;

public partial class SumPerGame
{
    public int? GameId { get; set; }

    public double? PenaltySum { get; set; }
}
