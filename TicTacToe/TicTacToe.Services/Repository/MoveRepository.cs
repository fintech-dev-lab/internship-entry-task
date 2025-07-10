using Microsoft.EntityFrameworkCore;
using TicTacToe.Core.BaseEntities;
using TicTacToe.Core.Entities;
using TicTacToe.Services.Repository.Interfaces;

namespace TicTacToe.Services.Repository;

public class MoveRepository: BaseRepository<Move>, IMoveRepository
{
    private readonly TicTacToeContext _context;

    public MoveRepository(TicTacToeContext context) : base(context)
    {
        _context = context;
    }
}