using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace CurrencyTracker.Models
{
    public class CurrencyDbContextFactory : IDesignTimeDbContextFactory<CurrencyDbContext>
    {
        public CurrencyDbContext CreateDbContext(string[] args)
        {
            // Чтение конфигурации из файла appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Создание экземпляра CurrencyDbContext с передачей строки подключения
            var optionsBuilder = new DbContextOptionsBuilder<CurrencyDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new CurrencyDbContext(optionsBuilder.Options);
        }
    }
}
