namespace TicTacToe.Contracts.DTO;

public class GameResponse
{
    public Guid Uuid { get; set; }

    public Guid FirstPlayerUuid { get; set; }
    public Guid SecondPlayerUuid { get; set; }

    public string[][] Board { get; set; }

    public List<MoveDto> Moves { get; set; }

    public Guid? WinnerUuid { get; set; }

    public bool IsDraw { get; set; }
}