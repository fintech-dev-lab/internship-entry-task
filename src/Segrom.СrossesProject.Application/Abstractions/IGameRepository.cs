using Segrom.СrossesProject.Domain.Entities;

namespace Segrom.СrossesProject.Application.Abstractions;

public interface IGameRepository
{
	Task Create(Game game, CancellationToken cancellationToken);
	Task<Game?> Get(Guid gameId, CancellationToken cancellationToken);
	Task Update(Game game, CancellationToken cancellationToken);
}