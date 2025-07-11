namespace TicTacToe.Contracts.DTO;

public class CreateGameRequest
{
    public Guid FirstPlayerUuid { get; set; }
    
    public Guid SecondPlayerUuid { get; set; }
}