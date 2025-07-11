using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CrossesAndZeroes.Infrastructure.Entities
{
    public class GameEntity
    {
        public Guid Id { get; set; }
        public string CurrentPlayer { get; set; }
        public string State { get; set; }
        public int MoveCount { get; set; }
        public int BoardSize { get; set; }
        public int WinLength { get; set; }

        // Храним уже как jagged массив, чтобы не срать ошибками
        public string BoardJson { get; set; }

        public char[][] GetBoard()
    => JsonSerializer.Deserialize<char[][]>(BoardJson)!;

        public void SetBoard(char[][] board)
            => BoardJson = JsonSerializer.Serialize(board);
    }
}