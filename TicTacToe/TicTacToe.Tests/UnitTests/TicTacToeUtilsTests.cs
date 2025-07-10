using TicTacToe.Enums;
using TicTacToe.Utils;

namespace TicTacToe.Tests.UnitTests
{
    public class TicTacToeUtilsTests
    {
        [Fact]
        public void HorizontalWin_X_ReturnsWinner()
        {
            char[,] board = {
                { ' ', 'X', 'X', 'X', ' ' },
                { 'O', 'O', ' ', ' ', ' ' },
                { ' ', ' ', ' ', ' ', ' ' },
                { ' ', ' ', ' ', ' ', ' ' },
                { ' ', ' ', ' ', ' ', ' ' },
            };

            var result = TicTacToeUtils.CheckGameStatus(board, 3);
            Assert.Equal(GameResult.XWin, result);
        }

        [Fact]
        public void VerticalWin_O_ReturnsWinner()
        {

            char[,] board = {
                { ' ', 'X', 'X', 'X', ' ' },
                { ' ', ' ', ' ', 'O', ' ' },
                { ' ', ' ', ' ', 'O', ' ' },
                { ' ', ' ', ' ', 'O', ' ' },
                { ' ', ' ', ' ', 'O', 'X' },
            };

            var result = TicTacToeUtils.CheckGameStatus(board, 4);
            Assert.Equal(GameResult.OWin, result);
        }

        [Fact]
        public void DiagonalWin_X_ReturnsWinner()
        {
            char[,] board = {
                { ' ', ' ', ' ', ' ', ' ' },
                { ' ', 'X', ' ', 'O', ' ' },
                { ' ', ' ', 'X', 'O', ' ' },
                { ' ', ' ', 'O', 'X', ' ' },
                { ' ', ' ', ' ', ' ', 'X' },
            };

            var result = TicTacToeUtils.CheckGameStatus(board, 4);
            Assert.Equal(GameResult.XWin, result);
        }

        [Fact]
        public void AntiDiagonalWin_O_ReturnsWinner()
        {
            char[,] board = {
                { 'X', ' ', ' ', ' ', 'O' },
                { ' ', 'X', ' ', 'O', ' ' },
                { ' ', ' ', 'O', ' ', ' ' },
                { ' ', 'O', ' ', 'X', 'X' },
                { 'O', ' ', ' ', ' ', 'X' },
            };

            var result = TicTacToeUtils.CheckGameStatus(board, 5);
            Assert.Equal(GameResult.OWin, result);
        }

        [Fact]
        public void FullBoard_Draw_ReturnsDraw()
        {
            char[,] board = {
                { 'X', 'O', 'X', 'O' },
                { 'O', 'X', 'O', 'O' },
                { 'X', 'X', 'O', 'X' },
                { 'O', 'O', 'X', 'X' },
            };

            var result = TicTacToeUtils.CheckGameStatus(board, 4);
            Assert.Equal(GameResult.Draw, result);
        }

        [Fact]
        public void EmptyCells_GameInProgress()
        {
            char[,] board = {
                { 'X', 'O', ' ' },
                { ' ', 'O', 'X' },
                { 'X', ' ', 'O' },
            };

            var result = TicTacToeUtils.CheckGameStatus(board, 3);
            Assert.Equal(GameResult.InProgress, result);
        }
    }
}
