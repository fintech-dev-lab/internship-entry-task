using TicTacToe.Data.Entites;

namespace TicTacToe.ViewModels.Response
{
    public class GameDto
    {
        public int BoardSize { get; set; }
        public bool IsFinished { get; set; }
        public DateTime CreatedAt { get; set; }
        public GameResult? Result { get; set; }

        public MoveDto[]? Moves { get; set; }
    }
}
