using System;
using System.Collections.Generic;

namespace Kegelkasse.Base.Models;

public partial class PlayersOfGame
{
    public int? Id { get; set; }

    public DateOnly? Date { get; set; }

    public string? Name { get; set; }
}
