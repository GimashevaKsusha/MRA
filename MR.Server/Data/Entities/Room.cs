using System;
using System.Collections.Generic;

namespace MR.Server.Data.Entities;

public partial class Room
{
    public int Id { get; set; }

    public Guid Uuid { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Playlist> Playlists { get; } = new List<Playlist>();
}
