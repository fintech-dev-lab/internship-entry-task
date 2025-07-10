using Microsoft.EntityFrameworkCore;
using TicTacToe.Core.Interfaces;

namespace TicTacToe.Core.BaseEntities;

public class BaseRepository<T> : IRepository<T>
    where T : class, IEntityWithUuid, new()
{
    private readonly DbContext _context;

    public BaseRepository(DbContext context)
    {
        _context = context;
    }

    public virtual async Task<T> GetAsync(Guid uuid, CancellationToken token)
    {
        DbSet<T> set = _context.Set<T>();
        T result = await set.AsNoTracking().FirstOrDefaultAsync(e => e.Uuid == uuid, token);
        return result;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken token)
    {
        DbSet<T> set = _context.Set<T>();
        return await set.AsNoTracking().ToListAsync(token);
    }

    public virtual async Task DeleteAsync(Guid uuid, CancellationToken token)
    {
        DbSet<T> set = _context.Set<T>();

        T entity = new T()
        {
            Uuid = uuid
        };

        set.Remove(entity);
        await _context.SaveChangesAsync(token);
    }

    public virtual async Task<T> CreateAsync(T entity, CancellationToken token)
    {
        DbSet<T> set = _context.Set<T>();

        await set.AddAsync(entity, token);
        await _context.SaveChangesAsync(token);

        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity, CancellationToken token)
    {
        DbSet<T> set = _context.Set<T>();

        set.Update(entity);
        await _context.SaveChangesAsync(token);

        return entity;
    }
}