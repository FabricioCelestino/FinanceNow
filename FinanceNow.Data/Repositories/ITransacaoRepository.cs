using FinanceNow.Modelos.Models;
using FinanceNow.Modelos.Models.Enums;

namespace FinanceNow.Data.Repositories;

public interface ITransacaoRepository: IRepository<Transacao>
{

    public Task<IReadOnlyCollection<Transacao>> FindTransacoesAsync(ushort ano, byte mes, TipoDeTransacao tipo, CancellationToken cancellationToken = default);

    public Task<Transacao> FindTransacaoByAsync(int? id, CancellationToken cancellationToken = default);

    
}