using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Segrom.СrossesProject.Application.Abstractions;
using Segrom.СrossesProject.Host.DTO;
using Segrom.СrossesProject.Host.Extensions;

namespace Segrom.СrossesProject.Host.Controllers.V1;

[ApiController]
[Route("v1/games/new")]
public class GamesNewController(
	ILogger<GamesNewController> logger, 
	IGameService gameService
	) : ControllerBase
{
	public record GamesNewResponse(
		[property: JsonPropertyName("game")]
		GameDto Game
	);
	
	[HttpGet]
	[ProducesResponseType(typeof(GamesNewResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Get(CancellationToken cancellationToken)
	{
		var game = await gameService.NewGame(cancellationToken);
		return Ok(new GamesNewResponse(Game: game.ToDto()));
	}
}