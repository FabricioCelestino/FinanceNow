using FinanceNow.Modelos.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceNow.Data.DataBase
{
    public class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Transacao> Transacoes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transacao>()
                .Property(t => t.Tipo)
                .HasConversion<string>();

            modelBuilder.Entity<Categoria>()
                .Property(c  => c.Tipo)
                .HasConversion<string>();   
        }
    }
}
