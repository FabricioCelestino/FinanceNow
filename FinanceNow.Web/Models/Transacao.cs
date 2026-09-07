using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceNow.Web.Models;

public class Transacao
{
       
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(maximumLength:50, MinimumLength = 3)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    public string Tipo { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Currency)]
    [Range(minimum:0, maximum:double.PositiveInfinity)]
    public double Valor { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}")]
    public DateOnly DataDeVencimento { get; set; }

    [ForeignKey(nameof(Categoria))]
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

       


}