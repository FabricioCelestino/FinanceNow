using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceNow.Modelos.Models
{
    public class Mes(int id, int ano, int mesNumero, string name)
    {
        public int Id { get; set; } = id;
        public int Ano { get; set; } = ano;
        public int MesNumero { get; set; } = mesNumero;
        public string Name { get; set; } = name;
    }
}
