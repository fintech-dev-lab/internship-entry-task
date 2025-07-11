using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Enums;

namespace TicTacToe.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(builder =>
            {
                builder.HasKey(g => g.Id);

                builder.Property(g => g.Board)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                        v => JsonSerializer.Deserialize<Player?[,]>(v, (JsonSerializerOptions?)null)
                    );
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
