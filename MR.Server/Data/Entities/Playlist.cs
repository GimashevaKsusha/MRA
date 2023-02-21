using System;
using System.Collections.Generic;

namespace MR.Server.Data.Entities;

public partial class Playlist
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public int SongId { get; set; }

    public int Number { get; set; }

    public virtual Room Room { get; set; } = null!;

    public virtual Song Song { get; set; } = null!;
}
