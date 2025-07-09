using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Segrom.СrossesProject.Application.Abstractions;
using Segrom.СrossesProject.Application.Exceptions;
using Segrom.СrossesProject.Host.Dtos;
using Segrom.СrossesProject.Host.Extensions;

namespace Segrom.СrossesProject.Host.Controllers.V1;

[ApiController]
[Route("v1/games/{id:guid}/move")]
public class GamesMoveController(
	ILogger<GamesNewController> logger, 
	IGameService gameService
	) : ControllerBase
{
	public record GamesMoveRequest(
		[property: JsonPropertyName("position")]
		PositionDto Position
	);
	
	public record GamesMoveResponse(
		[property: JsonPropertyName("game")]
		GameDto Game
	);
	
	[HttpPost]
	[ProducesResponseType(typeof(GamesMoveResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Get(
		[FromRoute] Guid id, 
		[FromBody] GamesMoveRequest request,
		CancellationToken cancellationToken)
	{
		try
		{
			var game = await gameService.Move(id, request.Position.ToDomain(), cancellationToken);
			var dto = game.ToDto();
			Response.Headers.Append(HeaderNames.ETag, dto.GetHashCode().ToString());
			return Ok(new GamesMoveResponse(dto));
		}
		catch (AppException e)
		{
			return BadRequest(e);
		}
	}
}