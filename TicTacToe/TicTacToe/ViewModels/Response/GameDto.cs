using TicTacToe.Enums;

namespace TicTacToe.ViewModels.Response
{
    public class GameDto
    {
        public int BoardSize { get; set; }
        public PlayerType? CurrentPlayer { get; set; }
        public bool IsFinished { get; set; }
        public DateTime CreatedAt { get; set; }
        public GameResult? Result { get; set; }

        public MoveDto[]? Moves { get; set; }
    }
}
