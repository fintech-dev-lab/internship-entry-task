using Microsoft.EntityFrameworkCore;
using TicTacToe.Core.BaseEntities;
using TicTacToe.Core.Entities;
using TicTacToe.Services.Repository.Interfaces;

namespace TicTacToe.Services.Repository;

public class GameRepository: BaseRepository<Game>, IGameRepository
{
    private readonly TicTacToeContext _context;

    public GameRepository(TicTacToeContext context) : base(context)
    {
        _context = context;
    }
}