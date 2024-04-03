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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
