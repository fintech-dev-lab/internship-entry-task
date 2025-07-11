using MediatR;
using TicTacToe.Application.Common.DTOs;
using TicTacToe.Application.Interfaces;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Features.Games.Queries
{
    public record GetGameByIdQuery(Guid Id) : IRequest<GameDto?>;

    public class GetGameByIdQueryHandler : IRequestHandler<GetGameByIdQuery, GameDto?>
    {
        private readonly IGameRepository _gameRepository;

        public GetGameByIdQueryHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<GameDto?> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
        {
            var game = await _gameRepository.GetByIdAsync(request.Id, cancellationToken);

            if (game is null)
            {
                return null;
            }

            return game.ToDto();
        }
    }

    public static class GameMapper
    {
        public static GameDto ToDto(this Game game)
        {
            var board = game.GetBoardState();
            var boardDto = new string?[game.BoardSize, game.BoardSize];

            for (int i = 0; i < game.BoardSize; i++)
            {
                for (int j = 0; j < game.BoardSize; j++)
                {
                    boardDto[i, j] = board[i, j]?.ToString();
                }
            }

            return new GameDto
            {
                GameId = game.Id,
                Board = boardDto,
                Status = game.Status.ToString(),
                NextPlayer = game.CurrentPlayer?.ToString()
            };
        }
    }
}
