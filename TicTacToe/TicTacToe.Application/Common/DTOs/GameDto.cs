namespace TicTacToe.Application.Common.DTOs
{
    public class GameDto
    {
        public Guid GameId { get; set; }
        public string?[,] Board { get; set; } 
        public string Status { get; set; }
        public string? NextPlayer { get; set; }
    }
}
