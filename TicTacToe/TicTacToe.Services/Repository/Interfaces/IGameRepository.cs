using TicTacToe.Core.Entities;
using TicTacToe.Core.Interfaces;

namespace TicTacToe.Services.Repository.Interfaces;

public interface IGameRepository: IRepository<Game>
{
    Task<Game> MakeMoveAsync(Game game, Move move, CancellationToken token);
}