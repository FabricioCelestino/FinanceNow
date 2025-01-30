using FinanceNow.Modelos.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceNow.Modelos.Models
{
    public class Transacao()
    {
       
        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; } 
        public TipoDeTransacao Tipo { get; set; }
        public double Valor { get; set; } 
        public DateOnly DataDeVencimento { get; set; }

        [ForeignKey(nameof(Categoria))]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

       


    }
}
