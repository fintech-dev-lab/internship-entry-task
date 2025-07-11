using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Enums;

namespace TicTacToe.Domain.Tests;

public class GameTests
{
    private readonly Func<int, bool> _isNotLuckyMove = _ => false;
    private readonly Func<int, bool> _isAlwaysLuckyMove = _ => true;

    [Fact]
    public void Constructor_ShouldCreateGame_WithCorrectInitialState()
    {
        // Arrange & Act
        var game = new Game(Guid.NewGuid(), 3, 3);

        // Assert
        Assert.Equal(3, game.BoardSize);
        Assert.Equal(GameStatus.InProgress, game.Status);
        Assert.Equal(Player.X, game.CurrentPlayer);
        Assert.Equal(1, game.MoveNumber);
    }

    [Fact]
    public void MakeMove_ShouldUpdateBoardAndSwitchPlayer_OnValidMove()
    {
        // Arrange
        var game = new Game(Guid.NewGuid(), 3, 3);

        // Act
        game.MakeMove(0, 0, _isNotLuckyMove);

        // Assert
        var board = game.GetBoard();
        Assert.Equal(Player.X, board[0, 0]);
        Assert.Equal(Player.O, game.CurrentPlayer);
        Assert.Equal(2, game.MoveNumber);
    }

    [Fact]
    public void MakeMove_ShouldSetOpponentSymbol_WhenMoveIsLucky()
    {
        // Arrange
        var game = new Game(Guid.NewGuid(), 3, 3);
        game.MakeMove(0, 0, _isNotLuckyMove); 
        game.MakeMove(0, 1, _isNotLuckyMove); 

        // Act
        game.MakeMove(1, 1, _isAlwaysLuckyMove);

        // Assert
        var board = game.GetBoard();
        Assert.Equal(Player.O, board[1, 1]);
        Assert.Equal(Player.O, game.CurrentPlayer);
    }

    [Fact]
    public void MakeMove_ShouldThrowException_WhenCellIsOccupied()
    {
        // Arrange
        var game = new Game(Guid.NewGuid(), 3, 3);
        game.MakeMove(0, 0, _isNotLuckyMove); 

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => game.MakeMove(0, 0, _isNotLuckyMove));
    }

    [Fact]
    public void MakeMove_ShouldSetStatusToXWins_OnVerticalWin()
    {
        // Arrange
        var game = new Game(Guid.NewGuid(), 3, 3);
        game.MakeMove(0, 0, _isNotLuckyMove); // X
        game.MakeMove(0, 1, _isNotLuckyMove); // O
        game.MakeMove(1, 0, _isNotLuckyMove); // X
        game.MakeMove(1, 1, _isNotLuckyMove); // O

        // Act 
        game.MakeMove(2, 0, _isNotLuckyMove);

        // Assert
        Assert.Equal(GameStatus.XWins, game.Status);
        Assert.Null(game.CurrentPlayer);
    }

    [Fact]
    public void MakeMove_ShouldSetStatusToOWins_OnDiagonalWin()
    {
        // Arrange
        var game = new Game(Guid.NewGuid(), 3, 3);
        game.MakeMove(0, 0, _isNotLuckyMove); // X
        game.MakeMove(0, 2, _isNotLuckyMove); // O
        game.MakeMove(1, 0, _isNotLuckyMove); // X
        game.MakeMove(1, 1, _isNotLuckyMove); // O
        game.MakeMove(0, 1, _isNotLuckyMove); // X

        // Act 
        game.MakeMove(2, 0, _isNotLuckyMove);

        // Assert
        Assert.Equal(GameStatus.OWins, game.Status);
    }

    [Fact]
    public void MakeMove_ShouldSetStatusToDraw_WhenBoardIsFull()
    {
        // Arrange
        var game = new Game(Guid.NewGuid(), 3, 3);
        
        game.MakeMove(0, 0, _isNotLuckyMove); // X
        game.MakeMove(0, 1, _isNotLuckyMove); // O
        game.MakeMove(0, 2, _isNotLuckyMove); // X
        game.MakeMove(1, 1, _isNotLuckyMove); // O 

        game = new Game(Guid.NewGuid(), 3, 3);
        game.MakeMove(0, 0, _isNotLuckyMove); // X
        game.MakeMove(1, 1, _isNotLuckyMove); // O
        game.MakeMove(0, 1, _isNotLuckyMove); // X
        game.MakeMove(0, 2, _isNotLuckyMove); // O
        game.MakeMove(2, 0, _isNotLuckyMove); // X
        game.MakeMove(1, 0, _isNotLuckyMove); // O
        game.MakeMove(1, 2, _isNotLuckyMove); // X
        game.MakeMove(2, 2, _isNotLuckyMove); // O

        // Act 
        game.MakeMove(2, 1, _isNotLuckyMove); // X

        // Assert
        Assert.Equal(GameStatus.Draw, game.Status);
        Assert.Null(game.CurrentPlayer);
    }
}