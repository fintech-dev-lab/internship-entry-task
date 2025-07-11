using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(builder =>
            {
                builder.HasKey(g => g.Id);

                builder.Property(g => g.BoardAsJson).HasColumnType("jsonb");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
