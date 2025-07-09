using Segrom.СrossesProject.Domain.Entities;
using Segrom.СrossesProject.Domain.Enums;
using Segrom.СrossesProject.PostgresRepository.DAO;

namespace Segrom.СrossesProject.PostgresRepository.Extensions;

internal static class MappingExtensions
{
	public static Game ToDomain(this GameDao dao)
	{
		var size = dao.Cells.GetLength(0);
		var domain = new Game(dao.Id, (uint)size, (uint)dao.LengthForWin);

		for (var y = 0; y < size; y++)
		{
			for (var x = 0; x < size; x++)
			{
				domain.Field.Cells[x,y] = (CellState)dao.Cells[x,y];
			}
		}
		domain.Winner = dao.Winner.HasValue ? (WinnerType)dao.Winner : null;
		domain.CurrentPlayer = (PlayerType)dao.CurrentPlayer;
		domain.State = (GameState)dao.State;
		return domain;
	}
	
	public static short[,] ToShortArray(this CellState[,] array)
	{
		var size = array.GetLength(0);
		var result = new short[size,size];
		
		for (var y = 0; y < size; y++)
		{
			for (var x = 0; x < size; x++)
			{
				result[x,y] = (short)array[x,y];
			}
		}
		return result;
	}
}