using Microsoft.EntityFrameworkCore;
using TicTacToe.Core.BaseEntities;
using TicTacToe.Core.Entities;
using TicTacToe.Services.Repository.Interfaces;

namespace TicTacToe.Services.Repository;

public class GameRepository : BaseRepository<Game>, IGameRepository
{
    private readonly TicTacToeContext _context;

    public GameRepository(TicTacToeContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Game> MakeMoveAsync(
        Game game,
        Move move,
        CancellationToken token)
    {
        DbSet<Game> gameSet = _context.Games;
        DbSet<Move> moveSet = _context.Moves;

        move.GameUuid = game.Uuid;
        await moveSet.AddAsync(move, token);
        
        // Since .AsNoTracking used in GetAsync 
        var updatedGame = gameSet.Update(game);
        
        await _context.SaveChangesAsync(token);

        return updatedGame.Entity;
    }
}