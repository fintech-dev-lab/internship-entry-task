using TicTacToe.Core.Interfaces;

namespace TicTacToe.Core.Entities;

public class Move: IEntityWithUuid
{
    public Guid Uuid { get; set; }

    public Guid GameUuid { get; set; }
    public Game Game { get; set; }

    public Guid PlayerUuid { get; set; }
    public User Player { get; set; }
    
    public int Row { get; set; }
    
    public int Column { get; set; }

    public DateTime Timestamp { get; set; }
}