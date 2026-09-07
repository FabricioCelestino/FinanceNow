using System.ComponentModel.DataAnnotations;

namespace FinanceNow.Web.Request
{
    public class RequestTransacao()
    {
        
        public int Id { get; set; }
        [Required(ErrorMessage = "Digite uma descrição")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Digite uma descrição entre 3 e 50 caracteres")]
        public string? Descricao { get; set; }

        [Required]
        public int Tipo { get; set; }

        [Required]
        [Range(0.01, double.PositiveInfinity, ErrorMessage = "O valor deve ser maior que zero")]
        [DataType(DataType.Currency)]
        public double Valor { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly DataDeVencimento { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione uma categoria")]
        public int CategoriaId { get; set; }
    }
}
