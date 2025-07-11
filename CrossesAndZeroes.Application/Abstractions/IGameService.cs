using CrossesAndZeroes.Application.DTOs;
using CrossesAndZeroes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossesAndZeroes.Application.Abstractions
{
    public interface IGameService
    {
        Task<Game> CreateGameAsync();
        Task<Game?> GetGameAsync(Guid id);
        Task<Game> MakeMoveAsync(Guid id, MoveDto move);
    }
}
