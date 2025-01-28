using FinanceNow.Modelos.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceNow.Modelos.Models
{
    public class Transacao(int id, string descricao, TipoDeTransacao tipo, double valor, DateTime dataDeVencimento, int categoriaId, int medId)
    {
        public int Id { get; set; } = id;
        public string Descricao { get; set; } = descricao;
        public TipoDeTransacao Tipo { get; set; } = tipo;
        public double Valor { get; set; } = valor;
        public DateTime DataDeVencimento { get; set; } = dataDeVencimento;

        [ForeignKey(nameof(Categoria))]
        public int CategoriaId { get; set; } = categoriaId;

        [ForeignKey(nameof(Mes))]
        public int MedId { get; set; } = medId;
    }
}
