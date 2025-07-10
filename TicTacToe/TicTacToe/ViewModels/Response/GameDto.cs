using TicTacToe.Enums;

namespace TicTacToe.ViewModels.Response
{
    public class GameDto
    {
        public int Id { get; set; }
        public int BoardSize { get; set; }
        public int WinLenght { get; set; }
        public TicTacToeSymbol? CurrentSymbol { get; set; }
     
        public GameResult Result { get; set; }
        public int MovesCount { get; set; }

        public MoveDto[]? Moves { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
