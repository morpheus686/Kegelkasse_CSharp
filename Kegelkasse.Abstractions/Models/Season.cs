using System;
using System.Collections.Generic;

namespace Kegelkasse.Base.Models;

public partial class Season
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
