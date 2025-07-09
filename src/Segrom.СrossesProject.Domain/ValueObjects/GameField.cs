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
		if (IsTypeHasSequenceGreaterOrEqual(CellState.X, length)) return CellState.X;
		if (IsTypeHasSequenceGreaterOrEqual(CellState.O, length)) return CellState.O;
		return null;
	}

	private bool IsTypeHasSequenceGreaterOrEqual(CellState cell, uint length)
	{
		for (var y = 0; y < size; y++)
		{
			for (var x = 0; x < size; x++)
			{
				if (Cells[x, y] != cell) continue;
				
				var sequenceLength = GetSequenceLength(x, y, cell, (1,0));
				if (sequenceLength >= length) return true;
				sequenceLength = GetSequenceLength(x, y, cell, (1,1));
				if (sequenceLength >= length) return true;
				sequenceLength = GetSequenceLength(x, y, cell, (0,1));
				if (sequenceLength >= length) return true;
				sequenceLength = GetSequenceLength(x, y, cell, (-1,1));
				if (sequenceLength >= length) return true;
				sequenceLength = GetSequenceLength(x, y, cell, (-1,0));
				if (sequenceLength >= length) return true;
				sequenceLength = GetSequenceLength(x, y, cell, (-1,-1));
				if (sequenceLength >= length) return true;
				sequenceLength = GetSequenceLength(x, y, cell, (0,-1));
				if (sequenceLength >= length) return true;
				sequenceLength = GetSequenceLength(x, y, cell, (1,-1));
				if (sequenceLength >= length) return true;
			}
		}
		return false;
	}

	private uint GetSequenceLength(int x, int y, CellState cell, (int X, int Y) d)
	{
		uint length = 1;
		while (true)
		{
			if (x + d.X >= Size || x + d.X < 0 || y + d.Y >= Size || y + d.Y < 0 || Cells[x + d.X, y + d.Y] != cell) 
				return length;
			x += d.X;
			y += d.Y;
			length += 1;
		}
	}
}