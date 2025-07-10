namespace TicTacToe.ViewModels.Request
{
    public class CreateGameDto
    {
        public int? BoardSize { get; set; }
        public int? WinLenght { get; set; }
    }
}
