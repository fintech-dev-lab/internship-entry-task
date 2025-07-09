using TicTacToe.Enums;

namespace TicTacToe.Data.Entites
{
    public class Game : BaseEntity
    {
        public int BoardSize { get; set; }
        public int WinCondition { get; set; }
        public PlayerType? CurrentPlayer { get; set; }
        public bool IsFinished { get; set; }
        public GameResult? Result { get; set; }
        public int MovesCount { get; set; }

        public List<Move> Moves { get; set; } = [];
    }
}
