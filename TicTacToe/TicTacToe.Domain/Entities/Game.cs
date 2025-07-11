using TicTacToe.Domain.Enums;

namespace TicTacToe.Domain.Entities
{
    public class Game
    {
        public Guid Id { get; private set; }
        public int BoardSize { get; private set; }
        public int WinCondition { get; private set; }
        public Player? CurrentPlayer { get; private set; }
        public GameStatus Status { get; private set; }
        public int MoveNumber { get; private set; }
        public Player?[,] Board { get; private set; }

        public Game(Guid id, int boardSize, int winCondition)
        {
            if (boardSize < 3)
                throw new ArgumentException("Board size must be at least 3.", nameof(boardSize));
            if (winCondition > boardSize)
                throw new ArgumentException("Win condition cannot be greater than the board size.", nameof(winCondition));

            Id = id;
            BoardSize = boardSize;
            WinCondition = winCondition;
            Status = GameStatus.InProgress;
            Board = new Player?[boardSize, boardSize];
            CurrentPlayer = Player.X;
            MoveNumber = 1;
        }

        public void MakeMove(int row, int column, Func<int, bool> isLuckyMove)
        {
            ValidateMove(row, column);

            var playerToSet = GetPlayerForCurrentMove(isLuckyMove);

            Board[row, column] = playerToSet;

            UpdateGameStatus(playerToSet, row, column);

            if (Status == GameStatus.InProgress)
            {
                SwitchPlayer();
                MoveNumber++;
            }
        }

        private void ValidateMove(int row, int column)
        {
            if (Status != GameStatus.InProgress)
                throw new InvalidOperationException("Game is already finished.");
            if (row >= BoardSize || column >= BoardSize || row < 0 || column < 0)
                throw new ArgumentOutOfRangeException(nameof(row), "Move is outside the board.");
            if (Board[row, column] != null)
                throw new InvalidOperationException("Cell is already occupied.");
        }

        private void SwitchPlayer()
        {
            CurrentPlayer = (CurrentPlayer == Player.X) ? Player.O : Player.X;
        }

        private Player GetPlayerForCurrentMove(Func<int, bool> isLuckyMove)
        {
            if (MoveNumber % 3 == 0)
            {
                if (isLuckyMove(10))
                {
                    return (CurrentPlayer == Player.X) ? Player.O : Player.X;
                }
            }
            return CurrentPlayer.Value;
        }

        private void UpdateGameStatus(Player lastPlayer, int lastRow, int lastCol)
        {
            if (CheckLine(lastPlayer, lastRow, lastCol, 1, 0) || 
                CheckLine(lastPlayer, lastRow, lastCol, 0, 1) || 
                CheckLine(lastPlayer, lastRow, lastCol, 1, 1) || 
                CheckLine(lastPlayer, lastRow, lastCol, 1, -1))   
            {
                Status = lastPlayer == Player.X ? GameStatus.XWins : GameStatus.OWins;
                CurrentPlayer = null;
            }
            else if (MoveNumber == BoardSize * BoardSize)
            {
                Status = GameStatus.Draw;
                CurrentPlayer = null;
            }
        }

        private bool CheckLine(Player player, int startRow, int startCol, int dRow, int dCol)
        {
            int count = 0;
            for (int i = -(WinCondition - 1); i < WinCondition; i++)
            {
                int r = startRow + i * dRow;
                int c = startCol + i * dCol;

                if (r >= 0 && r < BoardSize && c >= 0 && c < BoardSize && Board[r, c] == player)
                {
                    count++;
                    if (count >= WinCondition) return true;
                }
                else
                {
                    count = 0;
                }
            }
            return false;
        }

        private Game() { }
    }
}
