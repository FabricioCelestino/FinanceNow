using FinanceNow.Modelos.Models.Enums;
using FinanceNow.Modelos.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace FinanceNow.API.DTOs.TransacaoDTOs
{
    public record CreateTransacaoDTO(string Descricao, TipoDeTransacao Tipo, 
        double Valor, DateOnly DataDeVencimento, int CategoriaId)
    {
       
    }
}
