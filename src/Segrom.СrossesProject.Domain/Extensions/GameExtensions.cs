using Segrom.СrossesProject.Domain.Enums;
using Segrom.СrossesProject.Domain.Exceptions;

namespace Segrom.СrossesProject.Domain.Extensions;

public static class GameExtensions
{
	public static CellState GetCell(this PlayerType player) => player switch
	{
		PlayerType.XPlayer => CellState.X,
		PlayerType.OPlayer => CellState.O,
		_ => throw new ArgumentOutOfRangeException()
	};
	
	public static WinnerType GetWinner(this CellState cell) => cell switch
	{
		CellState.X => WinnerType.XPlayer,
		CellState.O => WinnerType.OPlayer,
		CellState.Empty => throw new  DomainException("Could not get winner from empty cell."),
		_ => throw new ArgumentOutOfRangeException()
	};
}