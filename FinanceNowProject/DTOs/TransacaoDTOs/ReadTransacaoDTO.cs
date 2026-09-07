using FinanceNow.API.DTOs.CategoriaDTOs;

namespace FinanceNow.API.DTOs.TransacaoDTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Descricao"></param>
/// <param name="Valor"></param>
/// <param name="Tipo"></param>
/// <param name="DataDeVencimento"></param>
public record ReadTransacaoDto(int Id,string Descricao, double Valor, string Tipo, 
    DateOnly DataDeVencimento, ReadCategoriaDto Categoria)
{
    /// <summary>
    /// 
    /// </summary>
    public ReadTransacaoDto() : this(0,string.Empty, 0, string.Empty, 
        default, new ReadCategoriaDto())
    {
    }
}