using System;
using System.Collections.Generic;

namespace Kegelkasse.Base.Models;

public partial class ResultOfGame
{
    public int? Id { get; set; }

    public int? TotalFull { get; set; }

    public int? TotalClear { get; set; }

    public int? TotalResult { get; set; }

    public int? TotalErrors { get; set; }
}
