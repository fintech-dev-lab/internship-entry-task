using Segrom.СrossesProject.Domain.Entities;
using Segrom.СrossesProject.Domain.ValueObjects;

namespace Segrom.СrossesProject.Application.Abstractions;

public interface IGameService
{
	Task<Game> NewGame(CancellationToken cancellationToken);
	Task<Game> GetGame(Guid gameId, CancellationToken cancellationToken);
	Task<Game> Move(Guid gameId, Position position, CancellationToken cancellationToken);
}