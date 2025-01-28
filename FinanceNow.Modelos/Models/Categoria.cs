using FinanceNow.Modelos.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceNow.Modelos.Models
{
    public class Categoria(int id, string name, TipoDeTransacao tipo)
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;
        public TipoDeTransacao Tipo { get; set; } = tipo;
    }
}
