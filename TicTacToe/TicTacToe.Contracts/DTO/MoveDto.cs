namespace TicTacToe.Contracts.DTO;

public class MoveDto
{
    public Guid Uuid { get; set; }

    public Guid PlayerUuid { get; set; }

    public int Row { get; set; }

    public int Column { get; set; }

    public DateTime Timestamp { get; set; }
}