using System;
using System.Collections.Generic;

namespace Kegelkasse.Common.Models;

public partial class PenaltyKind
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Penalty> Penalties { get; set; } = new List<Penalty>();
}
