using Microsoft.EntityFrameworkCore;
using TicTacToe.Core.Entities;
using TicTacToe.Services.Repository;

namespace TicTacToe.IntegrationTests;

public class GameRepositoryTests : IAsyncLifetime
{
    private TicTacToeContext _context;
    private GameRepository _repository;
    private DbContextOptions<TicTacToeContext> _options;

    private readonly Guid _gameId = Guid.NewGuid();
    private readonly Guid _player1Id = Guid.NewGuid();
    private readonly Guid _player2Id = Guid.NewGuid();
    private readonly Guid _moveId = Guid.NewGuid();

    public async Task InitializeAsync()
    {
        _options = new DbContextOptionsBuilder<TicTacToeContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TicTacToeContext(_options);
        await _context.Database.EnsureCreatedAsync();

        _repository = new GameRepository(_context);

        var player1 = new User { Uuid = _player1Id, FullName = "Player One" };
        var player2 = new User { Uuid = _player2Id, FullName = "Player Two" };

        var game = new Game
        {
            Uuid = _gameId,
            FirstPlayerUuid = _player1Id,
            SecondPlayerUuid = _player2Id,
            Board = new string[][] { new[] { "", "", "" }, new[] { "", "", "" }, new[] { "", "", "" } },
            Moves = new List<Move>()
        };

        await _context.Users.AddRangeAsync(player1, player2);
        await _context.Games.AddAsync(game);
        await _context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
    }

    [Fact]
    public async Task MakeMoveAsync_ShouldAddMoveAndUpdateGame()
    {
        // Arrange
        var move = new Move
        {
            Uuid = _moveId,
            GameUuid = _gameId,
            PlayerUuid = _player1Id,
            Row = 0,
            Column = 0,
            Timestamp = DateTime.UtcNow
        };

        var game = await _context.Games.FindAsync(_gameId);
        var changedBoard = game.Board;
        changedBoard[0][0] = "X";
        game.Board = changedBoard;

        // Act
        await _repository.MakeMoveAsync(game, move, CancellationToken.None);

        // Assert
        using var assertContext = new TicTacToeContext(_options);
        var savedMove = await assertContext.Moves.FindAsync(_moveId);
        var updatedGame = await assertContext.Games.FindAsync(_gameId);

        Assert.NotNull(savedMove);
        Assert.Equal(_player1Id, savedMove.PlayerUuid);
        Assert.Equal("X", updatedGame.Board[0][0]);
    }
}

