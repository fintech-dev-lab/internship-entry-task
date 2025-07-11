namespace TicTacToe.Core.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T> CreateAsync(T entity, CancellationToken token);
    
    Task<T> GetAsync(Guid uuid, CancellationToken token);
    
    Task<IEnumerable<T>> GetAllAsync(CancellationToken token);
    
    Task<T> UpdateAsync(T entity, CancellationToken token);

    Task DeleteAsync(Guid uuid, CancellationToken token);
}