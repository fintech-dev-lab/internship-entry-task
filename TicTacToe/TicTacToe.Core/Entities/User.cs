using TicTacToe.Core.Interfaces;

namespace TicTacToe.Core.Entities;

public class User: IEntityWithUuid
{
    public Guid Uuid { get; set; }

    public string FullName { get; set; }
}