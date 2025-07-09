namespace Segrom.СrossesProject.PostgresRepository.DAO;

internal sealed class GameDao
{
	public Guid Id { get; set; }
	public DateTime Created { get; set; }
	public DateTime Updated { get; set; }
	public short State { get; set; }
	public short? Winner { get; set; }
	public short CurrentPlayer { get; set; }
	public short[,] Cells { get; set; } = null;
	public int LengthForWin { get; set; }
};