using System.ComponentModel.DataAnnotations;
using FinanceNow.Modelos.Models.Enums;

namespace FinanceNow.API.DTOs.TransacaoDTOs;

public record CreateTransacaoDto
{
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Digite uma descrição entre 3 a 50 caracteres")]
    public string Descricao { get; set; } = string.Empty;

    [Required] public TipoDeTransacao Tipo { get; set; }

    [Required]
    [Range(0, double.PositiveInfinity, ErrorMessage = "O valor não pode ser negativo")]
    [DataType(DataType.Currency)]
    public double Valor { get; set; }

    [Required] public DateOnly DataDeVencimento { get; set; }

    [Required] public int CategoriaId { get; set; }

    public bool? Recorrente { get; set; } = false;
}