using System;
using System.Collections.Generic;

namespace Kegelkasse.Models;

public partial class GamePlayer
{
    public int Id { get; set; }

    public int Game { get; set; }

    public int Player { get; set; }

    public byte[]? Paid { get; set; }

    public int? Result { get; set; }

    public int? Full { get; set; }

    public int? Clear { get; set; }

    public int? Errors { get; set; }

    public int Played { get; set; }

    public virtual Game GameNavigation { get; set; } = null!;

    public virtual Player PlayerNavigation { get; set; } = null!;

    public virtual ICollection<PlayerPenalty> PlayerPenalties { get; set; } = new List<PlayerPenalty>();
}
