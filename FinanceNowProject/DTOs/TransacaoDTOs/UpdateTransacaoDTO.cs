using System.ComponentModel.DataAnnotations;
using FinanceNow.Modelos.Models.Enums;

namespace FinanceNow.API.DTOs.TransacaoDTOs;

/// <summary>
/// 
/// </summary>
public record UpdateTransacaoDto()
{

    public int Id { get; set; } 

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Digite uma descrição entre 3 a 50 caracteres")]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    public TipoDeTransacao Tipo { get; set; }

    [Required]
    [Range(minimum:0, maximum:double.PositiveInfinity, ErrorMessage = "O valor não pode ser negativo")]
    [DataType(DataType.Currency)]
    public double Valor { get; set; }

    [Required]
    public DateOnly DataDeVencimento { get; set; }

    [Required]
    public int CategoriaId { get; set; }



}