using Segrom.СrossesProject.Domain.Enums;

namespace Segrom.СrossesProject.Domain.ValueObjects;

public struct GameField(uint size)
{
	public readonly CellState[,] Cells = new CellState[size, size];
	public uint Size => size;
	
	public bool IsPositionOutOfBounds(Position position) => position.X >= Size || position.Y >= Size;

	public bool IsFieldFilledIn()
	{
		foreach (var cell in Cells)
		{
			if (cell == CellState.Empty) return false;
		}
		return true;
	}
	
	public CellState? TryGetCellWithLength(uint length)
	{
		throw new NotImplementedException();
	}
}