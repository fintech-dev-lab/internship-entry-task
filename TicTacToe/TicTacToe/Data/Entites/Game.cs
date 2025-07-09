namespace TicTacToe.Data.Entites
{
    public class Game : BaseEntity
    {
        public int BoardSize { get; set; }
        public bool IsFinished { get; set; }
        public GameResult? Result { get; set; }

        public List<Move> Moves { get; set; } = [];
    }

    public enum GameResult
    {
        XWin,
        OWin
    }
}
