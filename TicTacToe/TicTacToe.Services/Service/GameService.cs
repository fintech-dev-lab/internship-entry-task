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

    private readonly Random _random = new();

    public GameService(
        IGameRepository gameRepository,
        IMoveRepository moveRepository,
        IOptions<GameSettings> settings)
    {
        _gameRepository = gameRepository;
        _moveRepository = moveRepository;
        _settings = settings.Value;
    }

    public async Task<Game> GetGameAsync(Guid uuid, CancellationToken token)
    {
        return await _gameRepository.GetAsync(uuid, token);
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

    public async Task<Game> MakeMoveAsync(
        MakeMoveRequest request,
        CancellationToken token)
    {
        Game game = await _gameRepository.GetAsync(request.GameUuid, token);

        if (game.Winner is not null || game.IsDraw)
            throw new ArgumentException("Game is ended.");

        Move move = new Move
        {
            GameUuid = game.Uuid,
            Row = request.Row,
            Column = request.Column,
            Timestamp = DateTime.UtcNow
        };

        if ((game.Moves.Count + 1) % 3 == 0 && _random.Next(100) < 10)
            move.PlayerUuid = request.PlayerUuid == game.FirstPlayerUuid ? game.SecondPlayerUuid : game.FirstPlayerUuid;
        else
            move.PlayerUuid = request.PlayerUuid;

        string[][] changedBoard = game.Board;
        
        if (move.PlayerUuid == game.FirstPlayerUuid)
            changedBoard[move.Row][move.Column] = "X";
        else
            changedBoard[move.Row][move.Column] = "O";
        
        game.Board = changedBoard;

        game.Moves.Add(move);

        GameResult? gameResult = CheckForWinner(
            game.Board,
            game.FirstPlayerUuid,
            game.SecondPlayerUuid,
            move.Row,
            move.Column);

        if (gameResult is not null)
        {
            switch (gameResult)
            {
                case GameResult.Winner winnerResult:
                    game.WinnerUuid = winnerResult.PlayerUuid;
                    break;
                case GameResult.Draw:
                    game.IsDraw = true;
                    break;
            }
        }

        await _gameRepository.MakeMoveAsync(game, move, token);
        return game;
    }

    private GameResult? CheckForWinner(
        string[][] board,
        Guid firstPlayerUuid,
        Guid secondPlayerUuid,
        int lastMoveRow,
        int lastMoveCol)
    {
        int boardSize = board.Length;
        int winLength = _settings.WinLength;
        string marker = board[lastMoveRow][lastMoveCol];
        if (string.IsNullOrEmpty(marker))
            return null;

        Guid? GetWinnerFromMarker(string marker) => marker switch
        {
            "X" => firstPlayerUuid,
            "O" => secondPlayerUuid,
            _ => null
        };

        bool CheckLine(
            int startRow,
            int startCol,
            int dRow,
            int dCol)
        {
            int count = 0;
            for (int i = 0; i < winLength; i++)
            {
                int r = startRow + i * dRow;
                int c = startCol + i * dCol;

                if (r >= 0 && r < boardSize && c >= 0 && c < boardSize && board[r][c] == marker)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            return count == winLength;
        }

        int[] dRows = { 0, 1, 1, 1 };
        int[] dCols = { 1, 0, 1, -1 };

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < winLength; j++)
            {
                if (CheckLine(
                        lastMoveRow - j * dRows[i],
                        lastMoveCol - j * dCols[i],
                        dRows[i],
                        dCols[i]))
                {
                    Guid? winnerUuid = GetWinnerFromMarker(marker);
                    if (winnerUuid == null)
                        return null;
                    return new GameResult.Winner(winnerUuid.Value);
                }
            }
        }

        if (board.SelectMany(row => row).All(cell => !string.IsNullOrEmpty(cell)))
        {
            return new GameResult.Draw();
        }

        return null;
    }
}