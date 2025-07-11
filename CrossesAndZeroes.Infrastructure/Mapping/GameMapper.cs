using CrossesAndZeroes.Domain.Entities;
using CrossesAndZeroes.Domain.Enums;
using CrossesAndZeroes.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CrossesAndZeroes.Infrastructure.Mapping
{
    public static class GameMapper
    {
        public static Game ToDomain(GameEntity entity)
        {
            var board = JsonSerializer.Deserialize<char[][]>(entity.BoardJson)!;
            var game = new Game(entity.Id, entity.BoardSize, entity.WinLength);


            for (int i = 0; i < entity.BoardSize; i++)
                for (int j = 0; j < entity.BoardSize; j++)
                    game.Board[i][j] = board[i][j];

            SetPrivateProperty(game, "CurrentPlayer", entity.CurrentPlayer);
            SetPrivateProperty(game, "MoveCount", entity.MoveCount);
            SetPrivateProperty(game, "State", Enum.Parse<GameState>(entity.State));

            return game;
        }

        public static GameEntity ToEntity(Game domain)
        {
            return new GameEntity
            {
                Id = domain.Id,
                BoardSize = domain.BoardSize,
                WinLength = domain.WinLength,
                CurrentPlayer = domain.CurrentPlayer,
                MoveCount = domain.MoveCount,
                State = domain.State.ToString(),
                BoardJson = JsonSerializer.Serialize(domain.Board)
            };
        }

        //private static char[][] ConvertToJagged(char[,] board)
        //{
        //    int rows = board.GetLength(0);
        //    int cols = board.GetLength(1);
        //    var jagged = new char[rows][];

        //    for (int i = 0; i < rows; i++)
        //    {
        //        jagged[i] = new char[cols];
        //        for (int j = 0; j < cols; j++)
        //        {
        //            jagged[i][j] = board[i, j];
        //        }
        //    }

        //    return jagged;
        //}

        //private static char[,] ConvertTo2D(char[][] jagged)
        //{
        //    int rows = jagged.Length;
        //    int cols = jagged[0].Length;
        //    var board = new char[rows, cols];

        //    for (int i = 0; i < rows; i++)
        //    {
        //        for (int j = 0; j < cols; j++)
        //        {
        //            board[i, j] = jagged[i][j];
        //        }
        //    }

        //    return board;
        //}

        private static void SetPrivateProperty<T>(object obj, string propertyName, T value)
        {
            var prop = obj.GetType()
                .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            prop?.SetValue(obj, value);
        }

    }
}
