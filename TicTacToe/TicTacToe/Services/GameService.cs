using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicTacToe.Abstractions;
using TicTacToe.Data;
using TicTacToe.Data.Entites;
using TicTacToe.Enums;
using TicTacToe.Exceptions;
using TicTacToe.Models;
using TicTacToe.ViewModels.Request;
using TicTacToe.ViewModels.Response;

namespace TicTacToe.Services
{
    public class GameService : IGameService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _dbContext;

        public GameService(
            IMapper mapper,
            ApplicationContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GameDto> Create(GameOptions options)
        {
            var game = new Game()
            {
                BoardSize = options.BoardSize,
                WinCondition = options.WinCondition,
                CurrentPlayer = PlayerType.X,
                IsFinished = false,
            };

            await _dbContext.Games.AddAsync(game);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<GameDto>(game);
        }

        public async Task<GameDto> Get(int id)
        {
            var game = await _dbContext.Games.AsNoTracking().Include(g => g.Moves).FirstOrDefaultAsync(g => g.Id == id);
            if (game == null)
                throw new ServiceException("Game Not Found", "Game with id {id} not found", StatusCodes.Status404NotFound);

            return _mapper.Map<GameDto>(game);
        }

        public async Task<GameDto[]> Get()
        {
            var games = await _dbContext.Games.AsNoTracking().Include(g => g.Moves).ToArrayAsync();
            return _mapper.Map<GameDto[]>(games);
        }

        public async Task<GameDto> Move(int gameId, CreateMoveDto dto)
        {
            var game = await _dbContext.Games.Include(g => g.Moves).FirstOrDefaultAsync(g => g.Id == gameId);
            if (game == null)
                throw new ServiceException("Game Not Found", $"Game with id {gameId} not found", StatusCodes.Status404NotFound);

            if (game.IsFinished)
                throw new ServiceException("Game Finished", $"Game with id {gameId} is finished", StatusCodes.Status409Conflict);
            
            ValidateMove(dto, game);

            var random = new Random();
            if (game.MovesCount + 1 % 3 == 0 && random.Next(100) < 10)
                dto.Player = dto.Player == PlayerType.X ? PlayerType.O : PlayerType.X;

            var move = new Move() 
            {
                Column = dto.Column, 
                Row = dto.Row, 
                GameId = game.Id 
            };
            game.Moves.Add(move);
            game.MovesCount++;
            game.CurrentPlayer = game.CurrentPlayer == PlayerType.X ? PlayerType.O : PlayerType.X;

            var (isEndGame, gameResult) = CheckGameStatus(game);

            if(isEndGame)
            {
                game.Result = gameResult;
                game.IsFinished = true;
                game.CurrentPlayer = null;
            }

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<GameDto>(game);
        }

        private (bool isEndGame, GameResult result) CheckGameStatus(Game game)
        {
            throw new NotImplementedException();
        }

        private void ValidateMove(CreateMoveDto dto, Game game)
        {
            if (game.CurrentPlayer != dto.Player!.Value)
                throw new ServiceException("Invalid Player", "It's the other player's turn now", StatusCodes.Status409Conflict);

            if (game.BoardSize <= dto.Column || game.BoardSize <= dto.Row)
                throw new ServiceException("Invalid move", "Invalid column or row", StatusCodes.Status400BadRequest);

            if (game.Moves.Any(m => m.Column == dto.Column && m.Row == dto.Row))
                throw new ServiceException("Invalid move", "The cell is already taken", StatusCodes.Status400BadRequest);
        }
    }
}
