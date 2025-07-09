using Segrom.СrossesProject.Domain.Enums;
using Segrom.СrossesProject.Domain.ValueObjects;

namespace Segrom.СrossesProject.Domain.Test;

public sealed class GameFieldTests
{
	[Theory]
	[InlineData(0, 0, false)]
	[InlineData(2, 2, false)]
	[InlineData(3, 3, true)]
	[InlineData(4, 0, true)]
	[InlineData(0, 4, true)]
	public void IsPositionOutOfBounds_ValidatesCorrectly(uint x, uint y, bool expected)
	{
		// Arrange
		var field = new GameField(3);
		var position = new Position { X = x, Y = y };

		// Act
		var result = field.IsPositionOutOfBounds(position);

		// Assert
		Assert.Equal(expected, result);
	}

	[Fact]
	public void IsFieldFilledIn_WhenAllEmpty_ReturnsFalse()
	{
		// Arrange
		var field = new GameField(2);

		// Act
		var result = field.IsFieldFilledIn();

		// Assert
		Assert.False(result);
	}

	[Fact]
	public void IsFieldFilledIn_WhenPartiallyFilled_ReturnsFalse()
	{
		// Arrange
		var field = new GameField(2);
		field.Cells[0, 0] = CellState.X;

		// Act
		var result = field.IsFieldFilledIn();

		// Assert
		Assert.False(result);
	}

	[Fact]
	public void IsFieldFilledIn_WhenCompletelyFilled_ReturnsTrue()
	{
		// Arrange
		var field = new GameField(2);
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				field.Cells[i, j] = CellState.X;
			}
		}

		// Act
		var result = field.IsFieldFilledIn();

		// Assert
		Assert.True(result);
	}

	[Fact]
	public void TryGetCellWithLength_NoSequence_ReturnsNull()
	{
		// Arrange
		var field = new GameField(3);

		// Act
		var result = field.TryGetCellWithLength(3);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void TryGetCellWithLength_HorizontalXSequence_ReturnsX()
	{
		// Arrange
		var field = new GameField(3);
		for (uint i = 0; i < 3; i++)
		{
			field.Cells[i, 0] = CellState.X;
		}

		// Act
		var result = field.TryGetCellWithLength(3);

		// Assert
		Assert.Equal(CellState.X, result);
	}

	[Fact]
	public void TryGetCellWithLength_VerticalOSequence_ReturnsO()
	{
		// Arrange
		var field = new GameField(3);
		for (uint i = 0; i < 3; i++)
		{
			field.Cells[0, i] = CellState.O;
		}

		// Act
		var result = field.TryGetCellWithLength(3);

		// Assert
		Assert.Equal(CellState.O, result);
	}

	[Fact]
	public void TryGetCellWithLength_DiagonalSequence_ReturnsCellState()
	{
		// Arrange
		var field = new GameField(3)
		{
			Cells =
			{
				[0, 0] = CellState.X,
				[1, 1] = CellState.X,
				[2, 2] = CellState.X
			}
		};

		// Act
		var result = field.TryGetCellWithLength(3);

		// Assert
		Assert.Equal(CellState.X, result);
	}

	[Fact]
	public void TryGetCellWithLength_SequenceShorterThanRequired_ReturnsNull()
	{
		// Arrange
		var field = new GameField(3)
		{
			Cells =
			{
				[0, 0] = CellState.X,
				[1, 0] = CellState.X
			}
		};

		// Act
		var result = field.TryGetCellWithLength(3);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void TryGetCellWithLength_MultipleSequences_ReturnsFirstMatch()
	{
		// Arrange
		var field = new GameField(4);
		for (uint i = 0; i < 2; i++)
		{
			field.Cells[0, i] = CellState.O;
		}
		for (uint i = 0; i < 3; i++)
		{
			field.Cells[i, 0] = CellState.X;
		}

		// Act
		var result = field.TryGetCellWithLength(3);
		
		// Assert
		Assert.Equal(CellState.X, result);
	}
}