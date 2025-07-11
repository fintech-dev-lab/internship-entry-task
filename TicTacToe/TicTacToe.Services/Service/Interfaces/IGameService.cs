using TicTacToe.Contracts.DTO;
using TicTacToe.Core.Entities;
using TicTacToe.Services.Repository.Interfaces;

namespace TicTacToe.Services.Service.Interfaces;

public interface IGameService
{
    Task<Game> GetGameAsync(
        Guid uuid,
        CancellationToken token);
    
    Task<Game> CreateGameAsync(
        CreateGameRequest request,
        CancellationToken token);
    
    Task<Game> MakeMoveAsync(
        MakeMoveRequest request, 
        CancellationToken token);
}