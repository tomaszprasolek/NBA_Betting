using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NBA_Betting.Data.Entities;

namespace NBA_Betting.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<League> Leagues { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Bet> Bets { get; set; }
    public DbSet<LeagueMembership> LeagueMemberships { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        ConfigLeagueEntity(builder);

        // Team configuration
        builder.Entity<Team>(entity =>
        {
            entity.HasIndex(e => e.TeamName).IsUnique();
        });

        // Match configuration
        builder.Entity<Match>(entity =>
        {
            entity.HasIndex(e => e.LeagueId);
            entity.HasIndex(e => e.MatchDateTime);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.ModifiedDate).HasDefaultValueSql("GETDATE()");

            entity.HasOne(m => m.HomeTeam)
                .WithMany(t => t.HomeMatches)
                .HasForeignKey(m => m.HomeTeamId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(m => m.AwayTeam)
                .WithMany(t => t.AwayMatches)
                .HasForeignKey(m => m.AwayTeamId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // Bet configuration
        builder.Entity<Bet>(entity =>
        {
            entity.HasIndex(e => e.MatchId);
            entity.HasIndex(e => e.UserId);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.PointsAwarded).HasDefaultValue(0);
        });

        // LeagueMembership configuration
        builder.Entity<LeagueMembership>(entity =>
        {
            entity.HasKey(e => e.MembershipId); // Add this line
            entity.HasIndex(e => new { e.UserId, e.LeagueId }).IsUnique();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");

            // Configure LeagueMembership relationships
            entity
                .HasOne(lm => lm.League)
                .WithMany(l => l.Memberships)
                .HasForeignKey(lm => lm.LeagueId)
                .OnDelete(DeleteBehavior.NoAction); // Changed from Cascade

            entity
                .HasOne(lm => lm.User)
                .WithMany()
                .HasForeignKey(lm => lm.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Changed from Cascade

            // Add unique index on UserId and LeagueId
            entity
                .HasIndex(lm => new { lm.UserId, lm.LeagueId })
                .IsUnique();
        });
    }

    private static void ConfigLeagueEntity(ModelBuilder builder)
    {
        // League configuration
        builder.Entity<League>(entity =>
        {
            entity.HasIndex(e => e.LeagueCode).IsUnique();
            entity.HasIndex(e => e.LeagueName).IsUnique();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.ModifiedDate).HasDefaultValueSql("GETDATE()");

            // Add this configuration to prevent cascade delete from Owner
            entity.HasOne(l => l.Owner)
                .WithMany()
                .HasForeignKey(l => l.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
