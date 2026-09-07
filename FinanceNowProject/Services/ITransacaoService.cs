using FinanceNow.API.DTOs.TransacaoDTOs;
using FinanceNow.Modelos.Models;
using FinanceNow.Modelos.Models.Enums;

namespace FinanceNow.API.Services
{
    public interface ITransacaoService
    {
        Task<IReadOnlyCollection<Transacao>> GetByPeriodAsync(ushort ano, byte mes, TipoDeTransacao tipo, CancellationToken ct = default);     
        Task<Transacao?> GetByIdAsync(int? id, CancellationToken ct = default);
        Task<Transacao> CreateAsync(CreateTransacaoDto dto, CancellationToken ct = default);
        Task<bool> UpdateAsync(UpdateTransacaoDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
