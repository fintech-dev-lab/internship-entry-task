using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Segrom.СrossesProject.Application.Abstractions;
using Segrom.СrossesProject.Application.Exceptions;
using Segrom.СrossesProject.Host.Dtos;
using Segrom.СrossesProject.Host.Extensions;

namespace Segrom.СrossesProject.Host.Controllers.V1;

[ApiController]
[Route("v1/games/{id:guid}")]
public class GamesGetController(
	ILogger<GamesNewController> logger, 
	IGameService gameService
	) : ControllerBase
{
	public record GamesGetResponse(
		[property: JsonPropertyName("game")]
		GameDto Game
	);
	
	[HttpGet]
	[ProducesResponseType(typeof(GamesGetResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		try
		{
			var game = await gameService.GetGame(id, cancellationToken);
			return Ok(new GamesGetResponse(Game: game.ToDto()));
		}
		catch (AppException e)
		{
			return BadRequest(e);
		}
	}
}