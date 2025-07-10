using Microsoft.EntityFrameworkCore;
using TicTacToe.Data.Entites;

namespace TicTacToe.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Game> Games => Set<Game>();
        public DbSet<Move> Moves => Set<Move>();


        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().
                HasMany(g => g.Moves)
                .WithOne(m => m.Game).
                OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Move>().HasIndex(m => new { m.GameId, m.Column, m.Row }).IsUnique();
        }
    }
}
