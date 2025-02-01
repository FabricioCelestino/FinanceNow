using FinanceNow.Modelos.Models.Enums;

namespace FinanceNow.API.DTOs.CategoriaDTOs
{
    public record CreateCategoriaDTO(string Nome, TipoDeTransacao TipoDeTransacao)
    {
    }
}
