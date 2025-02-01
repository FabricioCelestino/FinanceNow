using FinanceNow.Modelos.Models.Enums;


namespace FinanceNow.Modelos.Models
{
    public class Categoria()
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public TipoDeTransacao Tipo { get; set; } 

        public virtual ICollection<Transacao>? Transacoes{ get; set; }
    }
}
