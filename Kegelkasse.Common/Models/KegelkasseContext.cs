using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Kegelkasse.Common.Models;

public partial class KegelkasseContext : DbContext
{
    public KegelkasseContext(DbContextOptions<KegelkasseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DefaultPlayer> DefaultPlayers { get; set; }

    public virtual DbSet<DefaultTeamPlayer> DefaultTeamPlayers { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GamePlayer> GamePlayers { get; set; }

    public virtual DbSet<PaidPerGame> PaidPerGames { get; set; }

    public virtual DbSet<Penalty> Penalties { get; set; }

    public virtual DbSet<PenaltyKind> PenaltyKinds { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<PlayerPenalty> PlayerPenalties { get; set; }

    public virtual DbSet<PlayersOfGame> PlayersOfGames { get; set; }

    public virtual DbSet<ResultOfGame> ResultOfGames { get; set; }

    public virtual DbSet<Season> Seasons { get; set; }

    public virtual DbSet<SumPerGame> SumPerGames { get; set; }

    public virtual DbSet<SumPerPlayer> SumPerPlayers { get; set; }

    public virtual DbSet<SumPerTeam> SumPerTeams { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamPenalty> TeamPenalties { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DefaultPlayer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("DefaultPlayers");

            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
        });

        modelBuilder.Entity<DefaultTeamPlayer>(entity =>
        {
            entity.ToTable("DefaultTeamPlayer");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.PlayerNavigation).WithMany(p => p.DefaultTeamPlayers)
                .HasForeignKey(d => d.Player)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.TeamNavigation).WithMany(p => p.DefaultTeamPlayers)
                .HasForeignKey(d => d.Team)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.ToTable("Game");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.Season).WithMany(p => p.Games)
                .HasForeignKey(d => d.SeasonId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.TeamNavigation).WithMany(p => p.Games).HasForeignKey(d => d.Team);
        });

        modelBuilder.Entity<GamePlayer>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Paid).HasColumnType("NUMERIC (2, 2)");

            entity.HasOne(d => d.GameNavigation).WithMany(p => p.GamePlayers).HasForeignKey(d => d.Game);

            entity.HasOne(d => d.PlayerNavigation).WithMany(p => p.GamePlayers)
                .HasForeignKey(d => d.Player)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PaidPerGame>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("PaidPerGame");
        });

        modelBuilder.Entity<Penalty>(entity =>
        {
            entity.ToTable("Penalty");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GetsValueByParent).HasDefaultValue(0);
            entity.Property(e => e.Penalty1)
                .HasColumnType("NUMERIC (2, 2)")
                .HasColumnName("penalty");

            entity.HasOne(d => d.PenaltyType).WithMany(p => p.Penalties)
                .HasForeignKey(d => d.PenaltyTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PenaltyKind>(entity =>
        {
            entity.ToTable("PenaltyKind");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.ToTable("Player");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<PlayerPenalty>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.GamePlayerNavigation).WithMany(p => p.PlayerPenalties).HasForeignKey(d => d.GamePlayer);

            entity.HasOne(d => d.PenaltyNavigation).WithMany(p => p.PlayerPenalties)
                .HasForeignKey(d => d.Penalty)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PlayersOfGame>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("PlayersOfGame");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<ResultOfGame>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ResultOfGame");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<Season>(entity =>
        {
            entity.HasIndex(e => e.Description, "IX_Seasons_Description").IsUnique();
        });

        modelBuilder.Entity<SumPerGame>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SumPerGame");
        });

        modelBuilder.Entity<SumPerPlayer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SumPerPlayer");
        });

        modelBuilder.Entity<SumPerTeam>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SumPerTeam");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.ToTable("Team");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<TeamPenalty>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.PenaltyNavigation).WithMany(p => p.TeamPenalties)
                .HasForeignKey(d => d.Penalty)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.TeamNavigation).WithMany(p => p.TeamPenalties)
                .HasForeignKey(d => d.Team)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
