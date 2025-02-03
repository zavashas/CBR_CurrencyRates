

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CurrencyTracker.Models
{
    ///<summary>
    ///Контекст базы данных для управления таблицами валют (Currencies) и курсов валют (CurrencyRates).
    ///</summary>

    public class CurrencyDbContext : DbContext
    {
        public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Currency>()
                        .HasKey(c => c.IdCurrency);
            modelBuilder.Entity<CurrencyRate>()
                        .HasKey(c => c.IdCurrencyRate);
        }
    }
}
