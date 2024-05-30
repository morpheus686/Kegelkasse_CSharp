using System;
using System.Collections.Generic;

namespace Strafenkatalog.Models;

public partial class PaidPerGame
{
    public int? Game { get; set; }

    public byte[]? Paid { get; set; }
}
