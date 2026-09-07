using AutoMapper;
using FinanceNow.API.DTOs.TransacaoDTOs;
using FinanceNow.Data.Repositories;
using FinanceNow.Modelos.Models;
using FinanceNow.Modelos.Models.Enums;

namespace FinanceNow.API.Services
{
    public class TransacaoService(ITransacaoRepository repository, IMapper mapper) : ITransacaoService
    {
        public async Task<Transacao> CreateAsync(CreateTransacaoDto dto, CancellationToken ct)
        {
            var entity = mapper.Map<Transacao>(dto);
            await repository.AddAsync(entity, ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var entity = await repository.FindTransacaoByAsync(id, ct);
            if (entity is null) return false;

            await repository.DeleteAsync(entity, ct);
            return true;
        }

        public async Task<Transacao?> GetByIdAsync(int? id, CancellationToken ct = default)
        {
           return await repository.FindTransacaoByAsync(id, ct);

        }

        public async Task<IReadOnlyCollection<Transacao>> GetByPeriodAsync(ushort ano, byte mes, TipoDeTransacao tipo, CancellationToken ct = default)
        {
            if (ano is < 2000 or > 3000) throw new ArgumentOutOfRangeException(nameof(ano), "O ano deve estar entre 2000 e 3000.");
            if (mes is < 1 or > 12) throw new ArgumentOutOfRangeException(nameof(mes), "O mês deve estar entre 1 e 12.");
            return await repository.FindTransacoesAsync(ano, mes, tipo, ct);
        }

        public async Task<bool> UpdateAsync(UpdateTransacaoDto dto, CancellationToken ct)
        {
            var exists = repository.FindTransacaoByAsync(dto.Id, ct);
            if (exists is null) return false;

            var entity = mapper.Map<Transacao>(dto);
            await repository.UpdateAsync(entity, ct);
            return true;

        }
    }
}
