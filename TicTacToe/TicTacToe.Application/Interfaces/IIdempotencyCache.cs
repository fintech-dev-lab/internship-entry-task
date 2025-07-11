using TicTacToe.Application.Common.DTOs;

namespace TicTacToe.Application.Interfaces
{
    public interface IIdempotencyCache
    {
        Task<GameDto?> GetAsync(string key);
        Task SetAsync(string key, GameDto response, TimeSpan? expiry = null);
    }
}
