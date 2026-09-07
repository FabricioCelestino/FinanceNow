using FinanceNow.Modelos.Models;

namespace FinanceNow.Data.Repositories;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task FindCategoriaAsync(int? id, CancellationToken cancellationToken = default);
    Task FindCategoriaByAsync(int? id, CancellationToken cancellationToken = default);
}