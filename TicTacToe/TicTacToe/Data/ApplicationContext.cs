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
            //TODO: Сделать индекс по трем колонкам
            //modelBuilder.Entity<Move>().HasIndex(m => m.GameId).IsUnique();
        }
    }
}
