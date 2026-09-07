using System.ComponentModel.DataAnnotations;
using FinanceNow.Modelos.Models.Enums;

namespace FinanceNow.API.DTOs.CategoriaDTOs;

public record UpdateCategoriaDto
{
    [Required]
    [StringLength(maximumLength: 15, MinimumLength = 3)]
    public string Nome { get; set; }
    [Required]
    public TipoDeTransacao Tipo { get; set; }
}