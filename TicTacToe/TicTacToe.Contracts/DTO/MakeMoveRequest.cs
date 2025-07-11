namespace TicTacToe.Contracts.DTO;

public class MakeMoveRequest
{
    public Guid GameUuid { get; set; }

    public Guid PlayerUuid { get; set; }

    public int Row { get; set; }

    public int Column { get; set; }

    public Guid MoveUuid { get; set; }
}