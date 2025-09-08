using System;
using System.Collections.Generic;

namespace Kegelkasse.Models;

public partial class DefaultTeamPlayer
{
    public int Id { get; set; }

    public int Player { get; set; }

    public int Team { get; set; }

    public virtual Player PlayerNavigation { get; set; } = null!;

    public virtual Team TeamNavigation { get; set; } = null!;
}
