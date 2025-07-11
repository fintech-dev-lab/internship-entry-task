using MediatR;
using Microsoft.Extensions.Options;
using TicTacToe.Application.Configuration;
using TicTacToe.Application.Interfaces;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Features.Games.Commands
{
    public record CreateGameCommand : IRequest<Guid>;

    public class CreateGameCommandHandler(IGameRepository gameRepository, IOptions<GameSettings> gameSettings) : IRequestHandler<CreateGameCommand, Guid>
    {
        private readonly IGameRepository _gameRepository = gameRepository;
        private readonly GameSettings _gameSettings = gameSettings.Value;

        public async Task<Guid> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            var game = new Game(Guid.NewGuid(), _gameSettings.BoardSize, _gameSettings.WinCondition);

            await _gameRepository.AddAsync(game, cancellationToken);

            return game.Id;
        }
    }
}
