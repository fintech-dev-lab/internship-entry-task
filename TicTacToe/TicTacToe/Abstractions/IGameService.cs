using TicTacToe.Models;
using TicTacToe.ViewModels.Request;
using TicTacToe.ViewModels.Response;

namespace TicTacToe.Abstractions
{
    public interface IGameService
    {
        public Task<GameDto> Create(CreateGameDto options);
        public Task<GameDto> Get(int id);
        public Task<GameDto[]> Get();
        public Task<GameDto> Move(int gameId, CreateMoveDto model);
    }
}
