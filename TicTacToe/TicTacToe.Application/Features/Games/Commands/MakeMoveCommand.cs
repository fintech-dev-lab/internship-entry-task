using MediatR;
using TicTacToe.Application.Interfaces;
using TicTacToe.Domain.Enums;

namespace TicTacToe.Application.Features.Games.Commands
{
    public record MakeMoveCommand(Guid GameId, Player Player, int Row, int Column) : IRequest<Unit>;

    public class MakeMoveCommandHandler : IRequestHandler<MakeMoveCommand, Unit>
    {
        private readonly IGameRepository _gameRepository;
        private readonly IRandomProvider _randomProvider;

        public MakeMoveCommandHandler(IGameRepository gameRepository, IRandomProvider randomProvider)
        {
            _gameRepository = gameRepository;
            _randomProvider = randomProvider;
        }

        public async Task<Unit> Handle(MakeMoveCommand request, CancellationToken cancellationToken)
        {
            var game = await _gameRepository.GetByIdAsync(request.GameId, cancellationToken);

            if (game is null)
            {
                throw new ApplicationException($"Game with id {request.GameId} not found.");
            }

            if (game.CurrentPlayer != request.Player)
            {
                throw new InvalidOperationException($"It's not player {request.Player}'s turn.");
            }

            game.MakeMove(request.Row, request.Column, _randomProvider.ShouldOccur);

            await _gameRepository.UpdateAsync(game, cancellationToken);

            return Unit.Value;
        }
    }
}
