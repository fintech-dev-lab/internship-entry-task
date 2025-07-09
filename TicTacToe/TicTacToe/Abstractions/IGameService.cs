using TicTacToe.Models;
using TicTacToe.ViewModels.Response;

namespace TicTacToe.Abstractions
{
    public interface IGameService
    {
        public Task<GameDto> Create(GameOptions options);
    }
}
