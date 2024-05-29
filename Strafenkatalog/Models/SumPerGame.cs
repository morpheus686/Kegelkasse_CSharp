using System;
using System.Collections.Generic;

namespace Strafenkatalog.Models;

public partial class SumPerGame
{
    public int? GameId { get; set; }

    public double? PenaltySum { get; set; }
}
