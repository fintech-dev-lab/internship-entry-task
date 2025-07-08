using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Segrom.СrossesProject.Application.Abstractions;

namespace Segrom.СrossesProject.Host.Controllers.V1;

[ApiController]
[Route("v1/games/new")]
public class GamesNewController(
	ILogger<GamesNewController> logger, 
	IGameService gameService
	) : ControllerBase
{
	public record GamesNewResponse(
		[property: JsonPropertyName("game_id")]
		Guid GameId
	);
	
	[HttpGet]
	public async Task<IActionResult> Get()
	{
		var game = await gameService.NewGame();
		return Ok(new GamesNewResponse(GameId: game.Id));
	}
}