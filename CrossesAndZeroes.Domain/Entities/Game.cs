using CrossesAndZeroes.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CrossesAndZeroes.Domain.Entities
{
    public class Game
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public int BoardSize { get; private set; }
        public char[][] Board { get; private set; }
        public string CurrentPlayer { get; set; } = "X";
        public GameState State { get; set; } = GameState.InProgress;
        public int MoveCount { get; set; } = 0;
        public int WinLength { get; private set; }

        private Game() { }

        public Game(Guid id, int boardSize, int winLength)
        {
            Id = id;
            BoardSize = boardSize;
            WinLength = winLength;
            CurrentPlayer = "X";
            State = GameState.InProgress;
            MoveCount = 0;

            Board = new char[boardSize][];
            for (int i = 0; i < boardSize; i++)
            {
                Board[i] = new char[boardSize];
                for (int j = 0; j < boardSize; j++)
                {
                    Board[i][j] = ' ';  
                }
            }
        }

        public void ApplyMove(int row, int col)
        {
            if (State != GameState.InProgress)
                throw new InvalidOperationException("Game is over");

            if (Board[row][col] != ' ')
                throw new InvalidOperationException("Cell already taken");

            bool opponentMove = false;
            var mark = CurrentPlayer;
            if ((MoveCount + 1) % 3 == 0 && RandomChance(0.1))
            {
                mark = CurrentPlayer == "X" ? "O" : "X";
                opponentMove = true;
            }


            Board[row][col] = mark[0];
            MoveCount++;

            if (CheckWin(row, col, mark[0]))
            {
                State = mark == "X" ? GameState.XWon : GameState.OWon;
                return;
            }

            if (MoveCount == BoardSize * BoardSize)
            {
                State = GameState.Draw;
                return;
            }

            CurrentPlayer = CurrentPlayer == "X" ? "O" : "X";
        }

        private bool CheckWin(int row, int col, char playerMark)
        {
            return CheckDirection(row, col, playerMark, 1, 0)     // горизонталь
                || CheckDirection(row, col, playerMark, 0, 1)     // вертикаль
                || CheckDirection(row, col, playerMark, 1, 1)     // диагональ /
                || CheckDirection(row, col, playerMark, 1, -1);   // диагональ \
        }

        private bool CheckDirection(int row, int col, char playerMark, int dx, int dy)
        {
            int count = 1;
            count += CountInDirection(row, col, playerMark, dx, dy);
            count += CountInDirection(row, col, playerMark, -dx, -dy);
            return count >= WinLength;
        }

        private int CountInDirection(int row, int col, char playerMark, int dx, int dy)
        {
            int count = 0;
            int x = row + dx;
            int y = col + dy;

            while (x >= 0 && x < BoardSize && y >= 0 && y < BoardSize && Board[x][y] == playerMark)
            {
                count++;
                x += dx;
                y += dy;
            }

            return count;
        }

        private bool RandomChance(double probability)
        {
            return Random.Shared.NextDouble() < probability;
        }
       
        public string StateDescription
        {
            get
            {
                return State switch
                {
                    GameState.InProgress => "Game is in progress",
                    GameState.XWon => "Player X has won",
                    GameState.OWon => "Player O has won",
                    GameState.Draw => "Draw",
                    _ => "Unknown state"
                };
            }
        }
        public string[] BoardVisualLines
        {
            get
            {
                var lines = new List<string>();
                for (int i = 0; i < BoardSize; i++)
                {
                    var line = " ";
                    for (int j = 0; j < BoardSize; j++)
                    {
                        line += Board[i][j];
                        if (j < BoardSize - 1)
                            line += " | ";
                    }
                    lines.Add(line);

                    if (i < BoardSize - 1)
                        lines.Add(new string('-', BoardSize * 4 - 3));
                }
                return lines.ToArray();
            }
        }
    }

}