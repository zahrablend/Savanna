using Common.Entities;
using Common.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Savanna.Infrastructure
{
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
            modelBuilder.Entity<GameEntity>().ToTable("Games");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Connection String Here");
        }
    }
}
