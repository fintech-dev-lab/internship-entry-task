using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Application.Features.Games.Commands;
using TicTacToe.Application.Features.Games.Queries;

namespace TicTacToe.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GamesController(IMediator mediator)
        {
            _mediator = mediator;
        }

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
            try
            {
                await _mediator.Send(new MakeMoveCommand(id, Enum.Parse<Domain.Enums.Player>(request.Player), request.Row, request.Column));
                var updatedGame = await _mediator.Send(new GetGameByIdQuery(id));
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
