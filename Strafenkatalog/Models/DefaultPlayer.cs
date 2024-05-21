using System;
using System.Collections.Generic;

namespace Strafenkatalog.Models;

public partial class DefaultPlayer
{
    public int? TeamId { get; set; }

    public string? Team { get; set; }

    public int? PlayerId { get; set; }

    public string? Name { get; set; }
}
