using CrossesAndZeroes.Application.Abstractions;
using CrossesAndZeroes.Application.DTOs;
using CrossesAndZeroes.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CrossesAndZeroes.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        
        [HttpPost]
        public async Task<ActionResult<Game>> CreateGame()
        {
            var game = await _gameService.CreateGameAsync();
            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(Guid id)
        {
            var game = await _gameService.GetGameAsync(id);
            if (game == null)
                return NotFound();
            return Ok(game);
        }

        
        [HttpPost("{id}/moves")]
        public async Task<ActionResult<Game>> MakeMove(Guid id, [FromBody] MoveDto move)
        {
            try
            {
                var game = await _gameService.MakeMoveAsync(id, move);
                return Ok(game);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
