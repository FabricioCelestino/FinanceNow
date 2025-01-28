using FinanceNow.Modelos.Models.Enums;


namespace FinanceNow.Modelos.Models
{
    public class Categoria(int id, string name, TipoDeTransacao tipo)
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;
        public TipoDeTransacao Tipo { get; set; } = tipo;

        public ICollection<Transacao>? Transacoes{ get; set; }
    }
}
