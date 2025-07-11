using CrossesAndZeroes.Domain.Entities;

namespace CrossesAndZeroes.Infrastructure.Repositories
{
    public interface IGameRepository
    {
        Task<Game?> GetAsync(Guid id);
        Task SaveAsync(Game game);
    }
}