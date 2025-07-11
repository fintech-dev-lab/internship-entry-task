using CrossesAndZeroes.Domain.Entities;
using CrossesAndZeroes.Infrastructure.Data;
using CrossesAndZeroes.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossesAndZeroes.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly GameDbContext _db;

        public GameRepository(GameDbContext db)
        {
            _db = db;
        }

        public async Task<Game?> GetAsync(Guid id)
        {
            var entity = await _db.Games.FindAsync(id);
            return entity == null ? null : GameMapper.ToDomain(entity);
        }

        public async Task SaveAsync(Game game)
        {
            var existing = await _db.Games.FindAsync(game.Id);
            var entity = GameMapper.ToEntity(game);

            if (existing == null)
            {
                _db.Games.Add(entity);
            }
            else
            {
                _db.Entry(existing).CurrentValues.SetValues(entity);
            }

            await _db.SaveChangesAsync();
        }
    }
}
