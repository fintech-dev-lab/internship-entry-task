using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TicTacToe.Abstractions;
using TicTacToe.Models;
using TicTacToe.ViewModels.Request;
using TicTacToe.ViewModels.Response;

namespace TicTacToe.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GamesController : BaseController
    {
        private readonly IOptions<GameOptions> _gameOptions;
        private readonly IGameService _gameService;

        public GamesController(
            IGameService gameService,
            IOptions<GameOptions> gameOptions)
        {
            _gameOptions = gameOptions;
            _gameService = gameService;
        }

        [HttpPost]
        public async Task<ActionResult<GameDto>> Create(CreateGameDto model)
        {
            var gameOptions = _gameOptions.Value;
            if (model.BoarSize != null)
                gameOptions.BoardSize = model.BoarSize.Value;
            else if (int.TryParse(Environment.GetEnvironmentVariable("DefaultBoardSize"), out var boardSize))
                gameOptions.BoardSize = boardSize;

            var gameDto = await _gameService.Create(gameOptions);
            return Ok(gameDto);
        }
    }
}
