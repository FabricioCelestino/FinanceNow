using System.ComponentModel.DataAnnotations;


namespace FinanceNow.Web.Models;

public class Categoria
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 3)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public string Tipo { get; set; } = string.Empty;

    
}