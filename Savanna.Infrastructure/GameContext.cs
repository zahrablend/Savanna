using Common.Entities;
using Common.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Savanna.Infrastructure;

public class GameContext : 
    IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public GameContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<GameEntity> Games { get; set; }

    public DbSet<AnimalEntity> Animals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GameEntity>()
        .HasMany(g => g.Animals) // One-to-many relationship
        .WithOne(a => a.Game) // Each animal is associated with one game
        .HasForeignKey(a => a.GameId); // The foreign key property
    }
}
