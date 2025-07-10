using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Contracts.DTO;
using TicTacToe.Core.Entities;
using TicTacToe.Services.Service.Interfaces;

namespace TicTacToe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly ILogger<GameController> _logger;
    private readonly IMapper _mapper;
    private readonly IGameService _gameService;

    public GameController(
        ILogger<GameController> logger,
        IMapper mapper,
        IGameService gameService)
    {
        _logger = logger;
        _mapper = mapper;
        _gameService = gameService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGame(
        [FromBody] CreateGameRequest request,
        CancellationToken token)
    {
        try
        {
            Game game = await _gameService.CreateGameAsync(request, token);
            GameResponse gameResponse = _mapper.Map<GameResponse>(game);
            return Ok(gameResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("move")]
    public async Task<IActionResult> MakeMove(
        [FromBody] MakeMoveRequest request,
        CancellationToken token)
    {
        try
        {
            Game game = await _gameService.MakeMoveAsync(request, token);
            GameResponse gameResponse = _mapper.Map<GameResponse>(game);
            return Ok(gameResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}