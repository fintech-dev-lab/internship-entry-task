namespace TicTacToe.Core.Entities;

public abstract class GameResult
{
    private GameResult() { }

    public sealed class Winner : GameResult
    {
        public Guid PlayerUuid { get; }
        public Winner(Guid playerUuid)
        {
            PlayerUuid = playerUuid;
        }
    }

    public sealed class Draw : GameResult
    {
        public Draw() { }
    }
}