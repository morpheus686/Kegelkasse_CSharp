﻿using System;
using System.Collections.Generic;

namespace Strafenkatalog.Models;

public partial class SumPerGame
{
    public string? Date { get; set; }

    public string? TeamName { get; set; }

    public double? PenaltySum { get; set; }
}
