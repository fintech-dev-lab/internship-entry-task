namespace TicTacToe.Data.Entites
{
    public class Move : BaseEntity
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
