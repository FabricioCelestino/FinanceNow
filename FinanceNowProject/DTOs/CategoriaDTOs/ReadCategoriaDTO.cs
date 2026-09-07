namespace FinanceNow.API.DTOs.CategoriaDTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Nome"></param>
public record ReadCategoriaDto(int Id, string Nome, string Tipo)
{
    /// <summary>
    /// 
    /// </summary>
    public ReadCategoriaDto() : this(0, string.Empty, string.Empty)
    {
    }
}