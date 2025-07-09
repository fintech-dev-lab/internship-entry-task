using Segrom.СrossesProject.Application.Abstractions;
using Segrom.СrossesProject.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Segrom.СrossesProject.GameProvider;

internal sealed class DatabaseProviderWithCache(
	IGameRepository gameRepository,
	IMemoryCache memoryCache
	): IGameProvider
{
	public async Task<Game> Create(uint fieldSize, uint lengthForWin, CancellationToken cancellationToken)
	{
		var game = new Game(
			id: Guid.CreateVersion7(),
			fieldSize: fieldSize,
			lengthForWin: lengthForWin
			);
		await gameRepository.Create(game, cancellationToken);
		
		memoryCache.Set(game.Id, game);
		
		return game;
	}

	public async Task<Game?> Get(Guid gameId, CancellationToken cancellationToken)
	{
		if (memoryCache.TryGetValue(gameId, out Game? game)) 
			return game;
		
		game = await gameRepository.Get(gameId, cancellationToken);
		memoryCache.Set(gameId, game);
		return game;
	}

	public async Task Update(Game game, CancellationToken cancellationToken)
	{
		memoryCache.Set(game.Id, game);
		await gameRepository.Update(game, cancellationToken);
	}
}