using Microsoft.EntityFrameworkCore;
using TicTacToe.Core.BaseEntities;
using TicTacToe.Core.Entities;
using TicTacToe.Services.Repository.Interfaces;

namespace TicTacToe.Services.Repository;

public class UserRepository: BaseRepository<User>, IUserRepository
{
    private readonly TicTacToeContext _context;

    public UserRepository(TicTacToeContext context) : base(context)
    {
        _context = context;
    }
}