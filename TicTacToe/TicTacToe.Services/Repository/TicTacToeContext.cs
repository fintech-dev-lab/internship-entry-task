using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TicTacToe.Core.Entities;

namespace TicTacToe.Services.Repository;

public class TicTacToeContext: DbContext
{
    public DbSet<Game> Games { get; set; }
    
    public DbSet<Move> Moves { get; set; }
    
    public DbSet<User> Users { get; set; }
    
    public TicTacToeContext(DbContextOptions<TicTacToeContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}