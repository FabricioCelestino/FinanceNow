using FinanceNow.Modelos.Models.Enums;

namespace FinanceNow.API.DTOs.TransacaoDTOs
{
    public record UpdateTransacaoDTO(string Descricao, TipoDeTransacao Tipo,
        double Valor, DateOnly DataDeVencimento, int CategoriaId)
    {
    }
}
