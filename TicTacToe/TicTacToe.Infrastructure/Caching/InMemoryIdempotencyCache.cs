using Microsoft.Extensions.Caching.Memory;
using TicTacToe.Application.Common.DTOs;
using TicTacToe.Application.Interfaces;

namespace TicTacToe.Infrastructure.Caching
{
    public class InMemoryIdempotencyCache(IMemoryCache cache) : IIdempotencyCache
    {
        private readonly IMemoryCache _cache = cache;

        public Task<GameDto?> GetAsync(string key)
        {
            _cache.TryGetValue(key, out GameDto? response);
            return Task.FromResult(response);
        }

        public Task SetAsync(string key, GameDto response, TimeSpan? expiry = null)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromHours(1)
            };
            _cache.Set(key, response, options);
            return Task.CompletedTask;
        }
    }
}
