using System;
using System.Collections.Generic;

namespace MR.Server.Data.Entities;

public partial class Song
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public TimeOnly Length { get; set; }

    public virtual ICollection<Playlist> Playlists { get; } = new List<Playlist>();
}
