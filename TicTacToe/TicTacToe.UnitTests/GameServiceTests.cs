using Microsoft.Extensions.Options;
using Moq;
using TicTacToe.Contracts.DTO;
using TicTacToe.Core.Entities;
using TicTacToe.Core.Interfaces;
using TicTacToe.Services.Repository;
using TicTacToe.Services.Repository.Interfaces;
using TicTacToe.Services.Service;

namespace TicTacToe.UnitTests;

// TODO: What to do then 10% chance hits
public class GameServiceTests
{
    private readonly Mock<IGameRepository> _gameRepositoryMock = new();
    private readonly Mock<IMoveRepository> _moveRepositoryMock = new();
    private readonly GameSettings _settings = new() { BoardSize = 3, WinLength = 3 };
    private readonly GameService _service;

    public GameServiceTests()
    {
        var mockRandom = new Mock<IRandomProvider>();
        mockRandom.Setup(r => r.Next(It.IsAny<int>())).Returns(42);

        _service = new GameService(
            _gameRepositoryMock.Object,
            _moveRepositoryMock.Object,
            Options.Create(_settings),
            mockRandom.Object);
    }

    [Fact]
    public async Task CreateGameAsync_CreatesEmptyGameWithCorrectPlayers()
    {
        // Arrange
        var req = new CreateGameRequest
        {
            FirstPlayerUuid = Guid.NewGuid(),
            SecondPlayerUuid = Guid.NewGuid()
        };

        _gameRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Game g, CancellationToken _) => g);

        // Act
        var result = await _service.CreateGameAsync(req, CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Board.Length);
        Assert.Equal(req.FirstPlayerUuid, result.FirstPlayerUuid);
        Assert.Equal(req.SecondPlayerUuid, result.SecondPlayerUuid);
        Assert.All(result.Board, row => Assert.All(row, cell => Assert.Equal(string.Empty, cell)));
    }

    [Fact]
    public async Task MakeMoveAsync_AddsMove_UpdatesBoard_ReturnsGame()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var player1 = Guid.NewGuid();
        var player2 = Guid.NewGuid();
        var board = new[]
        {
            new[] { "", "", "" },
            new[] { "", "", "" },
            new[] { "", "", "" },
        };

        var game = new Game
        {
            Uuid = gameId,
            FirstPlayerUuid = player1,
            SecondPlayerUuid = player2,
            Board = board,
            Moves = new List<Move>()
        };

        var moveRequest = new MakeMoveRequest
        {
            GameUuid = gameId,
            PlayerUuid = player1,
            Row = 1,
            Column = 1
        };

        _gameRepositoryMock.Setup(r => r.GetAsync(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        _gameRepositoryMock.Setup(r => r.MakeMoveAsync(game, It.IsAny<Move>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        // Act
        var result = await _service.MakeMoveAsync(moveRequest, CancellationToken.None);

        // Assert
        Assert.Single(result.Moves);
        Assert.Equal("X", result.Board[1][1]);
    }

    [Fact]
    public async Task MakeMoveAsync_ReturnsGameWithoutChanges_WhenMoveAlreadyExists()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var moveId = Guid.NewGuid();
        var player1 = Guid.NewGuid();
        var board = new[]
        {
            new[] { "X", "", "" },
            new[] { "", "", "" },
            new[] { "", "", "" },
        };

        var existingGame = new Game
        {
            Uuid = gameId,
            FirstPlayerUuid = player1,
            Board = board,
            Moves = new List<Move>()
        };

        var existingMove = new Move { Uuid = moveId };

        var moveRequest = new MakeMoveRequest
        {
            GameUuid = gameId,
            MoveUuid = moveId,
            PlayerUuid = player1,
            Row = 1,
            Column = 1
        };

        _gameRepositoryMock.Setup(r => r.GetAsync(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingGame);

        _moveRepositoryMock.Setup(r => r.GetAsync(moveId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingMove);

        // Act
        var result = await _service.MakeMoveAsync(moveRequest, CancellationToken.None);

        // Assert
        var occupiedCells = result.Board
            .SelectMany(row => row)
            .Count(cell => !string.IsNullOrEmpty(cell));
        
        Assert.Equal(1, occupiedCells);
        Assert.Empty(result.Moves);
    }

    [Fact]
    public async Task MakeMoveAsync_RegistersDraw()
    {
        // Arrange
        var player1 = Guid.NewGuid();
        var player2 = Guid.NewGuid();
        var board = new[]
        {
            new[] { "X", "O", "X" },
            new[] { "X", "O", "O" },
            new[] { "O", "X", "" }
        };

        var game = new Game
        {
            FirstPlayerUuid = player1,
            SecondPlayerUuid = player2,
            Board = board,
            Moves = new List<Move> { new(), new(), new(), new(), new(), new(), new(), new() }
        };

        _gameRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        _gameRepositoryMock.Setup(r => r.MakeMoveAsync(game, It.IsAny<Move>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        var request = new MakeMoveRequest
        {
            PlayerUuid = player1,
            Row = 2,
            Column = 2,
            GameUuid = game.Uuid
        };

        // Act
        var result = await _service.MakeMoveAsync(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsDraw);
    }

    [Fact]
    public async Task MakeMoveAsync_DetectsWin_Horizontal()
    {
        // Arrange
        var player1 = Guid.NewGuid();
        var player2 = Guid.NewGuid();
        var board = new[]
        {
            new[] { "X", "X", "" },
            new[] { "O", "O", "" },
            new[] { "", "", "" }
        };

        var game = new Game
        {
            FirstPlayerUuid = player1,
            SecondPlayerUuid = player2,
            Board = board,
            Moves = new List<Move> { new(), new() }
        };

        _gameRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        _gameRepositoryMock.Setup(r => r.MakeMoveAsync(game, It.IsAny<Move>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        var request = new MakeMoveRequest
        {
            PlayerUuid = player1,
            Row = 0,
            Column = 2,
            GameUuid = game.Uuid
        };

        // Act
        var result = await _service.MakeMoveAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(player1, result.WinnerUuid);
    }

    [Fact]
    public async Task MakeMoveAsync_DetectsWin_Vertical()
    {
        // Arrange
        var player1 = Guid.NewGuid();
        var player2 = Guid.NewGuid();
        var board = new[]
        {
            new[] { "X", "O", "" },
            new[] { "X", "O", "" },
            new[] { "", "", "" }
        };

        var game = new Game
        {
            FirstPlayerUuid = player1,
            SecondPlayerUuid = player2,
            Board = board,
            Moves = new List<Move> { new(), new() }
        };

        _gameRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        var request = new MakeMoveRequest
        {
            PlayerUuid = player1,
            Row = 2,
            Column = 0,
            GameUuid = game.Uuid
        };

        // Act
        var result = await _service.MakeMoveAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(player1, result.WinnerUuid);
    }

    [Fact]
    public async Task MakeMoveAsync_DetectsWin_Diagonal()
    {
        // Arrange
        var player1 = Guid.NewGuid();
        var player2 = Guid.NewGuid();
        var board = new[]
        {
            new[] { "X", "O", "" },
            new[] { "O", "X", "" },
            new[] { "", "", "" }
        };

        var game = new Game
        {
            FirstPlayerUuid = player1,
            SecondPlayerUuid = player2,
            Board = board,
            Moves = new List<Move> { new(), new() }
        };

        _gameRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        var request = new MakeMoveRequest
        {
            PlayerUuid = player1,
            Row = 2,
            Column = 2,
            GameUuid = game.Uuid
        };

        // Act
        var result = await _service.MakeMoveAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(player1, result.WinnerUuid);
    }

    [Fact]
    public async Task MakeMoveAsync_DetectsWin_AntiDiagonal()
    {
        // Arrange
        var player1 = Guid.NewGuid();
        var player2 = Guid.NewGuid();
        var board = new[]
        {
            new[] { "", "O", "X" },
            new[] { "O", "X", "" },
            new[] { "", "", "" }
        };

        var game = new Game
        {
            FirstPlayerUuid = player1,
            SecondPlayerUuid = player2,
            Board = board,
            Moves = new List<Move> { new(), new() }
        };

        _gameRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        var request = new MakeMoveRequest
        {
            PlayerUuid = player1,
            Row = 2,
            Column = 0,
            GameUuid = game.Uuid
        };

        // Act
        var result = await _service.MakeMoveAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(player1, result.WinnerUuid);
    }

    [Fact]
    public async Task MakeMoveAsync_NoWin_ReturnsNotWin()
    {
        // Arrange
        var player1 = Guid.NewGuid();
        var player2 = Guid.NewGuid();
        var board = new[]
        {
            new[] { "X", "O", "" },
            new[] { "", "", "" },
            new[] { "", "", "" }
        };

        var game = new Game
        {
            FirstPlayerUuid = player1,
            SecondPlayerUuid = player2,
            Board = board,
            Moves = new List<Move> { new() }
        };

        _gameRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        var request = new MakeMoveRequest
        {
            PlayerUuid = player2,
            Row = 1,
            Column = 1,
            GameUuid = game.Uuid
        };

        // Act
        var result = await _service.MakeMoveAsync(request, CancellationToken.None);

        // Assert
        Assert.Null(result.WinnerUuid);
    }
}