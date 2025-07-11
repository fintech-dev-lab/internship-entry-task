using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Application.Features.Games.Commands;
using TicTacToe.Application.Features.Games.Queries;
using TicTacToe.Application.Interfaces;

namespace TicTacToe.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController(IMediator mediator, IIdempotencyCache cache) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IIdempotencyCache _cache = cache;

        [HttpPost]
        public async Task<IActionResult> CreateGame()
        {
            var gameId = await _mediator.Send(new CreateGameCommand());
            var gameDto = await _mediator.Send(new GetGameByIdQuery(gameId));
            return CreatedAtAction(nameof(GetGameById), new { id = gameId }, gameDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetGameById(Guid id)
        {
            var gameDto = await _mediator.Send(new GetGameByIdQuery(id));
            return gameDto is null ? NotFound() : Ok(gameDto);
        }

        public record MakeMoveRequest(string Player, int Row, int Column);

        [HttpPost("{id:guid}/moves")]
        public async Task<IActionResult> MakeMove(Guid id, [FromBody] MakeMoveRequest request)
        {
            if (!Request.Headers.TryGetValue("Idempotency-Key", out var idempotencyKey))
            {
                return BadRequest(new ProblemDetails { Title = "Idempotency-Key header is missing." });
            }

            var key = idempotencyKey.ToString();

            Response.Headers.ETag = new Microsoft.Extensions.Primitives.StringValues(key);

            var cachedResponse = await _cache.GetAsync(key);
            if (cachedResponse is not null)
            {
                return Ok(cachedResponse);
            }

            try
            {
                var command = new MakeMoveCommand(id, Enum.Parse<Domain.Enums.Player>(request.Player), request.Row, request.Column);
                await _mediator.Send(command);

                var updatedGame = await _mediator.Send(new GetGameByIdQuery(id));

                if (updatedGame is not null)
                {
                    await _cache.SetAsync(key, updatedGame);
                }

                return Ok(updatedGame);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ProblemDetails { Title = "Invalid Move", Detail = ex.Message });
            }
            catch (ApplicationException ex)
            {
                return NotFound(new ProblemDetails { Title = "Not Found", Detail = ex.Message });
            }
        }
    }
}
