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
            if (model.BoardSize == null)
                if (int.TryParse(Environment.GetEnvironmentVariable("BoardSize"), out var boardSize))
                    model.BoardSize = boardSize;
                else
                    model.BoardSize = _gameOptions.Value.BoardSize;

            if (model.WinLenght == null)
                if (int.TryParse(Environment.GetEnvironmentVariable("WinLenght"), out var winLenght))
                    model.WinLenght = winLenght;
                else
                    model.WinLenght = _gameOptions.Value.WinLenght;

            var gameDto = await _gameService.Create(model);
            return Ok(gameDto);
        }

        [HttpGet("{gameId}")]
        public async Task<ActionResult<GameDto>> Get(int gameId)
        {
            var gameDto = await _gameService.Get(gameId);
            return Ok(gameDto);
        }

        [HttpGet]
        public async Task<ActionResult<GameDto>> Get()
        {
            var games = await _gameService.Get();
            return Ok(games);
        }

        [HttpPost("{gameId}/move")]
        public async Task<ActionResult> Move(int gameId, CreateMoveDto dto)
        {
            var gameDto = await _gameService.Move(gameId, dto);
            return Ok(gameDto);
        }
    }
}
