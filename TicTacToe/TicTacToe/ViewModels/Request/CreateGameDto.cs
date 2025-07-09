namespace TicTacToe.ViewModels.Request
{
    public class CreateGameDto
    {
        public int? BoarSize { get; set; }
        public int? WinCondition { get; set; }
    }
}
