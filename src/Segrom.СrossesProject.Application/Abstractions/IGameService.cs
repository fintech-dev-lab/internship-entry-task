using Segrom.СrossesProject.Domain.Models;

namespace Segrom.СrossesProject.Application.Abstractions;

public interface IGameService
{
	Task<Game> NewGame();
}