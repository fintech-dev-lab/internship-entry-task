using System.Text.Json;
using TicTacToe.Domain.Enums;

namespace TicTacToe.Domain.Entities;

public class Game
{
    public Guid Id { get; private set; }
    public int BoardSize { get; private set; }
    public int WinCondition { get; private set; }
    public Player? CurrentPlayer { get; private set; }
    public GameStatus Status { get; private set; }
    public int MoveNumber { get; private set; }

    public string BoardAsJson { get; private set; }

    // Приватный конструктор для EF Core
    private Game() { }

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
        CurrentPlayer = Player.X;
        MoveNumber = 1;

        var initialBoard = new Player?[boardSize, boardSize];
        SetBoard(initialBoard);
    }

    public void MakeMove(int row, int column, Func<int, bool> isLuckyMove)
    {
        var board = GetBoard();

        if (Status != GameStatus.InProgress)
            throw new InvalidOperationException("Game is already finished.");
        if (row >= BoardSize || column >= BoardSize || row < 0 || column < 0)
            throw new ArgumentOutOfRangeException(nameof(row), "Move is outside the board.");
        if (board[row, column] != null)
            throw new InvalidOperationException("Cell is already occupied.");

        var playerToSet = GetPlayerForCurrentMove(isLuckyMove);
        board[row, column] = playerToSet;

        UpdateGameStatus(board, playerToSet, row, column);

        SetBoard(board);

        if (Status == GameStatus.InProgress)
        {
            SwitchPlayer();
            MoveNumber++;
        }
    }

    // Этот метод возвращает удобный для работы [,] массив. Он ТОЛЬКО для внутренней логики.
    public Player?[,] GetBoard()
    {
        // Десериализуем JSON в безопасный тип [][]
        var jaggedArray = JsonSerializer.Deserialize<string?[][]>(BoardAsJson);
        // Конвертируем безопасный [][] в рабочий [,]
        return JaggedArrayToBoard(jaggedArray);
    }

    // --- Приватные методы-помощники для конвертации ---

    // Принимает удобный [,] массив и сохраняет его в JSON
    private void SetBoard(Player?[,] board)
    {
        // Конвертируем рабочий [,] в безопасный для сериализации [][]
        var jaggedArray = BoardToJaggedArray(board);
        // Сериализуем БЕЗОПАСНЫЙ тип [][]
        BoardAsJson = JsonSerializer.Serialize(jaggedArray);
    }

    // Конвертер из нашего рабочего [,] в безопасный [][]
    private string?[][] BoardToJaggedArray(Player?[,] board)
    {
        var jaggedArray = new string?[BoardSize][];
        for (int i = 0; i < BoardSize; i++)
        {
            jaggedArray[i] = new string?[BoardSize];
            for (int j = 0; j < BoardSize; j++)
            {
                jaggedArray[i][j] = board[i, j]?.ToString();
            }
        }
        return jaggedArray;
    }

    // Конвертер из безопасного [][] в наш рабочий [,]
    private Player?[,] JaggedArrayToBoard(string?[][] jaggedArray)
    {
        var board = new Player?[BoardSize, BoardSize];
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                if (string.IsNullOrEmpty(jaggedArray[i][j]))
                {
                    board[i, j] = null;
                }
                else
                {
                    board[i, j] = Enum.Parse<Player>(jaggedArray[i][j]);
                }
            }
        }
        return board;
    }

    // --- Остальная логика без изменений ---
    private void SwitchPlayer()
    {
        CurrentPlayer = (CurrentPlayer == Player.X) ? Player.O : Player.X;
    }
    private Player GetPlayerForCurrentMove(Func<int, bool> isLuckyMove)
    {
        if (MoveNumber % 3 == 0 && isLuckyMove(10))
        {
            return (CurrentPlayer == Player.X) ? Player.O : Player.X;
        }
        return CurrentPlayer.Value;
    }
    private void UpdateGameStatus(Player?[,] board, Player lastPlayer, int lastRow, int lastCol)
    {
        if (CheckLine(board, lastPlayer, lastRow, lastCol, 1, 0) ||
            CheckLine(board, lastPlayer, lastRow, lastCol, 0, 1) ||
            CheckLine(board, lastPlayer, lastRow, lastCol, 1, 1) ||
            CheckLine(board, lastPlayer, lastRow, lastCol, 1, -1))
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
    private bool CheckLine(Player?[,] board, Player player, int startRow, int startCol, int dRow, int dCol)
    {
        int count = 0;
        for (int i = -(WinCondition - 1); i < WinCondition; i++)
        {
            int r = startRow + i * dRow;
            int c = startCol + i * dCol;

            if (r >= 0 && r < BoardSize && c >= 0 && c < BoardSize && board[r, c] == player)
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
}