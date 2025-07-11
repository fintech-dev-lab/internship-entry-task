using CrossesAndZeroes.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CrossesAndZeroes.Infrastructure.Data
{
    public class GameDbContext : DbContext
    {
        public DbSet<GameEntity> Games { get; set; }

        public GameDbContext(DbContextOptions<GameDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameEntity>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.BoardJson).IsRequired();
                entity.Property(g => g.CurrentPlayer).IsRequired();
                entity.Property(g => g.State).IsRequired();
            });

        }
    }
}
