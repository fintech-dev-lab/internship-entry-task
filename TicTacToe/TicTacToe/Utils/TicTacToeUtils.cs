using TicTacToe.Enums;
using TicTacToe.Extentions;

namespace TicTacToe.Utils
{
    public class TicTacToeUtils
    {
        public static GameResult CheckGameStatus(char[,] board, int winLength)
        {
            int size = board.GetLength(0);
            char winner = CheckWinner(board, size, winLength);

            if (winner != '\0')
                return winner.ToString().ToEnum<TicTacToeSymbol>() == TicTacToeSymbol.X ? GameResult.XWin : GameResult.OWin; 

            // Проверка на ничью
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (board[i, j] == '\0')
                        return GameResult.InProcess;

            return GameResult.Draw;
        }

        private static char CheckWinner(char[,] board, int size, int k)
        {
            int[][] directions = new int[][]
            {
                [0, 1],  // горизонталь
                [1, 0],  // вертикаль
                [1, 1],  // диагональ
                [1, -1]  // диагональ
            };

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    char current = board[i, j];
                    if (current == '\0')
                        continue;

                    foreach (var dir in directions)
                    {
                        int dx = dir[0], dy = dir[1];
                        int count = 1;

                        for (int step = 1; step < k; step++)
                        {
                            int ni = i + dx * step;
                            int nj = j + dy * step;

                            if (ni < 0 || nj < 0 || ni >= size || nj >= size)
                                break;

                            if (board[ni, nj] == current)
                                count++;
                            else
                                break;
                        }

                        if (count >= k)
                            return current;
                    }
                }
            }

            return '\0'; // Победителя нет
        }
    }
}
