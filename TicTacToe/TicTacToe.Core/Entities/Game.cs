using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using TicTacToe.Core.Interfaces;

namespace TicTacToe.Core.Entities;

public class Game: IEntityWithUuid
{
    public Guid Uuid { get; set; }

    public Guid FirstPlayerUuid { get; set; }
    public User FirstPlayer { get; set; }
    
    public Guid SecondPlayerUuid { get; set; }
    public User SecondPlayer { get; set; }
    
    public string BoardJson { get; set; }

    [NotMapped]
    public string[][] Board
    {
        get => JsonSerializer.Deserialize<string[][]>(BoardJson)!;
        set => BoardJson = JsonSerializer.Serialize(value);
    }


    public List<Move> Moves { get; set; }
    
    public User? Winner { get; set; }
    public Guid? WinnerUuid { get; set; }
    
    public bool IsDraw { get; set; }
}