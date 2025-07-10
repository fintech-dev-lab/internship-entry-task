using TicTacToe.Data.Entites;

namespace TicTacToe.Extentions
{
    public static class GameExtentions
    {
        public static char[,] GetBoard(this Game game)
        {
            var board = new char[game.BoardSize, game.BoardSize];
            for(int row= 0; row < game.BoardSize; row++)
                for (int column = 0; column < game.BoardSize; column++)
                    board[row, column] = ' ';
            

            foreach(var move in game.Moves)
            {
                board[move.Row, move.Column] = char.Parse(move.Symbol.ToString());
            }
            return board;
        }
    }
}
