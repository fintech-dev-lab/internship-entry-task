using TicTacToe.Enums;

namespace TicTacToe.ViewModels.Request
{
    public class CreateMoveDto
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public TicTacToeSymbol? Symbol { get; set; }
    }
}
