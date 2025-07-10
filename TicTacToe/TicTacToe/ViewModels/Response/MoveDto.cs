using TicTacToe.Enums;

namespace TicTacToe.ViewModels.Response
{
    public class MoveDto
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public TicTacToeSymbol Symbol { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
