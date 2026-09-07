using FinanceNow.Data.DataBase;
using FinanceNow.Modelos.Models;
using FinanceNow.Modelos.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Runtime.ConstrainedExecution;

namespace FinanceNow.Data.Repositories;

public class TransacaoRepository(Context context) : ITransacaoRepository
{
    public async Task<IReadOnlyCollection<Transacao>> FindTransacoesAsync(ushort ano, byte mes, TipoDeTransacao tipo, CancellationToken cancellationToken = default)
    {
        var listaDeTransacoes = await context.Transacoes.Include(t => t.Categoria)
            .Where(t =>
                t.DataDeVencimento.Year == ano &&
                t.DataDeVencimento.Month == mes &&
                t.Tipo == tipo)
            .ToListAsync(cancellationToken);

        return listaDeTransacoes.AsReadOnly();
    }



    public async Task<Transacao> FindTransacaoByAsync(int? id, CancellationToken cancellationToken = default)
    {
        if (id is null) throw new ArgumentNullException(nameof(id), "Id não pode ser nulo.");
        var transacao = await context.Transacoes
            .Include(t => t.Categoria)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        return transacao ?? throw new KeyNotFoundException($"Transação com ID {id} não encontrada.");
    }

    public async Task AddAsync(Transacao transacao, CancellationToken cancellationToken = default)
    {
        int mes = 12;

        ArgumentNullException.ThrowIfNull(transacao,
            $"A transação fornecida não pode ser nula.");

        if (transacao.DataDeVencimento == default)
            throw new ArgumentException("Data de vencimento é obrigatória.", nameof(transacao));

        await context.AddAsync(transacao, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        if (transacao.Recorrente is true)
        {
            for (int i = 1; i < mes; i++)
            {
                var novaTransacao = new Transacao
                {
                    Descricao = transacao.Descricao,
                    Valor = transacao.Valor,
                    Tipo = transacao.Tipo,
                    CategoriaId = transacao.CategoriaId,
                    DataDeVencimento = transacao.DataDeVencimento.AddMonths(1),
                    Recorrente = true,
                    
                };
                novaTransacao.DataDeVencimento = transacao.DataDeVencimento.AddMonths(i);
                await context.AddAsync(novaTransacao, cancellationToken);
               
            }

            await context.SaveChangesAsync(cancellationToken);
        }

    }

    public async Task UpdateAsync(Transacao transacao, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(transacao, "A transação fornecida não pode ser nula.");

        var transacaoExistente = await FindTransacaoByAsync(transacao.Id, cancellationToken);

        context.Transacoes.Entry(transacaoExistente).CurrentValues.SetValues(transacao);
        await context.SaveChangesAsync(cancellationToken);
    }


    public async Task DeleteAsync(Transacao transacao, CancellationToken cancellationToken = default)
    {
        context.Transacoes.Remove(transacao);
        await context.SaveChangesAsync(cancellationToken);
    }
}