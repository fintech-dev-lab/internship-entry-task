using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossesAndZeroes.Application.DTOs
{
    public class GameDto
    {
        public Guid Id { get; set; }
        public int BoardSize { get; set; }
        public char[][] Board { get; set; } = default!;
        public string CurrentPlayer { get; set; } = default!;
        public string State { get; set; } = default!;
        public int MoveCount { get; set; }
        public int WinLength { get; set; }
        public string BoardVisual { get; set; }
    }
}
