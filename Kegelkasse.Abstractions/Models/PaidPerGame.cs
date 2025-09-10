using System;
using System.Collections.Generic;

namespace Kegelkasse.Base.Models;

public partial class PaidPerGame
{
    public int? Game { get; set; }

    public byte[]? Paid { get; set; }
}
