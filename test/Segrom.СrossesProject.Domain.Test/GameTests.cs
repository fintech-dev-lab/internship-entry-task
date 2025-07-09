using Segrom.СrossesProject.Domain.Entities;
using Segrom.СrossesProject.Domain.Enums;
using Segrom.СrossesProject.Domain.Exceptions;
using Segrom.СrossesProject.Domain.ValueObjects;

namespace Segrom.СrossesProject.Domain.Test;

public sealed class GameTests
{

    private readonly Guid _testId = Guid.NewGuid();
    private const uint FIELD_SIZE = 3;
    private const uint WIN_LENGTH = 3;
    
    private Game CreateGame() => new(_testId, FIELD_SIZE, WIN_LENGTH);

    [Fact]
    public void Constructor_InitializesCorrectState()
    {
        // Act
        var game = CreateGame();

        // Assert
        Assert.Equal(_testId, game.Id);
        Assert.Equal(FIELD_SIZE, game.Field.Size);
        Assert.Equal(GameState.Continues, game.State);
        Assert.Null(game.Winner);
        Assert.Equal(PlayerType.XPlayer, game.CurrentPlayer);
        Assert.Equal(WIN_LENGTH, game.LengthForWin);
    }

    [Fact]
    public void Move_OutOfBounds_ThrowsException()
    {
        // Arrange
        var game = CreateGame();
        var invalidPosition = new Position { X = FIELD_SIZE + 1, Y = 0 };

        // Act & Assert
        Assert.Throws<DomainException>(() => game.Move(invalidPosition));
    }

    [Fact]
    public void Move_OccupiedCell_ThrowsException()
    {
        // Arrange
        var game = CreateGame();
        var position = new Position { X = 0, Y = 0 };
        game.Move(position);

        // Act & Assert
        Assert.Throws<DomainException>(() => game.Move(position));
    }

    [Fact]
    public void Move_ValidMove_UpdatesCellAndSwitchesPlayer()
    {
        // Arrange
        var game = CreateGame();
        var position = new Position { X = 1, Y = 1 };

        // Act
        game.Move(position);

        // Assert
        Assert.Equal(CellState.X, game.Field.Cells[position.X, position.Y]);
        Assert.Equal(PlayerType.OPlayer, game.CurrentPlayer);
    }

    [Fact]
    public void Move_WinningHorizontalLine_FinishesGameWithXWinner()
    {
        // Arrange
        var game = CreateGame();
        var moves = new[]
        {
            new Position(0, 0),
            new Position(0, 1),
            new Position(1, 0),
            new Position(1, 1),
            new Position(2, 0) 
        };

        // Act
        foreach (var move in moves)
        {
            game.Move(move);
        }

        // Assert
        Assert.Equal(GameState.Finished, game.State);
        Assert.Equal(WinnerType.XPlayer, game.Winner);
    }

    [Fact]
    public void Move_WinningVerticalLine_FinishesGameWithOWinner()
    {
        // Arrange
        var game = CreateGame();
        var moves = new[]
        {
            new Position(0, 0),
            new Position(1, 0),
            new Position(0, 1),
            new Position(1, 1),
            new Position(2, 2),
            new Position(1, 2) 
        };

        // Act
        foreach (var move in moves)
        {
            game.Move(move);
        }

        // Assert
        Assert.Equal(GameState.Finished, game.State);
        Assert.Equal(WinnerType.OPlayer, game.Winner);
    }

    [Fact]
    public void Move_WinningDiagonal_FinishesGameWithXWinner()
    {
        // Arrange
        var game = CreateGame();
        var moves = new[]
        {
            new Position(0, 0),
            new Position(1, 0),
            new Position(1, 1),
            new Position(0, 1),
            new Position(2, 2) 
        };

        // Act
        foreach (var move in moves)
        {
            game.Move(move);
        }

        // Assert
        Assert.Equal(GameState.Finished, game.State);
        Assert.Equal(WinnerType.XPlayer, game.Winner);
    }

    [Fact]
    public void Move_DrawGame_FinishesWithDraw()
    {
        // Arrange
        var game = CreateGame();
        var moves = new[]
        {
            new Position(0, 0),
            new Position(2, 0),
            new Position(1, 0),
            
            new Position(0, 1),
            new Position(2, 1),
            new Position(1, 1),
            
            new Position(0, 2),
            new Position(2, 2),
            new Position(1, 2) 
        };

        // Act
        foreach (var move in moves)
        {
            game.Move(move);
        }

        // Assert
        Assert.Equal(GameState.Finished, game.State);
        Assert.Equal(WinnerType.Draw, game.Winner);
    }

    [Fact]
    public void Move_AfterGameFinished_ThrowsException()
    {
        // Arrange
        var game = CreateGame();
        var winningMoves = new[]
        {
            new Position(0, 0),
            new Position(0, 1),
            new Position(1, 0),
            new Position(1, 1),
            new Position(2, 0) 
        };

        foreach (var move in winningMoves)
        {
            game.Move(move);
        }

        // Act & Assert
        Assert.Throws<DomainException>(() => game.Move(new Position(2, 2)));
    }
}