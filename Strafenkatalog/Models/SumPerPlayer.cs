using System;
using System.Collections.Generic;

namespace Strafenkatalog.Models;

public partial class SumPerPlayer
{
    public int? GameId { get; set; }

    public DateOnly? Date { get; set; }

    public string? TeamName { get; set; }

    public int? PlayerId { get; set; }

    public string? Name { get; set; }

    public double? PenaltySum { get; set; }

    public int? Full { get; set; }

    public int? Clear { get; set; }

    public int? Errors { get; set; }

    public int? Played { get; set; }

    public int? Result { get; set; }
}
