namespace FinanceNow.API.DTOs.CategoriaDTOs
{
    public record ReadCategoriaDTO(string Name)
    {
        public ReadCategoriaDTO() : this(string.Empty)
        {
        }
    }
}
