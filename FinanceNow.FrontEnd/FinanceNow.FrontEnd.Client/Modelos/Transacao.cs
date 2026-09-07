namespace FinanceNow.FrontEnd.Client.Modelos
{
    public class Transacao
    {
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public double Valor { get; set; }
        public string? Tipo { get; set; }
        public DateOnly DataDeVencimento { get; set; }
        public Categoria? Categoria { get; set; }
    }
}
