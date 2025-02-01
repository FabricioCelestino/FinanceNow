using FinanceNow.API.DTOs.CategoriaDTOs;

namespace FinanceNow.API.DTOs.TransacaoDTOs
{
    public record ReadTransacaoDTO(string Descricao, double Valor, DateOnly DataDeVencimento, ReadCategoriaDTO ReadCategoriaDTO)
    {
        public ReadTransacaoDTO() : this(string.Empty, 0, default, new ReadCategoriaDTO())
        {
        }
    }
}
