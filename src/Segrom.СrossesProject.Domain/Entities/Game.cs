using Segrom.СrossesProject.Domain.Enums;
using Segrom.СrossesProject.Domain.Exceptions;
using Segrom.СrossesProject.Domain.Extensions;
using Segrom.СrossesProject.Domain.ValueObjects;

namespace Segrom.СrossesProject.Domain.Entities;

public sealed class Game(uint fieldSize, uint lengthForWin)
{
	public Guid Id { get; set; }

	public GameField Field { get; set; } = new(fieldSize);

	public GameState State { get; set; } = GameState.Continues;
	public WinnerType? Winner { get; set; }
	public PlayerType CurrentPlayer { get; set; } = PlayerType.XPlayer;

	public void Move(Position position)
	{
		if (Field.IsPositionOutOfBounds(position)) 
			throw new DomainException("Position is out of bounds");
		
		if (Field.Cells[position.X, position.Y] != CellState.Empty)
			throw new DomainException("Cell is already occupied");
		
		Field.Cells[position.X, position.Y] = CurrentPlayer.GetCell();
		CurrentPlayer = GetNextPlayer();
		UpdateGameState();
	}

	private PlayerType GetNextPlayer()
	{
		switch (CurrentPlayer)
		{
			case PlayerType.XPlayer:
				return PlayerType.OPlayer;
			case PlayerType.OPlayer:
				return PlayerType.XPlayer;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void UpdateGameState()
	{
		var winnerCell = Field.TryGetCellWithLength(lengthForWin);
		if (winnerCell is null)
		{
			if (!Field.IsFieldFilledIn()) return;
			Winner = WinnerType.Draw;
			State = GameState.Finished;
			return;
		}

		Winner = winnerCell.Value.GetWinner();
		State = GameState.Finished;
	}
}