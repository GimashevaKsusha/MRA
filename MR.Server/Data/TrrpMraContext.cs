using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Json;
using MR.Server.Data.Entities;
using System.Data.Common;

namespace MR.Server.Data;

public partial class TrrpMraContext : DbContext
{
    public TrrpMraContext() { }

    public TrrpMraContext(DbContextOptions<TrrpMraContext> options) : base(options) { }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Song> Songs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
            var connectionString = configuration.GetConnectionString("DBConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("playlists_pkey");

            entity.ToTable("playlists");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.SongId).HasColumnName("song_id");

            entity.HasOne(d => d.Room).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("playlists_rooms");

            entity.HasOne(d => d.Song).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.SongId)
                .HasConstraintName("playlists_songs");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rooms_pkey");

            entity.ToTable("rooms");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Uuid).HasColumnName("uuid");
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("songs_pkey");

            entity.ToTable("songs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Length).HasColumnName("length");
            entity.Property(e => e.Title).HasColumnName("title");
        });
    }
}
