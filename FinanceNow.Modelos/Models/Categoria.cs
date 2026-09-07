using FinanceNow.Modelos.Models.Enums;
using System.ComponentModel.DataAnnotations;


namespace FinanceNow.Modelos.Models;

public class Categoria
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 3)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public TipoDeTransacao Tipo { get; set; } 

    public virtual ICollection<Transacao>? Transacoes{ get; set; }
}