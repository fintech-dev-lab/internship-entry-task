using MediatR;
using TicTacToe.Application.Interfaces;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Features.Games.Commands
{
    public record CreateGameCommand : IRequest<Guid>;

    public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, Guid>
    {
        private readonly IGameRepository _gameRepository;

        public CreateGameCommandHandler(IGameRepository gameRepository)
        {
            gameRepository = _gameRepository;
        }

        public async Task<Guid> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            const int boardSize = 3;
            const int winCondition = 3;

            var game = new Game(Guid.NewGuid(), boardSize, winCondition);

            await _gameRepository.AddAsync(game, cancellationToken);

            return game.Id;
        }
    }
}
