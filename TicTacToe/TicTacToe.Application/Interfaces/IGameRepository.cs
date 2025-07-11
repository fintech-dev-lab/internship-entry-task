using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Interfaces
{
    public interface IGameRepository
    {
        Task<Game?> GetByIdAsync(Guid id, CancellationToken cancellation = default);
        Task AddAsync(Game game, CancellationToken cancellation = default);
        Task UpdateAsync(Game game, CancellationToken cancellation = default);
    }
}
