
using FinanceNow.Modelos.Models;

namespace FinanceNow.Data.Repositories;

public interface IRepository<in T> where T : class
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    
}