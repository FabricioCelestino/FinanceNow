using System.ComponentModel.DataAnnotations;
using FinanceNow.Modelos.Models.Enums;

namespace FinanceNow.API.DTOs.CategoriaDTOs;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public record CreateCategoriaDto
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
{
    [Required]
    [StringLength(maximumLength: 15, MinimumLength = 3)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public TipoDeTransacao Tipo { get; set; }
}