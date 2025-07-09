using Microsoft.Extensions.Options;
using Segrom.СrossesProject.Application.Abstractions;
using Segrom.СrossesProject.Application.Exceptions;
using Segrom.СrossesProject.Application.Options;
using Segrom.СrossesProject.Domain.Entities;
using Segrom.СrossesProject.Domain.Enums;
using Segrom.СrossesProject.Domain.Exceptions;
using Segrom.СrossesProject.Domain.ValueObjects;

namespace Segrom.СrossesProject.Application;

internal sealed class GameService(
	IOptionsMonitor<GameOptions> options,
	IGameProvider gameProvider
	): IGameService
{
	public async Task<Game> NewGame(CancellationToken cancellationToken)
	{
		return await gameProvider.Create(options.CurrentValue.FieldSize, cancellationToken);
	}

	public async Task<Game> GetGame(Guid gameId, CancellationToken cancellationToken)
	{
		var game = await gameProvider.Get(gameId, cancellationToken);
		if (game is null) 
			throw new AppException("Game not found");
		return game;
	}

	public async Task<Game> Move(Guid gameId, Position position, CancellationToken cancellationToken)
	{
		var game = await gameProvider.Get(gameId, cancellationToken);
		if (game is null) 
			throw new AppException("Game not found");
		if (game.State == GameState.Finished)
			throw new AppException("Game is finished");

		try
		{
			game.Move(position);
		}
		catch (DomainException e)
		{
			throw new AppException("Failed to move game", e);
		}

		await gameProvider.Update(game);
		return game;
	}
}