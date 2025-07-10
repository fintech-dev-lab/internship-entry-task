using Microsoft.Extensions.Options;
using TicTacToe.Contracts.DTO;
using TicTacToe.Core.Entities;
using TicTacToe.Services.Repository.Interfaces;
using TicTacToe.Services.Service.Interfaces;

namespace TicTacToe.Services.Service;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;

    private readonly IMoveRepository _moveRepository;

    private readonly GameSettings _settings;

    public GameService(
        IGameRepository gameRepository,
        IMoveRepository moveRepository,
        IOptions<GameSettings> settings)
    {
        _gameRepository = gameRepository;
        _moveRepository = moveRepository;
        _settings = settings.Value;
    }

    public async Task<Game> CreateGameAsync(
        CreateGameRequest request,
        CancellationToken token)
    {
        string[][] board = new string[_settings.BoardSize][]; 

        for (int i = 0; i < _settings.BoardSize; i++)
        {
            board[i] = new string[_settings.BoardSize]; 
            for (int j = 0; j < _settings.BoardSize; j++)
            {
                board[i][j] = string.Empty;
            }
        }
        
        var game = new Game()
        {
            FirstPlayerUuid = request.FirstPlayerUuid,
            SecondPlayerUuid = request.SecondPlayerUuid,
            Board = board,
            Moves = [],
        };

        return await _gameRepository.CreateAsync(game, token);
    }

    public Task<Game> MakeMoveAsync(
        MakeMoveRequest request,
        CancellationToken token)
    {
        throw new NotImplementedException();
    }
}