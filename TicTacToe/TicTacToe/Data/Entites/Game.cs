using TicTacToe.Enums;

namespace TicTacToe.Data.Entites
{
    public class Game : BaseEntity
    {
        public int BoardSize { get; set; }
        public int WinLenght { get; set; }
        public TicTacToeSymbol? CurrentSymbol { get; set; }

        public GameResult Result { get; set; }
        public int MovesCount { get; set; }

        public Guid ETag { get; set; }

        public List<Move> Moves { get; set; } = [];
    }
}
