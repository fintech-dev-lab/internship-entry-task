﻿using TicTacToe.Application.Interfaces;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Infrastructure.Persistence
{
    public class GameRepository(AppDbContext context) : IGameRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Game?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Games.FindAsync([id], cancellationToken);
        }

        public async Task AddAsync(Game game, CancellationToken cancellationToken = default)
        {
            await _context.Games.AddAsync(game, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Game game, CancellationToken cancellationToken = default)
        {
            _context.Games.Update(game);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
