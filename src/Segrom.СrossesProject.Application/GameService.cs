using Microsoft.Extensions.Options;
using Segrom.СrossesProject.Application.Abstractions;
using Segrom.СrossesProject.Application.Options;
using Segrom.СrossesProject.Domain.Models;

namespace Segrom.СrossesProject.Application;

internal sealed class GameService(IOptionsMonitor<GameOptions> options): IGameService
{
	public Task<Game> NewGame()
	{
		return Task.FromResult(new Game(options.CurrentValue.FieldSize));
	}
}